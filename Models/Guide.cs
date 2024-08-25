using System;
using System.Collections.Generic;

namespace ActivityClubAPIs.Models;

public partial class Guide
{
    public int UserId { get; set; }

    public string? Photo { get; set; }

    public DateOnly? JoiningDate { get; set; }

    public string? Profession { get; set; }

    public virtual ICollection<EventGuide> EventGuides { get; set; } = new List<EventGuide>();

    public virtual User User { get; set; } = null!;
}
