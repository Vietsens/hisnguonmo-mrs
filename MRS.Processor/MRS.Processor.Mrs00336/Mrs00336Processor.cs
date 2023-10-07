using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;

namespace MRS.Processor.Mrs00336
{
    class Mrs00336Processor : AbstractProcessor
    {
        Mrs00336Filter castFilter = null;

        List<Mrs00336RDO> listRdo = new List<Mrs00336RDO>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        //List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();  // Ds thông tin HSBA
        List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
        List<HIS_TRANSACTION> listTransactionHU = new List<HIS_TRANSACTION>();
        List<Mrs00336RDO> listRdoHU = new List<Mrs00336RDO>();
        List<HIS_TRANSACTION> listDepositBill = new List<HIS_TRANSACTION>();
        List<HIS_TRANSACTION> listBill = new List<HIS_TRANSACTION>();
        public Mrs00336Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00336Filter);
        }

        //get dữ liệu từ data base
        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00336Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                HisTransactionFilterQuery repay = new HisTransactionFilterQuery();
                HisTransactionViewFilterQuery ListTransactionFilter = new HisTransactionViewFilterQuery();
                ListTransactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                ListTransactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                ListTransactionFilter.IS_CANCEL = false;
                ListTransactionFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU };
                listTransaction = new HisTransactionManager(paramGet).GetView(ListTransactionFilter);
                if (castFilter.ACCOUNT_BOOK_IDs != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.ACCOUNT_BOOK_IDs.Contains(o.ACCOUNT_BOOK_ID)).ToList();
                }
                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }
                if (castFilter.LOGINNAMEs != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.LOGINNAMEs.Contains(o.CASHIER_LOGINNAME)).ToList();
                }

                //HisTransactionViewFilterQuery ListBillFilter = new HisTransactionViewFilterQuery();
                //ListBillFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                //ListBillFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                //ListBillFilter.IS_CANCEL = false;
                //ListBillFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                //listDepositBill = new HisTransactionManager(paramGet).GetView(ListBillFilter);
                if (IsNotNullOrEmpty(listTransaction))
                {
                    // lọc ID bảng treatment
                    var listTreatmentID = listTransaction.Select(o => o.TREATMENT_ID ?? 0).GroupBy(p => p).Select(q => q.First()).ToList();
                    // lọc danh sách bệnh nhân hoàn ứng
                    var skip = 0;
                    while (listTreatmentID.Count - skip > 0)
                    {
                        var listIds = listTreatmentID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var FilterHisTreatmentFilterQuery = new HisTreatmentViewFilterQuery
                        {
                            IDs = listIds
                        };
                        var listTreatmenttionSub = new HisTreatmentManager(paramGet).GetView(FilterHisTreatmentFilterQuery);
                        listTreatment.AddRange(listTreatmenttionSub);
                        //if (castFilter.IS_RP_OTHER_DATE == true)
                        {
                            var FilterDepositBillFilterQuery = new HisTransactionFilterQuery
                            {
                                TREATMENT_IDs = listIds,
                                IS_CANCEL = false,
                                TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU
                            };
                            var listDepositBillSub = new HisTransactionManager(paramGet).Get(FilterDepositBillFilterQuery);
                            
                            listDepositBill.AddRange(listDepositBillSub);
                        }

                        HisTransactionFilterQuery billFilter = new HisTransactionFilterQuery();
                        billFilter.TREATMENT_IDs = listIds;
                        billFilter.IS_CANCEL = false;
                        billFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                        var listBillSub = new HisTransactionManager(paramGet).Get(billFilter);
                        if (listBillSub != null)
                            listBill.AddRange(listBillSub);
                    }
                }

                if ((this.castFilter.PATIENT_TYPE_ID ?? 0) != 0)
                {
                    var treatmentIds = listTreatment.Where(o => o.TDL_PATIENT_TYPE_ID == this.castFilter.PATIENT_TYPE_ID).Select(p=>p.ID).ToList();
                    listTransaction = listTransaction.Where(o => treatmentIds.Contains(o.TREATMENT_ID??0)).ToList();
                }

                //chỉ lấy những giao dịch hoàn ứng
                //listTransactionHU = new ManagerSql().GetTransaction(castFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        
        //Xử lý dữ liệu để tạo listRdo
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                listRdo.Clear();
                listRdoHU.Clear();
                foreach (var r in listTransaction)
                {
                     V_HIS_TREATMENT treatement = listTreatment.FirstOrDefault(o => o.ID == r.TREATMENT_ID);
                    List<HIS_TRANSACTION> depositBillSub = listDepositBill.Where(o => o.TREATMENT_ID == r.TREATMENT_ID).ToList();
                    List<HIS_TRANSACTION> billSub = listBill.Where(o => o.TREATMENT_ID == r.TREATMENT_ID).ToList();
                    Mrs00336RDO rdo = new Mrs00336RDO(r, depositBillSub, billSub, treatement);
                   
                    listRdo.Add(rdo);
                }
                listRdoHU = listRdo.Where(o => o.BILL_AMOUNT == 0).ToList();

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdo.Clear();
                listRdoHU.Clear();
            }
            return result;
        }

        // xuất ra báo cáo
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                List<Mrs00336RDO> listRdoParent = listRdo.GroupBy(o => o.CASHIER_LOGINNAME).Select(s => s.First()).ToList();

                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.BILL_TIME).ThenBy(x=>x.NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "ReportParent", listRdoParent.OrderBy(o => o.CASHIER_LOGINNAME).ThenBy(x=>x.NUM_ORDER).ToList());
                objectTag.AddRelationship(store, "ReportParent", "Report", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

                objectTag.AddObjectData(store, "RepayOtherDate", listRdo.Where(p => p.IS_RP_OTHERDATE).OrderBy(o => o.BILL_TIME).ToList());
                objectTag.AddObjectData(store, "CsRepayOtherDate", listRdo.Where(p => p.IS_RP_OTHERDATE).GroupBy(o => o.CASHIER_LOGINNAME).Select(s => s.First()).OrderBy(o => o.CASHIER_LOGINNAME).ToList());
                objectTag.AddRelationship(store, "CsRepayOtherDate", "RepayOtherDate", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");

                List<Mrs00336RDO> listRdoHUParent = listRdoHU.GroupBy(o => o.CASHIER_LOGINNAME).Select(s => s.First()).ToList();
                objectTag.AddObjectData(store,"NoRepay",listRdoHU.OrderBy(x=>x.BILL_TIME).ThenBy(x=>x.NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "NoRepayParent", listRdoHUParent.OrderBy(o => o.CASHIER_LOGINNAME).ToList());
                objectTag.AddRelationship(store, "NoRepayParent", "NoRepay", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
