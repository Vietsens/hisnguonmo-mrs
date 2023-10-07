using FlexCel.Report;
using Inventec.Common.Logging;
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

namespace MRS.Processor.Mrs00526
{

    class Mrs00526Processor : AbstractProcessor
    {
        Mrs00526Filter castFilter = null;
        List<V_HIS_TREATMENT_FEE> ListTreatment;
        List<V_HIS_TREATMENT_FEE> ListTreatmentFee = new List<V_HIS_TREATMENT_FEE>();
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        Dictionary<string, Mrs00526RDO> dicTreaDp = new Dictionary<string, Mrs00526RDO>();
        List<Mrs00526RDO> listRdo = new List<Mrs00526RDO>();
        List<Mrs00526RDO> listDepartment = new List<Mrs00526RDO>();
        Dictionary<long, long> dicTranDepa = new Dictionary<long, long>();


        public Mrs00526Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00526Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00526Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu HIS_TREATMENT, MRS00526 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                ListTreatment = new List<V_HIS_TREATMENT_FEE>();

                HisTreatmentFeeViewFilterQuery treatFilter = new HisTreatmentFeeViewFilterQuery();
                treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                ListTreatment = new HisTreatmentManager(paramGet).GetFeeView(treatFilter);

                HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
                listCashierRoom = new HisCashierRoomManager(paramGet).GetView(cashierRoomFilter);

                //MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery mateFilter = new MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery();
                //listMaterialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).Get(mateFilter);

