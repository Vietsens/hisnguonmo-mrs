using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
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
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisOtherPaySource;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisPatientCase;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;

namespace MRS.Processor.Mrs00346
{
    class Mrs00346Processor : AbstractProcessor
    {
        Mrs00346Filter castFilter = null;
        List<Mrs00346RDO> listRdo = new List<Mrs00346RDO>();
        List<Mrs00346RDO> listParentRdo = new List<Mrs00346RDO>();
        List<Mrs00346RDO> ListRdoA = new List<Mrs00346RDO>();
        List<Mrs00346RDO> ListRdoB = new List<Mrs00346RDO>();
        List<Mrs00346RDO> ListRdoC = new List<Mrs00346RDO>();

        public Mrs00346Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        HIS_BRANCH branch = null;
        HIS_PATIENT_TYPE sereServPatientType = new HIS_PATIENT_TYPE();
        HIS_PATIENT_TYPE patientType = new HIS_PATIENT_TYPE();
        string TreatmentTypeNames = "";

        List<HIS_TREATMENT> listTreatmentFee = null;
        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();
        PropertyInfo[] pRdo = null;
        PropertyInfo[] pOtherSourcePrice = null;
        List<HIS_PATIENT_CASE> ListPatientCase = new List<HIS_PATIENT_CASE>();
        List<HIS_SERVICE> listService = new List<HIS_SERVICE>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        Dictionary<long, HIS_SERVICE> dicParentService = new Dictionary<long, HIS_SERVICE>();
        Dictionary<long, V_HIS_SERVICE_RETY_CAT> dicCategory = new Dictionary<long, V_HIS_SERVICE_RETY_CAT>();

        public override Type FilterType()
        {
            return typeof(Mrs00346Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00346Filter)this.reportFilter;
                pRdo = Properties.Get<Mrs00346RDO>();
                pOtherSourcePrice = pRdo.Where(o => o.Name.StartsWith("TOTAL_OTHER_SOURCE_PRICE_")).ToArray();
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao MRS00346: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                //List<long> listRoomId = new List<long>();
                branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == castFilter.BRANCH_ID);

