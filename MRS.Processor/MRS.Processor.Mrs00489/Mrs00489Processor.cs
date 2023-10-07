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
using MOS.MANAGER.HisEkipUser; 

namespace MRS.Processor.Mrs00489
{
    public class Mrs00489Processor : AbstractProcessor
    {
        Mrs00489Filter castFilter = null; 
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>();
        private List<Mrs00489RDO> ListRdoDetail = new List<Mrs00489RDO>();
        private List<Mrs00489RDO> ListRdo = new List<Mrs00489RDO>();
        List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_SERE_SERV_PTTT> ListSereServPttt = new List<V_HIS_SERE_SERV_PTTT>(); 
        List<V_HIS_EKIP_USER> ListEkipUser = new List<V_HIS_EKIP_USER>(); 
        List<V_HIS_EKIP_USER> listExecuteRole = new List<V_HIS_EKIP_USER>(); 
        Dictionary<long, string> dicPTTTService = new Dictionary<long, string>(); 
        private const int MAX_COUNT = 12; 
        CommonParam paramGet = new CommonParam(); 
        public Mrs00489Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00489Filter); 
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00489Filter)reportFilter); 

            var result = true; 
            try
            {

                GetTreatment(); 
                var listtreatmentId = listTreatment.Select(o => o.ID).ToList(); 

                GetSereServ(listtreatmentId); 

                var listSereServId = ListSereServ.Select(o => o.ID).ToList(); 
                GetSereServPttt(listSereServId); 
                
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
                var listEkipId = ListSereServ.Select(o => o.EKIP_ID ?? 0).ToList(); 
                GetEkipUser(listEkipId); 
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
                    HisEkipUserViewFilterQuery ekipFilter = new HisEkipUserViewFilterQuery(); 
                    ekipFilter.EKIP_IDs = listIDs; 
                    var ListEkipUserSub = new HisEkipUserManager(paramGet).GetView(ekipFilter); 
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
            List<long> SERVICE_TYPE_IDs = new List<long>(); 
            if (castFilter.CHECK_PT) SERVICE_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT); 
            if (castFilter.CHECK_TT) SERVICE_TYPE_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT); 

            //YC-DV
            if (IsNotNullOrEmpty(listtreatmentId))
            {
                var skip = 0; 
                while (listtreatmentId.Count - skip > 0)
                {
                    var listIDs = listtreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery(); 
                    ssFilter.TREATMENT_IDs = listIDs; 
                    ssFilter.SERVICE_TYPE_IDs = SERVICE_TYPE_IDs; 
                    ssFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID; 
                    ssFilter.REQUEST_DEPARTMENT_ID = castFilter.REQUEST_DEPARTMENT_ID; 
                    ssFilter.EXECUTE_ROOM_ID = castFilter.EXE_ROOM_ID; 
                    var ListSereServSub = new HisSereServManager(paramGet).GetView(ssFilter); 
                    ListSereServ.AddRange(ListSereServSub); 
                }
            }
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
                listExecuteRole = ListEkipUser.GroupBy(o => o.EXECUTE_ROLE_ID).Select(p => p.First()).ToList(); 
                Dictionary<string, PropertyInfo> dicAmountField = new Dictionary<string, PropertyInfo>(); 

                for (var i=0; i<listExecuteRole.Count; i++)
                {
                    if (!dicAmountField.ContainsKey(listExecuteRole[i].EXECUTE_ROLE_NAME)) dicAmountField.Add(listExecuteRole[i].EXECUTE_ROLE_NAME, typeof(Mrs00489RDO).GetProperty(String.Format("COUNT_EXECUTE_ROLE_{0}", i+1))); 
                }
                if (ListSereServ != null && ListSereServ.Count > 0)
                {
                    ListRdoDetail = (from ss in ListSereServ
                              join tm in listTreatment on ss.TDL_TREATMENT_ID equals tm.ID
                               select new Mrs00489RDO(ss, ListEkipUser, ListSereServPttt.FirstOrDefault(o => o.SERE_SERV_ID == ss.ID) ?? new V_HIS_SERE_SERV_PTTT(), tm, dicAmountField, dicPTTTService)).ToList();
                    
                    ListRdoDetail = GroupById(ListRdoDetail);
                    ListRdo.AddRange(ListRdoDetail);
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

        private List<Mrs00489RDO> GroupById(List<Mrs00489RDO> ListRdoDetail)
        {
            var group = ListRdoDetail.GroupBy(o => o.SERE_SERV_ID).ToList(); 
            ListRdoDetail.Clear(); 
            string listUser = ""; 
            Mrs00489RDO rdo; 
            List<Mrs00489RDO> listSub; 
            PropertyInfo[] pi = Properties.Get<Mrs00489RDO>(); 
            foreach (var item in group)
            {
                rdo = new Mrs00489RDO(); 
                listSub = item.ToList<Mrs00489RDO>(); 

                foreach (var field in pi)
                {
                    
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field))); 
                }
                ListRdoDetail.Add(rdo); 
            }
            return ListRdoDetail; 
        }

        private Mrs00489RDO IsMeaningful(List<Mrs00489RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00489RDO(); 
        }

        private List<Mrs00489RDO> GroupByTreatmentServiceAndPrice(List<Mrs00489RDO> ListRdo)
        {
            var group = ListRdo.GroupBy(o => new { o.TREATMENT_ID, o.SERVICE_ID, o.PRICE }).ToList();
            ListRdo.Clear(); 
            string listUser = ""; 
            decimal sum = 0; 
            Mrs00489RDO rdo; 
            List<Mrs00489RDO> listSub; 
            PropertyInfo[] pi = Properties.Get<Mrs00489RDO>(); 
            foreach (var item in group)
            {
                rdo = new Mrs00489RDO(); 
                listSub = item.ToList<Mrs00489RDO>(); 

                foreach (var field in pi)
                {
                    listUser = ""; 
                    sum = 0; 
                    if (field.Name.Contains("COUNT_EXECUTE_ROLE_"))
                    {
                        listUser = string.Join("; ", listSub.Select(s => (string)field.GetValue(s)).Distinct().ToList()); 
                        field.SetValue(rdo, listUser); 
                    }
                    else if (field.Name.Contains("AMOUNT"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s)); 
                        field.SetValue(rdo, sum); 
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field))); 
                    }
                }
                ListRdo.Add(rdo); 
            }
            return ListRdo; 
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00489Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00489Filter)this.reportFilter).TIME_TO)); 
            if (((Mrs00489Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00489Filter)this.reportFilter).EXECUTE_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("EXECUTE_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }
            if (((Mrs00489Filter)this.reportFilter).REQUEST_DEPARTMENT_ID != null)
            {
                var department = new HisDepartmentManager().GetById(((Mrs00489Filter)this.reportFilter).REQUEST_DEPARTMENT_ID ?? 0); 
                dicSingleTag.Add("REQUEST_DEPARTMENT_NAME", department.DEPARTMENT_NAME); 
            }
            if (((Mrs00489Filter)this.reportFilter).EXE_ROOM_ID != null)
            {
                var roomfilter = new HisRoomViewFilterQuery();
                roomfilter.ID = ((Mrs00489Filter)this.reportFilter).EXE_ROOM_ID ?? 0;
                var room = new HisRoomManager().GetView(roomfilter).First(); 
                dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME); 
            }
            for (var i = 0;  i < listExecuteRole.Count;  i++)
            {
                dicSingleTag.Add(String.Format("EXECUTE_ROLE_NAME_{0}", i + 1), listExecuteRole[i].EXECUTE_ROLE_NAME);
            }
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TREATMENT_ID).ToList());
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail.OrderBy(o => o.TREATMENT_ID).ToList()); 
        }
    }
}
