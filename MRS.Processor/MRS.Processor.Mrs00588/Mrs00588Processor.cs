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
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisPatient;

namespace MRS.Processor.Mrs00588
{
    public class Mrs00588Processor : AbstractProcessor
    {
        Mrs00588Filter filter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT_D> listPatientEthnic = new List<HIS_PATIENT_D>();
        Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicHisPatientTypeAlter = new Dictionary<long, HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long,List<HIS_DEPARTMENT_TRAN>> dicHisDepartmentTran = new Dictionary<long,List<HIS_DEPARTMENT_TRAN>>();
        List<HIS_SERVICE_REQ> listHisServiceReqExam = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listHisServiceReqMisu = new List<HIS_SERVICE_REQ>();
        List<HIS_SERVICE_REQ> listHisServiceReqSurg = new List<HIS_SERVICE_REQ>();
        List<HIS_SERE_SERV> listHisSereServByIntructionTime = new List<HIS_SERE_SERV>();
        List<HIS_DEPARTMENT> listHisDepartmentCln = new List<HIS_DEPARTMENT>();
        List<HIS_EXECUTE_ROOM> listHisExecuteRoomExam = new List<HIS_EXECUTE_ROOM>();
        decimal COUNT_SURG = 0;
        decimal COUNT_SURG_NT = 0;
        decimal COUNT_SURG_BH = 0;
        decimal COUNT_MISU = 0;
        decimal COUNT_MISU_NT = 0;
        decimal COUNT_MISU_BH = 0;
        decimal COUNT_PT = 0;
        decimal COUNT_PT_NT = 0;
        decimal COUNT_PT_BH = 0;
        decimal COUNT_TT = 0;
        decimal COUNT_TT_NT = 0;
        decimal COUNT_TT_BH = 0;
        List<Mrs00588RDO> ListRdoRoom = new List<Mrs00588RDO>();
        List<Mrs00588RDO> ListRdoDepartment = new List<Mrs00588RDO>();
        List<Mrs00588RDO> listCls = new List<Mrs00588RDO>();
        long PatientTypeIdBHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        List<long> DepartmentIdYHCTs = HisDepartmentCFG.HIS_DEPARTMENT_ID__YHCT;
        string thisReportTypeCode = "";

