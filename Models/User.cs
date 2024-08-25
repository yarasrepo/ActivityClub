using System;
using System.Collections.Generic;

namespace ActivityClubAPIs.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Gender { get; set; }

    public string? FullName { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public virtual Admin? Admin { get; set; }

    public virtual Guide? Guide { get; set; }

    public virtual Member? Member { get; set; }
}
