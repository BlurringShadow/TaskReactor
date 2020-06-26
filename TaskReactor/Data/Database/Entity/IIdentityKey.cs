namespace Data.Database.Entity
{
    interface IIdentityKey : IDatabaseModel
    {
        object Id { get; }
    }

    interface IIdentityKey<T> : IIdentityKey
    {
        new T Id { get; set; }
    }
}