namespace MagicVilla_VillaAPI.Models;

public interface IVillaStoreDatabaseSettings
{
    string VillaCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    
}