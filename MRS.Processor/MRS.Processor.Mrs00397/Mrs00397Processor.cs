using MOS.MANAGER.HisService;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00397;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00397
{
    public class Mrs00397Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        List<Mrs00397RDO> listRdoMedicine = new List<Mrs00397RDO>();
        List<Mrs00397RDO> listRdoMaterial = new List<Mrs00397RDO>();

        List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();


        public Mrs00397Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00397Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                ProcessGetPeriod(paramGet);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListRdo();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                //ListRdo.Clear();
                result = false;
            }
            return result;
        }

        //---------------------------Các Hàm xử lý dữ liệu ------------------------------------//

        //Lay ky du liệu cũ và gần time to nhất
        private void ProcessGetPeriod(CommonParam paramGet)
        {
            try
            {
                HisMediStockPeriodViewFilterQuery periodFilter = new HisMediStockPeriodViewFilterQuery();
                periodFilter.CREATE_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                periodFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
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
                List<Mrs00397RDO> listrdo = new List<Mrs00397RDO>();
                HisMestPeriodMediViewFilterQuery periodMediFilter = new HisMestPeriodMediViewFilterQuery();
                periodMediFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> hisMestPeriodMedi = new MOS.MANAGER.HisMestPeriodMedi.HisMestPeriodMediManager(paramGet).GetView(periodMediFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMedi))
                {
                    foreach (var item in hisMestPeriodMedi)
                    {
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MEDICINE_ID = item.MEDICINE_ID;

                        rdo.MEDICINE_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = item.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.EXP_PRICE = (item.IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMediFilter.IMP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                impMediFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID;

                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmediSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMediFilter.EXP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                expMediFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID ?? 0;

                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = rdo.IMP_PRICE * (1 + listmediSub.First().IMP_VAT_RATIO);
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00397RDO { MEDICINE_ID = s.First().MEDICINE_ID, IMP_PRICE = s.First().IMP_PRICE, EXP_PRICE = s.First().EXP_PRICE, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<Mrs00397RDO> listrdo = new List<Mrs00397RDO>();
                HisMestPeriodMateViewFilterQuery periodMateFilter = new HisMestPeriodMateViewFilterQuery();
                periodMateFilter.MEDI_STOCK_PERIOD_ID = neighborPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> hisMestPeriodMate = new MOS.MANAGER.HisMestPeriodMate.HisMestPeriodMateManager(paramGet).GetView(periodMateFilter);
                if (IsNotNullOrEmpty(hisMestPeriodMate))
                {
                    foreach (var item in hisMestPeriodMate)
                    {
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MATERIAL_ID = item.MATERIAL_ID;

                        rdo.MATERIAL_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.EXP_PRICE = (item.IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = item.AMOUNT;
                        listrdo.Add(rdo);
                    }
                }
                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                impMateFilter.IMP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                impMateFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID;

                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmateSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_FROM = neighborPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                expMateFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID ?? 0;

                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmateSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00397RDO { MATERIAL_ID = s.First().MATERIAL_ID, EXP_PRICE = s.First().EXP_PRICE, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<Mrs00397RDO> listrdo = new List<Mrs00397RDO>();

                HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                impMediFilter.IMP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                impMediFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> hisImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager().GetView(impMediFilter);
                if (IsNotNullOrEmpty(hisImpMestMedicine))
                {
                    var GroupImps = hisImpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID;

                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmediSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                expMediFilter.EXP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                expMediFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                expMediFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager().GetView(expMediFilter);
                if (IsNotNullOrEmpty(hisExpMestMedicine))
                {
                    var GroupExps = hisExpMestMedicine.GroupBy(g => g.MEDICINE_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listmediSub = group.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MEDICINE_ID = listmediSub.First().MEDICINE_ID ?? 0;

                        rdo.MEDICINE_TYPE_CODE = listmediSub.First().MEDICINE_TYPE_CODE;
                        rdo.MEDICINE_TYPE_NAME = listmediSub.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmediSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmediSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmediSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmediSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmediSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00397RDO { MEDICINE_ID = s.First().MEDICINE_ID, EXP_PRICE = s.First().EXP_PRICE, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                List<Mrs00397RDO> listrdo = new List<Mrs00397RDO>();

                HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                impMateFilter.IMP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                impMateFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                if (IsNotNullOrEmpty(hisImpMestMaterial))
                {
                    var GroupImps = hisImpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupImps)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID;

                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmateSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => s.AMOUNT);
                        listrdo.Add(rdo);
                    }
                }

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.EXP_TIME_TO = ((Mrs00397Filter)this.reportFilter).TIME_TO;
                expMateFilter.MEDI_STOCK_ID = ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> hisExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                if (IsNotNullOrEmpty(hisExpMestMaterial))
                {
                    var GroupExps = hisExpMestMaterial.GroupBy(g => g.MATERIAL_ID).ToList();
                    foreach (var group in GroupExps)
                    {
                        List<V_HIS_EXP_MEST_MATERIAL> listmateSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        Mrs00397RDO rdo = new Mrs00397RDO();
                        rdo.MATERIAL_ID = listmateSub.First().MATERIAL_ID ?? 0;

                        rdo.MATERIAL_TYPE_CODE = listmateSub.First().MATERIAL_TYPE_CODE;
                        rdo.MATERIAL_TYPE_NAME = listmateSub.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listmateSub.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = listmateSub.First().PACKAGE_NUMBER;
                        rdo.IMP_PRICE = listmateSub.First().IMP_PRICE;
                        rdo.EXP_PRICE = (listmateSub.First().IMP_VAT_RATIO + 1) * rdo.IMP_PRICE;
                        rdo.BEGIN_AMOUNT = listmateSub.Sum(s => -(s.AMOUNT));
                        listrdo.Add(rdo);
                    }
                }
                listrdo = listrdo.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00397RDO { MATERIAL_ID = s.First().MATERIAL_ID, EXP_PRICE = s.First().EXP_PRICE, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(su => su.BEGIN_AMOUNT) }).ToList();
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
                listRdoMedicine = listRdoMedicine.GroupBy(g => g.MEDICINE_ID).Select(s => new Mrs00397RDO { MEDICINE_ID = s.First().MEDICINE_ID, EXP_PRICE = s.First().EXP_PRICE, IMP_PRICE = s.First().IMP_PRICE, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();

                if (IsNotNullOrEmpty(listRdoMedicine))
                {
                    foreach (var rdo in listRdoMedicine)
                    {
                        rdo.SetMediConcentraAndEndAmount(listMedicineType, ref paramGet);
                    }
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu. MRS00091.");
                }

                listRdoMaterial = listRdoMaterial.GroupBy(g => g.MATERIAL_ID).Select(s => new Mrs00397RDO { MATERIAL_ID = s.First().MATERIAL_ID, EXP_PRICE = s.First().EXP_PRICE, IMP_PRICE = s.First().IMP_PRICE, MATERIAL_TYPE_CODE = s.First().MATERIAL_TYPE_CODE, MATERIAL_TYPE_NAME = s.First().MATERIAL_TYPE_NAME, PACKAGE_NUMBER = s.First().PACKAGE_NUMBER, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, BEGIN_AMOUNT = s.Sum(s1 => s1.BEGIN_AMOUNT), IMP_AMOUNT = s.Sum(s2 => s2.IMP_AMOUNT), EXP_AMOUNT = s.Sum(s3 => s3.EXP_AMOUNT) }).ToList();

                if (IsNotNullOrEmpty(listRdoMaterial))
                {
                    foreach (var rdo in listRdoMaterial)
                    {
                        rdo.SetMateConcentraAndEndAmount(listMaterialType, ref paramGet);
                    }
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu. MRS00091.");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();
            }
        }
        //----------------------------------------------Kết thúc xử lý----------------------------------------------------------//
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00397Filter)this.reportFilter).TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00397Filter)this.reportFilter).TIME_TO));
                if (IsNotNullOrEmpty(HisMediStockCFG.HisMediStocks))
                {
                    var hisMediStock = HisMediStockCFG.HisMediStocks.Where(o => o.ID == ((Mrs00397Filter)this.reportFilter).MEDI_STOCK_ID).ToList();
                    if (hisMediStock.Count > 0) dicSingleTag.Add("MEDI_STOCK_NAME", hisMediStock.First().MEDI_STOCK_NAME);
                }
                objectTag.AddObjectData(store, "Medicine", listRdoMedicine);
                objectTag.AddObjectData(store, "Material", listRdoMaterial);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }

        }




    }
}