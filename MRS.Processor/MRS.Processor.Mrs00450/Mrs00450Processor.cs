using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicinePaty;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
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
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialPaty;
using MOS.MANAGER.HisMaterialType;
using Inventec.Common.Logging;
using MOS.MANAGER.HisExpMestMaterial;

namespace MRS.Processor.Mrs00450
{
    class Mrs00450Processor : AbstractProcessor
    {
        Mrs00450Filter castFilter = null;
        List<Mrs00450RDO> listGroupRdo = new List<Mrs00450RDO>();


        public Mrs00450Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>();
        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MEDICINE_PATY> listMedicinePatys = new List<V_HIS_MEDICINE_PATY>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_MATERIAL> listMaterials = new List<V_HIS_MATERIAL>();
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_MATERIAL_PATY> listMaterialPatys = new List<V_HIS_MATERIAL_PATY>();

        List<V_HIS_MEDI_STOCK> listMediStocks = new List<V_HIS_MEDI_STOCK>();

        public override Type FilterType()
        {
            return typeof(Mrs00450Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00450Filter)this.reportFilter;
                var skip = 0;
                HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                mediStockFilter.ID = this.castFilter.MEDI_STOCK_BUSINESS_ID;
                listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter);

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = this.castFilter.DATE_TIME;
                impMediFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                impMediFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_BUSINESS_ID;
                var listSubImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(listSubImpMestMedicines))
                {
                    listSubImpMestMedicines = listSubImpMestMedicines.OrderBy(o => o.MEDICINE_TYPE_ID).ThenByDescending(t => t.IMP_TIME).ToList();
                    foreach (var impMestMedicine in listSubImpMestMedicines)
                    {
                        var _count = listImpMestMedicines.Where(w => w.MEDICINE_TYPE_ID == impMestMedicine.MEDICINE_TYPE_ID).Count();
                        if (_count <= 2)
                        {
                            listImpMestMedicines.Add(impMestMedicine);
                        }
                    }
                }
                var listMedicineIds = listImpMestMedicines.Where(w => w.MEDICINE_ID != null).Select(s => s.MEDICINE_ID).ToList();

                skip = 0;
                while (listMedicineIds.Count - skip > 0)
                {
                    var listIds = listMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicineViewFilterQuery filter = new HisMedicineViewFilterQuery();
                    filter.IDs = listIds;
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    var medicines = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetView(filter);
                    listMedicines.AddRange(medicines);
                }
                listMedicineIds = listMedicines.Select(s => s.ID).ToList();
                var listMedicineTypeIds = listMedicines.Where(w => w.MEDICINE_TYPE_ID != null).Select(s => s.MEDICINE_TYPE_ID).Distinct().ToList();
                skip = 0;
                while (listMedicineIds.Count - skip > 0)
                {
                    var listIds = listMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicinePatyViewFilterQuery filter = new HisMedicinePatyViewFilterQuery();
                    filter.MEDICINE_IDs = listIds;
                    filter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BUYMEDI;
                    var mediPatys = new MOS.MANAGER.HisMedicinePaty.HisMedicinePatyManager(paramGet).GetView(filter);
                    listMedicinePatys.AddRange(mediPatys);
                }

