﻿using Authenticate.API.IService;
using Azure;
using BCrypt.Net;
using JobBoard.Data;
using JobBoard.Dtos;
using JobBoard.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoard.Services
{
    public class AuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, JwtTokenGenerator jwtTokenGenerator)
        {
            _context = context;
            _configuration = configuration;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        public async Task<User> RegisterAsync(UserRegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                throw new Exception("Email already exists");

            var user = new User
            {
                Email = dto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Name = dto.Name,
                UserType = dto.UserType 
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> LoginAsync(UserLoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
                throw new Exception("Invalid credentials");



            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(User user)
        {
            return _jwtTokenGenerator.CreateJwtToken(user,user.UserType);
        }
    }
}