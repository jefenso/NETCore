namespace SampleWebApiAspNetCore.Entities
{
    public class HoloIDEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Group { get; set; }
        public int Gen { get; set; }
        public DateTime Created { get; set; }
    }
}
