using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
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
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00595
{
    public class Mrs00595Processor : AbstractProcessor
    {
        Mrs00595Filter filter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_SERE_SERV> listHisSereServ = new List<HIS_SERE_SERV>();
        List<HIS_SERVICE_REQ> listHisServiceReqExam = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listHisServiceReqMisu = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listHisServiceReqSurg = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServTest = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listHisSereServXray = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listHisSereServSuim = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listHisSereServEcg = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listHisSereServBrain = new List<HIS_SERE_SERV>();
        List<HIS_SERE_SERV> listHisSereServEndo = new List<HIS_SERE_SERV>();
        List<HIS_DEPARTMENT> listHisDepartment = new List<HIS_DEPARTMENT>();
        List<Mrs00595RDO> listRdo = new List<Mrs00595RDO>();
        public Dictionary<string, decimal> COUNT_EXAM = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_EXAM_FEMALE = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_EXAM_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_EXAM_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_EXAM_LESS15 = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_EXAM_TRAN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_PRE = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_FEMALE = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_LESS15 = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_IN_TRAN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> TOTAL_DATE = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> TOTAL_DATE_BH = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_OUT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_OUT_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_OUT_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_OUT_LESS15 = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_SURG = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SURG_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SURG_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SURG_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SURG_OUT = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_MISU = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_MISU_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_MISU_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_MISU_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_MISU_OUT = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_TEST = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_TEST_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_TEST_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_TEST_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_TEST_OUT = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_SUIM = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SUIM_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SUIM_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SUIM_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_SUIM_OUT = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_XRAY = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_XRAY_IN = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_XRAY_BHYT = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_XRAY_VP = new Dictionary<string, decimal>();
        public Dictionary<string, decimal> COUNT_XRAY_OUT = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_ECG = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_BRAIN = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> COUNT_ENDO = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> AVG_DATE = new Dictionary<string, decimal>();

        public Dictionary<string, decimal> TRAN_IN_RATIO = new Dictionary<string, decimal>();

        string thisReportTypeCode = "";

        public Mrs00595Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00595Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00595Filter)this.reportFilter;
            try
            {
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatfilter = new HisServiceRetyCatViewFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = "MRS00595"
                };
                var listHisServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatfilter);

                #region st
                var listTreatmentOut = new ManagerSql().GetTreatment(filter);
                listHisTreatment.AddRange(listTreatmentOut);

                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                
                var treatmentTreatIds = listHisTreatment.Where(p => p.CLINICAL_IN_TIME != null).Select(o => o.ID).ToList();
                if (treatmentTreatIds != null && treatmentTreatIds.Count > 0)
                {
                    var listHisDepartmentTranSub = new ManagerSql().GetDepartmentTran(filter);
                    if (listHisDepartmentTranSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                    else
                        listHisDepartmentTran.AddRange(listHisDepartmentTranSub);
                }
                 List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                var treatmentExamIds = listHisTreatment.Where(p => p.IN_TIME < filter.IN_TIME_TO && p.IN_TIME >= filter.IN_TIME_FROM).Select(o => o.ID).ToList();
                if (treatmentExamIds != null && treatmentExamIds.Count > 0)
                {
                    var listHisServiceReqSub = new ManagerSql().GetServiceReq(filter);
                    if (listHisServiceReqSub == null)
                        Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub GetView null");
                    else
                        listHisServiceReq.AddRange(listHisServiceReqSub);
                }
                #endregion
                
                    listHisServiceReqExam = listHisServiceReq.Where(o => (o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL||o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT) && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();
                    listHisServiceReqMisu = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT).ToList();
                    listHisServiceReqSurg = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT).ToList();

                    var INTRUCTION_TIME_FROM = filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM;
                    var INTRUCTION_TIME_TO = filter.IN_TIME_TO ?? filter.OUT_TIME_TO;

                    HisSereServFilterQuery HisSereSerrvfilter = new HisSereServFilterQuery();
                    HisSereSerrvfilter.TDL_INTRUCTION_TIME_FROM = INTRUCTION_TIME_FROM;
                    HisSereSerrvfilter.TDL_INTRUCTION_TIME_TO = INTRUCTION_TIME_TO;
                    HisSereSerrvfilter.TDL_SERVICE_TYPE_IDs = new List<long>()
                        {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN
                        };
                    HisSereSerrvfilter.IS_EXPEND = false;
                    HisSereSerrvfilter.HAS_EXECUTE = true;
                    listHisSereServ = new HisSereServManager(param).Get(HisSereSerrvfilter)??new List<HIS_SERE_SERV>();

                    listHisSereServTest = listHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList();
                    listHisSereServSuim = listHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).ToList();
                    if (filter.CATEGORY_CODE__XRAY != null)
                    {
                        listHisSereServXray = listHisSereServ.Where(o =>listHisServiceRetyCat.Exists(q => q.SERVICE_ID == o.SERVICE_ID && q.CATEGORY_CODE == filter.CATEGORY_CODE__XRAY)).ToList();
                    }
                    if (filter.CATEGORY_CODE__ECG != null)
                    {
                        listHisSereServEcg = listHisSereServ.Where(o =>listHisServiceRetyCat.Exists(q => q.SERVICE_ID == o.SERVICE_ID && q.CATEGORY_CODE == filter.CATEGORY_CODE__ECG)).ToList();
                    }
                    if (filter.CATEGORY_CODE__BRAIN != null)
                    {
                        listHisSereServBrain = listHisSereServ.Where(o =>listHisServiceRetyCat.Exists(q => q.SERVICE_ID == o.SERVICE_ID && q.CATEGORY_CODE == filter.CATEGORY_CODE__BRAIN)).ToList();
                    }
                    listHisSereServEndo = listHisSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).ToList();

                treatmentIds.AddRange(listHisSereServ.Select(o => o.TDL_TREATMENT_ID??0).ToList());
                treatmentIds = treatmentIds.Distinct().ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var listHisPatientTypeAlterSub = new ManagerSql().GetPatientTypeAlter(filter);
                    listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }
                

                //Danh sach khoa lâm sàng
                listHisDepartment = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.NUM_ORDER).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                Mrs00595RDO rdo = new Mrs00595RDO();
                foreach (var item in listHisDepartment)
                {
                    var listExam = listHisServiceReqExam.Where(p => p.EXECUTE_DEPARTMENT_ID == item.ID).ToList();
                    
                    this.COUNT_EXAM.Add(item.DEPARTMENT_CODE, listExam.Count());
                    this.COUNT_EXAM_FEMALE.Add(item.DEPARTMENT_CODE, listExam.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count());
                    this.COUNT_EXAM_BHYT.Add(item.DEPARTMENT_CODE, listExam.Where(o => IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_EXAM_VP.Add(item.DEPARTMENT_CODE, listExam.Where(o => !IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_EXAM_LESS15.Add(item.DEPARTMENT_CODE, listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) < 15).Count());
                    this.COUNT_EXAM_TRAN.Add(item.DEPARTMENT_CODE, listExam.Where(o => listHisTreatment.Exists
                    (p => p.OUT_TIME < (filter.IN_TIME_TO ?? filter.OUT_TIME_TO) && p.END_DEPARTMENT_ID == item.ID && p.ID == o.TREATMENT_ID && p.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Select(q => q.TREATMENT_ID).Distinct().Count());

                    var listIn = listHisTreatment.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatIn(p, listHisPatientTypeAlter))).ToList();
                    this.COUNT_IN.Add(item.DEPARTMENT_CODE, listIn.Count());
                    this.COUNT_IN_PRE.Add(item.DEPARTMENT_CODE, listIn.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && listHisPatientTypeAlter.Exists(q => q.DEPARTMENT_TRAN_ID == p.PREVIOUS_ID && q.TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM))).Count());
                    this.COUNT_IN_FEMALE.Add(item.DEPARTMENT_CODE, listIn.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count());
                    this.COUNT_IN_BHYT.Add(item.DEPARTMENT_CODE, listIn.Where(o => IsBhyt(o.ID)).Count());
                    this.COUNT_IN_VP.Add(item.DEPARTMENT_CODE, listIn.Where(o => !IsBhyt(o.ID)).Count());
                    this.COUNT_IN_LESS15.Add(item.DEPARTMENT_CODE, listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count());
                    this.COUNT_IN_TRAN.Add(item.DEPARTMENT_CODE, listIn.Where(o => o.OUT_TIME < (filter.IN_TIME_TO ?? filter.OUT_TIME_TO) && o.END_DEPARTMENT_ID == item.ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count());

                    this.TOTAL_DATE.Add(item.DEPARTMENT_CODE, listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.END_DEPARTMENT_ID == item.ID).Sum(s => Calculation.DayOfTreatment(s.CLINICAL_IN_TIME ?? 0, s.OUT_TIME, s.TREATMENT_END_TYPE_ID, s.TREATMENT_RESULT_ID, IsBhyt(s.ID) ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0));
                    this.TOTAL_DATE_BH.Add(item.DEPARTMENT_CODE, listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID) && o.END_DEPARTMENT_ID == item.ID).Sum(s => Calculation.DayOfTreatment(s.CLINICAL_IN_TIME ?? 0, s.OUT_TIME, s.TREATMENT_END_TYPE_ID, s.TREATMENT_RESULT_ID, PatientTypeEnum.TYPE.BHYT) ?? 0));
                    if (listIn.Count() > 0)
                    {
                        this.AVG_DATE.Add(item.DEPARTMENT_CODE, (decimal)(listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.END_DEPARTMENT_ID == item.ID).Sum(s => Calculation.DayOfTreatment(s.CLINICAL_IN_TIME ?? 0, s.OUT_TIME, s.TREATMENT_END_TYPE_ID, s.TREATMENT_RESULT_ID, IsBhyt(s.ID) ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI) ?? 0)) / (decimal)listIn.Count());
                        this.TRAN_IN_RATIO.Add(item.DEPARTMENT_CODE, (decimal)listIn.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count() / (decimal)listIn.Count());
                    }
                    var listOut = listHisTreatment.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatOut(p, listHisPatientTypeAlter))).ToList();
                    this.COUNT_OUT.Add(item.DEPARTMENT_CODE, listOut.Count());
                    this.COUNT_OUT_VP.Add(item.DEPARTMENT_CODE, listOut.Where(o => !IsBhyt(o.ID)).Count());
                    this.COUNT_OUT_BHYT.Add(item.DEPARTMENT_CODE, listOut.Where(o => IsBhyt(o.ID)).Count());
                    this.COUNT_OUT_LESS15.Add(item.DEPARTMENT_CODE, listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count());

                    var listSurg = listHisServiceReqSurg.Where(p => (p.REQUEST_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_SURG.Add(item.DEPARTMENT_CODE, listSurg.Count());
                    this.COUNT_SURG_BHYT.Add(item.DEPARTMENT_CODE, listSurg.Where(o => IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_SURG_VP.Add(item.DEPARTMENT_CODE, listSurg.Where(o => !IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_SURG_IN.Add(item.DEPARTMENT_CODE, listSurg.Where(o => IsTreat(o.TREATMENT_ID)).Count());
                    this.COUNT_SURG_OUT.Add(item.DEPARTMENT_CODE, listSurg.Where(o => !IsTreat(o.TREATMENT_ID)).Count());

                    var listMisu = listHisServiceReqMisu.Where(p => (p.REQUEST_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_MISU.Add(item.DEPARTMENT_CODE, listMisu.Count());
                    this.COUNT_MISU_BHYT.Add(item.DEPARTMENT_CODE, listMisu.Where(o => IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_MISU_VP.Add(item.DEPARTMENT_CODE, listMisu.Where(o => !IsBhyt(o.TREATMENT_ID)).Count());
                    this.COUNT_MISU_IN.Add(item.DEPARTMENT_CODE, listMisu.Where(o => IsTreat(o.TREATMENT_ID)).Count());
                    this.COUNT_MISU_OUT.Add(item.DEPARTMENT_CODE, listMisu.Where(o => !IsTreat(o.TREATMENT_ID)).Count());

                    var listTest = listHisSereServTest.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_TEST.Add(item.DEPARTMENT_CODE, listTest.Sum(s=>s.AMOUNT));
                    this.COUNT_TEST_BHYT.Add(item.DEPARTMENT_CODE, listTest.Where(o => IsBhyt(o.TDL_TREATMENT_ID??0)).Sum(s=>s.AMOUNT));
                    this.COUNT_TEST_VP.Add(item.DEPARTMENT_CODE, listTest.Where(o => !IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_TEST_IN.Add(item.DEPARTMENT_CODE, listTest.Where(o => IsTreat(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_TEST_OUT.Add(item.DEPARTMENT_CODE, listTest.Where(o => !IsTreat(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));

                    var listSuim = listHisSereServSuim.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_SUIM.Add(item.DEPARTMENT_CODE, listSuim.Sum(s=>s.AMOUNT));
                    this.COUNT_SUIM_BHYT.Add(item.DEPARTMENT_CODE, listSuim.Where(o => IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_SUIM_VP.Add(item.DEPARTMENT_CODE, listSuim.Where(o => !IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_SUIM_IN.Add(item.DEPARTMENT_CODE, listSuim.Where(o => IsTreat(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_SUIM_OUT.Add(item.DEPARTMENT_CODE, listSuim.Where(o => !IsTreat(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));

                    var listXray = listHisSereServXray.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    //Inventec.Common.Logging.LogSystem.Error("ss" + string.Join(",", listXray.Select(o => o.ID)));
                    this.COUNT_XRAY.Add(item.DEPARTMENT_CODE, listXray.Sum(s=>s.AMOUNT));
                    this.COUNT_XRAY_BHYT.Add(item.DEPARTMENT_CODE, listXray.Where(o => IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_XRAY_VP.Add(item.DEPARTMENT_CODE, listXray.Where(o => !IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_XRAY_IN.Add(item.DEPARTMENT_CODE, listXray.Where(o => IsTreat(o.TDL_TREATMENT_ID ?? 0)).Sum(s=>s.AMOUNT));
                    this.COUNT_XRAY_OUT.Add(item.DEPARTMENT_CODE, listXray.Where(o => !IsTreat(o.TDL_TREATMENT_ID??0)).Sum(s=>s.AMOUNT));

                    var listEcg = listHisSereServEcg.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_ECG.Add(item.DEPARTMENT_CODE, listEcg.Sum(s=>s.AMOUNT));

                    var listBrain = listHisSereServBrain.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_BRAIN.Add(item.DEPARTMENT_CODE, listBrain.Sum(s=>s.AMOUNT));

                    var listEndo = listHisSereServEndo.Where(p => (p.TDL_EXECUTE_DEPARTMENT_ID == item.ID)).ToList();
                    this.COUNT_ENDO.Add(item.DEPARTMENT_CODE, listEndo.Sum(s=>s.AMOUNT));

                }
                listRdo.Add(rdo);

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private bool IsBhyt(long treatmentID)
        {
            return lastHisPatientTypeAlter.Exists(o => o.TREATMENT_ID == treatmentID && o.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
        }

        private bool IsTreat(long treatmentID)
        {
            return lastHisPatientTypeAlter.Exists(o => o.TREATMENT_ID == treatmentID && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_FROM ?? filter.OUT_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_TO ?? filter.OUT_TIME_TO ?? 0));

            dicSingleTag.Add("COUNT_EXAM", this.COUNT_EXAM);
            dicSingleTag.Add("COUNT_EXAM_FEMALE", this.COUNT_EXAM_FEMALE);
            dicSingleTag.Add("COUNT_EXAM_BHYT", this.COUNT_EXAM_BHYT);
            dicSingleTag.Add("COUNT_EXAM_VP", this.COUNT_EXAM_VP);
            dicSingleTag.Add("COUNT_EXAM_LESS15", this.COUNT_EXAM_LESS15);
            dicSingleTag.Add("COUNT_EXAM_TRAN", this.COUNT_EXAM_TRAN);
            dicSingleTag.Add("COUNT_IN", this.COUNT_IN);
            dicSingleTag.Add("COUNT_IN_PRE", this.COUNT_IN_PRE);
            dicSingleTag.Add("COUNT_IN_FEMALE", this.COUNT_IN_FEMALE);
            dicSingleTag.Add("COUNT_IN_BHYT", this.COUNT_IN_BHYT);
            dicSingleTag.Add("COUNT_IN_VP", this.COUNT_IN_VP);
            dicSingleTag.Add("COUNT_IN_LESS15", this.COUNT_IN_LESS15);
            dicSingleTag.Add("COUNT_IN_TRAN", this.COUNT_IN_TRAN);
            dicSingleTag.Add("TOTAL_DATE", this.TOTAL_DATE);
            dicSingleTag.Add("TOTAL_DATE_BH", this.TOTAL_DATE_BH);
            dicSingleTag.Add("COUNT_OUT", this.COUNT_OUT);
            dicSingleTag.Add("COUNT_OUT_VP", this.COUNT_OUT_VP);
            dicSingleTag.Add("COUNT_OUT_BHYT", this.COUNT_OUT_BHYT);
            dicSingleTag.Add("COUNT_OUT_LESS15", this.COUNT_OUT_LESS15);

            dicSingleTag.Add("COUNT_SURG", this.COUNT_SURG);
            dicSingleTag.Add("COUNT_SURG_IN", this.COUNT_SURG_IN);
            dicSingleTag.Add("COUNT_SURG_BHYT", this.COUNT_SURG_BHYT);
            dicSingleTag.Add("COUNT_SURG_VP", this.COUNT_SURG_VP);
            dicSingleTag.Add("COUNT_SURG_OUT", this.COUNT_SURG_OUT);

            dicSingleTag.Add("COUNT_MISU", this.COUNT_MISU);
            dicSingleTag.Add("COUNT_MISU_IN", this.COUNT_MISU_IN);
            dicSingleTag.Add("COUNT_MISU_BHYT", this.COUNT_MISU_BHYT);
            dicSingleTag.Add("COUNT_MISU_VP", this.COUNT_MISU_VP);
            dicSingleTag.Add("COUNT_MISU_OUT", this.COUNT_MISU_OUT);

            dicSingleTag.Add("COUNT_TEST", this.COUNT_TEST);
            dicSingleTag.Add("COUNT_TEST_IN", this.COUNT_TEST_IN);
            dicSingleTag.Add("COUNT_TEST_BHYT", this.COUNT_TEST_BHYT);
            dicSingleTag.Add("COUNT_TEST_VP", this.COUNT_TEST_VP);
            dicSingleTag.Add("COUNT_TEST_OUT", this.COUNT_TEST_OUT);

            dicSingleTag.Add("COUNT_SUIM", this.COUNT_SUIM);
            dicSingleTag.Add("COUNT_SUIM_IN", this.COUNT_SUIM_IN);
            dicSingleTag.Add("COUNT_SUIM_BHYT", this.COUNT_SUIM_BHYT);
            dicSingleTag.Add("COUNT_SUIM_VP", this.COUNT_SUIM_VP);
            dicSingleTag.Add("COUNT_SUIM_OUT", this.COUNT_SUIM_OUT);

            dicSingleTag.Add("COUNT_XRAY", this.COUNT_XRAY);
            dicSingleTag.Add("COUNT_XRAY_IN", this.COUNT_XRAY_IN);
            dicSingleTag.Add("COUNT_XRAY_BHYT", this.COUNT_XRAY_BHYT);
            dicSingleTag.Add("COUNT_XRAY_VP", this.COUNT_XRAY_VP);
            dicSingleTag.Add("COUNT_XRAY_OUT", this.COUNT_XRAY_OUT);

            dicSingleTag.Add("COUNT_ECG", this.COUNT_ECG);

            dicSingleTag.Add("COUNT_BRAIN", this.COUNT_BRAIN);

            dicSingleTag.Add("COUNT_ENDO", this.COUNT_ENDO);

            dicSingleTag.Add("AVG_DATE", this.AVG_DATE);

            dicSingleTag.Add("TRAN_IN_RATIO", this.TRAN_IN_RATIO);


            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }
        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {

            return listHisDepartmentTran.FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ?? new HIS_DEPARTMENT_TRAN();

        }
        //Co dieu tri noi tru
        private bool HasTreatIn(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //Co dieu tri ngoai tru
        private bool HasTreatOut(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                  && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                if (IsNotNullOrEmpty(patientTypeAlterSub))
                {
                    result = true;
                }
                else
                {
                    var patientTypeAlter = listPatientTypeAlter.Where(o => o.TREATMENT_ID == departmentTran.TREATMENT_ID
                    && o.LOG_TIME <= departmentTran.DEPARTMENT_IN_TIME).ToList();
                    if (IsNotNullOrEmpty(patientTypeAlter))
                    {
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU;
                    }
                }

            }
            catch (Exception ex)
            {
                return false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

    }
    /*
     * câu lệnh so sánh xquang báo cáo 595 và 582
     select sum(amount) from his_sere_serv where is_no_execute is null and is_expend is null
and service_id in (select service_id from v_his_service_rety_cat where category_code = 'XQ'
and report_type_code = 'MRS00595')
and service_req_id in(
select id from v_his_service_req where 
--treatment_id in (select id from his_treatment where in_time between 20180801000000 and 20180831235959)
intruction_time between 20180801000000 and 20180831235959

--and service_req_stt_id =3 
and (is_no_execute is null or is_no_execute <>1));
select count(1) from v_his_service_req where 
--treatment_id in (select id from his_treatment where in_time between 20180801000000 and 20180831235959)
intruction_time between 20180801000000 and 20180831235959
and
id in (select service_req_id from his_sere_serv where service_id in (select service_id from v_his_service_rety_cat where category_code = 'XQ'
and report_type_code = 'MRS00595'))
--and service_req_stt_id =3 
and (is_no_execute is null or is_no_execute <>1);
     */

}
