using MagicVilla_VillaAPI.Models;
namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<Villa> villaList = new List<Villa>
        {
            new Villa { Id = "1", Name = "Pool View" ,Sqft = 1000, Occupancy = 4},
            new Villa { Id = "2", Name = "Beach View" ,Sqft = 1200, Occupancy = 6},
        };

    }
}
