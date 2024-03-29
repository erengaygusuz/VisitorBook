﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.Core.Dtos.UserDtos;
using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Core.ViewModels
{
    public class UserViewModel
    {
        public UserRequestDto User { get; set; }

        public UserAddressRequestDto? UserAddress { get; set; }

        public int RoleId { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> RoleList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> GenderList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CountyList { get; set; }
    }
}
