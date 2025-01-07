using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;
}
