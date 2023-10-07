using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTreatment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MOS.LibraryHein; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using HIS.Treatment.DateTime; 
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment; 

namespace MRS.Processor.Mrs00479
{
    //báo cáo hoạt động điều trị

    class Mrs00479Processor : AbstractProcessor
    {
        Mrs00479Filter castFilter = new Mrs00479Filter(); 
        List<Mrs00479RDO> listRdo = new List<Mrs00479RDO>(); 
        List<Mrs00479RDO> listRdoGroup = new List<Mrs00479RDO>(); 

        List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV_PTTT> listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServcieReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>(); 

        List<long> listServiceIds = new List<long>(); 
        
        public Mrs00479Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00479Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00479Filter)this.reportFilter; 

                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00479"; 
                var listServices = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter); 

                listServiceIds = listServices.Where(w=>w.CATEGORY_CODE == "PTTT").Select(s => s.SERVICE_ID).ToList(); 
                //--!!--!!-!!-1@!#12312312312

                HisServiceReqViewFilterQuery serviceReqViewFilter = new HisServiceReqViewFilterQuery(); 
                serviceReqViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                serviceReqViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                serviceReqViewFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID; 
                serviceReqViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                listServiceReqs = new HisServiceReqManager(param).GetView(serviceReqViewFilter); 
                dicServcieReq = listServiceReqs.ToDictionary(o => o.ID); 
                Inventec.Common.Logging.LogSystem.Info("dicServcieReq" + dicServcieReq.Count); 
                var treatmentIds = listServiceReqs.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                var skip = 0; 
                while (listServiceIds.Count - skip > 0)
                {
                    var listIds = listServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var skip1 = 0; 
                    while (treatmentIds.Count - skip1 > 0)
                    {
                        var limit = treatmentIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    
                        HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                        sereServViewFilter.TREATMENT_IDs = limit; 
                        sereServViewFilter.SERVICE_IDs = listIds; 
                        listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter)); 
                        Inventec.Common.Logging.LogSystem.Info("listSereServs" + listSereServs.Count); 
                    }
                }
                listSereServs = listSereServs.Where(o => o.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList(); 
                skip = 0; 
                while (listSereServs.Count - skip > 0)
                {
                    var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    
                   
                    HisSereServPtttViewFilterQuery sereServPtttViewFilter = new HisSereServPtttViewFilterQuery(); 
                    sereServPtttViewFilter.SERE_SERV_IDs = listIds.Select(s => s.ID).ToList(); 
                    listSereServPttts.AddRange(new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(param).GetView(sereServPtttViewFilter)); 
                    Inventec.Common.Logging.LogSystem.Info("listSereServPttts" + listSereServPttts.Count); 

                }

                var listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList(); 
                Inventec.Common.Logging.LogSystem.Info("listTreatmentIds" + listTreatmentIds.Count); 
                skip = 0; 
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    //HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                    //treatmentViewFilter.IDs = listIds; 
                    //listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter)); 

                    HisDepartmentTranViewFilterQuery departmentTranViewFilter = new HisDepartmentTranViewFilterQuery(); 
                    departmentTranViewFilter.TREATMENT_IDs = listIds; 
                    listDepartmentTrans.AddRange(new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).GetView(departmentTranViewFilter)); 
                    Inventec.Common.Logging.LogSystem.Info("listDepartmentTrans" + listDepartmentTrans.Count); 
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
                V_HIS_SERVICE_REQ req = null; 
                foreach (var sereServ in listSereServs)
                {
                    if (!dicServcieReq.ContainsKey(sereServ.SERVICE_REQ_ID??0 )) continue; 
                    req = dicServcieReq[sereServ.SERVICE_REQ_ID??0]; 
                    var rdo = new Mrs00479RDO(); 
                    var serviceReq = listServiceReqs.Where(w => w.ID == sereServ.SERVICE_REQ_ID).ToList(); 
                    if (IsNotNullOrEmpty(serviceReq))
                    {
                        var serviceReqFirst = serviceReq.First(); 
                        if (serviceReqFirst.ICD_NAME == null || serviceReqFirst.ICD_NAME.Length == 0)
                            serviceReqFirst.ICD_NAME = serviceReqFirst.ICD_TEXT ?? "."; 
                        rdo.SERVICE_REQ = serviceReqFirst; 

                        rdo.SERE_SERV = sereServ; 

                        var sereServPttt = listSereServPttts.Where(w => w.SERE_SERV_ID == sereServ.ID).ToList(); 
                        if (IsNotNullOrEmpty(sereServPttt))
                            rdo.SERE_SERV_PTTT = sereServPttt.First(); 

                        else rdo.SERE_SERV_PTTT = new V_HIS_SERE_SERV_PTTT(); 
                        var departmetnTran = listDepartmentTrans.Where(w => w.TREATMENT_ID == sereServ.TDL_TREATMENT_ID).ToList(); 
                        if (IsNotNullOrEmpty(departmetnTran))
                        {
                            departmetnTran = departmetnTran.OrderByDescending(o => o.DEPARTMENT_IN_TIME).ToList(); 

                            var nextDepa = departmetnTran.First(); 
                            foreach (var depaTran in departmetnTran)
                            {
                                if (depaTran.DEPARTMENT_ID != castFilter.EXECUTE_DEPARTMENT_ID)
                                    nextDepa = depaTran; 
                                if (depaTran.DEPARTMENT_ID == castFilter.EXECUTE_DEPARTMENT_ID && depaTran.DEPARTMENT_IN_TIME >= req.FINISH_TIME)
                                    nextDepa = depaTran; 
                                else
                                {
                                    rdo.NEXT_DEPARTMENT_NAME = nextDepa.DEPARTMENT_NAME; 
                                    break; 
                                }
                            }
                        }
                        listRdo.Add(rdo); 
                    }
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
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                departmentFilter.ID = castFilter.EXECUTE_DEPARTMENT_ID; 
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter); 
                if (IsNotNullOrEmpty(listDepartments))
                    dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", listDepartments.First().DEPARTMENT_NAME); 

                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "Rdo", listRdo); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
