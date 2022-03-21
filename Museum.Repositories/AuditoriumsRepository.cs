using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Museum.Data;
using Museum.Data.Context;
using Museum.Data.Entities;

namespace Museum.Repositories
{
    public interface IAuditoriumsRepository : IRepository<AuditoriumEntity>
    {
        Task<IEnumerable<AuditoriumEntity>> GetByAuditName(string name, int id);
        Task<IEnumerable<AuditoriumEntity>> GetByMuseumId(int museumId);
        Task<IEnumerable<AuditoriumEntity>> DeleteByMuseumId(int museumId);
    }
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private MuseumContext _museumContext;

        public AuditoriumsRepository(MuseumContext museumContext)
        {
            _museumContext = museumContext;
        }


        public async Task<IEnumerable<AuditoriumEntity>> GetByAuditName(string name, int id)
        {
            var data = await _museumContext.Auditoriums.Where(x => x.Name.Equals(name) && x.MuseumId.Equals(id)).ToListAsync();
            return data;
        }

        public AuditoriumEntity Delete(object id)
        {
            AuditoriumEntity existing = _museumContext.Auditoriums.Find(id);
            var result = _museumContext.Auditoriums.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<AuditoriumEntity>> GetAll()
        {
            var data = await _museumContext.Auditoriums.ToListAsync();

            return data;
        }

        public async Task<AuditoriumEntity> GetByIdAsync(object id)
        {
            return await _museumContext.Auditoriums.FindAsync(id);
        }

        public AuditoriumEntity Insert(AuditoriumEntity obj)
        {
            var data = _museumContext.Auditoriums.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _museumContext.SaveChanges();
        }

        public AuditoriumEntity Update(AuditoriumEntity obj)
        {
            var updatedEntry = _museumContext.Auditoriums.Attach(obj);
            _museumContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<IEnumerable<AuditoriumEntity>> GetByMuseumId(int museumId)
        {
            var result = _museumContext.Auditoriums.Where(x => x.MuseumId == museumId);
            return result;
        }

        public async Task<IEnumerable<AuditoriumEntity>> DeleteByMuseumId(int museumId)
        {
            IEnumerable<AuditoriumEntity> existing = _museumContext.Auditoriums.Where(x => x.MuseumId == museumId);
            List<AuditoriumEntity> result = new List<AuditoriumEntity>();
            foreach (AuditoriumEntity auditorium in existing)
            {
                var data = _museumContext.Remove(auditorium);
                result.Add(data.Entity);
            }

            return result;
        }
    }
}
