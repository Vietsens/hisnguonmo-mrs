using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisPatientTypeAlter; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTreatmentEndType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
namespace MRS.Processor.Mrs00607
{
    public class Mrs00607Processor : AbstractProcessor
    {
        Mrs00607Filter castFilter = null; 
        List<Mrs00607RDO> ListRdo = new List<Mrs00607RDO>();
        List<Mrs00607RDO> ListMediMateRdo = new List<Mrs00607RDO>();
        List<HIS_TREATMENT> ListHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_TREATMENT_END_TYPE> listHisTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        List<HIS_TRANSACTION> listTransaction = new List<HIS_TRANSACTION>();
        List<V_HIS_EXP_MEST_MEDICINE> listMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        public Mrs00607Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00607Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
              
                CommonParam paramGet = new CommonParam();
                List<V_HIS_EXP_MEST> listExpMest = new List<V_HIS_EXP_MEST>();
                castFilter = ((Mrs00607Filter)this.reportFilter);  
                
                Inventec.Common.Logging.LogSystem.Debug("Lay danh sach V_HIS_TREATMENT, MRS00607, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                if (castFilter.DATE != null)
                {
                    castFilter.TIME_FROM = castFilter.DATE + 235959;
                    castFilter.TIME_TO = castFilter.DATE + 235959;
                }
               
                if (castFilter.IS_END == true)
                {
                    castFilter.TIME_FROM = castFilter.TIME_TO;
                }
                HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                treatFilter.IS_PAUSE = true;
                treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.IN_TIME_TO = castFilter.TIME_TO;
                var treatments = new HisTreatmentManager(paramGet).Get(treatFilter);
                if (treatments != null)
                {
                    ListHisTreatment.AddRange(treatments);
                }
                HisTreatmentFilterQuery treatingFilter = new HisTreatmentFilterQuery();
                treatingFilter.IS_PAUSE = false;
                //treatingFilter.IN_TIME_FROM = castFilter.TIME_FROM;
                treatingFilter.IN_TIME_TO = castFilter.TIME_TO;
                var treatmentss = new HisTreatmentManager(paramGet).Get(treatingFilter);
                if (treatmentss != null)
                {
                    ListHisTreatment.AddRange(treatmentss);
                }

                if (castFilter.TREATMENT_TYPE_IDs != null)
                {
                    ListHisTreatment = ListHisTreatment.Where(o => castFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID??0)).ToList();
                }

                if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                {
                    ListHisTreatment = ListHisTreatment.Where(p => castFilter.TDL_PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }

                if (castFilter.INPUT_DATA_ID_PATIENT_STT == 1) //chưa ra viện
                {
                    ListHisTreatment = ListHisTreatment.Where(p => p.IS_PAUSE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                }
                if (ListHisTreatment != null)
                {
                    var skip = 0;
                    var treatmentIds = ListHisTreatment.Select(o => o.ID).ToList();
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranFilterQuery filter = new HisDepartmentTranFilterQuery();
                        filter.TREATMENT_IDs = listIds;
                        var listSub = new HisDepartmentTranManager(param).Get(filter);
                        if (listSub != null)
                        {
                            listHisDepartmentTran.AddRange(listSub);
                        }
                    }
                }

                if (castFilter.DEPARTMENT_IDs != null)
                {
                    var dicDepartmentTran = listHisDepartmentTran.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                    var dicNextDepartmentTran = listHisDepartmentTran.Where(o => o.PREVIOUS_ID.HasValue && o.DEPARTMENT_IN_TIME > 0).GroupBy(g => g.PREVIOUS_ID).ToDictionary(p => p.Key, q => q.First());
                    ListHisTreatment = ListHisTreatment.Where(o => dicDepartmentTran.ContainsKey(o.ID) && dicDepartmentTran[o.ID].Count(p => castFilter.DEPARTMENT_IDs.Contains(p.DEPARTMENT_ID) && p.DEPARTMENT_IN_TIME <= castFilter.TIME_TO && (!dicNextDepartmentTran.ContainsKey(p.ID) || dicNextDepartmentTran.ContainsKey(p.ID) && dicNextDepartmentTran[p.ID].DEPARTMENT_IN_TIME >= castFilter.TIME_FROM)) > 0).ToList();
                }
                if (ListHisTreatment != null)
                {
                    var skip = 0;
                    var treatmentIds = ListHisTreatment.Select(o => o.ID).ToList();
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIds = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = listIds;
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub != null)
                        {
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                        }
                        HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                        sereServFilter.TREATMENT_IDs = listIds;
                        var sereServs =  new HisSereServManager(paramGet).Get(sereServFilter);
                        listSereServ.AddRange(sereServs);

                        HisTransactionFilterQuery transacFilter = new HisTransactionFilterQuery();
                        transacFilter.TREATMENT_IDs = listIds;
                        transacFilter.TRANSACTION_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT;
                        var listTransSub = new HisTransactionManager().Get(transacFilter);
                        if (listTransSub != null)
                        {
                            listTransaction.AddRange(listTransSub);
                        }

                        HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
                        expFilter.TDL_TREATMENT_IDs = listIds;
                        var listExpMestSub = new HisExpMestManager().GetView(expFilter);
                        if (listExpMestSub != null)
                        {
                            FilterExpMestByTime(ref listExpMestSub);
                            listExpMest.AddRange(listExpMestSub);
                        }
                    }

