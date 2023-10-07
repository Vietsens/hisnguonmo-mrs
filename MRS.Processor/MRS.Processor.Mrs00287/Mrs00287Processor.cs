using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisBillFund;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
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
using MRS.SDO;
using Inventec.Common.Adapter;
using MRS.MANAGER.Manager;
using Newtonsoft.Json;
using MRS.Processor.Mrs00296;
using Inventec.Common.Logging;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00287
{
    public class Mrs00287Processor : AbstractProcessor
    {
        Mrs00287Filter castFilter = null;

        List<Mrs00287RDO> listRdo = new List<Mrs00287RDO>();
        List<Mrs00287RDO> listDetail = new List<Mrs00287RDO>();
        Dictionary<string, Mrs00287RDO> dicRdo = new Dictionary<string, Mrs00287RDO>();
        Dictionary<string, Mrs00287RDO> dicPayForm = new Dictionary<string, Mrs00287RDO>();
        Dictionary<string, Mrs00287RDO> dicPayUser = new Dictionary<string, Mrs00287RDO>();
        Dictionary<string, Mrs00287RDO> dicPayUserBank = new Dictionary<string, Mrs00287RDO>();
        List<V_HIS_TRANSACTION> listTransaction = null;
        List<V_HIS_TRANSACTION> listBill = null;
        List<V_HIS_TRANSACTION> listDeposit = null;
        List<V_HIS_TRANSACTION> listRepay = null;
        List<V_HIS_CASHIER_ROOM> listCashierRoom = null;

        List<HIS_SERE_SERV_BILL> listSSB = new List<HIS_SERE_SERV_BILL>();
        List<HIS_AREA> listArea = new List<HIS_AREA>();

        List<Mrs00296RDO> listSereServRdo = new List<Mrs00296RDO>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        Dictionary<string, Mrs00296RDO> dicTransactionArea = new Dictionary<string, Mrs00296RDO>();

        Dictionary<long, long> dicTreatId = new Dictionary<long, long>();
        Dictionary<long, List<V_HIS_BILL_FUND>> dicBillFund = new Dictionary<long, List<V_HIS_BILL_FUND>>();

        List<CARD> listCard = new List<CARD>();

        short IS_TRUE = 1;
        List<long> Holidays = new List<long>();
        List<long> listTranId = new List<long>();

        public Mrs00287Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00287Filter);
        }

        protected override bool GetData()
        {
            var valid = true;
            try
            {
                this.castFilter = (Mrs00287Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_BILL, V_HIS_DEPOSIT, V_HIS_REPAY, Mrs00287: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();
                //Get danh sách phòng
                GetRoom();

                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.CREATE_TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.CREATE_TIME_TO;
                //transactionFilter.IS_CANCEL = false;
                //transactionFilter.HAS_SALL_TYPE = false;
                listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(transactionFilter);
                if (castFilter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.EXACT_CASHIER_ROOM_IDs.Contains(o.CASHIER_ROOM_ID)).ToList();
                }
                listTransaction = listTransaction.Where(o => string.IsNullOrWhiteSpace(castFilter.CASHIER_LOGINNAME) || o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();

                // loc theo trang thai giao dich
                if (castFilter.INPUT_DATA_ID_STTRAN_TYPE == 1)
                {
                    listTransaction = listTransaction.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE).ToList();
                }
                if (castFilter.INPUT_DATA_ID_STTRAN_TYPE == 2)
                {
                    listTransaction = listTransaction.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }

               
                //them giao dich thanh toan huy
                if (castFilter.IS_ADD_BILL_CANCEL == true)
                {
                    HisTransactionViewFilterQuery billCancelFilter = new HisTransactionViewFilterQuery();
                    billCancelFilter.CANCEL_TIME_FROM = castFilter.CREATE_TIME_FROM;
                    billCancelFilter.CANCEL_TIME_TO = castFilter.CREATE_TIME_TO;
                    billCancelFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                    billCancelFilter.IS_CANCEL = true;
                    //transactionFilter.HAS_SALL_TYPE = false;
                    //transactionFilter.CASHIER_ROOM_IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                    var listBillCancel = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(billCancelFilter);
                    listBillCancel = listBillCancel.Where(o => o.TREATMENT_ID.HasValue).ToList();
                    if (listBillCancel != null)
                    {
                        //listBillCancel = listBillCancel.Where(o => IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM == castFilter.PAY_FORM_ID || castFilter.PAY_FORM_ID == null).ToList();
                        //if (castFilter.PAY_FORM_IDs != null)
                        //{
                        //    listBillCancel = listBillCancel.Where(o => castFilter.PAY_FORM_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)).ToList();
                        //}
                        if (castFilter.EXACT_CASHIER_ROOM_IDs != null)
                        {
                            listBillCancel = listBillCancel.Where(o => castFilter.EXACT_CASHIER_ROOM_IDs.Contains(o.CANCEL_CASHIER_ROOM_ID ?? 0)).ToList();
                        }
                        if (castFilter.CASHIER_LOGINNAME != null && castFilter.CASHIER_LOGINNAME.Length > 0)
                        {
                            listBillCancel = listBillCancel.Where(o => castFilter.CASHIER_LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                        }
                        foreach (var item in listBillCancel)
                        {
                            
                            if (item.PAY_FORM_ID == 8)
                            {
                                item.PAY_FORM_ID = 1;
                                var PAY_FORM = HisPayFormCFG.ListPayForm.FirstOrDefault(O => O.ID == 1);
                                if (PAY_FORM != null) 
                                {
                                    item.PAY_FORM_CODE = PAY_FORM.PAY_FORM_CODE;
                                    item.PAY_FORM_NAME = PAY_FORM.PAY_FORM_NAME;
                                
                                }
                            }
                            item.CASHIER_LOGINNAME = item.CANCEL_LOGINNAME;
                            item.CASHIER_USERNAME = item.CANCEL_USERNAME;
                            item.CASHIER_ROOM_ID = item.CANCEL_CASHIER_ROOM_ID ?? 0;
                            item.IS_CANCEL = 2;
                        }
                        listTransaction.AddRange(listBillCancel);
                        //listTransaction = listTransaction.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    }
                }
               

                //them giao dich tam ung huy
                if (castFilter.IS_ADD_DEPO_CANCEL == true)
                {
                    HisTransactionViewFilterQuery billCancelFilter = new HisTransactionViewFilterQuery();
                    billCancelFilter.CANCEL_TIME_FROM = castFilter.CREATE_TIME_FROM;
                    billCancelFilter.CANCEL_TIME_TO = castFilter.CREATE_TIME_TO;
                    billCancelFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                    billCancelFilter.IS_CANCEL = true;
                    //transactionFilter.HAS_SALL_TYPE = false;
                    //transactionFilter.CASHIER_ROOM_IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                    var listDepositCancel = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(billCancelFilter);
                    listDepositCancel = listDepositCancel.Where(o => o.TREATMENT_ID.HasValue).ToList();
                    if (listDepositCancel != null)
                    {
                        //listDepositCancel = listDepositCancel.Where(o => IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM == castFilter.PAY_FORM_ID || castFilter.PAY_FORM_ID == null).ToList();
                        //if (castFilter.PAY_FORM_IDs != null)
                        //{
                        //    listDepositCancel = listDepositCancel.Where(o => castFilter.PAY_FORM_IDs.Contains(IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM)).ToList();
                        //}

                        if (castFilter.EXACT_CASHIER_ROOM_IDs != null)
                        {
                            listDepositCancel = listDepositCancel.Where(o => castFilter.EXACT_CASHIER_ROOM_IDs.Contains(o.CANCEL_CASHIER_ROOM_ID ?? 0)).ToList();
                        }
                        if (castFilter.CASHIER_LOGINNAME != null && castFilter.CASHIER_LOGINNAME.Length>0)
                        {
                            listDepositCancel = listDepositCancel.Where(o => castFilter.CASHIER_LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                        }
                        foreach (var item in listDepositCancel)
                        {
                            if (item.PAY_FORM_ID == 8)
                            {
                                item.PAY_FORM_ID = 1;
                                var PAY_FORM = HisPayFormCFG.ListPayForm.FirstOrDefault(O => O.ID == 1);
                                if (PAY_FORM != null)
                                {
                                    item.PAY_FORM_CODE = PAY_FORM.PAY_FORM_CODE;
                                    item.PAY_FORM_NAME = PAY_FORM.PAY_FORM_NAME;

                                }
                            }
                            item.CASHIER_LOGINNAME = item.CANCEL_LOGINNAME;
                            item.CASHIER_USERNAME = item.CANCEL_USERNAME;
                            item.CASHIER_ROOM_ID = item.CANCEL_CASHIER_ROOM_ID ?? 0;
                            item.IS_CANCEL = 2;
                        }
                        listTransaction.AddRange(listDepositCancel);
                        //listTransaction = listTransaction.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    }
                }
                listTransaction = listTransaction.Where(o => o.TREATMENT_ID.HasValue).ToList();


                listTransaction = listTransaction.Where(o => o.PAY_FORM_ID == castFilter.PAY_FORM_ID || castFilter.PAY_FORM_ID == null).ToList();
                if (castFilter.PAY_FORM_IDs != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.PAY_FORM_IDs.Contains(o.PAY_FORM_ID)).ToList();
                }
               
                GetHolidays();
                if (castFilter.IS_HOLIDAYS == true && castFilter.IS_NOT_HOLIDAYS != true)
                {
                    listTransaction = listTransaction.Where(o => Holidays.Contains(o.TRANSACTION_DATE)).ToList();
                }
                if (castFilter.IS_NOT_HOLIDAYS == true && castFilter.IS_HOLIDAYS != true)
                {
                    listTransaction = listTransaction.Where(o => !Holidays.Contains(o.TRANSACTION_DATE)).ToList();
                }
                listBill = listTransaction.Where(o => o.TRANSACTION_TYPE_ID ==
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();

                if (castFilter.ADD__SALE_EXP != true)
                {
                    listBill = listBill.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP).ToList();
                }
                listDeposit = listTransaction.Where(o => o.TRANSACTION_TYPE_ID ==
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                listRepay = listTransaction.Where(o => o.TRANSACTION_TYPE_ID ==
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                //get phòng thu ngân
                GetCashierRooom();

                //lọc theo diện điều trị
                GetTreatmentFilter();

                GetSereServDetail(castFilter);

                //get danh sách thẻ
                GetCard(castFilter);

                //lọc theo danh sach thẻ
                FilterByCard(castFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00287");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void GetRoom()
        {
            listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
        }

        private void GetCashierRooom()
        {
            HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
            cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID;
            cashierRoomFilter.IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
            listCashierRoom = new MOS.MANAGER.HisCashierRoom.HisCashierRoomManager().GetView(cashierRoomFilter);
        }

        private void FilterByCard(Mrs00287Filter filter)
        {
            if (filter.INPUT_DATA_ID_BC_TYPE > 1)//trường hợp có chọn loại ngân hàng
            {
                List<string> listCardCode = listCard != null ? listCard.Select(o => o.BANK_CARD_CODE).ToList() : new List<string>();
                listTransaction = listTransaction.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listBill = listBill.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listDeposit = listDeposit.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listRepay = listRepay.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                var transactionIds = listTransaction.Select(o => o.ID).ToList();
                listSereServRdo = listSereServRdo.Where(o => transactionIds.Contains(o.TRANSACTION_ID ?? 0)).ToList();
                //dicTransactionArea = dicTransactionArea.Where(o => transactionIds.Contains(o.Value.TRANSACTION_ID ?? 0)).ToDictionary<string, Mrs00296RDO>();
            }
        }


        private void GetCard(Mrs00287Filter filter)
        {
            try
            {
                string query = "-- danh sach the\n";
                query += "select \n";
                query += "cc.bank_card_code,\n";
                query += "1\n";
                query += "from his_rs.his_card cc\n";
                query += "where 1=1\n";
                if (filter.INPUT_DATA_ID_BC_TYPE == 2)
                {
                    query += " AND cc.bank_card_code like '970422%'\n";
                }
                if (filter.INPUT_DATA_ID_BC_TYPE == 3)
                {
                    query += " AND cc.bank_card_code like '970412%'\n";
                }
                //query += "and cc.bank_code is not null \n";
                query += "and cc.bank_card_code is not null \n";


                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                listCard = new MOS.DAO.Sql.MyAppContext().GetSql<CARD>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
        private void GetTreatmentFilter()
        {
            try
            {
                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    var treatmentIds = listTransaction.Select(o => o.TREATMENT_ID ?? 0).Distinct().ToList();
                    List<long> treatmentIdFilter = new List<long>();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        treatmentFilter.IDs = limit;
                        var treatment = new HisTreatmentManager().Get(treatmentFilter);
                        if (treatment != null && treatment.Count > 0)
                        {
                            treatment = treatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();

                            treatmentIdFilter.AddRange(treatment.Select(o => o.ID).ToList());
                        }
                    }
                    listTransaction = listTransaction.Where(o => treatmentIdFilter.Contains(o.TREATMENT_ID ?? 0)).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetHolidays()
        {
            try
            {
                List<string> strHolidays = new List<string>() { "1_1", "30_4", "1_5", "2_9" };
                List<string> strLunarHolidays = new List<string>() { "10_3", "30_12", "29_12", "1_1", "2_1", "3_1" };
                List<DateTime> dateHolidays = new List<DateTime>();
                int year = DateTime.Today.Year;
                DateTime startDate = new DateTime(year - 1, 1, 1);
                DateTime endDate = new DateTime(year + 1, 1, 1);
                for (DateTime i = startDate; i < endDate; i = i.AddDays(1))
                {
                    if (i.DayOfWeek == DayOfWeek.Sunday || i.DayOfWeek == DayOfWeek.Saturday)
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                    //var vcal = new convertSolar2Lunar();
                    //int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    //var tempDay = arr[0] + "_" + arr[1];
                    if (strHolidays.Contains(string.Format("{0}_{1}", i.Day, i.Month)))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }

                    var vcal = new convertSolar2Lunar();
                    int[] arr = vcal.convertSolar2Lunars(i.Day, i.Month, i.Year, 7);
                    var tempDay = arr[0] + "_" + arr[1];
                    if (strLunarHolidays.Contains(tempDay))
                    {
                        DateTime j = i;
                        while (dateHolidays.Contains(j))
                        {
                            j = j.AddDays(1);
                        }
                        dateHolidays.Add(j);
                        Holidays.Add(Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(j) ?? 0);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }



        private void GetSereServDetail(Mrs00287Filter castFilter)
        {
            try
            {
                if (castFilter.IS_ADD_INFO_AREA == true)
                {
                    listSereServRdo = new ManagerSql().GetSereServDO(castFilter, listRoom);
                    if (castFilter.IS_ADD_BILL_CANCEL == true)
                    {
                        var listRdoBilCancel = new ManagerSql().GetBillCancel(castFilter, listRoom);
                        if (listRdoBilCancel != null)
                        {
                            listSereServRdo.AddRange(listRdoBilCancel);
                        }
                    }
                    if (castFilter.IS_ADD_DEPO_CANCEL == true)
                    {
                        var listRdoDepCancel = new ManagerSql().GetDepositCancel(castFilter, listRoom);
                        if (listRdoDepCancel != null)
                        {
                            listSereServRdo.AddRange(listRdoDepCancel);
                        }
                    }

                    listArea = new ManagerSql().GetArea();
                    AddInfor(listSereServRdo);
                    FilterTypePrice(listSereServRdo);

                    if (castFilter.AREA_IDs != null)
                    {
                        listSereServRdo = listSereServRdo.Where(o => castFilter.AREA_IDs.Contains(o.AREA_ID ?? 0)).ToList();
                    }
                    if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                    {
                        listSereServRdo = listSereServRdo.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                    }
                    listSereServRdo = listSereServRdo.GroupBy(g => new { g.SERE_SERV_ID, g.TRANSACTION_ID }).Select(p => p.First()).ToList();
                    dicTransactionArea = listSereServRdo.GroupBy(g => string.Format("{0}_{1}", g.TRANSACTION_ID, g.AREA_ID)).ToDictionary(p => p.Key, q => new Mrs00296RDO() { TRANSACTION_ID = q.First().TRANSACTION_ID, TREATMENT_ID = q.First().TREATMENT_ID, AREA_ID = q.First().AREA_ID, TOTAL_DEPOSIT_BILL_PRICE = q.Sum(s => s.TOTAL_DEPOSIT_BILL_PRICE), TOTAL_REPAY_PRICE = q.Sum(s => s.TOTAL_REPAY_PRICE), TOTAL_PATIENT_BHYT_PRICE = q.Sum(s => s.TOTAL_PATIENT_BHYT_PRICE), EXAM_PRICE = q.Where(r => r.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => s.TOTAL_DEPOSIT_BILL_PRICE) });
                }
            }
            catch (Exception ex)
            {

                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddInfor(List<Mrs00296RDO> listSereServRdo)
        {
            foreach (var item in listSereServRdo)
            {
                //sua lai phong chi dinh de tinh doanh thu

                var exRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM();
                var requestRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM();
                if (item.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                {
                    item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;

                    item.AREA_ID = exRoom.AREA_ID ?? 0;

                    item.TDL_REQUEST_DEPARTMENT_ID = exRoom.DEPARTMENT_ID;
                }
                //else if (!this.departmentKKB.Contains(requestRoom.DEPARTMENT_ID) && !this.departmentKLS.Contains(requestRoom.DEPARTMENT_ID))
                //{
                //    item.TDL_REQUEST_ROOM_ID = item.TDL_FIRST_EXAM_ROOM_ID ?? 0;
                //    var examRoom = listRoom.FirstOrDefault(o => o.ID == item.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM();
                //    item.TDL_REQUEST_DEPARTMENT_ID = examRoom.DEPARTMENT_ID;
                //    item.TDL_REQUEST_ROOM_CODE = examRoom.ROOM_CODE;
                //    item.TDL_REQUEST_ROOM_NAME = examRoom.ROOM_NAME;
                //    item.TDL_REQUEST_DEPARTMENT_CODE = examRoom.DEPARTMENT_CODE;
                //    item.TDL_REQUEST_DEPARTMENT_NAME = examRoom.DEPARTMENT_NAME;
                //    item.AREA_ID = examRoom.AREA_ID ?? 0;
                //}
                else
                {
                    //item.TDL_REQUEST_ROOM_ID = requestRoom.ID;
                    //item.TDL_REQUEST_DEPARTMENT_ID = requestRoom.DEPARTMENT_ID;
                    //item.TDL_REQUEST_ROOM_CODE = requestRoom.ROOM_CODE;
                    //item.TDL_REQUEST_ROOM_NAME = requestRoom.ROOM_NAME;
                    //item.TDL_REQUEST_DEPARTMENT_CODE = requestRoom.DEPARTMENT_CODE;
                    //item.TDL_REQUEST_DEPARTMENT_NAME = requestRoom.DEPARTMENT_NAME;
                    item.AREA_ID = requestRoom.AREA_ID ?? 0;
                    item.TDL_REQUEST_DEPARTMENT_ID = requestRoom.DEPARTMENT_ID;

                }
            }
        }


        private void FilterTypePrice(List<Mrs00296RDO> listSereServRdo)
        {
            foreach (var item in listSereServRdo)
            {
                if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && item.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    item.TOTAL_PATIENT_BHYT_PRICE = item.TOTAL_DEPOSIT_BILL_PRICE - item.CHENHLECH;
                }
                else
                {
                    item.TOTAL_PATIENT_BHYT_PRICE = 0;
                }
                if (castFilter.INPUT_DATA_ID_PRICE_TYPEs != null && !castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)1))//tự trả và viện phí
                {
                    if (item.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && item.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.TOTAL_REPAY_PRICE = 0;
                        item.TOTAL_PATIENT_BHYT_PRICE = 0;
                        item.TOTAL_DEPOSIT_BILL_PRICE = item.CHENHLECH;

                    }

                }

                if (castFilter.INPUT_DATA_ID_PRICE_TYPEs != null && !castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)2))//bảo hiểm
                {
                    item.TOTAL_DEPOSIT_BILL_PRICE -= item.CHENHLECH;
                    if (item.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT || item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.TOTAL_DEPOSIT_BILL_PRICE = 0;
                        item.TOTAL_REPAY_PRICE = 0;

                    }


                }

            }
        }

        protected override bool ProcessData()
        {
            bool valid = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                List<long> listTreatmentId = new List<long>();
                List<V_HIS_BILL_FUND> listBillFund = null;
                if (IsNotNullOrEmpty(listBill))
                {
                    listTreatmentId.AddRange(listBill.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList());
                    listBillFund = GetBillFundByListBill(ref paramGet);
                }
                if (IsNotNullOrEmpty(listDeposit))
                {
                    listTreatmentId.AddRange(listDeposit.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList());
                }
                if (IsNotNullOrEmpty(listRepay))
                {
                    listTreatmentId.AddRange(listRepay.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList());
                }

                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

                LogSystem.Info("1=========");
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    listTreatmentId = listTreatmentId.Distinct().ToList();
                    var listPatyAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatmentId);

                    if (IsNotNullOrEmpty(listPatyAlter))
                    {
                        listPatyAlter = listPatyAlter.OrderBy(o => o.LOG_TIME).ToList();
                        var Groups = listPatyAlter.GroupBy(o => o.TREATMENT_ID).ToList();
                        foreach (var group in Groups)
                        {
                            var listSub = group.ToList<V_HIS_PATIENT_TYPE_ALTER>();
                            dicCurrentPatyAlter[listSub.First().TREATMENT_ID] = listSub.Last();
                            foreach (var item in listSub)
                            {
                                if (item.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU && item.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY)
                                    continue;
                                dicPatientTypeAlter[item.TREATMENT_ID] = item;
                                break;
                            }
                        }
                    }
                    LogSystem.Info("dicCurrentPatyAlter:" + dicCurrentPatyAlter.Count);

                    ProcessListTransaction(dicCurrentPatyAlter, dicPatientTypeAlter, listBillFund);
                    ProcessDetail(listSereServRdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                valid = false;
            }
            return valid;
        }

        private void ProcessListTransaction(Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<V_HIS_BILL_FUND> listBillFund)
        {
            try
            {
                Dictionary<long, V_HIS_CASHIER_ROOM> dicCashierRoom = new Dictionary<long, V_HIS_CASHIER_ROOM>();
                if (!IsNotNullOrEmpty(listCashierRoom))
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc danh sach phong thu ngan: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listCashierRoom), listCashierRoom));
                    return;
                }

                LogSystem.Info("3=========");

                foreach (var item in listCashierRoom)
                {
                    dicCashierRoom[item.ID] = item;
                }

                LogSystem.Info("4=========");
                if (IsNotNullOrEmpty(listBillFund))
                {
                    foreach (var item in listBillFund)
                    {
                        if (!dicBillFund.ContainsKey(item.BILL_ID))
                            dicBillFund[item.BILL_ID] = new List<V_HIS_BILL_FUND>();
                        dicBillFund[item.BILL_ID].Add(item);
                    }
                }

                LogSystem.Info("5=========");
                if (IsNotNullOrEmpty(listBill))
                {
                    foreach (var item in listBill)
                    {
                        if (castFilter.IS_ADD_BILL_CANCEL != true)
                        {
                            if (item.IS_CANCEL == IS_TRUE)
                                continue;
                        }

                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua bill: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (castFilter.PATIENT_TYPE_IDs != null && !castFilter.PATIENT_TYPE_IDs.Contains(dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID))
                        {
                            continue;
                        }
                        //nếu thu trước khi vào nhập viện
                        if ((!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0)) || dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME > item.TRANSACTION_TIME)//
                        {
                            listTranId.Add(item.ID);
                            if (castFilter.IS_ADD_INFO_AREA == true)
                            {
                                List<string> keySub = dicTransactionArea.Keys.Where(o => o.StartsWith(string.Format("{0}_", item.ID))).ToList();
                                foreach (var key in keySub)
                                {
                                    var ssb = dicTransactionArea[key];
                                    //group theo thu ngân và khu vực
                                    string KeyGroup = "{0}_{1}";//cashier_loginname và area_id
                                    AddToDic(ref dicRdo, KeyGroup, item, ssb);

                                    //group theo thu ngan va hinh thuc thanh toan
                                    KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                                    AddToDic(ref dicPayUser, KeyGroup, item, ssb);

                                    //group theo hinh thuc thanh toan
                                    KeyGroup = "{2}";//pay_form_id
                                    AddToDic(ref dicPayForm, KeyGroup, item, ssb);

                                    //group theo thu ngân, hình thức thanh toán và loại thẻ
                                    KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                                    AddToDic(ref dicPayUserBank, KeyGroup, item, ssb);
                                }

                            }
                            else
                            {
                                //group theo thu ngân 
                                string KeyGroup = "{0}";//cashier_loginname
                                AddToDic(ref dicRdo, KeyGroup, item, null);

                                //group theo thu ngan va hinh thuc thanh toan
                                KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                                AddToDic(ref dicPayUser, KeyGroup, item, null);

                                //group theo hinh thuc thanh toan
                                KeyGroup = "{2}";//pay_form_id
                                AddToDic(ref dicPayForm, KeyGroup, item, null);

                                //group theo thu ngân, hình thức thanh toán và loại thẻ
                                KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                                AddToDic(ref dicPayUserBank, KeyGroup, item, null);
                            }
                        }
                    }
                }

                LogSystem.Info("6=========");

                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var item in listDeposit)
                    {
                        if (item.IS_CANCEL == IS_TRUE && castFilter.IS_ADD_DEPO_CANCEL != true)
                            continue;

                        if (!(item.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && item.TDL_SERE_SERV_DEPOSIT_COUNT.Value > 0))
                            continue;

                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua Deposit: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (castFilter.PATIENT_TYPE_IDs != null && !castFilter.PATIENT_TYPE_IDs.Contains(dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID))
                        {
                            continue;
                        }

                        listTranId.Add(item.ID);

                        if (castFilter.IS_ADD_INFO_AREA == true)
                        {
                            List<string> keySub = dicTransactionArea.Keys.Where(o => o.StartsWith(string.Format("{0}_", item.ID))).ToList();
                            foreach (var key in keySub)
                            {
                                var ssd = dicTransactionArea[key];
                                //group theo thu ngân và khu vực
                                string KeyGroup = "{0}_{1}";//cashier_loginname và area_id
                                AddToDic(ref dicRdo, KeyGroup, item, ssd);

                                //group theo thu ngan va hinh thuc thanh toan
                                KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                                AddToDic(ref dicPayUser, KeyGroup, item, ssd);

                                //group theo hinh thuc thanh toan
                                KeyGroup = "{2}";//pay_form_id
                                AddToDic(ref dicPayForm, KeyGroup, item, ssd);

                                //group theo thu ngân, hình thức thanh toán và loại thẻ
                                KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                                AddToDic(ref dicPayUserBank, KeyGroup, item, ssd);

                            }
                        }
                        else
                        {
                            //group theo thu ngân 
                            string KeyGroup = "{0}";//cashier_loginname
                            AddToDic(ref dicRdo, KeyGroup, item, null);

                            //group theo thu ngan va hinh thuc thanh toan
                            KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                            AddToDic(ref dicPayUser, KeyGroup, item, null);

                            //group theo hinh thuc thanh toan
                            KeyGroup = "{2}";//pay_form_id
                            AddToDic(ref dicPayForm, KeyGroup, item, null);

                            //group theo thu ngân, hình thức thanh toán và loại thẻ
                            KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                            AddToDic(ref dicPayUserBank, KeyGroup, item, null);
                        }
                    }
                }

                LogSystem.Info("7=========");

                //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listRepay), listRepay));
                if (IsNotNullOrEmpty(listRepay))
                {
                    foreach (var item in listRepay)
                    {
                        if (item.IS_CANCEL == IS_TRUE)
                            continue;

                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua Repay: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (castFilter.PATIENT_TYPE_IDs != null && !castFilter.PATIENT_TYPE_IDs.Contains(dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID))
                        {
                            continue;
                        }

                        if (item.REPAY_REASON_ID.HasValue && item.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__01)//ly do hoan lai tien tam ung
                        {
                            continue;
                        }

                        if ((item.TDL_SESE_DEPO_REPAY_COUNT.HasValue && item.TDL_SESE_DEPO_REPAY_COUNT.Value > 0) ||
                            (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0)) ||
                            dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME > item.TRANSACTION_TIME ||
                            (item.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__02))
                        {

                            listTranId.Add(item.ID);
                            if (castFilter.IS_ADD_INFO_AREA == true)
                            {
                                List<string> keySub = dicTransactionArea.Keys.Where(o => o.StartsWith(string.Format("{0}_", item.ID))).ToList();
                                foreach (var key in keySub)
                                {
                                    var ssr = dicTransactionArea[key];
                                    //group theo thu ngân và khu vực
                                    string KeyGroup = "{0}_{1}";//cashier_loginname và area_id
                                    AddToDic(ref dicRdo, KeyGroup, item, ssr);

                                    //group theo thu ngan va hinh thuc thanh toan
                                    KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                                    AddToDic(ref dicPayUser, KeyGroup, item, ssr);

                                    //group theo hinh thuc thanh toan
                                    KeyGroup = "{2}";//pay_form_id
                                    AddToDic(ref dicPayForm, KeyGroup, item, ssr);

                                    //group theo thu ngân, hình thức thanh toán và loại thẻ
                                    KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                                    AddToDic(ref dicPayUserBank, KeyGroup, item, ssr);
                                }
                            }
                            else
                            {
                                //group theo thu ngân 
                                string KeyGroup = "{0}";//cashier_loginname
                                AddToDic(ref dicRdo, KeyGroup, item, null);

                                //group theo thu ngan va hinh thuc thanh toan
                                KeyGroup = "{0}_{2}";//cashier_loginname và pay_form_id
                                AddToDic(ref dicPayUser, KeyGroup, item, null);

                                //group theo hinh thuc thanh toan
                                KeyGroup = "{2}";//pay_form_id
                                AddToDic(ref dicPayForm, KeyGroup, item, null);

                                //group theo thu ngân, hình thức thanh toán và loại thẻ
                                KeyGroup = "{0}_{2}_{3}";//cashier_loginname, pay_form_id và typeBankCard
                                AddToDic(ref dicPayUserBank, KeyGroup, item, null);
                            }
                        }
                        else
                        {
                            Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                            {
                                Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => dicPatientTypeAlter[item.TREATMENT_ID ?? 0]), dicPatientTypeAlter[item.TREATMENT_ID ?? 0]));
                            }
                        }
                    }
                }
                AddArea(dicRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddToDic(ref Dictionary<string, Mrs00287RDO> dic, string KeyGroup, V_HIS_TRANSACTION item, Mrs00296RDO ss)
        {
            Mrs00287RDO rdo = null;
            var typeBankCard = string.IsNullOrEmpty(item.TDL_BANK_CARD_CODE) || item.TDL_BANK_CARD_CODE.Length < 6 ? "" : item.TDL_BANK_CARD_CODE.Substring(0, 6);
            //if (item.IS_CANCEL == 2)
            //{
            //    item.PAY_FORM_ID = IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM;
            //    var pAY_FORM = HisPayFormCFG.ListPayForm.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM);
            //    if (pAY_FORM != null)
            //    {
            //        item.PAY_FORM_CODE = pAY_FORM.PAY_FORM_CODE;
            //        item.PAY_FORM_NAME = pAY_FORM.PAY_FORM_NAME;

            //    }
            //}
           
            
            string key = string.Format(KeyGroup, item.CASHIER_LOGINNAME, ss==null?0:ss.AREA_ID, item.PAY_FORM_ID, typeBankCard);
          
            if (dic.ContainsKey(key))
            {
                rdo = dic[key];
            }
            else
            {
                rdo = new Mrs00287RDO();
                rdo.AREA_ID =ss==null ? 0 : ss.AREA_ID ?? 0;
                var area = listArea.FirstOrDefault(o => o.ID == (ss==null ? 0 : ss.AREA_ID)) ?? new HIS_AREA();
                rdo.AREA_CODE = area.AREA_CODE;
                rdo.AREA_NAME = area.AREA_NAME;
                rdo.CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                rdo.CASHIER_USERNAME = item.CASHIER_USERNAME;

              
                    rdo.PAY_FORM_CODE = item.PAY_FORM_CODE;
                    rdo.PAY_FORM_NAME = item.PAY_FORM_NAME;
                    rdo.PAY_FORM_ID = item.PAY_FORM_ID;
                
                
                    
                
                rdo.TYPE_BANK_CARD = typeBankCard;
                dic[key] = rdo;
            }
            //đếm số hồ sơ điều trị
            if (!dicTreatId.ContainsKey(item.TREATMENT_ID ?? 0))
            {
                rdo.TOTAL_PATIENT_COUNT += 1;
                dicTreatId.Add(item.TREATMENT_ID ?? 0, item.TREATMENT_ID ?? 0);
            }

            if (ss != null)// tính tiền thu với trường hợp tách chi tiết khu vực
            {
                if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    if (item.IS_CANCEL != 2)
                    {
                        decimal DepBil = ss.TOTAL_DEPOSIT_BILL_PRICE;
                        rdo.DEP_BIL += DepBil;

                        if (item.AMOUNT > 0)
                        {
                            rdo.DEP_BIL_TRANSFER += DepBil * (item.TRANSFER_AMOUNT ?? 0) / item.AMOUNT;
                        }

                        rdo.PATIENT_BHYT_PRICE += ss.TOTAL_PATIENT_BHYT_PRICE;
                        rdo.CHENH_LECH += ss.TOTAL_DEPOSIT_BILL_PRICE - ss.TOTAL_PATIENT_BHYT_PRICE;
                        rdo.EXAM_PRICE += ss.EXAM_PRICE;
                        rdo.CLS_PRICE += ss.TOTAL_DEPOSIT_BILL_PRICE - ss.EXAM_PRICE;
                    }
                    else
                    {
                        decimal Repay = ss.TOTAL_DEPOSIT_BILL_PRICE;
                        rdo.REPAYs += Repay;
                    }
                }
                else if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    rdo.REPAYs += ss.TOTAL_REPAY_PRICE;
                }
            }
            else // tính tiền thu trong trường hợp không tách chi tiết khu vực
            {
                if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    decimal totalFund = 0;
                    if (dicBillFund.ContainsKey(item.ID))
                    {
                        totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                    }
                    if (item.IS_CANCEL != 2)
                    {

                        decimal DepBil = (item.AMOUNT - (item.EXEMPTION ?? 0) - (item.KC_AMOUNT ?? 0) - totalFund);//tạm ứng và hoàn ứng thì sẽ không có kc amount và totalFund
                        rdo.DEP_BIL += DepBil;
                        if (DepBil > 0)
                        {
                            rdo.DEP_BIL_TRANSFER += item.TRANSFER_AMOUNT ?? 0;
                        }
                    }
                    else
                    {
                        decimal Repay = (item.AMOUNT - (item.EXEMPTION ?? 0) - (item.KC_AMOUNT ?? 0) - totalFund);//tạm ứng và hoàn ứng thì sẽ không có kc amount và totalFund
                        
                        rdo.REPAYs += Repay;
                    }
                }
                else if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    rdo.REPAYs += item.AMOUNT;
                }
            }
        }

        private void ProcessDetail(List<Mrs00296RDO> listSereServRdo)
        {
            try
            {
                if (listTransaction != null)
                {
                    foreach (var item in listTransaction)
                    {
                        if (!listTranId.Contains(item.ID))
                        {
                            continue;
                        }
                        if (castFilter.IS_ADD_INFO_AREA == true)
                        {
                            List<string> keySub = dicTransactionArea.Keys.Where(o => o.StartsWith(string.Format("{0}_", item.ID))).ToList();
                            foreach (var key in keySub)
                            {
                                var ss = dicTransactionArea[key];
                                Mrs00287RDO rdo = new Mrs00287RDO();


                               // rdo.IS_CANCEL = item.IS_CANCEL;
                                //if (item.IS_CANCEL == 2)
                                //{
                                //    rdo.PAY_FORM_ID = IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM;
                                //    var pAY_FORM = HisPayFormCFG.ListPayForm.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM);
                                //    if (pAY_FORM != null)
                                //    {
                                //        rdo.PAY_FORM_CODE = pAY_FORM.PAY_FORM_CODE;
                                //        rdo.PAY_FORM_NAME = pAY_FORM.PAY_FORM_NAME;
                                    
                                //    }
                                 

                                //}
                                //else
                                //{
                                    rdo.PAY_FORM_ID = item.PAY_FORM_ID;


                                    rdo.PAY_FORM_CODE = item.PAY_FORM_CODE;
                                    rdo.PAY_FORM_NAME = item.PAY_FORM_NAME;
                                //}
                                rdo.CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                                rdo.CASHIER_USERNAME = item.CASHIER_USERNAME;
                                rdo.NUM_ORDER = item.NUM_ORDER;
                                rdo.TRANS_REQ_CODE = item.TRANS_REQ_CODE;
                                rdo.BANK_TRANSACTION_CODE = item.BANK_TRANSACTION_CODE;
                                rdo.BANK_TRANSACTION_TIME = item.BANK_TRANSACTION_TIME;
                                rdo.TDL_TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                                rdo.BANK_CARD_CODE = item.TDL_BANK_CARD_CODE;
                                rdo.TYPE_BANK_CARD = string.IsNullOrEmpty(item.TDL_BANK_CARD_CODE) || item.TDL_BANK_CARD_CODE.Length < 6 ? "" : item.TDL_BANK_CARD_CODE.Substring(0, 6);
                                rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                                rdo.AREA_ID = ss.AREA_ID ?? 0;
                                rdo.PATIENT_BHYT_PRICE = ss.TOTAL_PATIENT_BHYT_PRICE;

                                //bắt đầu xử lý lại tiền thu, chi nếu có lấy giao dịch hủy
                                if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                                {
                                    if (item.IS_CANCEL != 2)
                                    {
                                        decimal DepBil = ss.TOTAL_DEPOSIT_BILL_PRICE;
                                        rdo.DEP_BIL = DepBil;

                                        if (item.AMOUNT > 0)
                                        {
                                            rdo.DEP_BIL_TRANSFER = DepBil * (item.TRANSFER_AMOUNT ?? 0) / item.AMOUNT;
                                        }

                                        rdo.PATIENT_BHYT_PRICE = ss.TOTAL_PATIENT_BHYT_PRICE;
                                        rdo.CHENH_LECH = ss.TOTAL_DEPOSIT_BILL_PRICE - ss.TOTAL_PATIENT_BHYT_PRICE;
                                        rdo.EXAM_PRICE = ss.EXAM_PRICE;
                                        rdo.CLS_PRICE = ss.TOTAL_DEPOSIT_BILL_PRICE - ss.EXAM_PRICE;
                                    }
                                    else
                                    {
                                        decimal Repay = ss.TOTAL_DEPOSIT_BILL_PRICE;
                                        rdo.REPAYs = Repay;
                                    }
                                }
                                else if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                                {
                                    rdo.REPAYs = ss.TOTAL_REPAY_PRICE;
                                }

                                listDetail.Add(rdo);
                            }
                        }
                        else
                        {
                            var bill = listBill.Where(p => p.TREATMENT_ID == item.TREATMENT_ID);
                            var depo = listDeposit.Where(p => p.TREATMENT_ID == item.TREATMENT_ID);
                            var repay = listRepay.Where(p => p.TREATMENT_ID == item.TREATMENT_ID);
                            Mrs00287RDO rdo = new Mrs00287RDO();
                           // rdo.IS_CANCEL = item.IS_CANCEL;
                            //if (item.IS_CANCEL == 2)
                            //{
                            //    rdo.PAY_FORM_ID = IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM;
                            //    var pAY_FORM = HisPayFormCFG.ListPayForm.FirstOrDefault(o => o.ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM);
                            //    if (pAY_FORM != null)
                            //    {
                            //        rdo.PAY_FORM_CODE = pAY_FORM.PAY_FORM_CODE;
                            //        rdo.PAY_FORM_NAME = pAY_FORM.PAY_FORM_NAME;

                            //    }

                            //}
                            //else
                            //{
                                rdo.PAY_FORM_ID = item.PAY_FORM_ID;
                                rdo.PAY_FORM_CODE = item.PAY_FORM_CODE;
                                rdo.PAY_FORM_NAME = item.PAY_FORM_NAME;
                           // }
                            rdo.CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                            rdo.CASHIER_USERNAME = item.CASHIER_USERNAME;
                            rdo.NUM_ORDER = item.NUM_ORDER;
                            rdo.TRANS_REQ_CODE = item.TRANS_REQ_CODE;
                            rdo.BANK_TRANSACTION_CODE = item.BANK_TRANSACTION_CODE;
                            rdo.BANK_TRANSACTION_TIME = item.BANK_TRANSACTION_TIME;
                            rdo.TDL_TREATMENT_CODE = item.TDL_TREATMENT_CODE;
                            rdo.BANK_CARD_CODE = item.TDL_BANK_CARD_CODE;
                            rdo.TYPE_BANK_CARD = string.IsNullOrEmpty(item.TDL_BANK_CARD_CODE) || item.TDL_BANK_CARD_CODE.Length < 6 ? "" : item.TDL_BANK_CARD_CODE.Substring(0, 6);
                            rdo.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
                            if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU || item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                            {
                                decimal totalFund = 0;
                                if (dicBillFund.ContainsKey(item.ID))
                                {
                                    totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                                }
                                if (item.IS_CANCEL != 2)
                                {

                                    decimal DepBil = (item.AMOUNT - (item.EXEMPTION ?? 0) - (item.KC_AMOUNT ?? 0) - totalFund);//tạm ứng và hoàn ứng thì sẽ không có kc amount và totalFund
                                    rdo.DEP_BIL = DepBil;
                                    if (DepBil > 0)
                                    {
                                        rdo.DEP_BIL_TRANSFER = item.TRANSFER_AMOUNT ?? 0;
                                    }
                                }
                                else
                                {
                                    decimal Repay = (item.AMOUNT - (item.EXEMPTION ?? 0) - (item.KC_AMOUNT ?? 0) - totalFund);//tạm ứng và hoàn ứng thì sẽ không có kc amount và totalFund

                                    rdo.REPAYs = Repay;
                                }
                            }
                            else if (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                            {
                                rdo.REPAYs = item.AMOUNT;
                            }

                            listDetail.Add(rdo);
                        }



                    }
                }


                AddArea(listDetail);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddArea(Dictionary<string, Mrs00287RDO> dicRdo)
        {
            foreach (var item in dicRdo)
            {
                var area = listArea.FirstOrDefault(o => o.ID == item.Value.AREA_ID) ?? new HIS_AREA();
                item.Value.AREA_CODE = area.AREA_CODE;
                item.Value.AREA_NAME = area.AREA_NAME;
            }
        }

        private void AddArea(List<Mrs00287RDO> listDetail)
        {
            foreach (var item in listDetail)
            {
                var area = listArea.FirstOrDefault(o => o.ID == item.AREA_ID) ?? new HIS_AREA();
                item.AREA_CODE = area.AREA_CODE;
                item.AREA_NAME = area.AREA_NAME;
            }
        }

        private List<V_HIS_BILL_FUND> GetBillFundByListBill(ref CommonParam paramGet)
        {
            List<V_HIS_BILL_FUND> listBillFund = new List<V_HIS_BILL_FUND>();
            try
            {
                int start = 0;
                int count = listBill.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var hisBills = listBill.Skip(start).Take(limit).ToList();
                    HisBillFundViewFilterQuery billFundFilter = new HisBillFundViewFilterQuery();
                    billFundFilter.BILL_IDs = hisBills.Select(s => s.ID).ToList();
                    var hisBillFunds = new MOS.MANAGER.HisBillFund.HisBillFundManager(paramGet).GetView(billFundFilter);
                    if (IsNotNullOrEmpty(hisBillFunds))
                    {
                        listBillFund.AddRange(hisBillFunds);
                    }
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listBillFund = null;
            }
            return listBillFund;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID) ?? new HIS_BRANCH();
                dicSingleTag.Add("BRANCH_NAME", branch.BRANCH_NAME);
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CREATE_TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.CREATE_TIME_TO));
                dicSingleTag.Add("CASHIER_ROOM_NAME", listCashierRoom.Count > 0 && castFilter.EXACT_CASHIER_ROOM_IDs != null ? string.Join(" - ", listCashierRoom.Select(o => o.CASHIER_ROOM_NAME).ToList()) : "");
                dicSingleTag.Add("PATIENT_TYPE_NAME", string.Join(" - ", HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs == null || castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).Select(o => o.PATIENT_TYPE_NAME).ToList()));

                objectTag.AddObjectData(store, "Report", dicRdo.Select(s => s.Value).OrderBy(o => o.CASHIER_LOGINNAME).ToList());
                var payForm = dicPayForm.Select(s => s.Value).OrderBy(o => o.PAY_FORM_ID).ToList();
                foreach (var item in payForm)
                {
                    if (!dicSingleTag.ContainsKey(string.Format("PAY_FORM_CODE__{0}", item.PAY_FORM_CODE)))
                    dicSingleTag.Add(string.Format("PAY_FORM_CODE__{0}", item.PAY_FORM_CODE), item.PAY_FORM_CODE);
                    if (!dicSingleTag.ContainsKey(string.Format("PAY_FORM_NAME__{0}", item.PAY_FORM_CODE)))
                    dicSingleTag.Add(string.Format("PAY_FORM_NAME__{0}", item.PAY_FORM_CODE), item.PAY_FORM_NAME);
                }
                objectTag.AddObjectData(store, "PayForm", payForm);
                objectTag.AddObjectData(store, "PayUser", this.dicPayUser.Select(p => p.Value).OrderBy(o => o.CASHIER_LOGINNAME).ToList());
                objectTag.AddObjectData(store, "PayUserBank", this.dicPayUserBank.Select(p => p.Value).OrderBy(o => o.CASHIER_LOGINNAME).ToList());
                dicSingleTag.Add("AREA_NAMEs", castFilter.AREA_IDs == null ? "" : string.Join(" - ", (this.listArea ?? new List<HIS_AREA>()).Where(o => castFilter.AREA_IDs.Contains(o.ID)).Select(o => o.AREA_NAME).ToList()));
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAMEs", castFilter.REQUEST_DEPARTMENT_IDs == null ? "" : string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.ID)).Select(o => o.DEPARTMENT_NAME).ToList()));
                objectTag.AddObjectData(store, "Holidays", Holidays.Select(o => new HIS_TRANSACTION() { TRANSACTION_DATE = o }).ToList());
                objectTag.AddObjectData(store, "TransactionArea", dicTransactionArea.Values.ToList());

                objectTag.AddObjectData(store, "Detail", listDetail.Where(p => (p.DEP_BIL > 0 || p.REPAYs > 0) && !string.IsNullOrEmpty(p.CASHIER_LOGINNAME)).OrderBy(p => p.TDL_TREATMENT_CODE).ToList());

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}