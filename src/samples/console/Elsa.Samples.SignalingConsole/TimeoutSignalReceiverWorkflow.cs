using Elsa.Activities.Console;
using Elsa.Activities.ControlFlow;
using Elsa.Builders;

namespace Elsa.Samples.SignalingConsole
{
    public class TimeoutSignalReceiverWorkflow : IWorkflow
    {   
        public void Build(IWorkflowBuilder builder)
        {
            builder
                .TimeoutSignalReceived("Demo Signal")
                .WriteLine(() => $"TimeoutSignal received!");
        }
    }
}