using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisImpMestStt;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Core.MrsReport.RDO; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisImpMest; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisRoom; 

namespace MRS.Processor.Mrs00089
{
    public class Mrs00089Processor : AbstractProcessor
    {
        Mrs00089Filter castFilter = null;
        List<Mrs00089RDO> ListRdo = new List<Mrs00089RDO>();
        List<V_HIS_EXP_MEST> ListPrescription = new List<V_HIS_EXP_MEST>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();

        public Mrs00089Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00089Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                castFilter = ((Mrs00089Filter)this.reportFilter);
                ListRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                //LoadDataToRam(); 
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

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                ProcessListPrescription(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListPrescription()
        {
            try
            {
                if (ListPrescription.Count > 0)
                {
                    CommonParam paramGet = new CommonParam(); 
                    List<Mrs00089RDO> mrs89Rdos = new List<Mrs00089RDO>(); 
                    var Groups = ListPrescription.OrderBy(s => s.TDL_INTRUCTION_TIME).GroupBy(g => g.TDL_TREATMENT_ID).ToList(); 
                    foreach (var group in Groups)
                    {
                        List<V_HIS_EXP_MEST> listSub = group.ToList<V_HIS_EXP_MEST>(); 
                        if (listSub != null && listSub.Count > 0)
                        {
                            var treatment = listHisTreatment.FirstOrDefault(o => o.ID == listSub[0].TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                            var endRoom = ListRoom.Where(x => x.ID == treatment.END_ROOM_ID).FirstOrDefault();
                            Mrs00089RDO rdo = new Mrs00089RDO(); 
                            rdo.TREATMENT_ID = listSub[0].TDL_TREATMENT_ID??0; 
                            rdo.TREATMENT_CODE = listSub[0].TDL_TREATMENT_CODE; 
                            rdo.VIR_ADDRESS = listSub[0].TDL_PATIENT_ADDRESS; 
                            rdo.PATIENT_CODE = listSub[0].TDL_PATIENT_CODE; 
                            rdo.VIR_PATIENT_NAME = listSub[0].TDL_PATIENT_NAME; 
                            rdo.ICD_NAME = treatment.ICD_NAME; 
                            rdo.EXAM_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSub[0].TDL_INTRUCTION_TIME??0);
                            if (endRoom!=null)
                            {
                                rdo.END_ROOM_NAME = endRoom.ROOM_NAME;
                            }
                            CalcuatorAge(rdo, listSub[0]); 
                            ProcessListSubPrescription(paramGet, listSub, rdo); 
                            if (rdo.VIR_TOTAL_PRICE > 0)
                            {
                                mrs89Rdos.Add(rdo); 
                            }
                        }
                    }
                    ProcestListRdo(paramGet, mrs89Rdos); 
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu"); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void ProcessListSubPrescription(CommonParam paramGet, List<V_HIS_EXP_MEST> hisPrescriptions, Mrs00089RDO rdo)
        {
            try
            {
                int start = 0; 
                int count = hisPrescriptions.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    List<V_HIS_EXP_MEST> prescriptions = hisPrescriptions.Skip(start).Take(limit).ToList(); 

                    HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery(); 
                    expFilter.EXP_MEST_IDs = prescriptions.Select(s => s.ID).ToList(); 
                    expFilter.IS_EXPORT = true; 
                    //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    var hisExpMestMedis = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expFilter); 
                    if (hisExpMestMedis != null && hisExpMestMedis.Count > 0)
                    {
                        foreach (var item in hisPrescriptions)
                        {
                            var expMestMedis = hisExpMestMedis.Where(o => o.EXP_MEST_ID == item.ID).ToList(); 
                            if (expMestMedis != null && expMestMedis.Count > 0)
                            {
                                foreach (var exp in expMestMedis)
                                {
                                    rdo.VIR_TOTAL_PRICE += (exp.AMOUNT * exp.IMP_PRICE); 
                                }
                            }
                        }
                    }

                    HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery(); 
                    mobaFilter.MOBA_EXP_MEST_IDs = prescriptions.Select(s => s.ID).ToList(); 
                    var hisMobaImpMests = new HisImpMestManager(paramGet).GetView(mobaFilter); 
                    if (hisMobaImpMests != null && hisMobaImpMests.Count > 0)
                    {
                        ProcessListMobaImpMest(paramGet, hisMobaImpMests, rdo); 
                    }

                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListMobaImpMest(CommonParam paramGet, List<V_HIS_IMP_MEST> listMoba, Mrs00089RDO rdo)
        {
            try
            {
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedis = new List<V_HIS_IMP_MEST_MEDICINE>(); 
                int start = 0; 
                int count = listMoba.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    List<V_HIS_IMP_MEST> listSub = listMoba.Skip(start).Take(limit).ToList(); 
                    HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery(); 
                    impMediFilter.IMP_MEST_IDs = listSub.Select(s => s.ID).ToList(); 
                    impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT; 
                    //Config.HisImpMestSttCFG.IMP_MEST_STT_ID__IMPORTED; 
                    var impMestMedis = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter); 
                    if (impMestMedis != null && impMestMedis.Count > 0)
                    {
                        foreach (var item in impMestMedis)
                        {
                            rdo.VIR_TOTAL_PRICE -= (item.AMOUNT * item.IMP_PRICE); 
                        }
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Debug("Loi Co MobaImpMest nhung khong co HisImpMestMedicine:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => impMediFilter), impMediFilter)); 
                    }
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcestListRdo(CommonParam paramGet, List<Mrs00089RDO> mrs89Rdos)
        {
            try
            {
                int start = 0; 
                int count = mrs89Rdos.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var rdos = mrs89Rdos.Skip(start).Take(limit).ToList(); 
                    HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery(); 
                    filter.TREATMENT_IDs = rdos.Select(s => s.TREATMENT_ID).ToList(); 
                    filter.ORDER_DIRECTION = "DESC"; 
                    filter.ORDER_FIELD = "LOG_TIME"; 
                    var hisPatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter); 
                    if (hisPatientTypeAlters != null && hisPatientTypeAlters.Count > 0)
                    {
                        GetHeinCardNumber(paramGet, hisPatientTypeAlters, rdos); 
                    }

                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 
            }
        }

        private void GetHeinCardNumber(CommonParam paramGet, List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeALters, List<Mrs00089RDO> rdos)
        {
            try
            {
                foreach (var rdo in rdos)
                {
                    var patientTypeAlters = hisPatientTypeALters.Where(o => o.TREATMENT_ID == rdo.TREATMENT_ID).ToList(); 
                    if (patientTypeAlters != null && patientTypeAlters.Count > 0)
                    {
                        if (patientTypeAlters[0].PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            if (patientTypeAlters[0].HEIN_CARD_NUMBER != null)
                            {
                                rdo.HEIN_CARD_NUMBER = RDOCommon.GenerateHeinCardSeparate(patientTypeAlters[0].HEIN_CARD_NUMBER); 
                            }
                        }
                    }
                }
                ListRdo.AddRange(rdos); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void CalcuatorAge(Mrs00089RDO rdo, V_HIS_EXP_MEST prescription)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(prescription.TDL_PATIENT_DOB??0); 
                if (tuoi >= 0)
                {
                    if (prescription.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.MALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                        rdo.MALE_YEAR = ProcessYearDob(prescription.TDL_PATIENT_DOB??0); 
                    }
                    else
                    {
                        rdo.FEMALE_AGE = (tuoi >= 1) ? tuoi : 1; 
                        rdo.FEMALE_YEAR = ProcessYearDob(prescription.TDL_PATIENT_DOB??0); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
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

        private void LoadDataToRam()
        {
            try
            {
                HisExpMestViewFilterQuery filter = new HisExpMestViewFilterQuery(); 
                if (castFilter.MEDI_STOCK_IDs != null && castFilter.MEDI_STOCK_IDs.Count > 0)
                {
                    filter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs; 
                }
                else
                {
                    filter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                }
                filter.FINISH_DATE_FROM = castFilter.TIME_FROM; 
                filter.FINISH_DATE_TO = castFilter.TIME_TO; 
                filter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                filter.EXP_MEST_TYPE_IDs = new List<long>()
                {
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                }
                    ; 
                ListPrescription = new HisExpMestManager().GetView(filter);
                var treatmentIds = ListPrescription.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                //HSDT
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;

                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisTreatmentFilterQuery HisTreatmentfilter = new HisTreatmentFilterQuery();
                        HisTreatmentfilter.IDs = limit;
                        var listHisTreatmentSub = new HisTreatmentManager(param).Get(HisTreatmentfilter);
                        if (listHisTreatmentSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisTreatmentSub Get null");
                        else
                            listHisTreatment.AddRange(listHisTreatmentSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListPrescription = new List<V_HIS_EXP_MEST>(); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                objectTag.AddObjectData(store, "Report", ListRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