        public Mrs00588Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00588Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00588Filter)this.reportFilter;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("PatientTypeIdBHYT:" + PatientTypeIdBHYT);
                if (PatientTypeIdBHYT == 0)
                {
                    PatientTypeIdBHYT = 1;
                }
                //region st
                listHisTreatment = new ManagerSql().GetTreatment(filter);

                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    listHisPatientTypeAlter = new ManagerSql().GetPatientTypeAlter(filter);
                    dicHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).ToDictionary(p => p.Key, q => q.Last());
                }

                var treatmentTreatIds = listHisTreatment.Where(p => p.CLINICAL_IN_TIME != null).Select(o => o.ID).ToList();
                if (treatmentTreatIds != null && treatmentTreatIds.Count > 0)
                {
                    var listDepartmentTran = new ManagerSql().GetDepartmentTran(filter);
                    if (listDepartmentTran != null)
                    {
                        dicHisDepartmentTran = listDepartmentTran.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                    }
                }

                List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                var treatmentExamIds = listHisTreatment.Where(p => p.IN_TIME < filter.IN_TIME_TO && p.IN_TIME >= filter.IN_TIME_FROM).Select(o => o.ID).ToList();
                if (treatmentExamIds != null && treatmentExamIds.Count > 0)
                {
                    listHisServiceReq = new ManagerSql().GetServiceReq(filter);
                }

                listHisSereServByIntructionTime = new ManagerSql().GetSereServPTTTByIntructionTime(filter);

                var otherTreatmentIds = listHisSereServByIntructionTime.Select(s => s.TDL_TREATMENT_ID ?? 0).ToList();
                otherTreatmentIds = otherTreatmentIds.Where(o => !treatmentIds.Contains(o)).ToList();
                if (IsNotNullOrEmpty(otherTreatmentIds))
                {
                    var skip = 0;
                    while (otherTreatmentIds.Count - skip > 0)
                    {
                        var ids = otherTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        var patientTypeAlter = new HisPatientTypeAlterManager().Get(new HisPatientTypeAlterFilterQuery() { TREATMENT_IDs = ids });
                        if (IsNotNullOrEmpty(patientTypeAlter))
                        {
                            foreach (var item in patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID))
                            {
                                if (!dicHisPatientTypeAlter.ContainsKey(item.Key))
                                {
                                    dicHisPatientTypeAlter.Add(item.Key, item.Last());
                                }
                            }
                        }
                    }
                }
                //endregion

                //region st
                listHisServiceReqExam = listHisServiceReq.Where(o => (o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL || o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT) && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();
                listHisServiceReqMisu = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT).ToList();
                listHisServiceReqSurg = listHisServiceReq.Where(o => o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT).ToList();
                if (filter.DEPARTMENT_IDs != null && filter.EXAM_ROOM_IDs != null)
                {
                    listHisServiceReqMisu = listHisServiceReqMisu.Where(p => filter.DEPARTMENT_IDs.Contains(p.REQUEST_DEPARTMENT_ID) || filter.EXAM_ROOM_IDs.Contains(p.REQUEST_ROOM_ID)).ToList();
                    listHisServiceReqSurg = listHisServiceReqSurg.Where(p => filter.DEPARTMENT_IDs.Contains(p.REQUEST_DEPARTMENT_ID) || filter.EXAM_ROOM_IDs.Contains(p.REQUEST_ROOM_ID)).ToList();
                    listHisSereServByIntructionTime = listHisSereServByIntructionTime.Where(p => filter.DEPARTMENT_IDs.Contains(p.TDL_REQUEST_DEPARTMENT_ID) || filter.EXAM_ROOM_IDs.Contains(p.TDL_REQUEST_ROOM_ID)).ToList();
                }
                else if (filter.DEPARTMENT_IDs != null)
                {
                    listHisServiceReqMisu = listHisServiceReqMisu.Where(p => filter.DEPARTMENT_IDs.Contains(p.REQUEST_DEPARTMENT_ID)).ToList();
                    listHisServiceReqSurg = listHisServiceReqSurg.Where(p => filter.DEPARTMENT_IDs.Contains(p.REQUEST_DEPARTMENT_ID)).ToList();
                    listHisSereServByIntructionTime = listHisSereServByIntructionTime.Where(p => filter.DEPARTMENT_IDs.Contains(p.TDL_REQUEST_DEPARTMENT_ID)).ToList();
                }
                else if (filter.EXAM_ROOM_IDs != null)
                {
                    listHisServiceReqMisu = listHisServiceReqMisu.Where(p => filter.EXAM_ROOM_IDs.Contains(p.REQUEST_ROOM_ID)).ToList();
                    listHisServiceReqSurg = listHisServiceReqSurg.Where(p => filter.EXAM_ROOM_IDs.Contains(p.REQUEST_ROOM_ID)).ToList();
                    listHisSereServByIntructionTime = listHisSereServByIntructionTime.Where(p => filter.EXAM_ROOM_IDs.Contains(p.TDL_REQUEST_ROOM_ID)).ToList();
                }

                var listPatientIds = listHisTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                //Danh sách BN
                List<HIS_PATIENT_D> listPatient = new List<HIS_PATIENT_D>();
                if (listPatientIds != null && listPatientIds.Count > 0)
                {
                    listPatient = new ManagerSql().GetPatient(filter);
                    listPatientEthnic = listPatient.Where(o => (!string.IsNullOrWhiteSpace(o.ETHNIC_NAME)) && o.ETHNIC_NAME.Trim().ToUpper() != "KINH").ToList();
                    //Inventec.Common.Logging.LogSystem.Info("lastPatienttypeAlter" + lastPatienttypeAlter.Count);
                }

                //Danh sach khoa lâm sàng
                listHisDepartmentCln = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.NUM_ORDER).ToList();

                //Danh sách phòng khám
                HisExecuteRoomFilterQuery HisExecuteRoomFilter = new HisExecuteRoomFilterQuery()
                {
                    IS_EXAM = true,
                    ORDER_DIRECTION = "NUM_ORDER",
                    ORDER_FIELD = "ASC"
                };
                listHisExecuteRoomExam = new HisExecuteRoomManager().Get(HisExecuteRoomFilter);

                if (filter.DEPARTMENT_IDs != null && filter.EXAM_ROOM_IDs != null)
                {
                    listHisDepartmentCln = listHisDepartmentCln.Where(o => filter.DEPARTMENT_IDs.Contains(o.ID)).ToList();
                    listHisExecuteRoomExam = listHisExecuteRoomExam.Where(o => filter.EXAM_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                }
                else if (filter.DEPARTMENT_IDs != null)
                {
                    listHisDepartmentCln = listHisDepartmentCln.Where(o => filter.DEPARTMENT_IDs.Contains(o.ID)).ToList();
                    listHisExecuteRoomExam.Clear();
                }
                else if (filter.EXAM_ROOM_IDs != null)
                {
                    listHisExecuteRoomExam = listHisExecuteRoomExam.Where(o => filter.EXAM_ROOM_IDs.Contains(o.ROOM_ID)).ToList();
                    listHisDepartmentCln.Clear();
                }
                //endregion
                
                //số lần làm cận lâm sàng
                listCls = new ManagerSql().GetListCls(filter);
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
                foreach (var item in listHisDepartmentCln)
                {
                    Mrs00588RDO rdo = new Mrs00588RDO();
                    rdo.NAME = item.DEPARTMENT_NAME;
                    rdo.ID = item.ID;
                    rdo.REALITY_PATIENT_COUNT = item.REALITY_PATIENT_COUNT;
                    rdo.THEORY_PATIENT_COUNT = item.THEORY_PATIENT_COUNT;
                    //region st
                    var listExam = listHisServiceReqExam.Where(o => o.EXECUTE_DEPARTMENT_ID == item.ID).ToList();
                    rdo.COUNT_EXAM = listExam.Count();
                    rdo.COUNT_EXAM_FEMALE = listExam.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_EXAM_BHYT = listExam.Where(o => IsBhyt(o.TREATMENT_ID)).Count();
                    if (DepartmentIdYHCTs != null && DepartmentIdYHCTs.Contains(item.ID))
                    {
                        rdo.COUNT_EXAM_YHCT = listExam.Count();
                    }
                    rdo.COUNT_EXAM_VP = listExam.Where(o => !IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_LESS15 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_EXAM_LESS15_BHYT = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) < 15 && IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_OVER60 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_EXAM_OVER60_BHYT = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) >= 60 && IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_TRAN = listExam.Where(o => listHisTreatment.Exists
                    (p => p.END_DEPARTMENT_ID == item.ID && p.ID == o.TREATMENT_ID && p.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && p.OUT_TIME < filter.IN_TIME_TO && p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Select(q => q.TREATMENT_ID).Distinct().Count();
                    //endregion

                    var listIn = listHisTreatment.Where(o => dicHisDepartmentTran.ContainsKey(o.ID) && dicHisDepartmentTran[o.ID].Exists(p => p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatIn(p, listHisPatientTypeAlter))).ToList();
                    //region dang dieu tri noi tru
                    rdo.COUNT_IN = listIn.Count();
                    rdo.COUNT_IN_FEMALE = listIn.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_IN_BHYT = listIn.Where(o => IsBhyt(o.ID)).Count();
                    if (DepartmentIdYHCTs != null && DepartmentIdYHCTs.Contains(item.ID))
                    {
                        rdo.COUNT_IN_YHCT = listIn.Count();
                    }
                    rdo.COUNT_IN_VP = listIn.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_LESS15 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_IN_OVER60 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_IN_TRAN = listIn.Where(o => o.END_DEPARTMENT_ID == item.ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME < filter.IN_TIME_TO).Count();
                    //endregion

                    //region moi vao dieu tri noi tru
                    rdo.COUNT_IN_IMP = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM).Count();
                    rdo.COUNT_IN_IMP_FEMALE = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_IN_IMP_BHYT = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_IMP_VP = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && !IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_IMP_LESS15 = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_IN_IMP_LESS15_BHYT = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15 && IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_IMP_OVER60 = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_IN_IMP_OVER60_BHYT = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60 && IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_IMP_TRAN = listIn.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && o.END_DEPARTMENT_ID == item.ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME < filter.IN_TIME_TO).Count();
                    //endregion

                    rdo.TOTAL_DATE = listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    rdo.TOTAL_DATE_BH = listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID) && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                    var listLightDay = listHisTreatment.Where(o => dicHisDepartmentTran.ContainsKey(o.ID) && dicHisDepartmentTran[o.ID].Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatLightDay(p, listHisPatientTypeAlter))).ToList();
                    //region dang dieu tri ban ngay
                    rdo.COUNT_LIGHT = listLightDay.Count();
                    rdo.COUNT_LIGHT_FEMALE = listLightDay.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_LIGHT_BHYT = listLightDay.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_VP = listLightDay.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT__LESS15 = listLightDay.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_LIGHT_OVER60 = listLightDay.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_LIGHT_TRAN = listLightDay.Where(o => o.END_DEPARTMENT_ID == item.ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME < filter.IN_TIME_TO).Count();
                    //endregion

                    //region moi vao dieu tri ban ngay
                    rdo.COUNT_LIGHT_IMP = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM).Count();
                    rdo.COUNT_LIGHT_IMP_FEMALE = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_LIGHT_IMP_BHYT = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_IMP_VP = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && !IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_IMP_LESS15 = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_LIGHT_IMP_LESS15_BHYT = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15 && IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_IMP_OVER60 = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_LIGHT_IMP_OVER60_BHYT = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60 && IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_IMP_TRAN = listLightDay.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && o.END_DEPARTMENT_ID == item.ID && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && o.OUT_TIME < filter.IN_TIME_TO).Count();
                    //endregion

                    rdo.TOTAL_LIGHT_DATE = listLightDay.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    rdo.TOTAL_LIGHT_DATE_BH = listLightDay.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID) && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                    var listOut = listHisTreatment.Where(o => dicHisDepartmentTran.ContainsKey(o.ID) && dicHisDepartmentTran[o.ID].Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatOut(p, listHisPatientTypeAlter))).ToList();

                    //region dang dieu tri ngoai tru
                    rdo.COUNT_OUT = listOut.Count();
                    rdo.COUNT_OUT_BHYT = listOut.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_OUT_LESS15 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_OUT_OVER60 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    //endregion

                    //region moi vao dieu tri ngoai tru
                    rdo.COUNT_OUT_IMP = listOut.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM).Count();
                    rdo.COUNT_OUT_IMP_BHYT = listOut.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && IsBhyt(o.ID)).Count();
                    rdo.COUNT_OUT_IMP_LESS15 = listOut.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_OUT_IMP_OVER60 = listOut.Where(o => o.IN_TIME < filter.IN_TIME_TO && o.IN_TIME >= filter.IN_TIME_FROM && Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    //endregion

                    rdo.TOTAL_OUT_DATE = listOut.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    rdo.TOTAL_OUT_DATE_BH = listOut.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID) && o.END_DEPARTMENT_ID == item.ID).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                    var listDeath = listHisTreatment.Where(o => (listHisServiceReqExam.Exists(q => q.TREATMENT_ID == o.ID && q.EXECUTE_DEPARTMENT_ID == item.ID) || dicHisDepartmentTran.ContainsKey(o.ID) && dicHisDepartmentTran[o.ID].Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO)) && o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET).ToList();
                    rdo.COUNT_DEATH = listDeath.Count();
                    rdo.COUNT_DEATH_LESS1 = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 1).Count();
                    rdo.COUNT_DEATH_LESS1_FEMALE = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 1 && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_DEATH_LESS1_ETHANIC = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 1 && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();
                    rdo.COUNT_DEATH_LESS5 = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 5).Count();
                    rdo.COUNT_DEATH_LESS5_FEMALE = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 5 && o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_DEATH_LESS5_ETHANIC = listDeath.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 5 && listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();

                    //them thong tin cls
                    var cls = listCls.FirstOrDefault(o => o.ID == rdo.ID);
                    if (cls != null)
                    {
                        rdo.AMOUNT_CT = cls.AMOUNT_CT;
                        rdo.AMOUNT_XQUANG = cls.AMOUNT_XQUANG;
                        rdo.AMOUNT_TEST = cls.AMOUNT_TEST;
                        rdo.AMOUNT_MRI = cls.AMOUNT_MRI;
                        rdo.AMOUNT_SIEUAM = cls.AMOUNT_SIEUAM;
                        rdo.DIC_CATE_AMOUNT = cls.DIC_CATE_AMOUNT ?? new Dictionary<string, decimal>();
                    }

                    ListRdoDepartment.Add(rdo);
                }

                //region st
                foreach (var item in listHisExecuteRoomExam)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == item.ROOM_ID);
                    Mrs00588RDO rdo = new Mrs00588RDO();
                    rdo.NAME = item.EXECUTE_ROOM_NAME;
                    rdo.ID = item.ROOM_ID;
                    var listExam = listHisServiceReqExam.Where(o => o.EXECUTE_ROOM_ID == item.ROOM_ID).ToList();
                    rdo.COUNT_EXAM = listExam.Count();
                    rdo.COUNT_EXAM_FEMALE = listExam.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_EXAM_BHYT = listExam.Where(o => IsBhyt(o.TREATMENT_ID)).Count();
                    if (DepartmentIdYHCTs != null && room != null && DepartmentIdYHCTs.Contains(room.DEPARTMENT_ID))
                    {
                        rdo.COUNT_EXAM_YHCT = listExam.Count();
                    }
                    rdo.COUNT_EXAM_VP = listExam.Where(o => !IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_LESS15 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_EXAM_OVER60 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_EXAM_TRAN = listExam.Where(o => listHisTreatment.Exists
                    (p => p.END_ROOM_ID == item.ROOM_ID && p.ID == o.TREATMENT_ID && p.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && p.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)).Select(q => q.TREATMENT_ID).Distinct().Count();

                    var listIn = listHisTreatment.Where(o => listExam.Exists(p => p.TREATMENT_ID == o.ID) && dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                    rdo.COUNT_IN = listIn.Count();
                    rdo.COUNT_IN_FEMALE = listIn.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_IN_BHYT = listIn.Where(o => IsBhyt(o.ID)).Count();
                    if (DepartmentIdYHCTs != null && room != null && DepartmentIdYHCTs.Contains(room.DEPARTMENT_ID))
                    {
                        rdo.COUNT_IN_YHCT = listIn.Count();
                    }
                    rdo.COUNT_IN_VP = listIn.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_LESS15 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_IN_OVER60 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_IN_TRAN = listIn.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count();

                    rdo.TOTAL_DATE = listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    rdo.TOTAL_DATE_BH = listIn.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID)).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                    var listLightDay = listHisTreatment.Where(o => listExam.Exists(p => p.TREATMENT_ID == o.ID) && dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
                    rdo.COUNT_LIGHT = listLightDay.Count();
                    rdo.COUNT_LIGHT_FEMALE = listLightDay.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_LIGHT_BHYT = listLightDay.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_VP = listLightDay.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_LIGHT_LESS15 = listLightDay.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_LIGHT_OVER60 = listLightDay.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_LIGHT_TRAN = listLightDay.Where(o => o.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count();

                    rdo.TOTAL_LIGHT_DATE = listLightDay.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);
                    rdo.TOTAL_LIGHT_DATE_BH = listLightDay.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && IsBhyt(o.ID)).Sum(s => s.TREATMENT_DAY_COUNT ?? 0);

                    var listOut = listHisTreatment.Where(o => listExam.Exists(p => p.TREATMENT_ID == o.ID) && dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                    rdo.COUNT_OUT = listOut.Count();
                    rdo.COUNT_OUT_BHYT = listOut.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_OUT_LESS15 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 15).Count();
                    rdo.COUNT_OUT_OVER60 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    ListRdoRoom.Add(rdo);
                }

                COUNT_SURG = listHisServiceReqSurg.Count();
                COUNT_SURG_NT = listHisServiceReqSurg.Where(o => dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                COUNT_SURG_BH = listHisServiceReqSurg.Where(o => IsBhyt(o.TREATMENT_ID)).Count();

                COUNT_MISU = listHisServiceReqMisu.Count();
                COUNT_MISU_NT = listHisServiceReqMisu.Where(o =>  dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.TREATMENT_ID].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Count();
                COUNT_MISU_BH = listHisServiceReqMisu.Where(o => IsBhyt(o.TREATMENT_ID)).Count();

                var reqSurg = listHisSereServByIntructionTime.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).ToList();
                COUNT_PT = reqSurg.Sum(o => o.AMOUNT);
                COUNT_PT_NT = reqSurg.Where(o => dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.TDL_TREATMENT_ID??0].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(o => o.AMOUNT);
                COUNT_PT_BH = reqSurg.Where(o => IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(o => o.AMOUNT);

                var reqMisu = listHisSereServByIntructionTime.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
                COUNT_TT = reqMisu.Sum(o => o.AMOUNT);
                COUNT_TT_NT = reqMisu.Where(o =>  dicHisPatientTypeAlter.ContainsKey(o.ID) && dicHisPatientTypeAlter[o.TDL_TREATMENT_ID??0].TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(o => o.AMOUNT);
                COUNT_TT_BH = reqMisu.Where(o => IsBhyt(o.TDL_TREATMENT_ID ?? 0)).Sum(o => o.AMOUNT);

                result = true;
                //endregion
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        //region st
        private bool IsBhyt(long treatmentId)
        {
            return dicHisPatientTypeAlter.ContainsKey(treatmentId) && dicHisPatientTypeAlter[treatmentId].PATIENT_TYPE_ID == PatientTypeIdBHYT;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_TO ?? 0));
            if (filter.DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => filter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }

            dicSingleTag.Add("COUNT_SURG", COUNT_SURG);
            dicSingleTag.Add("COUNT_SURG_NT", COUNT_SURG_NT);
            dicSingleTag.Add("COUNT_SURG_BH", COUNT_SURG_BH);
            dicSingleTag.Add("COUNT_MISU", COUNT_MISU);
            dicSingleTag.Add("COUNT_MISU_NT", COUNT_MISU_NT);
            dicSingleTag.Add("COUNT_MISU_BH", COUNT_MISU_BH);
            dicSingleTag.Add("COUNT_PT", COUNT_PT);
            dicSingleTag.Add("COUNT_PT_NT", COUNT_PT_NT);
            dicSingleTag.Add("COUNT_PT_BH", COUNT_PT_BH);
            dicSingleTag.Add("COUNT_TT", COUNT_TT);
            dicSingleTag.Add("COUNT_TT_NT", COUNT_TT_NT);
            dicSingleTag.Add("COUNT_TT_BH", COUNT_TT_BH);
            objectTag.AddObjectData(store, "ReportRoom", ListRdoRoom);
            objectTag.AddObjectData(store, "ReportDepartment", ListRdoDepartment);
            objectTag.AddObjectData(store, "ReportCls", listCls);
        }

        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }
        //endregion

        //khoa lien ke
        private HIS_DEPARTMENT_TRAN NextDepartment(HIS_DEPARTMENT_TRAN o)
        {
            return dicHisDepartmentTran.ContainsKey(o.TREATMENT_ID)?(dicHisDepartmentTran[o.TREATMENT_ID].FirstOrDefault(p => p.TREATMENT_ID == o.TREATMENT_ID && p.PREVIOUS_ID == o.ID) ??new HIS_DEPARTMENT_TRAN()): new HIS_DEPARTMENT_TRAN();
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

        //Co dieu tri ban ngay
        private bool HasTreatLightDay(HIS_DEPARTMENT_TRAN departmentTran, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter)
        {
            bool result = false;
            try
            {
                var patientTypeAlterSub = listPatientTypeAlter.Where(o => o.DEPARTMENT_TRAN_ID == departmentTran.ID
                   && o.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY).ToList();
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
                        result = patientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last().TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTBANNGAY;
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

}
