using System;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.Models
{
    public abstract class ItemModel<TItem> : Model<TItem> where TItem : Item, new()
    {
        protected ItemModel([NotNull] TItem item) : base(item)
        {
        }

        [NotNull] public virtual string Title
        {
            get => _dataBaseModel.Title;
            set
            {
                _dataBaseModel.Title = value;
                NotifyOfPropertyChange();
            }
        }

        public virtual DateTime StartTime
        {
            get => _dataBaseModel.StartTime;
            set
            {
                _dataBaseModel.StartTime = value;
                NotifyOfPropertyChange();
            }
        }

        public virtual DateTime EndTime
        {
            get => _dataBaseModel.EndTime;
            set
            {
                _dataBaseModel.EndTime = value;
                NotifyOfPropertyChange();
            }
        }
    }
}