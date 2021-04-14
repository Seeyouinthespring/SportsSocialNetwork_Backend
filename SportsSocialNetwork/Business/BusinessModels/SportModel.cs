namespace SportsSocialNetwork.Business.BusinessModels
{
    public class SportDtoModel
    {
        public string Name { get; set; }
    }

    public class SportViewModel : SportDtoModel
    {
        public long Id { get; set; }
    }
}
