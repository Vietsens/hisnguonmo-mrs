using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00675
{
    class Mrs00675Processor : AbstractProcessor
    {
        Mrs00675Filter castFilter = null;
        string ReportType = "";
        List<V_HIS_SERVICE_RETY_CAT> ListServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> ListReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<Mrs00675RDO> ListRdo = new List<Mrs00675RDO>();

        public Mrs00675Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            ReportType = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00675Filter);
        }

        //tach mrs 330
        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00675Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                ListSereServ = new ManagerSql().GetSS(castFilter);

                HisServiceRetyCatViewFilterQuery retyCatFilter = new HisServiceRetyCatViewFilterQuery();
                retyCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                retyCatFilter.IS_ACTIVE = 1;
                retyCatFilter.REPORT_TYPE_CAT_IDs = castFilter.REPORT_TYPE_CAT_IDs;

                ListServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(retyCatFilter);

                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = ReportType;
                reportTypeCatFilter.IS_ACTIVE = 1;
                reportTypeCatFilter.IDs = castFilter.REPORT_TYPE_CAT_IDs;
                ListReportTypeCat = new HisReportTypeCatManager(paramGet).Get(reportTypeCatFilter);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    ListRdo = new List<Mrs00675RDO>();

                    ProcessRdo(ListSereServ);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessRdo(List<HIS_SERE_SERV> listSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(listSereServ) && IsNotNullOrEmpty(ListServiceRetyCat))
                {
                    listSereServ = listSereServ.Where(o => ListServiceRetyCat.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                }

                if (IsNotNullOrEmpty(listSereServ))
                {
                    if (IsNotNullOrEmpty(ListReportTypeCat))
                    {
                        foreach (var retyCat in ListReportTypeCat)
                        {
                            var grCategory = ListServiceRetyCat.Where(o => o.REPORT_TYPE_CAT_ID == retyCat.ID).ToList();
                            if (grCategory == null) continue;

                            Mrs00675RDO rdo = new Mrs00675RDO();
                            rdo.SERVICE_TYPE_NAME = retyCat.CATEGORY_NAME;
                            rdo.ROW_POS = retyCat.NUM_ORDER ?? 9999;
                            rdo.DIC_ROOM_COUNT = new Dictionary<string, long>();

                            var lstSereServ = listSereServ.Where(o => grCategory.Select(s => s.SERVICE_ID).Contains(o.SERVICE_ID)).ToList();
                            if (IsNotNullOrEmpty(lstSereServ))
                            {
                                var grReqRoom = lstSereServ.GroupBy(g => g.TDL_REQUEST_ROOM_ID).ToList();
                                foreach (var item in grReqRoom)
                                {
                                    var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.First().TDL_REQUEST_ROOM_ID);
                                    if (room == null) continue;

                                    if (!rdo.DIC_ROOM_COUNT.ContainsKey(room.ROOM_CODE))
                                    {
                                        rdo.DIC_ROOM_COUNT[room.ROOM_CODE] = 0;
                                    }

                                    rdo.DIC_ROOM_COUNT[room.ROOM_CODE] += item.Count();
                                }

                                //bo cac dich vu duoc cau hinh tranh tinh 2 lan.
                                listSereServ = listSereServ.Where(o => !lstSereServ.Select(s => s.ID).Contains(o.ID)).ToList();
                            }

                            ListRdo.Add(rdo);
                        }
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
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                ListRdo = ListRdo.OrderBy(o => o.ROW_POS).ThenBy(o => o.SERVICE_TYPE_NAME).ToList();

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.SetUserFunction(store, "Element", new RDOElement());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
