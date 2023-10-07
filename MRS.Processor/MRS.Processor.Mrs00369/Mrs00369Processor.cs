using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisBlood;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestBlood;
using MOS.MANAGER.HisBloodType;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00369
{
    class Mrs00369Processor : AbstractProcessor
    {
        Mrs00369Filter castFilter = null; 
        
        List<Mrs00369RDO> ListRdoGroup = new List<Mrs00369RDO>(); 
        List<Mrs00369RDO> ListRdo = new List<Mrs00369RDO>(); 

        List<V_HIS_MEDI_STOCK> ListCabinets = new List<V_HIS_MEDI_STOCK>(); 

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>(); 

        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>(); 

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>(); 

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>(); 
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>(); 
        List<V_HIS_BLOOD_TYPE> listBloodTypes = new List<V_HIS_BLOOD_TYPE>(); 

        List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>(); 
        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>(); 

         public string MEDI_STOCK_NAME =""; 
         public string IMP_MEST_TYPE_NAME = "";

         public const long GROUP_MEDI_ID = 1;
         public const long GROUP_MATE_ID = 2;
         public const long GROUP_CHEM_ID = 22; 
         public const long GROUP_BLOD_ID = 3; 
        
        public Mrs00369Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00369Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00369Filter)this.reportFilter; 

                HisMediStockViewFilterQuery mediStockViewFilter = new HisMediStockViewFilterQuery(); 
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).GetView(mediStockViewFilter); 

                MEDI_STOCK_NAME = listMediStocks.Where(s => s.ID == castFilter.MEDI_BIG_STOCK_ID).First().MEDI_STOCK_NAME; 

                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery(); 
                impMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                impMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                impMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_BIG_STOCK_ID; 
                impMestViewFilter.IMP_MEST_TYPE_IDs = castFilter.IMP_MEST_TYPE_IDs; 
                impMestViewFilter.IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT }; 
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilter); 

                var skip = 0; 
                // imp mest medicine/material/blood/ChemicalSubstance
                if (IsNotNullOrEmpty(listImpMests))
                {

                    listManuImpMests = listImpMests.Where(o=>o.IMP_MEST_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList();

                    if (!castFilter.IS_MEDICINE && !castFilter.IS_MATERIAL && !castFilter.IS_BLOOD && !castFilter.IS_CHEMICAL_SUBSTANCE)
                    {
                        castFilter.IS_MEDICINE = true;
                        castFilter.IS_MATERIAL = true;
                        castFilter.IS_BLOOD = true;
                        castFilter.IS_CHEMICAL_SUBSTANCE = true; 
                    }

                    if (castFilter.IS_MEDICINE)
                    {
                        skip = 0; 
                        while (listImpMests.Count - skip > 0)
                        {
                            var listIDs = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                            HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery(); 
                            impMestMedicineViewFilter.IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList(); 
                            listImpMestMedicines.AddRange(new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter)); 
                        }

                        if (IsNotNullOrEmpty(listImpMestMedicines))
                        {
                            skip = 0; 
                            var listtMedicineTypeIds = listImpMestMedicines.Select(s => s.MEDICINE_TYPE_ID).Distinct().ToList(); 
                            while (listtMedicineTypeIds.Count - skip > 0)
                            {
                                var listIDs = listtMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                                HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery(); 
                                medicineTypeViewFilter.IDs = listIDs; 
                                listMedicineTypes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter)); 
                            }
                        }
                    }

                    if (castFilter.IS_MATERIAL || castFilter.IS_CHEMICAL_SUBSTANCE)
                    {
                        skip = 0; 
                        while (listImpMests.Count - skip > 0)
                        {
                            var listIDs = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                            HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery(); 
                            impMestMaterialViewFilter.IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList(); 
                            listImpMestMaterials.AddRange(new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter)); 
                        }

                        if (IsNotNullOrEmpty(listImpMestMaterials))
                        {
                            skip = 0; 
                            var listtMaterialTypeIds = listImpMestMaterials.Select(s => s.MATERIAL_TYPE_ID).Distinct().ToList(); 
                            while (listtMaterialTypeIds.Count - skip > 0)
                            {
                                var listIDs = listtMaterialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                                HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery(); 
                                materialTypeViewFilter.IDs = listIDs; 
                                listMaterialTypes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter)); 
                            }
                        }
                        if (!castFilter.IS_CHEMICAL_SUBSTANCE)
                        {
                            listMaterialTypes = listMaterialTypes.Where(o => o.IS_CHEMICAL_SUBSTANCE != 1).ToList();
                            listImpMestMaterials = listImpMestMaterials.Where(o => listMaterialTypes.Exists(p => p.ID == o.MATERIAL_TYPE_ID)).ToList();
                        }
                        if (!castFilter.IS_MATERIAL)
                        {
                            listMaterialTypes = listMaterialTypes.Where(o => o.IS_CHEMICAL_SUBSTANCE == 1).ToList();
                            listImpMestMaterials = listImpMestMaterials.Where(o => listMaterialTypes.Exists(p => p.ID == o.MATERIAL_TYPE_ID)).ToList();
                        }
                    }

                    if (castFilter.IS_BLOOD)
                    {
                        skip = 0; 
                        while (listImpMests.Count - skip > 0)
                        {
                            var listIDs = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                            HisImpMestBloodViewFilterQuery impMestBloodViewFilter = new HisImpMestBloodViewFilterQuery(); 
                            impMestBloodViewFilter.IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList(); 
                            listImpMestBloods.AddRange(new MOS.MANAGER.HisImpMestBlood.HisImpMestBloodManager(param).GetView(impMestBloodViewFilter)); 
                        }

                        if (IsNotNullOrEmpty(listImpMestBloods))
                        {
                            skip = 0; 
                            var listBloodTypeIds = listImpMestBloods.Select(s => s.BLOOD_TYPE_ID).Distinct().ToList(); 
                            while (listBloodTypeIds.Count - skip > 0)
                            {
                                var listIDs = listBloodTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                                HisBloodTypeViewFilterQuery bloodTypeViewFilter = new HisBloodTypeViewFilterQuery(); 
                                bloodTypeViewFilter.IDs = listIDs;
                                listBloodTypes.AddRange(new MOS.MANAGER.HisBloodType.HisBloodTypeManager(param).GetView(bloodTypeViewFilter)); 
                            }
                        }
                    }
                }

                result = true; 
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
                if (IsNotNullOrEmpty(listImpMests))
                {
                    foreach (var impMest in listImpMests)
                    {
                        var listImpMestMedicine = listImpMestMedicines.Where(s => s.IMP_MEST_ID == impMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(listImpMestMedicine))
                        {
                            foreach (var impMestMedicine in listImpMestMedicine)
                            {
                                var rdo = new Mrs00369RDO(); 
                                rdo.GROUP_ID = GROUP_MEDI_ID; 
                                rdo.GROUP_NAME = "THUỐC"; 

                                rdo.SERVICE_ID = impMestMedicine.MEDICINE_ID; 
                                rdo.SERVICE_CODE = "TH" + impMestMedicine.MEDICINE_ID; 

                                rdo.SERVICE_TYPE_ID = impMestMedicine.MEDICINE_TYPE_ID; 
                                rdo.SERVICE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE; 
                                rdo.SERVICE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME; 

                                rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME; 
                                rdo.PACKING_TYPE_NAME = listMedicineTypes.Where(s => s.ID == impMestMedicine.MEDICINE_TYPE_ID).First().PACKING_TYPE_NAME; 

                                rdo.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME; 
                                rdo.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME; 

                                rdo.AMOUNT = impMestMedicine.AMOUNT; 
                                rdo.IMP_PRICE = impMestMedicine.IMP_PRICE; 

                                rdo.BID_NUMBER = impMestMedicine.BID_NUMBER; 
                                rdo.IMP_TIME = impMestMedicine.IMP_TIME; 

                                if (impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
                                {
                                    rdo.DOCUMENT_NUMBER = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_NUMBER; 
                                    rdo.DOCUMENT_DATE = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_DATE; 
                                }

                                ListRdo.Add(rdo); 
                            }
                        }

                        var listImpMestMaterial = listImpMestMaterials.Where(s => s.IMP_MEST_ID == impMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(listImpMestMaterial))
                        {
                            foreach (var impMestMaterial in listImpMestMaterial)
                            {
                                var rdo = new Mrs00369RDO(); 
                                var materialType = listMaterialTypes.FirstOrDefault(s => s.ID == impMestMaterial.MATERIAL_TYPE_ID)??new V_HIS_MATERIAL_TYPE();
                                if (materialType.IS_CHEMICAL_SUBSTANCE == 1)
                                {
                                    rdo.GROUP_ID = GROUP_CHEM_ID;
                                    rdo.GROUP_NAME = "HÓA CHẤT";

                                    rdo.SERVICE_CODE = "HC" + impMestMaterial.MATERIAL_ID;
                                }
                                else {
                                    rdo.GROUP_ID = GROUP_MATE_ID;
                                    rdo.GROUP_NAME = "VẬT TƯ";

                                    rdo.SERVICE_CODE = "VT" + impMestMaterial.MATERIAL_ID;
                                }
                                
                                rdo.SERVICE_ID = impMestMaterial.MATERIAL_ID; 

                                rdo.SERVICE_TYPE_ID = impMestMaterial.MATERIAL_TYPE_ID; 
                                rdo.SERVICE_TYPE_CODE = impMestMaterial.MATERIAL_TYPE_CODE; 
                                rdo.SERVICE_TYPE_NAME = impMestMaterial.MATERIAL_TYPE_NAME; 

                                rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                                rdo.PACKING_TYPE_NAME = impMestMaterial.PACKING_TYPE_NAME; 

                                rdo.MANUFACTURER_NAME = impMestMaterial.MANUFACTURER_NAME; 
                                rdo.NATIONAL_NAME = impMestMaterial.NATIONAL_NAME; 

                                rdo.AMOUNT = impMestMaterial.AMOUNT; 
                                rdo.IMP_PRICE = impMestMaterial.IMP_PRICE; 

                                rdo.BID_NUMBER = impMestMaterial.BID_NUMBER; 
                                rdo.IMP_TIME = impMestMaterial.IMP_TIME; 

                                if (impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
                                {
                                    rdo.DOCUMENT_NUMBER = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_NUMBER; 
                                    rdo.DOCUMENT_DATE = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_DATE; 
                                }

                                ListRdo.Add(rdo); 
                            }
                        }

                        var listImpMestBlood = listImpMestBloods.Where(s => s.IMP_MEST_ID == impMest.ID).ToList(); 
                        if (IsNotNullOrEmpty(listImpMestBlood))
                        {
                            foreach (var impMestBlood in listImpMestBlood)
                            {
                                var rdo = new Mrs00369RDO(); 
                                rdo.GROUP_ID = GROUP_BLOD_ID; 
                                rdo.GROUP_NAME = "MÁU"; 

                                rdo.SERVICE_ID = impMestBlood.BLOOD_ID; 
                                rdo.SERVICE_CODE = "MA" + impMestBlood.BLOOD_ID; 

                                rdo.SERVICE_TYPE_ID = impMestBlood.BLOOD_TYPE_ID; 
                                rdo.SERVICE_TYPE_CODE = impMestBlood.BLOOD_TYPE_CODE; 
                                rdo.SERVICE_TYPE_NAME = impMestBlood.BLOOD_TYPE_NAME; 

                                rdo.SERVICE_UNIT_NAME = impMestBlood.SERVICE_UNIT_NAME; 
                                rdo.PACKING_TYPE_NAME = listBloodTypes.Where(s => s.ID == impMestBlood.BLOOD_TYPE_ID).First().PACKING_TYPE_NAME; 

                                //rdo.MANUFACTURER_NAME = impMestBlood.MANUFACTURER_NAME; 
                                //rdo.NATIONAL_NAME = impMestBlood.NATIONAL_NAME; 

                                rdo.AMOUNT = 1; 
                                rdo.IMP_PRICE = impMestBlood.IMP_PRICE; 

                                rdo.BID_NUMBER = impMestBlood.BID_NUMBER; 
                                rdo.IMP_TIME = impMestBlood.IMP_TIME; 

                                if (impMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC)
                                {
                                    rdo.DOCUMENT_NUMBER = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_NUMBER; 
                                    rdo.DOCUMENT_DATE = listManuImpMests.Where(s => s.ID == impMest.ID).First().DOCUMENT_DATE; 
                                }

                                ListRdo.Add(rdo); 
                            }
                        }
                    }

                    ListRdo = ListRdo.GroupBy(g => g.SERVICE_CODE).Select(s => new Mrs00369RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME,

                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        PACKING_TYPE_NAME = s.First().PACKING_TYPE_NAME,

                        MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                        NATIONAL_NAME = s.First().NATIONAL_NAME,

                        AMOUNT = s.Sum(su => su.AMOUNT),
                        IMP_PRICE = s.First().IMP_PRICE,

                        BID_NUMBER = s.First().BID_NUMBER,
                        IMP_TIME = s.First().IMP_TIME,

                        DOCUMENT_NUMBER = s.First().DOCUMENT_NUMBER,
                        DOCUMENT_DATE = s.First().DOCUMENT_DATE
                    }).ToList(); 

                    ListRdoGroup = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00369RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME
                    }).ToList(); 
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
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup); 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s=>s.SERVICE_TYPE_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(s => s.SERVICE_TYPE_NAME).GroupBy(g => new { g.GROUP_ID, g.SERVICE_TYPE_ID, g.IMP_PRICE }).Select(s => new Mrs00369RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME,

                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        PACKING_TYPE_NAME = s.First().PACKING_TYPE_NAME,

                        MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                        NATIONAL_NAME = s.First().NATIONAL_NAME,

                        AMOUNT = s.Sum(su => su.AMOUNT),
                        IMP_PRICE = s.First().IMP_PRICE,

                        BID_NUMBER = s.First().BID_NUMBER,
                        IMP_TIME = s.First().IMP_TIME,

                        DOCUMENT_NUMBER = s.First().DOCUMENT_NUMBER,
                        DOCUMENT_DATE = s.First().DOCUMENT_DATE
                    }).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Report", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();

                objectTag.AddObjectData(store, "ImpMedicine", (castFilter.IS_MEDICINE == true) ? new ManagerSql().GetImpMedicine(castFilter) : new System.Data.DataTable());
                objectTag.AddObjectData(store, "ImpMaterial", (castFilter.IS_MATERIAL == true) ? new ManagerSql().GetImpMaterial(castFilter) : new System.Data.DataTable());
                objectTag.AddObjectData(store, "ImpChemical", (castFilter.IS_CHEMICAL_SUBSTANCE == true) ? new ManagerSql().GetImpChemical(castFilter) : new System.Data.DataTable());
                objectTag.AddObjectData(store, "ImpMedicineType", (castFilter.IS_MEDICINE == true) ? new ManagerSql().GetImpMedicineType(castFilter) : new System.Data.DataTable());
                objectTag.AddObjectData(store, "ImpMaterialType", (castFilter.IS_MATERIAL == true) ? new ManagerSql().GetImpMaterialType(castFilter) : new System.Data.DataTable());
                objectTag.AddObjectData(store, "ImpChemicalType", (castFilter.IS_CHEMICAL_SUBSTANCE == true) ? new ManagerSql().GetImpChemicalType(castFilter) : new System.Data.DataTable());

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
