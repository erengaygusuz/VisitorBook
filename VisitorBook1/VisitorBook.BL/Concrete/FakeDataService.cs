using AutoMapper;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Dtos.VisitedCountyDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;
using VisitorBook.Core.Entities;
using VisitorBook.Core.Enums;

namespace VisitorBook.BL.Concrete
{
    public class FakeDataService : IFakeDataService
    {
        public List<UserRequestDto> Users = new();
        public List<UserAddressRequestDto> UserAddresses = new();
        public List<VisitedCountyRequestDto> VisitedCounties = new();

        private readonly IRepository<County> _countyRepository;
        private readonly IService<VisitedCounty> _visitedCounty;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        private IEnumerable<County> counties;

        public FakeDataService(IRepository<County> countyRepository, IService<VisitedCounty> visitedCounty, UserManager<User> userManager, IMapper mapper)
        {
            _countyRepository = countyRepository;
            _visitedCounty = visitedCounty;
            _userManager = userManager;
            _mapper = mapper;

            counties = _countyRepository.GetAll(include: x => x.Include(a => a.City));
        }

        private List<UserRequestDto> GetGeneratedUserData(int amount)
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);

            return new Faker<UserRequestDto>(currentLanguage)
                .RuleFor(v => v.Name, f => f.Person.FirstName)
                .RuleFor(v => v.Surname, f => f.Person.LastName)
                .RuleFor(v => v.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(v => v.Email, f => f.Lorem.Word().ClampLength(5, 15).ToLower() + "@gmail.com")
                .RuleFor(v => v.Username, f => f.Lorem.Word().ClampLength(5, 15).ToLower())
                .RuleFor(v => v.Gender, f => f.PickRandom<Gender>().ToString())
                .RuleFor(v => v.UserAddress, f => GetGeneratedUserAddressData(f.PickRandom(counties.ToList())))
                .Generate(amount);
        }

        private UserAddressRequestDto? GetGeneratedUserAddressData(County county)
        {
            Random random = new Random();

            var nmbr = random.Next(0, 2);

            if (nmbr > 0)
            {
                return new Faker<UserAddressRequestDto>()
                    .RuleFor(v => v.CityId, f => county.City.Id)
                    .RuleFor(v => v.CountyId, f => county.Id)
                    .Generate(1).FirstOrDefault();
            }

            return null;
        }

        private List<VisitedCountyRequestDto> GetGeneratedVisitedCountyData(int amount)
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);

            return new Faker<VisitedCountyRequestDto>(currentLanguage)
                .RuleFor(v => v.UserId, f => f.PickRandom(_userManager.Users.ToList()).Id)
                .RuleFor(v => v.CountyId, f => f.PickRandom(counties.ToList()).Id)
                .RuleFor(v => v.VisitDate, f => f.Date.Between(new DateTime(day: 1, month: 1, year: 2000), new DateTime(day: 2, month: 11, year: 2023)))
                .Generate(amount);
        }

        public async Task InsertUserDatas(int amount)
        {
            var userList = _mapper.Map<List<User>>(GetGeneratedUserData(amount));

            foreach(var user in userList)
            {
                var result = await _userManager.CreateAsync(user, "12345");

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Visitor");
                }
            }
        }

        public async Task InsertVisitedCountyDatas(int amount)
        {
            var visitedCountyList = _mapper.Map<List<VisitedCounty>>(GetGeneratedVisitedCountyData(amount));

            foreach (var visitedCounty in visitedCountyList)
            {
                await _visitedCounty.AddAsync(visitedCounty);
            }
        }
    }
}
