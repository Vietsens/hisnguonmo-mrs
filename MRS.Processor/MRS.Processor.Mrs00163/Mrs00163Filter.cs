using MOS.Filter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00163
{
    /// <summary>
    /// Báo cáo tổng hợp tồn kho toàn viện số lượng theo kho, tại thời điểm lấy báo cáo
    /// </summary>
    public class Mrs00163Filter : FilterBase
    {
        public long? MEDI_STOCK_ID { get; set; }
        public Mrs00163Filter()
            : base()
        {
            
        }
    }
}
