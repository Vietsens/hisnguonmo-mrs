using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
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

namespace MRS.Processor.Mrs00410
{
    class Mrs00410Processor : AbstractProcessor
    {
        Mrs00410Filter castFilter = null;
        int countImp = 0;
        int countExp = 0;
        List<Mrs00410RDO> ListRdo = new List<Mrs00410RDO>();

        List<HIS_MEDI_STOCK> listMediStocks = new List<HIS_MEDI_STOCK>();

        List<HIS_MEDICINE_TYPE> listMedicineTypes = new List<HIS_MEDICINE_TYPE>();

        List<HIS_IMP_MEST> listImpMests = new List<HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedis = new List<V_HIS_IMP_MEST_MEDICINE>();

        List<HIS_EXP_MEST> listExpMests = new List<HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedis = new List<V_HIS_EXP_MEST_MEDICINE>();
        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();
        Dictionary<long, PropertyInfo> dicExpAmountType = new Dictionary<long, PropertyInfo>();
        Dictionary<long, PropertyInfo> dicImpAmountType = new Dictionary<long, PropertyInfo>();

        List<long> MOBA_IMP_MEST_TYPE_IDs = new List<long>()
        {
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
            IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL
        };

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
            IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BL
        };

        List<long> GN_HT_IDs = new List<long>();

