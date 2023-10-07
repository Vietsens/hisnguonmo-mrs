using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServSegr;
using MOS.MANAGER.HisServiceGroup;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Core.MrsReport.RDO;

namespace MRS.Processor.Mrs00604
{
    public class Mrs00604Processor : AbstractProcessor
    {
        Mrs00604Filter filter = new Mrs00604Filter();
        CommonParam paramGet = new CommonParam();
        List<Mrs00604RDO> listHisTreatment = new List<Mrs00604RDO>();
        List<Mrs00604RDO> ListRdo = new List<Mrs00604RDO>();
        HIS_BRANCH Branch = new HIS_BRANCH();
        public Mrs00604Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00604Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00604Filter)this.reportFilter;
                //get dữ liệu:

                listHisTreatment = new ManagerSql().GetSereServ(filter);



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

                ListRdo.Clear();

                if (IsNotNullOrEmpty(listHisTreatment))
                {
                    var group = listHisTreatment.GroupBy(o => o.ID).ToList();

                    foreach (var item in group)
                    {
                        if (filter.CATEGORY_CODE__KSKs != null && filter.ROOM_CODE__PCBs != null)
                        {
                            var roomIds = HisRoomCFG.HisRooms.Where(o => filter.ROOM_CODE__PCBs.Contains(o.ROOM_CODE)).Select(p => p.ID).ToList();
                            if (roomIds.Count > 0)
                            {
                                if (!item.ToList().Exists(o => filter.CATEGORY_CODE__KSKs.Contains(o.CATEGORY_CODE) && roomIds.Contains(o.TDL_EXECUTE_ROOM_ID ?? 0)))
                                    continue;
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Error("Khong tim thay phong can bo vien chuc");
                                return false;
                            }

                        }
                        Mrs00604RDO rdo = new Mrs00604RDO(item.First());
                        this.ProcessorSum(rdo, item.ToList<Mrs00604RDO>());
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

                ListRdo.Clear();
            }
            return result;
        }
        private void ProcessorSum(Mrs00604RDO r, List<Mrs00604RDO> listSub)
        {
            try
            {
                r.DIC_GROUP = new Dictionary<string, decimal>();
                r.DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
                r.DIC_GROUP_HEALTH = new Dictionary<string, string>();
                r.KH_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.XN_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.SA_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.CDHA_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE
                    .ID__CDHA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.TDCN_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.KHAC_PRICE = listSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                r.DIC_GROUP = listSub.GroupBy(o => o.CATEGORY_CODE ?? "").ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                r.DIC_GROUP_AMOUNT = listSub.GroupBy(o => o.CATEGORY_CODE ?? "").ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                r.DIC_GROUP_HEALTH = listSub.GroupBy(o => o.HEALTH_EXAM_RANK_CODE ?? "").ToDictionary(p => p.Key, q => "X");


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.IN_TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.IN_TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.AddObjectData(store, "Report", ListRdo);

        }


    }
}