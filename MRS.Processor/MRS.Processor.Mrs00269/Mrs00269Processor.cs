using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTranPatiForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
//using MOS.MANAGER.HisTranPati; 
using ACS.EFMODEL.DataModels;
using ACS.Filter;
using ACS.MANAGER.Core.AcsUser.Get;
using ACS.MANAGER.Manager;
using MOS.MANAGER.HisMediOrg;

namespace MRS.Processor.Mrs00269
{
    public class Mrs00269Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00269RDO> ListRdo = new List<Mrs00269RDO>();
        //private List<V_HIS_TRAN_PATI> ListTranpati = new List<V_HIS_TRAN_PATI>(); 
        private List<V_HIS_TREATMENT_4> ListTreatment = new List<V_HIS_TREATMENT_4>();
        private List<HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_TRAN_PATI_REASON> ListTranpatiReason = new List<HIS_TRAN_PATI_REASON>();
        List<HIS_TRAN_PATI_FORM> ListTranpatiForm = new List<HIS_TRAN_PATI_FORM>();
        List<HIS_TRAN_PATI_TECH> ListTranpatiTech = new List<HIS_TRAN_PATI_TECH>();
        System.Data.DataTable listData = new System.Data.DataTable();
        List<HIS_DEPARTMENT> KCCDepartments = new List<HIS_DEPARTMENT>();
        private string DiffTranOut = "";
        List<HIS_MEDI_ORG> ListMediOrg = new List<HIS_MEDI_ORG>();
        Dictionary<long, List<HIS_DEPARTMENT_TRAN>> DicDepartmentTran = new Dictionary<long, List<HIS_DEPARTMENT_TRAN>>();

