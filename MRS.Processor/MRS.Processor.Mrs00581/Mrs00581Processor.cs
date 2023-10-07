using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisBillFund;
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
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00581
{
    class Mrs00581Processor : AbstractProcessor
    {
        Mrs00581Filter castFilter = null;

        List<Mrs00581RDO> listRdo = new List<Mrs00581RDO>();
        List<Mrs00581RDO> listPatientType = new List<Mrs00581RDO>();

        List<V_HIS_TRANSACTION> listBill = new List<V_HIS_TRANSACTION>();

        List<HIS_BILL_FUND> ListBillFund = new List<HIS_BILL_FUND>();
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        Dictionary<string, SERVICE> dicService = new Dictionary<string, SERVICE>();
        Dictionary<long, HIS_SERVICE> dicSv = new Dictionary<long, HIS_SERVICE>();
        List<V_HIS_SERVICE_RETY_CAT> ListHisServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<HIS_SERVICE> Services = new List<HIS_SERVICE>();

        List<BILL_BALANCE> ListBillBalance = new List<BILL_BALANCE>();

        string cashierUsername = "";

        public Mrs00581Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00581Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00581Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_BILL, V_HIS_DEPOSIT, V_HIS_REPAY, MRS00581: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();
                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                transactionFilter.ACCOUNT_BOOK_IDs = castFilter.ACCOUNT_BOOK_IDs;
                transactionFilter.TREATMENT_CODE__EXACT = castFilter.TREATMENT_CODE__EXACT;
                //transactionFilter.IS_CANCEL = false; 
                transactionFilter.HAS_SALL_TYPE = false;
                var listBillMain = new HisTransactionManager().GetView(transactionFilter);
                if (listBillMain != null)
                {
                    listBill.AddRange(listBillMain);
                }
                HisTransactionViewFilterQuery transactionCancelFilter = new HisTransactionViewFilterQuery();
                transactionCancelFilter.CANCEL_TIME_FROM = castFilter.TIME_FROM;
                transactionCancelFilter.CANCEL_TIME_TO = castFilter.TIME_TO;
                transactionCancelFilter.IS_CANCEL = true;
                transactionCancelFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                transactionCancelFilter.ACCOUNT_BOOK_IDs = castFilter.ACCOUNT_BOOK_IDs;
                transactionCancelFilter.TREATMENT_CODE__EXACT = castFilter.TREATMENT_CODE__EXACT;
                transactionCancelFilter.HAS_SALL_TYPE = false;
                var listBillCan = new HisTransactionManager().GetView(transactionCancelFilter);
                if (listBillCan != null)
                {
                    foreach (var item in listBillCan)
                    {
                        item.IS_CANCEL = 2;
                    }
                    listBill.AddRange(listBillCan);
                }

                if (castFilter.IS_CANCEL.HasValue)
                {
                    if (castFilter.IS_CANCEL == 0)
                    {
                        listBill = listBill.Where(o => o.IS_CANCEL == 2).ToList();
                    }
                    if (castFilter.IS_CANCEL == 1)
                    {
                        listBill = listBill.Where(o => o.IS_CANCEL == null).ToList();
                    }
                }

                if (castFilter.INPUT_DATA_ID_GUARANTEE_TYPEs != null)
                {
                    if (castFilter.INPUT_DATA_ID_GUARANTEE_TYPEs.Contains(1) && !castFilter.INPUT_DATA_ID_GUARANTEE_TYPEs.Contains(2))
                    {
                        //chỉ lấy giao dịch có bão lãnh
                        listBill = listBill.Where(o => o.TDL_BILL_FUND_AMOUNT > 0).ToList();
                    }
                    if (castFilter.INPUT_DATA_ID_GUARANTEE_TYPEs.Contains(2) && !castFilter.INPUT_DATA_ID_GUARANTEE_TYPEs.Contains(1))
                    {
                        //chỉ lấy giao dịch không có bão lãnh
                        listBill = listBill.Where(o => o.TDL_BILL_FUND_AMOUNT == null || o.TDL_BILL_FUND_AMOUNT == 0).ToList();
                    }
                }
                HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
                cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID;
                cashierRoomFilter.IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                cashierRoomFilter.ID = castFilter.EXACT_CASHIER_ROOM_ID;
                listCashierRoom = new MOS.MANAGER.HisCashierRoom.HisCashierRoomManager(paramGet).GetView(cashierRoomFilter) ?? new List<V_HIS_CASHIER_ROOM>();

                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                serviceFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                Services = new HisServiceManager(paramGet).Get(serviceFilter) ?? new List<HIS_SERVICE>();


                HisServiceRetyCatViewFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00581";
                ListHisServiceRetyCat = new HisServiceRetyCatManager(paramGet).GetView(ServiceRetyCatFilter);


                listBill = listBill.Where(o => listCashierRoom.Exists(p => p.ID == o.CASHIER_ROOM_ID)).ToList();
                if (castFilter.IS_EINVOICE_CREATED != null)
                {
                    if (castFilter.IS_EINVOICE_CREATED == true)
                    {
                        listBill = listBill.Where(o => !string.IsNullOrWhiteSpace(o.EINVOICE_NUM_ORDER)).ToList();
                    }
                    else
                    {
                        listBill = listBill.Where(o => string.IsNullOrWhiteSpace(o.EINVOICE_NUM_ORDER)).ToList();
                    }
                }

                if (castFilter.LOGINNAME != null)
                {
                    listBill = listBill.Where(o => o.IS_CANCEL != 2 && castFilter.LOGINNAME == o.CASHIER_LOGINNAME || o.IS_CANCEL == 2 && castFilter.LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                }

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    listBill = listBill.Where(o => o.IS_CANCEL != 2 && castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME || o.IS_CANCEL == 2 && castFilter.CASHIER_LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                }


                if (castFilter.LOGINNAMEs != null)
                {
                    listBill = listBill.Where(o => o.IS_CANCEL != 2 && castFilter.LOGINNAMEs.Contains(o.CASHIER_LOGINNAME) || o.IS_CANCEL == 2 && castFilter.LOGINNAMEs.Contains(o.CANCEL_LOGINNAME)).ToList();
                }

                if (castFilter.CASHIER_LOGINNAMEs != null)
                {
                    listBill = listBill.Where(o => o.IS_CANCEL != 2 && castFilter.CASHIER_LOGINNAMEs.Contains(o.CASHIER_LOGINNAME) || o.IS_CANCEL == 2 && castFilter.CASHIER_LOGINNAMEs.Contains(o.CANCEL_LOGINNAME)).ToList();
                }

                if (castFilter.PAY_FORM_IDs != null)
                {
                    listBill = listBill.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                }

                cashierUsername = String.Join(",", listBill.Select(o => o.CASHIER_USERNAME).Distinct().ToList());

                //danh sách quỹ
                GetBillFund();

                //danh sách hiện dư
                GetBillBalance();

                //dịch vụ
                GetSv();

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00581");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetSv()
        {
            dicSv = new HisServiceManager().Get(new HisServiceFilterQuery()).ToDictionary(o => o.ID);
        }

        private void GetBillFund()
        {
            if (this.castFilter.FUND_IDs != null)
            {
                this.ListBillFund = new ManagerSql().GetBillFund(this.castFilter);
                List<long> billIdsFund = this.ListBillFund.Select(o => o.BILL_ID).Distinct().ToList();
                listBill = listBill.Where(o => billIdsFund.Contains(o.ID)).ToList();
            }
        }

        private void GetBillBalance()
        {
            this.ListBillBalance = new ManagerSql().GetBalance(this.castFilter);
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                var treatmentIds = listBill.Where(p => p.TREATMENT_ID.HasValue).Select(o => o.TREATMENT_ID.Value).Distinct().ToList();
                if (treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        List<V_HIS_TRANSACTION> listBillLocal = listBill.Where(o => listIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                        List<HIS_SERE_SERV_BILL> listHisSereServBill = new List<HIS_SERE_SERV_BILL>();
                        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
                        List<HIS_TRANSACTION> listDeposit = new List<HIS_TRANSACTION>();
                        List<HIS_TRANSACTION> listRepay = new List<HIS_TRANSACTION>();
                        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();

                        HisSereServBillFilterQuery listHisSereServBillFilter = new HisSereServBillFilterQuery();
                        listHisSereServBillFilter.TDL_TREATMENT_IDs = listIds;
                        listHisSereServBill = new HisSereServBillManager(param).Get(listHisSereServBillFilter) ?? new List<HIS_SERE_SERV_BILL>();

                        listHisSereServBill = listHisSereServBill.Where(o => listBillLocal.Exists(p => p.ID == o.BILL_ID)).ToList();
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            listHisSereServBill = listHisSereServBill.Where(o => castFilter.PATIENT_TYPE_IDs.Exists(p => p == o.TDL_PATIENT_TYPE_ID)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_ID != null)
                        {
                            listHisSereServBill = listHisSereServBill.Where(o => castFilter.PATIENT_TYPE_ID == o.TDL_PATIENT_TYPE_ID).ToList();
                        }

                        HisServiceReqFilterQuery listHisServiceReqFilter = new HisServiceReqFilterQuery();
                        listHisServiceReqFilter.TREATMENT_IDs = listIds;
                        listHisServiceReqFilter.HAS_EXECUTE = true;
                        listHisServiceReq = new HisServiceReqManager(param).Get(listHisServiceReqFilter) ?? new List<HIS_SERVICE_REQ>();
                        listHisServiceReq = listHisServiceReq.Where(o => listHisSereServBill.Exists(p => p.TDL_SERVICE_REQ_ID == o.ID)).ToList();

                        var lastServiceReq = listHisServiceReq.GroupBy(o => o.TREATMENT_ID).Select(p => p.OrderBy(q => q.INTRUCTION_TIME).LastOrDefault()).ToList();
                        if (castFilter.TREATMENT_TYPE_IDs != null)
                        {
                            listBillLocal = listBillLocal.Where(o => lastServiceReq.Exists(p => p.TREATMENT_ID == o.TREATMENT_ID && castFilter.TREATMENT_TYPE_IDs.Contains(p.TREATMENT_TYPE_ID ?? 0))).ToList();
                        }
                        HisTreatmentViewFilterQuery listHisTreatmentFilter = new HisTreatmentViewFilterQuery();
                        listHisTreatmentFilter.IDs = listIds;
                        listTreatment = new HisTreatmentManager(param).GetView(listHisTreatmentFilter) ?? new List<V_HIS_TREATMENT>();

                        if (castFilter.TDL_TREATMENT_TYPE_IDs != null)
                        {
                            listTreatment = listTreatment.Where(p => castFilter.TDL_TREATMENT_TYPE_IDs.Contains(p.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                            var trIds = listTreatment.Select(o => o.ID).ToList();
                            listBillLocal = listBillLocal.Where(o => trIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                        }

                        if (castFilter.INPUT_DATA_ID_IS_PAUSE == 1)
                        {
                            listTreatment = listTreatment.Where(p => p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                            var trIds = listTreatment.Select(o => o.ID).ToList();
                            listBillLocal = listBillLocal.Where(o => trIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                        }
                        else if (castFilter.INPUT_DATA_ID_IS_PAUSE == 2)
                        {
                            listTreatment = listTreatment.Where(p => p.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                            var trIds = listTreatment.Select(o => o.ID).ToList();
                            listBillLocal = listBillLocal.Where(o => trIds.Contains(o.TREATMENT_ID ?? 0)).ToList();
                        }

                        HisTransactionFilterQuery listDepositFilter = new HisTransactionFilterQuery();
                        listDepositFilter.TREATMENT_IDs = listIds;
                        listDepositFilter.IS_CANCEL = false;
                        listDepositFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU;
                        listDeposit = new HisTransactionManager(param).Get(listDepositFilter) ?? new List<HIS_TRANSACTION>();
                        HisTransactionFilterQuery listRepayFilter = new HisTransactionFilterQuery();
                        listRepayFilter.TREATMENT_IDs = listIds;
                        listRepayFilter.IS_CANCEL = false;
                        listRepayFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU;
                        listRepay = new HisTransactionManager(param).Get(listRepayFilter) ?? new List<HIS_TRANSACTION>();
                        listBillLocal = listBillLocal.Where(o => listHisSereServBill.Exists(p => p.BILL_ID == o.ID)).ToList();


                        foreach (var item in listBillLocal)
                        {
                            var sereServBillSub = listHisSereServBill.Where(o => o.BILL_ID == item.ID).ToList();
                            var serviceReqSub = listHisServiceReq.Where(o => sereServBillSub.Exists(p => p.TDL_SERVICE_REQ_ID == o.ID)).ToList();
                            var depositSub = listDeposit.Where(o => item.TREATMENT_ID == o.TREATMENT_ID).ToList();
                            var repaySub = listRepay.Where(o => item.TREATMENT_ID == o.TREATMENT_ID).ToList();
                            var serviceRetyCatSub = ListHisServiceRetyCat.Where(o => sereServBillSub.Exists(p => p.TDL_SERVICE_ID == o.SERVICE_ID)).ToList();
                            V_HIS_TREATMENT treatment = listTreatment.FirstOrDefault(o => o.ID == item.TREATMENT_ID) ?? new V_HIS_TREATMENT();
                            var billBalance = ListBillBalance.FirstOrDefault(o => o.ID == item.ID);
                            long? departmentId = serviceReqSub.OrderBy(o => o.INTRUCTION_TIME).Select(p => p.REQUEST_DEPARTMENT_ID).LastOrDefault();
                            if (castFilter.REQUEST_DEPARTMENT_ID != null)
                            {
                                if (departmentId != castFilter.REQUEST_DEPARTMENT_ID)
                                    continue;
                            }
                            long? roomId = serviceReqSub.OrderBy(o => o.INTRUCTION_TIME).Select(p => p.REQUEST_ROOM_ID).LastOrDefault();
                            if (castFilter.REQUEST_ROOM_ID != null)
                            {
                                if (roomId != castFilter.REQUEST_ROOM_ID)
                                    continue;
                            }
                            listRdo.Add(new Mrs00581RDO(item, serviceReqSub, sereServBillSub, treatment, ref dicService, serviceRetyCatSub, depositSub, repaySub, departmentId ?? 0, roomId ?? 0, billBalance, dicSv, castFilter));
                        }
                    }
                }

                foreach (var item in dicService.Values.ToList())
                {
                    var service = Services.FirstOrDefault(o => o.SERVICE_CODE == item.SERVICE_CODE);
                    if (service != null)
                    {
                        var serviceParent = Services.FirstOrDefault(o => o.ID == service.PARENT_ID);
                        if (serviceParent != null)
                        {
                            item.PARENT_SERVICE_CODE = serviceParent.SERVICE_CODE;
                            item.PARENT_SERVICE_NAME = serviceParent.SERVICE_NAME;
                        }
                    }
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
                //var listRdoCancle = listRdo.Where(x => x.IS_CANCEL == 1).ToList();
                List<Mrs00581RDO> listRdoCancel = new List<Mrs00581RDO>();
                listRdoCancel = listRdo.Where(x => x.IS_CANCEL == 2).ToList();
                if (IsNotNullOrEmpty(listRdoCancel))
                {
                    foreach (var item in listRdoCancel)
                    {
                        item.NOTE = "Hóa đơn hủy";
                        item.AN_TOTAL_PRICE = item.AN_TOTAL_PRICE * (-1);
                        item.BED_TOTAL_PRICE = item.BED_TOTAL_PRICE * (-1);
                        item.BLOOD_TOTAL_PRICE = item.BLOOD_TOTAL_PRICE * (-1);
                        item.CHI_PHI = item.CHI_PHI * (-1);
                        item.ENDO_TOTAL_PRICE = item.ENDO_TOTAL_PRICE * (-1);
                        item.EXAM_TOTAL_PRICE = item.EXAM_TOTAL_PRICE * (-1);
                        item.FUEX_TOTAL_PRICE = item.FUEX_TOTAL_PRICE * (-1);
                        item.GPBL_TOTAL_PRICE = item.GPBL_TOTAL_PRICE * (-1);
                        item.KHAC_TOTAL_PRICE = item.KHAC_TOTAL_PRICE * (-1);
                        item.MATERIAL_TOTAL_PRICE = item.MATERIAL_TOTAL_PRICE * (-1);
                        item.MEDICINE_TOTAL_PRICE = item.MEDICINE_TOTAL_PRICE * (-1);
                        item.PHCN_TOTAL_PRICE = item.PHCN_TOTAL_PRICE * (-1);
                        item.PT_TOTAL_PRICE = item.PT_TOTAL_PRICE * (-1);
                        item.SUIM_TOTAL_PRICE = item.SUIM_TOTAL_PRICE * (-1);
                        item.TEIN_TOTAL_PRICE = item.TEIN_TOTAL_PRICE * (-1);
                        item.TOTAL_PRICE = item.TOTAL_PRICE * (-1);
                        item.TOTAL_TUTRA = item.TOTAL_TUTRA * (-1);
                        item.TOTAL_PRICE_20 = item.TOTAL_PRICE_20 * (-1);
                        item.TOTAL_PRICE_30 = item.TOTAL_PRICE_30 * (-1);
                        item.TOTAL_PRICE_5 = item.TOTAL_PRICE_5 * (-1);
                        item.TOTAL_PRICE_BHYT = item.TOTAL_PRICE_BHYT * (-1);
                        item.TOTAL_PRICE_FEE = item.TOTAL_PRICE_FEE * (-1);
                        item.TOTAL_PRICE_OTHER = item.TOTAL_PRICE_OTHER * (-1);
                        item.TT_TOTAL_PRICE = item.TT_TOTAL_PRICE * (-1);
                        item.DEPOSIT_AMOUNT = item.DEPOSIT_AMOUNT * (-1);
                        item.REPAY_AMOUNT = item.REPAY_AMOUNT * (-1);
                        item.ACCOUNT_BOOK_CODE = item.ACCOUNT_BOOK_CODE;
                        item.ACCOUNT_BOOK_ID = item.ACCOUNT_BOOK_ID;
                        item.ACCOUNT_BOOK_NAME = item.ACCOUNT_BOOK_NAME;
                        item.CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                        item.CASHIER_ROOM_CODE = item.CASHIER_ROOM_CODE;
                        item.CASHIER_ROOM_NAME = item.CASHIER_ROOM_NAME;
                        item.CASHIER_USERNAME = item.CASHIER_USERNAME;
                        item.DEPARTMENT_CODE = item.DEPARTMENT_CODE;
                        item.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                        item.DIC_GROUP = item.DIC_GROUP;
                        item.DIEN = item.DIEN;
                        item.DIIM_TOTAL_PRICE = item.DIIM_TOTAL_PRICE * (-1);
                        item.DOB = item.DOB;
                        item.EINVOICE_NUM_ORDER = item.EINVOICE_NUM_ORDER;
                        item.EINVOICE_TIME = item.EINVOICE_TIME ?? 99990101000000;
                        item.EXEMPTION = item.EXEMPTION * (-1);
                        item.EXEMPTION_REASON = item.EXEMPTION_REASON;
                        item.GENDER_NAME = item.GENDER_NAME;
                        item.HEIN_CARD_NUMBER = item.HEIN_CARD_NUMBER;
                        item.IN_TIME = item.IN_TIME;
                        item.INVOICE_CODE = item.INVOICE_CODE;
                        item.INVOICE_SYS = item.INVOICE_SYS;
                        item.IS_CANCEL = item.IS_CANCEL;
                        item.KC_AMOUNT = item.KC_AMOUNT * (-1);
                        item.HIEN_DU = item.HIEN_DU * (-1);
                        item.NUM_ORDER = item.NUM_ORDER;
                        item.OUT_TIME = item.OUT_TIME;
                        item.PATIENT_CODE = item.PATIENT_CODE;
                        item.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                        item.PAY_FORM_CODE = item.PAY_FORM_CODE;
                        item.PAY_FORM_NAME = item.PAY_FORM_NAME;
                        item.ROOM_CODE = item.ROOM_CODE;
                        item.ROOM_NAME = item.ROOM_NAME;
                        item.SERVICE_TYPE_NAME = item.SERVICE_TYPE_NAME;
                        item.SYMBOL_CODE = item.SYMBOL_CODE;
                        item.TDL_BILL_FUND_AMOUNT = item.TDL_BILL_FUND_AMOUNT * (-1);
                        item.TDL_PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                        item.TEMPLATE_CODE = item.TEMPLATE_CODE;
                        item.TRANSACTION_CODE = item.TRANSACTION_CODE;
                        item.TRANSACTION_DATE = item.TRANSACTION_DATE;
                        item.TRANSACTION_MONTH = item.TRANSACTION_MONTH;
                        item.TRANSACTION_TIME = item.TRANSACTION_TIME;
                        item.TRANSACTION_TIME_STR = item.TRANSACTION_TIME_STR;
                        item.TREATMENT_CODE = item.TREATMENT_CODE;
                        item.TREATMENT_DAY_COUNT = item.TREATMENT_DAY_COUNT;
                        item.TREATMENT_ID = item.TREATMENT_ID;
                        item.VIR_PATIENT_NAME = item.VIR_PATIENT_NAME;
                        item.END_DEPARTMENT_CODE = item.END_DEPARTMENT_CODE;
                        item.END_DEPARTMENT_NAME = item.END_DEPARTMENT_NAME;
                        //listRdo.Add(item);
                    }
                }
                if (castFilter.IS_END_DEPARTMENT == true)
                {
                    foreach (var item in listRdo)
                    {
                        item.DEPARTMENT_CODE = item.END_DEPARTMENT_CODE;
                        item.DEPARTMENT_NAME = item.END_DEPARTMENT_NAME;
                    }
                }
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                //listRdo = listRdo.Where(x => x.EINVOICE_NUM_ORDER != null).ToList();
                dicSingleTag.Add("CASHIER_LOGINNAME", castFilter.LOGINNAME + castFilter.CASHIER_LOGINNAME);
                dicSingleTag.Add("CASHIER_USERNAME", cashierUsername);
                objectTag.AddObjectData(store, "Bhyt", listRdo.Where(o => o.TOTAL_PRICE_BHYT > 0).OrderBy(p => p.TRANSACTION_TIME).ToList());
                objectTag.AddObjectData(store, "Vp", listRdo.Where(o => o.TOTAL_PRICE_FEE > 0).OrderBy(p => p.TRANSACTION_TIME).ToList());
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.TRANSACTION_TIME).ToList());
                objectTag.AddObjectData(store, "Report1", listRdo.OrderBy(p => p.TRANSACTION_CODE).ToList());
                objectTag.AddObjectData(store, "Report2", listRdo.OrderBy(p => p.EINVOICE_NUM_ORDER == null).ThenBy(p => p.EINVOICE_NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "SortByNumOrder", listRdo.OrderBy(p => p.ACCOUNT_BOOK_ID).ThenBy(q => q.NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "Services", dicService.Values.OrderBy(p => p.SERVICE_TYPE_NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "ServiceTypes", dicService.Values.GroupBy(g => g.SERVICE_TYPE_CODE).Select(o => o.First()).OrderBy(p => p.SERVICE_TYPE_NUM_ORDER).ToList());
                objectTag.AddRelationship(store, "ServiceTypes", "Services", "SERVICE_TYPE_CODE", "SERVICE_TYPE_CODE");

                objectTag.AddObjectData(store, "ServiceParents", dicService.Values.GroupBy(g => g.PARENT_SERVICE_CODE).Select(o => o.First()).OrderBy(p => p.SERVICE_TYPE_NUM_ORDER).ToList());
                objectTag.AddRelationship(store, "ServiceParents", "Services", "PARENT_SERVICE_CODE", "PARENT_SERVICE_CODE");
                dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);

                HisBranchFilterQuery branchFilter = new HisBranchFilterQuery();
                branchFilter.ID = castFilter.BRANCH_ID;
                var listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchFilter).ToList();
                dicSingleTag.Add("BRANCH_NAME", listBranchs.First().BRANCH_NAME);
                objectTag.AddObjectData(store, "Exemption", listRdo.Where(o => o.EXEMPTION > 0).OrderBy(p => p.TRANSACTION_TIME).ToList());

                objectTag.AddObjectData(store, "TransactionDate", listRdo.OrderBy(p => p.TRANSACTION_TIME).GroupBy(g => g.TRANSACTION_DATE).Select(p => p.First()).ToList());
                objectTag.AddRelationship(store, "TransactionDate", "Report", "TRANSACTION_DATE", "TRANSACTION_DATE");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
