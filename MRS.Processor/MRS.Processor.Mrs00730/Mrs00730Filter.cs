using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00730
{
    class Mrs00730Filter
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public short? INPUT_DATA_ID_GROUP_TYPE { get; set; } //kiểu gộp: 1:Gộp theo lô và giá bán,2:Gộp theo loại và giá nhập,3:Gộp theo loại và giá bán,4:Gộp theo lô,5:Gộp theo loại
    }
}
