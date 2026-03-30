using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AgencyCaseManagement.Domain.Entities;

namespace AgencyCaseManagement.Infrastructure.Data.Seeders
{
    public class UserSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<UserSeeder> _logger;

        //tell deserializer to match "Open", "InProgress" Case Status by name
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public UserSeeder(AppDbContext context, ILogger<UserSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task UserSeedAsync(string jsonFilePath)
        {
            //skip if table already has data
            if (await _context.Users.AnyAsync())
            {
                _logger.LogInformation("Users table already seeded. Skipping.");
                return;
            }

            if (!File.Exists(jsonFilePath))
            {
                _logger.LogWarning("Seed file not found at path: {Path}", jsonFilePath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var users = JsonSerializer.Deserialize<List<User>>(json, _jsonOptions);

            if (users is null || users.Count == 0)
            {
                _logger.LogWarning("No cases found in seed file.");
                return;
            }

            await _context.Users.AddRangeAsync(users);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {Count} cases.", users.Count);
        }
    }
}