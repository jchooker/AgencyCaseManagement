using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AgencyCaseManagement.Domain.Entities;
using AgencyCaseManagement.Infrastructure;
using AgencyCaseManagement.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AgencyCaseManagement.Application.Services
{

    public interface IMeetingService
    {
        Task<MeetingListDTO> GetAllMeetingsAsync();
        Task<MeetingDTO?> GetMeetingViewModelByIdAsync(Guid id);
    }
    public class MeetingService : IMeetingService
    {
        private readonly AppDbContext _db;

        public MeetingService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<MeetingListDTO> GetAllMeetingsAsync()
        {
            var meetings = await _db.Meetings
                .Where(m => !m.IsDeleted)
                .Select(m => new MeetingDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    MeetingType = m.MeetingType,
                    CaseTitle = m.Case != null ? m.Case.Title : null,
                    ParticipantCount = m.Participants.Count
                })
                .ToListAsync();

            return new MeetingListDTO {  Meetings = meetings };
        }

        public async Task<MeetingDTO?> GetMeetingViewModelByIdAsync(Guid id)
        {
            return await _db.Meetings
                .Where(m => !m.IsDeleted && m.Id == id)
                .Select(m => new MeetingDTO
                {
                    Id = m.Id,
                    Title = m.Title,
                    StartTime = m.StartTime,
                    EndTime = m.EndTime,
                    MeetingType = m.MeetingType,
                    CaseTitle = m.Case != null ? m.Case.Title : null,
                    ParticipantCount = m.Participants.Count
                })
                .FirstOrDefaultAsync();
        }
    }
}
