using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatmentEndType;
using AutoMapper; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExecuteRoom; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using SDA.EFMODEL; 
using SDA.Filter;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Core.SdaDistrict.Get;
using SDA.MANAGER.Manager;
using SDA.MANAGER.Core.SdaProvince.Get; 

namespace MRS.Processor.Mrs00412
{
    class Mrs00412Processor : AbstractProcessor
    {
        List<Mrs00412RDO> ListRdo = new List<Mrs00412RDO>(); 
        List<Mrs00412RDO> ListRdoGroup = new List<Mrs00412RDO>(); 
        List<DISTRICT> listDistrictss = new List<DISTRICT>(); 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 

        Dictionary<long,V_HIS_TREATMENT> dicTreatments = new Dictionary<long,V_HIS_TREATMENT>(); 
        Dictionary<long,V_HIS_PATIENT> dicPatients = new Dictionary<long,V_HIS_PATIENT>();
        Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlters = new Dictionary<long, List<V_HIS_PATIENT_TYPE_ALTER>>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        Dictionary<long, V_HIS_SERVICE_1> dicService = new Dictionary<long, V_HIS_SERVICE_1>(); 
        
        List<SDA.EFMODEL.DataModels.SDA_DISTRICT> listDistricts = new List<SDA.EFMODEL.DataModels.SDA_DISTRICT>(); 
        //List<SDA.EFMODEL.DataModels.SDA_PROVINCE> provinces = new List<SDA.EFMODEL.DataModels.SDA_PROVINCE>(); 

