using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Data.Database.Entity
{
    /// <summary>
    /// Each task contains several goals to complete
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public sealed class Goal : KeyedEntity<int>, ISchedule
    {
        [Required, NotNull, JsonIgnore] public UserTask FromTask { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public TimeSpan DurationOfOneTime { get; set; }

        public Interval Interval { get; set; }
    }
}