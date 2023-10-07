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
using IMSys.DbConfig;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using System.Reflection;
using Inventec.Common.Repository;
using Inventec.Common.Logging;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;
using MOS.MANAGER.HisMedicineUseForm;

namespace MRS.Processor.Mrs00327
{
    class Mrs00327Processor : AbstractProcessor
    {

        Mrs00327Filter castFilter = null;
        List<long> listExpMestMove = new List<long>() 
        {
          /*684771,  */605969,593816,383246,380830,378231,377217,374939,374876,372104,370201,369962,365553,365491,353912,349558,344577,344362,337381,336518,329607,329388,329381,325142,322403,322397,321191,320116,316357,315724,315085,313928,300407,294614,294552,287858,285094,261462,258352,258347,242701,241711,227424,218755,211827,189169,172968,172677,170738,165842,160956,157002,156985,138729,130956,130837,130827,128356,128323,122696,112144,108074,95629,87615,81493,80373,66324,65514,63627,60345,56643,55631,52127,51363,49757,49742,45058,45027,534128,88318,138366,101659
};
        List<Mrs00327RDO> ListRdo = new List<Mrs00327RDO>();
        List<Mrs00327RDO> ListRdoParent = new List<Mrs00327RDO>();
        List<Mrs00327RDO> ListRdoGroup = new List<Mrs00327RDO>();

        List<Mrs00327RDO> listRdoMedicines = new List<Mrs00327RDO>();
        List<Mrs00327RDO> listRdoMaterials = new List<Mrs00327RDO>();
        List<Mrs00327RDO> listRdoChemicals = new List<Mrs00327RDO>();

        List<HIS_MEDI_STOCK> listMediStockNotBusis = new List<HIS_MEDI_STOCK>();
        List<HIS_MEDI_STOCK> listMediStockBusis = new List<HIS_MEDI_STOCK>();

        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypeParents = new List<V_HIS_MEDICINE_TYPE>();
        List<HIS_MATERIAL_TYPE> listMaterialTypeParents = new List<HIS_MATERIAL_TYPE>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypeLeafs = new List<V_HIS_MEDICINE_TYPE>();
        List<HIS_MATERIAL_TYPE> listMaterialTypeLeafs = new List<HIS_MATERIAL_TYPE>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineBeforeAlls = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<Mrs00327RDO> listExpMestMedicineBeforeAlls = new List<Mrs00327RDO>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialBeforeAlls = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<Mrs00327RDO> listExpMestMaterialBeforeAlls = new List<Mrs00327RDO>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicineOnAlls = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOnAlls = new List<V_HIS_EXP_MEST_MEDICINE>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterialOnAlls = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOnAlls = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_IMP_MEST> listImpMestOnAlls = new List<V_HIS_IMP_MEST>();
        List<HIS_EXP_MEST> listExpMestHasMobasOrChmAlls = new List<HIS_EXP_MEST>();

        List<V_HIS_EXP_MEST> listChmsExpMestAlls = new List<V_HIS_EXP_MEST>();

        List<HIS_MEDI_STOCK_PERIOD> listMediStockPeriodAlls = new List<HIS_MEDI_STOCK_PERIOD>();

        List<HIS_MEDICINE_USE_FORM> ListMedicineUseForm = new List<HIS_MEDICINE_USE_FORM>();
        List<long> filterMedicineTypeIds = new List<long>();
        List<long> filterMaterialTypeIds = new List<long>();
        public const int SERVICE_GROUP_ID__MEDI = 1;
        public const int SERVICE_GROUP_ID__MATE = 2;
        public const int SERVICE_GROUP_ID__CHEM = 3;
        public const int LIMIT = 2147483646;

        public bool IS_MEDICINE = false;
        public bool IS_MATERIAL = false;
        public bool IS_CHEMICAL = false;

        List<long> CHMS_EXP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BCS,
IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };

