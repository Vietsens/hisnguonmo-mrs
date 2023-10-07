using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentLogging;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00533
{
    public class Mrs00533Processor : AbstractProcessor
    {
        Mrs00533Filter castFilter = null;
        List<V_HIS_TREATMENT> ListTreatment;
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();

        List<Mrs00533RDO> listRdo = new List<Mrs00533RDO>();
        List<Mrs00533RDO> listDepartment = new List<Mrs00533RDO>();

        public Mrs00533Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00533Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00533Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_TREATMENT, Mrs00533 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                ListTreatment = new List<V_HIS_TREATMENT>();

                HisTreatmentLoggingFilterQuery logFilter = new HisTreatmentLoggingFilterQuery();
                logFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                logFilter.CREATE_TIME_TO = castFilter.TIME_TO;
                logFilter.TREATMENT_LOG_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_LOG_TYPE.ID__TKVH;
                var listTreatmentLog = new HisTreatmentLoggingManager(paramGet).Get(logFilter);

                if (IsNotNullOrEmpty(listTreatmentLog))
                {
                    var treatmentIds = listTreatmentLog.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                        treatFilter.IDs = limit;
                        treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                        var treat = new HisTreatmentManager(paramGet).GetView(treatFilter);
                        if (IsNotNullOrEmpty(treat))
                        {
                            ListTreatment.AddRange(treat);
                        }
                    }
                }

                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ListTreatment = ListTreatment.Where(o => o.IS_TEMPORARY_LOCK == 1).ToList();
                }

                if (castFilter.BRANCH_ID.HasValue)
                {
                    HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
                    cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID.Value;
                    listCashierRoom = new HisCashierRoomManager(paramGet).GetView(cashierRoomFilter);
                }

                MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery mateFilter = new MOS.MANAGER.HisMaterialType.HisMaterialTypeFilterQuery();
                listMaterialType = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).Get(mateFilter);

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT, Mrs00533." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
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
                    List<V_HIS_TRANSACTION> ListTran = new List<V_HIS_TRANSACTION>();
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

                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery();
                        feeFilter.IDs = listTreatmentId;
                        List<V_HIS_TREATMENT_FEE> ListTreatmentFee = new HisTreatmentManager(paramGet).GetFeeView(feeFilter);

                        // các transaction
                        HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                        transactionFilter.TREATMENT_IDs = listTreatmentId;
                        transactionFilter.IS_CANCEL = false;
                        transactionFilter.HAS_SALL_TYPE = false;
                        transactionFilter.ORDER_DIRECTION = "DESC";
                        transactionFilter.ORDER_FIELD = "CREATE_TIME";
                        List<V_HIS_TRANSACTION> listTransactions = new HisTransactionManager(paramGet).GetView(transactionFilter);
                        if (IsNotNullOrEmpty(listTransactions) && !String.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME))
                        {
                            listTransactions = listTransactions.Where(o => o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                        }
                        //var listTransactionIds = listTransactions.Select(s => s.ID).Distinct().ToList();
                        ListTran.AddRange(listTransactions);
                        List<V_HIS_TRANSACTION> listDeposit1s = new List<V_HIS_TRANSACTION>();
                        List<V_HIS_TRANSACTION> listRepay1s = new List<V_HIS_TRANSACTION>();
                        listDeposit1s = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                        listRepay1s = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                        if (!paramGet.HasException)
                        {
                            ProcessDetailListTreatment(hisTreatments, ListSereServ, ListPatientTypeAlter, ListTreatmentFee, listDeposit1s, listRepay1s, ListTran);
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

        private void ProcessDetailListTreatment(List<V_HIS_TREATMENT> hisTreatments, List<V_HIS_SERE_SERV> ListSereServ, List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter, List<V_HIS_TREATMENT_FEE> ListTreatmentFee, List<V_HIS_TRANSACTION> listDeposit1s, List<V_HIS_TRANSACTION> listRepay1s, List<V_HIS_TRANSACTION> ListTran)
        {
            if (!(IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListPatientTypeAlter) && IsNotNullOrEmpty(ListTreatmentFee)))
                return;

            Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
            Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>();
            Dictionary<long, V_HIS_TREATMENT_FEE> dicTreatmentFee = new Dictionary<long, V_HIS_TREATMENT_FEE>();
            Dictionary<long, V_HIS_TRANSACTION> dicTranBill = new Dictionary<long, V_HIS_TRANSACTION>();
            Dictionary<long, V_HIS_TRANSACTION> dicTranCashier = new Dictionary<long, V_HIS_TRANSACTION>();
            Dictionary<long, V_HIS_TRANSACTION> dicTranDepoRepay = new Dictionary<long, V_HIS_TRANSACTION>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicDeposit1 = new Dictionary<long, List<V_HIS_TRANSACTION>>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicRepay1 = new Dictionary<long, List<V_HIS_TRANSACTION>>();
            Dictionary<long, List<V_HIS_TRANSACTION>> dicBill = new Dictionary<long, List<V_HIS_TRANSACTION>>();

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

            List<long> listCashierRoomId = new List<long>();
            if (IsNotNullOrEmpty(this.listCashierRoom))
            {
                listCashierRoomId = listCashierRoom.Select(s => s.ID).ToList();
            }

            foreach (var item in ListTran)
            {
                if (!item.TREATMENT_ID.HasValue)
                    continue;
                dicTranDepoRepay[item.ID] = item;
                if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    if ((!this.castFilter.BRANCH_ID.HasValue) || listCashierRoomId.Contains(item.CASHIER_ROOM_ID))
                    {
                        if (!dicTranCashier.ContainsKey(item.TREATMENT_ID.Value))
                        {
                            dicTranCashier[item.TREATMENT_ID.Value] = item;
                        }
                    }
                    if (!dicBill.ContainsKey(item.TREATMENT_ID.Value))
                        dicBill[item.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                    dicBill[item.TREATMENT_ID.Value].Add(item);
                    if (dicTranBill.ContainsKey(item.TREATMENT_ID.Value))
                        continue;
                    dicTranBill[item.TREATMENT_ID.Value] = item;
                }
            }

            foreach (var sereServ in ListSereServ)
            {
                if (IsNotNull(sereServ)) //; && !sereServ.WITHDRAW_ID.HasValue)
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

            foreach (var treatmentFee in ListTreatmentFee)
            {
                if (IsNotNull(treatmentFee))
                {
                    dicTreatmentFee[treatmentFee.ID] = treatmentFee;
                }
            }

            foreach (var deposit1 in listDeposit1s)
            {
                if (IsNotNull(deposit1) && deposit1.TREATMENT_ID.HasValue)
                {
                    //tạm ứng mới
                    if (!dicDeposit1.ContainsKey(deposit1.TREATMENT_ID.Value))
                        dicDeposit1[deposit1.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                    dicDeposit1[deposit1.TREATMENT_ID.Value].Add(deposit1);
                }
            }

            foreach (var repay in listRepay1s)
            {
                if (IsNotNull(repay) && repay.TREATMENT_ID.HasValue)
                {
                    if (!dicRepay1.ContainsKey(repay.TREATMENT_ID.Value))
                        dicRepay1[repay.TREATMENT_ID.Value] = new List<V_HIS_TRANSACTION>();
                    dicRepay1[repay.TREATMENT_ID.Value].Add(repay);
                }
            }

            foreach (var treatment in hisTreatments)
            {
                if (!dicTranCashier.ContainsKey(treatment.ID))
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
                if (valid)
                {
                    Mrs00533RDO rdo = new Mrs00533RDO(treatment);
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
                        foreach (var sereServ in dicSereServ[treatment.ID])
                        {
                            if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                hisSereServHein.Add(sereServ);
                                if (!rdo.TOTAL_HEIN_LIMIT_PRICE.HasValue) rdo.TOTAL_HEIN_LIMIT_PRICE = 0;
                                if (!rdo.TOTAL_HEIN_PATIENT_PRICE.HasValue) rdo.TOTAL_HEIN_PATIENT_PRICE = 0;
                                if (!rdo.TOTAL_DIFFERENCE_PRICE.HasValue) rdo.TOTAL_DIFFERENCE_PRICE = 0;
                                if (!rdo.VIR_TOTAL_PATIENT_PRICE.HasValue) rdo.VIR_TOTAL_PATIENT_PRICE = 0;

                                rdo.VIR_TOTAL_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                rdo.TOTAL_HEIN_LIMIT_PRICE += (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                                rdo.TOTAL_HEIN_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                                //HIS_MATERIAL_TYPE material = listMaterialType.FirstOrDefault(o => o.SERVICE_ID == sereServ.SERVICE_ID);
                                //if (material != null && material.IS_STENT == 1)
                                //{
                                //    totalHeinPrice += (sereServ.ORIGINAL_PRICE * sereServ.AMOUNT);
                                //}
                                //else 
                                    if (sereServ.HEIN_LIMIT_PRICE.HasValue)
                                {
                                    if (sereServ.PRICE < sereServ.HEIN_LIMIT_PRICE.Value)
                                    {
                                        totalHeinPrice += (sereServ.AMOUNT * sereServ.PRICE * (1 + sereServ.VAT_RATIO));
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DIFFERENCE_PRICE += Math.Abs(sereServ.AMOUNT * (sereServ.PRICE * (1 + sereServ.VAT_RATIO) - sereServ.HEIN_LIMIT_PRICE.Value));
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
                                if (sereServ.VIR_TOTAL_HEIN_PRICE == 0)
                                {
                                    if (rdo.TOTAL_FEE_PATIENT_PRICE == null)
                                        rdo.TOTAL_FEE_PATIENT_PRICE = 0;
                                    rdo.TOTAL_FEE_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                }
                            }
                        }

                        rdo.TOTAL_HEIN_PRICE = totalHeinPrice;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Ho so dieu tri khong co sereServ nao: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                    }

                    if (dicTreatmentFee.ContainsKey(treatment.ID))
                    {
                        if (dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.HasValue && dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.Value > 0)
                        {
                            rdo.VIR_TOTAL_PATIENT_PRICE = dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE;
                        }
                        if (dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.HasValue && dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.Value > 0)
                        {
                            rdo.TOTAL_BILL_FUND = dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND;
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Khong lay duoc treatment_fee cua ho so dieu tri: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                    }

                    if (dicDeposit1.ContainsKey(treatment.ID))
                    {
                        foreach (var deposit in dicDeposit1[treatment.ID])
                        {
                            if (deposit.TREATMENT_TYPE_ID.HasValue)
                            {
                                if (deposit.TREATMENT_TYPE_ID.Value != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
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

                    if (dicRepay1.ContainsKey(treatment.ID))
                    {
                        foreach (var repay in dicRepay1[treatment.ID])
                        {
                            if (repay != null && repay.REPAY_REASON_ID.HasValue)
                            {
                                if (repay.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__01)//lý do hoàn lại tiền tạm ứng
                                {
                                    rdo.TOTAL_DEPOSIT_AMOUNT -= repay.AMOUNT;
                                }
                                if (repay.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__03)//lý do hoàn ứng ra viện
                                {
                                    if (rdo.TOTAL_WITHDRAW_AMOUNT == null)
                                        rdo.TOTAL_WITHDRAW_AMOUNT = 0;
                                    rdo.TOTAL_WITHDRAW_AMOUNT += repay.AMOUNT;
                                }
                                if (repay.REPAY_REASON_ID != HisRepayReasonCFG.get_REPAY_REASON_CODE__03 && repay.REPAY_REASON_ID != HisRepayReasonCFG.get_REPAY_REASON_CODE__01)
                                {
                                    rdo.TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                                }
                            }
                            else
                            {
                                rdo.TOTAL_BILL_EXAM_AMOUNT -= repay.AMOUNT;
                            }
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("TRUE/FALSE", dicTranBill.ContainsKey(treatment.ID)));
                    }

                    if (dicBill.ContainsKey(treatment.ID))
                    {
                        var listBill = dicBill[treatment.ID].OrderByDescending(o => o.TRANSACTION_TIME).ToList();
                        rdo.TOTAL_PATIENT_AMOUNT = listBill.FirstOrDefault().AMOUNT - (listBill.FirstOrDefault().KC_AMOUNT ?? 0) - (listBill.FirstOrDefault().TDL_BILL_FUND_AMOUNT ?? 0);
                    }
                    listRdo.Add(rdo);
                }
            }
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
                        var listSub = group.ToList<Mrs00533RDO>();
                        Mrs00533RDO rdo = new Mrs00533RDO();
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
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Department", listDepartment);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", listRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Department", "Report", "DEPARTMENT_ID", "DEPARTMENT_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
