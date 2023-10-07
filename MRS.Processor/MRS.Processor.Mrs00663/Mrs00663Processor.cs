using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

namespace MRS.Processor.Mrs00663
{
    class Mrs00663Processor : AbstractProcessor
    {
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<Mrs00663RDO> listTreatmentService = new List<Mrs00663RDO>();
        CommonParam paramGet = new CommonParam();
        List<Mrs00663RDO> ListRdo = new List<Mrs00663RDO>();
        List<Mrs00663RDO> RequestLoginname = new List<Mrs00663RDO>();
        List<Mrs00663RDO> RequestDepartment = new List<Mrs00663RDO>();
        List<Mrs00663RDO> listParentService = new List<Mrs00663RDO>();
        public Mrs00663Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00663Filter);
        }


        protected override bool GetData()///
        {
            var filter = ((Mrs00663Filter)reportFilter);
            bool result = true;
            try
            {

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery()
                {
                    IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                };
                listService = new HisServiceManager().Get(serviceFilter);

                //Yeu cau
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                if (filter.CHOOSE_TIME != false)
                {
                    reqFilter.INTRUCTION_TIME_FROM = filter.TIME_FROM;
                    reqFilter.INTRUCTION_TIME_TO = filter.TIME_TO;
                }
                else
                {
                    reqFilter.FINISH_TIME_FROM = filter.TIME_FROM;
                    reqFilter.FINISH_TIME_TO = filter.TIME_TO;
                }
                    reqFilter.EXECUTE_ROOM_IDs = filter.EXECUTE_ROOM_IDs;
                reqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__GPBL,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN };
                reqFilter.REQUEST_ROOM_IDs = filter.REQUEST_ROOM_IDs;
                reqFilter.HAS_EXECUTE = true;
                listServiceReq = new HisServiceReqManager().Get(reqFilter);
                if (IsNotNullOrEmpty(filter.EXECUTE_LOGINNAMEs))
                {
                    listServiceReq = listServiceReq.Where(o => filter.EXECUTE_LOGINNAMEs.Contains(o.REQUEST_LOGINNAME)).ToList();
                }
                if (IsNotNullOrEmpty(filter.REQUEST_LOGINNAMEs))
                {
                    listServiceReq = listServiceReq.Where(o => filter.REQUEST_LOGINNAMEs.Contains(o.REQUEST_LOGINNAME)).ToList();
                }
                //YC-DV
                if (IsNotNullOrEmpty(listServiceReq))
                {
                    var skip = 0;
                    while (listServiceReq.Count - skip > 0)
                    {
                        var limit = listServiceReq.Select(o => o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSereServ = new HisSereServFilterQuery()
                        {
                            SERVICE_REQ_IDs = limit,
                            SERVICE_IDs = filter.SERVICE_IDs,
                            TDL_SERVICE_TYPE_IDs = filter.SERVICE_TYPE_IDs,
                            HAS_EXECUTE=true
                        };
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSereServ);
                        listSereServ.AddRange(listSereServSub);
                    }
                }
                
                if (filter.EXACT_PARENT_SERVICE_IDs != null && filter.EXACT_PARENT_SERVICE_IDs.Count > 0)
                {
                    listService=listService.Where(o=>filter.EXACT_PARENT_SERVICE_IDs.Contains(o.PARENT_ID??0)).ToList();
                    listSereServ = listSereServ.Where(o => listService.Exists(p => p.ID == o.SERVICE_ID)).ToList();
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
            var result = true;
            try
            {

                ListRdo.Clear();

                if (IsNotNullOrEmpty(listSereServ))
                {
                    var group = listSereServ.OrderBy(o => o.TDL_TREATMENT_ID).GroupBy(p => new { p.SERVICE_ID, p.VIR_PRICE,p.TDL_REQUEST_LOGINNAME,p.TDL_REQUEST_DEPARTMENT_ID }).ToList();

                    foreach (var item in group)
                    {
                        HIS_SERE_SERV sereServ = item.First();
                        var req = listServiceReq.FirstOrDefault(o => o.ID == sereServ.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        Mrs00663RDO rdo = new Mrs00663RDO();
                        rdo.PATIENT_CODE = req.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = req.TDL_PATIENT_NAME;
                        rdo.DOB = req.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.ICD_NAME = req.ICD_NAME;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.ICD_TEXT = req.ICD_TEXT;
                        rdo.REQ_USERNAME = req.REQUEST_USERNAME;
                        rdo.REQ_LOGINNAME = req.REQUEST_LOGINNAME;
                        rdo.EXECUTE_USERNAME = req.EXECUTE_USERNAME;
                        rdo.EXECUTE_LOGINNAME = req.EXECUTE_LOGINNAME;
                        rdo.INSTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(req.INTRUCTION_TIME);
                        rdo.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(req.FINISH_TIME??0);
                        rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o=>o.ID==req.EXECUTE_ROOM_ID)??new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == req.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == req.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQUEST_DEPARTMENT_ID = req.REQUEST_DEPARTMENT_ID;
                        rdo.SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                        rdo.WORK_PLACE_NAME = req.TDL_PATIENT_WORK_PLACE_NAME;
                        rdo.ADDRESS = req.TDL_PATIENT_ADDRESS;
                        rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServ.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;

                        rdo.PATIENT_TYPE_ID = sereServ.PATIENT_TYPE_ID;

                        rdo.AMOUNT = item.ToList().Sum(s=>s.AMOUNT);
                        rdo.VIR_PRICE = sereServ.VIR_PRICE ?? 0;
                        rdo.VIR_TOTAL_PRICE = item.ToList().Sum(s => s.VIR_TOTAL_PRICE??0);


                        var Service = listService.FirstOrDefault(o => o.ID == sereServ.SERVICE_ID);
                        if (Service != null)
                        {
                            var ParentService = listService.FirstOrDefault(o => o.ID == Service.PARENT_ID);
                            if (ParentService != null)
                            {
                                rdo.PARENT_SERVICE_CODE = ParentService.SERVICE_CODE;
                                rdo.PARENT_SERVICE_NAME = ParentService.SERVICE_NAME;
                            }
                            else
                            {
                                var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == Service.SERVICE_TYPE_ID);
                                if (serviceType != null)
                                {
                                    rdo.PARENT_SERVICE_CODE = serviceType.SERVICE_TYPE_CODE;
                                    rdo.PARENT_SERVICE_NAME = serviceType.SERVICE_TYPE_NAME;
                                }
                            }
                        }
                       
                        ListRdo.Add(rdo);

                    }
                }
                RequestLoginname = ListRdo.GroupBy(o => new { o.REQ_USERNAME, o.REQUEST_DEPARTMENT_ID, o.PARENT_SERVICE_CODE }).Select(p => p.First()).ToList();
                RequestDepartment = RequestLoginname.GroupBy(o => new { o.REQUEST_DEPARTMENT_ID, o.PARENT_SERVICE_CODE }).Select(p => p.First()).ToList();

                listParentService = RequestDepartment.GroupBy(o => o.PARENT_SERVICE_CODE).Select(p => p.First()).ToList();

                result = true;
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


            if (((Mrs00663Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00663Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00663Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00663Filter)reportFilter).TIME_TO));
            }
            objectTag.AddObjectData(store, "Report", ListRdo);

            objectTag.AddObjectData(store, "RequestLoginname", RequestLoginname);
            objectTag.AddRelationship(store, "RequestLoginname", "Report", new string[] { "REQ_LOGINNAME", "REQUEST_DEPARTMENT_ID", "PARENT_SERVICE_CODE" }, new string[] { "REQ_LOGINNAME", "REQUEST_DEPARTMENT_ID", "PARENT_SERVICE_CODE" });
            objectTag.AddObjectData(store, "RequestDepartment", RequestDepartment);
            objectTag.AddRelationship(store, "RequestDepartment", "RequestLoginname", new string[] { "REQUEST_DEPARTMENT_ID", "PARENT_SERVICE_CODE" }, new string[] { "REQUEST_DEPARTMENT_ID", "PARENT_SERVICE_CODE" });
            objectTag.AddObjectData(store, "ParentService", listParentService);
            objectTag.AddRelationship(store, "ParentService", "RequestDepartment", "PARENT_SERVICE_CODE", "PARENT_SERVICE_CODE");
        }



    }

}
