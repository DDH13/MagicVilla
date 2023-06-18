using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Services;

public interface IVillaService
{
    List<Villa> Get();
    Villa Get(string id);
    Villa Create(Villa villa);
    void Update(string id, Villa villaIn);
    void Remove(Villa villaIn);
    
}