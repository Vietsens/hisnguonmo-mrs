using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;

namespace MRS.Processor.Mrs00132
{
    public class Mrs00132Processor : AbstractProcessor
    {
        Mrs00132Filter castFilter = null;
        List<Mrs00132RDO> surgData = new List<Mrs00132RDO>();
        List<V_HIS_SERE_SERV_2> sereServs = new List<V_HIS_SERE_SERV_2>();
        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_PTTT> sereServPttts = new List<V_HIS_SERE_SERV_PTTT>();

        public Mrs00132Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00132Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00132Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu HIS_TREATMENT, V_HIS_PATIENT_TYPE_ALTER, V_HIS_PATY_ALTER_BHYT MRS00119 Filter. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisSereServView2FilterQuery ssFilter = new HisSereServView2FilterQuery();
                ssFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                ssFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ssFilter.TDL_SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT;
                sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView2(ssFilter);
                List<long> treatmentIds = IsNotNullOrEmpty(sereServs) ? sereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList() : null;
                List<long> sereServIds = IsNotNullOrEmpty(sereServs) ? sereServs.Select(o => o.ID).ToList() : null;

                //Lay danh sach treatment tuong ung
                if (IsNotNullOrEmpty(sereServIds))
                {
                    var skip = 0;
                    while (sereServIds.Count - skip > 0)
                    {
                        var listIds = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var SereServPtttfilter = new HisSereServPtttViewFilterQuery
                        {
                            SERE_SERV_IDs = listIds,
                        };
                        var SereServPtttSub = new HisSereServPtttManager(paramGet).GetView(SereServPtttfilter);
                        sereServPttts.AddRange(SereServPtttSub);
                    }
                }

                //Lay danh sach treatment tuong ung
                treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetViewByIds(treatmentIds);

                //Lay danh sach V_HIS_PATIENT_TYPE_ALTER tuong ung
                PatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds);

