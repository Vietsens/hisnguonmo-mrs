using Inventec.Common.FlexCellExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Core.MrsReport
{
    public delegate void ProcessMrs(ref Store store, ref MemoryStream resultStream);
}
