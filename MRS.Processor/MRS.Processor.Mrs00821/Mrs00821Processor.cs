using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00821
{
    public class Mrs00821Processor:AbstractProcessor
    {
        public Mrs00821Filter filter = new Mrs00821Filter();
        public List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> ListSereServExpend = new List<HIS_SERE_SERV>();
        List<HIS_TRANSACTION> ListTransaction = new List<HIS_TRANSACTION>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<Mrs00821RDO> ListRdo = new List<Mrs00821RDO>();
        List<long> SERVICE_TYPE_IDs = new List<long>()
        { 
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
        };
        public Mrs00821Processor(CommonParam param,string reportTypeCode):base(param,reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00821Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00821Filter)this.reportFilter;
            bool result = false;
            try
            {
                HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                ssFilter.TDL_INTRUCTION_TIME_FROM = filter.TIME_FROM;
                ssFilter.TDL_INTRUCTION_TIME_TO = filter.TIME_TO;
                ssFilter.TDL_SERVICE_TYPE_IDs = SERVICE_TYPE_IDs;
                ssFilter.HAS_EXECUTE = true;
                if (filter.EXCUTE_DEPARTMENT_ID!=null)
                {
                    ssFilter.TDL_EXECUTE_DEPARTMENT_ID = filter.EXCUTE_DEPARTMENT_ID;
                }
                if (filter.EXCUTE_DEPARTMENT_IDs != null)
                {
                    ssFilter.TDL_EXECUTE_DEPARTMENT_IDs = filter.EXCUTE_DEPARTMENT_IDs;
                }
                if (filter.EXCUTE_ROOM_ID != null)
                {
                    ssFilter.TDL_EXECUTE_ROOM_ID = filter.EXCUTE_ROOM_ID;
                }
                if (filter.EXCUTE_ROOM_IDs != null)
                {
                    ssFilter.TDL_EXECUTE_ROOM_IDs = filter.EXCUTE_ROOM_IDs;
                }
                if (filter.REQ_DEPARTMENT_ID != null)
                {
                    ssFilter.TDL_REQUEST_DEPARTMENT_ID = filter.REQ_DEPARTMENT_ID;
                }
                if (filter.REQ_DEPARTMENT_IDs != null)
                {
                    ssFilter.TDL_REQUEST_DEPARTMENT_IDs = filter.REQ_DEPARTMENT_IDs;
                }
                if (filter.REQ_ROOM_ID != null)
                {
                    ssFilter.TDL_REQUEST_ROOM_ID = filter.REQ_ROOM_ID;
                }
                if (filter.EXCUTE_ROOM_IDs != null)
                {
                    ssFilter.TDL_REQUEST_ROOM_IDs = filter.REQ_ROOM_IDs;
                }
                ListSereServ = new HisSereServManager().Get(ssFilter);
                var sereServIds = ListSereServ.Select(x => x.ID).Distinct().ToList();
                var skip = 0;
                while (sereServIds.Count-skip>0)
                {
                    var limitIds = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServFilterQuery ssExpendFilter = new HisSereServFilterQuery();
                    ssExpendFilter.PARENT_IDs = limitIds;
                    ssExpendFilter.IS_EXPEND = true;
                    var ssExpends = new HisSereServManager().Get(ssExpendFilter);
                    ListSereServExpend.AddRange(ssExpends);
                    HisSereServBillFilterQuery ssBillFilter = new HisSereServBillFilterQuery();
                    ssBillFilter.SERE_SERV_IDs = limitIds;
                    ssBillFilter.IS_NOT_CANCEL = true;
                    var ssBills = new HisSereServBillManager().Get(ssBillFilter);
                    ListSereServBill.AddRange(ssBills);
                }
                var ssBillIds = ListSereServBill.Select(x => x.BILL_ID).Distinct().ToList();
                skip = 0;
                while (ssBillIds.Count-skip>0)
                {
                    var limitIds = ssBillIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisTransactionFilterQuery tranFilter = new HisTransactionFilterQuery();
                    tranFilter.IDs = limitIds;
                    tranFilter.IS_CANCEL = false;
                    var trans = new HisTransactionManager().Get(tranFilter);
                    ListTransaction.AddRange(trans);
                }
                result = true;
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
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var item in ListSereServ)
                    {
                        Mrs00821RDO rdo = new Mrs00821RDO();
                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.PRICE = item.PRICE;
                        rdo.VIR_TOTAL_PRICE = item.PRICE * item.AMOUNT;
                        var expend = ListSereServExpend.Where(x => x.PARENT_ID == item.ID && x.VIR_TOTAL_PRICE_NO_EXPEND!=null).ToList();
                        rdo.EXPEND_PRICE = expend.Sum(x => x.VIR_TOTAL_PRICE_NO_EXPEND??0);
                        rdo.TOTAL_EARN_PRICE = rdo.VIR_TOTAL_PRICE - rdo.EXPEND_PRICE;
                        ListRdo.Add(rdo);
                    }
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
           dicSingleTag.Add("TIME_FROM",Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
           dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
           objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