                //thông tin khoa bệnh nhân đang ở trước khi giao dịch
                GetDepaBeforeTran();

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT, MRS00526." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDepaBeforeTran()
        {
            var tranDepa = new ManagerSql().GetRequestDepartmentId(castFilter);
            if (tranDepa != null && tranDepa.Count > 0)
            {
                dicTranDepa = tranDepa.GroupBy(o => o.TRAN_ID).ToDictionary(o => o.Key, p => p.Last().REQ_ID??0);
            }

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

                    //do
                    //{
                    //    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    var hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                    //    List<long> listTreatmentId = hisTreatments.Select(s => s.ID).ToList();
                    //    HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                    //    ssFilter.TREATMENT_IDs = listTreatmentId;
                    //    ssFilter.HAS_EXECUTE = true;
                    //    ssFilter.IS_EXPEND = false;
                    //    List<HIS_SERE_SERV> ListSereServ = new HisSereServManager(paramGet).Get(ssFilter);

                    //    // các transaction
                    //    HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                    //    transactionFilter.TREATMENT_IDs = listTreatmentId;
                    //    transactionFilter.IS_CANCEL = false;
                    //    transactionFilter.HAS_SALL_TYPE = false;
                    //    transactionFilter.ORDER_DIRECTION = "DESC";
                    //    transactionFilter.ORDER_FIELD = "CREATE_TIME";
                    //    List<HIS_TRANSACTION> listTransactions = new HisTransactionManager(paramGet).Get(transactionFilter);

                    //    List<REQUEST_DEPARTMENT_ID> listRequestDepartmentId = new ManagerSql().GetRequestDepartmentId(listTransactions.Min(o => o.ID), listTransactions.Max(o => o.ID)) ?? new List<REQUEST_DEPARTMENT_ID>();

                    //    if (IsNotNullOrEmpty(listTransactions) && !String.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                    //    {
                    //        hisTreatments = hisTreatments.Where(o => listTransactions.Exists(p => p.TREATMENT_ID == o.ID && p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME)).ToList();
                    //    }
                    //    if (IsNotNullOrEmpty(listTransactions) && !String.IsNullOrWhiteSpace(castFilter.LOGINNAME))
                    //    {
                    //        hisTreatments = hisTreatments.Where(o => listTransactions.Exists(p => p.TREATMENT_ID == o.ID && p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CASHIER_LOGINNAME == castFilter.LOGINNAME)).ToList();
                    //    }
                    //    //var listTransactionIds = listTransactions.Select(s => s.ID).Distinct().ToList();
                    //    List<HIS_TRANSACTION> listDeposit = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                    //    List<HIS_TRANSACTION> listRepay = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                    //    List<HIS_TRANSACTION> listBill = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                    //    if (!paramGet.HasException)
                    //    {
                    //        ProcessDetailListTreatment(hisTreatments, ListSereServ, listDeposit, listRepay, listBill, listRequestDepartmentId);
                    //    }
                    //    else
                    //    {
                    //        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00288");
                    //    }
                    //    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //} while (count > 0);

                    while (count > 0)
                    {

                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var hisTreatments = ListTreatment.Skip(start).Take(limit).ToList();
                        List<long> listTreatmentId = hisTreatments.Select(s => s.ID).ToList();
                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.TREATMENT_IDs = listTreatmentId;
                        ssFilter.HAS_EXECUTE = true;
                        ssFilter.IS_EXPEND = false;
                        List<HIS_SERE_SERV> ListSereServ = new HisSereServManager(paramGet).Get(ssFilter);

                        // các transaction
                        HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                        transactionFilter.TREATMENT_IDs = listTreatmentId;
                        transactionFilter.IS_CANCEL = false;
                        transactionFilter.HAS_SALL_TYPE = false;
                        transactionFilter.ORDER_DIRECTION = "DESC";
                        transactionFilter.ORDER_FIELD = "CREATE_TIME";
                        List<HIS_TRANSACTION> listTransactions = new HisTransactionManager(paramGet).Get(transactionFilter);

                        //List<REQUEST_DEPARTMENT_ID> listRequestDepartmentId = new ManagerSql().GetRequestDepartmentId(listTransactions.Min(o => o.ID), listTransactions.Max(o => o.ID)) ?? new List<REQUEST_DEPARTMENT_ID>();

                        if (IsNotNullOrEmpty(listTransactions) && !String.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                        {
                            hisTreatments = hisTreatments.Where(o => listTransactions.Exists(p => p.TREATMENT_ID == o.ID && p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME)).ToList();
                        }
                        if (IsNotNullOrEmpty(listTransactions) && !String.IsNullOrWhiteSpace(castFilter.LOGINNAME))
                        {
                            hisTreatments = hisTreatments.Where(o => listTransactions.Exists(p => p.TREATMENT_ID == o.ID && p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CASHIER_LOGINNAME == castFilter.LOGINNAME)).ToList();
                        }
                        //var listTransactionIds = listTransactions.Select(s => s.ID).Distinct().ToList();
                        List<HIS_TRANSACTION> listDeposit = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                        List<HIS_TRANSACTION> listRepay = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                        List<HIS_TRANSACTION> listBill = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                        if (!paramGet.HasException)
                        {
                            ProcessDetailListTreatment(hisTreatments, ListSereServ, listDeposit, listRepay, listBill);
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
                LogSystem.Info("count " + listRdo.Count());
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.IN_TIME).ThenBy(t => t.OUT_TIME).ToList();
                    var Groups = listRdo.GroupBy(o => o.DEPARTMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00526RDO>();
                        Mrs00526RDO rdo = new Mrs00526RDO();
                        rdo.DEPARTMENT_ID = listSub.First().DEPARTMENT_ID;
                        rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                        rdo.TOTAL_BILL_EXAM_AMOUNT = listSub.Sum(s => s.TOTAL_BILL_EXAM_AMOUNT);
                        rdo.TOTAL_BILL_FUND = listSub.Sum(o => o.TOTAL_BILL_FUND);
                        rdo.TOTAL_DEPOSIT_AMOUNT = listSub.Sum(o => o.TOTAL_DEPOSIT_AMOUNT);
                        rdo.TOTAL_DIFFERENCE_PRICE = listSub.Sum(o => o.TOTAL_DIFFERENCE_PRICE);
                        rdo.TOTAL_HEIN_PAY_YOURSELF_PRICE = listSub.Sum(o => o.TOTAL_HEIN_PAY_YOURSELF_PRICE);
                        rdo.TOTAL_FEE_PATIENT_PRICE = listSub.Sum(o => o.TOTAL_FEE_PATIENT_PRICE);
                        rdo.TOTAL_HEIN_LIMIT_PRICE = listSub.Sum(o => o.TOTAL_HEIN_LIMIT_PRICE);
                        rdo.TOTAL_HEIN_PATIENT_PRICE = listSub.Sum(o => o.TOTAL_HEIN_PATIENT_PRICE);
                        rdo.TOTAL_HEIN_PRICE = listSub.Sum(o => o.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_PATIENT_AMOUNT = listSub.Sum(o => o.TOTAL_PATIENT_AMOUNT);
                        rdo.TOTAL_WITHDRAW_AMOUNT = listSub.Sum(o => o.TOTAL_WITHDRAW_AMOUNT);
                        rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = listSub.Sum(o => o.TOTAL_OTHER_SOURCE_PRICE);
                        listDepartment.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDetailListTreatment(List<V_HIS_TREATMENT_FEE> hisTreatments, List<HIS_SERE_SERV> ListSereServ, List<HIS_TRANSACTION> listDeposit1s, List<HIS_TRANSACTION> listRepay1s, List<HIS_TRANSACTION> listBill1s)
        {
            if (!(IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(hisTreatments)))
                return;

            foreach (var treatment in hisTreatments)
            {
                List<HIS_TRANSACTION> listBillSub = listBill1s.Where(p => p.TREATMENT_ID == treatment.ID).ToList();
                List<HIS_TRANSACTION> listDepositSub = listDeposit1s.Where(p => p.TREATMENT_ID == treatment.ID).ToList();
                List<HIS_TRANSACTION> listRepaySub = listRepay1s.Where(p => p.TREATMENT_ID == treatment.ID).ToList();
                List<HIS_SERE_SERV> listSereSerrvSub = ListSereServ.Where(p => p.TDL_TREATMENT_ID == treatment.ID).ToList();
                if (castFilter.IS_NO_PAY == true && listBillSub.Count > 0)
                {
                    continue;
                }
                if (listBillSub.Count <= 0)
                {
                    //tổng tiền tạm ứng - hoàn ứng = tổng tiền bệnh nhân phải trả
                    if (treatment.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE && !(Math.Truncate(((treatment.TOTAL_DEPOSIT_AMOUNT ?? 0) - (treatment.TOTAL_REPAY_AMOUNT ?? 0) + ((treatment.TOTAL_BILL_AMOUNT ?? 0) - (treatment.TOTAL_BILL_TRANSFER_AMOUNT ?? 0))) - ((treatment.TOTAL_PATIENT_PRICE ?? 0) - (treatment.TOTAL_BILL_EXEMPTION ?? 0) - (treatment.TOTAL_BILL_FUND ?? 0))) == 0))
                    {
                        continue;
                    }
                    else
                    {
                        long branchId = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).BRANCH_ID;
                        if (castFilter.BRANCH_ID.HasValue && castFilter.BRANCH_ID.Value != branchId)
                        {
                            continue;
                        }
                    }
                }
                //nếu bệnh nhân không thanh toán tại chi nhánh
                else if (castFilter.BRANCH_ID.HasValue && !listBillSub.Exists(o => listCashierRoom.Exists(p => o.CASHIER_ROOM_ID == p.ID && castFilter.BRANCH_ID.Value == p.BRANCH_ID)))
                {
                    continue;
                }

                if (this.castFilter.IS_ALL_TREAT == true || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                {
                    Mrs00526RDO rdo = new Mrs00526RDO(treatment);
                    rdo.LOG_TIME_EXAM = treatment.IN_TIME;

                    if (listSereSerrvSub.Count > 0)
                    {
                        List<HIS_SERE_SERV> hisSereServHein = new List<HIS_SERE_SERV>();
                        List<HIS_SERE_SERV> hisSereServFee = new List<HIS_SERE_SERV>();
                        foreach (var sereServ in listSereSerrvSub)
                        {
                            string keyTreaDp = string.Format("{0}_{1}", sereServ.TDL_TREATMENT_CODE, sereServ.TDL_REQUEST_DEPARTMENT_ID);
                            if (!this.dicTreaDp.ContainsKey(keyTreaDp))
                            {
                                this.dicTreaDp[keyTreaDp] = new Mrs00526RDO(treatment);
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            }
                            if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                hisSereServHein.Add(sereServ);
                                if (String.IsNullOrEmpty(rdo.HEIN_CARD_NUMBER) && !String.IsNullOrEmpty(sereServ.HEIN_CARD_NUMBER))
                                {
                                    rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                                    this.dicTreaDp[keyTreaDp].HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                                }
                                rdo.TOTAL_HEIN_LIMIT_PRICE += (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                rdo.TOTAL_HEIN_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                this.dicTreaDp[keyTreaDp].TOTAL_HEIN_LIMIT_PRICE += (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                this.dicTreaDp[keyTreaDp].TOTAL_HEIN_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                                decimal totalHeinPrice = 0;
                                decimal HeinPrice = 0;
                                if (sereServ.HEIN_LIMIT_PRICE.HasValue)
                                {
                                    if (sereServ.PRICE == 0)
                                    {
                                        HeinPrice = 0;
                                    }
                                    else
                                    {
                                        HeinPrice = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                                    }
                                }
                                else
                                {
                                    HeinPrice = sereServ.PRICE * (1 + sereServ.VAT_RATIO);
                                }
                                totalHeinPrice += HeinPrice * sereServ.AMOUNT;

                                rdo.TOTAL_DIFFERENCE_PRICE += ((sereServ.VIR_PRICE ?? 0) - HeinPrice) * sereServ.AMOUNT;
                                rdo.TOTAL_HEIN_PAY_YOURSELF_PRICE += (HeinPrice - (sereServ.VIR_HEIN_PRICE ?? 0) - (sereServ.VIR_PATIENT_PRICE_BHYT ?? 0)) * sereServ.AMOUNT;
                                this.dicTreaDp[keyTreaDp].TOTAL_DIFFERENCE_PRICE += ((sereServ.VIR_PRICE ?? 0) - HeinPrice) * sereServ.AMOUNT;
                                this.dicTreaDp[keyTreaDp].TOTAL_HEIN_PAY_YOURSELF_PRICE += (HeinPrice - (sereServ.VIR_HEIN_PRICE ?? 0) - (sereServ.VIR_PATIENT_PRICE_BHYT ?? 0)) * sereServ.AMOUNT;
                                rdo.TOTAL_HEIN_PRICE += totalHeinPrice;
                                this.dicTreaDp[keyTreaDp].TOTAL_HEIN_PRICE += totalHeinPrice;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += sereServ.OTHER_SOURCE_PRICE * sereServ.AMOUNT ?? 0;
                                this.dicTreaDp[keyTreaDp].TOTAL_OTHER_SOURCE_PRICE += sereServ.OTHER_SOURCE_PRICE * sereServ.AMOUNT ?? 0;
                            }
                            else
                            {
                                hisSereServFee.Add(sereServ);
                                if (sereServ.VIR_TOTAL_HEIN_PRICE == 0)
                                {
                                    rdo.TOTAL_FEE_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                    this.dicTreaDp[keyTreaDp].TOTAL_FEE_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                    this.dicTreaDp[keyTreaDp].TOTAL_OTHER_SOURCE_PRICE += sereServ.OTHER_SOURCE_PRICE ?? 0;
                                }
                            }
                            rdo.VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                            this.dicTreaDp[keyTreaDp].VIR_TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                            this.dicTreaDp[keyTreaDp].TOTAL_OTHER_SOURCE_PRICE += sereServ.OTHER_SOURCE_PRICE ?? 0;

                        }

                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co sereServ nao: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                    }

                    if (listDepositSub.Count > 0)
                    {
                        foreach (var deposit in listDepositSub)
                        {
                            long reqDpId = dicTranDepa.ContainsKey(deposit.ID) ? dicTranDepa[deposit.ID]: 0;

                            string keyTreaDp = string.Format("{0}_{1}", deposit.TDL_TREATMENT_CODE, reqDpId);
                            if (!this.dicTreaDp.ContainsKey(keyTreaDp))
                            {
                                this.dicTreaDp[keyTreaDp] = new Mrs00526RDO(treatment);
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_ID = reqDpId;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            }
                            if (deposit.TDL_SERE_SERV_DEPOSIT_COUNT == 0 || deposit.TDL_SERE_SERV_DEPOSIT_COUNT == null)
                            {
                                rdo.TOTAL_DEPOSIT_AMOUNT += deposit.AMOUNT;
                                this.dicTreaDp[keyTreaDp].TOTAL_DEPOSIT_AMOUNT += deposit.AMOUNT;
                            }
                            else
                            {
                                rdo.TOTAL_BILL_EXAM_AMOUNT += deposit.AMOUNT;
                                this.dicTreaDp[keyTreaDp].TOTAL_BILL_EXAM_AMOUNT += deposit.AMOUNT;
                            }
                        }
                    }

                    if (listRepaySub.Count > 0)
                    {
                        foreach (var repay in listRepaySub)
                        {
                            long reqDpId = dicTranDepa.ContainsKey(repay.ID) ? dicTranDepa[repay.ID] : 0;

                            string keyTreaDp = string.Format("{0}_{1}", repay.TDL_TREATMENT_CODE, reqDpId);
                            if (!this.dicTreaDp.ContainsKey(keyTreaDp))
                            {
                                this.dicTreaDp[keyTreaDp] = new Mrs00526RDO(treatment);
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_ID = reqDpId;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            }
                            if (repay != null && repay.REPAY_REASON_ID.HasValue)
                            {

                                if (repay.REPAY_REASON_ID == IMSys.DbConfig.HIS_RS.HIS_REPAY_REASON.ID__HOAN_TUNT)//lý do hoàn lại tiền tạm ứng
                                {
                                    rdo.TOTAL_DEPOSIT_AMOUNT -= repay.AMOUNT;
                                    this.dicTreaDp[keyTreaDp].TOTAL_DEPOSIT_AMOUNT -= repay.AMOUNT;
                                }
                                else if (repay.REPAY_REASON_ID == IMSys.DbConfig.HIS_RS.HIS_REPAY_REASON.ID__HOAN_NT_RV || repay.REPAY_REASON_ID == IMSys.DbConfig.HIS_RS.HIS_REPAY_REASON.ID__RV)//lý do hoàn ứng ra viện
                                {
                                    rdo.TOTAL_WITHDRAW_AMOUNT += repay.AMOUNT;
                                    this.dicTreaDp[keyTreaDp].TOTAL_WITHDRAW_AMOUNT += repay.AMOUNT;
                                }
                                else
                                {
                                    rdo.TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                                    this.dicTreaDp[keyTreaDp].TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                                }
                                //}
                            }
                            else
                            {
                                rdo.TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                                this.dicTreaDp[keyTreaDp].TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                            }
                        }
                    }

                    rdo.TOTAL_PATIENT_AMOUNT = (treatment.TOTAL_BILL_AMOUNT ?? 0) - (treatment.TOTAL_BILL_TRANSFER_AMOUNT ?? 0) - (treatment.TOTAL_BILL_FUND ?? 0) - (treatment.TOTAL_BILL_EXEMPTION ?? 0);
                    rdo.TOTAL_BILL_FUND = (treatment.TOTAL_BILL_FUND ?? 0) + (treatment.TOTAL_BILL_EXEMPTION ?? 0);
                    rdo.TOTAL_OTHER_SOURCE_PRICE = treatment.TOTAL_OTHER_SOURCE_PRICE ?? 0;
                    rdo.HAS_PAY = listBillSub.Count > 0;

                    if (listBillSub.Count > 0)
                    {
                        foreach (var bill in listBillSub)
                        {
                            long reqDpId = dicTranDepa.ContainsKey(bill.ID) ? dicTranDepa[bill.ID] : 0;

                            string keyTreaDp = string.Format("{0}_{1}", bill.TDL_TREATMENT_CODE, reqDpId);
                            if (!this.dicTreaDp.ContainsKey(keyTreaDp))
                            {
                                this.dicTreaDp[keyTreaDp] = new Mrs00526RDO(treatment);
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_ID = reqDpId;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                                this.dicTreaDp[keyTreaDp].REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == reqDpId) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                            }
                            this.dicTreaDp[keyTreaDp].TOTAL_PATIENT_AMOUNT += bill.AMOUNT - (bill.KC_AMOUNT ?? 0) - (bill.TDL_BILL_FUND_AMOUNT ?? 0) - (bill.EXEMPTION ?? 0);
                            this.dicTreaDp[keyTreaDp].TOTAL_BILL_FUND += (bill.TDL_BILL_FUND_AMOUNT ?? 0) + (bill.EXEMPTION ?? 0);
                            
                            this.dicTreaDp[keyTreaDp].HAS_PAY = true;
                        }
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
                //dicSingleTag.Add("CASHIER_LOGINNAME", cashierLoginname);
                //dicSingleTag.Add("CASHIER_USERNAME", cashierUsername);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Department", listDepartment);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", listRdo);
                if (dicTreaDp == null)
                {
                    dicTreaDp = new Dictionary<string, Mrs00526RDO>();
                }
                    exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TreaDp", dicTreaDp.Values.ToList());
                    exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Dp", dicTreaDp.Values.GroupBy(o => o.REQUEST_DEPARTMENT_ID).Select(p => p.First()).ToList());
                    exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Dp", "TreaDp", "REQUEST_DEPARTMENT_ID", "REQUEST_DEPARTMENT_ID");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Department", "Report", "DEPARTMENT_ID", "DEPARTMENT_ID");
                exportSuccess = exportSuccess && objectTag.SetUserFunction(store, "FuncRowNumber", new RDOCustomerFuncManyRownumberData());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
                dicSingleTag.Add("PROTECT", "");
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
