using System.ComponentModel.DataAnnotations;

namespace MediaCenterCMS.API.DTOs.Approval;

public class RejectRequest
{
    [Required]
    [MaxLength(500)]
    public string Reason { get; set; } = string.Empty;
}