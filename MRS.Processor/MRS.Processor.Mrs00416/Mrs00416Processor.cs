using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisRemuneration;
using MOS.MANAGER.HisExecuteRole;
using MOS.MANAGER.HisEkipUser;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisTreatment; 

namespace MRS.Processor.Mrs00416
{
    class Mrs00416Processor : AbstractProcessor
    {
        Mrs00416Filter castFilter = null; 
        List<Mrs00416RDO> listRdo = new List<Mrs00416RDO>(); 
        List<ROLE_SERESERV_PTTT> listExecuteRoleRDO = new List<ROLE_SERESERV_PTTT>(); 

        public Mrs00416Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<HIS_EXECUTE_ROLE> listExecuteRoles = new List<HIS_EXECUTE_ROLE>(); 

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<V_HIS_EKIP_USER> listEkipUsers = new List<V_HIS_EKIP_USER>(); 
        List<V_HIS_REMUNERATION> listRemunerations = new List<V_HIS_REMUNERATION>(); 
        List<V_HIS_SERE_SERV_PTTT> listSereServPTTTs = new List<V_HIS_SERE_SERV_PTTT>(); 
        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>(); 


        public override Type FilterType()
        {
            return typeof(Mrs00416Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00416Filter)this.reportFilter; 

                var skip = 0; 

                //Lay danh sach execute_role
                HisExecuteRoleFilterQuery executeRoleFilter = new HisExecuteRoleFilterQuery(); 
                listExecuteRoles = new MOS.MANAGER.HisExecuteRole.HisExecuteRoleManager(paramGet).Get(executeRoleFilter); 

                // Lay dich vu 
                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery(); 
                sereServFilter.EXECUTE_TIME_FROM = this.castFilter.TIME_FROM; 
                sereServFilter.EXECUTE_TIME_TO = this.castFilter.TIME_TO; 
                if (IsNotNullOrEmpty(this.castFilter.REQUEST_ROOM_IDs))
                {
                    sereServFilter.REQUEST_ROOM_IDs = this.castFilter.REQUEST_ROOM_IDs; 
                }
                if (IsNotNullOrEmpty(this.castFilter.EXCUTE_ROOM_IDs))
                {
                    sereServFilter.EXECUTE_ROOM_IDs = this.castFilter.EXCUTE_ROOM_IDs; 
                }
                
                if (IsNotNullOrEmpty(this.castFilter.PATIENT_TYPE_IDs))
                {
                    sereServFilter.PATIENT_TYPE_IDs = this.castFilter.PATIENT_TYPE_IDs; 
                }

                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter); 
               
                var listSereServIds = listSereServs.Select(s => s.ID).ToList(); 
                //lay dich vu PTTT
                while (listSereServIds.Count - skip > 0)
                {
                    var listSereServId = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServPtttViewFilterQuery sereServPTTTFilter = new HisSereServPtttViewFilterQuery(); 
                    sereServPTTTFilter.SERE_SERV_IDs = listSereServId; 
                    if (IsNotNullOrEmpty(this.castFilter.PTTT_GROUP_IDs))
                    {
                        sereServPTTTFilter.PTTT_GROUP_IDs = this.castFilter.PTTT_GROUP_IDs; 
                    }
                    var listSereServPTTT = new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(paramGet).GetView(sereServPTTTFilter); 
                    listSereServPTTTs.AddRange(listSereServPTTT); 
                }
                var listSSIdPTTTs = listSereServPTTTs.Select(s => s.SERE_SERV_ID).ToList(); 
                listSereServs = listSereServs.Where(w => listSSIdPTTTs.Contains(w.ID)).ToList(); 

                //lay Ekip
                List<long> listEkipIds = listSereServs.Where(w => w.EKIP_ID != null).Select(s => s.EKIP_ID.Value).Distinct().ToList(); 

                skip = 0; 
                while (listEkipIds.Count - skip > 0)
                {
                    var listEkipId = listEkipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisEkipUserViewFilterQuery ekipUserFilter = new HisEkipUserViewFilterQuery(); 
                    ekipUserFilter.EKIP_IDs = listEkipId; 
                    var listEkipUser = new MOS.MANAGER.HisEkipUser.HisEkipUserManager(paramGet).GetView(ekipUserFilter); 
                    listEkipUsers.AddRange(listEkipUser); 

                }

