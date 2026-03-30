using System.ComponentModel.DataAnnotations;
namespace AgencyCaseManagement.Web.ViewModels
{
    public class MeetingListViewModel
    {
        [Required]
        public List<MeetingViewModel> Meetings { get; set; } = null!;
    }
}
