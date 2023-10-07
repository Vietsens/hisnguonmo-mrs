using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisPatient;


namespace MRS.Processor.Mrs00173
{
    internal class Mrs00173Processor : AbstractProcessor
    {
        List<HIS_EKIP_USER> listEkip = new List<HIS_EKIP_USER>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        List<VSarReportMrs00173RDO> ListRdo = new List<VSarReportMrs00173RDO>();
        List<VSarReportMrs00173RDO> ListUserDepa = new List<VSarReportMrs00173RDO>();
        List<VSarReportMrs00173RDO> ListUserDepaTreaSv = new List<VSarReportMrs00173RDO>();
        List<VSarReportMrs00173RDO> ListPatient = new List<VSarReportMrs00173RDO>();
        List<VSarReportMrs00173RDO> ListUser = new List<VSarReportMrs00173RDO>();
        List<HIS_EKIP_USER> listHisEkipUser = new List<HIS_EKIP_USER>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();  // Gọi API của bảng sere_serv
        List<HIS_SERE_SERV_EXT> listHisSereServExt = new List<HIS_SERE_SERV_EXT>();  // Gọi API của bảng sere_serv_ext
        Dictionary<string, decimal> DicPtGroupPaty = new Dictionary<string, decimal>();
        Dictionary<string, decimal> DicTtGroupPaty = new Dictionary<string, decimal>();
        List<PTTT_DETAIL> ListPtttDetail = new List<PTTT_DETAIL>();
        Dictionary<string, decimal> DIC_GROUP_TOTAL_PRICE = new Dictionary<string, decimal>();
        List<HIS_PATIENT> listHisPatient = new List<HIS_PATIENT>();
        Mrs00173Filter CastFilter;

        public Mrs00173Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00173Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00173Filter)this.reportFilter;
                listHisService = new HisServiceManager().Get(new HisServiceFilterQuery());
                string query = "select * from his_ekip_user";
                listEkip = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EKIP_USER>(query);

                var metyFilterServiceReq = new HisServiceReqFilterQuery //(gán filter vào báo cáo)
                {
                    EXECUTE_ROOM_IDs = CastFilter.EXECUTE_ROOM_IDs,
                    EXECUTE_DEPARTMENT_ID = CastFilter.EXECUTE_DEPARTMENT_ID,
                    CREATE_TIME_FROM = CastFilter.DATE_FROM,
                    CREATE_TIME_TO = CastFilter.DATE_TO,
                    INTRUCTION_TIME_FROM = CastFilter.INTRUCTION_TIME_FROM,
                    INTRUCTION_TIME_TO = CastFilter.INTRUCTION_TIME_TO,
                    FINISH_TIME_FROM = CastFilter.FINISH_TIME_FROM,
                    FINISH_TIME_TO = CastFilter.FINISH_TIME_TO,
                    SERVICE_REQ_TYPE_IDs = new List<long>(){IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT}
                };
                listHisServiceReq = new HisServiceReqManager(paramGet).Get(metyFilterServiceReq);  // lấy dữ liệu từ servicereq với filter tương ứng 173

                listHisSereServ = new ManagerSql().GetSereServ(CastFilter);
                listHisSereServExt = new ManagerSql().GetSereServExt(CastFilter);

