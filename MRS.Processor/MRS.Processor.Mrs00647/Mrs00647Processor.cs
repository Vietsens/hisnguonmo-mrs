using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00647
{
    class Mrs00647Processor : AbstractProcessor
    {
        Mrs00647Filter castFilter = null;

        List<Mrs00647RDO> ListRdo = new List<Mrs00647RDO>();

        List<Mrs00647RDO> listRdoMedicine = new List<Mrs00647RDO>();
        List<Mrs00647RDO> listRdoMaterial = new List<Mrs00647RDO>();
        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();

        public Mrs00647Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00647Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00647Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_MEDI_STOCK, Mrs00647 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                    mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                    ListMediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter);

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_MEDI_STOCK, Mrs00647.");
                    }
                }
                else
                {
                    throw new DataMisalignedException("Filter khong truyen len MEDI_STOCK ID");
                }
                RDOImpExpMestTypeContext.Define(ref dicImpMestType, ref dicExpMestType);
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
                ProcessListMediStock();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListMediStock()
        {

            if (IsNotNullOrEmpty(ListMediStock))
            {
                CommonParam paramGet = new CommonParam();
                foreach (var medistock in ListMediStock)
                {
                    CurrentMediStock = medistock;
                    HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                    impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMediFilter.MEDI_STOCK_ID = medistock.ID;
                    impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);

                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                    expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMediFilter.MEDI_STOCK_ID = medistock.ID;
                    expMediFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);

                    HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                    impMateFilter.MEDI_STOCK_ID = medistock.ID;
                    impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager().GetView(impMateFilter);

                    HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                    expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                    expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                    expMateFilter.MEDI_STOCK_ID = medistock.ID;
                    expMateFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager().GetView(expMateFilter);

                    if (!paramGet.HasException)
                    {
                        ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine);
                        ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial);
                        ProcessGetPeriod(paramGet);
                        listRdoMedicine = groupById(listRdoMedicine);

                        listRdoMedicine = groupByServiceAndPriceAndSupplier(listRdoMedicine);

                        listRdoMaterial = groupById(listRdoMaterial);

                        listRdoMaterial = groupByServiceAndPriceAndSupplier(listRdoMaterial);
                        ListRdo.AddRange(listRdoMedicine);
                        ListRdo.AddRange(listRdoMaterial);
                        CountEndAmount(ListRdo);
                        //loai bo ton cuoi = 0
                        ListRdo = ListRdo.Where(o => o.END_AMOUNT != 0).ToList();
                    }
                    else
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00647.");
                    }
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00647.");
                }
            }
        }

        private void CountEndAmount(List<Mrs00647RDO> listRdoMedicine)
        {
            try
            {
                foreach (var item in listRdoMedicine)
                {
                    decimal ImpAmount = 0;
                    foreach (var r in dicImpMestType.Keys)
                    {
                        PropertyInfo p = typeof(Mrs00647RDO).GetProperty(string.Format(r));
                        ImpAmount += (decimal)p.GetValue(item);
                    }
                    decimal ExpAmount = 0;
                    foreach (var r in dicExpMestType.Keys)
                    {
                        PropertyInfo p = typeof(Mrs00647RDO).GetProperty(string.Format(r));
                        ExpAmount += (decimal)p.GetValue(item);
                    }
                    item.END_AMOUNT = item.BEGIN_AMOUNT + ImpAmount - ExpAmount;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }

        }

        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                listRdoMedicine.Clear();
                ProcessImpAmountMedicine(hisImpMestMedicine);
                ProcessExpAmountMedicine(hisExpMestMedicine);
                listRdoMedicine = groupById(listRdoMedicine);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMedicine.Clear();
            }
        }

        private void ProcessImpAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var listImpMestMediSub = new List<V_HIS_IMP_MEST_MEDICINE>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMediSub = hisImpMestMedicine.Where(o => o.IMP_MEST_TYPE_ID == dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMediSub)) continue;

                        ProcessImpAmountMedicineByImpMestType(listImpMestMediSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicine(List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var listExpMestMediSub = new List<V_HIS_EXP_MEST_MEDICINE>();
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMediSub = hisExpMestMedicine.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMediSub)) continue;
                        ProcessExpAmountMedicineByExpMestType(listExpMestMediSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMedicineByImpMestType(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00647RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => new { g.MEDICINE_ID, g.IMP_TIME }).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMedicineByExpMestType(List<V_HIS_EXP_MEST_MEDICINE> listExpMestMediSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMediSub))
                {
                    PropertyInfo p = typeof(Mrs00647RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMediSub.GroupBy(g => new { g.MEDICINE_ID, g.IMP_TIME }).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmediSub.First().IMP_VAT_RATIO;
                        rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                        p.SetValue(rdo, listmediSub.Sum(s => s.AMOUNT));
                        listRdoMedicine.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng nhập và xuất vật tư
        private void ProcessAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial, List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                listRdoMaterial.Clear();
                ProcessImpAmountMaterial(hisImpMestMaterial);
                ProcessExpAmountMaterial(hisExpMestMaterial);
                listRdoMaterial = groupById(listRdoMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
            }
        }

        private void ProcessImpAmountMaterial(List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var listImpMestMateSub = new List<V_HIS_IMP_MEST_MATERIAL>();
                    foreach (var item in dicImpMestType.Keys)
                    {
                        listImpMestMateSub = hisImpMestMaterial.Where(o => o.IMP_MEST_TYPE_ID == dicImpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listImpMestMateSub)) continue;
                        ProcessImpAmountMateByImpMestType(listImpMestMateSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMaterial(List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var listExpMestMateSub = new List<V_HIS_EXP_MEST_MATERIAL>();
                    foreach (var item in dicExpMestType.Keys)
                    {
                        listExpMestMateSub = hisExpMestMaterial.Where(o => o.EXP_MEST_TYPE_ID == dicExpMestType[item]).ToList();
                        if (!IsNotNullOrEmpty(listExpMestMateSub)) continue;
                        ProcessExpAmountMateByExpMestType(listExpMestMateSub, item);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessImpAmountMateByImpMestType(List<V_HIS_IMP_MEST_MATERIAL> listImpMestMateSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00647RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => new { g.MATERIAL_ID, g.IMP_TIME }).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                        //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpAmountMateByExpMestType(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMateSub, string fieldName)
        {
            try
            {
                if (IsNotNullOrEmpty(listExpMestMateSub))
                {
                    PropertyInfo p = typeof(Mrs00647RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMateSub.GroupBy(g => new { g.MATERIAL_ID, g.IMP_TIME }).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_VAT = listmateSub.First().IMP_VAT_RATIO;
                        rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                        //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                        p.SetValue(rdo, listmateSub.Sum(o => o.AMOUNT));
                        listRdoMaterial.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Lay ky du liệu cũ và gần timeFrom nhất
        private void ProcessGetPeriod(CommonParam paramGet)
        {
            HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
            periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
            periodFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
            List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new MOS.MANAGER.HisMediStockPeriod.HisMediStockPeriodManager(paramGet).GetView(periodFilter);
            if (!paramGet.HasException)
            {
                if (IsNotNullOrEmpty(hisMediStockPeriod))
                {
                    //Trường hợp có kỳ được chốt gần nhất
                    V_HIS_MEDI_STOCK_PERIOD neighborPeriod = hisMediStockPeriod.OrderByDescending(d => d.CREATE_TIME).ToList()[0];
                    ProcessBeinAmountMedicineByMediStockPeriod(paramGet, neighborPeriod);
                    processBeinAmountMaterialByMediStockPeriod(paramGet, neighborPeriod);
                }
                else
                {
                    // Trường hợp không có kỳ chốt gần nhât - chưa được chốt kỳ nào
                    ProcessBeinAmountMedicineNotMediStockPriod(paramGet);
                    ProcessBeinAmountMaterialNotMediStockPriod(paramGet);
                }

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                }
            }
            else
            {
                throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
            }
        }

        //Tinh so luong ton dau neu co chot ky gan nhat của thuốc
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00647RDO> listrdo = new List<Mrs00647RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    //get his_medicine
                    var medicine_ids = hisMestPeriodMedi.Select(s => s.MEDICINE_ID).Distinct().ToList();
                    List<HIS_MEDICINE> hisMedicine = new MOS.MANAGER.HisMedicine.HisMedicineManager(paramGet).GetByIds(medicine_ids);

                    foreach (var item in hisMestPeriodMedi)
                    {
                        var medicine = hisMedicine.FirstOrDefault(o => o.ID == item.MEDICINE_ID);
                        if (medicine == null) continue;

                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MEDICINE_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;

                        rdo.IMP_TIME = medicine.IMP_TIME;
                        rdo.ACTIVE_INGR_BHYT_CODE = medicine.ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = medicine.ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medicine.EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = medicine.PACKAGE_NUMBER;//Số lô

                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = 1;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00647RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    IMP_TIME = s.First().IMP_TIME,
                    ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                    ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                    PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();

                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nếu có chốt kỳ gần nhất của vật tư
        private void processBeinAmountMaterialByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00647RDO> listrdo = new List<Mrs00647RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    //get his_material
                    var material_ids = hisMestPeriodMate.Select(s => s.MATERIAL_ID).Distinct().ToList();
                    List<HIS_MATERIAL> hisMaterial = new MOS.MANAGER.HisMaterial.HisMaterialManager(paramGet).GetByIds(material_ids);

                    foreach (var item in hisMestPeriodMate)
                    {
                        var material = hisMaterial.FirstOrDefault(o => o.ID == item.MATERIAL_ID);
                        if (material == null) continue;

                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = item.MATERIAL_ID;
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        //rdo.ACTIVE_INGR_BHYT_CODE = material.ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = material.ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(material.EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = material.PACKAGE_NUMBER;//Số lô
                        listrdo.Add(rdo);
                    }
                }

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                        //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00647RDO rdo = new Mrs00647RDO();
                        rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = 2;
                        rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                        rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                        rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                        rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                        //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                        //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00647RDO
                {
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                    SUPPLIER_ID = s.First().SUPPLIER_ID,
                    SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                    SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                    IMP_PRICE = s.First().IMP_PRICE,
                    SERVICE_ID = s.First().SERVICE_ID,
                    SERVICE_CODE = s.First().SERVICE_CODE,
                    SERVICE_NAME = s.First().SERVICE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NUM_ORDER = s.First().NUM_ORDER,
                    IMP_TIME = s.First().IMP_TIME,
                    ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                    ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                    PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                    BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
                }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Tính số lượng tồn đầu nều không có chốt kỳ gần nhất của thuốc
        private void ProcessBeinAmountMedicineNotMediStockPriod(CommonParam paramGet)
        {
            List<Mrs00647RDO> listrdo = new List<Mrs00647RDO>();

            HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
            impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
            impMediFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
            impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);
            if (IsNotNullOrEmpty(hisImpMestMedicine))
            {
                var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                foreach (var group in GroupImps)
                {
                    List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                    Mrs00647RDO rdo = new Mrs00647RDO();
                    rdo.SERVICE_TYPE_NAME = "THUỐC";
                    rdo.SERVICE_TYPE_ID = 1;
                    rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                    rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                    rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID;
                    rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                    rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                    rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                    rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                    rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                    rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                    rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                    rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                    rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                    rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                    rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                    listrdo.Add(rdo);
                }
            }
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = new List<long>() { CurrentMediStock.ID };
            ExpMestMedicinefilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
            ExpMestMedicinefilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new ManagerSql(paramGet).GetView(ExpMestMedicinefilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00647");

            if (IsNotNullOrEmpty(hisExpMestMedicine))
            {
                var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                foreach (var group in GroupExps)
                {
                    List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                    Mrs00647RDO rdo = new Mrs00647RDO();
                    rdo.SERVICE_TYPE_NAME = "THUỐC";
                    rdo.SERVICE_TYPE_ID = 1;
                    rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                    rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                    rdo.MEDI_MATE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                    rdo.SERVICE_ID = listmediSub.First().SERVICE_ID;
                    rdo.SERVICE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                    rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                    rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                    rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                    rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                    rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                    rdo.IMP_TIME = listmediSub.First().IMP_TIME;
                    rdo.ACTIVE_INGR_BHYT_CODE = listmediSub.First().ACTIVE_INGR_BHYT_CODE;
                    rdo.ACTIVE_INGR_BHYT_NAME = listmediSub.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                    rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;//Số lô
                    listrdo.Add(rdo);
                }
            }

            listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00647RDO
            {
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                SUPPLIER_ID = s.First().SUPPLIER_ID,
                SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                IMP_PRICE = s.First().IMP_PRICE,
                SERVICE_ID = s.First().SERVICE_ID,
                SERVICE_CODE = s.First().SERVICE_CODE,
                SERVICE_NAME = s.First().SERVICE_NAME,
                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                IMP_TIME = s.First().IMP_TIME,
                NUM_ORDER = s.First().NUM_ORDER,
                ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
            }).ToList();
            listRdoMedicine.AddRange(listrdo);
        }

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet)
        {
            List<Mrs00647RDO> listrdo = new List<Mrs00647RDO>();

            HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
            impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
            impMateFilter.MEDI_STOCK_ID = CurrentMediStock.ID;
            impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
            if (IsNotNullOrEmpty(hisImpMestMaterial))
            {
                var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                foreach (var group in GroupImps)
                {
                    List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                    Mrs00647RDO rdo = new Mrs00647RDO();
                    rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                    rdo.SERVICE_TYPE_ID = 2;
                    rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                    rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                    rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID;
                    rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                    rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                    rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                    rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                    rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                    rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                    rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                    rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                    //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                    //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                    rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                    listrdo.Add(rdo);
                }
            }

            HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
            ExpMestMaterialfilter.MEDI_STOCK_IDs = new List<long>() { CurrentMediStock.ID };
            ExpMestMaterialfilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
            ExpMestMaterialfilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new ManagerSql(paramGet).GetView(ExpMestMaterialfilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu Mrs00647");

            if (IsNotNullOrEmpty(hisExpMestMaterial))
            {
                var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                foreach (var group in GroupExps)
                {
                    List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                    Mrs00647RDO rdo = new Mrs00647RDO();
                    rdo.SERVICE_TYPE_NAME = "VẬT TƯ";
                    rdo.SERVICE_TYPE_ID = 2;
                    rdo.MEDI_STOCK_ID = CurrentMediStock.ID;
                    rdo.MEDI_STOCK_NAME = CurrentMediStock.MEDI_STOCK_NAME;
                    rdo.MEDI_MATE_ID = listmateSub.First().MATERIAL_ID ?? 0;
                    rdo.SERVICE_ID = listmateSub.First().SERVICE_ID;
                    rdo.SERVICE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                    rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                    rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                    rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                    rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                    rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                    rdo.IMP_TIME = listmateSub.First().IMP_TIME;
                    //rdo.ACTIVE_INGR_BHYT_CODE = listmateSub.First().ACTIVE_INGR_BHYT_CODE;
                    //rdo.ACTIVE_INGR_BHYT_NAME = listmateSub.First().ACTIVE_INGR_BHYT_NAME;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                    rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;//Số lô
                    listrdo.Add(rdo);
                }
            }

            listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00647RDO
            {
                SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                MEDI_STOCK_ID = s.First().MEDI_STOCK_ID,
                MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                SUPPLIER_ID = s.First().SUPPLIER_ID,
                SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                IMP_PRICE = s.First().IMP_PRICE,
                SERVICE_ID = s.First().SERVICE_ID,
                SERVICE_CODE = s.First().SERVICE_CODE,
                SERVICE_NAME = s.First().SERVICE_NAME,
                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                NUM_ORDER = s.First().NUM_ORDER,
                IMP_TIME = s.First().IMP_TIME,
                ACTIVE_INGR_BHYT_CODE = s.First().ACTIVE_INGR_BHYT_CODE,
                ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
            }).ToList();

            listRdoMaterial.AddRange(listrdo);
        }

        //Gop theo id
        private List<Mrs00647RDO> groupById(List<Mrs00647RDO> listRdoMedicine)
        {
            List<Mrs00647RDO> result = new List<Mrs00647RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.MEDI_MATE_ID, g.IMP_TIME }).ToList();
                Decimal sum = 0;
                Mrs00647RDO rdo;
                List<Mrs00647RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00647RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00647RDO();
                    listSub = item.ToList<Mrs00647RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum > 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? listSub.First()));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00647RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<Mrs00647RDO> groupByServiceAndPriceAndSupplier(List<Mrs00647RDO> listRdoMedicine)
        {
            List<Mrs00647RDO> result = new List<Mrs00647RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdoMedicine))
                {
                    var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE, g.IMP_TIME }).ToList();
                    Decimal sum = 0;
                    Mrs00647RDO rdo;
                    List<Mrs00647RDO> listSub;
                    PropertyInfo[] pi = Properties.Get<Mrs00647RDO>();
                    foreach (var item in group)
                    {
                        rdo = new Mrs00647RDO();
                        listSub = item.ToList<Mrs00647RDO>();

                        bool hide = true;
                        foreach (var field in pi)
                        {
                            if (field.Name.Contains("_AMOUNT"))
                            {
                                sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                                if (hide && sum > 0) hide = false;
                                field.SetValue(rdo, sum);
                            }
                            else
                            {
                                field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00647RDO()));
                            }
                        }
                        if (!hide) result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00647RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<Mrs00647RDO> groupByServiceAndPrice(List<Mrs00647RDO> listRdoMedicine)
        {
            List<Mrs00647RDO> result = new List<Mrs00647RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00647RDO rdo;
                List<Mrs00647RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00647RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00647RDO();
                    listSub = item.ToList<Mrs00647RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum > 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s)))));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00647RDO>();
            }
            return result;
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

                ListRdo = ListRdo.OrderBy(o => o.IMP_TIME).ThenBy(t3 => t3.SERVICE_NAME).ToList();

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MediStocks", ListMediStock);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Services", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
