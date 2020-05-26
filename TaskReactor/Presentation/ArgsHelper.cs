using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Presentation
{
    [Export]
    public class ArgsHelper : Dictionary<(Type, Type, string), object>
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

        private void InjectFields([NotNull] IReflect instance, BindingFlags flags)
        {
            var bindingFlags = BindingFlags.Instance | flags;
            foreach (var fieldInfo in instance.GetFields(bindingFlags))
            {
                var converted = GetValueAndConvert(fieldInfo);
                if(!(converted is null))
                    fieldInfo.SetValue(this, converted);
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
                    var converted = GetValueAndConvert(parameterInfo, instance);
                    if(converted is null)
                        break;
                    parameters.Add(converted);
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

        private object GetValueAndConvert([NotNull] MemberInfo memberInfo)
        {
            var argsAttribute = memberInfo.GetCustomAttribute<ArgsAttribute>(true);
            var converterAttribute = memberInfo.GetCustomAttribute<TypeConverterAttribute>(true);

            if(argsAttribute is null ||
               !TryGetValue(
                   (argsAttribute.FromType, memberInfo.ReflectedType, argsAttribute.Key ?? memberInfo.Name),
                   out var value
               )) return null;

            var declaredType = memberInfo.DeclaringType;

            var message = $"Cannot assign value type {value?.GetType()} to declared type {declaredType}. " +
                          $"Need {nameof(TypeConverterAttribute)} to specify a suitable converter.";

            if(converterAttribute is null)
                return value is null || declaredType!.IsInstanceOfType(value) ?
                    value :
                    throw new ArgumentException(message);

            var typeConverter =
                (TypeConverter)Activator.CreateInstance(Type.GetType(converterAttribute.ConverterTypeName!)!);
            if(!typeConverter!.CanConvertTo(declaredType!))
                throw new ArgumentException(message);

            return typeConverter!.ConvertTo(value!, memberInfo.DeclaringType!);
        }

        private object GetValueAndConvert([NotNull] ParameterInfo parameterInfo, [NotNull] Type instance)
        {
            var argsAttribute = parameterInfo.GetCustomAttribute<ArgsAttribute>(true);
            var converterAttribute = parameterInfo.GetCustomAttribute<TypeConverterAttribute>(true);

            return argsAttribute is null ||
                   !TryGetValue(
                       (argsAttribute.FromType, instance, argsAttribute.Key ?? parameterInfo.Name),
                       out var value
                   ) ?
                null :
                converterAttribute is null ?
                    value :
                    ((TypeConverter)Activator.CreateInstance(Type.GetType(converterAttribute.ConverterTypeName!)!))!
                    .ConvertTo(value!, parameterInfo.ParameterType!);
        }
    }
}