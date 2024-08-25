using System;
using System.Collections.Generic;

namespace ActivityClubAPIs.Models;

public partial class Event
{
    public int Id { get; set; }

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public double? Cost { get; set; }

    public string? Destination { get; set; }

    public string? Status { get; set; }

    public int? CategoryId { get; set; }

    public virtual Lookup? Category { get; set; }

    public virtual ICollection<EventGuide> EventGuides { get; set; } = new List<EventGuide>();

    public virtual ICollection<EventMember> EventMembers { get; set; } = new List<EventMember>();
}
