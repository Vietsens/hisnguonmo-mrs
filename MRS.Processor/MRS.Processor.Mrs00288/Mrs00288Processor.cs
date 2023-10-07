using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00288
{
    class Mrs00288Processor : AbstractProcessor
    {
        Mrs00288Filter castFilter = null;
        List<Mrs00288RDO> listRdo = new List<Mrs00288RDO>();
        List<Mrs00288RDO> listDepartment = new List<Mrs00288RDO>();

        List<V_HIS_TREATMENT> ListTreatment;

        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();

        string cashierUsername = "";
        string cashierLoginname = "";

        public Mrs00288Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00288Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00288Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_TREATMENT, MRS00288 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                ListTreatment = new List<V_HIS_TREATMENT>();

                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                ListTreatment = new HisTreatmentManager(paramGet).GetView(treatFilter);

                HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
                cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID;
                listCashierRoom = new HisCashierRoomManager(paramGet).GetView(cashierRoomFilter);
                cashierLoginname = castFilter.CASHIER_LOGINNAME ?? castFilter.LOGINNAME;
                AcsUserFilterQuery AcsUserFilter = new AcsUserFilterQuery();
                AcsUserFilter.LOGINNAME = cashierLoginname;
                ACS_USER AcsUser = new AcsUserManager(paramGet).Get<ACS_USER>(AcsUserFilter);

                if (AcsUser != null)
                {
                    cashierUsername = AcsUser.USERNAME;
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT, MRS00288." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
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

                if (IsNotNullOrEmpty(ListTreatment))
                {
                    int start = 0;
                    int count = ListTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                        List<long> listTreatmentId = hisTreatments.Select(s => s.ID).ToList();
                        HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                        ssFilter.TREATMENT_IDs = listTreatmentId;
                        List<V_HIS_SERE_SERV> ListSereServ = new HisSereServManager(paramGet).GetView(ssFilter);

                        HisPatientTypeAlterViewFilterQuery patientAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                        patientAlterFilter.TREATMENT_IDs = listTreatmentId;
                        List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetView(patientAlterFilter);

                        // các transaction
                        HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                        transactionFilter.TREATMENT_IDs = listTreatmentId;
                        transactionFilter.IS_CANCEL = false;
                        transactionFilter.HAS_SALL_TYPE = false;
                        transactionFilter.ORDER_DIRECTION = "DESC";
                        transactionFilter.ORDER_FIELD = "TRANSACTION_TIME";
                        List<V_HIS_TRANSACTION> listTransactions = new HisTransactionManager(paramGet).GetView(transactionFilter);
                        listTransactions = listTransactions.Where(o => listCashierRoom.Exists(p => p.ID == o.CASHIER_ROOM_ID)).ToList();
                        if (string.IsNullOrWhiteSpace(cashierLoginname))
                        {
                            listTransactions = listTransactions.Where(o => o.CASHIER_LOGINNAME == cashierLoginname).ToList();
                        }
                        List<V_HIS_TRANSACTION> listDeposit = new List<V_HIS_TRANSACTION>();
                        List<V_HIS_TRANSACTION> listRepay = new List<V_HIS_TRANSACTION>();
                        List<V_HIS_TRANSACTION> listBill = new List<V_HIS_TRANSACTION>();
                        listDeposit = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                        listRepay = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                        listBill = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                        if (!paramGet.HasException)
                        {
                            ProcessDetailListTreatment(hisTreatments, ListSereServ, ListPatientTypeAlter, listDeposit, listRepay, listBill);
                        }
                        else
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00288");
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ProcessListDepartByRdo();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessListDepartByRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.IN_TIME).ThenBy(t => t.OUT_TIME).ToList();
                    var Groups = listRdo.GroupBy(o => o.DEPARTMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00288RDO>();
                        Mrs00288RDO rdo = new Mrs00288RDO();
                        rdo.DEPARTMENT_ID = listSub.First().DEPARTMENT_ID;
                        rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                        rdo.TOTAL_BILL_EXAM_AMOUNT = listSub.Sum(s => s.TOTAL_BILL_EXAM_AMOUNT);
                        rdo.TOTAL_BILL_FUND = listSub.Sum(o => o.TOTAL_BILL_FUND);
                        rdo.TOTAL_DEPOSIT_AMOUNT = listSub.Sum(o => o.TOTAL_DEPOSIT_AMOUNT);
                        rdo.TOTAL_DIFFERENCE_PRICE = listSub.Sum(o => o.TOTAL_DIFFERENCE_PRICE);
                        rdo.TOTAL_FEE_PATIENT_PRICE = listSub.Sum(o => o.TOTAL_FEE_PATIENT_PRICE);
                        rdo.TOTAL_HEIN_LIMIT_PRICE = listSub.Sum(o => o.TOTAL_HEIN_LIMIT_PRICE);
                        rdo.TOTAL_HEIN_PATIENT_PRICE = listSub.Sum(o => o.TOTAL_HEIN_PATIENT_PRICE);
                        rdo.TOTAL_HEIN_PRICE = listSub.Sum(o => o.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_PATIENT_AMOUNT = listSub.Sum(o => o.TOTAL_PATIENT_AMOUNT);
                        rdo.TOTAL_WITHDRAW_AMOUNT = listSub.Sum(o => o.TOTAL_WITHDRAW_AMOUNT);
                        rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE);
                        listDepartment.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDetailListTreatment(List<V_HIS_TREATMENT> hisTreatments, List<V_HIS_SERE_SERV> ListSereServ, List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter, List<V_HIS_TRANSACTION> listDeposit, List<V_HIS_TRANSACTION> listRepay, List<V_HIS_TRANSACTION> listBill)
        {
            if (!(IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListPatientTypeAlter)))
                return;

            Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
            Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicBill = new Dictionary<long, List<V_HIS_TRANSACTION>>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicDeposit = new Dictionary<long, List<V_HIS_TRANSACTION>>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicRepay = new Dictionary<long, List<V_HIS_TRANSACTION>>();

            ListPatientTypeAlter = ListPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();

            foreach (var patientTypeAlter in ListPatientTypeAlter)
            {
                if (IsNotNull(patientTypeAlter))
                {
                    if (!dicPatientTypeAlter.ContainsKey(patientTypeAlter.TREATMENT_ID))
                        dicPatientTypeAlter[patientTypeAlter.TREATMENT_ID] = new List<V_HIS_PATIENT_TYPE_ALTER>();
                    dicPatientTypeAlter[patientTypeAlter.TREATMENT_ID].Add(patientTypeAlter);
                }
            }


            foreach (var item in listBill)
            {
                if (!item.TREATMENT_ID.HasValue)
                    continue;
                if (!dicBill.ContainsKey(item.TREATMENT_ID.Value))
                    dicBill[item.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                dicBill[item.TREATMENT_ID.Value].Add(item);
            }

            foreach (var sereServ in ListSereServ)
            {
                if (IsNotNull(sereServ))
                {
                    if (!sereServ.TDL_TREATMENT_ID.HasValue)
                        continue;
                    if (sereServ.IS_NO_EXECUTE == 1 || sereServ.IS_EXPEND == 1)
                        continue;
                    if (!dicSereServ.ContainsKey(sereServ.TDL_TREATMENT_ID.Value))
                        dicSereServ[sereServ.TDL_TREATMENT_ID.Value] = new List<V_HIS_SERE_SERV>();
                    dicSereServ[sereServ.TDL_TREATMENT_ID.Value].Add(sereServ);
                }
            }


            foreach (var dp in listDeposit)
            {
                if (IsNotNull(dp) && dp.TREATMENT_ID.HasValue)
                {
                    //tạm ứng mới
                    if (!dicDeposit.ContainsKey(dp.TREATMENT_ID.Value))
                        dicDeposit[dp.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                    dicDeposit[dp.TREATMENT_ID.Value].Add(dp);

                }
            }

            foreach (var repay in listRepay)
            {
                if (IsNotNull(repay) && repay.TREATMENT_ID.HasValue)
                {
                    if (!dicRepay.ContainsKey(repay.TREATMENT_ID.Value))
                        dicRepay[repay.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                    dicRepay[repay.TREATMENT_ID.Value].Add(repay);
                }
            }

            foreach (var treatment in hisTreatments)
            {
                if (!dicBill.ContainsKey(treatment.ID))
                {
                    continue;
                }
                bool valid = false;
                if (dicPatientTypeAlter.ContainsKey(treatment.ID))
                {
                    var currentPatientTypeAlter = dicPatientTypeAlter[treatment.ID].First();
                    if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        valid = true;
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co danh sach patient_type_alter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                }
                if (valid)//Chỉ lấy các BN vào có nội trú
                {
                    Mrs00288RDO rdo = new Mrs00288RDO(treatment);
                    var outPatientTypeAlter = dicPatientTypeAlter[treatment.ID].FirstOrDefault(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    if (IsNotNull(outPatientTypeAlter))
                    {
                        rdo.LOG_TIME_EXAM = outPatientTypeAlter.LOG_TIME;
                    }
                    else
                    {
                        outPatientTypeAlter = dicPatientTypeAlter[treatment.ID].FirstOrDefault(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
                        if (IsNotNull(outPatientTypeAlter))
                        {
                            rdo.LOG_TIME_EXAM = outPatientTypeAlter.LOG_TIME;
                        }
                    }
                    if (dicSereServ.ContainsKey(treatment.ID))
                    {
                        List<V_HIS_SERE_SERV> hisSereServHein = new List<V_HIS_SERE_SERV>();
                        List<V_HIS_SERE_SERV> hisSereServFee = new List<V_HIS_SERE_SERV>();
                        decimal totalHeinPrice = 0;
                        decimal totalDifference = 0;
                        foreach (var sereServ in dicSereServ[treatment.ID])
                        {
                            if (!rdo.VIR_TOTAL_PATIENT_PRICE.HasValue) rdo.VIR_TOTAL_PATIENT_PRICE = 0;
                            rdo.VIR_TOTAL_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0);

                            if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                hisSereServHein.Add(sereServ);
                                if (!rdo.TOTAL_HEIN_LIMIT_PRICE.HasValue) rdo.TOTAL_HEIN_LIMIT_PRICE = 0;
                                if (!rdo.TOTAL_HEIN_PATIENT_PRICE.HasValue) rdo.TOTAL_HEIN_PATIENT_PRICE = 0;
                                if (!rdo.TOTAL_DIFFERENCE_PRICE.HasValue) rdo.TOTAL_DIFFERENCE_PRICE = 0;

                                rdo.TOTAL_HEIN_LIMIT_PRICE += (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                rdo.TOTAL_HEIN_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                                if (sereServ.HEIN_LIMIT_PRICE.HasValue)
                                {
                                    if (sereServ.VIR_PRICE.HasValue && sereServ.HEIN_LIMIT_PRICE.Value < sereServ.VIR_PRICE.Value)
                                    {
                                        rdo.TOTAL_DIFFERENCE_PRICE = sereServ.AMOUNT * (sereServ.VIR_PRICE.Value - sereServ.HEIN_LIMIT_PRICE.Value);
                                    }
                                    if (sereServ.PRICE < sereServ.HEIN_LIMIT_PRICE.Value)
                                    {
                                        totalHeinPrice += (sereServ.AMOUNT * sereServ.PRICE * (1 + sereServ.VAT_RATIO));
                                    }
                                    else
                                    {
                                        totalDifference += Math.Abs(sereServ.AMOUNT * (sereServ.PRICE * (1 + sereServ.VAT_RATIO) - sereServ.HEIN_LIMIT_PRICE.Value));
                                        totalHeinPrice += (sereServ.HEIN_LIMIT_PRICE.Value * sereServ.AMOUNT);
                                    }
                                }
                                else
                                {
                                    totalHeinPrice += (sereServ.AMOUNT * sereServ.PRICE * (1 + sereServ.VAT_RATIO));
                                }

                                if (String.IsNullOrEmpty(rdo.HEIN_CARD_NUMBER) && !String.IsNullOrEmpty(sereServ.HEIN_CARD_NUMBER))
                                    rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                            }
                            else
                            {
                                hisSereServFee.Add(sereServ);
                            }
                        }

                        rdo.TOTAL_HEIN_PRICE = totalHeinPrice;
                        rdo.TOTAL_FEE_PATIENT_PRICE = 0;
                        rdo.TOTAL_FEE_PATIENT_PRICE = (rdo.VIR_TOTAL_PATIENT_PRICE ?? 0) - (rdo.TOTAL_HEIN_PATIENT_PRICE ?? 0) - (rdo.TOTAL_DIFFERENCE_PRICE ?? 0);
                        if (IsNotNullOrEmpty(hisSereServFee))
                        {
                            var feePrice = hisSereServFee.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                            if (feePrice > 0)
                            {
                                rdo.TOTAL_FEE_PATIENT_PRICE += feePrice;
                            }
                        }

                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co sereServ nao: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                    }
                    if (dicBill.ContainsKey(treatment.ID))
                    {
                        rdo.TOTAL_BILL_FUND = 0;
                        foreach (var bill in dicBill[treatment.ID])
                        {
                            rdo.TOTAL_BILL_FUND += bill.TDL_BILL_FUND_AMOUNT;
                        }
                    }
                    if (dicDeposit.ContainsKey(treatment.ID))
                    {
                        foreach (var deposit in dicDeposit[treatment.ID])
                        {
                            V_HIS_TRANSACTION tran = null;

                            if (tran != null && tran.TREATMENT_TYPE_ID.HasValue)
                            {
                                if (tran.TREATMENT_TYPE_ID.Value != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                {
                                    rdo.TOTAL_BILL_EXAM_AMOUNT += deposit.AMOUNT;
                                }
                                else
                                {
                                    rdo.TOTAL_DEPOSIT_AMOUNT += deposit.AMOUNT;
                                }
                            }
                            else if (deposit.TDL_SERE_SERV_DEPOSIT_COUNT == 0 || deposit.TDL_SERE_SERV_DEPOSIT_COUNT == null)
                            {
                                rdo.TOTAL_DEPOSIT_AMOUNT += deposit.AMOUNT;
                            }
                            else
                            {
                                rdo.TOTAL_BILL_EXAM_AMOUNT += deposit.AMOUNT;
                            }
                        }
                    }

                    if (dicRepay.ContainsKey(treatment.ID))
                    {
                        foreach (var repay in dicRepay[treatment.ID])
                        {
                            V_HIS_TRANSACTION lastBill = new V_HIS_TRANSACTION();
                            if (dicBill.ContainsKey(treatment.ID) && dicBill[treatment.ID].Count > 0)
                            {
                                lastBill = dicBill[treatment.ID].Last();
                            }
                            if (repay.TDL_SESE_DEPO_REPAY_COUNT == null || repay.TDL_SESE_DEPO_REPAY_COUNT == 0)
                            {
                                if (repay.TRANSACTION_TIME > lastBill.TRANSACTION_TIME)
                                    continue;
                                rdo.TOTAL_DEPOSIT_AMOUNT = rdo.TOTAL_DEPOSIT_AMOUNT - repay.AMOUNT;
                            }
                            else
                            {
                                rdo.TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                            }
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("TRUE/FALSE", dicBill.ContainsKey(treatment.ID)));
                    }

                    var residual_Amount = (rdo.TOTAL_DEPOSIT_AMOUNT) + (rdo.TOTAL_BILL_FUND ?? 0) + (rdo.TOTAL_BILL_EXAM_AMOUNT) - (rdo.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    if (residual_Amount > 0)
                    {
                        rdo.TOTAL_WITHDRAW_AMOUNT = residual_Amount;
                    }
                    else if (residual_Amount < 0)
                    {
                        rdo.TOTAL_PATIENT_AMOUNT = Math.Abs(residual_Amount);
                    }
                    listRdo.Add(rdo);
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("CASHIER_LOGINNAME", cashierLoginname);
                dicSingleTag.Add("CASHIER_USERNAME", cashierUsername);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Department", listDepartment);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", listRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Department", "Report", "DEPARTMENT_ID", "DEPARTMENT_ID");
                exportSuccess = exportSuccess && objectTag.SetUserFunction(store, "FuncRowNumber", new RDOCustomerFuncManyRownumberData());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long DepartmentId = 0;
        long num_order = 0;
        public RDOCustomerFuncManyRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long departId = Convert.ToInt64(parameters[0]);

                if (DepartmentId == departId)
                {
                    num_order = num_order + 1;
                }
                else
                {
                    DepartmentId = departId;
                    num_order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return num_order;
        }
    }
}
