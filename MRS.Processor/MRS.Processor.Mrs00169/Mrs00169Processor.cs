using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisEmergencyWtime;
using MOS.MANAGER.HisPatientTypeAlter;
using FlexCel.Report;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMediOrg;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisTranPatiForm;
using MOS.MANAGER.HisTranPatiReason;
using MOS.MANAGER.HisTreatmentResult;
using MOS.MANAGER.HisTreatmentEndType;

namespace MRS.Processor.Mrs00169
{
    internal class Mrs00169Processor : AbstractProcessor
    {
        List<VSarReportMrs00169RDO> _listSarReportMrs00169Rdos = new List<VSarReportMrs00169RDO>();
        Mrs00169Filter CastFilter;
        System.Data.DataTable listData = new System.Data.DataTable();
        List<V_HIS_ROOM> ListVRoom = new List<V_HIS_ROOM>();
        List<HIS_TRAN_PATI_FORM> listTranPatiForm = new List<HIS_TRAN_PATI_FORM>();
        List<HIS_TRAN_PATI_REASON> listTranPatiReason = new List<HIS_TRAN_PATI_REASON>();
        List<HIS_TRAN_PATI_TECH> listTranPatiTech = new List<HIS_TRAN_PATI_TECH>();
        List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_TREATMENT_RESULT> listTreatmentResult = new List<HIS_TREATMENT_RESULT>();
        List<HIS_TREATMENT_END_TYPE> listTreatmentEndType = new List<HIS_TREATMENT_END_TYPE>();


        string thisReportTypeCode = "";
        public Mrs00169Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00169Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00169Filter)this.reportFilter;
                listData = new ManagerSql().GetSum(CastFilter, MRS.MANAGER.Core.MrsReport.Lib.GetCellValue.Get(this.reportTemplate.REPORT_TEMPLATE_URL, 1, 15));
                if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
                {
                    return true;
                }
                ListVRoom = new HisRoomManager(param).GetView(new HisRoomViewFilterQuery());
                var ListTreatment = GetTreatment();
                if (ListTreatment != null)
                {
                    ListTreatment = ListTreatment.Where(o => o.TRANSFER_IN_MEDI_ORG_CODE != null).ToList();
                }

                listTreatmentEndType = new HisTreatmentEndTypeManager().Get(new HisTreatmentEndTypeFilterQuery());
                listTreatmentResult = new HisTreatmentResultManager().Get(new HisTreatmentResultFilterQuery());
                listTranPatiForm = new HisTranPatiFormManager().Get(new HisTranPatiFormFilterQuery());
                listTranPatiReason = new HisTranPatiReasonManager().Get(new HisTranPatiReasonFilterQuery());
                listTranPatiTech = ListTranPatiTech();
                var listTreaIds = ListTreatment.Select(x => x.ID).Distinct().ToList();
                var skip = 0;
                while (listTreaIds.Count-skip>0)
                {
                    var limitIds = listTreaIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisPatientTypeAlterFilterQuery patientAlterFilter = new HisPatientTypeAlterFilterQuery();
                    patientAlterFilter.TREATMENT_IDs = limitIds;
                    var patientAlters = new HisPatientTypeAlterManager().Get(patientAlterFilter);
                    listPatientTypeAlter.AddRange(patientAlters);
                }
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    List<long> listTreatmentId = ListTreatment.Select(o => o.ID).ToList();
                    List<HIS_EMERGENCY_WTIME> listEmergencyWtime = new List<HIS_EMERGENCY_WTIME>();
                    listEmergencyWtime = GetEmergencyWtime();
                    ProcessFilterData(ListTreatment, listEmergencyWtime);
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
        private List<HIS_TRAN_PATI_TECH> ListTranPatiTech() 
        {
            List<HIS_TRAN_PATI_TECH> result = new List<HIS_TRAN_PATI_TECH>();
            string query = "";
            try
            {
                query += "SELECT * FROM HIS_TRAN_PATI_TECH";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRAN_PATI_TECH>(query);
            }
            catch (Exception ex)
            {
                result = null;
                LogSystem.Error(ex);
            }
            return result;
        }
        private List<HIS_TREATMENT> GetTreatment()
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                CommonParam paramGet = new CommonParam();
                HisTreatmentFilterQuery Filter = new HisTreatmentFilterQuery();
                //Filter.IN_TIME_FROM = CastFilter.DATE_FROM;
                //Filter.IN_TIME_TO = CastFilter.DATE_TO;
                if (CastFilter.CHECK_INTIME_OUTTIME == true)
                {
                    Filter.OUT_TIME_FROM = CastFilter.DATE_FROM;
                    Filter.OUT_TIME_TO = CastFilter.DATE_TO;
                }
                else
                {
                    Filter.IN_TIME_FROM = CastFilter.DATE_FROM;
                    Filter.IN_TIME_TO = CastFilter.DATE_TO;
                }
               
         


