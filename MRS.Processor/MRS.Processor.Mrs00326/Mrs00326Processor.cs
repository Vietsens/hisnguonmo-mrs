using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using FlexCel.Report;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Core.MrsReport.RDO.RDOImpExpMestType;

namespace MRS.Processor.Mrs00326
{
    class Mrs00326Processor : AbstractProcessor
    {
        Mrs00326Filter castFilter = null;

        List<Mrs00326RDO> ListRdo = new List<Mrs00326RDO>();

        List<Mrs00326RDO> listRdoMedicine = new List<Mrs00326RDO>();
        List<Mrs00326RDO> listRdoMaterial = new List<Mrs00326RDO>();
        Dictionary<string, long> dicExpMestType = new Dictionary<string, long>();
        Dictionary<string, long> dicImpMestType = new Dictionary<string, long>();

        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        V_HIS_MEDI_STOCK CurrentMediStock = new V_HIS_MEDI_STOCK();
        List<HIS_SUPPLIER> ListSuppiler = new List<HIS_SUPPLIER>();

        const string IMP_MANU = "MANU"; //Nhập từ nhà cung cấp
        const string IMP_BEPE = "BEPE"; //Nhập đầu kì
        const string IMP_CHMS = "CHMS"; //Nhập chuyển kho
        const string IMP_MOBA = "MOBA"; //Nhập thu hồi
        const string EXP_PRES = "PRES"; //Xuất đơn thuốc
        const string EXP_DEPA = "DEPA"; //Xuất khoa phòng
        const string EXP_CHMS = "CHMS"; //Xuất Chuyển kho
        const string EXP_MANU = "MANU"; //Xuất trả nhà cung cấp
        const string EXP_EXPE = "EXPE"; //Xuất hao phí
        const string EXP_LOST = "LOST"; //Xuất mất mát
        const string EXP_SALE = "SALE"; //Xuất bán
        const string EXP_LIQU = "LIQU"; //Xuất thanh lý

        public Mrs00326Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00326Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00326Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_MEDI_STOCK, MRS00326 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                //if (IsNotNullOrEmpty(castFilter.MEDI_STOCK_IDs))
                {
                    HisMediStockViewFilterQuery mediStockFilter = new HisMediStockViewFilterQuery();
                    mediStockFilter.IDs = castFilter.MEDI_STOCK_IDs;
                    ListMediStock = new MOS.MANAGER.HisMediStock.HisMediStockManager(paramGet).GetView(mediStockFilter);

                    ListSuppiler = new MOS.MANAGER.HisSupplier.HisSupplierManager().Get(new MOS.MANAGER.HisSupplier.HisSupplierFilterQuery());

                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_MEDI_STOCK, MRS00326.");
                    }
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

