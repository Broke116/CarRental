namespace CarRental.Web.Structure.Mapper
{
    public class AutoMapperConfig
    {
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(x =>
            { 
                x.AddProfile<MapperTranslator>();
            });
        }
    }
}