                //if (castFilter.BRANCH_ID.HasValue)
                //{
                //    listRoomId = MANAGER.Config.HisRoomCFG.HisRooms.Where(s => s.BRANCH_ID == castFilter.BRANCH_ID.Value).Select(s => s.ID).ToList();
                //}
                //else
                //{
                //    branch = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.branch_id);
                //    listRoomId = MANAGER.Config.HisRoomCFG.HisRooms.Select(s => s.ID).ToList();
                //}

                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                {
                    var listTreatmentTYpe = MANAGER.Config.HisTreatmentTypeCFG.HisTreatmentTypes.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.ID)).ToList();
                    foreach (var item in listTreatmentTYpe)
                    {
                        if (String.IsNullOrEmpty(TreatmentTypeNames))
                            TreatmentTypeNames = item.TREATMENT_TYPE_NAME;
                        else
                            TreatmentTypeNames += ", " + item.TREATMENT_TYPE_NAME;
                    }
                }

                //if (listRoomId == null || listRoomId.Count == 0)
                //{
                //    throw new Exception("Nguoi dung truyen BranchId khong chinh xac: " + castFilter.BRANCH_ID.Value);
                //}

                HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                if (castFilter.TIME_TYPE.HasValue)
                {
                    if (castFilter.TIME_TYPE.Value == 0)
                    {
                        treatFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.IN_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_PAUSE = castFilter.IS_PAUSE;
                        treatFilter.IS_ACTIVE = castFilter.IS_ACTIVE == true ? IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE : IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                    else if (castFilter.TIME_TYPE.Value == 1)
                    {
                        treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.OUT_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_PAUSE = true;
                        //treatFilter.END_ROOM_IDs = listRoomId;
                        treatFilter.IS_ACTIVE = castFilter.IS_ACTIVE == true ? IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE : IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                    else
                    {
                        treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                        //treatFilter.END_ROOM_IDs = listRoomId;
                    }
                }
                else
                {
                    treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                    treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    //treatFilter.END_ROOM_IDs = listRoomId;
                }
                if (castFilter.INPUT_DATA_ID_STT_TYPE != null)
                {
                    treatFilter = new HisTreatmentFilterQuery();

                    //đang điều trị
                    if (castFilter.INPUT_DATA_ID_STT_TYPE == 1)
                    {
                        treatFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.IN_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_PAUSE = false;
                    }
                    //đã kết thúc điều trị
                    else if (castFilter.INPUT_DATA_ID_STT_TYPE == 2)
                    {
                        treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.OUT_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_PAUSE = true;
                    }
                    //đã khóa viện phí
                    else if (castFilter.INPUT_DATA_ID_STT_TYPE == 3)
                    {
                        treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                    //đã khóa bảo hiểm
                    else if (castFilter.INPUT_DATA_ID_STT_TYPE == 4)
                    {
                        treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_LOCK_HEIN = true;
                    }
                    else
                    {
                        treatFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                        treatFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                    }
                }
                listTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).Get(treatFilter);
                if (castFilter.BRANCH_ID != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                }
                if (castFilter.LAST_DEPARTMENT_IDs != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o =>castFilter.LAST_DEPARTMENT_IDs.Contains(o.LAST_DEPARTMENT_ID??0)).ToList();
                }

                if (this.castFilter.SERE_SERV_PATIENT_TYPE_ID != null)
                {
                    sereServPatientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == this.castFilter.SERE_SERV_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                }
                if (this.castFilter.PATIENT_TYPE_ID != null)
                {
                    patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == this.castFilter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                    listTreatmentFee = listTreatmentFee.Where(o => o.TDL_PATIENT_TYPE_ID == this.castFilter.PATIENT_TYPE_ID).ToList();
                }
                if (this.castFilter.FEE_LOCK_LOGINNAMEs != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o => this.castFilter.FEE_LOCK_LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME ?? "")).ToList();
                }
                if (this.castFilter.FEE_LOCK_ROOM_IDs != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o => this.castFilter.FEE_LOCK_ROOM_IDs.Contains(o.FEE_LOCK_ROOM_ID ?? 0)).ToList();
                }
                if (this.castFilter.IS_TREAT != null)
                {
                    if (this.castFilter.IS_TREAT == true)
                    {
                        listTreatmentFee = listTreatmentFee.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                    else
                    {
                        listTreatmentFee = listTreatmentFee.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    }
                }
                if (this.castFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o => this.castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (this.castFilter.PATIENT_TYPE_IDs != null)
                {
                    listTreatmentFee = listTreatmentFee.Where(o => this.castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                HisOtherPaySourceFilterQuery otherPaySourceFilter = new HisOtherPaySourceFilterQuery();
                otherPaySourceFilter.ORDER_DIRECTION = "ASC";
                otherPaySourceFilter.ORDER_FIELD = "OTHER_PAY_SOURCE_CODE";
                this.listOtherPaySource = new HisOtherPaySourceManager().Get(otherPaySourceFilter);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00345");
                }
                HisPatientCaseFilterQuery pcFilter = new HisPatientCaseFilterQuery();
                ListPatientCase = new HisPatientCaseManager().Get(pcFilter);
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00346";
                listServiceRetyCat = new HisServiceRetyCatManager().GetView(serviceRetyCatFilter);
                HisServiceFilterQuery serviceFilter = new HisServiceFilterQuery();
                listService = new HisServiceManager().Get(serviceFilter);
                dicParentService = listService.GroupBy(p => p.ID).ToDictionary(p => p.Key, q => listService.FirstOrDefault(s => s.ID == q.First().PARENT_ID) ?? new HIS_SERVICE());
                dicCategory = listServiceRetyCat.GroupBy(p => p.SERVICE_ID).ToDictionary(p => p.Key, q => q.First());
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
                if (IsNotNullOrEmpty(listTreatmentFee))
                {
                    int start = 0;
                    int count = listTreatmentFee.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var listSub = listTreatmentFee.Skip(start).Take(limit).ToList();

                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        //ssFilter.PATIENT_TYPE_ID = this.castFilter.SERE_SERV_PATIENT_TYPE_ID;
                        ssFilter.HAS_EXECUTE = true;
                        var listSereServAll = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).Get(ssFilter);
                        var listSereServ = listSereServAll;
                        if (this.dicDataFilter.ContainsKey("IS_FILTER_NOT_BHYT") && this.dicDataFilter["IS_FILTER_NOT_BHYT"] != null && this.dicDataFilter["IS_FILTER_NOT_BHYT"].ToString() == "True")
                        {
                            foreach (var item in listSereServ)
                            {
                                if ((item.VIR_TOTAL_PATIENT_PRICE ?? 0) - (item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0) <= 0)
                                {
                                    item.IS_NO_EXECUTE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                                }
                                item.VIR_TOTAL_PRICE = (item.VIR_TOTAL_PATIENT_PRICE ?? 0) - (item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                item.VIR_TOTAL_PATIENT_PRICE = (item.VIR_TOTAL_PATIENT_PRICE ?? 0) - (item.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                                item.VIR_TOTAL_HEIN_PRICE = 0;
                                item.VIR_TOTAL_PATIENT_PRICE_BHYT = 0;
                            }
                            listSereServ = listSereServ.Where(o => o.IS_NO_EXECUTE == null).ToList();
                        }

                        if (this.castFilter.HAS_PRIMARY_PATIENT_TYPE_ID == true)
                        {
                            listSereServ = listSereServ.Where(o => o.PRIMARY_PATIENT_TYPE_ID.HasValue).ToList();
                        }

                        if (castFilter.SERVICE_TYPE_IDs != null)
                        {
                            listSereServ = listSereServ.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                        }

                        if (castFilter.SERE_SERV_PATIENT_TYPE_ID != null)
                        {
                            listSereServ = listSereServ.Where(o => castFilter.SERE_SERV_PATIENT_TYPE_ID == o.PATIENT_TYPE_ID).ToList();
                        }

                        if (castFilter.EXACT_PARENT_SERVICE_IDs != null)
                        {
                            if (listService != null)
                            {
                                listSereServ = listSereServ.Where(o => listService.Exists(p => p.ID == o.SERVICE_ID && castFilter.EXACT_PARENT_SERVICE_IDs.Contains(p.PARENT_ID ?? 0))).ToList();
                            }
                        }
                        HisSereServDepositViewFilterQuery ssdFilter = new HisSereServDepositViewFilterQuery();
                        ssdFilter.TDL_TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        //ssdFilter.PATIENT_TYPE_ID = this.castFilter.SERE_SERV_PATIENT_TYPE_ID;
                        ssdFilter.IS_CANCEL = false;
                        var listSereServDeposit = new HisSereServDepositManager(paramGet).GetView(ssdFilter);

                        HisTransactionFilterQuery transactionFilter = new HisTransactionFilterQuery();
                        transactionFilter.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        //transactionFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                        transactionFilter.IS_CANCEL = false;
                        var listTransaction = new HisTransactionManager(paramGet).Get(transactionFilter);
                        //yeu cau kham
                        HisServiceReqFilterQuery filterSr = new HisServiceReqFilterQuery();
                        filterSr.TREATMENT_IDs = listSub.Select(s => s.ID).ToList();
                        filterSr.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                        filterSr.SERVICE_REQ_STT_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT;
                        filterSr.HAS_EXECUTE = true;
                        var listServiceReqSub = new HisServiceReqManager(paramGet).Get(filterSr);
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00346");
                        }

                        Inventec.Common.Logging.LogSystem.Error("count listSub:" + listSub.Count);
                        this.ProcessDataDetail(listSub, listSereServAll, listSereServ, listTransaction, listServiceReqSub, listSereServDeposit, listService);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    this.ProcessParentRDO();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail(List<HIS_TREATMENT> listTreatmentFeeLocal, List<HIS_SERE_SERV> listSereServAll, List<HIS_SERE_SERV> listSereServLocal, List<HIS_TRANSACTION> listTransactionLocal, List<HIS_SERVICE_REQ> listServiceReqLocal, List<V_HIS_SERE_SERV_DEPOSIT> listSereServDepositLocal, List<HIS_SERVICE> listService)
        {
            foreach (var treatment in listTreatmentFeeLocal)
            {

                //var curentPatientType = listPatientTypeAlterLocal.Where(o => o.TREATMENT_ID == treatment.ID).OrderBy(p => p.LOG_TIME).ThenBy(q => q.ID).LastOrDefault() ?? new V_HIS_PATIENT_TYPE_ALTER();
                var curentBill = listTransactionLocal.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && o.TREATMENT_ID == treatment.ID).OrderBy(p => p.TRANSACTION_TIME).ThenBy(q => q.ID).LastOrDefault() ?? new HIS_TRANSACTION();
                var curentTran = listTransactionLocal.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                var treatFee = treatment;
                var hisSereServAll = listSereServAll.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                var hisSereServ = listSereServLocal.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                var hisSereServDeposit = listSereServDepositLocal.Where(o => o.TDL_TREATMENT_ID == treatment.ID).ToList();
                var hisServiceReqSub = listServiceReqLocal.Where(o => o.TREATMENT_ID == treatment.ID).ToList();

                if (castFilter.SERE_SERV_PATIENT_TYPE_IDs != null)
                {
                    var ssFil = hisSereServ.Where(o => castFilter.SERE_SERV_PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();
                    if (ssFil.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        hisSereServ = ssFil;
                    }
                }

                if (castFilter.SERE_SERV_PATIENT_TYPE_ID.HasValue)
                {
                    var ssFil = hisSereServ.Where(o => o.PATIENT_TYPE_ID == castFilter.SERE_SERV_PATIENT_TYPE_ID).ToList();
                    if (ssFil.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        hisSereServ = ssFil;
                    }
                }

                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    var ssFil = hisSereServ.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                    if (ssFil.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        hisSereServ = ssFil;
                    }
                }

                if (castFilter.EXACT_PARENT_SERVICE_IDs != null)
                {
                    var ssFil = hisSereServ.Where(o => listService.Exists(p => p.ID == o.SERVICE_ID && castFilter.EXACT_PARENT_SERVICE_IDs.Contains(p.PARENT_ID ?? 0))).ToList();
                    if (ssFil.Count <= 0)
                    {
                        continue;
                    }
                    else
                    {
                        hisSereServ = ssFil;
                    }
                }

                if (castFilter.SERE_SERV_PATIENT_TYPE_ID.HasValue)
                {
                    hisSereServDeposit = hisSereServDeposit.Where(o => o.TDL_PATIENT_TYPE_ID == castFilter.SERE_SERV_PATIENT_TYPE_ID).ToList();
                }
                Mrs00346RDO rdo = new Mrs00346RDO(treatment);
                rdo.DIC_EXPEND_PRICE = new Dictionary<string, decimal>();
                rdo.END_ROOM_ID = rdo.END_ROOM_ID ?? (hisSereServ.OrderBy(o => o.TDL_INTRUCTION_TIME).LastOrDefault() ?? new HIS_SERE_SERV()).TDL_REQUEST_ROOM_ID;
                rdo.END_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.END_DEPARTMENT_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                rdo.END_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == rdo.END_DEPARTMENT_ID) ?? new V_HIS_ROOM()).ROOM_NAME;

                if (castFilter.IS_SHOW_ROOM == true)
                {
                    rdo.END_DEPARTMENT_ID = rdo.END_ROOM_ID;
                    rdo.END_DEPARTMENT_CODE = rdo.END_ROOM_CODE;
                    rdo.END_DEPARTMENT_NAME = rdo.END_ROOM_NAME;
                }
                else
                {
                    rdo.END_DEPARTMENT_ID = rdo.END_DEPARTMENT_ID ?? rdo.LAST_DEPARTMENT_ID;
                    rdo.END_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    rdo.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == rdo.END_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                }

                //đối tượng bệnh nhân
                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == rdo.TDL_PATIENT_TYPE_ID);
                if (patientType != null)
                {
                    rdo.TDL_PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                    rdo.TDL_PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                }

                rdo.TOTAL_PATIENT_PRICE = hisSereServAll.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                rdo.TOTAL_PATIENT_PRICE_BHYT = hisSereServAll.Sum(o => o.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                rdo.TOTAL_PATIENT_PRICE_DIFF = hisSereServAll.Sum(o => (o.VIR_TOTAL_PATIENT_PRICE ?? 0) - (o.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                rdo.TOTAL_HEIN_PRICE = hisSereServAll.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0);
                rdo.TOTAL_DISCOUNT = hisSereServAll.Sum(o => o.DISCOUNT ?? 0);
                rdo.DEPOSIT_AMOUNT = curentTran.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                rdo.REPAY_AMOUNT = curentTran.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT);
                rdo.BILL_AMOUNT = curentTran.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => s.AMOUNT);
                rdo.TOTAL_PRICE = hisSereServAll.Sum(o => o.VIR_TOTAL_PRICE ?? 0);

                if (treatment.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    rdo.HEIN_TREATMENT_TYPE_CODE = treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU ? "DT" : "KH";
                    if (treatment.TDL_HEIN_CARD_NUMBER != null && treatment.TDL_HEIN_CARD_NUMBER.Length>=2)
                    {
                        rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                        rdo.HEIN_CARD_NUMBER_1 = treatment.TDL_HEIN_CARD_NUMBER.Substring(0, 2);
                        rdo.HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Doi tuong cu ho so dieu tri la BHYT nhung khong co thong tin BHYT: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.TREATMENT_CODE), treatment.TREATMENT_CODE));
                    }
                }
                if (curentBill.ID > 0)
                {
                    rdo.NUM_ORDER = curentBill.NUM_ORDER;
                    rdo.TRANSACTION_TIME = curentBill.TRANSACTION_TIME;
                    rdo.EXEMPTION = curentBill.EXEMPTION;
                    //thong tin hoa don dien tu
                    rdo.INVOICE_CODE = curentBill.INVOICE_CODE;
                    rdo.INVOICE_SYS = curentBill.INVOICE_SYS;
                    rdo.EINVOICE_NUM_ORDER = curentBill.EINVOICE_NUM_ORDER;
                }

                this.ProcessPatientCase(rdo, hisServiceReqSub, treatment.IN_ROOM_ID);
                this.ProcessDetaiPriceService(rdo, hisSereServ);
                if (this.castFilter.HAS_PRIMARY_PATIENT_TYPE_ID == true)
                {
                    rdo.TOTAL_PRICE = rdo.VIR_TOTAL_PRICE;
                    rdo.TOTAL_PATIENT_PRICE = rdo.VIR_TOTAL_PATIENT_PRICE;
                    rdo.TOTAL_PATIENT_PRICE_BHYT = rdo.VIR_TOTAL_PATIENT_PRICE_BHYT;
                    rdo.TOTAL_HEIN_PRICE = rdo.VIR_TOTAL_HEIN_PRICE;
                }
                this.ProcessDetaiDepositService(rdo, hisSereServDeposit);
            }
        }

        private void ProcessDetaiDepositService(Mrs00346RDO rdo, List<V_HIS_SERE_SERV_DEPOSIT> hisSereServDeposit)
        {
            try
            {
                foreach (var sereServDeposit in hisSereServDeposit)
                {
                    rdo.SESE_DEPO_AMOUNT += sereServDeposit.AMOUNT;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }



        private void ProcessPatientCase(Mrs00346RDO rdo, List<HIS_SERVICE_REQ> serviceReqSubs, long? inRoomId)
        {
            try
            {
                HIS_SERVICE_REQ sr = null;

                if (inRoomId != null)
                {
                    sr = serviceReqSubs.FirstOrDefault(o => o.EXECUTE_ROOM_ID == inRoomId && o.PATIENT_CASE_ID.HasValue);
                }
                if (sr == null)
                {
                    sr = serviceReqSubs.OrderByDescending(p => p.FINISH_TIME).FirstOrDefault(o => o.PATIENT_CASE_ID.HasValue);
                }
                if (sr != null)
                {
                    if (ListPatientCase != null)
                    {
                        var pc = ListPatientCase.FirstOrDefault(o => o.ID == sr.PATIENT_CASE_ID);
                        if (pc != null)
                        {
                            rdo.PATIENT_CASE_NAME = pc.PATIENT_CASE_NAME;
                            rdo.PATIENT_CASE_CODE = pc.PATIENT_CASE_CODE;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        void ProcessDetaiPriceService(Mrs00346RDO rdo, List<HIS_SERE_SERV> listSereServ)
        {
            rdo.DIC_SVT_TOTAL_PRICE = new Dictionary<string, decimal>();// listSereServ.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
            rdo.DIC_PAR_TOTAL_PRICE = listSereServ.GroupBy(o => dicParentService.ContainsKey(o.SERVICE_ID) ? dicParentService[o.SERVICE_ID].SERVICE_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
            rdo.DIC_CATE_TOTAL_PRICE = listSereServ.GroupBy(o => dicCategory.ContainsKey(o.SERVICE_ID) ? dicCategory[o.SERVICE_ID].CATEGORY_CODE ?? "NONE" : "NONE").ToDictionary(p => p.Key, q => q.Sum(s => s.VIR_TOTAL_PRICE ?? 0));
            foreach (var sereServ in listSereServ)
            {
                var serviceType = HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServ.TDL_SERVICE_TYPE_ID);
                if (serviceType != null)
                {
                    if (rdo.DIC_SVT_TOTAL_PRICE.ContainsKey(serviceType.SERVICE_TYPE_CODE))
                    {
                        rdo.DIC_SVT_TOTAL_PRICE[serviceType.SERVICE_TYPE_CODE] += sereServ.VIR_TOTAL_PRICE ?? 0;
                    }
                    else
                    {
                        rdo.DIC_SVT_TOTAL_PRICE.Add(serviceType.SERVICE_TYPE_CODE, sereServ.VIR_TOTAL_PRICE ?? 0);
                    }
                }
                
                if (this.castFilter.HAS_PRIMARY_PATIENT_TYPE_ID == true)
                {
                    sereServ.VIR_TOTAL_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE;
                }
                if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sereServ.PARENT_ID.HasValue)
                {
                    if (rdo.TOTAL_PRICE_EXPEND == null)
                        rdo.TOTAL_PRICE_EXPEND = 0;
                    rdo.TOTAL_PRICE_EXPEND += (sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                    var parentSS = listSereServ.FirstOrDefault(o => o.ID == sereServ.PARENT_ID.Value);
                    if (parentSS != null)
                    {
                        if (parentSS.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || parentSS.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT)
                        {
                            if (rdo.EXPEND_PTTT_AMOUNT == null)
                                rdo.EXPEND_PTTT_AMOUNT = 0;
                            rdo.EXPEND_PTTT_AMOUNT += (sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                        }
                        else if (parentSS.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G)
                        {
                            if (rdo.EXPEND_BED_AMOUNT == null)
                                rdo.EXPEND_BED_AMOUNT = 0;
                            rdo.EXPEND_BED_AMOUNT += (sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                        }
                    }
                }
                else if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    if (rdo.TOTAL_PRICE_EXPEND == null)
                        rdo.TOTAL_PRICE_EXPEND = 0;
                    rdo.TOTAL_PRICE_EXPEND += (sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0);
                    string key = string.Format("{0}", sereServ.TDL_SERVICE_TYPE_ID);
                    if (!rdo.DIC_EXPEND_PRICE.ContainsKey(key))
                    {
                        rdo.DIC_EXPEND_PRICE[key] = 0;
                    }
                    rdo.DIC_EXPEND_PRICE[key] += sereServ.VIR_TOTAL_PRICE_NO_EXPEND ?? 0;
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                {
                    if (rdo.TOTAL_TEST == null)
                        rdo.TOTAL_TEST = 0;
                    rdo.TOTAL_TEST += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                {
                    if (rdo.TOTAL_DIIM_FUEX == null)
                        rdo.TOTAL_DIIM_FUEX = 0;
                    rdo.TOTAL_DIIM_FUEX += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                {
                    if (rdo.TOTAL_MEDICINE == null)
                        rdo.TOTAL_MEDICINE = 0;
                    rdo.TOTAL_MEDICINE += (sereServ.VIR_TOTAL_PRICE ?? 0);
                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                    {
                        rdo.TOTAL_MEDICINE_RATIO += (sereServ.VIR_TOTAL_PRICE ?? 0);
                    }
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                {
                    if (rdo.TOTAL_BLOOD == null)
                        rdo.TOTAL_BLOOD = 0;
                    rdo.TOTAL_BLOOD += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                {
                    if (rdo.TOTAL_SURG_MISU == null)
                        rdo.TOTAL_SURG_MISU = 0;
                    rdo.TOTAL_SURG_MISU += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                {
                    if (rdo.TOTAL_MATERIAL == null)
                        rdo.TOTAL_MATERIAL = 0;
                    rdo.TOTAL_MATERIAL += (sereServ.VIR_TOTAL_PRICE ?? 0);
                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL)
                    {
                        rdo.TOTAL_MATERIAL_RATIO += (sereServ.VIR_TOTAL_PRICE ?? 0);
                    }
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                {
                    if (rdo.TOTAL_HIGHTECH == null)
                        rdo.TOTAL_HIGHTECH = 0;
                    rdo.TOTAL_HIGHTECH += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                {
                    if (rdo.TOTAL_MEDICINE_CANCER == null)
                        rdo.TOTAL_MEDICINE_CANCER = 0;
                    rdo.TOTAL_MEDICINE_CANCER += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                {
                    if (rdo.TOTAL_EXAM == null)
                        rdo.TOTAL_EXAM = 0;
                    rdo.TOTAL_EXAM += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                {
                    if (rdo.TOTAL_BED == null)
                        rdo.TOTAL_BED = 0;
                    rdo.TOTAL_BED += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                {
                    if (rdo.TOTAL_TRAN == null)
                        rdo.TOTAL_TRAN = 0;
                    rdo.TOTAL_TRAN += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }
                else
                {
                    if (rdo.TOTAL_OTHER == null)
                        rdo.TOTAL_OTHER = 0;
                    rdo.TOTAL_OTHER += (sereServ.VIR_TOTAL_PRICE ?? 0);
                }


                rdo.VIR_TOTAL_PRICE += (sereServ.VIR_TOTAL_PRICE ?? 0);

                rdo.VIR_TOTAL_PATIENT_PRICE += (sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0);

                rdo.VIR_TOTAL_PATIENT_PRICE_BHYT += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                rdo.DISCOUNT += (sereServ.DISCOUNT ?? 0);

                rdo.VIR_TOTAL_PATIENT_PRICE_DIFF += (sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0) + (sereServ.DISCOUNT ?? 0) - (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);

                rdo.VIR_TOTAL_HEIN_PRICE += (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);

                rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;

                this.ProcessorOtherSourcePrice(sereServ.OTHER_PAY_SOURCE_ID, (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT, ref rdo);

            }
            if (checkBhytNsd(rdo))
            {
                rdo.TOTAL_HEIN_PRICE_NDS = rdo.VIR_TOTAL_HEIN_PRICE;
                rdo.VIR_TOTAL_HEIN_PRICE = 0;
            }
            listRdo.Add(rdo);

            if (this.branch != null && (this.branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(rdo.HEIN_MEDI_ORG_CODE ?? " ")
                && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
            {
                ListRdoA.Add(rdo);
            }
            else if (checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
            {
                ListRdoB.Add(rdo);
            }
            else
            {
                ListRdoC.Add(rdo);
            }
        }

        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
                {
                    string provinceCode = HeinNumber.Substring(3, 2);
                    if (this.branch != null && this.branch.HEIN_PROVINCE_CODE.Equals(provinceCode))
                    {
                        result = true;
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

        private bool checkBhytNsd(Mrs00346RDO rdo)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE ?? " "))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE))
                {
                    if (rdo.HEIN_CARD_NUMBER != null && rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains((rdo.ICD_CODE ?? "   ").Substring(0, 3)))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }


        private void ProcessorOtherSourcePrice(long? _OtherPaySourceId, decimal OtherSourcePrice, ref Mrs00346RDO rdo)
        {
            if (_OtherPaySourceId == null)
                return;
            int count = pOtherSourcePrice.Length;
            if (this.listOtherPaySource != null && count > this.listOtherPaySource.Count)
                count = this.listOtherPaySource.Count;
            for (int i = 0; i < count; i++)
            {
                if (_OtherPaySourceId == this.listOtherPaySource[i].ID)
                {
                    decimal value = (decimal)pOtherSourcePrice[i].GetValue(rdo);
                    pOtherSourcePrice[i].SetValue(rdo, OtherSourcePrice + value);
                }
            }
        }

        void ProcessParentRDO()
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.END_DEPARTMENT_CODE).ThenBy(o => o.TREATMENT_CODE).ToList();
                    var Groups = listRdo.GroupBy(g => g.END_DEPARTMENT_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00346RDO>();
                        Mrs00346RDO rdo = new Mrs00346RDO();
                        rdo.END_DEPARTMENT_ID = listSub.First().END_DEPARTMENT_ID;
                        rdo.END_DEPARTMENT_CODE = listSub.First().END_DEPARTMENT_CODE;
                        rdo.END_DEPARTMENT_NAME = listSub.First().END_DEPARTMENT_NAME;

                        rdo.EXEMPTION = listSub.First().EXEMPTION;

                        rdo.DEPOSIT_AMOUNT = listSub.Sum(s => s.DEPOSIT_AMOUNT ?? 0);
                        rdo.EXPEND_BED_AMOUNT = listSub.Sum(s => s.EXPEND_BED_AMOUNT ?? 0);
                        rdo.EXPEND_PTTT_AMOUNT = listSub.Sum(s => s.EXPEND_PTTT_AMOUNT ?? 0);
                        rdo.OTHER_AMOUNT = listSub.Sum(s => s.OTHER_AMOUNT ?? 0);
                        rdo.REPAY_AMOUNT = listSub.Sum(s => s.REPAY_AMOUNT ?? 0);
                        rdo.SUB_AMOUNT = listSub.Sum(s => s.SUB_AMOUNT ?? 0);
                        rdo.TOTAL_BED = listSub.Sum(s => s.TOTAL_BED ?? 0);
                        rdo.TOTAL_BLOOD = listSub.Sum(s => s.TOTAL_BLOOD ?? 0);
                        rdo.TOTAL_DIIM_FUEX = listSub.Sum(s => s.TOTAL_DIIM_FUEX ?? 0);
                        rdo.TOTAL_DISCOUNT = listSub.Sum(s => s.TOTAL_DISCOUNT);
                        rdo.TOTAL_EXAM = listSub.Sum(s => s.TOTAL_EXAM ?? 0);
                        rdo.TOTAL_HEIN_PRICE = listSub.Sum(s => s.TOTAL_HEIN_PRICE);
                        rdo.TOTAL_HIGHTECH = listSub.Sum(s => s.TOTAL_HIGHTECH ?? 0);
                        rdo.TOTAL_MATERIAL = listSub.Sum(s => s.TOTAL_MATERIAL ?? 0);
                        rdo.TOTAL_MEDICINE = listSub.Sum(s => s.TOTAL_MEDICINE ?? 0);
                        rdo.TOTAL_MEDICINE_CANCER = listSub.Sum(s => s.TOTAL_MEDICINE_CANCER ?? 0);
                        rdo.TOTAL_OTHER = listSub.Sum(s => s.TOTAL_OTHER ?? 0);
                        rdo.TOTAL_PRICE = listSub.Sum(s => s.TOTAL_PRICE);
                        rdo.TOTAL_PATIENT_PRICE = listSub.Sum(s => s.TOTAL_PATIENT_PRICE);
                        rdo.TOTAL_PATIENT_PRICE_BHYT = listSub.Sum(s => s.TOTAL_PATIENT_PRICE_BHYT);
                        rdo.TOTAL_PATIENT_PRICE_DIFF = listSub.Sum(s => s.TOTAL_PATIENT_PRICE_DIFF);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE);
                        rdo.TOTAL_OTHER_SOURCE_PRICE_1 = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE_1);
                        rdo.TOTAL_OTHER_SOURCE_PRICE_2 = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE_2);
                        rdo.TOTAL_OTHER_SOURCE_PRICE_3 = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE_3);
                        rdo.TOTAL_OTHER_SOURCE_PRICE_4 = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE_4);
                        rdo.TOTAL_OTHER_SOURCE_PRICE_5 = listSub.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE_5);
                        rdo.TOTAL_SURG_MISU = listSub.Sum(s => s.TOTAL_SURG_MISU ?? 0);
                        rdo.TOTAL_TEST = listSub.Sum(s => s.TOTAL_TEST ?? 0);
                        rdo.TOTAL_TRAN = listSub.Sum(s => s.TOTAL_TRAN ?? 0);
                        rdo.SESE_DEPO_AMOUNT = listSub.Sum(s => s.SESE_DEPO_AMOUNT);
                        rdo.VIR_TOTAL_PRICE = listSub.Sum(s => s.VIR_TOTAL_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE);
                        rdo.VIR_TOTAL_HEIN_PRICE = listSub.Sum(s => s.VIR_TOTAL_HEIN_PRICE);
                        rdo.VIR_TOTAL_PATIENT_PRICE_BHYT = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT);
                        rdo.TOTAL_PRICE_EXPEND = listSub.Sum(s => s.TOTAL_PRICE_EXPEND ?? 0);
                        rdo.BILL_AMOUNT = listSub.Sum(s => s.BILL_AMOUNT);
                        listParentRdo.Add(rdo);
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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (branch != null)
                {
                    dicSingleTag.Add("BRANCH_NAME", branch.BRANCH_NAME);
                }
                if (sereServPatientType != null)
                {
                    dicSingleTag.Add("SERE_SERV_PATIENT_TYPE_NAME", "Đối tượng thanh toán: " + sereServPatientType.PATIENT_TYPE_NAME);
                }
                if (patientType != null)
                {
                    dicSingleTag.Add("PATIENT_TYPE_NAME", "Đối tượng Bệnh nhân: " + patientType.PATIENT_TYPE_NAME);
                }
                dicSingleTag.Add("TREATMENT_TYPE_NAMEs", TreatmentTypeNames);
                int count = pOtherSourcePrice.Length;
                if (this.listOtherPaySource != null && count > this.listOtherPaySource.Count)
                    count = this.listOtherPaySource.Count;
                for (int i = 0; i < count; i++)
                {
                    dicSingleTag.Add(string.Format("OTHER_PAY_SOURCE_CODE_{0}", i + 1), this.listOtherPaySource[i].OTHER_PAY_SOURCE_CODE);
                    dicSingleTag.Add(string.Format("OTHER_PAY_SOURCE_NAME_{0}", i + 1), this.listOtherPaySource[i].OTHER_PAY_SOURCE_NAME);
                }
                //nhà thu ngân:

                objectTag.AddObjectData(store, "Parent", listParentRdo);
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.FEE_LOCK_TIME).ToList());
                objectTag.AddRelationship(store, "Parent", "Report", "END_DEPARTMENT_ID", "END_DEPARTMENT_ID");
                objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                objectTag.AddObjectData(store, "NotBillPauses", listRdo.Where(p => p.TRANSACTION_TIME == 0 && p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "NotBillNotPauses", listRdo.Where(p => p.TRANSACTION_TIME == 0 && p.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "NotEInvoicePauses", listRdo.Where(p => string.IsNullOrWhiteSpace(p.EINVOICE_NUM_ORDER) && p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "NotEInvoiceNotPauses", listRdo.Where(p => string.IsNullOrWhiteSpace(p.EINVOICE_NUM_ORDER) && p.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "ParentService", dicParentService.Values.GroupBy(o => o.ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "CategoryService", dicCategory.Values.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "Rooms", HisRoomCFG.HisRooms);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
