using System;
using System.ComponentModel.DataAnnotations;

public class ApplicationRequest
{
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; }

    [Required]
    public string UserName { get; set; }

    [Required]
    public string RequestType { get; set; }

    [Required]
    public DateTime RequestDate { get; set; } = DateTime.Now;

    public bool IsApproved { get; set; } = false;

    public string Comments { get; set; }
}
