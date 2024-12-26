using System;
using System.Collections.Generic;

namespace Data_Access.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public bool IsVerified { get; set; }

    public string? VerificationToken { get; set; }
}
