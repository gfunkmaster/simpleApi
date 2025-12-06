
namespace SimpleApi.src.Models
{
    public class Address(string street, string city, string zipCode)
    {
        public string Street { get; set; } = street;
        public string City { get; set; } = city;
        public string ZipCode { get; set; } = zipCode;
    }
}