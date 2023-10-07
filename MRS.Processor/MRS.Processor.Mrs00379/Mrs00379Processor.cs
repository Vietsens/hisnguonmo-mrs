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
using MOS.MANAGER.HisExpMestReason;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00379
{
    class Mrs00379Processor : AbstractProcessor
    {
        Mrs00379Filter castFilter = null;

        // xuất nhập tồn theo kho

        List<Mrs00379RDO> ListRdo = new List<Mrs00379RDO>();
        List<Mrs00379RDO> ListRdoMate = new List<Mrs00379RDO>();
        List<Mrs00379RDO> ListRdoMedi = new List<Mrs00379RDO>();

        List<Mrs00379RDO> ListRdoGroup = new List<Mrs00379RDO>();
        List<Mrs00379RDO> ListRdoServiceType = new List<Mrs00379RDO>();

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();

        List<HIS_MEDI_STOCK_PERIOD> listMediStockPeriods = new List<HIS_MEDI_STOCK_PERIOD>();

        List<EXP_MEST_REASON> listExpMestReasons = new List<EXP_MEST_REASON>();

        List<V_HIS_EXP_MEST> listOrtherExpMests = new List<V_HIS_EXP_MEST>();

        public string MEDI_STOCK_NAME = "";

        public static long MEDI_GROUP = 1;
        public static long MATE_GROUP = 2;

        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();

        public Mrs00379Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00379Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00379Filter)this.reportFilter;
                //Tao loai nhap xuat
                makeRdo();
                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilter);

                HisExpMestViewFilterQuery expMestViewFilter = new HisExpMestViewFilterQuery();
                expMestViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestViewFilter.FINISH_DATE_FROM = castFilter.TIME_FROM;
                expMestViewFilter.FINISH_DATE_TO = castFilter.TIME_TO;
                listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(expMestViewFilter);

                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

                HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestMedicineViewFilter.IS_EXPORT = true;
                listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

                HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestMaterialViewFilter.IS_EXPORT = true;
                listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);

                HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery();
                mediStockPeriodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodFilter);

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);

                if (IsNotNullOrEmpty(listMediStocks))
                    MEDI_STOCK_NAME = listMediStocks.First().MEDI_STOCK_NAME;

                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void makeRdo()
        {
            
            Dictionary<string, long> dicExpMestType = new Dictionary<string,long>();

            Dictionary<string, long> dicImpMestType = new Dictionary<string,long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00379RDO>();

            foreach (var item in piAmount)
            {
                if (dicImpMestType.ContainsKey(item.Name))
                {
                    if (!dicImpAmountType.ContainsKey(dicImpMestType[item.Name])) dicImpAmountType[dicImpMestType[item.Name]] = item;
                }
                else if (dicExpMestType.ContainsKey(item.Name))
                {
                    if (!dicExpAmountType.ContainsKey(dicExpMestType[item.Name])) dicExpAmountType[dicExpMestType[item.Name]] = item;
                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                // begin amount
                ProcessBeginAmount();
                // imp/exp
                ProcessImpExpMest();
                // end amount
                GetServiceType();
                foreach (var rdo in ListRdoMedi)
                {
                    var medicineType = listMedicineTypes.Where(w => w.ID == rdo.SERVICE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType))
                    {
                        rdo.PARENT_ID = IsNotNull(medicineType.First().PARENT_ID) ? medicineType.First().PARENT_ID.Value : 0;        // medicineType.First().ID
                        if (rdo.PARENT_ID != 0)
                            rdo.PARENT_NAME = listMedicineTypes.Where(s => s.ID == rdo.PARENT_ID).First().MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medicineType.First().CONCENTRA;
                    }
                }

                ListRdo.AddRange(ListRdoMedi);

                foreach (var rdo in ListRdoMate)
                {
                    var materialType = listMaterialTypes.Where(w => w.ID == rdo.SERVICE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(materialType))
                    {
                        rdo.PARENT_ID = IsNotNull(materialType.First().PARENT_ID) ? materialType.First().PARENT_ID.Value : 0;        // materialType.First().ID
                        if (rdo.PARENT_ID != 0)
                            rdo.PARENT_NAME = listMaterialTypes.Where(s => s.ID == rdo.PARENT_ID).First().MATERIAL_TYPE_NAME;
                    }
                }

                ListRdo.AddRange(ListRdoMate);

                // group
                ListRdoGroup = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00379RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME
                }).ToList();

                ListRdoServiceType = ListRdo.GroupBy(g => g.PARENT_ID).Select(s => new Mrs00379RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    PARENT_ID = s.First().PARENT_ID,
                    PARENT_NAME = s.First().PARENT_NAME
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public void ProcessBeginAmount()
        {
            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                var mediPeriod = listMediStockPeriods.OrderByDescending(s => s.CREATE_TIME).First();

                HisMestPeriodMediViewFilterQuery mestPeriodMediViewFilter = new HisMestPeriodMediViewFilterQuery();
                mestPeriodMediViewFilter.MEDI_STOCK_PERIOD_ID = mediPeriod.ID;
                var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(param).GetView(mestPeriodMediViewFilter);
                // tồn đầu thuốc
                if (IsNotNullOrEmpty(listMestPeriodMedis))
                {
                    foreach (var medi in listMestPeriodMedis)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MEDI_GROUP;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = medi.MEDICINE_ID;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = medi.AMOUNT;

                        ListRdoMedi.Add(rdo);
                    }
                }

                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMedicineViewFilter.IMP_TIME_FROM = mediPeriod.CREATE_TIME + 1;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMediImpMests = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

                if (IsNotNullOrEmpty(listMediImpMests))
                {
                    foreach (var medi in listMediImpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MEDI_GROUP;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = medi.MEDICINE_ID;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = medi.AMOUNT;

                        ListRdoMedi.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMedicineViewFilter.EXP_TIME_FROM = mediPeriod.CREATE_TIME + 1;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMedicineViewFilter.IS_EXPORT = true;
                var listMediExpMests = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

                if (IsNotNullOrEmpty(listMediExpMests))
                {
                    foreach (var medi in listMediExpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MEDI_GROUP;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = medi.MEDICINE_ID ?? 0;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = -medi.AMOUNT;

                        ListRdoMedi.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdoMedi);

                HisMestPeriodMateViewFilterQuery mestPeriodMateViewFilter = new HisMestPeriodMateViewFilterQuery();
                mestPeriodMateViewFilter.MEDI_STOCK_PERIOD_ID = mediPeriod.ID;
                var listMestPeriodMates = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(param).GetView(mestPeriodMateViewFilter);
                // tồn đầu vật tư
                if (IsNotNullOrEmpty(listMestPeriodMates))
                {
                    foreach (var mate in listMestPeriodMates)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MATE_GROUP;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = mate.MATERIAL_ID;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = mate.AMOUNT;

                        ListRdoMate.Add(rdo);
                    }
                }

                HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMaterialViewFilter.IMP_TIME_FROM = mediPeriod.CREATE_TIME + 1;
                impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMateImpMests = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);

                if (IsNotNullOrEmpty(listMateImpMests))
                {
                    foreach (var mate in listMateImpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MATE_GROUP;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = mate.MATERIAL_ID;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = mate.AMOUNT;

                        ListRdoMate.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMaterialViewFilter.EXP_TIME_FROM = mediPeriod.CREATE_TIME + 1;
                expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMaterialViewFilter.IS_EXPORT = true;
                var listMateExpMests = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);

                if (IsNotNullOrEmpty(listMateExpMests))
                {
                    foreach (var mate in listMateExpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MATE_GROUP;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = mate.MATERIAL_ID ?? 0;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = -mate.AMOUNT;

                        ListRdoMate.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdoMate);
            }
            else
            {
                // thuốc
                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMediImpMests = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

                if (IsNotNullOrEmpty(listMediImpMests))
                {
                    foreach (var medi in listMediImpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MEDI_GROUP;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = medi.MEDICINE_ID;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = medi.AMOUNT;

                        ListRdoMedi.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMedicineViewFilter.IS_EXPORT = true;
                var listMediExpMests = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

                if (IsNotNullOrEmpty(listMediExpMests))
                {
                    foreach (var medi in listMediExpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MEDI_GROUP;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_ID = medi.MEDICINE_ID ?? 0;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = -medi.AMOUNT;

                        ListRdoMedi.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdoMedi);

                // vật tư
                HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMateImpMests = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);

                if (IsNotNullOrEmpty(listMateImpMests))
                {
                    foreach (var mate in listMateImpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MATE_GROUP;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = mate.MATERIAL_ID;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = mate.AMOUNT;

                        ListRdoMate.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMaterialViewFilter.IS_EXPORT = true;
                var listMateExpMests = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);

                if (IsNotNullOrEmpty(listMateExpMests))
                {
                    foreach (var mate in listMateExpMests)
                    {
                        var rdo = new Mrs00379RDO();
                        rdo.GROUP_ID = MATE_GROUP;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_ID = mate.MATERIAL_ID ?? 0;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = -mate.AMOUNT;

                        ListRdoMate.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdoMate);
            }
        }

        public void ProcessImpExpMest()
        {
            ProcessExpMestReason();

            // thuốc
            HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
            impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            var listMediImpMests = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

            if (IsNotNullOrEmpty(listMediImpMests))
            {
                foreach (var medi in listMediImpMests)
                {
                    var rdo = new Mrs00379RDO();
                    rdo.GROUP_ID = MEDI_GROUP;
                    rdo.GROUP_NAME = "THUỐC";
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                    rdo.SERVICE_ID = medi.MEDICINE_ID;
                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = medi.IMP_PRICE;
                    if (dicImpAmountType.ContainsKey(medi.IMP_MEST_TYPE_ID))
                        dicImpAmountType[medi.IMP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                    ListRdoMedi.Add(rdo);
                }
            }

            HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
            expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMedicineViewFilter.IS_EXPORT = true;
            var listMediExpMests = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

            if (IsNotNullOrEmpty(listMediExpMests))
            {
                foreach (var medi in listMediExpMests)
                {
                    var rdo = new Mrs00379RDO();
                    rdo.GROUP_ID = MEDI_GROUP;
                    rdo.GROUP_NAME = "THUỐC";
                    rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                    rdo.SERVICE_ID = medi.MEDICINE_ID ?? 0;
                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = medi.IMP_PRICE;
                    if (dicExpAmountType.ContainsKey(medi.EXP_MEST_TYPE_ID))
                        dicExpAmountType[medi.EXP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                    var other = listOrtherExpMests.Where(w => w.ID == medi.EXP_MEST_ID).ToList();
                    if (IsNotNullOrEmpty(other))
                    {
                        if (other.First().EXP_MEST_REASON_ID == null)
                            rdo.EXP_MEST_REASON_0 = medi.AMOUNT;
                        else
                        {
                            try
                            {
                                var reason = listExpMestReasons.Where(s => s.EXP_MEST_REASON_ID == other.First().EXP_MEST_REASON_ID).ToList();
                                if (IsNotNullOrEmpty(reason))
                                {
                                    System.Reflection.PropertyInfo piReason = typeof(Mrs00379RDO).GetProperty(reason.First().EXP_MEST_REASON_TAG);
                                    piReason.SetValue(rdo, Convert.ToDecimal(medi.AMOUNT));
                                }
                            }
                            catch { }
                        }
                    }
                    ListRdoMedi.Add(rdo);
                }
            }

            ProcessOptimizeList(ref ListRdoMedi);

            // vật tư
            HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
            impMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            var listMateImpMests = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);

            if (IsNotNullOrEmpty(listMateImpMests))
            {
                foreach (var mate in listMateImpMests)
                {
                    var rdo = new Mrs00379RDO();
                    rdo.GROUP_ID = MATE_GROUP;
                    rdo.GROUP_NAME = "VẬT TƯ";
                    rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                    rdo.SERVICE_ID = mate.MATERIAL_ID;
                    rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = mate.IMP_PRICE;

                    if (dicExpAmountType.ContainsKey(mate.IMP_MEST_TYPE_ID))
                        dicExpAmountType[mate.IMP_MEST_TYPE_ID].SetValue(rdo, mate.AMOUNT);

                    ListRdoMedi.Add(rdo);
                }
            }

            HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
            expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMaterialViewFilter.IS_EXPORT = true;
            var listMateExpMests = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);

            if (IsNotNullOrEmpty(listMateExpMests))
            {
                foreach (var mate in listMateExpMests)
                {
                    var rdo = new Mrs00379RDO();
                    rdo.GROUP_ID = MATE_GROUP;
                    rdo.GROUP_NAME = "VẬT TƯ";
                    rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                    rdo.SERVICE_ID = mate.MATERIAL_ID ?? 0;
                    rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = mate.IMP_PRICE;

                    if (dicExpAmountType.ContainsKey(mate.EXP_MEST_TYPE_ID))
                        dicExpAmountType[mate.EXP_MEST_TYPE_ID].SetValue(rdo, mate.AMOUNT);

                    var other = listOrtherExpMests.Where(w => w.ID == mate.EXP_MEST_ID).ToList();
                    if (IsNotNullOrEmpty(other))
                    {
                        if (other.First().EXP_MEST_REASON_ID == null)
                            rdo.EXP_MEST_REASON_0 = mate.AMOUNT;
                        else
                        {
                            try
                            {
                                var reason = listExpMestReasons.Where(s => s.EXP_MEST_REASON_ID == other.First().EXP_MEST_REASON_ID).ToList();
                                if (IsNotNullOrEmpty(reason))
                                {
                                    System.Reflection.PropertyInfo piDistrict = typeof(Mrs00379RDO).GetProperty(reason.First().EXP_MEST_REASON_TAG);
                                    piDistrict.SetValue(rdo, Convert.ToDecimal(mate.AMOUNT));
                                }
                            }
                            catch { }
                        }
                    }

                    ListRdoMedi.Add(rdo);
                }
            }

            ProcessOptimizeList(ref ListRdoMate);
        }

        public void ProcessExpMestReason()
        {
            if (IsNotNullOrEmpty(listExpMests))
            {
                listOrtherExpMests = listExpMests.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC).ToList();
            }

            var listExpMestReason = new List<HIS_EXP_MEST_REASON>();
            listExpMestReason.AddRange(new MOS.MANAGER.HisExpMestReason.HisExpMestReasonManager(param).Get(new HisExpMestReasonFilterQuery()));

            listExpMestReasons.Add(new EXP_MEST_REASON() { EXP_MEST_REASON_ID = 0, EXP_MEST_REASON_NAME = "Không xác định", EXP_MEST_REASON_TAG = "EXP_MEST_REASON_0" });
            int stt = 1;
            foreach (var reason in listExpMestReason.OrderBy(s => s.ID).ToList())
            {
                listExpMestReasons.Add(new EXP_MEST_REASON() { EXP_MEST_REASON_ID = reason.ID, EXP_MEST_REASON_NAME = reason.EXP_MEST_REASON_NAME, EXP_MEST_REASON_TAG = "EXP_MEST_REASON_" + stt });
                stt++;
            }
        }

        public void GetServiceType()
        {
            List<long> listIDs = ListRdoMedi.Select(s => s.SERVICE_TYPE_ID).ToList();
            HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery();
            medicineTypeViewFilter.IDs = listIDs.Distinct().ToList();
            listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter);

            var listParentMedis = listMedicineTypes.Where(w => w.PARENT_ID != null).ToList();
            var skip = 0;
            while (listParentMedis.Count - skip > 0)
            {
                var listIds = listParentMedis.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisMedicineTypeViewFilterQuery medicineTypeViewFilter2 = new HisMedicineTypeViewFilterQuery();
                medicineTypeViewFilter.IDs = listIds.Select(s => s.PARENT_ID.Value).ToList();
                listMedicineTypes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter2));
            }
            listMedicineTypes = listMedicineTypes.Distinct().ToList();

            listIDs = ListRdoMate.Select(s => s.SERVICE_TYPE_ID).ToList();
            HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
            materialTypeViewFilter.IDs = listIDs.Distinct().ToList();
            listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter);

            var listParentMates = listMaterialTypes.Where(w => w.PARENT_ID != null).ToList();
            skip = 0;
            while (listParentMates.Count - skip > 0)
            {
                var listIds = listParentMates.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                HisMaterialTypeViewFilterQuery materialTypeViewFilter2 = new HisMaterialTypeViewFilterQuery();
                materialTypeViewFilter.IDs = listIds.Select(s => s.PARENT_ID.Value).ToList();
                listMaterialTypes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter2));
            }
            listMaterialTypes = listMaterialTypes.Distinct().ToList();
        }

        public void ProcessOptimizeList(ref List<Mrs00379RDO> _listRdo)
        {
            var group = _listRdo.GroupBy(g => new { g.SERVICE_ID }).ToList();
            _listRdo.Clear();
            Decimal sum = 0;
            Mrs00379RDO rdo;
            List<Mrs00379RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00379RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00379RDO();
                listSub = item.ToList<Mrs00379RDO>();

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("_AMOUNT") || field.Name.Contains("EXP_MEST_REASON_"))
                    {
                        sum = listSub.Sum(s => (decimal?)field.GetValue(s) ?? 0);
                        if (hide && sum > 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00379RDO()));
                    }
                }
                if (!hide) _listRdo.Add(rdo);
            }
        }

        private bool IsMeaningful(object p)
        {
            return (IsNotNull(p) && p.ToString() != "0" && p.ToString() != "");
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

                listExpMestReasons = listExpMestReasons.OrderBy(s => s.EXP_MEST_REASON_ID).ToList();
                int stt = 0;
                foreach (var reason in listExpMestReasons)
                {
                    dicSingleTag.Add("EXP_MEST_REASON_ID_" + stt, reason.EXP_MEST_REASON_ID);
                    dicSingleTag.Add("EXP_MEST_REASON_NAME_" + stt, reason.EXP_MEST_REASON_NAME);
                    stt++;
                }

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoGroup", ListRdoGroup.OrderBy(s => s.GROUP_ID).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoServiceType", ListRdoServiceType.OrderBy(s => s.PARENT_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.SERVICE_TYPE_NAME).ToList());

                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "RdoServiceType", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoServiceType", "Rdo", "PARENT_ID", "PARENT_ID");

                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
