using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.Filter;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport.RDO;

namespace MRS.Processor.Mrs00192
{
    internal class Mrs00192Processor : AbstractProcessor
    {
        List<VSarReportMrs00192RDO> _listSarReportMrs00192Rdos = new List<VSarReportMrs00192RDO>();
        List<VSarReportMrs00192RDO> listSarReportMrs00192Rdos = new List<VSarReportMrs00192RDO>();
        List<VSarReportMrs00192RDO> listRdo = new List<VSarReportMrs00192RDO>();
        Mrs00192Filter CastFilter;
        List<HIS_CAREER> listCareer = new List<HIS_CAREER>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        List<HIS_PATIENT> listPatient = new List<HIS_PATIENT>();
        List<HIS_SERE_SERV> listSereServAlls = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE_REQ> listServiceReqAlls = new List<V_HIS_SERVICE_REQ>();
        List<HIS_DEPARTMENT_TRAN> listDepartmentTrans = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();

        long PatientTypeIdFEE = HisPatientTypeCFG.PATIENT_TYPE_ID__FEE;
        long PatientTypeIdBHYT = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        long PatientTypeIdIS_FREE = HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE;
        long PatientTypeIdKSK = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;

        public Mrs00192Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00192Filter);
        }
        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00192Filter)this.reportFilter;
                // TREATMENT: lấy hồ sơ của BN đã vao vien trong thoi gian chon
                listTreatment = getTreatment();
                string query = string.Format("select * from his_branch where id = {0}", branch_id);
                listBranch = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_BRANCH>(query);
                listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
                if (listTreatment != null)
                {
                    var listTreatmentIds = listTreatment.Select(o => o.ID).ToList();
                    var skip = 0;
                    while (listTreatmentIds.Count() - skip > 0)
                    {
                        var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var metyFilterSereServAll = new HisSereServFilterQuery
                        {
                            TREATMENT_IDs = listIds,
                            HAS_EXECUTE = true,
                            TDL_EXECUTE_DEPARTMENT_IDs = CastFilter.DEPARTMENT_IDs,
                            TDL_REQUEST_DEPARTMENT_IDs = CastFilter.REQUEST_DEPARTMENT_IDs
                        };
                        var listSereServAll = new HisSereServManager(paramGet).Get(metyFilterSereServAll);
                        if (listSereServAll != null)
                            listSereServAlls.AddRange(listSereServAll);
                        var metyFilterServiceReqAll = new HisServiceReqViewFilterQuery
                        {
                            TREATMENT_IDs = listIds,
                            HAS_EXECUTE = true
                        };
                        var listServiceReqAll = new HisServiceReqManager(paramGet).GetView(metyFilterServiceReqAll);
                        
                        if (listServiceReqAll != null)
                            listServiceReqAlls.AddRange(listServiceReqAll);

                        var metyFilterDepartmentTran = new HisDepartmentTranFilterQuery
                        {
                            TREATMENT_IDs = listIds
                        };
                        var listDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).Get(metyFilterDepartmentTran);
                        if (listDepartmentTran != null)
                            listDepartmentTrans.AddRange(listDepartmentTran);

                        var metyFilterPatientTypeAlter = new HisPatientTypeAlterFilterQuery
                        {
                            TREATMENT_IDs = listIds
                        };
                        var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).Get(metyFilterPatientTypeAlter);
                        if (listPatientTypeAlter != null)
                            listPatientTypeAlters.AddRange(listPatientTypeAlter);
                    }
                }

                //lọc y lệnh
                if (CastFilter.DEPARTMENT_IDs != null)
                {
                    listServiceReqAlls = listServiceReqAlls.Where(p => CastFilter.DEPARTMENT_IDs.Contains(p.EXECUTE_DEPARTMENT_ID)).ToList();
                    var srIds = listServiceReqAlls.Select(o => o.ID).Distinct().ToList();
                    listSereServAlls = listSereServAlls.Where(o => srIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                    var treaIds = listServiceReqAlls.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    listTreatment = listTreatment.Where(o => treaIds.Contains(o.ID)).ToList();
                }
                if (CastFilter.REQUEST_DEPARTMENT_IDs != null)
                {
                    listServiceReqAlls = listServiceReqAlls.Where(p => CastFilter.REQUEST_DEPARTMENT_IDs.Contains(p.REQUEST_DEPARTMENT_ID)).ToList();
                    var srIds = listServiceReqAlls.Select(o => o.ID).Distinct().ToList();
                    listSereServAlls = listSereServAlls.Where(o => srIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                    var treaIds = listServiceReqAlls.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    listTreatment = listTreatment.Where(o => treaIds.Contains(o.ID)).ToList();
                }
                if (CastFilter.EXAM_ROOM_IDs != null)
                {
                    listServiceReqAlls = listServiceReqAlls.Where(p => p.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && CastFilter.EXAM_ROOM_IDs.Contains(p.EXECUTE_ROOM_ID) || p.SERVICE_REQ_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH && CastFilter.EXAM_ROOM_IDs.Contains(p.REQUEST_ROOM_ID)).ToList();
                    var srIds = listServiceReqAlls.Select(o => o.ID).Distinct().ToList();
                    listSereServAlls = listSereServAlls.Where(o => srIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                    var treaIds = listServiceReqAlls.Select(o => o.TREATMENT_ID).Distinct().ToList();
                    listTreatment = listTreatment.Where(o => treaIds.Contains(o.ID)).ToList();

                }

                if (listTreatment != null)
                {
                    var listPatientIds = listTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                    var skip = 0;
                    while (listPatientIds.Count() - skip > 0)
                    {
                        var listIds = listPatientIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var patientfilter = new HisPatientFilterQuery
                        {
                            IDs = listIds
                        };
                        var listPatientSub = new HisPatientManager(paramGet).Get(patientfilter);
                        if (listPatientSub != null)
                            listPatient.AddRange(listPatientSub);
                    }
                }

                //bệnh nhân
                Dictionary<long, HIS_PATIENT> dicPatient = listPatient.ToDictionary(o => o.ID);

                //sereServ
                Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = listSereServAlls.GroupBy(o => o.TDL_TREATMENT_ID ?? 0).ToDictionary(p => p.Key, q => q.ToList());

                //công khám cuối
                Dictionary<long, V_HIS_SERVICE_REQ> dicLastServiceReq = listServiceReqAlls.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.OrderByDescending(o => o.FINISH_TIME).FirstOrDefault(s => s.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH) ?? new V_HIS_SERVICE_REQ());

                //công khám đầu
                Dictionary<long, V_HIS_SERVICE_REQ> dicFirstServiceReq = listServiceReqAlls.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.OrderBy(o => o.SERVICE_REQ_CODE).ThenBy(o => o.INTRUCTION_TIME).FirstOrDefault(s => s.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH) ?? new V_HIS_SERVICE_REQ());

                //các công khám


                //chuyển đổi đối tượng
                Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = listPatientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).FirstOrDefault() ?? new HIS_PATIENT_TYPE_ALTER());

                //chuyển khoa
                Dictionary<long, HIS_DEPARTMENT_TRAN> dicDepartmentTran = listDepartmentTrans.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.OrderBy(o => o.DEPARTMENT_IN_TIME).ThenBy(o => o.ID).FirstOrDefault(s => s.PREVIOUS_ID != null) ?? new HIS_DEPARTMENT_TRAN());
                if (CastFilter.INPUT_DATA_ID_REPORT_TYPE == 1)
                {
                    ProcessFilterData(dicSereServ, dicPatient, listTreatment, dicDepartmentTran, dicPatientTypeAlter, dicFirstServiceReq, dicLastServiceReq);
                }
                else if (CastFilter.INPUT_DATA_ID_REPORT_TYPE == 2)
                {
                    ProcessDataSereServ(dicSereServ, dicPatient, listTreatment, dicDepartmentTran, dicPatientTypeAlter, dicFirstServiceReq, dicLastServiceReq, listServiceReqAlls);
                }
                else
                {
                    ProcessFilterData(dicSereServ, dicPatient, listTreatment, dicDepartmentTran, dicPatientTypeAlter, dicFirstServiceReq, dicLastServiceReq);
                    ProcessDataSereServ(dicSereServ, dicPatient, listTreatment, dicDepartmentTran, dicPatientTypeAlter, dicFirstServiceReq, dicLastServiceReq, listServiceReqAlls);
                }

                result = true;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<HIS_TREATMENT> getTreatment()
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                string query = "";
                query += "select * from his_treatment\n";
                query += string.Format("where in_time between {0} and {1}\n", CastFilter.DATE_FROM, CastFilter.DATE_TO);
                if (IsNotNullOrEmpty(CastFilter.CREATOR_LOGINNAMEs))
                {
                    query += string.Format("and creator in ('{0}')\n", string.Join("','", CastFilter.CREATOR_LOGINNAMEs));
                }
                if (IsNotNullOrEmpty(CastFilter.LOGINNAME_DOCTORs))
                {
                    query += string.Format("and DOCTOR_LOGINNAME in ('{0}')\n", string.Join("','", CastFilter.LOGINNAME_DOCTORs));
                }
                //if (CastFilter.DEPARTMENT_IDs != null)
                //{
                //    query += string.Format("and in_department_id in ({0})\n", string.Join(",", CastFilter.DEPARTMENT_IDs));
                //}
                //if (CastFilter.EXAM_ROOM_IDs != null)
                //{
                //    query += string.Format("and in_room_id in ({0})\n", string.Join(",", CastFilter.EXAM_ROOM_IDs));
                //}
                if (CastFilter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and tdl_patient_type_id in ({0})\n", string.Join(",", CastFilter.PATIENT_TYPE_IDs));
                }
                Inventec.Common.Logging.LogSystem.Info(query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_TREATMENT>();
            }
            return result;

        }


        protected override bool ProcessData()
        {
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(CastFilter.DATE_TO));

            if (CastFilter.INPUT_DATA_ID_REPORT_TYPE == 1)
            {
                objectTag.AddObjectData(store, "Report", _listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report1", _listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report2", listRdo.OrderBy(p => p.COL_19_1).ToList());

                objectTag.AddObjectData(store, "Report3", _listSarReportMrs00192Rdos.OrderBy(p => p.INTRUCTION_TIME).ToList());
            }
            else if (CastFilter.INPUT_DATA_ID_REPORT_TYPE == 2)
            {

                objectTag.AddObjectData(store, "Report", listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report1", listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report2", listRdo.OrderBy(p => p.COL_19_1).ToList());

                objectTag.AddObjectData(store, "Report3", _listSarReportMrs00192Rdos.OrderBy(p => p.INTRUCTION_TIME).ToList());
            }
            else
            {
                objectTag.AddObjectData(store, "Report", _listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report1", listSarReportMrs00192Rdos);
                objectTag.AddObjectData(store, "Report2", listRdo.OrderBy(p => p.COL_19_1).ToList());

                objectTag.AddObjectData(store, "Report3", _listSarReportMrs00192Rdos.OrderBy(p => p.INTRUCTION_TIME).ToList());
            }




        }

        private void ProcessFilterData(Dictionary<long, List<HIS_SERE_SERV>> dicSereServ, Dictionary<long, HIS_PATIENT> dicPatient, List<HIS_TREATMENT> listTreatments, Dictionary<long, HIS_DEPARTMENT_TRAN> dicDepartmentTran, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, Dictionary<long, V_HIS_SERVICE_REQ> dicFirstServiceReq, Dictionary<long, V_HIS_SERVICE_REQ> dicLastServiceReq)
        {
            try
            {
                var COL_01 = 0;

                foreach (var treatment in listTreatments)
                {
                    // TREATMENT
                    var COL_02 = treatment.TDL_PATIENT_CODE;
                    var COL_03 = treatment.TDL_PATIENT_NAME;
                    var COL_04 = "";
                    var COL_05 = "";
                    var COL_05_1 = IsNotNullOrEmpty(treatment.TDL_PATIENT_CAREER_NAME) && (treatment.TDL_PATIENT_CAREER_NAME.Contains("<") || treatment.TDL_PATIENT_CAREER_NAME.Contains("em")) ? "X" : "";
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        COL_04 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                    }
                    else if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        COL_05 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.TDL_PATIENT_DOB);
                    }




                    var COL_06 = treatment.TDL_PATIENT_ADDRESS ?? "";
                    var patient = dicPatient.ContainsKey(treatment.PATIENT_ID) ? dicPatient[treatment.PATIENT_ID] : new HIS_PATIENT();
                    var COL_26 = patient.PHONE ?? treatment.TDL_PATIENT_PHONE ?? treatment.TDL_PATIENT_MOBILE;
                    // TRAN_PATI
                    var COL_36 = treatment.MEDI_ORG_NAME ?? "";
                    var COL_08 = treatment.TRANSFER_IN_MEDI_ORG_NAME ?? "";
                    var COL_09 = treatment.ICD_CODE ?? "" + " - " + treatment.ICD_NAME;
                    // Treatment
                    var COL_23 = treatment.IS_EMERGENCY != null ? "X" : "";
                    // SERE_SERV
                    var listSereServ = dicSereServ.ContainsKey(treatment.ID) ? dicSereServ[treatment.ID] : new List<HIS_SERE_SERV>();
                    var examServiceReq = dicLastServiceReq.ContainsKey(treatment.ID) ? dicLastServiceReq[treatment.ID] : new V_HIS_SERVICE_REQ();
                    var firstExamServiceReq = dicFirstServiceReq.ContainsKey(treatment.ID) ? dicFirstServiceReq[treatment.ID] : new V_HIS_SERVICE_REQ();
                    var patientTypeAlter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new HIS_PATIENT_TYPE_ALTER();


                    var GUARANTEE_USERNAME = patientTypeAlter.GUARANTEE_USERNAME ?? "";
                
                    
                    COL_01 = COL_01 + 1;
                    var COL_07 = treatment.TDL_HEIN_CARD_NUMBER ?? "";
                    var COL_10 = examServiceReq.ICD_NAME;
                    var COL_11 = "";
                    var COL_11_1 = "";
                    var COL_17 = "";
                    var COL_18 = "X";
                    COL_11 = string.Join(" \n ", listSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Select(p => p.TDL_SERVICE_CODE).ToList());
                    COL_11_1 = string.Join(" \n ", listSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Select(p => p.TDL_SERVICE_NAME).ToList());
                    if (COL_11.Length > 0)
                    {
                        COL_17 = "X";
                    }
                    var COL_19_1 = examServiceReq.REQUEST_USERNAME;
                    var COL_19 = examServiceReq.EXECUTE_USERNAME;
                    var COL_20 = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examServiceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    var COL_20_1 = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == firstExamServiceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    var COL_21 = patientTypeAlter.PATIENT_TYPE_ID == PatientTypeIdFEE ? "X" : "";
                    var COL_22 = "";
                    var COL_24 = patientTypeAlter.PATIENT_TYPE_ID == PatientTypeIdBHYT ? "X" : "";
                    var COL_27 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(examServiceReq.START_TIME ?? 0) ?? "";
                    //~~~~~~~~~~~~~~~~~~
                    var COL_13 = "";
                    var COL_14 = "";

                    var COL_28 = "";
                    var COL_29 = "";
                    var COL_30 = "";
                    var COL_31 = "";
                    var COL_32 = "";
                    var COL_33 = "";
                    var COL_34 = "";
                    var COL_35 = "";
                    var COL_12 = "";
                    var COL_15 = "";
                    var COL_16 = "";

                    // chuyen vien
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        COL_29 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                        // tuyen tren, tuyen duoi
                        if (treatment.MEDI_ORG_CODE != null)
                        {
                            if (treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT || treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT)
                            {
                                COL_13 = "X";
                            }
                            if (treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN)
                            {
                                COL_14 = "X";
                            }
                        }
                    }
                    // cap toa cho ve
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV)
                    {
                        COL_16 = "X";
                        COL_30 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                    }
                    // nhap vien: noi tru - ngoai tru

                    var departmentTranIn = dicDepartmentTran.ContainsKey(treatment.ID) ? dicDepartmentTran[treatment.ID] : new HIS_DEPARTMENT_TRAN();
                    var in_department_name = "";
                    var department = listDepartment.Where(x => x.ID == departmentTranIn.DEPARTMENT_ID).FirstOrDefault();
                    if (department != null)
                    {
                        in_department_name = department.DEPARTMENT_NAME;
                    }
                   // var GUARANTEE_USERNAME = "";
                   // var patientTypeAfter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new HIS_PATIENT_TYPE_ALTER();
                    //if (GUARANTEE_USERNAME != null)
                    //{
                   // var GUARANTEE_USERNAME = patientTypeAlter.GUARANTEE_USERNAME;
                    //}  
                    COL_28 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTranIn.DEPARTMENT_IN_TIME ?? 0) ?? "";
                    COL_32 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME) ?? "";
                    COL_33 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(examServiceReq.INTRUCTION_TIME) ?? "";
                    COL_34 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.CLINICAL_IN_TIME ?? 0) ?? "";
                    COL_35 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                    COL_31 = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTranIn.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    // noi tru - ngoai tru
                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        COL_12 = "X";
                    }
                    else if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        COL_15 = "X";
                    }
                    var COL_25 = "";
                    if (treatment.TDL_HEIN_CARD_NUMBER == null || treatment.TDL_HEIN_CARD_NUMBER.Length < 14 || treatment.TDL_HEIN_CARD_NUMBER.Substring(3, 2) != "01")
                        if (treatment.TDL_PATIENT_ADDRESS == null || !treatment.TDL_PATIENT_ADDRESS.ToUpper().Contains("HÀ NỘI"))
                        {
                            COL_25 = "X";
                        }
                    string isBHYT = "";
                    string isKSK = "";
                    string isFee = "";
                    string isChild = "";
                    string isHasMedi = "";
                    string isSick = "";
                    string isTreatBHYT = "";
                    string isTreatFree = "";
                    string isTreatVP = "";
                    string isTransOut = "";
                    string isLeftLine = "";

                    if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT)
                    {
                        isBHYT = "X";
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdKSK)
                    {
                        isKSK = "X";
                    }
                    else
                    {
                        isFee = "X";
                    }
                    if (RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB) < 15)
                    {
                        isChild = "X";
                    }
                    if (listSereServ.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Count() > 0)
                    {
                        isHasMedi = "X";
                    }
                    if (treatment.TREATMENT_END_TYPE_EXT_ID != null && treatment.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM)
                    {
                        isSick = "X";
                    }
                    if (treatment.CLINICAL_IN_TIME != null)
                    {
                        if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdBHYT)
                        {
                            isTreatBHYT = "X";
                        }
                        else if (treatment.TDL_PATIENT_TYPE_ID == PatientTypeIdIS_FREE)
                        {
                            isTreatFree = "X";
                        }
                        else
                        {
                            isTreatVP = "X";
                        }
                    }
                    if (treatment.TREATMENT_END_TYPE_ID != null && treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && treatment.IS_PAUSE == 1)
                    {
                        isTransOut = "X";
                    }
                    if (patientTypeAlter.RIGHT_ROUTE_CODE == MOS.LibraryHein.Bhyt.HeinRightRoute.HeinRightRouteCode.TRUE && listBranch != null && listBranch.Where(p => p.HEIN_MEDI_ORG_CODE == patientTypeAlter.HEIN_MEDI_ORG_CODE).Count() > 0)
                    {
                        isLeftLine = "X";
                    }
                    var IN_DATE = treatment.IN_DATE;
                    VSarReportMrs00192RDO rdo = new VSarReportMrs00192RDO();
                    rdo.COL_01 = COL_01 != null ? COL_01.ToString() : null;
                    rdo.COL_02 = COL_02 != null ? COL_02.ToString() : null;
                    rdo.COL_03 = COL_03 != null ? COL_03.ToString() : null;
                    rdo.COL_04 = COL_04 != null ? COL_04.ToString() : null;
                    rdo.COL_05 = COL_05 != null ? COL_05.ToString() : null;
                    rdo.COL_05_1 = COL_05_1 != null ? COL_05_1.ToString() : null;
                    rdo.COL_06 = COL_06 != null ? COL_06.ToString() : null;
                    rdo.COL_07 = COL_07 != null ? COL_07.ToString() : null;
                    rdo.COL_08 = COL_08 != null ? COL_08.ToString() : null;
                    rdo.COL_09 = COL_09 != null ? COL_09.ToString() : null;
                    rdo.COL_10 = COL_10 != null ? COL_10.ToString() : null;
                    rdo.COL_11_1 = COL_11_1 != null ? COL_11_1.ToString() : null;
                    rdo.COL_11 = COL_11 != null ? COL_11.ToString() : null;
                    rdo.COL_12 = COL_12 != null ? COL_12.ToString() : null;
                    rdo.COL_13 = COL_13 != null ? COL_13.ToString() : null;
                    rdo.COL_14 = COL_14 != null ? COL_14.ToString() : null;
                    rdo.COL_15 = COL_15 != null ? COL_15.ToString() : null;
                    rdo.COL_16 = COL_16 != null ? COL_16.ToString() : null;
                    rdo.COL_17 = COL_17 != null ? COL_17.ToString() : null;
                    rdo.COL_18 = COL_18 != null ? COL_18.ToString() : null;
                    rdo.COL_19 = COL_19 != null ? COL_19.ToString() : null;
                    rdo.COL_19_1 = COL_19_1 != null ? COL_19_1.ToString() : null;
                    rdo.COL_20 = COL_20 != null ? COL_20.ToString() : null;
                    rdo.COL_20_1 = COL_20_1 != null ? COL_20_1.ToString() : null;
                    rdo.COL_21 = COL_21 != null ? COL_21.ToString() : null;
                    rdo.COL_22 = COL_22 != null ? COL_22.ToString() : null;
                    rdo.COL_23 = COL_23 != null ? COL_23.ToString() : null;
                    rdo.COL_24 = COL_24 != null ? COL_24.ToString() : null;
                    rdo.COL_25 = COL_25 != null ? COL_25.ToString() : null;
                    rdo.COL_26 = COL_26 != null ? COL_26.ToString() : null;
                    rdo.COL_27 = COL_27 != null ? COL_27.ToString() : null;
                    rdo.COL_28 = COL_28 != null ? COL_28.ToString() : null;
                    rdo.COL_29 = COL_29 != null ? COL_29.ToString() : null;
                    rdo.COL_30 = COL_30 != null ? COL_30.ToString() : null;
                    rdo.COL_31 = COL_31 != null ? COL_31.ToString() : null;
                    rdo.COL_36 = COL_36 != null ? COL_36.ToString() : null;
                    rdo.IN_TIME = COL_32 != null ? COL_32.ToString() : null;
                    rdo.INTRUCTION_TIME = COL_33 != null ? COL_33.ToString() : null;
                    rdo.CLINICAL_IN_TIME = COL_34 != null ? COL_34.ToString() : null;
                    rdo.OUT_TIME = COL_35 != null ? COL_35.ToString() : null;
                    rdo.IN_DEPARTMENT_NAME = in_department_name;
                    rdo.GUARANTEE_USERNAME = GUARANTEE_USERNAME;
                    rdo.IS_BHYT = isBHYT;
                    rdo.IS_KSK = isKSK;
                    rdo.IS_FEE = isFee;
                    rdo.IS_CHILD = isChild;
                    rdo.IS_HAS_MEDI = isHasMedi;
                    rdo.IS_SICK = isSick;
                    rdo.IS_TREAT_BHYT = isTreatBHYT;
                    rdo.IS_TREAT_VP = isTreatVP;
                    rdo.IS_TREAT_FREE = isTreatFree;
                    rdo.IS_TRANS_OUT = isTransOut;
                    rdo.IS_LEFT_LINE = isLeftLine;
                    rdo.IN_DATE = IN_DATE;
                    _listSarReportMrs00192Rdos.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessDataSereServ(Dictionary<long, List<HIS_SERE_SERV>> dicSereServ, Dictionary<long, HIS_PATIENT> dicPatient, List<HIS_TREATMENT> listTreatments, Dictionary<long, HIS_DEPARTMENT_TRAN> dicDepartmentTran, Dictionary<long, HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter, Dictionary<long, V_HIS_SERVICE_REQ> dicFirstServiceReq, Dictionary<long, V_HIS_SERVICE_REQ> dicLastServiceReq, List<V_HIS_SERVICE_REQ> listServiceReqAlls)
        {
            try
            {
                var COL_01 = 0;
                var COL_00 = "";
                foreach (var ss in listSereServAlls.Where(o => o.TDL_SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH).ToList())
                {
                    // TREATMENT
                    var treatment = listTreatments.FirstOrDefault(p => p.ID == ss.TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                    var checkTreatment = listTreatments.FirstOrDefault(p => p.ID == treatment.ID && p.IN_TIME == treatment.IN_TIME);
                    if (checkTreatment != null && checkTreatment.TREATMENT_END_TYPE_ID != null)
                    {
                        COL_00 = "X";
                    }
                    var COL_02 = treatment.TDL_PATIENT_CODE;
                    var COL_03 = treatment.TDL_PATIENT_NAME;
                    var COL_04 = "";
                    var COL_05 = "";
                    var COL_05_1 = IsNotNullOrEmpty(treatment.TDL_PATIENT_CAREER_NAME) && (treatment.TDL_PATIENT_CAREER_NAME.Contains("<") || treatment.TDL_PATIENT_CAREER_NAME.Contains("em")) ? "X" : "";
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        COL_04 = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                    }
                    else if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                    {
                        COL_05 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.TDL_PATIENT_DOB);
                    }




                    var COL_06 = treatment.TDL_PATIENT_ADDRESS ?? "";
                    var patient = dicPatient.ContainsKey(treatment.PATIENT_ID) ? dicPatient[treatment.PATIENT_ID] : new HIS_PATIENT();
                    var COL_26 = patient.PHONE ?? treatment.TDL_PATIENT_PHONE ?? treatment.TDL_PATIENT_MOBILE;
                    // TRAN_PATI
                    var COL_36 = treatment.MEDI_ORG_NAME ?? "";
                    var COL_08 = treatment.TRANSFER_IN_MEDI_ORG_NAME ?? "";
                    var COL_09 = treatment.ICD_CODE ?? "" + " - " + treatment.ICD_NAME;
                    // Treatment
                    var COL_23 = treatment.IS_EMERGENCY != null ? "X" : "";
                    // SERE_SERV
                    var listSereServ = dicSereServ.ContainsKey(treatment.ID) ? dicSereServ[treatment.ID] : new List<HIS_SERE_SERV>();
                    var examServiceReq = dicLastServiceReq.ContainsKey(treatment.ID) ? dicLastServiceReq[treatment.ID] : new V_HIS_SERVICE_REQ();
                    var examServiceReqOld = listServiceReqAlls.OrderByDescending(o => o.FINISH_TIME).FirstOrDefault(s => s.TREATMENT_ID == ss.ID && s.ID == ss.SERVICE_REQ_ID && s.SERVICE_REQ_TYPE_ID == 1) ?? new V_HIS_SERVICE_REQ();
                    var firstExamServiceReq = dicFirstServiceReq.ContainsKey(treatment.ID) ? dicFirstServiceReq[treatment.ID] : new V_HIS_SERVICE_REQ();
                    var patientTypeAlter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new HIS_PATIENT_TYPE_ALTER();

                    var GUARANTEE_USERNAME = patientTypeAlter.GUARANTEE_USERNAME ?? "";

                    COL_01 = COL_01 + 1;
                    var COL_07 = treatment.TDL_HEIN_CARD_NUMBER ?? "";
                    var COL_10 = examServiceReq.ICD_NAME;
                    var COL_11 = "";
                    var COL_17 = "";
                    var COL_18 = "X";
                    COL_11 = string.Join(" \n ", listSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Select(p => p.TDL_SERVICE_CODE).ToList());
                    if (COL_11.Length > 0)
                    {
                        COL_17 = "X";
                    }
                    var COL_19_1 = examServiceReq.REQUEST_USERNAME;
                    var COL_19 = examServiceReq.EXECUTE_USERNAME;
                    var COL_20 = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examServiceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    var COL_20_1 = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examServiceReqOld.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    var COL_20_2 = examServiceReq.EXECUTE_DEPARTMENT_NAME;
                    var COL_20_3 = examServiceReq.EXECUTE_DEPARTMENT_CODE;
                    var COL_21 = patientTypeAlter.PATIENT_TYPE_ID == PatientTypeIdFEE ? "X" : "";
                    var COL_22 = "";
                    var COL_24 = patientTypeAlter.PATIENT_TYPE_ID == PatientTypeIdBHYT ? "X" : "";
                    var COL_27 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(examServiceReq.START_TIME ?? 0) ?? "";
                    //~~~~~~~~~~~~~~~~~~
                    var COL_13 = "";
                    var COL_14 = "";

                    var COL_28 = "";
                    var COL_29 = "";
                    var COL_30 = "";
                    var COL_31 = "";
                    var COL_32 = "";
                    var COL_33 = "";
                    var COL_34 = "";
                    var COL_35 = "";
                    var COL_12 = "";
                    var COL_15 = "";
                    var COL_16 = "";
                    var DOCTOR_TREATMENT = treatment.DOCTOR_USERNAME;
                    // chuyen vien
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                    {
                        COL_29 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                        // tuyen tren, tuyen duoi
                        if (treatment.MEDI_ORG_CODE != null)
                        {
                            if (treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT || treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT)
                            {
                                COL_13 = "X";
                            }
                            if (treatment.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN)
                            {
                                COL_14 = "X";
                            }
                        }
                    }
                    // cap toa cho ve
                    if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CTCV)
                    {
                        COL_16 = "X";
                        COL_30 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                    }
                    // nhap vien: noi tru - ngoai tru

                    var departmentTranIn = dicDepartmentTran.ContainsKey(treatment.ID) ? dicDepartmentTran[treatment.ID] : new HIS_DEPARTMENT_TRAN();
                    var in_department_name = "";
                    var department = listDepartment.Where(x => x.ID == departmentTranIn.DEPARTMENT_ID).FirstOrDefault();
                    if (department != null)
                    {
                        in_department_name = department.DEPARTMENT_NAME;
                    }
                    //var GUARANTEE_USERNAME = "";
                    //var patientTypeAfter = dicPatientTypeAlter.ContainsKey(treatment.ID) ? dicPatientTypeAlter[treatment.ID] : new HIS_PATIENT_TYPE_ALTER();
                    //if (GUARANTEE_USERNAME != null)
                    //{
                    //    GUARANTEE_USERNAME = patientTypeAfter.GUARANTEE_USERNAME;
                    //}  
                    COL_28 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(departmentTranIn.DEPARTMENT_IN_TIME ?? 0) ?? "";
                    COL_32 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.IN_TIME) ?? "";
                    COL_33 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(examServiceReq.INTRUCTION_TIME) ?? "";
                    COL_34 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.CLINICAL_IN_TIME ?? 0) ?? "";
                    COL_35 = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treatment.OUT_TIME ?? 0) ?? "";
                    COL_31 = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == departmentTranIn.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    // noi tru - ngoai tru
                    if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        COL_12 = "X";
                    }
                    else if (patientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU)
                    {
                        COL_15 = "X";
                    }
                    var COL_25 = "";
                    if (treatment.TDL_HEIN_CARD_NUMBER == null || treatment.TDL_HEIN_CARD_NUMBER.Length < 14 || treatment.TDL_HEIN_CARD_NUMBER.Substring(3, 2) != "01")
                        if (treatment.TDL_PATIENT_ADDRESS == null || !treatment.TDL_PATIENT_ADDRESS.ToUpper().Contains("HÀ NỘI"))
                        {
                            COL_25 = "X";
                        }
                    VSarReportMrs00192RDO rdo = new VSarReportMrs00192RDO();
                    rdo.COL_00 = COL_00 != null ? COL_00.ToString() : null;
                    rdo.COL_01 = COL_01 != null ? COL_01.ToString() : null;
                    rdo.COL_02 = COL_02 != null ? COL_02.ToString() : null;
                    rdo.COL_03 = COL_03 != null ? COL_03.ToString() : null;
                    rdo.COL_04 = COL_04 != null ? COL_04.ToString() : null;
                    rdo.COL_05 = COL_05 != null ? COL_05.ToString() : null;
                    rdo.COL_05_1 = COL_05_1 != null ? COL_05_1.ToString() : null;
                    rdo.COL_06 = COL_06 != null ? COL_06.ToString() : null;
                    rdo.COL_07 = COL_07 != null ? COL_07.ToString() : null;
                    rdo.COL_08 = COL_08 != null ? COL_08.ToString() : null;
                    rdo.COL_09 = COL_09 != null ? COL_09.ToString() : null;
                    rdo.COL_10 = COL_10 != null ? COL_10.ToString() : null;
                    rdo.COL_11 = COL_11 != null ? COL_11.ToString() : null;
                    rdo.COL_12 = COL_12 != null ? COL_12.ToString() : null;
                    rdo.COL_13 = COL_13 != null ? COL_13.ToString() : null;
                    rdo.COL_14 = COL_14 != null ? COL_14.ToString() : null;
                    rdo.COL_15 = COL_15 != null ? COL_15.ToString() : null;
                    rdo.COL_16 = COL_16 != null ? COL_16.ToString() : null;
                    rdo.COL_17 = COL_17 != null ? COL_17.ToString() : null;
                    rdo.COL_18 = COL_18 != null ? COL_18.ToString() : null;
                    rdo.COL_19 = COL_19 != null ? COL_19.ToString() : null;
                    rdo.COL_19_1 = COL_19_1 != null ? COL_19_1.ToString() : null;
                    rdo.COL_20 = COL_20 != null ? COL_20.ToString() : null;
                    rdo.COL_20_1 = COL_20_1 != null ? COL_20_1.ToString() : null;
                    rdo.COL_20_2 = COL_20_2 != null ? COL_20_2.ToString() : null;
                    rdo.COL_20_3 = COL_20_3 != null ? COL_20_3.ToString() : null;
                    rdo.COL_21 = COL_21 != null ? COL_21.ToString() : null;
                    rdo.COL_22 = COL_22 != null ? COL_22.ToString() : null;
                    rdo.COL_23 = COL_23 != null ? COL_23.ToString() : null;
                    rdo.COL_24 = COL_24 != null ? COL_24.ToString() : null;
                    rdo.COL_25 = COL_25 != null ? COL_25.ToString() : null;
                    rdo.COL_26 = COL_26 != null ? COL_26.ToString() : null;
                    rdo.COL_27 = COL_27 != null ? COL_27.ToString() : null;
                    rdo.COL_28 = COL_28 != null ? COL_28.ToString() : null;
                    rdo.COL_29 = COL_29 != null ? COL_29.ToString() : null;
                    rdo.COL_30 = COL_30 != null ? COL_30.ToString() : null;
                    rdo.COL_31 = COL_31 != null ? COL_31.ToString() : null;
                    rdo.COL_36 = COL_36 != null ? COL_36.ToString() : null;
                    rdo.IN_TIME = COL_32 != null ? COL_32.ToString() : null;
                    rdo.INTRUCTION_TIME = COL_33 != null ? COL_33.ToString() : null;
                    rdo.CLINICAL_IN_TIME = COL_34 != null ? COL_34.ToString() : null;
                    rdo.OUT_TIME = COL_35 != null ? COL_35.ToString() : null;
                    rdo.IN_DEPARTMENT_NAME = in_department_name;
                    rdo.GUARANTEE_USERNAME = GUARANTEE_USERNAME;
                    rdo.DOCTOR_TREATMENT = DOCTOR_TREATMENT;
                    listSarReportMrs00192Rdos.Add(rdo);
                }

                LogSystem.Info("count: " + listSarReportMrs00192Rdos.Count());
                if (listSarReportMrs00192Rdos != null)
                {
                    List<VSarReportMrs00192RDO> listCheck = new List<VSarReportMrs00192RDO>();
                    var listGroup = listSarReportMrs00192Rdos.Select(p => p.DOCTOR_TREATMENT).Distinct().ToList();
                    if (listGroup != null)
                    {
                        foreach (var item in listGroup)
                        {
                            var check = listSarReportMrs00192Rdos.Where(p => p.DOCTOR_TREATMENT == item).ToList();
                            VSarReportMrs00192RDO rdo = new VSarReportMrs00192RDO();
                            rdo.COL_19_1 = item;
                            rdo.DOCTOR_TREATMENT = item;
                            rdo.AMOUNT_IN = check.Count();
                            rdo.AMOUNT_IN_LASER = check.Where(p => !string.IsNullOrEmpty(p.COL_20_3) && p.COL_20_3 == "K03").Count();
                            rdo.AMOUNT_IN_SURGEON = check.Where(p => !string.IsNullOrEmpty(p.COL_20_3) && p.COL_20_3 == "K04").Count();
                            rdo.AMOUNT_IN_TBG = check.Where(p => !string.IsNullOrEmpty(p.COL_20_3) && p.COL_20_3 == "K06").Count();
                            rdo.AMOUNT_IN_UVA_UVB = check.Where(p => !string.IsNullOrEmpty(p.COL_20_3) && (p.COL_20_3.ToLower().Contains("uva") || p.COL_20_3.ToLower().Contains("uvb"))).Count();
                            listRdo.Add(rdo);
                        }
                    }
                }

            }

            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
