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
    public class MeetingSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MeetingSeeder> _logger;

        //tell deserializer to match "Open", "InProgress" Case Status by name
        private static readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public MeetingSeeder(AppDbContext context, ILogger<MeetingSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task MeetingSeedAsync(string jsonFilePath)
        {
            //skip if table already has data
            if (await _context.Meetings.AnyAsync())
            {
                _logger.LogInformation("Meetings table already seeded. Skipping.");
                return;
            }

            if (!File.Exists(jsonFilePath))
            {
                _logger.LogWarning("Seed file not found at path: {Path}", jsonFilePath);
                return;
            }

            var json = await File.ReadAllTextAsync(jsonFilePath);
            var meetings = JsonSerializer.Deserialize<List<Meeting>>(json, _jsonOptions);

            if (meetings is null || meetings.Count == 0)
            {
                _logger.LogWarning("No meetings found in seed file.");
                return;
            }

            else
            {
                //pull available case Ids
                var availableCaseIDs = await _context.Cases
                    .Where(c => !_context.Meetings.Any(m => m.CaseId == c.Id))
                    .Select(c => c.Id)
                    .ToListAsync();

                //shuffling
                var shuffled = availableCaseIDs.OrderBy(_ => new Random().Next()).ToList();

                foreach (var meeting in meetings)
                {
                    if (!shuffled.Any())
                    {
                        _logger.LogWarning("Ran out of available Case IDs during seeding!");
                        break;
                    }

                    var caseId = shuffled.First();
                    meeting.CaseId = caseId;
                    shuffled.Remove(caseId);
                }
                await _context.Meetings.AddRangeAsync(meetings);
                await _context.SaveChangesAsync();
                //await AssignCaseIDsToUnassignedMeetings(_context);
            }

            await _context.Meetings.AddRangeAsync(meetings);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Successfully seeded {Count} clients.", meetings.Count);
        }

        //private async Task AssignCaseIDsToUnassignedMeetings(AppDbContext context)
        //{
        //    var unassignedMeetings = await context.Meetings
        //        .Where(m => m.CaseId == null || m.CaseId == Guid.Empty)
        //        .ToListAsync();

        //    var availableCaseIDs = await context.Cases
        //        .Where(c => !context.Meetings.Any(m => m.CaseId == c.Id))
        //        .Select(c => c.Id)
        //        .ToListAsync();
        //    //shuffle the list
        //    var rng = new Random();
        //    var shuffled = availableCaseIDs.OrderBy(_ => rng.Next()).ToList();

        //    foreach (var meeting in unassignedMeetings)
        //    {
        //        if (!shuffled.Any())
        //        {
        //            Console.WriteLine("Ran out of available Case IDs!");
        //            break;
        //        }

        //        //get one, assign it, remove from "pool"
        //        var caseId = shuffled.First();
        //        meeting.CaseId = caseId;
        //        shuffled.Remove(caseId);
        //    }

        //    await context.SaveChangesAsync();
        //}

    }
}