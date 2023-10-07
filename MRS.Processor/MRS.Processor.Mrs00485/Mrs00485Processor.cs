using MOS.MANAGER.HisService;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00485
{
    class Mrs00485Processor : AbstractProcessor
    {
        Mrs00485Filter castFilter = null;
        List<Mrs00485RDO> listRdo = new List<Mrs00485RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_TRANSACTION> listTransactions = new List<V_HIS_TRANSACTION>();
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>();

        List<long> listServiceType01s = new List<long>();
        List<long> listServiceType02s = new List<long>();
        List<long> listServiceType03s = new List<long>();
        List<long> listServiceType04s = new List<long>();
        List<long> listServiceType05s = new List<long>();
        List<long> listServiceType06s = new List<long>();
        List<long> listServiceType07s = new List<long>();
        List<long> listServiceType08s = new List<long>();
        List<long> listServiceType09s = new List<long>();
        List<long> listServiceType10s = new List<long>();

        List<long> listServiceType11s = new List<long>();
        List<long> listServiceType12s = new List<long>();
        List<long> listServiceType13s = new List<long>();
        List<long> listServiceType14s = new List<long>();
        List<long> listServiceType15s = new List<long>();
        List<long> listServiceType16s = new List<long>();
        List<long> listServiceType17s = new List<long>();
        List<long> listServiceType18s = new List<long>();
        List<long> listServiceType19s = new List<long>();
        List<long> listServiceType20s = new List<long>();

        public Mrs00485Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00485Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00485Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => reportFilter), reportFilter));
                // cấu hình nhóm dịch vụ
                #region
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery();
                reportTypeCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00485";
                var listreportTypeCats = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param).Get(reportTypeCatFilter);

                HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatViewFilter.REPORT_TYPE_CODE__EXACT = "MRS00485";
                var listRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter);

                for (var i = 1; i <= 20; i++)
                {
                    try
                    {
                        switch (i)
                        {
                            case 1:
                                listServiceType01s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 1).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 2:
                                listServiceType02s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 2).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 3:
                                listServiceType03s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 3).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 4:
                                listServiceType04s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 4).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 5:
                                listServiceType05s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 5).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 6:
                                listServiceType06s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 6).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 7:
                                listServiceType07s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 7).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 8:
                                listServiceType08s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 8).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 9:
                                listServiceType09s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 9).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 10:
                                listServiceType10s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 10).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 11:
                                listServiceType11s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 11).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 12:
                                listServiceType12s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 12).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 13:
                                listServiceType13s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 13).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 14:
                                listServiceType14s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 14).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 15:
                                listServiceType15s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 15).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 16:
                                listServiceType16s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 16).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 17:
                                listServiceType17s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 17).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 18:
                                listServiceType18s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 18).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 19:
                                listServiceType19s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 19).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                            case 20:
                                listServiceType20s = listRetyCats.Where(w => listreportTypeCats.Where(ww => ww.NUM_ORDER == 20).Select(s => s.ID).Contains(w.REPORT_TYPE_CAT_ID)).Select(s => s.SERVICE_ID).ToList();
                                break;
                        }
                    }
                    catch { Inventec.Common.Logging.LogSystem.Error("Lỗi không thấy nhóm."); }
                }
                #endregion
                // danh sách giao dịch + dịch vụ
                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                treatmentViewFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentViewFilter.OUT_TIME_TO = castFilter.TIME_TO;
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter);

                //listTreatments = listTreatments.Where(w => w.CLINICAL_IN_TIME != null).ToList(); 

                var skip = 0;
                while (listTreatments.Count - skip > 0)
                {
                    var listIDs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTransactionViewFilterQuery transactionViewFilter = new HisTransactionViewFilterQuery();
                    transactionViewFilter.TREATMENT_IDs = listIDs.Select(s => s.ID).ToList();
                    transactionViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    transactionViewFilter.IS_CANCEL = false;
                    transactionViewFilter.HAS_SALL_TYPE = false;
                    listTransactions.AddRange(new MOS.MANAGER.HisTransaction.HisTransactionManager(param).GetView(transactionViewFilter));

                    HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery();
                    treatmentFeeViewFilter.PATIENT_IDs = listIDs.Select(s => s.PATIENT_ID).ToList();
                    listTreatmentFees.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetFeeView(treatmentFeeViewFilter));

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                    sereServViewFilter.TREATMENT_IDs = listIDs.Select(s => s.ID).ToList();
                    sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter));
                }

                //listTreatmentFees = listTreatmentFees.Where(w => listTreatments.Select(s => s.TREATMENT_CODE).Contains(w.TREATMENT_CODE)).ToList(); 
                listTransactions = listTransactions.Where(w => w.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE).ToList();
                listSereServs = listSereServs.Where(w => w.IS_DELETE != IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE && w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        /// <summary>
        /// V_HIS_TREATMENT_FEE:
        /// số tiêng bệnh nhân phải trả: TOTAL_PATIENT_PRICE
        /// số tiền BN đã trả: TOTAL_BILL_AMOUNT
        /// kết chuyển từ tạm ứng sang thanh toán: TOTAL_BILL_TRANFER_AMOUNT
        /// </summary>
        /// <returns></returns>
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                foreach (var treatment in listTreatments)
                {
                    var rdo = new Mrs00485RDO();

                    rdo.TREATMENT = treatment;
                    var treatmentFEE = listTreatmentFees.Where(w => w.TREATMENT_CODE == (treatment.TREATMENT_CODE)).ToList();
                    if (IsNotNullOrEmpty(treatmentFEE))
                    {
                        rdo.TREATMENT_FEE = treatmentFEE.First();
                        if (rdo.TREATMENT_FEE.TOTAL_PATIENT_PRICE != null && rdo.TREATMENT_FEE.TOTAL_PATIENT_PRICE > 0)
                        {
                            if (rdo.TREATMENT_FEE.TOTAL_DEPOSIT_AMOUNT != null && rdo.TREATMENT_FEE.TOTAL_DEPOSIT_AMOUNT > rdo.TREATMENT_FEE.TOTAL_BILL_AMOUNT)
                                rdo.SHORT = (rdo.TREATMENT_FEE.TOTAL_DEPOSIT_AMOUNT ?? 0) - (rdo.TREATMENT_FEE.TOTAL_BILL_AMOUNT ?? 0);
                            else
                                rdo.REDUNDANCY = (rdo.TREATMENT_FEE.TOTAL_BILL_AMOUNT ?? 0) - (rdo.TREATMENT_FEE.TOTAL_DEPOSIT_AMOUNT ?? 0);
                            var transactions = listTransactions.Where(w => w.TREATMENT_ID == treatment.ID).ToList();
                            if (IsNotNullOrEmpty(transactions))
                            {
                                var transCode = transactions.Where(w => w.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).OrderByDescending(w => w.TRANSACTION_TIME).ToList();
                                if (IsNotNullOrEmpty(transCode))
                                {
                                    rdo.TRANSACTION_CODE = transCode.First().TRANSACTION_CODE;
                                    rdo.TRANSACTION_TIME = transCode.First().TRANSACTION_TIME;
                                }
                            }
                            rdo.HEIN_05 = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID && w.HEIN_RATIO == (decimal)0.95).Sum(s => s.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.HEIN_20 = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID && w.HEIN_RATIO == (decimal)0.80).Sum(s => s.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.HEIN_40 = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID && w.HEIN_RATIO == (decimal)0.60).Sum(s => s.VIR_TOTAL_PATIENT_PRICE) ?? 0;

                            var listSereServNoHeins = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID && (w.HEIN_RATIO != (decimal)0.95 || w.HEIN_RATIO != (decimal)0.80 || w.HEIN_RATIO != (decimal)0.60)).ToList();

                            rdo.SERVICE_TYPE_01 = listSereServNoHeins.Where(w => listServiceType01s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_02 = listSereServNoHeins.Where(w => listServiceType02s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_03 = listSereServNoHeins.Where(w => listServiceType03s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_04 = listSereServNoHeins.Where(w => listServiceType04s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_05 = listSereServNoHeins.Where(w => listServiceType05s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_06 = listSereServNoHeins.Where(w => listServiceType06s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_07 = listSereServNoHeins.Where(w => listServiceType07s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_08 = listSereServNoHeins.Where(w => listServiceType08s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_09 = listSereServNoHeins.Where(w => listServiceType09s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_10 = listSereServNoHeins.Where(w => listServiceType10s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;

                            rdo.SERVICE_TYPE_11 = listSereServNoHeins.Where(w => listServiceType11s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_12 = listSereServNoHeins.Where(w => listServiceType12s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_13 = listSereServNoHeins.Where(w => listServiceType13s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_14 = listSereServNoHeins.Where(w => listServiceType14s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_15 = listSereServNoHeins.Where(w => listServiceType15s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_16 = listSereServNoHeins.Where(w => listServiceType16s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_17 = listSereServNoHeins.Where(w => listServiceType17s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_18 = listSereServNoHeins.Where(w => listServiceType18s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_19 = listSereServNoHeins.Where(w => listServiceType19s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;
                            rdo.SERVICE_TYPE_20 = listSereServNoHeins.Where(w => listServiceType20s.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE) ?? 0;

                            listRdo.Add(rdo);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex);
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
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.TREATMENT.TREATMENT_CODE).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
