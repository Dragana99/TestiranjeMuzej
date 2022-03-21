using Museum.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Museum.Domain.Interface
{
    public interface IExhibitionService
    {
        Task<IEnumerable<ExhibitionDomainModel>> GetAllAsync();
        Task<CreateExhibitionResultModel> CreateExhibition(ExhibitionDomainModel domainModel);
        Task<ResponseModel<ExhibitionDomainModel>> Delete(Guid id);
        Task<ResponseModel<ExhibitionDomainModel>> Update(ExhibitionDomainModel exhibitionDomainModel);
        Task<ResponseModel<ExhibitionDomainModel>> GetExhibitionById(Guid id);
        //Task<ResponseModel<IEnumerable<ExhibitionDomainModel>>> GetExhibitionsFiltered(FilterExhibitionsModel filterExhibitionsModel);
    }
}
