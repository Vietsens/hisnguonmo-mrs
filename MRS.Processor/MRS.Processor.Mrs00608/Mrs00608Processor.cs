using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisAccountBook;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00608
{
    public class Mrs00608Processor : AbstractProcessor
    {
        private const short BILL_TYPE_ID__NORMAL = 1;
        private const short BILL_TYPE_ID__SERVICE = 2;
        Mrs00608Filter filter = null;
        private List<Mrs00608RDO> listRdo = new List<Mrs00608RDO>();
        private List<Mrs00608RDO> listRdoCancel = new List<Mrs00608RDO>();
        private List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_TRANSACTION> ListHisTransaction = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV> ListHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_ACCOUNT_BOOK> ListHisAccountBook = new List<HIS_ACCOUNT_BOOK>();
        List<ACS_USER> acsUsers = new List<ACS_USER>();

        #region single key
        private decimal BEFORE_DEP_AMOUNT = 0;
        private decimal ON_DEP_AMOUNT = 0;
        private decimal ON_NEED_BILL_OUTKC_AMOUNT = 0;//số tiền cần thanh toán ngoài kết chuyển
        private decimal ON_BILL_OUTKC_AMOUNT = 0;//số tiền thanh toán ngoài kết chuyển
        private decimal ON_NEED_RECEVE_AMOUNT = 0;//Số tiền cần nhận từ thanh toán và tạm ứng
        private decimal ON_RECEVE_AMOUNT = 0;//Số tiền đã nhận từ thanh toán và tạm ứng
        private decimal ON_REP_AMOUNT = 0;
        private decimal THEN_DEP_AMOUNT = 0;
        private decimal TOTAL_ON_AMOUNT = 0;
        #endregion

        public Mrs00608Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00608Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00608Filter)reportFilter);
            var result = true;
            CommonParam param = new CommonParam();
            try
            {
                // get treatmentFee
                //AcsUserFilterQuery userFilter = new AcsUserFilterQuery();
                //acsUsers = new ACS.MANAGER.Manager.AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(userFilter);
                //acsUsers = acsUsers.Where(x => x.IS_ACTIVE == 1).ToList();
                HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                HisTreatmentfilter.FEE_LOCK_TIME_FROM = filter.FEE_LOCK_TIME_FROM;
                HisTreatmentfilter.FEE_LOCK_TIME_TO = filter.FEE_LOCK_TIME_TO;
                HisTreatmentfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                ListHisTreatment = new HisTreatmentManager(param).Get(HisTreatmentfilter);
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID??-1)).ToList();
                }
                if (filter.BRANCH_ID!=null)
                {
                    ListHisTreatment = ListHisTreatment.Where(x => x.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                if (ListHisTreatment != null)
                {
                    var treatmentIds = ListHisTreatment.Select(p => p.ID).ToList();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.TREATMENT_IDs = Ids;
                        sereServFilter.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(param).Get(sereServFilter);
                        if (listSereServSub != null)
                        {
                            ListHisSereServ.AddRange(listSereServSub);
                        }
                        HisTransactionFilterQuery HisTransactionfilter = new HisTransactionFilterQuery();
                        HisTransactionfilter.TREATMENT_IDs = Ids;
                        //HisTransactionfilter.IS_CANCEL = false;
                        HisTransactionfilter.HAS_SALL_TYPE = false;
                        HisTransactionfilter.HAS_TDL_SERE_SERV_DEPOSIT = false;
                        var ListHisTransactionSub = new HisTransactionManager(param).Get(HisTransactionfilter);
                        if (ListHisTransactionSub != null)
                        {
                            if (filter.PAY_FORM_IDs != null)
                            {
                                ListHisTransactionSub = ListHisTransactionSub.Where(o => filter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                            }
                            ListHisTransaction.AddRange(ListHisTransactionSub);
                        }

                    }
                }
                ListHisAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery() { IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE});
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
            var result = true;
            try
            {
                bool Cancel = true;
                //Các bệnh nhân bị hủy hóa đơn
                var listBillCancel = ListHisTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                var listTreatmentBillCancel = ListHisTreatment.Where(o => listBillCancel.Exists(p => p.TREATMENT_ID == o.ID)).ToList();
                this.listRdoCancel = this.ProcessTreatment(listTreatmentBillCancel, ListHisTransaction.Where(o => o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(), Cancel);
                //Tất cả bệnh nhân
                this.listRdo = this.ProcessTreatment(ListHisTreatment, ListHisTransaction.Where(o => o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(), !Cancel);
                AddKeySumAmount();
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private List<Mrs00608RDO> ProcessTreatment(List<HIS_TREATMENT> listTreatment, List<HIS_TRANSACTION> listTransaction,bool IsCancel)
        {
            List<Mrs00608RDO>  result = new List<Mrs00608RDO>();
            try
            {
                var listAccountBookIdNormal = ListHisAccountBook.Where(o => o.BILL_TYPE_ID == BILL_TYPE_ID__NORMAL).Select(p => p.ID).ToList();
                var listAccountBookIdService = ListHisAccountBook.Where(o => o.BILL_TYPE_ID == BILL_TYPE_ID__SERVICE).Select(p => p.ID).ToList();

                foreach (var item in listTreatment)
                {
                    var transactionSub = listTransaction.Where(o => o.TREATMENT_ID == item.ID).ToList();
                    if (transactionSub.Count == 0)
                        continue;
                    var sereServSub = ListHisSereServ.Where(o => o.TDL_TREATMENT_ID == item.ID).ToList();
                    Mrs00608RDO rdo = new Mrs00608RDO();
                    if (IsCancel)
                    {
                        rdo.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.CANCEL_TIME).Select(q => q.CANCEL_TIME ?? 0).FirstOrDefault());
                        rdo.BILL_CASHIER_LOGINNAME = (transactionSub.OrderBy(o=>o.CANCEL_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)??new HIS_TRANSACTION()).CANCEL_LOGINNAME;
                        if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_LOGINNAME))
                        {
                            rdo.BILL_CASHIER_LOGINNAME = (transactionSub.OrderBy(o => o.CANCEL_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) ?? new HIS_TRANSACTION()).CANCEL_LOGINNAME;
                            if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_LOGINNAME))
                            {
                                rdo.BILL_CASHIER_LOGINNAME = item.FEE_LOCK_LOGINNAME;
                            };
                        };
                        rdo.BILL_CASHIER_USERNAME = (transactionSub.OrderBy(o => o.CANCEL_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) ?? new HIS_TRANSACTION()).CANCEL_USERNAME;
                        if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_USERNAME))
                        {
                            rdo.BILL_CASHIER_USERNAME = (transactionSub.OrderBy(o => o.CANCEL_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) ?? new HIS_TRANSACTION()).CANCEL_USERNAME;
                            if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_USERNAME))
                            {
                                rdo.BILL_CASHIER_USERNAME = (acsUsers.FirstOrDefault(o => o.LOGINNAME == item.FEE_LOCK_LOGINNAME) ?? new ACS_USER()).USERNAME;
                            };
                        };
                    }
                    else
                    {
                        rdo.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.TRANSACTION_TIME).Select(q => q.TRANSACTION_TIME).FirstOrDefault());
                        rdo.BILL_CASHIER_LOGINNAME = (transactionSub.OrderBy(o => o.TRANSACTION_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) ?? new HIS_TRANSACTION()).CASHIER_LOGINNAME;
                        if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_LOGINNAME))
                        {
                            rdo.BILL_CASHIER_LOGINNAME = (transactionSub.OrderBy(o => o.TRANSACTION_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) ?? new HIS_TRANSACTION()).CASHIER_LOGINNAME;
                            if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_LOGINNAME))
                            {
                                rdo.BILL_CASHIER_LOGINNAME = item.FEE_LOCK_LOGINNAME;
                            };
                        };
                        rdo.BILL_CASHIER_USERNAME = (transactionSub.OrderBy(o => o.TRANSACTION_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) ?? new HIS_TRANSACTION()).CASHIER_USERNAME;
                        if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_USERNAME))
                        {
                            rdo.BILL_CASHIER_USERNAME = (transactionSub.OrderBy(o => o.TRANSACTION_TIME).FirstOrDefault(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU) ?? new HIS_TRANSACTION()).CASHIER_USERNAME;
                            if (string.IsNullOrWhiteSpace(rdo.BILL_CASHIER_USERNAME))
                            {
                                rdo.BILL_CASHIER_USERNAME = (acsUsers.FirstOrDefault(o => o.LOGINNAME == item.FEE_LOCK_LOGINNAME) ?? new ACS_USER()).USERNAME;
                            };
                        };
                    }
                    if (filter.CASHIER_LOGINNAMEs != null)
                    {
                        if (!filter.CASHIER_LOGINNAMEs.Exists(o => rdo.BILL_CASHIER_LOGINNAME==o))
                        {
                            continue;
                        }
                    }
                    rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                    rdo.PATIENT_CODE = item.TDL_PATIENT_CODE;
                    rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                    rdo.HEIN_CARD_NUMBER = item.TDL_HEIN_CARD_NUMBER;
                    rdo.ADDRESS = item.TDL_PATIENT_ADDRESS;
                    rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB);
                    rdo.AGE = CalcuatorAge(item.TDL_PATIENT_DOB);
                    rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.IN_TIME);
                    rdo.FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.FEE_LOCK_TIME ?? 0);
                    rdo.BIL_TRANSACTION_CODE = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(q => q.TRANSACTION_CODE).ToList());
                    rdo.DEP_TRANSACTION_CODE = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Select(q => q.TRANSACTION_CODE).ToList());
                    rdo.REP_TRANSACTION_CODE = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Select(q => q.TRANSACTION_CODE).ToList());
                    rdo.TOTAL_DEPOSIT_AMOUNT = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(p => p.AMOUNT);
                    rdo.TOTAL_REPAY_AMOUNT = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(p => p.AMOUNT);
                    rdo.REP_TRANSACTION_CODE = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Select(q => q.TRANSACTION_CODE).Distinct().ToList());
                    rdo.IS_SAME_DATE = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && (o.CANCEL_TIME <o.TRANSACTION_DATE || (o.CANCEL_TIME>o.TRANSACTION_DATE+1000000))).ToList().Count == 0;
                    if (IsCancel)
                    {
                        rdo.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.CANCEL_TIME).Select(q => q.CANCEL_TIME ?? 0).FirstOrDefault());
                        rdo.BILL_CASHIER_LOGINNAME = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(q => q.CANCEL_LOGINNAME).Distinct().ToList());
                        rdo.BILL_CASHIER_USERNAME = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(q => q.CANCEL_USERNAME).Distinct().ToList());
                    }
                    else
                    {
                        rdo.BILL_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderBy(p => p.TRANSACTION_TIME).Select(q => q.TRANSACTION_TIME).FirstOrDefault());
                        rdo.BILL_CASHIER_LOGINNAME = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(q => q.CASHIER_LOGINNAME).Distinct().ToList());
                        rdo.BILL_CASHIER_USERNAME = string.Join(", ", transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Select(q => q.CASHIER_USERNAME).Distinct().ToList());
                    }
                    var listPayFormIdNormal = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && listAccountBookIdNormal.Contains(o.ACCOUNT_BOOK_ID)).Select(q => q.PAY_FORM_ID).Distinct().ToList();
                    if (listPayFormIdNormal != null)
                    {
                        rdo.PAY_FORM_NORMAL_NAME = string.Join(", ", HisPayFormCFG.ListPayForm.Where(o => listPayFormIdNormal.Contains(o.ID)).Select(p => p.PAY_FORM_NAME).ToList());
                    }
                    var listPayFormIdService = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && listAccountBookIdService.Contains(o.ACCOUNT_BOOK_ID)).Select(q => q.PAY_FORM_ID).Distinct().ToList();
                    if (listPayFormIdService != null)
                    {
                        rdo.PAY_FORM_SERVICE_NAME = string.Join(", ", HisPayFormCFG.ListPayForm.Where(o => listPayFormIdService.Contains(o.ID)).Select(p => p.PAY_FORM_NAME).ToList());
                    }
                    rdo.PAY_FORM_NAME = string.Join(", ", HisPayFormCFG.ListPayForm.Where(o => transactionSub.Exists(q => q.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && q.PAY_FORM_ID == o.ID)).Select(p => p.PAY_FORM_NAME).ToList());
                    rdo.BILL_AMOUNT_VP = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && listAccountBookIdNormal.Contains(o.ACCOUNT_BOOK_ID)).Sum(p => p.AMOUNT);
                    rdo.BILL_AMOUNT_AN = sereServSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN).Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    rdo.BILL_AMOUNT_DV = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && listAccountBookIdService.Contains(o.ACCOUNT_BOOK_ID)).Sum(p => p.AMOUNT) - rdo.BILL_AMOUNT_AN;
                    rdo.TOTAL_PATIENT_PRICE = sereServSub.Sum(p => p.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    rdo.TOTAL_BILL_OUTKC_AMOUNT = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.AMOUNT - (p.KC_AMOUNT ?? 0));//không dùng
                    rdo.DIFF = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(p => p.AMOUNT) - transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(p => p.AMOUNT);
                    result.Add(rdo);
                }
                AddKeySumAmount();
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void AddKeySumAmount()
        {
            try
            {
                if (this.filter.NOT_CALCU_INV != true)
                {
                    this.BEFORE_DEP_AMOUNT = new Mrs00608RDOManager().GetBeforeAmount(filter) ?? 0;
                    this.ON_DEP_AMOUNT = new Mrs00608RDOManager().GetOnAmount(filter) ?? 0;
                    this.THEN_DEP_AMOUNT = new Mrs00608RDOManager().GetThenAmount(filter) ?? 0;
                }
                var SumDepositAmount = ListHisTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                var SumRepayAmount = ListHisTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT); ;
                var SumTotalPatientPrice = ListHisSereServ.Sum(p => p.VIR_TOTAL_PATIENT_PRICE ?? 0);

                this.ON_REP_AMOUNT = SumRepayAmount;
                this.ON_NEED_BILL_OUTKC_AMOUNT = SumTotalPatientPrice + SumRepayAmount - SumDepositAmount;
                this.ON_NEED_RECEVE_AMOUNT = this.ON_DEP_AMOUNT+this.ON_NEED_BILL_OUTKC_AMOUNT;

                this.TOTAL_ON_AMOUNT = SumTotalPatientPrice;


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CalcuatorAge(long DOB)
        {
            int? AGE = null;
            try
            {
                int? tuoi = RDOCommon.CalculateAge(DOB);
                if (tuoi >= 0)
                {
                    AGE = (tuoi >= 1) ? tuoi : 1;
                }
                return AGE;
            }
            catch (Exception ex)
            {
                return null;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00608Filter)reportFilter).FEE_LOCK_TIME_FROM));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00608Filter)reportFilter).FEE_LOCK_TIME_TO));

            objectTag.AddObjectData(store, "Report", listRdo);

            objectTag.AddObjectData(store, "ReportMore0", listRdo.Where(o => o.DIFF >= 0).ToList());
            objectTag.AddObjectData(store, "CashierMore0", listRdo.Where(o => o.DIFF >= 0).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierMore0", "ReportMore0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportLeft0", listRdo.Where(o => o.DIFF < 0).ToList());
            objectTag.AddObjectData(store, "CashierLeft0", listRdo.Where(o => o.DIFF < 0).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierLeft0", "ReportLeft0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportCancel", listRdoCancel);

            objectTag.AddObjectData(store, "ReportCancelMore0", listRdoCancel.Where(o => o.DIFF >= 0 && !o.IS_SAME_DATE).ToList());
            objectTag.AddObjectData(store, "CashierCancelMore0", listRdoCancel.Where(o => o.DIFF >= 0 && !o.IS_SAME_DATE).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierCancelMore0", "ReportCancelMore0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportCancelLeft0", listRdoCancel.Where(o => o.DIFF < 0 && !o.IS_SAME_DATE).ToList());
            objectTag.AddObjectData(store, "CashierCancelLeft0", listRdoCancel.Where(o => o.DIFF < 0 && !o.IS_SAME_DATE).GroupBy(o => o.BILL_CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "CashierCancelLeft0", "ReportCancelLeft0", "BILL_CASHIER_LOGINNAME", "BILL_CASHIER_LOGINNAME");
        
            dicSingleTag.Add("BEFORE_DEP_AMOUNT", BEFORE_DEP_AMOUNT);
            dicSingleTag.Add("ON_DEP_AMOUNT", ON_DEP_AMOUNT);
            dicSingleTag.Add("ON_REP_AMOUNT", ON_REP_AMOUNT);
            dicSingleTag.Add("ON_NEED_BILL_OUTKC_AMOUNT", ON_NEED_BILL_OUTKC_AMOUNT);
            dicSingleTag.Add("ON_NEED_RECEVE_AMOUNT", ON_NEED_RECEVE_AMOUNT);
            dicSingleTag.Add("ON_BILL_OUTKC_AMOUNT", ON_BILL_OUTKC_AMOUNT);
            dicSingleTag.Add("ON_RECEVE_AMOUNT", ON_RECEVE_AMOUNT);
            dicSingleTag.Add("THEN_DEP_AMOUNT", THEN_DEP_AMOUNT);
            dicSingleTag.Add("TOTAL_ON_AMOUNT", TOTAL_ON_AMOUNT);
        }

    }
}
