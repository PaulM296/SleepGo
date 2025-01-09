using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SleepGo.App.Interfaces;
using SleepGo.Domain.Entities;
using SleepGo.Infrastructure.Exceptions;

namespace SleepGo.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly SleepGoDbContext _context;

        public AuthenticationService(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager, SleepGoDbContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<AppUser> LoginUser(string Email, string Password)
        {
            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                throw new InvalidCredentialsException("Invalid credentials!");
            }

            var loginResult = await _signInManager.CheckPasswordSignInAsync(user, Password, false);

            if(!loginResult.Succeeded)
            {
                throw new InvalidCredentialsException("Invalid credentials!");
            }

            return user;
        }

        public async Task<AppUser> Register(AppUser newUser, UserProfile newUserProfile, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException("This email is already in use.");
            }

            var identityUser = new AppUser
            {
                Email = newUser.Email,
                UserName = newUser.UserName,
                Role = newUser.Role
            };

            var createdIdentity = await _userManager.CreateAsync(identityUser, password);
            if (!createdIdentity.Succeeded)
            {
                throw new Exception("User creation failed: " + string.Join(", ", createdIdentity.Errors.Select(e => e.Description)));
            }

            var foundRole = await _roleManager.FindByNameAsync(newUser.Role.ToString());
            if (foundRole == null)
            {
                var newRole = new IdentityRole<Guid> { Name = newUser.Role.ToString() };
                var roleResult = await _roleManager.CreateAsync(newRole);
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Role creation failed: " + string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                }
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(identityUser, newUser.Role.ToString());
            if(!addToRoleResult.Succeeded)
            {
                throw new Exception("Adding user to role failed: " + string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
            }

            var userProfile = new UserProfile
            {
                FirstName = newUserProfile.FirstName,
                LastName = newUserProfile.LastName,
                UserId = identityUser.Id,
                ProfilePicture = newUserProfile.ProfilePicture,
                DateOfBirth = newUserProfile.DateOfBirth,
                ImageId = newUserProfile.ImageId
            };

            try
            {
                _context.UserProfiles.Add(userProfile);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) 
            {
                throw new Exception("An error occurred while saving the user profile: " + ex.InnerException?.Message);
            }

            return identityUser;
        }
    }
}
