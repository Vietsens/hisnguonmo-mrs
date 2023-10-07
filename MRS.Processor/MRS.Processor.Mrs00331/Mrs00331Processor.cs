using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisServiceReq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisExecuteRoom;

namespace MRS.Processor.Mrs00331
{
    class Mrs00331Processor : AbstractProcessor
    {
        Mrs00331Filter castFilter = null;

        List<Mrs00331RDO> listRdo = new List<Mrs00331RDO>();

        List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();
        List<HIS_SERVICE_REQ> listServiceReqs = new List<HIS_SERVICE_REQ>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<V_HIS_EXECUTE_ROOM> listRooms = new List<V_HIS_EXECUTE_ROOM>();
        List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>();
        List<HIS_PATIENT> listPatients = new List<HIS_PATIENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<Mrs00331RDO> ListRdoDetail = new List<Mrs00331RDO>();
        List<Mrs00331RDO> ListDistrict = new List<Mrs00331RDO>();

        public string BRANCH_NAME = "";
        public string PARENT_ORGANIZATION_NAME = "";
        public string EXECUTE_DEPARTMENT_NAME = "";
        public string EXECUTE_DEPARTMENT_CODE = "";
        public string EXAM_ROOM_NAME = "";
        public string EXAM_ROOM_CODE = "";
        public string HEIN_PROVINCE_CODE = "";

        public Mrs00331Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00331Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00331Filter)this.reportFilter;

