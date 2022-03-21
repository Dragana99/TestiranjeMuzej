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
    public interface IExhabitRepository : IRepository<ExhabitEntity>
    {
        Task<IEnumerable<ExhabitEntity>> GetByName(string name);
        Task<IEnumerable<ExhabitEntity>> GetByYear(int year);
    }

    public class ExhabitsRepository : IExhabitRepository
    {
        private MuseumContext _museumContext;

        public ExhabitsRepository(MuseumContext museumContext)
        {
            _museumContext = museumContext;
        }

        public ExhabitEntity Delete(object id)
        {
            ExhabitEntity existing = _museumContext.Exhabit.Find(id);

            if (existing == null)
            {
                return null;
            }

            var result = _museumContext.Exhabit.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<ExhabitEntity>> GetAll()
        {
            return await _museumContext.Exhabit.ToListAsync();
        }

        public async Task<ExhabitEntity> GetByIdAsync(object id)
        {
            var data = await _museumContext.Exhabit.FindAsync(id);

            return data;
        }

        public async Task<IEnumerable<ExhabitEntity>> GetByName(string name)
        {
            var data = await _museumContext.Exhabit.Where(x => x.Name.Contains(name)).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<ExhabitEntity>> GetByYear(int year)
        {
            var data = await _museumContext.Exhabit.Where(x => x.Year == year).ToListAsync();
            return data;
        }

        public ExhabitEntity Insert(ExhabitEntity obj)
        {
            var data = _museumContext.Exhabit.Add(obj).Entity;
            return data;
        }

        public void Save()
        {
            _museumContext.SaveChanges();
        }

        public ExhabitEntity Update(ExhabitEntity obj)
        {
            var updatedEntry = _museumContext.Exhabit.Attach(obj).Entity;
            _museumContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
