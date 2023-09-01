using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Models;

namespace SampleWebApiAspNetCore.Repositories
{
    public interface IHoloIDRepository
    {
        HoloIDEntity GetSingle(int id);
        void Add(HoloIDEntity item);
        void Delete(int id);
        HoloIDEntity Update(int id, HoloIDEntity item);
        IQueryable<HoloIDEntity> GetAll(QueryParameters queryParameters);
        ICollection<HoloIDEntity> GetRandomHoloID();
        int Count();
        bool Save();
    }
}
