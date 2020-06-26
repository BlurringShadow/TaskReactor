using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace Data.Database.Entity
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public class UserTask : KeyedEntity<int>, IItem
    {
        [Required, NotNull, JsonIgnore] public User OwnerUser { get; set; }

        [InverseProperty(nameof(Goal.FromTask))]
        public IList<Goal> Goals { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}