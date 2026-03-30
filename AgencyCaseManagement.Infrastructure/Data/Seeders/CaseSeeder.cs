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
    public class CaseSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<CaseSeeder> _logger;

        //tell deserializer to match "Open", "InProgress" Case Status by name
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public CaseSeeder(AppDbContext context, ILogger<CaseSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task CaseSeedAsync(string jsonFilePath)
        {
            //skip if table already has data
            if (await _context.Cases.AnyAsync())
            {
                _logger.LogInformation("Cases table already seeded. Skipping.");
                return;
            }

            if (!File.Exists(jsonFilePath))
            {
                _logger.LogWarning("Seed file not found at path: {Path}", jsonFilePath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var cases = JsonSerializer.Deserialize<List<Case>>(json, _jsonOptions);

            if (cases is null || cases.Count == 0)
            {
                _logger.LogWarning("No cases found in seed file.");
                return;
            }

            await _context.Cases.AddRangeAsync(cases);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {Count} cases.", cases.Count);
        }
    }
}