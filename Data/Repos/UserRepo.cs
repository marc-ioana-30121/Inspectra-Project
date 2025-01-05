using System.Security.Cryptography;
using System.Text;
using PrettyNeatGenericAPI.Data.Models;
using PrettyNeatGenericAPI.Models.DbModels;
using PrettyNeatGenericAPI.Models.TableModels;
using Microsoft.EntityFrameworkCore;

namespace PrettyNeatGenericAPI.Repositories;

public class UserRepo : BaseRepo<User>
{
    readonly PrettyNeatGenericAPIDbContext _dbContext;

    public UserRepo(PrettyNeatGenericAPIDbContext dbContext, IHttpContextAccessor httpContextAccessor) : base(dbContext, httpContextAccessor)
    {
        _dbContext = dbContext;
    }

    public Task<User> Authenticate(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            return null;

        var user = _dbContext.User.FirstOrDefault(x => x.Username == username);

        // check if username exists
        if (user == null)
            return null;

        // check if password is correct
        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            return null;

        // authentication successful
        return Task.FromResult(user);
    }

    public async Task<PaginatedList<User>> GetPagedAsync(int pageNumber, int pageSize, string sortBy = "", string sortOrder = "asc", string? searchQuery = null)
    {
        var query = _dbContext.User.AsQueryable();

        // Filter by search query if provided
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(c => c.Username.Contains(searchQuery));
        }

        // Get the total number of customers before pagination
        var totalUsers = await query.CountAsync();

        query = sortOrder.ToLower() == "asc"
            ? query.OrderBy(x => EF.Property<string>(x, sortBy))
            : query.OrderByDescending(x => EF.Property<string>(x, sortBy));

        // Perform pagination
        var pagedUsers= await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // Return a paginated list of customers
        return new PaginatedList<User>(pagedUsers, totalUsers, pageNumber, pageSize);
    }

    public async Task<User> AddAsync(User user)
    {
        if (_dbContext.User.Any(x => x.Username == user.Username))
            return null;

        user.PasswordHash = new Byte[0]{};
        user.PasswordSalt = new Byte[0]{};

        await _dbContext.User.AddAsync(user);
        await _dbContext.SaveChangesAsync();
       
        return user;
    }

    public async Task<User> AddAsync(User user, string password)
    {
        // validation
        if (string.IsNullOrWhiteSpace(password))
            return null;

        if (_dbContext.User.Any(x => x.Username == user.Username))
            return null;

        byte[] passwordHash, passwordSalt;
        CreatePasswordHash(password, out passwordHash, out passwordSalt);

        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;
        user.Role = "User";

        await _dbContext.User.AddAsync(user);
        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task Update(User user, string password = null)
    {
        var existingUser = await _dbContext.User.FindAsync(user.Id);

        if (existingUser == null)
            return;

        if (user.Username != existingUser.Username)
        {
            // username has changed so check if the new username is already taken
            if (_dbContext.User.Any(x => x.Username == user.Username))
                //username already taken
                return;
        }

        // update user properties
        existingUser.Username = user.Username;
        existingUser.Role = user.Role;

        // update password if it was entered
        if (!string.IsNullOrWhiteSpace(password))
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            existingUser.PasswordHash = passwordHash;
            existingUser.PasswordSalt = passwordSalt;
        }

        _dbContext.User.Update(existingUser);
        await _dbContext.SaveChangesAsync();
    }

    // private helper methods
    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));

        using var hmac = new HMACSHA512();

        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
    {
        if (password == null)
            throw new ArgumentNullException(nameof(password));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(password));
        if (storedHash.Length != 64)
            throw new ArgumentException("Invalid length of password hash (64 bytes expected).", nameof(storedHash));
        if (storedSalt.Length != 128)
            throw new ArgumentException("Invalid length of password salt (128 bytes expected).", nameof(storedSalt));

        using var hmac = new HMACSHA512(storedSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != storedHash[i])
                return false;
        }

        return true;
    }
}