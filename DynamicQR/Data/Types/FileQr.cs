using System;
using System.Collections.Generic;

namespace DynamicQR.Data.Types;

public partial class FileQr
{
    public int FileId { get; set; }

    public byte[] File { get; set; } = null!;
}