                //Lay danh sach V_HIS_PATY_ALTER_BHYT tuong ung
                List<long> PatientTypeAlterIds = IsNotNullOrEmpty(PatientTypeAlters) ? PatientTypeAlters.Select(o => o.ID).ToList() : null;

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai trong qua trinh lay du lieu V_HIS_SERE_SERV, V_HIS_TREATMENT, V_HIS_PATIENT_TYPE_ALTER, V_HIS_PATY_ALTER_BHYT MRS00132." + Inventec.Common.Logging.LogUtil.TraceData("castFilter", castFilter));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void PrepareData(List<V_HIS_SERE_SERV_2> sereServs, List<V_HIS_TREATMENT> treatments, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, List<V_HIS_SERE_SERV_PTTT> sereServPttts)
        {
            try
            {
                foreach (var sereServ in sereServs)
                {
                    var treatment = treatments.FirstOrDefault(o => o.ID == sereServ.TDL_TREATMENT_ID);
                    V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = PatientTypeAlters.Where(o => o.TREATMENT_ID == treatment.ID)
                        .OrderByDescending(o => o.LOG_TIME)
                        .FirstOrDefault();
                    string HeinCardNumber = PatientTypeAlter != null
                        ? PatientTypeAlter.HEIN_CARD_NUMBER : "";

                    Mrs00132RDO rdo = new Mrs00132RDO();
                    rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                    rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                    rdo.DESCRIPTION_SURGERY = ""; //sereServ.DESCRIPTION; 
                    rdo.NOTE = ""; // sereServ.NOTE; 
                    rdo.TIME_SURG_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServ.EXECUTE_TIME ?? 0); //EXECUTE_TIME thời gian thực hiện phẫu thuật,  intruction_time thời gian chỉ định phẫu thuật
                    rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;

                    this.SetAgeAndYearOfBirth(rdo, treatment);
                    this.SetPatientTypeCheck(rdo, treatment, PatientTypeAlter, HeinCardNumber);

                    if (IsNotNullOrEmpty(sereServPttts))
                    {
                        var data = sereServPttts.FirstOrDefault(f => f.SERE_SERV_ID == sereServ.ID);
                        if (data != null)
                        {
                            rdo.SURG_PPPT = data.PTTT_METHOD_NAME;
                            rdo.SURG_PPVC = data.EMOTIONLESS_METHOD_NAME;
                            rdo.BEFORE_SURG = data.BEFORE_PTTT_ICD_NAME;
                            rdo.AFTER_SURG = data.AFTER_PTTT_ICD_NAME;
                            rdo.SURG_TYPE_NAME = data.PTTT_GROUP_NAME;
                            //rdo.ANESTHESIA_DOCTOR = data.ANESTHETIST_USERNAME; 
                            //rdo.EXECUTE_DOCTOR = data.SURGEON_USERNAME; 
                        }
                    }
                    this.surgData.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                this.surgData.Clear();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(sereServs))
                {
                    this.PrepareData(sereServs, treatments, PatientTypeAlters, sereServPttts);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void SetAgeAndYearOfBirth(Mrs00132RDO rdo, V_HIS_TREATMENT treatment)
        {
            int? age = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
            {
                rdo.MALE_AGE = (age >= 1) ? age : 1;
                rdo.MALE_YEAR = GetYear(treatment.TDL_PATIENT_DOB);
            }
            else
            {
                rdo.FEMALE_AGE = (age >= 1) ? age : 1;
                rdo.FEMALE_YEAR = GetYear(treatment.TDL_PATIENT_DOB);
            }
        }

        private string GetYear(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void SetPatientTypeCheck(Mrs00132RDO rdo, V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, string HeinCardNumber)
        {
            if (this.IsQu(PatientTypeAlter))
            {
                rdo.IS_QUAN = "X";
            }
            else if (this.IsBhQd(HeinCardNumber))
            {
                rdo.IS_BHQD = "X";
            }
            else if (this.IsCs(PatientTypeAlter))
            {
                rdo.IS_CS = "X";
            }
            else if (this.IsBhTq(HeinCardNumber)) //check TQ truoc khi check TE vi trong TQ co truong hop dau the TE197
            {
                rdo.IS_BHTQ = "X";
            }
            else if (this.IsBhQh(treatment, HeinCardNumber))
            {
                rdo.IS_BHQH = "X";
            }
            else if (this.IsBhTe(HeinCardNumber)) //check TQ truoc khi check TE vi trong TQ co truong hop dau the TE197
            {
                rdo.IS_BHTQ = "X";
            }
            else if (HeinCardNumber != null)//check BHYT khac sau cac doi tuong TQ, TE, QH, QD
            {
                rdo.IS_BHYT = "X";
            }
            else if (this.IsDv(PatientTypeAlter))
            {
                rdo.IS_QT = "X";
            }
            else if (this.IsQt(PatientTypeAlter))
            {
                rdo.IS_QT = "X";
            }
            else
            {
                rdo.IS_OTHER = "X";
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
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber);
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
        private bool IsBhQh(V_HIS_TREATMENT treatment, string HeinCardNumber)
        {
            return treatment != null
                //&& treatment.CAREER_ID == HisCareerCFG.CAREER_ID__RETIRED_MILITARY
                && this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber);
        }

        /// <summary>
        /// Doi tuong chinh sach
        /// </summary>
        /// <param name="PatientTypeAlter"></param>
        /// <returns></returns>
        private bool IsCs(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DTCS;
        }

        /// <summary>
        /// Doi tuong dich vu
        /// </summary>
        /// <param name="PatientTypeAlter"></param>
        /// <returns></returns>
        private bool IsDv(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        }

        /// <summary>
        /// Doi tuong dich vu
        /// </summary>
        /// <param name="PatientTypeAlter"></param>
        /// <returns></returns>
        private bool IsQu(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__QU;
        }

        /// <summary>
        /// Doi tuong quoc te
        /// </summary>
        /// <param name="PatientTypeAlter"></param>
        /// <returns></returns>
        private bool IsQt(V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter)
        {
            return PatientTypeAlter != null && PatientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__DTQT;
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

        /// <summary>
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                objectTag.AddObjectData<Mrs00132RDO>(store, "Surg", this.surgData);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
