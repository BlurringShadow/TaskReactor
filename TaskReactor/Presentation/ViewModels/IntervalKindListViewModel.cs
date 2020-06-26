#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 25
// Time: 下午 9:31

#endregion

using System;
using System.Linq;
using Caliburn.Micro;
using Data.Database.Entity;
using JetBrains.Annotations;
using Utilities;

namespace Presentation.ViewModels
{
    public class IntervalKindListViewModel : Screen
    {
        [NotNull] public BindableCollection<string> PresentStrCollection { get; } = new BindableCollection<string>();

        private int _selected;

        public int Selected { get => _selected; set => Set(ref _selected, value); }

        public IntervalKindListViewModel()
        {
            var values = EnumExtension.GetValues<IntervalKind>().ToArray();

            PresentStrCollection.Add(KindToString(null));
            PresentStrCollection.AddRange(new string[values.Length]);

            foreach (var value in values)
                PresentStrCollection[(int)value] = KindToString(value);
        }

        public static string KindToString(IntervalKind? kind) =>
            kind switch
            {
                IntervalKind.YearByWeek => "隔几年这周",
                IntervalKind.YearByDay => "隔几年这天",
                IntervalKind.MonthByWeek => "隔几个月这周",
                IntervalKind.MonthByDay => "隔几个月这天",
                IntervalKind.ByWeek => "隔几周这天",
                IntervalKind.ByDay => "隔几天",
                null => "单次",
                _ => throw new ArgumentOutOfRangeException()
            };
    }
}