                    if (listExpMest != null)
                    {
                        List<long> expMestIds = listExpMest.Select(p => p.ID).ToList();
                        skip = 0;
                        while (expMestIds.Count - skip > 0)
                        {
                            var listIds = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                            mediFilter.EXP_MEST_IDs = listIds;
                            var listMediSub = new HisExpMestMedicineManager().GetView(mediFilter);
                            if (listMediSub != null)
                            {
                                listMedicine.AddRange(listMediSub);
                            }

                            HisExpMestMaterialViewFilterQuery mateFilter = new HisExpMestMaterialViewFilterQuery();
                            mateFilter.EXP_MEST_IDs = listIds;
                            var listMateSub = new HisExpMestMaterialManager().GetView(mateFilter);
                            if (listMateSub != null)
                            {
                                listMaterial.AddRange(listMateSub);
                            }
                        }
                    }
                    lastPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).GroupBy(q => q.TREATMENT_ID).Select(r => r.Last()).ToList();
                    //if (castFilter.DEPARTMENT_IDs != null)
                    //{
                    //    ListHisTreatment = ListHisTreatment.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && castFilter.DEPARTMENT_IDs.Contains(p.DEPARTMENT_ID))).ToList();
                    //}

                    if (castFilter.INPUT_DATA_ID_PATIENT_STT == 2) //đã thanh toán
                    {
                        ListHisTreatment = ListHisTreatment.Where(p => listTransaction.Exists(o => o.TREATMENT_ID == p.ID)).ToList();
                    }

