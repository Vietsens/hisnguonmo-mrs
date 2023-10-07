using MOS.MANAGER.HisService;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00051
{
    public class Mrs00051Processor : AbstractProcessor
    {
        Mrs00051Filter castFilter = null;
        List<Mrs00051RDO> ListRdo = new List<Mrs00051RDO>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();

        string Department_Name = "";
        string Room_Name = "";

        public Mrs00051Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00051Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00051Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("castFilter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
               
                    var executeRooms = new HisExecuteRoomManager().GetView(new HisExecuteRoomViewFilterQuery());

                    var executeRoom = new V_HIS_EXECUTE_ROOM();
                    if (castFilter.EXACT_EXECUTE_ROOM_ID != null)
                    {
                        executeRoom = executeRooms.FirstOrDefault(o => o.ID == castFilter.EXACT_EXECUTE_ROOM_ID);
                    }
                    if (castFilter.EXE_ROOM_ID != null)
                    {
                        executeRoom = executeRooms.FirstOrDefault(o => o.ROOM_ID == castFilter.EXE_ROOM_ID);
                    }
                    if (castFilter.EXECUTE_ROOM_ID != null)
                    {
                        executeRoom = executeRooms.FirstOrDefault(o => o.ROOM_ID == castFilter.EXECUTE_ROOM_ID);
                    }
                    if (castFilter.EXAM_ROOM_ID != null)
                    {
                        executeRoom = executeRooms.FirstOrDefault(o => o.ROOM_ID == castFilter.EXAM_ROOM_ID);
                    }
                        Room_Name = executeRoom.EXECUTE_ROOM_NAME;
                        Department_Name = executeRoom.DEPARTMENT_NAME;

                    HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                    filter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                    filter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                    if (executeRoom.ROOM_ID > 0)
                    {
                        filter.REQUEST_ROOM_ID__OR__EXECUTE_ROOM_ID = executeRoom.ROOM_ID;
                    }
                    ListServiceReq = new HisServiceReqManager(new CommonParam()).Get(filter);

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
            bool result = false;
            try
            {

                ProcessListServiceReq(ref result);
                result = true;

                Inventec.Common.Logging.LogSystem.Info("result" + result);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListServiceReq(ref bool result)
        {
            try
            {

                if (ListServiceReq != null && ListServiceReq.Count > 0)
                {
                    ListServiceReq = ListServiceReq.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT).ToList().OrderBy(o => o.SERVICE_REQ_STT_ID).ToList();
                    if (ListServiceReq.Count > 0)
                    {
                        CommonParam paramGet = new CommonParam();

                        int start = 0;
                        int count = ListServiceReq.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);

                            List<HIS_SERVICE_REQ> HisServiceReqs = ListServiceReq.Skip(start).Take(limit).ToList();
                            ProcessDetailListServiceReq(paramGet, HisServiceReqs);
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu");
                        }
                        ListRdo = ListRdo.OrderBy(o => o.PATIENT_CODE).ThenBy(o => o.SERVICE_REQ_CODE).ThenBy(o => o.REQUEST_DEPARTMENT_NAME).ThenBy(o => o.REQUEST_ROOM_NAME).ToList();

                        Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
        }

        private void ProcessDetailListServiceReq(CommonParam paramGet, List<HIS_SERVICE_REQ> hisServiceReqs)
        {
            try
            {
                if (hisServiceReqs.Count > 0)
                {
                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                    sereServFilter.SERVICE_REQ_IDs = hisServiceReqs.Select(s => s.ID).ToList();
                    var listSereServ = new HisSereServManager(paramGet).GetView(sereServFilter);
                    ProcessDetailListSereServ(hisServiceReqs, listSereServ);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ();
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new HIS_SERVICE_REQ();
            }
            return result;
        }

        private void ProcessDetailListSereServ(List<HIS_SERVICE_REQ> hisServiceReqs, List<V_HIS_SERE_SERV> hisSereServs)
        {
            try
            {
                if (hisSereServs.Count > 0)
                {
                    dicServiceReq = hisServiceReqs.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());
                    foreach (var sereServ in hisSereServs)
                    {
                        Mrs00051RDO rdo = new Mrs00051RDO();
                        rdo.PATIENT_CODE = req(sereServ).TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = req(sereServ).TDL_PATIENT_NAME;
                        rdo.REQUEST_DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME;
                        rdo.REQUEST_ROOM_NAME = sereServ.REQUEST_ROOM_NAME;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        rdo.AMOUNT = sereServ.AMOUNT;
                        rdo.EXECUTE_USERNAME = req(sereServ).EXECUTE_USERNAME;
                        rdo.SERVICE_REQ_CODE = req(sereServ).SERVICE_REQ_CODE;
                        var serviceReq = hisServiceReqs.FirstOrDefault(o => o.ID == sereServ.SERVICE_REQ_ID);
                        if (serviceReq != null)
                        {
                            rdo.FINISH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.FINISH_TIME ?? 0);
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", (Department_Name ?? "").ToUpper());
                dicSingleTag.Add("EXECUTE_ROOM_NAME", (Room_Name ?? "").ToUpper());

                objectTag.AddObjectData(store, "Report", ListRdo);
                Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
