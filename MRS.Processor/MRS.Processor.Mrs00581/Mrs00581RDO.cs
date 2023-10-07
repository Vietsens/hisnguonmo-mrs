using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00581
{
    public class Mrs00581RDO
    {
        public long TREATMENT_ID { get; set; }
        public string TREATMENT_CODE { get; set; }
        public string TRANSACTION_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string GENDER_NAME { get; set; }
        public long DOB { get; set; }

        public string CASHIER_LOGINNAME { get; set; }
        public string CASHIER_USERNAME { get; set; }
        public string CASHIER_ROOM_CODE { get; set; }
        public string CASHIER_ROOM_NAME { get; set; }

        public long ACCOUNT_BOOK_ID { get; set; }
        public string ACCOUNT_BOOK_CODE { get; set; }
        public string ACCOUNT_BOOK_NAME { get; set; }

        public decimal? EXEMPTION { get; set; }
        public decimal TDL_BILL_FUND_AMOUNT { get; set; }
        public string EXEMPTION_REASON { get; set; }
        public decimal KC_AMOUNT { get; set; }

        public long? TRANSACTION_TIME { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public decimal? TOTAL_PRICE_BHYT { get; set; }
        public decimal? TOTAL_PRICE_FEE { get; set; }
        public decimal? TOTAL_PRICE_BHYT_AFTER { get; set; }
        public decimal? TOTAL_PRICE_FEE_AFTER { get; set; }
        //thong tin hoa don dien tu
        public string INVOICE_CODE { get; set; }
        public string INVOICE_SYS { get; set; }
        public string EINVOICE_NUM_ORDER { get; set; }
        public long? EINVOICE_TIME { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string ROOM_NAME { get; set; }
        public string ROOM_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string NOTE { set; get; }
        public short? IS_CANCEL { set; get; }
        public string REQUEST_DEPARTMENT_CODE { set; get; }
        public string REQUEST_DEPARTMENT_NAME { set; get; }
        public string END_DEPARTMENT_CODE { set; get; }
        public string END_DEPARTMENT_NAME { set; get; }
        public Mrs00581RDO() { }

        public Mrs00581RDO(V_HIS_TRANSACTION data, List<HIS_SERVICE_REQ> listServiceReq, List<HIS_SERE_SERV_BILL> listSereServBill, V_HIS_TREATMENT treatment, ref Dictionary<string, SERVICE> dicService, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, List<HIS_TRANSACTION> listDeposit, List<HIS_TRANSACTION> listRepay, long requestDepartmentId, long requestRoomId, BILL_BALANCE billBalance, Dictionary<long, HIS_SERVICE> dicSv,Mrs00581Filter filter)
        {
            try
            {
                List<HIS_TRANSACTION> depositSub = listDeposit.Where(o => o.TREATMENT_ID == data.TREATMENT_ID).ToList();
                List<HIS_TRANSACTION> repaySub = listRepay.Where(o => o.TREATMENT_ID == data.TREATMENT_ID).ToList();
                DateTime? billTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(data.TRANSACTION_TIME);
                repaySub = (repaySub != null && repaySub.Count > 0) ? repaySub.Where(o => IsDateDiffOK(o.TRANSACTION_TIME, data.TRANSACTION_TIME)).ToList() : new List<HIS_TRANSACTION>();

                List<HIS_SERE_SERV_BILL> sereServBillSub = listSereServBill.Where(o => o.BILL_ID == data.ID).ToList();
                List<HIS_SERVICE_REQ> serviceReqSub = listServiceReq.Where(o => sereServBillSub.Exists(p => p.TDL_SERVICE_REQ_ID == o.ID)).ToList();
                IS_CANCEL = data.IS_CANCEL;
                var lastServiceReq = serviceReqSub.OrderBy(q => q.INTRUCTION_TIME).LastOrDefault() ?? new HIS_SERVICE_REQ();
                this.PAY_FORM_CODE = HisPayFormCFG.ListPayForm.FirstOrDefault(p => p.ID == data.PAY_FORM_ID).PAY_FORM_CODE ?? new HIS_PAY_FORM().PAY_FORM_CODE;
                this.PAY_FORM_NAME = HisPayFormCFG.ListPayForm.FirstOrDefault(p => p.ID == data.PAY_FORM_ID).PAY_FORM_NAME ?? new HIS_PAY_FORM().PAY_FORM_NAME;
                this.DIEN = lastServiceReq.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? "DT" : "KH";
                this.PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID;
                this.TREATMENT_DAY_COUNT = treatment.TREATMENT_DAY_COUNT ?? 0;
                if (treatment.END_DEPARTMENT_ID != null)
                {
                    END_DEPARTMENT_CODE = treatment.END_DEPARTMENT_CODE;
                    END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                }
                else
                {
                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
                    END_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                    END_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                }
                if (requestDepartmentId > 0)
                {
                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == requestDepartmentId);
                    if (department != null)
                    {
                        this.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                        this.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    }
                }
                else
                {
                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.LAST_DEPARTMENT_ID);
                    if (department != null)
                    {
                        this.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                        this.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                    }
                }
                if (requestRoomId > 0)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == requestRoomId);
                    if (room != null)
                    {
                        this.ROOM_CODE = room.ROOM_CODE;
                        this.ROOM_NAME = room.ROOM_NAME;
                    }
                }
                else
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == (treatment.IN_ROOM_ID ?? treatment.END_ROOM_ID ?? treatment.TDL_FIRST_EXAM_ROOM_ID ?? 0));
                    if (room != null)
                    {
                        this.ROOM_CODE = room.ROOM_CODE;
                        this.ROOM_NAME = room.ROOM_NAME;
                    }
                }
                this.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                this.SERVICE_TYPE_NAME = string.Join(", ", HisServiceTypeCFG.HisServiceTypes.Where(p => sereServBillSub.Exists(q => q.TDL_SERVICE_TYPE_ID == p.ID)).Select(o => o.SERVICE_TYPE_NAME).ToList());
                this.ACCOUNT_BOOK_CODE = data.ACCOUNT_BOOK_CODE;
                this.ACCOUNT_BOOK_ID = data.ACCOUNT_BOOK_ID;
                this.ACCOUNT_BOOK_NAME = data.ACCOUNT_BOOK_NAME;
                this.CASHIER_LOGINNAME = data.CASHIER_LOGINNAME;
                this.CASHIER_ROOM_CODE = data.CASHIER_ROOM_CODE;
                this.CASHIER_ROOM_NAME = data.CASHIER_ROOM_NAME;
                this.CASHIER_USERNAME = data.CASHIER_USERNAME;
                this.TRANSACTION_TIME = data.TRANSACTION_TIME;
                this.TRANSACTION_DATE = data.TRANSACTION_DATE;
                this.TRANSACTION_MONTH = (long)(data.TRANSACTION_DATE / 100000000);
                this.TRANSACTION_CODE = data.TRANSACTION_CODE;
                this.TEMPLATE_CODE = data.TEMPLATE_CODE;
                this.SYMBOL_CODE = data.SYMBOL_CODE;
                this.NUM_ORDER = data.NUM_ORDER;
                //thong tin hoa don dien tu
                this.INVOICE_CODE = data.INVOICE_CODE;
                this.INVOICE_SYS = data.INVOICE_SYS;
                this.EINVOICE_NUM_ORDER = data.EINVOICE_NUM_ORDER;
                this.EINVOICE_TIME = data.EINVOICE_TIME ?? 99990101000000;
                this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(data.TRANSACTION_TIME);
                this.DOB = data.TDL_PATIENT_DOB ?? 0;
                this.HIEN_DU = billBalance.HIEN_DU ?? 0;
                this.EXEMPTION = data.EXEMPTION;
                this.TDL_BILL_FUND_AMOUNT = data.TDL_BILL_FUND_AMOUNT ?? 0;
                this.KC_AMOUNT = data.KC_AMOUNT ?? 0;
                this.PATIENT_CODE = data.TDL_PATIENT_CODE;

                this.TREATMENT_ID = data.TREATMENT_ID ?? 0;
                this.VIR_PATIENT_NAME = data.TDL_PATIENT_NAME;
                this.TDL_PATIENT_ADDRESS = data.TDL_PATIENT_ADDRESS;
                this.IN_TIME = treatment.IN_TIME;
                this.OUT_TIME = treatment.OUT_TIME ?? 0;
                this.EXEMPTION_REASON = data.EXEMPTION_REASON;
                this.TREATMENT_CODE = data.TREATMENT_CODE;
                this.TOTAL_PRICE_BHYT = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE);
                this.TOTAL_PRICE_FEE = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE);
                this.TOTAL_PRICE_BHYT_AFTER = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE);
                this.TOTAL_PRICE_FEE_AFTER = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE);
                this.TOTAL_PRICE = sereServBillSub.Sum(o => o.PRICE);
                this.TOTAL_PRICE_ROUND = Math.Round(sereServBillSub.Sum(o => o.PRICE), 0);
                this.CHI_PHI = sereServBillSub.Sum(o => (o.TDL_TOTAL_HEIN_PRICE ?? 0) + (o.TDL_TOTAL_PATIENT_PRICE ?? 0));
                if (TDL_BILL_FUND_AMOUNT > 0)
                {
                    this.TOTAL_PRICE_BHYT_AFTER = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE) - data.TDL_BILL_FUND_AMOUNT ?? 0;
                    this.TOTAL_PRICE_FEE_AFTER = sereServBillSub.Where(o => o.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Sum(o => o.TDL_TOTAL_PATIENT_PRICE) - data.TDL_BILL_FUND_AMOUNT ?? 0;
                    if (this.TOTAL_PRICE_BHYT_AFTER < 0)
                    {
                        this.TOTAL_PRICE_BHYT_AFTER = 0;
                    }
                    if (this.TOTAL_PRICE_FEE_AFTER < 0)
                    {
                        this.TOTAL_PRICE_FEE_AFTER = 0;
                    }
                }
                if (depositSub != null)
                {
                    this.DEPOSIT_AMOUNT = depositSub.Sum(o => o.AMOUNT);
                    this.DEPOSIT_AMOUNT_ROUND = Math.Round(depositSub.Sum(o => o.AMOUNT), 0);
                    this.DEPOSIT_NUM_ORDER = string.Join(";", depositSub.Select(o => o.NUM_ORDER).Distinct().ToList());
                }
                if (repaySub != null)
                {
                    this.REPAY_AMOUNT = repaySub.Sum(o => o.AMOUNT);
                    this.REPAY_NUM_ORDER = string.Join(";", repaySub.Select(o => o.NUM_ORDER).Distinct().ToList());

                }
                var serviceIds = sereServBillSub.Select(o=>o.TDL_SERVICE_ID??0).Distinct().ToList();
                
                var dicCategoryCode = serviceIds.ToDictionary(p=>p,q=>CategoryCode(q, listHisServiceRetyCat));
                var dicParentCode = serviceIds.ToDictionary(p=>p,q=>ParentCode(q, dicSv));
                var dicServiceCode = serviceIds.ToDictionary(p=>p,q=>SvCode(q, dicSv));
                var dicSvtCode = HisServiceTypeCFG.HisServiceTypes.ToDictionary(p=>p.ID,q=>q.SERVICE_TYPE_CODE);
                dicSvtCode.Add(0,"");
                var dicPatyCode = HisPatientTypeCFG.PATIENT_TYPEs.ToDictionary(p=>p.ID,q=>q.PATIENT_TYPE_CODE);
                dicPatyCode.Add(0,"");
                this.DIC_PRICE = sereServBillSub.GroupBy(o => string.Format(filter.KEY_PRICE ?? "{0}", dicSvtCode[o.TDL_SERVICE_TYPE_ID ?? 0], dicParentCode[o.TDL_SERVICE_ID ?? 0], dicCategoryCode[o.TDL_SERVICE_ID ?? 0], dicServiceCode[o.TDL_SERVICE_ID ?? 0], dicPatyCode[o.TDL_PATIENT_TYPE_ID ?? 0], dicPatyCode[o.TDL_PRIMARY_PATIENT_TYPE_ID ?? 0])).ToDictionary(p => p.Key, q => q.Sum(s => (s.PRICE)));
                this.DIC_PRICE_TUTRA = sereServBillSub.GroupBy(o => string.Format(filter.KEY_PRICE ?? "{0}", dicSvtCode[o.TDL_SERVICE_TYPE_ID ?? 0], dicParentCode[o.TDL_SERVICE_ID ?? 0], dicCategoryCode[o.TDL_SERVICE_ID ?? 0], dicServiceCode[o.TDL_SERVICE_ID ?? 0], dicPatyCode[o.TDL_PATIENT_TYPE_ID ?? 0], dicPatyCode[o.TDL_PRIMARY_PATIENT_TYPE_ID ?? 0])).ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                this.DIC_GROUP = sereServBillSub.GroupBy(o => dicCategoryCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.PRICE)));
                this.DIC_GROUP_TUTRA = sereServBillSub.GroupBy(o => dicCategoryCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                this.DIC_PR = sereServBillSub.GroupBy(o => dicParentCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.PRICE)));
                this.DIC_PR_TUTRA = sereServBillSub.GroupBy(o => dicParentCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                this.DIC_SV = sereServBillSub.GroupBy(o => dicServiceCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.PRICE)));
                this.DIC_SV_TUTRA = sereServBillSub.GroupBy(o => dicServiceCode[o.TDL_SERVICE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                this.DIC_SVT = sereServBillSub.GroupBy(o => dicSvtCode[o.TDL_SERVICE_TYPE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.PRICE)));
                this.DIC_SVT_TUTRA = sereServBillSub.GroupBy(o => dicSvtCode[o.TDL_SERVICE_TYPE_ID ?? 0]).ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                this.TOTAL_TUTRA = sereServBillSub.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0));

                foreach (var item in sereServBillSub)
                {
                    switch (item.TDL_SERVICE_TYPE_ID)
                    {
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN:
                            this.TEIN_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA:
                            this.DIIM_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA:
                            this.SUIM_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN:
                            this.FUEX_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS:
                            this.ENDO_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT:
                            this.PT_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT:
                            this.TT_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G:
                            this.BED_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH:
                            this.EXAM_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC:
                            this.MEDICINE_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT:
                            this.MATERIAL_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU:
                            this.BLOOD_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL:
                            this.GPBL_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC:
                            this.KHAC_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN:
                            this.PHCN_TOTAL_PRICE += item.PRICE;
                            break;
                        case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN:
                            this.AN_TOTAL_PRICE += item.PRICE;
                            break;
                    }

                    this.TOTAL_PRICE_1 = Math.Round(this.TEIN_TOTAL_PRICE, 0)
                                       + Math.Round(this.AN_TOTAL_PRICE, 0)
                                       + Math.Round(this.BED_TOTAL_PRICE, 0)
                                       + Math.Round(this.BLOOD_TOTAL_PRICE, 0)
                                       + Math.Round(this.DIIM_TOTAL_PRICE, 0)
                                       + Math.Round(this.ENDO_TOTAL_PRICE, 0)
                                       + Math.Round(this.EXAM_TOTAL_PRICE, 0)
                                       + Math.Round(this.FUEX_TOTAL_PRICE, 0)
                                       + Math.Round(this.GPBL_TOTAL_PRICE, 0)
                                       + Math.Round(this.KHAC_TOTAL_PRICE, 0)
                                       + Math.Round(this.MATERIAL_TOTAL_PRICE, 0)
                                       + Math.Round(this.MEDICINE_TOTAL_PRICE, 0)
                                       + Math.Round(this.PHCN_TOTAL_PRICE, 0)
                                       + Math.Round(this.PT_TOTAL_PRICE, 0)
                                       + Math.Round(this.SUIM_TOTAL_PRICE, 0);

                    if ((double)(item.TDL_HEIN_RATIO ?? 0) < (double)0.71 && (double)(item.TDL_HEIN_RATIO ?? 0) > (double)0.69)
                    {
                        this.TOTAL_PRICE_30 += item.PRICE;
                        this.TOTAL_PRICE_30_DCT += (item.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    }
                    else if ((double)(item.TDL_HEIN_RATIO ?? 0) < (double)0.81 && (double)(item.TDL_HEIN_RATIO ?? 0) > (double)0.79)
                    {
                        this.TOTAL_PRICE_20 += item.PRICE;
                        this.TOTAL_PRICE_20_DCT += (item.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    }

                    else if ((double)(item.TDL_HEIN_RATIO ?? 0) < (double)0.96 && (double)(item.TDL_HEIN_RATIO ?? 0) > (double)0.94)
                    {
                        this.TOTAL_PRICE_5 += item.PRICE;
                        this.TOTAL_PRICE_5_DCT += (item.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    }
                    else
                    {
                        this.TOTAL_PRICE_OTHER += item.PRICE;
                        this.TOTAL_PRICE_OTHER_DCT += (item.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                    }


                    string serviceKey = (item.TDL_SERVICE_ID ?? 0).ToString() + "_" + Math.Round(item.TDL_PRICE ?? 0).ToString();
                    if (data.IS_CANCEL == null)
                    {
                        if (!dicService.ContainsKey(serviceKey))
                        {
                            dicService.Add(serviceKey, new SERVICE());
                            dicService[serviceKey].SERVICE_CODE = item.TDL_SERVICE_CODE;
                            dicService[serviceKey].SERVICE_NAME = item.TDL_SERVICE_NAME;
                            dicService[serviceKey].SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                            dicService[serviceKey].SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                            dicService[serviceKey].SERVICE_TYPE_NUM_ORDER = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == item.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).NUM_ORDER ?? 1000;
                            dicService[serviceKey].PRICE = item.TDL_PRICE ?? 0;
                            dicService[serviceKey].AMOUNT_IN = lastServiceReq.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? (item.TDL_AMOUNT ?? 0) : 0;
                            dicService[serviceKey].AMOUNT_OUT = lastServiceReq.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? 0 : (item.TDL_AMOUNT ?? 0);
                            dicService[serviceKey].TOTAL_PRICE = item.PRICE;
                        }
                        else
                        {
                            dicService[serviceKey].AMOUNT_IN += lastServiceReq.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? (item.TDL_AMOUNT ?? 0) : 0;
                            dicService[serviceKey].AMOUNT_OUT += lastServiceReq.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? 0 : (item.TDL_AMOUNT ?? 0);
                            dicService[serviceKey].TOTAL_PRICE += item.PRICE;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string SvCode(long serviceId, Dictionary<long, HIS_SERVICE> dicSv)
        {
            if (dicSv.ContainsKey(serviceId))
            {
                var sv = dicSv[serviceId];
                if (sv != null && sv.SERVICE_CODE != null)
                {
                    return sv.SERVICE_CODE;
                }
            }
            return "NONE";
        }

        private string ParentCode(long serviceId, Dictionary<long, HIS_SERVICE> dicSv)
        {
            if (dicSv.ContainsKey(serviceId))
            {
                var sv = dicSv[serviceId];
                if (sv != null && sv.PARENT_ID != null)
                {
                    if (dicSv.ContainsKey(sv.PARENT_ID ?? 0))
                    {
                        var pr = dicSv[sv.PARENT_ID ?? 0];
                        if (pr != null && pr.SERVICE_CODE != null)
                        {
                            return pr.SERVICE_CODE;
                        }
                    }
                }
            }
            return "NONE";
        }

        private bool IsDateDiffOK(long repayTime, long BillTime)
        {
            try
            {

                if (repayTime != null && BillTime != null)
                {
                    System.DateTime? dateBefore = System.DateTime.ParseExact(BillTime.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    System.DateTime? dateAfter = System.DateTime.ParseExact(repayTime.ToString(), "yyyyMMddHHmmss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                    if (dateBefore != null && dateAfter != null && dateAfter >= dateBefore)
                    {
                        TimeSpan difference = dateAfter.Value - dateBefore.Value;
                        return (double)difference.TotalSeconds < (double)60;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }

        }


        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                if (listHisServiceRetyCat.Count == 0)
                {
                    return "";

                }
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }


        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_TUTRA { get; set; }

        public Dictionary<string, decimal> DIC_SVT { get; set; }
        public Dictionary<string, decimal> DIC_SVT_TUTRA { get; set; }
        public decimal TOTAL_TUTRA { get; set; }
        public decimal TEIN_TOTAL_PRICE { get; set; }

        public decimal DIIM_TOTAL_PRICE { get; set; }

        public decimal SUIM_TOTAL_PRICE { get; set; }

        public decimal FUEX_TOTAL_PRICE { get; set; }

        public decimal ENDO_TOTAL_PRICE { get; set; }

        public decimal PT_TOTAL_PRICE { get; set; }

        public decimal TT_TOTAL_PRICE { get; set; }

        public decimal BED_TOTAL_PRICE { get; set; }

        public decimal EXAM_TOTAL_PRICE { get; set; }

        public decimal MEDICINE_TOTAL_PRICE { get; set; }

        public decimal MATERIAL_TOTAL_PRICE { get; set; }

        public decimal BLOOD_TOTAL_PRICE { get; set; }

        public decimal GPBL_TOTAL_PRICE { get; set; }

        public decimal KHAC_TOTAL_PRICE { get; set; }

        public decimal PHCN_TOTAL_PRICE { get; set; }

        public decimal AN_TOTAL_PRICE { get; set; }

        public long TRANSACTION_DATE { get; set; }

        public string DIEN { get; set; }

        public decimal TOTAL_PRICE { get; set; }

        public long TRANSACTION_MONTH { get; set; }

        public string TEMPLATE_CODE { get; set; }

        public string SYMBOL_CODE { get; set; }

        public long NUM_ORDER { get; set; }

        public string HEIN_CARD_NUMBER { get; set; }

        public long? PATIENT_TYPE_ID { get; set; }

        public decimal TREATMENT_DAY_COUNT { get; set; }

        public string TDL_PATIENT_ADDRESS { get; set; }

        public long IN_TIME { get; set; }

        public long OUT_TIME { get; set; }

        public decimal CHI_PHI { get; set; }

        public decimal DEPOSIT_AMOUNT { get; set; }

        public decimal TOTAL_PRICE_30 { get; set; }

        public decimal TOTAL_PRICE_20 { get; set; }

        public decimal TOTAL_PRICE_5 { get; set; }

        public decimal TOTAL_PRICE_OTHER { get; set; }

        public decimal REPAY_AMOUNT { get; set; }

        public string PAY_FORM_CODE { get; set; }

        public string PAY_FORM_NAME { get; set; }

        public decimal TOTAL_PRICE_ROUND { get; set; }

        public decimal DEPOSIT_AMOUNT_ROUND { get; set; }

        public decimal TOTAL_PRICE_1 { get; set; }

        public decimal HIEN_DU { get; set; }

        public string DEPOSIT_NUM_ORDER { get; set; }

        public string REPAY_NUM_ORDER { get; set; }

        public decimal TOTAL_PRICE_30_DCT { get; set; }

        public decimal TOTAL_PRICE_20_DCT { get; set; }

        public decimal TOTAL_PRICE_5_DCT { get; set; }

        public decimal TOTAL_PRICE_OTHER_DCT { get; set; }

        public Dictionary<string, decimal> DIC_PR { get; set; }

        public Dictionary<string, decimal> DIC_PR_TUTRA { get; set; }

        public Dictionary<string, decimal> DIC_SV { get; set; }

        public Dictionary<string, decimal> DIC_SV_TUTRA { get; set; }

        public Dictionary<string, decimal> DIC_PRICE_TUTRA { get; set; }

        public Dictionary<string, decimal> DIC_PRICE { get; set; }
    }
    public class SERVICE
    {
        public string SERVICE_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT_IN { get; set; }
        public decimal AMOUNT_OUT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public long SERVICE_TYPE_NUM_ORDER { get; set; }
    }
    public class BILL_BALANCE
    {
        public long ID { get; set; }
        public decimal? HIEN_DU { get; set; }
    }

}
