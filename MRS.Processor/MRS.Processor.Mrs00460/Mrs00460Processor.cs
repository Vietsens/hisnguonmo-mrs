using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using HIS.Treatment.DateTime; 
 

namespace MRS.Processor.Mrs00460
{
    //báo cáo hoạt động khám chữa bệnh

    class Mrs00460Processor : AbstractProcessor
    {
        Mrs00460Filter castFilter = null; 
        List<Mrs00460RDO> listRdo = new List<Mrs00460RDO>(); 
        List<Mrs00460RDO> listRdoGroup = new List<Mrs00460RDO>(); 

        List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>(); 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV> listSereServOthers = new List<V_HIS_SERE_SERV>(); 
        List<HIS_PATIENT> listPatients = new List<HIS_PATIENT>(); 
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TREATMENT> listEmergencys = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 

        public decimal VOUND_CARE = 0; 
        public decimal STITCH_REMOVAL = 0; 
        public decimal FLUID_TRANFUSION = 0; 
        public decimal VACCINATION = 0; 
        public decimal TUBING = 0; 
        public decimal ASPIRATION = 0; 
        public decimal FNA = 0; 
        public decimal MINOR_SURGERY = 0; 
        public decimal JOIN_INJECTION = 0; 
        public decimal INTRUSMENTAL_OBORTION = 0; 
        public decimal OTHER = 0; 
        public decimal ETN = 0; 

        public List<long> VOUND_CAREs = new List<long>(); 
        public List<long> STITCH_REMOVALs = new List<long>(); 
        public List<long> FLUID_TRANFUSIONs = new List<long>(); 
        public List<long> VACCINATIONs = new List<long>(); 
        public List<long> TUBINGs = new List<long>(); 
        public List<long> ASPIRATIONs = new List<long>(); 
        public List<long> FNAs = new List<long>(); 
        public List<long> MINOR_SURGERYs = new List<long>(); 
        public List<long> JOIN_INJECTIONs = new List<long>(); 
        public List<long> INTRUSMENTAL_OBORTIONs = new List<long>(); 
        public List<long> OTHERs = new List<long>(); 
        public List<long> ETNs = new List<long>(); 

        public Mrs00460Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00460Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00460Filter)this.reportFilter; 

                listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(new HisBranchFilterQuery()); 

                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery(); 
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00460"; 
                var listServiceRetCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(retyCastFilter); 

