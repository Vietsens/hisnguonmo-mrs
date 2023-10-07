using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisAccidentHurt;
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
using HIS.Common.Treatment;

namespace MRS.Processor.Mrs00142
{
    public class Mrs00142Processor : AbstractProcessor
    {
        Mrs00142Filter castFilter = null;
        List<Mrs00142RDO> ListRdo = new List<Mrs00142RDO>();
        HIS_BRANCH _Branch = null;
        List<V_HIS_HEIN_APPROVAL> ListHeinApproval;

        public Mrs00142Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00142Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00142Filter)this.reportFilter;
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu V_HIS_HEIN_APPROVAL, MRS00142. Filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisHeinApprovalViewFilterQuery appFilter = new HisHeinApprovalViewFilterQuery();
                appFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                appFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                appFilter.BRANCH_ID = castFilter.BRANCH_ID;
                appFilter.ORDER_FIELD = "EXECUTE_TIME";
                appFilter.ORDER_DIRECTION = "ACS";
                ListHeinApproval = new MOS.MANAGER.HisHeinApproval.HisHeinApprovalManager(paramGet).GetView(appFilter);
                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_HEIN_APPROVAL, MRS00142.");
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
            bool result = false;
            try
            {
                ProcessListHeinApproval(ListHeinApproval);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListHeinApproval(List<V_HIS_HEIN_APPROVAL> ListHeinApproval)
        {
            try
            {
                if (IsNotNullOrEmpty(ListHeinApproval))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = ListHeinApproval.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisHeinApproval = ListHeinApproval.Skip(start).Take(limit).ToList();
                        HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                        var listTreatId = hisHeinApproval.Select(s => s.TREATMENT_ID).Distinct().ToList();
                        treatmentFilter.IDs = listTreatId;
                        List<V_HIS_TREATMENT> ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter);

                        HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                        ssFilter.HEIN_APPROVAL_IDs = hisHeinApproval.Select(s => s.ID).ToList();
                        List<V_HIS_SERE_SERV_3> ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView3(ssFilter);
                        if (ListSereServ != null)
                        {
                            ListSereServ = ListSereServ.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }

                        HisAccidentHurtViewFilterQuery accidentHurtFilter = new HisAccidentHurtViewFilterQuery();
                        accidentHurtFilter.TREATMENT_IDs = listTreatId;
                        List<V_HIS_ACCIDENT_HURT> ListAccidentHurt = new MOS.MANAGER.HisAccidentHurt.HisAccidentHurtManager(paramGet).GetView(accidentHurtFilter);

                        if (!paramGet.HasException)
                        {
                            ProcessDetailHeinApproval(paramGet, hisHeinApproval, ListSereServ, ListTreatment, ListAccidentHurt);
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailHeinApproval(CommonParam paramGet, List<V_HIS_HEIN_APPROVAL> hisHeinApproval, List<V_HIS_SERE_SERV_3> ListSereServ, List<V_HIS_TREATMENT> ListTreatment, List<V_HIS_ACCIDENT_HURT> ListAccidentHurt)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListTreatment))
                {
                    Dictionary<long, HIS_ICD> dicIcd = GetIcd();
                    foreach (var heinApproval in hisHeinApproval)
                    {
                        Mrs00142RDO rdo = new Mrs00142RDO();

                        var hisSereServHeins = ListSereServ.Where(o => o.HEIN_APPROVAL_ID == heinApproval.ID).ToList();
                        var treatment = ListTreatment.FirstOrDefault(o => o.ID == heinApproval.TREATMENT_ID);
                        if (IsNotNullOrEmpty(hisSereServHeins) && IsNotNull(treatment))
                        {
                            rdo.HEIN_APPROVAL_ID = heinApproval.ID;
                            rdo.HEIN_APPROVAL_CODE = heinApproval.HEIN_APPROVAL_CODE;
                            rdo.PATIENT_ID = treatment.PATIENT_ID;
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.DOB_STR = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 8);
                            if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                            {
                                rdo.GENDER_CODE = (short)2;
                            }
                            else if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                            {
                                rdo.GENDER_CODE = (short)1;
                            }
                            rdo.VIR_ADDRESS = heinApproval.ADDRESS;
                            rdo.HEIN_CARD_NUMBER = heinApproval.HEIN_CARD_NUMBER;
                            rdo.MEDI_ORG_CODE = heinApproval.HEIN_MEDI_ORG_CODE;
                            rdo.HEIN_DATE_FROM_STR = heinApproval.HEIN_CARD_FROM_TIME.ToString().Substring(0, 8);
                            rdo.HEIN_DATE_TO_STR = heinApproval.HEIN_CARD_TO_TIME.ToString().Substring(0, 8);
                            rdo.ICD_NAME = treatment.ICD_NAME;
                            rdo.ICD_CODE = treatment.ICD_CODE;
                            rdo.ICD_TEXT = treatment.ICD_TEXT;
                            ProcessInfoBhyt(rdo, heinApproval);


                            if (IsNotNull(treatment))
                            {
                                rdo.MEDI_ORG_CODE_TRAN = treatment.MEDI_ORG_CODE;
                            }

                            if (IsNotNullOrEmpty(ListAccidentHurt))
                            {
                                var accidentHurt = ListAccidentHurt.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID);
                                if (IsNotNull(accidentHurt))
                                {
                                    rdo.ACCIDENT_HURT_CODE = Convert.ToInt16(accidentHurt.ACCIDENT_HURT_TYPE_CODE);
                                }
                            }

                            rdo.DATE_IN_STR = treatment.IN_TIME.ToString().Substring(0, 8);
                            if (treatment.OUT_TIME.HasValue)
                            {
                                rdo.DATE_OUT_STR = treatment.OUT_TIME.Value.ToString().Substring(0, 8);
                                if (heinApproval.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                                {
                                    if (treatment.TREATMENT_DAY_COUNT.HasValue)
                                    {
                                        rdo.TOTAL_DATE = Convert.ToInt64(treatment.TREATMENT_DAY_COUNT.Value);
                                    }
                                    else
                                    {
                                        rdo.TOTAL_DATE = Calculation.DayOfTreatment(treatment.CLINICAL_IN_TIME.HasValue ? treatment.CLINICAL_IN_TIME : treatment.IN_TIME, treatment.OUT_TIME, treatment.TREATMENT_END_TYPE_ID, treatment.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT);
                                    }
                                }
                            }
                            if (treatment.TREATMENT_RESULT_ID.HasValue)
                            {
                                if (treatment.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                                {
                                    rdo.TREATMENT_RESULT_CODE = (short)1;
                                }
                                else if (treatment.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                                {
                                    rdo.TREATMENT_RESULT_CODE = (short)2;
                                }
                                else if (treatment.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                                {
                                    rdo.TREATMENT_RESULT_CODE = (short)3;
                                }
                                else if (treatment.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                                {
                                    rdo.TREATMENT_RESULT_CODE = (short)4;
                                }
                                else if (treatment.TREATMENT_RESULT_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET)
                                {
                                    rdo.TREATMENT_RESULT_CODE = (short)5;
                                }
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("Treatment khong co Treatment_Result_Id: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                            }

                            if (treatment.TREATMENT_END_TYPE_ID.HasValue)
                            {
                                if (treatment.TREATMENT_END_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN)
                                {
                                    rdo.TREATMENT_END_TYPE_CODE = (short)1;
                                }
                                else if (treatment.TREATMENT_END_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                                {
                                    rdo.TREATMENT_END_TYPE_CODE = (short)2;
                                }
                                else if (treatment.TREATMENT_END_TYPE_ID.Value == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON)
                                {
                                    rdo.TREATMENT_END_TYPE_CODE = (short)3;
                                }
                                else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN)
                                {
                                    rdo.TREATMENT_END_TYPE_CODE = (short)4;
                                }
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("Treatment khong co Treatment_End_Type_IDd: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment), treatment));
                            }

                            if (treatment.FEE_LOCK_TIME.HasValue)
                            {
                                rdo.FEE_LOCK_DATE_STR = treatment.FEE_LOCK_TIME.Value.ToString().Substring(0, 8);
                            }
                            else
                            {
                                Inventec.Common.Logging.LogSystem.Info("Khong co thoi gian duyet khoa tai chinh (FEE_LOCK_TIME). " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => treatment.FEE_LOCK_TIME), treatment.FEE_LOCK_TIME));
                            }

                            var hisSereServMedicine = hisSereServHeins.Where(o =>
                                (
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT
                                ) && o.AMOUNT > 0).ToList();
                            if (IsNotNullOrEmpty(hisSereServMedicine))
                            {
                                rdo.MEDICINE_TOTAL_PRICE = hisSereServMedicine.Sum(s => s.VIR_TOTAL_PRICE.Value);
                            }

                            var hisSereServMaterial = hisSereServHeins.Where(o =>
                                (
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL ||
                                o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT
                                ) && o.AMOUNT > 0).ToList();
                            if (IsNotNullOrEmpty(hisSereServMaterial))
                            {
                                rdo.MATERIAL_TOTAL_PRICE = hisSereServMaterial.Sum(s => s.VIR_TOTAL_PRICE.Value);
                            }
                            rdo.VIR_TOTAL_PRICE = hisSereServHeins.Sum(s => s.VIR_TOTAL_PRICE.Value);
                            rdo.VIR_TOTAL_PATIENT_PRICE = hisSereServHeins.Sum(s => s.VIR_TOTAL_PATIENT_PRICE.Value);
                            rdo.VIR_TOTAL_HEIN_PRICE = hisSereServHeins.Sum(s => s.VIR_TOTAL_HEIN_PRICE.Value);
                            rdo.YEAR_FINAL = DateTime.Now.Year;
                            rdo.MONTH_FINAL = DateTime.Now.Month;

                            if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                            {
                                rdo.TREATMENT_TYPE_CODE = (short)1;
                            }
                            else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                rdo.TREATMENT_TYPE_CODE = (short)2;
                            }
                            else if (heinApproval.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TREATMENT_TYPE_CODE = (short)3;
                            }
                            else
                                rdo.TREATMENT_TYPE_CODE = (short)0;

                            if (treatment.END_DEPARTMENT_ID.HasValue)
                            {
                                var department = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == treatment.END_DEPARTMENT_ID.Value);
                                if (IsNotNull(department))
                                {
                                    rdo.DEPARTMENT_BHYT_CODE = department.BHYT_CODE;
                                }
                            }
                            rdo.CURRENT_MEDI_ORG_CODE = this._Branch.HEIN_MEDI_ORG_CODE;
                            ListRdo.Add(rdo);
                        }
                    }
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Info("Khong lay duoc du lieu de thuc hien tong hop MRS00142. " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => hisHeinApproval), hisHeinApproval));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private Dictionary<long, HIS_ICD> GetIcd()
        {
            Dictionary<long, HIS_ICD> result = new Dictionary<long, HIS_ICD>();
            try
            {
                CommonParam param = new CommonParam();
                HisIcdFilterQuery filter = new HisIcdFilterQuery();
                var listIcd = new MOS.MANAGER.HisIcd.HisIcdManager(param).Get(filter);
                foreach (var item in listIcd)
                {
                    if (!result.ContainsKey(item.ID)) result[item.ID] = item;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        private void ProcessInfoBhyt(Mrs00142RDO rdo, V_HIS_HEIN_APPROVAL heinApproval)
        {
            try
            {
                if (IsNotNull(heinApproval) && IsNotNull(rdo))
                {
                    rdo.LIVE_AREA_CODE = heinApproval.LIVE_AREA_CODE;
                    if (heinApproval.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE)
                    {
                        rdo.RIGHT_ROUTE_CODE = (short)1;
                    }
                    else if (heinApproval.RIGHT_ROUTE_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinRightRouteType.HeinRightRouteTypeCode.EMERGENCY)
                    {
                        rdo.RIGHT_ROUTE_CODE = (short)2;
                    }
                    else
                    {
                        rdo.RIGHT_ROUTE_CODE = (short)3;
                    }
                    rdo.RATIO = new MOS.LibraryHein.Bhyt.BhytHeinProcessor().GetDefaultHeinRatio(heinApproval.HEIN_TREATMENT_TYPE_CODE, heinApproval.HEIN_CARD_NUMBER, this._Branch.HEIN_LEVEL_CODE, heinApproval.RIGHT_ROUTE_CODE) ?? 0;

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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                objectTag.AddObjectData(store, "HeinApprovals", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