                //lay renumeration(tien phu cap)
                HisRemunerationViewFilterQuery remunerationFilter = new HisRemunerationViewFilterQuery(); 
                listRemunerations = new MOS.MANAGER.HisRemuneration.HisRemunerationManager(paramGet).GetView(remunerationFilter); 
                var listTreatmentId = listSereServs.Select(o => o.TDL_TREATMENT_ID.Value).ToList(); 
                List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 
                skip = 0; 
                while (listTreatmentId.Count - skip > 0)
                {
                    var listIds = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisTreatmentViewFilterQuery TreatmentViewFilter = new HisTreatmentViewFilterQuery(); 
                    TreatmentViewFilter.IDs = listIds; 
                    listTreatment.AddRange(new HisTreatmentManager(param).GetView(TreatmentViewFilter)); 
                }

                dicTreatment = listTreatment.ToDictionary(o => o.ID); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                //lay danh sach vai tro
                if (IsNotNullOrEmpty(listExecuteRoles))
                {
                   
                    listExecuteRoles = listExecuteRoles.OrderBy(s => s.EXECUTE_ROLE_NAME).ToList(); 
                    int i = 1; 
                    foreach (var executeRole in listExecuteRoles)
                    {
                        var role = new ROLE_SERESERV_PTTT(); 
                        role.EXCUTE_ROLE_ID = executeRole.ID; 
                        role.EXCUTE_ROLE_NAME = executeRole.EXECUTE_ROLE_NAME; 
                        role.EXCUTE_ROLE_TAG_NAME = "ROLE_USERNAME_" + i; 
                        role.EXCUTE_ROLE_TAG_PRICE = "ROLE_PRICE_" + i; 
                        i++; 

                        listExecuteRoleRDO.Add(role); 
                    }
                }

                if (IsNotNullOrEmpty(listSereServs))
                {
                    V_HIS_TREATMENT treatment = null; 
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicTreatment.ContainsKey(sereServ.TDL_TREATMENT_ID.Value)) continue; 
                        treatment = dicTreatment[sereServ.TDL_TREATMENT_ID ?? 0]; 
                        var sereServPTTT = listSereServPTTTs.Where(w => w.SERE_SERV_ID == sereServ.ID); 
                        var ekipUsers = listEkipUsers.Where(w => w.EKIP_ID == sereServ.EKIP_ID).ToList(); 
                        Mrs00416RDO rdo = new Mrs00416RDO(); 
                        rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE; 
                        rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME; 
                        rdo.DOB = treatment.TDL_PATIENT_DOB.ToString().Substring(0, 4); 
                        rdo.PTTT_GROUP = sereServPTTT.FirstOrDefault().PTTT_GROUP_NAME; 
                        if (IsNotNullOrEmpty(ekipUsers))
                        {
                            foreach (var ekipUser in ekipUsers)
                            {
                                var remuneration = listRemunerations.Where(w => w.EXECUTE_ROLE_ID == ekipUser.EXECUTE_ROLE_ID && w.SERVICE_ID == sereServ.SERVICE_ID); 

                                var is_ExecuteRole = listExecuteRoleRDO.Where(w => w.EXCUTE_ROLE_ID == ekipUser.EXECUTE_ROLE_ID); 
                                if (is_ExecuteRole != null && is_ExecuteRole.Count() > 0)
                                {
                                    System.Reflection.PropertyInfo name = typeof(Mrs00416RDO).GetProperty(is_ExecuteRole.First().EXCUTE_ROLE_TAG_NAME); 
                                    System.Reflection.PropertyInfo price = typeof(Mrs00416RDO).GetProperty(is_ExecuteRole.First().EXCUTE_ROLE_TAG_PRICE); 
                                    name.SetValue(rdo, ekipUser.USERNAME); 
                                    price.SetValue(rdo, Convert.ToDecimal(remuneration.FirstOrDefault().PRICE)); 
                                }
                            }
                        }

                        listRdo.Add(rdo); 
                    }

                }



            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                //lay vai tro
                foreach(var executeRole in listExecuteRoleRDO)
                {
                    dicSingleTag.Add(executeRole.EXCUTE_ROLE_TAG_NAME,executeRole.EXCUTE_ROLE_NAME); 
                }

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
