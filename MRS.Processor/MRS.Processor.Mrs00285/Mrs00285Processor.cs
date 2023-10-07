using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisRoomType;
using MOS.MANAGER.HisCareer;
using MOS.MANAGER.HisCare;
using MOS.MANAGER.HisMilitaryRank;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
 

using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisPatient; 
using AutoMapper; 

namespace MRS.Processor.Mrs00285
{
    public class Mrs00285Processor : AbstractProcessor
    {

        private Mrs00285RDO bhquCC = new Mrs00285RDO(); //Doi tuong Quan, cao cap
        private Mrs00285RDO bhquOther = new Mrs00285RDO(); //Doi tuong Quan, khac
        CommonParam paramGet = new CommonParam(); 
        private Mrs00285RDO bhqh = new Mrs00285RDO(); 
        private Mrs00285RDO bhtq = new Mrs00285RDO(); 
        private Mrs00285RDO bhqd = new Mrs00285RDO(); 
        private Mrs00285RDO fee = new Mrs00285RDO(); 
        private Mrs00285RDO otherBhytA = new Mrs00285RDO(); 
        private Mrs00285RDO otherBhyt = new Mrs00285RDO(); 

        private List<Mrs00285RDO> others = new List<Mrs00285RDO>(); 
        private List<string> examSerrviceTypeNames = new List<string>(); 
        private List<Mrs00285RDO> ListRdo = new List<Mrs00285RDO>(); 
        List<V_HIS_SERE_SERV> LisSereServ = new List<V_HIS_SERE_SERV>(); 
        List<HIS_SERVICE_REQ> listServcieReq = new List<HIS_SERVICE_REQ>(); 
        List<V_HIS_SERVICE> listExamService = new List<V_HIS_SERVICE>(); 
        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>(); 
        Dictionary<long, V_HIS_PATIENT> dicPatient = new Dictionary<long, V_HIS_PATIENT>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlterExam = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlterTreat = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();

        private List<string> TreatmentCodes = new List<string>();

