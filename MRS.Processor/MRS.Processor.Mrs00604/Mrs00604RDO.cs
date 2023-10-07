using MOS.EFMODEL.DataModels; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00604
{
    public class Mrs00604RDO :HIS_TREATMENT
    {
        public decimal KH_PRICE { get; set; }
        public decimal XN_PRICE { get; set; }
        public decimal SA_PRICE { get; set; }
        public decimal CDHA_PRICE { get; set; }
        public decimal TDCN_PRICE { get; set; }
        public decimal KHAC_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public Mrs00604RDO()
        {

        }

        public Mrs00604RDO(Mrs00604RDO data)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00604RDO>(); 
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));   
                }
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }



        public string CATEGORY_CODE { get; set; }

        public long? TDL_EXECUTE_ROOM_ID { get; set; }

        public string HEALTH_EXAM_RANK_NAME { get; set; }
        public string HEALTH_EXAM_RANK_CODE { get; set; }
        public long? HEALTH_EXAM_RANK_ID { get; set; }

        public long TDL_SERVICE_TYPE_ID { get; set; }

        public decimal? VIR_TOTAL_PRICE { get; set; }

        public decimal AMOUNT { get; set; }

        public Dictionary<string, string> DIC_GROUP_HEALTH { get; set; }
    }
}