                listHisSereServ = listHisSereServ.Where(o => listHisServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                DicPtGroupPaty = listHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).GroupBy(p => PtttGroupPatyCode(p.SERVICE_ID, p.PATIENT_TYPE_ID)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                DicTtGroupPaty = listHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).GroupBy(p => PtttGroupPatyCode(p.SERVICE_ID, p.PATIENT_TYPE_ID)).ToDictionary(q => q.Key, q => q.Sum(s => s.AMOUNT));
                var ekipIds = listHisSereServ.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(ekipIds))
                {
                    var skip = 0;
                    while (ekipIds.Count - skip > 0)
                    {
                        var listIds = ekipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var HisEkipUserfilter = new HisEkipUserFilterQuery
                        {
                            EKIP_IDs = listIds,// hiển thị số dòng trên 1 trang = 100 (biến: MAX_REQUEST_LENGTH_PARAM)
                        };
                        var listEkipUserSub = new HisEkipUserManager(paramGet).Get(HisEkipUserfilter);
                        listHisEkipUser.AddRange(listEkipUserSub);
                    }
                }

                var PatientIds = listHisSereServ.Select(o => o.TDL_PATIENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(PatientIds))
                {
                    var skip = 0;
                    while (PatientIds.Count - skip > 0)
                    {
                        var listIds = PatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientfilter = new HisPatientFilterQuery();
                        patientfilter.IDs = listIds;

                        var listPatient = new HisPatientManager(paramGet).Get(patientfilter);
                        listHisPatient.AddRange(listPatient);
                    }
                }

                var ServiceIds = listHisSereServ.Select(o => o.SERVICE_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(PatientIds))
                {
                    var skip = 0;
                    while (ServiceIds.Count - skip > 0)
                    {
                        var listIds = ServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisServiceFilterQuery servicefilter = new HisServiceFilterQuery();
                        servicefilter.IDs = listIds;

                        var listservice = new HisServiceManager(paramGet).Get(servicefilter);
                        listHisService.AddRange(listservice);
                    }
                }

                
                ProcessFilterData(listHisSereServ);
                ListPtttDetail = (from r in listHisSereServ select new PTTT_DETAIL(r, listHisServiceReq.FirstOrDefault(o => o.ID == r.SERVICE_REQ_ID), listHisSereServExt.FirstOrDefault(o => o.SERE_SERV_ID == r.ID))).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private string PtttGroupPatyCode(long serviceId, long patientTypeId)
        {
            try
            {

                var service = listHisService.FirstOrDefault(o => o.ID == serviceId) ?? new HIS_SERVICE();
                var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP();
                return (ptttGroup.PTTT_GROUP_CODE ?? "")+"_"+((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID==patientTypeId)??new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE??"");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.INTRUCTION_TIME_FROM ?? CastFilter.DATE_FROM ?? CastFilter.FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.INTRUCTION_TIME_TO ?? CastFilter.DATE_TO ?? CastFilter.FINISH_TIME_TO ?? 0));
            dicSingleTag.Add("DIC_PT_GROUP_PATY", DicPtGroupPaty);
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == CastFilter.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("DIC_TT_GROUP_PATY", DicTtGroupPaty);
            dicSingleTag.Add("DIC_GROUP_TOTAL_PRICE", DIC_GROUP_TOTAL_PRICE);
            objectTag.AddObjectData(store, "Report", ListUser);
            objectTag.AddObjectData(store, "UserDepa", ListUserDepa);
            objectTag.AddObjectData(store, "Depa", ListUserDepa.GroupBy(o => o.DEPARTMENT_CODE).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Depa", "UserDepa", "DEPARTMENT_CODE", "DEPARTMENT_CODE");
            objectTag.AddObjectData(store, "PtttDetail", ListPtttDetail);
            objectTag.AddObjectData(store, "UserDepaTreaSv", ListUserDepaTreaSv);
            objectTag.AddRelationship(store, "Depa", "UserDepaTreaSv", "DEPARTMENT_CODE", "DEPARTMENT_CODE");
            objectTag.AddObjectData(store, "Patient", ListPatient.OrderBy(p => p.PATIENT_CODE).ThenBy(p => p.EXECUTE_TIME).ToList());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        private void ProcessFilterData(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> listSereServ)
        {
            try
            {
                foreach (var item in listHisEkipUser)
                {
                    VSarReportMrs00173RDO rdo = new VSarReportMrs00173RDO();
                    var sereServSub = listSereServ.Where(o => o.EKIP_ID == item.EKIP_ID).ToList() ?? new List<HIS_SERE_SERV>();
                    foreach (var sereServ in sereServSub)
                    {
                        var service = listHisService.FirstOrDefault(o => o.ID == sereServ.SERVICE_ID) ?? new HIS_SERVICE();
                        var serviceReq = listHisServiceReq.FirstOrDefault(o => o.ID == sereServ.SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
                        var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == service.PTTT_GROUP_ID) ?? new HIS_PTTT_GROUP();
                        var executeRole = HisExecuteRoleCFG.EXECUTE_ROLEs.FirstOrDefault(o => o.ID == item.EXECUTE_ROLE_ID) ?? new HIS_EXECUTE_ROLE();
                        var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.DEPARTMENT_ID);
                        HIS_PATIENT patient = listHisPatient.FirstOrDefault(o => o.ID == sereServ.TDL_PATIENT_ID) ?? new HIS_PATIENT();
                        HIS_SERVICE service1 = listHisService.FirstOrDefault(o => o.ID == sereServ.SERVICE_ID) ?? new HIS_SERVICE();
                        
                        if (department == null)
                        {
                            HIS_EMPLOYEE employee = null;
                            department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == item.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                        }
                        rdo.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                        rdo.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                        rdo.USERNAME = item.USERNAME;
                        rdo.LOGINNAME = item.LOGINNAME;
                        rdo.AMOUNT = sereServ.AMOUNT;
                        rdo.VIR_TOTAL_PRICE = sereServ.VIR_TOTAL_PRICE??0;
                        rdo.PTTT_GROUP_CODE = ptttGroup.PTTT_GROUP_CODE;
                        rdo.PTTT_GROUP_NAME = ptttGroup.PTTT_GROUP_NAME;
                        rdo.EXECUTE_TIME = sereServ.EXECUTE_TIME;
                        
                        rdo.EXECUTE_ROLE_ID = item.EXECUTE_ROLE_ID;
                        rdo.EXECUTE_ROLE_CODE = executeRole.EXECUTE_ROLE_CODE;
                        //rdo.PATIENT_ID= sereServ.TDL_PATIENT_ID;

                        if (serviceReq != null)
                        {
                            rdo.START_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.START_TIME ?? 0);
                        }

                        if (IsNotNull(patient))
                        {
                            
                            rdo.PATIENT_NAME = patient.VIR_PATIENT_NAME;
                            rdo.PATIENT_CODE = patient.PATIENT_CODE;
                        }
                        if (IsNotNull(service1))
                        {
                            rdo.SERVICE_NAME = service1.SERVICE_NAME;
                          

                        }
                        
                        ListRdo.Add(rdo);
                    }
                    
                }
                DIC_GROUP_TOTAL_PRICE = ListRdo.GroupBy(o => (o.PTTT_GROUP_CODE ?? "")).ToDictionary(p => p.Key, p => p.Sum(o => o.VIR_TOTAL_PRICE));

                var groupByUserDepa = ListRdo.GroupBy(o => new { o.LOGINNAME, o.DEPARTMENT_CODE }).ToList();
                foreach (var item in groupByUserDepa)
                {
                    VSarReportMrs00173RDO rdo = new VSarReportMrs00173RDO();
                    List<VSarReportMrs00173RDO> listSub = item.ToList<VSarReportMrs00173RDO>();
                    rdo.LOGINNAME = listSub.First().LOGINNAME;
                    rdo.USERNAME = listSub.First().USERNAME;
                    rdo.DEPARTMENT_CODE = listSub.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                    rdo.DIC_ROLE_GROUP = listSub.GroupBy(o => (o.EXECUTE_ROLE_CODE ?? "") + "_" + (o.PTTT_GROUP_CODE ?? "")).ToDictionary(p => p.Key, p => p.Sum(o => o.AMOUNT));
                    ListUserDepa.Add(rdo);
                }
                var groupByLoginname = ListRdo.GroupBy(o => o.LOGINNAME).ToList();
                //ListRdo.Clear();
                foreach (var item in groupByLoginname)
                {
                    VSarReportMrs00173RDO rdo = new VSarReportMrs00173RDO();
                    List<VSarReportMrs00173RDO> listSub = item.ToList<VSarReportMrs00173RDO>();
                    rdo.LOGINNAME = listSub.First().LOGINNAME;
                    rdo.USERNAME = listSub.First().USERNAME;
                    rdo.DIC_ROLE_GROUP = listSub.GroupBy(o => (o.EXECUTE_ROLE_CODE ?? "") + "_" + (o.PTTT_GROUP_CODE ?? "")).ToDictionary(p => p.Key, p => p.Sum(o => o.AMOUNT));
                    ListUser.Add(rdo);
                }
                var groupByUserDepaTreaSv = ListRdo.GroupBy(o => new { o.LOGINNAME, o.DEPARTMENT_CODE,o.PATIENT_NAME,o.SERVICE_NAME }).ToList();
                foreach (var item in groupByUserDepaTreaSv)
                {
                    VSarReportMrs00173RDO rdo = new VSarReportMrs00173RDO();
                    List<VSarReportMrs00173RDO> listSub = item.ToList<VSarReportMrs00173RDO>();
                    rdo.LOGINNAME = listSub.First().LOGINNAME;
                    rdo.USERNAME = listSub.First().USERNAME;
                    rdo.DEPARTMENT_CODE = listSub.First().DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = listSub.First().DEPARTMENT_NAME;
                    rdo.PATIENT_NAME = listSub.First().PATIENT_NAME;
                    
                    rdo.SERVICE_NAME = listSub.First().SERVICE_NAME;
                    rdo.DIC_ROLE_GROUP = listSub.GroupBy(o => (o.EXECUTE_ROLE_CODE ?? "") + "_" + (o.PTTT_GROUP_CODE ?? "")).ToDictionary(p => p.Key, p => p.Sum(o => o.AMOUNT));
                    ListUserDepaTreaSv.Add(rdo);
                }

                var groupByPatient = ListRdo.GroupBy(o => new { o.PATIENT_NAME, o.SERVICE_NAME, o.EXECUTE_TIME, o.PTTT_GROUP_CODE }).ToList();
                foreach (var item in groupByPatient)
                {
                    VSarReportMrs00173RDO rdo = new VSarReportMrs00173RDO();
                    List<VSarReportMrs00173RDO> listSub = item.ToList<VSarReportMrs00173RDO>();
                    
                    rdo.PATIENT_NAME = listSub.First().PATIENT_NAME;
                    rdo.PATIENT_CODE = listSub.First().PATIENT_CODE;
                    rdo.SERVICE_NAME = listSub.First().SERVICE_NAME;
                    rdo.PTTT_GROUP_CODE = listSub.First().PTTT_GROUP_CODE;
                    rdo.PTTT_GROUP_NAME = listSub.First().PTTT_GROUP_NAME;
                    rdo.EXECUTE_TIME = listSub.First().EXECUTE_TIME;
                    rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(listSub.First().EXECUTE_TIME ?? 0);
                    rdo.START_TIME_STR = listSub.First().START_TIME_STR;
                    rdo.MAIN_EXECUTE = string.Join(",", listSub.Where(p => p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Main || p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__Anesthetist).Select(p => p.USERNAME));
                    rdo.EXTRA_EXECUTE = string.Join(",", listSub.Where(p => p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM1 || p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PM2 || p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe1 || p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__PMe2).Select(p => p.USERNAME));
                    rdo.HELPING = string.Join(",", listSub.Where(p => p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__YTDD || p.EXECUTE_ROLE_ID == HisExecuteRoleCFG.HisExecuteRoleId__DCVPTTT).Select(p => p.USERNAME));
                    rdo.VIR_TOTAL_PRICE = listSub.First().VIR_TOTAL_PRICE;
                    ListPatient.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
