using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisTransaction; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00452
{
    //báo cáo bans thuốc của nhà thuốc

    class Mrs00452Processor : AbstractProcessor
    {
        Mrs00452Filter castFilter = null; 
        List<Mrs00452RDO> listRdo = new List<Mrs00452RDO>(); 

        List<HIS_MEDI_STOCK> listMediStockBusiness = new List<HIS_MEDI_STOCK>(); 
        List<V_HIS_EXP_MEST> listSaleExpMests = new List<V_HIS_EXP_MEST>(); 
        List<V_HIS_TRANSACTION> listBills = new List<V_HIS_TRANSACTION>(); 
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>(); 

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>(); 
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>(); 

        public Mrs00452Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00452Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00452Filter)this.reportFilter; 

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery(); 
                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                    mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs; 
                listMediStockBusiness = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);

                Inventec.Common.Logging.LogSystem.Info("listMediStockBusiness" + listMediStockBusiness.Count);
                if (IsNotNullOrEmpty(listMediStockBusiness))
                {
                    HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                    expMestFilter.FINISH_TIME_FROM = castFilter.TIME_FROM; 
                    expMestFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                    expMestFilter.MEDI_STOCK_IDs = listMediStockBusiness.Select(s => s.ID).ToList(); 
                    expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(expMestFilter);

                    Inventec.Common.Logging.LogSystem.Info("listExpMest" + listExpMest.Count);
                    listSaleExpMests = listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList();

                    Inventec.Common.Logging.LogSystem.Info("listSaleExpMests" + listSaleExpMests.Count);
                    var skip = 0; 
                    if (IsNotNullOrEmpty(listSaleExpMests))
                    {
                        while (listSaleExpMests.Count - skip > 0)
                        {
                            var listIds = listSaleExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                            var listBillIds = listIds.Where(s => s.BILL_ID != null).ToList(); 
                            if (IsNotNullOrEmpty(listBillIds))
                            {
                                HisTransactionViewFilterQuery billViewFilter = new HisTransactionViewFilterQuery(); 
                                billViewFilter.IDs = listBillIds.Select(s => s.BILL_ID.Value).ToList(); 
                                listBills.AddRange(new HisTransactionManager(param).GetView(billViewFilter)); 
                            }

                            HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery(); 
                            expMestMedicineViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                            expMestMedicineViewFilter.IS_EXPORT = true;
                            listExpMestMedicines.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter)); 

                            HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                            expMestMaterialViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                            expMestMaterialViewFilter.IS_EXPORT = true; 
                            listExpMestMaterials.AddRange(new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter)); 
                        }

                        skip = 0; 
                        if (IsNotNullOrEmpty(listExpMestMedicines))
                        {
                            while (listExpMestMedicines.Count - skip > 0)
                            {
                                var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                                HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery(); 
                                medicineTypeViewFilter.IDs = listIds.Select(s => s.MEDICINE_TYPE_ID).ToList(); 
                                listMedicineTypes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter)); 
                            }
                        }

                        skip = 0; 
                        if (IsNotNullOrEmpty(listExpMestMaterials))
                        {
                            while (listExpMestMaterials.Count - skip > 0)
                            {
                                var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                                HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery(); 
                                materialTypeViewFilter.IDs = listIds.Select(s => s.MATERIAL_TYPE_ID).ToList();
                                listMaterialTypes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter)); 
                            }
                        }
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
                CommonParam paramGet = new CommonParam(); 

                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    foreach (var medicine in listExpMestMedicines)
                    {
                        var sale = listSaleExpMests.Where(s => s.ID == medicine.EXP_MEST_ID).ToList(); 
                        if (IsNotNullOrEmpty(sale))
                        {
                            var bill = listBills.Where(s => s.ID == sale.First().BILL_ID).ToList(); 
                            if (IsNotNullOrEmpty(bill))
                            {
                                var rdo = new Mrs00452RDO(); 
                                rdo.SALE_EXP_MEST = sale.First(); 
                                rdo.BILL = bill.First();
                                rdo.SERVICE_TYPE_CODE = medicine.MEDICINE_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME;
                                rdo.SERVICE_ID = medicine.MEDICINE_ID ?? 0; 
                                var medicineType = listMedicineTypes.Where(s => s.ID == medicine.MEDICINE_TYPE_ID).ToList(); 
                                if (IsNotNullOrEmpty(medicineType))
                                    rdo.CONCENTRA = medicineType.First().CONCENTRA; 
                                rdo.NUM_ORDER = bill.First().NUM_ORDER; 
                                rdo.TRANSACTION_CODE = bill.First().TRANSACTION_CODE; 

                                rdo.AMOUNT = medicine.AMOUNT; 
                                rdo.IMP_PRICE = medicine.IMP_PRICE; 
                                rdo.PRICE = medicine.PRICE ?? 0;
                                rdo.VAT_RATIO = medicine.VAT_RATIO ?? 0;
                                rdo.IMP_VAT_RATIO = medicine.IMP_VAT_RATIO;
                                rdo.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER; 

                                listRdo.Add(rdo); 
                            }
                        }
                    }
                }

                if (IsNotNullOrEmpty(listExpMestMaterials))
                {
                    foreach (var material in listExpMestMaterials)
                    {
                        var sale = listSaleExpMests.Where(s => s.ID == material.EXP_MEST_ID).ToList(); 
                        if (IsNotNullOrEmpty(sale))
                        {
                            var bill = listBills.Where(s => s.ID == sale.First().BILL_ID).ToList(); 
                            if (IsNotNullOrEmpty(bill))
                            {
                                var rdo = new Mrs00452RDO(); 
                                rdo.SALE_EXP_MEST = sale.First(); 
                                rdo.BILL = bill.First();
                                rdo.SERVICE_TYPE_CODE = material.MATERIAL_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = material.MATERIAL_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME;
                                rdo.SERVICE_ID = material.MATERIAL_ID ?? 0; 
                                var materialType = listMaterialTypes.Where(s => s.ID == material.MATERIAL_TYPE_ID).ToList(); 
                                if (IsNotNullOrEmpty(materialType))
                                    rdo.CONCENTRA = materialType.First().CONCENTRA; 
                                rdo.NUM_ORDER = bill.First().NUM_ORDER; 
                                rdo.TRANSACTION_CODE = bill.First().TRANSACTION_CODE; 

                                rdo.AMOUNT = material.AMOUNT; 
                                rdo.IMP_PRICE = material.IMP_PRICE; 
                                rdo.PRICE = material.PRICE ?? 0; 
                                rdo.VAT_RATIO = material.VAT_RATIO ?? 0;
                                rdo.IMP_VAT_RATIO = material.IMP_VAT_RATIO;
                                rdo.PACKAGE_NUMBER = material.PACKAGE_NUMBER; 

                                listRdo.Add(rdo); 
                            }
                        }
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO); 
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.SERVICE_TYPE_NAME).ToList());
                Inventec.Common.Logging.LogSystem.Info("ListRdo" + listRdo.Count);
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
