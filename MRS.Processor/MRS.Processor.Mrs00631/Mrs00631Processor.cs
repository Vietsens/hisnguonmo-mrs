using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBillFund;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00631
{
    class Mrs00631Processor : AbstractProcessor
    {
        Mrs00631Filter castFilter = null;

        List<Mrs00631RDO> ListRdo = new List<Mrs00631RDO>();
        List<Mrs00631RDO> ListPatientType = new List<Mrs00631RDO>();

        List<V_HIS_TRANSACTION> ListTransaction = new List<V_HIS_TRANSACTION>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> DicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

        string cashierUsername = "";

        public Mrs00631Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00631Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00631Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();

                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.HAS_SALL_TYPE = false;
                ListTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(transactionFilter);

                if (IsNotNullOrEmpty(ListTransaction))
                {
                    if (castFilter.CASHIER_LOGINNAME != null)
                    {
                        ListTransaction = ListTransaction.Where(o => castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                        cashierUsername = String.Join(",", ListTransaction.Select(o => o.CASHIER_USERNAME).Distinct().ToList());
                    }

                    List<long> listTreatmentId = new List<long>();
                    listTreatmentId = ListTransaction.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();

                    if (IsNotNullOrEmpty(listTreatmentId))
                    {
                        listTreatmentId = listTreatmentId.Distinct().ToList();

                        var skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                            patyAlterFilter.TREATMENT_IDs = listIds;
                            var listPatyAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);
                            if (IsNotNullOrEmpty(listPatyAlter))
                            {
                                listPatyAlter = listPatyAlter.OrderBy(o => o.LOG_TIME).ToList();
                                var Groups = listPatyAlter.GroupBy(o => o.TREATMENT_ID).ToList();
                                foreach (var group in Groups)
                                {
                                    if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                                    {
                                        if (castFilter.PATIENT_TYPE_IDs.Contains(group.Last().PATIENT_TYPE_ID))
                                            DicCurrentPatyAlter[group.Last().TREATMENT_ID] = group.Last();
                                    }
                                    else
                                    {
                                        DicCurrentPatyAlter[group.Last().TREATMENT_ID] = group.Last();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
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
                    CommonParam paramGet = new CommonParam();

                    var listBillFund = GetBillFundByListBill(ref paramGet);

                    Dictionary<long, List<V_HIS_BILL_FUND>> dicBillFund = new Dictionary<long, List<V_HIS_BILL_FUND>>();

                    if (IsNotNullOrEmpty(listBillFund))
                    {
                        foreach (var item in listBillFund)
                        {
                            if (!dicBillFund.ContainsKey(item.BILL_ID))
                                dicBillFund[item.BILL_ID] = new List<V_HIS_BILL_FUND>();
                            dicBillFund[item.BILL_ID].Add(item);
                        }
                    }

                    foreach (var item in ListTransaction)
                    {
                        if (item.IS_CANCEL == 1)
                            continue;

                        if (!DicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua bill: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        decimal totalFund = 0;
                        if (dicBillFund.ContainsKey(item.ID))
                        {
                            totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                        }

                        ListRdo.Add(new Mrs00631RDO(item, totalFund, DicCurrentPatyAlter[item.TREATMENT_ID ?? 0]));
                    }

                    if (IsNotNullOrEmpty(ListRdo))
                    {
                        ListRdo = ListRdo.OrderBy(o => o.CREATE_TIME).ThenBy(t => t.TREATMENT_ID).ToList();
                        var Groups = ListRdo.GroupBy(o => o.PATIENT_TYPE_ID).ToList();
                        foreach (var group in Groups)
                        {
                            var listSub = group.ToList<Mrs00631RDO>();
                            Mrs00631RDO rdo = new Mrs00631RDO();
                            rdo.PATIENT_TYPE_ID = listSub.First().PATIENT_TYPE_ID;
                            rdo.PATIENT_TYPE_NAME = listSub.First().PATIENT_TYPE_NAME;
                            rdo.TOTAL_DEPOSIT_BILL_AMOUNT = listSub.Sum(s => s.TOTAL_DEPOSIT_BILL_AMOUNT);
                            rdo.TOTAL_REPAY_AMOUNT = listSub.Sum(o => o.TOTAL_REPAY_AMOUNT);

                            ListPatientType.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private List<V_HIS_BILL_FUND> GetBillFundByListBill(ref CommonParam paramGet)
        {
            List<V_HIS_BILL_FUND> listBillFund = new List<V_HIS_BILL_FUND>();
            try
            {
                var listBill = ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                if (IsNotNullOrEmpty(listBill))
                {
                    var skip = 0;
                    while (listBill.Count - skip > 0)
                    {
                        var listIds = listBill.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisBillFundViewFilterQuery billFundFilter = new HisBillFundViewFilterQuery();
                        billFundFilter.BILL_IDs = listIds.Select(s => s.ID).ToList();
                        var hisBillFunds = new MOS.MANAGER.HisBillFund.HisBillFundManager(paramGet).GetView(billFundFilter);
                        if (IsNotNullOrEmpty(hisBillFunds))
                        {
                            listBillFund.AddRange(hisBillFunds);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listBillFund = null;
            }
            return listBillFund;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }
                dicSingleTag.Add("CASHIER_LOGINNAME", castFilter.CASHIER_LOGINNAME);
                dicSingleTag.Add("CASHIER_USERNAME", cashierUsername);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientType", ListPatientType);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "PatientType", "Transaction", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
