using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00579;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;

namespace MRS.Processor.Mrs00579
{
    public class Mrs00579Processor : AbstractProcessor
    {
        private List<Mrs00579RDO> ListRdo = new List<Mrs00579RDO>();
        List<Mrs00579RDO> ListRdoTiemVacxin = new List<Mrs00579RDO>();
        Mrs00579Filter filter = null;
        string thisReportTypeCode = "";

        public Mrs00579Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00579Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00579Filter)this.reportFilter;
            try
            {
                var listHisExecuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());
                if (filter.EXACT_EXECUTE_ROOM_ID__EXE != null)
                {
                    filter.EXE_ROOM_ID = listHisExecuteRoom.FirstOrDefault(o => o.ID == filter.EXACT_EXECUTE_ROOM_ID__EXE).ROOM_ID;
                }
                if (filter.EXACT_EXECUTE_ROOM_IDs__REQ != null)
                {
                    filter.REQUEST_ROOM_IDs = listHisExecuteRoom.Where(o => filter.EXACT_EXECUTE_ROOM_IDs__REQ.Contains(o.ID)).Select(p=>p.ROOM_ID).ToList();
                }
                ListRdo = new Mrs00579RDOManager().GetMrs00579RDOByCreateTime(filter.EXE_ROOM_ID, filter.CREATE_TIME_FROM, filter.CREATE_TIME_TO, filter.REQUEST_ROOM_IDs);
                ListRdoTiemVacxin = ListRdo.Where(x => x.VACCINE_ID != null).ToList();
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
            bool result = false;
            try
            {
                if (ListRdo != null)
                {
                    ListRdo = ListRdo.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    foreach (var item in ListRdo)
                    {
                        item.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.INTRUCTION_TIME??0);
                        item.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.DOB ?? 0);
                    }
                }
                if (ListRdoTiemVacxin!=null)
                {
                    foreach (var item in ListRdoTiemVacxin)
                    {
                        item.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.INTRUCTION_TIME ?? 0);
                        if (item.DOB!=null)
                        {
                            item.DOB_STR = item.DOB.ToString().Substring(0, 4);
                        }
                    }
                        
                }
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00579RDO>();
                result = false;
            }
            return result;
        }

      

       

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("CREATE_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.CREATE_TIME_FROM));
            dicSingleTag.Add("CREATE_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.CREATE_TIME_TO));
          
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "VacXin", ListRdoTiemVacxin);
        }

    }

}