        public Mrs00410Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00410Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00410Filter)this.reportFilter;

                if (this.castFilter.IS_GN == 1)
                {
                    GN_HT_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN);
                }
                if (this.castFilter.IS_GN == 0)
                {
                    GN_HT_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT);
                }
                if (this.castFilter.IS_GN == null)
                {
                    GN_HT_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN);
                    GN_HT_IDs.Add(IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT);
                }
                //cac nhom thuoc mo rong
                if (this.castFilter.MEDICINE_GROUP_IDs != null)
                {
                    GN_HT_IDs.AddRange(this.castFilter.MEDICINE_GROUP_IDs);
                }
                //Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00410: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                //Tao loai nhap xuat
                makeRdo();
                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.IDs = this.castFilter.MEDI_STOCK_IDs;
                listMediStocks = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);

                HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                listMedicineTypes = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).Get(medicineTypeFilter);
                listMedicineTypes = listMedicineTypes.Where(s => GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0) /*s.IS_ADDICTIVE != null || s.IS_NEUROLOGICAL != null*/).ToList();

                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestFilter.MEDI_STOCK_IDs= this.castFilter.MEDI_STOCK_IDs;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).Get(impMestFilter);
                //Inventec.Common.Logging.LogSystem.Info("listImpMests" + listImpMests.Count);
                var skip = 0;
                while (listImpMests.Count - skip > 0)
                {
                    var listIds = listImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                    impMestMedicineViewFilter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    listImpMestMedis.AddRange(new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter));
                }
                listImpMestMedis = listImpMestMedis.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_ADDICTIVE != null || s.IS_NEUROLOGICAL != null*/) && (this.castFilter.MEDI_STOCK_IDs != null || (!CHMS_IMP_MEST_TYPE_IDs.Contains(s.IMP_MEST_TYPE_ID)))).ToList();
                //Inventec.Common.Logging.LogSystem.Info("listImpMestMedis" + listImpMestMedis.Count);
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expMestFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expMestFilter.MEDI_STOCK_IDs = this.castFilter.MEDI_STOCK_IDs;
                listExpMests = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expMestFilter);
                //Inventec.Common.Logging.LogSystem.Info("listExpMests" + listExpMests.Count);
                skip = 0;
                while (listExpMests.Count - skip > 0)
                {
                    var listIds = listExpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicineViewFilter.EXP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    expMestMedicineViewFilter.IS_EXPORT = true;
                    listExpMestMedis.AddRange(new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter));
                }
                listExpMestMedis = listExpMestMedis.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_ADDICTIVE != null || s.IS_NEUROLOGICAL != null*/) && (this.castFilter.MEDI_STOCK_IDs!=null||(!CHMS_EXP_MEST_TYPE_IDs.Contains(s.EXP_MEST_TYPE_ID)))).ToList();
                //Inventec.Common.Logging.LogSystem.Info("listExpMestMedis" + listExpMestMedis.Count);
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
            RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
            //Danh sach loai SL nhap, loai SL xuat
            PropertyInfo[] piAmount = Properties.Get<Mrs00410RDO>();

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
                if (IsNotNullOrEmpty(listMediStocks))
                {
                    foreach (var mediStock in listMediStocks)
                    {
                        ProcessBeginAmount(mediStock);
                    }

                    ProcessImpMestAmount();

                    ProcessExpMestAmount();

                    ProcessEndAmount();
                }
                //Inventec.Common.Logging.LogSystem.Info("ListRdo" + ListRdo.Count);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessBeginAmount(HIS_MEDI_STOCK mediStock)
        {
            HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery();
            mediStockPeriodFilter.TO_TIME_TO = castFilter.TIME_FROM;
            mediStockPeriodFilter.MEDI_STOCK_ID = mediStock.ID;
            var listMediStockPeriods = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(param).Get(mediStockPeriodFilter);

            if (IsNotNullOrEmpty(listMediStockPeriods))
            {
                var mediStockPeriod = listMediStockPeriods.OrderByDescending(s => s.TO_TIME).First();

                HisMestPeriodMediViewFilterQuery mestPeriodMediViewFilter = new HisMestPeriodMediViewFilterQuery();
                mestPeriodMediViewFilter.MEDI_STOCK_PERIOD_ID = mediStockPeriod.ID;
                var listMestPeriodMedis = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(param).GetView(mestPeriodMediViewFilter);

                if (IsNotNullOrEmpty(listMestPeriodMedis))
                {
                    foreach (var medi in listMestPeriodMedis)
                    {
                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType))
                        {
                            var rdo = new Mrs00410RDO();
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                            {
                                rdo.IS_ADDICTIVE = 1;
                            }

                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                            {
                                rdo.IS_NEUROLOGICAL = 1;
                            }

                            rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.CONCENTRA = medicineType.First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                            rdo.AMOUNT_BEGIN = medi.AMOUNT;

                            rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                            rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                            rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                            ListRdo.Add(rdo);
                        }
                    }
                }

                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                impMestMedicineViewFilter.IMP_TIME_FROM = mediStockPeriod.TO_TIME + 1;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMediStockImpMests = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                listMediStockImpMests = listMediStockImpMests.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_NEUROLOGICAL != null || s.IS_ADDICTIVE != null*/) && (this.castFilter.MEDI_STOCK_IDs != null || (!CHMS_IMP_MEST_TYPE_IDs.Contains(s.IMP_MEST_TYPE_ID)))).ToList();

                if (IsNotNullOrEmpty(listMediStockImpMests))
                {
                    foreach (var medi in listMediStockImpMests)
                    {
                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType))
                        {
                            var rdo = new Mrs00410RDO();
                            //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                            //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                            {
                                rdo.IS_ADDICTIVE = 1;
                            }

                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                            {
                                rdo.IS_NEUROLOGICAL = 1;
                            }

                            rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.CONCENTRA = medi.CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                            rdo.AMOUNT_BEGIN = medi.AMOUNT;

                            rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                            rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                            rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                            ListRdo.Add(rdo);
                        }
                    }
                }

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMedicineViewFilter.EXP_TIME_FROM = mediStockPeriod.TO_TIME + 1;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMedicineViewFilter.IS_EXPORT = true;
                var listMediStockExpMests = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                listMediStockExpMests = listMediStockExpMests.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_NEUROLOGICAL != null || s.IS_ADDICTIVE != null*/) && (this.castFilter.MEDI_STOCK_IDs != null || (!CHMS_EXP_MEST_TYPE_IDs.Contains(s.EXP_MEST_TYPE_ID)))).ToList();

                if (IsNotNullOrEmpty(listMediStockExpMests))
                {
                    foreach (var medi in listMediStockExpMests)
                    {
                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType))
                        {
                            var rdo = new Mrs00410RDO();
                            //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                            //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                            {
                                rdo.IS_ADDICTIVE = 1;
                            }

                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                            {
                                rdo.IS_NEUROLOGICAL = 1;
                            }

                            rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.CONCENTRA = medicineType.First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                            rdo.AMOUNT_BEGIN = -(medi.AMOUNT);

                            rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                            rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                            rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                            ListRdo.Add(rdo);
                        }
                    }
                }

                ProcessOptimizeList(ref ListRdo);
            }
            else
            {
                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                var listMediStockImpMests = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                listMediStockImpMests = listMediStockImpMests.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_NEUROLOGICAL != null || s.IS_ADDICTIVE != null*/) && (this.castFilter.MEDI_STOCK_IDs != null || (!CHMS_IMP_MEST_TYPE_IDs.Contains(s.IMP_MEST_TYPE_ID)))).ToList();

                if (IsNotNullOrEmpty(listMediStockImpMests))
                {
                    foreach (var medi in listMediStockImpMests)
                    {
                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType))
                        {
                            var rdo = new Mrs00410RDO();
                            //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                            //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                            {
                                rdo.IS_ADDICTIVE = 1;
                            }

                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                            {
                                rdo.IS_NEUROLOGICAL = 1;
                            }

                            rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.CONCENTRA = medi.CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                            rdo.AMOUNT_BEGIN = medi.AMOUNT;

                            rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                            rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                            rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                            ListRdo.Add(rdo);
                        }
                    }
                }

                HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineViewFilter.MEDI_STOCK_ID = mediStock.ID;
                expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMestMedicineViewFilter.IS_EXPORT = true;
                var listMediStockExpMests = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                listMediStockExpMests = listMediStockExpMests.Where(s => (GN_HT_IDs.Contains(s.MEDICINE_GROUP_ID ?? 0)/*s.IS_NEUROLOGICAL != null || s.IS_ADDICTIVE != null*/) && (this.castFilter.MEDI_STOCK_IDs != null || (!CHMS_EXP_MEST_TYPE_IDs.Contains(s.EXP_MEST_TYPE_ID)))).ToList();

                if (IsNotNullOrEmpty(listMediStockExpMests))
                {
                    foreach (var medi in listMediStockExpMests)
                    {
                        var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                        if (IsNotNullOrEmpty(medicineType))
                        {
                            var rdo = new Mrs00410RDO();
                            //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                            //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                            {
                                rdo.IS_ADDICTIVE = 1;
                            }

                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                            {
                                rdo.IS_NEUROLOGICAL = 1;
                            }

                            rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.CONCENTRA = medicineType.First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;

                            rdo.AMOUNT_BEGIN = -(medi.AMOUNT);

                            rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                            rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                            rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                            rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                            ListRdo.Add(rdo);
                        }
                    }
                }

                ProcessOptimizeList(ref ListRdo);
            }
        }

        private void ProcessImpMestAmount()
        {
            if (IsNotNullOrEmpty(listImpMestMedis))
            {
                foreach (var medi in listImpMestMedis)
                {
                    var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType))
                    {
                        var rdo = new Mrs00410RDO();
                        //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                        //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                        if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                        {
                            rdo.IS_ADDICTIVE = 1;
                        }

                        if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                        {
                            rdo.IS_NEUROLOGICAL = 1;
                        }

                        rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medi.CONCENTRA;
                        rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.IMP_AMOUNT = medi.AMOUNT;
                        if (dicImpAmountType.ContainsKey(medi.IMP_MEST_TYPE_ID))
                            dicImpAmountType[medi.IMP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                        rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                        rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                        rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                        ListRdo.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdo);
            }

        }

        private void ProcessExpMestAmount()
        {
            if (IsNotNullOrEmpty(listExpMestMedis))
            {
                foreach (var medi in listExpMestMedis)
                {
                    var medicineType = listMedicineTypes.Where(s => s.ID == medi.MEDICINE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType))
                    {
                        var rdo = new Mrs00410RDO();
                        //rdo.IS_ADDICTIVE = medi.IS_ADDICTIVE;
                        //rdo.IS_NEUROLOGICAL = medi.IS_NEUROLOGICAL;
                        if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                        {
                            rdo.IS_ADDICTIVE = 1;
                        }

                        if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                        {
                            rdo.IS_NEUROLOGICAL = 1;
                        }

                        rdo.MEDICINE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medicineType.First().CONCENTRA;
                        rdo.MEDICINE_TYPE_PROPRIETARY_NAME = medicineType.First().MEDICINE_TYPE_PROPRIETARY_NAME;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.EXP_AMOUNT = medi.AMOUNT;
                        if (dicExpAmountType.ContainsKey(medi.EXP_MEST_TYPE_ID))
                            dicExpAmountType[medi.EXP_MEST_TYPE_ID].SetValue(rdo, medi.AMOUNT);

                        rdo.ACTIVE_INGR_BHYT_CODE = medicineType.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = medicineType.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.MEDICINE_TYPE_CODE = medicineType.First().MEDICINE_TYPE_CODE;
                        rdo.NATIONAL_NAME = medicineType.First().NATIONAL_NAME;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.REGISTER_NUMBER = medicineType.First().REGISTER_NUMBER;
                        rdo.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        rdo.TCY_NUM_ORDER = medicineType.First().TCY_NUM_ORDER;

                        ListRdo.Add(rdo);
                    }
                }

                ProcessOptimizeList(ref ListRdo);
            }
        }

        private void ProcessEndAmount()
        {
            if (IsNotNullOrEmpty(ListRdo))
            {
                foreach (var medi in ListRdo)
                {
                    medi.AMOUNT_END = (medi.AMOUNT_BEGIN + medi.IMP_AMOUNT)
                        - (medi.EXP_AMOUNT);
                }
            }
        }

        public void ProcessOptimizeList(ref List<Mrs00410RDO> _listRdo)
        {
            var group = _listRdo.GroupBy(g => new { g.MEDICINE_TYPE_ID }).ToList();
            _listRdo.Clear();
            Decimal sum = 0;
            Mrs00410RDO rdo;
            List<Mrs00410RDO> listSub;
            PropertyInfo[] pi = Properties.Get<Mrs00410RDO>();
            foreach (var item in group)
            {
                rdo = new Mrs00410RDO();
                listSub = item.ToList<Mrs00410RDO>();

                bool hide = true;
                foreach (var field in pi)
                {
                    if (field.Name.Contains("AMOUNT"))
                    {
                        sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                        if (hide && sum != 0) hide = false;
                        field.SetValue(rdo, sum);
                    }
                    else
                    {
                        field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00410RDO()));
                    }
                }
                if (!hide)
                    _listRdo.Add(rdo);
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
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                dicSingleTag.Add("CREATE_TIME", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.MEDICINE_TYPE_NAME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
