using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using FlexCel.Report;
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
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00375;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using Inventec.Common.Logging;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00375
{
    public class Mrs00375Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<Mrs00375RDO> ListRdo = new List<Mrs00375RDO>();
        //List<Mrs00375RDO> ListReportTypeCats = new List<Mrs00375RDO>(); 
        List<HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        List<V_HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientType = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
        public Mrs00375Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00375Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                //Chon nhom bao cao
                HisReportTypeCatFilterQuery reportTypeCatFilter = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00375"
                };
                listReportTypeCat = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(paramGet).Get(reportTypeCatFilter).Where(o => o.REPORT_TYPE_CODE == "MRS00375").ToList();


                // Chon nhom dich vu
                HisServiceRetyCatFilterQuery ServiceRetyCatFilter = new HisServiceRetyCatFilterQuery();
                ServiceRetyCatFilter.REPORT_TYPE_CAT_IDs = listReportTypeCat.Select(o => o.ID).ToList();
                listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(paramGet).Get(ServiceRetyCatFilter);
                var listServiceRetyCatid = listServiceRetyCat.Select(o => o.SERVICE_ID).ToList();

                // yeu cau

                var reqFilter = new HisServiceReqViewFilterQuery
                {
                    INTRUCTION_TIME_FROM = ((Mrs00375Filter)this.reportFilter).TIME_FROM,
                    INTRUCTION_TIME_TO = ((Mrs00375Filter)this.reportFilter).TIME_TO

                };
                listServiceReq = new HisServiceReqManager(paramGet).GetView(reqFilter);

                var listServiceReqId = listServiceReq.Select(o => o.ID).Distinct().ToList();

                // YC-DV

                if (IsNotNullOrEmpty(listServiceReqId))
                {
                    var skip = 0;
                    while (listServiceReqId.Count - skip > 0)
                    {
                        var listids = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        var ssFilter = new HisSereServFilterQuery
                        {
                            SERVICE_REQ_IDs = listids
                        };
                        var listSereServSub = new HisSereServManager(paramGet).Get(ssFilter);
                        ListSereServ.AddRange(listSereServSub);
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (IsNotNullOrEmpty(listServiceRetyCatid)) ListSereServ = ListSereServ.Where(o => listServiceRetyCatid.Contains(o.SERVICE_ID)).ToList();
                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        var listTreatmentId = ListSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                        // chuyen doi tuong
                        skip = 0;
                        while (listTreatmentId.Count - skip > 0)
                        {
                            var listids = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            var patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery
                            {
                                TREATMENT_IDs = listids,
                                ORDER_DIRECTION = "DESC",
                                ORDER_FIELD = "LOG_TIME"
                            };
                            var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                            if (IsNotNullOrEmpty(listPatientTypeAlterSub))
                            {
                                foreach (var item in listPatientTypeAlterSub)
                                {
                                    if (!dicPatientType.ContainsKey(item.TREATMENT_ID))
                                        dicPatientType[item.TREATMENT_ID] = item;
                                    //lấy đối tượng cuối cùng, thời gian lớn nhất
                                    if (dicPatientType[item.TREATMENT_ID].LOG_TIME < item.LOG_TIME)
                                    {
                                        dicPatientType[item.TREATMENT_ID] = item;
                                    }
                                }
                            }
                            listPatientTypeAlter.AddRange(listPatientTypeAlterSub);
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }
                    }

                }
                //var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(param).GetView(patientTypeAlterFilter);      

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    foreach (var serser in ListSereServ)
                    {
                        Mrs00375RDO rdo = new Mrs00375RDO();

                        var serviceRetys = listServiceRetyCat.Where(o => o.SERVICE_ID == serser.SERVICE_ID).ToList();

                        if (IsNotNullOrEmpty(serviceRetys))
                        {
                            long rpTypeCatId = serviceRetys.Select(p => p.REPORT_TYPE_CAT_ID).FirstOrDefault();
                            rdo.REPORT_TYPE_CAT_ID = rpTypeCatId;

                            rdo.CATEGORY_NAME = listReportTypeCat.FirstOrDefault(q => q.ID == rpTypeCatId).CATEGORY_NAME ?? "";
                            rdo.CATEGORY_CODE = listReportTypeCat.FirstOrDefault(q => q.ID == rpTypeCatId).CATEGORY_CODE ?? "";
                        }

                        var serviceReq = listServiceReq.FirstOrDefault(o => o.ID == serser.SERVICE_REQ_ID);
                        if (serviceReq == null) continue;
                        rdo.PATIENT_CODE = serviceReq.TDL_PATIENT_CODE;
                        rdo.TREATMENT_ID = serser.TDL_TREATMENT_ID ?? 0;
                        rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.INTRUCTION_TIME);
                        rdo.PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                        rdo.VIR_ADDRESS = serviceReq.TDL_PATIENT_ADDRESS;

                        rdo.ICD_MAIN_TEXT = serviceReq.ICD_NAME;

                        if (serviceReq.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = Inventec.Common.DateTime.Calculation.AgeCaption(serviceReq.TDL_PATIENT_DOB);
                        }
                        else // Tuoi nu
                        {
                            rdo.FEMALE_AGE = Inventec.Common.DateTime.Calculation.AgeCaption(serviceReq.TDL_PATIENT_DOB);
                        }

                        if (dicPatientType.ContainsKey(serviceReq.TREATMENT_ID) && dicPatientType[serviceReq.TREATMENT_ID].PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            rdo.IS_BHYRT = 'x';
                        }
                        rdo.REQUEST_DEPARTMENT_NAME = serviceReq.REQUEST_DEPARTMENT_NAME;

                        switch (rdo.CATEGORY_CODE)
                        {
                            case "375Beck":
                                {
                                    rdo.TNTL_BECK = "x";
                                    break;
                                }
                            case "375Zung":
                                {
                                    rdo.TNTL_ZUNG = "x";
                                    break;
                                }
                            case "375Raven":
                                {
                                    rdo.TNTL_RAVEN = "x";
                                    break;
                                }
                            case "375Hamilto":
                                {
                                    rdo.TDGTC_HAMILTON = "x";
                                    break;
                                }
                            case "375TE":
                                {
                                    rdo.TDGTC_TE = "x";
                                    break;
                                }
                            case "375WMS":
                                {
                                    rdo.TDGTN_WMS = "x";
                                    break;
                                }
                            case "375MMPI":
                                {
                                    rdo.TDGNC_MMPI = "x";
                                    break;
                                }
                            case "375PSQI":
                                {
                                    rdo.TNRLGN_PSQI = "x";
                                    break;
                                }
                            case "375LAH":
                                {
                                    rdo.TDGLOAU_HAMILTON = "x";
                                    break;
                                }
                            case "375DENVER":
                                {
                                    rdo.TDGSPTTE_DENVER = "x";
                                    break;
                                }
                            case "375CHAT":
                                {
                                    rdo.TSLTKTE_CHAT = "x";
                                    break;
                                }
                            case "375CARS":
                                {
                                    rdo.TDGMDTK_CARS = "x";
                                    break;
                                }
                            case "375CBCL":
                                {
                                    rdo.TDGHVTE_CBCL = "x";
                                    break;
                                }
                            case "375WAIS":
                                {
                                    rdo.TN_WAIS = "x";
                                    break;
                                }
                            case "375WICS":
                                {
                                    rdo.TN_WICS = "x";
                                    break;
                                }
                            case "375EPI":
                                {
                                    rdo.BNKNCHN_EPI = "x";
                                    break;
                                }
                            case "375BILT":
                                {
                                    rdo.THANG_VANDERBILT = "x";
                                    break;
                                }
                            case "375GDS":
                                {
                                    rdo.TDGTCNG_GDS = "x";
                                    break;
                                }
                            case "375EPDS":
                                {
                                    rdo.TDGTCSS_EPDS = "x";
                                    break;
                                }
                            case "375DASS":
                                {
                                    rdo.TDGLATCSTRESS_DASS = "x";
                                    break;
                                }
                            case "375Young":
                                {
                                    rdo.TDGTC_YOUNG = "x";
                                    break;
                                }
                            case "375MMSE":
                                {
                                    rdo.MMSE = "x";
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        ListRdo.Add(rdo);
                    }
                    ListRdo = ListRdo.GroupBy(o => o.TREATMENT_ID).Select(p => new Mrs00375RDO
                    {
                        PATIENT_NAME = p.First().PATIENT_NAME,
                        INTRUCTION_TIME = p.First().INTRUCTION_TIME,
                        PATIENT_CODE = p.First().PATIENT_CODE,
                        INTRUCTION_TIME_FROM = p.First().INTRUCTION_TIME_FROM,
                        VIR_ADDRESS = p.First().VIR_ADDRESS,
                        PATIENT_TYPE_ID = p.First().PATIENT_TYPE_ID,
                        TREATMENT_ID = p.First().TREATMENT_ID,
                        ICD_MAIN_TEXT = p.First().ICD_MAIN_TEXT,
                        REQUEST_DEPARTMENT_NAME = p.First().REQUEST_DEPARTMENT_NAME,
                        REPORT_TYPE_CAT_ID = p.First().REPORT_TYPE_CAT_ID,
                        CATEGORY_NAME = p.First().CATEGORY_NAME,
                        CATEGORY_CODE = p.First().CATEGORY_CODE,
                        IS_BHYRT = p.First().IS_BHYRT,
                        MALE_AGE = p.First().MALE_AGE,
                        FEMALE_AGE = p.First().FEMALE_AGE,
                        // CÁC NHÓM DỊCH VỤ
                        TNTL_BECK = p.Where(q => q.TNTL_BECK == "x").ToList().Count > 0 ? "x" : "",
                        TNTL_ZUNG = p.Where(q => q.TNTL_ZUNG == "x").ToList().Count > 0 ? "x" : "",
                        TNTL_RAVEN = p.Where(q => q.TNTL_RAVEN == "x").ToList().Count > 0 ? "x" : "",
                        TDGTC_HAMILTON = p.Where(q => q.TDGTC_HAMILTON == "x").ToList().Count > 0 ? "x" : "",
                        TDGTC_TE = p.Where(q => q.TDGTC_TE == "x").ToList().Count > 0 ? "x" : "",
                        TDGTN_WMS = p.Where(q => q.TDGTN_WMS == "x").ToList().Count > 0 ? "x" : "",
                        TDGNC_MMPI = p.Where(q => q.TDGNC_MMPI == "x").ToList().Count > 0 ? "x" : "",
                        TNRLGN_PSQI = p.Where(q => q.TNRLGN_PSQI == "x").ToList().Count > 0 ? "x" : "",
                        TDGLOAU_HAMILTON = p.Where(q => q.TDGLOAU_HAMILTON == "x").ToList().Count > 0 ? "x" : "",
                        //  TDGNC_MMPI  =p.Where(q => q.TNTL_BECK=="x").ToList().Count>0?"x":"" ,
                        TDGSPTTE_DENVER = p.Where(q => q.TDGSPTTE_DENVER == "x").ToList().Count > 0 ? "x" : "",
                        TSLTKTE_CHAT = p.Where(q => q.TSLTKTE_CHAT == "x").ToList().Count > 0 ? "x" : "",
                        TDGMDTK_CARS = p.Where(q => q.TDGMDTK_CARS == "x").ToList().Count > 0 ? "x" : "",
                        TDGHVTE_CBCL = p.Where(q => q.TDGHVTE_CBCL == "x").ToList().Count > 0 ? "x" : "",
                        TN_WAIS = p.Where(q => q.TN_WAIS == "x").ToList().Count > 0 ? "x" : "",
                        TN_WICS = p.Where(q => q.TN_WICS == "x").ToList().Count > 0 ? "x" : "",
                        BNKNCHN_EPI = p.Where(q => q.BNKNCHN_EPI == "x").ToList().Count > 0 ? "x" : "",
                        THANG_VANDERBILT = p.Where(q => q.THANG_VANDERBILT == "x").ToList().Count > 0 ? "x" : "",
                        TDGTCNG_GDS = p.Where(q => q.TDGTCNG_GDS == "x").ToList().Count > 0 ? "x" : "",
                        MMSE = p.Where(q => q.MMSE == "x").ToList().Count > 0 ? "x" : "",

                        TDGTCSS_EPDS = p.Where(q => q.TDGTCSS_EPDS == "x").ToList().Count > 0 ? "x" : "",
                        TDGLATCSTRESS_DASS = p.Where(q => q.TDGLATCSTRESS_DASS == "x").ToList().Count > 0 ? "x" : "",
                        TDGTC_YOUNG = p.Where(q => q.TDGTC_YOUNG == "x").ToList().Count > 0 ? "x" : "",

                    }).ToList();
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00375Filter)this.reportFilter).TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00375Filter)this.reportFilter).TIME_TO));
                if (IsNotNullOrEmpty(listReportTypeCat))
                {
                    //    dicSingleTag.Add("CATEGORY_NAMEs", String.Join(",", listReportTypeCat.Select(o => o.CATEGORY_NAME).ToList())); 
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