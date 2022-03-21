using Microsoft.EntityFrameworkCore;
using Museum.Data.Context;
using Museum.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Repositories
{
    public interface IExhibitionRepository : IRepository<ExhibitionEntity>
    {
        IEnumerable<ExhibitionEntity> GetByAuditoriumId(int salaId);
        IEnumerable<ExhibitionEntity> GetByExhabitId(Guid id);
    }

    public class ExhibitionsRepository : IExhibitionRepository
    {
        private MuseumContext _museumContext;

        public ExhibitionsRepository(MuseumContext museumContext)
        {
            _museumContext = museumContext;
        }

        public ExhibitionEntity Delete(object id)
        {
            ExhibitionEntity existing = _museumContext.Exhibition.Find(id);
            var result = _museumContext.Exhibition.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<ExhibitionEntity>> GetAll()
        {
            var data = await _museumContext.Exhibition.Include(x => x.Exhabit).Include(x => x.Auditorium).ToListAsync();

            return data;
        }

        public async Task<ExhibitionEntity> GetByIdAsync(object id)
        {
            return await _museumContext.Exhibition.Include(x => x.Exhabit).Include(x => x.Auditorium).SingleOrDefaultAsync(x => x.Id == (Guid)id);
        }

        public IEnumerable<ExhibitionEntity> GetByAuditoriumId(int auditoriumId)
        {
            var projectionsData = _museumContext.Exhibition.Include(x => x.Exhabit).Where(x => x.AuditoriumId == auditoriumId);

            return projectionsData;
        }

        public IEnumerable<ExhibitionEntity> GetByExhabitId(Guid id)
        {
            var projectionsData = _museumContext.Exhibition.Where(x => x.ExhabitId == id);
            return projectionsData;
        }

        public ExhibitionEntity Insert(ExhibitionEntity obj)
        {
            var data = _museumContext.Exhibition.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _museumContext.SaveChanges();
        }

        public ExhibitionEntity Update(ExhibitionEntity obj)
        {
            var updatedEntry = _museumContext.Exhibition.Attach(obj).Entity;
            _museumContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}