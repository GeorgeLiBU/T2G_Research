using System;

namespace T2G.UnityAdapter
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExecutionAttribute : Attribute
    {
        public string Instruction { get; private set; }

        public ExecutionAttribute(string instruction)
        {
            Instruction = instruction;
        }
    }

    public abstract class ExecutionBase
    {
        public ExecutionBase() {  }
        public abstract void HandleExecution(Executor.Instruction instruction);
    }
}