                #region blah...blah...
                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                branchFilter.ID = castFilter.BRANCH_ID;
                listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchFilter);

                if (IsNotNullOrEmpty(listBranchs))
                {
                    this.HEIN_PROVINCE_CODE = string.Join(",", listBranchs.Select(o => (o.HEIN_PROVINCE_CODE ?? "").ToUpper()).ToList());
                    this.PARENT_ORGANIZATION_NAME = string.Join(";\r\n", listBranchs.Select(o => (o.PARENT_ORGANIZATION_NAME ?? "").ToUpper()).ToList());
                    this.BRANCH_NAME = string.Join(";\r\n", listBranchs.Select(o => o.BRANCH_NAME.ToUpper()).ToList());
                }
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.ID = castFilter.EXECUTE_DEPARTMENT_ID;
                departmentFilter.BRANCH_ID = castFilter.BRANCH_ID;
                listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);

                if (IsNotNullOrEmpty(listDepartments))
                {
                    this.EXECUTE_DEPARTMENT_CODE = string.Join(",", listDepartments.Select(o => o.DEPARTMENT_CODE.ToUpper()).ToList());
                    this.EXECUTE_DEPARTMENT_NAME = string.Join(";\r\n", listDepartments.Select(o => o.DEPARTMENT_NAME.ToUpper()).ToList());
                }
                listRooms = new HisExecuteRoomManager().GetView(new HisExecuteRoomViewFilterQuery() { IS_EXAM = true });
                if (castFilter.EXAM_ROOM_ID != null)
                {
                    listRooms = listRooms.Where(o => o.ROOM_ID == castFilter.EXAM_ROOM_ID).ToList();
                }
                if (castFilter.EXAM_ROOM_IDs != null)
                {
                    listRooms = listRooms.Where(o => castFilter.EXAM_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                if (castFilter.BRANCH_ID != null)
                {
                    listRooms = listRooms.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                }
                if (castFilter.EXECUTE_DEPARTMENT_ID != null)
                {
                    listRooms = listRooms.Where(o => o.DEPARTMENT_ID == castFilter.EXECUTE_DEPARTMENT_ID).ToList();
                }
                if (IsNotNullOrEmpty(listRooms))
                {
                    this.EXAM_ROOM_CODE = string.Join(",", listRooms.Select(o => o.EXECUTE_ROOM_CODE.ToUpper()).ToList());
                    this.EXAM_ROOM_NAME = string.Join(";\r\n", listRooms.Select(o => o.EXECUTE_ROOM_NAME.ToUpper()).ToList());
                }
                #endregion

                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                if (castFilter.CHOOSE_TIME ==true)
                {
                    serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                    serviceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                }
                else
                {
                    serviceReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                    serviceReqFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                    serviceReqFilter.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                }
                serviceReqFilter.EXECUTE_ROOM_IDs = listRooms.Select(o => o.ROOM_ID).ToList();
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                listServiceReqs = new HisServiceReqManager(param).Get(serviceReqFilter);

                var skip = 0;
                while (listServiceReqs.Count - skip > 0)
                {
                    var listIds = listServiceReqs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.IDs = listIds.Select(s => s.TREATMENT_ID).ToList();
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).Get(treatmentFilter));
                }

                listTreatments = listTreatments.GroupBy(o => o.ID).Select(p => p.First()).ToList();

                skip = 0;
                while (listTreatments.Count - skip > 0)
                {
                    var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;


                    HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));

                    HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                    patientFilter.IDs = listIds.Select(s => s.PATIENT_ID).ToList();
                    listPatients.AddRange(new MOS.MANAGER.HisPatient.HisPatientManager(param).Get(patientFilter));
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

                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    long patientTypeIdBhyt = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                    long patientTypeIdVp = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
                    foreach (var sr in listServiceReqs)
                    {
                        var treatment = listTreatments.FirstOrDefault(s => s.ID == sr.TREATMENT_ID);
                        if (treatment != null)
                        {
                            var patient = listPatients.FirstOrDefault(s => s.ID == treatment.PATIENT_ID);
                            if (patient != null)
                            {
                                var rdo = new Mrs00331RDO();
                                rdo.EXECUTE_ROOM_ID = sr.EXECUTE_ROOM_ID;
                                rdo.EXECUTE_ROOM_CODE = (listRooms.FirstOrDefault(o => o.ROOM_ID == sr.EXECUTE_ROOM_ID) ?? new V_HIS_EXECUTE_ROOM()).EXECUTE_ROOM_CODE;
                                rdo.EXECUTE_ROOM_NAME = (listRooms.FirstOrDefault(o => o.ROOM_ID == sr.EXECUTE_ROOM_ID) ?? new V_HIS_EXECUTE_ROOM()).EXECUTE_ROOM_NAME;
                                rdo.TREATMENT_CODE = sr.TDL_TREATMENT_CODE;

                                rdo.TOTAL_EXAM = 1;

                                DateTime now = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(sr.INTRUCTION_TIME) ?? DateTime.Now;
                                int age = now.Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(sr.TDL_PATIENT_DOB) ?? DateTime.Now).Year;
                                if (age == 6 && (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(sr.TDL_PATIENT_DOB) ?? DateTime.Now) > now.AddYears(-age)) age--;

                                if (sr.TDL_HEIN_CARD_NUMBER != null && sr.TDL_HEIN_CARD_NUMBER.Length > 0) rdo.EXAM_HEIN = 1;

                                if ((this.HEIN_PROVINCE_CODE + ",").Contains(patient.PROVINCE_CODE + ","))
                                {
                                    rdo.EXAM_IN_PROVINCE = 1;
                                    if (age < 6) rdo.UNDER_6_IN_PROVINCE = 1;

                                    var patientTypeAlter = listPatientTypeAlters.Where(s => s.LOG_TIME > sr.INTRUCTION_TIME && s.EXECUTE_ROOM_ID == sr.EXECUTE_ROOM_ID
                                        && s.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && s.TREATMENT_ID == sr.TREATMENT_ID).ToList();
                                    if (IsNotNullOrEmpty(patientTypeAlter))
                                        rdo.CLINICAL_IN_PROVINCE = 1;

                                    rdo.DISTRICT_CODE = patient.DISTRICT_CODE;
                                    rdo.DISTRICT_NAME = patient.DISTRICT_NAME;
                                }
                                else
                                {
                                    rdo.EXAM_OUT_PROVINCE = 1;
                                    if (age < 6) rdo.UNDER_6_OUT_PROVINCE = 1;

                                    var patientTypeAlter = listPatientTypeAlters.Where(s => s.LOG_TIME >= sr.INTRUCTION_TIME && s.EXECUTE_ROOM_ID == sr.EXECUTE_ROOM_ID
                                        && s.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM && s.TREATMENT_ID == sr.TREATMENT_ID).ToList();
                                    if (IsNotNullOrEmpty(patientTypeAlter))
                                        rdo.CLINICAL_OUT_PROVINCE = 1;
                                }

                                if (age < 15) rdo.UNDER_15 = 1;
                                if (age < 15 && age >= 6) rdo.OVER_6_AND_UNDER_15 = 1;

                                if (age > 60) rdo.OVER_60 = 1;

                                if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && treatment.END_ROOM_ID == sr.EXECUTE_ROOM_ID)
                                    rdo.TRAN_PATI = 1;

                                if (treatment.TDL_PATIENT_TYPE_ID == patientTypeIdBhyt)
                                    rdo.TOTAL_EXAM_BHYT = 1;

                                if (treatment.TDL_PATIENT_TYPE_ID == patientTypeIdVp)
                                    rdo.TOTAL_EXAM_VP = 1;

                                if (treatment.IS_EMERGENCY == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                    rdo.TOTAL_EXAM_CC = 1;

                                listRdo.Add(rdo);
                            }
                        }
                    }
                    ListRdoDetail = listRdo.Where(o => o.TOTAL_EXAM > 0).ToList();
                    ListDistrict = listRdo.GroupBy(g => g.DISTRICT_CODE).Select(p=>p.First()).ToList();

                    listRdo = listRdo.GroupBy(g => g.EXECUTE_ROOM_ID).Select(s => new Mrs00331RDO
                        {
                            EXECUTE_ROOM_ID = s.First().EXECUTE_ROOM_ID,
                            EXECUTE_ROOM_CODE = s.First().EXECUTE_ROOM_CODE,
                            EXECUTE_ROOM_NAME = s.First().EXECUTE_ROOM_NAME,

                            TOTAL_EXAM = s.Sum(su => su.TOTAL_EXAM),
                            EXAM_IN_PROVINCE = s.Sum(su => su.EXAM_IN_PROVINCE),
                            EXAM_OUT_PROVINCE = s.Sum(su => su.EXAM_OUT_PROVINCE),
                            EXAM_HEIN = s.Sum(su => su.EXAM_HEIN),

                            CLINICAL_IN_PROVINCE = s.Sum(su => su.CLINICAL_IN_PROVINCE),
                            CLINICAL_OUT_PROVINCE = s.Sum(su => su.CLINICAL_OUT_PROVINCE),

                            UNDER_6_IN_PROVINCE = s.Sum(su => su.UNDER_6_IN_PROVINCE),
                            UNDER_6_OUT_PROVINCE = s.Sum(su => su.UNDER_6_OUT_PROVINCE),

                            UNDER_15 = s.Sum(su => su.UNDER_15),
                            OVER_6_AND_UNDER_15 = s.Sum(su => su.OVER_6_AND_UNDER_15),
                            OVER_60 = s.Sum(su => su.OVER_60),

                            TRAN_PATI = s.Sum(su => su.TRAN_PATI)
                        }).ToList();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("BRANCH_NAME", this.BRANCH_NAME);
                dicSingleTag.Add("PARENT_ORGANIZATION_NAME_2", this.PARENT_ORGANIZATION_NAME);
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", this.EXECUTE_DEPARTMENT_NAME);
                dicSingleTag.Add("EXAM_ROOM_NAME", this.EXAM_ROOM_NAME);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.EXECUTE_ROOM_NAME).ToList());
                objectTag.AddObjectData(store, "listDetail", ListRdoDetail.OrderBy(o => o.TREATMENT_CODE).ToList());
                objectTag.AddObjectData(store, "listDistrict", ListDistrict.OrderBy(o => o.DISTRICT_CODE??"Z").ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


    }
}
