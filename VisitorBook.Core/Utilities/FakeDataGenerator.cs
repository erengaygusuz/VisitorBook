using Bogus;
using Bogus.Extensions;
using VisitorBook.Core.Enums;
using VisitorBook.Core.Models;

namespace VisitorBook.DAL.Data
{
    public class FakeDataGenerator
    {
        public List<Visitor> Visitors = new();
        public List<VisitedCounty> VisitedCounties = new();
        public List<VisitorAddress> VisitorAddresses = new();
        public List<City> Cities = new();
        public List<County> Counties = new();

        public void GenerateData(int populateConstant = 1)
        {
            if (!Cities.Any())
            {
                Cities.AddRange(GetCityGenerator().Generate(30 * populateConstant));
            }

            if (!Counties.Any())
            {
                Counties.AddRange(GetCountyGenerator().Generate(900 * populateConstant));
            }

            if (!VisitedCounties.Any())
            {
                Visitors.AddRange(GetVisitorGenerator().Generate(100 * populateConstant));

                Random random = new Random();

                Visitors.ForEach(e => e.VisitorAddressId = (random.Next(0, 2) == 0 ? GetVisitorAddressData().Id : null));
            }

            if (!VisitedCounties.Any())
            {
                var visitedCounties = GetVisitedCountyGenerator().Generate(200 * populateConstant);

                VisitedCounties.AddRange(visitedCounties);
            }
        }

        public Faker<City> GetCityGenerator()
        {
            return new Faker<City>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
                .RuleFor(v => v.Code, f => f.Address.ZipCode())
                .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        }

        public Faker<County> GetCountyGenerator()
        {
            return new Faker<County>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
                .RuleFor(v => v.Latitude, f => f.Address.Latitude())
                .RuleFor(v => v.Longitude, f => f.Address.Longitude())
                .RuleFor(v => v.CityId, f => f.PickRandom(Cities).Id)
                .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        }

        public Faker<Visitor> GetVisitorGenerator()
        {
            return new Faker<Visitor>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.Name, f => f.Person.FirstName)
                .RuleFor(v => v.Surname, f => f.Person.LastName)
                .RuleFor(v => v.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(v => v.Gender, f => f.PickRandom<Gender>())
                .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        }

        public VisitorAddress GetVisitorAddressData()
        {
            var visitorAddressGenerator = GetVisitorAddressGenerator();

            var generatedVisitorAddress = visitorAddressGenerator.Generate();

            VisitorAddresses.Add(generatedVisitorAddress);

            return generatedVisitorAddress;
        }

        public Faker<VisitorAddress> GetVisitorAddressGenerator()
        {
            return new Faker<VisitorAddress>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.CountyId, f => f.PickRandom(Counties).Id)
                .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        }

        public Faker<VisitedCounty> GetVisitedCountyGenerator()
        {
            return new Faker<VisitedCounty>()
                .RuleFor(v => v.Id, f => Guid.NewGuid())
                .RuleFor(v => v.VisitorId, f => f.PickRandom(Visitors.Where(a => a.VisitorAddressId != null)).Id)
                .RuleFor(v => v.CountyId, f => f.PickRandom(Counties).Id)
                .RuleFor(v => v.VisitDate, f => f.Date.Between(new DateTime(day: 1, month: 1, year: 2000), new DateTime(day: 2, month: 11, year: 2023)))
                .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        }
    }
}
