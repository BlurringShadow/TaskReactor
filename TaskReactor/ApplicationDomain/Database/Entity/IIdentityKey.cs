namespace ApplicationDomain.Database.Entity
{
    public interface IIdentityKey : IDatabaseModel
    {
        int Id { get; set; }
    }
}