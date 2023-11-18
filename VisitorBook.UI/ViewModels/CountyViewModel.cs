﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using VisitorBook.UI.Models.Inputs;

namespace VisitorBook.UI.ViewModels
{
    public class CountyViewModel
    {
        public CountyInput County { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }
    }
}
