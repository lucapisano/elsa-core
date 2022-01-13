using System;
using System.Runtime.CompilerServices;
using Elsa.Activities.Signaling;
using Elsa.Builders;
using Elsa.Services.Models;

// ReSharper disable ExplicitCallerInfoArgument
// ReSharper disable once CheckNamespace
namespace Elsa.Activities.ControlFlow
{
    public static class TimeoutSignalReceivedBuilderExtensions
    {
        public static IActivityBuilder TimeoutSignalReceived(this IBuilder builder, Action<ISetupActivity<TimeoutSignalReceived>> setup, [CallerLineNumber] int lineNumber = default, [CallerFilePath] string? sourceFile = default) =>
            builder.Then(setup, null, lineNumber, sourceFile);

        public static IActivityBuilder TimeoutSignalReceived(this IBuilder builder, Func<ActivityExecutionContext, string> signal, [CallerLineNumber] int lineNumber = default, [CallerFilePath] string? sourceFile = default) =>
            builder.TimeoutSignalReceived(activity => activity.Set(x => x.Signal, signal), lineNumber, sourceFile);

        public static IActivityBuilder TimeoutSignalReceived(this IBuilder builder, Func<string> signal, [CallerLineNumber] int lineNumber = default, [CallerFilePath] string? sourceFile = default) =>
            builder.TimeoutSignalReceived(activity => activity.Set(x => x.Signal, signal), lineNumber, sourceFile);

        public static IActivityBuilder TimeoutSignalReceived(this IBuilder builder, string signal, [CallerLineNumber] int lineNumber = default, [CallerFilePath] string? sourceFile = default) =>
            builder.TimeoutSignalReceived(activity => activity.Set(x => x.Signal, signal), lineNumber, sourceFile);
    }
}