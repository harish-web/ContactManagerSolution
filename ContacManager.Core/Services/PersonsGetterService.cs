using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.DTO;
using ContacManager.Core.Servicecontracts;
using CsvHelper;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Reflection;


namespace ContacManager.Core.Services
{
    public class PersonsGetterService : IPersonsGetterService
    {

        //private readonly ICountriesService countriesService;
        private readonly IPersonsRepository personsRepository;
        //List<Person> persons;
        //public PersonsService(bool initilize = true)
        public PersonsGetterService(IPersonsRepository personsRepository, ICountriesAdderService countriesService)
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

        public async Task<List<PersonResponse>> GetAllPersons()
        {

            //return persons.Select(person => ConvertPersonToPersonResponse(person)).ToList();
            /*The client projection contains a reference to a constant expression of 
                'Services.PersonsService' through 
                the instance method 'ConvertPersonToPersonResponse'.This could potentially cause a memory leak*/
            //var persons = await personsRepository.Persons.Include("Country").ToListAsync();
            var persons = await personsRepository.GetAllPerson();
            return persons.Select(person => person.ToPersonResponse()).ToList();
            //return personsRepository.Sp_GetAllPersons().Select(person => ConvertPersonToPersonResponse(person)).ToList();
        }

        public async Task<PersonResponse?> GetPersonByPersionID(Guid? personID)
        {
            if (!personID.HasValue)
            {
                return null;

            }
            //Person? person = await personsRepository..Persons.Include("Country").FirstOrDefaultAsync(temp => temp.PersonId == personID);
            Person? person = await personsRepository.GetPersonById(personID.Value);

            if (person == null)
            {
                return null;
            }
            return person.ToPersonResponse();
        }

        public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<Person> result = searchBy switch
            {
                nameof(PersonResponse.PersonName) => await personsRepository.GetFilteredPersons(temp => temp.PersonName.Contains(searchString)),
                nameof(PersonResponse.Email) => await personsRepository.GetFilteredPersons(temp => temp.Email.Contains(searchString)),
                // nameof(PersonResponse.DateOfBirth) => await personsRepository.GetFilteredPersons(temp => temp.DateOfBirth.Contains(searchString, StringComparison.OrdinalIgnoreCase)),
                nameof(PersonResponse.Address) => await personsRepository.GetFilteredPersons(temp => temp.Address.Contains(searchString)),
                nameof(PersonResponse.Gender) => await personsRepository.GetFilteredPersons(temp => string.Equals(temp.Gender, searchString)),
                nameof(PersonResponse.CountryId) => await personsRepository.GetFilteredPersons(temp => temp.Country.CountryName.Contains(searchString)),
                _ => await personsRepository.GetFilteredPersons(temp => true)
            };

            return result.Select(temp => temp.ToPersonResponse()).ToList();

        }


        /*public async Task<List<PersonResponse>> GetFilteredPersons(string searchBy, string? searchString)
        {
            List<PersonResponse> allPersons = await GetAllPersons();
            List<PersonResponse> matchingPersons = allPersons;

            if(string.IsNullOrEmpty(searchString) || string.IsNullOrEmpty(searchBy))
            {
                return matchingPersons;
            }

            /*var result = searchBy switch
            {
                nameof(PersonResponse.PersonName) => allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName) ? temp.PersonName.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList(),
                _ => throw new NotImplementedException()
            };



            switch (searchBy)
            {
                case nameof(PersonResponse.PersonName):

                      matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.PersonName)?temp.PersonName.Contains(searchString,StringComparison.OrdinalIgnoreCase):true)).ToList(); 
                 break;
                case nameof(PersonResponse.Email):

                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Email) ? temp.Email.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                   
                    
                    break;
                case nameof(PersonResponse.Address):

                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Address) ? temp.Address.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.Gender):

                    //matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? temp.Gender.Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Gender) ? string.Equals(temp.Gender, searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;
                case nameof(PersonResponse.CountryId):
                    
                    matchingPersons = allPersons.Where(temp => (!string.IsNullOrEmpty(temp.Country.ToString()) ? temp.Country.ToString().Contains(searchString, StringComparison.OrdinalIgnoreCase) : true)).ToList();
                    break;


            }
            return matchingPersons;
        }*/