                    if (castFilter.INPUT_DATA_ID_PATIENT_STT == 3) //đã ra viện nhưng chưa thanh toán
                    {
                        ListHisTreatment = ListHisTreatment.Where(p => p.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && !listTransaction.Exists(o => o.TREATMENT_ID == p.ID)).ToList();
                    }
                }
                listHisTreatmentEndType = new HisTreatmentEndTypeManager(new CommonParam()).Get(new HisTreatmentEndTypeFilterQuery());
                //var treatIds = ListHisTreatment.Select(x=>x.ID).ToList();
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void FilterExpMestByTime(ref List<V_HIS_EXP_MEST> listExpMestSub)
        {
            if (castFilter.DATE != null)
            {
                listExpMestSub = listExpMestSub.Where(o => o.TDL_INTRUCTION_TIME <= castFilter.DATE + 235959).ToList();
            }
            else if (castFilter.TIME_FROM != null || castFilter.TIME_TO != null)
            {
                if (castFilter.IS_END == true)
                {
                    listExpMestSub = listExpMestSub.Where(o => o.TDL_INTRUCTION_TIME <= castFilter.TIME_TO).ToList();
                }
                else
                {
                    listExpMestSub = listExpMestSub.Where(o => o.TDL_INTRUCTION_TIME <= castFilter.TIME_TO && o.TDL_INTRUCTION_TIME >= castFilter.TIME_FROM).ToList();
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            castFilter = ((Mrs00607Filter)this.reportFilter);
            try
            {
                
                if (IsNotNullOrEmpty(ListHisTreatment))
                {
                    var dicDepartmentTran = listHisDepartmentTran.GroupBy(g => g.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());

                    foreach (var treatment in ListHisTreatment)
                    {
                        var treatmentEndType = listHisTreatmentEndType.FirstOrDefault(o => o.ID == treatment.TREATMENT_END_TYPE_ID) ?? new HIS_TREATMENT_END_TYPE();

                        Mrs00607RDO rdo = new Mrs00607RDO();

                        if (dicDepartmentTran.ContainsKey(treatment.ID))
                        {
                            var departmentTran = dicDepartmentTran[treatment.ID].Where(p =>  p.DEPARTMENT_IN_TIME <= castFilter.TIME_TO).OrderBy(o => o.DEPARTMENT_IN_TIME).ThenBy(p => p.ID).LastOrDefault(q => q.TREATMENT_ID == treatment.ID);
                            if (departmentTran != null)
                            {
                                rdo.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTran.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                                rdo.DEPARTMENT_ID = departmentTran.DEPARTMENT_ID;
                            }
                        }
                        
                        rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        rdo.TREATMENT_END_TYPE_CODE = treatmentEndType.TREATMENT_END_TYPE_CODE;
                        rdo.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        rdo.TDL_PATIENT_PHONE = treatment.TDL_PATIENT_PHONE??treatment.TDL_PATIENT_MOBILE;
                        rdo.VIR_ADDRESS = treatment.TDL_PATIENT_ADDRESS;
                        rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                        rdo.JOB = treatment.TDL_PATIENT_CAREER_NAME;
                        rdo.DATE_IN_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.IN_TIME);
                        rdo.IN_TIME = treatment.IN_TIME;
                        IsBhyt(rdo, treatment);
                        CalcuatorAge(rdo, treatment);
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
                        if (treatment.OUT_TIME.HasValue)
                        {
                            if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                            {
                                rdo.DATE_TRIP_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                            }
                            else if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET)
                            {
                                rdo.DATE_DEAD_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);

                                if (treatment.DEATH_WITHIN_ID == HisDeathWithinCFG.DEATH_WITHIN_ID__24HOURS)
                                {
                                    rdo.IS_DEAD_IN_24H = "X";
                                }
                            }
                            else
                            {
                                rdo.DATE_OUT_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.OUT_TIME.Value);
                            }

                            rdo.TOTAL_DATE_TREATMENT = DateDiff.diffDate(treatment.IN_TIME, treatment.OUT_TIME.Value);
                        }
                        if (castFilter.IS_CACULATION_PRICE== true)
                        {
                            var listSere = listSereServ.Where(x => x.TDL_TREATMENT_ID == treatment.ID).ToList();
                            IsCaculatorPrice(rdo, listSere);
                        }
                        ListRdo.Add(rdo);
                    }

                    ProcessMediMate(listMedicine, listMaterial);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessMediMate(List<V_HIS_EXP_MEST_MEDICINE> listMedi, List<V_HIS_EXP_MEST_MATERIAL> listMate)
        {
            try
            {
                if (listMedi != null)
                {
                    var group = listMedi.GroupBy(p => new { p.MEDICINE_TYPE_CODE, p.MEDICINE_TYPE_NAME, p.IMP_PRICE, p.IMP_VAT_RATIO }).ToList();
                    foreach (var item in group)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listSub = item.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00607RDO rdo = new Mrs00607RDO();
                        rdo.MEDI_MATE_TYPE_CODE = listSub[0].MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = listSub[0].MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = listSub[0].CONCENTRA;
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        rdo.TOTAL_MEDI_MATE_PRICE = listSub.Sum(p => p.AMOUNT * p.IMP_PRICE * (1 + p.IMP_VAT_RATIO));
                        rdo.TYPE = "THUỐC";
                        ListMediMateRdo.Add(rdo);
                    }
                }

                if (listMate != null)
                {
                    var group = listMate.GroupBy(p => new { p.MATERIAL_TYPE_CODE, p.MATERIAL_TYPE_NAME, p.IMP_PRICE, p.IMP_VAT_RATIO }).ToList();
                    foreach (var item in group)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listSub = item.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00607RDO rdo = new Mrs00607RDO();
                        rdo.MEDI_MATE_TYPE_CODE = listSub[0].MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = listSub[0].MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = "";
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        rdo.TOTAL_MEDI_MATE_PRICE = listSub.Sum(p => p.AMOUNT * p.IMP_PRICE * (1 + p.IMP_VAT_RATIO));
                        if (listSub[0].IS_CHEMICAL_SUBSTANCE == 1)
                        {
                            rdo.TYPE = "HÓA CHẤT";
                        }
                        else
                        {
                            rdo.TYPE = "VẬT TƯ";
                        }
                        ListMediMateRdo.Add(rdo);
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }
        private void IsCaculatorPrice(Mrs00607RDO rdo, List<HIS_SERE_SERV> sere)
        {
            try
            {
                foreach (var item in sere)
                {
                    rdo.TOTAL_PRICE += (item.PRICE * (1 + item.VAT_RATIO)) * item.AMOUNT;
                    var patient_price = ((item.PRICE * (1 + item.VAT_RATIO)) * item.AMOUNT) - ((item.PRICE * (1 + item.VAT_RATIO)) * item.AMOUNT) * (item.HEIN_RATIO ?? 1);
                    var hein_price = ((item.PRICE * (1 + item.VAT_RATIO)) * item.AMOUNT) * (item.HEIN_RATIO ?? 1);
                    rdo.TOTAL_PATIENT_PRICE += patient_price;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void CalcuatorAge(Mrs00607RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1;
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
        
        private void IsBhyt(Mrs00607RDO rdo, HIS_TREATMENT treatment)
        {
            try
            {
                var patientTypeAlter = lastPatientTypeAlter.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID);
                if (patientTypeAlter!=null)
                {

                    if (patientTypeAlter.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.IS_BHYT = "X";
                            rdo.HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER;
                            rdo.DIAGNOSE_TUYENDUOI = treatment.TRANSFER_IN_ICD_NAME;
                            rdo.ICD_CODE_TUYENDUOI = treatment.TRANSFER_IN_ICD_CODE;
                            rdo.GIOITHIEU = treatment.TRANSFER_IN_MEDI_ORG_NAME;
                        }
                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM)
                        {
                            rdo.DIAGNOSE_KKB = treatment.ICD_NAME ?? treatment.ICD_TEXT;
                            rdo.ICD_CODE_KKB = treatment.ICD_CODE ?? treatment.ICD_TEXT;
                        }
                        else
                        {
                            rdo.DIAGNOSE_KDT = treatment.ICD_NAME ?? treatment.ICD_TEXT;
                            rdo.ICD_CODE_KDT = treatment.ICD_CODE ?? treatment.ICD_TEXT;
                        }
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

                if (castFilter.TIME_FROM != null)
                {
                    dicSingleTag.Add("DATE", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0) + " Đến " + Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                    
                }
                else
                {
                    dicSingleTag.Add("DATE", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.DATE ?? 0));
                }

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
                if (castFilter.INPUT_DATA_ID_PATIENT_STT == 1)
                {
                    dicSingleTag.Add("PATIENT_STT_NAME", "Chưa ra viện");
                }
                else if (castFilter.INPUT_DATA_ID_PATIENT_STT == 2)
                {
                    dicSingleTag.Add("PATIENT_STT_NAME", "Đã thanh toán");
                }
                else if (castFilter.INPUT_DATA_ID_PATIENT_STT == 3)
                {
                    dicSingleTag.Add("PATIENT_STT_NAME", "Đã ra viện nhưng chưa thanh toán");
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", String.Join(",", (HisDepartmentCFG.DEPARTMENTs ?? new List<HIS_DEPARTMENT>()).Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).Select(o => o.DEPARTMENT_NAME).ToList()));
                }
                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_CODE).ToList(); 
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "MediMate", ListMediMateRdo.OrderBy(p => p.MEDI_MATE_TYPE_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
