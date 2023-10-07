using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisSereServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00078
{
    class Mrs00078Processor : AbstractProcessor
    {
        Mrs00078Filter castFilter = null;

        List<Mrs00078RDO> ListRdo = new List<Mrs00078RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApprovalBhyt = new List<V_HIS_HEIN_APPROVAL>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        List<long> listHeinServiceTypeId;

        HIS_BRANCH _Branch = null;

        public Mrs00078Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00078Filter);
        }
        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()//asd;flksjdfoiaeroj///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00078Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu bao cao MRS00078: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => this.castFilter), this.castFilter));
                CommonParam paramGet = new CommonParam();


                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ACS";

                ListHeinApprovalBhyt = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager().GetView(approvalFilter);

                ListMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00078");
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
                if (IsNotNullOrEmpty(ListHeinApprovalBhyt))
                {
                    if (IsNotNullOrEmpty(ListMedicineType))
                    {
                        foreach (var medicine in ListMedicineType)
                        {
                            dicMedicineType[medicine.ID] = medicine;
                        }
                    }
                    CommonParam paramGet = new CommonParam();
                    listHeinServiceTypeId = new List<long>()
                    {
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM,
                    IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL
                    };

                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM);
                    }
                    if (HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    int start = 0;
                    int count = ListHeinApprovalBhyt.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts = ListHeinApprovalBhyt.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = heinApprovalBhyts.Select(s => s.ID).ToList();
                        var ListSereServ = new HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00078");
                        }
                        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();

                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = heinApprovalBhyts.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        treatments = new HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        ProcessListHeinApprovalDetail(heinApprovalBhyts, ListSereServ,treatments);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    ListRdo = ProcessListRDO();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo = new List<Mrs00078RDO>();
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> heinApprovalBhyts, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> treatments)
        {
            Dictionary<long, V_HIS_HEIN_APPROVAL> dicHeinApproval = new Dictionary<long, V_HIS_HEIN_APPROVAL>();

            if (IsNotNullOrEmpty(heinApprovalBhyts))
            {
                foreach (var item in heinApprovalBhyts)
                {
                    dicHeinApproval[item.ID] = item;
                }
            }

            if (IsNotNullOrEmpty(ListSereServ))
            {
                foreach (var item in ListSereServ)
                {
                    if (item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE
                        || item.AMOUNT <= 0 || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.HEIN_APPROVAL_ID == null)
                        continue;
                    if (listHeinServiceTypeId.Contains(item.TDL_HEIN_SERVICE_TYPE_ID.Value) && dicHeinApproval.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                    {
                        var heinApproval = dicHeinApproval[item.HEIN_APPROVAL_ID.Value];
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        Mrs00078RDO rdo = new Mrs00078RDO();
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.MEDICINE_SODANGKY_NAME = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.REGISTER_NUMBER = item.REGISTER_NUMBER;
                        rdo.MEDICINE_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.MEDICINE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MEDICINE_HAMLUONG_NAME = item.MEDICINE_TYPE_CONCENTRA;
                        rdo.TOTAL_HEIN_PRICE = Math.Round(item.ORIGINAL_PRICE * (1 + item.VAT_RATIO), 2);
                        rdo.BHYT_PAY_RATE = Math.Round(item.ORIGINAL_PRICE > 0 ? (item.HEIN_LIMIT_PRICE.HasValue ? (item.HEIN_LIMIT_PRICE.Value / (item.ORIGINAL_PRICE * (1 + item.VAT_RATIO))) * 100 : (item.PRICE / item.ORIGINAL_PRICE) * 100) : 0, 0);
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                        {
                            rdo.AMOUNT_NOITRU = item.AMOUNT;
                        }
                        else if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                        {
                            rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                        }
                        rdo.TOTAL_AMOUNT = item.AMOUNT;
                        rdo.TOTAL_PRICE = Math.Round(Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,item, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH()), 2);
                        var treatment = treatments.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID) ?? new V_HIS_TREATMENT();
                        if (checkBhytNsd(treatment))
                        {
                            rdo.TOTAL_HEIN_PRICE_NDS = item.VIR_TOTAL_HEIN_PRICE ?? 0;
                        }
                        else
                        {
                            rdo.VIR_TOTAL_HEIN_PRICE = item.VIR_TOTAL_HEIN_PRICE ?? 0;
                        }
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (item.OTHER_SOURCE_PRICE ?? 0) * item.AMOUNT;

                        //if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                        //{
                        //    rdo.MedicineLineId = (long)1;
                        //    ListRdo.Add(rdo);
                        //}
                        //else 
                        if (item.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            V_HIS_MEDICINE_TYPE medicineType = null;
                            if (item.MEDICINE_TYPE_ID.HasValue && dicMedicineType.ContainsKey(item.MEDICINE_TYPE_ID.Value))
                            {
                                medicineType = dicMedicineType[item.MEDICINE_TYPE_ID.Value];
                            }
                            if (IsNotNull(medicineType) && medicineType.MEDICINE_LINE_ID.HasValue)
                            {
                                rdo.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;
                                rdo.MEDICINE_CODE_DMBYT = medicineType.ACTIVE_INGR_BHYT_CODE;
                                rdo.MEDICINE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                rdo.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                rdo.MEDICINE_DUONGDUNG_CODE = medicineType.MEDICINE_USE_FORM_CODE;
                                if (medicineType.MEDICINE_LINE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                                {
                                    rdo.MedicineLineId = (long)1;
                                }
                                else if (medicineType.MEDICINE_LINE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                {
                                    rdo.MedicineLineId = (long)2;
                                }
                                else if (medicineType.MEDICINE_LINE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                {
                                    rdo.MedicineLineId = (long)3;
                                }
                            }
                            if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                            {
                                rdo.MedicineLineId = (long)1;
                            }
                            ListRdo.Add(rdo);
                        }
                        else
                        {
                            rdo.MedicineLineId = (long)4;
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
        }

        private bool checkBhytNsd(V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(treatment.ICD_CODE ?? ""))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                {
                    if ((treatment.TDL_HEIN_CARD_NUMBER ?? "  ").Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains((treatment.ICD_CODE ?? "   ").Substring(0, 3)))
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

        private List<Mrs00078RDO> ProcessListRDO()
        {
            List<Mrs00078RDO> listCurrent = new List<Mrs00078RDO>();
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    var groupRDOs = ListRdo.GroupBy(o => new { o.MEDICINE_CODE_DMBYT, o.TOTAL_HEIN_PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00078RDO> listsub = group.ToList<Mrs00078RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00078RDO rdo = new Mrs00078RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.REGISTER_NUMBER = listsub[0].REGISTER_NUMBER;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_DUONGDUNG_CODE = listsub[0].MEDICINE_DUONGDUNG_CODE;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            rdo.MedicineLineId = listsub[0].MedicineLineId;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.TOTAL_PRICE += item.TOTAL_PRICE;
                                rdo.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                                rdo.VIR_TOTAL_HEIN_PRICE += item.VIR_TOTAL_HEIN_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            rdo.TOTAL_AMOUNT = listsub.Sum(x => x.TOTAL_AMOUNT);
                            listCurrent.Add(rdo);
                            //if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            //{
                            //    listCurrent.Add(rdo);
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.MedicineLineId).ThenBy(o => o.MEDICINE_STT_DMBYT).ToList();
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

                Inventec.Common.FlexCellExport.ProcessObjectTag objTag = new Inventec.Common.FlexCellExport.ProcessObjectTag();
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
