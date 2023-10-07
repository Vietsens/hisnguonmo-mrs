using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00487
{
    class Mrs00487Processor : AbstractProcessor
    {
        Mrs00487Filter castFilter = null;
        List<Mrs00487RDO> listRdo = new List<Mrs00487RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_BILL> listSereServBills = new List<V_HIS_SERE_SERV_BILL>();
        //List<V_HIS_BILL> listBills = new List<V_HIS_BILL>(); 

        List<string> listExecuteRoomCode01s = new List<string>();
        List<string> listExecuteRoomCode02s = new List<string>();
        List<string> listExecuteRoomCode03s = new List<string>();
        List<string> listExecuteRoomCode04s = new List<string>();
        List<string> listExecuteRoomCode05s = new List<string>();
        List<string> listExecuteRoomCode06s = new List<string>();
        List<string> listExecuteRoomCode07s = new List<string>();
        List<string> listExecuteRoomCode08s = new List<string>();
        List<string> listExecuteRoomCode09s = new List<string>();
        List<string> listExecuteRoomCode10s = new List<string>();
        List<string> listExecuteRoomCode11s = new List<string>();
        List<string> listExecuteRoomCode12s = new List<string>();
        List<string> listExecuteRoomCode13s = new List<string>();
        List<string> listExecuteRoomCode14s = new List<string>();
        List<string> listExecuteRoomCode15s = new List<string>();
        List<string> listExecuteRoomCode16s = new List<string>();
        List<string> listExecuteRoomCode17s = new List<string>();
        List<string> listExecuteRoomCode18s = new List<string>();
        List<string> listExecuteRoomCode19s = new List<string>();
        List<string> listExecuteRoomCode20s = new List<string>();

        public Mrs00487Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00487Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00487Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00487: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                // cấu hình
                GetDataConfig(ref listExecuteRoomCode01s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_01");
                GetDataConfig(ref listExecuteRoomCode02s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_02");
                GetDataConfig(ref listExecuteRoomCode03s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_03");
                GetDataConfig(ref listExecuteRoomCode04s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_04");
                GetDataConfig(ref listExecuteRoomCode05s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_05");
                GetDataConfig(ref listExecuteRoomCode06s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_06");
                GetDataConfig(ref listExecuteRoomCode07s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_07");
                GetDataConfig(ref listExecuteRoomCode08s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_08");
                GetDataConfig(ref listExecuteRoomCode09s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_09");
                GetDataConfig(ref listExecuteRoomCode10s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_10");
                GetDataConfig(ref listExecuteRoomCode11s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_11");
                GetDataConfig(ref listExecuteRoomCode12s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_12");
                GetDataConfig(ref listExecuteRoomCode13s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_13");
                GetDataConfig(ref listExecuteRoomCode14s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_14");
                GetDataConfig(ref listExecuteRoomCode15s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_15");
                GetDataConfig(ref listExecuteRoomCode16s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_16");
                GetDataConfig(ref listExecuteRoomCode17s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_17");
                GetDataConfig(ref listExecuteRoomCode18s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_18");
                GetDataConfig(ref listExecuteRoomCode19s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_19");
                GetDataConfig(ref listExecuteRoomCode20s, "MRS.HIS_MEDI_STOCK.487_EXE_SERVICE_ROOM_CODE_20");
                // dữ liệu
                HisTransactionViewFilterQuery transactionViewFilter = new HisTransactionViewFilterQuery();
                transactionViewFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                transactionViewFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO;
                transactionViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                transactionViewFilter.IS_CANCEL = false;
                transactionViewFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT };
                transactionViewFilter.HAS_SALL_TYPE = false;
                listTransactions = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactionViewFilter);

                listTransactions = listTransactions.Where(w => w.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE).ToList();

                var skip = 0;

                while (listTransactions.Count - skip > 0)
                {
                    var listIDs = listTransactions.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //HisBillViewFilterQuery billViewFilter = new HisBillViewFilterQuery(); 
                    //billViewFilter.TRANSACTION_IDs = listIDs.Select(s => s.ID).ToList(); 
                    //listBills.AddRange(new MOS.MANAGER.HisBill.HisBillManager(param).GetView(billViewFilter); 

                    HisSereServBillViewFilterQuery sereServBillViewFilter = new HisSereServBillViewFilterQuery();
                    sereServBillViewFilter.BILL_IDs = listIDs.Select(s => s.ID).ToList();
                    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillViewFilter));

                    HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                    treatmentViewFilter.IDs = listIDs.Where(o => o.TREATMENT_ID.HasValue).Select(s => s.TREATMENT_ID.Value).ToList();
                    listTreatments.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter));
                }

                //skip = 0; 
                //while (listBills.Count - skip > 0)
                //{
                //    var listIDs = listBills.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                //    HisSereServBillViewFilterQuery sereServBillViewFilter = new HisSereServBillViewFilterQuery(); 
                //    sereServBillViewFilter.BILL_IDs = listIDs.Select(s => s.ID).ToList(); 
                //    listSereServBills.AddRange(new MOS.MANAGER.HisSereServBill.HisSereServBillManager(param).GetView(sereServBillViewFilter); 
                //}

                skip = 0;
                var sereServIds = listSereServBills.Select(s => s.SERE_SERV_ID).Distinct().ToList();
                while (sereServIds.Count - skip > 0)
                {
                    var listIDs = sereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                    sereServViewFilter.IDs = listIDs;
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter));
                }


            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        protected void GetDataConfig(ref List<string> listExeServiceRooms, string code)
        {
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                listExeServiceRooms = value.Split(',').ToList();

                Inventec.Common.Logging.LogSystem.Info(code + ": " + String.Join(", ", listExeServiceRooms));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                foreach (var transaction in listTransactions)
                {
                    var rdo = new Mrs00487RDO();
                    rdo.TRANSACTION = transaction;
                    var treatment = listTreatments.Where(w => w.ID == transaction.TREATMENT_ID).ToList();
                    if (IsNotNullOrEmpty(treatment))
                        rdo.TREATMENT = treatment.First();

                    //var bill = listBills.Where(w => w.TRANSACTION_ID == transaction.ID).ToList(); 
                    //if(IsNotNullOrEmpty(bill))
                    {
                        //var sereServBill = listSereServBills.Where(w => w.BILL_ID == bill.First().ID).ToList(); 
                        var sereServBill = listSereServBills.Where(w => w.BILL_ID == transaction.ID).ToList();
                        if (IsNotNullOrEmpty(sereServBill))
                        {
                            var listSereServ = listSereServs.Where(w => sereServBill.Select(s => s.SERE_SERV_ID).Contains(w.ID)).ToList();
                            if (IsNotNullOrEmpty(listSereServ))
                            {
                                rdo.SERVICE_TYPE_01 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode01s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_02 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode02s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_03 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode03s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_04 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode04s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_05 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode05s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_06 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode06s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_07 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode07s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_08 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode08s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_09 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode09s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_10 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode10s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_11 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode11s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_12 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode12s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_13 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode13s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_14 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode14s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_15 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode15s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_16 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode16s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_17 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode17s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_18 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode18s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_19 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode19s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                                rdo.SERVICE_TYPE_20 = listSereServ.Where(w => w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                                && listExecuteRoomCode20s.Contains(w.EXECUTE_ROOM_CODE)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;

                                listRdo.Add(rdo);
                            }
                        }
                    }
                }

                listRdo = listRdo.Where(w =>
                w.SERVICE_TYPE_01 != 0 || w.SERVICE_TYPE_02 != 0 || w.SERVICE_TYPE_03 != 0 || w.SERVICE_TYPE_04 != 0 || w.SERVICE_TYPE_05 != 0
                || w.SERVICE_TYPE_06 != 0 || w.SERVICE_TYPE_07 != 0 || w.SERVICE_TYPE_08 != 0 || w.SERVICE_TYPE_09 != 0 || w.SERVICE_TYPE_10 != 0
                || w.SERVICE_TYPE_11 != 0 || w.SERVICE_TYPE_12 != 0 || w.SERVICE_TYPE_13 != 0 || w.SERVICE_TYPE_14 != 0 || w.SERVICE_TYPE_15 != 0
                || w.SERVICE_TYPE_16 != 0 || w.SERVICE_TYPE_17 != 0 || w.SERVICE_TYPE_18 != 0 || w.SERVICE_TYPE_19 != 0 || w.SERVICE_TYPE_20 != 0).ToList();
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
                dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.TRANSACTION.TRANSACTION_TIME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
