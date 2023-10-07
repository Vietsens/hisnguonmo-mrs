using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00643
{
    public class Mrs00643RDOTreatment
    {
        public long PATIENT_ID { get; set; }
        public long? DOB_MALE { get; set; }
        public long? DOB_FEMALE { get; set; }
        public long? TOTAL_DATE { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }

        public string TREATMENT_CODE { get; set; }
        public string PATIENT_CODE { get; set; }
        public string PATIENT_NAME { get; set; }
        public string MEDIORG_CODE { get; set; }
        public string MEDIORG_NAME { get; set; }
        public string ICD_CODE_MAIN { get; set; }
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
        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_AMOUNT { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }
        public string END_DEPARTMENT_NAME { get; set; }
        public string END_ROOM_DEPARTMENT_NAME { get; set; }

        public string PARENT_NAME { get; set; }
        public string PTTT_GROUP_NAME { get; set; }

        public Mrs00643RDOTreatment() { }

        public Mrs00643RDOTreatment(HIS_TREATMENT r, List<HIS_SERE_SERV> ListHisSereServ, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat, Mrs00643Filter filter)
        {
           
            this.HEIN_CARD_NUMBER = r.TDL_HEIN_CARD_NUMBER;
            this.TREATMENT_CODE = r.TREATMENT_CODE;
            this.PATIENT_CODE = r.TDL_PATIENT_CODE;
            this.PATIENT_NAME = r.TDL_PATIENT_NAME.TrimEnd(' ');
            this.MEDIORG_NAME = r.TDL_HEIN_MEDI_ORG_NAME;
            this.MEDIORG_CODE = r.TDL_HEIN_MEDI_ORG_CODE;
            this.ICD_CODE_MAIN = r.ICD_CODE;
            this.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            if (r.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
            {
                this.END_ROOM_DEPARTMENT_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == r.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            }
            else
            {
                this.END_ROOM_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            }
            this.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(r.IN_TIME);
          
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
            }

            this.SetExtendField(r, ListHisSereServ, ListHisServiceRetyCat,  filter);
        }

        public void SetExtendField(HIS_TREATMENT r, List<HIS_SERE_SERV> ListHisSereServ, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat, Mrs00643Filter filter)
        {
            try
            {
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
                }
                this.DIC_GROUP = new Dictionary<string, decimal>();
                this.DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
                
                    this.TREAT_PATIENT_TYPE_ID = r.TDL_PATIENT_TYPE_ID??0;
                    this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == r.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    this.TREATMENT_TYPE_ID = r.TDL_TREATMENT_TYPE_ID??0;
                var sereServSub = ListHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
                if (sereServSub != null)
                {

                   this.EXAM_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.BED_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TEST_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.DIIM_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.SURGMISU_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                        || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MEDICINE_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.MATERIAL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.OTHER_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TOTAL_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    if (ListHisServiceRetyCat.Count > 0)
                    {
                        this.DIC_GROUP = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                        this.DIC_GROUP_AMOUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, ListHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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
    }

    public class Mrs00643RDOService
    {
        public string SERVICE_STT_DMBYT { get; set; }
        public string SERVICE_CODE_DMBYT { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string BRANCH_NAME { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal PRICE { get; set; }

        public string CATEGORY_CODE { get; set; }
        public long? SERVICE_ID { get; set; }
        public long SERVICE_TYPE_ID { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }

        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }


        public string PARENT_NAME { get; set; }

        public string PTTT_GROUP_NAME { get; set; }

        public Mrs00643RDOService()
        {
        }

        public Mrs00643RDOService(HIS_SERE_SERV data, List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat, List<HIS_TREATMENT> ListHisTreatment, List<HIS_SERVICE> ListService, List<HIS_PTTT_GROUP> ListPtttGroup)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.PARENT_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == data.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                var service = ListService.FirstOrDefault(o => o.ID == data.SERVICE_ID) ?? new HIS_SERVICE();
                this.PTTT_GROUP_NAME = (ListPtttGroup.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP()).PTTT_GROUP_NAME;
                if (data.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                {
                    var parent = ListService.FirstOrDefault(o => o.ID == service.PARENT_ID) ?? new HIS_SERVICE();
                    this.PARENT_NAME = parent.SERVICE_NAME;
                }  
                this.SERVICE_TYPE_ID = data.TDL_SERVICE_TYPE_ID;
                this.SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == data.TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                this.SERVICE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.SERVICE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.SERVICE_TYPE_NAME = data.TDL_HEIN_SERVICE_BHYT_NAME??data.TDL_SERVICE_NAME;
                this.PRICE =  data.PRICE;
                this.TOTAL_PRICE =data.VIR_TOTAL_PRICE ?? 0;
                var treatment = ListHisTreatment.FirstOrDefault(o => o.ID == data.TDL_TREATMENT_ID);
                if (treatment != null)
                {
                    this.TREAT_PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID??0;
                    this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==treatment.TDL_PATIENT_TYPE_ID)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NOITRU = data.AMOUNT;
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NGOAITRU = data.AMOUNT;
                    }
                }
                if (ListHisServiceRetyCat.Count > 0)
                {
                    this.CATEGORY_CODE = "";
                    var serviceRetyCat = ListHisServiceRetyCat.FirstOrDefault(o=>o.SERVICE_ID==data.SERVICE_ID);
                    if(serviceRetyCat!=null)
                    {
                        this.CATEGORY_CODE = serviceRetyCat.CATEGORY_CODE;
                    }
                }
            }
        }
    }

    public class Mrs00643RDOMaterial
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

        public Mrs00643RDOMaterial(HIS_SERE_SERV data, List<HIS_TREATMENT> ListHisTreatment, List<V_HIS_MATERIAL_TYPE> ListHisMaterialType)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.MATERIAL_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.MATERIAL_CODE_DMBYT_1 = data.TDL_MATERIAL_GROUP_BHYT ;
                this.MATERIAL_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.MATERIAL_TYPE_NAME_BYT = data.TDL_HEIN_SERVICE_BHYT_NAME;
                this.MATERIAL_TYPE_NAME = data.TDL_SERVICE_NAME;
                this.MATERIAL_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o=>o.ID==data.TDL_SERVICE_UNIT_ID)??new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                this.PRICE = data.PRICE;
                this.MATERIAL_PRICE = data.ORIGINAL_PRICE;
                this.TOTAL_PRICE =data.VIR_TOTAL_PRICE ?? 0;
                var treatment = ListHisTreatment.FirstOrDefault(o => o.ID == data.TDL_TREATMENT_ID);
                if (treatment != null)
                {
                    this.TREAT_PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID??0;
                    this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NOITRU = data.AMOUNT;
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NGOAITRU = data.AMOUNT;
                    }
                }
                var materialType = ListHisMaterialType.FirstOrDefault(o => o.SERVICE_ID == data.SERVICE_ID);
                if (materialType!=null)
                {
                    this.MATERIAL_QUYCACH_NAME = materialType.PACKING_TYPE_NAME;
                }
            }
        }

        public Mrs00643RDOMaterial()
        {
            // TODO: Complete member initialization
        }
    }

    public class Mrs00643RDOMedicine
    {
        public string MEDICINE_HOATCHAT_NAME { get; set; }
        public string MEDICINE_CODE_DMBYT { get; set; }
        public string MEDICINE_STT_DMBYT { get; set; }
        public string MEDICINE_TYPE_NAME { get; set; }
        public string MEDICINE_DUONGDUNG_NAME { get; set; }
        public string MEDICINE_HAMLUONG_NAME { get; set; }
        public string MEDICINE_SODANGKY_NAME { get; set; }
        public string MEDICINE_UNIT_NAME { get; set; }
        public string BID_NUM_ORDER { get; set; }
        public string BID_NUMBER { get; set; }
        public decimal AMOUNT_NGOAITRU { get; set; }
        public decimal AMOUNT_NOITRU { get; set; }
        public decimal? PRICE { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public long TREAT_PATIENT_TYPE_ID { get; set; }
        public string TREAT_PATIENT_TYPE_NAME { get; set; }

        public long? SERVICE_ID { get; set; }
        public Mrs00643RDOMedicine(HIS_SERE_SERV data, List<HIS_TREATMENT> ListHisTreatment, List<V_HIS_MEDICINE_TYPE> ListHisMedicineType)
        {
            if (data != null)
            {
                this.SERVICE_ID = data.SERVICE_ID;
                this.MEDICINE_CODE_DMBYT = data.TDL_HEIN_SERVICE_BHYT_CODE;
                this.MEDICINE_STT_DMBYT = data.TDL_HEIN_ORDER;
                this.MEDICINE_TYPE_NAME = data.TDL_SERVICE_NAME;
                this.MEDICINE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == data.TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                this.PRICE = data.PRICE;
                this.TOTAL_PRICE =data.VIR_TOTAL_PRICE ?? 0;
                this.BID_NUM_ORDER = data.TDL_MEDICINE_BID_NUM_ORDER;
                var treatment = ListHisTreatment.FirstOrDefault(o => o.ID == data.TDL_TREATMENT_ID);
                if (treatment != null)
                {
                    this.TREAT_PATIENT_TYPE_ID = treatment.TDL_PATIENT_TYPE_ID??0;
                    this.TREAT_PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NOITRU = data.AMOUNT;
                    }
                    else if (treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        this.AMOUNT_NGOAITRU = data.AMOUNT;
                    }
                }
                var medicineType = ListHisMedicineType.FirstOrDefault(o => o.SERVICE_ID == data.SERVICE_ID);
                if (medicineType!=null)
                {
                    this.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                    this.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                    this.MEDICINE_HAMLUONG_NAME = medicineType.CONCENTRA;
                    this.MEDICINE_SODANGKY_NAME = medicineType.REGISTER_NUMBER;
                }
            }
        }

        public Mrs00643RDOMedicine()
        {
            // TODO: Complete member initialization
        }
    }
}
