namespace ApplicationDomain.Database.Entity
{
    public class KeyedEntity<T> : DatabaseModel, IIdentityKey<T>
    {
        public T Id { get; set; }
        object IIdentityKey.Id => Id;
    }
}