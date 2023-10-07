using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00183
{
    class Mrs00183Processor : AbstractProcessor
    {
        Mrs00183Filter castFilter = null;
        List<Mrs00183RDO> listRdo = new List<Mrs00183RDO>();
        List<Mrs00183RDO> listParentRdo = new List<Mrs00183RDO>();
        HIS_CASHIER_ROOM cashierRoom;
        CommonParam paramGet = new CommonParam();
        public Mrs00183Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        HIS_BRANCH branch = null;
        HIS_PATIENT_TYPE patientType = null;
        string TreatmentTypeNames = "";

        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();

        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_TREATMENT_FEE> dicTreatmentFee = new Dictionary<long, V_HIS_TREATMENT_FEE>();
        Dictionary<long, List<V_HIS_TRANSACTION>> dicTransaction = new Dictionary<long, List<V_HIS_TRANSACTION>>();
        Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>();

        public override Type FilterType()
        {
            return typeof(Mrs00183Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00183Filter)this.reportFilter;
                //phong thu ngan
                if (this.castFilter.CASHIER_ROOM_ID.HasValue)
                {
                    HisCashierRoomFilterQuery filter = new HisCashierRoomFilterQuery();
                    filter.ID = this.castFilter.CASHIER_ROOM_ID;
                    var cr = new HisCashierRoomManager().Get(filter);
                    this.cashierRoom = IsNotNullOrEmpty(cr) ? cr[0] : null;
                }
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00183Filter)this.reportFilter;
                //HSDT khoa vien phi
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                ListTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                var treatmentIds = ListTreatment.Select(o => o.ID).ToList();

                List<V_HIS_TRANSACTION> listTransaction = new List<V_HIS_TRANSACTION>();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    //Giao dich
                    var skip = 0;
                    while (treatmentIds.Count() - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTransactionViewFilterQuery transactionfilter = new HisTransactionViewFilterQuery();
                        transactionfilter.TREATMENT_IDs = limit;
                        var listSub = new HisTransactionManager(paramGet).GetView(transactionfilter);
                        listTransaction.AddRange(listSub);
                    }
                    if (castFilter.CASHIER_LOGINNAME != null && castFilter.CASHIER_LOGINNAME != "")
                    {
                        listTransaction = listTransaction.Where(o => o.CASHIER_LOGINNAME == castFilter.CASHIER_LOGINNAME).ToList();
                    }
                    if (castFilter.LOGINNAME != null && castFilter.LOGINNAME != "")
                    {
                        listTransaction = listTransaction.Where(o => o.CASHIER_LOGINNAME == castFilter.LOGINNAME).ToList();
                    }
                    if (castFilter.CASHIER_ROOM_ID != null && castFilter.CASHIER_ROOM_ID != 0)
                    {
                        listTransaction = listTransaction.Where(o => o.CASHIER_ROOM_ID == castFilter.CASHIER_ROOM_ID).ToList();
                    }
                    dicTransaction = listTransaction.GroupBy(o => o.TREATMENT_ID ?? 0).ToDictionary(q => q.Key, q => q.ToList());
                }

                if (IsNotNullOrEmpty(treatmentIds))
                {
                    //YC_DV
                    var skip = 0;
                    List<V_HIS_SERE_SERV> listSereServ = new List<V_HIS_SERE_SERV>();
                    while (treatmentIds.Count() - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery SSfilter = new HisSereServViewFilterQuery();
                        SSfilter.TREATMENT_IDs = limit;
                        var listSub = new HisSereServManager(paramGet).GetView(SSfilter);
                        listSereServ.AddRange(listSub);
                    }
                    dicSereServ = listSereServ.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).ToDictionary(q => q.Key, q => q.ToList());
                }
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    //Doi tuong
                    dicPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(treatmentIds).OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(g => g.TREATMENT_ID).ToDictionary(s => s.Key, s => s.Last());
                }
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    //chi phí
                    var skip = 0;
                    List<V_HIS_TREATMENT_FEE> listTreatmentfee = new List<V_HIS_TREATMENT_FEE>();
                    while (treatmentIds.Count() - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery();
                        //HisSereServViewFilterQuery SSfilter = new HisSereServViewFilterQuery();
                        //SSfilter.TREATMENT_IDs = limit;
                        feeFilter.IDs = limit;
                        var listSub = new HisTreatmentManager(paramGet).GetFeeView(feeFilter);
                        listTreatmentfee.AddRange(listSub);
                    }
                    dicTreatmentFee = listTreatmentfee.GroupBy(o => o.ID).ToDictionary(q => q.Key, q => q.First());
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
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    foreach (var treatment in ListTreatment)
                    {
                        if (!dicTransaction.ContainsKey(treatment.ID)) continue;
                        if (!dicPatientTypeAlter.ContainsKey(treatment.ID)) continue;

                        if (dicPatientTypeAlter[treatment.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM) continue;
                        Mrs00183RDO rdo = new Mrs00183RDO(treatment);
                        if (dicSereServ.ContainsKey(treatment.ID))
                        {
                            #region Các khoản
                            List<V_HIS_SERE_SERV> hisSereServHein = new List<V_HIS_SERE_SERV>();
                            List<V_HIS_SERE_SERV> hisSereServFee = new List<V_HIS_SERE_SERV>();
                            decimal totalHeinPrice = 0;
                            decimal totalDifference = 0;
                            foreach (var sereServ in dicSereServ[treatment.ID])
                            {
                                if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                                {
                                    hisSereServHein.Add(sereServ);

                                    if (sereServ.HEIN_LIMIT_PRICE.HasValue)
                                    {
                                        //Nếu có limit thì:
                                        //+ tiền chi phí BH <= limit thì lấy tiền chi phí
                                        //+ tiền chi phí BH > limit thì lấy limit. tiền vượt = tiền chi phí trừ tiền limit
                                        if (sereServ.PRICE <= (sereServ.HEIN_LIMIT_PRICE.Value))
                                        {
                                            totalHeinPrice += (sereServ.AMOUNT * sereServ.PRICE * (1 + sereServ.VAT_RATIO));
                                        }
                                        else
                                        {
                                            totalDifference += Math.Abs(sereServ.AMOUNT * (sereServ.PRICE * (1 + sereServ.VAT_RATIO) - sereServ.HEIN_LIMIT_PRICE.Value));
                                            totalHeinPrice += (sereServ.HEIN_LIMIT_PRICE.Value * sereServ.AMOUNT);
                                        }
                                    }
                                    else
                                    {
                                        //Nếu không có limit thì tiền chi phí BH = tiền chi phí
                                        totalHeinPrice += (sereServ.AMOUNT * sereServ.PRICE * (1 + sereServ.VAT_RATIO));
                                    }

                                    if (String.IsNullOrEmpty(rdo.HEIN_CARD_NUMBER) && !String.IsNullOrEmpty(sereServ.HEIN_CARD_NUMBER))
                                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER;
                                }
                                else
                                {
                                    hisSereServFee.Add(sereServ);
                                }

                            }
                            #endregion

                            #region khoản BHYT
                            if (IsNotNullOrEmpty(hisSereServHein))
                            {
                                rdo.TOTAL_HEIN_PRICE = totalHeinPrice;
                                rdo.TOTAL_HEIN_LIMIT_PRICE = hisSereServHein.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);

                                if (rdo.TOTAL_HEIN_PRICE > 0 && rdo.TOTAL_HEIN_LIMIT_PRICE > 0)
                                {
                                    var subPrice = rdo.TOTAL_HEIN_PRICE - rdo.TOTAL_HEIN_LIMIT_PRICE;
                                    if (subPrice > 0)
                                    {
                                        rdo.TOTAL_HEIN_PATIENT_PRICE = subPrice;
                                    }
                                }
                                rdo.TOTAL_DIFFERENCE_PRICE = totalDifference;
                            }
                            #endregion

                            #region Khoản phí
                            if (IsNotNullOrEmpty(hisSereServFee))
                            {
                                var feePrice = hisSereServFee.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                                if (feePrice > 0)
                                {
                                    rdo.TOTAL_FEE_PATIENT_PRICE = feePrice;
                                }
                            }
                            #endregion
                        }
                        if (dicTreatmentFee.ContainsKey(treatment.ID))
                        {
                            #region chi phí
                            if (dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.HasValue && dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.Value > 0)
                            {
                                rdo.VIR_TOTAL_PATIENT_PRICE = dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.HasValue ? dicTreatmentFee[treatment.ID].TOTAL_PATIENT_PRICE.Value : 0;
                            }
                            if (dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.HasValue && dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.Value > 0)
                            {
                                rdo.TOTAL_BILL_FUND = dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.HasValue ? dicTreatmentFee[treatment.ID].TOTAL_BILL_FUND.Value : 0;
                            }

                            #endregion
                        }


                        #region tam ung, hoan ung, thanh toan
                        foreach (var transaction in dicTransaction[treatment.ID])
                        {
                            if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU)
                            {
                                if (transaction.TDL_SERE_SERV_DEPOSIT_COUNT == 0 ||
                                    transaction.TDL_SERE_SERV_DEPOSIT_COUNT == null)
                                {
                                    rdo.TOTAL_DEPOSIT_AMOUNT += transaction.AMOUNT;
                                }
                                else
                                {
                                    rdo.TOTAL_BILL_EXAM_AMOUNT += transaction.AMOUNT;
                                }
                            }
                            else
                                if (transaction.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU)
                                {
                                    if (transaction.TDL_SESE_DEPO_REPAY_COUNT == 0 ||
                                        transaction.TDL_SESE_DEPO_REPAY_COUNT == null)
                                    {
                                        rdo.TOTAL_DEPOSIT_AMOUNT -= transaction.AMOUNT;
                                    }
                                    else
                                    {
                                        rdo.TOTAL_BILL_EXAM_AMOUNT -= transaction.AMOUNT;
                                    }

                                }
                        }
                        #endregion

                        var residual_Amount = rdo.TOTAL_DEPOSIT_AMOUNT + rdo.TOTAL_BILL_FUND + rdo.TOTAL_BILL_EXAM_AMOUNT - rdo.VIR_TOTAL_PATIENT_PRICE;
                        if (residual_Amount > 0)
                        {
                            rdo.TOTAL_WITHDRAW_AMOUNT = residual_Amount;
                        }
                        else if (residual_Amount < 0)
                        {
                            rdo.TOTAL_PATIENT_AMOUNT = Math.Abs(residual_Amount);
                        }
                        listRdo.Add(rdo);
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("FEE_LOCK_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                if (cashierRoom != null)
                {
                    dicSingleTag.Add("CASHIER_ROOM_CODE", cashierRoom.CASHIER_ROOM_CODE);
                    dicSingleTag.Add("CASHIER_ROOM_NAME", cashierRoom.CASHIER_ROOM_NAME);
                }

                listRdo = listRdo.OrderBy(o => o.IN_TIME).ThenBy(t => t.OUT_TIME).ToList();

                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
