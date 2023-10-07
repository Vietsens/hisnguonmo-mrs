using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisBillFund;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using System.Threading;

namespace MRS.Processor.Mrs00685
{
    public class Mrs00685Processor : AbstractProcessor
    {
        Mrs00685Filter filter = new Mrs00685Filter();
        CommonParam paramGet = new CommonParam();
        //List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>(); 
        //List<HIS_SESE_DEPO_REPAY> ListSeseDepoRepay = new List<HIS_SESE_DEPO_REPAY>(); 

      
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        HIS_BRANCH Branch = new HIS_BRANCH();

        public Mrs00685Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00685Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00685Filter)this.reportFilter;
                //get dữ liệu:
             

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

               
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }
            return result;
        }

      

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());

            if (IsNotNull(filter.ACCOUNT_BOOK_ID))
            {
                var AccountBook = new HisAccountBookManager().GetById((long)filter.ACCOUNT_BOOK_ID);
                dicSingleTag.Add("ACCOUNT_BOOK_CODE_NAME", AccountBook.ACCOUNT_BOOK_CODE + " - " + AccountBook.ACCOUNT_BOOK_NAME);
                dicSingleTag.Add("ACCOUNT_BOOK_CREATOR", AccountBook.CREATOR);
            }
            

            var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { LOGINNAME = filter.CASHIER_LOGINNAME });
            dicSingleTag.Add("CASHIER_USERNAME", (acsUser.FirstOrDefault(o => o.LOGINNAME == filter.CASHIER_LOGINNAME) ?? new ACS_USER()).USERNAME);
            dicSingleTag.Add("CASHIER_LOGINNAME", filter.CASHIER_LOGINNAME ?? "");
            dicSingleTag.Add("CASHIER_ROOM_NAME", ((new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery()) ?? new List<V_HIS_CASHIER_ROOM>()).FirstOrDefault(o => o.ID == filter.EXACT_CASHIER_ROOM_ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);

            objectTag.AddObjectData(store, "Report", new ManagerSql().Get(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)));

            objectTag.AddObjectData(store, "Parent", new ManagerSql().Get(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16)));

            objectTag.AddRelationship(store, "Parent", "Report", "PARENT_SERVICE_NAME", "PARENT_SERVICE_NAME");
        }

      
    }
}