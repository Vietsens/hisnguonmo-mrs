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

namespace MRS.Processor.Mrs00669
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00669Processor : AbstractProcessor
    {
        Mrs00669Filter castFilter = new Mrs00669Filter();

        protected string BRANCH_NAME = "";

        public Mrs00669Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00669Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00669Filter)this.reportFilter;
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

                   objectTag.AddObjectData(store, "Report", new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)));
               objectTag.AddObjectData(store, "Keys", new ManagerSql().GetSum(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16)));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
       
    }
    //public class HIS_TRANSACTION_E:HIS_TRANSACTION
    //{
    //    public string DEPARTMENT_NAME { get; set; }
    
    //}
}
