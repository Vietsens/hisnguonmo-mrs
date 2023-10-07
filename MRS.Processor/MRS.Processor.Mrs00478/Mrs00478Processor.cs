using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.LibraryHein;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;

namespace MRS.Processor.Mrs00478
{
    class Mrs00478Processor : AbstractProcessor
    {
        Mrs00478Filter castFilter = new Mrs00478Filter();
        List<Mrs00478RDO> listRdo = new List<Mrs00478RDO>();
        List<Mrs00478RDO> listRdoGroup = new List<Mrs00478RDO>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_IMP_MEST> listMobaImpMest = new List<HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        //Dictionary<long, long> dicImpMestExpMest = new Dictionary<long, long>();
        public Mrs00478Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00478Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00478Filter)this.reportFilter;
                var listServiceReqFollow = new List<HIS_SERVICE_REQ>();


                HisServiceReqFilterQuery serviceReqFilter = new HisServiceReqFilterQuery();
                serviceReqFilter.REQUEST_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                serviceReqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                serviceReqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                serviceReqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT,IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT};
                listServiceReqFollow = new HisServiceReqManager(param).Get(serviceReqFilter);
              var listServiceReq = new List<HIS_SERVICE_REQ>();

                var skip = 0;
                var listServiceReqIds = listServiceReqFollow.Select(o => o.PARENT_ID ?? 0).Distinct().ToList();
                while (listServiceReqIds.Count - skip > 0)
                {
                    var listIds = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisServiceReqFilterQuery sqFilter = new HisServiceReqFilterQuery();
                    sqFilter.EXECUTE_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                    sqFilter.HAS_EXECUTE = true;
                    sqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT };
                    sqFilter.IDs = listIds;
                    var listServiceReqSub = new HisServiceReqManager(param).Get(sqFilter);
                    if (IsNotNullOrEmpty(listServiceReqSub))
                        listServiceReq.AddRange(listServiceReqSub);

                    var serviceReqFollowId = listServiceReqFollow.Where(p => listServiceReqSub.Exists(q => q.ID == p.PARENT_ID)).Select(o => o.ID).Distinct().ToList() ?? new List<long>();
                    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                    expMestFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                    expMestFilter.REQ_DEPARTMENT_ID = castFilter.EXECUTE_DEPARTMENT_ID;
                    expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                    expMestFilter.SERVICE_REQ_IDs = serviceReqFollowId;
                    var listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter) ?? new List<HIS_EXP_MEST>();

                    var listExpMestIds = listExpMests.Where(w => serviceReqFollowId.Contains(w.SERVICE_REQ_ID ?? 0)).Select(o => o.ID).Distinct().ToList();
                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_MEST_IDs = listExpMestIds;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    expMestMedicineViewFilter.ORDER_DIRECTION = "ASC";
                    expMestMedicineViewFilter.ORDER_FIELD = "MATERIAL_TYPE_NAME";
                    listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));

                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_MEST_IDs = listExpMestIds;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    expMestMaterialViewFilter.ORDER_DIRECTION = "ASC";
                    expMestMaterialViewFilter.ORDER_FIELD = "MATERIAL_TYPE_NAME";
                    listExpMestMaterials.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter));

                    HisImpMestFilterQuery mobaImpMestFilter = new HisImpMestFilterQuery();
                    mobaImpMestFilter.IMP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL, IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH };
                    mobaImpMestFilter.MOBA_EXP_MEST_IDs = listExpMestIds;
                    listMobaImpMest.AddRange(new HisImpMestManager().Get(mobaImpMestFilter) ?? new List<HIS_IMP_MEST>());
                }
              
                skip = 0;
                while (listMobaImpMest.Count - skip > 0)
                {
                    var listIds = listMobaImpMest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).Distinct().ToList();
                    impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMestMedicineViewFilter.ORDER_DIRECTION = "ASC";
                    impMestMedicineViewFilter.ORDER_FIELD = "MEDICINE_TYPE_NAME";
                    listImpMestMedicines.AddRange(new HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>());
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).Distinct().ToList();
                    impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    impMestMaterialViewFilter.ORDER_DIRECTION = "ASC";
                    impMestMaterialViewFilter.ORDER_FIELD = "MATERIAL_TYPE_NAME";
                    listImpMestMaterials.AddRange(new HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>());
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
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();

                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    foreach (var medicine in listExpMestMedicines)
                    {
                        var rdo = new Mrs00478RDO();
                        rdo.GROUP_SERVICE = "THUỐC";

                        rdo.SERVICE_TYPE_CODE = medicine.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME;

                        rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME;
                        rdo.NATIONAL_NAME = medicine.NATIONAL_NAME;
                        rdo.AMOUNT = medicine.AMOUNT - listImpMestMedicines.Where(o => o.TH_EXP_MEST_MEDICINE_ID == medicine.ID).Sum(s => s.AMOUNT);

                        listRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestMaterials))
                {
                    var listMaterialTypes = new List<HIS_MATERIAL_TYPE>();
                    var skip = 0;
                    while (listExpMestMaterials.Count - skip > 0)
                    {
                        var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisMaterialTypeFilterQuery materialTypeFilter = new HisMaterialTypeFilterQuery();
                        materialTypeFilter.IDs = listIds.Select(s => s.MATERIAL_TYPE_ID).ToList();
                        listMaterialTypes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).Get(materialTypeFilter));
                    }

                    foreach (var material in listExpMestMaterials)
                    {
                        var maty = listMaterialTypes.Where(w => w.ID == material.MATERIAL_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(maty))
                        {
                            var rdo = new Mrs00478RDO();

                            if (maty.First().IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                                rdo.GROUP_SERVICE = "HÓA CHẤT";
                            else
                                rdo.GROUP_SERVICE = "VẬT TƯ";

                            rdo.SERVICE_TYPE_CODE = material.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = material.MATERIAL_TYPE_NAME;

                            rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME;
                            rdo.NATIONAL_NAME = material.NATIONAL_NAME;
                            rdo.AMOUNT = material.AMOUNT - listImpMestMaterials.Where(o => o.TH_EXP_MEST_MATERIAL_ID == material.ID).Sum(s => s.AMOUNT);

                            listRdo.Add(rdo);
                        }
                    }
                   
                }

                // gom  nhóm
                listRdo = listRdo.GroupBy(g => new { g.GROUP_SERVICE, g.SERVICE_TYPE_CODE, g.SERVICE_UNIT_NAME, g.NATIONAL_NAME }).Select(s => new Mrs00478RDO()
                {
                    GROUP_SERVICE = s.First().GROUP_SERVICE,
                    SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,

                    NATIONAL_NAME = s.First().NATIONAL_NAME,
                    AMOUNT = s.Sum(su => su.AMOUNT) 
                }).ToList();
                listRdoGroup = listRdo.GroupBy(g => g.GROUP_SERVICE).Select(s => new Mrs00478RDO() { GROUP_SERVICE = s.First().GROUP_SERVICE }).ToList();
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);
                if (IsNotNullOrEmpty(listMediStocks))
                    dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", listMediStocks.Select(s => s.MEDI_STOCK_NAME)));

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.ID = castFilter.EXECUTE_DEPARTMENT_ID;
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter);
                if (IsNotNullOrEmpty(listDepartments))
                    dicSingleTag.Add("DEPARTMENT_NAME", listDepartments.First().DEPARTMENT_NAME);

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo);
                objectTag.AddObjectData(store, "RdoGroup", listRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "Rdo", "GROUP_SERVICE", "GROUP_SERVICE");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
