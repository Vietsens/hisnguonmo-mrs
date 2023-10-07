using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPayForm;
using MOS.MANAGER.HisTransaction;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using AutoMapper;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00281
{
    class Mrs00281Processor : AbstractProcessor
    {

        CommonParam paramGet = new CommonParam();
        //
        List<V_HIS_TRANSACTION> listDeposit = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TRANSACTION> listRepay = new List<V_HIS_TRANSACTION>();
        List<Mrs00281RDO> ListRdo = new List<Mrs00281RDO>();
        List<Mrs00281RDO> ListDetailRdo = new List<Mrs00281RDO>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, HIS_TREATMENT> dicTreatment = new Dictionary<long, HIS_TREATMENT>();
        string ACCOUNT_BOOK_CREATOR = "";
        ACS_USER User = new ACS_USER();
        short IS_TRUE = 1;
        public Mrs00281Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00281Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00281Filter)reportFilter);
            bool result = true;
            try
            {
                //get dữ liệu:

                HisTransactionViewFilterQuery depositFilter = new HisTransactionViewFilterQuery();
                depositFilter.TRANSACTION_TIME_FROM = ((Mrs00281Filter)this.reportFilter).TIME_FROM;
                depositFilter.TRANSACTION_TIME_TO = ((Mrs00281Filter)this.reportFilter).TIME_TO;
                depositFilter.CREATOR = ((Mrs00281Filter)this.reportFilter).LOGINNAME;
                depositFilter.TRANSACTION_TYPE_IDs = new List<long>() 
                {
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU,
                    IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU
                };
                var listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(depositFilter);
                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    listTransaction = listTransaction.Where(p => filter.CASHIER_LOGINNAMEs.Contains(p.CASHIER_LOGINNAME)).ToList();
                }
                ////depositFilter.HAS_DERE_DETAIL = false; 
                //listDeposit = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(depositFilter); 
                //listDeposit = listDeposit.Where(o => !o.TDL_DERE_DETAIL_COUNT.HasValue || o.TDL_DERE_DETAIL_COUNT <= 0).ToList(); 
                listDeposit = listTransaction.Where(o =>
                    o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU &&
                    (!o.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue || o.TDL_SERE_SERV_DEPOSIT_COUNT <= 0)).ToList();

                listRepay = listTransaction.Where(o =>
                    o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU &&
                    (!o.TDL_SESE_DEPO_REPAY_COUNT.HasValue || o.TDL_SESE_DEPO_REPAY_COUNT <= 0)).ToList();

                //HisTransactionViewFilterQuery repayFilter = new HisTransactionViewFilterQuery(); 
                //repayFilter.CREATE_TIME_FROM = ((Mrs00281Filter)this.reportFilter).TIME_FROM; 
                //repayFilter.CREATE_TIME_TO = ((Mrs00281Filter)this.reportFilter).TIME_TO; 
                //repayFilter.CREATOR = ((Mrs00281Filter)this.reportFilter).LOGINNAME; 
                //repayFilter.TRANSACTION_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU }; 
                ////repayFilter.HAS_DERE_DETAIL = false; 
                //listRepay = new MOS.MANAGER.HisTransaction.HisTransactionManager(paramGet).GetView(repayFilter); 
                //listRepay = listRepay.Where(o => !o.TDL_DERE_DETAIL_COUNT.HasValue || o.TDL_DERE_DETAIL_COUNT <= 0).ToList(); 

                HisCashierRoomViewFilterQuery listCashierRoomFilter = new HisCashierRoomViewFilterQuery();
                listCashierRoomFilter.BRANCH_ID = filter.BRANCH_ID;
                if (filter.EXACT_CASHIER_ROOM_IDs != null) listCashierRoomFilter.IDs = filter.EXACT_CASHIER_ROOM_IDs;
                var listCashierRoom = new HisCashierRoomManager(paramGet).GetView(listCashierRoomFilter);

                var listTreatmentId = new List<long>();
                if (IsNotNullOrEmpty(listDeposit))
                {
                    listDeposit = listDeposit.Where(o => o.IS_CANCEL != IS_TRUE && o.CASHIER_ROOM_ID != null && listCashierRoom.Select(p => p.ID).Contains((long)o.CASHIER_ROOM_ID)).OrderBy(p => p.TRANSACTION_TIME).ToList();

                    listTreatmentId.AddRange(listDeposit.Select(o => o.TREATMENT_ID ?? 0).ToList());
                }
                if (IsNotNullOrEmpty(listRepay))
                {
                    listRepay = listRepay.Where(o => o.IS_CANCEL != IS_TRUE && o.CASHIER_ROOM_ID != null && listCashierRoom.Select(p => p.ID).Contains((long)o.CASHIER_ROOM_ID)).OrderBy(p => p.TRANSACTION_TIME).ToList();
                    listTreatmentId.AddRange(listRepay.Select(o => o.TREATMENT_ID ?? 0).ToList());
                }
                listTreatmentId = listTreatmentId.Distinct().ToList();
                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
                    //Lấy đối tượng để xem các đối tượng đó 
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery treatmentfilter = new HisTreatmentFilterQuery()
                        {
                            IDs = listIDs
                        };
                        var ListTreatmentLib = new HisTreatmentManager(paramGet).Get(treatmentfilter);
                        ListTreatment.AddRange(ListTreatmentLib);

                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,

                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        if (filter.PATIENT_TYPE_IDs != null)
                        {
                            LisPatientTypeAlterLib = LisPatientTypeAlterLib.Where(p => filter.PATIENT_TYPE_IDs.Contains(p.PATIENT_TYPE_ID)).ToList();
                        }
                        listPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                    foreach (var item in ListTreatment)
                    {
                        if (!dicTreatment.ContainsKey(item.ID)) dicTreatment[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(listDeposit)) listDeposit = listDeposit.Where(o => IsTreatIn(o, listPatientTypeAlter)).ToList();
                if (IsNotNullOrEmpty(listRepay)) listRepay = listRepay.Where(o => IsTreatIn(o, listPatientTypeAlter)).ToList();
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
                ListRdo.Clear();
                var listPayFom = new MOS.MANAGER.HisPayForm.HisPayFormManager().Get(new HisPayFormFilterQuery()) ?? new List<HIS_PAY_FORM>();
                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var depo in listDeposit)
                    {
                        Mrs00281RDO rdo = new Mrs00281RDO();
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)depo.TRANSACTION_TIME);
                        rdo.TREATMENT_CODE = dicTreatment.ContainsKey(depo.TREATMENT_ID ?? 0) ? dicTreatment[depo.TREATMENT_ID ?? 0].TREATMENT_CODE : "";
                        rdo.VIR_PATIENT_NAME = depo.TDL_PATIENT_NAME;
                        rdo.DEPOSIT_AMOUNT = depo.AMOUNT;
                        rdo.DEPOSIT_USERNAME = depo.CASHIER_LOGINNAME;

                        var listPayFomSub = listPayFom.Where(o => o.ID == depo.PAY_FORM_ID).ToList();
                        rdo.PAY_FORM_NAME = IsNotNullOrEmpty(listPayFomSub) ? listPayFomSub.First().PAY_FORM_NAME : "";
                        rdo.CASHIER_ROOM_ID = depo.CASHIER_ROOM_ID;
                        rdo.CASHIER_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.CASHIER_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        ListRdo.Add(rdo);
                    }

                    var listDepoCancel = listDeposit.Where(p => p.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                    foreach (var depo1 in listDepoCancel)
                    {
                        Mrs00281RDO rdo = new Mrs00281RDO();
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)depo1.TRANSACTION_TIME);
                        rdo.TREATMENT_CODE = dicTreatment.ContainsKey(depo1.TREATMENT_ID ?? 0) ? dicTreatment[depo1.TREATMENT_ID ?? 0].TREATMENT_CODE : "";
                        rdo.VIR_PATIENT_NAME = depo1.TDL_PATIENT_NAME;
                        rdo.DEPOSIT_AMOUNT = depo1.AMOUNT;
                        rdo.DEPOSIT_USERNAME = depo1.CASHIER_LOGINNAME;

                        var listPayFomSub = listPayFom.Where(o => o.ID == depo1.PAY_FORM_ID).ToList();
                        rdo.PAY_FORM_NAME = IsNotNullOrEmpty(listPayFomSub) ? listPayFomSub.First().PAY_FORM_NAME : "";
                        rdo.CASHIER_ROOM_ID = depo1.CASHIER_ROOM_ID;
                        rdo.CASHIER_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo1.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.CASHIER_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo1.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        ListDetailRdo.Add(rdo);
                    }
                }
                if (IsNotNullOrEmpty(listRepay))
                {
                    foreach (var depo in listRepay)
                    {
                        Mrs00281RDO rdo = new Mrs00281RDO();
                        rdo.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)depo.TRANSACTION_TIME);
                        rdo.TREATMENT_CODE = dicTreatment.ContainsKey(depo.TREATMENT_ID ?? 0) ? dicTreatment[depo.TREATMENT_ID ?? 0].TREATMENT_CODE : "";
                        rdo.VIR_PATIENT_NAME = depo.TDL_PATIENT_NAME;
                        rdo.REPAY_AMOUNT = depo.AMOUNT;
                        rdo.REPAY_USERNAME = depo.CASHIER_USERNAME;
                        rdo.REPAY_LOGINNAME = depo.CASHIER_LOGINNAME;
                        var listPayFomSub = listPayFom.Where(o => o.ID == depo.PAY_FORM_ID).ToList();
                        rdo.PAY_FORM_NAME = IsNotNullOrEmpty(listPayFomSub) ? listPayFomSub.First().PAY_FORM_NAME : "";
                        rdo.CASHIER_ROOM_ID = depo.CASHIER_ROOM_ID;
                        rdo.CASHIER_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
                        rdo.CASHIER_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == depo.CASHIER_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        ListRdo.Add(rdo);
                        ListDetailRdo.Add(rdo);
                    }
                }
                ListRdo = ListRdo.OrderBy(q => q.TRANSACTION_TIME_STR).GroupBy(o => o.TREATMENT_CODE).Select(p => new Mrs00281RDO
                {
                    TRANSACTION_TIME_STR = p.First().TRANSACTION_TIME_STR,
                    TREATMENT_CODE = p.First().TREATMENT_CODE,
                    VIR_PATIENT_NAME = p.First().VIR_PATIENT_NAME,
                    DEPOSIT_USERNAME = p.First().DEPOSIT_USERNAME,
                    REPAY_USERNAME = p.First().REPAY_USERNAME,
                    DEPOSIT_AMOUNT = p.Sum(s => s.DEPOSIT_AMOUNT),
                    REPAY_AMOUNT = p.Sum(s => s.REPAY_AMOUNT),
                    PAY_FORM_NAME = p.First().PAY_FORM_NAME,
                }).ToList();

                ListDetailRdo = ListDetailRdo.GroupBy(P => new { P.REPAY_LOGINNAME, P.CASHIER_ROOM_ID }).Select(p => new Mrs00281RDO
                {
                    CASHIER_ROOM_ID = p.First().CASHIER_ROOM_ID,
                    CASHIER_ROOM_CODE = p.First().CASHIER_ROOM_CODE,
                    CASHIER_ROOM_NAME = p.First().CASHIER_ROOM_NAME,
                    REPAY_LOGINNAME = p.First().REPAY_LOGINNAME,
                    REPAY_USERNAME = p.First().REPAY_USERNAME,
                    REPAY_AMOUNT = p.Sum(o => o.REPAY_AMOUNT)
                }).ToList();

                result = true;
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
            if (((Mrs00281Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00281Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00281Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00281Filter)reportFilter).TIME_TO));
            }

            dicSingleTag.Add("ACCOUNT_BOOK_CREATOR", ((Mrs00281Filter)this.reportFilter).LOGINNAME);

            ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ThenBy(p => p.TRANSACTION_TIME_STR).ToList();
            objectTag.AddObjectData(store, "Report", ListRdo);

            LogSystem.Info("cashier_room: " + string.Join(",", ListDetailRdo.Select(p => p.CASHIER_ROOM_ID)));
            ListDetailRdo = ListDetailRdo.OrderBy(p => p.CASHIER_ROOM_ID).ThenBy(p => p.REPAY_LOGINNAME).ToList();
            objectTag.AddObjectData(store, "Detail", ListDetailRdo);
        }

        private bool IsTreatIn(V_HIS_TRANSACTION o, List<V_HIS_PATIENT_TYPE_ALTER> LisPatientTypeAlter)
        {
            bool Result = false;
            try
            {
                var listInTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (p.LOG_TIME < o.TRANSACTION_TIME)).ToList();
                if (IsNotNullOrEmpty(listInTreat))
                {

                    Result = listInTreat.OrderBy(p => p.LOG_TIME).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                }
                else
                {
                    listInTreat = LisPatientTypeAlter.Where(p => p.TREATMENT_ID == o.TREATMENT_ID && (p.LOG_TIME >= o.TRANSACTION_TIME)).ToList();
                    if (IsNotNullOrEmpty(listInTreat)) Result = listInTreat.OrderBy(p => p.LOG_TIME).First().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                Result = false;
            }
            return Result;
        }


    }
}
