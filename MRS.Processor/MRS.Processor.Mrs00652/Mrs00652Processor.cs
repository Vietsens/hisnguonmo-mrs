using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisOtherPaySource;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00652
{
    class Mrs00652Processor : AbstractProcessor
    {
        Mrs00652Filter castFilter = null;
        List<Mrs00652RDO> ListRdoARV = new List<Mrs00652RDO>();
        List<Mrs00652RDO> ListRdoA = new List<Mrs00652RDO>();
        List<Mrs00652RDO> ListRdoB = new List<Mrs00652RDO>();
        List<Mrs00652RDO> ListRdoC = new List<Mrs00652RDO>();
        List<Mrs00652RDO> ListSumTotal = new List<Mrs00652RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        
        Dictionary<long, V_HIS_TREATMENT> DicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
        HIS_BRANCH _Branch = null;
        short HasSereServ = 0;
        private decimal TotalAmount = 0;
        string MaterialPriceOption = "";
        PropertyInfo[] pRdo = null;
        PropertyInfo[] pOtherSourcePrice = null;
        List<HIS_OTHER_PAY_SOURCE> listOtherPaySource = new List<HIS_OTHER_PAY_SOURCE>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();

        public Mrs00652Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00652Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                castFilter = ((Mrs00652Filter)this.reportFilter);
                pRdo = Properties.Get<Mrs00652RDO>();
                pOtherSourcePrice = pRdo.Where(o => o.Name.StartsWith("TOTAL_OTHER_SOURCE_PRICE_")).ToArray();
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;

                
                
                List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();

                if (castFilter.TIME_TYPE.HasValue)
                {
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    if (castFilter.TIME_TYPE.Value == 0)
                    {
                        treatmentFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFilter.IN_TIME_TO = castFilter.TIME_TO;
                    }
                    else if (castFilter.TIME_TYPE.Value == 1)
                    {
                        treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                    }
                    else
                    {
                        treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                        treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                    }

                    treatmentFilter.IS_PAUSE = true;
                    treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                    treatmentFilter.END_ROOM_IDs = castFilter.END_ROOM_IDs;
                    listTreatment = new HisTreatmentManager().GetView(treatmentFilter);

                    ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
                    int skip = 0;
                    while (listTreatment.Count - skip > 0)
                    {
                        var listId = listTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                        approvalFilter.TREATMENT_IDs = listId.Select(s => s.ID).ToList();
                        approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                        approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                        approvalFilter.ORDER_DIRECTION = "ASC";
                        var hein = new HisHeinApprovalManager().GetView(approvalFilter);
                        if (IsNotNullOrEmpty(hein))
                        {
                            ListHeinApproval.AddRange(hein);
                        }
                    }
                }
                else
                {
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                    approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager().GetView(approvalFilter);

                    if (IsNotNullOrEmpty(ListHeinApproval))
                    {
                        var treatmentIds = ListHeinApproval.Select(s => s.TREATMENT_ID).Distinct().ToList();

                        int skip = 0;
                        while (treatmentIds.Count - skip > 0)
                        {
                            var listId = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisTreatmentViewFilterQuery TreatmentFilter = new HisTreatmentViewFilterQuery();
                            TreatmentFilter.IDs = listId;
                            TreatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                            TreatmentFilter.END_ROOM_IDs = castFilter.END_ROOM_IDs;
                            var treatment = new HisTreatmentManager().GetView(TreatmentFilter);
                            if (IsNotNullOrEmpty(treatment))
                            {
                                listTreatment.AddRange(treatment);
                            }
                        }

                    }
                    ListHeinApproval = ListHeinApproval.Where(o => listTreatment.Exists(p => p.ID == o.TREATMENT_ID)).ToList();
                }

                if (IsNotNullOrEmpty(listTreatment))
                {
                    DicTreatment = listTreatment.ToDictionary(o => o.ID, o => o);
                }

                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.ORDER_DIRECTION = "DESC";
                serviceRetyCatFilter.ORDER_FIELD = "ID";
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = "MRS00652";
                this.listServiceRetyCat = new HisServiceRetyCatManager().GetView(serviceRetyCatFilter)?? new List<V_HIS_SERVICE_RETY_CAT>();

                HisOtherPaySourceFilterQuery otherPaySourceFilter = new HisOtherPaySourceFilterQuery();
                otherPaySourceFilter.ORDER_DIRECTION = "ASC";
                otherPaySourceFilter.ORDER_FIELD = "OTHER_PAY_SOURCE_CODE";
                this.listOtherPaySource = new HisOtherPaySourceManager().Get(otherPaySourceFilter);

                Inventec.Common.Logging.LogSystem.Info("ListHeinApproval" + ListHeinApproval.Count);
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
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    //lọc theo diện điều trị
                    FilterTreatmentType();

                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();

                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        var ListSereServ = new HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00652");
                        }
                        GeneralDataForCategoryCode(ListSereServ, listServiceRetyCat);
                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, DicTreatment);
                        
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
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

        private void FilterTreatmentType()
        {
            if (castFilter.TREATMENT_TYPE_IDs != null)
            {
                ListHeinApproval = ListHeinApproval.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TREATMENT_TYPE_ID)).ToList();
            }
        }

        private void GeneralDataForCategoryCode(List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat)
        {
            try
            {
                var listSSMedicine = ListSereServ.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC);
                if (listSSMedicine != null)
                {
                    var group = listSSMedicine.GroupBy(p => new { p.TDL_HEIN_SERVICE_BHYT_CODE, p.PRICE }).ToList();
                    foreach (var item in group)
                    {
                        
                        List<V_HIS_SERE_SERV_3> listSub = item.ToList<V_HIS_SERE_SERV_3>();
                        var serviceRety = listServiceRetyCat.FirstOrDefault(p => p.SERVICE_ID == listSub[0].SERVICE_ID);
                        Mrs00652RDO rdo = new Mrs00652RDO();
                        rdo.HEIN_SERVICE_CODE = listSub[0].TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_NAME = listSub[0].TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.CATEGORY_CODE = serviceRety.CATEGORY_CODE == "ARV" ? serviceRety.CATEGORY_CODE : "";
                        rdo.HEIN_RATIO = listSub[0].HEIN_RATIO * 100;
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].PRICE * (1 + listSub[0].VAT_RATIO);
                        rdo.TOTAL_PRICE = rdo.AMOUNT * rdo.PRICE;
                        rdo.TOTAL_HEIN_PRICE = listSub.Sum(p => p.VIR_TOTAL_HEIN_PRICE ?? 0);
                        ListRdoARV.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, Dictionary<long, V_HIS_TREATMENT> dicTreatment)
        {
            try
            {
                Inventec.Common.Logging.LogSystem.Info("hisHeinApprovals.Count  " + hisHeinApprovals.Count.ToString());
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                    Inventec.Common.Logging.LogSystem.Info("ListSereServ  " + ListSereServ.Count.ToString());
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var sere in ListSereServ)
                        {
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0 && sere.HEIN_APPROVAL_ID.HasValue)
                            {
                                if (!dicSereServ.ContainsKey(sere.HEIN_APPROVAL_ID.Value))
                                    dicSereServ[sere.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[sere.HEIN_APPROVAL_ID.Value].Add(sere);
                            }
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        
                        if (dicSereServ.ContainsKey(heinApproval.ID))
                        {
                            var existsedRdoA = ListRdoA.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE && (o.HEIN_CARD_NUMBER == heinApproval.HEIN_CARD_NUMBER || this.castFilter.IS_MERGE_TREATMENT == true));
                            var existsedRdoB = ListRdoB.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE && (o.HEIN_CARD_NUMBER == heinApproval.HEIN_CARD_NUMBER || this.castFilter.IS_MERGE_TREATMENT == true));
                            var existsedRdoC = ListRdoC.FirstOrDefault(o => o.TREATMENT_CODE == heinApproval.TREATMENT_CODE && (o.HEIN_CARD_NUMBER == heinApproval.HEIN_CARD_NUMBER || this.castFilter.IS_MERGE_TREATMENT == true));
                            if (existsedRdoA != null || existsedRdoB != null || existsedRdoC != null)
                            {
                                if (existsedRdoA != null)
                                {
                                    int id = ListRdoA.IndexOf(existsedRdoA);
                                    ProcessTotalPrice(ListRdoA[id], dicSereServ[heinApproval.ID]);
                                }
                                else if (existsedRdoB != null)
                                {
                                    int id = ListRdoB.IndexOf(existsedRdoB);
                                    ProcessTotalPrice(ListRdoB[id], dicSereServ[heinApproval.ID]);
                                }
                                else if (existsedRdoC != null)
                                {
                                    int id = ListRdoC.IndexOf(existsedRdoC);
                                    ProcessTotalPrice(ListRdoC[id], dicSereServ[heinApproval.ID]);
                                }
                            }
                            else
                            {
                                Mrs00652RDO rdo = new Mrs00652RDO(heinApproval);
                                if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                                {
                                    var treatment = dicTreatment[heinApproval.TREATMENT_ID];

                                    rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                    rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME);
                                    rdo.IN_TIME = treatment.IN_TIME;
                                    rdo.CLOSE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0);
                                    rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                                    rdo.END_DEPARTMENT_CODE = treatment.END_DEPARTMENT_CODE;
                                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                                    {
                                        rdo.END_ROOM_DEPARTMENT_NAME = treatment.END_ROOM_NAME;
                                    }
                                    else
                                    {
                                        rdo.END_ROOM_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                                    }

                                    rdo.TOTAL_DATE = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT);
                                }

                                //Inventec.Common.Logging.LogSystem.Info("heinApproval.TREATMENT_ID  " + heinApproval.TREATMENT_ID);
                                HasSereServ = 0;
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID]);
                                if (HasSereServ == 0)
                                {
                                    continue;
                                }
                                //khong co gia thi bo qua
                                if (!CheckPrice(rdo)) continue;

                                if ((this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(rdo.HEIN_APPROVAL.HEIN_MEDI_ORG_CODE)
                                    && checkBhytProvinceCode((this.castFilter.IS_PROVINCE_FROM_MEDI_ORG==true)?string.Format("123{0}9ABCDEF",rdo.HEIN_APPROVAL.HEIN_MEDI_ORG_CODE): rdo.HEIN_APPROVAL.HEIN_CARD_NUMBER))
                                {
                                    ListRdoA.Add(rdo);
                                }
                                else if (checkBhytProvinceCode((this.castFilter.IS_PROVINCE_FROM_MEDI_ORG==true) ? string.Format("123{0}9ABCDEF", rdo.HEIN_APPROVAL.HEIN_MEDI_ORG_CODE) : rdo.HEIN_APPROVAL.HEIN_CARD_NUMBER))
                                {
                                   ListRdoB.Add(rdo);
                                }
                                else
                                {
                                    ListRdoC.Add(rdo);
                                }
                            }
                        }
                    }

                    Inventec.Common.Logging.LogSystem.Info("ListRdoA" + ListRdoA.Count);
                    Inventec.Common.Logging.LogSystem.Info("ListRdoB" + ListRdoB.Count);
                    Inventec.Common.Logging.LogSystem.Info("ListRdoC" + ListRdoC.Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA.Clear();
                ListRdoB.Clear();
                ListRdoC.Clear();
            }
        }

        private void ProcessTotalPrice(Mrs00652RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                rdo.DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
                rdo.DIC_TOTAL_HEIN_PRICE = new Dictionary<string, decimal>();
                rdo.DIC_TOTAL_OTHER_SOURCE_PRICE = new Dictionary<string, decimal>();
                foreach (var sereServ in hisSereServs)
                {
                    if (castFilter.DEPARTMENT_ID != null)
                    {
                        if ((sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH && sereServ.TDL_HEIN_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT && sereServ.TDL_REQUEST_DEPARTMENT_ID != castFilter.DEPARTMENT_ID)
                            ||
                            ((sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT) && sereServ.TDL_EXECUTE_DEPARTMENT_ID != castFilter.DEPARTMENT_ID))
                        {
                            continue;
                        }
                    }
                    HasSereServ = 1;
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;

                    var branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == rdo.HEIN_APPROVAL.BRANCH_ID) ?? new HIS_BRANCH();
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, branch, MaterialPriceOption == "1");

                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            rdo.TEST_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            rdo.DIIM_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM)
                        {
                            rdo.MEDICINE_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
                        {
                            rdo.BLOOD_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                        {
                            rdo.SURGMISU_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM)
                        {
                            rdo.MATERIAL_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                        {
                            rdo.MATERIAL_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT)
                        {
                            rdo.MEDICINE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            rdo.BED_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            rdo.SERVICE_PRICE_RATIO += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            rdo.TRAN_PRICE += TotalPriceTreatment;
                        }
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TT)
                        {
                            rdo.TT_PRICE += TotalPriceTreatment;
                        }

                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                            rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        if(this.IsMedicineTypeHIV(sereServ.TDL_SERVICE_CODE) &&(sereServ.TDL_HEIN_SERVICE_TYPE_ID ==IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM|| sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM) ||this.IsIcdHIV(rdo.ICD_CODE_MAIN))
                        {
                            rdo.TOTAL_BHVN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        }  

                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0)*sereServ.AMOUNT;

                        this.ProcessorOtherSourcePrice(sereServ.OTHER_PAY_SOURCE_ID, (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT, ref rdo);

                        //var tyle = Math.Round(sereServ.ORIGINAL_PRICE > 0 ? (sereServ.HEIN_LIMIT_PRICE.HasValue ? (sereServ.HEIN_LIMIT_PRICE.Value / (sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO))) : (sereServ.PRICE / sereServ.ORIGINAL_PRICE)) : 0, 0);

                        //decimal chenhLechVatTu = 0;
                        //var listHeinServiceTypeMate = new List<long> { IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM, IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM, IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL, IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT };
                        //if (MaterialPriceOption == "1" && listHeinServiceTypeMate.Contains(sereServ.TDL_HEIN_SERVICE_TYPE_ID ?? 0))
                        //{
                        //    chenhLechVatTu = (sereServ.PRICE - sereServ.ORIGINAL_PRICE) * (1 + sereServ.VAT_RATIO) * sereServ.AMOUNT;
                        //}

                        //decimal bncct = 0;
                        //if ((sereServ.TDL_HST_BHYT_CODE == "15" /*&& (xml3.TyLeTT == 50 || xml3.TyLeTT == 30)*/)
                        //    || (sereServ.TDL_HST_BHYT_CODE == "13" /*&& (xml3.TyLeTT == 30 || xml3.TyLeTT == 10)*/)
                        //    || (sereServ.TDL_HST_BHYT_CODE == "8" && ((tyle * 100) == 50 || (tyle * 100) == 80))
                        //    || branch.HEIN_LEVEL_CODE == MOS.LibraryHein.Bhyt.HeinLevel.HeinLevelCode.COMMUNE)
                        //{
                        //    bncct = TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        //}
                        //else
                        //{
                        //    bncct = (TotalPriceTreatment - chenhLechVatTu) * tyle - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        //}

                        rdo.TOTAL_PATIENT_PRICE_CCT += (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        rdo.TOTAL_PATIENT_PRICE_TT += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0) - (sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                        var serviceRetyCat = listServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == sereServ.SERVICE_ID);
                        if (serviceRetyCat != null)
                        {
                            if (!rdo.DIC_TOTAL_PRICE.ContainsKey(serviceRetyCat.CATEGORY_CODE))
                            {
                                rdo.DIC_TOTAL_PRICE[serviceRetyCat.CATEGORY_CODE] = TotalPriceTreatment;
                            }
                            else
                            {
                                rdo.DIC_TOTAL_PRICE[serviceRetyCat.CATEGORY_CODE] += TotalPriceTreatment;
                            }
                            if (!rdo.DIC_TOTAL_HEIN_PRICE.ContainsKey(serviceRetyCat.CATEGORY_CODE))
                            {
                                rdo.DIC_TOTAL_HEIN_PRICE[serviceRetyCat.CATEGORY_CODE] = sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            else
                            {
                                rdo.DIC_TOTAL_HEIN_PRICE[serviceRetyCat.CATEGORY_CODE] += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            }
                            if (!rdo.DIC_TOTAL_OTHER_SOURCE_PRICE.ContainsKey(serviceRetyCat.CATEGORY_CODE))
                            {
                                rdo.DIC_TOTAL_OTHER_SOURCE_PRICE[serviceRetyCat.CATEGORY_CODE] = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.DIC_TOTAL_OTHER_SOURCE_PRICE[serviceRetyCat.CATEGORY_CODE] += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                            }
                        }
                    }
                }

                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }

                TotalAmount += rdo.TOTAL_HEIN_PRICE;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool IsMedicineTypeHIV(string medicineTypeCode)
        {
            //Inventec.Common.Logging.LogSystem.Info(string.Format("Ma thuoc loc: {0},", this.castFilter.MEDICINE_TYPE_CODE__HIVs)+string.Format("Ma thuoc chon: {0},", medicineTypeCode));
            if (string.IsNullOrWhiteSpace(this.castFilter.MEDICINE_TYPE_CODE__HIVs))
                return false;
            return !string.IsNullOrWhiteSpace(medicineTypeCode) && string.Format(",{0},", this.castFilter.MEDICINE_TYPE_CODE__HIVs).Contains(string.Format(",{0},", medicineTypeCode));
        }

        private bool IsIcdHIV(string IcdCode)
        {
            if (string.IsNullOrWhiteSpace(this.castFilter.ICD_CODE__HIVs))
                return false;
            return !string.IsNullOrWhiteSpace(IcdCode) && string.Format(",{0},", this.castFilter.ICD_CODE__HIVs).Contains(string.Format(",{0},", IcdCode));
        }

        private void ProcessorOtherSourcePrice(long? _OtherPaySourceId, decimal OtherSourcePrice, ref Mrs00652RDO rdo)
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

        private bool CheckPrice(Mrs00652RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TT_PRICE > 0 || rdo.TRAN_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
                {
                    string provinceCode = HeinNumber.Substring(3, 2);
                    if (this._Branch.HEIN_PROVINCE_CODE.Equals(provinceCode))
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

        private bool checkBhytNsd(Mrs00652RDO rdo)
        {
            bool result = false;
            try
            {
                if (ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                ProcessSumTotal();

                objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA.OrderBy(o=>o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB.OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC.OrderBy(o => o.IN_TIME).ToList());
                objectTag.AddObjectData(store, "SumTotals", ListSumTotal);
                objectTag.AddObjectData(store, "ReportTypeCats", this.listServiceRetyCat.GroupBy(o=>o.CATEGORY_CODE).Select(o=>o.First()).ToList());
                objectTag.AddObjectData(store, "ReportTypeCatARV", ListRdoARV.Where(p => p.CATEGORY_CODE == "ARV" && (p.HEIN_RATIO == 95 || p.HEIN_RATIO == 80)).OrderBy(p => p.HEIN_SERVICE_NAME).ToList());
                dicSingleTag.Add("PRICE_ARV_TEXT", ListRdoARV.Sum(p => p.TOTAL_PRICE));
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessSumTotal()
        {
            try
            {
                Mrs00652RDO rdo = new Mrs00652RDO();
                rdo.TOTAL_PRICE = (ListRdoA.Sum(s => s.TOTAL_PRICE) + ListRdoB.Sum(s => s.TOTAL_PRICE) + ListRdoC.Sum(s => s.TOTAL_PRICE));
                rdo.TEST_PRICE = (ListRdoA.Sum(s => s.TEST_PRICE) + ListRdoB.Sum(s => s.TEST_PRICE) + ListRdoC.Sum(s => s.TEST_PRICE));
                rdo.DIIM_PRICE = (ListRdoA.Sum(s => s.DIIM_PRICE) + ListRdoB.Sum(s => s.DIIM_PRICE) + ListRdoC.Sum(s => s.DIIM_PRICE));
                rdo.MEDICINE_PRICE = (ListRdoA.Sum(s => s.MEDICINE_PRICE) + ListRdoB.Sum(s => s.MEDICINE_PRICE) + ListRdoC.Sum(s => s.MEDICINE_PRICE));
                rdo.BLOOD_PRICE = (ListRdoA.Sum(s => s.BLOOD_PRICE) + ListRdoB.Sum(s => s.BLOOD_PRICE) + ListRdoC.Sum(s => s.BLOOD_PRICE));
                rdo.SURGMISU_PRICE = (ListRdoA.Sum(s => s.SURGMISU_PRICE) + ListRdoB.Sum(s => s.SURGMISU_PRICE) + ListRdoC.Sum(s => s.SURGMISU_PRICE));
                rdo.MATERIAL_PRICE = (ListRdoA.Sum(s => s.MATERIAL_PRICE) + ListRdoB.Sum(s => s.MATERIAL_PRICE) + ListRdoC.Sum(s => s.MATERIAL_PRICE));
                rdo.SERVICE_PRICE_RATIO = (ListRdoA.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoB.Sum(s => s.SERVICE_PRICE_RATIO) + ListRdoC.Sum(s => s.SERVICE_PRICE_RATIO));
                rdo.MEDICINE_PRICE_RATIO = (ListRdoA.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoB.Sum(s => s.MEDICINE_PRICE_RATIO) + ListRdoC.Sum(s => s.MEDICINE_PRICE_RATIO));
                rdo.MATERIAL_PRICE_RATIO = (ListRdoA.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoB.Sum(s => s.MATERIAL_PRICE_RATIO) + ListRdoC.Sum(s => s.MATERIAL_PRICE_RATIO));
                rdo.EXAM_PRICE = (ListRdoA.Sum(s => s.EXAM_PRICE) + ListRdoB.Sum(s => s.EXAM_PRICE) + ListRdoC.Sum(s => s.EXAM_PRICE));
                rdo.BED_PRICE = (ListRdoA.Sum(s => s.BED_PRICE) + ListRdoB.Sum(s => s.BED_PRICE) + ListRdoC.Sum(s => s.BED_PRICE));
                rdo.TRAN_PRICE = (ListRdoA.Sum(s => s.TRAN_PRICE) + ListRdoB.Sum(s => s.TRAN_PRICE) + ListRdoC.Sum(s => s.TRAN_PRICE));
                rdo.TT_PRICE = (ListRdoA.Sum(s => s.TT_PRICE) + ListRdoB.Sum(s => s.TT_PRICE) + ListRdoC.Sum(s => s.TT_PRICE));
                rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                rdo.TOTAL_HEIN_PRICE = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                rdo.TOTAL_BHVN_PRICE = (ListRdoA.Sum(s => s.TOTAL_BHVN_PRICE) + ListRdoB.Sum(s => s.TOTAL_BHVN_PRICE) + ListRdoC.Sum(s => s.TOTAL_BHVN_PRICE));
                rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                rdo.TOTAL_PATIENT_PRICE_CCT = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE_CCT) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE_CCT) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE_CCT));
                rdo.TOTAL_PATIENT_PRICE_TT = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE_TT) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE_TT) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE_TT));
                rdo.TOTAL_OTHER_SOURCE_PRICE = (ListRdoA.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoB.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE) + ListRdoC.Sum(s => s.TOTAL_OTHER_SOURCE_PRICE));
                ListSumTotal.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
