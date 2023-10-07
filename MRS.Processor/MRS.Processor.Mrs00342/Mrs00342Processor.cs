using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
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
using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisSupplier;

namespace MRS.Processor.Mrs00342
{
    public class Mrs00342Processor : AbstractProcessor
    {
        List<Mrs00342RDO> ListRdo = new List<Mrs00342RDO>();
        Mrs00342Filter castFilter = new Mrs00342Filter();

        List<V_HIS_MEDI_STOCK_PERIOD> listMediStockPeriods = new List<V_HIS_MEDI_STOCK_PERIOD>();
        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_EXP_MEST> listManuExpMests = new List<V_HIS_EXP_MEST>();

        List<V_HIS_IMP_MEST> listChmsImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_EXP_MEST> listChmsExpMests = new List<V_HIS_EXP_MEST>();

        List<V_HIS_EXP_MEST> listPrescriptions = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST> listSaleExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_EXP_MEST> listDepaExpMests = new List<V_HIS_EXP_MEST>();

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
            //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HCS
        };

        List<long> CHMS_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS,
            //IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HCS
        };

        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
        };

        List<long> PRES_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
        };

        public decimal? AMOUNT_BEGIN = 0;
        public decimal? TOTAL_PRICE_BEGIN = 0;
        public decimal? AMOUNT_END = 0;
        public decimal? TOTAL_PRICE_END = 0;

        SERVICE service = new SERVICE();

        CommonParam paramGet = new CommonParam();
        public Mrs00342Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00342Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00342Filter)this.reportFilter);

                // MEDI_STOCK
                HisMediStockViewFilterQuery mediStockViewFilter = new HisMediStockViewFilterQuery();
                mediStockViewFilter.ID = castFilter.MEDI_STOCK_ID;
                var mediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockViewFilter);
                if (IsNotNullOrEmpty(mediStocks))
                    service.MEDI_STOCK_NAME = mediStocks.FirstOrDefault().MEDI_STOCK_NAME;
                // MEDI_STOCK_PERIOD
                HisMediStockPeriodViewFilterQuery mediStockPeriodViewFilter = new HisMediStockPeriodViewFilterQuery();
                mediStockPeriodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                mediStockPeriodViewFilter.CREATE_TIME_TO = castFilter.TIME_FROM - 1;
                listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(mediStockPeriodViewFilter);
                // MEDICINE or MATERIAL
                if (IsNotNull(castFilter.MEDICINE_TYPE_ID))
                {
                    #region
                    //HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery(); 
                    //medicineTypeViewFilter.ID = castFilter.MEDICINE_TYPE_ID; 
                    //var medicines = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(medicineTypeViewFilter); 
                    //if (IsNotNullOrEmpty(medicines))
                    //{
                    //    service.SERVICE_TYPE_NAME = medicines.First().MEDICINE_TYPE_NAME; 
                    //    service.SERVICE_TYPE_CODE = medicines.First().MEDICINE_TYPE_CODE; 
                    //    service.SERVICE_UNIT_NAME = medicines.First().SERVICE_UNIT_NAME; 
                    //    service.NATIONAL_NAME = medicines.First().NATIONAL_NAME; 
                    //}

                    //HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery(); 
                    //impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                    //impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                    //impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //impMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                    //listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineViewFilter); 

                    //var skip = 0; 
                    //while (listImpMestMedicines.Count - skip > 0)
                    //{
                    //    var listIds = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    //    HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery(); 
                    //    impMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList(); 
                    //    var listImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestViewFilter); 
                    //    listImpMests.AddRange(listImpMest); 

                    //    HisManuImpMestViewFilterQuery manuImpMestviewFilter = new HisManuImpMestViewFilterQuery(); 
                    //    manuImpMestviewFilter.IMP_MEST_IDs = listIds.Select(s=>s.IMP_MEST_ID).ToList(); 
                    //    listManuImpMests.AddRange(new MOS.MANAGER.HisManuImpMestManager(param).GetView(manuImpMestviewFilter); 
                    //}

                    //HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery(); 
                    //expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                    //expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                    //expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //expMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                    //expMestMedicineViewFilter.IN_EXECUTE = true; 
                    //listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineViewFilter); 

                    //HisManuExpMestViewFilterQuery manuExpMestViewFilter = new HisManuExpMestViewFilterQuery(); 
                    //manuExpMestViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                    //manuExpMestViewFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                    //manuExpMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //manuExpMestViewFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    //listManuExpMests = new MOS.MANAGER.HisManuExpMest.HisManuExpMestManager(param).GetView(manuExpMestViewFilter); 

                    //skip = 0; 
                    //while (listExpMestMedicines.Count - skip > 0)
                    //{
                    //    var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    //    HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery(); 
                    //    expMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID).ToList(); 
                    //    var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestViewFilter); 
                    //    listExpMests.AddRange(listExpMest); 
                    //}
                    #endregion
                    GetDataMedicine();
                }
                else if (IsNotNull(castFilter.MATERIAL_TYPE_ID))
                {
                    #region
                    //HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery(); 
                    //materialTypeViewFilter.ID = castFilter.MEDICINE_TYPE_ID; 
                    //var materials = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).GetView(materialTypeViewFilter); 
                    //if (IsNotNullOrEmpty(materials))
                    //{
                    //    service.SERVICE_TYPE_NAME = materials.First().MATERIAL_TYPE_NAME; 
                    //    service.SERVICE_TYPE_CODE = materials.First().MATERIAL_TYPE_CODE; 
                    //    service.SERVICE_UNIT_NAME = materials.First().SERVICE_UNIT_NAME; 
                    //    service.NATIONAL_NAME = materials.First().NATIONAL_NAME; 
                    //}

                    //HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery(); 
                    //impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM; 
                    //impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO; 
                    //impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //impMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                    //listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter); 

                    //var skip = 0; 
                    //while (listImpMestMaterials.Count - skip > 0)
                    //{
                    //    var listIds = listImpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    //    HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery(); 
                    //    impMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList(); 
                    //    var listImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestViewFilter); 
                    //    listImpMests.AddRange(listImpMest); 

                    //    HisManuImpMestViewFilterQuery manuImpMestviewFilter = new HisManuImpMestViewFilterQuery(); 
                    //    manuImpMestviewFilter.IMP_MEST_IDs = listIds.Select(s => s.IMP_MEST_ID).ToList(); 
                    //    listManuImpMests.AddRange(new MOS.MANAGER.HisManuImpMestManager(param).GetView(manuImpMestviewFilter); 
                    //}

                    //HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery(); 
                    //expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                    //expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                    //expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //expMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID; 
                    //expMestMaterialViewFilter.IN_EXECUTE = true; 
                    //listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMaterialViewFilter); 

                    //HisManuExpMestViewFilterQuery manuExpMestViewFilter = new HisManuExpMestViewFilterQuery(); 
                    //manuExpMestViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM; 
                    //manuExpMestViewFilter.EXP_TIME_TO = castFilter.TIME_TO; 
                    //manuExpMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID; 
                    //manuExpMestViewFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    //listManuExpMests = new MOS.MANAGER.HisManuExpMest.HisManuExpMestManager(param).GetView(manuExpMestViewFilter); 

                    //skip = 0; 
                    //while (listExpMestMaterials.Count - skip > 0)
                    //{
                    //    var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //    HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery(); 
                    //    expMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID).ToList(); 
                    //    var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestViewFilter); 
                    //    listExpMests.AddRange(listExpMest); 
                    //}
                    #endregion
                    GetDataMaterial();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected void GetDataMedicine()
        {
            HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery();
            medicineTypeViewFilter.ID = castFilter.MEDICINE_TYPE_ID;
            var medicines = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).GetView(medicineTypeViewFilter);
            if (IsNotNullOrEmpty(medicines))
            {
                service.SERVICE_TYPE_NAME = medicines.First().MEDICINE_TYPE_NAME;
                service.SERVICE_TYPE_CODE = medicines.First().MEDICINE_TYPE_CODE;
                service.SERVICE_UNIT_NAME = medicines.First().SERVICE_UNIT_NAME;
                service.NATIONAL_NAME = medicines.First().NATIONAL_NAME;
            }
            // nhập
            HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
            impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            impMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
            impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineViewFilter);

            var skip = 0;
            while (listImpMestMedicines.Count - skip > 0)
            {
                var listIds = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                var listImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestViewFilter);
                listImpMests.AddRange(listImpMest);

                listManuImpMests.AddRange(listImpMest.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList());

                listChmsImpMests.AddRange(listImpMest.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList());

                listMobaImpMests.AddRange(listImpMest.Where(o => MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList());
            }

            // xuất
            HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
            expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            expMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
            expMestMedicineViewFilter.IS_EXPORT = true;
            listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineViewFilter);

            skip = 0;
            while (listExpMestMedicines.Count - skip > 0)
            {
                var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery();
                expMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestViewFilter);
                listExpMests.AddRange(listExpMest);

                listManuExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC).ToList());

                listChmsExpMests.AddRange(listExpMest.Where(o => CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList());

                listSaleExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList());

                listDepaExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList());

                listPrescriptions.AddRange(listExpMest.Where(o => PRES_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList());
            }
        }

        protected void GetDataMaterial()
        {
            HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
            materialTypeViewFilter.ID = castFilter.MATERIAL_TYPE_ID;
            var materials = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(paramGet).GetView(materialTypeViewFilter);
            if (IsNotNullOrEmpty(materials))
            {
                service.SERVICE_TYPE_NAME = materials.First().MATERIAL_TYPE_NAME;
                service.SERVICE_TYPE_CODE = materials.First().MATERIAL_TYPE_CODE;
                service.SERVICE_UNIT_NAME = materials.First().SERVICE_UNIT_NAME;
                service.NATIONAL_NAME = materials.First().NATIONAL_NAME;
            }
            // nhập
            HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
            impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            impMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MATERIAL_TYPE_ID;
            listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter);

            var skip = 0;
            while (listImpMestMaterials.Count - skip > 0)
            {
                var listIds = listImpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                var listImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(impMestViewFilter);
                listImpMests.AddRange(listImpMest);

                listManuImpMests.AddRange(listImpMest.Where(o => o.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).ToList());

                listChmsImpMests.AddRange(listImpMest.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList());

                listMobaImpMests.AddRange(listImpMest.Where(o => MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList());
            }

            // xuất
            HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
            expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            expMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MATERIAL_TYPE_ID;
            expMestMaterialViewFilter.IS_EXPORT = true;
            listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMaterialViewFilter);

            skip = 0;
            while (listExpMestMaterials.Count - skip > 0)
            {
                var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery();
                expMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID ?? 0).ToList();
                var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).GetView(expMestViewFilter);
                listExpMests.AddRange(listExpMest);

                listManuExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__TNCC).ToList());

                listChmsExpMests.AddRange(listExpMest.Where(o => CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList());

                listSaleExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList());

                listDepaExpMests.AddRange(listExpMest.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP).ToList());

                listPrescriptions.AddRange(listExpMest.Where(o => PRES_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList());

            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                Dictionary<long, HIS_SUPPLIER> dicSupplier = new HisSupplierManager().Get(new HisSupplierFilterQuery()).ToDictionary(o => o.ID);
                // tồn đầu kỳ
                ProcessBegin();
                // nhập xuất trong kỳ
                // thuốc

                if (IsNotNull(castFilter.MEDICINE_TYPE_ID))
                {
                    foreach (var listImpMestMedicine in listImpMestMedicines)
                    {
                        var impMests = listImpMests.Where(s => s.ID == listImpMestMedicine.IMP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(impMests))
                        {
                            var impMest = impMests.First();
                            var rdo = new Mrs00342RDO();
                            rdo.SUPPLIER_NAME = dicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0) ? dicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME : "";
                            //rdo.MEDI_STOCK_NAME = impMest.EXP_MEDI_STOCK_NAME; 

                            //rdo.MOBA_EXP_MEST_CODE = impMest.MOBA_EXP_MEST_ID; 
                            //rdo.TREATMENT_CODE = impMest.TDL_TREATMENT_CODE; 
                            //rdo.VIR_PATIENT_NAME = impMest.TDL_PATIENT_NAME; 
                            rdo.IMP_EXP_TIME = listImpMestMedicine.IMP_TIME ?? 0;
                            rdo.IMP_EXP_CODE = impMest.IMP_MEST_CODE;
                            rdo.IMP_EXP_TYPE = impMest.IMP_MEST_TYPE_NAME;
                            rdo.PRICE = listImpMestMedicine.IMP_PRICE;
                            rdo.IMP_AMOUNT = listImpMestMedicine.AMOUNT;
                            rdo.TOTAL_IMP_PRICE = listImpMestMedicine.IMP_PRICE * listImpMestMedicine.AMOUNT;
                            rdo.IMP_VAT_RATIO = listImpMestMedicine.IMP_VAT_RATIO;
                            rdo.VAT_RATIO = listImpMestMedicine.VAT_RATIO ?? 0;
                            ListRdo.Add(rdo);

                            AMOUNT_END += rdo.IMP_AMOUNT;
                            TOTAL_PRICE_END += listImpMestMedicine.IMP_PRICE * (1 + listImpMestMedicine.IMP_VAT_RATIO) * listImpMestMedicine.AMOUNT;
                        }
                    }
                    foreach (var listExpMestMedicine in listExpMestMedicines)
                    {
                        var expMests = listExpMests.Where(s => s.ID == listExpMestMedicine.EXP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(expMests))
                        {
                            var expMest = expMests.First();
                            var rdo = new Mrs00342RDO();

                            rdo.SUPPLIER_NAME = dicSupplier.ContainsKey(expMest.SUPPLIER_ID ?? 0) ? dicSupplier[expMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME : "";
                            rdo.CLIENT_NAME = expMest.TDL_PATIENT_NAME;
                            rdo.TREATMENT_CODE = expMest.TDL_TREATMENT_CODE;
                            //rdo.expMest.IMP_MEDI_STOCK_NAME; 
                            rdo.DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME;
                            rdo.ROOM_NAME = expMest.REQ_ROOM_NAME;
                            //    }
                            //}
                            //else if (expMest.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                            //{
                            //    var pres = listPrescriptions.Where(s => s.EXP_MEST_ID == expMest.ID).ToList(); 
                            //    if (IsNotNullOrEmpty(pres))
                            //    {
                            //        rdo.TREATMENT_CODE = pres.First().TREATMENT_CODE; 
                            //        rdo.VIR_PATIENT_NAME = pres.First().VIR_PATIENT_NAME; 
                            //    }
                            //}

                            rdo.IMP_EXP_TIME = listExpMestMedicine.EXP_TIME ?? 0;
                            rdo.IMP_EXP_CODE = expMest.EXP_MEST_CODE;
                            rdo.IMP_EXP_TYPE = expMest.EXP_MEST_TYPE_NAME;
                            rdo.PRICE = listExpMestMedicine.IMP_PRICE;
                            rdo.EXP_AMOUNT = listExpMestMedicine.AMOUNT;
                            rdo.TOTAL_EXP_PRICE = listExpMestMedicine.AMOUNT * listExpMestMedicine.IMP_PRICE;
                            rdo.IMP_VAT_RATIO = listExpMestMedicine.IMP_VAT_RATIO;
                            rdo.VAT_RATIO = listExpMestMedicine.VAT_RATIO ?? 0;
                            ListRdo.Add(rdo);

                            AMOUNT_END += -(rdo.EXP_AMOUNT);
                            TOTAL_PRICE_END += -(listExpMestMedicine.AMOUNT * (1 + listExpMestMedicine.IMP_VAT_RATIO) * listExpMestMedicine.IMP_PRICE);
                        }
                    }
                }
                //vật tư
                else if (IsNotNull(castFilter.MATERIAL_TYPE_ID))
                {
                    foreach (var listImpMestMaterial in listImpMestMaterials)
                    {
                        var impMests = listImpMests.Where(s => s.ID == listImpMestMaterial.IMP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(impMests))
                        {
                            var impMest = impMests.First();
                            var rdo = new Mrs00342RDO();
                            rdo.SUPPLIER_NAME = dicSupplier.ContainsKey(impMest.SUPPLIER_ID ?? 0) ? dicSupplier[impMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME : "";
                            //rdo.MEDI_STOCK_NAME = impMest.EXP_MEDI_STOCK_NAME; 
                            //rdo.MOBA_EXP_MEST_CODE = impMest.MOBA_EXP_MEST_ID; 
                            //rdo.TREATMENT_CODE = impMest.TDL_TREATMENT_CODE; 
                            //rdo.VIR_PATIENT_NAME = impMest.TDL_PATIENT_NAME; 
                            rdo.IMP_EXP_TIME = listImpMestMaterial.IMP_TIME ?? 0;
                            rdo.IMP_EXP_CODE = impMest.IMP_MEST_CODE;
                            rdo.IMP_EXP_TYPE = impMest.IMP_MEST_TYPE_NAME;
                            rdo.PRICE = listImpMestMaterial.IMP_PRICE;
                            rdo.IMP_AMOUNT = listImpMestMaterial.AMOUNT;
                            rdo.TOTAL_IMP_PRICE = listImpMestMaterial.IMP_PRICE * listImpMestMaterial.AMOUNT;
                            rdo.IMP_VAT_RATIO = listImpMestMaterial.IMP_VAT_RATIO;
                            rdo.VAT_RATIO = listImpMestMaterial.VAT_RATIO ?? 0;
                            ListRdo.Add(rdo);

                            AMOUNT_END += rdo.IMP_AMOUNT;
                            TOTAL_PRICE_END += listImpMestMaterial.IMP_PRICE * (1 + listImpMestMaterial.IMP_VAT_RATIO) * listImpMestMaterial.AMOUNT;
                        }
                    }
                    foreach (var listExpMestMaterial in listExpMestMaterials)
                    {
                        var expMests = listExpMests.Where(s => s.ID == listExpMestMaterial.EXP_MEST_ID).ToList();
                        if (IsNotNullOrEmpty(expMests))
                        {
                            var expMest = expMests.First();
                            var rdo = new Mrs00342RDO();

                            rdo.SUPPLIER_NAME = dicSupplier.ContainsKey(expMest.SUPPLIER_ID ?? 0) ? dicSupplier[expMest.SUPPLIER_ID ?? 0].SUPPLIER_NAME : "";
                            rdo.CLIENT_NAME = expMest.TDL_PATIENT_NAME;
                            rdo.TREATMENT_CODE = expMest.TDL_TREATMENT_CODE;
                            //rdo.expMest.IMP_MEDI_STOCK_NAME; 
                            rdo.DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME;
                            rdo.ROOM_NAME = expMest.REQ_ROOM_NAME;
                            //    }
                            //}
                            //else if (expMest.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                            //{
                            //    var pres = listPrescriptions.Where(s => s.EXP_MEST_ID == expMest.ID).ToList(); 
                            //    if (IsNotNullOrEmpty(pres))
                            //    {
                            //        rdo.TREATMENT_CODE = pres.First().TREATMENT_CODE; 
                            //        rdo.VIR_PATIENT_NAME = pres.First().VIR_PATIENT_NAME; 
                            //    }
                            //}

                            rdo.IMP_EXP_TIME = listExpMestMaterial.EXP_TIME ?? 0;
                            rdo.IMP_EXP_CODE = expMest.EXP_MEST_CODE;
                            rdo.IMP_EXP_TYPE = expMest.EXP_MEST_TYPE_NAME;
                            rdo.PRICE = listExpMestMaterial.IMP_PRICE;
                            rdo.EXP_AMOUNT = listExpMestMaterial.AMOUNT;
                            rdo.TOTAL_EXP_PRICE = listExpMestMaterial.AMOUNT * listExpMestMaterial.IMP_PRICE;
                            rdo.IMP_VAT_RATIO = listExpMestMaterial.IMP_VAT_RATIO;
                            rdo.VAT_RATIO = listExpMestMaterial.VAT_RATIO ?? 0;
                            ListRdo.Add(rdo);

                            AMOUNT_END += -(rdo.EXP_AMOUNT);
                            TOTAL_PRICE_END += -(listExpMestMaterial.AMOUNT * (1 + listExpMestMaterial.IMP_VAT_RATIO) * listExpMestMaterial.IMP_PRICE);
                        }
                    }
                }
                // tồn cuối
                AMOUNT_END += AMOUNT_BEGIN;
                TOTAL_PRICE_END += TOTAL_PRICE_BEGIN;

                ListRdo = ListRdo.OrderBy(s => s.IMP_EXP_TIME).ToList();
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        public void ProcessBegin()
        {
            // có kỳ kiểm kê trước đó
            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                var mediStockPeriod = listMediStockPeriods.OrderByDescending(s => s.CREATE_TIME).First();
                // thuốc
                if (IsNotNull(castFilter.MEDICINE_TYPE_ID))
                {
                    HisMestPeriodMediViewFilterQuery mestPeriodMediViewFilter = new HisMestPeriodMediViewFilterQuery();
                    mestPeriodMediViewFilter.MEDI_STOCK_PERIOD_ID = mediStockPeriod.ID;
                    var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediViewFilter);
                    listMestPeriodMedis = listMestPeriodMedis.Where(s => s.MEDICINE_TYPE_ID == castFilter.MEDICINE_TYPE_ID).ToList();
                    foreach (var listMestPeriodMedi in listMestPeriodMedis)
                    {
                        AMOUNT_BEGIN += listMestPeriodMedi.AMOUNT;
                        TOTAL_PRICE_BEGIN += listMestPeriodMedi.IMP_PRICE * (1 + listMestPeriodMedi.IMP_VAT_RATIO) * listMestPeriodMedi.AMOUNT;
                    }

                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_TIME_FROM = mediStockPeriod.CREATE_TIME + 1;
                    impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    var listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineViewFilter);
                    foreach (var listImpMestMedicine in listImpMestMedicines)
                    {
                        AMOUNT_BEGIN += listImpMestMedicine.AMOUNT;
                        TOTAL_PRICE_BEGIN += listImpMestMedicine.IMP_PRICE * (1 + listImpMestMedicine.IMP_VAT_RATIO) * listImpMestMedicine.AMOUNT;
                    }

                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_TIME_FROM = mediStockPeriod.CREATE_TIME + 1;
                    expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineViewFilter);
                    foreach (var listExpMestMedicine in listExpMestMedicines)
                    {
                        AMOUNT_BEGIN -= listExpMestMedicine.AMOUNT;
                        TOTAL_PRICE_BEGIN -= listExpMestMedicine.IMP_PRICE * (1 + listExpMestMedicine.IMP_VAT_RATIO) * listExpMestMedicine.AMOUNT;
                    }
                }
                // vật tư
                else if (IsNotNull(castFilter.MATERIAL_TYPE_ID))
                {
                    HisMestPeriodMateViewFilterQuery mestPeriodMateViewFilter = new HisMestPeriodMateViewFilterQuery();
                    mestPeriodMateViewFilter.MEDI_STOCK_PERIOD_ID = mediStockPeriod.ID;
                    var listMestPeriodMates = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateViewFilter);
                    listMestPeriodMates = listMestPeriodMates.Where(s => s.MATERIAL_TYPE_ID == castFilter.MATERIAL_TYPE_ID).ToList();
                    foreach (var listMestPeriodMate in listMestPeriodMates)
                    {
                        AMOUNT_BEGIN += listMestPeriodMate.AMOUNT;
                        TOTAL_PRICE_BEGIN += listMestPeriodMate.IMP_PRICE * (1 + listMestPeriodMate.IMP_VAT_RATIO) * listMestPeriodMate.AMOUNT;
                    }

                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_TIME_FROM = mediStockPeriod.CREATE_TIME + 1;
                    impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    var listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter);
                    foreach (var listImpMestMaterial in listImpMestMaterials)
                    {
                        AMOUNT_BEGIN += listImpMestMaterial.AMOUNT;
                        TOTAL_PRICE_BEGIN += listImpMestMaterial.IMP_PRICE * (1 + listImpMestMaterial.IMP_VAT_RATIO) * listImpMestMaterial.AMOUNT;
                    }

                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_TIME_FROM = mediStockPeriod.CREATE_TIME + 1;
                    expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    var listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMaterialViewFilter);
                    foreach (var listExpMestMaterial in listExpMestMaterials)
                    {
                        AMOUNT_BEGIN -= listExpMestMaterial.AMOUNT;
                        TOTAL_PRICE_BEGIN -= listExpMestMaterial.IMP_PRICE * (1 + listExpMestMaterial.IMP_VAT_RATIO) * listExpMestMaterial.AMOUNT;
                    }
                }
            }
            // không có kỳ kiểm kê nào trước đó
            else
            {
                // thuốc
                if (IsNotNull(castFilter.MEDICINE_TYPE_ID))
                {
                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    var listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMestMedicineViewFilter);
                    foreach (var listImpMestMedicine in listImpMestMedicines)
                    {
                        AMOUNT_BEGIN += listImpMestMedicine.AMOUNT;
                        TOTAL_PRICE_BEGIN += listImpMestMedicine.IMP_PRICE * (1 + listImpMestMedicine.IMP_VAT_RATIO) * listImpMestMedicine.AMOUNT;
                    }

                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expMestMedicineViewFilter.MEDICINE_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMestMedicineViewFilter);
                    foreach (var listExpMestMedicine in listExpMestMedicines)
                    {
                        AMOUNT_BEGIN -= listExpMestMedicine.AMOUNT;
                        TOTAL_PRICE_BEGIN -= listExpMestMedicine.IMP_PRICE * (1 + listExpMestMedicine.IMP_VAT_RATIO) * listExpMestMedicine.AMOUNT;
                    }
                }
                // vật tư
                else if (IsNotNull(castFilter.MATERIAL_TYPE_ID))
                {
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    impMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    var listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMestMaterialViewFilter);
                    foreach (var listImpMestMaterial in listImpMestMaterials)
                    {
                        AMOUNT_BEGIN += listImpMestMaterial.AMOUNT;
                        TOTAL_PRICE_BEGIN += listImpMestMaterial.IMP_PRICE * (1 + listImpMestMaterial.IMP_VAT_RATIO) * listImpMestMaterial.AMOUNT;
                    }

                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                    expMestMaterialViewFilter.MATERIAL_TYPE_ID = castFilter.MEDICINE_TYPE_ID;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    var listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMestMaterialViewFilter);
                    foreach (var listExpMestMaterial in listExpMestMaterials)
                    {
                        AMOUNT_BEGIN -= listExpMestMaterial.AMOUNT;
                        TOTAL_PRICE_BEGIN -= listExpMestMaterial.IMP_PRICE * (1 + listExpMestMaterial.IMP_VAT_RATIO) * listExpMestMaterial.AMOUNT;
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                if (IsNotNull(service))
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", service.MEDI_STOCK_NAME);
                    dicSingleTag.Add("SERVICE_TYPE_NAME", service.SERVICE_TYPE_NAME);
                    dicSingleTag.Add("SERVICE_TYPE_CODE", service.SERVICE_TYPE_CODE);
                    dicSingleTag.Add("SERVICE_UNIT_NAME", service.SERVICE_UNIT_NAME);
                    dicSingleTag.Add("NATIONAL_NAME", service.NATIONAL_NAME);

                    dicSingleTag.Add("AMOUNT_BEGIN", AMOUNT_BEGIN);
                    dicSingleTag.Add("TOTAL_PRICE_BEGIN", TOTAL_PRICE_BEGIN);
                    dicSingleTag.Add("AMOUNT_END", AMOUNT_END);
                    dicSingleTag.Add("TOTAL_PRICE_END", TOTAL_PRICE_END);
                }

                dicSingleTag.Add("CREATE_TIME", "Ngày " + (DateTime.Now.Day < 10 ? ("0" + DateTime.Now.Day) : DateTime.Now.Day.ToString()) + " tháng " + (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month) : DateTime.Now.Month.ToString()) + " năm " + DateTime.Now.Year);

                objectTag.AddObjectData(store, "Rdo", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
