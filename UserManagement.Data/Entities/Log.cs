using System;

namespace UserManagement.Models;

public class Log
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Action { get; set; } = default!;
    public DateTime WhenUtc { get; set; } = DateTime.UtcNow;
    public string? Details { get; set; } 

    public User? User { get; set; }
}
