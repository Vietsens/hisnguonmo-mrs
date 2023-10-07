using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisAccountBook;
using MOS.MANAGER.HisBillFund;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisTransaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using System.Threading;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisKskContract;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisWorkPlace;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00249
{
    public class Mrs00249Processor : AbstractProcessor
    {
        Mrs00249Filter filter = new Mrs00249Filter();
        CommonParam paramGet = new CommonParam();
        //List<V_HIS_BILL_FUND> ListBillFund = new List<V_HIS_BILL_FUND>();
        List<Mrs00249RDO> ListTransaction = new List<Mrs00249RDO>();
        List<SSB> ListSereServBill = new List<SSB>();
        List<Dictionary<string, DepartmentRoomBill>> listDicDepaRoomBill = new List<Dictionary<string, DepartmentRoomBill>>();
        List<List<long>> listRepaySameDate = new List<List<long>>();
        string key = "{0}_{1}_{2}";
        //List<HIS_SERE_SERV_DEPOSIT> ListSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>(); 
        //List<HIS_SESE_DEPO_REPAY> ListSeseDepoRepay = new List<HIS_SESE_DEPO_REPAY>(); 

        //List<Mrs00249RDO> ListTransaction = new List<Mrs00249RDO>();
        List<Mrs00249RDO> ListBhytRdo = new List<Mrs00249RDO>();
        List<Mrs00249RDO> ListOtherRdo = new List<Mrs00249RDO>();
        List<Mrs00249RDO> ListTreatmentBhytRdo = new List<Mrs00249RDO>();
        List<Mrs00249RDO> ListTreatmentOtherRdo = new List<Mrs00249RDO>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        HIS_BRANCH Branch = new HIS_BRANCH();
        List<LAST_ALTER_INFO> listLastAlterInfo = new List<LAST_ALTER_INFO>();
        List<CARD_INFO> listCardInfo = new List<CARD_INFO>();
        List<HIS_KSK_CONTRACT> listKskContract = new List<HIS_KSK_CONTRACT>();
        List<HIS_WORK_PLACE> listWorkPlace = new List<HIS_WORK_PLACE>();
        List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
        List<TYPE_PRICE> listTypePrice = new List<TYPE_PRICE>();
        List<TREATMENT_FEE> listTreatmentFee = new List<TREATMENT_FEE>();
        List<TYPE_PRICE> listDepDiscount = new List<TYPE_PRICE>();
        List<HISTORY_TIME> listHistoryTime = new List<HISTORY_TIME>();
        System.Data.DataTable listData = new System.Data.DataTable();
        System.Data.DataTable listParent = new System.Data.DataTable();
        //List<REQUEST_DEPARTMENT_ID> listClinicalDepa = new List<REQUEST_DEPARTMENT_ID>();

        List<HIS_SERVICE> listHisService = new List<HIS_SERVICE>();

        Dictionary<string, string> dicParent = new Dictionary<string, string>();

        List<Mrs00249RDO> listRepay = new List<Mrs00249RDO>();
        Dictionary<long, List<long>> dicByTreatmentId = new Dictionary<long, List<long>>();
        Dictionary<string, List<long>> dicByTreatmentIdBillSameDate = new Dictionary<string, List<long>>();
        Dictionary<long, Dictionary<string, decimal>> dicSvtPrice = new Dictionary<long, Dictionary<string, decimal>>();
        Dictionary<long, Dictionary<string, decimal>> dicHSvtPrice = new Dictionary<long, Dictionary<string, decimal>>();
        Dictionary<long, Dictionary<string, decimal>> dicSvtHeinPrice = new Dictionary<long, Dictionary<string, decimal>>();
        Dictionary<long, Dictionary<string, decimal>> dicHSvtHeinPrice = new Dictionary<long, Dictionary<string, decimal>>();
        List<HIS_PATIENT_CLASSIFY> listClassify = new List<HIS_PATIENT_CLASSIFY>();
        List<HIS_CASHIER_ROOM> listCashierRoom = new List<HIS_CASHIER_ROOM>();

        List<COUNT_FEE_LOCK> ListCountFeeLock = new List<COUNT_FEE_LOCK>();

        List<string> listCashierLoginname = new List<string>();

        public Mrs00249Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00249Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                filter = (Mrs00249Filter)this.reportFilter;

                //if(filter.IS_NOT_SPLIT_DEPA)
                //get dữ liệu:
                listData = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                listParent = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 16));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0)
                {
                    return true;
                }
                //chỉ lấy thông tin hủy không lấy tiền hủy
                if (filter.ADD_CANCEL_INFO == true)
                {
                    filter.TAKE_CANCEL = false;
                }

                ListTransaction = new ManagerSql().GetTransaction(filter);
                Inventec.Common.Logging.LogSystem.Info("Finish get Transaction.");

                Inventec.Common.Logging.LogSystem.Info("Start get takeSSB.");
                ListSereServBill = new ManagerSql().GetSSB(filter) ?? new List<SSB>();
                Inventec.Common.Logging.LogSystem.Info("Finish get takeSSB.");

                GetDepartment();


                Inventec.Common.Logging.LogSystem.Info("Start get PatientTypeAlter.");

                listLastAlterInfo = new ManagerSql().GetLastAlterInfo(filter) ?? new List<LAST_ALTER_INFO>();

                Inventec.Common.Logging.LogSystem.Info("Finish get PatientTypeAlter.");

                //if (filter.IS_ADD_CLINICAL_DEPA == true)
                //{
                //    listClinicalDepa = new ManagerSql().GetClinicalDepa(filter) ?? new List<REQUEST_DEPARTMENT_ID>();
                //}

                Inventec.Common.Logging.LogSystem.Info("Start get CardInfo.");

                listCardInfo = new ManagerSql().GetCardInfo(filter) ?? new List<CARD_INFO>();

                Inventec.Common.Logging.LogSystem.Info("Finish get CardInfo.");

                Inventec.Common.Logging.LogSystem.Info("Start get KskContract.");
                var listKskContractId = listLastAlterInfo.Where(o => o.KSK_CONTRACT_ID.HasValue).Select(o => o.KSK_CONTRACT_ID.Value).Distinct().ToList();
                if (IsNotNullOrEmpty(listKskContractId))
                {
                    var skip = 0;
                    while (listKskContractId.Count - skip > 0)
                    {
                        var listIds = listKskContractId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisKskContractFilterQuery HisKskContractFilterQuery = new HisKskContractFilterQuery()
                        {
                            IDs = listIds
                        };
                        var listKskContractSub = new HisKskContractManager(paramGet).Get(HisKskContractFilterQuery);
                        listKskContract.AddRange(listKskContractSub);
                    }

                    Inventec.Common.Logging.LogSystem.Info("Finish get KskContract.");
                }

                Inventec.Common.Logging.LogSystem.Info("Start get WorkPlace.");
                var listWorkPlaceId = listKskContract.Select(o => o.WORK_PLACE_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listWorkPlaceId))
                {
                    var skip = 0;
                    while (listWorkPlaceId.Count - skip > 0)
                    {
                        var listIds = listWorkPlaceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisWorkPlaceFilterQuery HisWorkPlaceFilterQuery = new HisWorkPlaceFilterQuery()
                        {
                            IDs = listIds
                        };
                        var listWorkPlaceSub = new HisWorkPlaceManager(paramGet).Get(HisWorkPlaceFilterQuery);
                        listWorkPlace.AddRange(listWorkPlaceSub);
                    }

                    Inventec.Common.Logging.LogSystem.Info("Finish get WorkPlace.");
                }

                Inventec.Common.Logging.LogSystem.Info("Start get ExpMest.");
                var listBillId = ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.SALE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SALE_TYPE.ID__SALE_EXP).Select(o => o.ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listBillId))
                {
                    var skip = 0;
                    while (listBillId.Count - skip > 0)
                    {
                        var listIds = listBillId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestFilterQuery HisExpMestFilterQuery = new HisExpMestFilterQuery()
                        {
                            BILL_IDs = listIds
                        };
                        var listExpMestSub = new HisExpMestManager(paramGet).Get(HisExpMestFilterQuery);
                        listExpMest.AddRange(listExpMestSub);
                    }

                    Inventec.Common.Logging.LogSystem.Info("Finish get ExpMest.");
                }

                HisServiceFilterQuery HisServicefilter = new HisServiceFilterQuery();
                listHisService = new HisServiceManager().Get(HisServicefilter);
                GetDicParentService();
                GetCountFeeLock();

                //thêm đối tượng chi tiết
                GetPatientClassify();

                //phòng thu ngân
                GetCashierRoom();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetDepartment()
        {
            listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
        }

        private void GetPatientClassify()
        {
            listClassify = new ManagerSql().GetPatientClassify() ?? new List<HIS_PATIENT_CLASSIFY>();
        }

        private void GetCashierRoom()
        {
            listCashierRoom = new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery()) ?? new List<HIS_CASHIER_ROOM>();
        }

        private void GetCountFeeLock()
        {

            Inventec.Common.Logging.LogSystem.Info("Start get CountFeeLock.");
            ListCountFeeLock = new ManagerSql().GetCountFeeLock(filter) ?? new List<COUNT_FEE_LOCK>();
            Inventec.Common.Logging.LogSystem.Info("Finish get CountFeeLock.");
        }

        private void GetDicParentService()
        {

            foreach (var item in HisServiceTypeCFG.HisServiceTypes.OrderBy(o => o.NUM_ORDER ?? 1000).ToList())
            {
                dicParent.Add(string.Format("CODE_{0}__{1}", item.SERVICE_TYPE_CODE, 0), item.SERVICE_TYPE_CODE);
                dicParent.Add(string.Format("NAME_{0}__{1}", item.SERVICE_TYPE_CODE, 0), item.SERVICE_TYPE_NAME);
                var serviceSub = listHisService.Where(o => o.SERVICE_TYPE_ID == item.ID && o.PARENT_ID == null && listHisService.Exists(p => p.PARENT_ID == o.ID)).OrderBy(o => o.NUM_ORDER ?? 1000).ToList();
                if (serviceSub == null || serviceSub.Count == 0)
                {
                    continue;
                }
                for (int i = 0; i < serviceSub.Count; i++)
                {
                    var service = serviceSub[i];
                    dicParent.Add(string.Format("CODE_{0}__{1}", item.SERVICE_TYPE_CODE, i + 1), string.Format("{0}_{1}", item.SERVICE_TYPE_CODE, service.SERVICE_CODE));
                    dicParent.Add(string.Format("NAME_{0}__{1}", item.SERVICE_TYPE_CODE, i + 1), service.SERVICE_NAME);
                }

            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0)
                {
                    return true;
                }

                //xu ly giao dich
                if (IsNotNullOrEmpty(ListTransaction))
                {


                    if (IsNotNullOrEmpty(ListTransaction))
                    {
                        Inventec.Common.Logging.LogSystem.Info("Start Processor TransactionSum.");
                        listTreatmentFee = new ManagerSql().GetTreatmentFee(this.filter) ?? new List<TREATMENT_FEE>();
                        listTypePrice = new ManagerSql().GetTypePrice(this.filter) ?? new List<TYPE_PRICE>();
                        listDepDiscount = new ManagerSql().GetDepDiscount(this.filter) ?? new List<TYPE_PRICE>();
                        listHistoryTime = new ManagerSql().GetHistoryTime(this.filter) ?? new List<HISTORY_TIME>();
                        List<Thread> listThread = new List<Thread>();
                        listRepay = ListTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                        //them bot thoi gian hoan ung
                        listRepay = AddTime(listRepay);

                        dicByTreatmentId = ListTransaction.Where(o => o.TREATMENT_ID > 0 &&/*Loai bo check hoan ung huy*/(o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU ? o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE : true)).GroupBy(g => g.TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.Select(o => o.ID).OrderBy(r => r).ToList());
                        dicByTreatmentIdBillSameDate = ListTransaction.Where(o => o.TREATMENT_ID > 0 && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).GroupBy(g => string.Format("{0}_{1}", g.TREATMENT_ID ?? 0, g.TRANSACTION_DATE)).ToDictionary(p => p.Key, q => q.Select(o => o.ID).OrderBy(r => r).ToList());
                        dicSvtPrice = ListSereServBill.GroupBy(g => g.BILL_ID).ToDictionary(p => p.Key, q => q.GroupBy(g1 => g1.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p1 => p1.Key, q1 => q1.Sum(s => s.PRICE)));
                        dicHSvtPrice = ListSereServBill.GroupBy(g => g.BILL_ID).ToDictionary(p => p.Key, q => q.GroupBy(g1 => g1.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p1 => p1.Key, q1 => q1.Sum(s => s.PRICE)));
                        dicSvtHeinPrice = ListSereServBill.GroupBy(g => g.BILL_ID).ToDictionary(p => p.Key, q => q.GroupBy(g1 => g1.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p1 => p1.Key, q1 => q1.Sum(s => s.TDL_TOTAL_HEIN_PRICE ?? 0)));
                        dicHSvtHeinPrice = ListSereServBill.GroupBy(g => g.BILL_ID).ToDictionary(p => p.Key, q => q.GroupBy(g1 => g1.HEIN_SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p1 => p1.Key, q1 => q1.Sum(s => s.TDL_TOTAL_HEIN_PRICE ?? 0)));
                        Inventec.Common.Logging.LogSystem.Info("To Dictionary _OK"); ;
                        int count = (int)ListTransaction.Count / 4;
                        for (int i = 0; i <= 4; i++)
                        {
                            ParamThread threadParam = new ParamThread();
                            threadParam.transactionSub = ListTransaction.Skip(count * i).Take(i == 4 ? i : count).ToList();
                            if (filter.ADD_SSB_INFO == true)
                            {
                                threadParam.ssbSub = ListSereServBill.Where(o => threadParam.transactionSub.Exists(p => p.ID == o.BILL_ID)).ToList();
                            }
                            else { threadParam.ssbSub = new List<SSB>(); }

                            threadParam.dicDepaRoomBill = new Dictionary<string, DepartmentRoomBill>();
                            listDicDepaRoomBill.Add(threadParam.dicDepaRoomBill);
                            threadParam.listRepaySameDate = new List<long>();
                            listRepaySameDate.Add(threadParam.listRepaySameDate);
                            Thread thread = new Thread(processorRdo);
                            thread.Start(threadParam);
                            listThread.Add(thread);
                        }

                        foreach (var item in listThread)
                        {
                            item.Join();
                        }

                        Inventec.Common.Logging.LogSystem.Info("Finish Processor TransactionSum.");
                    }


                    if (filter.DEPARTMENT_IDs != null)
                    {
                        ListTransaction = ListTransaction.Where(p => filter.DEPARTMENT_IDs.Contains(p.LAST_DEPARTMENT_ID ?? 0)).ToList();

                    }
                    ListTransaction = ListTransaction.OrderBy(p => p.TRANSACTION_CODE).ToList();
                }

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListTransaction.Clear();
            }
            return result;
        }

        private List<Mrs00249RDO> AddTime(List<Mrs00249RDO> listRepay)
        {
            foreach (var item in listRepay)
            {
                DateTime time = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(item.CREATE_TIME ?? 0) ?? new DateTime();
                item.ADD_MIN_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(time.AddMinutes(this.filter.LIMIT_TIME ?? 2)) ?? 0;
                item.SUB_MIN_TIME = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(time.AddMinutes(-this.filter.LIMIT_TIME ?? -2)) ?? 0;
            }
            return listRepay;
        }

        private void ParentCodeName(SSB item)
        {
            try
            {
                item.PARENT_NUM_ORDER = 1000 * ((HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.SERVICE_TYPE_CODE == item.SERVICE_TYPE_CODE) ?? new HIS_SERVICE_TYPE()).NUM_ORDER ?? 999);
                item.PARENT_SERVICE_CODE = item.SERVICE_TYPE_CODE;
                item.PARENT_SERVICE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.SERVICE_TYPE_CODE == item.SERVICE_TYPE_CODE) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                if (item.PARENT_ID != null)
                {
                    var parent = listHisService.FirstOrDefault(o => o.ID == item.PARENT_ID);
                    if (parent != null)
                    {
                        item.PARENT_NUM_ORDER += parent.NUM_ORDER ?? 999;
                        item.PARENT_SERVICE_CODE += "_" + parent.SERVICE_CODE;
                        item.PARENT_SERVICE_NAME = parent.SERVICE_NAME;
                    }

                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void DepartmentRoomCodeName(long requestDepartmentId, long requestRoomId, List<V_HIS_ROOM> listHisRoom, DepartmentRoomBill rdo)
        {
            try
            {
                rdo.TDL_REQUEST_ROOM_ID = requestRoomId;
                rdo.TDL_REQUEST_DEPARTMENT_ID = requestDepartmentId;
                var room = listHisRoom.FirstOrDefault(o => o.ID == requestRoomId) ?? new V_HIS_ROOM();
                rdo.REQUEST_ROOM_CODE = room.ROOM_CODE;
                rdo.REQUEST_ROOM_NAME = room.ROOM_NAME;
                if (requestDepartmentId == 0) LogSystem.Error("khong co khoa chi dinh");
                var department = listDepartment.FirstOrDefault(o => o.ID == requestDepartmentId) ?? new HIS_DEPARTMENT();
                if (string.IsNullOrWhiteSpace(department.DEPARTMENT_CODE)) LogSystem.Error("khong lay duoc khoa chi dinh departmentID:" + requestDepartmentId);
                rdo.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                rdo.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void processorRdo(object threadParam)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("CountDepartment:" + listDepartment.Count);
                if (threadParam == null) return;
                ParamThread param = threadParam as ParamThread;
                var transactionIds = param.transactionSub.Select(o => o.ID).ToList();
                var treatmentIds = param.transactionSub.Select(o => o.TREATMENT_ID ?? 0).ToList();
                var listLastAlterInfoSub = listLastAlterInfo.Where(o => transactionIds.Contains(o.TRANSACTION_ID)).ToList();
                //var listClinicalDepaSub = listClinicalDepa.Where(o => transactionIds.Contains(o.TRAN_ID)).ToList();
                var listCardInfoSub = listCardInfo.Where(o => transactionIds.Contains(o.TRANSACTION_ID)).ToList();
                var listTypePriceSub = listTypePrice.Where(o => transactionIds.Contains(o.BILL_ID)).ToList();
                var listDepDiscountSub = listDepDiscount.Where(o => transactionIds.Contains(o.BILL_ID)).ToList();
                var listlistExpMestSub = listExpMest.Where(o => transactionIds.Contains(o.BILL_ID ?? 0)).ToList();
                var listTreatmentFeeSub = listTreatmentFee.Where(o => treatmentIds.Contains(o.ID)).ToList();
                var listHistoryTimeSub = listHistoryTime.Where(o => transactionIds.Contains(o.TRANSACTION_ID)).ToList();
                foreach (var item in param.transactionSub)
                {

                    processRdo(item, listLastAlterInfoSub, listCardInfoSub, listTypePriceSub, listDepDiscountSub, listKskContract, listlistExpMestSub, listWorkPlace, listTreatmentFeeSub, param.listRepaySameDate, listHistoryTimeSub);

                    if (filter.DEPARTMENT_IDs != null && !filter.DEPARTMENT_IDs.Contains(item.LAST_DEPARTMENT_ID ?? 0))
                    {
                        continue;
                    }
                    var ssbSub = param.ssbSub.Where(o => o.BILL_ID == item.ID).ToList();
                    AddInforSsb(ssbSub);
                    SplitDepartmentRoom(item, ssbSub, param.dicDepaRoomBill);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void processRdo(Mrs00249RDO item, List<LAST_ALTER_INFO> listLastAlterInfo, List<CARD_INFO> listCardInfo, List<TYPE_PRICE> listTypePrice, List<TYPE_PRICE> listDepDiscount, List<HIS_KSK_CONTRACT> listKskContract, List<HIS_EXP_MEST> listExpMest, List<HIS_WORK_PLACE> listWorkPlace, List<TREATMENT_FEE> listTreatmentFee, List<long> listRepaySameDate, List<HISTORY_TIME> listHistoryTime)
        {
            try
            {
                item.VIR_PATIENT_NAME = item.TDL_PATIENT_NAME;
                item.VIR_PATIENT_CODE = item.TDL_PATIENT_CODE;
                SetExtendField(item, listLastAlterInfo, listCardInfo, listTypePrice, listDepDiscount, listKskContract, listExpMest, listWorkPlace, listTreatmentFee, listRepaySameDate, listHistoryTime);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SplitDepartmentRoom(Mrs00249RDO item, List<SSB> ssbSub, Dictionary<string, DepartmentRoomBill> dicSsbSub)
        {
            //xu ly chi tiet dich vu
            if (ssbSub.Count > 0)
            {
                var grSsb = ssbSub.GroupBy(o => string.Format(key, o.BILL_ID, o.TDL_REQUEST_DEPARTMENT_ID, o.TDL_REQUEST_ROOM_ID)).ToList();
                foreach (var igr in grSsb)
                {
                    string groupKey = igr.Key;

                    if (!dicSsbSub.ContainsKey(groupKey))
                    {
                        DepartmentRoomBill rdo = new DepartmentRoomBill();
                        rdo.DIC_PRICE = new Dictionary<string, decimal>();
                        rdo.DIC_PRICE_DIFF = new Dictionary<string, decimal>();
                        rdo.DIC_PRICE_DIFF_NR = new Dictionary<string, decimal>();
                        rdo.DIC_TOTAL_HEIN_PRICE = new Dictionary<string, decimal>();
                        rdo.DIC_SERVICE_TYPE_PRICE = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY_BHYT = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY_DCT = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY_BN = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY_TT = new Dictionary<string, decimal>();
                        rdo.DIC_CATEGORY_DIFF = new Dictionary<string, decimal>();
                        rdo.TOTAL_PRICE_GD12H = igr.Where(o => o.SERVICE_NAME.ToLower().Contains("gói giảm đau theo yêu cầu 12h đầu sau phẫu thuật")).Sum(s => s.PRICE);
                        rdo.mainRdo = item;
                        rdo.TRANSACTION_ID = item.TRANSACTION_ID;
                        DepartmentRoomCodeName(igr.First().TDL_REQUEST_DEPARTMENT_ID, igr.First().TDL_REQUEST_ROOM_ID, HisRoomCFG.HisRooms, rdo);
                        dicSsbSub[groupKey] = rdo;
                    }

                    dicSsbSub[groupKey].DIC_TOTAL_HEIN_PRICE = igr.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TDL_TOTAL_HEIN_PRICE ?? 0));

                    dicSsbSub[groupKey].DIC_PRICE = igr.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.PRICE));

                    dicSsbSub[groupKey].NOTE_PRICE = string.Join(";", igr.GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").Select(o => string.Format("{0}: {1}", o.First().PARENT_SERVICE_NAME, o.Sum(s => s.PRICE))).ToList());

                    dicSsbSub[groupKey].DIC_PRICE_DIFF = igr.Where(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) > (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)).GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                    dicSsbSub[groupKey].DIC_PRICE_DIFF_NR = igr.Where(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) > (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)).Where(o => o.HEIN_SERVICE_TYPE_CODE != "TH_TL").GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));

                    dicSsbSub[groupKey].NOTE_PRICE_DIFF = string.Join(";", igr.Where(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) > (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)).Where(p => p.HEIN_SERVICE_TYPE_CODE != "TH_TL").GroupBy(o => o.PARENT_SERVICE_CODE ?? "NONE").Select(o => string.Format("{0}: {1}", o.First().PARENT_SERVICE_NAME, o.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)))).ToList());

                    dicSsbSub[groupKey].DIC_SERVICE_TYPE_PRICE = igr.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.PRICE));

                    //tong chi phi
                    dicSsbSub[groupKey].DIC_CATEGORY = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TDL_TOTAL_PRICE ?? 0));


                    dicSsbSub[groupKey].TOTAL_PRICE = igr.Sum(s => s.TDL_TOTAL_PRICE ?? 0);
                    //benh nhan dong chi tra
                    dicSsbSub[groupKey].DIC_CATEGORY_DCT = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    dicSsbSub[groupKey].TOTAL_PRICE_DCT = igr.Sum(s => s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    //benh nhan tra
                    dicSsbSub[groupKey].DIC_CATEGORY_BN = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TDL_TOTAL_PATIENT_PRICE ?? 0));
                    dicSsbSub[groupKey].TOTAL_PRICE_BN = igr.Sum(s => s.TDL_TOTAL_PATIENT_PRICE ?? 0);

                    //ngoai dong chi tra
                    dicSsbSub[groupKey].DIC_CATEGORY_DIFF = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                    dicSsbSub[groupKey].TOTAL_PRICE_DIFF = igr.Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    dicSsbSub[groupKey].TOTAL_PRICE_DIFF_R = igr.Where(o => o.HEIN_SERVICE_TYPE_CODE == "TH_TL").Sum(s => (s.TDL_TOTAL_PATIENT_PRICE ?? 0) - (s.TDL_TOTAL_PATIENT_PRICE_BHYT ?? 0));

                    //bao hiem tra
                    dicSsbSub[groupKey].DIC_CATEGORY_BHYT = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.TDL_TOTAL_HEIN_PRICE ?? 0));
                    dicSsbSub[groupKey].TOTAL_PRICE_BHYT = igr.Sum(s => (s.TDL_TOTAL_HEIN_PRICE ?? 0));

                    //thuc thu
                    dicSsbSub[groupKey].DIC_CATEGORY_TT = igr.GroupBy(o => o.CATEGORY_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.PRICE - s.PRICE * (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && item.AMOUNT > 0 ? ((item.EXEMPTION ?? 0) / item.AMOUNT) : 0)));
                    dicSsbSub[groupKey].TOTAL_PRICE_TT = igr.Sum(s => s.PRICE - s.PRICE * (item.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && item.AMOUNT > 0 ? ((item.EXEMPTION ?? 0) / item.AMOUNT) : 0));
                }
            }
            else
            {
                string groupKey = string.Format(key, item.ID, 0, 0);
                if (!dicSsbSub.ContainsKey(groupKey))
                {
                    DepartmentRoomBill rdo = new DepartmentRoomBill();
                    rdo.DIC_PRICE = new Dictionary<string, decimal>();
                    rdo.DIC_PRICE_DIFF = new Dictionary<string, decimal>();
                    rdo.DIC_PRICE_DIFF_NR = new Dictionary<string, decimal>();
                    rdo.DIC_TOTAL_HEIN_PRICE = new Dictionary<string, decimal>();
                    rdo.DIC_SERVICE_TYPE_PRICE = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY_BHYT = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY_DCT = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY_BN = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY_TT = new Dictionary<string, decimal>();
                    rdo.DIC_CATEGORY_DIFF = new Dictionary<string, decimal>();
                    rdo.TRANSACTION_ID = item.TRANSACTION_ID;
                    rdo.mainRdo = item;
                    dicSsbSub[groupKey] = rdo;
                }

            }
        }

        private void AddInforSsb(List<SSB> ssbSub)
        {
            foreach (var item in ssbSub)
            {

                ParentCodeName(item);
                if (filter.IS_EXAM_REQ_TO_EXE_ROOM == true)
                {
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                    {
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                    }
                }
            }
        }

        private void SetExtendField(Mrs00249RDO rdo, List<LAST_ALTER_INFO> listLastAlterInfo, List<CARD_INFO> listCardInfo, List<TYPE_PRICE> listTypePrice, List<TYPE_PRICE> listDepDiscount, List<HIS_KSK_CONTRACT> listKskContract, List<HIS_EXP_MEST> listExpMest, List<HIS_WORK_PLACE> listWorkPlace, List<TREATMENT_FEE> listTreatmentFe, List<long> listRepaySameDate, List<HISTORY_TIME> listHistoryTime)
        {
            try
            {
                var historyTime = listHistoryTime.Where(p => p.TRANSACTION_ID == rdo.ID).ToList();
                rdo.TRANSACTION_COUNT = historyTime.Count + 1;
                rdo.CANCEL = rdo.IS_CANCEL ?? 0;
                rdo.TRANSACTION_ID = rdo.ID;
                rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.IN_TIME ?? 0);
                rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.OUT_TIME ?? 0);
                rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(rdo.TRANSACTION_TIME);
                if (rdo.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && filter.ADD_CANCEL_INFO == true)
                {
                    rdo.AMOUNT = 0;
                    rdo.KC_AMOUNT = 0;
                    rdo.KC_AMOUNTs = 0;
                    rdo.EXEMPTION = 0;
                    rdo.TDL_BILL_FUND_AMOUNT = 0;
                }
                if (listTreatmentFe != null && listTreatmentFe.Count > 0)
                {
                    var treatmentFee = listTreatmentFe.FirstOrDefault(o => rdo.TREATMENT_ID == o.ID);
                    if (treatmentFee != null)
                    {
                        rdo.CLINICAL_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatmentFee.HOSPITALIZE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.CLINICAL_DEPARTMENT_ID = treatmentFee.HOSPITALIZE_DEPARTMENT_ID ?? 0;

                        rdo.CLINICAL_DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatmentFee.HOSPITALIZE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    }
                }
                if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                {
                    rdo.REPAYs = rdo.AMOUNT;
                    var treatmentFee = listTreatmentFee.FirstOrDefault(o => o.ID == rdo.TREATMENT_ID);
                    if (treatmentFee != null)
                    {
                        rdo.TOTAL_HEIN_PRICE = treatmentFee.TOTAL_HEIN_PRICE ?? 0;
                    }
                }

                else if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                {
                    rdo.DEP_BIL += rdo.AMOUNT;

                    if (listDepDiscount != null && listDepDiscount.Count > 0)
                    {
                        var depDiscount = listDepDiscount.FirstOrDefault(o => o.BILL_ID == rdo.ID);
                        if (depDiscount != null)
                        {
                            rdo.SSD_DISCOUNT = depDiscount.SS_DISCOUNT ?? 0;
                        }
                    }

                }
                else if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT)
                {
                    rdo.DEP_BIL = rdo.AMOUNT /*- (rdo.EXEMPTION ?? 0) - totalFund*/;
                    rdo.KC_AMOUNTs = rdo.KC_AMOUNT ?? 0;
                    rdo.EXTRA_AMOUNT = rdo.AMOUNT - rdo.KC_AMOUNTs;

                    var treatmentFee = listTreatmentFee.FirstOrDefault(o => o.ID == rdo.TREATMENT_ID);
                    if (treatmentFee != null)
                    {
                        rdo.SS_DISCOUNT = treatmentFee.TOTAL_DISCOUNT ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE = treatmentFee.TOTAL_OTHER_SOURCE_PRICE ?? 0;
                        rdo.TOTAL_TREATMENT_DEPO_AMOUNT = treatmentFee.TOTAL_DEPOSIT_AMOUNT ?? 0;
                        rdo.TOTAL_TREATMENT_REP_AMOUNT = treatmentFee.TOTAL_REPAY_AMOUNT ?? 0;
                        rdo.TOTAL_HEIN_PRICE = treatmentFee.TOTAL_HEIN_PRICE ?? 0;
                    }
                    if (listTypePrice != null && listTypePrice.Count > 0)
                    {
                        var typePrice = listTypePrice.FirstOrDefault(o => o.BILL_ID == rdo.ID);
                        if (typePrice != null)
                        {
                            rdo.TOTAL_DIFFERENCE_PRICE = typePrice.TOTAL_DIFFERENCE_PRICE ?? 0;
                            rdo.TOTAL_PATIENT_PRICE_BHYT = typePrice.TOTAL_PATIENT_PRICE_BHYT ?? 0;
                            rdo.TOTAL_PATIENT_PRICE_VP = typePrice.TOTAL_PATIENT_PRICE_VP ?? 0;

                        }


                    }


                    var tranIds = dicByTreatmentId.ContainsKey(rdo.TREATMENT_ID ?? 0) ? dicByTreatmentId[rdo.TREATMENT_ID ?? 0] : new List<long>();
                    var billIds = dicByTreatmentIdBillSameDate.ContainsKey(string.Format("{0}_{1}", rdo.TREATMENT_ID ?? 0, rdo.TRANSACTION_DATE)) ? dicByTreatmentIdBillSameDate[string.Format("{0}_{1}", rdo.TREATMENT_ID ?? 0, rdo.TRANSACTION_DATE)] : new List<long>();
                    var repay = listRepay.Where(p => rdo.AMOUNT == rdo.KC_AMOUNT && (p.AMOUNT > 0 && rdo.AMOUNT > 0 || p.AMOUNT < 0 && rdo.AMOUNT < 0) && p.TREATMENT_ID == rdo.TREATMENT_ID && p.CREATE_TIME <= rdo.CREATE_TIME - rdo.CREATE_TIME % 1000000 + 235959 && p.CREATE_TIME >= rdo.CREATE_TIME - rdo.CREATE_TIME % 1000000 && (rdo.CREATE_TIME < p.ADD_MIN_TIME && rdo.CREATE_TIME > p.SUB_MIN_TIME)).ToList();

                    if (repay != null && repay.Count > 0)
                    {

                        listRepaySameDate.AddRange(repay.Select(o => o.ID).ToList());
                        rdo.CHI_RA = repay.Sum(s => s.AMOUNT);
                        rdo.HIEN_DU = (rdo.KC_AMOUNT ?? 0) + repay.Sum(s => s.AMOUNT);
                    }
                    else
                    {
                        rdo.HIEN_DU = (rdo.KC_AMOUNT ?? 0);
                    }
                    rdo.THU_THEM = (rdo.AMOUNT) - (rdo.KC_AMOUNT ?? 0);
                    rdo.CAN_TRU = rdo.THU_THEM - rdo.CHI_RA;

                    //tổng tiền theo từng loại dịch vụ
                    if (dicHSvtPrice.ContainsKey(rdo.ID))
                    {
                        rdo.DIC_HSVT_PRICE = dicHSvtPrice[rdo.ID];
                    }
                    else
                    {
                        rdo.DIC_HSVT_PRICE = new Dictionary<string, decimal>();
                    }
                    if (dicSvtPrice.ContainsKey(rdo.ID))
                    {
                        rdo.DIC_SVT_PRICE = dicSvtPrice[rdo.ID];
                        if (dicSvtPrice[rdo.ID].ContainsKey("AN"))
                        rdo.TOTAL_PRICE_AN = dicSvtPrice[rdo.ID]["AN"];
                    }
                    else
                    {
                        rdo.DIC_SVT_PRICE = new Dictionary<string, decimal>();
                    }

                    //tổng tiền bh theo từng loại dịch vụ
                    if (dicHSvtHeinPrice.ContainsKey(rdo.ID))
                    {
                        rdo.DIC_HSVT_HEIN_PRICE = dicHSvtHeinPrice[rdo.ID];

                        rdo.TOTAL_HEIN_PRICE_IN_BILL = dicHSvtHeinPrice[rdo.ID].Values.Sum(s => s);
                    }
                    else
                    {
                        rdo.DIC_HSVT_HEIN_PRICE = new Dictionary<string, decimal>();
                    }
                    if (dicSvtHeinPrice.ContainsKey(rdo.ID))
                    {
                        rdo.DIC_SVT_HEIN_PRICE = dicSvtHeinPrice[rdo.ID];
                    }
                    else
                    {
                        rdo.DIC_SVT_HEIN_PRICE = new Dictionary<string, decimal>();
                    }
                }
                else if (rdo.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__NO)
                {

                    rdo.AMOUNT_NO = rdo.AMOUNT;
                }
                if (listCardInfo != null && listCardInfo.Count > 0)
                {
                    var lastInfo = listCardInfo.FirstOrDefault(o => o.TRANSACTION_ID == rdo.ID);
                    if (lastInfo != null)
                    {
                        rdo.CARD_NUMBER = lastInfo.CARD_NUMBER;
                        rdo.CARD_CODE = lastInfo.CARD_CODE;
                        rdo.CARD_MAC = lastInfo.CARD_MAC;
                        rdo.BANK_CARD_CODE = lastInfo.BANK_CARD_CODE;
                    }
                }
                if (listLastAlterInfo != null && listLastAlterInfo.Count > 0)
                {
                    var lastInfo = listLastAlterInfo.FirstOrDefault(o => o.TRANSACTION_ID == rdo.ID);
                    if (lastInfo != null)
                    {
                        rdo.LAST_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == lastInfo.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.LAST_DEPARTMENT_ID = lastInfo.DEPARTMENT_ID ?? 0;
                        rdo.TREATMENT_TYPE_ID = lastInfo.TREATMENT_TYPE_ID;
                        rdo.PATIENT_TYPE_ID = lastInfo.PATIENT_TYPE_ID ?? 0;
                        rdo.HEIN_CARD_NUMBER = lastInfo.HEIN_CARD_NUMBER;
                        if (lastInfo.KSK_CONTRACT_ID.HasValue && listKskContract != null)
                        {
                            var KskContract = listKskContract.FirstOrDefault(o => o.ID == lastInfo.KSK_CONTRACT_ID.Value);
                            if (KskContract != null)
                            {
                                rdo.KSK_CONTRACT_CODE = KskContract.KSK_CONTRACT_CODE;
                                if (listWorkPlace != null)
                                {
                                    var workPlace = listWorkPlace.FirstOrDefault(o => o.ID == KskContract.WORK_PLACE_ID);
                                    if (workPlace != null)
                                    {
                                        rdo.WORK_PLACE_NAME = workPlace.WORK_PLACE_NAME;
                                    }
                                }
                            }
                        }
                        rdo.REQUEST_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == lastInfo.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.REQUEST_DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == lastInfo.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    }
                    if (filter.DEPARTMENT_IDs != null && !filter.DEPARTMENT_IDs.Contains(rdo.LAST_DEPARTMENT_ID ?? 0))
                    {
                        rdo.LAST_DEPARTMENT_ID = 0;
                    }
                }



                if (listExpMest != null && listExpMest.Count > 0)
                {
                    var expMest = listExpMest.Where(o => o.BILL_ID == rdo.ID).ToList();
                    if (expMest != null && expMest.Count > 0)
                    {
                        var Exp = expMest.OrderByDescending(o => o.FINISH_TIME ?? 0).FirstOrDefault();
                        rdo.LAST_MEDI_STOCK_NAME = (MANAGER.Config.HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == Exp.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                    }
                }

                rdo.TRANSACTION_TYPE_NAME = (rdo.TDL_SERE_SERV_DEPOSIT_COUNT > 0) ? "Tạm ứng dịch vụ" :
                     ((rdo.TDL_SESE_DEPO_REPAY_COUNT > 0) ? "Hoàn ứng dịch vụ" : rdo.TRANSACTION_TYPE_NAME);
                rdo.IS_DEPOSIT_DETAIL = (rdo.TDL_SERE_SERV_DEPOSIT_COUNT > 0);
                rdo.NUM_ORDER_STR = string.Format("{0:0000000}", Convert.ToInt64(rdo.NUM_ORDER));

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.CREATE_TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.CREATE_TIME_TO));
            dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.CREATE_TIME_FROM));
            dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.CREATE_TIME_TO));
            dicSingleTag.Add("TIME_NOW", DateTime.Now.ToLongTimeString());

            if (IsNotNull(filter.ACCOUNT_BOOK_ID))
            {
                var AccountBook = new HisAccountBookManager().GetById((long)filter.ACCOUNT_BOOK_ID);
                dicSingleTag.Add("ACCOUNT_BOOK_CODE_NAME", AccountBook.ACCOUNT_BOOK_CODE + " - " + AccountBook.ACCOUNT_BOOK_NAME);
                dicSingleTag.Add("ACCOUNT_BOOK_CREATOR", AccountBook.CREATOR);
            }
            else
            {
                dicSingleTag.Add("ACCOUNT_BOOK_CODE_NAME", string.Join("; ", ListTransaction.Select(o => o.ACCOUNT_BOOK_CODE + " - " + o.ACCOUNT_BOOK_NAME).Distinct().ToList()));
                dicSingleTag.Add("ACCOUNT_BOOK_CREATOR", string.Join(" - ", ListTransaction.Select(o => o.CASHIER_USERNAME).Distinct().ToList()));
            }

            if (filter.BRANCH_ID != 0) dicSingleTag.Add("BRANCH_NAME", Branch.BRANCH_NAME);



            dicSingleTag.Add("CASHIER_ROOM_NAMEs", string.Join(" - ", listCashierRoom.Where(o => filter.EXACT_CASHIER_ROOM_IDs != null && filter.EXACT_CASHIER_ROOM_IDs.Contains(o.ID)).Select(o => o.CASHIER_ROOM_NAME).ToList()));

            dicSingleTag.Add("CASHIER_ROOM_NAME", (listCashierRoom.FirstOrDefault(o => filter.EXACT_CASHIER_ROOM_ID != null && filter.EXACT_CASHIER_ROOM_ID == o.ID
                ) ?? new HIS_CASHIER_ROOM()).CASHIER_ROOM_NAME);
            dicSingleTag.Add("PATIENT_CLASSIFY_NAME", string.Join(" - ", listClassify.Where(o => filter.PATIENT_CLASSIFY_IDs != null && filter.PATIENT_CLASSIFY_IDs.Contains(o.ID)).Select(o => o.PATIENT_CLASSIFY_NAME).ToList()));


            if (filter.IS_BILL_NORMAL.HasValue)
            {
                if (!filter.IS_BILL_NORMAL.Value)
                {
                    dicSingleTag.Add("BILL_TYPE_NAME", "Hóa đơn Dịch vụ");
                }
                else if (filter.IS_BILL_NORMAL.Value)
                {
                    dicSingleTag.Add("BILL_TYPE_NAME", "Hóa đơn Thường");
                }
            }

            if (this.filter.LOAI_CAN_TRU != null)
            {
                ListTransaction = ListTransaction.Where(o => this.filter.LOAI_CAN_TRU == true && o.CAN_TRU > 0 || this.filter.LOAI_CAN_TRU == false && o.CAN_TRU < 0).ToList();

                dicSingleTag.Add("LOAI_CAN_TRU_STR", this.filter.LOAI_CAN_TRU == true?"THU THÊM":"CHI RA");
            }

            if (this.filter.TREATMENT_TYPE_IDs != null)
            {
                ListTransaction = ListTransaction.Where(o => this.filter.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID ?? 0)).ToList();

                dicSingleTag.Add("TREATMENT_TYPE_NAME", string.Join(" - ", HisTreatmentTypeCFG.HisTreatmentTypes.Where(o => this.filter.TREATMENT_TYPE_IDs.Contains(o.ID)).Select(p => p.TREATMENT_TYPE_NAME).ToList()));
            }

            if (listData != null && listData.Rows != null && listData.Rows.Count > 0 && listParent != null && listParent.Rows != null && listParent.Rows.Count > 0)
            {
                objectTag.AddObjectData(store, "Child", listData);
                objectTag.AddObjectData(store, "Parent", listParent);
                objectTag.AddRelationship(store, "Parent", "Child", "RELATION", "RELATION");
                return;
            }

            ListTreatmentBhytRdo = ListTransaction.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(p => p.TREATMENT_ID).Select(q => new Mrs00249RDO() { TREATMENT_ID = q.Key, DEP_BIL = q.Sum(s => s.DEP_BIL), REPAYs = q.Sum(s => s.REPAYs), REDIASUAL_AMOUNT = q.Sum(s => s.DEP_BIL - s.KC_AMOUNTs - s.REPAYs) }).ToList();
            ListTreatmentOtherRdo = ListTransaction.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(p => p.TREATMENT_ID).Select(q => new Mrs00249RDO() { TREATMENT_ID = q.Key, DEP_BIL = q.Sum(s => s.DEP_BIL), REPAYs = q.Sum(s => s.REPAYs), REDIASUAL_AMOUNT = q.Sum(s => s.DEP_BIL - s.KC_AMOUNTs - s.REPAYs) }).ToList();
            ListBhytRdo = ListTransaction.Where(o => o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();
            ListOtherRdo = ListTransaction.Where(o => o.PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).ToList();

            objectTag.AddObjectData(store, "Report", ListBhytRdo);
            objectTag.AddObjectData(store, "ReportSum", ListTransaction.GroupBy(o => new { o.TRANSACTION_TYPE_CODE, o.IS_DEPOSIT_DETAIL }).Select(p => new Mrs00249RDO() { TRANSACTION_TYPE_CODE = p.First().TRANSACTION_TYPE_CODE, IS_DEPOSIT_DETAIL = p.First().IS_DEPOSIT_DETAIL, AMOUNT = p.Sum(s => s.AMOUNT) }).ToList());
            objectTag.AddObjectData(store, "ReportOther", ListOtherRdo);
            objectTag.AddObjectData(store, "Treatment", ListTreatmentBhytRdo);
            objectTag.AddObjectData(store, "TreatmentOther", ListTreatmentOtherRdo);
            objectTag.AddRelationship(store, "Treatment", "Report", "TREATMENT_ID", "TREATMENT_ID");
            objectTag.AddRelationship(store, "TreatmentOther", "ReportOther", "TREATMENT_ID", "TREATMENT_ID");
            objectTag.AddObjectData(store, "Child", ListTransaction.Where(q => q.TEMPLATE_CODE != null).ToList());
            objectTag.AddObjectData(store, "Parent", ListTransaction.Where(q => q.TEMPLATE_CODE != null).GroupBy(o => new { o.TREATMENT_ID, o.CASHIER_LOGINNAME }).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "GrandParent", ListTransaction.Where(q => q.TEMPLATE_CODE != null).GroupBy(o => o.CASHIER_LOGINNAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
            objectTag.AddRelationship(store, "Parent", "Child", new string[] { "CASHIER_LOGINNAME", "TREATMENT_ID" }, new string[] { "CASHIER_LOGINNAME", "TREATMENT_ID" });

            if (filter.IS_GROUP_BILL == true)
            {
                ListTransaction = ListTransaction.GroupBy(p => p.TRANSACTION_CODE).Select(q => q.First()).ToList();
            }
            objectTag.AddObjectData(store, "Transaction", ListTransaction);
            objectTag.AddObjectData(store, "Depo", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.CANCEL != 1).ToList());
            objectTag.AddObjectData(store, "DepoCancel", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.CANCEL == 1).ToList());
            objectTag.AddObjectData(store, "Repay", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && p.CANCEL != 1).ToList());
            objectTag.AddObjectData(store, "RepayCancel", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && p.CANCEL == 1).ToList());
            objectTag.AddObjectData(store, "Bill", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CANCEL != 1).ToList());
            objectTag.AddObjectData(store, "BillCancel", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.CANCEL == 1).ToList());
            objectTag.AddObjectData(store, "CountFeeLock", ListCountFeeLock);

            //thanh toán
            dicSingleTag.Add("TOTAL_BILL_AMOUNT", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_BILL_AMOUNT_CASH", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_BILL_AMOUNT_BANKING", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_BILL_AMOUNT_POS", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__QUET_THE).Sum(o => o.AMOUNT));

            //tạm ứng
            dicSingleTag.Add("TOTAL_DEPOSIT_AMOUNT", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_DEPOSIT_AMOUNT_CASH", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_DEPOSIT_AMOUNT_BANKING", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_DEPOSIT_AMOUNT_POS", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__QUET_THE).Sum(o => o.AMOUNT));

            //hoàn ứng
            dicSingleTag.Add("TOTAL_REPAY_AMOUNT", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_REPAY_AMOUNT_CASH", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__TM).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_REPAY_AMOUNT_BANKING", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__CK).Sum(o => o.AMOUNT));
            dicSingleTag.Add("TOTAL_REPAY_AMOUNT_POS", ListTransaction.Where(p => p.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && p.PAY_FORM_ID == IMSys.DbConfig.HIS_RS.HIS_PAY_FORM.ID__QUET_THE).Sum(o => o.AMOUNT));


            dicSingleTag.Add("TOTAL_FUND_AMOUNT", ListTransaction.Sum(o => o.TDL_BILL_FUND_AMOUNT));
            dicSingleTag.Add("TOTAL_EXEMPTION_AMOUNT", ListTransaction.Sum(o => o.EXEMPTION));
            objectTag.AddObjectData(store, "TransactionDate", ListTransaction.GroupBy(o => new { o.TRANSACTION_DATE, o.CASHIER_LOGINNAME }).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "User", ListTransaction.GroupBy(o => o.CASHIER_LOGINNAME).Select(p => p.First()).ToList());

            objectTag.AddRelationship(store, "User", "TransactionDate", "CASHIER_LOGINNAME", "CASHIER_LOGINNAME");
            objectTag.AddRelationship(store, "TransactionDate", "Transaction", new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" }, new string[] { "CASHIER_LOGINNAME", "TRANSACTION_DATE" });

            objectTag.AddObjectData(store, "DepartmentRoomBill", listDicDepaRoomBill.SelectMany(o => o.Values.ToList()).ToList());

            objectTag.AddObjectData(store, "Ssb", ListSereServBill);

            objectTag.AddRelationship(store, "Transaction", "DepartmentRoomBill", "TRANSACTION_ID", "TRANSACTION_ID");

            if (listCashierLoginname != null && listCashierLoginname.Count > 0)
            {
                objectTag.AddObjectData(store, "NoRepay", ListTransaction.Where(p => listCashierLoginname.Contains(p.CASHIER_LOGINNAME ?? "")).Where(o => o.TRANSACTION_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU || o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU &&/*Loai bo check hoan ung huy*/o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && !listRepaySameDate.SelectMany(p => p).ToList().Contains(o.ID)).ToList());

            }
            else
            {

                objectTag.AddObjectData(store, "NoRepay", ListTransaction.Where(o => o.TRANSACTION_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU || o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU &&/*Loai bo check hoan ung huy*/o.IS_CANCEL != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && !listRepaySameDate.SelectMany(p => p).ToList().Contains(o.ID)).ToList());
            }
            var acsUser = new AcsUserManager(new CommonParam()).Get<List<ACS_USER>>(new AcsUserFilterQuery());
            dicSingleTag.Add("CASHIER_USERNAME", string.Join(" - ", acsUser.Where(o => o.LOGINNAME == filter.CASHIER_LOGINNAME || filter.CASHIER_LOGINNAMEs != null && filter.CASHIER_LOGINNAMEs.Contains(o.LOGINNAME ?? "")).Select(p => p.USERNAME).ToList()));
            dicSingleTag.Add("CASHIER_LOGINNAME", string.Join(" - ", acsUser.Where(o => o.LOGINNAME == filter.CASHIER_LOGINNAME || filter.CASHIER_LOGINNAMEs != null && filter.CASHIER_LOGINNAMEs.Contains(o.LOGINNAME ?? "")).Select(p => p.LOGINNAME).ToList()));

            var listServ = ListSereServBill.GroupBy(o => o.PARENT_SERVICE_CODE).Select(p => p.First()).OrderBy(q => q.PARENT_NUM_ORDER).ToList();
            for (int i = 0; i < listServ.Count; i++)
            {
                dicSingleTag.Add(string.Format("PARENT_NUM_ORDER__{0}", i + 1), listServ[i].PARENT_NUM_ORDER);
                dicSingleTag.Add(string.Format("PARENT_SERVICE_CODE__{0}", i + 1), listServ[i].PARENT_SERVICE_CODE);
                dicSingleTag.Add(string.Format("PARENT_SERVICE_NAME__{0}", i + 1), listServ[i].PARENT_SERVICE_NAME);
            }
            foreach (var item in dicParent)
            {
                if (!dicSingleTag.ContainsKey(item.Key))
                {
                    dicSingleTag.Add(item.Key, item.Value);
                }
                else
                {
                    dicSingleTag[item.Key] = item.Value;
                }
            }
        }

    }
}