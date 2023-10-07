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
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00481
{
    //báo cáo thanh toán nội trú chuyển khoản

    class Mrs00481Processor : AbstractProcessor
    {
        Mrs00481Filter castFilter = new Mrs00481Filter();
        List<Mrs00481RDO> listRdo = new List<Mrs00481RDO>();

        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();
        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();

        List<HIS_SERE_SERV_DEPOSIT> listSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();

        protected bool IS_BILL = false;
        protected bool IS_DEPOSIT = false;
        protected string TITLE_REPORT = "";
        protected string BRANCH_NAME = "";

        public Mrs00481Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00481Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00481Filter)this.reportFilter;
                //=======================================================================
                if (castFilter.IS_DEPOSIT != null && castFilter.IS_DEPOSIT == true && castFilter.IS_BILL == false)
                {
                    this.IS_DEPOSIT = true;
                    TITLE_REPORT = "TIỀN TẠM THU";
                }
                else
                {
                    castFilter.IS_BILL = true;
                    TITLE_REPORT = "CHUYỂN KHOẢN NỘI TRÚ";
                }

                var listRooms = new MOS.MANAGER.HisRoom.HisRoomManager(param).GetView(new HisRoomViewFilterQuery());

                if (IsNotNull(castFilter.BRANCH_ID))
                {
                    listRooms = listRooms.Where(w => w.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                    if (IsNotNullOrEmpty(listRooms))
                        this.BRANCH_NAME = " - " + listRooms.First().BRANCH_NAME.ToUpper();
                }
                //=======================================================================
                HisTransactionViewFilterQuery transactionViewFilter = new HisTransactionViewFilterQuery();
                transactionViewFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionViewFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                if (IS_DEPOSIT)
                    transactionViewFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                else
                {
                    transactionViewFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                    transactionViewFilter.HAS_SALL_TYPE = false;
                }
                transactionViewFilter.IS_CANCEL = false;
                listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactionViewFilter);

                listTransactions = listTransactions.Where(w => w.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK
                    && listRooms.Select(s => s.ID).Contains(w.CASHIER_ROOM_ID)).ToList();

                var skip = 0;
                var listPatyAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
                while (listTransactions.Count - skip > 0)
                {
                    var listIds = listTransactions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery();
                    patientTypeAlterFilter.TREATMENT_IDs = listIds.Select(s => s.TREATMENT_ID.Value).ToList();
                    patientTypeAlterFilter.TREATMENT_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU, IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU };
                    listPatyAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter));
                }

                listTransactions = listTransactions.Where(w => listPatyAlters.Select(s => s.TREATMENT_ID).Contains(w.TREATMENT_ID.Value)).ToList();
                // ===================================== tạm ứng
                if (this.IS_DEPOSIT)
                {
                    var listDeposits = new List<V_HIS_TRANSACTION>();
                    listDeposits = listTransactions.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();

                    skip = 0;
                    var listSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
                    while (listDeposits.Count - skip > 0)
                    {
                        var listIds = listDeposits.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServDepositFilterQuery SereServDepositFilter = new HisSereServDepositFilterQuery();
                        SereServDepositFilter.DEPOSIT_IDs = listIds.Select(s => s.ID).ToList();
                        listSereServDeposit.AddRange(new HisSereServDepositManager(param).Get(SereServDepositFilter));
                    }

                    var listTransactionIds = listDeposits.Where(w => listSereServDeposit.Select(s => s.DEPOSIT_ID).Contains(w.ID)).Select(s => s.ID).Distinct().ToList();

                    listTransactions = listTransactions.Where(w => !listTransactionIds.Contains(w.ID)).ToList();
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
                CommonParam paramGet = new CommonParam();

                foreach (var transaction in listTransactions)
                {
                    var rdo = new Mrs00481RDO();
                    rdo.TRANSACTION = transaction;
                    listRdo.Add(rdo);
                }
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

                dicSingleTag.Add("TITLE_REPORT", this.TITLE_REPORT);
                dicSingleTag.Add("BRANCH_NAME", this.BRANCH_NAME);

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
