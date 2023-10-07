using MOS.EFMODEL.DataModels; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00613
{
    class Mrs00613RDO:HIS_TRANSACTION
    {
       
        public Mrs00613RDO(HIS_TRANSACTION r) 
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.HIS_TRANSACTION>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
        }

        public string TRANSACTION_TIME_STR { get; set; }//Ngày tháng 
        public string DOB_YEAR_STR { get; set; }//năm sinh
        public string USE_FUND_REASON { get; set; }//Nội dung nghiệp vụ kinh tế phát sinh
        public string FUND_NAME { get; set; }//Tên quỹ 
        public decimal FUND_AMOUNT { get; set; }//Số tiền quỹ
        public string NUM_ORDER_STR { get; set; }//Tên quỹ 
        public long IN_TIME { get; set; }//Tên quỹ 
        public string IN_TIME_STR { get; set; }//Tên quỹ 

    }

    public class HIS_TRANSACTION_D : HIS_TRANSACTION
    {
        public long IN_TIME { get; set; }//Ngày khám
        public string FUND_NAME { get; set; }//tên quỹ
        public decimal BILL_FUND_AMOUNT { get; set; }//tiền quỹ
    }
}
