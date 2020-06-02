using System;
using ApplicationDomain.Database.Entity;
using JetBrains.Annotations;

namespace ApplicationDomain.DataModel
{
    public abstract class ItemModel<TItem> : Model<TItem> where TItem : class, IItem, new()
    {
        protected ItemModel([NotNull] TItem item) : base(item)
        {
        }

        protected ItemModel()
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