        public Mrs00269Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00269Filter);
        }

        protected override bool GetData()///
        {
            var filter = ((Mrs00269Filter)reportFilter);
            var result = true;
            try
            {
                listData = new ManagerSql().GetSum(filter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }

                HisTreatmentView4FilterQuery filtermain = new HisTreatmentView4FilterQuery();
                filtermain.OUT_TIME_FROM = filter.TIME_FROM;
                filtermain.OUT_TIME_TO = filter.TIME_TO;
                filtermain.IS_PAUSE = true;
                filtermain.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                filtermain.END_ROOM_IDs = filter.EXAM_ROOM_IDs;
                ListTreatment = new HisTreatmentManager(paramGet).GetView4(filtermain);//
                if (filter.DEPARTMENT_ID != null)
                {
                    ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
                if (IsNotNullOrEmpty(filter.DEPARTMENT_IDs))
                {
                    ListTreatment = ListTreatment.Where(o => filter.DEPARTMENT_IDs.Contains(o.END_DEPARTMENT_ID.Value)).ToList();
                }

                if (filter.BRANCH_ID != null)
                {
                    ListTreatment = ListTreatment.Where(o => o.BRANCH_ID == filter.BRANCH_ID).ToList();
                }
                if (IsNotNullOrEmpty(filter.BRANCH_IDs))
                {
                    ListTreatment = ListTreatment.Where(o => filter.BRANCH_IDs.Contains(o.BRANCH_ID)).ToList();
                }
                var ListTreatmentId = ListTreatment.Select(o => o.ID).ToList();

                //Đối tượng điều trị

                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterFilterQuery()
                        {
                            TREATMENT_IDs = listIDs,
                            ORDER_DIRECTION = "ASC",
                            ORDER_FIELD = "ID"
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).Get(patientTypeAlterFilter);
                        ListPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                    ListPatientTypeAlter = ListPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }

                if (IsNotNullOrEmpty(filter.TREATMENT_TYPE_IDs))
                {
                    ListTreatment = ListTreatment.Where(o => ListPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.ID && filter.TREATMENT_TYPE_IDs.Contains(p.TREATMENT_TYPE_ID))).ToList();
                    ListTreatmentId = ListTreatment.Select(o => o.ID).ToList();
                }

                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisDepartmentTranFilterQuery tranFilter = new HisDepartmentTranFilterQuery();
                        tranFilter.TREATMENT_IDs = listIDs;
                        var departmentTran = new HisDepartmentTranManager().Get(tranFilter);
                        if (IsNotNullOrEmpty(departmentTran))
                        {
                            foreach (var item in departmentTran)
                            {
                                if (!DicDepartmentTran.ContainsKey(item.TREATMENT_ID))
                                {
                                    DicDepartmentTran[item.TREATMENT_ID] = new List<HIS_DEPARTMENT_TRAN>();
                                }

                                DicDepartmentTran[item.TREATMENT_ID].Add(item);
                            }
                        }
                    }
                }

                //li do chuyen vien
                HisTranPatiReasonFilterQuery reasonFilter = new HisTranPatiReasonFilterQuery();
                ListTranpatiReason = new HisTranPatiReasonManager().Get(reasonFilter);

                HisTranPatiFormFilterQuery reasonFormFilter = new HisTranPatiFormFilterQuery();
                ListTranpatiForm = new HisTranPatiFormManager().Get(reasonFormFilter);



                ListTranpatiTech = new ManagerSql().GetTranTech(filter);

                HisMediOrgFilterQuery orgFilter = new HisMediOrgFilterQuery() { IS_ACTIVE = 1 };
                ListMediOrg = new HisMediOrgManager().Get(orgFilter);
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
            var result = true;
            try
            {
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                ListRdo.Clear();

                //if (!IsNotNullOrEmpty(ListTranpati)) return true; 
                var listDepartment = new HisDepartmentManager(paramGet).Get(new HisDepartmentFilterQuery());
                foreach (var tranPati in ListTreatment)
                {
                    Mrs00269RDO rdo = new Mrs00269RDO(tranPati);
                    rdo.CREATE_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(tranPati.OUT_TIME ?? 0);
                    rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(tranPati.IN_TIME);
                    rdo.CREATOR = tranPati.END_USERNAME;
                    rdo.TREATMENT_CODE = tranPati.TREATMENT_CODE;
                    //var TreatmentSubPt = ListTreatment.Where(o => o.ID == tranPati.TREATMENT_ID).ToList(); 

                    rdo.PATIENT_NAME = tranPati.TDL_PATIENT_NAME;
                    rdo.TRANSPORT_VEHICLE = tranPati.TRANSPORT_VEHICLE;

                    CalcuatorAge(rdo, tranPati);
                    var listPatientTypeAlterSub = ListPatientTypeAlter.Where(o => o.TREATMENT_ID == tranPati.ID && o.HEIN_CARD_NUMBER != null).ToList();
                    if (IsNotNullOrEmpty(listPatientTypeAlterSub))
                    {
                        rdo.HEIN_CARD_NUMBER = listPatientTypeAlterSub.First().HEIN_CARD_NUMBER;
                        rdo.IS_BHYT = "x";
                    }

                    HIS_DEPARTMENT endDepartment = IsNotNullOrEmpty(listDepartment) ? listDepartment.Where(q => q.ID == tranPati.END_DEPARTMENT_ID).FirstOrDefault() : null;
                    HIS_TRAN_PATI_REASON reason = ListTranpatiReason.FirstOrDefault(o => o.ID == tranPati.TRAN_PATI_REASON_ID);
                    HIS_TRAN_PATI_FORM form_reason = ListTranpatiForm.FirstOrDefault(o => o.ID == tranPati.TRAN_PATI_FORM_ID);
                    HIS_TRAN_PATI_TECH tech_reason = ListTranpatiTech.FirstOrDefault(o => o.ID == tranPati.TRAN_PATI_TECH_ID);
                    if (reason != null)
                    {
                        rdo.TRAN_PATI_REASON_NAME = reason.TRAN_PATI_REASON_NAME;
                    }
                    if (form_reason != null) 
                    {
                        rdo.TRAN_PATI_FORM_NAME = form_reason.TRAN_PATI_FORM_NAME;
                    }
                    if (tech_reason != null)
                    {
                        rdo.TRAN_PATI_TECH_NAME = tech_reason.TRAN_PATI_TECH_NAME;
                    }
                    rdo.DEPARTMENT_NAME = IsNotNull(endDepartment) ? endDepartment.DEPARTMENT_NAME : "";

                    rdo.ICD_CODE = tranPati.ICD_CODE;
                    rdo.ICD_NAME = tranPati.ICD_NAME;
                    rdo.TRANSFER_IN_ICD_CODE = tranPati.TRANSFER_IN_ICD_CODE;
                    rdo.TRANSFER_IN_ICD_NAME = tranPati.TRANSFER_IN_ICD_NAME;
                    rdo.FORM_1a = tranPati.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT ? "x" : "";
                    rdo.FORM_1b = tranPati.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT ? "x" : "";
                    rdo.FORM_2 = tranPati.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL ? "x" : "";
                    rdo.FORM_3 = tranPati.TRAN_PATI_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN ? "x" : "";
                    rdo.REASON_4 = tranPati.TRAN_PATI_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___01 ? "x" : "";
                    rdo.REASON_5 = tranPati.TRAN_PATI_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___02 ? "x" : "";
                    rdo.MEDI_ORG_NAME = tranPati.MEDI_ORG_NAME;
                    rdo.MEDI_ORG_CODE = tranPati.MEDI_ORG_CODE;
                    var mediOrg = ListMediOrg.FirstOrDefault(o => o.MEDI_ORG_CODE == tranPati.MEDI_ORG_CODE);
                    if (mediOrg != null && !string.IsNullOrWhiteSpace(mediOrg.LEVEL_CODE))
                    {
                        var br = MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == tranPati.BRANCH_ID);
                        if (br != null && !String.IsNullOrWhiteSpace(br.HEIN_LEVEL_CODE))
                        {
                            try
                            {
                                if (long.Parse(mediOrg.LEVEL_CODE) > long.Parse(br.HEIN_LEVEL_CODE))
                                {
                                    rdo.MEDI_ORG_TYPE = 1;
                                }
                                else if (long.Parse(mediOrg.LEVEL_CODE) < long.Parse(br.HEIN_LEVEL_CODE))
                                {
                                    rdo.MEDI_ORG_TYPE = -1;
                                }
                                else if (long.Parse(mediOrg.LEVEL_CODE) == long.Parse(br.HEIN_LEVEL_CODE))
                                {
                                    rdo.MEDI_ORG_TYPE = 0;
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }

                    if (tranPati.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.END_ROOM_DEPA_NAME = IsNotNull(endDepartment) ? endDepartment.DEPARTMENT_NAME : "";
                    }
                    else
                    {
                        var room = MANAGER.Config.HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == tranPati.END_ROOM_ID);
                        rdo.END_ROOM_DEPA_NAME = IsNotNull(room) ? room.ROOM_NAME : "";
                    }

                    if (DicDepartmentTran.ContainsKey(tranPati.ID))
                    {
                        long time = tranPati.CLINICAL_IN_TIME.HasValue ? tranPati.CLINICAL_IN_TIME.Value : tranPati.IN_TIME;
                        var depaTran = DicDepartmentTran[tranPati.ID].Where(o => o.DEPARTMENT_IN_TIME.HasValue && o.DEPARTMENT_IN_TIME.Value >= time).OrderBy(o => o.DEPARTMENT_IN_TIME).FirstOrDefault();
                        
                        if (depaTran != null)
                        {
                            rdo.IN_DEPA_ICD_CODE = depaTran.ICD_CODE;
                            rdo.IN_DEPA_ICD_NAME = depaTran.ICD_NAME;
                        }

                        //khoa chuyển vào
                        var depaTranin = DicDepartmentTran[tranPati.ID].Where(o => o.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME).FirstOrDefault();
                        if (depaTranin != null)
                        {
                            var dep = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == depaTranin.DEPARTMENT_ID);
                            rdo.IN_DEPA_NAME = IsNotNull(dep) ? dep.DEPARTMENT_NAME : "";
                            string totalDay = (depaTranin.DEPARTMENT_IN_TIME - tranPati.IN_DATE).ToString();
                            DateTime? depaDate = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(depaTranin.DEPARTMENT_IN_TIME ?? 0);
                            DateTime? tranDate = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(tranPati.IN_DATE);
                            double totalDaytreatment = depaDate.Value.Subtract(tranDate.Value).TotalDays;
                            //rdo.TOTAL_DAYS_IN_TREATMENT = totalDay.Length > 6 ? (totalDay.Length == 7 ? totalDay.Remove(1, 6) : totalDay.Remove(2, 7)) : "0";
                            if (totalDaytreatment>0)
                            {
                                rdo.TOTAL_DAYS_IN_TREATMENT = "0";
                            }
                            else
                            {
                                rdo.TOTAL_DAYS_IN_TREATMENT = totalDaytreatment.ToString();
                            }
                            rdo.TRAN_IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(depaTranin.DEPARTMENT_IN_TIME ?? 0);
                        }
                    }

                    var patient = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == tranPati.TDL_PATIENT_TYPE_ID);
                    if (patient != null)
                    {
                        rdo.PATIENT_TYPE_NAME = patient.PATIENT_TYPE_NAME;
                    }
                    

                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void CalcuatorAge(Mrs00269RDO rdo, V_HIS_TREATMENT_4 treatment)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.AGE_MALE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        rdo.AGE_FEMALE = (tuoi >= 1) ? tuoi : 1;
                    }

                    rdo.AGE_STR = tuoi;
                }

                if (treatment.TDL_PATIENT_IS_HAS_NOT_DAY_DOB == 1)
                {
                    rdo.DOB_STR = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                else
                {
                    rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00269Filter)reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00269Filter)reportFilter).TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ((Mrs00269Filter)reportFilter).DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);

            if (((Mrs00269Filter)reportFilter).DEPARTMENT_CODE__OUTPATIENTs != null)
            {
                List<string> KCCDepartmentCodes = ((Mrs00269Filter)reportFilter).DEPARTMENT_CODE__OUTPATIENTs.Split(',').ToList();
                KCCDepartments = HisDepartmentCFG.DEPARTMENTs.Where(o => KCCDepartmentCodes.Contains(o.DEPARTMENT_CODE)).ToList();
                DiffTranOut = string.Join(",", ListRdo.Where(o => o.V_HIS_TREATMENT_4.TDL_TREATMENT_TYPE_ID == 3 && KCCDepartments.Exists(p => p.ID == o.V_HIS_TREATMENT_4.END_DEPARTMENT_ID)).Select(q => q.TREATMENT_CODE).ToList());
                dicSingleTag.Add("DIF_TRANOUT", "Chuyển viện ở khoa cấp cứu: " + DiffTranOut);

                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    objectTag.AddObjectData(store, "Report", listData);
                    return;
                }
                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.V_HIS_TREATMENT_4.OUT_CODE).ToList());
            }
        }
    }
}
