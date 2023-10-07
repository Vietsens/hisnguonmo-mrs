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

namespace MRS.Processor.Mrs00605
{
    public class Mrs00605Processor : AbstractProcessor
    {
        Mrs00605Filter filter = null;
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> listPatientEthnic = new List<HIS_PATIENT>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_SERVICE_REQ> listHisServiceReqExam = new List<HIS_SERVICE_REQ>();
        List<HIS_DEPARTMENT> listHisDepartmentCln = new List<HIS_DEPARTMENT>();
        List<HIS_EXECUTE_ROOM> listHisExecuteRoomExam = new List<HIS_EXECUTE_ROOM>();
        List<Mrs00605RDO> ListRdo = new List<Mrs00605RDO>();
        Mrs00605RDO RdoSum = new Mrs00605RDO();
        string thisReportTypeCode = "";

        public Mrs00605Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00605Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00605Filter)this.reportFilter;
            try
            {
                #region st
                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter.IS_OUT = false;
                listHisTreatmentfilter.IN_TIME_TO = filter.IN_TIME_TO;
                var listTreatment = new HisTreatmentManager().Get(listHisTreatmentfilter);
                listHisTreatment.AddRange(listTreatment);
                listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter.IN_TIME_TO = filter.IN_TIME_TO;
                listHisTreatmentfilter.IS_OUT = true;
                listHisTreatmentfilter.OUT_TIME_FROM = filter.IN_TIME_FROM;
                var listTreatmentOut = new HisTreatmentManager().Get(listHisTreatmentfilter);
                listHisTreatment.AddRange(listTreatmentOut);

                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {


                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);

                    }
                    lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }

                var treatmentTreatIds = listHisTreatment.Where(p => p.CLINICAL_IN_TIME != null).Select(o => o.ID).ToList();
                if (treatmentTreatIds != null && treatmentTreatIds.Count > 0)
                {


                    var skip = 0;
                    while (treatmentTreatIds.Count - skip > 0)
                    {
                        var Ids = treatmentTreatIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);

                    }
                }
                List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                var treatmentExamIds = listHisTreatment.Where(p => p.IN_TIME < filter.IN_TIME_TO && p.IN_TIME >= filter.IN_TIME_FROM).Select(o => o.ID).ToList();
                if (treatmentExamIds != null && treatmentExamIds.Count > 0)
                {


                    var skip = 0;
                    while (treatmentExamIds.Count - skip > 0)
                    {
                        var Ids = treatmentExamIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                        HisServiceReqfilter.TREATMENT_IDs = Ids;
                        HisServiceReqfilter.ORDER_DIRECTION = "ID";
                        HisServiceReqfilter.ORDER_FIELD = "ASC";
                        HisServiceReqfilter.HAS_EXECUTE = true;
                        HisServiceReqfilter.SERVICE_REQ_TYPE_IDs = new List<long>() 
                         { 
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH
                         };
                        var listHisServiceReqSub = new HisServiceReqManager(param).Get(HisServiceReqfilter);
                        if (listHisServiceReqSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisServiceReqSub GetView null");
                        else
                            listHisServiceReq.AddRange(listHisServiceReqSub);
                    }
                }
                #endregion

                #region st


                listHisServiceReqExam = listHisServiceReq.Where(o => (o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL || o.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT) && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList();

                var listPatientIds = listHisTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                //Danh sách BN
                List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
                if (listPatientIds != null && listPatientIds.Count > 0)
                {
                    var skip = 0;

                    while (listPatientIds.Count - skip > 0)
                    {
                        var limit = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                        patientFilter.IDs = limit;
                        patientFilter.ORDER_FIELD = "ID";
                        patientFilter.ORDER_DIRECTION = "ASC";

                        var listPatientSub = new HisPatientManager(param).Get(patientFilter);
                        if (listPatientSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listPatientSub Get null");
                        else
                            listPatient.AddRange(listPatientSub);
                    }
                    listPatientEthnic = listPatient.Where(o => (!string.IsNullOrWhiteSpace(o.ETHNIC_NAME)) && o.ETHNIC_NAME.Trim().ToUpper() != "KINH").ToList();
                }
                //Danh sach khoa lâm sàng
                listHisDepartmentCln = HisDepartmentCFG.DEPARTMENTs.Where(o => o.IS_CLINICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).OrderBy(o => o.NUM_ORDER).ToList();

                if (filter.DEPARTMENT_IDs != null)
                {
                    listHisDepartmentCln = listHisDepartmentCln.Where(o => filter.DEPARTMENT_IDs.Contains(o.ID)).ToList();
                }

                #endregion

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
            bool result = true;
            try
            {
                foreach (var item in listHisDepartmentCln)
                {
                    Mrs00605RDO rdo = new Mrs00605RDO();
                    rdo.NAME = item.DEPARTMENT_NAME;
                    var listExam = listHisServiceReqExam.Where(o => o.EXECUTE_DEPARTMENT_ID == item.ID).ToList();
                    rdo.COUNT_EXAM = listExam.Count();
                    rdo.COUNT_EXAM_FEMALE = listExam.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_EXAM_MALE = listExam.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).Count();
                    rdo.COUNT_EXAM_BHYT = listExam.Where(o => IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_VP = listExam.Where(o => !IsBhyt(o.TREATMENT_ID)).Count();
                    rdo.COUNT_EXAM_LESS6 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) < 6).Count();
                    rdo.COUNT_EXAM_OVER60 = listExam.Where(o => Age(o.INTRUCTION_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_EXAM_ETHANIC = listExam.Where(o => listPatientEthnic.Exists(p => p.ID == o.TDL_PATIENT_ID)).Count();
                    rdo.COUNT_EXAM_KINH = listExam.Where(o => !listPatientEthnic.Exists(p => p.ID == o.TDL_PATIENT_ID)).Count();
                    var listIn = listHisTreatment.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatIn(p, listHisPatientTypeAlter))).ToList();
                    rdo.COUNT_IN = listIn.Count();
                    rdo.COUNT_IN_FEMALE = listIn.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_IN_MALE = listIn.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).Count();
                    rdo.COUNT_IN_BHYT = listIn.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_VP = listIn.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_IN_LESS6 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6).Count();
                    rdo.COUNT_IN_OVER60 = listIn.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_IN_ETHANIC = listIn.Where(o => listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();
                    rdo.COUNT_IN_KINH = listIn.Where(o => !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();
                    var listOut = listHisTreatment.Where(o => listHisDepartmentTran.Exists(p => p.TREATMENT_ID == o.ID && p.DEPARTMENT_ID == item.ID && (NextDepartment(p).DEPARTMENT_IN_TIME ?? 99999999999999) >= filter.IN_TIME_FROM && p.DEPARTMENT_IN_TIME < filter.IN_TIME_TO && HasTreatOut(p, listHisPatientTypeAlter))).ToList();
                    rdo.COUNT_OUT = listOut.Count();
                    rdo.COUNT_OUT_FEMALE = listOut.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE).Count();
                    rdo.COUNT_OUT_MALE = listOut.Where(o => o.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE).Count();
                    rdo.COUNT_OUT_BHYT = listOut.Where(o => IsBhyt(o.ID)).Count();
                    rdo.COUNT_OUT_VP = listOut.Where(o => !IsBhyt(o.ID)).Count();
                    rdo.COUNT_OUT_LESS6 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) < 6).Count();
                    rdo.COUNT_OUT_OVER60 = listOut.Where(o => Age(o.IN_TIME, o.TDL_PATIENT_DOB) >= 60).Count();
                    rdo.COUNT_OUT_ETHANIC = listOut.Where(o => listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();
                    rdo.COUNT_OUT_KINH = listOut.Where(o => !listPatientEthnic.Exists(p => p.ID == o.PATIENT_ID)).Count();
                   
                    ListRdo.Add(rdo);
                }
                RdoSum = SumListRDO(ListRdo);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private Mrs00605RDO SumListRDO(List<Mrs00605RDO> listRdo)
        {
            Mrs00605RDO Current = new Mrs00605RDO();
            try
            {
                if (listRdo.Count > 0)
                {

                    Current.COUNT_EXAM = listRdo.Sum(s => s.COUNT_EXAM);
                    Current.COUNT_EXAM_FEMALE = listRdo.Sum(s => s.COUNT_EXAM_FEMALE);
                    Current.COUNT_EXAM_MALE = listRdo.Sum(s => s.COUNT_EXAM_MALE);
                    Current.COUNT_EXAM_BHYT = listRdo.Sum(s => s.COUNT_EXAM_BHYT);
                    Current.COUNT_EXAM_VP = listRdo.Sum(s => s.COUNT_EXAM_VP);
                    Current.COUNT_EXAM_LESS6 = listRdo.Sum(s => s.COUNT_EXAM_LESS6);
                    Current.COUNT_EXAM_OVER60 = listRdo.Sum(s => s.COUNT_EXAM_OVER60);
                    Current.COUNT_EXAM_ETHANIC = listRdo.Sum(s => s.COUNT_EXAM_ETHANIC);
                    Current.COUNT_EXAM_KINH = listRdo.Sum(s => s.COUNT_EXAM_KINH);
                    Current.COUNT_IN = listRdo.Sum(s => s.COUNT_IN);
                    Current.COUNT_IN_FEMALE = listRdo.Sum(s => s.COUNT_IN_FEMALE);
                    Current.COUNT_IN_MALE = listRdo.Sum(s => s.COUNT_IN_MALE);
                    Current.COUNT_IN_BHYT = listRdo.Sum(s => s.COUNT_IN_BHYT);
                    Current.COUNT_IN_VP = listRdo.Sum(s => s.COUNT_IN_VP);
                    Current.COUNT_IN_LESS6 = listRdo.Sum(s => s.COUNT_IN_LESS6);
                    Current.COUNT_IN_OVER60 = listRdo.Sum(s => s.COUNT_IN_OVER60);
                    Current.COUNT_IN_ETHANIC = listRdo.Sum(s => s.COUNT_IN_ETHANIC);
                    Current.COUNT_IN_KINH = listRdo.Sum(s => s.COUNT_IN_KINH);
                    Current.COUNT_OUT = listRdo.Sum(s => s.COUNT_OUT);
                    Current.COUNT_OUT_FEMALE = listRdo.Sum(s => s.COUNT_OUT_FEMALE);
                    Current.COUNT_OUT_MALE = listRdo.Sum(s => s.COUNT_OUT_MALE);
                    Current.COUNT_OUT_BHYT = listRdo.Sum(s => s.COUNT_OUT_BHYT);
                    Current.COUNT_OUT_VP = listRdo.Sum(s => s.COUNT_OUT_VP);
                    Current.COUNT_OUT_LESS6 = listRdo.Sum(s => s.COUNT_OUT_LESS6);
                    Current.COUNT_OUT_OVER60 = listRdo.Sum(s => s.COUNT_OUT_OVER60);
                    Current.COUNT_OUT_ETHANIC = listRdo.Sum(s => s.COUNT_OUT_ETHANIC);
                    Current.COUNT_OUT_KINH = listRdo.Sum(s => s.COUNT_OUT_KINH);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                Current = null;
            }
            return Current;
        }

        private bool IsBhyt(long treatmentId)
        {
            return lastHisPatientTypeAlter.Exists(q => q.TREATMENT_ID == treatmentId && q.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.IN_TIME_TO ?? 0));
            if (filter.DEPARTMENT_IDs != null)
            {
                dicSingleTag.Add("DEPARTMENT_NAME", string.Join(" - ", HisDepartmentCFG.DEPARTMENTs.Where(o => filter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
            }

            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00605RDO>();
            foreach (var item in pi)
            {
                dicSingleTag.Add(item.Name, item.GetValue(RdoSum));
            }
            objectTag.AddObjectData(store, "Report", ListRdo);
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

}
