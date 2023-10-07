using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisEkipUser; 

namespace MRS.Processor.Mrs00441
{
    class Mrs00441Processor : AbstractProcessor
    {
        Mrs00441Filter castFilter = null; 
        List<Mrs00441RDO> listRdo = new List<Mrs00441RDO>(); 

        public Mrs00441Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV_PTTT> listSereServPTTTs = new List<V_HIS_SERE_SERV_PTTT>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        List<V_HIS_ROOM> listRooms = new List<V_HIS_ROOM>();
        List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
        List<HIS_EKIP_USER> listHisEkipUser = new List<HIS_EKIP_USER>(); 


        public override Type FilterType()
        {
            return typeof(Mrs00441Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00441Filter)this.reportFilter; 
                var skip = 0; 
                //lay khoa
                if (IsNotNull(this.castFilter.DEPARTMENT_ID))
                {
                    HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                    departmentFilter.ID = this.castFilter.DEPARTMENT_ID; 
                    listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(departmentFilter); 
                }
                //lay phong
                if (IsNotNull(this.castFilter.ROOM_ID))
                {
                    HisRoomViewFilterQuery roomFilter = new HisRoomViewFilterQuery(); 
                    roomFilter.ID = this.castFilter.ROOM_ID; 
                    listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(paramGet).GetView(roomFilter); 
                }

                //yeu cau
                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery(); 
                serviceReqFilter.INTRUCTION_TIME_FROM = this.castFilter.TIME_FROM; 
                serviceReqFilter.INTRUCTION_TIME_TO = this.castFilter.TIME_TO; 
                serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT; 
                if (IsNotNull(this.castFilter.DEPARTMENT_ID))
                {
                    serviceReqFilter.REQUEST_DEPARTMENT_ID = this.castFilter.DEPARTMENT_ID; 
                }
                if (IsNotNull(this.castFilter.ROOM_ID))
                {
                    serviceReqFilter.EXECUTE_ROOM_ID = this.castFilter.ROOM_ID; 
                }

                listServiceReqs = new HisServiceReqManager(paramGet).GetView(serviceReqFilter); 
                var treatmentIds = listServiceReqs.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                dicServiceReq = listServiceReqs.ToDictionary(o => o.ID); 

                //YC-DV
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    skip = 0; 
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                        sereServFilter.TREATMENT_IDs = listIds; 
                        sereServFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT; 
                        if (IsNotNull(this.castFilter.DEPARTMENT_ID))
                        {
                            sereServFilter.REQUEST_DEPARTMENT_ID = this.castFilter.DEPARTMENT_ID; 
                        }
                        if (IsNotNull(this.castFilter.ROOM_ID))
                        {
                            sereServFilter.EXECUTE_ROOM_ID = this.castFilter.ROOM_ID; 
                        }
                        var sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter);

                        if (sereServs != null)
                        {
                            listSereServ.AddRange(sereServs);
                        }
                    }
                    if (listSereServ != null)
                    {
                        listSereServ = listSereServ.Where(o => dicServiceReq.ContainsKey(o.SERVICE_REQ_ID??0)).ToList();
                    }
                }

                var ekipIds = listSereServ.Select(o => o.EKIP_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(ekipIds))
                {
                    skip = 0;
                    while (ekipIds.Count - skip > 0)
                    {
                        var listIDs = ekipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisEkipUserFilterQuery HisEkipUserfilter = new HisEkipUserFilterQuery();
                        HisEkipUserfilter.EKIP_IDs = listIDs;
                        HisEkipUserfilter.ORDER_FIELD = "ID";
                        HisEkipUserfilter.ORDER_DIRECTION = "ASC";
                        var listHisEkipUserSub = new HisEkipUserManager(paramGet).Get(HisEkipUserfilter);
                        if (listHisEkipUserSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisEkipUserSub" + listHisEkipUserSub.Count);
                        else
                        {
                            listHisEkipUser.AddRange(listHisEkipUserSub);
                        }

                    }
                }


                Inventec.Common.Logging.LogSystem.Info("listSereServs" + listSereServ.Count); 
                var listSereServIds = listSereServ.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listSereServIds.Count - skip > 0)
                {
                    var listIds = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServPtttViewFilterQuery filter = new HisSereServPtttViewFilterQuery(); 
                    filter.SERE_SERV_IDs = listIds; 
                    var sereServPTTTs = new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(paramGet).GetView(filter); 
                    listSereServPTTTs.AddRange(sereServPTTTs); 
                }
                Inventec.Common.Logging.LogSystem.Info("listSereServPTTTs" + listSereServPTTTs.Count); 

                listSereServ = listSereServ.Where(w => listSereServPTTTs.Select(s => s.SERE_SERV_ID).Contains(w.ID)).ToList(); 
                Inventec.Common.Logging.LogSystem.Info("listSereServ" + listSereServ.Count); 
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
                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var sereServ in listSereServ)
                    {
                        var sereServPTTT = listSereServPTTTs.Where(w => w.SERE_SERV_ID == sereServ.ID).ToList();
                        var ekipUserSub = listHisEkipUser.Where(w => w.EKIP_ID == sereServ.EKIP_ID).ToList(); 
                        Mrs00441RDO rdo = new Mrs00441RDO();
                        rdo.V_HIS_SERE_SERV_PTTT = sereServPTTT.FirstOrDefault()??new V_HIS_SERE_SERV_PTTT();
                        rdo.V_HIS_SERE_SERV = sereServ;
                        rdo.V_HIS_SERVICE_REQ = req(sereServ);
                        if (IsNotNullOrEmpty(sereServPTTT))
                        {
                            rdo.PTTT_GROUP_NAME = sereServPTTT.First().PTTT_GROUP_NAME; 
                        }
                        rdo.TREATMENT_CODE = req(sereServ).TREATMENT_CODE; 
                        rdo.PATIENT_NAME = req(sereServ).TDL_PATIENT_NAME; 
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                        rdo.INTRUCTION_TIME = req(sereServ).INTRUCTION_TIME;
                        if (IsNotNullOrEmpty(ekipUserSub))
                        {
                            rdo.USERNAMEs = string.Join(" - ",ekipUserSub.Select(o=>o.USERNAME).Distinct().ToList());
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

        private V_HIS_SERVICE_REQ req(V_HIS_SERE_SERV sereServ)
        {
            return dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID??0)?dicServiceReq[sereServ.SERVICE_REQ_ID??0]:new V_HIS_SERVICE_REQ(); 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                if (IsNotNullOrEmpty(listDepartments))
                {
                    dicSingleTag.Add("DEPARTMENT", listDepartments.First().DEPARTMENT_NAME); 
                }
                if (IsNotNullOrEmpty(listRooms))
                {
                    dicSingleTag.Add("ROOM", listRooms.First().ROOM_NAME); 
                }

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.PTTT_GROUP_NAME).ToList()); 
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new MergeManyRowData()); 

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }
    class MergeManyRowData : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                string s_Data = parameters[0].ToString(); 
                if (s_Data == s_CurrentData)
                {
                    return true; 
                }
                else
                {
                    s_CurrentData = s_Data; 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return false; 
            }
            throw new NotImplementedException(); 
        }
    }
}
