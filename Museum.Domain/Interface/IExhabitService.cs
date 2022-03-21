using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Domain.Interface
{
    public interface IExhabitService
    {
  
        Task<IEnumerable<ExhabitDomainModel>> GetAllExhabits();
        Task<ResponseModel<ExhabitDomainModel>> GetExhabitByIdAsync(Guid id);
        Task<ResponseModel<IEnumerable<ExhabitDomainModel>>> GetExhabitsByAuditoriumId(int id);
        Task<ResponseModel<ExhabitDomainModel>> AddExhabit(ExhabitDomainModel newExhabit);
        Task<ResponseModel<ExhabitDomainModel>> UpdateExhabit(ExhabitDomainModel updateExhabit);
        Task<ResponseModel<ExhabitDomainModel>> DeleteExhabit(Guid id);

    }
}
