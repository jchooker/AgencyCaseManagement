using Microsoft.AspNetCore.Mvc;
using AgencyCaseManagement.Web.ViewModels;
using AgencyCaseManagement.Infrastructure;
using AgencyCaseManagement.Application.Services;
using Microsoft.EntityFrameworkCore;

namespace AgencyCaseManagement.Web.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IMeetingService _meetingService;

        public DashboardController(AppDbContext db, IMeetingService meetingService)
        {
            _db = db;
            _meetingService = meetingService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("/Dashboard/GetAllMeetingsAsync")]
        public async Task<IActionResult> GetAllMeetingsAsync()
        {
            var meetings = await _meetingService.GetAllMeetingsAsync();

            var events = meetings.Meetings.Select(m => new
            {
                id = m.Id,
                title = m.Title,
                start = m.StartTime,
                end = m.EndTime,
                extendedProps = new
                {
                    caseTitle = m.CaseTitle,
                    participantCount = m.ParticipantCount,
                    meetingType = m.MeetingType
                }
            }).ToList();
            return Json(events);
        }
    }
}
