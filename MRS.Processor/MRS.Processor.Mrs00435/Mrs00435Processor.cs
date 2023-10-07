using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00435
{
    class Mrs00435Processor : AbstractProcessor
    {
        Mrs00435Filter castFilter = null; 
        List<Mrs00435RDO> listRdo = new List<Mrs00435RDO>(); 
        private string thisReportTypeCode = "MRS00435"; 

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_TREATMENT_FEE> listTreatmentFees = new List<V_HIS_TREATMENT_FEE>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 

        List<long> listXNs = new List<long>(); 
        List<long> listXQCCs = new List<long>(); 
        List<long> listXQNCCs = new List<long>(); 
        List<long> listCTs = new List<long>(); 
        List<long> listDNDs = new List<long>(); 
        List<long> listDTDs = new List<long>(); 
        List<long> listHCGs = new List<long>(); 
        List<long> listHIVs = new List<long>(); 
        List<long> listAPTMs = new List<long>(); 
        List<long> listSBAs = new List<long>(); 

        public Mrs00435Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00435Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00435Filter)this.reportFilter; 

                HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                treatmentViewFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatmentViewFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter); 

                ProcessPatyAlter(); 

                var skip = 0; 
                while (listTreatments.Count - skip > 0)
                {
                    var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                    listPatientTypeAlters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));  

                    HisTreatmentFeeViewFilterQuery treatmentFeeViewFilter = new HisTreatmentFeeViewFilterQuery(); 
                    treatmentFeeViewFilter.PATIENT_IDs = listIds.Select(s => s.PATIENT_ID).ToList(); 
                    listTreatmentFees.AddRange(new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetFeeView(treatmentFeeViewFilter));  

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                    sereServViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                    sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                    sereServViewFilter.HAS_EXECUTE = true;
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter));  
                }

                HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery(); 
                serviceRetyCatViewFilter.REPORT_TYPE_CODE__EXACT = thisReportTypeCode; 
                var listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter); 

                listXNs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_XN").Select(s => s.SERVICE_ID).ToList(); 
                listXQCCs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_XQCC").Select(s => s.SERVICE_ID).ToList(); 
                listXQNCCs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_XQNCC").Select(s => s.SERVICE_ID).ToList(); 
                listCTs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_CT").Select(s => s.SERVICE_ID).ToList(); 
                listDNDs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_DND").Select(s => s.SERVICE_ID).ToList(); 
                listDTDs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_DTD").Select(s => s.SERVICE_ID).ToList(); 
                listHCGs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_HCG").Select(s => s.SERVICE_ID).ToList(); 
                listHIVs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_HIV").Select(s => s.SERVICE_ID).ToList(); 
                listAPTMs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_APTM").Select(s => s.SERVICE_ID).ToList(); 
                listSBAs = listServiceRetyCats.Where(w => w.CATEGORY_CODE == "435_SBA").Select(s => s.SERVICE_ID).ToList(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected void ProcessPatyAlter()
        {
            var skip = 0; 
            var listPatyAlterFilters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
            while (listTreatments.Count - skip > 0)
            {
                var listIds = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM); 
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                patientTypeAlterViewFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList(); 
                if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                    patientTypeAlterViewFilter.PATIENT_TYPE_IDs = castFilter.PATIENT_TYPE_IDs; 
                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                    patientTypeAlterViewFilter.TREATMENT_TYPE_IDs = castFilter.TREATMENT_TYPE_IDs;
                listPatyAlterFilters.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));  
            }

            listTreatments = listTreatments.Where(w => listPatyAlterFilters.Select(s => s.TREATMENT_ID).Contains(w.ID)).ToList(); 
        }

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 

                if(IsNotNullOrEmpty(listTreatments))
                {
                    foreach(var treatment in listTreatments)
                    {
                        var patyAlters = listPatientTypeAlters.Where(w => w.TREATMENT_ID == treatment.ID).ToList(); 
                        if(IsNotNullOrEmpty(patyAlters))
                        {
                            var patyAlter = patyAlters.OrderByDescending(o => o.LOG_TIME).First(); 
                            if((castFilter.PATIENT_TYPE_IDs == null || IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs.Where(w=>w==patyAlter.PATIENT_TYPE_ID).ToList())) && (castFilter.TREATMENT_TYPE_IDs == null || IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs.Where(w=>w==patyAlter.TREATMENT_TYPE_ID).ToList())))
                            {
                                var rdo = new Mrs00435RDO(); 
                                rdo.TREATMENT = treatment; 
                                rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                                rdo.ADDRESS = treatment.TDL_PATIENT_ADDRESS; 
                                rdo.STORE_CODE = treatment.STORE_CODE; 
                                rdo.END_DEPARTMENT_NAME = treatment.END_DEPARTMENT_NAME; 
                                var sereServs = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID).ToList(); 
                                if(IsNotNullOrEmpty(sereServs))
                                {
                                    rdo.MONEY_EXAM = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_BED = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_TEST = sereServs.Where(w => listXNs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_XQUANG_CC = sereServs.Where(w => listXQCCs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_XQUANG = sereServs.Where(w => listXQNCCs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_CT = sereServs.Where(w => listCTs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_SA = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_DND = sereServs.Where(w => listDNDs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_DTD = sereServs.Where(w => listDTDs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_NS = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_TT = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_MEDICINE = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_MATERIAL = sereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_HCG = sereServs.Where(w => listHCGs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_HIV = sereServs.Where(w => listHIVs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_APHETAMIN = sereServs.Where(w => listAPTMs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 
                                    rdo.MONEY_SAO_BA = sereServs.Where(w => listSBAs.Contains(w.SERVICE_ID)).Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    rdo.MONEY_DCT_5 = sereServs.Where(w => w.HEIN_RATIO == (decimal)0.95).Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0); 
                                    rdo.MONEY_DCT_20 = sereServs.Where(w => w.HEIN_RATIO == (decimal)0.8).Sum(su => su.VIR_TOTAL_HEIN_PRICE ?? 0); 

                                    rdo.TOTAL_PRICE_2 = sereServs.Sum(su => su.VIR_TOTAL_PATIENT_PRICE ?? 0); 

                                    var treatmentFee = listTreatmentFees.Where(w => w.TREATMENT_CODE.Equals(treatment.TREATMENT_CODE)).ToList(); 
                                    if (IsNotNullOrEmpty(treatmentFee))
                                    {
                                        rdo.TREATMENT_FEE = treatmentFee.First(); 

                                        rdo.MONEY_FREE = treatmentFee.First().TOTAL_BILL_EXEMPTION ?? 0; 
                                        rdo.TOTAL_PRICE = treatmentFee.First().TOTAL_PRICE ?? 0; 
                                        rdo.DCT = treatmentFee.First().TOTAL_HEIN_PRICE ?? 0; 
                                    }

                                    listRdo.Add(rdo); 
                                }
                            }
                        }
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
                    dicSingleTag.Add("DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                var listTreatmentTypes = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager(param).Get(new HisTreatmentTypeFilterQuery()); 
                if (IsNotNullOrEmpty(castFilter.TREATMENT_TYPE_IDs))
                    listTreatmentTypes = listTreatmentTypes.Where(w => castFilter.TREATMENT_TYPE_IDs.Contains(w.ID)).ToList(); 

                dicSingleTag.Add("TREATMENT_TYPE_NAME", String.Join(", ", listTreatmentTypes.Select(s => s.TREATMENT_TYPE_NAME.ToUpper()).ToList())); 

                var listPatientTypes = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(param).Get(new HisPatientTypeFilterQuery()); 
                if (IsNotNullOrEmpty(castFilter.PATIENT_TYPE_IDs))
                    listPatientTypes = listPatientTypes.Where(w => castFilter.PATIENT_TYPE_IDs.Contains(w.ID)).ToList(); 

                dicSingleTag.Add("PATIENT_TYPE_NAME", String.Join(", ", listPatientTypes.Select(s => s.PATIENT_TYPE_NAME.ToUpper()).ToList())); 

                bool exportSuccess = true; 
                objectTag.AddObjectData(store, "Report", listRdo); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
