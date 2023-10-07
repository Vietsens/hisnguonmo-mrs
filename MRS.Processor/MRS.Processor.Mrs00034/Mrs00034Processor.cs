using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisRoom;

namespace MRS.Processor.Mrs00034
{
    public class Mrs00034Processor : AbstractProcessor
    {
        Mrs00034Filter castFilter = null;
        List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ> ListCurrentServiceReq = new List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();
        private const string EXAM = "EXAM";
        private const string TEST = "TEST";
        private const string DIIM = "DIIM";
        private const string MISU = "MISU";
        private const string FUEX = "FUEX";
        private const string SURG = "SURG";
        private const string SUIM = "SUIM";
        private const string ENDO = "ENDO";
        private List<Mrs00034RDO> ListRdo = new List<Mrs00034RDO>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        CommonParam paramGet = new CommonParam();
        public Mrs00034Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00034Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00034Filter)reportFilter);
            var result = true;
            try
            {
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.INTRUCTION_TIME_FROM = castFilter.INTRUCTION_TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.INTRUCTION_TIME_TO;
                if (IsNotNullOrEmpty(castFilter.REQUEST_ROOM_IDs))
                filter.REQUEST_ROOM_IDs = castFilter.REQUEST_ROOM_IDs;
                ListCurrentServiceReq = new HisServiceReqManager(paramGet).Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                if (ListCurrentServiceReq != null && ListCurrentServiceReq.Count > 0)
                {
                    ListCurrentServiceReq = ListCurrentServiceReq.Where(o => o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT && o.FINISH_TIME != null).ToList();
                    if (ListCurrentServiceReq.Count > 0)
                    {
                        var Groups = ListCurrentServiceReq.OrderBy(o => o.REQUEST_DEPARTMENT_ID).ToList().GroupBy(g => g.REQUEST_ROOM_ID).ToList();
                        foreach (var group in Groups)
                        {
                            List<HIS_SERVICE_REQ> listSub = group.ToList<HIS_SERVICE_REQ>();
                            if (castFilter.REQUEST_ROOM_IDs == null || (!castFilter.REQUEST_ROOM_IDs.Contains(listSub[0].REQUEST_ROOM_ID))) continue;
                            if (listSub != null && listSub.Count > 0)
                            {
                                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == listSub[0].REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                                Mrs00034RDO rdo = new Mrs00034RDO();
                                rdo.REQUEST_DEPARTMENT_NAME = room.DEPARTMENT_NAME;
                                rdo.REQUEST_ROOM_CODE = room.ROOM_CODE;
                                rdo.REQUEST_ROOM_NAME = room.ROOM_NAME;
                                foreach (var item in listSub)
                                {
                                    if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH)
                                    {
                                        rdo.AMOUNT_EXAM += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN)
                                    {
                                        rdo.AMOUNT_TEST += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__CDHA)
                                    {
                                        rdo.AMOUNT_DIIM += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT)
                                    {
                                        rdo.AMOUNT_MISU += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TDCN)
                                    {
                                        rdo.AMOUNT_FUEX += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT)
                                    {
                                        rdo.AMOUNT_SURG += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__SA)
                                    {
                                        rdo.AMOUNT_SUIM += 1;
                                    }
                                    else if (item.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__NS)
                                    {
                                        rdo.AMOUNT_ENDO += 1;
                                    }
                                }
                                ListRdo.Add(rdo);
                            }
                        }
                    }
                }
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00034Filter)this.reportFilter).INTRUCTION_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00034Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00034Filter)this.reportFilter).INTRUCTION_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00034Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
            if (((Mrs00034Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00034Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0);
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            }
            if (((Mrs00034Filter)this.reportFilter).REQUEST_ROOM_ID != null)
            {
                HisRoomViewFilterQuery roomFilter = new HisRoomViewFilterQuery();
                roomFilter.ID = ((Mrs00034Filter)this.reportFilter).REQUEST_ROOM_ID ?? 0;
                var room = new HisRoomManager().GetView(roomFilter);
                if (IsNotNullOrEmpty(room)) dicSingleTag.Add("REQUEST_ROOM_NAME", room.First().ROOM_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
