using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
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
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisTreatment;


namespace MRS.Processor.Mrs00447
{
    // báo cáo sỏ điện tim / khí dung

    class Mrs00447Processor : AbstractProcessor
    {
        Mrs00447Filter castFilter = null;
        List<Mrs00447RDO> listRdo = new List<Mrs00447RDO>();

        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV_EXT> ListSereServExt = new List<HIS_SERE_SERV_EXT>();
        List<HIS_SERVICE_REQ> listServiceReqs = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        //List<HIS_PATIENT> listPatients = new List<HIS_PATIENT>(); 
        //List<HIS_ICD> listIcds = new List<HIS_ICD>(); 
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>();

        public string TITLE_REPORT = "";

        public Mrs00447Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00447Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00447Filter)this.reportFilter;

                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00447";
                var listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(serviceRetyCatFilter);
                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                serviceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                serviceReqFilter.HAS_EXECUTE = true;
                serviceReqFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                serviceReqFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID;
                serviceReqFilter.REQUEST_ROOM_ID = castFilter.REQUEST_ROOM_ID;
                listServiceReqs.AddRange(new MOS.MANAGER.HisServiceReq.HisServiceReqManager(param).Get(serviceReqFilter));
                if (castFilter.REQUEST_TREATMENT_TYPE_ID != null)
                {
                    listServiceReqs = listServiceReqs.Where(o => castFilter.REQUEST_TREATMENT_TYPE_ID == o.TREATMENT_TYPE_ID).ToList();
                }

