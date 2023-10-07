using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisHeinApproval;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using FlexCel.Report;
using HIS.Common.Treatment;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisWorkPlace;

namespace MRS.Processor.Mrs00065
{
    class Mrs00065Processor : AbstractProcessor
    {
        Mrs00065Filter castFilter = null;
        List<Mrs00065RDO> ListRdo = new List<Mrs00065RDO>();
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        HIS_BRANCH _Branch = null;
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_WORK_PLACE> listWorkPlace = new List<HIS_WORK_PLACE>();

        public Mrs00065Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00065Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
		protected override bool GetData()
		{
            bool result = true;
            try
            {
                this.castFilter = (Mrs00065Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00065: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                if (castFilter.OUT_TIME_FROM.HasValue && castFilter.OUT_TIME_TO.HasValue)
                {
                    HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                    treatmentFilter.OUT_TIME_FROM = castFilter.OUT_TIME_FROM;
                    treatmentFilter.OUT_TIME_TO = castFilter.OUT_TIME_TO;
                    treatmentFilter.IS_PAUSE = true;
                    
                    ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);
                    if (castFilter.PATIENT_TYPE_IDs != null)
                    {
                        ListTreatment = ListTreatment.Where(p => castFilter.PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                    }
                    int skip = 0;
                    while (ListTreatment.Count - skip > 0)
                    {
                        var listId = ListTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                        approvalFilter.TREATMENT_IDs = listId.Select(s => s.ID).ToList();
                        approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                        approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
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
                    //Duyet giam dinh
                    HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                    approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                    approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                    approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                    approvalFilter.BRANCH_IDs = castFilter.BRANCH_IDs;
                    approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                    approvalFilter.ORDER_DIRECTION = "ASC";
                    ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00065");
                }

                //danh sách nơi làm việc
                GetWorkPlace();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetWorkPlace()
        {
            listWorkPlace = new HisWorkPlaceManager().Get(new HisWorkPlaceFilterQuery());
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    ListHeinApproval = ListHeinApproval.Where(o => o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM).ToList();
                }

                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                        List<V_HIS_HEIN_APPROVAL> hisHeinApprovals = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisSereServView3FilterQuery ssHeinFilter = new HisSereServView3FilterQuery();
                        ssHeinFilter.HEIN_APPROVAL_IDs = hisHeinApprovals.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssHeinFilter);
                        //if (ListSereServ != null)
                        //{
                        //    ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        //}

                        List<V_HIS_TREATMENT> treatment = new List<V_HIS_TREATMENT>();
                        if (!IsNotNullOrEmpty(ListTreatment))
                        {
                            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                            treatmentFilter.IDs = hisHeinApprovals.Select(s => s.TREATMENT_ID).ToList().Distinct().ToList();
                            treatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter);
                        }
                        else
                        {
                            treatment.AddRange(ListTreatment);
                        }

                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu MRS00065");
                        }

                        //bệnh nhân
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = treatment.Select(s => s.PATIENT_ID).ToList().Distinct().ToList();
                        var patients = new HisPatientManager(paramGet).Get(patientFilter);
                        ProcessListHeinApprovalDetail(hisHeinApprovals, ListSereServ, treatment, patients);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApprovalDetail(List<V_HIS_HEIN_APPROVAL> hisHeinApprovals, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment,List<HIS_PATIENT> patients)
        {
            try
            {
                Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                Dictionary<long, HIS_PATIENT> dicPatient= new Dictionary<long, HIS_PATIENT>();
                Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                if (IsNotNullOrEmpty(hisHeinApprovals))
                {
                    if (IsNotNullOrEmpty(ListTreatment))
                    {
                        foreach (var item in ListTreatment)
                        {
                            dicTreatment[item.ID] = item;
                        }
                    }
                    if (IsNotNullOrEmpty(patients))
                    {
                        foreach (var item in patients)
                        {
                            dicPatient[item.ID] = item;
                        }
                    }
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var item in ListSereServ)
                        {
                            if (item.HEIN_APPROVAL_ID == null || item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.TDL_HEIN_SERVICE_TYPE_ID == null || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.AMOUNT <= 0)
                                continue;
                            if (!dicSereServ.ContainsKey(item.HEIN_APPROVAL_ID.Value))
                                dicSereServ[item.HEIN_APPROVAL_ID.Value] = new List<V_HIS_SERE_SERV_3>();
                            dicSereServ[item.HEIN_APPROVAL_ID.Value].Add(item);
                        }
                    }

                    foreach (var heinApproval in hisHeinApprovals)
                    {
                        if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            continue;
                        this._Branch = HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == heinApproval.BRANCH_ID);
                        Mrs00065RDO rdo = new Mrs00065RDO(heinApproval);
                        if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.TREATMENT_TYPE_CODE = "1";
                        }
                        else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                        {
                            rdo.TREATMENT_TYPE_CODE = "2";
                        }
                        else
                        {
                            rdo.TREATMENT_TYPE_CODE = "3";
                        }

                        rdo.PATIENT_CODE = heinApproval.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = heinApproval.TDL_PATIENT_NAME;
                        rdo.DOB = Convert.ToInt64(heinApproval.TDL_PATIENT_DOB.ToString().Substring(0, 8));
                        if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.GENDER_CODE = "1";
                        }
                        else if (heinApproval.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.GENDER_CODE = "2";
                        }

