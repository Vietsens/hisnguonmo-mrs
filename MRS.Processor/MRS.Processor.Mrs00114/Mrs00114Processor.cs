using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisRoom; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisServiceType; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterialType; 

namespace MRS.Processor.Mrs00114
{
    public class Mrs00114Processor : AbstractProcessor
    {
        Mrs00114Filter castFilter = null; 
        List<Mrs00114RDO> ListMedicineRdo = new List<Mrs00114RDO>(); 
        List<Mrs00114RDO> ListMaterialRdo = new List<Mrs00114RDO>();
        List<V_HIS_SERVICE_METY> ListServiceMety = new List<V_HIS_SERVICE_METY>();
        List<V_HIS_SERVICE_MATY> ListServiceMaty = new List<V_HIS_SERVICE_MATY>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_MATERIAL_TYPE> listMaterialType = new List<HIS_MATERIAL_TYPE>();
        private string DEPARTMENT_NAMEs; 
        private string ROOM_NAMEs; 
        private string SERVICE_TYPE_NAME; 

        public Mrs00114Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00114Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00114Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_SERVICE_METY, V_HIS_SERVICE_MATY. MRS00114, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 
                
                HisServiceMetyViewFilterQuery metyFilter = new HisServiceMetyViewFilterQuery(); 
                metyFilter.SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID; 
                ListServiceMety = new MOS.MANAGER.HisServiceMety.HisServiceMetyManager(paramGet).GetView(metyFilter); 

                HisServiceMatyViewFilterQuery matyFilter = new HisServiceMatyViewFilterQuery(); 
                matyFilter.SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID; 
                ListServiceMaty = new MOS.MANAGER.HisServiceMaty.HisServiceMatyManager(paramGet).GetView(matyFilter);

                HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                ssFilter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                ssFilter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ssFilter.TDL_SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID;
                ssFilter.TDL_EXECUTE_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                ssFilter.TDL_EXECUTE_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                ssFilter.TDL_EXECUTE_ROOM_IDs = castFilter.ROOM_IDs;
                ssFilter.TDL_EXECUTE_ROOM_ID = castFilter.ROOM_ID;
                ListSereServ = new HisSereServManager(paramGet).Get(ssFilter);
                listMaterialType = new HisMaterialTypeManager(paramGet).Get(new HisMaterialTypeFilterQuery());
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_SERVICE_METY va V_HIS_SERVICE_MATY, MRS00114." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet)); 
                }

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
            bool result = false; 
            try
            {
                ProcessListServiceMetyAndMaty(ListServiceMety, ListServiceMaty); 
                GetDepartmentAndRoom(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListServiceMetyAndMaty(List<V_HIS_SERVICE_METY> ListServiceMety, List<V_HIS_SERVICE_MATY> ListServiceMaty)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
              
                ProcessListServiceMety(ListServiceMety, ListSereServ); 
                ProcessListServiceMaty(ListServiceMaty, ListSereServ); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListMedicineRdo.Clear(); 
                ListMaterialRdo.Clear(); 
            }
        }

