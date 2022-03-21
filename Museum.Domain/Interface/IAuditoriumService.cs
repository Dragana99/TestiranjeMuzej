using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Domain.Interface
{
    public interface IAuditoriumService
    {
        Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync();
        Task<AuditoriumDomainModel> GetAuditoriumByIdAsync(int id);
        Task<IEnumerable<AuditoriumDomainModel>> GetAuditoriumByMuseumId(int museumId);
        Task<AuditoriumDomainModel> DeleteAuditorium(int id);
        Task<IEnumerable<AuditoriumDomainModel>> DeleteAuditoriumByMuseumId(int museumId);
        Task<CreateAuditoriumResultModel> CreateAuditorium(AuditoriumDomainModel domainModel);
        Task<AuditoriumDomainModel> UpdateAuditorium(AuditoriumDomainModel updateAuditorium);

    }
}
