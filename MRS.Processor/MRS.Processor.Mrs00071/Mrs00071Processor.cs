using MOS.MANAGER.HisAccountBook;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00071
{
    public class Mrs00071Processor : AbstractProcessor
    {
        Mrs00071Filter castFilter = null;
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<Mrs00071RDO> ListRdo = new List<Mrs00071RDO>();
        List<Mrs00071RDO> ListRdoTreatment = new List<Mrs00071RDO>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();

        public Mrs00071Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00071Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00071Filter)this.reportFilter);

                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_TRANSACTION, MRS00071 filter. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = castFilter.TRANSACTION_TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = castFilter.TRANSACTION_TIME_TO;
                ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).Get(tranFilter);

                if (castFilter.LOGINNAME != null)
                {
                    ListTransaction = ListTransaction.Where(o => castFilter.LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    ListTransaction = ListTransaction.Where(o => castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }

                var treatmentIds = ListTransaction.Where(o => o.TREATMENT_ID.HasValue).Select(o => o.TREATMENT_ID.Value).Distinct().ToList();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var IdSubs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisTreatmentfilter = new HisTreatmentFilterQuery() { IDs = IdSubs };
                        var HisTreatmentSub = new HisTreatmentManager(param).Get(HisTreatmentfilter);
                        ListTreatment.AddRange(HisTreatmentSub);
                    }
                }

                if (castFilter.PATIENT_TYPE_ID != null)
                {
                    ListTransaction = ListTransaction.Where(o => ListTreatment.Exists(p => p.ID == o.TREATMENT_ID && p.TDL_PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID)).ToList();
                }

                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    ListTransaction = ListTransaction.Where(o => ListTreatment.Exists(p => p.ID == o.TREATMENT_ID && castFilter.TREATMENT_TYPE_IDs.Contains(p.TDL_TREATMENT_TYPE_ID ?? 0))).ToList();
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
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTransaction))
                {
                    ListTransaction = ListTransaction.Where(n => n.TREATMENT_ID.HasValue && !n.SALE_TYPE_ID.HasValue).OrderBy(o => o.TREATMENT_ID.Value).ThenBy(p => p.TRANSACTION_TIME).ToList();
                    foreach (var item in ListTransaction)
                    {
                        Mrs00071RDO rdo = new Mrs00071RDO(item);
                        var treatment = ListTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID);
                        if (treatment != null)
                        {
                            rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                        }

                        ListRdo.Add(rdo);
                    }

                    if (ListRdo != null)
                    {
                        ListRdoTreatment = ListRdo.GroupBy(o => o.TREATMENT_ID).Select(p => new Mrs00071RDO() { TREATMENT_ID = p.First().TREATMENT_ID, TDL_PATIENT_NAME = p.First().TDL_PATIENT_NAME, BILL_AMOUNT = p.Sum(s => s.BILL_AMOUNT), DEPOSIT_AMOUNT = p.Sum(s => s.DEPOSIT_AMOUNT), REPAY_AMOUNT = p.Sum(s => s.REPAY_AMOUNT), CANCEL_AMOUNT = p.Sum(s => s.CANCEL_AMOUNT), REDIASUAL_AMOUNT = p.Sum(s => s.BILL_AMOUNT + s.DEPOSIT_AMOUNT - s.REPAY_AMOUNT - s.CANCEL_AMOUNT) }).ToList();
                    }
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TRANSACTION_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TRANSACTION_TIME_TO ?? 0));
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Treatment", ListRdoTreatment);
                objectTag.AddRelationship(store, "Treatment", "Report", "TREATMENT_ID", "TREATMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