        private void ProcessListServiceMety(List<V_HIS_SERVICE_METY> ListServiceMety, List<HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListServiceMety) && IsNotNullOrEmpty(ListSereServ))
                {
                   
                    var Groups = ListServiceMety.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.SERVICE_UNIT_ID }).ToList(); 
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERVICE_METY> listSub = group.ToList<V_HIS_SERVICE_METY>(); 
                        List<long> listServiceId = listSub.Select(s => s.SERVICE_ID).ToList(); 
                        List<HIS_SERE_SERV> hisSereServs = ListSereServ.Where(o => listServiceId.Contains(o.SERVICE_ID)).ToList(); 
                        Mrs00114RDO rdo = new Mrs00114RDO(listSub); 
                        var GroupService = hisSereServs.GroupBy(g => g.SERVICE_ID).ToList(); 
                        foreach (var groupss in GroupService)
                        {
                            List<HIS_SERE_SERV> listssSub = groupss.ToList<HIS_SERE_SERV>(); 
                            var serviceMety = listSub.FirstOrDefault(f => f.SERVICE_ID == listssSub.First().SERVICE_ID); 
                            if (serviceMety != null)
                            {
                                rdo.TOTAL_AMOUNT += (listssSub.Sum(s => s.AMOUNT)) * serviceMety.EXPEND_AMOUNT; 
                            }
                        }
                        ListMedicineRdo.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListMedicineRdo.Clear(); 
            }
        }

        private void ProcessListServiceMaty(List<V_HIS_SERVICE_MATY> ListServiceMaty, List<HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListServiceMaty) && IsNotNullOrEmpty(ListSereServ))
                {
                 
                    var Groups = ListServiceMaty.GroupBy(g => new { g.MATERIAL_TYPE_ID, g.SERVICE_UNIT_ID }).ToList(); 
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERVICE_MATY> listSub = group.ToList<V_HIS_SERVICE_MATY>(); 
                        List<long> listServiceId = listSub.Select(s => s.SERVICE_ID).ToList(); 
                        List<HIS_SERE_SERV> hisSereServs = ListSereServ.Where(o => listServiceId.Contains(o.SERVICE_ID)).ToList(); 
                        Mrs00114RDO rdo = new Mrs00114RDO(listSub);
                        rdo.IS_CHEMICAL_SUBSTANCE = (listMaterialType.FirstOrDefault(o => o.ID == group.Key.MATERIAL_TYPE_ID) ?? new HIS_MATERIAL_TYPE()).IS_CHEMICAL_SUBSTANCE;
                        var GroupService = hisSereServs.GroupBy(g => g.SERVICE_ID).ToList(); 
                        foreach (var groupss in GroupService)
                        {
                            List<HIS_SERE_SERV> listssSub = groupss.ToList<HIS_SERE_SERV>(); 
                            var serviceMety = listSub.FirstOrDefault(f => f.SERVICE_ID == listssSub.First().SERVICE_ID); 
                            if (serviceMety != null)
                            {
                                rdo.TOTAL_AMOUNT += (listssSub.Sum(s => s.AMOUNT)) * serviceMety.EXPEND_AMOUNT; 
                            }
                        }
                        ListMaterialRdo.Add(rdo); 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void GetDepartmentAndRoom()
        {
            try
            {
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    var department = new HisDepartmentManager().GetById(castFilter.DEPARTMENT_ID.Value); 
                    if (department != null)
                    {
                        DEPARTMENT_NAMEs = department.DEPARTMENT_NAME; 
                    }
                }
                else if (IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs))
                {
                    HisDepartmentFilterQuery departFilter = new HisDepartmentFilterQuery(); 
                    departFilter.IDs = castFilter.DEPARTMENT_IDs; 
                    var departments = new HisDepartmentManager().Get(departFilter); 
                    if (IsNotNullOrEmpty(departments))
                    {
                        foreach (var depart in departments)
                        {
                            if (IsNotNull(DEPARTMENT_NAMEs))
                            {
                                DEPARTMENT_NAMEs = DEPARTMENT_NAMEs + " - " + depart.DEPARTMENT_NAME; 
                            }
                            else
                            {
                                DEPARTMENT_NAMEs = depart.DEPARTMENT_NAME; 
                            }
                        }
                    }
                }

                if (castFilter.ROOM_ID.HasValue)
                {
                    var room = new HisRoomManager().GetView(new HisRoomViewFilterQuery(){ID =castFilter.ROOM_ID.Value }).FirstOrDefault(); 
                    if (IsNotNull(room))
                    {
                        ROOM_NAMEs = room.ROOM_NAME; 
                    }
                }
                else if (IsNotNullOrEmpty(castFilter.ROOM_IDs))
                {
                    HisRoomViewFilterQuery roomFilter = new HisRoomViewFilterQuery(); 
                    roomFilter.IDs = castFilter.ROOM_IDs; 
                    var rooms = new HisRoomManager().GetView(roomFilter); 
                    foreach (var room in rooms)
                    {
                        if (IsNotNull(ROOM_NAMEs))
                        {
                            ROOM_NAMEs = ROOM_NAMEs + " - " + room.ROOM_NAME; 
                        }
                        else
                        {
                            ROOM_NAMEs = room.ROOM_NAME; 
                        }
                    }
                }
                if (castFilter.SERVICE_TYPE_ID > 0)
                {
                    var serviceType = new HisServiceTypeManager().GetById(castFilter.SERVICE_TYPE_ID); 
                    if (IsNotNull(serviceType))
                    {
                        SERVICE_TYPE_NAME = serviceType.SERVICE_TYPE_NAME; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INSTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INSTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                dicSingleTag.Add("SERVICE_TYPE_NAME", SERVICE_TYPE_NAME); 
                dicSingleTag.Add("DEPARTMENT_NAMEs", DEPARTMENT_NAMEs); 
                dicSingleTag.Add("ROOM_NAMEs", ROOM_NAMEs);

                objectTag.AddObjectData(store, "ReportMaterial", ListMaterialRdo.Where(o => o.IS_CHEMICAL_SUBSTANCE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList());
                objectTag.AddObjectData(store, "ReportChemicalSubstance", ListMaterialRdo.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList()); 
                objectTag.AddObjectData(store, "ReportMedicine", ListMedicineRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
