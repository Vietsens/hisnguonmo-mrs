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

namespace MRS.Processor.Mrs00035
{
    public class Mrs00035Processor : AbstractProcessor
    {
        Mrs00035Filter castFilter = null;
        List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ> ListCurrentServiceReq = new List<MOS.EFMODEL.DataModels.HIS_SERVICE_REQ>();
        private const string EXAM = "EXAM";
        private const string TEST = "TEST";
        private const string DIIM = "DIIM";
        private const string MISU = "MISU";
        private const string FUEX = "FUEX";
        private const string SURG = "SURG";
        private const string SUIM = "SUIM";
        private const string ENDO = "ENDO";
        private List<Mrs00035RDO> ListRdo = new List<Mrs00035RDO>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        CommonParam paramGet = new CommonParam();
        public Mrs00035Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00035Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00035Filter)reportFilter);
            var result = true;
            try
            {
                List<long> requestRoomIds = null;
                if (IsNotNullOrEmpty(castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    HisRoomFilterQuery HisRoomFilter = new HisRoomFilterQuery();
                    var listHisRoom = new HisRoomManager().Get(HisRoomFilter);
                    listHisRoom = listHisRoom.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID)).ToList();
                    requestRoomIds = listHisRoom.Select(o => o.ID).ToList();
                }
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.REQUEST_ROOM_IDs = requestRoomIds;
                filter.INTRUCTION_TIME_FROM = castFilter.INTRUCTION_TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.INTRUCTION_TIME_TO;
                filter.FINISH_TIME_FROM = castFilter.FINISH_TIME_FROM;
                filter.FINISH_TIME_TO = castFilter.FINISH_TIME_TO;
                ListCurrentServiceReq = new HisServiceReqManager().Get(filter);

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
                        var Groups = ListCurrentServiceReq.OrderBy(o => o.REQUEST_DEPARTMENT_ID).ToList().GroupBy(g => g.REQUEST_DEPARTMENT_ID).ToList();
                        foreach (var group in Groups)
                        {
                            List<HIS_SERVICE_REQ> listSub = group.ToList<HIS_SERVICE_REQ>();
                            if (listSub != null && listSub.Count > 0)
                            {
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub[0].REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                                Mrs00035RDO rdo = new Mrs00035RDO();
                                rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00035Filter)this.reportFilter).INTRUCTION_TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00035Filter)this.reportFilter).FINISH_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00035Filter)this.reportFilter).INTRUCTION_TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00035Filter)this.reportFilter).FINISH_TIME_TO ?? 0));
           
            if (((Mrs00035Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs != null)
            {
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.IDs = ((Mrs00035Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs;
                var department = new HisDepartmentManager().Get(departmentFilter);
                if (IsNotNullOrEmpty(department)) dicSingleTag.Add("REQUEST_DEPARTMENT_NAMEs", string.Join(", ", department.Select(o => o.DEPARTMENT_NAME)));
            }
            
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
