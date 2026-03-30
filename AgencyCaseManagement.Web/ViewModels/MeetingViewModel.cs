using AgencyCaseManagement.Domain.Entities;

namespace AgencyCaseManagement.Web.ViewModels
{
    public class MeetingViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime {  get; set; }
        public DateTime EndTime { get; set; }
        public MeetingType MeetingType { get; set; }
        public string CaseTitle { get; set; }
        public int ParticipantCount { get; set; }
    }
}
