using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBillFund;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisCashierRoom;

namespace MRS.Processor.Mrs00339
{
    class Mrs00339Processor : AbstractProcessor
    {
        Mrs00339Filter castFilter = null;
        List<Mrs00339RDO> listRdo = new List<Mrs00339RDO>();
        List<Mrs00339RDO> listTotal = new List<Mrs00339RDO>();

        List<V_HIS_TRANSACTION> listBill = null;
        List<V_HIS_TRANSACTION> listDeposit = null;
        List<V_HIS_TRANSACTION> listRepay = null;
        List<V_HIS_TRANSACTION> listTransaction = null;
        List<V_HIS_CASHIER_ROOM> listCashierRoom = null;
        HIS_BRANCH _Branch = null;

        public Mrs00339Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00339Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00339Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_BILL, V_HIS_DEPOSIT, V_HIS_REPAY, MRS00339: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.HAS_SALL_TYPE = false;
                listTransaction = new HisTransactionManager(paramGet).GetView(transactionFilter);
                listBill = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                listDeposit = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                listRepay = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                HisCashierRoomViewFilterQuery cashierFilter = new HisCashierRoomViewFilterQuery();
                cashierFilter.BRANCH_ID = castFilter.BRANCH_ID;
                listCashierRoom = new HisCashierRoomManager(paramGet).GetView(cashierFilter);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00339");
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
                CommonParam paramGet = new CommonParam();
                List<long> listTreatmentId = new List<long>();
                List<V_HIS_BILL_FUND> listBillFund = null;
                if (IsNotNullOrEmpty(listBill))
                {
                    listTreatmentId.AddRange(listBill.Where(o => o.TREATMENT_ID.HasValue).Select(s => s.TREATMENT_ID.Value).Distinct().ToList());
                    listBillFund = GetBillFundByListBill(ref paramGet);
                }
                if (IsNotNullOrEmpty(listDeposit))
                {
                    listTreatmentId.AddRange(listDeposit.Where(o => o.TREATMENT_ID.HasValue).Select(s => s.TREATMENT_ID.Value).Distinct().ToList());
                }
                if (IsNotNullOrEmpty(listRepay))
                {
                    listTreatmentId.AddRange(listRepay.Where(o => o.TREATMENT_ID.HasValue).Select(s => s.TREATMENT_ID.Value).Distinct().ToList());
                }

                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    listTreatmentId = listTreatmentId.Distinct().ToList();
                    int start = 0;
                    int count = listTreatmentId.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var listId = listTreatmentId.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patyAlterFilter.TREATMENT_IDs = listId;
                        var listPatyAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter);
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua tring tong hop du lieu MRS00339");
                        }

                        if (IsNotNullOrEmpty(listPatyAlter))
                        {
                            listPatyAlter = listPatyAlter.OrderBy(o => o.LOG_TIME).ToList();
                            var Groups = listPatyAlter.GroupBy(o => o.TREATMENT_ID).ToList();
                            foreach (var group in Groups)
                            {
                                var listSub = group.ToList<V_HIS_PATIENT_TYPE_ALTER>();
                                foreach (var item in listSub)
                                {
                                    if (item.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                        continue;
                                    dicPatientTypeAlter[item.TREATMENT_ID] = item;
                                    break;
                                }
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ProcessListTransaction(dicPatientTypeAlter, listBillFund);
                    ProcessTotal();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessTotal()
        {
            try
            {
                if (listRdo != null && listRdo.Count > 0)
                {
                    Mrs00339RDO rdo = new Mrs00339RDO();
                    rdo.TOTAL_BILL_CK_AMOUNT = listRdo.Sum(o => o.TOTAL_BILL_CK_AMOUNT);
                    rdo.TOTAL_BILL_FUND_AMOUNT = listRdo.Sum(o => o.TOTAL_BILL_FUND_AMOUNT);
                    rdo.TOTAL_DEPOSIT_BILL_AMOUNT = listRdo.Sum(o => o.TOTAL_DEPOSIT_BILL_AMOUNT);
                    rdo.TOTAL_REPAY_AMOUNT = listRdo.Sum(o => o.TOTAL_REPAY_AMOUNT);
                    rdo.KC_AMOUNT = listRdo.Sum(o => o.KC_AMOUNT);
                    rdo.EXEMPTION = listRdo.Sum(o => o.EXEMPTION);
                    listTotal.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessListTransaction(Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<V_HIS_BILL_FUND> listBillFund)
        {
            try
            {
                Dictionary<long, List<V_HIS_BILL_FUND>> dicBillFund = new Dictionary<long, List<V_HIS_BILL_FUND>>();
                Dictionary<long, V_HIS_CASHIER_ROOM> dicCashierRoom = new Dictionary<long, V_HIS_CASHIER_ROOM>();
                if (!IsNotNullOrEmpty(listCashierRoom))
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc danh sach phong thu ngan: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listCashierRoom), listCashierRoom));
                    return;
                }

                foreach (var item in listCashierRoom)
                {
                    dicCashierRoom[item.ID] = item;
                }

                if (IsNotNullOrEmpty(listBillFund))
                {
                    var billFunds = listBillFund.GroupBy(o => new { o.BILL_ID }).ToList();
                    foreach (var item in billFunds)
                    {
                        dicBillFund.Add(item.First().BILL_ID, item.ToList<V_HIS_BILL_FUND>());
                    }
                }
                List<Mrs00339RDO> rdos = new List<Mrs00339RDO>();

                if (IsNotNullOrEmpty(listBill))
                {
                    foreach (var item in listBill)
                    {
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        if (!dicCashierRoom.ContainsKey(item.CASHIER_ROOM_ID))
                            continue;
                        if (!item.TREATMENT_ID.HasValue)
                            continue;

                        if ((!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID.Value)) || dicPatientTypeAlter[item.TREATMENT_ID.Value].LOG_TIME > item.TRANSACTION_TIME)
                        {
                            decimal totalFund = 0;
                            if (dicBillFund.ContainsKey(item.ID))
                            {
                                totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                            }
                            rdos.Add(new Mrs00339RDO(item, totalFund));
                        }
                    }
                }

                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var item in listDeposit)
                    {
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;

                        if (!dicCashierRoom.ContainsKey(item.CASHIER_ROOM_ID))
                            continue;

                        rdos.Add(new Mrs00339RDO(item));
                    }
                }

                if (IsNotNullOrEmpty(listRepay))
                {
                    foreach (var item in listRepay)
                    {
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;

                        if (!dicCashierRoom.ContainsKey(item.CASHIER_ROOM_ID))
                            continue;
                        if (!item.TREATMENT_ID.HasValue)
                            continue;

                        if ((item.TDL_SESE_DEPO_REPAY_COUNT.HasValue && item.TDL_SESE_DEPO_REPAY_COUNT.Value > 0) || (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID.Value)) || dicPatientTypeAlter[item.TREATMENT_ID.Value].LOG_TIME > item.TRANSACTION_TIME)
                        {
                            rdos.Add(new Mrs00339RDO(item));
                        }
                    }
                }

                if (rdos != null && rdos.Count > 0)
                {
                    var patient = rdos.GroupBy(o => new { o.PATIENT_ID }).ToList();
                    foreach (var item in patient)
                    {
                        Mrs00339RDO rdo = new Mrs00339RDO(item.ToList());
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            };
        }

        private List<V_HIS_BILL_FUND> GetBillFundByListBill(ref CommonParam paramGet)
        {
            List<V_HIS_BILL_FUND> result = new List<V_HIS_BILL_FUND>();
            try
            {
                int start = 0;
                int count = listBill.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var hisBills = listBill.Skip(start).Take(limit).ToList();
                    HisBillFundViewFilterQuery billFundFilter = new HisBillFundViewFilterQuery();
                    billFundFilter.BILL_IDs = hisBills.Select(s => s.ID).ToList();
                    var hisBillFunds = new MOS.MANAGER.HisBillFund.HisBillFundManager(paramGet).GetView(billFundFilter);
                    if (IsNotNullOrEmpty(hisBillFunds))
                    {
                        result.AddRange(hisBillFunds);
                    }
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("BRANCH_NAME", _Branch.BRANCH_NAME);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", listRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Total", listTotal);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
