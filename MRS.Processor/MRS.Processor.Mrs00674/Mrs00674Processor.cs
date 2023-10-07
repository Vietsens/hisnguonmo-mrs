using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SDA.Filter;
using SDA.EFMODEL;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using System.Data;
using System.Reflection;

namespace MRS.Processor.Mrs00674
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00674Processor : AbstractProcessor
    {
        Mrs00674Filter castFilter = new Mrs00674Filter();
        List<EXP_MEST_SUB> listExpMestSub = new List<EXP_MEST_SUB>();
        List<EXP_MEST_SUB> listSub = new List<EXP_MEST_SUB>();
        protected string BRANCH_NAME = "";

        public Mrs00674Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00674Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00674Filter)this.reportFilter;
                listSub = new ManagerSql().GetListExpMestSubCode(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)) ?? new List<EXP_MEST_SUB>();
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
                if (listSub.Count == 0) return true;
                listSub = listSub.OrderBy(o=>o.EXP_MEST_SUB_CODE).ToList();
                int count = (int)(listSub.Count/3)+1;
                for (int i = 0; i < count; i++)
                {
                    EXP_MEST_SUB rdo = new EXP_MEST_SUB();
                    if (3 * i < listSub.Count)
                    {
                        rdo.EXP_MEST_SUB_CODE = listSub[3 * i].EXP_MEST_SUB_CODE;
                        rdo.TDL_TOTAL_PRICE = listSub[3 * i].TDL_TOTAL_PRICE;
                        rdo.TRANSFER_AMOUNT = listSub[3 * i].TRANSFER_AMOUNT;
                    }
                    if (3 * i+1 < listSub.Count)
                    {
                        rdo.EXP_MEST_SUB_CODE_1 = listSub[3 * i + 1].EXP_MEST_SUB_CODE;
                        rdo.TDL_TOTAL_PRICE_1 = listSub[3 * i + 1].TDL_TOTAL_PRICE;
                        rdo.TRANSFER_AMOUNT_1 = listSub[3 * i + 1].TRANSFER_AMOUNT;
                    }
                    if (3 * i+2 < listSub.Count)
                    {
                        rdo.EXP_MEST_SUB_CODE_2 = listSub[3 * i + 2].EXP_MEST_SUB_CODE;
                        rdo.TDL_TOTAL_PRICE_2 = listSub[3 * i + 2].TDL_TOTAL_PRICE;
                        rdo.TRANSFER_AMOUNT_2 = listSub[3 * i + 2].TRANSFER_AMOUNT;
                    }
                    listExpMestSub.Add(rdo);
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                if (castFilter.EXACT_EXT_CASHIER_ROOM_ID != null)
                {
                    dicSingleTag.Add("CASHIER_ROOM_NAME", (new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery()).FirstOrDefault(o => castFilter.EXACT_EXT_CASHIER_ROOM_ID == o.ID) ?? new HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);
                    dicSingleTag.Add("EXACT_EXT_CASHIER_ROOM_ID", castFilter.EXACT_EXT_CASHIER_ROOM_ID);
                }
                if (castFilter.EXAM_ROOM_IDs != null)
                {
                    dicSingleTag.Add("REQ_ROOM_NAME", string.Join(" - ", HisRoomCFG.HisRooms.Where(o => castFilter.EXAM_ROOM_IDs.Contains(o.ID)).Select(p => p.ROOM_NAME).ToList()));
                }
                objectTag.AddObjectData(store, "Report", new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)));
                objectTag.AddObjectData(store, "Cancel", new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16)));
                objectTag.AddObjectData(store, "ExpMestSub", listExpMestSub);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
