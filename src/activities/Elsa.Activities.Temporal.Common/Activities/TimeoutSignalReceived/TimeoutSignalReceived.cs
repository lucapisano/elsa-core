using System;
using System.Linq;
using System.Threading.Tasks;
using Elsa.Activities.Signaling.Models;
using Elsa.Activities.Temporal.Common.ActivityResults;
using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Design;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using NodaTime;

// ReSharper disable once CheckNamespace
namespace Elsa.Activities.Signaling
{
    /// <summary>
    /// Suspends workflow execution until the specified signal is received.
    /// </summary>
    [Trigger(
        Category = "Workflows",
        Description = "Suspend workflow execution until the specified signal is received.",
        Outcomes = new[] { OutcomeNames.Done, OutcomeNames.Timeout }
    )]
    public class TimeoutSignalReceived : Activity
    {
        private readonly IClock _clock;

        public TimeoutSignalReceived(IClock clock)
        {
            _clock = clock;
        }

        [ActivityInput(Hint = "The name of the signal to wait for.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public string Signal { get; set; } = default!;

        [ActivityInput(Hint = "The timeout interval at which this activity should resume and timeout.", SupportedSyntaxes = new[] { SyntaxNames.JavaScript, SyntaxNames.Liquid })]
        public Duration Timeout { get; set; } = default!;

        [ActivityOutput(Hint = "The input that was received with the signal.")]
        public object? SignalInput { get; set; }
        
        [ActivityOutput] public object? Output { get; set; }
        public Instant? ExecuteAt
        {
            get => GetState<Instant?>();
            set => SetState(value);
        }
        protected override bool OnCanExecute(ActivityExecutionContext context)
        {
            if (ExecuteAt <= _clock.GetCurrentInstant())
                return true;/*se il timeout è scaduto, può eseguire l'attività e restituire Outcome(Timeout)*/
            if (context.Input is Signal triggeredSignal)
                return string.Equals(triggeredSignal.SignalName, Signal, StringComparison.OrdinalIgnoreCase);

            return false;
        }

        public override async ValueTask<IActivityExecutionResult> ExecuteAsync(ActivityExecutionContext context)
        {
            if (context.WorkflowExecutionContext.IsFirstPass)
            {   
                return OnResume(context);
            }
            else
            {
                if (Timeout.TotalTicks > 0)
                {
                    ExecuteAt = _clock.GetCurrentInstant().Plus(Timeout);
                    context.JournalData.Add("Timeout Time", ExecuteAt);
                    return Combine(Suspend(), new ScheduleWorkflowResult(ExecuteAt.Value));
                }
                else
                {
                    context.JournalData.Add("Timeout Time", "never");
                    return Suspend();
                }
            }
        }
        async Task RemoveBlocks(ActivityExecutionContext context)
        {
            // Remove blocking activity.
            var blockingActivity = context.WorkflowInstance.BlockingActivities.FirstOrDefault(x => x.ActivityId == context.ActivityId);
            if (blockingActivity != null)
                await context.WorkflowExecutionContext.RemoveBlockingActivityAsync(blockingActivity);
        }
        protected override async ValueTask<IActivityExecutionResult> OnResumeAsync(ActivityExecutionContext context)
        {
            await RemoveBlocks(context);
            if (ExecuteAt <= _clock.GetCurrentInstant())
            {
                return Outcome(OutcomeNames.Timeout);
            }
            var triggeredSignal = context.GetInput<Signal>()!;
            SignalInput = triggeredSignal.Input;
            Output = triggeredSignal.Input;
            context.LogOutputProperty(this, nameof(Output), Output);
            return Done();
        }
    }
}