using System.ComponentModel.DataAnnotations;

namespace SampleWebApiAspNetCore.Dtos
{
    public class HoloIDCreateDto
    {
        [Required]
        public string? Name { get; set; }
        public string? Group { get; set; }
        public int Gen { get; set; }
        public DateTime Created { get; set; }
    }
}
