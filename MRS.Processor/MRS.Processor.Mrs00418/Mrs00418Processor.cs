using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;

using MOS.MANAGER.HisServiceRetyCat;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00418
{
    public class Mrs00418Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00418RDO> Mrs00418RDOSereServs = new List<Mrs00418RDO>();
        private List<SeSe> listSereServ = new List<SeSe>();
        //Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();
        //private List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER> lastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        Dictionary<long, List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER>> dicPatientTypeAlter = new Dictionary<long, List<HIS_PATIENT_TYPE_ALTER>>();
        Dictionary<long, MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER> dicLastPatientTypeAlter = new Dictionary<long, MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER>();
        //private List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();

        Mrs00418Filter filter = null;

        public Mrs00418Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00418Filter);
        }

        protected override bool GetData()
        {
            filter = ((Mrs00418Filter)reportFilter);
            int start = 0;
            var result = true;
            CommonParam param = new CommonParam();
            try
            {


                listSereServ = new ManagerSql().GetSS(filter);
                listSereServ = listSereServ.GroupBy(o => o.ID).Select(p => p.First()).ToList();

                listSereServ = listSereServ.Where(o => o.IS_DELETE == 0).ToList();
                if (!string.IsNullOrWhiteSpace(filter.EXCLUDE_SERVICE_CODEs))
                {
                    var excludeServiceCodes = filter.EXCLUDE_SERVICE_CODEs.Split(',').ToList();
                    listSereServ = listSereServ.Where(o => !excludeServiceCodes.Contains(o.TDL_SERVICE_CODE)).ToList();
                }
                if (filter.DEPARTMENT_ID != null)
                {
                    listSereServ = listSereServ.Where(o => o.TDL_EXECUTE_DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    listSereServ = listSereServ.Where(o => filter.DEPARTMENT_IDs.Contains(o.TDL_EXECUTE_DEPARTMENT_ID)).ToList();
                }
                var listTreatmentId = listSereServ.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                // tinh tong so luot kham trong ngay và get patientTypeAlter
                List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
                int count = listTreatmentId.Count();
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listSub = listTreatmentId.Skip(start).Take(limit).ToList();
                    HisPatientTypeAlterFilterQuery patyAlterFilter = new HisPatientTypeAlterFilterQuery();
                    patyAlterFilter.TREATMENT_IDs = listSub;
                    patyAlterFilter.ORDER_DIRECTION = "ASC";
                    patyAlterFilter.ORDER_FIELD = "ID";
                    var listPatientTypeAlterSub = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).Get(patyAlterFilter);
                    patientTypeAlters.AddRange(listPatientTypeAlterSub);
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
                //this.lastPatientTypeAlter = patientTypeAlters.OrderBy(p => p.LOG_TIME).GroupBy(o => o.TREATMENT_ID).Select(p => p.Last()).ToList();
                dicPatientTypeAlter = patientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.ToList());
                dicLastPatientTypeAlter = patientTypeAlters.GroupBy(o => o.TREATMENT_ID).ToDictionary(p => p.Key, q => q.OrderBy(r=>r.LOG_TIME).LastOrDefault());
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
                Mrs00418RDOSereServs.Clear();
                ProcessRdos(this.listSereServ);
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessRdos(List<SeSe> sereServs)
        {
            try
            {
                var sereServGroupByExecuteRoom = sereServs.GroupBy(o => o.TDL_EXECUTE_ROOM_ID).ToList();
                foreach (var group in sereServGroupByExecuteRoom)
                {
                    var subGroup = group.ToList<SeSe>();
                    Mrs00418RDO rdo = new Mrs00418RDO();
                    if (subGroup != null && subGroup.Count > 0)
                    {
                        //số lượng tổng
                        rdo.TreatmentAllAmount = subGroup.Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.ALL_AMOUNT = subGroup.Sum(s => s.AMOUNT);
                        //số lượng nội trú
                        rdo.TreatmentInAmount = subGroup.Where(o => LastTreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.IN_AMOUNT = subGroup.Where(o => TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                        //số lượng ngoại trú
                        rdo.TreatmentOutAmount = subGroup.Where(o => LastTreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.OUT_AMOUNT = subGroup.Where(o => TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Sum(s => s.AMOUNT);
                        //số lượng khám
                        rdo.TreatmentExamAmount = subGroup.Where(o => LastTreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.EXAM_AMOUNT = subGroup.Where(o => TreatmentType(o) == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM).Sum(s => s.AMOUNT);

                        rdo.EXECUTE_DEPARTMENT_ID = subGroup.First().TDL_EXECUTE_DEPARTMENT_ID;

                        rdo.EXECUTE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == subGroup.First().TDL_EXECUTE_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

                        rdo.EXECUTE_ROOM_ID = subGroup.First().TDL_EXECUTE_ROOM_ID;

                        rdo.EXECUTE_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == subGroup.First().TDL_EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                    }

                    if (rdo.EXECUTE_ROOM_ID > 0)
                    {
                        this.Mrs00418RDOSereServs.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private long TreatmentType(SeSe sereServ)
        {
            long result = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
            if (filter.IS_SEPRATE_TREAT == true)
            {
                result = IntructionTreatmentType(sereServ);
            }
            else
            {
                result = LastTreatmentType(sereServ);
            
            }
            return result;
        }

        private long LastTreatmentType(SeSe sereServ)
        {
            if (dicLastPatientTypeAlter.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
            {
                var patientTypeAlter = dicLastPatientTypeAlter[sereServ.TDL_TREATMENT_ID ?? 0];
                if (patientTypeAlter != null)
                {
                    return patientTypeAlter.TREATMENT_TYPE_ID;
                }
            }
            return IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
        }

        private long IntructionTreatmentType(SeSe sereServ)
        {
            if (dicPatientTypeAlter.ContainsKey(sereServ.TDL_TREATMENT_ID ?? 0))
            {
                var patientTypeAlter = dicPatientTypeAlter[sereServ.TDL_TREATMENT_ID ?? 0].OrderBy(o => o.LOG_TIME).LastOrDefault(p => p.LOG_TIME <= sereServ.TDL_INTRUCTION_TIME);
                if (patientTypeAlter != null)
                {
                   return patientTypeAlter.TREATMENT_TYPE_ID;
                }
            }
            return IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00418Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("EXECUTE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00418Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00418Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("EXECUTE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00418Filter)reportFilter).TIME_TO));
            }
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
            dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
            bool exportSuccess = true;
            objectTag.AddObjectData(store, "Report", this.Mrs00418RDOSereServs);
            exportSuccess = exportSuccess && store.SetCommonFunctions();
        }

    }
}
