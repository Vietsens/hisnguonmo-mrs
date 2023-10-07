using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisCashout;
using AutoMapper; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExecuteRoom; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using SDA.EFMODEL; 
using SDA.Filter; 
using MOS.MANAGER.HisTransaction; 

namespace MRS.Processor.Mrs00414
{
    class Mrs00414Processor : AbstractProcessor
    {
        List<Mrs00414RDO> ListRdo = new List<Mrs00414RDO>(); 



        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>(); 
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>(); 
        

        public Mrs00414Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00414Filter); 
        }


        protected override bool GetData()
        {
            var filter = ((Mrs00414Filter)reportFilter); 
            bool result = true; 
            try
            {
                // Dich vu thuộc nhóm dinh dưỡng
                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery(); 
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00414"; 
                var listServiceRetCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(retyCastFilter); 

                var listServiceIds = listServiceRetCats.Select(s => s.SERVICE_ID).ToList(); 

                List<HIS_CASHOUT> listCashouts = new List<HIS_CASHOUT>(); 
                var skip = 0; 
                while (listServiceIds.Count - skip > 0)
                {
                    var listIds = listServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisCashoutFilterQuery cashoutFilter = new HisCashoutFilterQuery(); 
                    cashoutFilter.CASHOUT_TIME_FROM = filter.TIME_FROM; 
                    cashoutFilter.CASHOUT_TIME_TO = filter.TIME_TO; 
                    listCashouts.AddRange(new MOS.MANAGER.HisCashout.HisCashoutManager(param).Get(cashoutFilter));  
                }

                List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>(); 
                skip = 0; 
                while (listCashouts.Count - skip > 0)
                {
                    var listIds = listCashouts.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTransactionViewFilterQuery transactionViewFilter = new HisTransactionViewFilterQuery(); 
                    transactionViewFilter.CASHOUT_IDs = listIds.Select(s => s.ID).ToList(); 
                    listTransactions.AddRange(new HisTransactionManager(param).GetView(transactionViewFilter));  
                }

                skip = 0; 
                while (listTransactions.Count - skip > 0)
                {
                    var listIds = listTransactions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTransactionViewFilterQuery billViewFilter = new HisTransactionViewFilterQuery(); 
                    billViewFilter.IDs = listIds.Select(s => s.ID).ToList(); 
                    listBills.AddRange(new HisTransactionManager(param).GetView(billViewFilter));  
                }

                skip = 0; 
                while (listBills.Count - skip > 0)
                {
                    var listIds = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServBillViewFilterQuery sereServBillViewFilter = new HisSereServBillViewFilterQuery(); 
                    sereServBillViewFilter.BILL_IDs = listIds.Select(s => s.ID).ToList(); 
                    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillViewFilter));  
                }

                List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
                skip = 0; 
                while (listSereServBills.Count - skip > 0)
                {
                    var listIds = listSereServBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                    sereServViewFilter.IDs = listSereServBills.Select(s => s.SERE_SERV_ID).ToList();
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter));  
                }

                listSereServs = listSereServs.Where(s => listServiceIds.Contains(s.SERVICE_ID)).ToList(); 

                listSereServBills = listSereServBills.Where(w => listSereServs.Select(s => s.ID).Contains(w.SERE_SERV_ID)).ToList(); 
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
                var filter = ((Mrs00414Filter)reportFilter); 

                foreach (var sereSereBill in listSereServBills)
                {
                    var rdo = new Mrs00414RDO(); 
                    var bill = listBills.Where(w => w.ID == sereSereBill.BILL_ID).ToList(); 
                    if (IsNotNullOrEmpty(bill))
                    {
                        rdo.PATIENT_CODE = bill.First().TDL_PATIENT_CODE; 
                        rdo.PATIENT_NAME = bill.First().TDL_PATIENT_NAME; 

                        rdo.TREATMENT_ID = bill.First().TREATMENT_ID??0; 
                        rdo.TREATMENT_CODE = bill.First().TREATMENT_CODE; 

                        rdo.TRANSACTION_CODE = bill.First().TRANSACTION_CODE;        // mã giao dịch

                        if (sereSereBill.TDL_BILL_TYPE_ID == HisBillTypeCFG.bill_type_id_02)
                        {
                            rdo.BILL_02_NUMBER = bill.First().NUM_ORDER.ToString(); 
                            // transaction_code: mã giao dịch
                            // num_order: số phiếu trong sổ
                            rdo.BILL_02_AMOUNT = sereSereBill.AMOUNT; 
                            rdo.BILL_02_PRICE = sereSereBill.PRICE; 
                        }
                        else
                        {
                            rdo.BILL_01_NUMBER = bill.First().NUM_ORDER.ToString(); 
                            rdo.BILL_01_AMOUNT = sereSereBill.AMOUNT; 
                            rdo.BILL_01_PRICE = sereSereBill.PRICE; 
                        }
                    }
                    
                    ListRdo.Add(rdo); 
                }

                ListRdo = ListRdo.GroupBy(g => g.TREATMENT_ID).Select(s => new Mrs00414RDO
                {
                    PATIENT_CODE = s.First().PATIENT_CODE,
                    PATIENT_NAME = s.First().PATIENT_NAME,

                    TREATMENT_ID = s.First().TREATMENT_ID,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,

                    TRANSACTION_CODE = String.Join(", ", ListRdo.Where(w => w.TREATMENT_ID == s.First().TREATMENT_ID).Select(ss => ss.TRANSACTION_CODE).ToArray()),

                    BILL_01_NUMBER = String.Join(", ", ListRdo.Where(w => w.TREATMENT_ID == s.First().TREATMENT_ID).Select(ss => ss.BILL_01_NUMBER).ToArray()),
                    BILL_01_AMOUNT = s.Sum(su => su.BILL_01_AMOUNT),
                    BILL_01_PRICE = s.Sum(su => su.BILL_01_PRICE),

                    BILL_02_NUMBER = String.Join(", ", ListRdo.Where(w => w.TREATMENT_ID == s.First().TREATMENT_ID).Select(ss => ss.BILL_02_NUMBER).ToArray()),
                    BILL_02_AMOUNT = s.Sum(su => su.BILL_02_AMOUNT),
                    BILL_02_PRICE = s.Sum(su => su.BILL_02_PRICE)
                }).ToList(); 

            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (((Mrs00414Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00414Filter)reportFilter).TIME_FROM)); 
                }
                if (((Mrs00414Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00414Filter)reportFilter).TIME_TO)); 
                }

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo); 
                //exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup.OrderBy(s => s.DEPARTMENT_NAME).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

}
