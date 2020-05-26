namespace ApplicationDomain.Models.Database.Entity
{
    public interface IIdentityKey : IDataBaseModel
    {
        int Id { get; set; }
    }
}