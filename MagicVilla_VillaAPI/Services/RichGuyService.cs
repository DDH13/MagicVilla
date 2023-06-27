using MagicVilla_VillaAPI.Models;
using MongoDB.Driver;

namespace MagicVilla_VillaAPI.Services;

public class RichGuyService : IRichGuyService
{
    private readonly IMongoCollection<RichGuy> _richGuys;

    public RichGuyService(IDatabaseSettings settings)
    {
        var client = new MongoClient(settings.ConnectionString);
        var database = client.GetDatabase(settings.DatabaseName);

        _richGuys = database.GetCollection<RichGuy>("RichGuys");
    }

    public List<RichGuy> Get() =>
        _richGuys.Find(richGuy => true).ToList();

    public RichGuy Get(string id) =>
        _richGuys.Find<RichGuy>(richGuy => richGuy.Id == id).FirstOrDefault();

    public RichGuy Create(RichGuy richGuy)
    {
        _richGuys.InsertOne(richGuy);
        return richGuy;
    }

    public void Update(string id, RichGuy richGuyIn) =>
        _richGuys.ReplaceOne(richGuy => richGuy.Id == id, richGuyIn);

    public void Remove(RichGuy richGuyIn) =>
        _richGuys.DeleteOne(richGuy => richGuy.Id == richGuyIn.Id);

    public void Remove(string id) => 
        _richGuys.DeleteOne(richGuy => richGuy.Id == id);
}