using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
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
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00480
{
    //báo cáo hoạt động điều trị

    class Mrs00480Processor : AbstractProcessor
    {
        Mrs00480Filter castFilter = new Mrs00480Filter();
        List<Mrs00480RDO> listRdo = new List<Mrs00480RDO>();

        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>();
        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();

        protected bool TT_NOITRU = false;
        protected bool TT_TU_NGT = false;
        protected string BRANCH_NAME = "";

        public Mrs00480Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00480Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00480Filter)this.reportFilter;
                //=======================================================================
                var listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).GetView(new HisRoomViewFilterQuery());

                if (IsNotNull(castFilter.BRANCH_ID))
                {
                    listRooms = listRooms.Where(w => w.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                    if (IsNotNullOrEmpty(listRooms))
                        this.BRANCH_NAME = " - " + listRooms.First().BRANCH_NAME;
                }
                //=======================================================================
                HisTransactionViewFilterQuery transactionViewFilter = new HisTransactionViewFilterQuery();
                transactionViewFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionViewFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionViewFilter.IS_CANCEL = false;
                transactionViewFilter.HAS_SALL_TYPE = false;
                listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactionViewFilter);

                listTransactions = listTransactions.Where(w => w.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK
                    && listRooms.Select(s => s.ID).Contains(w.CASHIER_ROOM_ID)).ToList();

                GetDataTTTUNgT();
                // ====================
                listBills = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                var skip = 0;

                while (listBills.Count - skip > 0)
                {
                    var listIds = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServBillViewFilterQuery sereServBillViewFilter = new HisSereServBillViewFilterQuery();
                    sereServBillViewFilter.BILL_IDs = listIds.Select(s => s.ID).ToList();
                    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillViewFilter));
                }

                skip = 0;
                var listServiceIds = listSereServBills.Select(s => s.SERVICE_ID).Distinct().ToList();
                while (listServiceIds.Count - skip > 0)
                {
                    var listIds = listServiceIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery();
                    serviceFilter.IDs = listIds;
                    listServices.AddRange(new MOS.MANAGER.HisService.HisServiceManager(param).GetView(serviceFilter));
                }
                /*
                // ================= tạm ứng dịch vụ
                skip = 0; 
                while (listDeposits.Count - skip > 0)
                {
                    var listIds = listDeposits.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisDereDetailViewFilterQuery dereDetailViewFilter = new HisDereDetailViewFilterQuery(); 
                    dereDetailViewFilter.DEPOSIT_IDs = listIds.Select(s=>s.ID).ToList(); 
                    listDereDetails.AddRange(new MOS.MANAGER.HisDereDetail.HisDereDetailManager(param).GetView(dereDetailViewFilter)); 
                }
                 */
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
                CommonParam paramGet = new CommonParam();

                foreach (var transaction in listTransactions)
                {
                    var bills = listBills.Where(w => w.ID == transaction.ID).ToList();
                    if (IsNotNullOrEmpty(bills))
                    {
                        var sereServBills = listSereServBills.Where(w => bills.Select(s => s.ID).Contains(w.BILL_ID)).ToList();
                        foreach (var sereServBill in sereServBills)
                        {
                            var rdo = new Mrs00480RDO();
                            rdo.TRANSACTION = transaction;
                            rdo.AMOUNT = sereServBill.PRICE;
                            var service = listServices.Where(w => w.ID == sereServBill.SERVICE_ID).ToList();
                            if (IsNotNullOrEmpty(service))
                                rdo.SERVICE_NAME = service.First().SERVICE_NAME;
                            listRdo.Add(rdo);
                        }
                    }
                    //else
                    //{
                    //    var dereDetails = listDereDetails.Where(w => w.TRANSACTION_ID == transaction.ID).ToList(); 
                    //    foreach (var dereDetail in dereDetails)
                    //    {
                    //        var rdo = new Mrs00480RDO(); 
                    //        rdo.TRANSACTION = transaction; 
                    //        rdo.AMOUNT = dereDetail.VIR_TOTAL_PRICE ?? 0; 
                    //        rdo.SERVICE_NAME = dereDetail.SERVICE_NAME; 
                    //        listRdo.Add(rdo); 
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected void GetDataTTTUNgT()
        {
            var skip = 0;
            var listpatyAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
            var listTreatments = new List<V_HIS_TREATMENT>();
            while (listTransactions.Count - skip > 0)
            {
                var listIds = listTransactions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                treatmentViewFilter.IDs = listIds.Select(s => s.TREATMENT_ID.Value).ToList();
                treatmentViewFilter.IS_OUT = true;
                listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter));
            }

            skip = 0;
            while (listTreatments.Count - skip > 0)
            {
                var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisPatientTypeAlterViewFilterQuery patyAlterViewFilter = new HisPatientTypeAlterViewFilterQuery();
                patyAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                listpatyAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patyAlterViewFilter));
            }

            List<long> listTreatmentIds = new List<long>();
            var listPatyAlterGroupByTreatmentIds = listpatyAlters.GroupBy(g => g.TREATMENT_ID);
            foreach (var treatment in listPatyAlterGroupByTreatmentIds)
            {
                if (!IsNotNullOrEmpty(treatment.Where(w => w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU || w.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList()))
                    listTreatmentIds.Add(treatment.First().TREATMENT_ID);
            }

            listTransactions = listTransactions.Where(w => listTreatmentIds.Contains(w.TREATMENT_ID.Value)).ToList();
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

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo);
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
