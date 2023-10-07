using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisEkip;
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
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisEkipUser; 

namespace MRS.Processor.Mrs00499
{
    public class Mrs00499Processor : AbstractProcessor
    {
        Mrs00499Filter castFilter = null; 
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 
        private List<Mrs00499RDO> ListRdo = new List<Mrs00499RDO>(); 
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV_PTTT> ListSereServPttt = new List<V_HIS_SERE_SERV_PTTT>(); 
        List<HIS_EKIP_USER> ListEkipUser = new List<HIS_EKIP_USER>(); 
        List<V_HIS_EKIP_USER> listExecuteRole = new List<V_HIS_EKIP_USER>(); 
        Dictionary<long, string> dicPTTTService = new Dictionary<long, string>(); 
        string CategoryNames = ""; 
        private const int MAX_COUNT = 12; 
        CommonParam paramGet = new CommonParam(); 
        string reportTypeCode = ""; 
        List<long> listServiceId = new List<long>(); 
        public Mrs00499Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            this.reportTypeCode = reportTypeCode; 
        }

        public override Type FilterType()
        {
            return typeof(Mrs00499Filter); 
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00499Filter)reportFilter); 

            var result = true; 
            try
            {

                GetTreatment(); 
                var listtreatmentId = listTreatment.Select(o => o.ID).ToList(); 
                GetSereServ(listtreatmentId); 

                var listSereServId = ListSereServ.Select(o => o.ID).ToList(); 
                GetSereServPttt(listSereServId); 

                var listEkipId = ListSereServ.Select(o => o.EKIP_ID??0).ToList(); 
                GetEkipUser(listEkipId); 


                //Dich vu
                HisServiceView1FilterQuery listPtServiceFilter = new HisServiceView1FilterQuery()
                {
                    SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT
                }; 
                dicPTTTService = new HisServiceManager(paramGet).GetView1(listPtServiceFilter).GroupBy(p => p.ID).ToDictionary(o => o.Key, o => o.First().PTTT_GROUP_NAME); 

                HisServiceView1FilterQuery listTtServiceFilter = new HisServiceView1FilterQuery()
                {
                    SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT
                }; 
                var listTtService = new HisServiceManager(paramGet).GetView1(listTtServiceFilter); 
                foreach (var item in listTtService)
                {
                    if (!dicPTTTService.ContainsKey(item.ID)) dicPTTTService[item.ID] = item.PTTT_GROUP_NAME; 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void GetEkipUser(List<long> listEkipId)
        {
            if (IsNotNullOrEmpty(listEkipId))
            {
                var skip = 0; 
                while (listEkipId.Count - skip > 0)
                {
                    var listIDs = listEkipId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisEkipUserFilterQuery ekipFilter = new HisEkipUserFilterQuery(); 
                    ekipFilter.EKIP_IDs = listIDs; 
                    var ListEkipUserSub = new HisEkipUserManager(paramGet).Get(ekipFilter); 
                    ListEkipUser.AddRange(ListEkipUserSub); 
                }
            }
        }

        private void GetSereServPttt(List<long> listSereServId)
        {
            if (IsNotNullOrEmpty(listSereServId))
            {
                var skip = 0; 
                while (listSereServId.Count - skip > 0)
                {
                    var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServPtttViewFilterQuery SereServPtttFilter = new HisSereServPtttViewFilterQuery(); 
                    SereServPtttFilter.SERE_SERV_IDs = listIDs; 
                    var ListSereServPtttSub = new HisSereServPtttManager(paramGet).GetView(SereServPtttFilter); 
                    ListSereServPttt.AddRange(ListSereServPtttSub); 
                }
            }
        }

        private void GetSereServ(List<long> listtreatmentId)
        {
            GetServiceRetyCat(); 

            //YC-DV
            if (IsNotNullOrEmpty(listServiceId))
            {
                var skip = 0; 
                while (listServiceId.Count - skip > 0)
                {
                    var limit = listServiceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    if (IsNotNullOrEmpty(listtreatmentId))
                    {
                        var skipSub = 0; 
                        while (listtreatmentId.Count - skipSub > 0)
                        {
                            var listIDs = listtreatmentId.Skip(skipSub).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skipSub = skipSub + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                            HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                            ssFilter.TREATMENT_IDs = listIDs; 
                            ssFilter.SERVICE_IDs = limit; 
                            ssFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID; 
                            ssFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID; 
                            ssFilter.EXECUTE_ROOM_ID = castFilter.EXE_ROOM_ID; 
                            ssFilter.PATIENT_TYPE_ID = castFilter.PATIENT_TYPE_ID; 
                            ssFilter.HAS_EXECUTE = true; 
                            var ListSereServSub = new HisSereServManager(paramGet).GetView(ssFilter); 
                            ListSereServ.AddRange(ListSereServSub); 
                        }
                    }
                }
            }
        }

        private void GetServiceRetyCat()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery(); 
            serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.reportTypeCode; 
            serviceRetyCatFilter.REPORT_TYPE_CAT_IDs = castFilter.REPORT_TYPE_CAT_IDs; 
            listServiceId = (new HisServiceRetyCatManager().GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>()).Select(o => o.SERVICE_ID).Distinct().ToList(); 
            CategoryNames = string.Join(", ", (new HisServiceRetyCatManager().GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>()).Select(o => (o.CATEGORY_NAME ?? "").ToUpper()).Distinct().ToList()); 
        }

        private void GetTreatment()
        {
            CommonParam paramGet = new CommonParam(); 
            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
            treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
            treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
            listTreatment = new HisTreatmentManager(paramGet).GetView(treatmentFilter); 
        }

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {

                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    var listSereServHasPttt = ListSereServ.Where(o => o.TDL_SERVICE_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList(); 
                    var listSereServNoPttt = ListSereServ.Where(o => o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT && o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList(); 
                    ListRdo.AddRange((from ss in listSereServHasPttt
                                      join tm in listTreatment on ss.TDL_TREATMENT_ID equals tm.ID
                                      select new Mrs00499RDO(ss, ListSereServPttt.FirstOrDefault(o => o.SERE_SERV_ID == ss.ID) ?? new V_HIS_SERE_SERV_PTTT(), tm, ListEkipUser,dicPTTTService)).ToList() ?? new List<Mrs00499RDO>()); 
                    ListRdo.AddRange((from ss in listSereServNoPttt
                                      join tm in listTreatment on ss.TDL_TREATMENT_ID equals tm.ID
                                      select new Mrs00499RDO(ss, tm,dicPTTTService)).ToList() ?? new List<Mrs00499RDO>()); 
                    ListRdo = GroupById(ListRdo); 
                    ListRdo = GroupByTreatmentServiceAndPrice(ListRdo); 
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }
        private List<Mrs00499RDO> GroupById(List<Mrs00499RDO> ListRdo)
        {
            var group = ListRdo.GroupBy(o => o.SERE_SERV_ID).ToList(); 
            ListRdo.Clear(); 
            string listUser = ""; 
            Mrs00499RDO rdo; 
            List<Mrs00499RDO> listSub; 
            PropertyInfo[] pi = Properties.Get<Mrs00499RDO>(); 
            foreach (var item in group)
            {
                rdo = new Mrs00499RDO(); 
                listSub = item.ToList<Mrs00499RDO>(); 

                foreach (var field in pi)
                {
                    
                        field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault())); 
                }
                ListRdo.Add(rdo); 
            }
            return ListRdo; 
        }

        private List<Mrs00499RDO> GroupByTreatmentServiceAndPrice(List<Mrs00499RDO> ListRdo)
        {
            var group = ListRdo.GroupBy(o =>new {o.TREATMENT_ID,o.SERVICE_ID,o.PRICE}).ToList(); 
            ListRdo.Clear(); 
            string listUser = ""; 
            decimal sum = 0; 
            long count = 0; 
            Mrs00499RDO rdo; 
            List<Mrs00499RDO> listSub; 
            PropertyInfo[] pi = Properties.Get<Mrs00499RDO>(); 
            foreach (var item in group)
            {
                rdo = new Mrs00499RDO(); 
                listSub = item.ToList<Mrs00499RDO>(); 

                foreach (var field in pi)
                {
                    listUser = ""; 
                    sum = 0; 
                    if (field.Name.Contains("AMOUNT"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s)); 
                        field.SetValue(rdo, sum); 
                    }
                    else if (field.Name.Contains("COUNT_USER"))
                    {
                        count = listSub.Sum(s => (long)field.GetValue(s)); 
                        field.SetValue(rdo, count); 
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00499Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00499Filter)this.reportFilter).TIME_TO)); 
            dicSingleTag.Add("SERVICE_CATEGORY_NAMEs", this.CategoryNames); 
            if (((Mrs00499Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00499Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }
            if (((Mrs00499Filter)this.reportFilter).EXE_ROOM_ID != null)
            {
                var room = new HisRoomManager().GetViewByIds(new List<long>{((Mrs00499Filter)this.reportFilter).EXE_ROOM_ID ?? 0}).First(); 
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME); 
            }
            if (((Mrs00499Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00499Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }
            for (var i = 0;  i < listExecuteRole.Count;  i++)
            {
                dicSingleTag.Add(String.Format("EXECUTE_ROLE_NAME_{0}", i + 1), listExecuteRole[i].EXECUTE_ROLE_NAME); 
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o=>o.TREATMENT_ID).ToList()); 
        }
    }
}
