using Bogus;
using Bogus.Extensions;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;

namespace VisitorBook.Core.Utilities
{
    public class FakeDataGenerator
    {
        public List<User> Users = new();
        public List<VisitedCounty> VisitedCounties = new();
        public List<UserAddress> VisitorAddresses = new();
        public List<City> Cities = new();
        public List<County> Counties = new();
        public List<Region> Regions = new();
        public List<SubRegion> SubRegions = new();
        public List<Country> Countries = new();
        public List<Role> Roles = new();

        public void GenerateData(int populateConstant = 1)
        {
            //#region Real Data Generation 

            //if (!Roles.Any())
            //{
            //    Roles.AddRange(GetRoleGenerator().Generate(30 * populateConstant));
            //}

            //if (!Regions.Any())
            //{
            //    Regions.AddRange(GetRegionGenerator().Generate(30 * populateConstant));
            //}

            //if (!SubRegions.Any())
            //{
            //    SubRegions.AddRange(GetSubRegionGenerator().Generate(30 * populateConstant));
            //}

            //if (!Countries.Any())
            //{
            //    Countries.AddRange(GetCountryGenerator().Generate(30 * populateConstant));
            //}

            //if (!Cities.Any())
            //{
            //    Cities.AddRange(GetCityGenerator().Generate(30 * populateConstant));
            //}

            //if (!Counties.Any())
            //{
            //    Counties.AddRange(GetCountyGenerator().Generate(900 * populateConstant));
            //}

            //#endregion

            //#region Fake Data Generation 

            //if (!Users.Any())
            //{
            //    Users.AddRange(GetUserGenerator().Generate(30 * populateConstant));
            //}

            //if (!VisitedCounties.Any())
            //{
            //    Visitors.AddRange(GetVisitorGenerator().Generate(100 * populateConstant));

            //    Random random = new Random();

            //    Visitors.ForEach(e => e.VisitorAddressId = (random.Next(0, 2) == 0 ? GetVisitorAddressData().Id : null));
            //}

            //if (!VisitedCounties.Any())
            //{
            //    var visitedCounties = GetVisitedCountyGenerator().Generate(200 * populateConstant);

            //    VisitedCounties.AddRange(visitedCounties);
            //}

            //#endregion
        }

        //public Faker<Region> GetRegionGenerator()
        //{
        //    return new Faker<Region>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<SubRegion> GetSubRegionGenerator()
        //{
        //    return new Faker<SubRegion>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<Country> GetCountryGenerator()
        //{
        //    return new Faker<Country>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.Code, f => f.Address.ZipCode())
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<Role> GetRoleGenerator()
        //{
        //    return new Faker<Role>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower());
        //}

        //public Faker<User> GetUserGenerator()
        //{
        //    return new Faker<User>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => f.Person.FirstName)
        //        .RuleFor(v => v.Surname, f => f.Person.LastName)
        //        .RuleFor(v => v.BirthDate, f => f.Person.DateOfBirth)
        //        .RuleFor(v => v.Gender, f => f.PickRandom<Gender>())
        //        .RuleFor(v => v.UserName, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.NormalizedUserName, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.Email, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.NormalizedEmail, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.EmailConfirmed, f => f.Random.Bool())
        //        .RuleFor(v => v.NormalizedEmail, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower());
        //}

        //public Faker<City> GetCityGenerator()
        //{
        //    return new Faker<City>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.Code, f => f.Address.ZipCode())
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<County> GetCountyGenerator()
        //{
        //    return new Faker<County>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.Name, f => char.ToUpper(f.Lorem.Word().ClampLength(5, 8)[0]) + f.Lorem.Word().ClampLength(5, 8).Substring(1).ToLower())
        //        .RuleFor(v => v.Latitude, f => f.Address.Latitude())
        //        .RuleFor(v => v.Longitude, f => f.Address.Longitude())
        //        .RuleFor(v => v.CityId, f => f.PickRandom(Cities).Id)
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<Visitor> GetVisitorGenerator()
        //{
        //    return new Faker<Visitor>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.UserId, f => f.PickRandom(Users).Id)
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public VisitorAddress GetVisitorAddressData()
        //{
        //    var visitorAddressGenerator = GetVisitorAddressGenerator();

        //    var generatedVisitorAddress = visitorAddressGenerator.Generate();

        //    VisitorAddresses.Add(generatedVisitorAddress);

        //    return generatedVisitorAddress;
        //}

        //public Faker<VisitorAddress> GetVisitorAddressGenerator()
        //{
        //    return new Faker<VisitorAddress>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.CountyId, f => f.PickRandom(Counties).Id)
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}

        //public Faker<VisitedCounty> GetVisitedCountyGenerator()
        //{
        //    return new Faker<VisitedCounty>()
        //        .RuleFor(v => v.Id, f => Guid.NewGuid())
        //        .RuleFor(v => v.VisitorId, f => f.PickRandom(Visitors.Where(a => a.VisitorAddressId != null)).Id)
        //        .RuleFor(v => v.CountyId, f => f.PickRandom(Counties).Id)
        //        .RuleFor(v => v.VisitDate, f => f.Date.Between(new DateTime(day: 1, month: 1, year: 2000), new DateTime(day: 2, month: 11, year: 2023)))
        //        .RuleFor(v => v.CreatedDate, f => DateTime.Now);
        //}
    }
}