                Filter.END_ROOM_IDs = CastFilter.ROOM_IDs;
               // Filter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                result = new HisTreatmentManager().Get(Filter);
                if (this.CastFilter.MEDI_ORG_ID != null)
                {
                    var mediOrg = new HisMediOrgManager().Get(new HisMediOrgFilterQuery() { ID = this.CastFilter.MEDI_ORG_ID });
                    if (mediOrg != null && mediOrg.Count > 0)
                        result = result.Where(o => o.TRANSFER_IN_MEDI_ORG_CODE == mediOrg.First().MEDI_ORG_CODE).ToList();
                }
                if (this.CastFilter.MEDI_ORG_IDs != null)
                {
                    var mediOrg = new HisMediOrgManager().Get(new HisMediOrgFilterQuery() { IDs = this.CastFilter.MEDI_ORG_IDs });

                    if (mediOrg != null && mediOrg.Count > 0)
                    {
                        var mediOrgCodes = mediOrg.Select(o => o.MEDI_ORG_CODE??"").Distinct().ToList();
                        result = result.Where(o => mediOrgCodes.Contains(o.TRANSFER_IN_MEDI_ORG_CODE??"")).ToList();
                    }
                }
                if (this.CastFilter.DEPARTMENT_ID != null)
                {
                    result = result.Where(o => !string.IsNullOrWhiteSpace(o.DEPARTMENT_IDS) && (o.DEPARTMENT_IDS + ",").StartsWith(this.CastFilter.DEPARTMENT_ID.ToString() + ",")).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<HIS_TREATMENT>();
            }
            return result;
        }


        private List<HIS_EMERGENCY_WTIME> GetEmergencyWtime()
        {
            List<HIS_EMERGENCY_WTIME> result = new List<HIS_EMERGENCY_WTIME>();
            try
            {
                CommonParam paramGet = new CommonParam();
                HisEmergencyWtimeFilterQuery filter = new HisEmergencyWtimeFilterQuery();
                result = new MOS.MANAGER.HisEmergencyWtime.HisEmergencyWtimeManager(param).Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new List<HIS_EMERGENCY_WTIME>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));

            if (listData != null && listData.Rows != null && listData.Rows.Count > 0)
            {
                objectTag.AddObjectData(store, "Report", listData);
                return;
            }
            _listSarReportMrs00169Rdos = _listSarReportMrs00169Rdos.OrderBy(o => o.MEDI_ORG_NAME).ToList();
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00169Rdos);
            objectTag.AddObjectData(store, "TopTenIcd", _listSarReportMrs00169Rdos.GroupBy(o => o.PREFIX_TF_ICD_CODE).Select(p => new VSarReportMrs00169RDO() { PREFIX_TF_ICD_CODE = p.First().PREFIX_TF_ICD_CODE, ICD_COUNT = p.Count() }).OrderByDescending(q => q.ICD_COUNT).ToList());
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
        }

        private void ProcessFilterData(List<HIS_TREATMENT> ListTreatment, List<HIS_EMERGENCY_WTIME> ListEmergencyWtime)
        {
            try
            {
                Dictionary<long, HIS_EMERGENCY_WTIME> dicEmergencyWtime = CreateDicEmergencyWtime(ListEmergencyWtime);
                foreach (var Treatment in ListTreatment)
                {
                    var rdo = new VSarReportMrs00169RDO();
                    rdo.HIS_TREATMENT = Treatment;
                    rdo.TRANSFER_IN_CODE = Treatment.TRANSFER_IN_CODE;
                    rdo.PATIENT_CODE = Treatment.TDL_PATIENT_CODE;
                    rdo.TREATMENT_CODE = Treatment.TREATMENT_CODE;
                    rdo.PATIENT_NAME = Treatment.TDL_PATIENT_NAME;
                    rdo.DOB_YEAR = Treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                    rdo.ADDRESS = Treatment.TDL_PATIENT_ADDRESS;
                    rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Treatment.IN_TIME);
                    rdo.PATIENT_TYPE_NAME = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => Treatment.TDL_PATIENT_TYPE_ID == o.ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                  
                    rdo.MEDI_ORG_NAME = Treatment.TRANSFER_IN_MEDI_ORG_NAME;
                    rdo.MEDI_ORG_CODE = Treatment.TRANSFER_IN_MEDI_ORG_CODE;
                    rdo.TRANSFER_IN_ICD_NAME = Treatment.TRANSFER_IN_ICD_NAME;
                    rdo.TRANSFER_IN_ICD_CODE = Treatment.TRANSFER_IN_ICD_CODE;
                    rdo.ICD_NAME = Treatment.IN_ICD_NAME;
                    rdo.ICD_CODE = Treatment.IN_ICD_CODE;
                    rdo.OUT_ICD_CODE = Treatment.ICD_CODE;
                    rdo.OUT_ICD_NAME = Treatment.ICD_NAME;
                    rdo.PREFIX_TF_ICD_CODE = !string.IsNullOrWhiteSpace(Treatment.TRANSFER_IN_ICD_CODE) && Treatment.TRANSFER_IN_ICD_CODE.Length >= 3 ? Treatment.TRANSFER_IN_ICD_CODE.Substring(0, 3) : "__";
                    rdo.EMERGENCY = (Treatment.EMERGENCY_WTIME_ID != null && dicEmergencyWtime.ContainsKey(Treatment.EMERGENCY_WTIME_ID ?? 0)) ? dicEmergencyWtime[Treatment.EMERGENCY_WTIME_ID ?? 0].EMERGENCY_WTIME_NAME : "";
                    rdo.TREATMENT_METHOD = Treatment.TREATMENT_METHOD;

                    rdo.OUT_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Treatment.OUT_TIME ?? 0);

                    rdo.END_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => Treatment.END_DEPARTMENT_ID == o.ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                   // rdo.TREATMENT_END_TYPE_NAME = (HisTreatmentEndTypeCFG.TREATMENTENDTYPEs.FirstOrDefault(o => Treatment.TREATMENT_END_TYPE_ID == o.id) ?? new HIS_TREATMENT_END_TYPE()).TREATMENT_END_TYPE_NAME;
                    int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(Treatment.TDL_PATIENT_DOB, Treatment.IN_TIME);
                    if (tuoi.HasValue)
                    {
                        if (Treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.AGE_MALE = (tuoi >= 1) ? tuoi : 1;
                        }
                        else
                        {
                            rdo.AGE_FEMALE = (tuoi >= 1) ? tuoi : 1;
                        }
                    }
                    rdo.TDL_HEIN_CARD_NUMBER = Treatment.TDL_HEIN_CARD_NUMBER;

                    rdo.FORM_1a = Treatment.TRANSFER_IN_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT ? "X" : "";
                    rdo.FORM_1b = Treatment.TRANSFER_IN_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT ? "X" : "";
                    rdo.FORM_2 = Treatment.TRANSFER_IN_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL ? "X" : "";
                    rdo.FORM_3 = Treatment.TRANSFER_IN_FORM_ID == HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN ? "X" : "";
                    rdo.REASON_4 = Treatment.TRANSFER_IN_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___01 ? "X" : "";
                    rdo.REASON_5 = Treatment.TRANSFER_IN_REASON_ID == HisTranPatiReasonCFG.HIS_TRAN_PATI_REASON___02 ? "X" : "";
                    rdo.RESULT_6 = Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__DO || Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KHOI ? "X" : "";
                    rdo.RESULT_7 = Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__KTD || Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__NANG ? "X" : "";
                    rdo.RESULT_8 = Treatment.TREATMENT_RESULT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_RESULT.ID__CHET ? "X" : "";
                    rdo.RESULT_9 = "";
                    var room = ListVRoom.Where(x => x.ID == Treatment.END_ROOM_ID).FirstOrDefault();
                    if (room != null)
                    {
                        rdo.ROOM_NAME = room.ROOM_NAME;
                    }
                    var tranPatiReason = listTranPatiReason.Where(x => x.ID == Treatment.TRAN_PATI_REASON_ID).FirstOrDefault();
                    if (tranPatiReason != null)
                    {
                        rdo.TRAN_PATI_REASON_NAME = tranPatiReason.TRAN_PATI_REASON_NAME;
                    }
                    var treatmentEndType = listTreatmentEndType.Where(x => x.ID == Treatment.TREATMENT_END_TYPE_ID).FirstOrDefault();
                    if (treatmentEndType != null)
                    {
                        rdo.TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                    }
                    var treatmentResult = listTreatmentResult.Where(x => x.ID == Treatment.TREATMENT_RESULT_ID).FirstOrDefault();
                    if (treatmentResult != null)
                    {
                        rdo.TREATMENT_RESULT_NAME = treatmentResult.TREATMENT_RESULT_NAME;
                    }

                    var tranPatiForm = listTranPatiForm.Where(x => x.ID == Treatment.TRAN_PATI_FORM_ID).FirstOrDefault();
                    if (tranPatiForm != null)
                    {
                        rdo.TRAN_PATI_FORM_NAME = tranPatiForm.TRAN_PATI_FORM_NAME;
                    }
                    var tranPatiTech = listTranPatiTech.Where(x => x.ID == Treatment.TRAN_PATI_TECH_ID).FirstOrDefault();
                    if (tranPatiTech != null)
                    {
                        rdo.TRAN_PATI_TECH_NAME = tranPatiTech.TRAN_PATI_TECH_NAME;
                    }
                    var patientTypeAlter = listPatientTypeAlter.Where(x => x.TREATMENT_ID == Treatment.ID).FirstOrDefault();
                    if (patientTypeAlter != null)
                    {
                        if (patientTypeAlter.RIGHT_ROUTE_CODE == "DT")
                        {
                            rdo.RIGHT_ROUTE_NAME = "Đúng tuyến";
                        }
                        else
                        {
                            rdo.RIGHT_ROUTE_NAME = "Trái tuyến";
                        }
                        if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == "CC")
                        {
                            rdo.RIGHT_ROUTE_TYPE_NAME = "Đúng tuyến (Cấp cứu)";
                        }
                        if (patientTypeAlter.RIGHT_ROUTE_TYPE_CODE == "GT")
                        {
                            rdo.RIGHT_ROUTE_TYPE_NAME = "Đúng tuyến (Có giới thiệu)";
                        }
                    }
                    rdo.EXIT_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => Treatment.LAST_DEPARTMENT_ID == o.ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    _listSarReportMrs00169Rdos.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private Dictionary<long, HIS_EMERGENCY_WTIME> CreateDicEmergencyWtime(List<HIS_EMERGENCY_WTIME> ListEmergencyWtime)
        {
            Dictionary<long, HIS_EMERGENCY_WTIME> result = new Dictionary<long, HIS_EMERGENCY_WTIME>();
            try
            {
                foreach (var item in ListEmergencyWtime)
                {
                    if (!result.ContainsKey(item.ID)) result[item.ID] = item;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return new Dictionary<long, HIS_EMERGENCY_WTIME>();
            }
            return result;
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        //long MediStockId; 
        string SameType;
        public override object Evaluate(object[] parameters)
        {
            //if (parameters == null || parameters.Length <= 0)
            //    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 

            bool result = false;
            try
            {
                //long mediId = Convert.ToInt64(parameters[0]); 
                string MediOrgName = System.Convert.ToString(parameters[0]);

                if (MediOrgName != null)
                {
                    if (SameType == MediOrgName)
                    {
                        return true;
                    }
                    else
                    {
                        SameType = MediOrgName;
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }
    }
}
