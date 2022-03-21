using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Domain.Interface
{
    public interface IMuseumService
    {
        Task<IEnumerable<MuseumDomainModel>> GetAllAsync();
        Task<MuseumDomainModel> GetMuseumByIdAsync(int id);
        Task<CreateMuseumResultModel> AddMuseum(MuseumDomainModel newMuseum);
        Task<MuseumDomainModel> UpdateMuseum(MuseumDomainModel updateMuseum);
        Task<MuseumDomainModel> DeleteMuseum(int museumId);
    }
}
