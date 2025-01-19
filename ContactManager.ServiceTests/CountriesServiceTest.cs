using AutoFixture;
using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.DTO;
using ContacManager.Core.Servicecontracts;
using ContacManager.Core.Services;

using FluentAssertions;
using Moq;

using Xunit.Abstractions;
using Xunit.Sdk;


namespace CrudTest
{
    public  class CountriesServiceTest
    {
        private readonly ICountriesAdderService countriesAdderService;
        private readonly ICountriesGetterService countriesGetterService;
        private readonly ICountriesUploadFromExcelService countriesUploadFromExcelService;
        private readonly Mock<ICountriesRepository> _countriesRepositoryMock;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IFixture fixture;
        private readonly ITestOutputHelper testOutputHelper;   

        public CountriesServiceTest(ITestOutputHelper testOutputHelper)
        {
          /*  var coutriesInitialData = new List<Country>();
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>
                (new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            
            ApplicationDbContext dbContext = dbContextMock.Object;
            dbContextMock.CreateDbSetMock(temp => temp.Countries, coutriesInitialData);*/


            _countriesRepositoryMock = new Mock<ICountriesRepository>();
            _countriesRepository = _countriesRepositoryMock.Object;

            //var dbContext = new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options);
            //countriesService = new CountriesService(dbContext);
            countriesAdderService = new CountriesAdderService(_countriesRepository);
            countriesGetterService = new CountriesGetterService(_countriesRepository);
            
            countriesUploadFromExcelService = new CountriesUploadFromExcelService(_countriesRepository);

            this.testOutputHelper = testOutputHelper;
            fixture = new Fixture();

        }

        #region AddCountry
        [Fact]
        public async Task AddCountry_NullCountry()
        {

            //Arrange
            CountryAddRequest request = null;

            Func<Task> action = async () => { await countriesAdderService.AddCountry(request); };

          /*  //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await countriesService.AddCountry(request);
            });
*/
           await action.Should().ThrowAsync<ArgumentNullException>();

          
        }
        [Fact]
        public async Task AddCountry_DuplicateCountry()
        {
            //Arrange
            /*var request1 = new CountryAddRequest { CountryName = "Usa"};
            var request2 = new CountryAddRequest { CountryName = "Usa" };*/

            var country_expected = fixture.Build<CountryAddRequest>().With(temp => temp.CountryName, "Usa").Create();

            _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<String>())).ReturnsAsync(country_expected.ToCountry());

           // _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(country_expected.ToCountry());




