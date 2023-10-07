using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00433
{
    class Mrs00433Processor : AbstractProcessor
    {
        Mrs00433Filter castFilter = null; 
        List<Mrs00433RDO> listRdo = new List<Mrs00433RDO>(); 
        List<Mrs00433RDO> listRdoGroup = new List<Mrs00433RDO>(); 

        List<Mrs00433RDO> listRdoCurrMonth = new List<Mrs00433RDO>(); 
        List<Mrs00433RDO> listRdoPreMonth = new List<Mrs00433RDO>(); 



        public Mrs00433Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 

        List<V_HIS_TREATMENT> listPreTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> listPrePatientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<V_HIS_SERE_SERV> listPreSereServs = new List<V_HIS_SERE_SERV>(); 

        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCats = new List<V_HIS_SERVICE_RETY_CAT>(); 

        public decimal TOTAL_PRICE_TRUXQ = 0; 
        public string s_PreMonth = ""; 

        public override Type FilterType()
        {
            return typeof(Mrs00433Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00433Filter)this.reportFilter; 

                var skip = 0; 
                // Dich vu X_QUANG CAN CHOP
                HisServiceRetyCatViewFilterQuery retyCastFilter = new HisServiceRetyCatViewFilterQuery(); 
                retyCastFilter.REPORT_TYPE_CODE__EXACT = "MRS00433"; 
                listServiceRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).GetView(retyCastFilter); 


                #region du lieu theo filter
                // Treatment
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = this.castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = this.castFilter.TIME_TO; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 
                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList(); 
                // Loc BN BHYT, Dien dieu tri
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery(); 
                    filter.TREATMENT_IDs = listIds; 
                    if (IsNotNullOrEmpty(this.castFilter.TREATMENT_TYPE_IDs))
                    {
                        filter.TREATMENT_TYPE_IDs = this.castFilter.TREATMENT_TYPE_IDs; 
                    }
                    filter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                    var patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter); 
                    listPatientTypeAlters.AddRange(patientTypeAlters); 
                }

                listTreatments = listTreatments.Where(w => listPatientTypeAlters.Select(s => s.TREATMENT_ID).Contains(w.ID)).ToList(); 
                listTreatmentIds = listTreatments.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery(); 
                    filter.TREATMENT_IDs = listIds; 
                    var sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filter); 
                    listSereServs.AddRange(sereServs); 
                }
                #endregion
                #region thang truoc
                DateTime preMonth = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(this.castFilter.TIME_TO)??DateTime.Now; 
                preMonth = preMonth.AddMonths(-1); 

                long preMonthFrom = (long)Convert.ToInt64(preMonth.ToString("yyyyMM") + "01000000"); 
                long preMonthTo = (long)Convert.ToInt64(preMonth.ToString("yyyyMM") + "31235959"); 
                s_PreMonth = preMonth.ToString("MM/yyyy"); 

                // Treatment
                HisTreatmentViewFilterQuery treatmentFilterPre = new HisTreatmentViewFilterQuery(); 
                treatmentFilterPre.FEE_LOCK_TIME_FROM = preMonthFrom; 
                treatmentFilterPre.FEE_LOCK_TIME_TO = preMonthTo; 
                listPreTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilterPre); 
                var listPreTreatmentIds = listPreTreatments.Select(s => s.ID).ToList(); 
                // Loc BN BHYT, Dien dieu tri
                skip = 0; 
                while (listPreTreatmentIds.Count - skip > 0)
                {
                    var listIds = listPreTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery(); 
                    filter.TREATMENT_IDs = listIds; 
                    if (IsNotNullOrEmpty(this.castFilter.TREATMENT_TYPE_IDs))
                    {
                        filter.TREATMENT_TYPE_IDs = this.castFilter.TREATMENT_TYPE_IDs; 
                    }
                    filter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT; 
                    var patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter); 
                    listPrePatientTypeAlters.AddRange(patientTypeAlters); 
                }

                listPreTreatments = listPreTreatments.Where(w => listPrePatientTypeAlters.Select(s => s.TREATMENT_ID).Contains(w.ID)).ToList(); 
                listPreTreatmentIds = listPreTreatments.Select(s => s.ID).ToList(); 
                skip = 0; 
                while (listPreTreatmentIds.Count - skip > 0)
                {
                    var listIds = listPreTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery(); 
                    filter.TREATMENT_IDs = listIds; 
                    var sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filter); 
                    listPreSereServs.AddRange(sereServs); 
                }

                #endregion

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
                var listServiceIds = listServiceRetyCats.Select(s => s.SERVICE_ID).ToList(); 
                listRdoCurrMonth = getRdo(listTreatments,listPatientTypeAlters,listSereServs,listServiceIds, 1); 
                listRdoPreMonth = getRdo(listPreTreatments, listPrePatientTypeAlters, listPreSereServs, listServiceIds, -1); 

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
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                dicSingleTag.Add("PREMONTH", s_PreMonth); 
                dicSingleTag.Add("TOTAL_PRICE_TRUXQ", TOTAL_PRICE_TRUXQ); 
                objectTag.AddObjectData(store, "Report", listRdoCurrMonth); 
                objectTag.AddObjectData(store, "PreReport", listRdoPreMonth); 

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        public List<Mrs00433RDO> getRdo(List<V_HIS_TREATMENT> listTreatments, List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters, List<V_HIS_SERE_SERV> listSereServs, List<long> listServiceIds, long codeMonth)
        {
            try 
            {
                listRdo.Clear(); 
                if (IsNotNullOrEmpty(listTreatments))
                {
                    
                    foreach (var treatment in listTreatments)
                    {
                        var patientTypeAlters = listPatientTypeAlters.Where(w => w.TREATMENT_ID == treatment.ID).OrderByDescending(o => o.LOG_TIME).ToList(); 
                        var sereServs = listSereServs.Where(w => w.TDL_TREATMENT_ID == treatment.ID).ToList(); 

                        Mrs00433RDO rdo = new Mrs00433RDO(); 
                        rdo.TOTAL_PATIENT = 1;
                        rdo.TOTAL_TREATMENT_TIME = DateDiff.diffDate(treatment.CLINICAL_IN_TIME, treatment.OUT_TIME); 
                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            if (patientTypeAlters.FirstOrDefault().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                            {
                                rdo.TREATMENT_TYPE_NAME = "Nội trú"; 
                            }
                            if (patientTypeAlters.FirstOrDefault().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM || patientTypeAlters.FirstOrDefault().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                            {
                                rdo.TREATMENT_TYPE_NAME = "Ngoại trú"; 
                            }
                        }

                        if (IsNotNullOrEmpty(sereServs))
                        {
                            foreach (var sereServ in sereServs)
                            {
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                                {
                                    rdo.TOTAL_PRICE_XN += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA)
                                {
                                    rdo.TOTAL_PRICE_CDHA += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_UT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL)
                                {
                                    rdo.TOTAL_PRICE_THUOC += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                                {
                                    rdo.TOTAL_PRICE_MAU += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT)
                                {
                                    rdo.TOTAL_PRICE_PTTT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_NDM || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TT)
                                {
                                    rdo.TOTAL_PRICE_VTYT += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT)
                                {
                                    rdo.TOTAL_PRICE_GIUONG += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                                if (sereServ.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                                {
                                    rdo.TOTAL_PRICE_KHAM += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }

                                rdo.TOTAL_PRICE += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                rdo.TOTAL_PATIENT_PRICE += sereServ.VIR_TOTAL_PATIENT_PRICE ?? 0; 
                                rdo.TOTAL_HEIN_PRICE += sereServ.VIR_TOTAL_HEIN_PRICE ?? 0; 
                                if (listServiceIds.Contains(sereServ.SERVICE_ID) && codeMonth >0)
                                {
                                    TOTAL_PRICE_TRUXQ += sereServ.VIR_TOTAL_PRICE ?? 0; 
                                }
                            }
                        }

                        listRdo.Add(rdo); 
                    }
                    // Nhom theo dien dieu tri
                    listRdoGroup = listRdo.GroupBy(gr => gr.TREATMENT_TYPE_NAME).Select(s => new Mrs00433RDO
                    {
                        TREATMENT_TYPE_NAME = s.First().TREATMENT_TYPE_NAME,
                        TOTAL_PATIENT = s.Sum(su => su.TOTAL_PATIENT),
                        TOTAL_TREATMENT_TIME = s.Sum(su => su.TOTAL_TREATMENT_TIME),
                        TOTAL_PRICE_XN = s.Sum(su => su.TOTAL_PRICE_XN),
                        TOTAL_PRICE_CDHA = s.Sum(su => su.TOTAL_PRICE_CDHA),
                        TOTAL_PRICE_THUOC = s.Sum(su => su.TOTAL_PRICE_THUOC),
                        TOTAL_PRICE_MAU = s.Sum(su => su.TOTAL_PRICE_MAU),
                        TOTAL_PRICE_PTTT = s.Sum(su => su.TOTAL_PRICE_PTTT),
                        TOTAL_PRICE_VTYT = s.Sum(su => su.TOTAL_PRICE_VTYT),
                        TOTAL_PRICE_GIUONG = s.Sum(su => su.TOTAL_PRICE_GIUONG),
                        TOTAL_PRICE_KHAM = s.Sum(su => su.TOTAL_PRICE_KHAM),
                        TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE),
                        TOTAL_PATIENT_PRICE = s.Sum(su => su.TOTAL_PATIENT_PRICE),
                        TOTAL_HEIN_PRICE = s.Sum(su => su.TOTAL_HEIN_PRICE),
                    }).ToList(); 

                }
                return listRdoGroup; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return null; 
        }
    }
}
