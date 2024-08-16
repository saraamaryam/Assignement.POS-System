﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using POS.API.Models.Entities;

namespace POS.API.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task<User> LogInAsync(string name, string password);

        Task SeedUsersAsync();

        Task<User> GetUserByIdAsync(int id);

        Task<bool> UpdateUserRoleAsync(string username, UserRole role);
        

    }
}
