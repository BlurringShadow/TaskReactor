using System;
using JetBrains.Annotations;

namespace Presentation.Utilities
{
    //弥补navigation service的扩展函数只能调用property public setter的不足
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter)]
    public sealed class ArgsAttribute : Attribute
    {
        [NotNull] public readonly Type FromType;
        public readonly string Key;
        public readonly Func<object, object> Converter;

        public ArgsAttribute([NotNull] Type fromType, string key = null, Func<object, object> converter = null)
        {
            FromType = fromType;
            Key = key;
            Converter = converter;
        }
    }
}