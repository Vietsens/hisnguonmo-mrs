using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisPatient;

namespace MRS.Processor.Mrs00516
{
    class Mrs00516Processor : AbstractProcessor
    {
        List<Mrs00516RDO> ListRdoDetail = new List<Mrs00516RDO>();
        List<Mrs00516RDO> ListRdoDetailNew = new List<Mrs00516RDO>();
        List<Mrs00516RDO> ListRdo = new List<Mrs00516RDO>();
        List<Mrs00516RDO> ListRdoDetailAddBillOther = new List<Mrs00516RDO>();
        List<Mrs00516RDO> ListRdoAddBillOther = new List<Mrs00516RDO>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_TREATMENT_TYPE> listTreatmentType = new List<HIS_TREATMENT_TYPE>();
        List<V_HIS_CASHIER_ROOM> ListCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        List<V_HIS_TRANSACTION> ListCurrentTransaction = new List<V_HIS_TRANSACTION>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_ACCOUNT_BOOK> ListAccountBook = new List<HIS_ACCOUNT_BOOK>();
        private DataTable listRdo = new DataTable();
        private Dictionary<string, string> dicSingleKey = new Dictionary<string, string>();
        private string querry;
        List<List<DataTable>> dataObject = new List<List<DataTable>>();
        public Mrs00516Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00516Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00516Filter)reportFilter);
            bool result = true;
            try
            {
                if (new MRS.MANAGER.Core.MrsReport.Lib.ProcessExcel().GetByCell<Mrs00516Filter>(ref dicSingleKey, ref dataObject, filter, this.reportTemplate.REPORT_TEMPLATE_URL, 15))
                {
                    return true;
                }
                ListAccountBook = new HisAccountBookManager().Get(new HisAccountBookFilterQuery());
                HisTransactionViewFilterQuery filterTransaction = new HisTransactionViewFilterQuery();
                filterTransaction.TRANSACTION_TIME_FROM = filter.TIME_FROM;
                filterTransaction.TRANSACTION_TIME_TO = filter.TIME_TO;
                if (filter.ACCOUNT_BOOK_IDs!=null)
	            {
		            filterTransaction.ACCOUNT_BOOK_IDs = filter.ACCOUNT_BOOK_IDs;
	            }
                //filterTransaction.IS_CANCEL = false;
                //filterTransaction.HAS_SALL_TYPE = false;
                ListCurrentTransaction = new HisTransactionManager(param).GetView(filterTransaction);
                if(filter.PAY_FORM_IDs !=null)
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID ??0)).ToList();
                }
              

                var listreatmentIds = ListCurrentTransaction.Select(o => o.TREATMENT_ID??0).Distinct().ToList();
                if (IsNotNullOrEmpty(listreatmentIds))
                {
                    var skip = 0;
                    while (listreatmentIds.Count - skip > 0)
                    {
                        var Ids = listreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranFilterQuery departmentTranFilter = new HisDepartmentTranFilterQuery();
                        departmentTranFilter.TREATMENT_IDs = Ids;
                        //departmentTranFilter.DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                        departmentTranFilter.ORDER_DIRECTION = "ASC";
                        departmentTranFilter.ORDER_FIELD = "ID";
                        var listDepartmentTranSub = new HisDepartmentTranManager(param).Get(departmentTranFilter);
                        if (listDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listDepartmentTranSub GetView null");
                        else
                            ListDepartmentTran.AddRange(listDepartmentTranSub);
                    }         
                }

                if (filter.BRANCH_IDs != null)
                {



                    var CashierRoomIDs = (new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery() { BRANCH_IDs = filter.BRANCH_IDs }) ?? new List<V_HIS_CASHIER_ROOM>()).Select(o => o.ID).ToList();

                    ListCurrentTransaction = ListCurrentTransaction.Where(o => CashierRoomIDs.Contains(o.CASHIER_ROOM_ID)).ToList();


                }

                if (filter.BRANCH_ID != null)
                {



                    var CashierRoomIDs = (new HisCashierRoomManager().GetView(new HisCashierRoomViewFilterQuery() { BRANCH_ID = filter.BRANCH_ID }) ?? new List<V_HIS_CASHIER_ROOM>()).Select(o => o.ID).ToList();

                    ListCurrentTransaction = ListCurrentTransaction.Where(o => CashierRoomIDs.Contains(o.CASHIER_ROOM_ID)).ToList();


                }

               

                

              
                
                ListDepartmentTran = ListDepartmentTran.OrderBy(o => o.DEPARTMENT_IN_TIME ?? 0).ToList();
                listPatientTypeAlter = new HisPatientTypeAlterManager(new CommonParam()).GetByTreatmentIds(listreatmentIds);

                ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.PATIENT_TYPE_IDs == null || listPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && filter.PATIENT_TYPE_IDs.Contains( p.PATIENT_TYPE_ID))).ToList();
                
                ListCurrentTransaction = ListCurrentTransaction.Where(o =>
                    filter.PATIENT_TYPE_ID == null
                    || listPatientTypeAlter.Exists(p =>
                        p.TREATMENT_ID == o.TREATMENT_ID
                        && p.PATIENT_TYPE_ID == filter.PATIENT_TYPE_ID)).ToList();

                if (filter.CASHIER_LOGINNAME != null) ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();

                if (IsNotNullOrEmpty(filter.CASHIER_LOGINNAMEs)) ListCurrentTransaction = ListCurrentTransaction.Where(o => filter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();

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
            var result = true;
            try
            {
                ListRdo.Clear();

                if (dataObject.Count > 0)
                {
                    return true;
                }
                if (ListCurrentTransaction != null && ListCurrentTransaction.Count > 0)
                {
                    ListRdoDetail = (from r in ListCurrentTransaction select new Mrs00516RDO(r, listPatientTypeAlter, ListDepartmentTran, ListAccountBook)).ToList();
                    ListRdoDetailAddBillOther = ListRdoDetail.Where(o => o.SALE_TYPE_ID == null || o.SALE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_OTHER).ToList();
                    ListRdoDetailNew = ListRdoDetail;
                    ListRdoDetail = ListRdoDetail.Where(o => o.SALE_TYPE_ID == null).ToList();

                    GroupByCashier();
                    GroupByCashierAddBillOther();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void GroupByCashier()
        {
            string errorField = "";
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.CASHIER_LOGINNAME,o.CASHIER_USERNAME }).ToList();
                decimal sum = 0;
                Mrs00516RDO rdo;
                List<Mrs00516RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00516RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00516RDO();
                    listSub = item.ToList<Mrs00516RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByCashierAddBillOther()
        {
            string errorField = "";
            try
            {
                var group = ListRdoDetailAddBillOther.GroupBy(o => new { o.CASHIER_LOGINNAME }).ToList();
                decimal sum = 0;
                Mrs00516RDO rdo;
                List<Mrs00516RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00516RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00516RDO();
                    listSub = item.ToList<Mrs00516RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL") || field.Name.Contains("AMOUNT"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoAddBillOther.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private Mrs00516RDO IsMeaningful(List<Mrs00516RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00516RDO();
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00516Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00516Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00516Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00516Filter)reportFilter).TIME_TO));
            }


            if (dataObject.Count > 0)
            {
                for (int i = 0; i < 15; i++)
                {
                    objectTag.AddObjectData(store, "Report" + i, dataObject[i].Count > 0 ? dataObject[i][0] : new DataTable());
                    objectTag.AddObjectData(store, "Parent" + i, dataObject[i].Count > 1 ? dataObject[i][1] : new DataTable());
                    objectTag.AddObjectData(store, "GrandParent" + i, dataObject[i].Count > 2 ? dataObject[i][2] : new DataTable());
                    objectTag.AddRelationship(store, "Parent" + i, "Report" + i, "PARENT_KEY", "PARENT_KEY");
                    objectTag.AddRelationship(store, "GrandParent" + i, "Parent" + i, "GRAND_PARENT_KEY", "GRAND_PARENT_KEY");
                }
            }
            objectTag.AddObjectData(store, "ReportAddBillOther", ListRdoAddBillOther);
            objectTag.AddObjectData(store, "ReportDetailAddBillOther", ListRdoDetailAddBillOther);
            objectTag.AddRelationship(store, "ReportAddBillOther", "ReportDetailAddBillOther", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            objectTag.AddRelationship(store, "Report", "ReportDetail", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

            objectTag.AddObjectData(store, "ReportNew", ListRdoDetailNew);
            objectTag.AddRelationship(store, "Report", "ReportNew", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

        }
    }
}
