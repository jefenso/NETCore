using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Services
{
    public class SeedDataService : ISeedDataService
    {
        public void Initialize(HoloIDDbContext context)
        {
            context.HoloIDItems.Add(new HoloIDEntity() { Gen = 1, Group = "Area 15", Name = "Moona Hoshinova", Created = DateTime.Now });
            context.HoloIDItems.Add(new HoloIDEntity() { Gen = 1, Group = "Area 15", Name = "Ayunda Risu", Created = DateTime.Now });
            context.HoloIDItems.Add(new HoloIDEntity() { Gen = 2, Group = "Holoro", Name = "Pavolia Reine", Created = DateTime.Now });
            context.HoloIDItems.Add(new HoloIDEntity() { Gen = 3, Group = "HoloH3ro", Name = "Kaela Kovalskia", Created = DateTime.Now });

            context.SaveChanges();
        }
    }
}
