using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00729
{
    class Mrs00729Processor : AbstractProcessor
    {
        Mrs00729Filter castFilter = null;
        List<Mrs00729RDO.Sheet1> ListRdoSheet1 = new List<Mrs00729RDO.Sheet1>();
        List<Mrs00729RDO.Sheet2> ListRdoSheet2 = new List<Mrs00729RDO.Sheet2>();
        List<Mrs00729RDO.Sheet2> ListRdoOld = new List<Mrs00729RDO.Sheet2>();
        List<Mrs00729RDO.Sheet3> ListRdoSheet3 = new List<Mrs00729RDO.Sheet3>();
        List<Mrs00729RDO.Sheet1> ListRdo1 = new List<Mrs00729RDO.Sheet1>();
        List<Mrs00729RDO.Sheet2> ListRdo2 = new List<Mrs00729RDO.Sheet2>();
        List<Mrs00729RDO.Sheet3> ListRdo3 = new List<Mrs00729RDO.Sheet3>();
        CommonParam paramGet = new CommonParam();
        
        
        public Mrs00729Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00729Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00729Filter)reportFilter;
            try
            {
                ListRdoSheet1 = new ManagerSql().GetDataNgoaiTru(castFilter) ?? new List<Mrs00729RDO.Sheet1>();
                ListRdoSheet2 = new ManagerSql().GetDataNoiTru(castFilter) ?? new List<Mrs00729RDO.Sheet2>();
                ListRdoSheet3 = new ManagerSql().GetDataDepartment(castFilter) ?? new List<Mrs00729RDO.Sheet3>();
                //ListRdoOld = new ManagerSql().GetDataOldNoiTru(castFilter) ?? new List<Mrs00729RDO.Sheet2>();
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
                if (ListRdoSheet1 != null) 
                {
                    ProcessSheet1(ListRdoSheet1);
                }

                if (ListRdoSheet2 != null)
                {
                    ProcessSheet2(ListRdoSheet2);
                }

                if (ListRdoSheet3 != null)
                {
                    ProcessSheet3(ListRdoSheet3);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        private void ProcessSheet1(List<Mrs00729RDO.Sheet1> listSheet1)
        {
            try
            {
                var group = listSheet1.GroupBy(p => p.ROOM_CODE).ToList();
                foreach (var item in group)
                {
                    List<Mrs00729RDO.Sheet1> listSub = item.ToList<Mrs00729RDO.Sheet1>();
                    Mrs00729RDO.Sheet1 rdo = new Mrs00729RDO.Sheet1();
                    rdo.COUNT_APPOINTMENT = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__HEN).Distinct().Count();
                    rdo.COUNT_BHYT = listSub.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Distinct().Count();
                    rdo.COUNT_DEATH = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET).Distinct().Count();
                    rdo.COUNT_IN = listSub.Where(p => p.EXAM_END_TYPE != 2).Distinct().Count();
                    rdo.COUNT_NGOAITRU = listSub.Where(p => p.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Distinct().Count();
                    rdo.COUNT_NOT_BHYT = listSub.Where(p => p.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Distinct().Count();
                    rdo.COUNT_PT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Distinct().Count();
                    rdo.COUNT_TRANSFER_EXAM_ROOM = listSub.Where(p => p.PREVIOUS_SERVICE_REQ_ID != null).Distinct().Count();
                    rdo.COUNT_TRANSFER_ROUTE = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Distinct().Count();
                    rdo.COUNT_TT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Distinct().Count();
                    rdo.ROOM_CODE = listSub[0].ROOM_CODE;
                    rdo.ROOM_NAME = listSub[0].ROOM_NAME;
                    rdo.COUNT_CHILD_LESS_THAN_6 = listSub.Where(p => RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) < 6 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 0).Distinct().Count();
                    rdo.COUNT_CHILD_MORE_THAN_6_LESS_THAN_18 = listSub.Where(p => RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 6 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) < 18).Distinct().Count();
                    rdo.COUNT_MORE_THAN_18_LESS_THAN_60 = listSub.Where(p => RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) < 60 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 18).Distinct().Count();
                    rdo.COUNT_MORE_THAN_60 = listSub.Where(p => RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 60).Distinct().Count();
                    ListRdo1.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessSheet2(List<Mrs00729RDO.Sheet2> listSheet2)
        {
            try
            {
                
                var group = listSheet2.GroupBy(p => p.DEPARTMENT_CODE).ToList();
                foreach (var item in group)
                {
                    List<Mrs00729RDO.Sheet2> listSub = item.ToList<Mrs00729RDO.Sheet2>();
                    
                    Mrs00729RDO.Sheet2 rdo = new Mrs00729RDO.Sheet2();
                    
                    rdo.DEPARTMENT_CODE = listSub[0].DEPARTMENT_CODE;
                    rdo.DEPARTMENT_NAME = listSub[0].DEPARTMENT_NAME;
                    rdo.COUNT_OLD = listSub.Where(p => p.OUT_TIME == null && p.IN_TIME <= castFilter.TIME_FROM).Distinct().Count();
                    rdo.COUNT_NOW = listSub.Where(p => p.OUT_TIME == null && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_IN_CLINICAL = listSub.Where(p => p.CLINICAL_IN_TIME >= castFilter.TIME_FROM && p.CLINICAL_IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_OUT = listSub.Where(p => p.OUT_TIME != null && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_DEATH = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_XINVE = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_IN = listSub.Where(p => p.EXAM_END_TYPE != 2).Distinct().Count();
                    rdo.COUNT_BHYT = listSub.Where(p => p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_NOT_BHYT = listSub.Where(p => p.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_ALL = listSub.Where(p => p.IN_TIME >= castFilter.TIME_FROM && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_TRANSFER_DEPARTMENT = listSub.Where(p => p.PREVIOUS_SERVICE_REQ_ID != null && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.COUNT_TRANSFER_ROUTE = listSub.Where(p => p.EXAM_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Distinct().Count();
                    rdo.TREATMENT_DAY_COUNT_ALL = listSub.Where(p => p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Sum(p => p.TREATMENT_DAY_COUNT);
                    rdo.COUNT_BED_TREATMENT = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G && p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO).Count();
                    
                    
                    rdo.COUNT_CHILD_LESS_THAN_6 = listSub.Where(p => p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) <= 6 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 0).Distinct().Count();
                    rdo.COUNT_CHILD_MORE_THAN_6_LESS_THAN_18 = listSub.Where(p => p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 6 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) <= 18).Distinct().Count();
                    rdo.COUNT_MORE_THAN_18_LESS_THAN_60 = listSub.Where(p => p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) <= 60 && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 18).Distinct().Count();
                    rdo.COUNT_MORE_THAN_60 = listSub.Where(p => p.IN_TIME >= castFilter.TIME_TO && p.IN_TIME <= castFilter.TIME_TO && RDOCommon.CalculateAge(p.TDL_PATIENT_DOB ?? 0) > 60).Distinct().Count();
                    ListRdo2.Add(rdo);
                }

                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessSheet3(List<Mrs00729RDO.Sheet3> listSheet3)
        {
            try
            {
                var group = listSheet3.GroupBy(p => p.EXECUTE_DEPARTMENT_ID).ToList();
                foreach (var item in group)
                {
                    List<Mrs00729RDO.Sheet3> listSub = item.ToList<Mrs00729RDO.Sheet3>();
                    Mrs00729RDO.Sheet3 rdo = new Mrs00729RDO.Sheet3();
                    rdo.EXECUTE_DEPARTMENT_ID = listSub[0].REQUEST_DEPARTMENT_ID;
                    rdo.EXECUTE_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub[0].EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    rdo.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub[0].EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub[0].REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                    rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listSub[0].REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                    rdo.COUNT_PT_1 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && p.PTTT_GROUP_ID == 1).Count();
                    rdo.COUNT_PT_2 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && p.PTTT_GROUP_ID == 2).Count();
                    rdo.COUNT_PT_3 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && p.PTTT_GROUP_ID == 3).Count();
                    rdo.COUNT_PT_DB = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && p.PTTT_GROUP_ID == 4).Count();
                    rdo.COUNT_TT_1 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && p.PTTT_GROUP_ID == 1).Count();
                    rdo.COUNT_TT_2 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && p.PTTT_GROUP_ID == 2).Count();
                    rdo.COUNT_TT_3 = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && p.PTTT_GROUP_ID == 3).Count();
                    rdo.COUNT_TT_DB = listSub.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT && p.PTTT_GROUP_ID == 4).Count();
                    ListRdo3.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00729Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00729Filter)this.reportFilter).TIME_TO ?? 0));

            

            objectTag.AddObjectData(store, "ReportSheet1", ListRdo1.OrderBy(p => p.ROOM_CODE).ToList());
            objectTag.AddObjectData(store, "ReportSheet2", ListRdo2.OrderBy(p => p.DEPARTMENT_CODE).ToList());
            objectTag.AddObjectData(store, "ReportSheet3", ListRdo3.OrderBy(p => p.EXECUTE_DEPARTMENT_CODE).ToList());
            objectTag.AddObjectData(store, "ExecuteDepartment", ListRdo3.GroupBy(p => p.EXECUTE_DEPARTMENT_CODE).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ExecuteDepartment", "ReportSheet3", "EXECUTE_DEPARTMENT_CODE", "EXECUTE_DEPARTMENT_CODE");
        }

        
    }
}
