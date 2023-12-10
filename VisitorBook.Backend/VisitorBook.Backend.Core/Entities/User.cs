﻿using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using VisitorBook.Backend.Core.Attributes;
using VisitorBook.Backend.Core.Enums;

namespace VisitorBook.Backend.Core.Entities
{
    public class User : IdentityUser<int>
    {
        [Required]
        [NotEmptyGuid]
        public Guid GId { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        [MinLength(3)]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime BirthDate { get; set; }

        [Required]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }
    }
}