            //Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await countriesAdderService.AddCountry(country_expected);

            });
           
            Assert.Equal("Country name already exist",exception.ParamName);
            // Assert
            // Verify GetCountryByCountryName was called
          //  mockRepository.Verify(repo => repo.GetCountryByCountryName(countryName), Times.Once);

            // Verify AddCountry was called
           // mockRepository.Verify(repo => repo.AddCountry(It.Is<Country>(c => c.Name == countryName)), Times.Once);

        }
        [Fact]
        public async Task AddCountry_CountryNameNull()
        {
            //Arrange
            var request = new CountryAddRequest { CountryName = null, };
            
            //Assertion
            await Assert.ThrowsAsync<ArgumentNullException>(async () => 
            { 
                //Act
              await countriesAdderService.AddCountry(request);

            } );
            

        }

        [Fact]
        public async Task AddCountry_PropertyCountryDetails()
        {
            //Arrange
           // var request = new CountryAddRequest { CountryName = "Japan", };
           var countryAddreq = fixture.Create<CountryAddRequest>();

            _countriesRepositoryMock.Setup(temp => temp.GetCountryByCountryName(It.IsAny<String>())).ReturnsAsync(null as Country);

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(countryAddreq.ToCountry());

             testOutputHelper.WriteLine("Expected :"+ countryAddreq.ToCountry().ToString() );

           //Act
            CountryResponse countryResponse =  await countriesAdderService.AddCountry(countryAddreq);


           
           // List<CountryResponse> countries_from_getAllCountries =  await countriesService.GetAllCountries();

            //Assert
           Assert.True(countryResponse.CountryID != Guid.Empty);

            testOutputHelper.WriteLine("Actual :" + countryResponse.ToString());
            // Assert
            // Verify GetCountryByCountryName was called
            _countriesRepositoryMock.Verify(repo => repo.GetCountryByCountryName(countryResponse.CountryName), Times.Once);

            // Verify AddCountry was called
            _countriesRepositoryMock.Verify(repo => repo.AddCountry(It.Is<Country>(c => c.CountryName == countryResponse.CountryName)), Times.Once);


            // Assert.Contains(countryResponse, countries_from_getAllCountries);

            /*  var countryName = "Canada";
              var countryAddRequest = new CountryAddRequest { CountryName = countryName };

              // Simulate that the country does not exist
              mockRepository
                  .Setup(repo => repo.GetCountryByCountryName(countryName))
                  .ReturnsAsync((Country)null); // No country exists

              // Mock AddCountry to simulate success
              mockRepository
                  .Setup(repo => repo.AddCountry(It.IsAny<Country>()))
                  .Returns(Task.CompletedTask); // Simulate a successful add

              var countriesService = new CountriesService(mockRepository.Object);

              // Act
              await countriesService.AddCountry(countryAddRequest);

              // Assert
              // Verify GetCountryByCountryName was called
              mockRepository.Verify(repo => repo.GetCountryByCountryName(countryName), Times.Once);

              // Verify AddCountry was called
              mockRepository.Verify(repo => repo.AddCountry(It.Is<Country>(c => c.Name == countryName)), Times.Once);*/

        }
        #endregion

        #region GetAllCountries
        [Fact]
        public async Task getAllCountries_EmptyList()
        {

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(new List<Country>());
            //Arrange And Act
            List<CountryResponse> actualCountriesResponseList = await countriesGetterService.GetAllCountries();
            
            //Assert
            Assert.Empty(actualCountriesResponseList);

            // Verify GetAllCountries was called once
            _countriesRepositoryMock.Verify(repo => repo.GetAllCountries(), Times.Once);

        }
       [ Fact]
        public async Task getAllCountries_AddFewCountries()
        {
            //arrange 
            var countryList = new List<Country>
            {
                fixture.Create<Country>(),
                fixture.Create<Country>(),
                fixture.Create<Country>(),
            };
            // Mock behavior: "USA" already exists
           // _countriesRepositoryMock.Setup(repo => repo.GetCountryByCountryName(It.IsAny<String>())).ReturnsAsync((Country country) => country);

            // "Canada" does not exist
            _countriesRepositoryMock.Setup(repo => repo.GetCountryByCountryName(It.IsAny<String>())).ReturnsAsync(null as Country);


            _countriesRepositoryMock.Setup(repo => repo.AddCountry(It.IsAny<Country>())).ReturnsAsync((Country country) => country);

            _countriesRepositoryMock.Setup(temp => temp.GetAllCountries()).ReturnsAsync(countryList);

      /*      // Assert
            Assert.NotNull(result);
            Assert.Equal(countriesToAdd.Count, result.Count); // Ensure same number of countries are returned
            foreach (var country in countriesToAdd)
            {
                var addedCountry = result.FirstOrDefault(c => c.Name == country.CountryName);
                Assert.NotNull(addedCountry); // Ensure that the country was added
                Assert.Equal(country.CountryName, addedCountry.Name); // Ensure the country name matches
            }

            // Verify AddCountry was called for each country
            mockRepository.Verify(repo => repo.AddCountry(It.IsAny<Country>()), Times.Exactly(countriesToAdd.Count));*/

            List<CountryResponse> country_all_response = await countriesGetterService.GetAllCountries();
            country_all_response.Select(c=>c.CountryName).Should().BeEquivalentTo(countryList.Select(c=>c.CountryName));

          /*  // Verify GetCountryByCountryName was called for both countries
            _countriesRepositoryMock.Verify(repo => repo.GetCountryByCountryName("USA"), Times.Once);
            _countriesRepositoryMock.Verify(repo => repo.GetCountryByCountryName("Canada"), Times.Once);

            // Verify AddCountry was not called for "USA" but called for "Canada"
            _countriesRepositoryMock.Verify(repo => repo.AddCountry(It.Is<Country>(c => c.CountryName == "USA")), Times.Never);
            _countriesRepositoryMock.Verify(repo => repo.AddCountry(It.Is<Country>(c => c.CountryName == "Canada")), Times.Never);*/ // Won't be reached in exception flow

           
            
            
            
            //Arrage
            /*var country_request_list = new List<CountryAddRequest>
            {
                new() { CountryName = "USA" },
                new() { CountryName = "Japan" }
            };

            //Act
            var country_list_from_addCountries = new List<CountryResponse>();

            foreach (CountryAddRequest country in country_request_list)
            {
                country_list_from_addCountries.Add(await countriesService.AddCountry(country));
            }
            List<CountryResponse> actual_country_response_list = await countriesService.GetAllCountries();

            //Assert
            foreach (CountryResponse expected_country in country_list_from_addCountries)
            {
                Assert.Contains(expected_country, actual_country_response_list);
            }*/
        }
        #endregion

        #region GetCountryByCountryId

        [Fact]
        public async Task GetCountryByCountryId_NullCountryId()
        {
            //Arrange
            Guid? countryId = null;

            //Act
            CountryResponse? country_response_from_get_method = await countriesGetterService.GetCountryByCountryId(countryId);

            //Assert 
            Assert.Null(country_response_from_get_method);

        }
        [Fact]
        public async Task GetCountryByCountryId_ValidCountryId()
        {
            var countryAddreq = fixture.Create<CountryAddRequest>();
           


            _countriesRepositoryMock.Setup(temp => temp.GetcountryByCountryId(It.IsAny<Guid>())).ReturnsAsync(countryAddreq.ToCountry);

            _countriesRepositoryMock.Setup(temp => temp.AddCountry(It.IsAny<Country>())).ReturnsAsync(countryAddreq.ToCountry());

           CountryResponse resultCountry = await countriesAdderService.AddCountry(countryAddreq);
        
            CountryResponse? conRes = await countriesGetterService.GetCountryByCountryId(resultCountry.CountryID);

            Assert.Equal(resultCountry.CountryName, conRes.CountryName);
            //Arrange
            /* var countryAddRequest = new CountryAddRequest()
             { CountryName = "China" };

             CountryResponse country_res_by_add = await countriesService.AddCountry(countryAddRequest);

             //Act 
             CountryResponse? conRes = await countriesService.GetCountryByCountryId(country_res_by_add.CountryID);

             Assert.Equal(country_res_by_add, conRes);*/
        }
        #endregion



    }
}
