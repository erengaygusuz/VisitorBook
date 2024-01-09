using AutoMapper;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;
using VisitorBook.Core.Abstract;
using VisitorBook.Core.Constants;
using VisitorBook.Core.Dtos.CountyDtos;
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

        private readonly IService<County> _countyService;
        private readonly IService<VisitedCounty> _visitedCountyService;
        private readonly IService<UserAddress> _userAddressService;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        private IEnumerable<CountyResponseDto> counties;

        public FakeDataService(IService<County> countyService, IService<VisitedCounty> visitedCountyService, UserManager<User> userManager, 
            IMapper mapper, IService<UserAddress> userAddressService)
        {
            _countyService = countyService;
            _visitedCountyService = visitedCountyService;
            _userManager = userManager;
            _mapper = mapper;
            _userAddressService = userAddressService;

            counties = _countyService.GetAllAsync<CountyResponseDto>().GetAwaiter().GetResult();
        }

        private void GeneratedUserData(int amount)
        {
            var datas = new Faker<UserRequestDto>()
                .RuleFor(v => v.Name, f => f.Person.FirstName)
                .RuleFor(v => v.Surname, f => f.Person.LastName)
                .RuleFor(v => v.BirthDate, f => f.Person.DateOfBirth)
                .RuleFor(v => v.Email, f => f.Lorem.Word().ClampLength(5, 15).ToLower() + "@gmail.com")
                .RuleFor(v => v.Username, f => f.Lorem.Word().ClampLength(5, 15).ToLower())
                .RuleFor(v => v.Gender, f => f.PickRandom<Gender>().ToString())
                .Generate(amount);

            Users.AddRange(datas);
        }

        private void GeneratedUserAddressData(int amount)
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);

            Random random = new Random();

            for (int i = 0; i < amount; i++)
            {
                var nmbr = random.Next(0, 2);

                if (nmbr > 0)
                {
                    UserAddresses.Add(new Faker<UserAddressRequestDto>(currentLanguage)
                        .RuleFor(v => v.CountyId, f => f.PickRandom(counties).Id)
                        .Generate(1).FirstOrDefault());
                }

                else
                {
                    UserAddresses.Add(null);
                }
            }
        }

        private void GeneratedVisitedCountyData(int amount)
        {
            var currentLanguage = Thread.CurrentThread.CurrentCulture.Name.Substring(0, 2);

            var datas = new Faker<VisitedCountyRequestDto>(currentLanguage)
                .RuleFor(v => v.UserId, f => f.PickRandom(_userManager.Users.ToList()).Id)
                .RuleFor(v => v.CountyId, f => f.PickRandom(counties).Id)
                .RuleFor(v => v.VisitDate, f => f.Date.Between(new DateTime(day: 1, month: 1, year: 2000), new DateTime(day: 2, month: 11, year: 2023)))
                .Generate(amount);

            VisitedCounties.AddRange(datas);
        }

        public async Task InsertUserDatas(int amount)
        {
            GeneratedUserData(amount);
            GeneratedUserAddressData(amount);

            var userList = _mapper.Map<List<User>>(Users);

            for (int i = 0; i < userList.Count; i++)
            {
                var userExistByEmail = await _userManager.FindByEmailAsync(userList[i].Email);
                var userExistByUsername = await _userManager.FindByNameAsync(userList[i].UserName);

                if (userExistByEmail == null && userExistByUsername == null)
                {
                    var result = await _userManager.CreateAsync(userList[i], "12345");

                    if (UserAddresses[i] != null)
                    {
                        var user = await _userManager.FindByEmailAsync(userList[i].Email);

                        await _userAddressService.AddAsync(new UserAddressRequestDto
                        {
                            UserId = user.Id,
                            CountyId = UserAddresses[i].CountyId
                        });
                    }

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(userList[i], AppRoles.Visitor);
                    }
                }
            }
        }

        public async Task InsertVisitedCountyDatas(int amount)
        {
            GeneratedVisitedCountyData(amount);

            foreach (var visitedCounty in VisitedCounties)
            {
                await _visitedCountyService.AddAsync(visitedCounty);
            }
        }
    }
}
