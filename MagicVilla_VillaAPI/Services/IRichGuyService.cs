using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Services;

public interface IRichGuyService
{
    List<RichGuy> Get();
    RichGuy Get(string id);
    RichGuy Create(RichGuy richGuy);
    void Update(string id, RichGuy richGuyIn);
    void Remove(RichGuy richGuyIn);
    
}