
using ContacManager.Core.DTO;


namespace ContacManager.Core.Servicecontracts
{
    public interface IPersonsGetterService
    {


        public Task<List<PersonResponse>> GetAllPersons();

        public Task<PersonResponse?> GetPersonByPersionID(Guid? personID);

        public Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString);

        public Task<MemoryStream> GetPersonsCSV();

        public Task<MemoryStream> GetPesonsExcel();

    }
}
