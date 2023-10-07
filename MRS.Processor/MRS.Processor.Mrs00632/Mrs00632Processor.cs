using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisDepartment;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisDataStore;

namespace MRS.Processor.Mrs00632
{
    public class Mrs00632Processor : AbstractProcessor
    {
        Mrs00632Filter castFilter = null;
        List<Mrs00632RDO> ListRdo = new List<Mrs00632RDO>();
        Dictionary<string, HIS_ICD> dicIcd = new Dictionary<string, HIS_ICD>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_DATA_STORE> ListDataStore = new List<HIS_DATA_STORE>();
        

        public Mrs00632Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00632Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00632Filter)this.reportFilter);
                LoadDataToRam();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTreatmentViewFilterQuery treatmentFeeLockFilter = new HisTreatmentViewFilterQuery();
                treatmentFeeLockFilter.FEE_LOCK_TIME_FROM = this.castFilter.TIME_FROM;
                treatmentFeeLockFilter.FEE_LOCK_TIME_TO = this.castFilter.TIME_TO;
                treatmentFeeLockFilter.END_DEPARTMENT_IDs = this.castFilter.DEPARTMENT_IDs;
                treatmentFeeLockFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                var ListTreatmentFeeLock = new HisTreatmentManager().GetView(treatmentFeeLockFilter);
                if (ListTreatmentFeeLock != null)
                {
                    ListTreatment.AddRange(ListTreatmentFeeLock);
                }


                HisTreatmentViewFilterQuery treatmentStoreFilter = new HisTreatmentViewFilterQuery();
                treatmentStoreFilter.STORE_TIME_FROM = this.castFilter.TIME_FROM;
                treatmentStoreFilter.STORE_TIME_TO = this.castFilter.TIME_TO;
                treatmentStoreFilter.END_DEPARTMENT_IDs = this.castFilter.DEPARTMENT_IDs;
                treatmentStoreFilter.HAS_DATA_STORE = true;
                var ListTreatmentStore = new HisTreatmentManager().GetView(treatmentStoreFilter);
                if (ListTreatmentStore != null)
                {
                    ListTreatment.AddRange(ListTreatmentStore);
                }
                ListTreatment = ListTreatment.GroupBy(o => o.ID).Select(p => p.First()).ToList();
                 ListDataStore = new HisDataStoreManager(param).Get(new HisDataStoreFilterQuery());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListTreatment.Clear();
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (ListTreatment != null)
                {
                    foreach (var treatment in ListTreatment)
                    {
                        Mrs00632RDO rdo = new Mrs00632RDO();
                        rdo.V_HIS_TREATMENT = treatment;
                        rdo.TREATMENT_TYPE_NAME = (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == treatment.TDL_TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME;
                        
                        //var DataStoreID = ListTreatment.Select(o => o.DATA_STORE_ID).Distinct().ToList();
                       // var DataStore = ListDataStore.Where(o => DataStoreIDs.Contains(o.ID)).ToList();
                       var DataStore = ListDataStore.Where(x => x.ID == treatment.DATA_STORE_ID).FirstOrDefault();
                       if (DataStore != null)
                       {
                           rdo.CREATORDATASTORE = DataStore.CREATOR;
                       }
                        
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.HAS_STORE = treatment.DATA_STORE_ID != null;
                        rdo.END_DEPARTMENT_ID = treatment.END_DEPARTMENT_ID;
                        rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME;
                        rdo.IS_BHYT = (treatment.TDL_HEIN_CARD_NUMBER != null) ? "X" : "";

                        if (rdo.HAS_STORE == true)
                        {

                            rdo.DALUUTRU = "X";
                        
                        }
                        if (rdo.HAS_STORE == false)
                        {

                            rdo.CHUALUUTRU = "X";

                        }

                        SetDateTreatMent(ref rdo, treatment);
                        CalcuatorAge(rdo, treatment);
                        SetJobForPatient(rdo, treatment);
                        if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI)
                        {
                            rdo.IS_CURED = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO)
                        {
                            rdo.IS_ABATEMENT = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD)
                        {
                            rdo.IS_UNCHANGED = "X";
                        }
                        else if (treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG)
                        {
                            rdo.IS_AGGRAVATION = "X";
                        }
                        ListRdo.Add(rdo);
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

        private void SetDateTreatMent(ref Mrs00632RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                if (treatment != null)
                {
                    if (treatment.CLINICAL_IN_TIME.HasValue)
                    {
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.CLINICAL_IN_TIME ?? 0);
                    }
                    else
                    {
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                    }

                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        rdo.DATE_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                    }
                    else
                    {
                        rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME ?? 0);
                    }
                    if (treatment.TDL_HEIN_CARD_NUMBER != null)
                    {
                        if (treatment.TRANSFER_IN_ICD_NAME != null && treatment.TRANSFER_IN_ICD_NAME != "")
                        {
                            rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                        }
                        else
                        {
                            rdo.DIAGNOSE_TUYENDUOI = (treatment.TRANSFER_IN_ICD_CODE != null && dicIcd.ContainsKey(treatment.TRANSFER_IN_ICD_CODE)) ? dicIcd[treatment.TRANSFER_IN_ICD_CODE].ICD_NAME : "";
                        }

                        rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;
                        rdo.GIOITHIEU = treatment.MEDI_ORG_NAME;
                    }
                    if (treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                    {
                        rdo.DIAGNOSE_KKB = treatment.ICD_NAME;
                    }
                    else
                    {
                        rdo.DIAGNOSE_KDT = treatment.ICD_NAME;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CalcuatorAge(Mrs00632RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = CalculAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.MALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
                        rdo.FEMALE_YEAR = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
                if (tuoi == 0)
                {
                    rdo.IS_DUOI_12THANG = "X";
                }
                else if (tuoi >= 1 && tuoi <= 15)
                {
                    rdo.IS_1DEN15TUOI = "X";
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private int? CalculAge(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private void SetJobForPatient(Mrs00632RDO rdo, V_HIS_TREATMENT treatment)
        {
            try
            {

                rdo.JOB = treatment.TDL_PATIENT_CAREER_NAME;
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
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }
                objectTag.AddObjectData(store, "Report", ListRdo.ToList());
                objectTag.AddObjectData(store, "ReportStore", ListRdo.Where(o => o.HAS_STORE == true).ToList());
                objectTag.AddObjectData(store, "ReportNoStore", ListRdo.Where(o => o.HAS_STORE == false).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
