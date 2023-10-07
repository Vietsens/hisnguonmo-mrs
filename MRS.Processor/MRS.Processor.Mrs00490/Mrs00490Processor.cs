using MOS.MANAGER.HisPatientType;
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
 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisSereServPttt; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServBill; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisSereServExt; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisTreatment; 
using System.Reflection; 
using Inventec.Common.Repository; 

namespace MRS.Processor.Mrs00490
{
    public class Mrs00490Processor : AbstractProcessor
    {
        Mrs00490Filter castFilter = null; 
        private List<Mrs00490RDO> ListRdo = new List<Mrs00490RDO>();
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERVICE_REQ> ListServiceReq = new List<V_HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV> ListSereServAddition = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>(); 
        List<HIS_PATIENT> ListPatient = new List<HIS_PATIENT>(); 
        CommonParam paramGet = new CommonParam(); 
        public Mrs00490Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00490Filter); 
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00490Filter)reportFilter); 

            var result = true; 
            try
            {
                
                GetSereServ(); 

                var listPatientId = ListServiceReq.Select(o => o.TDL_PATIENT_ID).Distinct().ToList(); 
                GetPatient(listPatientId);
                var listTreatmentId = ListServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList(); 
                GetSereServ(listTreatmentId); 
                
               
                listTreatmentId = ListSereServAddition.Select(o => o.TDL_TREATMENT_ID??0).Distinct().ToList(); 
                ListPatientTypeAlter = new HisPatientTypeAlterManager().GetViewByTreatmentIds(listTreatmentId) ?? new List<V_HIS_PATIENT_TYPE_ALTER>(); 
      
                ListPatientTypeAlter = ListPatientTypeAlter.Where(p => p.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).GroupBy(o => o.TREATMENT_ID).Select(q => q.FirstOrDefault()).ToList(); 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void GetPatient(List<long> listPatientId)
        {
            if (IsNotNullOrEmpty(listPatientId))
            {
                var skip = 0; 
                while (listPatientId.Count - skip > 0)
                {
                    var limit = listPatientId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisPatientFilterQuery patientFilter = new HisPatientFilterQuery(); 
                    patientFilter.IDs = limit; 

                    var ListPatientSub = new HisPatientManager(paramGet).Get(patientFilter); 
                    ListPatient.AddRange(ListPatientSub); 
                }
            }
        }

        private void GetSereServ(List<long> listTreatmentId)
        {
            if (IsNotNullOrEmpty(listTreatmentId))
            {
                var skip = 0;
                var listSereServAll = new List<V_HIS_SERE_SERV>();
                while (listTreatmentId.Count - skip > 0)
                {
                    var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                    ssFilter.TREATMENT_IDs = limit; 
                    var ListSereServSub = new HisSereServManager(paramGet).GetView(ssFilter);
                    listSereServAll.AddRange(ListSereServSub); 
                }
                var srIds = ListServiceReq.Select(o => o.ID).ToList();
                ListSereServ = listSereServAll.Where(o => srIds.Contains(o.SERVICE_REQ_ID??0)).ToList();
                var ssPrIds = ListSereServ.Select(o => o.ID).Distinct().ToList();
                ListSereServAddition = listSereServAll.Where(o => ssPrIds.Contains(o.PARENT_ID??0)&&o.IS_EXPEND==1 &&(o.TDL_SERVICE_TYPE_ID==6||o.TDL_SERVICE_TYPE_ID==7)).ToList(); 
            }
        }

        private void GetSereServ()
        {
            //YC-DV
            List<long> SERVICE_REQ_TYPE_IDs = new List<long>(); 
            if (castFilter.CHECK_PT) SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT);
            if (castFilter.CHECK_TT) SERVICE_REQ_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT);

            HisServiceReqViewFilterQuery sqFilter = new HisServiceReqViewFilterQuery();
            sqFilter.INTRUCTION_TIME_FROM = castFilter.INTRUCTION_TIME_FROM;
            sqFilter.INTRUCTION_TIME_TO = castFilter.INTRUCTION_TIME_TO;
            sqFilter.FINISH_TIME_FROM = castFilter.FINISH_TIME_FROM;
            sqFilter.FINISH_TIME_TO = castFilter.FINISH_TIME_TO; 
            sqFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID; 
            sqFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID; 
            sqFilter.EXECUTE_DEPARTMENT_IDs = castFilter.EXECUTE_DEPARTMENT_IDs; 
            sqFilter.REQUEST_DEPARTMENT_IDs = castFilter.REQUEST_DEPARTMENT_IDs; 
            sqFilter.SERVICE_REQ_TYPE_IDs = SERVICE_REQ_TYPE_IDs; 
            ListServiceReq = new HisServiceReqManager(paramGet).GetView(sqFilter) ?? new List<V_HIS_SERVICE_REQ>(); 
        }


        protected override bool ProcessData()
        {
            var result = true; 
            try
            {

                if (ListSereServAddition != null && ListSereServAddition.Count > 0)
                {
                    ListRdo = (from sereServAddition in ListSereServAddition
                               join patient in ListPatient on sereServAddition.TDL_PATIENT_ID equals patient.ID 
                               join patientTypeAlter in ListPatientTypeAlter on sereServAddition.TDL_TREATMENT_ID equals patientTypeAlter.TREATMENT_ID
                               into patientTypeAlterFull
                               from PatientTypeAlterOfsereServ in patientTypeAlterFull.DefaultIfEmpty()
                               select new Mrs00490RDO(sereServAddition, PatientTypeAlterOfsereServ, patient)).ToList(); 
                    ListRdo = GroupByTreatmentId(ListRdo); 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }
        private List<Mrs00490RDO> GroupByTreatmentId(List<Mrs00490RDO> ListRdo)
        {
            string keyGroup = "{0}";

            //khi có điều kiện lọc từ template thì đổi sang key từ template
            if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXPEND") && this.dicDataFilter["KEY_GROUP_EXPEND"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXPEND"].ToString()))
            {
                keyGroup = this.dicDataFilter["KEY_GROUP_EXPEND"].ToString();
            }
            var group = ListRdo.GroupBy(o => string.Format(keyGroup,o.TDL_TREATMENT_ID,o.TDL_REQUEST_DEPARTMENT_ID)).ToList(); 
            ListRdo.Clear(); 
            Decimal sum = 0; 
            Mrs00490RDO rdo; 
            List<Mrs00490RDO> listSub; 
            PropertyInfo[] pi = Properties.Get<Mrs00490RDO>(); 
            foreach (var item in group)
            {
                rdo = new Mrs00490RDO(); 
                listSub = item.ToList<Mrs00490RDO>(); 

                foreach (var field in pi)
                {
                    if (field.Name.Contains("_EXPEND_AMOUNT"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s)); 
                        field.SetValue(rdo, sum); 
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault())); 
                    }
                }
                ListRdo.Add(rdo); 
            }
            return ListRdo; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            LogSystem.Info("7"); 
            dicSingleTag.Add("FINISH_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00490Filter)this.reportFilter).FINISH_TIME_FROM??((Mrs00490Filter)this.reportFilter).INTRUCTION_TIME_FROM??0));
            dicSingleTag.Add("FINISH_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00490Filter)this.reportFilter).FINISH_TIME_TO ?? ((Mrs00490Filter)this.reportFilter).INTRUCTION_TIME_TO ?? 0)); 
            if (((Mrs00490Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00490Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }
            if (((Mrs00490Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00490Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }

            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TDL_TREATMENT_ID).ToList()); 
        }
    }
}
