using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.SDO
{
    public class DataGetSDO
    {
        public bool IsChecked { get; set; }
        public long ID { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public long PARENT { get; set; }
        public long GRAND_PARENT { get; set; }
        public bool? IS_OUTPUT0 { get; set; }
    }
}
