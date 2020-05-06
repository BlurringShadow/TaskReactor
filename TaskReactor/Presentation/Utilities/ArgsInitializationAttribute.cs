using System;

namespace TaskReactor.Utilities
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ArgsInitializationAttribute : Attribute
    {
    }
}