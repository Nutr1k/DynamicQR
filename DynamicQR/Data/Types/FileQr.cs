using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class FileQr
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public byte[] File { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