                        rdo.VIR_ADDRESS = heinApproval.ADDRESS;
                        rdo.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
                        rdo.HEIN_MEDI_ORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
                        rdo.HEIN_CARD_FROM_TIME_STR = Convert.ToInt64(heinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8));
                        rdo.HEIN_CARD_TO_TIME_STR = Convert.ToInt64(heinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8));

                        if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                        {
                            var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                            rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                            rdo.OPEN_TIME_SEPARATE_STR = treatment.IN_TIME.ToString().Substring(0, 12);
                            rdo.CLOSE_TIME_SEPARATE_STR = treatment.OUT_TIME.HasValue ? treatment.OUT_TIME.Value.ToString().Substring(0, 12) : "";
                            rdo.IN_TIME = treatment.IN_TIME;
                            rdo.OUT_TIME = treatment.OUT_TIME;
                            rdo.CLINICAL_IN_TIME = treatment.CLINICAL_IN_TIME;
                            rdo.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
                            if (dicPatient.ContainsKey(heinApproval.PATIENT_ID))
                            {
                                var patient = dicPatient[heinApproval.PATIENT_ID]??new HIS_PATIENT();
                                rdo.TDL_PATIENT_WORK_PLACE_NAME = patient.WORK_PLACE;
                                var workPlace = (listWorkPlace ?? new List<HIS_WORK_PLACE>()).FirstOrDefault(o => o.ID == patient.WORK_PLACE_ID);
                                if (workPlace != null)
                                {
                                    rdo.TDL_PATIENT_WORK_PLACE_NAME = workPlace.WORK_PLACE_NAME;
                                }
                            }

                            rdo.TOTAL_DAY = this.CountDay(treatment);
                            rdo.TREATMENT_DAY_COUNT_6556 = HIS.Common.Treatment.Calculation.DayOfTreatment6556(treatment.IN_TIME, treatment.CLINICAL_IN_TIME, treatment.OUT_TIME, treatment.TDL_TREATMENT_TYPE_ID ?? 0) ?? 0;
                            if (treatment.END_DEPARTMENT_ID.HasValue)
                            {
                                var departmentCodeBHYT = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID);
                                if (departmentCodeBHYT != null)
                                {
                                    rdo.DEPARTMENT_CODE = departmentCodeBHYT.BHYT_CODE;
                                    rdo.END_DEPARTMENT_NAME = departmentCodeBHYT.DEPARTMENT_NAME;
                                }
                            }
                            if (treatment.END_ROOM_ID.HasValue)
                            {
                                var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == treatment.END_ROOM_ID);
                                if (room != null)
                                {
                                    rdo.END_ROOM_CODE = room.ROOM_CODE;
                                    rdo.END_ROOM_NAME = room.ROOM_NAME;
                                }
                            }

                            //Ket qua dieu tri: 1: Khỏi;  2: Đỡ;  3: Không thay đổi;  4: Nặng hơn;  5: Tử vong
                            if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                            {
                                rdo.TREATMENT_RESULT_CODE = "1";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                            {
                                rdo.TREATMENT_RESULT_CODE = "2";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                            {
                                rdo.TREATMENT_RESULT_CODE = "3";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                            {
                                rdo.TREATMENT_RESULT_CODE = "4";
                            }
                            else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                            {
                                rdo.TREATMENT_RESULT_CODE = "5";
                            }

                            //Tinh trang ra vien: 1: Ra viện;  2: Chuyển viện;  3: Trốn viện;  4: Xin ra viện
                            if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__RAVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "1";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__CHUYENVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "2";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__TRONVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "3";
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == HisTreatmentEndType_BHYTCFG.TREATMENT_END_TYPE_ID__XINRAVIEN)
                            {
                                rdo.TREATMENT_END_TYPE_CODE = "4";
                            }
                            rdo.ICD_CODE_EXTRA = treatment.ICD_SUB_CODE; //

                            if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                            {
                                if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                                {
                                    rdo.REASON_INPUT_CODE = "2";
                                }
                                else
                                {
                                    rdo.REASON_INPUT_CODE = "1";
                                }
                            }
                            else if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.FALSE)
                            {
                                rdo.REASON_INPUT_CODE = "3";
                            }

                            //Ma noi chuyen
                            rdo.MEDI_ORG_FROM_CODE = dicTreatment[heinApproval.TREATMENT_ID].TRANSFER_IN_MEDI_ORG_CODE;

                            if (heinApproval.EXECUTE_TIME.HasValue)
                            {
                                rdo.INSURANCE_YEAR = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(0, 4));
                                rdo.INSURANCE_MONTH = Convert.ToInt64(heinApproval.EXECUTE_TIME.ToString().Substring(4, 2));
                            }

                            rdo.HEIN_LIVE_AREA_CODE = heinApproval.LIVE_AREA_CODE;
                            rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
                            //Noi thanh toan: 1: thanh toan tai co so;  2: thanh toan truc tiep
                            rdo.PLACE_PAYMENT_CODE = 1;
                            //Giam dinh: 0: không thẩm định;  1: chấp nhận;  2: điều chỉnh;  3: xuất toán
                            rdo.INSURANCE_STT = 0;
                            rdo.REASON_OUT_PRICE = 0;
                            rdo.REASON_OUT = "";
                            rdo.POLYLINE_PRICE = 0;
                            rdo.EXCESS_PRICE = 0;
                            rdo.ROUTE_CODE = "";
                            if (dicSereServ.ContainsKey(heinApproval.ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.ID]);
                            }

                            //khong co gia thi bo qua
                            if (!CheckPrice(rdo)) continue;

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private long CountDay(V_HIS_TREATMENT treatment)
        {
            long result = 0;
            try
            {
                PropertyInfo p = typeof(V_HIS_TREATMENT).GetProperty("TREATMENT_DAY_COUNT");
                if (p != null)
                {
                    var DayCount = p.GetValue(treatment);
                    if (DayCount != null)
                    {
                        result = Int64.Parse(DayCount.ToString());
                    }
                    else
                    {
                        result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                    }
                }
                else
                {
                    result = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, treatment.TDL_HEIN_CARD_NUMBER != null ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = 0;
            }
            return result;
        }

        private void ProcessTotalPrice(Mrs00065RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                    {
                        if (sereServ.OTHER_SOURCE_PRICE > 0)
                        {
                            
                            string T_NGUONKHAC_OPTION = "";
                            try
                            {
                                T_NGUONKHAC_OPTION = TNguonKhacOptionCFG.XML4210_T_NGUONKHAC_OPTION;
                                Inventec.Common.Logging.LogSystem.Info("nguon khac:" + T_NGUONKHAC_OPTION);
                                if (T_NGUONKHAC_OPTION == "2")
                                {
                                    rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                                }
                            }
                            catch (Exception)
                            {
                                LogSystem.Warn("can not load config XML.EXPORT.4210.T_NGUONKHAC_OPTION.");
                            }
                        }
                        continue;
                    }
                    var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());

                    if (sereServ.TDL_HEIN_SERVICE_TYPE_ID != null)
                    {
                        if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            rdo.EXAM_PRICE += TotalPriceTreatment;
                        }
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
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
                        rdo.TOTAL_PATIENT_PRICE_BHYT += sereServ.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0;
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                        if (sereServ.OTHER_SOURCE_PRICE > 0)
                            rdo.TOTAL_OTHER_SOURCE_PRICE_NEW += Math.Round((sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT, 0);
                    }
                }

                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckPrice(Mrs00065RDO rdo)
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

        private bool checkBhytNsd(Mrs00065RDO rdo)
        {
            bool result = false;
            try
            {
                if (MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Other.Contains(rdo.ICD_CODE_MAIN))
                {
                    result = true;
                }
                else if (!String.IsNullOrEmpty(rdo.ICD_CODE_MAIN))
                {
                    if (rdo.HEIN_CARD_NUMBER.Substring(0, 2).Equals("TE") && MRS.MANAGER.Config.ReportBhytNdsIcdCodeCFG.ReportBhytNdsIcdCode__Te.Contains(rdo.ICD_CODE_MAIN.Substring(0, 3)))
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0) + Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.OUT_TIME_TO ?? 0));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.SetUserFunction(store, "fRound", new CustomerFuncRoundData());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
    class CustomerFuncRoundData : TFlexCelUserFunction
    {

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            decimal result = 0;
            try
            {
                decimal dataIn1 = Convert.ToDecimal(parameters[0]);
                int dataIn2 = Convert.ToInt32(parameters[1]);
                result = Math.Round(dataIn1, dataIn2);


            }
            catch (Exception ex)
            {
                return 0;
            }

            return result;
        }
    }
}