                if (castFilter.REQUEST_TREATMENT_TYPE_IDs != null)
                {
                    listServiceReqs = listServiceReqs.Where(o => castFilter.REQUEST_TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID ?? 0)).ToList();
                }

                var treatmentIds = listServiceReqs.Select(o => o.TREATMENT_ID).Distinct().ToList();
                dicPatientTypeAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(treatmentIds).OrderBy(q => q.LOG_TIME).ThenBy(o => o.ID).GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, p => p.ToList());

                List<HIS_SERE_SERV> listSereServAll = new List<HIS_SERE_SERV>();
                if (IsNotNullOrEmpty(listServices))
                {
                    var serviceIds = listServices.Select(o => o.SERVICE_ID).Distinct().ToList();
                    var skip = 0;
                    while (serviceIds.Count - skip > 0)
                    {
                        var listIds = serviceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.SERVICE_IDs = listIds;
                        sereServFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                        sereServFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                        sereServFilter.HAS_EXECUTE = true;
                        var listSereServAllSub = new MOS.MANAGER.HisSereServ.HisSereServManager(param).Get(sereServFilter);
                        if (listSereServAllSub != null)
                        {
                            listSereServAll.AddRange(listSereServAllSub);
                        }
                    }
                    listSereServAll = listSereServAll.Where(o => listServiceReqs.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }

                if (IsNotNullOrEmpty(castFilter.EXECUTE_ROOM_IDs))
                {
                    listSereServAll = listSereServAll.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.TDL_EXECUTE_ROOM_ID)).ToList();
                }

                if (IsNotNullOrEmpty(castFilter.EXE_ROOM_IDs))
                {
                    listSereServAll = listSereServAll.Where(o => castFilter.EXE_ROOM_IDs.Contains(o.TDL_EXECUTE_ROOM_ID)).ToList();
                }

                if (castFilter.IS_ECG == true || castFilter.IS_NEB == true)
                {
                    if (castFilter.IS_ECG == true)
                    {
                        listSereServs.AddRange(listSereServAll.Where(o => listServices.Exists(w => w.SERVICE_ID == o.SERVICE_ID && w.CATEGORY_CODE == "447_DTIM")).ToList());
                        TITLE_REPORT += "ĐIỆN TIM";
                    }
                    else if (castFilter.IS_NEB == true)
                    {
                        listSereServs.AddRange(listSereServAll.Where(o => listServices.Exists(w => w.SERVICE_ID == o.SERVICE_ID && w.CATEGORY_CODE == "447_KDUG")).ToList());
                        TITLE_REPORT += "KHÍ DUNG";
                    }
                }
                else
                {
                    listSereServs = listSereServAll;
                }

                if (IsNotNull(castFilter.TREATMENT_TYPE_ID))
                {
                    listSereServs = listSereServs.Where(o => castFilter.TREATMENT_TYPE_ID == treatmentTypeId(o.TDL_TREATMENT_ID.Value)).ToList();
                }

                //ket qua

                if (IsNotNullOrEmpty(listSereServs))
                {
                    List<long> listSereServId = listSereServs.Select(o => o.ID).ToList();
                    var skip = 0;
                    while (listSereServId.Count - skip > 0)
                    {
                        var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServExtFilterQuery serExtFilter = new HisSereServExtFilterQuery();
                        serExtFilter.SERE_SERV_IDs = listIDs;
                        var hisSereServExts = new HisSereServExtManager(paramGet).Get(serExtFilter);
                        if (IsNotNullOrEmpty(hisSereServExts))
                            ListSereServExt.AddRange(hisSereServExts);
                    }
                }

                //get dịch vụ máy
                GetServiceMachine();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();

                //get hồ sơ điều trị
                GetTreatment();

                //lọc theo đối tượng
                FilterByPaty();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetTreatment()
        {
            if (IsNotNullOrEmpty(listSereServs))
            {
                List<long> treatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID??0).Distinct().ToList();
                var skip = 0;
                while (treatmentIds.Count - skip > 0)
                {
                    var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                    treaFilter.IDs = listIDs;
                    var treatments = new HisTreatmentManager().Get(treaFilter);
                    if (IsNotNullOrEmpty(treatments))
                        ListTreatment.AddRange(treatments);
                }
            }
        }

        private void FilterByPaty()
        {
            if (castFilter.PATIENT_TYPE_IDs != null)
            {
                listSereServs = listSereServs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();

                var serviceReqIds = listSereServs.Select(o => o.SERVICE_REQ_ID ?? 0).ToList();
                listServiceReqs = listServiceReqs.Where(o => serviceReqIds.Contains(o.ID)).ToList();

                var sereServIds = listSereServs.Select(o => o.ID).ToList();
                ListSereServExt = ListSereServExt.Where(o => sereServIds.Contains(o.SERE_SERV_ID)).ToList();
            }
            if (castFilter.TDL_PATIENT_TYPE_IDs != null)
            {
                ListTreatment = ListTreatment.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                var treatmentIds = ListTreatment.Select(o => o.ID).ToList();
                listSereServs = listSereServs.Where(o => treatmentIds.Contains(o.TDL_TREATMENT_ID??0)).ToList();
                listServiceReqs = listServiceReqs.Where(o => treatmentIds.Contains(o.TREATMENT_ID)).ToList();
            }
        }

        private void FilterByMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = dicServiceMachine.Values.SelectMany(p => p.ToList()).Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                listSereServs = listSereServs.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();

                var serviceReqIds = listSereServs.Select(o => o.SERVICE_REQ_ID ?? 0).ToList();
                listServiceReqs = listServiceReqs.Where(o => serviceReqIds.Contains(o.ID)).ToList();

                var sereServIds = listSereServs.Select(o => o.ID).ToList();
                ListSereServExt = ListSereServExt.Where(o => sereServIds.Contains(o.SERE_SERV_ID)).ToList();
            }
            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                ListSereServExt = ListSereServExt.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.MACHINE_ID ?? 0)).ToList();
                var sereServIds = ListSereServExt.Select(o => o.SERE_SERV_ID).ToList();
                listSereServs = listSereServs.Where(o => sereServIds.Contains(o.ID)).ToList();
                var serviceReqIds = listSereServs.Select(o => o.SERVICE_REQ_ID ?? 0).ToList();
                listServiceReqs = listServiceReqs.Where(o => serviceReqIds.Contains(o.ID)).ToList();
            }
        }

        private void GetMachine()
        {
            string query = "select * from his_machine where is_delete=0";
            ListMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MACHINE>(query) ?? new List<HIS_MACHINE>();
        }

        private void GetServiceMachine()
        {
            string query = "select * from his_service_machine where is_delete=0";
            var ListServiceMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_MACHINE>(query);
            if (ListServiceMachine != null && ListServiceMachine.Count > 0)
            {
                dicServiceMachine = ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }

        private long treatmentTypeId(long p)
        {
            return dicPatientTypeAlter.ContainsKey(p) ? dicPatientTypeAlter[p].Last().TREATMENT_TYPE_ID : 0;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                List<V_HIS_ROOM> listRoom = HisRoomCFG.HisRooms ?? new List<V_HIS_ROOM>();
                List<HIS_DEPARTMENT> listDepartment = HisDepartmentCFG.DEPARTMENTs ?? new List<HIS_DEPARTMENT>();
                List<HIS_PATIENT_TYPE> listPatientType = HisPatientTypeCFG.PATIENT_TYPEs ?? new List<HIS_PATIENT_TYPE>();

                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        var serviceReq = listServiceReqs.Where(s => s.ID == sereServ.SERVICE_REQ_ID).ToList();
                        var requestRoom = listRoom.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_ROOM_ID);
                        var requestDepartment = listDepartment.FirstOrDefault(o => o.ID == sereServ.TDL_REQUEST_DEPARTMENT_ID);
                        var patientType = listPatientType.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID);
                        var sse = ListSereServExt.FirstOrDefault(o => o.SERE_SERV_ID == sereServ.ID);
                        var trea = ListTreatment.FirstOrDefault(o => o.ID == sereServ.TDL_TREATMENT_ID);
                        if (!IsNotNullOrEmpty(serviceReq)) continue;
                        var rdo = new Mrs00447RDO();
                        rdo.PATIENT_ID = serviceReq.First().TDL_PATIENT_ID;
                        rdo.PATIENT_CODE = serviceReq.First().TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = serviceReq.First().TDL_PATIENT_NAME;

                        rdo.TREATMENT_ID = serviceReq.First().TREATMENT_ID;
                        rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;

                        rdo.GENDER = serviceReq.First().TDL_PATIENT_GENDER_NAME;
                        rdo.DOB = serviceReq.First().TDL_PATIENT_DOB;

                        if (serviceReq.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE) rdo.DOB_MALE = serviceReq.First().TDL_PATIENT_DOB;
                        else if (serviceReq.First().TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE) rdo.DOB_FEMALE = serviceReq.First().TDL_PATIENT_DOB;
                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;

                        if (IsNotNullOrEmpty(serviceReq))
                        {
                            rdo.ADDRESS = serviceReq.First().TDL_PATIENT_ADDRESS;
                            rdo.ICD_CODE = serviceReq.First().ICD_CODE;
                            rdo.ICD_NAME = serviceReq.First().ICD_NAME;
                            rdo.ICD_NAME = serviceReq.First().ICD_NAME;
                            rdo.ICD_TEXT = serviceReq.First().ICD_TEXT;

                            rdo.START_TIME = serviceReq.First().START_TIME;
                        }

                        rdo.INTRUCTION_TIME = serviceReq.First().INTRUCTION_TIME;
                        rdo.FINISH_TIME = serviceReq.First().FINISH_TIME;

                        rdo.REQUEST_USERNAME = serviceReq.First().REQUEST_USERNAME;

                        rdo.EXECUTE_LOGINNAME = serviceReq.First().EXECUTE_LOGINNAME;

                        rdo.EXECUTE_USERNAME = serviceReq.First().EXECUTE_USERNAME;
                        if (sse != null)
                        {
                            rdo.DESCRIPTION = sse.DESCRIPTION;
                            rdo.CONCLUDE = sse.CONCLUDE;
                            var machineExt = ListMachine.Where(p => p.ID == sse.MACHINE_ID).ToList();
                            if (machineExt.Count > 0)
                            {
                                rdo.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                                rdo.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                            }
                        }
                        if (requestRoom != null)
                        {
                            rdo.REQUEST_ROOM_NAME = requestRoom.ROOM_NAME;
                        }
                        if (requestDepartment != null)
                        {
                            rdo.REQUEST_DEPARTMENT_NAME = requestDepartment.DEPARTMENT_NAME;
                        }
                        if (patientType != null)
                        {
                            rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                        }
                        if (trea != null)
                        {
                            var tdlPatientType = listPatientType.FirstOrDefault(o => o.ID == trea.TDL_PATIENT_TYPE_ID);
                            if (tdlPatientType != null)
                            {
                                rdo.TDL_PATIENT_TYPE_CODE = tdlPatientType.PATIENT_TYPE_CODE;
                                rdo.TDL_PATIENT_TYPE_NAME = tdlPatientType.PATIENT_TYPE_NAME;
                            }
                        }
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        var serviceMachine = dicServiceMachine.ContainsKey(sereServ.SERVICE_ID) ? dicServiceMachine[sereServ.SERVICE_ID] : null;
                        if (serviceMachine != null && serviceMachine.Count > 0)
                        {
                            var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                            if (machine.Count > 0)
                            {
                                rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                                rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                            }
                        }

                        listRdo.Add(rdo);
                    }
                }

                //listRdo.Distinct(); 
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

                dicSingleTag.Add("TITLE_REPORT", this.TITLE_REPORT);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.INTRUCTION_TIME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
                if (castFilter.EXECUTE_MACHINE_IDs != null)
                {
                    var machine = this.ListMachine.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("EXECUTE_MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
                }
                if (castFilter.MACHINE_IDs != null)
                {
                    var machine = this.ListMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
                }
                if (castFilter.PATIENT_TYPE_IDs != null)
                {
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
                }
                if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                {
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
