using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisTransaction; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00599
{
    public class Mrs00599Processor : AbstractProcessor
    {
        Mrs00599Filter filter = new Mrs00599Filter(); 
        CommonParam paramGet = new CommonParam(); 
        List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>(); 
        List<Mrs00599RDO> ListRdo = new List<Mrs00599RDO>(); 
        HIS_BRANCH Branch = new HIS_BRANCH(); 
        public Mrs00599Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00599Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                filter = (Mrs00599Filter)this.reportFilter; 
                //get dữ liệu:
              
                HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = filter.TRANSACTION_TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = filter.TRANSACTION_TIME_TO; 
                ListTransaction = new HisTransactionManager(paramGet).GetView(tranFilter);
             
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 

                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {

                ListRdo.Clear(); 
                if (IsNotNullOrEmpty(ListTransaction))
                {
                    ListTransaction = ListTransaction.Where(o => o.TIG_TRANSACTION_CODE != null).ToList(); 

                    if (IsNotNullOrEmpty(ListTransaction))
                    {
                        ListRdo = (from b in ListTransaction select new Mrs00599RDO(b)).ToList(); 
                    }
                }
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 

                ListRdo.Clear(); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TRANSACTION_TIME_TO)); 
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());

            objectTag.AddObjectData(store, "Report", ListRdo); 

        }

     
    }
}