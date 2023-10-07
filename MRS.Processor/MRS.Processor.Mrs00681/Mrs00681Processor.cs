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

namespace MRS.Processor.Mrs00681
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00681Processor : AbstractProcessor
    {
        Mrs00681Filter castFilter = new Mrs00681Filter();
        Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();

        List<List<DataTable>> dataObject = new List<List<DataTable>>();

        protected string BRANCH_NAME = "";

        public Mrs00681Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00681Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00681Filter)this.reportFilter;

                if (new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell<Mrs00681Filter>(ref dicSingleKey, ref dataObject, this.castFilter, this.dicDataFilter, this.reportTemplate.REPORT_TEMPLATE_URL, 15))
                {
                    return true;
                }
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
                if (dicSingleKey != null && dicSingleKey.Count > 0)
            {
                foreach (var item in dicSingleKey)
                {
                    if (!dicSingleTag.ContainsKey(item.Key))
                    {
                        dicSingleTag.Add(item.Key, item.Value);
                    }
                    else
                    {
                        dicSingleTag[item.Key] = item.Value;
                    }
                }
            }

                if (dataObject.Count > 0)
                {
                    for (int i = 0; i < 15; i++)
                    {
                        objectTag.AddObjectData(store, "Report" + i, dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
