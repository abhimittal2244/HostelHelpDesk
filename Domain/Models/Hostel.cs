using Microsoft.AspNetCore.Antiforgery;
using System.ComponentModel.DataAnnotations;

namespace HostelHelpDesk.Domain.Models
{
    public class Hostel
    {
        public int Id { get; set; } 
        public required string HostelName { get; set; }
    }
}
