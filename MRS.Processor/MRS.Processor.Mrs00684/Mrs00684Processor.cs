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

namespace MRS.Processor.Mrs00684
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00684Processor : AbstractProcessor
    {
        Mrs00684Filter castFilter = new Mrs00684Filter();

        protected string BRANCH_NAME = "";

        public Mrs00684Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00684Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00684Filter)this.reportFilter;
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

                var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { LOGINNAME = castFilter.CASHIER_LOGINNAME });
                dicSingleTag.Add("CASHIER_USERNAME", (acsUser.FirstOrDefault(o => o.LOGINNAME == castFilter.CASHIER_LOGINNAME) ?? new ACS_USER()).USERNAME);
                dicSingleTag.Add("CASHIER_LOGINNAME", castFilter.CASHIER_LOGINNAME ?? "");
                dicSingleTag.Add("CASHIER_ROOM_NAME", ((new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery()) ?? new List<V_HIS_CASHIER_ROOM>()).FirstOrDefault(o => o.ID == castFilter.EXACT_CASHIER_ROOM_ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);
                dicSingleTag.Add("TITLE", castFilter.IS_TREAT_IN.HasValue ? (castFilter.IS_TREAT_IN.Value==true?"NGOẠI TRÚ":"NỘI TRÚ") : "");
                objectTag.AddObjectData(store, "Report", new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
