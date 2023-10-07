using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00377
{
    class Mrs00377Processor : AbstractProcessor
    {
        Mrs00377Filter castFilter = null;
        Dictionary<long, List<Mrs00377RDO>> dicListRdo = new Dictionary<long, List<Mrs00377RDO>>();
        List<Mrs00377RDO> ListRdo = new List<Mrs00377RDO>();
        List<Mrs00377RDO> ListRdoGroup = new List<Mrs00377RDO>();

        List<Mrs00377RDO> ListRdoMedicines = new List<Mrs00377RDO>();
        List<Mrs00377RDO> ListRdoMaterials = new List<Mrs00377RDO>();

        List<HIS_MEDI_STOCK> listMediStocks = new List<HIS_MEDI_STOCK>();
        List<HIS_MEDI_STOCK> listMediStockTongs = new List<HIS_MEDI_STOCK>();

        List<V_HIS_IMP_MEST> listImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();

        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST_BLOOD> listImpMestBloods = new List<V_HIS_IMP_MEST_BLOOD>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();

        List<V_HIS_MEDICINE_TYPE> listMedicineTypes = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialTypes = new List<V_HIS_MATERIAL_TYPE>();
        List<long> IdKhoTong = new List<long>();
        List<long> IdKhoNoiTru = new List<long>();
        List<long> IdKhoNgoaiTru = new List<long>();
        List<long> IdKhoCongDong = new List<long>();
        public const long GROUP_ID__MEDI = 1;
        public const long GROUP_ID__MATE = 2;
        public const long GROUP_ID__BLOO = 3;

        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL
        };

        List<long> CHMS_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BCS,
            //IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__HCS
        };

        public Mrs00377Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00377Filter);
        }

        protected override bool GetData()
        {
            this.castFilter = (Mrs00377Filter)this.reportFilter;
            bool result = false;
            try
            {
                List<long> listMediStockIds = new List<long>();
                if (!castFilter.IS_PROGRAM)
                {
                    if (IsNotNullOrEmpty(HisMediStockTypeCFG.MEDI_STOCK_ID_TONG))
                    {
                        IdKhoTong.AddRange(HisMediStockTypeCFG.MEDI_STOCK_ID_TONG);
                    }

                    if (IsNotNullOrEmpty(HisMediStockTypeCFG.MEDI_STOCK_ID_NOI_TRU))
                    {
                        IdKhoNoiTru.AddRange(HisMediStockTypeCFG.MEDI_STOCK_ID_NOI_TRU);
                    }

                    if (IsNotNullOrEmpty(HisMediStockTypeCFG.MEDI_STOCK_ID_NGOAI_TRU))
                    {
                        IdKhoNgoaiTru.AddRange(HisMediStockTypeCFG.MEDI_STOCK_ID_NGOAI_TRU);
                    }

                    listMediStockIds.AddRange(IdKhoTong);
                    listMediStockIds.AddRange(IdKhoNoiTru);
                    listMediStockIds.AddRange(IdKhoNgoaiTru);
                }
                else
                {
                    IdKhoTong.AddRange(GetIds("MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.TCT"));
                    IdKhoNoiTru.AddRange(GetIds("MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NTCT"));
                    IdKhoNgoaiTru.AddRange(GetIds("MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.NGTCT"));
                    IdKhoCongDong.AddRange(GetIds("MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.CONG_DONG"));
                    listMediStockIds.AddRange(IdKhoTong);
                    listMediStockIds.AddRange(IdKhoNoiTru);
                    listMediStockIds.AddRange(IdKhoNgoaiTru);
                    listMediStockIds.AddRange(IdKhoCongDong);
                }

                var skip = 0;
                while (listMediStockIds.Count - skip > 0)
                {
                    var listMediStockIdss = listMediStockIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    // MEDI_STOCK
                    HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                    mediStockFilter.IDs = listMediStockIdss;
                    listMediStocks.AddRange(new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter));
                }

                HisMediStockFilterQuery mediStockFilter2 = new HisMediStockFilterQuery();
                mediStockFilter2.IDs = castFilter.IS_PROGRAM ? GetIds("MRS.HIS_RS.HIS_MEDI_STOCK.MEDI_STOCK_CODE.TCT") : HisMediStockTypeCFG.MEDI_STOCK_ID_TONG;
                listMediStockTongs = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter2);

                HisMedicineTypeViewFilterQuery medicineTypeViewFilter = new HisMedicineTypeViewFilterQuery();
                listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).GetView(medicineTypeViewFilter);

                HisMaterialTypeViewFilterQuery materialTypeViewFilter = new HisMaterialTypeViewFilterQuery();
                listMaterialTypes = new MOS.MANAGER.HisMaterialType.HisMaterialTypeManager(param).GetView(materialTypeViewFilter);

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
                if (IsNotNullOrEmpty(listMediStocks))
                {
                    foreach (var mediStock in listMediStocks)
                    {
                        // tính tồn đầu
                        ProcessBeginAmount(mediStock);
                        // nhập trong kỳ
                        ProcessImpMestAmount(mediStock);
                        // xuất trong kỳ
                        ProcessExpMestAmount(mediStock);
                    }

                    foreach (var medicine in ListRdoMedicines.OrderBy(s => s.SERVICE_TYPE_NAME).ToList())
                    {
                        medicine.END_AMOUNT_TONG = medicine.BEGIN_AMOUNT_TONG + medicine.IMP_AMOUNT_TONG - medicine.EXP_AMOUNT_TONG;

                        medicine.END_AMOUNT_NOI_TRU = medicine.BEGIN_AMOUNT_NOI_TRU + medicine.IMP_AMOUNT_NOI_TRU - medicine.EXP_AMOUNT_NOI_TRU;
                        medicine.EXP_AMOUNT_NOI_TRU = medicine.EXP_AMOUNT_NOI_TRU - medicine.IMP_AMOUNT_NOI_TRU_MOBA;

                        medicine.END_AMOUNT_NGOAI_TRU = medicine.BEGIN_AMOUNT_NGOAI_TRU + medicine.IMP_AMOUNT_NGOAI_TRU - medicine.EXP_AMOUNT_NGOAI_TRU;
                        medicine.EXP_AMOUNT_NGOAI_TRU = medicine.EXP_AMOUNT_NGOAI_TRU - medicine.IMP_AMOUNT_NGOAI_TRU_MOBA;
                        medicine.END_AMOUNT_CONG_DONG = medicine.BEGIN_AMOUNT_CONG_DONG + medicine.IMP_AMOUNT_CONG_DONG - medicine.EXP_AMOUNT_CONG_DONG;
                        medicine.EXP_AMOUNT_CONG_DONG = medicine.EXP_AMOUNT_CONG_DONG - medicine.IMP_AMOUNT_CONG_DONG_MOBA;
                        medicine.END_AMOUNT_TVIEN = medicine.BEGIN_AMOUNT_TVIEN + medicine.BEGIN_AMOUNT_TVIEN - medicine.EXP_AMOUNT_TVIEN;
                    }

                    ListRdo.AddRange(ListRdoMedicines);

                    foreach (var material in ListRdoMaterials.OrderBy(s => s.SERVICE_TYPE_NAME).ToList())
                    {
                        material.END_AMOUNT_TONG = material.BEGIN_AMOUNT_TONG + material.IMP_AMOUNT_TONG - material.EXP_AMOUNT_TONG;

                        material.END_AMOUNT_NOI_TRU = material.BEGIN_AMOUNT_NOI_TRU + material.IMP_AMOUNT_NOI_TRU - material.EXP_AMOUNT_NOI_TRU;
                        material.EXP_AMOUNT_NOI_TRU = material.EXP_AMOUNT_NOI_TRU - material.IMP_AMOUNT_NOI_TRU_MOBA;

                        material.END_AMOUNT_NGOAI_TRU = material.BEGIN_AMOUNT_NGOAI_TRU + material.IMP_AMOUNT_NGOAI_TRU - material.EXP_AMOUNT_NGOAI_TRU;
                        material.EXP_AMOUNT_NGOAI_TRU = material.EXP_AMOUNT_NGOAI_TRU - material.IMP_AMOUNT_NGOAI_TRU_MOBA;

                        material.END_AMOUNT_CONG_DONG = material.BEGIN_AMOUNT_CONG_DONG + material.IMP_AMOUNT_CONG_DONG - material.EXP_AMOUNT_CONG_DONG;
                        material.EXP_AMOUNT_CONG_DONG = material.EXP_AMOUNT_CONG_DONG - material.IMP_AMOUNT_CONG_DONG_MOBA;
                        material.END_AMOUNT_TVIEN = material.BEGIN_AMOUNT_TVIEN + material.BEGIN_AMOUNT_TVIEN - material.EXP_AMOUNT_TVIEN;
                    }

                    ListRdo.AddRange(ListRdoMaterials);

                    ListRdoGroup = ListRdo.GroupBy(s => s.GROUP_ID).Select(s => new Mrs00377RDO { GROUP_ID = s.First().GROUP_ID, GROUP_NAME = s.First().GROUP_NAME }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public void ProcessBeginAmount(HIS_MEDI_STOCK mediStock)
        {
            HisMediStockPeriodViewFilterQuery mediStockPeriodViewFilter = new HisMediStockPeriodViewFilterQuery();
            mediStockPeriodViewFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
            mediStockPeriodViewFilter.MEDI_STOCK_ID = mediStock.ID;
            var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).GetView(mediStockPeriodViewFilter);

            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                // có kỳ kiểm kê
                #region medicine with period
                var period = listMediStockPeriods.OrderByDescending(s => s.CREATE_TIME).First();
                // tồn đầu thuốc
                HisMestPeriodMediViewFilterQuery mestPeriodMediViewFilter = new HisMestPeriodMediViewFilterQuery();
                mestPeriodMediViewFilter.MEDI_STOCK_PERIOD_ID = period.ID;
                var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(param).GetView(mestPeriodMediViewFilter);
                if (IsNotNullOrEmpty(listMestPeriodMedis))
                {
                    foreach (var medi in listMestPeriodMedis)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MEDI;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = medi.MEDICINE_ID;

                        rdo.SERVICE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;

                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = medi.AMOUNT;
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = medi.AMOUNT;
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = medi.AMOUNT;
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = medi.AMOUNT;
                        rdo.BEGIN_AMOUNT_TVIEN = medi.AMOUNT;
                        ListRdoMedicines.Add(rdo);
                    }

                    // nhập trước kỳ
                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                    impMestMedicineViewFilter.IMP_TIME_FROM = period.CREATE_TIME + 1;
                    impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                    if (IsNotNullOrEmpty(listImpMestMedicines))
                    {
                        var impGroups = listImpMestMedicines.GroupBy(s => s.MEDICINE_ID).ToList();
                        foreach (var imp in impGroups)
                        {
                            var rdo = new Mrs00377RDO();
                            rdo.GROUP_ID = GROUP_ID__MEDI;
                            rdo.GROUP_NAME = "THUỐC";
                            rdo.MEDI_STOCK_ID = mediStock.ID;
                            rdo.SERVICE_ID = imp.First().MEDICINE_ID;

                            rdo.SERVICE_TYPE_ID = imp.First().MEDICINE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = imp.First().MEDICINE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = imp.First().MEDICINE_TYPE_NAME;

                            rdo.CONCENTRA = imp.First().CONCENTRA;

                            rdo.SERVICE_UNIT_NAME = imp.First().SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = imp.First().IMP_PRICE;

                            if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = imp.Sum(s => s.AMOUNT);
                            else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = imp.Sum(s => s.AMOUNT);
                            else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = imp.Sum(s => s.AMOUNT);
                            else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = imp.Sum(s => s.AMOUNT);
                            rdo.BEGIN_AMOUNT_TVIEN = imp.Sum(s => s.AMOUNT);
                            ListRdoMedicines.Add(rdo);
                        }
                    }

                    // xuất trước kỳ
                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                    expMestMedicineViewFilter.EXP_TIME_FROM = period.CREATE_TIME + 1;
                    expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                    if (IsNotNullOrEmpty(listExpMestMedicines))
                    {
                        var expGroups = listExpMestMedicines.GroupBy(s => s.MEDICINE_ID).ToList();
                        foreach (var exp in expGroups)
                        {
                            var rdo = new Mrs00377RDO();
                            rdo.GROUP_ID = GROUP_ID__MEDI;
                            rdo.GROUP_NAME = "THUỐC";
                            rdo.MEDI_STOCK_ID = mediStock.ID;
                            rdo.SERVICE_ID = exp.First().MEDICINE_ID ?? 0;

                            rdo.SERVICE_TYPE_ID = exp.First().MEDICINE_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = exp.First().MEDICINE_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = exp.First().MEDICINE_TYPE_NAME;

                            var medicineType = listMedicineTypes.Where(s => s.ID == exp.First().MEDICINE_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                            rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = exp.First().IMP_PRICE;

                            if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = exp.Sum(s => -(s.AMOUNT));
                            else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = exp.Sum(s => -(s.AMOUNT));
                            else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = exp.Sum(s => -(s.AMOUNT));
                            else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = exp.Sum(s => -(s.AMOUNT));
                            rdo.BEGIN_AMOUNT_TVIEN = exp.Sum(s => -(s.AMOUNT));
                            ListRdoMedicines.Add(rdo);
                        }
                    }

                    ListRdoMedicines = ListRdoMedicines.GroupBy(g => g.SERVICE_ID).Select(s => new Mrs00377RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        SERVICE_ID = s.First().SERVICE_ID,

                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                        CONCENTRA = s.First().CONCENTRA,

                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                        BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                        IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                        EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                        END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                        BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                        IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                        IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                        EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                        END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                        BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                        IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                        IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                        EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                        END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                        BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                        IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG),
                        IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                        EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                        END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG)
                    }).ToList();
                }
                #endregion

                #region material with period
                // tồn đầu vật tư
                HisMestPeriodMateViewFilterQuery mestPeriodMateViewFilter = new HisMestPeriodMateViewFilterQuery();
                mestPeriodMateViewFilter.MEDI_STOCK_PERIOD_ID = period.ID;
                var listMestPeriodMates = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(param).GetView(mestPeriodMateViewFilter);
                if (IsNotNullOrEmpty(listMestPeriodMates))
                {
                    foreach (var mate in listMestPeriodMates)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MATE;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = mate.MATERIAL_ID;

                        rdo.SERVICE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;

                        var medicineType = listMaterialTypes.Where(s => s.ID == mate.MATERIAL_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = mate.AMOUNT;
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = mate.AMOUNT;
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = mate.AMOUNT;
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = mate.AMOUNT;
                        rdo.BEGIN_AMOUNT_TVIEN = mate.AMOUNT;
                        ListRdoMaterials.Add(rdo);
                    }

                    // nhập trước kỳ
                    HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    impMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                    impMestMaterialViewFilter.IMP_TIME_FROM = period.CREATE_TIME + 1;
                    impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                    impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                    if (IsNotNullOrEmpty(listImpMestMaterials))
                    {
                        var impGroups = listImpMestMaterials.GroupBy(s => s.MATERIAL_ID).ToList();
                        foreach (var imp in impGroups)
                        {
                            var rdo = new Mrs00377RDO();
                            rdo.GROUP_ID = GROUP_ID__MATE;
                            rdo.GROUP_NAME = "VẬT TƯ";
                            rdo.MEDI_STOCK_ID = mediStock.ID;
                            rdo.SERVICE_ID = imp.First().MATERIAL_ID;

                            rdo.SERVICE_TYPE_ID = imp.First().MATERIAL_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = imp.First().MATERIAL_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = imp.First().MATERIAL_TYPE_NAME;

                            var materialType = listMaterialTypes.Where(s => s.ID == imp.First().MATERIAL_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(materialType)) rdo.CONCENTRA = materialType.First().CONCENTRA;

                            rdo.SERVICE_UNIT_NAME = imp.First().SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = imp.First().IMP_PRICE;

                            if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = imp.Sum(s => s.AMOUNT);
                            else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = imp.Sum(s => s.AMOUNT);
                            else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = imp.Sum(s => s.AMOUNT);
                            rdo.BEGIN_AMOUNT_TVIEN = imp.Sum(s => s.AMOUNT);
                            ListRdoMaterials.Add(rdo);
                        }
                    }

                    // xuất trước kỳ
                    HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                    expMestMaterialViewFilter.EXP_TIME_FROM = period.CREATE_TIME + 1;
                    expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                    expMestMaterialViewFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
                    if (IsNotNullOrEmpty(listExpMestMaterials))
                    {
                        var expGroups = listExpMestMaterials.GroupBy(s => s.MATERIAL_ID).ToList();
                        foreach (var exp in expGroups)
                        {
                            var rdo = new Mrs00377RDO();
                            rdo.GROUP_ID = GROUP_ID__MATE;
                            rdo.GROUP_NAME = "VẬT TƯ";
                            rdo.MEDI_STOCK_ID = mediStock.ID;
                            rdo.SERVICE_ID = exp.First().MATERIAL_ID ?? 0;

                            rdo.SERVICE_TYPE_ID = exp.First().MATERIAL_TYPE_ID;
                            rdo.SERVICE_TYPE_CODE = exp.First().MATERIAL_TYPE_CODE;
                            rdo.SERVICE_TYPE_NAME = exp.First().MATERIAL_TYPE_NAME;

                            var materialType = listMaterialTypes.Where(s => s.ID == exp.First().MATERIAL_TYPE_ID).ToList();
                            if (IsNotNullOrEmpty(materialType)) rdo.CONCENTRA = materialType.First().CONCENTRA;

                            rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                            rdo.IMP_PRICE = exp.First().IMP_PRICE;

                            if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = exp.Sum(s => -(s.AMOUNT));
                            else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = exp.Sum(s => -(s.AMOUNT));
                            else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = exp.Sum(s => -(s.AMOUNT));
                            rdo.BEGIN_AMOUNT_TVIEN = exp.Sum(s => -(s.AMOUNT));
                            ListRdoMaterials.Add(rdo);
                        }
                    }

                    ListRdoMaterials = ListRdoMaterials.GroupBy(g => g.SERVICE_ID).Select(s => new Mrs00377RDO
                    {
                        GROUP_ID = s.First().GROUP_ID,
                        GROUP_NAME = s.First().GROUP_NAME,
                        MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                        SERVICE_ID = s.First().SERVICE_ID,

                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                        CONCENTRA = s.First().CONCENTRA,

                        SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                        IMP_PRICE = s.First().IMP_PRICE,
                        BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                        BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                        IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                        EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                        END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                        BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                        IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                        IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                        EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                        END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                        BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                        IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                        IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                        EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                        END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                        BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                        IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG),
                        IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                        EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                        END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG)
                    }).ToList();
                }
                #endregion
            }
            else
            {
                // không có kỳ kiểm kê
                #region medicine
                // nhập trước kỳ
                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                if (IsNotNullOrEmpty(listImpMestMedicines))
                {
                    var impGroups = listImpMestMedicines.GroupBy(s => s.MEDICINE_ID).ToList();
                    foreach (var imp in impGroups)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MEDI;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = imp.First().MEDICINE_ID;

                        rdo.SERVICE_TYPE_ID = imp.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = imp.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = imp.First().MEDICINE_TYPE_NAME;

                        rdo.CONCENTRA = imp.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = imp.First().SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = imp.First().IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = imp.Sum(s => s.AMOUNT);
                        rdo.BEGIN_AMOUNT_TVIEN = imp.Sum(s => s.AMOUNT);
                        ListRdoMedicines.Add(rdo);
                    }
                }

                // xuất trước kỳ
                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMedicineViewFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    var expGroups = listExpMestMedicines.GroupBy(s => s.MEDICINE_ID).ToList();
                    foreach (var exp in expGroups)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MEDI;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = exp.First().MEDICINE_ID ?? 0;

                        rdo.SERVICE_TYPE_ID = exp.First().MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = exp.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = exp.First().MEDICINE_TYPE_NAME;

                        var medicineType = listMedicineTypes.Where(s => s.ID == exp.First().MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = exp.First().IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = exp.Sum(s => -(s.AMOUNT));
                        rdo.BEGIN_AMOUNT_TVIEN = exp.Sum(s => -(s.AMOUNT));
                        ListRdoMedicines.Add(rdo);
                    }
                }

                ListRdoMedicines = ListRdoMedicines.GroupBy(g => new { g.SERVICE_ID, g.SERVICE_UNIT_NAME, g.IMP_PRICE }).Select(s => new Mrs00377RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    SERVICE_ID = s.First().SERVICE_ID,

                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                    CONCENTRA = s.First().CONCENTRA,

                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                    BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                    IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                    EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                    END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                    BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                    IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                    IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                    EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                    END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                    BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                    IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                    IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                    EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                    END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                    BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                    IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG),
                    IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                    EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                    END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG)
                }).ToList();
                #endregion
                #region material
                // nhập trước kỳ
                HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                impMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
                if (IsNotNullOrEmpty(listImpMestMaterials))
                {
                    var impGroups = listImpMestMaterials.GroupBy(s => s.MATERIAL_ID).ToList();
                    foreach (var imp in impGroups)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MATE;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = imp.First().MATERIAL_ID;

                        rdo.SERVICE_TYPE_ID = imp.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = imp.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = imp.First().MATERIAL_TYPE_NAME;

                        var materialType = listMaterialTypes.Where(s => s.ID == imp.First().MATERIAL_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(materialType)) rdo.CONCENTRA = materialType.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = imp.First().SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = imp.First().IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = imp.Sum(s => s.AMOUNT);
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = imp.Sum(s => s.AMOUNT);
                        rdo.BEGIN_AMOUNT_TVIEN = imp.Sum(s => s.AMOUNT);
                        ListRdoMaterials.Add(rdo);
                    }
                }

                // xuất trước kỳ
                HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMaterialViewFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
                if (IsNotNullOrEmpty(listExpMestMaterials))
                {
                    var expGroups = listExpMestMaterials.GroupBy(s => s.MATERIAL_ID).ToList();
                    foreach (var exp in expGroups)
                    {
                        var rdo = new Mrs00377RDO();
                        rdo.GROUP_ID = GROUP_ID__MATE;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.MEDI_STOCK_ID = mediStock.ID;
                        rdo.SERVICE_ID = exp.First().MATERIAL_ID ?? 0;

                        rdo.SERVICE_TYPE_ID = exp.First().MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = exp.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = exp.First().MATERIAL_TYPE_NAME;

                        var materialType = listMaterialTypes.Where(s => s.ID == exp.First().MATERIAL_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(materialType)) rdo.CONCENTRA = materialType.First().CONCENTRA;

                        rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = exp.First().IMP_PRICE;

                        if (IdKhoTong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_TONG = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoNoiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NOI_TRU = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoNgoaiTru.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_NGOAI_TRU = exp.Sum(s => -(s.AMOUNT));
                        else if (IdKhoCongDong.Contains(mediStock.ID)) rdo.BEGIN_AMOUNT_CONG_DONG = exp.Sum(s => -(s.AMOUNT));
                        rdo.BEGIN_AMOUNT_TVIEN = exp.Sum(s => -(s.AMOUNT));
                        ListRdoMaterials.Add(rdo);
                    }
                }

                ListRdoMaterials = ListRdoMaterials.GroupBy(g => new { g.SERVICE_ID, g.SERVICE_UNIT_NAME, g.IMP_PRICE }).Select(s => new Mrs00377RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    SERVICE_ID = s.First().SERVICE_ID,

                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                    CONCENTRA = s.First().CONCENTRA,

                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                    BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                    IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                    EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                    END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                    BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                    IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                    IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                    EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                    END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                    BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                    IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                    IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                    EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                    END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                    BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                    IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG),
                    IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                    EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                    END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG)
                }).ToList();
                #endregion
            }
        }

        public void ProcessImpMestAmount(HIS_MEDI_STOCK mediStock)
        {
            HisImpMestViewFilterQuery chmsImpMestViewFilter = new HisImpMestViewFilterQuery();
            chmsImpMestViewFilter.MEDI_STOCK_ID = mediStock.ID;
            chmsImpMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            chmsImpMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            chmsImpMestViewFilter.IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
            chmsImpMestViewFilter.IMP_MEST_TYPE_IDs = this.CHMS_IMP_MEST_TYPE_IDs;
            List<V_HIS_IMP_MEST> listChmsImpMests = new HisImpMestManager(param).GetView(chmsImpMestViewFilter);

            #region medicine
            HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
            impMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
            impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMedicineViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
            if (IsNotNullOrEmpty(listImpMestMedicines))
            {
                foreach (var impMestMedicine in listImpMestMedicines)
                {
                    var rdo = new Mrs00377RDO();
                    rdo.GROUP_ID = GROUP_ID__MEDI;
                    rdo.GROUP_NAME = "THUỐC";
                    rdo.MEDI_STOCK_ID = mediStock.ID;
                    rdo.SERVICE_ID = impMestMedicine.MEDICINE_ID;

                    rdo.SERVICE_TYPE_ID = impMestMedicine.MEDICINE_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;

                    var medicineType = listMedicineTypes.Where(s => s.ID == impMestMedicine.MEDICINE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                    rdo.SERVICE_UNIT_NAME = impMestMedicine.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = impMestMedicine.IMP_PRICE;

                    if (IdKhoTong.Contains(mediStock.ID)) rdo.IMP_AMOUNT_TONG = impMestMedicine.AMOUNT;
                    else if (IdKhoNoiTru.Contains(mediStock.ID))
                    {
                        rdo.IMP_AMOUNT_NOI_TRU = impMestMedicine.AMOUNT;
                        if (listChmsImpMests.Select(s => s.ID).Contains(impMestMedicine.IMP_MEST_ID))
                        {
                            //if (IsNotNullOrEmpty(listMediStockTongs.Where(s => listChmsImpMests.Select(c => c.EXP_MEDI_STOCK_CODE).Contains(s.MEDI_STOCK_CODE)).ToList()))
                            //    rdo.IMP_AMOUNT_NOI_TRU_KT = impMestMedicine.AMOUNT; 
                        }

                    }
                    else if (IdKhoNgoaiTru.Contains(mediStock.ID))
                    {
                        rdo.IMP_AMOUNT_NGOAI_TRU = impMestMedicine.AMOUNT;
                        if (listChmsImpMests.Select(s => s.ID).Contains(impMestMedicine.IMP_MEST_ID))
                        {
                            //if (IsNotNullOrEmpty(listMediStockTongs.Where(s => listChmsImpMests.Select(c => c.EXP_MEDI_STOCK_CODE).Contains(s.MEDI_STOCK_CODE)).ToList()))
                            //    rdo.IMP_AMOUNT_NGOAI_TRU_KT = impMestMedicine.AMOUNT; 
                        }

                    }
                    else if (IdKhoCongDong.Contains(mediStock.ID))
                    {
                        rdo.IMP_AMOUNT_CONG_DONG = impMestMedicine.AMOUNT;
                        if (listChmsImpMests.Select(s => s.ID).Contains(impMestMedicine.IMP_MEST_ID))
                        {
                            //if (IsNotNullOrEmpty(listMediStockTongs.Where(s => listChmsImpMests.Select(c => c.EXP_MEDI_STOCK_CODE).Contains(s.MEDI_STOCK_CODE)).ToList()))
                            //    rdo.IMP_AMOUNT_CONG_DONG_KT = impMestMedicine.AMOUNT; 
                        }

                    }
                    rdo.IMP_AMOUNT_TVIEN = impMestMedicine.AMOUNT;
                    ListRdoMedicines.Add(rdo);
                }
            }

            ListRdoMedicines = ListRdoMedicines.GroupBy(g => new { g.SERVICE_ID, g.SERVICE_UNIT_NAME, g.IMP_PRICE }).Select(s => new Mrs00377RDO
            {
                GROUP_ID = s.First().GROUP_ID,
                GROUP_NAME = s.First().GROUP_NAME,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                SERVICE_ID = s.First().SERVICE_ID,

                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                CONCENTRA = s.First().CONCENTRA,

                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                IMP_PRICE = s.First().IMP_PRICE,

                BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                IMP_AMOUNT_TVIEN = s.Sum(su => su.IMP_AMOUNT_TVIEN),
                BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG),

                IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),

                IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG)
            }).ToList();
            #endregion
            #region material
            HisImpMestMaterialViewFilterQuery impMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
            impMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
            impMestMaterialViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            impMestMaterialViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
            impMestMaterialViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new HisImpMestMaterialManager(param).GetView(impMestMaterialViewFilter);
            if (IsNotNullOrEmpty(listImpMestMaterials))
            {
                foreach (var impMestMaterial in listImpMestMaterials)
                {
                    var rdo = new Mrs00377RDO();
                    rdo.GROUP_ID = GROUP_ID__MATE;
                    rdo.GROUP_NAME = "VẬT TƯ";
                    rdo.MEDI_STOCK_ID = mediStock.ID;
                    rdo.SERVICE_ID = impMestMaterial.MATERIAL_ID;

                    rdo.SERVICE_TYPE_ID = impMestMaterial.MATERIAL_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = impMestMaterial.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = impMestMaterial.MATERIAL_TYPE_NAME;

                    var medicineType = listMedicineTypes.Where(s => s.ID == impMestMaterial.MATERIAL_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                    rdo.SERVICE_UNIT_NAME = impMestMaterial.SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = impMestMaterial.IMP_PRICE;

                    if (IdKhoTong.Contains(mediStock.ID)) rdo.IMP_AMOUNT_TONG = impMestMaterial.AMOUNT;
                    else if (IdKhoNoiTru.Contains(mediStock.ID))
                    {
                        rdo.IMP_AMOUNT_NOI_TRU = impMestMaterial.AMOUNT;
                        if (listChmsImpMests.Select(s => s.ID).Contains(impMestMaterial.IMP_MEST_ID))
                        {
                            //if (IsNotNullOrEmpty(listMediStockTongs.Where(s => listChmsImpMests.Select(c => c.EXP_MEDI_STOCK_CODE).Contains(s.MEDI_STOCK_CODE)).ToList()))
                            //    rdo.IMP_AMOUNT_NOI_TRU_KT = impMestMaterial.AMOUNT; 
                        }


                    }
                    else if (IdKhoNgoaiTru.Contains(mediStock.ID))
                    {
                        rdo.IMP_AMOUNT_NGOAI_TRU = impMestMaterial.AMOUNT;
                        if (listChmsImpMests.Select(s => s.ID).Contains(impMestMaterial.IMP_MEST_ID))
                        {
                            //if (IsNotNullOrEmpty(listMediStockTongs.Where(s => listChmsImpMests.Select(c => c.EXP_MEDI_STOCK_CODE).Contains(s.MEDI_STOCK_CODE)).ToList()))
                            //    rdo.IMP_AMOUNT_NGOAI_TRU_KT = impMestMaterial.AMOUNT; 
                        }

                    }
                    rdo.IMP_AMOUNT_TVIEN = impMestMaterial.AMOUNT;
                    ListRdoMaterials.Add(rdo);
                }
            }

            ListRdoMaterials = ListRdoMaterials.GroupBy(g => new { g.SERVICE_TYPE_CODE, g.IMP_PRICE, g.SERVICE_UNIT_NAME }).Select(s => new Mrs00377RDO
            {
                GROUP_ID = s.First().GROUP_ID,
                GROUP_NAME = s.First().GROUP_NAME,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                SERVICE_ID = s.First().SERVICE_ID,

                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                CONCENTRA = s.First().CONCENTRA,

                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                IMP_PRICE = s.First().IMP_PRICE,
                BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                IMP_AMOUNT_TVIEN = s.Sum(su => su.IMP_AMOUNT_TVIEN),
                BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NOI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_MOBA),
                IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_NGOAI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_MOBA),
                IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                IMP_AMOUNT_CONG_DONG_MOBA = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_MOBA),
                IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG),

                IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG)
            }).ToList();
            #endregion
        }

        public void ProcessExpMestAmount(HIS_MEDI_STOCK mediStock)
        {
            #region medicine
            // xuất trong kỳ
            HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
            expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
            expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMedicineViewFilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
            List<V_HIS_IMP_MEST> listMobaImpMests = new List<V_HIS_IMP_MEST>();
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
            List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();

            if (IsNotNullOrEmpty(listExpMestMedicines))
            {

                //thu hoi
                HisImpMestViewFilterQuery mobaImpMestViewFilter = new HisImpMestViewFilterQuery();
                mobaImpMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                mobaImpMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                mobaImpMestViewFilter.IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT };
                mobaImpMestViewFilter.IMP_MEST_TYPE_IDs = this.MOBA_IMP_MEST_TYPE_IDs;
                listMobaImpMests = new HisImpMestManager(param).GetView(mobaImpMestViewFilter);
                var skip = 0;
                while (listMobaImpMests.Count - skip > 0)
                {
                    //thu hoi
                    var lists = listMobaImpMests.Select(o => o.ID).ToList().Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestMedicineViewFilterQuery ImpMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    ImpMestMedicineViewFilter.IMP_MEST_IDs = lists;
                    var listImpMestMedicineSub = new HisImpMestMedicineManager(param).GetView(ImpMestMedicineViewFilter);

                    listImpMestMedicine.AddRange(listImpMestMedicineSub);

                }
                skip = 0;
                while (listMobaImpMests.Count - skip > 0)
                {
                    //thu hoi
                    var lists = listMobaImpMests.Select(o => o.ID).ToList().Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestMaterialViewFilterQuery ImpMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery();
                    ImpMestMaterialViewFilter.IMP_MEST_IDs = lists;
                    var listImpMestMaterialSub = new HisImpMestMaterialManager(param).GetView(ImpMestMaterialViewFilter);

                    listImpMestMaterial.AddRange(listImpMestMaterialSub);

                }
            }


            if (IsNotNullOrEmpty(listExpMestMedicines))
            {
                var expGroups = listExpMestMedicines.GroupBy(s => s.MEDICINE_ID).ToList();
                foreach (var exp in expGroups)
                {
                    var rdo = new Mrs00377RDO();
                    rdo.GROUP_ID = GROUP_ID__MEDI;
                    rdo.GROUP_NAME = "THUỐC";
                    rdo.MEDI_STOCK_ID = mediStock.ID;
                    rdo.SERVICE_ID = exp.First().MEDICINE_ID ?? 0;

                    rdo.SERVICE_TYPE_ID = exp.First().MEDICINE_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = exp.First().MEDICINE_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = exp.First().MEDICINE_TYPE_NAME;

                    var medicineType = listMedicineTypes.Where(s => s.ID == exp.First().MEDICINE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType)) rdo.CONCENTRA = medicineType.First().CONCENTRA;

                    rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = exp.First().IMP_PRICE;

                    if (IdKhoTong.Contains(mediStock.ID)) rdo.EXP_AMOUNT_TONG = exp.Sum(s => s.AMOUNT);
                    else if (IdKhoNoiTru.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_NOI_TRU = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho nội trú
                        var listImpMestMedicineSub = listImpMestMedicine.Where(o => exp.Select(p => p.MEDICINE_ID).ToList().Contains(o.MEDICINE_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMedicineSub))
                            rdo.IMP_AMOUNT_NOI_TRU_MOBA = listImpMestMedicineSub.Count > 0 ? listImpMestMedicineSub.Sum(q => q.AMOUNT) : 0;
                    }
                    else if (IdKhoNgoaiTru.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_NGOAI_TRU = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho ngoại trú
                        var listImpMestMedicineSub = listImpMestMedicine.Where(o => exp.Select(p => p.MEDICINE_ID).ToList().Contains(o.MEDICINE_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMedicineSub))
                            rdo.IMP_AMOUNT_NGOAI_TRU_MOBA = listImpMestMedicineSub.Count > 0 ? listImpMestMedicineSub.Sum(q => q.AMOUNT) : 0;
                    }
                    else if (IdKhoCongDong.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_CONG_DONG = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho cộng đồng
                        var listImpMestMedicineSub = listImpMestMedicine.Where(o => exp.Select(p => p.MEDICINE_ID).ToList().Contains(o.MEDICINE_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMedicineSub))
                            rdo.IMP_AMOUNT_CONG_DONG_MOBA = listImpMestMedicineSub.Count > 0 ? listImpMestMedicineSub.Sum(q => q.AMOUNT) : 0;
                    }
                    rdo.EXP_AMOUNT_TVIEN = exp.Sum(s => s.AMOUNT);
                    ListRdoMedicines.Add(rdo);
                }
            }

            ListRdoMedicines = ListRdoMedicines.GroupBy(g => new { g.SERVICE_TYPE_CODE, g.IMP_PRICE, g.SERVICE_UNIT_NAME }).Select(s => new Mrs00377RDO
            {
                GROUP_ID = s.First().GROUP_ID,
                GROUP_NAME = s.First().GROUP_NAME,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                SERVICE_ID = s.First().SERVICE_ID,

                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                CONCENTRA = s.First().CONCENTRA,

                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                IMP_PRICE = s.First().IMP_PRICE,

                BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                IMP_AMOUNT_TVIEN = s.Sum(su => su.IMP_AMOUNT_TVIEN),
                EXP_AMOUNT_TVIEN = s.Sum(su => su.EXP_AMOUNT_TVIEN),
                BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NOI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_MOBA),
                IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_NGOAI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_MOBA),
                IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                IMP_AMOUNT_CONG_DONG_MOBA = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_MOBA),
                IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG),

                IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG)
            }).ToList();
            #endregion
            #region material
            // xuất trong kỳ
            HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
            expMestMaterialViewFilter.MEDI_STOCK_ID = mediStock.ID;
            expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
            expMestMaterialViewFilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
            if (IsNotNullOrEmpty(listExpMestMaterials))
            {
                var expGroups = listExpMestMaterials.GroupBy(s => s.MATERIAL_ID).ToList();
                foreach (var exp in expGroups)
                {
                    var rdo = new Mrs00377RDO();
                    rdo.GROUP_ID = GROUP_ID__MATE;
                    rdo.GROUP_NAME = "VẬT TƯ";
                    rdo.MEDI_STOCK_ID = mediStock.ID;
                    rdo.SERVICE_ID = exp.First().MATERIAL_ID ?? 0;

                    rdo.SERVICE_TYPE_ID = exp.First().MATERIAL_TYPE_ID;
                    rdo.SERVICE_TYPE_CODE = exp.First().MATERIAL_TYPE_CODE;
                    rdo.SERVICE_TYPE_NAME = exp.First().MATERIAL_TYPE_NAME;

                    var materialType = listMaterialTypes.Where(s => s.ID == exp.First().MATERIAL_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(materialType)) rdo.CONCENTRA = materialType.First().CONCENTRA;

                    rdo.SERVICE_UNIT_NAME = exp.First().SERVICE_UNIT_NAME;
                    rdo.IMP_PRICE = exp.First().IMP_PRICE;

                    if (IdKhoTong.Contains(mediStock.ID)) rdo.EXP_AMOUNT_TONG = exp.Sum(s => s.AMOUNT);
                    else if (IdKhoNoiTru.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_NOI_TRU = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho nội trú
                        var listImpMestMaterialSub = listImpMestMaterial.Where(o => exp.Select(p => p.MATERIAL_ID).ToList().Contains(o.MATERIAL_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMaterialSub))
                            rdo.IMP_AMOUNT_NOI_TRU_MOBA = listImpMestMaterialSub.Count > 0 ? listImpMestMaterialSub.Sum(q => q.AMOUNT) : 0;
                    }
                    else if (IdKhoNgoaiTru.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_NGOAI_TRU = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho ngoại trú
                        var listImpMestMaterialSub = listImpMestMaterial.Where(o => exp.Select(p => p.MATERIAL_ID).ToList().Contains(o.MATERIAL_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMaterialSub))
                            rdo.IMP_AMOUNT_NGOAI_TRU_MOBA = listImpMestMaterialSub.Count > 0 ? listImpMestMaterialSub.Sum(q => q.AMOUNT) : 0;
                    }
                    else if (IdKhoCongDong.Contains(mediStock.ID))
                    {
                        rdo.EXP_AMOUNT_CONG_DONG = exp.Sum(s => s.AMOUNT);
                        //Nhập thu hồi kho ngoại trú
                        var listImpMestMaterialSub = listImpMestMaterial.Where(o => exp.Select(p => p.MATERIAL_ID).ToList().Contains(o.MATERIAL_ID)).ToList();
                        if (IsNotNullOrEmpty(listImpMestMaterialSub))
                            rdo.IMP_AMOUNT_CONG_DONG_MOBA = listImpMestMaterialSub.Count > 0 ? listImpMestMaterialSub.Sum(q => q.AMOUNT) : 0;
                    }
                    rdo.EXP_AMOUNT_TVIEN = exp.Sum(s => s.AMOUNT);
                    ListRdoMaterials.Add(rdo);
                }
            }

            ListRdoMaterials = ListRdoMaterials.GroupBy(g => new { g.SERVICE_ID, g.SERVICE_UNIT_NAME, g.IMP_PRICE }).Select(s => new Mrs00377RDO
            {
                GROUP_ID = s.First().GROUP_ID,
                GROUP_NAME = s.First().GROUP_NAME,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                SERVICE_ID = s.First().SERVICE_ID,

                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,

                CONCENTRA = s.First().CONCENTRA,

                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                IMP_PRICE = s.First().IMP_PRICE,

                BEGIN_AMOUNT_TVIEN = s.Sum(su => su.BEGIN_AMOUNT_TVIEN),
                IMP_AMOUNT_TVIEN = s.Sum(su => su.IMP_AMOUNT_TVIEN),
                EXP_AMOUNT_TVIEN = s.Sum(su => su.EXP_AMOUNT_TVIEN),
                BEGIN_AMOUNT_TONG = s.Sum(su => su.BEGIN_AMOUNT_TONG),
                IMP_AMOUNT_TONG = s.Sum(su => su.IMP_AMOUNT_TONG),
                EXP_AMOUNT_TONG = s.Sum(su => su.EXP_AMOUNT_TONG),
                END_AMOUNT_TONG = s.Sum(su => su.END_AMOUNT_TONG),

                BEGIN_AMOUNT_NOI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NOI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_MOBA),
                IMP_AMOUNT_NOI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NOI_TRU_KT),
                EXP_AMOUNT_NOI_TRU = s.Sum(su => su.EXP_AMOUNT_NOI_TRU),
                END_AMOUNT_NOI_TRU = s.Sum(su => su.END_AMOUNT_NOI_TRU),

                BEGIN_AMOUNT_NGOAI_TRU = s.Sum(su => su.BEGIN_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_NGOAI_TRU_MOBA = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_MOBA),
                IMP_AMOUNT_NGOAI_TRU_KT = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU_KT),
                EXP_AMOUNT_NGOAI_TRU = s.Sum(su => su.EXP_AMOUNT_NGOAI_TRU),
                END_AMOUNT_NGOAI_TRU = s.Sum(su => su.END_AMOUNT_NGOAI_TRU),

                BEGIN_AMOUNT_CONG_DONG = s.Sum(su => su.BEGIN_AMOUNT_CONG_DONG),
                IMP_AMOUNT_CONG_DONG_MOBA = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_MOBA),
                IMP_AMOUNT_CONG_DONG_KT = s.Sum(su => su.IMP_AMOUNT_CONG_DONG_KT),
                EXP_AMOUNT_CONG_DONG = s.Sum(su => su.EXP_AMOUNT_CONG_DONG),
                END_AMOUNT_CONG_DONG = s.Sum(su => su.END_AMOUNT_CONG_DONG),

                IMP_AMOUNT_NOI_TRU = s.Sum(su => su.IMP_AMOUNT_NOI_TRU),
                IMP_AMOUNT_NGOAI_TRU = s.Sum(su => su.IMP_AMOUNT_NGOAI_TRU),
                IMP_AMOUNT_CONG_DONG = s.Sum(su => su.IMP_AMOUNT_CONG_DONG)
            }).ToList();
            #endregion
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

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(o => o.SERVICE_TYPE_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "RdoGroup", ListRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "Rdo", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = new List<long>();
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                string[] ar = value.Split(new char[] { ',' });
                if (ar != null && ar.Count() > 0)
                {
                    HisMediStockFilterQuery filter = new HisMediStockFilterQuery();
                    //filter.DEPARTMENT_CODE = value; //TODO
                    var data = new MOS.MANAGER.HisMediStock.HisMediStockManager().Get(filter);
                    if (data != null && data.Count > 0)
                    {
                        //tìm chính xác mã kho theo cấu hình
                        var lstMediStock = new List<HIS_MEDI_STOCK>();
                        foreach (var item in ar)
                        {
                            var stocks = data.Where(o => o.MEDI_STOCK_CODE == item).ToList();
                            if (stocks != null && stocks.Count > 0)
                            {
                                lstMediStock.AddRange(stocks);
                            }
                        }

                        if (lstMediStock != null && lstMediStock.Count > 0)
                            data = lstMediStock;
                        else
                            data = null;
                    }

                    if (!(data != null && data.Count > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                    result = data.Select(o => o.ID).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }
    }
}
