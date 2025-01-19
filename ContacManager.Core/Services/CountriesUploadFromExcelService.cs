
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using ContacManager.Core.Domain.RepositoryContracts;
using ContacManager.Core.Domain.Entities;
using ContacManager.Core.Servicecontracts;

namespace ContacManager.Core.Services
{
    public class CountriesUploadFromExcelService : ICountriesUploadFromExcelService
    {
        private readonly ICountriesRepository countriesReposiory;

        //List<Country> _countries;
        //public CountriesService(bool initialize = true)
        public CountriesUploadFromExcelService(ICountriesRepository countriesReposiory)
        {
            this.countriesReposiory = countriesReposiory;


            #region ForMockData
            /* _countries = new List<Country>();
             if (initialize)
             {

              _countries.AddRange(new List<Country>
              {
                 new Country
                 {
                     CountryId = Guid.Parse("B06B9935-3366-41E4-9253-8C5B5B023DA7"),
                     CountryName = "India"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("78D56FC0-15E8-42B3-B8AE-4527A22BF2DB"),
                     CountryName = "USA"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("9E6C9DE3-FCF0-4D3C-B4C9-839B1FA4A347"),
                     CountryName = "China"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("DD7D144F-E206-4B06-AC53-4B315C5795BC"),
                     CountryName = "UK"
                 },
                 new Country
                 {
                     CountryId = Guid.Parse("7F8372F5-5C22-4A43-8E0F-9BE28C69FAB3"),
                     CountryName = "Russia"
                 },
              });

             }*/

            #endregion


        }
        public async Task<int> UploadFromExcelFile(IFormFile formFile)
        {
            MemoryStream memoryStream = new MemoryStream();
            int countriesInserted = 0;
            await formFile.CopyToAsync(memoryStream);
            using (var excelPackage = new ExcelPackage(memoryStream))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Countries"];
                if (worksheet == null)
                {


                    int rowCount = worksheet.Dimension.Rows;
                    int columnCount = worksheet.Dimension.Columns;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        for (int col = 1; col <= 1; col++)
                        {
                            string? CellValue = worksheet.Cells[row, col].Value.ToString();
                            if (!string.IsNullOrEmpty(CellValue))
                            {
                                string countryName = CellValue.Trim();
                                // int countFromDb = countriesReposiory.Countries.Where(country => country.CountryName == countryName).Count();
                                int countFromDb = (await countriesReposiory.GetAllCountries()).Where(country => country.CountryName == countryName).Count();
                                if (countFromDb == 0)
                                {
                                    /* countriesReposiory.Countries.Add(new Country
                                     {
                                         CountryName = countryName,
                                     });
                                     await countriesReposiory.SaveChangesAsync();*/
                                    await countriesReposiory.AddCountry(new Country
                                    {
                                        CountryName = countryName,
                                    });
                                    countriesInserted++;
                                }
                            }
                        }

                    }
                }
            }


            return countriesInserted;
        }

    }
}
