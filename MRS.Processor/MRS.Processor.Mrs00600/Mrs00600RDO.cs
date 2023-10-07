using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00600
{
    class Mrs00600RDO : HIS_TREATMENT
    {
        public string TRANSACTION_CODE { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public decimal KH_PRICE { get; set; }
        public decimal XN_PRICE { get; set; }
        public decimal SA_PRICE { get; set; }
        public decimal CDHA_PRICE { get; set; }
        public decimal TDCN_PRICE { get; set; }
        public decimal KHAC_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public bool IS_ROOM_PCB { get; set; }
        public Mrs00600RDO()
        {

        }

        public Mrs00600RDO(HIS_TREATMENT data, List<HIS_SERE_SERV> listHisSereServ, List<HIS_TRANSACTION> listHisTransaction, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, Mrs00600Filter filter)
        {
            try
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00600RDO>(this, data);
              
                SetExtendField(this, listHisSereServ, listHisTransaction, listHisServiceRetyCat, filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00600RDO r, List<HIS_SERE_SERV> listHisSereServ, List<HIS_TRANSACTION> listHisTransaction, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, Mrs00600Filter filter)
        {
            try
            {
                this.DIC_GROUP = new Dictionary<string, decimal>();
                this.DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
                var sereServSub = listHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
                if (IsRoomPcb(sereServSub, filter, listHisServiceRetyCat))
                {
                    this.IS_ROOM_PCB = true;
                }
                var transaction = listHisTransaction.FirstOrDefault(o => o.TREATMENT_ID == r.ID);
                if (transaction != null)
                {
                    this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(transaction.TRANSACTION_TIME);
                    this.TRANSACTION_TIME = transaction.TRANSACTION_TIME;
                    this.TRANSACTION_CODE = transaction.TRANSACTION_CODE;
                }
                if (sereServSub != null)
                {
                    this.KH_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.XN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.SA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.CDHA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE
                        .ID__CDHA).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TDCN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.KHAC_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                    this.TOTAL_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_PRICE ?? 0);

                    if (listHisServiceRetyCat.Count > 0)
                    {
                        this.DIC_GROUP = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
                        this.DIC_GROUP_AMOUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                    }
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool IsRoomPcb(List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> sereServSub, Mrs00600Filter filter, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            bool result = true;
            try
            {
                if (filter.ROOM_CODE__PCBs != null)
                {
                    var roomIds = HisRoomCFG.HisRooms.Where(o => filter.ROOM_CODE__PCBs.Contains(o.ROOM_CODE)).Select(p => p.ID).ToList();
                    if (roomIds.Count > 0)
                    {
                        result = sereServSub.Exists(q => listHisServiceRetyCat.Exists(p => filter.CATEGORY_CODE__KSKs.Contains(p.CATEGORY_CODE) && p.SERVICE_ID == q.SERVICE_ID) && roomIds.Contains(q.TDL_EXECUTE_ROOM_ID));
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Error("Khong tim thay phong can bo vien chuc");
                        result = false;
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

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }


    }
}
