using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00402
{
    class Mrs00402Processor : AbstractProcessor
    {
        Mrs00402Filter castFilter = null;

        List<Mrs00402RDO> ListRdo = new List<Mrs00402RDO>();
        List<Mrs00402RDO> ListRdoGroup = new List<Mrs00402RDO>();

        //List<V_HIS_MANU_IMP_MEST> listManuImpMests = new List<V_HIS_MANU_IMP_MEST>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        public string MEDI_STOCK_NAME;

        public Mrs00402Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00402Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00402Filter)this.reportFilter;

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                MEDI_STOCK_NAME = string.Join("; ", new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter).Select(o=>o.MEDI_STOCK_NAME).ToList());

                HisImpMestViewFilterQuery manuImpMestViewFilter = new HisImpMestViewFilterQuery();
                manuImpMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                manuImpMestViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                manuImpMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                manuImpMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                manuImpMestViewFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                var listManuImpMests = new HisImpMestManager(param).GetView(manuImpMestViewFilter);

                var skip = 0;
                while (listManuImpMests.Count - skip > 0)
                {
                    var listIds = listManuImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var impMedi = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                    if (IsNotNullOrEmpty(impMedi))
                    {
                        listImpMestMedicines.AddRange(impMedi);
                    }

                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var impMate = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                    if (IsNotNullOrEmpty(impMate))
                    {
                        listImpMestMaterials.AddRange(impMate);
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
                if (IsNotNullOrEmpty(listImpMestMedicines))
                {
                    foreach (var impMestMedicine in listImpMestMedicines)
                    {
                        var rdo = new Mrs00402RDO();
                        rdo.GROUP_ID = 1;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = impMestMedicine.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = impMestMedicine.MEDICINE_ID;
                        rdo.MANUFACTURER_NAME = impMestMedicine.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = impMestMedicine.NATIONAL_NAME;
                        rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                        rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                        rdo.EXPIRED_DATE = impMestMedicine.EXPIRED_DATE;
                        rdo.AMOUNT = impMestMedicine.AMOUNT;
                        rdo.IMP_PRICE = impMestMedicine.IMP_PRICE;
                        rdo.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                        rdo.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;
                        rdo.SUPPLIER_ID = impMestMedicine.SUPPLIER_ID;
                        rdo.IMP_TIME = impMestMedicine.IMP_TIME;

                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listImpMestMaterials))
                {
                    foreach (var impMestMaterial in listImpMestMaterials)
                    {
                        var rdo = new Mrs00402RDO();
                        rdo.GROUP_ID = 2;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = impMestMaterial.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = impMestMaterial.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = impMestMaterial.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = impMestMaterial.MATERIAL_ID;
                        rdo.MANUFACTURER_NAME = impMestMaterial.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = impMestMaterial.NATIONAL_NAME;
                        rdo.PACKAGE_NUMBER = impMestMaterial.PACKAGE_NUMBER;
                        rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                        rdo.EXPIRED_DATE = impMestMaterial.EXPIRED_DATE;
                        rdo.AMOUNT = impMestMaterial.AMOUNT;
                        rdo.IMP_PRICE = impMestMaterial.IMP_PRICE;
                        rdo.SUPPLIER_CODE = impMestMaterial.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = impMestMaterial.SUPPLIER_NAME;
                        rdo.SUPPLIER_ID = impMestMaterial.SUPPLIER_ID;
                        rdo.IMP_TIME = impMestMaterial.IMP_TIME;

                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    ListRdo = ListRdo.OrderBy(s => s.SERVICE_TYPE_NAME).ToList();
                    ListRdoGroup = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00402RDO { GROUP_ID = s.First().GROUP_ID, GROUP_NAME = s.First().GROUP_NAME }).ToList();
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
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoGroup", ListRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "Rdo", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