        /*        public async Task<MemoryStream> GetPersonsCSV()
                {
                    MemoryStream memoryStream = new MemoryStream();
                    StreamWriter stream = new StreamWriter(memoryStream);
                    CsvWriter csvWriter = new CsvWriter(stream,System.Globalization.CultureInfo.InvariantCulture,leaveOpen:true);
                    csvWriter.WriteHeader<PersonResponse>();
                    await csvWriter.NextRecordAsync();//next line
                    List<PersonResponse> personsResponse = personsRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();
                    await csvWriter.WriteRecordsAsync(personsResponse); 
                    memoryStream.Position = 0;
                    return memoryStream;

                }*/
        public async Task<MemoryStream> GetPersonsCSV()
        {
            MemoryStream memoryStream = new MemoryStream();
            StreamWriter stream = new StreamWriter(memoryStream);

            CsvConfiguration csvConfiguration = new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture);
            CsvWriter csvWriter = new CsvWriter(stream, csvConfiguration);
            csvWriter.WriteField(nameof(PersonResponse.PersonName));
            csvWriter.WriteField(nameof(PersonResponse.Email));
            csvWriter.WriteField(nameof(PersonResponse.DateOfBirth));
            csvWriter.WriteField(nameof(PersonResponse.Age));
            csvWriter.WriteField(nameof(PersonResponse.Gender));
            csvWriter.WriteField(nameof(PersonResponse.Country));
            csvWriter.WriteField(nameof(PersonResponse.Address));
            csvWriter.WriteField(nameof(PersonResponse.RecieveNewsLetter));
            await csvWriter.NextRecordAsync();//next line
            //List<PersonResponse> personsResponse = personsRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();
            List<PersonResponse> personsResponse = await GetAllPersons();
            foreach (var person in personsResponse)
            {
                csvWriter.WriteField(person.PersonName);
                csvWriter.WriteField(person.Email);
                if (person.DateOfBirth.HasValue)
                {
                    var val = person.DateOfBirth.HasValue ? person.DateOfBirth.Value.ToString("yyyy-MM-dd") : "";
                    csvWriter.WriteField(val);
                }
                csvWriter.WriteField(person.Age);
                csvWriter.WriteField(person.Gender);
                csvWriter.WriteField(person.Country);
                csvWriter.WriteField(person.Address);
                csvWriter.WriteField(person.RecieveNewsLetter);
                await csvWriter.NextRecordAsync();
                await csvWriter.FlushAsync();
            }

            memoryStream.Position = 0;
            return memoryStream;

        }

        public async Task<MemoryStream> GetPesonsExcel()
        {
            var memoryStream = new MemoryStream();

            using (ExcelPackage excelPackage = new ExcelPackage(memoryStream))
            {
                //List<PersonResponse> personsResponse = personsRepository.Persons.Include("Country").Select(temp => temp.ToPersonResponse()).ToList();
                List<PersonResponse> personsResponse = await GetAllPersons();
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.Add("Persons");
                int row = 1;
                int column = 1;


                var properties = typeof(PersonResponse).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                foreach (var prop in properties)
                {

                    worksheet.Cells[row, column].Value = prop.Name;
                    Console.WriteLine($"Property Name: {prop.Name}");
                    column++;
                }

                row = 2;
                column = 1;
                foreach (var personResponse in personsResponse)
                {
                    foreach (var property in properties)
                    {
                        // Get the value of the property for the current instance
                        var value = property.GetValue(personResponse);

                        // Example of writing value to a worksheet (assuming you have a worksheet object)
                        worksheet.Cells[row, column].Value = value; // Replace 'worksheet' with your Excel worksheet object

                        Console.WriteLine($"Property Name: {property.Name}, Value: {value}");
                        column++;
                    }
                    column = 1;
                    row++;
                }


                //worksheet.Cells[$"A1:J{row}"].AutoFitColumns();
                await excelPackage.SaveAsync();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }
    }
}
