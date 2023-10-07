using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein.Bhyt.HeinTreatmentType;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00119
{
    public class Mrs00119Processor : AbstractProcessor
    {

        Mrs00119Filter filter = null;
        private Mrs00119RDO examBhqd = new Mrs00119RDO();
        private Mrs00119RDO examBhqh = new Mrs00119RDO();
        private Mrs00119RDO examBhtq = new Mrs00119RDO();
        private Mrs00119RDO examBhytA = new Mrs00119RDO(); ///
        private Mrs00119RDO examBhyt = new Mrs00119RDO();
        private Mrs00119RDO examFee = new Mrs00119RDO();
        private List<Mrs00119RDO> examOthers = new List<Mrs00119RDO>();

        private Mrs00119RDO treatBhqd = new Mrs00119RDO();
        private Mrs00119RDO treatBhqh = new Mrs00119RDO();
        private Mrs00119RDO treatBhtq = new Mrs00119RDO();
        private Mrs00119RDO treatBhytA = new Mrs00119RDO(); ///
        private Mrs00119RDO treatBhyt = new Mrs00119RDO();
        private Mrs00119RDO treatFee = new Mrs00119RDO();
        private List<Mrs00119RDO> treatOthers = new List<Mrs00119RDO>();

        private Mrs00119RDO presBhqd = new Mrs00119RDO();
        private Mrs00119RDO presBhqh = new Mrs00119RDO();
        private Mrs00119RDO presBhtq = new Mrs00119RDO();
        private Mrs00119RDO presBhytA = new Mrs00119RDO(); ///
        private Mrs00119RDO presBhyt = new Mrs00119RDO();
        private Mrs00119RDO presFee = new Mrs00119RDO();
        private List<Mrs00119RDO> presOthers = new List<Mrs00119RDO>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> sereServs = new List<HIS_SERE_SERV>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
        Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();

        public Mrs00119Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00119Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                filter = (Mrs00119Filter)this.reportFilter;
                //Lay danh sach treatment tuong ung theo thoi gian vao vien
                var treatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IN_TIME_FROM = filter.TIME_FROM,
                    IN_TIME_TO = filter.TIME_TO
                };
                listTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                Inventec.Common.Logging.LogSystem.Info("listTreatment" + listTreatment.Count);
                //Lấy danh sách bệnh nhân theo khoa
                var listTreatmentIds = listTreatment.Select(s => s.ID).ToList();
                listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
                //if (filter.DEPARTMENT_ID != null)
                {
                    var skip = 0;
                    while (listTreatmentIds.Count - skip > 0)
                    {
                        var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var departmentTranFilter = new HisDepartmentTranFilterQuery
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var departmentTranViews = new HisDepartmentTranManager(paramGet).Get(departmentTranFilter);
                        listDepartmentTrans.AddRange(departmentTranViews);
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("listDepartmentTrans" + listDepartmentTrans.Count);
                var treatmentIds = listDepartmentTrans.Select(s => s.TREATMENT_ID).Distinct().ToList();

                //Lay danh sach V_HIS_PATIENT_TYPE_ALTER tuong ung

                PatientTypeAlters = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds);
                Inventec.Common.Logging.LogSystem.Info("PatientTypeAlters" + PatientTypeAlters.Count);
                //Lay danh sach HIS_SERE_SERV tuong ung
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var sereServFilter = new HisSereServFilterQuery
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var sereServSub = new HisSereServManager(paramGet).Get(sereServFilter);
                        sereServs.AddRange(sereServSub);
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("sereServs" + sereServs.Count);
                //Lay danh sach patient tuong ung treatment
                var listPatientId = listTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                List<HIS_PATIENT> listPatient = GetPatient(listPatientId);
                dicPatient = listPatient.ToDictionary(o => o.ID);
                Inventec.Common.Logging.LogSystem.Info("listPatient" + listPatient.Count);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<HIS_PATIENT> GetPatient(List<long> listPatientId)
        {
            List<HIS_PATIENT> result = new List<HIS_PATIENT>();
            try
            {
                CommonParam paramGet = new CommonParam();
                if (IsNotNullOrEmpty(listPatientId))
                {
                    var skip = 0;
                    while (listPatientId.Count - skip > 0)
                    {
                        var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery ss2Filter = new HisPatientFilterQuery();
                        ss2Filter.IDs = listIDs;
                        var listPatientSub = new HisPatientManager(paramGet).Get(ss2Filter);
                        result.AddRange(listPatientSub);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<HIS_PATIENT>();
            }
            return result;

        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                this.InitOthers(ref examOthers);
                this.InitOthers(ref treatOthers);
                this.InitOthers(ref presOthers);
                if (IsNotNullOrEmpty(PatientTypeAlters))
                {
                    foreach (V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter in PatientTypeAlters)
                    {
                        //nhom chuyen khoa theo tung hsdt
                        ///(Chua xac dinh chon khoa)this.dicDepartmentTran = listDepartmentTrans.Where(q => q.DEPARTMENT_IN_TIME != null).GroupBy(p => p.TREATMENT_ID).ToDictionary(o => o.Key, o => o.First());
                        V_HIS_TREATMENT treatment = listTreatment.Where(o => o.ID == PatientTypeAlter.TREATMENT_ID)
                            .FirstOrDefault();
                        HIS_PATIENT patient = dicPatient.ContainsKey(PatientTypeAlter.TDL_PATIENT_ID) ? dicPatient[PatientTypeAlter.TDL_PATIENT_ID] : new HIS_PATIENT();
                        ///(Chua xac dinh chon khoa)long InDepartmentTime = dicDepartmentTran.ContainsKey(PatientTypeAlter.TREATMENT_ID) ? dicDepartmentTran[PatientTypeAlter.TREATMENT_ID].DEPARTMENT_IN_TIME ?? 99999999999999 : 99999999999999;
                        ///(Chua xac dinh chon khoa)long InDepartmentId = dicDepartmentTran.ContainsKey(PatientTypeAlter.TREATMENT_ID) && PatientTypeAlter.DEPARTMENT_TRAN_ID == dicDepartmentTran[PatientTypeAlter.TREATMENT_ID] .ID? dicDepartmentTran[PatientTypeAlter.TREATMENT_ID].DEPARTMENT_ID : 0;

                        string HeinCardNumber = PatientTypeAlter != null
                            ? PatientTypeAlter.HEIN_CARD_NUMBER : "";
                        List<HIS_SERE_SERV> treatmentSereServs = sereServs != null ? sereServs.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList() : null;
                        var listDepartmentTranSub = listDepartmentTrans.Where(o => o.TREATMENT_ID == PatientTypeAlter.TREATMENT_ID && o.DEPARTMENT_ID == this.filter.DEPARTMENT_ID).ToList();
                       
                        //Dem du lieu ngoai tru
                        if (this.IsExam(PatientTypeAlter))
                        ///(Chua xac dinh chon khoa)if ((InDepartmentId == ((Mrs00119Filter)this.reportFilter).DEPARTMENT_ID) || (PatientTypeAlter.LOG_TIME < InDepartmentTime && PatientTypeAlters.Where(o => o.TREATMENT_ID == PatientTypeAlter.TREATMENT_ID && o.LOG_TIME > PatientTypeAlter.LOG_TIME && o.LOG_TIME <= InDepartmentTime).ToList().Count == 0))// Nếu lúc chuyển đối tượng điều trị nằm trong khoa hoặc lúc chuyển trước khi vào khoa nhưng tới khi vào khoa không chuyển lần nào.
                            {
                            //Trường hợp lọc theo khoa, nếu các log chuyển khoa của HSDT đó có diện điều trị là khám thì lấy.
                                
                                if (this.filter.DEPARTMENT_ID==null||listDepartmentTranSub.Exists(o => TreatmentTypeId(o, PatientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM))
                                {
                                    this.CountData(treatment, patient, PatientTypeAlter, HeinCardNumber, ref examBhqd, ref examBhtq, ref examBhqh, ref examFee, ref examBhytA, ref examBhyt, ref examOthers); 
                                }
                            }
                        //Dem du lieu noi tru
                        if (this.IsTreat(PatientTypeAlter))
                        {
                            if (this.filter.DEPARTMENT_ID == null || listDepartmentTranSub.Exists(o => TreatmentTypeId(o, PatientTypeAlters) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU))
                            {
                                this.CountData(treatment, patient, PatientTypeAlter, HeinCardNumber, ref treatBhqd, ref treatBhtq, ref treatBhqh, ref treatFee, ref treatBhytA, ref treatBhyt, ref treatOthers);
                            }///
                        }
                        ///
                        //Dem du lieu cap phat thuoc
                        if (treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU 
                            && treatment.TDL_TREATMENT_TYPE_ID == PatientTypeAlter.TREATMENT_TYPE_ID
                            && this.IsHasMedicines(treatmentSereServs)&&listDepartmentTranSub.Exists(o => TreatmentTypeId(o, PatientTypeAlters) >0))
                        {
                            this.CountData(treatment, patient, PatientTypeAlter, HeinCardNumber, ref presBhqd, ref presBhtq, ref presBhqh, ref presFee, ref presBhytA, ref presBhyt, ref presOthers); ///
                        }
                    }

                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;

            }
            return result;
        }

        //Dien dieu tri
        private long TreatmentTypeId(HIS_DEPARTMENT_TRAN departmentTran, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            long result = 0;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).Last().TREATMENT_TYPE_ID;
                }
                else
                {
                    patientTypeAlterSub = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                        && o.LOG_TIME > departmentTran.DEPARTMENT_IN_TIME
                        && o.LOG_TIME < (NextDepartment(departmentTran).DEPARTMENT_IN_TIME ?? 99999999999999)).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlterSub))
                    {
                        result = patientTypeAlterSub.OrderBy(o => o.LOG_TIME).First().TREATMENT_TYPE_ID;
                    }
                }
                if (result != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                {
                    result = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                }
            }
            catch (Exception ex)
            {
                return 0;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return listDepartmentTrans.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }
        //Dem du lieu
        private void CountData(V_HIS_TREATMENT treatment,
            HIS_PATIENT patient, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, string HeinCardNumber,
            ref Mrs00119RDO bhqdCount, ref Mrs00119RDO bhtqCount, ref Mrs00119RDO bhqhCount,
            ref Mrs00119RDO feeCount, ref Mrs00119RDO bhytACount, ref Mrs00119RDO bhytCount,
            ref List<Mrs00119RDO> othersCount)///
        {
            if (treatment != null && PatientTypeAlter != null)
            {
                if (this.IsBhQd(HeinCardNumber))
                {
                    bhqdCount.Amount++;
                    bhqdCount.treatmentcodes+=treatment.TREATMENT_CODE + ",";
                }
                else if (this.IsBhTq(HeinCardNumber))
                {
                    bhtqCount.Amount++;
                    bhtqCount.treatmentcodes += treatment.TREATMENT_CODE + ",";
                }
                else if (this.IsBhQh(treatment, patient, HeinCardNumber))
                {
                    bhqhCount.Amount++;
                    bhqhCount.treatmentcodes += treatment.TREATMENT_CODE + ",";
                }
                else if (this.IsBhytA(HeinCardNumber))///
                {
                    bhytACount.Amount++; ///
                    bhytACount.treatmentcodes += treatment.TREATMENT_CODE + ",";
                }
                else if (HeinCardNumber != null)
                {
                    bhytCount.Amount++;
                    bhytCount.treatmentcodes += treatment.TREATMENT_CODE + ",";
                }
                else if (PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    feeCount.Amount++;
                    feeCount.treatmentcodes += treatment.TREATMENT_CODE + ",";
                }
                else
                {
                    Mrs00119RDO rdo = othersCount.Where(o => o.PatientTypeId == PatientTypeAlter.PATIENT_TYPE_ID).FirstOrDefault();
                    if (rdo != null)
                    {
                        rdo.Amount++;
                        rdo.treatmentcodes += treatment.TREATMENT_CODE + ",";
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            try
            {

                var filter = (Mrs00119Filter)this.reportFilter;

                if (filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("IN_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                }
                if (filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("IN_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
                }

                dicSingleTag.Add("EXAM_BH_QH_AMOUNT", examBhqh.Amount);
                dicSingleTag.Add("EXAM_BH_TQ_AMOUNT", examBhtq.Amount);
                dicSingleTag.Add("EXAM_BH_QD_AMOUNT", examBhqd.Amount);
                dicSingleTag.Add("EXAM_BH_OTHER_BHYTA_AMOUNT", examBhytA.Amount); ///
                dicSingleTag.Add("EXAM_BH_OTHER_BHYT_AMOUNT", examBhyt.Amount);
                dicSingleTag.Add("EXAM_FEE_AMOUNT", examFee.Amount);

                dicSingleTag.Add("TREAT_BH_QH_AMOUNT", treatBhqh.Amount);
                dicSingleTag.Add("TREAT_BH_TQ_AMOUNT", treatBhtq.Amount);
                dicSingleTag.Add("TREAT_BH_QD_AMOUNT", treatBhqd.Amount);
                dicSingleTag.Add("TREAT_BH_OTHER_BHYTA_AMOUNT", treatBhytA.Amount); ///
                dicSingleTag.Add("TREAT_BH_OTHER_BHYT_AMOUNT", treatBhyt.Amount);
                dicSingleTag.Add("TREAT_FEE_AMOUNT", treatFee.Amount);

                dicSingleTag.Add("PRES_BH_QH_AMOUNT", presBhqh.Amount);
                dicSingleTag.Add("PRES_BH_TQ_AMOUNT", presBhtq.Amount);
                dicSingleTag.Add("PRES_BH_QD_AMOUNT", presBhqd.Amount);
                dicSingleTag.Add("PRES_BH_OTHER_BHYTA_AMOUNT", presBhytA.Amount); ///
                dicSingleTag.Add("PRES_BH_OTHER_BHYT_AMOUNT", presBhyt.Amount);
                dicSingleTag.Add("PRES_FEE_AMOUNT", presFee.Amount);

                dicSingleTag.Add("EXAM_BH_QH_AMOUNTs", examBhqh.treatmentcodes);
                dicSingleTag.Add("EXAM_BH_TQ_AMOUNTs", examBhtq.treatmentcodes);
                dicSingleTag.Add("EXAM_BH_QD_AMOUNTs", examBhqd.treatmentcodes);
                dicSingleTag.Add("EXAM_BH_OTHER_BHYTA_AMOUNTs", examBhytA.treatmentcodes); ///
                dicSingleTag.Add("EXAM_BH_OTHER_BHYT_AMOUNTs", examBhyt.treatmentcodes);
                dicSingleTag.Add("EXAM_FEE_AMOUNTs", examFee.treatmentcodes);

                dicSingleTag.Add("TREAT_BH_QH_AMOUNTs", treatBhqh.treatmentcodes);
                dicSingleTag.Add("TREAT_BH_TQ_AMOUNTs", treatBhtq.treatmentcodes);
                dicSingleTag.Add("TREAT_BH_QD_AMOUNTs", treatBhqd.treatmentcodes);
                dicSingleTag.Add("TREAT_BH_OTHER_BHYTA_AMOUNTs", treatBhytA.treatmentcodes); ///
                dicSingleTag.Add("TREAT_BH_OTHER_BHYT_AMOUNTs", treatBhyt.treatmentcodes);
                dicSingleTag.Add("TREAT_FEE_AMOUNTs", treatFee.treatmentcodes);

                dicSingleTag.Add("PRES_BH_QH_AMOUNTs", presBhqh.treatmentcodes);
                dicSingleTag.Add("PRES_BH_TQ_AMOUNTs", presBhtq.treatmentcodes);
                dicSingleTag.Add("PRES_BH_QD_AMOUNTs", presBhqd.treatmentcodes);
                dicSingleTag.Add("PRES_BH_OTHER_BHYTA_AMOUNTs", presBhytA.treatmentcodes); ///
                dicSingleTag.Add("PRES_BH_OTHER_BHYT_AMOUNTs", presBhyt.treatmentcodes);
                dicSingleTag.Add("PRES_FEE_AMOUNTs", presFee.treatmentcodes);

                objectTag.AddObjectData(store, "examOthers", examOthers);
                objectTag.AddObjectData(store, "treatOthers", treatOthers);
                objectTag.AddObjectData(store, "presOthers", presOthers);


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }

        }

        /// <summary>
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhQd(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber);
        }

        /// <summary>
        /// Co phai doi tuong BHYT tq hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber);
        }
        /// <summary>
        /// Co phai doi tuong BHYT A hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhytA(string HeinCardNumber)///
        {
            return this.IsHeinCardNumberLeverWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__AS, HeinCardNumber); ///
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhTe(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TE, HeinCardNumber);
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan hưu hay khong
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhQh(V_HIS_TREATMENT treatment, HIS_PATIENT dicPatient, string HeinCardNumber)
        {
            return treatment != null
                && dicPatient.CAREER_ID == HisCareerCFG.CAREER_ID__RETIRED_MILITARY
                && this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber);
        }

        /// <summary>
        /// Co chi dinh thuoc hay khong
        /// </summary>
        /// <param name="sereServs">Toan bo sere_servs tuong ung voi treatment</param>
        /// <returns></returns>
        private bool IsHasMedicines(List<HIS_SERE_SERV> sereServs)
        {
            return IsNotNullOrEmpty(sereServs) && sereServs.Where(o => o.MEDICINE_ID.HasValue).Any();
        }

        /// <summary>
        /// La noi tru hay ko
        /// </summary>
        /// <param name="patientTypeAlter"></param>
        /// <returns></returns>
        private bool IsExam(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.EXAM);
        }
        private bool IsTreat(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return !IsExam(patientTypeAlter);
            //return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.I_TREAT);
        }

        /// <summary>
        /// Co so BHYT bat dau voi cac ma trong danh sach hay khong
        /// </summary>
        /// <param name="prefixs"></param>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsHeinCardNumberPrefixWith(List<string> prefixs, string HeinCardNumber)
        {
            if (IsNotNullOrEmpty(prefixs) && !string.IsNullOrWhiteSpace(HeinCardNumber))
            {
                foreach (string s in prefixs)
                {
                    if (HeinCardNumber.StartsWith(s))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsHeinCardNumberLeverWith(List<string> prefixs, string HeinCardNumber)
        {
            if (IsNotNullOrEmpty(prefixs) && !string.IsNullOrWhiteSpace(HeinCardNumber))
            {
                foreach (string s in prefixs)
                {
                    if (HeinCardNumber.Substring(2, 1) == s)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        /// Co so BHYT bat dau voi ma da cho hay khong
        /// </summary>
        /// <param name="prefixs"></param>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsHeinCardNumberPrefixWith(string prefix, string HeinCardNumber)
        {
            return !string.IsNullOrWhiteSpace(prefix)
                && !string.IsNullOrWhiteSpace(HeinCardNumber)
                && HeinCardNumber.StartsWith(prefix);
        }

        /// <summary>
        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        /// </summary>
        /// <param name="others"></param>
        private void InitOthers(ref List<Mrs00119RDO> others)
        {
            others = HisPatientTypeCFG.PATIENT_TYPEs
                .Where(o => o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                .Select(t => new Mrs00119RDO
                {
                    Amount = 0,
                    PatientTypeId = t.ID,
                    PatientTypeName = t.PATIENT_TYPE_NAME
                }).ToList();
        }

        private Mrs00119Filter ProcessCastFilterQuery(Mrs00119Filter mrs00119Filter)
        {
            Mrs00119Filter filter = null;
            try
            {
                if (mrs00119Filter == null) throw new ArgumentNullException("Input param mrs00119Filter is null");

                Mapper.CreateMap<Mrs00119Filter, Mrs00119Filter>();
                filter = Mapper.Map<Mrs00119Filter, Mrs00119Filter>(mrs00119Filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return filter;
        }
    }

    class CustomerFuncRownumberServiceType : TFlexCelUserFunction
    {
        int numOrder = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                if (serviceTypeId > 0)
                {
                    numOrder++;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return numOrder;
        }
    }

    class CustomerFuncRownumberService : TFlexCelUserFunction
    {
        long ServiceTypeId = 0;
        int Num_Order = 0;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                long serviceId = Convert.ToInt64(parameters[1]);
                if (serviceTypeId == ServiceTypeId)
                {
                    Num_Order = Num_Order + 1;
                }
                else
                {
                    ServiceTypeId = serviceTypeId;
                    Num_Order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Num_Order;
        }
    }

    class CustomerFuncRownumberTreatment : TFlexCelUserFunction
    {
        long ServiceTypeId = 0;
        long ServiceId = 0;
        int Num_Order = 0;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                long serviceTypeId = Convert.ToInt64(parameters[0]);
                long serviceId = Convert.ToInt64(parameters[1]);
                long treatmentId = Convert.ToInt64(parameters[2]);
                if ((serviceTypeId == ServiceTypeId) && (serviceId == ServiceId))
                {
                    Num_Order = Num_Order + 1;
                }
                else
                {
                    ServiceTypeId = serviceTypeId;
                    ServiceId = serviceId;
                    Num_Order = 1;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return Num_Order;
        }
    }



    public class HTreatmentTypeCode
    {
        public const string EXAM = "01";
        public const string O_TREAT = "02";
        public const string I_TREAT = "03";


    }


}