        public Mrs00412Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00412Filter); 
        }


        protected override bool GetData()
        {
            var filter = ((Mrs00412Filter)reportFilter); 
            bool result = true; 
            try
            {
                //Yeu cau
                HisServiceReqViewFilterQuery reqFilter = new HisServiceReqViewFilterQuery(); 
                reqFilter.FINISH_TIME_FROM = filter.TIME_FROM; 
                reqFilter.FINISH_TIME_TO = filter.TIME_TO; 
                reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH; 
                var ListServiceReq = new HisServiceReqManager().GetView(reqFilter); 
                dicServiceReq = ListServiceReq.ToDictionary(o => o.ID); 

                //Yc - dv
                var listServiceReqId = ListServiceReq.Select(s => s.ID).ToList(); 

                if (IsNotNullOrEmpty(listServiceReqId))
                {
                    var skip = 0; 
                    while (listServiceReqId.Count - skip > 0)
                    {
                        var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        HisSereServViewFilterQuery filterSs = new HisSereServViewFilterQuery(); 
                        filterSs.SERVICE_REQ_IDs = listIDs; 
                        var listSereServSub = new HisSereServManager().GetView(filterSs); 
                        listSereServs.AddRange(listSereServSub); 
                    }
                }
                

                var ski = 0;
                var treatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                var listTreatment = new List<V_HIS_TREATMENT>();
                var listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
                var listPatient = new List<V_HIS_PATIENT>();
                while (treatmentIds.Count - ski > 0)
                {
                    var listIDs = treatmentIds.Skip(ski).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    ski = ski + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                    treatmentViewFilter.IDs = listIDs;
                    var treatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewFilter);
                    if (treatments != null)
                    {
                        listTreatment.AddRange(treatments);
                    }

                    HisPatientViewFilterQuery patientViewFilter = new HisPatientViewFilterQuery();
                    patientViewFilter.IDs = treatments.Select(s => s.PATIENT_ID).ToList(); // đang bị lỗi dll, build lại và đẩy code

                    listPatient.AddRange(new MOS.MANAGER.HisPatient.HisPatientManager(param).GetView(patientViewFilter));  

                    HisPatientTypeAlterViewFilterQuery patientTypeAlterViewFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    patientTypeAlterViewFilter.TREATMENT_IDs = listIDs;
                    listPatientTypeAlter.AddRange(new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterViewFilter));  
                }
                dicPatients = listPatient.GroupBy(o=>o.ID).ToDictionary(p=>p.Key,q=>q.First());
                dicPatientTypeAlters = listPatientTypeAlter.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                dicTreatments = listTreatment.GroupBy(g => g.ID).ToDictionary(p => p.Key, q => q.First());

                // danh sách chi nhánh của bệnh viện
                var listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(new HisBranchFilterQuery());

                var listProvinces = new SdaProvinceManager(new CommonParam()).Get<List<SDA_PROVINCE>>(new SdaProvinceFilterQuery());

                var provinces = listProvinces.Where(w => listBranchs.Select(s => s.HEIN_PROVINCE_CODE).Contains(w.PROVINCE_CODE)).ToList(); 

                SdaDistrictFilter districtFilter = new SdaDistrictFilter(); 
                districtFilter.PROVINCE_IDs = provinces.Select(s => s.ID).ToList();
                listDistricts = new SdaDistrictManager(new CommonParam()).Get<List<SDA_DISTRICT>>(new SdaDistrictFilterQuery());
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
            var result = true; 
            try
            {
                var filter = ((Mrs00412Filter)reportFilter); 

                // lấy danh sách quận huyện
                if (IsNotNullOrEmpty(listDistricts))
                {
                    listDistricts = listDistricts.OrderBy(s => s.DISTRICT_NAME).ToList(); 
                    int i = 1; 
                    foreach (var district in listDistricts)
                    {
                        var dtr = new DISTRICT(); 
                        dtr.DISTRICT_CODE = district.DISTRICT_CODE; 
                        dtr.DISTRICT_NAME = district.DISTRICT_NAME; 
                        dtr.DISTRICT_TAG = "DISTRICT_NAME_" + i; 
                        i++; 

                        listDistrictss.Add(dtr); 
                    }
                }

                // lấy danh sách các dịch vụ khám
                if (IsNotNullOrEmpty(listSereServs))
                {
                    V_HIS_SERVICE_REQ req = null; 
                    var listSereServRdo = new List<Mrs00412RDO>(); 
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue; 
                        req = dicServiceReq[sereServ.SERVICE_REQ_ID??0]; 
                        var rdo = new Mrs00412RDO(); 
                        rdo.DEPARTMENT_ID = sereServ.TDL_EXECUTE_DEPARTMENT_ID; 
                        rdo.DEPARTMENT_NAME = sereServ.EXECUTE_DEPARTMENT_NAME; 

                        rdo.EXECUTE_ROOM_ID = sereServ.TDL_EXECUTE_ROOM_ID; 
                        rdo.EXECUTE_ROOM_NAME = sereServ.EXECUTE_ROOM_NAME;

                        var tuoi = Inventec.Common.DateTime.Calculation.Age(req.TDL_PATIENT_DOB);
                        rdo.TOTAL = 1;
                        if (tuoi < 16)
                        {
                            rdo.TE__16T = 1;
                            if (tuoi < 6)
                            {
                                rdo.TE__6T = 1;
                            }
                        }
                        if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            rdo.FEE = 1;
                        else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.HEIN = 1;
                            if (IsNotNull(sereServ.HEIN_CARD_NUMBER))
                            {
                                if (sereServ.HEIN_CARD_NUMBER.Substring(0, 2) == "CB")
                                    rdo.HEIN_CB = 1;
                                else if (sereServ.HEIN_CARD_NUMBER.Substring(0, 2) == "ND")
                                    rdo.HEIN_ND = 1;
                                else if (sereServ.HEIN_CARD_NUMBER.Substring(0, 2) == "TE")
                                    rdo.HEIN_TE = 1;
                                if (sereServ.HEIN_CARD_NUMBER.Substring(0, 2) == "TE")
                                    if (tuoi < 6)
                                        rdo.HEIN_TE__6T = 1;
                            }
                        }
                        if (req.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.FEMALE = 1;
                            if (IsNotNull(sereServ.HEIN_CARD_NUMBER))
                            {
                                rdo.HEIN_FEMALE = 1;
                            }
                        }
                        if (Inventec.Common.DateTime.Calculation.Age(req.TDL_PATIENT_DOB) >= 60)
                        {
                            rdo.OLDER_THAN_60 = 1;
                            if (IsNotNull(sereServ.HEIN_CARD_NUMBER))
                            {
                                rdo.HEIN_OLDER_THAN_60 = 1;
                            }
                        }

                        var patient = dicPatients.ContainsKey(req.TDL_PATIENT_ID)?dicPatients[req.TDL_PATIENT_ID]:null;
                        if (patient != null)
                        {
                            rdo.PROVINCE_CODE = patient.PROVINCE_CODE;
                            rdo.DISTRICT_CODE = patient.DISTRICT_CODE;

                            var district = listDistrictss.Where(s => s.DISTRICT_CODE == patient.DISTRICT_CODE); 
                            if (district != null && district.Count() > 0)
                            {
                                try
                                {
                                    System.Reflection.PropertyInfo piDistrict = typeof(Mrs00412RDO).GetProperty(district.First().DISTRICT_TAG); 
                                    piDistrict.SetValue(rdo, Convert.ToDecimal(1)); 
                                }
                                catch { }
                            }
                            else
                            {
                                rdo.OTHER_PROVINCE = 1; 
                            }
                        }

                        var treatment = dicTreatments.ContainsKey(sereServ.TDL_TREATMENT_ID??0)?dicTreatments[sereServ.TDL_TREATMENT_ID??0]:null; 
                        if (treatment != null)
                        {
                            if (treatment.CLINICAL_IN_TIME != null)
                            {
                                var patientTypeAlter = dicPatientTypeAlters.ContainsKey(sereServ.TDL_TREATMENT_ID??0)?dicPatientTypeAlters[sereServ.TDL_TREATMENT_ID??0].Where(s=>s.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).ToList():null; 
                                if (IsNotNullOrEmpty(patientTypeAlter))
                                {
                                    if (patientTypeAlter.OrderBy(s => s.LOG_TIME).First().EXECUTE_ROOM_ID == sereServ.TDL_EXECUTE_ROOM_ID)
                                        rdo.IN_CLINICAL = 1; 
                                }
                            }

                            
                            if (treatment.END_ROOM_ID == sereServ.TDL_EXECUTE_ROOM_ID && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                                rdo.TRAN_OUT = 1; 
                        }


                        ListRdo.Add(rdo); 
                    }


                    ListRdo = ListRdo.GroupBy(g => g.EXECUTE_ROOM_ID).Select(s => new Mrs00412RDO 
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,

                        EXECUTE_ROOM_ID = s.First().EXECUTE_ROOM_ID,
                        EXECUTE_ROOM_NAME = s.First().EXECUTE_ROOM_NAME,

                        TOTAL = s.Sum(su=>su.TOTAL),
                        FEE = s.Sum(su=>su.FEE),
                        HEIN = s.Sum(su=>su.HEIN),
                        HEIN_CB = s.Sum(su=>su.HEIN_CB),
                        HEIN_ND = s.Sum(su => su.HEIN_ND),
                        HEIN_TE = s.Sum(su => su.HEIN_TE),
                        HEIN_TE__6T = s.Sum(su => su.HEIN_TE__6T),
                        TE__16T = s.Sum(su => su.TE__16T),
                        TE__6T = s.Sum(su => su.TE__6T),
                        FEMALE = s.Sum(su => su.FEMALE),
                        HEIN_FEMALE = s.Sum(su => su.HEIN_FEMALE),
                        OLDER_THAN_60 = s.Sum(su => su.OLDER_THAN_60),
                        HEIN_OLDER_THAN_60 = s.Sum(su => su.HEIN_OLDER_THAN_60),

                        DISTRICT_NAME_1 = s.Sum(su=>su.DISTRICT_NAME_1),
                        DISTRICT_NAME_2 = s.Sum(su => su.DISTRICT_NAME_2),
                        DISTRICT_NAME_3 = s.Sum(su => su.DISTRICT_NAME_3),
                        DISTRICT_NAME_4 = s.Sum(su => su.DISTRICT_NAME_4),
                        DISTRICT_NAME_5 = s.Sum(su => su.DISTRICT_NAME_5),
                        DISTRICT_NAME_6 = s.Sum(su => su.DISTRICT_NAME_6),
                        DISTRICT_NAME_7 = s.Sum(su => su.DISTRICT_NAME_7),
                        DISTRICT_NAME_8 = s.Sum(su => su.DISTRICT_NAME_8),
                        DISTRICT_NAME_9 = s.Sum(su => su.DISTRICT_NAME_9),
                        DISTRICT_NAME_10 = s.Sum(su => su.DISTRICT_NAME_10),

                        DISTRICT_NAME_11 = s.Sum(su => su.DISTRICT_NAME_11),
                        DISTRICT_NAME_12 = s.Sum(su => su.DISTRICT_NAME_12),
                        DISTRICT_NAME_13 = s.Sum(su => su.DISTRICT_NAME_13),
                        DISTRICT_NAME_14 = s.Sum(su => su.DISTRICT_NAME_14),
                        DISTRICT_NAME_15 = s.Sum(su => su.DISTRICT_NAME_15),
                        DISTRICT_NAME_16 = s.Sum(su => su.DISTRICT_NAME_16),
                        DISTRICT_NAME_17 = s.Sum(su => su.DISTRICT_NAME_17),
                        DISTRICT_NAME_18 = s.Sum(su => su.DISTRICT_NAME_18),
                        DISTRICT_NAME_19 = s.Sum(su => su.DISTRICT_NAME_19),
                        DISTRICT_NAME_20 = s.Sum(su => su.DISTRICT_NAME_20),

                        DISTRICT_NAME_21 = s.Sum(su => su.DISTRICT_NAME_21),
                        DISTRICT_NAME_22 = s.Sum(su => su.DISTRICT_NAME_22),
                        DISTRICT_NAME_23 = s.Sum(su => su.DISTRICT_NAME_23),
                        DISTRICT_NAME_24 = s.Sum(su => su.DISTRICT_NAME_24),
                        DISTRICT_NAME_25 = s.Sum(su => su.DISTRICT_NAME_25),
                        DISTRICT_NAME_26 = s.Sum(su => su.DISTRICT_NAME_26),
                        DISTRICT_NAME_27 = s.Sum(su => su.DISTRICT_NAME_27),
                        DISTRICT_NAME_28 = s.Sum(su => su.DISTRICT_NAME_28),
                        DISTRICT_NAME_29 = s.Sum(su => su.DISTRICT_NAME_29),
                        DISTRICT_NAME_30 = s.Sum(su => su.DISTRICT_NAME_20),

                        DISTRICT_NAME_31 = s.Sum(su => su.DISTRICT_NAME_31),
                        DISTRICT_NAME_32 = s.Sum(su => su.DISTRICT_NAME_32),
                        DISTRICT_NAME_33 = s.Sum(su => su.DISTRICT_NAME_33),
                        DISTRICT_NAME_34 = s.Sum(su => su.DISTRICT_NAME_34),
                        DISTRICT_NAME_35 = s.Sum(su => su.DISTRICT_NAME_35),
                        DISTRICT_NAME_36 = s.Sum(su => su.DISTRICT_NAME_36),
                        DISTRICT_NAME_37 = s.Sum(su => su.DISTRICT_NAME_37),
                        DISTRICT_NAME_38 = s.Sum(su => su.DISTRICT_NAME_38),
                        DISTRICT_NAME_39 = s.Sum(su => su.DISTRICT_NAME_39),
                        DISTRICT_NAME_40 = s.Sum(su => su.DISTRICT_NAME_40),

                        DISTRICT_NAME_41 = s.Sum(su => su.DISTRICT_NAME_41),
                        DISTRICT_NAME_42 = s.Sum(su => su.DISTRICT_NAME_42),
                        DISTRICT_NAME_43 = s.Sum(su => su.DISTRICT_NAME_43),
                        DISTRICT_NAME_44 = s.Sum(su => su.DISTRICT_NAME_44),
                        DISTRICT_NAME_45 = s.Sum(su => su.DISTRICT_NAME_45),
                        DISTRICT_NAME_46 = s.Sum(su => su.DISTRICT_NAME_46),
                        DISTRICT_NAME_47 = s.Sum(su => su.DISTRICT_NAME_47),
                        DISTRICT_NAME_48 = s.Sum(su => su.DISTRICT_NAME_48),
                        DISTRICT_NAME_49 = s.Sum(su => su.DISTRICT_NAME_49),
                        DISTRICT_NAME_50 = s.Sum(su => su.DISTRICT_NAME_50),

                        DISTRICT_NAME_51 = s.Sum(su => su.DISTRICT_NAME_51),
                        DISTRICT_NAME_52 = s.Sum(su => su.DISTRICT_NAME_52),
                        DISTRICT_NAME_53 = s.Sum(su => su.DISTRICT_NAME_53),
                        DISTRICT_NAME_54 = s.Sum(su => su.DISTRICT_NAME_54),
                        DISTRICT_NAME_55 = s.Sum(su => su.DISTRICT_NAME_55),
                        DISTRICT_NAME_56 = s.Sum(su => su.DISTRICT_NAME_56),
                        DISTRICT_NAME_57 = s.Sum(su => su.DISTRICT_NAME_57),
                        DISTRICT_NAME_58 = s.Sum(su => su.DISTRICT_NAME_58),
                        DISTRICT_NAME_59 = s.Sum(su => su.DISTRICT_NAME_59),
                        DISTRICT_NAME_60 = s.Sum(su => su.DISTRICT_NAME_60),

                        DISTRICT_NAME_61 = s.Sum(su => su.DISTRICT_NAME_61),
                        DISTRICT_NAME_62 = s.Sum(su => su.DISTRICT_NAME_62),
                        DISTRICT_NAME_63 = s.Sum(su => su.DISTRICT_NAME_63),
                        DISTRICT_NAME_64 = s.Sum(su => su.DISTRICT_NAME_64),
                        DISTRICT_NAME_65 = s.Sum(su => su.DISTRICT_NAME_65),
                        DISTRICT_NAME_66 = s.Sum(su => su.DISTRICT_NAME_66),
                        DISTRICT_NAME_67 = s.Sum(su => su.DISTRICT_NAME_67),
                        DISTRICT_NAME_68 = s.Sum(su => su.DISTRICT_NAME_68),
                        DISTRICT_NAME_69 = s.Sum(su => su.DISTRICT_NAME_69),
                        DISTRICT_NAME_70 = s.Sum(su => su.DISTRICT_NAME_70),

                        DISTRICT_NAME_71 = s.Sum(su => su.DISTRICT_NAME_71),
                        DISTRICT_NAME_72 = s.Sum(su => su.DISTRICT_NAME_72),
                        DISTRICT_NAME_73 = s.Sum(su => su.DISTRICT_NAME_73),
                        DISTRICT_NAME_74 = s.Sum(su => su.DISTRICT_NAME_74),
                        DISTRICT_NAME_75 = s.Sum(su => su.DISTRICT_NAME_75),
                        DISTRICT_NAME_76 = s.Sum(su => su.DISTRICT_NAME_76),
                        DISTRICT_NAME_77 = s.Sum(su => su.DISTRICT_NAME_77),
                        DISTRICT_NAME_78 = s.Sum(su => su.DISTRICT_NAME_78),
                        DISTRICT_NAME_79 = s.Sum(su => su.DISTRICT_NAME_79),
                        DISTRICT_NAME_80 = s.Sum(su => su.DISTRICT_NAME_80),

                        DISTRICT_NAME_81 = s.Sum(su => su.DISTRICT_NAME_81),
                        DISTRICT_NAME_82 = s.Sum(su => su.DISTRICT_NAME_82),
                        DISTRICT_NAME_83 = s.Sum(su => su.DISTRICT_NAME_83),
                        DISTRICT_NAME_84 = s.Sum(su => su.DISTRICT_NAME_84),
                        DISTRICT_NAME_85 = s.Sum(su => su.DISTRICT_NAME_85),
                        DISTRICT_NAME_86 = s.Sum(su => su.DISTRICT_NAME_86),
                        DISTRICT_NAME_87 = s.Sum(su => su.DISTRICT_NAME_87),
                        DISTRICT_NAME_88 = s.Sum(su => su.DISTRICT_NAME_88),
                        DISTRICT_NAME_89 = s.Sum(su => su.DISTRICT_NAME_89),
                        DISTRICT_NAME_90 = s.Sum(su => su.DISTRICT_NAME_90),

                        DISTRICT_NAME_91 = s.Sum(su => su.DISTRICT_NAME_91),
                        DISTRICT_NAME_92 = s.Sum(su => su.DISTRICT_NAME_92),
                        DISTRICT_NAME_93 = s.Sum(su => su.DISTRICT_NAME_93),
                        DISTRICT_NAME_94 = s.Sum(su => su.DISTRICT_NAME_94),
                        DISTRICT_NAME_95 = s.Sum(su => su.DISTRICT_NAME_95),
                        DISTRICT_NAME_96 = s.Sum(su => su.DISTRICT_NAME_96),
                        DISTRICT_NAME_97 = s.Sum(su => su.DISTRICT_NAME_97),
                        DISTRICT_NAME_98 = s.Sum(su => su.DISTRICT_NAME_98),
                        DISTRICT_NAME_99 = s.Sum(su => su.DISTRICT_NAME_99),
                        DISTRICT_NAME_100 = s.Sum(su => su.DISTRICT_NAME_100),

                        OTHER_PROVINCE = s.Sum(su=>su.OTHER_PROVINCE),
                        TRAN_OUT = s.Sum(su=>su.TRAN_OUT),
                        IN_CLINICAL = s.Sum(su=>su.IN_CLINICAL)
                    }).ToList(); 

                    ListRdoGroup = ListRdo.GroupBy(g => g.DEPARTMENT_ID).Select(s => new Mrs00412RDO 
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME
                    }).ToList(); 
                }

            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (((Mrs00412Filter)reportFilter).TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00412Filter)reportFilter).TIME_FROM)); 
                }
                if (((Mrs00412Filter)reportFilter).TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00412Filter)reportFilter).TIME_TO)); 
                }

                dicSingleTag.Add("CREATE_TIME", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year); 

                // danh sách các quận, huyện trong tỉnh
                foreach (var districts in listDistrictss.OrderBy(s => s.DISTRICT_TAG).ToList())
                {
                    dicSingleTag.Add(districts.DISTRICT_TAG, districts.DISTRICT_NAME); 
                }

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.EXECUTE_ROOM_NAME).ToList()); 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup.OrderBy(s => s.DEPARTMENT_NAME).ToList()); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }

}
