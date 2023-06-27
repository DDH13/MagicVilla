namespace MagicVilla_VillaAPI.Models;

public interface IDatabaseSettings
{
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
    
}