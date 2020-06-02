using System;
using Caliburn.Micro;

namespace ApplicationDomain.DataModel
{
    interface IModel : INotifyPropertyChangedEx
    {
        Type InstanceType { get; }
    }
}