#region File Info

// Created by BlurringShadow,
// Year: 2020
// Month: 06
// Day: 23
// Time: 下午 10:45

#endregion

using System;
using JetBrains.Annotations;

namespace ApplicationDomain.ModelService
{
    /// <summary>
    /// Provide schedule notification service for <see cref="TModel"/>
    /// </summary>
    interface IScheduleNotificationService<TModel>
    {
        /// <summary>
        /// Check the model is registered
        /// </summary>
        /// <returns> Return true if registered </returns>
        public bool ContainsModel([NotNull] TModel t);

        /// <summary>
        /// Add a action. <para/>
        /// If model already exists than return false and do nothing.
        /// </summary>
        /// <param name="t"> The model </param>
        /// <param name="notifyAction"> Provided notify action </param>
        /// <returns>Return false if model already exists</returns>
        public void AddModelAction([NotNull] TModel t, [NotNull] Action<TModel> notifyAction);

        /// <summary>
        /// Update the action <para/>
        /// </summary>
        /// <param name="t"> The model </param>
        /// <param name="notifyAction"> Provided notify action </param>
        public void UpdateModelAction([NotNull] TModel t, [NotNull] Action<TModel> notifyAction);

        /// <summary>
        /// Remove the model registeration
        /// </summary>
        public void RemoveModel([NotNull] TModel t);
    }
}