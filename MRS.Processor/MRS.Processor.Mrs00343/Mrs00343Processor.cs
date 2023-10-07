using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisCashierRoom;
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

namespace MRS.Processor.Mrs00343
{
    public class Mrs00343Processor : AbstractProcessor
    {
        Mrs00343Filter castFilter = null; 
        List<Mrs00343RDO> listRdo = new List<Mrs00343RDO>(); 

        List<V_HIS_TRANSACTION> listDeposit = null; 
        List<V_HIS_TRANSACTION> listTransaction = null; 
        List<V_HIS_CASHIER_ROOM> listCashierRoom = null; 
        HIS_BRANCH _Branch = null; 
        decimal totalPrice = 0; 


        public Mrs00343Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00343Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00343Filter)this.reportFilter; 
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID); 
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac"); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPOSIT, MRS00343: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                CommonParam paramGet = new CommonParam(); 

                HisCashierRoomViewFilterQuery cashierRoomFilter = new HisCashierRoomViewFilterQuery(); 
                cashierRoomFilter.BRANCH_ID = castFilter.BRANCH_ID; 
                listCashierRoom = new MOS.MANAGER.HisCashierRoom.HisCashierRoomManager(paramGet).GetView(cashierRoomFilter); 

                HisTransactionViewFilterQuery tranFilter = new HisTransactionViewFilterQuery();
                tranFilter.TRANSACTION_TIME_FROM = castFilter.TIME_FROM;
                tranFilter.TRANSACTION_TIME_TO = castFilter.TIME_TO; 
                listTransaction = new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(tranFilter); 
                listDeposit = listTransaction.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList(); 
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00339"); 
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
                List<long> listTreatmentId = new List<long>(); 
                if (IsNotNullOrEmpty(listDeposit))
                {
                    listTreatmentId.AddRange(listDeposit.Where(o=>o.TREATMENT_ID.HasValue).Select(s => s.TREATMENT_ID.Value).Distinct().ToList()); 
                }

                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>(); 

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    listTreatmentId = listTreatmentId.Distinct().ToList(); 
                    int start = 0; 
                    int count = listTreatmentId.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        var listId = listTreatmentId.Skip(start).Take(limit).ToList(); 

                        HisPatientTypeAlterViewFilterQuery patyAlterFilter = new HisPatientTypeAlterViewFilterQuery(); 
                        patyAlterFilter.TREATMENT_IDs = listId; 
                        var listPatyAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patyAlterFilter); 
                        if (paramGet.HasException)
                        {
                            throw new Exception("Co exception xay ra tai DAOGET trong qua tring tong hop du lieu MRS00339"); 
                        }

                        if (IsNotNullOrEmpty(listPatyAlter))
                        {
                            listPatyAlter = listPatyAlter.OrderBy(o => o.LOG_TIME).ToList(); 
                            var Groups = listPatyAlter.GroupBy(o => o.TREATMENT_ID).ToList(); 
                            foreach (var group in Groups)
                            {
                                var listSub = group.ToList<V_HIS_PATIENT_TYPE_ALTER>(); 
                                foreach (var item in listSub)
                                {
                                    if (item.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                                        continue; 
                                    dicPatientTypeAlter[item.TREATMENT_ID] = item; 
                                    break; 
                                }
                            }
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }

                ProcessListTransaction(dicPatientTypeAlter); 
                ProcessTotal(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

        private void ProcessTotal()
        {
            try
            {
                if (listRdo != null && listRdo.Count > 0)
                {
                    totalPrice = listRdo.Sum(o => o.TOTAL_DEPOSIT_AMOUNT) ?? 0; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListTransaction(Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter)
        {
            try
            {
                Dictionary<long, V_HIS_TRANSACTION> dicTransaction = new Dictionary<long, V_HIS_TRANSACTION>(); 
                Dictionary<long, V_HIS_CASHIER_ROOM> dicCashierRoom = new Dictionary<long, V_HIS_CASHIER_ROOM>(); 

                if (!IsNotNullOrEmpty(listCashierRoom))
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc danh sach phong thu ngan: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listCashierRoom), listCashierRoom)); 
                    return; 
                }

                foreach (var item in listCashierRoom)
                {
                    dicCashierRoom[item.ID] = item; 
                }

                if (IsNotNullOrEmpty(listTransaction))
                {
                    foreach (var item in listTransaction)
                    {
                        dicTransaction[item.ID] = item; 
                    }
                }

                if (IsNotNullOrEmpty(listDeposit))
                {
                    foreach (var item in listDeposit)
                    {
                        if (item.IS_CANCEL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue; 

                        if (!dicCashierRoom.ContainsKey(item.CASHIER_ROOM_ID))
                            continue; 

                        if (!(item.TDL_SERE_SERV_DEPOSIT_COUNT.HasValue && item.TDL_SERE_SERV_DEPOSIT_COUNT.Value > 0))
                            continue; 

                        listRdo.Add(new Mrs00343RDO(item)); 
                    }
                }
                listRdo = listRdo.Where(o => o.TOTAL_DEPOSIT_AMOUNT > 0).ToList(); 
                listRdo = listRdo.OrderBy(o => o.CASHIER_USERNAME).ThenBy(t=>t.VIR_PATIENT_NAME).ToList(); 
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
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                string sumPriceStr = string.Format("{0:0.####}", Inventec.Common.Number.Convert.NumberToNumberRoundMax4(totalPrice)); 
                string sumPriceText = Inventec.Common.String.Convert.CurrencyToVneseStringNoUpcase(sumPriceStr); 
                string userName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetUserName(); 

                dicSingleTag.Add("TOTAL_PRICE", totalPrice); 
                dicSingleTag.Add("TOTAL_PRICE_STR", Inventec.Common.String.Convert.UppercaseFirst(sumPriceText)); 
                dicSingleTag.Add("BRANCH_NAME", _Branch.BRANCH_NAME.ToUpper()); 
                dicSingleTag.Add("CURRENT_CREATOR", userName); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Transaction", listRdo); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
