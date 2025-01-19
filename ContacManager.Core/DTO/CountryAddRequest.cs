

using ContacManager.Core.Domain.Entities;

namespace ContacManager.Core.DTO
{
    public class CountryAddRequest
    {
        public string CountryName { get; set; } = string.Empty;

        public Country ToCountry()
        {
            return new Country()
            {

                CountryName = CountryName
            };
        }
    }
}
