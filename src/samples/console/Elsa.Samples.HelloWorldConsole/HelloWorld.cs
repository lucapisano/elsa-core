using Elsa.Activities.Console;
using Elsa.Activities.Workflows;
using Elsa.Builders;

namespace Elsa.Samples.HelloWorldConsole
{
    /// <summary>
    /// A basic workflow with just one WriteLine activity.
    /// </summary>
    public class HelloWorld : IWorkflow
    {
        public void Build(IWorkflowBuilder builder) => builder.WriteLine("Hello World!")
            .RunWorkflowWithName(nameof(HelloWorld2), RunWorkflow.RunWorkflowMode.Blocking);
    }
    /// <summary>
    /// A basic workflow with just one WriteLine activity.
    /// </summary>
    public class HelloWorld2 : IWorkflow
    {
        public void Build(IWorkflowBuilder builder) => builder.WithName(nameof(HelloWorld2))
            .WriteLine("Hello World2!");
    }
}