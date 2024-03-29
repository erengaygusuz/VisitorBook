﻿using VisitorBook.Core.Dtos.VisitorAddressDtos;

namespace VisitorBook.Core.Dtos.UserDtos
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public string Gender { get; set; }
        public string? Picture { get; set; }
        public UserAddressResponseDto? UserAddress { get; set; }
    }
}
