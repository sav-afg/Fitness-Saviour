


using Microsoft.EntityFrameworkCore;
using WebsiteFirstDraft.Data.DatabaseTableModels;

namespace WebsiteFirstDraft.Data.Models
{
    // AuthService handles authentication-related database operations
    public class AuthService
    {
        // AppDbContext is used to access the database (works with both SQL Server and In-Memory)
        private readonly AppDbContext _context;

        // Constructor injection allows AppDbContext to be provided
        // by ASP.NET Core's dependency injection system
        public AuthService(AppDbContext context)
        {
            _context = context;
        }

        // Attempts to log a user in by checking their credentials against the database
        // Returns true if the username and password match exactly one record
        public async Task<bool> LoginAsync(string username, string password)
        {
            // Hash the entered password before comparing it to the stored hash
            var hashedPassword = PasswordHelper.HashPassword(password);

            // Check if a user exists with the given username and hashed password
            var userExists = await _context.Users
                .AnyAsync(u => u.Username == username && u.PasswordHash == hashedPassword);

            // Login is successful if a matching user is found
            return userExists;
        }

        // Get user by username (useful for loading user data after login)
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Username == username);
        }

        // Check if username already exists (useful for registration)
        public async Task<bool> UsernameExistsAsync(string username)
        {
            return await _context.Users
                .AnyAsync(u => u.Username == username);
        }
    }
}


