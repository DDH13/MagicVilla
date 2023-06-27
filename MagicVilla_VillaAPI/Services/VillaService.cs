using MagicVilla_VillaAPI.Models;
using MongoDB.Driver;

namespace MagicVilla_VillaAPI.Services;

public class VillaService : IVillaService
{
    private readonly IMongoCollection<Villa> _villas;

    public VillaService(IDatabaseSettings settings, IMongoClient mongoClient)
    {
        var database = mongoClient.GetDatabase(settings.DatabaseName);
        _villas = database.GetCollection<Villa>("Villas");
    }
    public List<Villa> Get()
    {
        return _villas.Find(villa => true).ToList();
    }

    public Villa Get(string id)
    {
        return _villas.Find(villa => villa.Id == id).FirstOrDefault();

    }

    public Villa Create(Villa villa)
    {
        try
        {
            _villas.InsertOne(villa);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return villa;
    }

    public void Update(string id, Villa villaIn)
    {
        _villas.ReplaceOne(villa => villa.Id == id, villaIn);
    }

    public void Remove(Villa villaIn)
    {
        _villas.DeleteOne(villa => villa.Id == villaIn.Id);
    }
}