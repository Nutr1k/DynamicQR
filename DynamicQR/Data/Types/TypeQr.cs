using Chirper.Data.Types;
using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class TypeQr: IEntity
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public string JsonTemplateSchema { get; set; } = null!;

    public virtual ICollection<Qr> Qrs { get; set; } = new List<Qr>();
}
