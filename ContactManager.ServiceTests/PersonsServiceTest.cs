using AutoFixture;
using AutoFixture.Kernel;
using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.DTO;
using ContacManager.Core.Enum;
using ContacManager.Core.Servicecontracts;
using ContacManager.Core.Services;
using EntityFrameworkCoreMock;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Services;
using System.Linq.Expressions;
using System.Reflection;
using Xunit.Abstractions;

namespace CrudTest
{
    public class PersonsServiceTest
    {
        private readonly IPersonsAdderService? personsAdderService;
        private readonly IPersonsGetterService? personsGetterService;
        private readonly IPersonsSorterService? personsSorterService;
        private readonly IPersonsUpdaterService? personsUpdaterService;
        private readonly IPersonsDeleterService? personsDeleterService;

        private readonly ICountriesAdderService countryService;
        private readonly ITestOutputHelper testOutputHelper;
        private readonly IFixture fixture;
        private readonly Mock<IPersonsRepository> _personsRepositoryMock;
        private readonly IPersonsRepository _personRepository;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;

        public PersonsServiceTest(ITestOutputHelper testOutputHelper)
        {

           // var coutriesInitialData = new List<Country>();
            var personsInitialData = new List<Person>();

            _personsRepositoryMock = new Mock<IPersonsRepository>();
            _personRepository = _personsRepositoryMock.Object;

           // _countriesRepositoryMock = new Mock<ICountriesRepository> ();
           // _countriesRepository = _countriesRepositoryMock.Object;

           // DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>
              //  (new DbContextOptionsBuilder<ApplicationDbContext>().Options);

            //ApplicationDbContext dbContext = dbContextMock.Object;
           // dbContextMock.CreateDbSetMock(temp => temp.Countries, coutriesInitialData);
           // dbContextMock.CreateDbSetMock(temp => temp.Persons, personsInitialData);

            
                
           // countryService = new CountriesService(_countriesRepository);
            personsAdderService = new PersonsAdderService(_personRepository, countryService);
            personsGetterService = new PersonsGetterService(_personRepository, countryService);
            personsSorterService = new PersonsSorterService(_personRepository, countryService);
            personsUpdaterService = new PersonsUpdaterService(_personRepository, countryService);
            personsDeleterService = new PersonsDeleterService(_personRepository, countryService);



            /* countryService = new CountriesService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
             personsService = new PersonsService(new ApplicationDbContext
                 (new DbContextOptionsBuilder<ApplicationDbContext>().Options), countryService);
            */
            this.testOutputHelper = testOutputHelper;
            this.fixture = new Fixture();
        }

