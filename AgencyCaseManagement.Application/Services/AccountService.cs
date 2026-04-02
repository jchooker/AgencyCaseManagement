using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyCaseManagement.Application.Results;
using AgencyCaseManagement.Application.DTOs;
using AgencyCaseManagement.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace AgencyCaseManagement.Application.Services
{
    public interface IAccountService
    {
        Task<RegisterResult> RegisterAsync(RegisterDTO dto);
        Task<LoginResult> LoginAsync(LoginDTO dto);
        Task<BeginActivationResult> BeginActivationAsync(BeginActivationDTO dto);
    }
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<RegisterResult> RegisterAsync(RegisterDTO dto)
        {
            var result = new RegisterResult();

            if (string.IsNullOrWhiteSpace(dto.Email) ||
                string.IsNullOrWhiteSpace(dto.FirstName) ||
                string.IsNullOrWhiteSpace(dto.LastName) ||
                string.IsNullOrWhiteSpace(dto.UserName) ||
                string.IsNullOrWhiteSpace(dto.Password) ||
                string.IsNullOrWhiteSpace(dto.ConfirmPassword)
                )
            {
                result.Errors.Add("All fields are required.");
                return result;
            }

            //confirm pw match
            if (dto.Password != dto.ConfirmPassword)
            {
                result.Errors.Add("Passwords do not match.");
                return result;
            }

            //check for existing email
            var existingByEmail = await _userManager.FindByEmailAsync(dto.Email);
            if (existingByEmail != null)
            {
                result.Errors.Add("An account with this email address already exists.");
                return result;
            }

            var existingByUserName = await _userManager.FindByNameAsync(dto.UserName);
            if (existingByUserName != null)
            {
                result.Errors.Add("An account with this Username already exists! Please select a unique Username.");
                return result;
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                UserName = dto.UserName,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailConfirmed = false
            };

            var createResult = await _userManager.CreateAsync(user, dto.Password);

            if (!createResult.Succeeded)
            {
                foreach (var err in createResult.Errors)
                {
                    result.Errors.Add(err.Description);
                }
                return result;
            }

            result.Succeeded = true;
            result.RequiresActivation = false; //with pw, no activation needed

            return result;
        }
        public async Task<LoginResult> LoginAsync(LoginDTO dto) 
        {
            var result = new LoginResult();

            if (string.IsNullOrWhiteSpace(dto.EmailOrUserName) || string.IsNullOrWhiteSpace(dto.Password))
            {
                result.Errors.Add("Username / email and password are required.");
                return result;
            }

            User? user = null;

            user = await _userManager.FindByEmailAsync(dto.EmailOrUserName);

            //if not found by email, try username
            if (user == null)
                user = await _userManager.FindByNameAsync(dto.EmailOrUserName);

            if (user == null)
            {
                result.Errors.Add("Invalid login attempt.");
                return result;
            }

            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                result.Errors.Add("Your account has not been finalized. Please complete account setup!");
                return result;
            }

            var validPw = await _userManager.CheckPasswordAsync(user, dto.Password);
            if (!validPw)
            {
                result.Errors.Add("Invalid login attempt.");
                return result;
            }

            await _signInManager.SignInAsync(user, dto.RememberMe);

            result.Succeeded = true;
            return result;

            //await _signInManager

            //var user = emailCheck == null ? userNameCheck : emailCheck;
        }

        public async Task<BeginActivationResult> BeginActivationAsync(BeginActivationDTO dto)
        {
            var result = new BeginActivationResult();

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                result.Errors.Add("Email is required.");
                return result;
            }

            var user = await _userManager.FindByEmailAsync(dto.Email);

            if (user == null)
            {
                result.Errors.Add("No account was found for that email address.");
                return result;
            }

            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                result.Errors.Add("This account has already been finalized. Please " +
                    "log in with the correct username / email and password combination.");
                return result;
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            result.Succeeded = true;
            result.RequiresPasswordSetup = true;
            result.Email = user.Email;
            result.Token = token;
            result.Message = "Account activation begun successfully.";

            return result;
        }
    }

}
