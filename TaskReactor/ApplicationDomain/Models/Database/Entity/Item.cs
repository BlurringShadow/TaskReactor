using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;

namespace ApplicationDomain.Models.Database.Entity
{
    /// <summary>
    /// Single time event
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "NotNullMemberIsNotInitialized")]
    public abstract class Item : DatabaseModel
    {
        [Required, NotNull] public string Title { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Time when it first start
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Time when it end
        /// </summary>
        [Required, Column(TypeName = nameof(SqliteType.Integer))]
        public DateTime EndTime { get; set; }
    }
}