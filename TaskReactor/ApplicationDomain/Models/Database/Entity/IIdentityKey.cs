namespace ApplicationDomain.Models.Database.Entity
{
    public interface IIdentityKey : IDatabaseModel
    {
        int Id { get; set; }
    }
}