
using ContacManager.Core.DTO;
using ContacManager.Core.Enum;

namespace ContacManager.Core.Servicecontracts
{
    public interface IPersonsSorterService
    {

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOption);



    }
}
