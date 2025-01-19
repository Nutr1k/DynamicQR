using Chirper.Data.Types;
using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class Qr:IOwnedEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Type { get; set; }

    public int Uid { get; set; }

    public string? Title { get; set; }

    public string JsonVariables { get; set; } = null!;

    public virtual TypeQr TypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
