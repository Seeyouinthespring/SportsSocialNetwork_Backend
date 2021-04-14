using AutoMapper;

namespace SportsSocialNetwork.Helpers
{
    internal static class MapHelper
    {
        public static TDestination MapTo<TDestination>(this object source)
        {
            return Mapper.Map<TDestination>(source);
        }
    }
}
