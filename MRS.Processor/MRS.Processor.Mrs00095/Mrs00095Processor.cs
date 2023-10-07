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

namespace MRS.Processor.Mrs00095
{
    public class Mrs00095Processor : AbstractProcessor
    {
        Mrs00095Filter castFilter = null; 
        List<Mrs00095RDO> ListRdo = new List<Mrs00095RDO>(); 
        List<V_HIS_TRANSACTION> hisTransactions; 
        CommonParam paramGet = new CommonParam(); 
        
        public Mrs00095Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00095Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00095Filter)this.reportFilter); 

                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TRANSACTION MRS00095." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO; 
                hisTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(tranFilter); 
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
                if (IsNotNullOrEmpty(hisTransactions))
                {
                    hisTransactions = hisTransactions.Where(o => /* o.IS_TRANSFER_ACCOUNTING != 1 &&*/ o.IS_CANCEL != 1).ToList(); 
                    var Groups = hisTransactions.GroupBy(g => g.CASHIER_LOGINNAME).ToList(); 
                    foreach (var group in Groups)
                    {
                        List<V_HIS_TRANSACTION> listSub = group.ToList<V_HIS_TRANSACTION>(); 
                        ListRdo.Add(new Mrs00095RDO(listSub, paramGet)); 
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xa ra tai DAOGET trong qua trinh tong hop du lieu MRS00095."); 
                    }
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

                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
