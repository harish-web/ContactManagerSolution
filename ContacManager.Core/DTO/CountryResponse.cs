

using ContacManager.Core.Domain.Entities;

namespace ContacManager.Core.DTO
{
    public class CountryResponse
    {
        public Guid CountryID { get; set; }
        public string CountryName { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if (obj.GetType() != typeof(CountryResponse))
                return false;
            CountryResponse conResponse = (CountryResponse)obj;
            return CountryID == conResponse.CountryID && CountryName == conResponse.CountryName;
        }
        public override string ToString()
        {
            return $"CountryName ={CountryName}, CountyId ={CountryID}";
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
    public static class CountryExtensions
    {
        public static CountryResponse ToCountryResponse(this Country country)
        {
            return new CountryResponse
            {
                CountryID = country.CountryId,
                CountryName = country.CountryName,
            };
        }
    }


}
