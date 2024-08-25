using System;
using System.Collections.Generic;

namespace ActivityClubAPIs.Models;

public partial class EventGuide
{
    public int Id { get; set; }

    public int? GuideId { get; set; }

    public int? EventId { get; set; }

    public virtual Event? Event { get; set; }

    public virtual Guide? Guide { get; set; }
}
