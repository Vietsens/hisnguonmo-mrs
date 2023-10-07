using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDeathWithin;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisEmployee;
using MOS.MANAGER.HisWorkPlace;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisTreatmentResult;
using MOS.MANAGER.HisHeinApproval;

namespace MRS.Processor.Mrs00398
{
    class Mrs00398Processor : AbstractProcessor
    {
        Mrs00398Filter castFilter = null;
        List<Mrs00398RDO> listRdo = new List<Mrs00398RDO>();
        List<Mrs00398RDO> listDepartment = new List<Mrs00398RDO>();
        List<HIS_EMPLOYEE> ListEmployee = new List<HIS_EMPLOYEE>();
        List<TREATMENT_NUMFILM> listTreatmentNumFilm = new List<TREATMENT_NUMFILM>();
        public Mrs00398Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        List<HIS_TREATMENT> listTreatment = null;
        List<HIS_TREATMENT_END_TYPE> listTreatmentEndType = null;
        List<HIS_PATIENT_CLASSIFY> listPatientClassify = new List<HIS_PATIENT_CLASSIFY>();
        List<HIS_TREATMENT_RESULT> listTreatmentResult = null;
        List<HIS_WORK_PLACE> listWorkPlace = null;
        HIS_DEPARTMENT department = null;
        List<HIS_HEIN_APPROVAL> listHeinApprova = new List<HIS_HEIN_APPROVAL>();

        public override Type FilterType()
        {
            return typeof(Mrs00398Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00398Filter)this.reportFilter;
                listTreatmentNumFilm = new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("MRS00398: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisEmployeeFilterQuery HisEmployeefilter = new HisEmployeeFilterQuery() { };
                ListEmployee = new HisEmployeeManager().Get(HisEmployeefilter);
                HisTreatmentEndTypeFilterQuery HisTreatmentEndTypefilter = new HisTreatmentEndTypeFilterQuery() { };
                listTreatmentEndType = new HisTreatmentEndTypeManager().Get(HisTreatmentEndTypefilter);
                HisTreatmentResultFilterQuery HisTreatmentResultfilter = new HisTreatmentResultFilterQuery() { };
                listTreatmentResult = new HisTreatmentResultManager().Get(HisTreatmentResultfilter);
                HisWorkPlaceFilterQuery HisWorkPlacefilter = new HisWorkPlaceFilterQuery() { };
                listWorkPlace = new HisWorkPlaceManager().Get(HisWorkPlacefilter);
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                
                treatmentFilter.IS_PAUSE = true;
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID.Value);
                    if (department == null)
                    {
                        throw new Exception("KHong lay duoc department theo id filter");
                    }
                    treatmentFilter.END_DEPARTMENT_IDs = new List<long>() { castFilter.DEPARTMENT_ID.Value };
                }
                if (castFilter.DEPARTMENT_IDs!=null)
                {
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                }

                listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).Get(treatmentFilter);


