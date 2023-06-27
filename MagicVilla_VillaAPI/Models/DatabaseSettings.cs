namespace MagicVilla_VillaAPI.Models;

public class DatabaseSettings: IDatabaseSettings
{
    public string ConnectionString { get; set; } = String.Empty;
    public string DatabaseName { get; set; } = String.Empty;

}