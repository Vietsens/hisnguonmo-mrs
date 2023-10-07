using MOS.MANAGER.HisBranch;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Common.Treatment;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00072
{
    class Mrs00072Processor : AbstractProcessor
    {
        Mrs00072Filter castFilter = null;
        List<Mrs00072RDO> ListRdoA = new List<Mrs00072RDO>();
        List<Mrs00072RDO> ListRdoB = new List<Mrs00072RDO>();
        List<Mrs00072RDO> ListRdoC = new List<Mrs00072RDO>();

        Dictionary<short, Mrs00072RDO> dicRdoA = new Dictionary<short, Mrs00072RDO>();
        Dictionary<short, Mrs00072RDO> dicRdoB = new Dictionary<short, Mrs00072RDO>();
        Dictionary<short, Mrs00072RDO> dicRdoC = new Dictionary<short, Mrs00072RDO>();

        List<Mrs00072RDO> ListTotalRdoA = new List<Mrs00072RDO>();
        List<Mrs00072RDO> ListTotalRdoB = new List<Mrs00072RDO>();
        List<Mrs00072RDO> ListTotalRdoC = new List<Mrs00072RDO>();
        List<Mrs00072RDO> ListDetailRdo = new List<Mrs00072RDO>();

        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        private decimal TotalAmount = 0;

        const short ThongTuyen = 3;
        const short CapCuu = 2;
        const short DungTuyen = 1;
        const short TraiTuyen = 0;

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        HIS_BRANCH _Branch = null;
        string MaterialPriceOption = "";

        Dictionary<long, int> DicTreatmentId = new Dictionary<long, int>();

        public Mrs00072Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00072Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00072Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => this.castFilter.BRANCH_ID == null || o.ID == this.castFilter.BRANCH_ID);
                MaterialPriceOption = MaterialPriceOptionCFG.MATERIAL_PRICE_OPTION_VALUE;
                //Inventec.Common.Logging.LogSystem.Info(Inventec.Common.Logging.LogUtil.TraceData("Bat dau lay du lieu bao cao MRS00072: " + Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                CommonParam paramGet = new CommonParam();
                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                approvalFilter.ORDER_DIRECTION = "ASC";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                //Inventec.Common.Logging.LogSystem.Info("ListHeinApproval" + ListHeinApproval.Count);
                ListHeinApproval = ListHeinApproval.Where(o => !String.IsNullOrEmpty(o.HEIN_CARD_NUMBER)).ToList();

                //lọc nội tỉnh ngoại tỉnh
                FilterProvinceType();

                //lọc đúng tuyến trái tuyến
                FilterRouteType();

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00072");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterRouteType()
        {
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE).ToList();
            }
            if (castFilter.INPUT_DATA_ID_ROUTE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => o.RIGHT_ROUTE_CODE != MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE).ToList();
            }
        }

        private void FilterProvinceType()
        {
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 1)
            {
                ListHeinApproval = ListHeinApproval.Where(o => checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
            if (castFilter.INPUT_DATA_ID_PROVINCE_TYPE == 2)
            {
                ListHeinApproval = ListHeinApproval.Where(o => !checkBhytProvinceCode(o.HEIN_CARD_NUMBER)).ToList();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    long countListSereServ = 0;
                    long countListTreatment = 0;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                            if (castFilter.SERVICE_TYPE_IDs != null)
                            {
                                ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o.TDL_SERVICE_TYPE_ID)).ToList();
                            }
                            if (castFilter.SERVICE_IDs != null)
                            {
                                ListSereServ = ListSereServ.Where(o => castFilter.SERVICE_IDs.Contains(o.SERVICE_ID)).ToList();
                            }
                            if (castFilter.SS_PATIENT_TYPE_IDs != null)
                            {
                                ListSereServ = ListSereServ.Where(o => castFilter.SS_PATIENT_TYPE_IDs.Contains(o.PATIENT_TYPE_ID)).ToList();
                            }
                            if (!string.IsNullOrWhiteSpace(castFilter.HEIN_RATIO))
                            {
                                ListSereServ = ListSereServ.Where(o => (o.HEIN_RATIO * 100).ToString() == castFilter.HEIN_RATIO).ToList();
                            }
                        }

                        if (ListSereServ != null) countListSereServ += ListSereServ.Count;
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                        treatmentFilter.END_DEPARTMENT_IDs = castFilter.END_DEPARTMENT_IDs;
                        
                        ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        if (castFilter.TREATMENT_TYPE_IDs != null)
                        {
                            ListTreatment = ListTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                        }
                        if (castFilter.CASHIER_LOGINNAMEs != null)
                        {
                            ListTreatment = ListTreatment.Where(o => castFilter.CASHIER_LOGINNAMEs.Contains(o.FEE_LOCK_LOGINNAME)).ToList();
                        }
                        if (castFilter.PATIENT_TYPE_IDs != null)
                        {
                            ListTreatment = ListTreatment.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                        }
                        if (castFilter.IS_POLICE_OFFICER == true)
                        {
                            ListTreatment = ListTreatment.Where(o => o.TDL_HEIN_CARD_NUMBER.Substring(0, 2) == "CA").ToList();
                        }
                        HisServiceReqFilterQuery srFilter = new HisServiceReqFilterQuery();
                        if (ListSereServ != null)
                        {
                            srFilter.IDs = ListSereServ.Select(p => p.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                        }
                        ListServiceReq = new HisServiceReqManager().Get(srFilter);
                        if (castFilter.REQUEST_DEPARTMENT_IDs != null)
                        {
                            ListServiceReq = ListServiceReq.Where(o => castFilter.REQUEST_DEPARTMENT_IDs.Contains(o.REQUEST_DEPARTMENT_ID)).ToList();
                        }
                        if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                        {
                            ListServiceReq = ListServiceReq.Where(o => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(o.EXECUTE_DEPARTMENT_ID)).ToList();
                        }
                        if (castFilter.EXECUTE_ROOM_IDs != null)
                        {
                            ListServiceReq = ListServiceReq.Where(o => castFilter.EXECUTE_ROOM_IDs.Contains(o.EXECUTE_ROOM_ID)).ToList();
                        }
                        if (ListTreatment != null) countListTreatment += ListTreatment.Count;
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00072.");
                        }

                        GeneralDataByListHeinApproval(hisHeinApprovals, ListSereServ, ListTreatment);

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    ListRdoA = dicRdoA.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();
                    ListRdoB = dicRdoB.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();
                    ListRdoC = dicRdoC.Select(s => s.Value).OrderBy(o => o.DUNG_TRAI).ToList();

                    ListRdoA = CheckList(ListRdoA);
                    ListRdoB = CheckList(ListRdoB);
                    ListRdoC = CheckList(ListRdoC);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoA = new List<Mrs00072RDO>();
                ListRdoB = new List<Mrs00072RDO>();
                ListRdoC = new List<Mrs00072RDO>();
                ListTotalRdoA = new List<Mrs00072RDO>();
                ListTotalRdoB = new List<Mrs00072RDO>();
                ListTotalRdoC = new List<Mrs00072RDO>();
                result = false;
            }
            return result;
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var treatment in ListTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        ProcessDetail(ListSereServ);
                        foreach (var item in ListSereServ)
                        {
                            if (item.HEIN_APPROVAL_ID == null || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == (short)1 || item.AMOUNT <= 0)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);

                        }
                        
                    }
                    hisHeinApprovals = hisHeinApprovals.Where(o => dicTreatment.ContainsKey(o.TREATMENT_ID)).ToList();
                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                        {
                            this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                            Mrs00072RDO rdo = null;
                            short isDungTrai;
                            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                isDungTrai = DungTuyen;
                                if(castFilter.SPLIT_RIGHT_ROUTE == true)
                                {
                                    if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.OVER)
                                    {
                                        isDungTrai = ThongTuyen;
                                    }
                                    else if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                                    {
                                        isDungTrai = CapCuu;
                                    }
                                }    
                                
                            }
                            else
                            {
                                isDungTrai = TraiTuyen;
                            }

                            if (IsInFilterMediOrgAccept(castFilter.ACCEPT_HEIN_MEDI_ORG_CODE, heinApproval.HEIN_MEDI_ORG_CODE)
                                    && (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE ?? "").Contains(heinApproval.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                            {
                                if (!dicRdoA.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00072RDO(heinApproval,castFilter);
                                    dicRdoA[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoA[isDungTrai];
                                }
                            }
                            else if (checkBhytProvinceCode(heinApproval.HEIN_CARD_NUMBER))
                            {
                                if (!dicRdoB.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00072RDO(heinApproval, castFilter);
                                    dicRdoB[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoB[isDungTrai];
                                }
                            }
                            else
                            {
                                if (!dicRdoC.ContainsKey(isDungTrai))
                                {
                                    rdo = new Mrs00072RDO(heinApproval, castFilter);
                                    dicRdoC[isDungTrai] = rdo;
                                }
                                else
                                {
                                    rdo = dicRdoC[isDungTrai];
                                }
                            }

                            if (IsNotNull(rdo) && dicSereServ.ContainsKey(heinApproval.ID) && dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID], heinApproval, dicTreatment[heinApproval.TREATMENT_ID]);
                                
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

        private void ProcessDetail(List<V_HIS_SERE_SERV_3> sereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(sereServ))
                {
                    foreach (var item in sereServ)
                    {
                        var treatment = ListTreatment.FirstOrDefault(p => p.ID == item.TDL_TREATMENT_ID);
                        var serviceReq = ListServiceReq.FirstOrDefault(p => p.ID == item.SERVICE_REQ_ID);
                        Mrs00072RDO rdo = new Mrs00072RDO();
                        if (treatment != null)
                        {
                            rdo.TDL_PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
                            rdo.TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID ?? 0;
                        }
                        rdo.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        if (serviceReq != null)
                        {
                            rdo.EXECUTE_TIME = serviceReq.START_TIME;
                            rdo.EXECUTE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.START_TIME ?? 0);
                        }
                        
                        rdo.PRICE = item.PRICE;
                        rdo.AMOUNT = item.AMOUNT;
                        rdo.SERVICE_PRICE_RATIO = (item.HEIN_RATIO ?? 0) * 100;
                        rdo.TOTAL_PRICE = item.VIR_PRICE ?? 0;
                        rdo.TOTAL_HEIN_PRICE = item.VIR_HEIN_PRICE ?? 0;
                        rdo.TOTAL_PATIENT_PRICE = item.VIR_PATIENT_PRICE ?? 0;
                        ListDetailRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessTotalPrice(Mrs00072RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs, V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisSereServs))
                {
                    decimal totalHeinPrice = 0;
                    foreach (var sereServ in hisSereServs)
                    {
                        if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                            continue;
                        //var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH());
                        var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID) ?? new HIS_BRANCH(), MaterialPriceOption == "1");

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
                            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU|| sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CPM)
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
                            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            {
                                rdo.BED_PRICE += TotalPriceTreatment;
                            }
                            else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                            {
                                rdo.EXAM_PRICE += TotalPriceTreatment;
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
                            totalHeinPrice += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                            rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        }
                    }

                    //Ho so co 2 the chi tinh 1 lan
                    if (DicTreatmentId.ContainsKey(heinApproval.TREATMENT_ID))
                    {
                        DicTreatmentId[heinApproval.TREATMENT_ID] += 1;
                    }
                    else
                    {
                        DicTreatmentId[heinApproval.TREATMENT_ID] = 1;
                        rdo.TOTAL_COUNT++;
                    }

                    if (treatment.CLINICAL_IN_TIME.HasValue && treatment.OUT_TIME.HasValue)
                    {
                        if (treatment.TREATMENT_DAY_COUNT.HasValue)
                        {
                            rdo.TOTAL_DATE += Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                        }
                        else
                        {
                            rdo.TOTAL_DATE += Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0;
                        }
                    }
                    if (checkBhytNsd(heinApproval, treatment))
                    {
                        rdo.TOTAL_HEIN_PRICE_NDS += totalHeinPrice;
                    }
                    else
                    {
                        rdo.TOTAL_HEIN_PRICE += totalHeinPrice;
                    }
                    TotalAmount += totalHeinPrice;
                }
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
                Mrs00072RDO rdoA = new Mrs00072RDO();
                foreach (var item in ListRdoA)
                {
                    rdoA.BED_PRICE += item.BED_PRICE;
                    rdoA.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoA.DIIM_PRICE += item.DIIM_PRICE;
                    rdoA.EXAM_PRICE += item.EXAM_PRICE;
                    rdoA.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoA.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoA.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoA.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoA.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoA.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoA.TEST_PRICE += item.TEST_PRICE;
                    rdoA.TOTAL_DATE += item.TOTAL_DATE;
                    rdoA.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoA.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoA.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoA.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoA.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoA.TRAN_PRICE += item.TRAN_PRICE;
                    rdoA.TT_PRICE += item.TT_PRICE;
                    rdoA.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoA.Add(rdoA);

                Mrs00072RDO rdoB = new Mrs00072RDO();
                foreach (var item in ListRdoB)
                {
                    rdoB.BED_PRICE += item.BED_PRICE;
                    rdoB.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoB.DIIM_PRICE += item.DIIM_PRICE;
                    rdoB.EXAM_PRICE += item.EXAM_PRICE;
                    rdoB.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoB.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoB.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoB.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoB.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoB.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoB.TEST_PRICE += item.TEST_PRICE;
                    rdoB.TOTAL_DATE += item.TOTAL_DATE;
                    rdoB.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoB.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoB.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoB.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoB.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoB.TRAN_PRICE += item.TRAN_PRICE;
                    rdoB.TT_PRICE += item.TT_PRICE;
                    rdoB.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoB.Add(rdoB);

                Mrs00072RDO rdoC = new Mrs00072RDO();
                foreach (var item in ListRdoC)
                {
                    rdoC.BED_PRICE += item.BED_PRICE;
                    rdoC.BLOOD_PRICE += item.BLOOD_PRICE;
                    rdoC.DIIM_PRICE += item.DIIM_PRICE;
                    rdoC.EXAM_PRICE += item.EXAM_PRICE;
                    rdoC.MATERIAL_PRICE += item.MATERIAL_PRICE;
                    rdoC.MATERIAL_PRICE_RATIO += item.MATERIAL_PRICE_RATIO;
                    rdoC.MEDICINE_PRICE += item.MEDICINE_PRICE;
                    rdoC.MEDICINE_PRICE_RATIO += item.MEDICINE_PRICE_RATIO;
                    rdoC.SERVICE_PRICE_RATIO += item.SERVICE_PRICE_RATIO;
                    rdoC.SURGMISU_PRICE += item.SURGMISU_PRICE;
                    rdoC.TEST_PRICE += item.TEST_PRICE;
                    rdoC.TOTAL_DATE += item.TOTAL_DATE;
                    rdoC.TOTAL_COUNT += item.TOTAL_COUNT;
                    rdoC.TOTAL_HEIN_PRICE += item.TOTAL_HEIN_PRICE;
                    rdoC.TOTAL_HEIN_PRICE_NDS += item.TOTAL_HEIN_PRICE_NDS;
                    rdoC.TOTAL_PATIENT_PRICE += item.TOTAL_PATIENT_PRICE;
                    rdoC.TOTAL_PRICE += item.TOTAL_PRICE;
                    rdoC.TRAN_PRICE += item.TRAN_PRICE;
                    rdoC.TT_PRICE += item.TT_PRICE;
                    rdoC.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                }
                ListTotalRdoC.Add(rdoC);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00072RDO> CheckList(List<Mrs00072RDO> listRdo)
        {
            List<Mrs00072RDO> result = new List<Mrs00072RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    foreach (var rdo in listRdo)
                    {
                        //khong co gia thi bo qua
                        if (!CheckPrice(rdo)) continue;

                        result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = new List<Mrs00072RDO>();
            }
            return result;
        }

        private bool CheckPrice(Mrs00072RDO rdo)
        {
            bool result = false;
            try
            {
                result = rdo.BED_PRICE > 0 || rdo.BLOOD_PRICE > 0 || rdo.DIIM_PRICE > 0 || rdo.EXAM_PRICE > 0 ||
                    rdo.MATERIAL_PRICE > 0 || rdo.MEDICINE_PRICE > 0 || rdo.SURGMISU_PRICE > 0 || rdo.TEST_PRICE > 0 ||
                    rdo.TOTAL_HEIN_PRICE > 0 || rdo.TOTAL_HEIN_PRICE_NDS > 0 || rdo.TOTAL_PATIENT_PRICE > 0 || rdo.TOTAL_PRICE > 0 || rdo.TRAN_PRICE > 0;
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private bool IsInFilterMediOrgAccept(string filterAccept, string HeinMediOrgCode)
        {
            if (string.IsNullOrWhiteSpace(filterAccept)) return true;
            if (filterAccept.Contains(HeinMediOrgCode ?? "")) return true;
            return false;
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

        private bool checkBhytNsd(V_HIS_HEIN_APPROVAL heinApproval, V_HIS_TREATMENT treatment)
        {
            bool result = false;
            try
            {
                if (MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(treatment.ICD_CODE))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(treatment.ICD_CODE))
                {
                    if (heinApproval.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(treatment.ICD_CODE.Substring(0, 3)))
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

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                ProcessSumTotal();

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalAs", ListTotalRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalBs", ListTotalRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotalCs", ListTotalRdoC);
                exportSuccess = exportSuccess && objectTag.SetUserFunction(store, "FuncRownumber", new CustomerFuncRownumberData());
                objectTag.AddObjectData(store, "Detail", ListDetailRdo.OrderBy(p => p.SERVICE_CODE).ThenBy(p => p.TDL_PATIENT_CODE).ThenBy(p => p.EXECUTE_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class CustomerFuncRownumberData : TFlexCelUserFunction
    {
        public CustomerFuncRownumberData()
        {
        }
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            long result = 0;
            try
            {
                long rownumber = Convert.ToInt64(parameters[0]);
                result = (rownumber + 1);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return result;
        }
    }
}
