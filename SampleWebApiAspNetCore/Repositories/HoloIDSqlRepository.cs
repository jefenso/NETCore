using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Models;
using System.Linq.Dynamic.Core;

namespace SampleWebApiAspNetCore.Repositories
{
    public class HoloIDSqlRepository : IHoloIDRepository
    {
        private readonly HoloIDDbContext _holoIDDbContext;

        public HoloIDSqlRepository(HoloIDDbContext holoIDDbContext)
        {
            _holoIDDbContext = holoIDDbContext;
        }

        public HoloIDEntity GetSingle(int id)
        {
            return _holoIDDbContext.HoloIDItems.FirstOrDefault(x => x.Id == id);
        }

        public void Add(HoloIDEntity item)
        {
            _holoIDDbContext.HoloIDItems.Add(item);
        }

        public void Delete(int id)
        {
            HoloIDEntity holoIDItem = GetSingle(id);
            _holoIDDbContext.HoloIDItems.Remove(holoIDItem);
        }

        public HoloIDEntity Update(int id, HoloIDEntity item)
        {
            _holoIDDbContext.HoloIDItems.Update(item);
            return item;
        }

        public IQueryable<HoloIDEntity> GetAll(QueryParameters queryParameters)
        {
            IQueryable<HoloIDEntity> _allItems = _holoIDDbContext.HoloIDItems.OrderBy(queryParameters.OrderBy,
              queryParameters.IsDescending());

            if (queryParameters.HasQuery())
            {
                _allItems = _allItems
                    .Where(x => x.Gen.ToString().Contains(queryParameters.Query.ToLowerInvariant())
                    || x.Name.ToLowerInvariant().Contains(queryParameters.Query.ToLowerInvariant()));
            }

            return _allItems
                .Skip(queryParameters.PageCount * (queryParameters.Page - 1))
                .Take(queryParameters.PageCount);
        }

        public int Count()
        {
            return _holoIDDbContext.HoloIDItems.Count();
        }

        public bool Save()
        {
            return (_holoIDDbContext.SaveChanges() >= 0);
        }

        public ICollection<HoloIDEntity> GetRandomMeal()
        {
            List<HoloIDEntity> toReturn = new List<HoloIDEntity>();

            toReturn.Add(GetRandomItem("Area 15"));
            toReturn.Add(GetRandomItem("Holoro"));
            toReturn.Add(GetRandomItem("HoloH3ro"));

            return toReturn;
        }

        private HoloIDEntity GetRandomItem(string type)
        {
            return _holoIDDbContext.HoloIDItems
                .Where(x => x.Group == type)
                .OrderBy(o => Guid.NewGuid())
                .FirstOrDefault();
        }

        public ICollection<HoloIDEntity> GetRandomHoloID()
        {
            throw new NotImplementedException();
        }
    }
}