        List<long> CHMS_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS,
IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BL
        };

        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
        };

        private List<long> EXP_MEST_PRES = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT
        };

        private List<long> IMP_MEST_PRES = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL
        };

        public string SERVICE_GROUP_NAME_CHOICE = "";

        public Mrs00327Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00327Filter);
        }

        protected override bool GetData()///
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00327Filter)this.reportFilter;
                //================================================================================================================
                //Tao cau hinh kho khong lay
                var config = Loader.dictionaryConfig.ContainsKey("HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE_NOT_INPUT") ? Loader.dictionaryConfig["HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE_NOT_INPUT"] : null;
                if (config == null) throw new ArgumentNullException("HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE_NOT_INPUT");
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;

                var listMediStockNotInPutCode = value.Split(',');
                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                var listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);
                if (IsNotNullOrEmpty(listMediStockNotInPutCode))
                {
                    listMediStocks = listMediStocks.Where(o => !listMediStockNotInPutCode.Contains(o.MEDI_STOCK_CODE)).ToList();
                }
                //kho khong nha thuoc
                listMediStockNotBusis = listMediStocks.Where(w => w.IS_BUSINESS != 1).ToList();
                //kho nha thuoc
                listMediStockBusis = listMediStocks.Where(w => w.IS_BUSINESS == 1).ToList();

                //Tao loai nhap xuat
                makeRdo();
                //================================================================================================================
                if (castFilter.IS_MEDICINE)
                {
                    this.IS_MEDICINE = true;
                    SERVICE_GROUP_NAME_CHOICE = "Thuốc";
                }
                if (castFilter.IS_MATERIAL)
                {
                    this.IS_MATERIAL = true;
                    SERVICE_GROUP_NAME_CHOICE = ", Vật tư";
                }
                if (castFilter.IS_CHEMICAL_SUBSTANCE)
                {
                    this.IS_CHEMICAL = true;
                    SERVICE_GROUP_NAME_CHOICE = ", Hóa chất";
                }
                if ((castFilter.IS_CHEMICAL_SUBSTANCE && castFilter.IS_MATERIAL && castFilter.IS_MEDICINE) || ((!castFilter.IS_CHEMICAL_SUBSTANCE) && (!castFilter.IS_MATERIAL) && (!castFilter.IS_MEDICINE)))
                {
                    SERVICE_GROUP_NAME_CHOICE = "Tất cả";
                    this.IS_MEDICINE = true;
                    this.IS_MATERIAL = true;
                    this.IS_CHEMICAL = true;
                }

                #region Vùng get dữ liệu tổng hợp

                HisMedicineUseFormFilterQuery medicineUseFormFilter = new HisMedicineUseFormFilterQuery();
                //medicineTypeFilter.IS_LEAF = false;
                ListMedicineUseForm = new MOS.MANAGER.HisMedicineUseForm.HisMedicineUseFormManager(param).Get(medicineUseFormFilter);

                //================================================================================================================ Type
                if (this.IS_MEDICINE)
                {
                    HisMedicineTypeViewFilterQuery medicineTypeFilter = new HisMedicineTypeViewFilterQuery();
                    //medicineTypeFilter.IS_LEAF = false;
                    var listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeFilter);
                    Inventec.Common.Logging.LogSystem.Info("listMedicineTypes" + listMedicineTypes.Count);

                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {

                        listMedicineTypes = listMedicineTypes.Where(o => string.Format(",{0},", this.castFilter.MEDICINE_TYPE_CODEs).Contains(string.Format(",{0},", o.MEDICINE_TYPE_CODE))).ToList();
                        filterMedicineTypeIds = listMedicineTypes.Select(o => o.ID).ToList();
                    }

                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {

                        listMedicineTypes = listMedicineTypes.Where(o => this.castFilter.MEDICINE_TYPE_IDs.Contains(o.ID)).ToList();
                        filterMedicineTypeIds = listMedicineTypes.Select(o => o.ID).ToList();
                    }
                    listMedicineTypeParents = listMedicineTypes.Where(o => o.IS_LEAF != 1).ToList();
                    listMedicineTypeLeafs = listMedicineTypes.Where(o => o.IS_LEAF == 1).ToList();
                }
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    HisMaterialTypeFilterQuery materialTypeFilter = new HisMaterialTypeFilterQuery();
                    //materialTypeFilter.IS_LEAF = false;
                    var listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).Get(materialTypeFilter);
                    Inventec.Common.Logging.LogSystem.Info("listMaterialTypes" + listMaterialTypes.Count);

                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listMaterialTypes = listMaterialTypes.Where(o => string.Format(",{0},", this.castFilter.MATERIAL_TYPE_CODEs).Contains(string.Format(",{0},", o.MATERIAL_TYPE_CODE))).ToList();
                        filterMaterialTypeIds = listMaterialTypes.Select(o => o.ID).ToList();
                    }

                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listMaterialTypes = listMaterialTypes.Where(o => this.castFilter.MATERIAL_TYPE_IDs.Contains(o.ID)).ToList();
                        filterMaterialTypeIds = listMaterialTypes.Select(o => o.ID).ToList();
                    }
                    listMaterialTypeParents = listMaterialTypes.Where(o => o.IS_LEAF != 1).ToList();
                    listMaterialTypeLeafs = listMaterialTypes.Where(o => o.IS_LEAF == 1).ToList();
                }
                //================================================================================================================Phieu chuyen
                var listExpMestMedicineMove = new List<V_HIS_EXP_MEST_MEDICINE>();
                var listExpMestMaterialMove = new List<V_HIS_EXP_MEST_MATERIAL>();
                HisImpMestFilterQuery HisImpMestfilter = new HisImpMestFilterQuery();
                HisImpMestfilter.CHMS_EXP_MEST_IDs = listExpMestMove;
                HisImpMestfilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                var listImpMestMove = new HisImpMestManager(param).Get(HisImpMestfilter);
                if (listImpMestMove != null)
                {
                    listExpMestMove = listExpMestMove.Where(o => !listImpMestMove.Exists(p => p.CHMS_EXP_MEST_ID == o)).ToList();
                }
                if (this.IS_MEDICINE)
                {
                    HisExpMestMedicineViewFilterQuery HisExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
                    HisExpMestMedicinefilter.EXP_MEST_IDs = listExpMestMove;
                    HisExpMestMedicinefilter.EXP_MEST_TYPE_IDs = CHMS_EXP_MEST_TYPE_IDs;
                    HisExpMestMedicinefilter.IS_EXPORT = true;
                    listExpMestMedicineMove = new HisExpMestMedicineManager(param).GetView(HisExpMestMedicinefilter);
                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMedicineMove = listExpMestMedicineMove.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {
                        listExpMestMedicineMove = listExpMestMedicineMove.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                }
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    HisExpMestMaterialViewFilterQuery HisExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
                    HisExpMestMaterialfilter.EXP_MEST_IDs = listExpMestMove;
                    HisExpMestMaterialfilter.IS_EXPORT = true;
                    HisExpMestMaterialfilter.EXP_MEST_TYPE_IDs = CHMS_EXP_MEST_TYPE_IDs;
                    listExpMestMaterialMove = new HisExpMestMaterialManager(param).GetView(HisExpMestMaterialfilter);
                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMaterialMove = listExpMestMaterialMove.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listExpMestMaterialMove = listExpMestMaterialMove.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                }
                //================================================================================================================Truoc ki
                if (this.IS_MEDICINE)
                {
                    CommonParam imMedicineParam = new CommonParam();
                    imMedicineParam.Start = 0;
                    imMedicineParam.Limit = LIMIT;
                    int iMe = 0;
                    do
                    {
                        iMe++;
                        HisImpMestMedicineViewFilterQuery impMestMedicineBeforeAllViewFilter = new HisImpMestMedicineViewFilterQuery();
                        impMestMedicineBeforeAllViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                        impMestMedicineBeforeAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        impMestMedicineBeforeAllViewFilter.ORDER_DIRECTION = "ASC";
                        impMestMedicineBeforeAllViewFilter.ORDER_FIELD = "ID";
                        impMestMedicineBeforeAllViewFilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                        var listImpMestMedicineBefore = new HisImpMestMedicineManager(imMedicineParam).GetView(impMestMedicineBeforeAllViewFilter);
                        listImpMestMedicineBeforeAlls.AddRange(listImpMestMedicineBefore);
                        Inventec.Common.Logging.LogSystem.Info("listImpMestMedicineBeforeAlls" + listImpMestMedicineBeforeAlls.Count);
                        imMedicineParam.Start += listImpMestMedicineBefore.Count;
                    }
                    while ((LIMIT * iMe) == imMedicineParam.Start);
                    listImpMestMedicineBeforeAlls = listImpMestMedicineBeforeAlls.Where(o => !CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {
                        listImpMestMedicineBeforeAlls = listImpMestMedicineBeforeAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }

                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {
                        listImpMestMedicineBeforeAlls = listImpMestMedicineBeforeAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                    //Xuất trước kỳ
                    HisExpMestMedicineViewFilterQuery expMestMedicineBeforeAllViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineBeforeAllViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMedicineBeforeAllViewFilter.IS_EXPORT = true;
                    expMestMedicineBeforeAllViewFilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                    listExpMestMedicineBeforeAlls = new ManagerSql().Get(expMestMedicineBeforeAllViewFilter);
                    Inventec.Common.Logging.LogSystem.Info("listExpMestMedicineBeforeAlls" + listExpMestMedicineBeforeAlls.Count);

                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMedicineBeforeAlls = listExpMestMedicineBeforeAlls.Where(o => filterMedicineTypeIds.Contains(o.SERVICE_TYPE_ID)).ToList();
                    }

                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {
                        listExpMestMedicineBeforeAlls = listExpMestMedicineBeforeAlls.Where(o => filterMedicineTypeIds.Contains(o.SERVICE_TYPE_ID)).ToList();
                    }
                    if (listExpMestMedicineMove != null)
                    {
                        foreach (var item in listExpMestMedicineMove.Where(o => o.EXP_TIME < castFilter.TIME_FROM).ToList())
                        {
                            Mrs00327RDO rdo = new Mrs00327RDO();
                            rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MEDI;
                            rdo.SERVICE_GROUP_NAME = "THUỐC";

                            rdo.SERVICE_TYPE_ID = item.MEDICINE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                            rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = item.CONCENTRA;
                            rdo.NATIONAL_NAME = item.NATIONAL_NAME;
                            rdo.REGISTER_NUMBER = item.REGISTER_NUMBER;
                            rdo.TUTORIAL = item.TUTORIAL;

                            var me = listMedicineTypeLeafs.FirstOrDefault(o => o.ID == item.MEDICINE_TYPE_ID);
                            if (me != null)
                            {
                                rdo.ACTIVE_INGR_BHYT_CODE = me.ACTIVE_INGR_BHYT_CODE;
                                rdo.ACTIVE_INGR_BHYT_NAME = me.ACTIVE_INGR_BHYT_NAME;
                                rdo.CONCENTRA = me.CONCENTRA;
                                rdo.NATIONAL_NAME = me.NATIONAL_NAME;
                                rdo.REGISTER_NUMBER = me.REGISTER_NUMBER;
                                rdo.TUTORIAL = me.TUTORIAL;
                                rdo.DOSAGE_FORM = me.DOSAGE_FORM;
                                rdo.MANUFACTURER_NAME = me.MANUFACTURER_NAME;
                                rdo.PACKING_TYPE_NAME = me.PACKING_TYPE_NAME;
                                rdo.MEDICINE_GROUP_NAME = me.MEDICINE_GROUP_NAME;
                                var useForm = ListMedicineUseForm.FirstOrDefault(o => o.ID == me.MEDICINE_USE_FORM_ID);
                                if (useForm != null)
                                {
                                    rdo.MEDICINE_USE_FORM_NAME = useForm.MEDICINE_USE_FORM_NAME;
                                }
                            }

                            rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
                            rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                            rdo.BEGIN_AMOUNT = -(item.AMOUNT);
                            listExpMestMedicineBeforeAlls.Add(rdo);
                        }
                    }
                }
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    CommonParam imMaterialParam = new CommonParam();
                    imMaterialParam.Start = 0;
                    imMaterialParam.Limit = LIMIT;
                    int iMa = 0;
                    do
                    {
                        iMa++;
                        HisImpMestMaterialViewFilterQuery impMestMaterialBeforeAllViewFilter = new HisImpMestMaterialViewFilterQuery();
                        impMestMaterialBeforeAllViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                        impMestMaterialBeforeAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        impMestMaterialBeforeAllViewFilter.ORDER_DIRECTION = "ASC";
                        impMestMaterialBeforeAllViewFilter.ORDER_FIELD = "ID";
                        impMestMaterialBeforeAllViewFilter.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                        var listImpMestMaterialBefore = new HisImpMestMaterialManager(imMaterialParam).GetView(impMestMaterialBeforeAllViewFilter);
                        listImpMestMaterialBeforeAlls.AddRange(listImpMestMaterialBefore);
                        Inventec.Common.Logging.LogSystem.Info("listImpMestMaterialBeforeAlls" + listImpMestMaterialBeforeAlls.Count);
                        imMaterialParam.Start += listImpMestMaterialBefore.Count;
                    }
                    while ((LIMIT * iMa) == imMaterialParam.Start);
                    listImpMestMaterialBeforeAlls = listImpMestMaterialBeforeAlls.Where(o => !CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listImpMestMaterialBeforeAlls = listImpMestMaterialBeforeAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }

                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listImpMestMaterialBeforeAlls = listImpMestMaterialBeforeAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                    HisExpMestMaterialViewFilterQuery expMestMaterialBeforeAllViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialBeforeAllViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMaterialBeforeAllViewFilter.IS_EXPORT = true;
                    expMestMaterialBeforeAllViewFilter.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                    listExpMestMaterialBeforeAlls = new ManagerSql().Get(expMestMaterialBeforeAllViewFilter);

                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMaterialBeforeAlls = listExpMestMaterialBeforeAlls.Where(o => filterMaterialTypeIds.Contains(o.SERVICE_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listExpMestMaterialBeforeAlls = listExpMestMaterialBeforeAlls.Where(o => filterMaterialTypeIds.Contains(o.SERVICE_TYPE_ID)).ToList();
                    }
                    if (listExpMestMaterialMove != null)
                    {
                        foreach (var item in listExpMestMaterialMove.Where(o => o.EXP_TIME < castFilter.TIME_FROM).ToList())
                        {
                            Mrs00327RDO rdo = new Mrs00327RDO();

                            rdo.SERVICE_TYPE_ID = item.MATERIAL_TYPE_ID;
                            var materialType = listMaterialTypeLeafs.Where(s => s.ID == rdo.SERVICE_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(materialType))
                            {
                                rdo.IS_STENT = materialType.First().IS_STENT == 1 ? "STENT" : "NO_STENT";
                            }
                            rdo.SERVICE_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                            rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                            rdo.NATIONAL_NAME = item.NATIONAL_NAME;
                            rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
                            rdo.IMP_PRICE = item.IMP_PRICE * (1 + item.IMP_VAT_RATIO);
                            rdo.BEGIN_AMOUNT = -(item.AMOUNT);
                            listExpMestMaterialBeforeAlls.Add(rdo);
                        }
                    }
                }

                //================================================================================================================Nhap Trong ki
                if (this.IS_MEDICINE)
                {
                    HisImpMestMedicineViewFilterQuery impMestMedicineOnAllViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineOnAllViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMestMedicineOnAllViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMestMedicineOnAllViewFilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                    impMestMedicineOnAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    listImpMestMedicineOnAlls = new HisImpMestMedicineManager(param).GetView(impMestMedicineOnAllViewFilter);
                    Inventec.Common.Logging.LogSystem.Info("listImpMestMedicineOnAlls" + listImpMestMedicineOnAlls.Count);
                    listImpMestMedicineOnAlls = listImpMestMedicineOnAlls.Where(o => !CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {
                        listImpMestMedicineOnAlls = listImpMestMedicineOnAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {
                        listImpMestMedicineOnAlls = listImpMestMedicineOnAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                }
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    HisImpMestMaterialViewFilterQuery impMestMaterialOnAllViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialOnAllViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMestMaterialOnAllViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMestMaterialOnAllViewFilter.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                    impMestMaterialOnAllViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    listImpMestMaterialOnAlls = new HisImpMestMaterialManager(param).GetView(impMestMaterialOnAllViewFilter);
                    listImpMestMaterialOnAlls = listImpMestMaterialOnAlls.Where(o => !CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                    Inventec.Common.Logging.LogSystem.Info("listImpMestMaterialOnAlls" + listImpMestMaterialOnAlls.Count);

                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listImpMestMaterialOnAlls = listImpMestMaterialOnAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listImpMestMaterialOnAlls = listImpMestMaterialOnAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                }
                List<long> listImpMestId = new List<long>();
                if (listImpMestMedicineOnAlls != null)
                {
                    listImpMestId.AddRange(listImpMestMedicineOnAlls.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)
                        || MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).Select(s => s.IMP_MEST_ID).ToList());
                }
                if (listImpMestMaterialOnAlls != null)
                {
                    listImpMestId.AddRange(listImpMestMaterialOnAlls.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)
                        || MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).Select(s => s.IMP_MEST_ID).ToList());
                }
                if (IsNotNullOrEmpty(listImpMestId))
                {
                    listImpMestId = listImpMestId.Distinct().ToList();
                    var skip = 0;

                    while (listImpMestId.Count - skip > 0)
                    {
                        var listIds = listImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                        mobaImpMestViewFilter.IDs = listIds;
                        listImpMestOnAlls.AddRange(new HisImpMestManager(param).GetView(mobaImpMestViewFilter));
                    }
                    Inventec.Common.Logging.LogSystem.Info("listImpMestOnAlls" + listImpMestOnAlls.Count);
                    skip = 0;
                    List<long> listExpMestHasMobasOrChmId = new List<long>();
                    if (listImpMestOnAlls != null)
                    {
                        listExpMestHasMobasOrChmId.AddRange(listImpMestOnAlls.Select(s => s.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList());
                    }
                    if (listImpMestOnAlls != null)
                    {
                        listExpMestHasMobasOrChmId.AddRange(listImpMestOnAlls.Select(s => s.CHMS_EXP_MEST_ID ?? 0).Distinct().ToList());
                    }
                    while (listExpMestHasMobasOrChmId.Count - skip > 0)
                    {
                        listExpMestHasMobasOrChmId = listExpMestHasMobasOrChmId.Distinct().ToList();
                        var listIds = listExpMestHasMobasOrChmId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                        expMestFilter.IDs = listIds;
                        listExpMestHasMobasOrChmAlls.AddRange(new HisExpMestManager(param).Get(expMestFilter));
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("listExpMestHasMobasOrChmAlls" + listExpMestHasMobasOrChmAlls.Count);
                //================================================================================================================Xuat Trong ki
                if (this.IS_MEDICINE)
                {
                    HisExpMestMedicineViewFilterQuery expMestMedicineOnAllViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineOnAllViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMedicineOnAllViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMedicineOnAllViewFilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                    expMestMedicineOnAllViewFilter.IS_EXPORT = true;
                    listExpMestMedicineOnAlls = new HisExpMestMedicineManager(param).GetView(expMestMedicineOnAllViewFilter);
                    listExpMestMedicineOnAlls = listExpMestMedicineOnAlls.Where(o => !CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                    Inventec.Common.Logging.LogSystem.Info("listExpMestMedicineOnAlls" + listExpMestMedicineOnAlls.Count);
                    if (listExpMestMedicineMove != null)
                    {
                        listExpMestMedicineOnAlls.AddRange(listExpMestMedicineMove.Where(o => o.EXP_TIME < castFilter.TIME_TO && o.EXP_TIME >= castFilter.TIME_FROM).ToList());
                    }
                    if (this.castFilter.MEDICINE_TYPE_CODEs != null && this.castFilter.MEDICINE_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMedicineOnAlls = listExpMestMedicineOnAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MEDICINE_TYPE_IDs != null)
                    {
                        listExpMestMedicineOnAlls = listExpMestMedicineOnAlls.Where(o => filterMedicineTypeIds.Contains(o.MEDICINE_TYPE_ID)).ToList();
                    }

                }
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    HisExpMestMaterialViewFilterQuery expMestMaterialOnAllViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialOnAllViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMestMaterialOnAllViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMestMaterialOnAllViewFilter.MATERIAL_TYPE_IDs = castFilter.MATERIAL_TYPE_IDs;
                    expMestMaterialOnAllViewFilter.IS_EXPORT = true;
                    listExpMestMaterialOnAlls = new HisExpMestMaterialManager(param).GetView(expMestMaterialOnAllViewFilter);
                    listExpMestMaterialOnAlls = listExpMestMaterialOnAlls.Where(o => !CHMS_EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID)).ToList();
                    Inventec.Common.Logging.LogSystem.Info("listExpMestMaterialOnAlls" + listExpMestMaterialOnAlls.Count);

                    if (listExpMestMaterialMove != null)
                    {
                        listExpMestMaterialOnAlls.AddRange(listExpMestMaterialMove.Where(o => o.EXP_TIME < castFilter.TIME_TO && o.EXP_TIME >= castFilter.TIME_FROM).ToList());
                    }
                    if (this.castFilter.MATERIAL_TYPE_CODEs != null && this.castFilter.MATERIAL_TYPE_CODEs.Length > 0)
                    {
                        listExpMestMaterialOnAlls = listExpMestMaterialOnAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                    if (this.castFilter.MATERIAL_TYPE_IDs != null)
                    {
                        listExpMestMaterialOnAlls = listExpMestMaterialOnAlls.Where(o => filterMaterialTypeIds.Contains(o.MATERIAL_TYPE_ID)).ToList();
                    }
                }
                List<long> listExpMestId = new List<long>();
                if (listExpMestMedicineOnAlls != null)
                {
                    listExpMestId.AddRange(listExpMestMedicineOnAlls.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).Select(s => s.EXP_MEST_ID ?? 0).ToList());
                }
                if (listExpMestMaterialOnAlls != null)
                {
                    listExpMestId.AddRange(listExpMestMaterialOnAlls.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK).Select(s => s.EXP_MEST_ID ?? 0).ToList());
                }
                Inventec.Common.Logging.LogSystem.Info("listExpMestId" + listExpMestId.Count);
                if (listExpMestId.Count > 0)
                {
                    listExpMestId = listExpMestId.Distinct().ToList();
                    var skip = 0;
                    while (listExpMestId.Count - skip > 0)
                    {
                        var listIds = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        // xuất chuyển kho
                        HisExpMestViewFilterQuery chmsExpMestViewFilter = new HisExpMestViewFilterQuery();
                        chmsExpMestViewFilter.IDs = listIds;
                        chmsExpMestViewFilter.EXP_MEST_TYPE_IDs = CHMS_EXP_MEST_TYPE_IDs;
                        listChmsExpMestAlls.AddRange(new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(chmsExpMestViewFilter));
                    }
                }
                //================================================================================================================Cac ki

                HisMediStockPeriodFilterQuery mediStockPeriodViewFilter = new HisMediStockPeriodFilterQuery();
                mediStockPeriodViewFilter.TO_TIME_TO = castFilter.TIME_FROM - 1;
                listMediStockPeriodAlls = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodViewFilter);

                #endregion
                Inventec.Common.Logging.LogSystem.Info("listChmsExpMestAlls" + listChmsExpMestAlls.Count);
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
            //Danh sach loai nhap, loai xuat
            Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
            Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00327RDO>();

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

        //Hàm lấy dữ liệu trước kì
        private void GetMediMateBefore()
        {

        }

        //Hàm lấy dữ liệu xuất chuyển kho ko có phiếu nhập
        private void GetExpNoImp()
        {
            HisImpMestFilterQuery HisImpMestfilter = new HisImpMestFilterQuery();
            HisImpMestfilter.IMP_MEST_TYPE_IDs = new List<long>()
            {
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK,
            };
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listMediStockNotBusis))
                {
                    // lấy từng kho lấy báo cáo
                    foreach (var mediStock in listMediStockNotBusis)
                    {
                        // tính tồn đầu của kho
                        ProcessBeginAmount(mediStock.ID);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMedicineBs" + listRdoMedicines.Count);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMaterialBs" + listRdoMaterials.Count);
                        // tính lượng nhập trong kỳ
                        ProcessImpMest(mediStock.ID);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMedicineIs" + listRdoMedicines.Count);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMaterialIs" + listRdoMaterials.Count);
                        // tính lượng xuất trong kỳ
                        ProcessExpMest(mediStock.ID);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMedicineEs" + listRdoMedicines.Count);
                        Inventec.Common.Logging.LogSystem.Info("listRdoMaterialEs" + listRdoMaterials.Count);
                    }

                    // Blah... blah...
                    if (this.IS_MEDICINE) ProcessOptimizeList(ref listRdoMedicines);
                    if (this.IS_MATERIAL) ProcessOptimizeList(ref listRdoMaterials);
                    if (this.IS_CHEMICAL) ProcessOptimizeList(ref listRdoChemicals);

                    if (this.IS_MEDICINE) ListRdo.AddRange(listRdoMedicines);
                    if (this.IS_MATERIAL) ListRdo.AddRange(listRdoMaterials);
                    if (this.IS_CHEMICAL) ListRdo.AddRange(listRdoChemicals);

                    foreach (var rdo in ListRdo)
                    {
                        rdo.END_AMOUNT = rdo.BEGIN_AMOUNT + rdo.IMP_MEST - rdo.EXP_MEST;
                        rdo.EXP_AMOUNT_BUSI = rdo.EXP_MEST_CHMS_BUSI - rdo.IMP_MEST_CHMS_BUSI;
                        #region Bo
                        rdo.IMP_AMOUNT_NCC = rdo.ID__NCC_IMP_AMOUNT - rdo.ID__TNCC_EXP_AMOUNT;
                        rdo.IMP_AMOUNT_OTHER = rdo.ID__DK_IMP_AMOUNT + rdo.ID__KK_IMP_AMOUNT + rdo.ID__KHAC_IMP_AMOUNT;
                        rdo.EXP_AMOUNT_CAB = rdo.ID__DTT_EXP_AMOUNT - rdo.ID__DTTTL_IMP_AMOUNT;
                        rdo.EXP_AMOUNT_NT = rdo.ID__DNT_EXP_AMOUNT + rdo.ID__PL_EXP_AMOUNT - rdo.ID__DNTTL_IMP_AMOUNT - rdo.ID__THT_IMP_AMOUNT;
                        rdo.EXP_AMOUNT_NGT = rdo.ID__DPK_EXP_AMOUNT - rdo.ID__DMTL_IMP_AMOUNT - rdo.ID__HPTL_IMP_AMOUNT - rdo.ID__TH_IMP_AMOUNT;
                        rdo.EXP_AMOUNT_OTHER = rdo.ID__HPKP_EXP_AMOUNT + rdo.ID__KHAC_EXP_AMOUNT + rdo.ID__DM_EXP_AMOUNT;
                        #endregion
                    }

                    ListRdoGroup = ListRdo.GroupBy(g => g.SERVICE_GROUP_ID).Select(s => new Mrs00327RDO
                    {
                        SERVICE_GROUP_ID = s.First().SERVICE_GROUP_ID,
                        SERVICE_GROUP_NAME = s.First().SERVICE_GROUP_NAME
                    }).ToList();

                    ListRdoParent = ListRdo.GroupBy(g => new { g.SERVICE_GROUP_ID, g.PARENT_ID }).Select(s => new Mrs00327RDO
                    {
                        SERVICE_GROUP_ID = s.First().SERVICE_GROUP_ID,
                        PARENT_ID = s.First().PARENT_ID,
                        PARENT_CODE = s.First().PARENT_CODE,
                        PARENT_NAME = s.First().PARENT_NAME
                    }).ToList();
                }
                Inventec.Common.Logging.LogSystem.Info("ListRdoParent" + ListRdoParent.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected void ProcessBeginAmount(long mediStockId)
        {
            //HisMediStockPeriodFilterQuery mediStockPeriodViewFilter = new HisMediStockPeriodFilterQuery();
            //mediStockPeriodViewFilter.TO_TIME_TO = castFilter.TIME_FROM - 1;
            //mediStockPeriodViewFilter.MEDI_STOCK_ID = mediStockId;
            //var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodViewFilter);
            var listMediStockPeriods = listMediStockPeriodAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();
            //if (IsNotNullOrEmpty(listMediStockPeriods)) : Chua dung
            //{
            //    var mediStockPeriod = listMediStockPeriods.OrderByDescending(s => s.TO_TIME).First();
            //    ProcessBeginAmountWithPeriod(mediStockPeriod);
            //}
            //else
            {
                GetMediMateBefore();
                ProcessBeginAmountWithNoPeriod(mediStockId);
            }
        }

        protected void ProcessBeginAmountWithNoPeriod(long mediStockId)
        {
            if (this.IS_MEDICINE)
            {
                #region medicine
                if (this.IS_MEDICINE)
                {
                    List<long> listMedicineTypeIds = new List<long>();
                    // nhập thuốc trước kỳ
                    #region MyRegion

                    #endregion
                    //lay nhap cua cac kho roi chia ra
                    //HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    //impMestMedicineViewFilter.MEDI_STOCK_ID = mediStockId;
                    //impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    //impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    //var listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                    var listImpMestMedicines = listImpMestMedicineBeforeAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();

                    listMedicineTypeIds.AddRange(listImpMestMedicines.Select(s => s.MEDICINE_TYPE_ID));
                    // xuất thuốc trước kỳ
                    //HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    //expMestMedicineViewFilter.MEDI_STOCK_ID = mediStockId;
                    //expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    //expMestMedicineViewFilter.IS_EXPORT = true;
                    //var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                    var listExpMestMedicines = listExpMestMedicineBeforeAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();

                    listMedicineTypeIds.AddRange(listExpMestMedicines.Select(s => s.SERVICE_TYPE_ID));
                    // loại thuốc 
                    listMedicineTypeIds = listMedicineTypeIds.Distinct().ToList();
                    var skip = 0;
                    var listMedicineTypeOutTimes = new List<V_HIS_MEDICINE_TYPE>();
                    #region MyRegion

                    #endregion
                    //while (listMedicineTypeIds.Count - skip > 0)
                    //{
                    //    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                    //    medicineTypeFilter.IDs = listIds;
                    //    listMedicineTypeOutTimes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).Get(medicineTypeFilter));
                    //}
                    listMedicineTypeOutTimes = listMedicineTypeLeafs.Where(o => listMedicineTypeIds.Contains(o.ID)).ToList();

                    foreach (var medi in listImpMestMedicines)
                    {
                        var medicine = listMedicineTypeOutTimes.Where(w => w.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicine))
                        {
                            var rdo = new Mrs00327RDO();
                            rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MEDI;
                            rdo.SERVICE_GROUP_NAME = "THUỐC";

                            rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = medi.CONCENTRA;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.REGISTER_NUMBER = medi.REGISTER_NUMBER;
                            //rdo.TUTORIAL = medi.TUTORIAL;
                            var me = listMedicineTypeLeafs.FirstOrDefault(o => o.ID == medi.MEDICINE_TYPE_ID);
                            if (me != null)
                            {
                                rdo.ACTIVE_INGR_BHYT_CODE = me.ACTIVE_INGR_BHYT_CODE;
                                rdo.ACTIVE_INGR_BHYT_NAME = me.ACTIVE_INGR_BHYT_NAME;
                                rdo.CONCENTRA = me.CONCENTRA;
                                rdo.NATIONAL_NAME = me.NATIONAL_NAME;
                                rdo.REGISTER_NUMBER = me.REGISTER_NUMBER;
                                rdo.TUTORIAL = me.TUTORIAL;
                                rdo.DOSAGE_FORM = me.DOSAGE_FORM;
                                rdo.MANUFACTURER_NAME = me.MANUFACTURER_NAME;
                                rdo.PACKING_TYPE_NAME = me.PACKING_TYPE_NAME;
                                rdo.MEDICINE_GROUP_NAME = me.MEDICINE_GROUP_NAME;
                                var useForm = ListMedicineUseForm.FirstOrDefault(o => o.ID == me.MEDICINE_USE_FORM_ID);
                                if (useForm != null)
                                {
                                    rdo.MEDICINE_USE_FORM_NAME = useForm.MEDICINE_USE_FORM_NAME;
                                }
                            }

                            long parentId = 0;
                            string parentCode = "TH";
                            string parentName = "Không có nhóm";
                            FindParent(medicine.First(), ref parentId, ref parentCode, ref parentName);
                            rdo.PARENT_ID = parentId;
                            rdo.PARENT_CODE = parentCode;
                            rdo.PARENT_NAME = parentName;

                            rdo.IMP_PRICE = medi.IMP_PRICE * (1 + medi.IMP_VAT_RATIO);
                            rdo.MEDI_STOCK_ID = mediStockId;
                            rdo.BEGIN_AMOUNT = medi.AMOUNT;

                            listRdoMedicines.Add(rdo);
                        }
                    }

                    foreach (var medi in listExpMestMedicines)
                    {
                        var medicine = listMedicineTypeOutTimes.Where(w => w.ID == medi.SERVICE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicine))
                        {
                            var rdo = new Mrs00327RDO();
                            rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MEDI;
                            rdo.SERVICE_GROUP_NAME = "THUỐC";

                            rdo.SERVICE_TYPE_ID = medi.SERVICE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = medicine.First().MEDICINE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = medicine.First().MEDICINE_TYPE_NAME;
                            rdo.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == medicine.First().TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = medi.CONCENTRA;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.REGISTER_NUMBER = medi.REGISTER_NUMBER;
                            rdo.TUTORIAL = medi.TUTORIAL;
                            var me = listMedicineTypeLeafs.FirstOrDefault(o => o.ID == medi.SERVICE_TYPE_ID);
                            if (me != null)
                            {
                                rdo.ACTIVE_INGR_BHYT_CODE = me.ACTIVE_INGR_BHYT_CODE;
                                rdo.ACTIVE_INGR_BHYT_NAME = me.ACTIVE_INGR_BHYT_NAME;
                                rdo.CONCENTRA = me.CONCENTRA;
                                rdo.NATIONAL_NAME = me.NATIONAL_NAME;
                                rdo.REGISTER_NUMBER = me.REGISTER_NUMBER;
                                rdo.TUTORIAL = me.TUTORIAL;
                                rdo.DOSAGE_FORM = me.DOSAGE_FORM;
                                rdo.MANUFACTURER_NAME = me.MANUFACTURER_NAME;
                                rdo.PACKING_TYPE_NAME = me.PACKING_TYPE_NAME;
                                rdo.MEDICINE_GROUP_NAME = me.MEDICINE_GROUP_NAME;
                                var useForm = ListMedicineUseForm.FirstOrDefault(o => o.ID == me.MEDICINE_USE_FORM_ID);
                                if (useForm != null)
                                {
                                    rdo.MEDICINE_USE_FORM_NAME = useForm.MEDICINE_USE_FORM_NAME;
                                }
                            }

                            long parentId = 0;
                            string parentCode = "TH";
                            string parentName = "Không có nhóm";
                            FindParent(medicine.First(), ref parentId, ref parentCode, ref parentName);
                            rdo.PARENT_ID = parentId;
                            rdo.PARENT_CODE = parentCode;
                            rdo.PARENT_NAME = parentName;

                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.MEDI_STOCK_ID = mediStockId;

                            rdo.BEGIN_AMOUNT = medi.BEGIN_AMOUNT;

                            listRdoMedicines.Add(rdo);
                        }
                    }
                }

                /*các trường cần khi lấy báo cáo
                 MEDI_STOCK_ID
                 * IMP_PRICE = IMP_PRICE*(1+IMP_VAT_RATIO)
                 * BEGIN_AMOUNT
                 * SERVICE_TYPE_ID = MEDICINE_TYPE_ID
                 * 
                 */
                #endregion
            }
            if (this.IS_MATERIAL || this.IS_CHEMICAL)
            {
                #region material and chemical
                if (this.IS_CHEMICAL || this.IS_MATERIAL)
                {
                    List<long> listMaterialTypeIds = new List<long>();
                    #region MyRegion

                    #endregion
                    //HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    //impMestMaterialViewFilter.MEDI_STOCK_ID = mediStockId;
                    //impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    //impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    //var listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                    var listImpMestMaterials = listImpMestMaterialBeforeAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();

                    listMaterialTypeIds.AddRange(listImpMestMaterials.Select(w => w.MATERIAL_TYPE_ID).ToList());

                    //HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    //expMestMaterialViewFilter.MEDI_STOCK_ID = mediStockId;
                    //expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    //expMestMaterialViewFilter.IS_EXPORT = true;
                    //var listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
                    var listExpMestMaterials = listExpMestMaterialBeforeAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();

                    listMaterialTypeIds.AddRange(listExpMestMaterials.Select(w => w.SERVICE_TYPE_ID).ToList());

                    listMaterialTypeIds = listMaterialTypeIds.Distinct().ToList();
                    var skip = 0;
                    var listMaterialTypeOutTimes = new List<HIS_MATERIAL_TYPE>();
                    #region MyRegion

                    #endregion
                    //while (listMaterialTypeIds.Count - skip > 0)
                    //{
                    //    var listIds = listMaterialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisMaterialTypeFilterQuery materialTypeOutTimeFilter = new HisMaterialTypeFilterQuery();
                    //    materialTypeOutTimeFilter.IDs = listIds;
                    //    listMaterialTypeOutTimes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).Get(materialTypeOutTimeFilter));
                    //}
                    listMaterialTypeOutTimes = listMaterialTypeLeafs.Where(o => listMaterialTypeIds.Contains(o.ID)).ToList();

                    foreach (var mate in listImpMestMaterials)
                    {
                        var materialType = listMaterialTypeOutTimes.Where(s => s.ID == mate.MATERIAL_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(materialType))
                        {
                            if ((this.IS_CHEMICAL && materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                || (this.IS_MATERIAL && materialType.First().IS_CHEMICAL_SUBSTANCE != 1))
                            {
                                var rdo = new Mrs00327RDO();
                                rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                                rdo.IS_STENT = materialType.First().IS_STENT == 1 ? "STENT" : "NO_STENT";
                                rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;

                                long parentId = 0;
                                string parentCode = "VT";
                                string parentName = "Không có nhóm";
                                FindParent(materialType.First(), ref parentId, ref parentCode, ref parentName);
                                rdo.PARENT_ID = parentId;
                                rdo.PARENT_CODE = parentCode;
                                rdo.PARENT_NAME = parentName;

                                rdo.IMP_PRICE = mate.IMP_PRICE * (1 + mate.IMP_VAT_RATIO);
                                rdo.MEDI_STOCK_ID = mediStockId;

                                rdo.BEGIN_AMOUNT = mate.AMOUNT;

                                if (materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                {
                                    rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__CHEM;
                                    rdo.SERVICE_GROUP_NAME = "HÓA CHẤT";
                                    listRdoChemicals.Add(rdo);
                                }
                                else
                                {
                                    rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MATE;
                                    rdo.SERVICE_GROUP_NAME = "VẬT TƯ";
                                    listRdoMaterials.Add(rdo);
                                }
                            }
                        }
                    }

                    foreach (var mate in listExpMestMaterials)
                    {
                        var materialType = listMaterialTypeOutTimes.Where(s => s.ID == mate.SERVICE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(materialType))
                        {
                            if ((this.IS_CHEMICAL && materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                || (this.IS_MATERIAL && materialType.First().IS_CHEMICAL_SUBSTANCE != 1))
                            {
                                var rdo = new Mrs00327RDO();
                                rdo.SERVICE_TYPE_ID = mate.SERVICE_TYPE_ID;
                                rdo.IS_STENT = materialType.First().IS_STENT == 1 ? "STENT" : "NO_STENT";
                                rdo.SERVICE_TYPE_CODE = materialType.First().MATERIAL_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = materialType.First().MATERIAL_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == materialType.First().TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;

                                long parentId = 0;
                                string parentCode = "VT";
                                string parentName = "Không có nhóm";
                                FindParent(materialType.First(), ref parentId, ref parentCode, ref parentName);
                                rdo.PARENT_ID = parentId;
                                rdo.PARENT_CODE = parentCode;
                                rdo.PARENT_NAME = parentName;

                                rdo.IMP_PRICE = mate.IMP_PRICE;

                                rdo.BEGIN_AMOUNT = mate.BEGIN_AMOUNT;
                                rdo.MEDI_STOCK_ID = mediStockId;

                                if (materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                {
                                    rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__CHEM;
                                    rdo.SERVICE_GROUP_NAME = "HÓA CHẤT";
                                    listRdoChemicals.Add(rdo);
                                }
                                else
                                {
                                    rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MATE;
                                    rdo.SERVICE_GROUP_NAME = "VẬT TƯ";
                                    listRdoMaterials.Add(rdo);
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        protected void ProcessImpMest(long mediStockId)
        {
            if (this.IS_MEDICINE)
            {
                #region medicine
                if (this.IS_MEDICINE)
                {
                    //Tạo 1 lần rồi chia ra các kho
                    #region MyRegion

                    #endregion
                    //HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    //impMestMedicineViewFilter.MEDI_STOCK_ID = mediStockId;
                    //impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    //impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    //impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    //var listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                    var listImpMestMedicines = listImpMestMedicineOnAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();
                    if (IsNotNullOrEmpty(listImpMestMedicines))
                    {
                        var skip = 0;
                        var listImpMestMedis = new List<V_HIS_IMP_MEST>();
                        var listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
                        // Tạo luôn 1 lần theo listImpMestMedicines
                        #region MyRegion

                        #endregion
                        //while (listImpMestMedicines.Count - skip > 0)
                        //{
                        //    var listIds = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //    HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                        //    mobaImpMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                        //    listImpMestMedis.AddRange(new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(mobaImpMestViewFilter));

                        //    // loại thuốc
                        //    HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                        //    medicineTypeFilter.IDs = listIds.Select(s => s.MEDICINE_TYPE_ID).ToList();
                        //    listMedicineTypes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).Get(medicineTypeFilter));
                        //}
                        listImpMestMedis = listImpMestOnAlls.Where(o => listImpMestMedicines.Exists(p => p.IMP_MEST_ID == o.ID)).ToList();
                        listMedicineTypes = listMedicineTypeLeafs.Where(o => listImpMestMedicines.Exists(p => p.MEDICINE_TYPE_ID == o.ID)).ToList();
                        skip = 0;
                        var listExpMestMedis = new List<HIS_EXP_MEST>();
                        var mobaExpId = listImpMestMedis.Select(s => s.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList();
                        //Tạo luôn 1 lần theo listImpMestMedis
                        #region MyRegion

                        #endregion
                        //while (mobaExpId.Count - skip > 0)
                        //{
                        //    var listIds = mobaExpId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                        //    expMestFilter.IDs = listIds;
                        //    listExpMestMedis.AddRange(new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter));
                        //}
                        listExpMestMedis = listExpMestHasMobasOrChmAlls.Where(o => mobaExpId.Contains(o.ID)).ToList();

                        var listMobaImpMestMedis = new List<V_HIS_IMP_MEST>();
                        listMobaImpMestMedis = listImpMestMedis.Where(o => MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                        var listChmsImpMestMedis = new List<V_HIS_IMP_MEST>();
                        listChmsImpMestMedis = listImpMestMedis.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                        foreach (var medi in listImpMestMedicines)
                        {
                            var medicine = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                            var impMest = listImpMestMedis.FirstOrDefault(o => o.ID == medi.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                            if (IsNotNullOrEmpty(medicine))
                            {
                                var rdo = new Mrs00327RDO();
                                rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MEDI;
                                rdo.SERVICE_GROUP_NAME = "THUỐC";

                                rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                                rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;

                                long _parent_id = 0;
                                string _parent_code = "TH";
                                string _parent_name = "Không có nhóm";
                                FindParent(medicine.First(), ref _parent_id, ref _parent_code, ref _parent_name);
                                rdo.PARENT_ID = _parent_id;
                                rdo.PARENT_CODE = _parent_code;
                                rdo.PARENT_NAME = _parent_name;

                                rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                                rdo.IMP_PRICE = medi.IMP_PRICE * (1 + medi.IMP_VAT_RATIO);
                                rdo.MEDI_STOCK_ID = mediStockId;

                                rdo.IMP_MEST = medi.AMOUNT;
                                if (dicImpAmountType.ContainsKey(medi.IMP_MEST_TYPE_ID))
                                    dicImpAmountType[medi.IMP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                                if (CHMS_IMP_MEST_TYPE_IDs.Contains(medi.IMP_MEST_TYPE_ID))
                                {
                                    var chms = listExpMestHasMobasOrChmAlls.Where(w => w.ID == impMest.CHMS_EXP_MEST_ID && listMediStockBusis.Select(s => s.ID).Contains(w.MEDI_STOCK_ID)).ToList();
                                    if (IsNotNullOrEmpty(chms))
                                        rdo.IMP_MEST_CHMS_BUSI = medi.AMOUNT;

                                }

                                var me = listMedicineTypeLeafs.FirstOrDefault(o => o.ID == medi.MEDICINE_TYPE_ID);
                                if (me != null)
                                {
                                    rdo.ACTIVE_INGR_BHYT_CODE = me.ACTIVE_INGR_BHYT_CODE;
                                    rdo.ACTIVE_INGR_BHYT_NAME = me.ACTIVE_INGR_BHYT_NAME;
                                    rdo.CONCENTRA = me.CONCENTRA;
                                    rdo.NATIONAL_NAME = me.NATIONAL_NAME;
                                    rdo.REGISTER_NUMBER = me.REGISTER_NUMBER;
                                    rdo.TUTORIAL = me.TUTORIAL;
                                    rdo.DOSAGE_FORM = me.DOSAGE_FORM;
                                    rdo.MANUFACTURER_NAME = me.MANUFACTURER_NAME;
                                    rdo.PACKING_TYPE_NAME = me.PACKING_TYPE_NAME;
                                    rdo.MEDICINE_GROUP_NAME = me.MEDICINE_GROUP_NAME;
                                    var useForm = ListMedicineUseForm.FirstOrDefault(o => o.ID == me.MEDICINE_USE_FORM_ID);
                                    if (useForm != null)
                                    {
                                        rdo.MEDICINE_USE_FORM_NAME = useForm.MEDICINE_USE_FORM_NAME;
                                    }
                                }
                                listRdoMedicines.Add(rdo);
                            }
                        }
                    }
                }
                #endregion
            }
            if (this.IS_MATERIAL || this.IS_CHEMICAL)
            {
                #region material and chemical
                if (this.IS_MATERIAL || this.IS_CHEMICAL)
                {
                    List<long> listMaterialTypeIds = new List<long>();
                    #region MyRegion

                    #endregion
                    //HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    //impMestMaterialViewFilter.MEDI_STOCK_ID = mediStockId;
                    //impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    //impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    //impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    //var listImpMestMaterials = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                    var listImpMestMaterials = listImpMestMaterialOnAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();
                    listMaterialTypeIds.AddRange(listImpMestMaterials.Select(w => w.MATERIAL_TYPE_ID).ToList());

                    listMaterialTypeIds = listMaterialTypeIds.Distinct().ToList();
                    var skip = 0;
                    var listMaterialTypeInTimes = new List<HIS_MATERIAL_TYPE>();
                    #region MyRegion

                    #endregion
                    //while (listMaterialTypeIds.Count - skip > 0)
                    //{
                    //    var listIds = listMaterialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisMaterialTypeFilterQuery materialTypeOutTimeFilter = new HisMaterialTypeFilterQuery();
                    //    materialTypeOutTimeFilter.IDs = listIds;
                    //    listMaterialTypeInTimes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).Get(materialTypeOutTimeFilter));
                    //}
                    listMaterialTypeInTimes = listMaterialTypeLeafs.Where(o => listMaterialTypeIds.Contains(o.ID)).ToList();
                    #region MyRegion

                    #endregion
                    if (IsNotNullOrEmpty(listImpMestMaterials))
                    {
                        skip = 0;
                        var listImpMestMates = new List<V_HIS_IMP_MEST>();
                        //while (listImpMestMaterials.Count - skip > 0)
                        //{
                        //    var listIds = listImpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //    HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                        //    mobaImpMestViewFilter.IDs = listIds.Select(s => s.IMP_MEST_ID).ToList();
                        //    listImpMestMates.AddRange(new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(mobaImpMestViewFilter));
                        //}
                        listImpMestMates = listImpMestOnAlls.Where(o => listImpMestMaterials.Exists(p => p.IMP_MEST_ID == o.ID)).ToList();
                        skip = 0;
                        #region MyRegion

                        #endregion
                        //while (mobaExpId.Count - skip > 0)
                        //{
                        //    var listIds = mobaExpId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                        //    expMestFilter.IDs = listIds;
                        //    listExpMestMates.AddRange(new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter));
                        //}
                        var listMobaImpMestMates = new List<V_HIS_IMP_MEST>();
                        listMobaImpMestMates = listImpMestMates.Where(o => MOBA_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                        var listChmsImpMestMates = new List<V_HIS_IMP_MEST>();
                        listChmsImpMestMates = listImpMestMates.Where(o => CHMS_IMP_MEST_TYPE_IDs.Contains(o.IMP_MEST_TYPE_ID)).ToList();

                        foreach (var mate in listImpMestMaterials)
                        {
                            var materialType = listMaterialTypeInTimes.Where(s => s.ID == mate.MATERIAL_TYPE_ID).ToList();
                            var impMest = listImpMestMates.FirstOrDefault(o => o.ID == mate.IMP_MEST_ID) ?? new V_HIS_IMP_MEST();
                            if (IsNotNullOrEmpty(materialType))
                            {
                                if ((this.IS_CHEMICAL && materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                    || (this.IS_MATERIAL && materialType.First().IS_CHEMICAL_SUBSTANCE != 1))
                                {
                                    var rdo = new Mrs00327RDO();
                                    rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MATE;
                                    rdo.SERVICE_GROUP_NAME = "VẬT TƯ";
                                    rdo.IS_STENT = materialType.First().IS_STENT == 1 ? "STENT" : "NO_STENT";

                                    rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                                    rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                                    rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;

                                    long parentId = 0;
                                    string parentCode = "VT";
                                    string parentName = "Không có nhóm";
                                    FindParent(materialType.First(), ref parentId, ref parentCode, ref parentName);
                                    rdo.PARENT_ID = parentId;
                                    rdo.PARENT_CODE = parentCode;
                                    rdo.PARENT_NAME = parentName;

                                    rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;

                                    rdo.IMP_PRICE = mate.IMP_PRICE * (1 + mate.IMP_VAT_RATIO);
                                    rdo.MEDI_STOCK_ID = mediStockId;

                                    rdo.IMP_MEST = mate.AMOUNT;
                                    if (dicImpAmountType.ContainsKey(mate.IMP_MEST_TYPE_ID))
                                        dicImpAmountType[mate.IMP_MEST_TYPE_ID].SetValue(rdo, mate.AMOUNT);

                                    if (CHMS_IMP_MEST_TYPE_IDs.Contains(mate.IMP_MEST_TYPE_ID))
                                    {
                                        var chms = listExpMestHasMobasOrChmAlls.Where(w => w.ID == impMest.CHMS_EXP_MEST_ID && listMediStockBusis.Select(s => s.ID).Contains(w.MEDI_STOCK_ID)).ToList();
                                        if (IsNotNullOrEmpty(chms))
                                            rdo.IMP_MEST_CHMS_BUSI = mate.AMOUNT;
                                    }

                                    if (materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                    {
                                        rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__CHEM;
                                        rdo.SERVICE_GROUP_NAME = "HÓA CHẤT";
                                        listRdoChemicals.Add(rdo);
                                    }
                                    else
                                    {
                                        rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MATE;
                                        rdo.SERVICE_GROUP_NAME = "VẬT TƯ";
                                        listRdoMaterials.Add(rdo);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        protected void ProcessExpMest(long mediStockId)
        {
            if (this.IS_MEDICINE)
            {
                #region medicine
                if (this.IS_MEDICINE)
                {
                    #region MyRegion

                    #endregion
                    //HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    ////expMestMedicineViewFilter.IN_EXECUTE = true;
                    //expMestMedicineViewFilter.MEDI_STOCK_ID = mediStockId;
                    //expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    //expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    //expMestMedicineViewFilter.IS_EXPORT = true;
                    //var listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                    var listExpMestMedicines = listExpMestMedicineOnAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();
                    var skip = 0;
                    var listChmsExpMestMedis = new List<V_HIS_EXP_MEST>();
                    var listMedicineTypeInTimes = new List<V_HIS_MEDICINE_TYPE>();
                    #region MyRegion

                    #endregion
                    //while (listExpMestMedicines.Count - skip > 0)
                    //{
                    //    var listIds = listExpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    //    // xuất chuyển kho
                    //    HisExpMestViewFilterQuery chmsExpMestViewFilter = new HisExpMestViewFilterQuery();
                    //    chmsExpMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID).ToList();
                    //    chmsExpMestViewFilter.EXP_MEST_TYPE_IDs = CHMS_EXP_MEST_TYPE_IDs;
                    //    listChmsExpMestMedis.AddRange(new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(chmsExpMestViewFilter));
                    //    // loại thuốc
                    //    HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                    //    medicineTypeFilter.IDs = listIds.Select(s => s.MEDICINE_TYPE_ID).ToList();
                    //    listMedicineTypeInTimes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).Get(medicineTypeFilter));
                    //}
                    listChmsExpMestMedis = listChmsExpMestAlls.Where(o => listExpMestMedicines.Exists(p => p.EXP_MEST_ID == o.ID)).ToList();
                    listMedicineTypeInTimes = listMedicineTypeLeafs.Where(o => listExpMestMedicines.Exists(p => p.MEDICINE_TYPE_ID == o.ID)).ToList();

                    if (IsNotNullOrEmpty(listExpMestMedicines))
                    {
                        foreach (var medi in listExpMestMedicines)
                        {
                            var medicine = listMedicineTypeInTimes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(medicine))
                            {
                                var rdo = new Mrs00327RDO();
                                rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MEDI;
                                rdo.SERVICE_GROUP_NAME = "THUỐC";

                                rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                                rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                                rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                                rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                                rdo.CONCENTRA = medi.CONCENTRA;
                                rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                                rdo.REGISTER_NUMBER = medi.REGISTER_NUMBER;
                                rdo.TUTORIAL = medi.TUTORIAL;
                                var me = listMedicineTypeLeafs.FirstOrDefault(o => o.ID == medi.MEDICINE_TYPE_ID);
                                if (me != null)
                                {
                                    rdo.ACTIVE_INGR_BHYT_CODE = me.ACTIVE_INGR_BHYT_CODE;
                                    rdo.ACTIVE_INGR_BHYT_NAME = me.ACTIVE_INGR_BHYT_NAME;
                                    rdo.CONCENTRA = me.CONCENTRA;
                                    rdo.NATIONAL_NAME = me.NATIONAL_NAME;
                                    rdo.REGISTER_NUMBER = me.REGISTER_NUMBER;
                                    rdo.TUTORIAL = me.TUTORIAL;
                                    rdo.DOSAGE_FORM = me.DOSAGE_FORM;
                                    rdo.MANUFACTURER_NAME = me.MANUFACTURER_NAME;
                                    rdo.PACKING_TYPE_NAME = me.PACKING_TYPE_NAME;
                                    rdo.MEDICINE_GROUP_NAME = me.MEDICINE_GROUP_NAME;
                                    var useForm = ListMedicineUseForm.FirstOrDefault(o => o.ID == me.MEDICINE_USE_FORM_ID);
                                    if (useForm != null)
                                    {
                                        rdo.MEDICINE_USE_FORM_NAME = useForm.MEDICINE_USE_FORM_NAME;
                                    }
                                }

                                long parentId = 0;
                                string parentCode = "TH";
                                string parentName = "Không có nhóm";
                                FindParent(medicine.First(), ref parentId, ref parentCode, ref parentName);
                                rdo.PARENT_ID = parentId;
                                rdo.PARENT_CODE = parentCode;
                                rdo.PARENT_NAME = parentName;

                                rdo.IMP_PRICE = medi.IMP_PRICE * (1 + medi.IMP_VAT_RATIO);
                                rdo.MEDI_STOCK_ID = mediStockId;

                                rdo.EXP_MEST = medi.AMOUNT;
                                if (dicExpAmountType.ContainsKey(medi.EXP_MEST_TYPE_ID))
                                    dicExpAmountType[medi.EXP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                                if (CHMS_EXP_MEST_TYPE_IDs.Contains(medi.EXP_MEST_TYPE_ID))
                                {
                                    var chms = listChmsExpMestMedis.Where(w => w.ID == medi.EXP_MEST_ID && listMediStockBusis.Select(s => s.ID).Contains(w.IMP_MEDI_STOCK_ID ?? 0)).ToList();
                                    if (IsNotNullOrEmpty(chms))
                                        rdo.EXP_MEST_CHMS_BUSI = medi.AMOUNT;
                                }
                                listRdoMedicines.Add(rdo);
                            }
                        }
                    }
                }
                #endregion
            }
            if (this.IS_MATERIAL || this.IS_CHEMICAL)
            {
                #region material and chemical
                if (this.IS_CHEMICAL || this.IS_MATERIAL)
                {
                    List<long> listMaterialTypeIds = new List<long>();
                    #region MyRegion

                    #endregion
                    //HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    //expMestMaterialViewFilter.MEDI_STOCK_ID = mediStockId;
                    //expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    //expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    //expMestMaterialViewFilter.IS_EXPORT = true;
                    //var listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
                    var listExpMestMaterials = listExpMestMaterialOnAlls.Where(o => o.MEDI_STOCK_ID == mediStockId).ToList();
                    listMaterialTypeIds.AddRange(listExpMestMaterials.Select(w => w.MATERIAL_TYPE_ID).ToList());

                    listMaterialTypeIds = listMaterialTypeIds.Distinct().ToList();
                    var skip = 0;
                    var listMaterialTypeInTimes = new List<HIS_MATERIAL_TYPE>();
                    #region MyRegion

                    #endregion
                    //while (listMaterialTypeIds.Count - skip > 0)
                    //{
                    //    var listIds = listMaterialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisMaterialTypeFilterQuery materialTypeOutTimeFilter = new HisMaterialTypeFilterQuery();
                    //    materialTypeOutTimeFilter.IDs = listIds;
                    //    listMaterialTypeInTimes.AddRange(new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).Get(materialTypeOutTimeFilter));
                    //}
                    listMaterialTypeInTimes = listMaterialTypeLeafs.Where(o => listMaterialTypeIds.Contains(o.ID)).ToList();
                    skip = 0;
                    var listChmsExpMestMates = new List<V_HIS_EXP_MEST>();
                    #region MyRegion

                    #endregion
                    //while (listExpMestMaterials.Count - skip > 0)
                    //{
                    //    var listIds = listExpMestMaterials.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    //    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    //    HisExpMestViewFilterQuery chmsExpMestViewFilter = new HisExpMestViewFilterQuery();
                    //    chmsExpMestViewFilter.IDs = listIds.Select(s => s.EXP_MEST_ID).ToList();
                    //    chmsExpMestViewFilter.EXP_MEST_TYPE_IDs = CHMS_EXP_MEST_TYPE_IDs;
                    //    listChmsExpMestMates.AddRange(new MOS.MANAGER.HisExpMest.HisExpMestManager(param).GetView(chmsExpMestViewFilter));
                    //}
                    listChmsExpMestMates = listChmsExpMestAlls.Where(o => listExpMestMaterials.Exists(p => p.EXP_MEST_ID == o.ID)).ToList();
                    if (IsNotNullOrEmpty(listExpMestMaterials))
                    {
                        foreach (var mate in listExpMestMaterials)
                        {
                            var materialType = listMaterialTypeInTimes.Where(s => s.ID == mate.MATERIAL_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(materialType))
                            {
                                if ((this.IS_CHEMICAL && materialType.First().IS_CHEMICAL_SUBSTANCE == 1) || (this.IS_MATERIAL && materialType.First().IS_CHEMICAL_SUBSTANCE != 1))
                                {
                                    var rdo = new Mrs00327RDO();

                                    rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                                    rdo.IS_STENT = materialType.First().IS_STENT == 1 ? "STENT" : "NO_STENT";
                                    rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                                    rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;
                                    rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;

                                    long parentId = 0;
                                    string parentCode = "VT";
                                    string parentName = "Không có nhóm";
                                    FindParent(materialType.First(), ref parentId, ref parentCode, ref parentName);
                                    rdo.PARENT_ID = parentId;
                                    rdo.PARENT_CODE = parentCode;
                                    rdo.PARENT_NAME = parentName;

                                    rdo.IMP_PRICE = mate.IMP_PRICE * (1 + mate.IMP_VAT_RATIO);
                                    rdo.MEDI_STOCK_ID = mediStockId;

                                    rdo.EXP_MEST = mate.AMOUNT;

                                    if (dicExpAmountType.ContainsKey(mate.EXP_MEST_TYPE_ID))
                                        dicExpAmountType[mate.EXP_MEST_TYPE_ID].SetValue(rdo, mate.AMOUNT);


                                    if (CHMS_EXP_MEST_TYPE_IDs.Contains(mate.EXP_MEST_TYPE_ID))
                                    {

                                        var chms = listChmsExpMestMates.Where(w => w.ID == mate.EXP_MEST_ID && listMediStockBusis.Select(s => s.ID).Contains(w.IMP_MEDI_STOCK_ID ?? 0)).ToList();
                                        if (IsNotNullOrEmpty(chms))
                                            rdo.EXP_MEST_CHMS_BUSI = mate.AMOUNT;
                                    }

                                    if (materialType.First().IS_CHEMICAL_SUBSTANCE == 1)
                                    {
                                        rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__CHEM;
                                        rdo.SERVICE_GROUP_NAME = "HÓA CHẤT";
                                        listRdoChemicals.Add(rdo);
                                    }
                                    else
                                    {
                                        rdo.SERVICE_GROUP_ID = SERVICE_GROUP_ID__MATE;
                                        rdo.SERVICE_GROUP_NAME = "VẬT TƯ";
                                        listRdoMaterials.Add(rdo);
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }

        protected void FindParent(V_HIS_MEDICINE_TYPE medicineType, ref long parentID, ref string parentCode, ref string parentName)
        {
            parentID = 0;
            parentCode = "TH";
            parentName = "Không có nhóm";

            if (IsNotNull(medicineType.PARENT_ID))
            {
                var medicine = listMedicineTypeParents.Where(w => w.ID == medicineType.PARENT_ID.Value).ToList();
                if (IsNotNullOrEmpty(medicine))
                {
                    long? parent = medicine.First().PARENT_ID;
                    parentID = medicine.First().ID;
                    parentCode = "TH" + medicine.First().MEDICINE_TYPE_CODE;
                    parentName = medicine.First().MEDICINE_TYPE_NAME;
                    while (IsNotNull(parent))
                    {
                        medicine = listMedicineTypeParents.Where(w => w.ID == parent).ToList();
                        if (IsNotNullOrEmpty(medicine))
                        {
                            parent = medicine.First().PARENT_ID;
                            parentID = medicine.First().ID;
                            parentCode = "TH" + medicine.First().MEDICINE_TYPE_CODE;
                            parentName = medicine.First().MEDICINE_TYPE_NAME;
                        }
                    }
                }
            }
        }

        protected void FindParent(HIS_MATERIAL_TYPE materialType, ref long parentID, ref string parentCode, ref string parentName)
        {
            if (materialType.IS_CHEMICAL_SUBSTANCE == 1)
            {
                parentID = 0;
                parentCode = "HC";
                parentName = "Không có nhóm";

                if (IsNotNull(materialType.PARENT_ID))
                {
                    var chemical = listMaterialTypeParents.Where(w => w.ID == materialType.PARENT_ID.Value).ToList();
                    if (IsNotNullOrEmpty(chemical))
                    {
                        long? parent = chemical.First().PARENT_ID;
                        parentID = chemical.First().ID;
                        parentCode = "HC" + chemical.First().MATERIAL_TYPE_CODE;
                        parentName = chemical.First().MATERIAL_TYPE_NAME;
                        while (IsNotNull(parent))
                        {
                            chemical = listMaterialTypeParents.Where(w => w.ID == parent).ToList();
                            if (IsNotNullOrEmpty(chemical))
                            {
                                parent = chemical.First().PARENT_ID;
                                parentID = chemical.First().ID;
                                parentCode = "HC" + chemical.First().MATERIAL_TYPE_CODE;
                                parentName = chemical.First().MATERIAL_TYPE_NAME;
                            }
                        }
                    }
                }
            }
            else
            {
                parentID = 0;
                parentCode = "VT";
                parentName = "Không có nhóm";

                if (IsNotNull(materialType.PARENT_ID))
                {
                    var material = listMaterialTypeParents.Where(w => w.ID == materialType.PARENT_ID.Value).ToList();
                    if (IsNotNullOrEmpty(material))
                    {
                        long? parent = material.First().PARENT_ID;
                        parentID = material.First().ID;
                        parentCode = "VT" + material.First().MATERIAL_TYPE_CODE;
                        parentName = material.First().MATERIAL_TYPE_NAME;
                        while (IsNotNull(parent))
                        {
                            material = listMaterialTypeParents.Where(w => w.ID == parent).ToList();
                            if (IsNotNullOrEmpty(material))
                            {
                                parent = material.First().PARENT_ID;
                                parentID = material.First().ID;
                                parentCode = "VT" + material.First().MATERIAL_TYPE_CODE;
                                parentName = material.First().MATERIAL_TYPE_NAME;
                            }
                        }
                    }
                }
            }
        }

        public void ProcessOptimizeList(ref List<Mrs00327RDO> _listRdo)
        {
            foreach (var item in _listRdo)
            {
                item.IMP_PRICE = Math.Round(item.IMP_PRICE, 0, MidpointRounding.AwayFromZero);
            }
            string errorField = "";
            try
            {
                var group = _listRdo.GroupBy(g => new { g.SERVICE_GROUP_ID, g.PARENT_CODE, g.SERVICE_TYPE_CODE, g.IMP_PRICE, g.ACTIVE_INGR_BHYT_CODE, g.REGISTER_NUMBER }).ToList();
                _listRdo.Clear();

                Decimal sum = 0;
                Mrs00327RDO rdo;
                List<Mrs00327RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00327RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00327RDO();
                    listSub = item.ToList<Mrs00327RDO>();
                    rdo.DIC_BEGIN = listSub.GroupBy(o => o.MEDI_STOCK_ID).ToDictionary(o => MediStockCode(o.Key), p => p.Sum(s => s.BEGIN_AMOUNT));

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT") || field.Name.Contains("IMP_MEST") || field.Name.Contains("EXP_MEST"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) _listRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
            _listRdo = _listRdo.Where(s => s.BEGIN_AMOUNT != 0 || s.IMP_MEST != 0 || s.EXP_MEST != 0).ToList();
        }

        private string MediStockCode(long key)
        {
            return (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == key) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
        }

        private Mrs00327RDO IsMeaningful(List<Mrs00327RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00327RDO();
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

                dicSingleTag.Add("SERVICE_GROUP_NAME_CHOICE", SERVICE_GROUP_NAME_CHOICE);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoGroup", ListRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoParent", ListRdoParent.OrderBy(s => s.PARENT_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "RdoParent", "SERVICE_GROUP_ID", "SERVICE_GROUP_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.SERVICE_TYPE_NAME).ThenBy(p => p.IMP_PRICE).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoParent", "Rdo", "PARENT_CODE", "PARENT_CODE");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
