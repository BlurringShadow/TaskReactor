#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 25
// Time: 下午 9:56

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Utilities
{
    public static class EnumExtension
    {
        [NotNull]
        public static IEnumerable<TEnum> GetValues<TEnum>() where TEnum : Enum =>
            Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
    }
}