                        AddSupplierInfo();
                    }
                    else
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00326.");
                    }
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00326.");
                }
            }

        }

        private void AddSupplierInfo()
        {
            try
            {
                if (IsNotNullOrEmpty(ListRdo))
                {
                    foreach (var item in ListRdo)
                    {
                        if (item.SUPPLIER_ID.HasValue)
                        {
                            var supplier = ListSuppiler.FirstOrDefault(o => o.ID == item.SUPPLIER_ID);
                            if (supplier != null)
                            {
                                item.SUPPLIER_BANK_ACCOUNT = supplier.BANK_ACCOUNT;
                                item.SUPPLIER_BANK_INFO = supplier.BANK_INFO;
                                item.SUPPLIER_FAX = supplier.FAX;
                                item.SUPPLIER_PHONE = supplier.PHONE;
                                item.SUPPLIER_POSITION = supplier.POSITION;
                                item.SUPPLIER_TAX_CODE = supplier.TAX_CODE;
                            }
                        }
                    }
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
                    PropertyInfo p = typeof(Mrs00326RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;

                    var GroupImps = listImpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                    PropertyInfo p = typeof(Mrs00326RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMediSub.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                    PropertyInfo p = typeof(Mrs00326RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listImpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                    PropertyInfo p = typeof(Mrs00326RDO).GetProperty(fieldName);
                    if (!IsNotNull(p)) return;
                    var GroupImps = listExpMestMateSub.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                List<Mrs00326RDO> listrdo = new List<Mrs00326RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00326RDO
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
                List<Mrs00326RDO> listrdo = new List<Mrs00326RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        Mrs00326RDO rdo = new Mrs00326RDO();
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
                        listrdo.Add(rdo);
                    }
                }

                listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00326RDO
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
            List<Mrs00326RDO> listrdo = new List<Mrs00326RDO>();

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
                    Mrs00326RDO rdo = new Mrs00326RDO();
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
                    listrdo.Add(rdo);
                }
            }
            HisExpMestMedicineViewFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
            ExpMestMedicinefilter.MEDI_STOCK_IDs = new List<long>() { CurrentMediStock.ID };
            ExpMestMedicinefilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
            ExpMestMedicinefilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new ManagerSql(paramGet).GetView(ExpMestMedicinefilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00326");

            if (IsNotNullOrEmpty(hisExpMestMedicine))
            {
                var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                foreach (var group in GroupExps)
                {
                    List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                    Mrs00326RDO rdo = new Mrs00326RDO();
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
                    listrdo.Add(rdo);
                }
            }

            listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00326RDO
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
                BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
            }).ToList();
            listRdoMedicine.AddRange(listrdo);
        }

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet)
        {
            List<Mrs00326RDO> listrdo = new List<Mrs00326RDO>();

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
                    Mrs00326RDO rdo = new Mrs00326RDO();
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
                    listrdo.Add(rdo);
                }
            }

            HisExpMestMaterialViewFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
            ExpMestMaterialfilter.MEDI_STOCK_IDs = new List<long>() { CurrentMediStock.ID };
            ExpMestMaterialfilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
            ExpMestMaterialfilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new ManagerSql(paramGet).GetView(ExpMestMaterialfilter);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00326");

            if (IsNotNullOrEmpty(hisExpMestMaterial))
            {
                var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                foreach (var group in GroupExps)
                {
                    List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                    Mrs00326RDO rdo = new Mrs00326RDO();
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
                    listrdo.Add(rdo);
                }
            }

            listrdo = listrdo.GroupBy(g => g.MEDI_MATE_ID).Select(s => new Mrs00326RDO
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
                BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT)
            }).ToList();

            listRdoMaterial.AddRange(listrdo);

        }

        //Gop theo id
        private List<Mrs00326RDO> groupById(List<Mrs00326RDO> listRdoMedicine)
        {
            List<Mrs00326RDO> result = new List<Mrs00326RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => g.MEDI_MATE_ID).ToList();
                Decimal sum = 0;
                Mrs00326RDO rdo;
                List<Mrs00326RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00326RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00326RDO();
                    listSub = item.ToList<Mrs00326RDO>();

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
                return new List<Mrs00326RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu, theo nha cung cap
        private List<Mrs00326RDO> groupByServiceAndPriceAndSupplier(List<Mrs00326RDO> listRdoMedicine)
        {
            List<Mrs00326RDO> result = new List<Mrs00326RDO>();
            try
            {
                if (IsNotNullOrEmpty(listRdoMedicine))
                {
                    var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).ToList();
                    Decimal sum = 0;
                    Mrs00326RDO rdo;
                    List<Mrs00326RDO> listSub;
                    PropertyInfo[] pi = Properties.Get<Mrs00326RDO>();
                    foreach (var item in group)
                    {
                        rdo = new Mrs00326RDO();
                        listSub = item.ToList<Mrs00326RDO>();

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
                                field.SetValue(rdo, field.GetValue(listSub.FirstOrDefault(s => IsMeaningful(field.GetValue(s))) ?? new Mrs00326RDO()));
                            }
                        }
                        if (!hide) result.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00326RDO>();
            }
            return result;
        }

        //Gop theo gia, theo dich vu
        private List<Mrs00326RDO> groupByServiceAndPrice(List<Mrs00326RDO> listRdoMedicine)
        {
            List<Mrs00326RDO> result = new List<Mrs00326RDO>();
            try
            {
                var group = listRdoMedicine.GroupBy(g => new { g.SERVICE_ID, g.IMP_PRICE }).ToList();
                Decimal sum = 0;
                Mrs00326RDO rdo;
                List<Mrs00326RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00326RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00326RDO();
                    listSub = item.ToList<Mrs00326RDO>();

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
                return new List<Mrs00326RDO>();
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
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.SERVICE_TYPE_ID).ThenByDescending(t2 => t2.NUM_ORDER).ThenBy(t3 => t3.SERVICE_NAME).ToList();

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MediStocks", ListMediStock);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Services", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "MediStocks", "Services", "ID", "MEDI_STOCK_ID");
                exportSuccess = exportSuccess && objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long MediStockId;
        int SameType;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (SameType == ServiceId && MediStockId == mediId)
                    {
                        return true;
                    }
                    else
                    {
                        MediStockId = mediId;
                        SameType = ServiceId;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }

    class RDOCustomerFuncManyRownumberData : TFlexCelUserFunction
    {
        long Medi_Stock_Id;
        int Service_Type_Id;
        long num_order = 0;

        public RDOCustomerFuncManyRownumberData()
        {
        }

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long mediId = Convert.ToInt64(parameters[0]);
                int ServiceId = Convert.ToInt32(parameters[1]);

                if (mediId > 0 && ServiceId > 0)
                {
                    if (Service_Type_Id == ServiceId && Medi_Stock_Id == mediId)
                    {
                        num_order = num_order + 1;
                    }
                    else
                    {
                        Medi_Stock_Id = mediId;
                        Service_Type_Id = ServiceId;
                        num_order = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return num_order;
        }
    }
}