        #region AddPerson
        [Fact]
        public async Task AddPerson_NullPerson_ToBeArugumentNullException()
        {
            //Arrange
            PersonAddRequest? personAddRequest = null;
            //Assert //method mocking not required since in it will return arument null excepion in service iteself
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await personsAdderService.AddPerson(personAddRequest));
        }
        [Fact]
        public async Task AddPerson_PersonNameIsNull_TobeArugumentException()
        {
             
            //Arrange
            //PersonAddRequest personAddRequest = new() { PersonName = null };
            PersonAddRequest personAddRequest = fixture.Build<PersonAddRequest>().With(temp=>temp.PersonName,null as string).Create();
            
            Person person = personAddRequest.ToPerson();

            //mocking the method
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);


            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () => await personsAdderService.AddPerson(personAddRequest));
            
        }

        //For coverting this as method mocking once the repository mocks copleted

        [Fact]
        public async Task AddPerson_PersonPropertyPersonDetails()
        {
            //Arrange
            // var personAddRequest = fixture.Create<PersonAddRequest>();

            var personAddRequest = fixture.Build<PersonAddRequest>().With(temp => temp.Email, "harish.ooty@gmail.com").Create<PersonAddRequest>();
            /* var personAddRequest = new PersonAddRequest
             {
                 PersonName =
                 "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = Guid.NewGuid(),
                 Gender = GenderOptions.Male
             };*/

            var person = personAddRequest.ToPerson();
            var personResponse_expected = person.ToPersonResponse();

            //ReturnAsync Part of septup call chain //How mocked Add method should behave when we call through
            //mocked repository
            _personsRepositoryMock.Setup(temp => temp.AddPerson(It.IsAny<Person>())).ReturnsAsync(person);



            //Act
            PersonResponse? personResponse_from_add = await personsAdderService.AddPerson(personAddRequest);

            // List<PersonResponse> personResponseList = await personsService.GetAllPersons();
            personResponse_expected.PersonID = personResponse_from_add.PersonID;


            //Assert
            // Assert.True(personResponse_from_add.PersonID != Guid.Empty);//test one

            //FluentAssertions
           personResponse_from_add.PersonID.Should().NotBe(Guid.Empty);//test one

            personResponse_from_add.PersonID.Should().Be(personResponse_expected.PersonID);
            personResponse_from_add.Should().Be(personResponse_expected);

            //Assert.Contains(personResponse_from_add, personResponseList);
        }
       /* [Fact]
        public async Task AddPerson_PersonPropertyPersonDetails()
        {
            //Arrange
            // var personAddRequest = fixture.Create<PersonAddRequest>();

            var personAddRequest = fixture.Build<PersonAddRequest>().With(temp=>temp.Email,"harish.ooty@gmail.com").Create<PersonAddRequest>();
            *//* var personAddRequest = new PersonAddRequest
             {
                 PersonName =
                 "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = Guid.NewGuid(),
                 Gender = GenderOptions.Male
             };*//*
            //Act
            PersonResponse? personResponse_from_add = await personsService.AddPerson(personAddRequest);
            List<PersonResponse> personResponseList = await personsService.GetAllPersons();
            //Assert
            Assert.True(personResponse_from_add.PersonID != Guid.Empty);

            Assert.Contains(personResponse_from_add, personResponseList);
        }*/

        #endregion

        #region GetPersonById

        [Fact]
        public async Task GetPersonByPersonId_NullPersonID_TobeNull()
        {
            //Arrange
            Guid? personId = null;

            //Act 
            PersonResponse? personResponse_from_get = await personsGetterService.GetPersonByPersionID(personId);

            //Assert
           // Assert.Null(personResponse_from_get);

            //Fluent Asset
            personResponse_from_get.Should().BeNull();
        }
        [Fact]
        public async Task GetPersonByPersonId_WithPersonId_TobeSucessFull()
        {
            //Arrange
            //var countryAddRequest = fixture.Create<CountryAddRequest>();
            // var countryAddRequest = new CountryAddRequest { CountryName = "USA" };
            //CountryResponse countryResponse = await countryService.AddCountry(countryAddRequest);

            //var personAddRequest = fixture.Build<PersonAddRequest>().With(temp => temp.Email,"harish.ooty@gmail.com").Create();

            Person person = fixture.Build<Person>().With(temp => temp.Email, "harish.ooty@gmail.com").With(temp=>temp.Country,null as Country).Create();

            PersonResponse person_response_expected = person.ToPersonResponse();

            //Act
            /* var personAddRequest = new PersonAddRequest
             {
                 PersonName =
                 "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = countryResponse.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };*/
            //Mock
            //mocking the method
            _personsRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);
            //PersonResponse personResponse_from_add = await personsService.AddPerson(personAddRequest);
            // PersonResponse person_response = personsService.AddPerson(personAddRequest);

            PersonResponse personResponse_from_get = await personsGetterService.GetPersonByPersionID(person.PersonId);

            //Assert
            //Assert.Equal(person_response_expected, personResponse_from_get);

            personResponse_from_get.Should().Be(person_response_expected);
            //Expected
            testOutputHelper.WriteLine(person_response_expected.ToString());
            //Expected
            testOutputHelper.WriteLine(personResponse_from_get.ToString());



        }


        #endregion


        #region GetAllPerson
        [Fact]
        public async Task GetAllPersons_EmptyList()
        {
            //Arrange
            // List<PersonResponse> persons =  await personsService.GetAllPersons();
            List<Person> persons = new List<Person>();
            _personsRepositoryMock.Setup(temp => temp.GetAllPerson()).ReturnsAsync(persons);

            //Act
            List<PersonResponse> personsResponse_from_getAll = await personsGetterService.GetAllPersons();

            //Assert
            //Assert.Empty(persons);
            //Fluent Assert
            personsResponse_from_getAll.Should().BeEmpty();
        }
        [Fact]
        public async Task GetAllPersons_AddSomePersons_ToBeSuccessFul()
        {
            //Arrange
            /*var request1 = new CountryAddRequest { CountryName = "Usa" };
            var request2 = new CountryAddRequest { CountryName = "China" };*/

            /* var request1 = fixture.Create<CountryAddRequest>();
             var request2 = fixture.Create<CountryAddRequest>();
             CountryResponse conres = await countryService.AddCountry(request1);
             CountryResponse conres2 = await countryService.AddCountry(request2);*/

            var persons = new List<Person>
            {
                fixture.Build<Person>().With(temp => temp.Email , "harish2.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish3.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish4.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish5.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish6.ooty@gmail.com").Create()

            };
            List<PersonResponse> personsResponse_expected = persons.Select(per => per.ToPersonResponse()).ToList();  

            /* var personAddRequest = new PersonAddRequest
             {
                 PersonName =
                 "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conres.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             var personAddRequest2 = new PersonAddRequest
             {
                 PersonName =
                 "Harish2",
                 Email = "harish2.ooty@gmail.com",
                 Address = "xxxxxxxxx2",
                 CountryId = conres2.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-02-01")
             };*/

            /*  PersonResponse pr1 = await personsService.AddPerson(personAddRequest);
              PersonResponse pr2 = await personsService.AddPerson(personAddRequest2);
              var list_from_add = new List<PersonResponse>();
              list_from_add.Add(pr1);
              list_from_add.Add(pr2);*/

            _personsRepositoryMock.Setup(temp => temp.GetAllPerson()).ReturnsAsync(persons);
            
            //Act
            List<PersonResponse> list_from_get = await personsGetterService.GetAllPersons();

            testOutputHelper.WriteLine("Expected:");
            foreach (var personInExpected in personsResponse_expected)
            {
                testOutputHelper.WriteLine(personInExpected.ToString());
            }

            testOutputHelper.WriteLine("Actual:");
            foreach (var personInActual in list_from_get)
            {
               testOutputHelper.WriteLine( personInActual.ToString() ); 
            }

            //Fluent Assert
            list_from_get.Should().BeEquivalentTo( personsResponse_expected );

        }


        #endregion

        #region getfilteredPersons
        [Fact]
        public async Task GetFilteredPesons_EmptyString()//will return all persons empty string search
        {
            /* //Arrange
             var request1 = new CountryAddRequest { CountryName = "Usa" };
             var request2 = new CountryAddRequest { CountryName = "China" };

             CountryResponse conres = await countryService.AddCountry(request1);
             CountryResponse conres2 = await countryService.AddCountry(request2);

             var personAddRequest = new PersonAddRequest
             {
                 PersonName =
                 "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conres.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             var personAddRequest2 = new PersonAddRequest
             {
                 PersonName =
                 "Harish2",
                 Email = "harish2.ooty@gmail.com",
                 Address = "xxxxxxxxx2",
                 CountryId = conres2.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-02-01")
             };

             PersonResponse pr1 = await personsService.AddPerson(personAddRequest);
             PersonResponse pr2 = await personsService.AddPerson(personAddRequest2);

             var list_from_add = new List<PersonResponse>();
             list_from_add.Add(pr1);
             list_from_add.Add(pr2);

             List<PersonResponse> list_from_get_serach = await personsService.GetFilteredPersons(nameof(Person.PersonName),"");

             foreach (PersonResponse? pr in list_from_add)
             {
                 Assert.Contains(pr, list_from_get_serach);
             }*/
           // List<PersonResponse> list_from_get_serach = await personsService.GetFilteredPersons(nameof(Person.PersonName), "");
            var persons = new List<Person>
            {
                fixture.Build<Person>().With(temp => temp.Email , "harish2.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish3.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish4.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish5.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish6.ooty@gmail.com").Create()

            };
            List<PersonResponse> personsResponse_expected = persons.Select(per => per.ToPersonResponse()).ToList();
            testOutputHelper.WriteLine("Expected:");
            
            foreach (var personInExpected in personsResponse_expected)
            {
                testOutputHelper.WriteLine(personInExpected.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person,bool>>>())).ReturnsAsync(persons);
           /* _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync((Expression<Func<Person, bool>> filter) =>
        persons.AsQueryable().Where(filter).ToList());*/
            //
            List<PersonResponse> list_from_get_serach = await personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "");

            testOutputHelper.WriteLine("Actual:");
            foreach (var personInActual in list_from_get_serach)
            {
                testOutputHelper.WriteLine(personInActual.ToString());
            }

            //Fluent Assert
            list_from_get_serach.Should().BeEquivalentTo(personsResponse_expected);

        }

        [Fact]
        public async Task GetFildteredPersons_SearchByName()//will return all persons matching string
        {
            /* //Arrange
             var request1 = new CountryAddRequest { CountryName = "Usa" };
             var request2 = new CountryAddRequest { CountryName = "China" };

             CountryResponse conres = await countryService.AddCountry(request1);
             CountryResponse conres2 = await countryService.AddCountry(request2);

             var personAddRequest = new PersonAddRequest
             {
                 PersonName = "Mary",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conres.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             var personAddRequest2 = new PersonAddRequest
             {
                 PersonName =
                 "Harish2",
                 Email = "harish2.ooty@gmail.com",
                 Address = "xxxxxxxxx2",
                 CountryId = conres2.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-02-01")
             };

             PersonResponse pr1 = await personsService.AddPerson(personAddRequest);
             PersonResponse pr2 = await personsService.AddPerson(personAddRequest2);

             var list_from_add = new List<PersonResponse>();
             list_from_add.Add(pr1);
             list_from_add.Add(pr2);

             List<PersonResponse>? list_from_get_serach = await personsService.GetFilteredPersons(nameof(Person.PersonName), "ma");

             foreach (PersonResponse? pr in list_from_add)
             {
                 if(pr.PersonName.Contains("ma",StringComparison.OrdinalIgnoreCase))
                 Assert.Contains(pr, list_from_get_serach);
             }*/

            var persons = new List<Person>
            {
                fixture.Build<Person>().With(temp => temp.Email , "harish2.ooty@gmail.com").With(temp =>temp.PersonName,"hkas").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish3.ooty@gmail.com").With(temp =>temp.PersonName,"hai").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish4.ooty@gmail.com").With(temp =>temp.PersonName,"caa").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish5.ooty@gmail.com").With(temp =>temp.PersonName,"had").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish6.ooty@gmail.com").With(temp =>temp.PersonName,"has").Create(),

            };
            List<PersonResponse> personsResponse_expected = persons.Select(per => per.ToPersonResponse()).ToList();

            //List<PersonResponse>  personsResponse_expected1 = personsResponse_expected.Where(personResponse => personResponse.PersonName.Contains("ha")).ToList();
            testOutputHelper.WriteLine("Expected:");

            foreach (var personInExpected in personsResponse_expected)
            {
                testOutputHelper.WriteLine(personInExpected.ToString());
            }

          
            _personsRepositoryMock.Setup(temp => temp.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>())).ReturnsAsync(persons);
           /* _personsRepositoryMock.Setup(repo => repo.GetFilteredPersons(It.IsAny<Expression<Func<Person, bool>>>()))
    .ReturnsAsync((Expression<Func<Person, bool>> filter) =>
        persons.AsQueryable().Where(filter).ToList());*/

            //
            List<PersonResponse> list_from_get_serach = await personsGetterService.GetFilteredPersons(nameof(Person.PersonName), "ha");

            testOutputHelper.WriteLine("Actual:");
            foreach (var personInActual in list_from_get_serach)
            {
                testOutputHelper.WriteLine(personInActual.ToString());
            }

            //Fluent Assert
            list_from_get_serach.Should().BeEquivalentTo(personsResponse_expected);

        }

        #endregion
        #region
        [Fact]
        public async Task GetSortedPersons()
        {
            /* //Arrange
             var request1 = new CountryAddRequest { CountryName = "Usa" };
             var request2 = new CountryAddRequest { CountryName = "China" };

             CountryResponse conres = await countryService.AddCountry(request1);
             CountryResponse conres2 = await countryService.AddCountry(request2);

             var personAddRequest = new PersonAddRequest
             {
                 PersonName = "Mary",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conres.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             var personAddRequest2 = new PersonAddRequest
             {
                 PersonName =
                 "Harish2",
                 Email = "harish2.ooty@gmail.com",
                 Address = "xxxxxxxxx2",
                 CountryId = conres2.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-02-01")
             };

             PersonResponse pr1 = await personsService.AddPerson(personAddRequest);
             PersonResponse pr2 = await personsService.AddPerson(personAddRequest2);

             var list_from_add = new List<PersonResponse>();
             list_from_add.Add(pr1);
             list_from_add.Add(pr2);

             //Act
             List<PersonResponse>? allPersons = await personsService.GetAllPersons();
             List<PersonResponse>? list_from_get_Sort = await personsService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

             list_from_add = list_from_add.OrderByDescending(temp => temp.PersonName).ToList();


             *//* foreach (PersonResponse? pr in list_from_add)
              {
                  if (pr.PersonName.Contains("ma", StringComparison.OrdinalIgnoreCase))
                      Assert.Contains(pr, list_from_get_Sort);
              }*//*
             for (int i = 0; i < list_from_add.Count; i++)
             {
                 Assert.Equal(list_from_add[i], list_from_get_Sort[i]);
             }*/

            var persons = new List<Person>
            {
                fixture.Build<Person>().With(temp => temp.Email , "harish1.ooty@gmail.com").With(temp =>temp.PersonName,"ha").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish5.ooty@gmail.com").With(temp =>temp.PersonName,"ah").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish3.ooty@gmail.com").With(temp =>temp.PersonName,"cb").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish2.ooty@gmail.com").With(temp =>temp.PersonName,"so").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish6.ooty@gmail.com").With(temp =>temp.PersonName,"pe").Create(),

            };
            List<PersonResponse> personsResponse_expected = persons.Select(per => per.ToPersonResponse()).ToList();
            personsResponse_expected = personsResponse_expected.OrderByDescending(temp => temp.PersonName).ToList();
            testOutputHelper.WriteLine("Expected:");

            foreach (var personInExpected in personsResponse_expected)
            {
                testOutputHelper.WriteLine(personInExpected.ToString());
            }

            _personsRepositoryMock.Setup(temp => temp.GetAllPerson()).ReturnsAsync(persons);

            List<PersonResponse>? allPersons = await personsGetterService.GetAllPersons();
            List<PersonResponse>? list_from_get_Sort = personsSorterService.GetSortedPersons(allPersons, nameof(Person.PersonName), SortOrderOptions.DESC);

            testOutputHelper.WriteLine("Actual:");
            foreach (var personInActual in list_from_get_Sort)
            {
                testOutputHelper.WriteLine(personInActual.ToString());
            }

            //Fluent Assert
            list_from_get_Sort.Should().BeEquivalentTo(personsResponse_expected);
            //list_from_get_Sort.Should().BeInDescendingOrder(temp=>temp.PersonName);

        }
        #endregion
        #region
        [Fact]
        public void UpudatePerson_NullPerson()
        {
            //Arrange 
            PersonUpdateRequest? personUpdateRequest = null;
            //Act
            personsUpdaterService.UpdatePerson(personUpdateRequest);

            Assert.Null(personUpdateRequest);
        }

        [Fact]
        public async Task UdatePerson_InvalidPersonId_ArugumentException()
        {
            //Arrange 
            var personUpdateRequest = new PersonUpdateRequest
            {
                PersonID = Guid.NewGuid()
            };


            await Assert.ThrowsAsync<ArgumentException>(async () => await personsUpdaterService.UpdatePerson(personUpdateRequest));

        }
        [Fact]
        public async Task UpudatePerson_PersonNameNull()
        {
            //Arrange 
            /*var countryAddRequest = new CountryAddRequest { CountryName ="Uk" };
            var conRes = await countryService.AddCountry(countryAddRequest);
        var personAddRequest = new PersonAddRequest
            {
                PersonName = "John",
                //CountryId = conRes.CountryID,
                Email = "harish.ooty@gmail.com",
                Gender = GenderOptions.Male,
                //Addresss   = "xxxxxxxxxxxxxxx"


            };
            PersonResponse? perRes = await personsService?.AddPerson(personAddRequest);
            PersonUpdateRequest? perUdadteRequestFormResponseObj = perRes?.ToPersonUdpateRequest();
            perUdadteRequestFormResponseObj.PersonName = null;
           

            //Assert
           await Assert.ThrowsAsync<ArgumentException>(async () => await personsService.UpdatePerson(perUdadteRequestFormResponseObj));*/

            var persons = fixture.Build<Person>().With(temp => temp.Email, "harish1.ooty@gmail.com").With(temp => temp.PersonName,null as string).With(gen => gen.Gender, GenderOptions.Male.ToString()).Create();
            PersonResponse? perRes = persons.ToPersonResponse();
            PersonUpdateRequest? perUdadteRequestFormResponseObj = perRes?.ToPersonUdpateRequest();
            perUdadteRequestFormResponseObj.PersonName = null;

            var action = async () => await personsUpdaterService.UpdatePerson(perUdadteRequestFormResponseObj);
            await action.Should().ThrowAsync<ArgumentException>();  
        }
        [Fact]
        public async Task UpudatePerson_PersonNameAndEmail()
        {
            /* //Arrange 
             CountryAddRequest countryAddRequest = new() { CountryName = "Uk" };
             CountryResponse conRes = await countryService.AddCountry(countryAddRequest);
             var personAddRequest = new PersonAddRequest
             {
                 PersonName = "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conRes.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             PersonResponse? perRes = await personsService.AddPerson(personAddRequest);
             PersonUpdateRequest perUdadteRequestFormResponseObj = perRes.ToPersonUdpateRequest();
             perUdadteRequestFormResponseObj.PersonName = "Williams";
             perUdadteRequestFormResponseObj.Email = "william@gmail.com";

             //Act
             PersonResponse PersonResUpdate = await personsService.UpdatePerson(perUdadteRequestFormResponseObj);

             PersonResponse PersonResponseById = await personsService.GetPersonByPersionID(PersonResUpdate.PersonID);

             //Assert
             Assert.Equal(PersonResponseById, PersonResUpdate);*/


            var person = fixture.Build<Person>().With(temp => temp.Email, "harish1.ooty6666@gmail.com").With(temp => temp.PersonName, "ha").With(gen => gen.Gender, GenderOptions.Male.ToString()).Create();
            PersonResponse? perRes = person.ToPersonResponse();

            PersonUpdateRequest? perUdadteRequestFormResponseObj = perRes?.ToPersonUdpateRequest();
            _personsRepositoryMock.Setup(temp => temp.UpdatePerson()).ReturnsAsync(1);

            _personsRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);



         
             PersonResponse person_response_update =    await personsUpdaterService.UpdatePerson(perUdadteRequestFormResponseObj);


            person_response_update.Should().Be(person_response_update);
            #endregion

        }
            #region deletePerson
        [Fact]
        public async Task  DeletPerson_InvalidPersonId()
        {
            //Arrange
           /* CountryAddRequest countryAddRequest = new CountryAddRequest() { CountryName = "Uk" };
            CountryResponse conRes = countryService.AddCountry(countryAddRequest);
            PersonAddRequest? personAddRequest = new PersonAddRequest()
            {
                PersonName = "Harish",
                Email = "harish.ooty@gmail.com",
                Addresss = "xxxxxxxxx",
                CountryId = conRes.CountryID,
                Gender = GenderOptions.Male,
                RecieveNewsLetter = false,
                DateOfBirth = DateTime.Parse("2020-01-01")
            };
            PersonResponse perRes = personsService.AddPerson(personAddRequest);*/
            Guid personId = Guid.NewGuid();
            //Act
            bool isDeleted = await personsDeleterService.DeletePerson(personId);

            //Assert
            Assert.False(isDeleted);


        }
        [Fact]
        public async Task  DeletPerson_ValidPersonId()
        {

            var persons = new List<Person>
            {
                fixture.Build<Person>().With(temp => temp.Email , "harish1.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish5.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish3.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish2.ooty@gmail.com").Create(),
                fixture.Build<Person>().With(temp => temp.Email , "harish6.ooty@gmail.com").Create()

            };

            var person = persons.First();



            //Arrange
            /* var countryAddRequest = new CountryAddRequest { CountryName = "Uk" };
             CountryResponse conRes = await countryService.AddCountry(countryAddRequest);
             var personAddRequest = new PersonAddRequest
             {
                 PersonName = "Harish",
                 Email = "harish.ooty@gmail.com",
                 Address = "xxxxxxxxx",
                 CountryId = conRes.CountryID,
                 Gender = GenderOptions.Male,
                 RecieveNewsLetter = false,
                 DateOfBirth = DateTime.Parse("2020-01-01")
             };
             PersonResponse perRes = await personsService.AddPerson(personAddRequest);*/
            _personsRepositoryMock.Setup(temp => temp.DeletePersonByPersonId(It.IsAny<Guid>())).ReturnsAsync(true);
            _personsRepositoryMock.Setup(temp => temp.GetPersonById(It.IsAny<Guid>())).ReturnsAsync(person);

            //Act
            bool isDeleted =  await personsDeleterService.DeletePerson(person.PersonId);

            //Assert
            //Assert.True(isDeleted);
            isDeleted.Should().BeTrue();


        }

        #endregion
    }
}
