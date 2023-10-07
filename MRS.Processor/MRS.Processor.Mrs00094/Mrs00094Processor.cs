using MOS.MANAGER.HisTransaction;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00094
{
    public class Mrs00094Processor : AbstractProcessor
    {
        Mrs00094Filter castFilter = null; 
        List<Mrs00094RDO> ListRdo = new List<Mrs00094RDO>(); 
        List<V_HIS_TRANSACTION> ListCurrentBill; 

        public Mrs00094Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00094Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00094Filter)this.reportFilter); 

                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_BILL MRS00094." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTransactionViewFilterQuery billFilter = new HisTransactionViewFilterQuery();
                billFilter.CANCEL_TIME_FROM = castFilter.TIME_FROM;
                billFilter.CANCEL_TIME_TO = castFilter.TIME_TO;
                billFilter.IS_CANCEL = true; 
                billFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT }; 
                ListCurrentBill = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(billFilter); 

                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Error("Co exception xay ra khi lay du lieu V_HIS_BILL MRS00094." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
                    throw new DataMisalignedException(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                if (IsNotNullOrEmpty(ListCurrentBill))
                {
                    ListCurrentBill = ListCurrentBill.Where(o => o.IS_CANCEL == 1).ToList(); 
                    ListRdo = (from r in ListCurrentBill select new Mrs00094RDO(r)).ToList(); 
                    result = true; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                ListRdo = ListRdo.OrderBy(o => o.CANCEL_USERNAME).ThenBy(t => t.TRANSACTION_CODE).ToList(); 
                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