        public Mrs00285Processor(CommonParam param, string reportTypeCode)

            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00285Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00285Filter)reportFilter); 
            var result = true; 
            try
            {
                //HSDT
                var treatmentFilter = new HisTreatmentViewFilterQuery
                {
                    IN_TIME_FROM = filter.TIME_FROM,
                    IN_TIME_TO = filter.TIME_TO
                }; 
                this.dicTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter).ToDictionary(o => o.ID); 
                var listTreatmentId = dicTreatment.Keys.ToList(); 


                //vao khoa
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    this.ListDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>(); 
                    var skip = 0; 
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var departmentTranViewFilter = new HisDepartmentTranViewFilterQuery
                        {
                            TREATMENT_IDs = listIDs,
                            DEPARTMENT_ID = filter.DEPARTMENT_ID
                        }; 
                        var departmentTranViews = new HisDepartmentTranManager(paramGet).GetView(departmentTranViewFilter); 
                        this.ListDepartmentTran.AddRange(departmentTranViews); 
                    }
                }

                //chuyen doi tuong
                List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0; 
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            ORDER_FIELD = "LOG_TIME",
                            ORDER_DIRECTION = "ASC"
                        }; 
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter); 
                        LisPatientTypeAlter.AddRange(LisPatientTypeAlterLib); 
                    }
                }
                var patientIds = LisPatientTypeAlter.Select(o => o.TDL_PATIENT_ID).Distinct().ToList(); 
                //BN
                List<V_HIS_PATIENT> patient = new List<V_HIS_PATIENT>(); 

                if (IsNotNullOrEmpty(patientIds))
                {
                    var skip = 0; 
                    while (patientIds.Count - skip > 0)
                    {
                        var listIDs = patientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var patientViewFilter = new HisPatientViewFilterQuery
                        {
                            IDs = listIDs
                        }; 
                        var patientSub = new HisPatientManager(paramGet).GetView(patientViewFilter); 
                        patient.AddRange(patientSub); 
                    }
                }
                this.dicPatient = patient.ToDictionary(p => p.ID); 

                //Chuyen sang kham
                this.dicPatientTypeAlterExam = LisPatientTypeAlter.Where(o => IsExam(o)).GroupBy(p => p.TREATMENT_ID).ToDictionary(q => q.Key, q => q.First()); 

                //Chuyen sang DTri
                this.dicPatientTypeAlterTreat = LisPatientTypeAlter.Where(o => !IsExam(o)).GroupBy(p => p.TREATMENT_ID).ToDictionary(q => q.Key, q => q.First()); 

                //phong tiep don
                HisRoomViewFilterQuery rfilter = new HisRoomViewFilterQuery(); 
                rfilter.ROOM_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD; // HisRoomTypeCFG.HIS_ROOM_TYPE_ID__TD; 
                var listRoomTD = new HisRoomManager(paramGet).GetView(rfilter); 

                //yeu cau kham

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0; 
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var Reqfilter = new HisServiceReqFilterQuery()
                        {
                            TREATMENT_IDs = limit,
                            SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                            REQUEST_ROOM_IDs = listRoomTD.Select(o => o.ID).ToList(),
                            HAS_EXECUTE = true,
                            EXECUTE_DEPARTMENT_ID = filter.DEPARTMENT_ID
                        }; 
                        var listServcieReqSub = new HisServiceReqManager(paramGet).Get(Reqfilter); 
                        this.listServcieReq.AddRange(listServcieReqSub); 
                    }
                }
                var listServiceReqId = listServcieReq.Select(o => o.ID).Distinct().ToList(); 
                // YC-DV

                if (IsNotNullOrEmpty(listServiceReqId))
                {
                    var skip = 0; 
                    while (listServiceReqId.Count - skip > 0)
                    {
                        var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery()
                        {
                            SERVICE_REQ_IDs = listIDs
                        }; 
                        var LisSereServSub = new HisSereServManager(paramGet).GetView(filterSereServ); 
                        this.LisSereServ.AddRange(LisSereServSub); 
                    }
                    
                }
                Inventec.Common.Logging.LogSystem.Info("Các HSDT làm TT: " + String.Join(", ", listServcieReq.Select(z => z.TDL_TREATMENT_CODE).ToList())); 
                //dv kham
                HisServiceViewFilterQuery svFilter = new HisServiceViewFilterQuery(); 
                svFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH;
                svFilter.ORDER_FIELD = "NUM_ORDER";
                svFilter.ORDER_DIRECTION = "ASC";
                svFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                this.listExamService = new HisServiceManager(paramGet).GetView(svFilter).Take(Mrs00285RDO.MAX_EXAM_SERVICE_TYPE_NUM).ToList();

                //this.listExamService = this.listExamService.Where(o => this.LisSereServ.Exists(p => p.SERVICE_ID == o.ID)).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                if (IsNotNullOrEmpty(LisSereServ))
                {
                    this.examSerrviceTypeNames = listExamService.Select(o => o.SERVICE_NAME).ToList();
                    this.TreatmentCodes = listExamService.Select(o => "").ToList();
                    //Doi tuong khac
                    this.InitOthers(ref this.others); 


                    foreach (var ss in LisSereServ)
                    {
                        if (!dicTreatment.ContainsKey(ss.TDL_TREATMENT_ID ?? 0)) continue; 
                        if (!dicPatient.ContainsKey(ss.TDL_PATIENT_ID ?? 0)) continue; 
                        //Dem kham:
                        if (dicPatientTypeAlterExam.ContainsKey(ss.TDL_TREATMENT_ID ?? 0))
                        {
                            this.Count(listExamService, ss.SERVICE_ID, dicTreatment[ss.TDL_TREATMENT_ID ?? 0], dicPatientTypeAlterExam[ss.TDL_TREATMENT_ID ?? 0], dicPatientTypeAlterExam[ss.TDL_TREATMENT_ID ?? 0].HEIN_CARD_NUMBER ?? "", dicPatient[ss.TDL_PATIENT_ID ?? 0]); 
                        }

                        //Dem dieu tri:
                        if (dicPatientTypeAlterTreat.ContainsKey(ss.TDL_TREATMENT_ID ?? 0))
                        {
                            this.Count(listExamService, ss.SERVICE_ID, dicTreatment[ss.TDL_TREATMENT_ID ?? 0], dicPatientTypeAlterTreat[ss.TDL_TREATMENT_ID ?? 0], dicPatientTypeAlterTreat[ss.TDL_TREATMENT_ID ?? 0].HEIN_CARD_NUMBER ?? "", dicPatient[ss.TDL_PATIENT_ID ?? 0]); 
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00285Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("LOG_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00285Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00285Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("LOG_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00285Filter)reportFilter).TIME_TO)); 
            }

            this.AddSingleData(dicSingleTag, this.bhquCC, "BhQuCC"); 
            this.AddSingleData(dicSingleTag, this.bhquOther, "BhQuOther"); 
            this.AddSingleData(dicSingleTag, this.bhqh, "BhQh"); 
            this.AddSingleData(dicSingleTag, this.bhtq, "BhTq"); 
            this.AddSingleData(dicSingleTag, this.bhqd, "BhQd"); 
            this.AddSingleData(dicSingleTag, this.otherBhyt, "OtherBhyt"); 
            this.AddSingleData(dicSingleTag, this.otherBhytA, "OtherBhytA"); 
            this.AddSingleData(dicSingleTag, this.fee, "Fee");

            this.AddSingleData(dicSingleTag, this.examSerrviceTypeNames,this.TreatmentCodes);

            objectTag.AddObjectData(store, "Others", this.others); 
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncRownumberData()); 
            objectTag.AddObjectData(store, "Report", ListRdo); 
        }

        private void Count(List<V_HIS_SERVICE> examServiceTypes, long serviceId, V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter, string HeinCardNumber, V_HIS_PATIENT patient)
        {
            if (treatment != null && patientTypeAlter != null)
            {
                bool isTreat = !this.IsExam(patientTypeAlter); 

                if (this.IsQuCc(treatment, patientTypeAlter, patient))//Doi tuong quan doi quan ham cao cap
                {
                     bhquCC.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref bhquCC);
                }
                else if (this.IsQuOther(treatment, patientTypeAlter, patient))//Doi tuong quan doi quan ham khac
                {
                     bhquOther.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref bhquOther);
                }
                else if (this.IsBhQh(treatment, HeinCardNumber, patient))//nghe nghiep Quan huu
                {
                     bhqh.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref bhqh);
                }
                else if (this.IsBhTq(HeinCardNumber))// than nhan
                {
                     bhtq.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref bhtq);
                }
                else if (this.IsBhQd(HeinCardNumber))//the quan doi
                {
                     bhqd.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref bhqd);
                }
                else if (this.IsBhytA(HeinCardNumber))//the bao hiem a
                {

                     otherBhytA.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref otherBhytA);
                }
                else if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                     otherBhyt.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                    this.Count(examServiceTypes, serviceId, isTreat, ref otherBhyt);
                }
                else if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                {

                     fee.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";

                    this.Count(examServiceTypes, serviceId, isTreat, ref fee);
                }
                else
                {
                    Mrs00285RDO rdo = others.Where(o => o.PatientTypeId == patientTypeAlter.PATIENT_TYPE_ID).ToList().Count > 0 ? others.Where(o => o.PatientTypeId == patientTypeAlter.PATIENT_TYPE_ID).FirstOrDefault() : null; 
                    if (rdo == null)
                    {
                        rdo = new Mrs00285RDO(); 
                        rdo.PatientTypeId = patientTypeAlter.PATIENT_TYPE_ID; 
                        rdo.PatientTypeName = patientTypeAlter.PATIENT_TYPE_NAME; 
                        others.Add(rdo); 
                    }
                    else
                    {
                         rdo.TREATMENT_CODE = (!isTreat)?treatment.TREATMENT_CODE:"";
                        this.Count(examServiceTypes, serviceId, isTreat, ref rdo);
                    }
                }
            }
        }

        /// <summary>
        /// Thiet lap gia tri ban dau cho danh sach cac dien doi tuong (ko phai la BHYT va vien phi)
        /// </summary>
        /// <param name="others"></param>
        private void InitOthers(ref List<Mrs00285RDO> others)
        {
            others = HisPatientTypeCFG.PATIENT_TYPEs
                .Where(o => o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT
                    && o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__FEE
                    && o.ID != HisPatientTypeCFG.PATIENT_TYPE_ID__QU)
                .Select(t => new Mrs00285RDO
                {
                    PatientTypeId = t.ID,
                    PatientTypeName = t.PATIENT_TYPE_NAME
                }).ToList(); 
        }

        private void Count(List<V_HIS_SERVICE> examServiceTypes, long serviceId, bool isTreat, ref Mrs00285RDO rdo)
        {
            Mrs00285RDOFieldType fieldType = isTreat ? Mrs00285RDOFieldType.TREAT : Mrs00285RDOFieldType.EXAM; 
            for (int i = 0;  i < examServiceTypes.Count;  i++)
            {
                if (examServiceTypes[i].ID == serviceId)
                {
                    int val = rdo.GetValue(i + 1, fieldType); 
                    rdo.SetValue(i + 1, fieldType, val + 1);
                    TreatmentCodes[i] += rdo.TREATMENT_CODE+", ";
                    return; 
                }
            }
        }

        //Doi tuong quan Can bo cao cap
        private bool IsQuCc(V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter, V_HIS_PATIENT patient)
        {
            return patientTypeAlter != null && treatment != null
                && IsNotNullOrEmpty(HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR)
                && patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__QU
                && patient.MILITARY_RANK_ID.HasValue
                && HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR.Contains(patient.MILITARY_RANK_ID.Value); 
        }

        //Doi tuong quan khac (ko phai la Can bo cao cap)
        private bool IsQuOther(V_HIS_TREATMENT treatment, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter, V_HIS_PATIENT patient)
        {
            return patientTypeAlter != null && treatment != null
                && patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__QU
                && (!patient.MILITARY_RANK_ID.HasValue || !IsNotNullOrEmpty(HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR) || !HisMilitaryRankCFG.HIS_MILITARY_RANK_ID__SENIOR.Contains(patient.MILITARY_RANK_ID.Value)); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan huu hay khong
        /// </summary>
        /// <param name="HeinCardNumber"></param>
        /// <returns></returns>
        private bool IsBhQd(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QDS, HeinCardNumber); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT tq hay khong
        /// </summary>
        /// <param name="HeinCardNumber"></param>
        /// <returns></returns>
        private bool IsBhTq(string HeinCardNumber)
        {
            return this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__TQS, HeinCardNumber); 
        }
        /// <summary>
        /// Co phai doi tuong BHYT A hay khong
        /// </summary>
        /// <param name="HeinCardNumber"></param>
        /// <returns></returns>
        private bool IsBhytA(string HeinCardNumber)
        {
            return this.IsHeinCardNumberLeverWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__AS, HeinCardNumber); 
        }

        /// <summary>
        /// Co phai doi tuong BHYT quan hưu hay khong
        /// </summary>
        /// <param name="treatment"></param>
        /// <param name="HeinCardNumber"></param>
        /// <returns></returns>
        private bool IsBhQh(V_HIS_TREATMENT treatment, string HeinCardNumber, V_HIS_PATIENT patient)
        {
            return treatment != null
                && patient.CAREER_ID == HisCareerCFG.CAREER_ID__RETIRED_MILITARY
                && this.IsHeinCardNumberPrefixWith(MilitaryHeinCFG.HEIN_CARD_NUMBER_PREFIX__QHS, HeinCardNumber); 
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
        /// La noi tru hay ko
        /// </summary>
        /// <param name="patientTypeAlter"></param>
        /// <returns></returns>
        private bool IsExam(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.EXAM); 
        }
        private bool IsInTreat(V_HIS_PATIENT_TYPE_ALTER patientTypeAlter)
        {
            return patientTypeAlter != null && !string.IsNullOrWhiteSpace(patientTypeAlter.TREATMENT_TYPE_CODE) && patientTypeAlter.TREATMENT_TYPE_CODE.Equals(HTreatmentTypeCode.I_TREAT); 
        }
        private void AddSingleData(Dictionary<string, object> dicSingleData, Mrs00285RDO rdo, string keyPrefix)
        {
            for (int i = 0;  i < Mrs00285RDO.MAX_EXAM_SERVICE_TYPE_NUM;  i++)
            {
                dicSingleData.Add(string.Format("{0}_ExamServiceType{1}ExamAmount", keyPrefix, i + 1), rdo.GetValue(i + 1, Mrs00285RDOFieldType.EXAM)); 
                dicSingleData.Add(string.Format("{0}_ExamServiceType{1}TreatAmount", keyPrefix, i + 1), rdo.GetValue(i + 1, Mrs00285RDOFieldType.TREAT));
                
            }
        }

        private void AddSingleData(Dictionary<string, object> dicSingleData, List<string> examServiceTypeNames, List<string> TreatmentCodes)
        {
            for (int i = 0; i < examServiceTypeNames.Count; i++)
            {
                dicSingleData.Add(string.Format("EXAM_SERVICE_TYPE_NAME_{0}", i + 1), examServiceTypeNames[i]);
                dicSingleData.Add(string.Format("EXAM_SERVICE_TYPE_TREATMENT_CODE_{0}", i + 1), TreatmentCodes[i]);
            }
        }

        private Mrs00285Filter ProcessCastFilterQuery(Mrs00285Filter mrs00119Filter)
        {
            Mrs00285Filter castFilter = null; 
            try
            {
                if (mrs00119Filter == null) throw new ArgumentNullException("Input param mrs00119Filter is null"); 

                Mapper.CreateMap<Mrs00285Filter, Mrs00285Filter>(); 
                castFilter = Mapper.Map<Mrs00285Filter, Mrs00285Filter>(mrs00119Filter); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return castFilter; 
        }
    }
    public class HTreatmentTypeCode
    {
        public const string EXAM = "01"; 
        public const string O_TREAT = "02"; 
        public const string I_TREAT = "03"; 


    }
    class RDOCustomerFuncRownumberData : TFlexCelUserFunction
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
/*
 check khoa kham benh:
 */
