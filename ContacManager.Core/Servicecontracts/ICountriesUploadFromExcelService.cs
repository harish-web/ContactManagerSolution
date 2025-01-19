using Microsoft.AspNetCore.Http;

namespace ContacManager.Core.Servicecontracts
{
    public interface ICountriesUploadFromExcelService
    {
      
        Task<int> UploadFromExcelFile(IFormFile formFile);
       
    }
}
