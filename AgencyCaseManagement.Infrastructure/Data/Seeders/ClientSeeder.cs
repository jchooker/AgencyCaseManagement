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
    public class ClientSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ClientSeeder> _logger;

        //tell deserializer to match "Open", "InProgress" Case Status by name
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public ClientSeeder(AppDbContext context, ILogger<ClientSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ClientSeedAsync(string jsonFilePath)
        {
            //skip if table already has data
            if (await _context.Clients.AnyAsync())
            {
                _logger.LogInformation("Clients table already seeded. Skipping.");
                return;
            }

            if (!File.Exists(jsonFilePath))
            {
                _logger.LogWarning($"Seed file not found at path: {jsonFilePath}", jsonFilePath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var clients = JsonSerializer.Deserialize<List<Client>>(json, _jsonOptions);

            if (clients is null || clients.Count == 0)
            {
                _logger.LogWarning("No clients found in seed file.");
                return;
            }

            await _context.Clients.AddRangeAsync(clients);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {Count} clients.", clients.Count);
        }
    }
}