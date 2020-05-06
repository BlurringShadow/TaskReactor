using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Presentation.Utilities
{
    [Export]
    public class ArgsHelper : Dictionary<ValueTuple<Type, Type, string>, object>
    {
        public void Update<TFrom, TTo>(object value, [CallerMemberName] string key = null) =>
            Update(typeof(TFrom), typeof(TTo), value, key);

        public void Update(Type from, Type to, object value, [CallerMemberName] string key = null)
        {
            var keyTuple = new ValueTuple<Type, Type, string>(from, to, key);
            if(ContainsKey(keyTuple))
                this[keyTuple] = value;
            else Add(keyTuple, value);
        }

        public void Inject<T>(bool includeNonPublic = false) => Inject(typeof(T), includeNonPublic);

        public void Inject([NotNull] Type instance, bool includeNonPublic = false)
        {
            var flags = BindingFlags.Public |
                        BindingFlags.Static |
                        (includeNonPublic ? BindingFlags.NonPublic : default);

            InjectFields(instance, flags);
            InjectMethods(instance, flags);
        }

        private void InjectFields([NotNull] Type instance, BindingFlags flags)
        {
            var bindingFlags = BindingFlags.Instance | flags;
            foreach (var fieldInfo in instance.GetFields(bindingFlags))
            {
                var attribute = fieldInfo.GetCustomAttribute<ArgsAttribute>(true);

                if(attribute is null) continue;

                var key = attribute.Key ?? fieldInfo.Name;

                if(TryGetValue((attribute.FromType, instance, key), out var value))
                    fieldInfo.SetValue(this, value);
            }
        }

        private void InjectMethods([NotNull] Type instance, BindingFlags flags)
        {
            var bindingFlags = BindingFlags.InvokeMethod | flags;
            foreach (var methodInfo in instance.GetMethods(bindingFlags))
            {
                if(methodInfo.GetCustomAttribute<ArgsInitializationAttribute>() is null) continue;

                var parametersInfo = methodInfo.GetParameters();
                var parameters = new ArrayList(parametersInfo.Length);

                var index = 0;
                ParameterInfo parameterInfo;

                for (; index < parametersInfo.Length; index++)
                {
                    parameterInfo = parametersInfo[index];
                    var attribute = parameterInfo.GetCustomAttribute<ArgsAttribute>();

                    if(attribute is null) break;

                    var key = attribute.Key ?? parameterInfo.Name;

                    if(TryGetValue((attribute.FromType, instance, key), out var value))
                        parameters.Add(value);
                    else break;
                }

                while(true)
                {
                    if(index == parametersInfo.Length)
                    {
                        methodInfo.Invoke(this, parameters.ToArray());
                        break;
                    }

                    parameterInfo = parametersInfo[index];
                    if(parameterInfo.HasDefaultValue) parameters.Add(parameterInfo.DefaultValue);
                    else break;

                    index++;
                }
            }
        }
    }
}