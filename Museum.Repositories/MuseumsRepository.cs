using Microsoft.EntityFrameworkCore;
using Museum.Data.Context;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Repositories
{
    public interface IMuseumsRepository : IRepository<Data.Entities.MuseumEntity>
    {
        Task<Data.Entities.MuseumEntity> GetByMuseumName(string name);
    }

    public class MuseumsRepository : IMuseumsRepository
    {
        private MuseumContext _museumsContext;

        public MuseumsRepository(MuseumContext museumsContext)
        {
            _museumsContext = museumsContext;
        }

        public Data.Entities.MuseumEntity Delete(object id)
        {
            Data.Entities.MuseumEntity existing = _museumsContext.Museum.Find(id);
            var result = _museumsContext.Museum.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Data.Entities.MuseumEntity>> GetAll()
        {
            var data = await _museumsContext.Museum.ToListAsync();

            return data;
        }

        public async Task<Data.Entities.MuseumEntity> GetByIdAsync(object id)
        {
            var data = await _museumsContext.Museum.FindAsync(id);

            return data;
        }

        public Data.Entities.MuseumEntity Insert(Data.Entities.MuseumEntity obj)
        {
            return _museumsContext.Museum.Add(obj).Entity;
        }

        public void Save()
        {
            _museumsContext.SaveChanges();
        }

        public Data.Entities.MuseumEntity Update(Data.Entities.MuseumEntity obj)
        {
            var updatedEntry = _museumsContext.Museum.Attach(obj);
            _museumsContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }

        public async Task<Data.Entities.MuseumEntity> GetByMuseumName(string name)
        {
            var data = await _museumsContext.Museum.SingleOrDefaultAsync(x => x.Name == name);

            return data;
        }
    }
}