                if (this.castFilter.IS_TREAT != null)
                {
                    if (this.castFilter.IS_TREAT == true)
                    {
                        listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                    else
                    {
                        listTreatment = listTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }

                if (this.castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatment = listTreatment.Where(o => this.castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }

                if (this.castFilter.PATIENT_CLASSIFY_IDs != null)
                {
                    listTreatment = listTreatment.Where(o => this.castFilter.PATIENT_CLASSIFY_IDs.Contains(o.TDL_PATIENT_CLASSIFY_ID ?? 0)).ToList();
                }

                if (castFilter.IS_NOT_TRANSFER.HasValue)
                {
                    if (castFilter.IS_NOT_TRANSFER.Value)
                    {
                        listTreatment = listTreatment.Where(o => o.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).ToList();
                    }
                    else if (!castFilter.IS_NOT_TRANSFER.Value)
                    {
                        listTreatment = listTreatment.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).ToList();
                    }
                }
                else
                    if (castFilter.TREATMENT_END_TYPE_IDs != null)
                    {
                        listTreatment = listTreatment.Where(o => castFilter.TREATMENT_END_TYPE_IDs.Contains(o.TREATMENT_END_TYPE_ID ?? 0)).ToList();
                    }
                var listPatientId = listTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                if (listPatientId != null)
                {
                    var skip = 0;
                    while (listPatientId.Count - skip > 0)
                    {
                        var listIDs = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisPatientfilter = new HisPatientFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var ListPatientSub = new HisPatientManager(paramGet).Get(HisPatientfilter);
                        listPatient.AddRange(ListPatientSub);
                    }
                }

                var listTreatmentId = listTreatment.Select(o => o.ID).Distinct().ToList();
                if (listTreatmentId != null)
                {
                    var skip = 0;
                    while(listTreatmentId.Count - skip >0)
                    {
                        var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery()
                        {
                            TREATMENT_IDs = listIds
                        };
                        var ListPatientTypeAlterSub = new HisPatientTypeAlterManager(paramGet).Get(HisPatientTypeAlterfilter);
                        listPatientTypeAlter.AddRange(ListPatientTypeAlterSub);
                    
                    }
                
                }

          

                //get PatientClassify
                GetPatientClassify();

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du liue MRS00398");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetPatientClassify()
        {
            listPatientClassify = new ManagerSql().GetPatientClassify();
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTreatment))
                {
                    this.ProcessDataDetail();
                    this.ProcessListRdo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {
            foreach (var treatment in listTreatment)
            {
                if (!treatment.END_DEPARTMENT_ID.HasValue)
                    continue;

                Mrs00398RDO rdo = new Mrs00398RDO(treatment);
                rdo.ICD_MAIN_TEXT = treatment.ICD_NAME;

                rdo.HEIN_MEDI_ORG_CODE = (HisBranchCFG.HisBranchs.FirstOrDefault() ?? new HIS_BRANCH()).HEIN_MEDI_ORG_CODE;
                rdo.TREATMENT_ID = treatment.ID;
                rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                rdo.ICD_NAME = treatment.ICD_NAME;
                rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                rdo.END_ADDRESS = (!string.IsNullOrWhiteSpace(treatment.TDL_PATIENT_ADDRESS) &&!string.IsNullOrWhiteSpace(treatment.TDL_PATIENT_COMMUNE_NAME) && treatment.TDL_PATIENT_ADDRESS.IndexOf(treatment.TDL_PATIENT_COMMUNE_NAME) > 1) ? treatment.TDL_PATIENT_ADDRESS.Substring(0, treatment.TDL_PATIENT_ADDRESS.IndexOf(treatment.TDL_PATIENT_COMMUNE_NAME) - 1) : "";
                rdo.OUT_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID.Value;
                rdo.OUT_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                rdo.OUT_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                rdo.OUT_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                rdo.OUT_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                rdo.END_ORDER = treatment.OUT_CODE;
                rdo.OUT_TIME = treatment.OUT_TIME ?? 0;
                rdo.IN_TIME = treatment.IN_TIME;
                if(treatment.TREATMENT_END_TYPE_ID == 2)
                {
                    rdo.CV_TIME = treatment.OUT_TIME;
                }
                rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                {
                    rdo.IS_CURED = "X";
                }
                else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                {
                    rdo.IS_REDUCE = "X";
                }
                else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                {
                    rdo.IS_DIE = "X";
                }

                else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                {
                    rdo.IS_CONSTANT = "X";
                }
                else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                {
                    rdo.IS_HEAVIER = "X";
                }
                if (treatment.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.IS_BHYT = "X";
                }


                //var ListTreatmentIds = listHeinApprova.Where(o => o.TREATMENT_ID == treatment.ID).Select(o => o.TREATMENT_ID).ToList();
                      
                //int skip = 0;
                //while (ListTreatmentIds.Count - skip > 0)
                //{
                //    var listId = ListTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                //    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                //    HisHeinApprovalFilterQuery heinapprovaFilter = new HisHeinApprovalFilterQuery();
                //   // HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                //    heinapprovaFilter.TREATMENT_IDs = listId;


                //    var heinapprova = new HisHeinApprovalManager().Get(heinapprovaFilter);


                //    if (IsNotNullOrEmpty(heinapprova))
                //    {
                //        listHeinApprova.AddRange(heinapprova);
                //    }
                //}
                HIS_PATIENT_TYPE_ALTER patienttypealter = listPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID);

                if (patienttypealter != null)
                {

                    if (treatment.TREATMENT_END_TYPE_ID == 6 && patienttypealter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {

                        rdo.RAVIEN_DT = "X";
                    }
                    if (treatment.TREATMENT_END_TYPE_ID == 6 && patienttypealter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                    {

                        rdo.RAVIEN_TT = "X";
                    }
                }
                if (treatment.TREATMENT_END_TYPE_ID == 6 && treatment.TDL_PATIENT_GENDER_ID == 2 && (treatment.IN_TIME - treatment.TDL_PATIENT_DOB) > 600000000000)
                {
                    rdo.RAVIEN_NAM_60 = "X";
                
                
                }
                if (treatment.TREATMENT_END_TYPE_ID == 6 && treatment.TDL_PATIENT_GENDER_ID == 1 && (treatment.IN_TIME - treatment.TDL_PATIENT_DOB) > 600000000000)
                {
                    rdo.RAVIEN_NU_60 = "X";


                }
                if (treatment.TREATMENT_END_TYPE_ID == 6 && (treatment.IN_TIME - treatment.TDL_PATIENT_DOB) < 150000000000)
                {
                    rdo.RAVIEN_15 = "X";


                }
                if (!String.IsNullOrEmpty(treatment.ICD_NAME))
                {
                    rdo.ICD_NAME = treatment.ICD_NAME;
                }
                else
                {
                    rdo.ICD_NAME = treatment.ICD_NAME;
                }
                if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET || treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                {
                    rdo.DATE_DEAD_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);

                    if (treatment.DEATH_WITHIN_ID == MANAGER.Config.HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                    {
                        rdo.IS_DEAD_IN_24H = "X";
                    }
                }
                else
                {
                    rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                }
                CalcuatorAge(rdo, treatment);
                if (treatment.CLINICAL_IN_TIME.HasValue)
                {
                    rdo.TOTAL_DATE_TREATMENT = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME);
                }
                HIS_EMPLOYEE employee = ListEmployee.FirstOrDefault(o => o.LOGINNAME == treatment.DOCTOR_LOGINNAME);
                if (employee != null)
                {
                    rdo.DIPLOMA = employee.DIPLOMA;
                }

                HIS_PATIENT patient = listPatient.FirstOrDefault(o => o.ID == treatment.PATIENT_ID);
                if (patient != null)
                {
                    rdo.FATHER_NAME = patient.FATHER_NAME;
                    rdo.MOTHER_NAME = patient.MOTHER_NAME;
                    rdo.WORK_PLACE_CODE = (listWorkPlace.FirstOrDefault(o => o.ID == patient.WORK_PLACE_ID) ?? new HIS_WORK_PLACE()).WORK_PLACE_CODE;
                }
                if (listTreatmentNumFilm != null && listTreatmentNumFilm.Count > 0)
                {
                    var treatmentNumFilm = listTreatmentNumFilm.FirstOrDefault(o => o.ID == treatment.ID);
                    if (treatmentNumFilm != null)
                    {
                        rdo.MRI = treatmentNumFilm.MRI;
                        rdo.CT = treatmentNumFilm.CT;
                        rdo.XQ = treatmentNumFilm.XQ;
                    }
                }
                if (listTreatmentEndType != null)
                {
                    var treatmentEndType = listTreatmentEndType.FirstOrDefault(o => o.ID == treatment.TREATMENT_END_TYPE_ID);
                    if (treatmentEndType != null)
                    {
                        rdo.TREATMENT_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                        rdo.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                    }
                }
                if (listTreatmentResult != null)
                {
                    var treatmentResult = listTreatmentResult.FirstOrDefault(o => o.ID == treatment.TREATMENT_RESULT_ID);
                    if (treatmentResult != null)
                    {
                        rdo.TREATMENT_RESULT_CODE = treatmentResult.TREATMENT_RESULT_CODE;
                        rdo.TREATMENT_RESULT_NAME = treatmentResult.TREATMENT_RESULT_NAME;
                    }
                }

                //thông tin đối tượng chi tiết
                var patientClassify = listPatientClassify.FirstOrDefault(o => o.ID == treatment.TDL_PATIENT_CLASSIFY_ID);
                if (patientClassify != null)
                {
                    rdo.PATIENT_CLASSIFY_CODE = patientClassify.PATIENT_CLASSIFY_CODE;
                    //rdo.PATIENT_CLASSIFY_NAME = patientClassify.PATIENT_CLASSIFY_NAME;
                }
                rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                rdo.STORE_CODE = treatment.STORE_CODE;

                listRdo.Add(rdo);
            }
        }

        void ProcessListRdo()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.END_ORDER).ThenBy(t => t.OUT_TIME).ToList();
                    listDepartment = listRdo.GroupBy(g => g.OUT_DEPARTMENT_ID).Select(s => new Mrs00398RDO() { OUT_DEPARTMENT_ID = s.First().OUT_DEPARTMENT_ID, OUT_DEPARTMENT_CODE = s.First().OUT_DEPARTMENT_CODE, OUT_DEPARTMENT_NAME = s.First().OUT_DEPARTMENT_NAME }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00398RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                int tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB) ?? 0;
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

                var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.branch_id);
                if (branch != null && !dicSingleTag.ContainsKey("ORGANIZATION_CODE"))
                {
                    dicSingleTag.Add("ORGANIZATION_CODE", branch.BRANCH_CODE);
                }
                if (department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
                }
                objectTag.AddObjectData(store, "Department", listDepartment);
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.END_CODE).ToList());
                objectTag.AddRelationship(store, "Department", "Report", "OUT_DEPARTMENT_ID", "OUT_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "Sick", listRdo.Where(o => o.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM).ToList());
                objectTag.AddRelationship(store, "Department", "Sick", "OUT_DEPARTMENT_ID", "OUT_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "RvReport", listRdo.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && o.TREATMENT_END_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).ToList());
                objectTag.AddRelationship(store, "Department", "RvReport", "OUT_DEPARTMENT_ID", "OUT_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "DepartmentInside", listRdo.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && !(castFilter.DEPARTMENT_CODE__OUTPATIENTs ?? "").Contains(o.OUT_DEPARTMENT_CODE)).GroupBy(g => g.OUT_DEPARTMENT_ID).Select(s => new Mrs00398RDO() { OUT_DEPARTMENT_ID = s.First().OUT_DEPARTMENT_ID, OUT_DEPARTMENT_CODE = s.First().OUT_DEPARTMENT_CODE, OUT_DEPARTMENT_NAME = s.First().OUT_DEPARTMENT_NAME }).ToList());
                objectTag.AddObjectData(store, "ReportInside", listRdo.OrderBy(p => p.END_CODE).Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && !(castFilter.DEPARTMENT_CODE__OUTPATIENTs ?? "").Contains(o.OUT_DEPARTMENT_CODE)).ToList());
                objectTag.AddRelationship(store, "DepartmentInside", "ReportInside", "OUT_DEPARTMENT_ID", "OUT_DEPARTMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
