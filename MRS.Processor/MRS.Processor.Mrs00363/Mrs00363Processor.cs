using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00363
{
    class Mrs00363Processor : AbstractProcessor
    {
        Mrs00363Filter castFilter = null;
        List<Mrs00363RDO> ListRdo = new List<Mrs00363RDO>();
        List<Mrs00363RDO> ListRdoDepartments = new List<Mrs00363RDO>();
        List<Mrs00363RDO> ListRdoRooms = new List<Mrs00363RDO>();

        List<Mrs00363Execute> listMrsExecutes = new List<Mrs00363Execute>();

        List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>();
        List<V_HIS_SERE_SERV> listSereServMediMates = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();

        public Mrs00363Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00363Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00363Filter)this.reportFilter;
                if (!IsNotNullOrEmpty(castFilter.ROOM_IDs))
                    throw new Exception("Khong co phong yeu cau");

                foreach (var item in castFilter.ROOM_IDs)
                {
                    HisServiceReqViewFilterQuery ServiceReqViewFilter = new HisServiceReqViewFilterQuery();
                    ServiceReqViewFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                    ServiceReqViewFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                    ServiceReqViewFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    ServiceReqViewFilter.REQUEST_ROOM_ID = item;
                    var serviceReq = new HisServiceReqManager(param).GetView(ServiceReqViewFilter);
                    if (IsNotNull(serviceReq))
                    {
                        listServiceReqs.AddRange(serviceReq);
                    }
                }

                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    var skip = 0;
                    while (listServiceReqs.Count - skip > 0)
                    {
                        var listIDs = listServiceReqs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var treatmentViewFilter = new HisTreatmentViewFilterQuery()
                        {
                            IDs = listIDs.Select(s => s.TREATMENT_ID).ToList()
                        };
                        var listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter);
                        listTreatments.AddRange(listTreatment);
                    }

                    skip = 0;
                    while (listTreatments.Count - skip > 0)
                    {
                        var listIDs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServViewFilter2 = new HisSereServViewFilterQuery();
                        sereServViewFilter2.REQUEST_ROOM_IDs = castFilter.ROOM_IDs;
                        sereServViewFilter2.TREATMENT_IDs = listIDs.Select(s => s.ID).ToList();
                        //sereServViewFilter2.SERVICE_TYPE_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM, IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM };
                        var listSereServMediMate = new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter2);
                        if (IsNotNullOrEmpty(listSereServMediMate))
                        {
                            listSereServMediMate = listSereServMediMate.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM).ToList();
                            listSereServMediMates.AddRange(listSereServMediMate);
                        }
                    }
                }

                result = true;
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
                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    foreach (var sereServ in listServiceReqs)
                    {
                        var treatment = listTreatments.Where(s => s.ID == sereServ.TREATMENT_ID).ToList();
                        var sereServMediMate = listSereServMediMates.Where(s => s.TDL_TREATMENT_ID == sereServ.TREATMENT_ID && s.TDL_REQUEST_ROOM_ID == sereServ.EXECUTE_ROOM_ID && s.TDL_REQUEST_LOGINNAME == sereServ.EXECUTE_LOGINNAME).ToList();

                        Mrs00363Execute execute = new Mrs00363Execute();
                        execute.LOGINNAME = sereServ.EXECUTE_LOGINNAME;
                        execute.EXECUTE_NAME = sereServ.EXECUTE_USERNAME;

                        execute.DEPARTMENT_ID = sereServ.EXECUTE_DEPARTMENT_ID;
                        execute.DEPARTMENT_NAME = sereServ.EXECUTE_DEPARTMENT_NAME;
                        execute.ROOM_ID = sereServ.EXECUTE_ROOM_ID;
                        execute.ROOM_NAME = sereServ.EXECUTE_ROOM_NAME;

                        if (sereServMediMate != null && sereServMediMate.Count > 0)
                            execute.IS_PRESCRIPTION = 1;

                        if (treatment.First().END_LOGINNAME == sereServ.EXECUTE_LOGINNAME && treatment.First().END_ROOM_ID == sereServ.EXECUTE_ROOM_ID && treatment.First().TREATMENT_END_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                        {
                            execute.IS_TRAN_PATI = 1;
                        }
                        else if (treatment.First().CLINICAL_IN_TIME != null && treatment.First().END_ROOM_ID != sereServ.EXECUTE_ROOM_ID)
                        {
                            HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                            patientTypeAlterViewFilter.TREATMENT_ID = treatment.First().ID;
                            patientTypeAlterViewFilter.TREATMENT_TYPE_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU };
                            var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter).OrderBy(s => s.LOG_TIME).ToList();
                            if (IsNotNullOrEmpty(listPatientTypeAlter))
                            {
                                if (listPatientTypeAlter.First().CREATOR == sereServ.EXECUTE_LOGINNAME)
                                    execute.IS_HOSPITALIZE = 1;
                            }
                        }

                        listMrsExecutes.Add(execute);
                    }

                    var listMrsExecuteGroupByDepartmentIds = listMrsExecutes.GroupBy(s => s.DEPARTMENT_ID);
                    foreach (var listMrsExecuteGroupByDepartmentId in listMrsExecuteGroupByDepartmentIds)
                    {
                        var listExecuteGroupByRoomIds = listMrsExecuteGroupByDepartmentId.GroupBy(s => s.ROOM_ID);
                        foreach (var listExecuteGroupByRoomId in listExecuteGroupByRoomIds)
                        {
                            var i = 0;
                            var listExecuteGroupByLoginnames = listExecuteGroupByRoomId.GroupBy(s => s.LOGINNAME);
                            foreach (var listExecuteGroupByLoginname in listExecuteGroupByLoginnames)
                            {
                                i++;
                                var rdo = new Mrs00363RDO();
                                rdo.NUMBER = i;
                                rdo.DEPARTMENT_ID = listExecuteGroupByLoginname.First().DEPARTMENT_ID;
                                rdo.DEPARTMENT_NAME = listExecuteGroupByLoginname.First().DEPARTMENT_NAME;

                                rdo.ROOM_ID = listExecuteGroupByLoginname.First().ROOM_ID;
                                rdo.ROOM_NAME = listExecuteGroupByLoginname.First().ROOM_NAME;

                                rdo.LOGINNAME = listExecuteGroupByLoginname.First().LOGINNAME;
                                rdo.EXECUTE_NAME = listExecuteGroupByLoginname.First().EXECUTE_NAME;

                                rdo.END_OF_EXAM_AMOUNT = listExecuteGroupByLoginname.Count();
                                rdo.HOSPITALIZE_AMOUNT = listExecuteGroupByLoginname.Sum(s => s.IS_HOSPITALIZE);
                                rdo.TRAN_PATI_AMOUNT = listExecuteGroupByLoginname.Sum(s => s.IS_TRAN_PATI);

                                rdo.PRESCRIPTION_AMOUNT = listExecuteGroupByLoginname.Sum(s => s.IS_PRESCRIPTION);

                                ListRdo.Add(rdo);
                            }
                        }
                    }

                    ListRdoDepartments = ListRdo.GroupBy(g => g.DEPARTMENT_ID).Select(s => new Mrs00363RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME
                    }).ToList();

                    ListRdoRooms = ListRdo.GroupBy(g => g.ROOM_ID).Select(s => new Mrs00363RDO
                        {
                            DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                            ROOM_ID = s.First().ROOM_ID,
                            ROOM_NAME = s.First().ROOM_NAME
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
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Department", ListRdoDepartments);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Room", ListRdoRooms);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Department", "Room", "DEPARTMENT_ID", "DEPARTMENT_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Room", "Rdo", "ROOM_ID", "ROOM_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
