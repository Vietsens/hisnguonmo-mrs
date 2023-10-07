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
using MRS.Processor.Mrs00296;
using Inventec.Common.Logging;
using MOS.MANAGER.HisRoom;

namespace MRS.Processor.Mrs00289
{
    class Mrs00289Processor : AbstractProcessor
    {
        Mrs00289Filter castFilter = null;

        List<Mrs00289RDO> listRdo = new List<Mrs00289RDO>();
        List<Mrs00289RDO> listPatientType = new List<Mrs00289RDO>();
        List<Mrs00289RDO> listRdoHasCancel = new List<Mrs00289RDO>();

        List<V_HIS_TRANSACTION> listBill = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listDeposit = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listRepay = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
        List<V_HIS_CASHIER_ROOM> listCashierRoom = new List<V_HIS_CASHIER_ROOM>();
        List<Mrs00296RDO> listSereServRdo = new List<Mrs00296RDO>();
        List<HIS_AREA> listArea = new List<HIS_AREA>();
        Dictionary<string, Mrs00296RDO> dicTransactionArea = new Dictionary<string, Mrs00296RDO>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();

        HIS_PATIENT_TYPE patientTypeSource = new HIS_PATIENT_TYPE();

        HIS_PATIENT_TYPE patientTypeDest = new HIS_PATIENT_TYPE();

        List<CARD> listCard = new List<CARD>();

        string cashierUsername = "";
        List<long> Holidays = new List<long>();

