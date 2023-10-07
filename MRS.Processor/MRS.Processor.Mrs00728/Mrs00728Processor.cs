using IMSys.DbConfig.HIS_RS;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMedicineType;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00728
{
    class Mrs00728Processor : AbstractProcessor
    {
        List<Mrs00728RDO> listDetailRdo = new List<Mrs00728RDO>();
        List<Mrs00728RDO> listRdo = new List<Mrs00728RDO>();
        List<ManagerSql.Data> listData = new List<ManagerSql.Data>();
        Mrs00728Filter filter = null;
        public Mrs00728Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00728Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00728Filter)reportFilter;
            try
            {
                
                listData = new ManagerSql().Get(filter);
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
                if (IsNotNullOrEmpty(listData))
                {
                    foreach (var item in listData)
                    {
                        Mrs00728RDO rdo = new Mrs00728RDO();
                        rdo.REQUEST_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                        rdo.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                        rdo.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.MEDICINE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.MANUFRACTURER_CODE = item.MANUFACTURER_CODE;
                        rdo.MANUFRACTURER_NAME = item.MANUFACTURER_NAME;
                        rdo.PRICE = item.PRICE;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.INTRUCTION_TIME = item.TDL_INTRUCTION_DATE;
                        rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_INTRUCTION_DATE);
                        rdo.REQUEST_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        //rdo.EXP_TIME = item.EXP_TIME ?? 0;
                        //rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXP_TIME ?? 0);
                        listDetailRdo.Add(rdo);
                    }


                    var groupByUserMetyDate = listData.GroupBy(q => new { q.TDL_REQUEST_LOGINNAME, q.MEDICINE_TYPE_CODE, q.TDL_INTRUCTION_DATE, q.PRICE }).ToList();
                    foreach (var group in groupByUserMetyDate)
                    {
                        var item = group.First();
                        Mrs00728RDO rdo = new Mrs00728RDO();
                        rdo.REQUEST_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                        rdo.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                        rdo.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.MEDICINE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.MANUFRACTURER_CODE = item.MANUFACTURER_CODE;
                        rdo.MANUFRACTURER_NAME = item.MANUFACTURER_NAME;
                        rdo.PRICE = item.PRICE;
                        rdo.AMOUNT = group.Sum(s => s.AMOUNT);
                        rdo.INTRUCTION_TIME = item.TDL_INTRUCTION_DATE;
                        rdo.INTRUCTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_INTRUCTION_DATE);
                        //rdo.EXP_TIME = item.EXP_TIME ?? 0;
                        //rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.EXP_TIME ?? 0);
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
                dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
                dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));


                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.MEDICINE_TYPE_CODE).ThenBy(o => o.INTRUCTION_TIME).GroupBy(q => new { q.REQUEST_LOGINNAME, q.MEDICINE_TYPE_CODE, q.INTRUCTION_TIME, q.PRICE }).Select(p => p.First()).ToList());

                objectTag.AddObjectData(store, "Detail", listDetailRdo.OrderBy(o => o.MEDICINE_TYPE_CODE).ThenBy(o => o.INTRUCTION_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
