using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00022
{
    public class Mrs00022Processor : AbstractProcessor
    {
        List<Mrs00022RDO> listRdo = new List<Mrs00022RDO>();
        Mrs00022Filter castFilter = null;
        List<HIS_SERVICE_REQ> listTemp;

        public Mrs00022Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00022Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00022Filter)this.reportFilter);
                CommonParam getParam = new CommonParam();
                HisServiceReqFilterQuery filter = new HisServiceReqFilterQuery();
                filter.REQUEST_ROOM_ID = castFilter.ROOM_ID;
                filter.INTRUCTION_DATE_FROM = castFilter.TIME_FROM;
                filter.INTRUCTION_DATE_TO = castFilter.TIME_TO;
                filter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                listTemp = new HisServiceReqManager(getParam).Get(filter);

                if (getParam.HasException)
                {
                    result = false;
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
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listTemp))
                {
                    var lstRdo = (from r in listTemp select new Mrs00022RDO(r)).ToList();
                    if (IsNotNullOrEmpty(lstRdo))
                    {
                        var groups = lstRdo.GroupBy(o => o.EXECUTE_ROOM_ID).ToList();
                        foreach (var gr in groups)
                        {
                            var rdo = new Mrs00022RDO();
                            rdo = gr.FirstOrDefault();
                            rdo.COUNT_GROUP1 = gr.Sum(o => o.COUNT_GROUP1);
                            rdo.COUNT_GROUP2 = gr.Sum(o => o.COUNT_GROUP2);
                            rdo.COUNT_GROUP3 = gr.Sum(o => o.COUNT_GROUP3);
                            listRdo.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                var resultRoom = new HisRoomManager().GetViewByIds(new List<long>() { castFilter.ROOM_ID });
                V_HIS_ROOM room = IsNotNullOrEmpty(resultRoom) ? resultRoom.FirstOrDefault() : new V_HIS_ROOM();
                if (room != null)
                {
                    dicSingleTag.Add("ROOM_CODE", room.ROOM_CODE);
                    dicSingleTag.Add("ROOM_NAME", room.ROOM_NAME);
                }

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
