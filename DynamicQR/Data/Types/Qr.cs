using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class Qr
{
    public int Id { get; set; }

    public int Type { get; set; }

    public int Uid { get; set; }

    public string? Title { get; set; }

    public byte[]? File { get; set; }

    public virtual User IdNavigation { get; set; } = null!;

    public virtual TypeQr TypeNavigation { get; set; } = null!;
}
