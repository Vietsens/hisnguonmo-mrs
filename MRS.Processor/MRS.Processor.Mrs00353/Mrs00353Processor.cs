using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMedicineType;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00353
{
    public class Mrs00353Processor : AbstractProcessor
    {
        Mrs00353Filter castFilter = null;

        List<Mrs00353RDO> ListRdoGeneric = new List<Mrs00353RDO>();
        List<Mrs00353RDO> ListRdoCpyhct = new List<Mrs00353RDO>();
        List<Mrs00353RDO> ListRdoVtyhct = new List<Mrs00353RDO>();
        List<Mrs00353RDO> ListRdoOther = new List<Mrs00353RDO>();

        List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();

        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_3> sereServs = new List<V_HIS_SERE_SERV_3>();

        List<long> listHeinServiceTypeId;

        public Mrs00353Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00353Filter);
        }

        string NumDigits = NumDigitsOptionCFG.NUM_DIGITS_OPTION_VALUE;
        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00353Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, MRS00353, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                List<V_HIS_TREATMENT> tmp = this.GetTreatment();

                ///lay du lieu patient_type_alter va treament
                this.GetPatientTypeAlterAndTreatment(tmp, ref this.treatments, ref this.patientTypeAlters);
                this.sereServs = this.GetSereServ(this.treatments);
                this.dicMedicineType = this.GetMedicineType();

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu, MRS00353");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private Dictionary<long, V_HIS_MEDICINE_TYPE> GetMedicineType()
        {
            var listMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
            Dictionary<long, V_HIS_MEDICINE_TYPE> result = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
            if (IsNotNullOrEmpty(listMedicineType))
            {
                foreach (var medicine in listMedicineType)
                {
                    result[medicine.ID] = medicine;
                }
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(this.patientTypeAlters))
                {
                    listHeinServiceTypeId = new List<long>();
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM);
                    listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL);

                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU);
                    }
                    if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__THUOC)
                    {
                        listHeinServiceTypeId.Add(IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC);
                    }

                    GeneralDataByListPatientTypeAlter(this.patientTypeAlters, this.sereServs);

                    if (IsNotNullOrEmpty(ListRdoCpyhct))
                    {
                        ListRdoCpyhct = ProcessListRDO(ListRdoCpyhct);
                    }

                    if (IsNotNullOrEmpty(ListRdoGeneric))
                    {
                        ListRdoGeneric = ProcessListRDO(ListRdoGeneric);
                    }

                    if (IsNotNullOrEmpty(ListRdoOther))
                    {
                        ListRdoOther = ProcessListRDO(ListRdoOther);
                    }

                    if (IsNotNullOrEmpty(ListRdoVtyhct))
                    {
                        ListRdoVtyhct = ProcessListRDO(ListRdoVtyhct);
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

        private void GeneralDataByListPatientTypeAlter(List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters, List<V_HIS_SERE_SERV_3> listSereServ)
        {
            try
            {
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    foreach (var hisPatientTypeAlter in hisPatientTypeAlters)
                    {
                        dicPatientTypeAlter[hisPatientTypeAlter.TREATMENT_ID] = hisPatientTypeAlter;
                    }
                }

                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var sereServ in listSereServ)
                    {
                        if (sereServ.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sereServ.AMOUNT <= 0 || sereServ.TDL_HEIN_SERVICE_TYPE_ID == null)
                            continue;
                        if (!listHeinServiceTypeId.Contains(sereServ.TDL_HEIN_SERVICE_TYPE_ID.Value))
                            continue;
                        Mrs00353RDO rdo = new Mrs00353RDO();
                        rdo.SERVICE_ID = sereServ.SERVICE_ID;
                        rdo.MEDICINE_SODANGKY_NAME = sereServ.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.MEDICINE_STT_DMBYT = sereServ.TDL_HEIN_ORDER;
                        rdo.MEDICINE_TYPE_NAME = sereServ.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.MEDICINE_HAMLUONG_NAME = sereServ.MEDICINE_TYPE_CONCENTRA;
                        rdo.MEDICINE_UNIT_NAME = sereServ.SERVICE_UNIT_NAME;
                        rdo.VIR_PRICE = sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO);
                        rdo.BHYT_PAY_RATE = Math.Round(sereServ.ORIGINAL_PRICE > 0 ? (sereServ.HEIN_LIMIT_PRICE.HasValue ? (sereServ.HEIN_LIMIT_PRICE.Value / (sereServ.ORIGINAL_PRICE * (1 + sereServ.VAT_RATIO))) * 100 : (sereServ.PRICE / sereServ.ORIGINAL_PRICE) * 100) : 0, 0);
                        rdo.TOTAL_OTHER_SOURCE_PRICE = (sereServ.OTHER_SOURCE_PRICE ?? 0) * sereServ.AMOUNT;

                        if (sereServ.TDL_TREATMENT_ID > 0 && dicPatientTypeAlter.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
                        {
                            if (dicPatientTypeAlter[sereServ.TDL_TREATMENT_ID ?? 0].HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sereServ.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU = sereServ.AMOUNT;
                            }
                            rdo.VIR_TOTAL_PRICE = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(NumDigits,sereServ, HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == ListHeinApproval.FirstOrDefault(p => p.ID == sereServ.HEIN_APPROVAL_ID).BRANCH_ID) ?? new HIS_BRANCH());
                            if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ListRdoOther.Add(rdo);
                            }
                            else
                            {
                                V_HIS_MEDICINE_TYPE medicineType = null;
                                if (sereServ.MEDICINE_TYPE_ID.HasValue && dicMedicineType.ContainsKey(sereServ.MEDICINE_TYPE_ID.Value))
                                {
                                    medicineType = dicMedicineType[sereServ.MEDICINE_TYPE_ID.Value];
                                }
                                if (IsNotNull(medicineType))
                                {

                                    if (String.IsNullOrEmpty(rdo.MEDICINE_HOATCHAT_NAME))
                                        rdo.MEDICINE_HOATCHAT_NAME = medicineType.ACTIVE_INGR_BHYT_NAME;

                                    if (String.IsNullOrEmpty(rdo.MEDICINE_CODE_DMBYT))
                                        rdo.MEDICINE_CODE_DMBYT = medicineType.ACTIVE_INGR_BHYT_CODE;

                                    rdo.MEDICINE_UNIT_NAME = medicineType.SERVICE_UNIT_NAME;
                                    rdo.MEDICINE_DUONGDUNG_NAME = medicineType.MEDICINE_USE_FORM_NAME;
                                    if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__TTD)
                                    {

                                        ListRdoGeneric.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__CP_YHCT)
                                    {
                                        ListRdoCpyhct.Add(rdo);
                                    }
                                    else if (medicineType.MEDICINE_LINE_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_LINE.ID__VT_YHCT)
                                    {
                                        ListRdoVtyhct.Add(rdo);
                                    }
                                }
                                else
                                {
                                    ListRdoOther.Add(rdo);
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

        private List<Mrs00353RDO> ProcessListRDO(List<Mrs00353RDO> listRDO)
        {
            List<Mrs00353RDO> listCurrent = new List<Mrs00353RDO>();
            try
            {
                if (listRDO.Count > 0)
                {
                    var groupRDOs = listRDO.GroupBy(o => new { o.SERVICE_ID, o.VIR_PRICE, o.MEDICINE_SODANGKY_NAME, o.BHYT_PAY_RATE }).ToList();
                    foreach (var group in groupRDOs)
                    {
                        List<Mrs00353RDO> listsub = group.ToList<Mrs00353RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00353RDO rdo = new Mrs00353RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.MEDICINE_CODE_DMBYT = listsub[0].MEDICINE_CODE_DMBYT;
                            rdo.MEDICINE_STT_DMBYT = listsub[0].MEDICINE_STT_DMBYT;
                            rdo.MEDICINE_TYPE_NAME = listsub[0].MEDICINE_TYPE_NAME;
                            rdo.MEDICINE_SODANGKY_NAME = listsub[0].MEDICINE_SODANGKY_NAME;
                            rdo.MEDICINE_HAMLUONG_NAME = listsub[0].MEDICINE_HAMLUONG_NAME;
                            rdo.MEDICINE_DUONGDUNG_NAME = listsub[0].MEDICINE_DUONGDUNG_NAME;
                            rdo.MEDICINE_HOATCHAT_NAME = listsub[0].MEDICINE_HOATCHAT_NAME;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            rdo.BHYT_PAY_RATE = listsub[0].BHYT_PAY_RATE;
                            rdo.MEDICINE_UNIT_NAME = listsub[0].MEDICINE_UNIT_NAME;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                                rdo.TOTAL_OTHER_SOURCE_PRICE += item.TOTAL_OTHER_SOURCE_PRICE;
                            }
                            listCurrent.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.MEDICINE_STT_DMBYT).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                Decimal Total = 0;
                if (ListRdoGeneric.Count > 0) Total += ListRdoGeneric.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoCpyhct.Count > 0) Total += ListRdoCpyhct.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoVtyhct.Count > 0) Total += ListRdoVtyhct.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListRdoOther.Count > 0) Total += ListRdoOther.Sum(o => o.VIR_TOTAL_PRICE);
                if (Total > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(Total.ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng");
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineGeneric", ListRdoGeneric);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineCpyhct", ListRdoCpyhct);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineVtyhct", ListRdoVtyhct);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineOther", ListRdoOther);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<V_HIS_TREATMENT> GetTreatment()
        {
            ///lay du lieu treatment
            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
            treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
            treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
            treatmentFilter.IS_PAUSE = true;
            treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
            return new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetView(treatmentFilter);
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
    }
}
