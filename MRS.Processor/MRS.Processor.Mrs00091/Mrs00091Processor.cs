using MOS.MANAGER.HisService;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestStt;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.Proccessor.Mrs00091;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MRS.Processor.Mrs00091
{
    public class Mrs00091Processor : AbstractProcessor
    {
        Mrs00091Filter castFilter = null;
        List<Mrs00091RDO> listRdoMedicine = new List<Mrs00091RDO>();
        List<Mrs00091RDO> listRdoMaterial = new List<Mrs00091RDO>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        private string MEDI_STOCK_CODE;
        private string MEDI_STOCK_NAME;
        string thisReportTypeCode = "";
        public Mrs00091Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }
        public override Type FilterType()
        {
            return typeof(Mrs00091Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00091Filter)this.reportFilter;
            try
            {
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                //expMediFilter.IN_EXECUTE = true; 
                hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                hisImpMestMaterial = new HisImpMestMaterialManager().GetView(impMateFilter);

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_TO;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                //expMateFilter.IN_EXECUTE = true; 
                hisExpMestMaterial = new HisExpMestMaterialManager().GetView(expMateFilter);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }
        //Tính số lượng nhập và xuất thuốc
        private void ProcessAmountMedicine(List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine, List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine)
        {
            try
            {
                ProcessImpAmountMedicine(hisImpMestMedicine);
                ProcessExpAmountMedicine(hisExpMestMedicine);
                listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00091RDO { MEDICINE_ID = s.First().MEDICINE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();
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
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.IMP_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listRdoMedicine.Add(rdo);
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
                    var GroupImps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.EXP_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
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
                ProcessImpAmountMaterial(hisImpMestMaterial);
                ProcessExpAmountMaterial(hisExpMestMaterial);
                listRdoMaterial = listRdoMaterial.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00091RDO { MATERIAL_ID = s.First().MATERIAL_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();
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
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.IMP_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listRdoMaterial.Add(rdo);
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
                    var GroupImps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.EXP_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
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
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.CREATE_TIME_TO = castFilter.TIME_FROM;
                periodFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                List<V_HIS_MEDI_STOCK_PERIOD> hisMediStockPeriod = new HisMediStockPeriodManager(paramGet).GetView(periodFilter);
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
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMaterial.Clear();
                listRdoMedicine.Clear();
            }
        }

        //Tinh so luong ton dau neu co chot ky gan nhat của thuốc
        private void ProcessBeinAmountMedicineByMediStockPeriod(CommonParam paramGet, V_HIS_MEDI_STOCK_PERIOD neighborPeriod)
        {
            try
            {
                List<Mrs00091RDO> listrdo = new List<Mrs00091RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = item.MEDICINE_ID;
                        rdo.MEDICINE_TYPE_ID = item.MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true; // HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMediFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00091RDO { MEDICINE_ID = s.First().MEDICINE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<Mrs00091RDO> listrdo = new List<Mrs00091RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = item.MATERIAL_ID;
                        rdo.MATERIAL_TYPE_ID = item.MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = item.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = item.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = item.SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true; // HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMateFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00091RDO { MATERIAL_ID = s.First().MATERIAL_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
            try
            {
                List<Mrs00091RDO> listrdo = new List<Mrs00091RDO>();

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new HisImpMestMedicineManager().GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMediFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true; // HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMediFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new HisExpMestMedicineManager().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID ?? 0;
                        rdo.MEDICINE_TYPE_ID = listmediSub.First().MEDICINE_TYPE_ID;
                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmediSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmediSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmediSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmediSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmediSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00091RDO { MEDICINE_ID = s.First().MEDICINE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
                listRdoMedicine.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // Tính số lượng tồn đầu nếu không có chốt kỳ gần nhất của vật tư
        private void ProcessBeinAmountMaterialNotMediStockPriod(CommonParam paramGet)
        {
            try
            {
                List<Mrs00091RDO> listrdo = new List<Mrs00091RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = castFilter.TIME_FROM - 1;
                impMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = castFilter.TIME_FROM - 1;
                expMateFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true; // HisExpMestSttCFG.EXP_MEST_STT_ID__EXPORTED; 
                //expMateFilter.IN_EXECUTE = true; 
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00091RDO rdo = new Mrs00091RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID ?? 0;
                        rdo.MATERIAL_TYPE_ID = listmateSub.First().MATERIAL_TYPE_ID;
                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.SUPPLIER_ID = listmateSub.First().SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = listmateSub.First().SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = listmateSub.First().SUPPLIER_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.EXPIRED_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listmateSub.First().EXPIRED_DATE ?? 0);
                        rdo.NUM_ORDER = listmateSub.First().NUM_ORDER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00091RDO { MATERIAL_ID = s.First().MATERIAL_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
                listRdoMaterial.AddRange(listrdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
        private void ProcessListRdo()
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00091RDO { MEDICINE_ID = s.First().MEDICINE_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();

                listRdoMedicine = listRdoMedicine.GroupBy(g => new { g.MEDICINE_TYPE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s => new Mrs00091RDO { SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).Where(o => o.BEGIN_AMOUNT > 0 || o.IMP_AMOUNT > 0 || o.EXP_AMOUNT > 0).ToList();
                if (IsNotNullOrEmpty(listRdoMedicine))
                {
                    foreach (var rdo in listRdoMedicine)
                    {
                        rdo.SetMediConcentraAndEndAmount(listMedicineType, ref paramGet);
                    }
                }

                listRdoMaterial = listRdoMaterial.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00091RDO { MATERIAL_ID = s.First().MATERIAL_ID, SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();

                listRdoMaterial = listRdoMaterial.GroupBy(g => new { g.MATERIAL_TYPE_ID, g.SUPPLIER_ID, g.IMP_PRICE }).Select(s => new Mrs00091RDO { SUPPLIER_ID = s.First().SUPPLIER_ID, SUPPLIER_CODE = s.First().SUPPLIER_CODE, SUPPLIER_NAME = s.First().SUPPLIER_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, EXPIRED_STR = s.First().EXPIRED_STR, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_ID = s.First().MATERIAL_TYPE_ID, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, NUM_ORDER = s.First().NUM_ORDER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).Where(o => o.BEGIN_AMOUNT > 0 || o.IMP_AMOUNT > 0 || o.EXP_AMOUNT > 0).ToList();

                if (IsNotNullOrEmpty(listRdoMaterial))
                {
                    foreach (var rdo in listRdoMaterial)
                    {
                        rdo.SetMateConcentraAndEndAmount(listMaterialType, ref paramGet);
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();
            }
        }
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessAmountMedicine(hisImpMestMedicine, hisExpMestMedicine);
                ProcessAmountMaterial(hisImpMestMaterial, hisExpMestMaterial);
                ProcessGetPeriod(paramGet);
                ProcessListRdo();
                var mediStock = new HisMediStockManager(paramGet).Get(new HisMediStockFilterQuery());
                if (castFilter.MEDI_STOCK_ID != null)
                    mediStock = mediStock.Where(o => o.ID == castFilter.MEDI_STOCK_ID).ToList();
                if (IsNotNull(mediStock))
                {
                    MEDI_STOCK_CODE = string.Join(", ", mediStock.Select(o => o.MEDI_STOCK_CODE).ToList());
                    MEDI_STOCK_NAME = string.Join(", ", mediStock.Select(o => o.MEDI_STOCK_NAME).ToList());
                }

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
            dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
            dicSingleTag.Add("MEDI_STOCK_CODE_AND_NAME", MEDI_STOCK_CODE + " - " + MEDI_STOCK_NAME);

            if (castFilter.IS_IMP == true)
            {
                listRdoMedicine = listRdoMedicine.Where(o => o.IMP_AMOUNT > 0).ToList();
                listRdoMaterial = listRdoMaterial.Where(o => o.IMP_AMOUNT > 0).ToList();
            }
            else if (castFilter.IS_EXP == true)
            {
                listRdoMedicine = listRdoMedicine.Where(o => o.EXP_AMOUNT > 0).ToList();
                listRdoMaterial = listRdoMaterial.Where(o => o.EXP_AMOUNT > 0).ToList();
            }
            else if (castFilter.IS_LEFT == true)
            {
                listRdoMedicine = listRdoMedicine.Where(o => o.BEGIN_AMOUNT > 0 || o.END_AMOUNT > 0).ToList();
                listRdoMaterial = listRdoMaterial.Where(o => o.BEGIN_AMOUNT > 0 || o.END_AMOUNT > 0).ToList();
            }

            dicSingleTag.Add("MEDI_TOTAL_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(listRdoMedicine.Sum(o => ((o.BEGIN_AMOUNT + o.IMP_AMOUNT) - o.EXP_AMOUNT) * o.IMP_PRICE)).ToString()));
            dicSingleTag.Add("MATE_TOTAL_PRICE_TEXT", Inventec.Common.String.Convert.CurrencyToVneseString(Math.Round(listRdoMaterial.Sum(o => ((o.BEGIN_AMOUNT + o.IMP_PRICE) - o.EXP_AMOUNT) * o.IMP_PRICE)).ToString()));
            listRdoMedicine = listRdoMedicine.OrderBy(t => t.MEDICINE_TYPE_NAME).ToList();
            listRdoMaterial = listRdoMaterial.OrderBy(t => t.MATERIAL_TYPE_NAME).ToList();
            objectTag.AddObjectData(store, "listRdoMedicine", listRdoMedicine);
            objectTag.AddObjectData(store, "listRdoMaterial", listRdoMaterial);
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
}
