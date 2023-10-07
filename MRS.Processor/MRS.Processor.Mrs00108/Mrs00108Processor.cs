using MOS.MANAGER.HisTreatment;
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

namespace MRS.Processor.Mrs00108
{
    public class Mrs00108Processor : AbstractProcessor
    {
        Mrs00108Filter castFilter = null;
        List<Mrs00108RDO> ListRdo = new List<Mrs00108RDO>();
        List<V_HIS_TREATMENT> ListTreatment;

        public Mrs00108Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00108Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00108Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TREATMENT, MRS00108 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);
                if (!paramGet.HasException)
                {
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

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessListTreatment(ListTreatment);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatment(List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    CommonParam paramGet = new CommonParam();
                    foreach (var Treatment in ListTreatment)
                    {
                        HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                        tranFilter.TREATMENT_ID = Treatment.ID;
                        tranFilter.HAS_SALL_TYPE = false;
                        List<V_HIS_TRANSACTION> ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(tranFilter);
                        if (!paramGet.HasException)
                        {
                            ListTransaction = ListTransaction.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE /*&& o.IS_TRANSFER_ACCOUNTING != IMSys.DbConfig.HIS_RS.HIS_BILL.IS_TRANSFER_ACCOUNTING__TRUE*/).ToList();
                            if (IsNotNullOrEmpty(ListTransaction))
                            {
                                ListRdo.Add(new Mrs00108RDO(ListTransaction, Treatment.FEE_LOCK_TIME));
                            }
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00108.");
                        }
                    }

                    ListRdo = ListRdo.GroupBy(g => g.FEE_LOCK_DATE).Select(s => new Mrs00108RDO { FEE_LOCK_DATE = s.First().FEE_LOCK_DATE, FEE_LOCK_DATE_STR = s.First().FEE_LOCK_DATE_STR, TOTAL_BILL_AMOUNT = s.Sum(s1 => s1.TOTAL_BILL_AMOUNT), TOTAL_DEPOSIT_AMOUNT = s.Sum(s2 => s2.TOTAL_DEPOSIT_AMOUNT), TOTAL_REPAY_AMOUNT = s.Sum(s3 => s3.TOTAL_REPAY_AMOUNT) }).OrderBy(o => o.FEE_LOCK_DATE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
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
