using System;
using System.Collections.Generic;

namespace ActivityClubAPIs.Models;

public partial class Member
{
    public int UserId { get; set; }

    public string? Photo { get; set; }

    public string? MobileNumber { get; set; }

    public string? EmergencyNumber { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public string? Profession { get; set; }

    public string? Nationality { get; set; }

    public virtual ICollection<EventMember> EventMembers { get; set; } = new List<EventMember>();

    public virtual User User { get; set; } = null!;
}
