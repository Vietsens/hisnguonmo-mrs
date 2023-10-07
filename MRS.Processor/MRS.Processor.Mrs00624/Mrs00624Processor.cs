using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00624
{
    class Mrs00624Processor : AbstractProcessor
    {
        Mrs00624Filter castFilter = null;
        List<Mrs00624RDO> ListRdo = new List<Mrs00624RDO>();

        List<Mrs00624RDO> ListCurrentTransaction = new List<Mrs00624RDO>();
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();

        public Mrs00624Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00624Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00624Filter)this.reportFilter;

                ListCurrentTransaction = new ManagerSql().GetTransaction(castFilter);
                GetDataDepartmentTran(ListCurrentTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList());
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GetDataDepartmentTran(List<long> treatmentIds)
        {
            if (treatmentIds.Count > 0)
            {
                treatmentIds = treatmentIds.Distinct().ToList();
                var skip = 0;
                while (treatmentIds.Count - skip > 0)
                {
                    var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    // xuất chuyển kho
                    HisDepartmentTranFilterQuery listHisDepartmentTranFilter = new HisDepartmentTranFilterQuery();
                    listHisDepartmentTranFilter.TREATMENT_IDs = listIds;
                    listHisDepartmentTranFilter.ORDER_FIELD = "ID";
                    listHisDepartmentTranFilter.ORDER_DIRECTION = "DESC";
                    listHisDepartmentTran.AddRange(new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(param).Get(listHisDepartmentTranFilter));
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (ListCurrentTransaction != null && ListCurrentTransaction.Count > 0)
                {
                    ListRdo = (from r in ListCurrentTransaction select new Mrs00624RDO(r, listHisDepartmentTran, castFilter)).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery() { LOGINNAME = castFilter.CANCEL_LOGINNAME });
                dicSingleTag.Add("CANCEL_USERNAME", (acsUser.FirstOrDefault(o=>o.LOGINNAME==castFilter.CANCEL_LOGINNAME) ?? new ACS_USER()).USERNAME);
                dicSingleTag.Add("CANCEL_LOGINNAME", castFilter.CANCEL_LOGINNAME ?? "");
                dicSingleTag.Add("CASHIER_ROOM_NAME", ((new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery())??new List<V_HIS_CASHIER_ROOM>()).FirstOrDefault(o=>o.ID== castFilter.EXACT_CASHIER_ROOM_ID) ??new V_HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);
                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TRANSACTION_CODE).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