        public Mrs00289Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00289Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00289Filter)this.reportFilter;
                if (!string.IsNullOrWhiteSpace(castFilter.CHANGE_TYPE_TO_TYPE))
                {
                    string[] patientTypeCodes = castFilter.CHANGE_TYPE_TO_TYPE.Split(new String[]{"->"}, StringSplitOptions.RemoveEmptyEntries);
                    if(patientTypeCodes.Length==2)
                    {
                        string patientTypeCodeSource = "";
                        string patientTypeCodeDest = "";
                        patientTypeCodeSource = patientTypeCodes[0];
                        patientTypeCodeDest = patientTypeCodes[1];
                        if (!string.IsNullOrWhiteSpace(patientTypeCodeSource) && !string.IsNullOrWhiteSpace(patientTypeCodeDest))
                        {
                            patientTypeSource = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.PATIENT_TYPE_CODE == patientTypeCodeSource);
                            patientTypeDest = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.PATIENT_TYPE_CODE == patientTypeCodeDest);
                           
                        }
                    }
                }
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_BILL, V_HIS_DEPOSIT, V_HIS_REPAY, MRS00289: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                HisTransactionViewFilterQuery transactionFilter = new HisTransactionViewFilterQuery();
                transactionFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionFilter.TRANSACTION_TYPE_IDs = castFilter.TRANSACTION_TYPE_IDs;
                //transactionFilter.HAS_SALL_TYPE = false;
                //transactionFilter.CASHIER_ROOM_IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(transactionFilter);

                if (castFilter.LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }

                if (castFilter.CASHIER_LOGINNAME != null)
                {
                    listTransaction = listTransaction.Where(o => castFilter.CASHIER_LOGINNAME == o.CASHIER_LOGINNAME).ToList();
                }
                //them giao dich thanh toan huy
                if (castFilter.IS_ADD_BILL_CANCEL == true)
                {
                    HisTransactionViewFilterQuery billCancelFilter = new HisTransactionViewFilterQuery();
                    billCancelFilter.CANCEL_TIME_FROM = castFilter.TIME_FROM;
                    billCancelFilter.CANCEL_TIME_TO = castFilter.TIME_TO;
                    billCancelFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                    billCancelFilter.IS_CANCEL = true;
                    //transactionFilter.HAS_SALL_TYPE = false;
                    //transactionFilter.CASHIER_ROOM_IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                    var listBillCancel = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(billCancelFilter);
                    listBillCancel = listBillCancel.Where(o => o.TREATMENT_ID.HasValue).ToList();
                    if (listBillCancel != null)
                    {

                        if (castFilter.LOGINNAME != null)
                        {
                            listBillCancel = listBillCancel.Where(o => castFilter.LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                        }

                        if (castFilter.CASHIER_LOGINNAME != null)
                        {
                            listBillCancel = listBillCancel.Where(o => castFilter.CASHIER_LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                        }
                        foreach (var item in listBillCancel)
                        {
                            item.CASHIER_LOGINNAME = item.CANCEL_LOGINNAME;
                            item.CASHIER_USERNAME = item.CANCEL_USERNAME;
                        }
                        listTransaction.AddRange(listBillCancel);
                        listTransaction = listTransaction.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    }
                }

                //them giao dich tam ung huy
                if (castFilter.IS_ADD_DEPO_CANCEL == true)
                {
                    HisTransactionViewFilterQuery billCancelFilter = new HisTransactionViewFilterQuery();
                    billCancelFilter.CANCEL_TIME_FROM = castFilter.TIME_FROM;
                    billCancelFilter.CANCEL_TIME_TO = castFilter.TIME_TO;
                    billCancelFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU };
                    billCancelFilter.IS_CANCEL = true;
                    //transactionFilter.HAS_SALL_TYPE = false;
                    //transactionFilter.CASHIER_ROOM_IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                    var listDepositCancel = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(billCancelFilter);
                    listDepositCancel = listDepositCancel.Where(o => o.TREATMENT_ID.HasValue).ToList();
                    if (listDepositCancel != null)
                    {

                        if (castFilter.EXACT_CASHIER_ROOM_IDs != null)
                        {
                            listDepositCancel = listDepositCancel.Where(o => castFilter.EXACT_CASHIER_ROOM_IDs.Contains(o.CANCEL_CASHIER_ROOM_ID ?? 0)).ToList();
                        }
                        if (castFilter.CASHIER_LOGINNAME != null)
                        {
                            listDepositCancel = listDepositCancel.Where(o => castFilter.CASHIER_LOGINNAME == o.CANCEL_LOGINNAME).ToList();
                        }
                        foreach (var item in listDepositCancel)
                        {
                            item.CASHIER_LOGINNAME = item.CANCEL_LOGINNAME;
                            item.CASHIER_USERNAME = item.CANCEL_USERNAME;
                        }
                        listTransaction.AddRange(listDepositCancel);
                        listTransaction = listTransaction.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                    }
                }
                listTransaction = listTransaction.Where(o => o.TREATMENT_ID.HasValue).ToList();

                HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery();
                cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID;
                cashierRoomFilter.IDs = castFilter.EXACT_CASHIER_ROOM_IDs;
                listCashierRoom = new MOS.MANAGER.HisCashierRoom.HisCashierRoomManager(paramGet).GetView(cashierRoomFilter);

                listTransaction = listTransaction.Where(o => listCashierRoom.Exists(p => p.ID == o.CASHIER_ROOM_ID)).ToList();

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

                listBill = listTransaction.Where(o => (castFilter.PAY_FORM_ID.HasValue ? castFilter.PAY_FORM_ID.Value == o.PAY_FORM_ID : true) && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                if (castFilter.ADD__SALE_EXP != true)
                {
                    listBill = listBill.Where(o => o.SALE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP).ToList();
                }
                listDeposit = listTransaction.Where(o => (castFilter.PAY_FORM_ID.HasValue ? castFilter.PAY_FORM_ID.Value == o.PAY_FORM_ID : true) && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU &&
                    o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue).ToList();

                listRepay = listTransaction.Where(o => (castFilter.PAY_FORM_ID.HasValue ? castFilter.PAY_FORM_ID.Value == o.PAY_FORM_ID : true) && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();

                cashierUsername = String.Join(",", listTransaction.Select(o => o.CASHIER_USERNAME).Distinct().ToList());

                GetSereServDetail(castFilter);

                //get danh sách thẻ
                GetCard(castFilter);

                //lọc theo danh sach thẻ
                FilterByCard(castFilter);

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00289");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByCard(Mrs00289Filter filter)
        {
            if (filter.INPUT_DATA_ID_BC_TYPE > 1)//trường hợp có chọn loại ngân hàng
            {
                List<string> listCardCode = listCard != null ? listCard.Select(o => o.BANK_CARD_CODE).ToList() : new List<string>();
                listTransaction = listTransaction.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listBill = listBill.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listDeposit = listDeposit.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                listRepay = listRepay.Where(o => !string.IsNullOrWhiteSpace(o.TDL_BANK_CARD_CODE) && listCardCode.Contains(o.TDL_BANK_CARD_CODE ?? "")).ToList();
                var transactionIds = listTransaction.Select(o => o.ID).ToList();
                listSereServRdo = listSereServRdo.Where(o => transactionIds.Contains(o.TRANSACTION_ID??0)).ToList();
                //dicTransactionArea = dicTransactionArea.Where(o => transactionIds.Contains(o.Value.TRANSACTION_ID ?? 0)).ToDictionary<string, Mrs00296RDO>();
            }
        }

        private void GetCard(Mrs00289Filter filter)
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
        private void GetSereServDetail(Mrs00289Filter castFilter)
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
                    dicTransactionArea = listSereServRdo.GroupBy(g => string.Format("{0}_{1}_{2}", g.TRANSACTION_ID, g.AREA_ID, g.SS_PATIENT_TYPE_ID)).ToDictionary(p => p.Key, q => new Mrs00296RDO() { TRANSACTION_ID = q.First().TRANSACTION_ID, AREA_ID = q.First().AREA_ID, SS_PATIENT_TYPE_ID = q.First().SS_PATIENT_TYPE_ID, TOTAL_DEPOSIT_BILL_PRICE = q.Sum(s => s.TOTAL_DEPOSIT_BILL_PRICE), TOTAL_REPAY_PRICE = q.Sum(s => s.TOTAL_REPAY_PRICE), TOTAL_PATIENT_BHYT_PRICE = q.Sum(s => s.TOTAL_PATIENT_BHYT_PRICE) });
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
                item.PRE_CASHIER_USERNAME = item.CASHIER_USERNAME;
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
                if (item.SS_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && item.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    item.TOTAL_PATIENT_BHYT_PRICE = item.TOTAL_DEPOSIT_BILL_PRICE - item.CHENHLECH;
                }
                else
                {
                    item.TOTAL_PATIENT_BHYT_PRICE = 0;
                }
                if (castFilter.INPUT_DATA_ID_PRICE_TYPEs != null && !castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)1))
                {
                    if (item.SS_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && item.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.TOTAL_REPAY_PRICE = 0;
                        item.TOTAL_PATIENT_BHYT_PRICE = 0;
                        item.TOTAL_DEPOSIT_BILL_PRICE = item.CHENHLECH;

                    }

                }

                if (castFilter.INPUT_DATA_ID_PRICE_TYPEs != null && !castFilter.INPUT_DATA_ID_PRICE_TYPEs.Contains((short)2))
                {
                    item.TOTAL_DEPOSIT_BILL_PRICE -= item.CHENHLECH;
                    if (item.SS_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT || item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        item.TOTAL_DEPOSIT_BILL_PRICE = 0;
                        item.TOTAL_REPAY_PRICE = 0;

                    }


                }

            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                List<long> listTreatmentId = new List<long>();
                List<V_HIS_BILL_FUND> listBillFund = null;
                if (IsNotNullOrEmpty(listBill))
                {
                    listBillFund = GetBillFundByListBill(ref paramGet);
                }

                if (IsNotNullOrEmpty(listTransaction))
                {
                    listTreatmentId = listTransaction.Select(s => s.TREATMENT_ID ?? 0).Distinct().ToList();
                }
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();

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
                    ProcessListPatientType();
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListPatientType()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.CREATE_TIME).ThenBy(t => t.TREATMENT_ID).ToList();
                    var Groups = listRdo.GroupBy(o => o.PATIENT_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00289RDO>();
                        Mrs00289RDO rdo = new Mrs00289RDO();
                        rdo.PATIENT_TYPE_ID = listSub.First().PATIENT_TYPE_ID;
                        rdo.PATIENT_TYPE_NAME = listSub.First().PATIENT_TYPE_NAME;
                        rdo.TOTAL_DEPOSIT_BILL_AMOUNT = listSub.Sum(s => s.TOTAL_DEPOSIT_BILL_AMOUNT);
                        rdo.TOTAL_REPAY_AMOUNT = listSub.Sum(o => o.TOTAL_REPAY_AMOUNT);
                        if ((castFilter.PATIENT_TYPE_ID ?? 0) == 0 || listSub.First().PATIENT_TYPE_ID == castFilter.PATIENT_TYPE_ID)
                        {
                            listPatientType.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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
        //lấy ds phòng thu ngân
        private void ProcessListTransaction(Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicCurrentPatyAlter, Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, List<V_HIS_BILL_FUND> listBillFund)
        {
            try
            {
                Dictionary<long, List<V_HIS_BILL_FUND>> dicBillFund = new Dictionary<long, List<V_HIS_BILL_FUND>>();
                if (!IsNotNullOrEmpty(listCashierRoom))
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc danh sach phong thu ngan: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listCashierRoom), listCashierRoom));
                    return;
                }

                if (IsNotNullOrEmpty(listBillFund))
                {
                    foreach (var item in listBillFund)
                    {
                        if (!dicBillFund.ContainsKey(item.BILL_ID))
                            dicBillFund[item.BILL_ID] = new List<V_HIS_BILL_FUND>();
                        dicBillFund[item.BILL_ID].Add(item);
                    }
                }

                if (IsNotNullOrEmpty(listBill))
                {
                    foreach (var item in listBill)
                    {
                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua bill: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (castFilter.PATIENT_TYPE_ID != null && castFilter.PATIENT_TYPE_ID != dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID)
                            continue;


                        if ((!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0)) || dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME > item.TRANSACTION_TIME)//Chỉ lấy các thanh toán của BN ngoại trú hoặc trước khi vào nội trú
                        {
                            if (castFilter.IS_ADD_INFO_AREA == true)
                            {

                                var ssSub = dicTransactionArea.Values.Where(o => o.TRANSACTION_ID == item.ID).ToList();
                                foreach (var ss in ssSub)
                                {

                                    var area = listArea.FirstOrDefault(o => o.ID == ss.AREA_ID) ?? new HIS_AREA();
                                    ss.AREA_CODE = area.AREA_CODE;
                                    ss.AREA_NAME = area.AREA_NAME;
                                    if (castFilter.IS_CHANGE_TO_BHYT == true)
                                    {
                                        if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                        {
                                            ss.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                        }
                                    }
                                    if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                                    {
                                        if (patientTypeSource.ID == ss.SS_PATIENT_TYPE_ID)
                                        {
                                            ss.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                        }
                                    }
                                    var ssPaty = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.SS_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                                    ss.SS_PATIENT_TYPE_CODE = ssPaty.PATIENT_TYPE_CODE;
                                    ss.SS_PATIENT_TYPE_NAME = ssPaty.PATIENT_TYPE_NAME;
                                    Mrs00289RDO rdo = new Mrs00289RDO(item, 0, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], this.castFilter, ss);
                                    if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                                    {
                                        rdo.LOG_TIME = dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME;
                                    }
                                    listRdoHasCancel.Add(rdo);
                                    if (item.IS_CANCEL != 1|| castFilter.IS_ADD_BILL_CANCEL == true)
                                        listRdo.Add(rdo);
                                }
                            }
                            else
                            {
                                decimal totalFund = 0;
                                if (dicBillFund.ContainsKey(item.ID))
                                {
                                    totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                                }

                                Mrs00289RDO rdo = new Mrs00289RDO(item, totalFund, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], this.castFilter, null);
                                if (dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                                {
                                    rdo.LOG_TIME = dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME;
                                }
                                if (castFilter.IS_CHANGE_TO_BHYT == true)
                                {
                                    if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        rdo.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                    }
                                }

                                if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                                {
                                    if (patientTypeSource.ID == rdo.SS_PATIENT_TYPE_ID)
                                    {
                                        rdo.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                    }
                                }
                                listRdoHasCancel.Add(rdo);
                                if (item.IS_CANCEL != 1 || castFilter.IS_ADD_BILL_CANCEL == true)
                                    listRdo.Add(rdo);
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var item in listDeposit)
                    {
                        if (!(item.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && item.TDL_SERE_SERV_DEPOSIT_COUNT.Value > 0))//Chỉ lấy các tạm ứng dịch vụ
                            continue;

                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua Deposit: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (castFilter.PATIENT_TYPE_ID != null && castFilter.PATIENT_TYPE_ID != dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID)
                            continue;

                        if (castFilter.IS_ADD_INFO_AREA == true)
                        {
                            var ssSub = dicTransactionArea.Values.Where(o => o.TRANSACTION_ID == item.ID).ToList();
                            foreach (var ss in ssSub)
                            {
                                var area = listArea.FirstOrDefault(o => o.ID == ss.AREA_ID) ?? new HIS_AREA();
                                ss.AREA_CODE = area.AREA_CODE;
                                ss.AREA_NAME = area.AREA_NAME;
                                if (castFilter.IS_CHANGE_TO_BHYT ==true)
                                {
                                    if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        ss.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                    }
                                }

                                if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                                {
                                    if (patientTypeSource.ID == ss.SS_PATIENT_TYPE_ID)
                                    {
                                        ss.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                    }
                                }
                                var ssPaty = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.SS_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                                ss.SS_PATIENT_TYPE_CODE = ssPaty.PATIENT_TYPE_CODE;
                                ss.SS_PATIENT_TYPE_NAME = ssPaty.PATIENT_TYPE_NAME;
                                Mrs00289RDO rdo = new Mrs00289RDO(item, null, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], item.TRANSACTION_TYPE_ID, this.castFilter, ss);
                                listRdoHasCancel.Add(rdo);
                                if (item.IS_CANCEL != 1 || castFilter.IS_ADD_DEPO_CANCEL == true)
                                    listRdo.Add(rdo);
                            }
                        }
                        else
                        {
                            decimal totalFund = 0;
                            if (dicBillFund.ContainsKey(item.ID))
                            {
                                totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                            }

                            Mrs00289RDO rdo = new Mrs00289RDO(item, null, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], item.TRANSACTION_TYPE_ID, this.castFilter, null);
                            if (castFilter.IS_CHANGE_TO_BHYT ==true)
                            {
                                if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    rdo.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                }
                            }

                            if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                            {
                                if (patientTypeSource.ID == rdo.SS_PATIENT_TYPE_ID)
                                {
                                    rdo.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                }
                            }
                            listRdoHasCancel.Add(rdo);
                            if (item.IS_CANCEL != 1 || castFilter.IS_ADD_DEPO_CANCEL == true)
                                listRdo.Add(rdo);
                        }
                    }
                }

                if (IsNotNullOrEmpty(listRepay))
                {
                    foreach (var item in listRepay)
                    {
                        if (!dicCurrentPatyAlter.ContainsKey(item.TREATMENT_ID ?? 0))
                        {
                            Inventec.Common.Logging.LogSystem.Info("Khong lay duoc currentPatientTypeAlter cua Repay: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => item), item));
                            continue;
                        }

                        if (item.REPAY_REASON_ID.HasValue && item.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__01)//ly do hoan lai tien tam ung
                        {
                            continue;
                        }
                        if (castFilter.PATIENT_TYPE_ID != null && castFilter.PATIENT_TYPE_ID != dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID)
                            continue;

                        if (castFilter.IS_REPAY_SERVICE == true && item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && (item.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0)
                            continue;

                        if ((item.TDL_SESE_DEPO_REPAY_COUNT.HasValue && item.TDL_SESE_DEPO_REPAY_COUNT.Value > 0) || (!dicPatientTypeAlter.ContainsKey(item.TREATMENT_ID ?? 0)) || dicPatientTypeAlter[item.TREATMENT_ID ?? 0].LOG_TIME > item.TRANSACTION_TIME ||
                            (item.REPAY_REASON_ID == HisRepayReasonCFG.get_REPAY_REASON_CODE__02))//Chỉ lấy hoàn ứng dịch vụ hoặc hoàn ứng BN ngoại trú hoặc hoàn ứng trước khi vào nội trú hoặc lí do hoàn ứng là thanh toán ngoại trú - đổi thẻ
                        {
                            var preCash = listTransaction.FirstOrDefault(o => o.TREATMENT_ID == item.TREATMENT_ID && o.TRANSACTION_TIME < item.TRANSACTION_TIME);

                            if (castFilter.IS_ADD_INFO_AREA == true)
                            {
                                var ssSub = dicTransactionArea.Values.Where(o => o.TRANSACTION_ID == item.ID).ToList();
                                foreach (var ss in ssSub)
                                {
                                    var area = listArea.FirstOrDefault(o => o.ID == ss.AREA_ID) ?? new HIS_AREA();
                                    ss.AREA_CODE = area.AREA_CODE;
                                    ss.AREA_NAME = area.AREA_NAME;
                                    var ssPaty = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.SS_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                                    ss.SS_PATIENT_TYPE_CODE = ssPaty.PATIENT_TYPE_CODE;
                                    ss.SS_PATIENT_TYPE_NAME = ssPaty.PATIENT_TYPE_NAME;
                                    if (castFilter.IS_CHANGE_TO_BHYT  == true)
                                    {
                                        if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                        {
                                            ss.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                        }
                                    }

                                    if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                                    {
                                        if (patientTypeSource.ID == ss.SS_PATIENT_TYPE_ID)
                                        {
                                            ss.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                        }
                                    }
                                    Mrs00289RDO rdo = new Mrs00289RDO(item, preCash, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], item.TRANSACTION_TYPE_ID, this.castFilter, ss);
                                    listRdoHasCancel.Add(rdo);
                                    if (item.IS_CANCEL != 1)
                                        listRdo.Add(rdo);
                                }
                            }
                            else
                            {
                                decimal totalFund = 0;
                                if (dicBillFund.ContainsKey(item.ID))
                                {
                                    totalFund = dicBillFund[item.ID].Sum(s => s.AMOUNT);
                                }

                                Mrs00289RDO rdo = new Mrs00289RDO(item, preCash, dicCurrentPatyAlter[item.TREATMENT_ID ?? 0], item.TRANSACTION_TYPE_ID, this.castFilter, null);
                                if (castFilter.IS_CHANGE_TO_BHYT == true)
                                {
                                    if (dicCurrentPatyAlter[item.TREATMENT_ID ?? 0].PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                    {
                                        rdo.SS_PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                                    }
                                }

                                if (patientTypeSource != null && patientTypeSource.ID > 0 && patientTypeDest != null && patientTypeDest.ID > 0)
                                {
                                    if (patientTypeSource.ID == rdo.SS_PATIENT_TYPE_ID)
                                    {
                                        rdo.SS_PATIENT_TYPE_ID = patientTypeDest.ID;
                                    }
                                }
                                listRdoHasCancel.Add(rdo);
                                if (item.IS_CANCEL != 1)
                                    listRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("CASHIER_LOGINNAME", castFilter.LOGINNAME + castFilter.CASHIER_LOGINNAME);
                dicSingleTag.Add("CASHIER_USERNAME", cashierUsername);
                dicSingleTag.Add("CASHIER_ROOM_NAMEs", listCashierRoom.Count > 0 && castFilter.EXACT_CASHIER_ROOM_IDs != null ? string.Join(" - ", listCashierRoom.Select(o => o.CASHIER_ROOM_NAME).ToList()) : "");
                dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => castFilter.PATIENT_TYPE_ID == o.ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientType", listPatientType);
                if (castFilter.IS_ASCENDING == true)
                {
                    exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", listRdo.Where(o => !string.IsNullOrEmpty(o.CASHIER_USERNAME)).OrderBy(p => p.AREA_ID).ThenBy(p => p.CASHIER_ROOM_ID).ThenBy(p => p.CASHIER_LOGINNAME).ThenBy(p => p.DEPOSIT_BILL_NUM_ORDER).ThenBy(p => p.REPAY_NUM_ORDER).ToList());
                }
                else
                {
                    exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", listRdo.Where(o => !string.IsNullOrEmpty(o.CASHIER_USERNAME)).OrderBy(p => p.AREA_ID).ThenByDescending(p => p.PAY_FORM_ID).ThenByDescending(o => o.ACCOUNT_BOOK_CODE).ThenByDescending(p => p.TRANSACTION_TYPE_CODE).ThenByDescending(p => p.DEPOSIT_BILL_NUM_ORDER).ThenByDescending(p => p.REPAY_NUM_ORDER).ToList());
                }

                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "PatientType", "Transaction", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "CashierLoginname", listRdo.GroupBy(g => new { g.PATIENT_TYPE_ID, g.CASHIER_ROOM_CODE, g.CASHIER_LOGINNAME, g.PRE_CASHIER_LOGINNAME }).Select(o => o.First()).OrderBy(p => p.CASHIER_LOGINNAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "CashierRoom", listRdo.GroupBy(g => new { g.PATIENT_TYPE_ID, g.CASHIER_ROOM_CODE }).Select(o => o.First()).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Paty", listRdo.GroupBy(g => g.PATIENT_TYPE_ID).Select(o => o.First()).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Paty", "CashierRoom", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierRoom", "CashierLoginname", new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE" }, new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE" });
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierLoginname", "Transaction", new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME", "PRE_CASHIER_LOGINNAME" }, new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME", "PRE_CASHIER_LOGINNAME" });


                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "CashierLoginnameG", listRdo.GroupBy(g => new { g.PATIENT_TYPE_ID, g.CASHIER_ROOM_CODE, g.CASHIER_LOGINNAME }).Select(o => o.First()).OrderBy(p => p.CASHIER_LOGINNAME).ToList());

                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierRoom", "CashierLoginnameG", new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE" }, new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE" });
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "CashierLoginnameG", "CashierLoginname", new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME" }, new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME" });

                exportSuccess = exportSuccess && store.SetCommonFunctions();

                objectTag.AddObjectData(store, "TransactionTotal", listRdoHasCancel.OrderBy(o => o.ACCOUNT_BOOK_CODE).ThenBy(p => p.TRANSACTION_TYPE_CODE).ThenBy(p => p.DEPOSIT_BILL_NUM_ORDER).ThenBy(p => p.REPAY_NUM_ORDER).ToList());
                objectTag.AddRelationship(store, "PatientType", "TransactionTotal", "PATIENT_TYPE_ID", "PATIENT_TYPE_ID");
                objectTag.AddRelationship(store, "CashierLoginname", "TransactionTotal", new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME", "PRE_CASHIER_LOGINNAME" }, new string[] { "PATIENT_TYPE_ID", "CASHIER_ROOM_CODE", "CASHIER_LOGINNAME", "PRE_CASHIER_LOGINNAME" });

                var payForm = listRdo.GroupBy(o => o.PAY_FORM_ID).ToList();
                foreach (var item in payForm)
                {
                    LogSystem.Info("item" + item.First().PAY_FORM_CODE);
                    dicSingleTag.Add(string.Format("PAY_FORM_CODE__{0}", item.First().PAY_FORM_CODE), item.First().PAY_FORM_CODE);
                    dicSingleTag.Add(string.Format("PAY_FORM_NAME__{0}", item.First().PAY_FORM_CODE), item.First().PAY_FORM_NAME);
                }
                dicSingleTag.Add("AREA_NAMEs", castFilter.AREA_IDs == null ? "" : string.Join(" - ", (this.listArea ?? new List<HIS_AREA>()).Where(o => castFilter.AREA_IDs.Contains(o.ID)).Select(o => o.AREA_NAME).ToList()));
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAMEs", castFilter.REQUEST_DEPARTMENT_IDs == null ? "" : string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.ID)).Select(o => o.DEPARTMENT_NAME).ToList()));

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "LoginnameDepas", (new ManagerSql().GetLoginnameDepa() ?? new List<LoginnameDepa>()));
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ReportDetail", listSereServRdo.Where(o => !string.IsNullOrEmpty(o.CASHIER_USERNAME)).OrderBy(p => p.AREA_ID).ThenBy(p => p.CASHIER_ROOM_ID).ThenBy(p => p.CASHIER_LOGINNAME).ThenBy(p => p.NUM_ORDER).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