                VOUND_CAREs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "VOUND_CARE").Select(s => s.SERVICE_ID).ToList(); 
                STITCH_REMOVALs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "STITCH_REM").Select(s => s.SERVICE_ID).ToList(); 
                FLUID_TRANFUSIONs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "FLUID_TRAN").Select(s => s.SERVICE_ID).ToList(); 
                VACCINATIONs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "VACCIN").Select(s => s.SERVICE_ID).ToList(); 
                TUBINGs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "TUBING").Select(s => s.SERVICE_ID).ToList(); 
                ASPIRATIONs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "ASPIRATION").Select(s => s.SERVICE_ID).ToList(); 
                FNAs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "FNA").Select(s => s.SERVICE_ID).ToList(); 
                MINOR_SURGERYs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "MINOR_SURG").Select(s => s.SERVICE_ID).ToList(); 
                JOIN_INJECTIONs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "JOIN_INJEC").Select(s => s.SERVICE_ID).ToList(); 
                INTRUSMENTAL_OBORTIONs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "INTRUSMENT").Select(s => s.SERVICE_ID).ToList(); 
                OTHERs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "OTHER").Select(s => s.SERVICE_ID).ToList(); 
                ETNs = listServiceRetCats.Where(w => w.CATEGORY_CODE == "ETN").Select(s => s.SERVICE_ID).ToList(); 

                HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                sereServViewFilter.INTRUCTION_DATE_FROM = castFilter.TIME_FROM; 
                sereServViewFilter.INTRUCTION_DATE_TO = castFilter.TIME_TO; 
                sereServViewFilter.EXECUTE_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs; 
                sereServViewFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH; 
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter); 

                var listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList(); 
                var listPatientIds = listSereServs.Select(s => s.PARENT_ID.GetValueOrDefault(0)).Distinct().ToList(); 
                var listRoomIds = listSereServs.Select(s => s.TDL_EXECUTE_ROOM_ID).ToList(); 

                var skip = 0; 
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                    treatmentViewFilter.IDs = listIDs; 
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter)); 


                    listEmergencys = listTreatments.Where(o => o.IS_EMERGENCY != null).ToList(); 
                    HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterViewFilter.TREATMENT_IDs = listIDs; 
                    listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter)); 
                }

                skip = 0; 
                while (listPatientIds.Count - skip > 0)
                {
                    var listIDs = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientFilterQuery patientFilter = new HisPatientFilterQuery(); 
                    patientFilter.IDs = listIDs; 
                    listPatients.AddRange(new MOS.MANAGER.HisPatient.HisPatientManager(param).Get(patientFilter)); 
                }

                skip = 0; 
                while (listRoomIds.Count - skip > 0)
                {
                    var listIDs = listRoomIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServViewFilterQuery sereServViewFilter2 = new HisSereServViewFilterQuery(); 
                    sereServViewFilter2.REQUEST_ROOM_IDs = listIDs;
                    listSereServOthers.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter2)); 
                }

                listSereServOthers = listSereServOthers.Where(w => w.IS_DELETE != 1 && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 
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

                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        var rdo = new Mrs00460RDO(); 
                        rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID??0; 

                        rdo.DEPARTMENT_ID = sereServ.TDL_EXECUTE_DEPARTMENT_ID; 
                        rdo.DEPARTMENT_NAME = sereServ.EXECUTE_DEPARTMENT_NAME; 

                        rdo.ROOM_ID = sereServ.TDL_EXECUTE_ROOM_ID; 
                        rdo.ROOM_NAME = sereServ.EXECUTE_ROOM_NAME; 

                        rdo.EXAM_FIRST = 1; 
                        rdo.EXAM_REEX = 0; 
                        var listSereServ = listSereServs.Where(w => w.TDL_TREATMENT_ID == sereServ.TDL_TREATMENT_ID && w.ID != sereServ.ID).ToList(); 
                        if (IsNotNullOrEmpty(listSereServ))
                        {
                            foreach (var first in listSereServ)
                            {
                                if (sereServ.EXECUTE_TIME < first.EXECUTE_TIME)
                                {
                                    rdo.EXAM_FIRST = 0; 
                                    rdo.EXAM_REEX = 1; 
                                }
                            }
                        }

                        rdo.PROVINCE = 1; 
                        var patient = listPatients.Where(w => w.ID == sereServ.TDL_PATIENT_ID).ToList(); 
                        if (IsNotNullOrEmpty(patient))
                        {
                            if (patient.First().PROVINCE_CODE == listBranchs.First().HEIN_PROVINCE_CODE)
                                rdo.PROVINCE = 0; 
                        }

                        var patientTypeAlter = listPatientTypeAlters.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID 
                            && w.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList(); 
                        if (IsNotNullOrEmpty(patientTypeAlter))
                        {
                            if (patientTypeAlter.OrderBy(o => o.LOG_TIME).First().EXECUTE_ROOM_ID == sereServ.TDL_EXECUTE_ROOM_ID)
                                if (!IsNotNullOrEmpty(listRdo.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID && w.HOSPITALI_ZED == 1).ToList()))
                                    rdo.HOSPITALI_ZED = 1; 
                        }

                        var emergency = listEmergencys.Where(w => w.ID == sereServ.TDL_TREATMENT_ID).ToList(); 
                        if (IsNotNullOrEmpty(emergency))
                            rdo.TRAN_TO_ED = 1; 

                        var treatment = listTreatments.Where(w => w.ID == sereServ.TDL_TREATMENT_ID).ToList(); 
                        if (IsNotNullOrEmpty(treatment))
                        {
                            if (treatment.First().TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN
                                && treatment.First().END_ROOM_ID == sereServ.TDL_EXECUTE_ROOM_ID)
                                if (!IsNotNullOrEmpty(listRdo.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID && w.TRANS_HOS == 1).ToList()))
                                    rdo.TRANS_HOS = 1; 
                        }

                        listRdo.Add(rdo); 
                    }

                    listRdo = listRdo.GroupBy(g => new { g.DEPARTMENT_ID, g.ROOM_ID }).Select(s => new Mrs00460RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,

                        ROOM_ID = s.First().ROOM_ID,
                        ROOM_NAME = s.First().ROOM_NAME,

                        EXAM_FIRST = s.Sum(su => su.EXAM_FIRST),
                        EXAM_REEX = s.Sum(su => su.EXAM_REEX),

                        PROVINCE = s.Sum(su => su.PROVINCE),
                        HOSPITALI_ZED = s.Sum(su => su.HOSPITALI_ZED),
                        TRAN_TO_ED = s.Sum(su => su.TRAN_TO_ED),
                        TRANS_HOS = s.Sum(su => su.TRANS_HOS)
                    }).ToList(); 

                    listRdoGroup = listRdo.GroupBy(g => g.DEPARTMENT_ID).Select(s => new Mrs00460RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                    }).ToList(); 

                    // INDAY TREATMENT
                    VOUND_CARE = listSereServOthers.Where(w => VOUND_CAREs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    STITCH_REMOVAL = listSereServOthers.Where(w => STITCH_REMOVALs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    FLUID_TRANFUSION = listSereServOthers.Where(w => FLUID_TRANFUSIONs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    VACCINATION = listSereServOthers.Where(w => VACCINATIONs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    TUBING = listSereServOthers.Where(w => TUBINGs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    ASPIRATION = listSereServOthers.Where(w => ASPIRATIONs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    FNA = listSereServOthers.Where(w => FNAs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    MINOR_SURGERY = listSereServOthers.Where(w => MINOR_SURGERYs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    JOIN_INJECTION = listSereServOthers.Where(w => JOIN_INJECTIONs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    INTRUSMENTAL_OBORTION = listSereServOthers.Where(w => INTRUSMENTAL_OBORTIONs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    OTHER = listSereServOthers.Where(w => OTHERs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
                    ETN = listSereServOthers.Where(w => ETNs.Contains(w.SERVICE_ID)).Sum(s => s.AMOUNT); 
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

                dicSingleTag.Add("VOUND_CARE", VOUND_CARE); 
                dicSingleTag.Add("STITCH_REMOVAL", STITCH_REMOVAL); 
                dicSingleTag.Add("FLUID_TRANFUSION", FLUID_TRANFUSION); 
                dicSingleTag.Add("VACCINATION", VACCINATION); 
                dicSingleTag.Add("TUBING", TUBING); 
                dicSingleTag.Add("ASPIRATION", ASPIRATION); 
                dicSingleTag.Add("FNA", FNA); 
                dicSingleTag.Add("MINOR_SURGERY", MINOR_SURGERY); 
                dicSingleTag.Add("JOIN_INJECTION", JOIN_INJECTION); 
                dicSingleTag.Add("INTRUSMENTAL_OBORTION", INTRUSMENTAL_OBORTION); 
                dicSingleTag.Add("OTHER", OTHER); 
                dicSingleTag.Add("ETN", ETN); 

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                departmentFilter.IDs = castFilter.DEPARTMENT_IDs; 
                var departments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter); 
                if (IsNotNullOrEmpty(departments))
                    dicSingleTag.Add("DEPARTMENT_NAME", String.Join(", ", departments.Select(s => s.DEPARTMENT_NAME).ToArray())); 

                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "RdoGroup", listRdo.OrderBy(o => o.DEPARTMENT_NAME).ToList()); 
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.ROOM_NAME).ToList()); 
                objectTag.AddRelationship(store, "RdoGroup", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
