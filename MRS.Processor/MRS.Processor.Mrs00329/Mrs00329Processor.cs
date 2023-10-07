using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatmentLogging;
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Core.AcsUser;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;

namespace MRS.Processor.Mrs00329
{
    public class Mrs00329Processor : AbstractProcessor
    {
        private Mrs00329Filter castFilter;

        List<Mrs00329RDO> ListRdoA = new List<Mrs00329RDO>();
        List<Mrs00329RDO> ListRdoB = new List<Mrs00329RDO>();
        List<Mrs00329RDO> ListRdoC = new List<Mrs00329RDO>();

        List<Mrs00329RDO> ListSumTotal = new List<Mrs00329RDO>();

        private decimal TotalAmount = 0;

        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_3> sereServs = new List<V_HIS_SERE_SERV_3>();
        Dictionary<string, ACS_USER> dicAcsUser = new Dictionary<string, ACS_USER>();
        HIS_BRANCH _Branch = null;

        public Mrs00329Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00329Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00329Filter)this.reportFilter);
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu HIS_TREATMENT, MRS00329, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                List<V_HIS_TREATMENT> tmp = this.GetTreatment();
                this.GetAcsUser();
                ///lay du lieu patient_type_alter va treament
                this.GetPatientTypeAlterAndTreatment(tmp, ref this.treatments, ref this.patientTypeAlters);
                this.sereServs = this.GetSereServ(this.treatments);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetAcsUser()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                AcsUserFilterQuery acsUserfilter = new AcsUserFilterQuery();
                acsUserfilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                dicAcsUser = (new AcsUserManager(paramGet).Get<List<ACS_USER>>(acsUserfilter) ?? new List<ACS_USER>()).GroupBy(o => o.LOGINNAME).ToDictionary(p => p.Key, p => p.First());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

       
        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(patientTypeAlters))
                {
                    GeneralDataByListHeinApproval(this.patientTypeAlters, this.sereServs, this.treatments);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void GeneralDataByListHeinApproval(List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters, List<V_HIS_SERE_SERV_3> listSereServ, List<V_HIS_TREATMENT> listTreatment)
        {
            try
            {
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
                    Dictionary<long, List<V_HIS_SERE_SERV_3>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV_3>>();

                    if (IsNotNullOrEmpty(listTreatment))
                    {
                        foreach (var treatment in listTreatment)
                        {
                            dicTreatment[treatment.ID] = treatment;
                        }
                    }

                    if (IsNotNullOrEmpty(listSereServ))
                    {
                        foreach (var sere in listSereServ)
                        {
                            if (sere.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && sere.AMOUNT > 0)
                            {
                                if (!dicSereServ.ContainsKey(sere.TDL_TREATMENT_ID ?? 0))
                                    dicSereServ[sere.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_SERE_SERV_3>();
                                dicSereServ[sere.TDL_TREATMENT_ID ?? 0].Add(sere);
                            }
                        }
                    }

                    foreach (var heinApproval in hisPatientTypeAlters)
                    {
                        if (CheckHeinCardNumberType(heinApproval.HEIN_CARD_NUMBER))
                        {
                            Mrs00329RDO rdo = new Mrs00329RDO(heinApproval, dicTreatment);

                            if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                var treatment = dicTreatment[heinApproval.TREATMENT_ID];
                                rdo.ICD_CODE_MAIN = treatment.ICD_CODE;
                                rdo.OPEN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                                if (treatment.OUT_TIME.HasValue)
                                {
                                    rdo.CLOSE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                                }
                            }
                            if (dicSereServ.ContainsKey(heinApproval.TREATMENT_ID))
                            {
                                ProcessTotalPrice(rdo, dicSereServ[heinApproval.TREATMENT_ID]);
                            }
                            if (dicTreatment.ContainsKey(heinApproval.TREATMENT_ID) && !string.IsNullOrWhiteSpace(dicTreatment[heinApproval.TREATMENT_ID].CREATOR))
                            {
                                if (dicAcsUser.ContainsKey(dicTreatment[heinApproval.TREATMENT_ID].CREATOR))
                                {
                                    rdo.FEE_LOCK_USERNAME = dicAcsUser[dicTreatment[heinApproval.TREATMENT_ID].CREATOR].USERNAME;
                                }
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

        private void ProcessTotalPrice(Mrs00329RDO rdo, List<V_HIS_SERE_SERV_3> hisSereServs)
        {
            try
            {
                foreach (var sereServ in hisSereServs)
                {
                    if (!sereServ.VIR_TOTAL_HEIN_PRICE.HasValue || sereServ.VIR_TOTAL_HEIN_PRICE.Value <= 0)
                        continue;
                    var TotalPriceTreatment = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO) * sereServ.AMOUNT;
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
                        else if (sereServ.TDL_HEIN_SERVICE_TYPE_ID ==
IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
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
                        rdo.TOTAL_PRICE += TotalPriceTreatment;
                        rdo.TOTAL_PATIENT_PRICE += TotalPriceTreatment - (sereServ.VIR_TOTAL_HEIN_PRICE ?? 0);
                        rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0;
                        rdo.TOTAL_OTHER_SOURCE_PRICE += (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;
                    }
                }
                TotalAmount += rdo.TOTAL_HEIN_PRICE;
                if (checkBhytNsd(rdo))
                {
                    rdo.TOTAL_HEIN_PRICE_NDS = rdo.TOTAL_HEIN_PRICE;
                    rdo.TOTAL_HEIN_PRICE = 0;
                }

                //khong co gia thi bo qua
                if (!CheckPrice(rdo)) return;

                if (this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(rdo.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(rdo.HEIN_CARD_NUMBER))
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
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckPrice(Mrs00329RDO rdo)
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

        private bool CheckHeinCardNumberType(string HeinCardNumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(HeinCardNumber))
                {
                    if (IsNotNullOrEmpty(MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01))
                    {
                        foreach (var type in MANAGER.Config.HeinCardNumberTypeCFG.HeinCardNumber__HeinType__01)
                        {
                            if (HeinCardNumber.StartsWith(type))
                            {
                                result = true;
                                break;
                            }
                        }
                    }
                    else
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

        private void ProcessSumTotal()
        {
            try
            {
                Mrs00329RDO rdo = new Mrs00329RDO();
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
                rdo.TOTAL_PATIENT_PRICE = (ListRdoA.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoB.Sum(s => s.TOTAL_PATIENT_PRICE) + ListRdoC.Sum(s => s.TOTAL_PATIENT_PRICE));
                rdo.TOTAL_HEIN_PRICE = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE));
                rdo.TOTAL_HEIN_PRICE_NDS = (ListRdoA.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoB.Sum(s => s.TOTAL_HEIN_PRICE_NDS) + ListRdoC.Sum(s => s.TOTAL_HEIN_PRICE_NDS));
                ListSumTotal.Add(rdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
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

        private bool checkBhytNsd(Mrs00329RDO rdo)
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("AMOUNT_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(TotalAmount).ToString()));

                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("BRANCH_NAME", this._Branch.BRANCH_NAME);
                dicSingleTag.Add("BRANCH_CODE", this._Branch.BRANCH_CODE);
                dicSingleTag.Add("ADDRESS", this._Branch.ADDRESS);

                ProcessSumTotal();

                bool exportSuccess = true;

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeAs", ListRdoA);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeBs", ListRdoB);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "PatientTypeCs", ListRdoC);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SumTotals", ListSumTotal);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        /// <summary>
        /// Lay patient_type_alter dua vao treatmen_id
        /// </summary>
        /// <param name="treatmentIds"></param>
        /// <returns></returns>
        private void GetPatientTypeAlterAndTreatment(List<V_HIS_TREATMENT> treatments, ref List<V_HIS_TREATMENT> treatmentsToUse, ref List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlterToUse)
        {
            patientTypeAlterToUse = new List<V_HIS_PATIENT_TYPE_ALTER>();
            treatmentsToUse = new List<V_HIS_TREATMENT>();
            try
            {
                if (IsNotNullOrEmpty(treatments))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = treatments.Count;
                    List<V_HIS_PATIENT_TYPE_ALTER> tmp = new List<V_HIS_PATIENT_TYPE_ALTER>();
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var lst = treatments.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                        filter.TREATMENT_IDs = lst.Select(o => o.ID).ToList();
                        filter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter);

                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            tmp.AddRange(patientTypeAlters);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (IsNotNullOrEmpty(tmp))
                    {
                        foreach (V_HIS_TREATMENT treatment in treatments)
                        {
                            V_HIS_PATIENT_TYPE_ALTER p = tmp
                                .Where(o => o.TREATMENT_ID == treatment.ID)
                                .OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).FirstOrDefault();
                            if (p != null)
                            {
                                patientTypeAlterToUse.Add(p);
                                treatmentsToUse.Add(treatment);
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

        private List<V_HIS_TREATMENT> GetTreatment()
        {

            return new ManagerSql().GetTreatment(this.castFilter);
        }

        private List<V_HIS_SERE_SERV_3> GetSereServ(List<V_HIS_TREATMENT> treatments)
        {
            List<V_HIS_SERE_SERV_3> result = new List<V_HIS_SERE_SERV_3>();
            int start = 0;
            int count = treatments.Count;
            while (count > 0)
            {
                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                var tmp = treatments.Skip(start).Take(limit).ToList();

                HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                ssFilter.TREATMENT_IDs = tmp.Select(s => s.ID).ToList();
                ssFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                List<V_HIS_SERE_SERV_3> sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView3(ssFilter);
                if (IsNotNullOrEmpty(sereServs))
                {
                    result.AddRange(sereServs);
                }
                start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
            }
            return result;
        }
    }
}
