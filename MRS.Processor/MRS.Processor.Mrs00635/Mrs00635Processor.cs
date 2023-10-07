using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;


namespace MRS.Processor.Mrs00635
{

    class Mrs00635Processor : AbstractProcessor
    {
        Mrs00635Filter castFilter = null;
        List<Mrs00635RDO> listSereServRdo = new List<Mrs00635RDO>();
        List<Mrs00635RDO> listRdo = new List<Mrs00635RDO>();
        List<Mrs00635RDO> listSereServDetailRdo = new List<Mrs00635RDO>();
        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        public Mrs00635Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00635Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00635Filter)this.reportFilter;

                HisServiceFilterQuery HisServicefilter = new HisServiceFilterQuery();
                HisServicefilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                listHisService = new HisServiceManager(paramGet).Get(HisServicefilter);

                listSereServDetailRdo = new ManagerSql().GetSereServDO(this.castFilter);
                
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
                listRdo = listSereServDetailRdo;
                if (listSereServDetailRdo != null && listSereServDetailRdo.Count > 0)
                {
                    listSereServRdo = listSereServDetailRdo.GroupBy(p => new { p.SERVICE_ID, p.VIR_PRICE }).Select(o => new Mrs00635RDO()
                    {
                        SERVICE_ID = o.First().SERVICE_ID,
                        SERVICE_CODE = o.First().SERVICE_CODE,
                        SERVICE_NAME = o.First().SERVICE_NAME,
                        SERVICE_TYPE_CODE = o.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = o.First().SERVICE_TYPE_NAME,
                        AMOUNT = o.Sum(p => p.AMOUNT),
                        VIR_PRICE = o.First().VIR_PRICE,
                        VIR_TOTAL_PRICE = o.Sum(p => p.VIR_TOTAL_PRICE),

                        PTTT_GROUP_NAME = o.First().PTTT_GROUP_NAME,

                    }).ToList();
                }

                if (listSereServDetailRdo != null && listSereServDetailRdo.Count > 0)
                {
                    foreach (var rdo in listSereServRdo)
                    {
                        var service = listHisService.FirstOrDefault(o => o.ID == rdo.SERVICE_ID);
                        if (service != null)
                        {
                            var parentService = listHisService.FirstOrDefault(o => o.ID == service.PARENT_ID);
                            if (parentService != null)
                            {
                                rdo.PARENT_SERVICE_CODE = parentService.SERVICE_CODE;
                                rdo.PARENT_SERVICE_NAME = parentService.SERVICE_NAME;
                            }
                        }

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

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

            if (this.castFilter.REQUEST_DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.castFilter.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            }

            if (this.castFilter.REQUEST_DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", string.Join(" - ", (HisDepartmentCFG.DEPARTMENTs ?? new List<HIS_DEPARTMENT>()).Where(o => this.castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }

            if (this.castFilter.EXECUTE_DEPARTMENT_ID != null)
            {
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.castFilter.EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            }

            if (this.castFilter.EXECUTE_DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", string.Join(" - ", (HisDepartmentCFG.DEPARTMENTs ?? new List<HIS_DEPARTMENT>()).Where(o => this.castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }

            if (this.castFilter.REQUEST_ROOM_ID != null)
            {
                dicSingleTag.Add("REQUEST_ROOM_NAME", (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.castFilter.REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME);
            }

            if (this.castFilter.REQUEST_ROOM_IDs != null)
            {
                dicSingleTag.Add("REQUEST_ROOM_NAME", string.Join(" - ", (HisRoomCFG.HisRooms ?? new List<V_HIS_ROOM>()).Where(o => this.castFilter.REQUEST_ROOM_IDs.Contains(o.ID)).Select(p => p.ROOM_NAME).ToList()));
            }

            if (this.castFilter.EXECUTE_ROOM_ID != null)
            {
                dicSingleTag.Add("EXECUTE_ROOM_NAME", (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.castFilter.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME);
            }

            if (this.castFilter.EXECUTE_ROOM_IDs != null)
            {
                dicSingleTag.Add("EXECUTE_ROOM_NAME", string.Join(" - ", (HisRoomCFG.HisRooms ?? new List<V_HIS_ROOM>()).Where(o => this.castFilter.EXECUTE_ROOM_IDs.Contains(o.ID)).Select(p => p.ROOM_NAME).ToList()));
            }
            objectTag.AddObjectData(store, "Child", listSereServDetailRdo.OrderBy(s => s.TDL_INTRUCTION_DATE).ToList());
            objectTag.AddObjectData(store, "Report", listSereServRdo.OrderBy(s => s.PARENT_SERVICE_NAME).ToList());

            objectTag.AddObjectData(store, "Parent", HisServiceTypeCFG.HisServiceTypes.OrderBy(o => o.NUM_ORDER).Where(p => listSereServRdo.Exists(q => q.SERVICE_TYPE_CODE == p.SERVICE_TYPE_CODE)).ToList());
            objectTag.AddRelationship(store, "Report", "Child", new string[] { "SERVICE_ID", "VIR_PRICE" }, new string[] { "SERVICE_ID", "VIR_PRICE" });
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_TYPE_CODE", "SERVICE_TYPE_CODE");
            objectTag.AddObjectData(store, "Detail", listRdo.OrderBy(s => s.TDL_INTRUCTION_DATE).ToList());
        }
    }
}
