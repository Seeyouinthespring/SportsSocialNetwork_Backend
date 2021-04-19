using SportsSocialNetwork.Helpers;
using Newtonsoft.Json;

namespace SportsSocialNetwork.Business.Enums
{
    [JsonConverter(typeof(StringToEnumConverter))]
    public enum TypeOfBuilding : byte 
    {
        None = 0,
        Roof = 1,
        Sides = 2,
        Grid = 3,
        Hall = 4
    }

    //[JsonConverter(typeof(StringEnumConverter))]
    [JsonConverter(typeof(StringToEnumConverter))]
    public enum TypeOfCovering : byte
    {
        Parquet = 0,
        NaturalLawn = 1,
        ArtificialTurf = 2,
        Rubber = 3,
        Priming = 4,
        ArtificialIce = 5,
        SyntheticIce = 6,
        NaturalIce = 7,
        Tile = 8
    }

    [JsonConverter(typeof(StringToEnumConverter))]
    public enum VisitingStatus : byte
    {
        PlanningToVisit = 0,
        ExactlyVisit = 1,
    }

}