                skip = 0;
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMedicineTypeViewFilterQuery filter = new HisMedicineTypeViewFilterQuery();
                    filter.IDs = listIds;
                    var medicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(filter);
                    listMedicineTypes.AddRange(medicineTypes);
                }

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = this.castFilter.DATE_TIME;
                impMateFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                impMateFilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_BUSINESS_ID;
                var listSubImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(listSubImpMestMaterials))
                {
                    listSubImpMestMaterials = listSubImpMestMaterials.OrderBy(o => o.MATERIAL_TYPE_ID).ThenByDescending(t => t.IMP_TIME).ToList();
                    foreach (var impMestMaterial in listSubImpMestMaterials)
                    {
                        var _count = listImpMestMaterials.Where(w => w.MATERIAL_TYPE_ID == impMestMaterial.MATERIAL_TYPE_ID).Count();
                        if (_count <= 2)
                        {
                            listImpMestMaterials.Add(impMestMaterial);
                        }
                    }
                }
                var listMaterialIds = listImpMestMaterials.Where(w => w.MATERIAL_TYPE_ID != null).Select(s => s.MATERIAL_ID).ToList();

                skip = 0;
                while (listMaterialIds.Count - skip > 0)
                {
                    var listIds = listMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMaterialViewFilterQuery filter = new HisMaterialViewFilterQuery();
                    filter.IDs = listIds;
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                    var Materials = new MOS.MANAGER.HisMaterial.HisMaterialManager(paramGet).GetView(filter);
                    listMaterials.AddRange(Materials);
                }
                listMaterialIds = listMaterials.Select(s => s.ID).ToList();
                var listMaterialTypeIds = listMaterials.Where(w => w.MATERIAL_TYPE_ID != null).Select(s => s.MATERIAL_TYPE_ID).Distinct().ToList();
                skip = 0;
                while (listMaterialIds.Count - skip > 0)
                {
                    var listIds = listMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMaterialPatyViewFilterQuery filter = new HisMaterialPatyViewFilterQuery();
                    filter.MATERIAL_IDs = listIds;
                    filter.PATIENT_TYPE_ID = MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BUYMEDI;
                    var mediPatys = new MOS.MANAGER.HisMaterialPaty.HisMaterialPatyManager(paramGet).GetView(filter);
                    listMaterialPatys.AddRange(mediPatys);
                }

                skip = 0;
                while (listMaterialTypeIds.Count - skip > 0)
                {
                    var listIds = listMaterialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisMaterialTypeViewFilterQuery filter = new HisMaterialTypeViewFilterQuery();
                    filter.IDs = listIds;
                    var MaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).GetView(filter);
                    listMaterialTypes.AddRange(MaterialTypes);
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
                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    if (castFilter.SERVICE_TYPE_IDs.Count == 1)
                    {
                        if (castFilter.SERVICE_TYPE_IDs.Contains(6))
                        {
                            ProcessMedicine(listMedicines);
                        }
                        else if (castFilter.SERVICE_TYPE_IDs.Contains(7))
                        {
                            ProcessMaterial(listMaterials);
                        }
                    }
                    else
                    {
                        ProcessMedicine(listMedicines);
                        ProcessMaterial(listMaterials);
                    }
                }
                else
                {
                    ProcessMedicine(listMedicines);
                    ProcessMaterial(listMaterials);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessMedicine(List<V_HIS_MEDICINE> listMedicines)
        {
            try
            {
                if (IsNotNullOrEmpty(listMedicines))
                {

                    List<Mrs00450RDO> listRdo = new List<Mrs00450RDO>();
                    foreach (var medicine in listMedicines)
                    {
                        var medicineType = listMedicineTypes.Where(w => w.ID == medicine.MEDICINE_TYPE_ID).ToList();
                        var medicinePatys = listMedicinePatys.Where(w => w.MEDICINE_ID == medicine.ID && w.MODIFY_TIME <= this.castFilter.DATE_TIME).OrderByDescending(o => o.MODIFY_TIME).ToList();
                        if (getBeginAmount(medicine.ID, this.castFilter.DATE_TIME) > 0)
                        {
                            Mrs00450RDO rdo = new Mrs00450RDO();
                            rdo.MEDICINE_TYPE_CODE = medicine.MEDICINE_TYPE_CODE;
                            rdo.MEDICINE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME;
                            if (IsNotNullOrEmpty(medicineType))
                            {
                                rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            }
                            rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO);


                            if (IsNotNullOrEmpty(medicineType))
                            {
                                rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            }
                            rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME;
                            if (IsNotNullOrEmpty(medicinePatys))
                            {
                                rdo.EXP_PRICE = medicinePatys.First().EXP_PRICE * (1 + medicinePatys.First().EXP_VAT_RATIO);
                            }

                            listRdo.Add(rdo);
                        }

                    }
                    var listGroupRdoMedi = listRdo.GroupBy(gr => new
                    {
                        gr.ACTIVE_INGR_BHYT_NAME,
                        gr.EXP_PRICE,
                        gr.MEDICINE_TYPE_CODE,
                        gr.MEDICINE_TYPE_NAME,
                        gr.SERVICE_UNIT_NAME,
                        gr.IMP_PRICE

                    }).Select(s => s.First()).ToList();
                    listGroupRdo.AddRange(listGroupRdoMedi);
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }

        private void ProcessMaterial(List<V_HIS_MATERIAL> listMaterials)
        {
            try
            {
                if (IsNotNullOrEmpty(listMaterials))
                {

                    List<Mrs00450RDO> listRdo = new List<Mrs00450RDO>();
                    foreach (var material in listMaterials)
                    {
                        var materialType = listMaterialTypes.Where(w => w.ID == material.MATERIAL_TYPE_ID).ToList();
                        var materialPatys = listMaterialPatys.Where(w => w.MATERIAL_ID == material.ID && w.MODIFY_TIME <= this.castFilter.DATE_TIME).OrderByDescending(o => o.MODIFY_TIME).ToList();
                        if (getBeginAmountMate(material.ID, this.castFilter.DATE_TIME) > 0)
                        {
                            Mrs00450RDO rdo = new Mrs00450RDO();
                            rdo.MEDICINE_TYPE_CODE = material.MATERIAL_TYPE_CODE;
                            rdo.MEDICINE_TYPE_NAME = material.MATERIAL_TYPE_NAME;
                            if (IsNotNullOrEmpty(materialType))
                            {
                                rdo.ACTIVE_INGR_BHYT_NAME = "";
                            }
                            rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO);

                            if (IsNotNullOrEmpty(materialType))
                            {
                                rdo.ACTIVE_INGR_BHYT_NAME = "";
                            }
                            rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME;
                            if (IsNotNullOrEmpty(materialPatys))
                            {
                                rdo.EXP_PRICE = materialPatys.First().EXP_PRICE * (1 + materialPatys.First().EXP_VAT_RATIO);
                            }

                            listRdo.Add(rdo);
                        }
                    }
                
                
                    var listGroupRdoMate = listRdo.GroupBy(gr => new
                    {
                        gr.ACTIVE_INGR_BHYT_NAME,
                        gr.EXP_PRICE,
                        gr.MEDICINE_TYPE_CODE,
                        gr.MEDICINE_TYPE_NAME,
                        gr.SERVICE_UNIT_NAME,
                        gr.IMP_PRICE

                    }).Select(s => s.First()).ToList();
                    listGroupRdo.AddRange(listGroupRdoMate);
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                //dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("DATE_TIME", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.DATE_TIME));
                dicSingleTag.Add("MEDI_STOCK", listMediStocks.First().MEDI_STOCK_NAME);
                objectTag.AddObjectData(store, "Report", listGroupRdo.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList());

                //objectTag.AddObjectData(store, "Group", listRdoGroup.OrderBy(s => s.GROUP_DATE).ToList()); 
                //bool exportSuccess = true; 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_DATE", "GROUP_DATE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tinh ton dau ky thuoc
        public decimal getBeginAmount(long l_MedicineId, long l_DateTime)
        {
            try
            {
                CommonParam paramGet = new CommonParam();

                HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                impFilter.IMP_TIME_TO = l_DateTime;
                impFilter.MEDICINE_ID = l_MedicineId;
                impFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impFilter);

                HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                expFilter.EXP_TIME_TO = l_DateTime;
                expFilter.MEDICINE_ID = l_MedicineId;
                expFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                expFilter.IS_EXPORT = true;
                var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expFilter);

                return listImpMestMedicines.Sum(su => su.AMOUNT) - listExpMestMedicines.Sum(su => su.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return 0;
        }

        public decimal getBeginAmountMate(long l_MaterialId, long l_DateTime)
        {
            try
            {
                CommonParam paramGet = new CommonParam();

                HisImpMestMaterialViewFilterQuery impFilter = new HisImpMestMaterialViewFilterQuery();
                impFilter.IMP_TIME_TO = l_DateTime;
                impFilter.MATERIAL_ID = l_MaterialId;
                impFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impFilter);

                HisExpMestMaterialViewFilterQuery expFilter = new HisExpMestMaterialViewFilterQuery();
                expFilter.EXP_TIME_TO = l_DateTime;
                expFilter.MATERIAL_ID = l_MaterialId;
                expFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                expFilter.IS_EXPORT = true;
                var listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expFilter);

                return listImpMestMaterials.Sum(su => su.AMOUNT) - listExpMestMaterials.Sum(su => su.AMOUNT);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return 0;
        }

    }
}
