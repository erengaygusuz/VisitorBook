﻿namespace VisitorBook.Backend.Core.Dtos.AuthDtos
{
    public class RegisterRequestDto
    {
        public string Username { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public string PasswordConfirm { get; set; }
    }
}