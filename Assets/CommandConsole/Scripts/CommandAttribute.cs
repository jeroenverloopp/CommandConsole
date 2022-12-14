using System;

namespace CommandConsole
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CommandAttribute : Attribute
    {
        public readonly string Prefix;
        
        public CommandAttribute(string prefix = null)
        {
            Prefix = prefix;
        }
        
    }
}