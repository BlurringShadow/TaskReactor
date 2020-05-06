using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using JetBrains.Annotations;
using TaskReactor.Utilities;

namespace TaskReactor.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class Schedule : IDataBaseModel
    {
        [Required, NotNull] public User OwnerUser { get; set; }

        [Required] public int OwnerUserId { get; set; }

        [Required, NotNull] public string Title { get; set; }

        [Required, Column(TypeName = nameof(SqlDbType.BigInt))]
        public DateTime StartTime { get; set; }

        [Required, Column(TypeName = nameof(SqlDbType.BigInt))]
        public TimeSpan DurationOfOneTime { get; set; }

        [Required, Column(TypeName = nameof(SqlDbType.BigInt))]
        public DateTime EndTime { get; set; }

        [NotMapped] public TimeSpan StartTimeOfDay => StartTime.TimeOfDay;

        [NotMapped] public TimeSpan EndTimeOfDay => StartTimeOfDay + DurationOfOneTime;

        [Required, NotNull] public Interval Interval { get; set; }
    }
}