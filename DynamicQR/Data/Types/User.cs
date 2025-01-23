using Chirper.Data.Types;
using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class User : IEntity
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public virtual ICollection<FileQr> FileQrs { get; set; } = new List<FileQr>();

    public virtual ICollection<Qr> Qrs { get; set; } = new List<Qr>();
}
