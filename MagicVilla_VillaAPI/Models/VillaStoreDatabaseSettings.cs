namespace MagicVilla_VillaAPI.Models;

public class VillaStoreDatabaseSettings: IVillaStoreDatabaseSettings
{
    public string VillaCollectionName { get; set; } = String.Empty;
    public string ConnectionString { get; set; } = String.Empty;
    public string DatabaseName { get; set; } = String.Empty;

}