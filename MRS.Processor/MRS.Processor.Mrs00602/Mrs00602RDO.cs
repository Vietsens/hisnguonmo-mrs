using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00602
{
    class Mrs00602RDOTreatment
    {
        public string DOCTOR_LOGINNAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }
        public string END_USERNAME { set; get; }
        public string DEPARTMENT_NAME { set; get; }
        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long? TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_CODE { get; set; }
        public string HEIN_MEDI_ORG_CODE { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string HEIN_MEDI_ORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
        public string ICD_CODE_EXTRA { get; set; }
        public string IN_TIME_STR { get; set; }
        public string OPEN_TIME { get; set; }
        public string CLOSE_TIME { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string HEIN_TREATMENT_TYPE_CODE { get; set; }

        public decimal TOTAL_PRICE { get; set; }
        public decimal TEST_PRICE { get; set; }
        public decimal DIIM_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal BLOOD_PRICE { get; set; }
        public decimal SURGMISU_PRICE { get; set; }
        public decimal MATERIAL_PRICE { get; set; }
        public decimal BED_PRICE { get; set; }
        public decimal TRAN_PRICE { get; set; }
        public decimal EXAM_PRICE { get; set; }
        public decimal OTHER_PRICE { get; set; }
        public decimal? EXEMPTION { get; set; }
        public decimal TOTAL_FEE { get; set; }
        public string VIR_ADDRESS { get; set; }
        public string MEDI_ORG_FROM_CODE { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_PATIENT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_PARENT_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_AMOUNT { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string LAST_DEPARTMENT_NAME { get; set; }
        public string LAST_DEPARTMENT_CODE { get; set; }
        public string END_ROOM_DEPARTMENT_NAME { get; set; }

        public string PARENT_NAME { get; set; }
        public string PTTT_GROUP_NAME { get; set; }
        public bool TREATMENT_TYPE_EXAM { get; set; }
        public string FEE_LOCK_LOGINNAME { get; set; }
        public string FEE_LOCK_USERNAME { get; set; }
        public string LAST_CASHIER_LOGINNAME { get; set; }
        public string LAST_CASHIER_USERNAME { get; set; }
        public long DOB { get; set; }
        public string GENDER_CODE { get; set; }
        public long HEIN_CARD_FROM_TIME_STR { get; set; }
        public long HEIN_CARD_TO_TIME_STR { get; set; }
        public string REASON_INPUT_CODE { get; set; }
        public object OPEN_TIME_SEPARATE_STR { get; set; }
        public string CLOSE_TIME_SEPARATE_STR { get; set; }
        public long? TOTAL_DAY { get; set; }
        public string TREATMENT_RESULT_CODE { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TOTAL_HEIN_PRICE { get; set; }
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public long INSURANCE_YEAR { get; set; }
        public long INSURANCE_MONTH { get; set; }
        public string HEIN_LIVE_AREA_CODE { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string CURRENT_MEDI_ORG_CODE { get; set; }
        public long PLACE_PAYMENT_CODE { get; set; }
        public long INSURANCE_STT { get; set; }
        public decimal REASON_OUT_PRICE { get; set; }
        public string REASON_OUT { get; set; }
        public decimal POLYLINE_PRICE { get; set; }
        public decimal EXCESS_PRICE { get; set; }
        public string ROUTE_CODE { get; set; }
        public string REQUEST_LOGINNAME { set; get; }
        public string REQUEST_USERNAME { set; get; }
        public string TRANSACTION_CODE { get; set; }
        public Mrs00602RDOTreatment() { }

        public Mrs00602RDOTreatment(HIS_TREATMENT r, List<HIS_SERE_SERV> ListHisSereServ, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat, List<HIS_TRANSACTION> ListTransaction, List<HIS_SERE_SERV_BILL> ListHisSereServBill, Mrs00602Filter filter, List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter, Dictionary<long, string> dicParentCode)
        {
            if (filter.THROW_TREATMENT_TYPE_EXAM == true && r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                this.TREATMENT_TYPE_EXAM = true;
            }
            else
            {
                this.TREATMENT_TYPE_EXAM = false;
            }
            var sereserv = ListHisSereServ.Where(x => x.TDL_TREATMENT_ID == r.ID).ToList();
            REQUEST_LOGINNAME = string.Join(";",sereserv.Select(o=>o.TDL_REQUEST_LOGINNAME).Distinct().ToList());
            REQUEST_USERNAME = string.Join(";", sereserv.Select(o => o.TDL_REQUEST_USERNAME).Distinct().ToList());
            this.HEIN_CARD_NUMBER = r.TDL_HEIN_CARD_NUMBER;
            this.TREATMENT_CODE = r.TREATMENT_CODE;
            this.PATIENT_CODE = r.TDL_PATIENT_CODE;
            this.PATIENT_NAME = r.TDL_PATIENT_NAME.TrimEnd(' ');
            this.MEDIORG_NAME = r.TDL_HEIN_MEDI_ORG_NAME;
            this.MEDIORG_CODE = r.TDL_HEIN_MEDI_ORG_CODE;
            this.HEIN_MEDI_ORG_NAME = r.TDL_HEIN_MEDI_ORG_NAME;
            this.HEIN_MEDI_ORG_CODE = r.TDL_HEIN_MEDI_ORG_CODE;
            this.ICD_CODE_MAIN = r.ICD_CODE;
            this.ICD_CODE_EXTRA = r.ICD_TEXT;
            this.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            this.LAST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            this.LAST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.FEE_LOCK_LOGINNAME = r.FEE_LOCK_LOGINNAME;
            this.FEE_LOCK_USERNAME = r.FEE_LOCK_USERNAME;

            if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                this.END_ROOM_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            }
            else
            {
                this.END_ROOM_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.IN_TIME);
            this.EXEMPTION = (ListTransaction.FirstOrDefault(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT) ?? new HIS_TRANSACTION()).EXEMPTION;

            if (r.OUT_TIME.HasValue & r.CLINICAL_IN_TIME.HasValue)
            {
                this.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.CLINICAL_IN_TIME.Value);
                this.CLOSE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.OUT_TIME.Value);
                this.TOTAL_DATE = Calculation.DayOfTreatment(r.CLINICAL_IN_TIME, r.OUT_TIME, r.TREATMENT_END_TYPE_ID, r.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                if (this.TOTAL_DATE == 0)
                {
                    this.TOTAL_DATE = null;
                }
            }
            else if (r.CLINICAL_IN_TIME.HasValue)
            {
                this.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.CLINICAL_IN_TIME.Value);
            }//Ket qua dieu tri: 1: Khỏi;  2: Đỡ;  3: Không thay đổi;  4: Nặng hơn;  5: Tử vong
            if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
            {
                this.TREATMENT_RESULT_CODE = "1";
            }
            else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
            {
                this.TREATMENT_RESULT_CODE = "2";
            }
            else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
            {
                this.TREATMENT_RESULT_CODE = "3";
            }
            else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
            {
                this.TREATMENT_RESULT_CODE = "4";
            }
            else if (r.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
            {
                this.TREATMENT_RESULT_CODE = "5";
            }
            //Tinh trang ra vien: 1: Ra viện;  2: Chuyển viện;  3: Trốn viện;  4: Xin ra viện
            if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN)
            {
                this.TREATMENT_END_TYPE_CODE = "1";
            }
            else if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
            {
                this.TREATMENT_END_TYPE_CODE = "2";
            }
            else if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
            {
                this.TREATMENT_END_TYPE_CODE = "3";
            }
            else if (r.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
            {
                this.TREATMENT_END_TYPE_CODE = "4";
            }

            this.SetExtendField(r, ListHisSereServ, ListHisServiceRetyCat, ListTransaction, ListHisSereServBill, filter, ListHisPatientTypeAlter, dicParentCode);
        }

        public void SetExtendField(HIS_TREATMENT r, List<HIS_SERE_SERV> ListHisSereServ, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat, List<HIS_TRANSACTION> ListTransaction, List<HIS_SERE_SERV_BILL> ListHisSereServBill, Mrs00602Filter filter, List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter, Dictionary<long, string> DicParentCode)
        {
            try
            {
                var bill = ListTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                if (bill.Count > 0)
                {
                    this.LAST_CASHIER_LOGINNAME = bill.OrderBy(o => o.TRANSACTION_TIME).Select(o => o.CASHIER_LOGINNAME).LastOrDefault();
                    this.LAST_CASHIER_USERNAME = bill.OrderBy(o => o.TRANSACTION_TIME).Select(o => o.CASHIER_USERNAME).LastOrDefault();
                    this.TRANSACTION_CODE = bill.First().TRANSACTION_CODE;
                    this.TRANSACTION_TIME = bill.First().TRANSACTION_TIME;
                    this.EINVOICE_NUM_ORDER = bill.First().EINVOICE_NUM_ORDER;
                }

                if (r.TDL_PATIENT_DOB > 0)
                {
                    if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        this.DOB_MALE = long.Parse((r.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    else if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        this.DOB_FEMALE = long.Parse((r.TDL_PATIENT_DOB.ToString()).Substring(0, 4));
                    }
                    this.DOB = Inventec.Common.TypeConvert.Parse.ToInt64(r.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                }
                if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                {
                    this.GENDER_CODE = "1";
                }
                else if (r.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    this.GENDER_CODE = "2";
                }
                this.FEE_LOCK_TIME = r.FEE_LOCK_TIME;
                DOCTOR_LOGINNAME = r.DOCTOR_LOGINNAME;
                DOCTOR_USERNAME = r.DOCTOR_USERNAME;
                END_USERNAME = r.END_USERNAME;
                END_LOGINNAME = r.END_LOGINNAME;
                var depament = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(x => x.ID == r.IN_DEPARTMENT_ID);
                if (depament != null)
                {
                    DEPARTMENT_NAME = depament.DEPARTMENT_NAME;
                    DEPARTMENT_CODE = depament.DEPARTMENT_CODE;
                }
                this.VIR_ADDRESS = r.TDL_PATIENT_ADDRESS;
                var patientTypeAlter = ListHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == r.ID && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                if (patientTypeAlter != null)
                {
                    this.HEIN_CARD_FROM_TIME_STR = Inventec.Common.TypeConvert.Parse.ToInt64((patientTypeAlter.HEIN_CARD_FROM_TIME ?? 0).ToString().Substring(0, 8));
                    this.HEIN_CARD_TO_TIME_STR = Inventec.Common.TypeConvert.Parse.ToInt64((patientTypeAlter.HEIN_CARD_TO_TIME ?? 0).ToString().Substring(0, 8));

                    if (patientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {
                        if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                        {
                            this.REASON_INPUT_CODE = "2";
                        }
                        else
                        {
                            this.REASON_INPUT_CODE = "1";
                        }
                    }
                    else if (patientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    {
                        this.REASON_INPUT_CODE = "3";
                    }
                    this.HEIN_LIVE_AREA_CODE = patientTypeAlter.LIVE_AREA_CODE;
                }
                this.MEDI_ORG_FROM_CODE = r.MEDI_ORG_CODE;
                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU || r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    this.OPEN_TIME_SEPARATE_STR = r.IN_TIME.ToString().Substring(0, 12);
                    this.CLOSE_TIME_SEPARATE_STR = r.OUT_TIME.HasValue ? r.OUT_TIME.Value.ToString().Substring(0, 12) : "";
                    if (r.OUT_TIME.HasValue)
                    {
                        if (r.TREATMENT_DAY_COUNT.HasValue)
                        {
                            this.TOTAL_DAY = Convert.ToInt64(r.TREATMENT_DAY_COUNT.Value);
                        }
                        else
                        {
                            this.TOTAL_DAY = Calculation.DayOfTreatment(r.CLINICAL_IN_TIME.HasValue ? r.CLINICAL_IN_TIME : r.IN_TIME, r.OUT_TIME, r.TREATMENT_END_TYPE_ID, r.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                        }
                    }
                }
                else
                {
                    if (r.OUT_TIME.HasValue && r.CLINICAL_IN_TIME.HasValue)
                    {
                        this.OPEN_TIME_SEPARATE_STR = r.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                        this.CLOSE_TIME_SEPARATE_STR = r.OUT_TIME.Value.ToString().Substring(0, 12);
                        if (r.TREATMENT_DAY_COUNT.HasValue)
                        {
                            this.TOTAL_DAY = Convert.ToInt64(r.TREATMENT_DAY_COUNT.Value);
                        }
                        else
                        {
                            this.TOTAL_DAY = Calculation.DayOfTreatment(r.CLINICAL_IN_TIME.HasValue ? r.CLINICAL_IN_TIME : r.IN_TIME, r.OUT_TIME, r.TREATMENT_END_TYPE_ID, r.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                        }
                        if (this.TOTAL_DAY == 0)
                        {
                            this.TOTAL_DAY = null;
                        }
                    }
                    else if (r.CLINICAL_IN_TIME.HasValue)
                    {
                        this.OPEN_TIME_SEPARATE_STR = r.CLINICAL_IN_TIME.Value.ToString().Substring(0, 12);
                    }
                    else
                    {
                        this.OPEN_TIME_SEPARATE_STR = r.IN_TIME.ToString().Substring(0, 12);
                    }
                }

                //DataRDO.TOTAL_DAY = 0; 
                if (r.END_DEPARTMENT_ID.HasValue)
                {
                    var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.END_DEPARTMENT_ID);
                    if (departmentCodeBHYT != null)
                    {
                        this.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                    }
                }

                if (r.FEE_LOCK_TIME.HasValue)
                {
                    this.INSURANCE_YEAR = Convert.ToInt64(r.FEE_LOCK_TIME.ToString().Substring(0, 4));
                    this.INSURANCE_MONTH = Convert.ToInt64(r.FEE_LOCK_TIME.ToString().Substring(4, 2));
                }

                if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    this.TREATMENT_TYPE_CODE = "1";
                }
                else if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                {
                    this.TREATMENT_TYPE_CODE = "2";
                }
                else
                {
                    this.TREATMENT_TYPE_CODE = "3";
                }

                var branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == r.BRANCH_ID);
                if (branch != null)
                {
                    this.CURRENT_MEDI_ORG_CODE = branch.HEIN_MEDI_ORG_CODE;
                }

                //Noi thanh toan: 1: thanh toan tai co so;  2: thanh toan truc tiep
                this.PLACE_PAYMENT_CODE = 1;
                //Giam dinh: 0: không thẩm định;  1: chấp nhận;  2: điều chỉnh;  3: xuất toán
                this.INSURANCE_STT = 0;
                this.REASON_OUT_PRICE = 0;
                this.REASON_OUT = "";
                this.POLYLINE_PRICE = 0;
                this.EXCESS_PRICE = 0;
                this.ROUTE_CODE = "";
                this.DIC_GROUP = new Dictionary<string, decimal>();
                this.DIC_PARENT_PRICE = new Dictionary<string, decimal>();
                this.DIC_PARENT_PATIENT_PRICE = new Dictionary<string, decimal>();
                this.DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
                {
                    this.TREAT_PATIENT_TYPE_ID = r.TDL_PATIENT_TYPE_ID ?? 0;
                    this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == r.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    this.TREATMENT_TYPE_ID = r.TDL_TREATMENT_TYPE_ID ?? 0;
                }
                var sereServSub = ListHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
                if (sereServSub != null)
                {


                    if (filter.THROW_OUTTREAT_SERVICE == true && r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        sereServSub = sereServSub.Where(o => ListHisSereServBill.Exists(p => p.SERE_SERV_ID == o.ID && bill.Exists(q => q.ID == p.BILL_ID && q.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && q.TRANSACTION_TIME > r.CLINICAL_IN_TIME))).ToList();
                    }

                    #region CHI PHÍ THEO LOẠI DỊCH VỤ + THUỐC VÀ VẬT TƯ Y TẾ NGOÀI DANH MỤC
                    this.EXAM_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.BED_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TEST_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.DIIM_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.CDHA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TDCN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.SURGMISU_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MEDICINE_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MATERIAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.BLOOD_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.OTHER_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MEDICNE_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MATERIAL_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

                    this.EXAM_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.BED_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.TEST_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.DIIM_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.CDHA_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.TDCN_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.SURGMISU_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => s.VIR_HEIN_PRICE ?? 0);
                    this.MEDICINE_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.MATERIAL_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.BLOOD_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.OTHER_HEIN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.MEDICNE_HEIN_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.MATERIAL_HEIN_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    #endregion

                    #region CHI PHÍ BỆNH NHÂN TRẢ THEO LOẠI DỊCH VỤ + THUỐC VÀ VẬT TƯ Y TẾ NGOÀI DANH MỤC
                    this.EXAM_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.BED_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.TEST_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.DIIM_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.CDHA_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.TDCN_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.SURGMISU_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.MEDICINE_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.MATERIAL_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.BLOOD_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.OTHER_PATIENT_PRICE =  sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.MEDICNE_PATIENT_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.MATERIAL_PATIENT_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    this.TRAN_PATIENT_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(s => (s.VIR_PATIENT_PRICE ?? 0) - (s.VIR_PATIENT_PRICE_BHYT ?? 0));
                    #endregion

                    #region CHI PHÍ THEO LOẠI DỊCH VỤ BHYT
                    //tổng tiền theo dịch vụ bhyt
                    this.HEIN_EXAM_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_BED_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_TEST_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_CDHA_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_TDCN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_TTPT_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_MEDICINE_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_MATERIAL_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_BLOOD_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_MEDICNE_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_MATERIAL_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.HEIN_TRANSPORT_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

                    //tiền bh
                    this.HEIN_EXAM_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_BED_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT
                          || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_TEST_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);                   
                    this.HEIN_CDHA_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_TDCN_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_TTPT_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_MEDICINE_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_MATERIAL_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL
                        || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.BLOOD_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);                    
                    this.MEDICNE_HEIN_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.MATERIAL_HEIN_PRICE_NDM = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.HEIN_TRANSPORT_HEIN_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    #endregion


                    this.TOTAL_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TOTAL_PATIENT_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                    this.TOTAL_PATIENT_PRICE_SELF = (sereServSub.Sum(p => p.VIR_TOTAL_PRICE) ?? 0) - (sereServSub.Sum(p => p.VIR_TOTAL_HEIN_PRICE) ?? 0) - (sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)) - (sereServSub.Sum(s => (s.OTHER_SOURCE_PRICE ?? 0) * s.AMOUNT));
                    this.TOTAL_HEIN_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                    this.TOTAL_PATIENT_PRICE_BHYT = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.TOTAL_OTHER_SOURCE_PRICE = sereServSub.Sum(s => (s.OTHER_SOURCE_PRICE ?? 0) * s.AMOUNT);
                    this.DIC_PARENT_PATIENT_PRICE = sereServSub.GroupBy(o => ParentCode(o.SERVICE_ID, DicParentCode)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0));
                    this.DIC_PARENT_PRICE = sereServSub.GroupBy(o => ParentCode(o.SERVICE_ID, DicParentCode)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                    if (ListHisServiceRetyCat.Count > 0)
                    {
                        this.DIC_GROUP = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                        this.DIC_GROUP_AMOUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    }
                    this.TOTAL_FEE = this.TOTAL_PRICE - (this.EXEMPTION ?? 0);
                    
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        private string ParentCode(long p, Dictionary<long, string> DicParentCode)
        {
            if (DicParentCode.ContainsKey(p))
            {
                return DicParentCode[p];
            }
            else
            {
                return "NONE";
            }
        }

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }


        public decimal EXAM_HEIN_PRICE { get; set; }

        public decimal BED_HEIN_PRICE { get; set; }

        public decimal TEST_HEIN_PRICE { get; set; }

        public decimal DIIM_HEIN_PRICE { get; set; }

        public decimal SURGMISU_HEIN_PRICE { get; set; }

        public decimal MEDICINE_HEIN_PRICE { get; set; }

        public decimal MATERIAL_HEIN_PRICE { get; set; }

        public decimal BLOOD_HEIN_PRICE { get; set; }

        public decimal OTHER_HEIN_PRICE { get; set; }

        public decimal MEDICNE_PRICE_NDM { get; set; }

        public decimal MATERIAL_PRICE_NDM { get; set; }

        public decimal MATERIAL_HEIN_PRICE_NDM { get; set; }

        public decimal MEDICNE_HEIN_PRICE_NDM { get; set; }

        public long TRANSACTION_TIME { get; set; }

        public long? FEE_LOCK_TIME { get; set; }

        public string EINVOICE_NUM_ORDER { get; set; }

        public decimal CDHA_HEIN_PRICE { get; set; }

        public decimal TDCN_HEIN_PRICE { get; set; }

        public decimal CDHA_PRICE { get; set; }

        public decimal TDCN_PRICE { get; set; }

        public decimal TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal TOTAL_PATIENT_PRICE_SELF { get; set; }

        public decimal TTPT_PRICE { get; set; }

        public decimal HEIN_EXAM_PRICE { get; set; }

        public decimal HEIN_BED_PRICE { get; set; }

        public decimal HEIN_TEST_PRICE { get; set; }

        public decimal HEIN_CDHA_PRICE { get; set; }

        public decimal HEIN_TTPT_PRICE { get; set; }

        public decimal HEIN_MEDICINE_PRICE { get; set; }

        public decimal HEIN_MATERIAL_PRICE { get; set; }

        public decimal HEIN_BLOOD_PRICE { get; set; }

        public decimal HEIN_MEDICNE_PRICE_NDM { get; set; }

        public decimal HEIN_MATERIAL_PRICE_NDM { get; set; }

        public decimal HEIN_EXAM_HEIN_PRICE { get; set; }

        public decimal HEIN_BED_HEIN_PRICE { get; set; }

        public decimal HEIN_TEST_HEIN_PRICE { get; set; }

        public decimal HEIN_CDHA_HEIN_PRICE { get; set; }

        public decimal HEIN_TDCN_HEIN_PRICE { get; set; }

        public decimal HEIN_TTPT_HEIN_PRICE { get; set; }

        public decimal HEIN_MEDICINE_HEIN_PRICE { get; set; }

        public decimal HEIN_MATERIAL_HEIN_PRICE { get; set; }

        public decimal HEIN_TDCN_PRICE { get; set; }

        public decimal HEIN_TRANSPORT_PRICE { get; set; }

        public decimal HEIN_TRANSPORT_HEIN_PRICE { get; set; }

        public decimal OTHER_PATIENT_PRICE { get; set; }

        public decimal BLOOD_PATIENT_PRICE { get; set; }

        public decimal MATERIAL_PATIENT_PRICE { get; set; }

        public decimal MEDICINE_PATIENT_PRICE { get; set; }

        public decimal SURGMISU_PATIENT_PRICE { get; set; }

        public decimal TDCN_PATIENT_PRICE { get; set; }

        public decimal CDHA_PATIENT_PRICE { get; set; }

        public decimal DIIM_PATIENT_PRICE { get; set; }

        public decimal TEST_PATIENT_PRICE { get; set; }

        public decimal BED_PATIENT_PRICE { get; set; }

        public decimal EXAM_PATIENT_PRICE { get; set; }

        public decimal MEDICNE_PATIENT_PRICE_NDM { get; set; }

        public decimal MATERIAL_PATIENT_PRICE_NDM { get; set; }

        public decimal TRAN_PATIENT_PRICE { get; set; }
    }
    public class Mrs00602RDOService
    {
        private const string RouteCodeA = "A";
        private const string RouteCodeB = "B";
        public string SERVICE_STT_DMBYT { get; set; }
        public string SERVICE_CODE_DMBYT { get; set; }
        public string SERVICE_UNIT_NAME { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string BRANCH_NAME { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }
        public decimal BHYT_PAY_RATE { get; set; }
        public decimal? DIFF_PRICE { get; set; }
        public decimal? PRIMARY_PRICE { get; set; }
        public decimal? ORIGINAL_PRICE { get; set; }
        public decimal? VAT_RATIO { get; set; }
        public decimal? PRICE_FEE { get; set; }
        public string CATEGORY_CODE { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_CODE { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }
        public string DOCTOR_LOGINNAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }
        public string END_USERNAME { set; get; }
        public string PARENT_NAME { get; set; }
        public string DEPARTMENT_NAME { set; get; }
        public string PTTT_GROUP_NAME { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public string ROUTE_CODE { get; set; }
        public decimal AMOUNT_NOITRU_ROUTE { get; set; }
        public decimal AMOUNT_NGOAITRU_ROUTE { get; set; }
        public decimal TOTAL_PRICE_ROUTE { get; set; }
        public string REQUEST_LOGINNAME { set; get; }
        public string REQUEST_USERNAME { set; get; }
        public string TRANSACTION_CODE { get; set; }
        public Mrs00602RDOService(HIS_SERE_SERV data, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat
        , List<HIS_SERVICE> ListService,
        List<HIS_PTTT_GROUP> ListPtttGroup,
            long branchId, HIS_TREATMENT treatment,
            List<HIS_PATIENT_TYPE_ALTER> ListHisPatientTypeAlter)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.TDL_SERVICE_CODE = data.TDL_SERVICE_CODE;
                this.TDL_SERVICE_NAME = data.TDL_SERVICE_NAME;
                this.PARENT_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == data.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                var service = ListService.FirstOrDefault(o => o.ID == data.SERVICE_ID) ?? new HIS_SERVICE();
                if (ListPtttGroup != null)
                {
                    this.PTTT_GROUP_NAME = (ListPtttGroup.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                }

                if (data.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                {
                    var parent = ListService.FirstOrDefault(o => o.ID == service.PARENT_ID) ?? new HIS_SERVICE();
                    this.PARENT_NAME = parent.SERVICE_NAME;
                }
                this.TDL_HEIN_SERVICE_TYPE_ID = data.TDL_HEIN_SERVICE_TYPE_ID;
                this.SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == data.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                this.SERVICE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == data.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                this.SERVICE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.SERVICE_TYPE_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME ?? data.TDL_SERVICE_NAME;
                this.PRICE = data.PRICE;
                this.BHYT_PAY_RATE = Math.Round(data.ORIGINAL_PRICE > 0 ? (data.HEIN_LIMIT_PRICE.HasValue ? (data.HEIN_LIMIT_PRICE.Value / (data.ORIGINAL_PRICE * (1 + data.VAT_RATIO))) * 100 : (data.PRICE / data.ORIGINAL_PRICE) * 100) : 0, 0);
                this.ORIGINAL_PRICE = data.ORIGINAL_PRICE;
                this.PRIMARY_PRICE = data.PRIMARY_PRICE;
                this.VAT_RATIO = data.VAT_RATIO;
                DOCTOR_LOGINNAME = treatment.DOCTOR_LOGINNAME;
                DOCTOR_USERNAME = treatment.DOCTOR_USERNAME;
                END_LOGINNAME = treatment.END_LOGINNAME;
                END_USERNAME = treatment.END_USERNAME;
                REQUEST_LOGINNAME = data.TDL_REQUEST_LOGINNAME;
                REQUEST_USERNAME = data.TDL_REQUEST_USERNAME;
                var depament = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(x => x.ID == treatment.IN_DEPARTMENT_ID);
                if (depament != null)
                {
                    DEPARTMENT_NAME = depament.DEPARTMENT_NAME;
                }

                this.TREAT_PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID ?? 0;
                this.TREAT_PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
                this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                var patientTypeAlter = ListHisPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == data.TDL_TREATMENT_ID && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == treatment.BRANCH_ID) ?? new HIS_BRANCH();
                if (patientTypeAlter != null)
                {
                    if ((branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(patientTypeAlter.HEIN_MEDI_ORG_CODE ?? ""))
                    {
                        this.ROUTE_CODE = RouteCodeA;
                    }
                    else
                    {
                        this.ROUTE_CODE = RouteCodeB;
                    }
                }
                else
                {
                    this.ROUTE_CODE = RouteCodeB;
                }
                if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NOITRU = data.AMOUNT;
                    this.AMOUNT_NOITRU_ROUTE = data.AMOUNT;
                }
                else if (treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NGOAITRU = data.AMOUNT;
                    this.AMOUNT_NGOAITRU_ROUTE = data.AMOUNT;
                }

                this.PRICE_FEE = 0;
                if (data.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV && !data.PRIMARY_PATIENT_TYPE_ID.HasValue)
                {
                    List<V_HIS_SERVICE_PATY> patys = new List<V_HIS_SERVICE_PATY>();
                    if (MANAGER.Config.HisServicePatyCFG.DicServicePaty != null && MANAGER.Config.HisServicePatyCFG.DicServicePaty.ContainsKey(data.SERVICE_ID))
                    {
                        patys = MANAGER.Config.HisServicePatyCFG.DicServicePaty[data.SERVICE_ID];
                    }
                    else
                    {
                        patys = MANAGER.Config.HisServicePatyCFG.DATAs;
                    }

                    var currentPaty = MOS.ServicePaty.ServicePatyUtil.GetApplied(patys, branchId, null, data.TDL_REQUEST_ROOM_ID, data.TDL_REQUEST_DEPARTMENT_ID, data.TDL_INTRUCTION_TIME, treatment != null ? treatment.IN_TIME : 0, data.SERVICE_ID, treatment.TDL_PATIENT_TYPE_ID ?? 0, null);
                    if (currentPaty != null)
                    {
                        PRICE_FEE = currentPaty.PRICE * (1 + currentPaty.VAT_RATIO);
                    }
                }
                else
                {
                    if (data.HEIN_LIMIT_PRICE.HasValue && (data.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G || data.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH))
                    {
                        this.PRICE_FEE = data.HEIN_LIMIT_PRICE;
                    }
                    else if (data.LIMIT_PRICE.HasValue)
                    {
                        this.PRICE_FEE = data.LIMIT_PRICE;
                    }
                    else if (data.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                    {
                        this.PRICE_FEE = data.VIR_PRICE;
                    }
                }

                this.DIFF_PRICE = (data.VIR_PRICE ?? 0) - this.PRICE_FEE;

                this.TOTAL_PRICE = data.VIR_TOTAL_PRICE ?? 0;
                this.TOTAL_PRICE_ROUTE = data.VIR_TOTAL_PRICE ?? 0;
                if (ListHisServiceRetyCat.Count > 0)
                {
                    this.CATEGORY_CODE = "";
                    var serviceRetyCat = ListHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == data.SERVICE_ID);
                    if (serviceRetyCat != null)
                    {
                        this.CATEGORY_CODE = serviceRetyCat.CATEGORY_CODE;
                        this.SERVICE_TYPE_CODE = "";
                    }
                }
                else
                {
                    this.CATEGORY_CODE = "";
                   
                }
            }
        }

        public Mrs00602RDOService()
        {
            // TODO: Complete member initialization
        }
    }

    public class Mrs00602RDOMaterial
    {
        public string MATERIAL_STT_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT { get; set; }
        public string MATERIAL_CODE_DMBYT_1 { get; set; }
        public string MATERIAL_TYPE_NAME_BYT { get; set; }
        public string MATERIAL_TYPE_NAME { get; set; }
        public string MATERIAL_QUYCACH_NAME { get; set; }
        public string MATERIAL_UNIT_NAME { get; set; }
        public decimal MATERIAL_PRICE { get; set; } // gia mua vao
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public long? SERVICE_ID { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }
        public string DOCTOR_LOGINNAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }
        public string END_USERNAME { set; get; }
        public string REQUEST_LOGINNAME { set; get; }
        public string REQUEST_USERNAME { set; get; }
        public string DEPARTMENT_NAME { set; get; }
        public string TRANSACTION_CODE { get; set; }
        public decimal VAT_RATIO { get; set; }
        public Mrs00602RDOMaterial(HIS_SERE_SERV data, HIS_TREATMENT trea, List<V_HIS_MATERIAL_TYPE> ListHisMaterialType)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.MATERIAL_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.MATERIAL_CODE_DMBYT_1 = data.TDL_MATERIAL_GROUP_BHYT;
                this.MATERIAL_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.MATERIAL_TYPE_NAME_BYT = data.TDL_HEIN_SERVICE_BHYT_NAME;
                this.MATERIAL_TYPE_NAME = data.TDL_SERVICE_NAME;
                this.MATERIAL_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == data.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                this.PRICE = data.VIR_PRICE;
                this.MATERIAL_PRICE = data.ORIGINAL_PRICE;
                this.TOTAL_PRICE = data.VIR_TOTAL_PRICE ?? 0;
                this.TREAT_PATIENT_TYPE_ID = trea.TDL_PATIENT_TYPE_ID ?? 0;
                END_USERNAME = trea.END_USERNAME;
                END_LOGINNAME = trea.END_LOGINNAME;
                DOCTOR_LOGINNAME = trea.DOCTOR_LOGINNAME;
                DOCTOR_USERNAME = trea.DOCTOR_USERNAME;
                REQUEST_LOGINNAME = data.TDL_REQUEST_LOGINNAME;
                REQUEST_USERNAME = data.TDL_REQUEST_USERNAME;
                this.VAT_RATIO = data.VAT_RATIO;
                var depament = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(x => x.ID == trea.IN_DEPARTMENT_ID);
                if (depament != null)
                {
                    DEPARTMENT_NAME = depament.DEPARTMENT_NAME;
                }
                this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == trea.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                if (trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NOITRU = data.AMOUNT;
                }
                else if (trea.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NGOAITRU = data.AMOUNT;
                }
                var materialType = ListHisMaterialType.FirstOrDefault(o => o.SERVICE_ID == data.SERVICE_ID);
                if (materialType != null)
                {
                    this.MATERIAL_QUYCACH_NAME = materialType.PACKING_TYPE_NAME;
                }
            }
        }

        public Mrs00602RDOMaterial()
        {
            // TODO: Complete member initialization
        }
    }

    public class Mrs00602RDOMedicine
    {
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_CODE_DMBYT { get; set; }
        public string MEDICINE_STT_DMBYT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }
        public string DEPARTMENT_NAME { set; get; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }
        public string DOCTOR_LOGINNAME { set; get; }
        public string DOCTOR_USERNAME { set; get; }
        public string END_LOGINNAME { set; get; }
        public string END_USERNAME { set; get; }
        public long? SERVICE_ID { get; set; }
        public string REQUEST_LOGINNAME { set; get; }
        public string REQUEST_USERNAME { set; get; }
        public string TRANSACTION_CODE { get; set; }
        public decimal VAT_RATIO { get; set; }
        public Mrs00602RDOMedicine(HIS_SERE_SERV data, HIS_TREATMENT trea, List<V_HIS_MEDICINE_TYPE> ListHisMedicineType)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.MEDICINE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.MEDICINE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.MEDICINE_TYPE_NAME = data.TDL_SERVICE_NAME;
                this.MEDICINE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == data.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                this.PRICE = data.VIR_PRICE;
                REQUEST_USERNAME = data.TDL_REQUEST_USERNAME;
                REQUEST_LOGINNAME = data.TDL_REQUEST_LOGINNAME;
                if (trea != null)
                {
                    DOCTOR_USERNAME = trea.DOCTOR_USERNAME;
                    DOCTOR_LOGINNAME = trea.DOCTOR_LOGINNAME;
                    END_LOGINNAME = trea.END_LOGINNAME;
                    END_USERNAME = trea.DOCTOR_USERNAME;
                }
                var depament = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(x => x.ID == trea.IN_DEPARTMENT_ID);
                if (depament != null)
                {
                    DEPARTMENT_NAME = depament.DEPARTMENT_NAME;
                }
                this.TOTAL_PRICE = data.VIR_TOTAL_PRICE ?? 0;
                this.BID_NUM_ORDER = data.TDL_MEDICINE_BID_NUM_ORDER;
                this.TREAT_PATIENT_TYPE_ID = trea.TDL_PATIENT_TYPE_ID ?? 0;
                this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == trea.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                if (trea.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NOITRU = data.AMOUNT;
                }
                else if (trea.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                {
                    this.AMOUNT_NGOAITRU = data.AMOUNT;
                }
                var medicineType = ListHisMedicineType.FirstOrDefault(o => o.SERVICE_ID == data.SERVICE_ID);
                if (medicineType != null)
                {
                    this.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                    this.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                    this.MEDICINE_HAMLUONG_NAME = medicineType.CONCENTRA;
                    this.MEDICINE_SODANGKY_NAME = medicineType.REGISTER_NUMBER;
                }
            }
        }

        public Mrs00602RDOMedicine()
        {
            // TODO: Complete member initialization
        }
    }
}
