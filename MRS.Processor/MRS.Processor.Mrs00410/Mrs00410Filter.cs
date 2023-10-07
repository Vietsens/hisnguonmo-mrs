using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00410
{
    /// <summary>
    /// báo cáo sử dụng thuốc gây nghiện hướng tâm thần
    /// </summary>
    public class Mrs00410Filter : FilterBase
    {
        public long TIME_FROM { get; set; }
        public long TIME_TO { get; set; }
        public List<long> MEDI_STOCK_IDs { get; set; }
        public List<long> MEDICINE_GROUP_IDs { get; set; }
        public short? IS_GN { get; set; }//null:all; 1:GN; 0:HT

        public Mrs00410Filter()
            : base()
        {

        }
    }
}
