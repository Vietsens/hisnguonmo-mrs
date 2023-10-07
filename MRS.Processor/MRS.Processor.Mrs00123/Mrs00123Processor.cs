using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisMilitaryRank;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00123
{
    public class Mrs00123Processor : AbstractProcessor
    {
        Mrs00123Filter castFilter = null;
        private Mrs00123RDO bhquCC = new Mrs00123RDO(); //Doi tuong Quan, cao cap
        private Mrs00123RDO bhquOther = new Mrs00123RDO(); //Doi tuong Quan, khac

        private Mrs00123RDO bhqh = new Mrs00123RDO();
        private Mrs00123RDO bhtq = new Mrs00123RDO();
        private Mrs00123RDO bhqd = new Mrs00123RDO();
        private Mrs00123RDO fee = new Mrs00123RDO();
        private Mrs00123RDO otherBhytA = new Mrs00123RDO();
        private Mrs00123RDO otherBhyt = new Mrs00123RDO();

        private List<Mrs00123RDO> others = new List<Mrs00123RDO>();
        private List<string> departmentNames = new List<string>();
        Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>> dicDepartmentTran = new Dictionary<long, List<V_HIS_DEPARTMENT_TRAN>>();
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();
        List<HIS_DEPARTMENT> departments;
        List<V_HIS_TREATMENT> treatments;
        Dictionary<long, HIS_PATIENT> dicPatient = new Dictionary<long, HIS_PATIENT>();
        List<V_HIS_DEPARTMENT_TRAN> departmentTrans = new List<V_HIS_DEPARTMENT_TRAN>();
        List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters;

        public Mrs00123Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00123Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00123Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00123 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                departments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());

                //Lay danh sach treatment tuong ung theo thoi gian vao vien
                treatments = GetTreatment(castFilter.TIME_FROM, castFilter.TIME_TO);

                var listPatientId = treatments.Select(o => o.PATIENT_ID).Distinct().ToList();
                //Lay danh sach patient tuong ung treatment
                List<HIS_PATIENT> listPatient = GetPatient(listPatientId);
                dicPatient = listPatient.ToDictionary(o => o.ID);
                //Lấy danh sách bệnh nhân theo khoa
                var listTreatmentIds = treatments.Select(s => s.ID).ToList();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var departmentTranViewFilter = new HisDepartmentTranViewFilterQuery
                    {
                        TREATMENT_IDs = listIDs
                    };
                    var departmentTranViews = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(departmentTranViewFilter);
                    departmentTrans.AddRange(departmentTranViews);
                }

                List<long> treatmentIds = treatments.Select(o => o.ID).ToList();

                //Lay danh sach patient_tye_alter tuong ung
                PatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds);

                //Lay danh sach paty_alter_bhyt tuong ung
                List<long> PatientTypeAlterIds = IsNotNullOrEmpty(PatientTypeAlters) ? PatientTypeAlters.Select(o => o.ID).ToList() : null;

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai trong qua trinh lay du lieu V_HIS_DEPARTMENT_TRAN, HIS_DEPARTMENT MRS00123." + Inventec.Common.Logging.LogUtil.TraceData("castFilter", castFilter));
                }
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
                        var listPatientSub = new MOS.MANAGER.HisPatient.HisPatientManager(paramGet).Get(ss2Filter);
                        result.AddRange(listPatientSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_PATIENT>();
            }
            return result;
        }

        private List<V_HIS_TREATMENT> GetTreatment(long TIME_FROM, long TIME_TO)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam();
                var listTreatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IN_TIME_FROM = TIME_FROM,
                    IN_TIME_TO = TIME_TO
                };
                result = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(listTreatmentFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<V_HIS_TREATMENT>();
            }
            return result;
        }

        private void PrepareData(List<HIS_DEPARTMENT> departments, List<V_HIS_TREATMENT> treatments, Dictionary<long, HIS_PATIENT> dicPatient, List<V_HIS_DEPARTMENT_TRAN> departmentTrans, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters)
        {
            if (IsNotNullOrEmpty(departments) && IsNotNullOrEmpty(treatments) && IsNotNullOrEmpty(departmentTrans))
            {
                //Sap xep theo thu tu alphabet va chi lay toi da so luong khoa phong ma bao cao co the dap ung
                List<HIS_DEPARTMENT> exportDepartments = departments.OrderBy(o => o.DEPARTMENT_NAME).Take(Mrs00123RDO.MAX_DEPARTMENT_NUM).ToList();
                this.departmentNames = exportDepartments.Select(o => o.DEPARTMENT_NAME).ToList();
                //luoc bot nhung lan chuyen lai khoa
                departmentTrans = departmentTrans.OrderBy(o => o.DEPARTMENT_IN_TIME).GroupBy(p => new { p.DEPARTMENT_ID, p.TREATMENT_ID }).Select(q => q.First()).ToList();
                //nhom chuyen khoa theo tung hsdt
                this.dicDepartmentTran = departmentTrans.GroupBy(p => p.TREATMENT_ID).ToDictionary(o => o.Key, o => o.ToList());
                //nhom chuyen doi tuong theo tung hsdt
                this.dicPatientTypeAlter = PatientTypeAlters.OrderBy(p => p.LOG_TIME).GroupBy(o => o.TREATMENT_ID).ToDictionary(o => o.Key, o => o.ToList());

                //Khoi tao ban dau cho cac doi tuong khac
                this.InitOthers(ref this.others);

                //Lay danh sach department_trans voi danh sach departments tuong ung va distinct theo department_id va treatment_id 
                //(vi bao cao chi dem so luot ho so dieu tri, chu khong lay theo so luot vao ra khoa.
                //Nghia la: neu 1 ho so dieu tri vao khoa nhieu hon 1 lan thi cung chi duoc tinh 1 lan.

                long logTimeTreat = 0;
                long logTimeOut = 0;
                V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter = new V_HIS_PATIENT_TYPE_ALTER();
                HIS_PATIENT patient = new HIS_PATIENT();
                foreach (var treatment in treatments)
                {
                    if (!dicDepartmentTran.ContainsKey(treatment.ID)) continue;
                    if (!dicPatientTypeAlter.ContainsKey(treatment.ID)) continue;
                    //lay doi tuong cua benh nhan
                    PatientTypeAlter = dicPatientTypeAlter[treatment.ID].OrderBy(o => o.LOG_TIME).LastOrDefault();
                    //lay thong tin benh nhan
                    patient = dicPatient.ContainsKey(treatment.PATIENT_ID) ? dicPatient[treatment.PATIENT_ID] : new HIS_PATIENT();
                    //thoi gian vao noi tru
                    var patientType = dicPatientTypeAlter[treatment.ID].FirstOrDefault(o => o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU);
                    if (IsNotNull(patientType))
                    {
                        logTimeTreat = patientType.LOG_TIME;
                    }

                    foreach (var departmenttran in dicDepartmentTran[treatment.ID])
                    {
                        //thoi gian ra khoa
                        var depaTime = dicDepartmentTran[treatment.ID].FirstOrDefault(o => o.PREVIOUS_ID == departmenttran.DEPARTMENT_ID);
                        if (IsNotNull(depaTime) && depaTime.DEPARTMENT_IN_TIME.HasValue)
                        {
                            logTimeOut = depaTime.DEPARTMENT_IN_TIME.Value;
                        }
                        else
                        {
                            logTimeOut = treatment.OUT_TIME ?? 99999999999999;
                        }

                        //Neu khong vao noi tru hoac vao noi tru sau khi ra khoa -> dem kham
                        if (logTimeTreat == 0 || logTimeTreat > logTimeOut)
                            this.Count(exportDepartments, departmenttran.DEPARTMENT_ID, treatment, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, PatientTypeAlter, patient);
                        //Neu vao dieu tri truoc khi vao khoa -> dem dieu tri
                        else if (logTimeTreat <= departmenttran.DEPARTMENT_IN_TIME)
                            this.Count(exportDepartments, departmenttran.DEPARTMENT_ID, treatment, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, PatientTypeAlter, patient);
                        //Neu vao dieu tri sau khi vao khoa va truoc khi ra khoa -> dem ca dieu tri va kham
                        else if (logTimeTreat > departmenttran.DEPARTMENT_IN_TIME && logTimeTreat < logTimeOut)
                        {
                            this.Count(exportDepartments, departmenttran.DEPARTMENT_ID, treatment, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM, PatientTypeAlter, patient);
                            this.Count(exportDepartments, departmenttran.DEPARTMENT_ID, treatment, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, PatientTypeAlter, patient);
                        }
                    }
                }

            }
        }

        private void Count(List<HIS_DEPARTMENT> departments, long departmentId, V_HIS_TREATMENT treatment, long treatmentTypeId, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, HIS_PATIENT patient)
        {
            if (treatment != null && PatientTypeAlter != null)
            {
                bool isTreat = !this.IsExam(treatmentTypeId);

                if (this.IsQuCc(treatment, PatientTypeAlter, patient))
                {
                    this.Count(departments, departmentId, isTreat, ref bhquCC);
                }
                else if (this.IsQuOther(treatment, PatientTypeAlter, patient))
                {
                    this.Count(departments, departmentId, isTreat, ref bhquOther);
                }
                else if (this.IsBhQh(treatment, PatientTypeAlter.HEIN_CARD_NUMBER ?? ""))
                {
                    this.Count(departments, departmentId, isTreat, ref bhqh);
                }
                else if (this.IsBhTq(PatientTypeAlter.HEIN_CARD_NUMBER ?? ""))
                {
                    this.Count(departments, departmentId, isTreat, ref bhtq);
                }
                else if (this.IsBhQd(PatientTypeAlter.HEIN_CARD_NUMBER ?? ""))
                {
                    this.Count(departments, departmentId, isTreat, ref bhqd);
                }
                else if (this.IsBhytA(PatientTypeAlter.HEIN_CARD_NUMBER ?? ""))
                {
                    this.Count(departments, departmentId, isTreat, ref otherBhytA);
                }
                else if (PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    this.Count(departments, departmentId, isTreat, ref otherBhyt);
                }
                else if (PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {
                    this.Count(departments, departmentId, isTreat, ref fee);
                }
                else
                {
                    Mrs00123RDO rdo = others.Where(o => o.PatientTypeId == PatientTypeAlter.PATIENT_TYPE_ID).FirstOrDefault();
                    if (rdo == null)
                    {
                        rdo = new Mrs00123RDO();
                        rdo.PatientTypeId = PatientTypeAlter.PATIENT_TYPE_ID;
                        rdo.PatientTypeName = PatientTypeAlter.PATIENT_TYPE_NAME;
                        others.Add(rdo);
                    }
                    this.Count(departments, departmentId, isTreat, ref rdo);
                }
            }
        }

        /// <summary>
        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        /// </summary>
        /// <param name="others"></param>
        private void InitOthers(ref List<Mrs00123RDO> others)
        {
            others = MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs
                .Where(o => o.ID != MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                    && o.ID != MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__FEE
                    && o.ID != MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__QU)
                .Select(t => new Mrs00123RDO
                {
                    PatientTypeId = t.ID,
                    PatientTypeName = t.PATIENT_TYPE_NAME
                }).ToList();
        }

        private void Count(List<HIS_DEPARTMENT> departments, long departmentId, bool isTreat, ref Mrs00123RDO rdo)
        {
            Mrs00123RDOFieldType fieldType = isTreat ? Mrs00123RDOFieldType.TREAT : Mrs00123RDOFieldType.EXAM;
            for (int i = 0; i < departments.Count; i++)
            {
                if (departments[i].ID == departmentId)
                {
                    int val = rdo.GetValue(i + 1, fieldType);
                    rdo.SetValue(i + 1, fieldType, val + 1);
                    return;
                }
            }
        }

        //Doi tuong quan Can bo cao cap
        private bool IsQuCc(V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, HIS_PATIENT patient)
        {
            return PatientTypeAlter != null && treatment != null
                && IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR)
                && PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__QU
                && patient.MILITARY_RANK_ID.HasValue
                && MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR.Contains(patient.MILITARY_RANK_ID.Value);
        }

        //Doi tuong quan khac (ko phai la Can bo cao cap)
        private bool IsQuOther(V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER PatientTypeAlter, HIS_PATIENT patient)
        {
            return PatientTypeAlter != null && treatment != null
                && PatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__QU
                && (!patient.MILITARY_RANK_ID.HasValue ||
                !IsNotNullOrEmpty(MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR) ||
                !MRS.MANAGER.Config.HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR.Contains(patient.MILITARY_RANK_ID.Value));
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhQd(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber);
        }

        /// <summary>
        /// Co phai doi tuong BHYT tq hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber);
        }

        /// <summary>
        /// Co phai doi tuong BHYT A hay khong
        /// </summary>
        /// <param name="bhyt"></param>
        /// <returns></returns>
        private bool IsBhytA(string HeinCardNumber)
        {
            return this.IsHeinCardNumberLeverWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__AS, HeinCardNumber);
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
                && this.IsHeinCardNumberPrefixWith(MRS.MANAGER.Config.MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber);
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

        /// <summary>
        /// La kham hay ko
        /// </summary>
        /// <param name="PatientTypeAlter"></param>
        /// <returns></returns>
        private bool IsExam(long treatmentTypeId)
        {
            return treatmentTypeId.Equals(IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM);
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                this.PrepareData(departments, treatments, dicPatient, departmentTrans, PatientTypeAlters);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddSingleData(Dictionary<string, object> dicSingleData, Mrs00123RDO rdo, string keyPrefix)
        {
            for (int i = 0; i < Mrs00123RDO.MAX_DEPARTMENT_NUM; i++)
            {
                dicSingleData.Add(string.Format("{0}_Department{1}ExamAmount", keyPrefix, i + 1), rdo.GetValue(i + 1, Mrs00123RDOFieldType.EXAM));
                dicSingleData.Add(string.Format("{0}_Department{1}TreatAmount", keyPrefix, i + 1), rdo.GetValue(i + 1, Mrs00123RDOFieldType.TREAT));
            }
        }

        private void AddSingleData(Dictionary<string, object> dicSingleData, List<string> departmentNames)
        {
            for (int i = 0; i < departmentNames.Count; i++)
            {
                dicSingleData.Add(string.Format("DEPARTMENT_NAME_{0}", i + 1), departmentNames[i]);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("LOG_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("LOG_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                this.AddSingleData(dicSingleTag, this.bhquCC, "BhQuCC");
                this.AddSingleData(dicSingleTag, this.bhquOther, "BhQuOther");
                this.AddSingleData(dicSingleTag, this.bhqh, "BhQh");
                this.AddSingleData(dicSingleTag, this.bhtq, "BhTq");
                this.AddSingleData(dicSingleTag, this.bhqd, "BhQd");
                this.AddSingleData(dicSingleTag, this.otherBhyt, "OtherBhyt");
                this.AddSingleData(dicSingleTag, this.otherBhytA, "OtherBhytA");
                this.AddSingleData(dicSingleTag, this.fee, "Fee");

                this.AddSingleData(dicSingleTag, this.departmentNames);

                objectTag.AddObjectData(store, "Others", this.others);
                objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    public class HTreatmentTypeCode
    {
        public const string EXAM = "01";
        public const string O_TREAT = "02";
        public const string I_TREAT = "03";
    }

    class RDOCustomerFuncRownumberData : FlexCel.Report.TFlexCelUserFunction
    {
        public RDOCustomerFuncRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            long result = 0;
            try
            {
                long rownumber = Convert.ToInt64(parameters[0]);
                result = (rownumber + 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return result;
        }
    }
}
