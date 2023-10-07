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

namespace MRS.Processor.Mrs00668
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00668Processor : AbstractProcessor
    {
        Mrs00668Filter castFilter = new Mrs00668Filter();
        List<DEPOSIT> listDeposit = new List<DEPOSIT>();

        List<BILL> listBill = new List<BILL>();


        List<TREATMENT> listTreatment = new List<TREATMENT>();


        public Mrs00668Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00668Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00668Filter)this.reportFilter;
                listTreatment = new ManagerSql().GetTreatment(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15)) ?? new List<TREATMENT>();
                listDeposit = new ManagerSql().GetDeposit(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16)) ?? new List<DEPOSIT>();
                listBill = new ManagerSql().GetBill(castFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 17)) ?? new List<BILL>();
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
                foreach (var item in listTreatment)
                {

                    var listSubDeposit = listDeposit.Where(o => o.TREATMENT_ID == item.TREATMENT_ID).ToList();
                    
                    var listSubBill = listBill.Where(o => o.TREATMENT_ID == item.TREATMENT_ID).ToList();
                    if (listSubBill.Count > 0)
                    {
                        item.BILL1=listSubBill[0];
                    }
                    if (listSubBill.Count > 1)
                    {
                        item.BILL2 = listSubBill[1];
                    }
                    if (listSubBill.Count > 2)
                    {
                        item.BILL3 = listSubBill[2];
                    }
                    
                }
                listTreatment = listTreatment.Where(o => listDeposit.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID)).ToList();
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

                HisCashierRoomViewFilterQuery cashierRoomfilter = new HisCashierRoomViewFilterQuery();
                cashierRoomfilter.ID = this.castFilter.EXACT_CASHIER_ROOM_ID;
                var listCashierRooms = new HisCashierRoomManager(param).GetView(cashierRoomfilter) ?? new List<V_HIS_CASHIER_ROOM>();
                var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { LOGINNAME = castFilter.CASHIER_LOGINNAME });
                dicSingleTag.Add("CASHIER_USERNAME", (acsUser.FirstOrDefault(o => o.LOGINNAME == castFilter.CASHIER_LOGINNAME) ?? new ACS_USER()).USERNAME);
                dicSingleTag.Add("CASHIER_ROOM_NAME", (listCashierRooms.FirstOrDefault(o => o.ID == castFilter.EXACT_CASHIER_ROOM_ID) ?? new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);

                objectTag.AddObjectData(store, "Report", listDeposit);

                objectTag.AddObjectData(store, "Treatment", listTreatment);

                objectTag.AddRelationship(store, "Treatment", "Report", "TREATMENT_ID", "TREATMENT_ID");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
