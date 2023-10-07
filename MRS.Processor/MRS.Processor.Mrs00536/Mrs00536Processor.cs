using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterial;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00536
{
    class Mrs00536Processor : AbstractProcessor
    {
        Mrs00536Filter castFilter;
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<HIS_EXP_MEST> listAggrExpMest = new List<HIS_EXP_MEST>();
        List<HIS_IMP_MEST> listChmsImpMest = new List<HIS_IMP_MEST>();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_MEDICINE> listMedicine = new List<V_HIS_MEDICINE>();
        List<V_HIS_MATERIAL> listMaterial = new List<V_HIS_MATERIAL>();
        List<long> department_ids = new List<long>();
        HIS_MEDI_STOCK mediStock;

        List<Mrs00536RDO> listRdo = new List<Mrs00536RDO>();

        List<long> EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };

        List<long> IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HPTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH
        };

        public Mrs00536Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00536Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00536Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu tu V_HIS_TREATMENT, Mrs00536 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                mediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetById(castFilter.MEDI_STOCK_ID);
                if (mediStock == null)
                    throw new Exception("Kho nguoi dung truyen vao kho khong dung: " + castFilter.MEDI_STOCK_ID);
                //mediStockName = medistock.MEDI_STOCK_NAME;

                if (!IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs) && IsNotNullOrEmpty(MANAGER.Config.HisDepartmentCFG.DEPARTMENTs))
                {
                    department_ids = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.Select(o => o.ID).ToList();
                }
                else
                {
                    department_ids = castFilter.DEPARTMENT_IDs;
                }

                List<long> listAggrIds = new List<long>();
                List<long> listChmsImpIds = new List<long>();
                if (!castFilter.IS_MEDI_MATE_CHEM.HasValue)
                {
                    List<long> medicineTypeIds = new List<long>();
                    HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                    impFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impFilter);
                    medicineTypeIds.AddRange(listImpMestMedicine.Select(o => o.MEDICINE_TYPE_ID).Distinct().ToList());
                    var chms = listImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                    if (IsNotNullOrEmpty(chms))
                    {
                        listChmsImpIds.AddRange(chms.Select(s => s.IMP_MEST_ID).Distinct().ToList());
                    }

                    HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                    expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expFilter.IS_EXPORT = true;
                    expFilter.EXP_MEST_TYPE_IDs = EXP_MEST_TYPE_IDs;
                    listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expFilter);
                    medicineTypeIds.AddRange(listExpMestMedicine.Select(o => o.MEDICINE_TYPE_ID).Distinct().ToList());
                    var blExpMest = listExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                    if (IsNotNullOrEmpty(blExpMest))
                    {
                        listAggrIds.AddRange(blExpMest.Select(o => o.AGGR_EXP_MEST_ID ?? 0).Distinct().ToList());
                    }

                    medicineTypeIds = medicineTypeIds.Distinct().ToList();
                    if (IsNotNullOrEmpty(medicineTypeIds))
                    {
                        var skip = 0;
                        while (medicineTypeIds.Count - skip > 0)
                        {
                            var limit = medicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var mediTypeFilter = new MOS.MANAGER.HisMedicineType.HisMedicineTypeViewFilterQuery();
                            mediTypeFilter.IDs = limit;
                            var lstmety = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(mediTypeFilter);
                            if (IsNotNullOrEmpty(lstmety))
                            {
                                listMedicineType.AddRange(lstmety);
                            }
                        }
                    }
                    var medicineIds = listExpMestMedicine.Select(x => x.MEDICINE_ID??0).ToList();
                    if (IsNotNullOrEmpty(medicineIds))
                    {
                        var skip = 0;
                        while (medicineIds.Count - skip > 0)
                        {
                            var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var medicineFilter = new MOS.MANAGER.HisMedicine.HisMedicineViewFilterQuery();
                            medicineFilter.IDs = limit;
                            var lstmety = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(medicineFilter);
                            if (IsNotNullOrEmpty(lstmety))
                            {
                                listMedicine.AddRange(lstmety);
                            }
                        }
                    }
                }
                else if (castFilter.IS_MEDI_MATE_CHEM.HasValue)
                {
                    List<long> materialTypeIds = new List<long>();
                    HisImpMestMaterialViewFilterQuery impFilter = new HisImpMestMaterialViewFilterQuery();
                    impFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impFilter);
                    materialTypeIds.AddRange(listImpMestMaterial.Select(o => o.MATERIAL_TYPE_ID).Distinct().ToList());
                    var chms = listImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                    if (IsNotNullOrEmpty(chms))
                    {
                        listChmsImpIds.AddRange(chms.Select(s => s.IMP_MEST_ID).Distinct().ToList());
                    }

                    HisExpMestMaterialViewFilterQuery expFilter = new HisExpMestMaterialViewFilterQuery();
                    expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expFilter.IS_EXPORT = true;
                    expFilter.EXP_MEST_TYPE_IDs = EXP_MEST_TYPE_IDs;
                    listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expFilter);
                    materialTypeIds.AddRange(listExpMestMaterial.Select(o => o.MATERIAL_TYPE_ID).Distinct().ToList());
                    var blExpMest = listExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                    if (IsNotNullOrEmpty(blExpMest))
                    {
                        listAggrIds.AddRange(blExpMest.Select(o => o.AGGR_EXP_MEST_ID ?? 0).Distinct().ToList());
                    }

                    materialTypeIds = materialTypeIds.Distinct().ToList();
                    if (IsNotNullOrEmpty(materialTypeIds))
                    {
                        var skip = 0;
                        while (materialTypeIds.Count - skip > 0)
                        {
                            var limit = materialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var matyTypeFilter = new MOS.MANAGER.HisMaterialType.HisMaterialTypeViewFilterQuery();
                            matyTypeFilter.IDs = limit;
                            var lstmaty = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).GetView(matyTypeFilter);
                            if (IsNotNullOrEmpty(lstmaty))
                            {
                                listMaterialType.AddRange(lstmaty);
                            }
                        }
                    }
                    var materialIds = listExpMestMaterial.Select(x => x.MATERIAL_ID ?? 0).ToList();
                    if (IsNotNullOrEmpty(materialIds))
                    {
                        var skip = 0;
                        while (materialIds.Count - skip > 0)
                        {
                            var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            var materialFilter = new HisMaterialViewFilterQuery();
                            materialFilter.IDs = limit;
                            var lstmaty = new HisMaterialManager(paramGet).GetView(materialFilter);
                            if (IsNotNullOrEmpty(lstmaty))
                            {
                                listMaterial.AddRange(lstmaty);
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(listAggrIds))
                {
                    var skip = 0;
                    while (listAggrIds.Count - skip > 0)
                    {
                        var limit = listAggrIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        MOS.MANAGER.HisExpMest.HisExpMestFilterQuery aggrFilter = new MOS.MANAGER.HisExpMest.HisExpMestFilterQuery();
                        aggrFilter.IDs = limit;
                        var aggrExp = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(aggrFilter);
                        if (IsNotNullOrEmpty(aggrExp))
                            listAggrExpMest.AddRange(aggrExp);
                    }
                }

                if (IsNotNullOrEmpty(listChmsImpIds))
                {
                    var skip = 0;
                    while (listChmsImpIds.Count - skip > 0)
                    {
                        var limit = listChmsImpIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        MOS.MANAGER.HisImpMest.HisImpMestFilterQuery impFilter = new MOS.MANAGER.HisImpMest.HisImpMestFilterQuery();
                        impFilter.IDs = limit;
                        var chmsImp = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).Get(impFilter);
                        if (IsNotNullOrEmpty(chmsImp))
                            listChmsImpMest.AddRange(chmsImp);
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listRdo.Clear();
                Dictionary<long, HIS_EXP_MEST> dicAggr = listAggrExpMest.ToDictionary(o => o.ID);
                Dictionary<long, HIS_EXP_MEST> dicChmsExpMest = new Dictionary<long, HIS_EXP_MEST>();
                if (IsNotNullOrEmpty(listChmsImpMest))
                {
                    var expMestIds = listChmsImpMest.Select(o => o.CHMS_EXP_MEST_ID ?? 0).Distinct().ToList();
                    var dicChmsImp = listChmsImpMest.ToDictionary(o => o.CHMS_EXP_MEST_ID ?? 0);
                    if (IsNotNullOrEmpty(expMestIds))
                    {
                        var skip = 0;
                        while (expMestIds.Count - skip > 0)
                        {
                            var limit = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            MOS.MANAGER.HisExpMest.HisExpMestFilterQuery expFilter = new MOS.MANAGER.HisExpMest.HisExpMestFilterQuery();
                            expFilter.IDs = limit;
                            var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(new CommonParam()).Get(expFilter);
                            if (IsNotNullOrEmpty(expMest))
                            {
                                foreach (var item in expMest)
                                {
                                    if (dicChmsImp.ContainsKey(item.ID))
                                    {
                                        dicChmsExpMest.Add(dicChmsImp[item.ID].ID, item);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!castFilter.IS_MEDI_MATE_CHEM.HasValue && IsNotNullOrEmpty(listMedicineType) && (IsNotNullOrEmpty(listImpMestMedicine) || IsNotNullOrEmpty(listExpMestMedicine)))
                {
                    Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>> dicImp = new Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>>();
                    Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>> dicExp = new Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>>();

                    foreach (var item in listImpMestMedicine)
                    {
                        if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK && dicChmsExpMest.ContainsKey(item.IMP_MEST_ID)
                            && department_ids.Contains(dicChmsExpMest[item.IMP_MEST_ID].REQ_DEPARTMENT_ID))
                        {
                            if (!dicImp.ContainsKey(item.MEDICINE_TYPE_ID))
                                dicImp[item.MEDICINE_TYPE_ID] = new List<V_HIS_IMP_MEST_MEDICINE>();
                            dicImp[item.MEDICINE_TYPE_ID].Add(item);
                        }
                        else
                        {
                            if (!department_ids.Contains(item.REQ_DEPARTMENT_ID ?? 0))
                                continue;
                            if (!IMP_MEST_TYPE_IDs.Contains(item.IMP_MEST_TYPE_ID))
                                continue;
                            if (!dicImp.ContainsKey(item.MEDICINE_TYPE_ID))
                                dicImp[item.MEDICINE_TYPE_ID] = new List<V_HIS_IMP_MEST_MEDICINE>();
                            dicImp[item.MEDICINE_TYPE_ID].Add(item);
                        }
                    }

                    foreach (var item in listExpMestMedicine)
                    {
                        if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL && dicAggr.ContainsKey(item.AGGR_EXP_MEST_ID ?? 0)
                            && department_ids.Contains(dicAggr[item.AGGR_EXP_MEST_ID ?? 0].REQ_DEPARTMENT_ID))
                        {
                            if (!dicExp.ContainsKey(item.MEDICINE_TYPE_ID))
                                dicExp[item.MEDICINE_TYPE_ID] = new List<V_HIS_EXP_MEST_MEDICINE>();
                            dicExp[item.MEDICINE_TYPE_ID].Add(item);
                        }
                        else
                        {
                            if (!department_ids.Contains(item.REQ_DEPARTMENT_ID))
                                continue;
                            if (!EXP_MEST_TYPE_IDs.Contains(item.EXP_MEST_TYPE_ID))
                                continue;

                            if (!dicExp.ContainsKey(item.MEDICINE_TYPE_ID))
                                dicExp[item.MEDICINE_TYPE_ID] = new List<V_HIS_EXP_MEST_MEDICINE>();
                            dicExp[item.MEDICINE_TYPE_ID].Add(item);
                        }
                    }

                    foreach (var item in listMedicineType)
                    {
                        Mrs00536RDO rdo = new Mrs00536RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00536RDO>(rdo, item);
                        if (dicImp.ContainsKey(item.ID))
                        {
                            var ckImpMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                            var impMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                            rdo.IMP_AMOUNT = IsNotNullOrEmpty(impMest) ? impMest.Sum(s => s.AMOUNT) : 0;
                            rdo.CK_IMP_AMOUNT = IsNotNullOrEmpty(ckImpMest) ? ckImpMest.Sum(s => s.AMOUNT) : 0;
                        }

                        if (dicExp.ContainsKey(item.ID))
                        {
                            var ckExpMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK ||
                                o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                            var expMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK &&
                                o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                            rdo.EXP_AMOUNT = IsNotNullOrEmpty(expMest) ? expMest.Sum(s => s.AMOUNT) : 0;
                            rdo.CK_EXP_AMOUNT = IsNotNullOrEmpty(ckExpMest) ? ckExpMest.Sum(s => s.AMOUNT) : 0;
                        }
                        var medicine = listMedicine.Where(x=>x.MEDICINE_TYPE_ID==item.ID).FirstOrDefault();
                        if (medicine!=null)
	                    {
                            rdo.IMP_PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);
	                    }
                       
                        if (rdo.IMP_AMOUNT == 0 && rdo.EXP_AMOUNT == 0 && rdo.CK_IMP_AMOUNT == 0 && rdo.CK_EXP_AMOUNT == 0)
                            continue;
                        listRdo.Add(rdo);
                    }
                }
                else if (castFilter.IS_MEDI_MATE_CHEM.HasValue && IsNotNullOrEmpty(listMaterialType) && (IsNotNullOrEmpty(listExpMestMaterial) || IsNotNullOrEmpty(listImpMestMaterial)))
                {
                    Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>> dicImp = new Dictionary<long, List<V_HIS_IMP_MEST_MATERIAL>>();
                    Dictionary<long, List<V_HIS_EXP_MEST_MATERIAL>> dicExp = new Dictionary<long, List<V_HIS_EXP_MEST_MATERIAL>>();

                    foreach (var item in listImpMestMaterial)
                    {
                        if (item.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK && dicChmsExpMest.ContainsKey(item.IMP_MEST_ID)
                            && department_ids.Contains(dicChmsExpMest[item.IMP_MEST_ID].REQ_DEPARTMENT_ID))
                        {
                            if (!dicImp.ContainsKey(item.MATERIAL_TYPE_ID))
                                dicImp[item.MATERIAL_TYPE_ID] = new List<V_HIS_IMP_MEST_MATERIAL>();
                            dicImp[item.MATERIAL_TYPE_ID].Add(item);
                        }
                        else
                        {
                            if (!department_ids.Contains(item.REQ_DEPARTMENT_ID ?? 0))
                                continue;
                            if (!IMP_MEST_TYPE_IDs.Contains(item.IMP_MEST_TYPE_ID))
                                continue;
                            if (!dicImp.ContainsKey(item.MATERIAL_TYPE_ID))
                                dicImp[item.MATERIAL_TYPE_ID] = new List<V_HIS_IMP_MEST_MATERIAL>();
                            dicImp[item.MATERIAL_TYPE_ID].Add(item);
                        }
                    }

                    foreach (var item in listExpMestMaterial)
                    {
                        if (item.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL && dicAggr.ContainsKey(item.AGGR_EXP_MEST_ID ?? 0)
                            && department_ids.Contains(dicAggr[item.AGGR_EXP_MEST_ID ?? 0].REQ_DEPARTMENT_ID))
                        {
                            if (!dicExp.ContainsKey(item.MATERIAL_TYPE_ID))
                                dicExp[item.MATERIAL_TYPE_ID] = new List<V_HIS_EXP_MEST_MATERIAL>();
                            dicExp[item.MATERIAL_TYPE_ID].Add(item);
                        }
                        else
                        {
                            if (!department_ids.Contains(item.REQ_DEPARTMENT_ID))
                                continue;
                            if (!EXP_MEST_TYPE_IDs.Contains(item.EXP_MEST_TYPE_ID))
                                continue;
                            if (!dicExp.ContainsKey(item.MATERIAL_TYPE_ID))
                                dicExp[item.MATERIAL_TYPE_ID] = new List<V_HIS_EXP_MEST_MATERIAL>();
                            dicExp[item.MATERIAL_TYPE_ID].Add(item);
                        }
                    }

                    if (castFilter.IS_MEDI_MATE_CHEM.Value == 1)//vat tu
                    {
                        var listmaty = listMaterialType.Where(o => o.IS_CHEMICAL_SUBSTANCE != 1).ToList();
                        if (IsNotNullOrEmpty(listmaty))
                        {
                            foreach (var item in listmaty)
                            {
                                Mrs00536RDO rdo = new Mrs00536RDO();
                                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00536RDO>(rdo, item);
                                rdo.MEDICINE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                                rdo.MEDICINE_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                                if (dicImp.ContainsKey(item.ID))
                                {
                                    var ckImpMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                                    var impMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                                    rdo.IMP_AMOUNT = IsNotNullOrEmpty(impMest) ? impMest.Sum(s => s.AMOUNT) : 0;
                                    rdo.CK_IMP_AMOUNT = IsNotNullOrEmpty(ckImpMest) ? ckImpMest.Sum(s => s.AMOUNT) : 0;
                                    
                                }

                                if (dicExp.ContainsKey(item.ID))
                                {
                                    var ckExpMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK ||
                                        o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                                    var expMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK &&
                                        o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                                    rdo.EXP_AMOUNT = IsNotNullOrEmpty(expMest) ? expMest.Sum(s => s.AMOUNT) : 0;
                                    rdo.CK_EXP_AMOUNT = IsNotNullOrEmpty(ckExpMest) ? ckExpMest.Sum(s => s.AMOUNT) : 0;
                                }
                                var material = listMaterial.Where(x => x.MATERIAL_TYPE_ID == item.ID).FirstOrDefault();
                                if (material != null)
                                {
                                    rdo.IMP_PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);
                                }
                                if (rdo.IMP_AMOUNT == 0 && rdo.EXP_AMOUNT == 0 && rdo.CK_IMP_AMOUNT == 0 && rdo.CK_EXP_AMOUNT == 0)
                                    continue;
                                listRdo.Add(rdo);
                            }
                        }
                    }
                    else if (castFilter.IS_MEDI_MATE_CHEM.Value == 0)//hoachat
                    {
                        var listChemi = listMaterialType.Where(o => o.IS_CHEMICAL_SUBSTANCE == 1).ToList();
                        if (IsNotNullOrEmpty(listChemi))
                        {
                            foreach (var item in listChemi)
                            {
                                Mrs00536RDO rdo = new Mrs00536RDO();
                                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00536RDO>(rdo, item);
                                rdo.MEDICINE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                                rdo.MEDICINE_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                                if (dicImp.ContainsKey(item.ID))
                                {
                                    var ckImpMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                                    var impMest = dicImp[item.ID].Where(o => o.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).ToList();
                                    rdo.IMP_AMOUNT = IsNotNullOrEmpty(impMest) ? impMest.Sum(s => s.AMOUNT) : 0;
                                    rdo.CK_IMP_AMOUNT = IsNotNullOrEmpty(ckImpMest) ? ckImpMest.Sum(s => s.AMOUNT) : 0;
                                }

                                if (dicExp.ContainsKey(item.ID))
                                {
                                    var ckExpMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK ||
                                        o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
                                        ).ToList();
                                    var expMest = dicExp[item.ID].Where(o => o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK &&
                                        o.EXP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL).ToList();
                                    rdo.EXP_AMOUNT = IsNotNullOrEmpty(expMest) ? expMest.Sum(s => s.AMOUNT) : 0;
                                    rdo.CK_EXP_AMOUNT = IsNotNullOrEmpty(ckExpMest) ? ckExpMest.Sum(s => s.AMOUNT) : 0;
                                }
                                var material = listMaterial.Where(x => x.MATERIAL_TYPE_ID == item.ID).FirstOrDefault();
                                if (material != null)
                                {
                                    rdo.IMP_PRICE = material.IMP_PRICE *(1 + material.IMP_VAT_RATIO);
                                }
                                if (rdo.IMP_AMOUNT == 0 && rdo.EXP_AMOUNT == 0 && rdo.CK_IMP_AMOUNT == 0 && rdo.CK_EXP_AMOUNT == 0)
                                    continue;
                                listRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                listRdo.Clear();
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                if (!castFilter.IS_MEDI_MATE_CHEM.HasValue)
                {
                    dicSingleTag.Add("TYPE", "Thuốc");
                }
                else if (castFilter.IS_MEDI_MATE_CHEM.HasValue && castFilter.IS_MEDI_MATE_CHEM.Value == 1)
                {
                    dicSingleTag.Add("TYPE", "Vật tư");
                }
                else if (castFilter.IS_MEDI_MATE_CHEM.HasValue && castFilter.IS_MEDI_MATE_CHEM.Value == 0)
                {
                    dicSingleTag.Add("TYPE", "Hóa chất");
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", mediStock.MEDI_STOCK_NAME);
                var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.Where(o => department_ids.Contains(o.ID)).ToList();
                dicSingleTag.Add("DEPARTMENT_NAMEs", String.Join(";", department.Select(o => o.DEPARTMENT_NAME)));
                if (IsNotNullOrEmpty(listRdo))
                {
                    listRdo = listRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                }
                objectTag.AddObjectData(store, "Report", listRdo);
                store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
