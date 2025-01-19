
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.DTO;
using ContacManager.Core.Enum;
using ContacManager.Core.Servicecontracts;


namespace ContacManager.Core.Services
{
    public class PersonsSorterService : IPersonsSorterService
    {

        //private readonly ICountriesService countriesService;
        private readonly IPersonsRepository personsRepository;
        //List<Person> persons;
        //public PersonsService(bool initilize = true)
        public PersonsSorterService(IPersonsRepository personsRepository, ICountriesAdderService countriesService)
        {

            this.personsRepository = personsRepository;
            //this.countriesService = countriesService;

            #region MockDat
            /*countriesService = new CountriesService(initilize);
            if (initilize)
            {
                persons.AddRange(new List<Person>
                {
                    new Person {
                        PersonId = Guid.Parse("9AE15A17-62E6-488E-B5DC-92B0041F4624"),
                        PersonName = "Northrup",
                        Email = "nhastilow0@utexas.edu",
                        Gender = "Male",
                        DateOfBirth = DateTime.Parse("2000-12 -28"),
                        Address = "High Crossing Point",
                        RecieveNewsLetter = false,
                        CountryId = Guid.Parse("B06B9935-3366-41E4-9253-8C5B5B023DA7")
                    },

                    new Person {
                        PersonId = Guid.Parse("A6E6E6C3-AC64-4387-91C9-3E12FEC27487"),
                        PersonName = "Beverly",
                        Email = "brichold1@godaddy.com",
                        Gender = "Male",
                        DateOfBirth = DateTime.Parse("2005-11-28"),
                        Address = "Di Loreto Center",
                        RecieveNewsLetter = true,
                        CountryId = Guid.Parse("B06B9935-3366-41E4-9253-8C5B5B023DA7")

                    },

                   
                    new Person {
                        PersonId = Guid.Parse("865729BA-715B-44EE-8760-7A28D25E3E13"),
                        PersonName = "Cristian",
                        Email = "celflain2@si.edu",
                        Gender = "Male",
                        DateOfBirth = DateTime.Parse("2001-12-06"),
                        Address = "Talisman Parkway",
                        RecieveNewsLetter = false,
                        CountryId = Guid.Parse("78D56FC0-15E8-42B3-B8AE-4527A22BF2DB")
                    },
                    new Person {
                        PersonId = Guid.Parse("94120B75-9028-4565-A9A3-68B41C08BA1D"),
                        PersonName = "Cristian",
                        Email = "twaylen3@google.co.jp",
                        Gender = "Male",
                        DateOfBirth = DateTime.Parse("2021-06-06"),
                        Address = "35234 Di Loreto Center",
                        RecieveNewsLetter = false,
                        CountryId = Guid.Parse("9E6C9DE3-FCF0-4D3C-B4C9-839B1FA4A347")
                    },
                                        new Person {
                        PersonId = Guid.Parse("80D8A1CE-5037-4CFF-87AB-15E662CC43F9"),
                        PersonName = "Conan",
                        Email = "cmasdon4 @cdc.gov",
                        Gender = "Male",
                        DateOfBirth = DateTime.Parse("2021-06-16"),
                        Address = "9780 Menomonie Alley",
                        RecieveNewsLetter = true,
                        CountryId = Guid.Parse("DD7D144F-E206-4B06-AC53-4B315C5795BC")
                    },

                                        new Person {
                        PersonId = Guid.Parse("162422A8-27EE-48BC-82D8-1946040E7477"),
                        PersonName = "Peggi",
                        Email = "pwitherspoon5@patch.com",
                        Gender = "Female",
                        DateOfBirth = DateTime.Parse("2001-01-16"),
                        Address = "9733 Anhalt Point",
                        RecieveNewsLetter = true,
                        CountryId = Guid.Parse("7F8372F5-5C22-4A43-8E0F-9BE28C69FAB3")
                    },
                                         new Person {
                        PersonId = Guid.Parse("9DCFC992-CCBE-4E61-AD9A-B4632B34930C"),
                        PersonName = "Katheryn",
                        Email = "kshedden6@shareasale.com",
                        Gender = "Female",
                        DateOfBirth = DateTime.Parse("2001-07-12"),
                        Address = "9733 Anhalt Point",
                        RecieveNewsLetter = false,
                        CountryId = Guid.Parse("7F8372F5-5C22-4A43-8E0F-9BE28C69FAB3")
                    },

                });
            

            }*/
            #endregion


        }

        public List<PersonResponse> GetSortedPersons(List<PersonResponse> allPersons, string sortBy, SortOrderOptions sortOrderOption)
        {
            /* switch (sortBy)
             {
                 case nameof(PersonResponse.PersonName):
                 {
                         if (sortOrderOption == SortOrderOptions.ASC)
                         {
                             return allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList();
                         }

                 }
                 break;
             }*/


            if (string.IsNullOrEmpty(sortBy))
            {
                return allPersons;
            }


            List<PersonResponse> sortedPersons =
                (sortBy, sortOrderOption)
            switch
                {
                    (nameof(PersonResponse.PersonName), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.PersonName), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.PersonName, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Email), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Email), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Email, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Age), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Age).ToList(),
                    (nameof(PersonResponse.Age), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Age).ToList(),
                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.DateOfBirth).ToList(),
                    (nameof(PersonResponse.DateOfBirth), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.DateOfBirth).ToList(),
                    (nameof(PersonResponse.Gender), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Gender), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Gender, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Address), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Address), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Address, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.RecieveNewsLetter), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.RecieveNewsLetter).ToList(),
                    (nameof(PersonResponse.RecieveNewsLetter), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.RecieveNewsLetter).ToList(),
                    (nameof(PersonResponse.Country), SortOrderOptions.ASC) => allPersons.OrderBy(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    (nameof(PersonResponse.Country), SortOrderOptions.DESC) => allPersons.OrderByDescending(temp => temp.Country, StringComparer.OrdinalIgnoreCase).ToList(),
                    _ => allPersons
                };
            return sortedPersons;
        }


    }
}
