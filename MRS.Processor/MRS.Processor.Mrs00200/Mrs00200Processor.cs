using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestType;
using MOS.MANAGER.HisExpMestType;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisPatientTypeAlter;
//using MOS.MANAGER.HisPrescription; 
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.Proccessor.Mrs00200;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using MOS.MANAGER.HisTreatment;
using Inventec.Common.Repository;
using FlexCel.Report;
using MOS.MANAGER.HisExpMestReason;

namespace MRS.Processor.Mrs00200
{
    public class Mrs00200Processor : AbstractProcessor
    {
        Mrs00200Filter filter = new Mrs00200Filter();
        private List<Mrs00200RDO> ListRdo = new List<Mrs00200RDO>();
        private List<Mrs00200RDO> Material = new List<Mrs00200RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> ListHisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MEDICINE> ListHisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> ListHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> ListHisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<V_HIS_MEDI_STOCK> ListMediStock = new List<V_HIS_MEDI_STOCK>();
        List<HIS_EXP_MEST_REASON> ListExpMestReason = new List<HIS_EXP_MEST_REASON>();
        CommonParam paramGet = new CommonParam();
        const long THUOC = 1;
        const long VATTU = 2;

        const long BH_NT = 1;
        const long ND_NT = 2;
        const long BH_NGT = 3;
        const long ND_NGT = 4;
        const long BH_LESS6 = 5;
        const long ND_LESS6 = 6;
        const long BH_MORE6 = 7;
        const long ND_MORE6 = 8;
        string thisReportTypeCode = "";
        public Mrs00200Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }
        public override Type FilterType()
        {
            return typeof(Mrs00200Filter);
        }

        protected override bool GetData()
        {
            var result = true;

            filter = (Mrs00200Filter)this.reportFilter;
            if (filter.IS_MATERIAL == null) filter.IS_MATERIAL = false;
            if (filter.IS_MEDICINE == null) filter.IS_MEDICINE = false;
            if (filter.IS_MATERIAL == false && filter.IS_MEDICINE == false) filter.IS_MEDICINE = filter.IS_MATERIAL = true;
            try
            {
                //Danh sách kho
                ListMediStock = HisMediStockCFG.HisMediStocks;
                if (filter.MEDI_STOCK_IDs != null) ListMediStock = ListMediStock.Where(o => filter.MEDI_STOCK_IDs.Contains(o.ID)).ToList();
                if (filter.MEDI_STOCK_ID != null) ListMediStock = ListMediStock.Where(o => filter.MEDI_STOCK_ID == o.ID).ToList();
                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());

                ListExpMestReason = new HisExpMestReasonManager(paramGet).Get(new HisExpMestReasonFilterQuery());

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
            bool result = false;
            try
            {
                foreach (var medistock in ListMediStock.Select(o => o.ID).ToList())
                {
                    //tính tồn đầu
                    ProcessBeginAmount(medistock);

                    // nhập trong kỳ
                    ProcessImpMestAmount(medistock);
                    // xuất trong kỳ
                    ProcessExpMestAmount(medistock);
                    //Gộp theo thuốc, theo giá
                    ProcessGroup(medistock);

                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessBeginAmount(long medistock)
        {
            var filter = (Mrs00200Filter)this.reportFilter;
            try
            {
                HisMediStockPeriodFilterQuery periodFilter = new HisMediStockPeriodFilterQuery();
                periodFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                periodFilter.CREATE_TIME_TO = filter.TIME_TO;
                periodFilter.MEDI_STOCK_ID = medistock;
                var listPeriod = new HisMediStockPeriodManager(paramGet).Get(periodFilter);
                //Có chốt kì
                if (IsNotNullOrEmpty(listPeriod))
                {
                    var lastPeriod = listPeriod.OrderBy(o => o.CREATE_TIME).Last();
                    HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery();
                    mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = lastPeriod.ID;
                    List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter);
                    HisMestPeriodMateViewFilterQuery mestPeriodMateFilter = new HisMestPeriodMateViewFilterQuery();
                    mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = lastPeriod.ID;
                    List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateFilter);
                    if (filter.IS_MEDICINE == true)
                    {
                        #region tondauthuoc
                        foreach (var medi in listMestPeriodMedi)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = THUOC;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                            rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                            rdo.BEGIN_AMOUNT = medi.AMOUNT;
                            rdo.END_AMOUNT = medi.AMOUNT;
                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;

                            ListRdo.Add(rdo);
                        }
                        HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                        impFilter.IMP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                        impFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                        impFilter.MEDI_STOCK_ID = medistock;

                        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
                        foreach (var medi in listImpMestMedicine)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = THUOC;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                            rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;

                            rdo.BEGIN_AMOUNT = medi.AMOUNT;
                            rdo.END_AMOUNT = medi.AMOUNT;
                            ListRdo.Add(rdo);

                        }

                        HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                        expFilter.EXP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                        expFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                        expFilter.MEDI_STOCK_ID = medistock;
                        expFilter.IS_EXPORT = true;

                        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expFilter);
                        foreach (var medi in listExpMestMedicine)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = THUOC;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                            rdo.BEGIN_AMOUNT = -medi.AMOUNT;
                            rdo.END_AMOUNT = -medi.AMOUNT;
                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;

                            ListRdo.Add(rdo);
                        }

                        #endregion
                    }
                    if (filter.IS_MATERIAL == true)
                    {
                        #region tondauvattu
                        foreach (var mate in listMestPeriodMate)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = VATTU;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                            rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                            rdo.BEGIN_AMOUNT = mate.AMOUNT;
                            rdo.END_AMOUNT = mate.AMOUNT;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                            rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMaterialType.Where(o => o.ID == mate.MATERIAL_TYPE_ID).First().CONCENTRA;
                            rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;

                            ListRdo.Add(rdo);

                        }

                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                        impMateFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                        impMateFilter.MEDI_STOCK_ID = medistock;
                        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                        foreach (var mate in listImpMestMaterial)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = VATTU;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                            rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                            rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                            rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.BEGIN_AMOUNT = mate.AMOUNT;
                            rdo.END_AMOUNT = mate.AMOUNT;
                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMaterialType.Where(o => o.ID == mate.MATERIAL_TYPE_ID).First().CONCENTRA;
                            rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                            ListRdo.Add(rdo);

                        }
                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.MEDI_STOCK_ID = medistock;
                        expMateFilter.EXP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                        expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                        expMateFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                        foreach (var mate in listExpMestMaterial)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = VATTU;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                            rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                            rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                            rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                            rdo.BEGIN_AMOUNT = -mate.AMOUNT;
                            rdo.END_AMOUNT = -mate.AMOUNT;
                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMaterialType.Where(o => o.ID == mate.MATERIAL_TYPE_ID).First().CONCENTRA;
                            rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                            ListRdo.Add(rdo);
                        }

                        #endregion
                    }
                }
                else
                //không có chốt kì
                {
                    if (filter.IS_MEDICINE == true)
                    {
                        #region tondauthuoc

                        HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                        impFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                        impFilter.MEDI_STOCK_ID = medistock;
                        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
                        foreach (var medi in listImpMestMedicine)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = THUOC;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                            rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                            rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                            rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                            rdo.BEGIN_AMOUNT = medi.AMOUNT;
                            rdo.END_AMOUNT = medi.AMOUNT;
                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            ListRdo.Add(rdo);

                        }
                        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
                        HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                        expFilter.MEDI_STOCK_ID = medistock;
                        expFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                        expFilter.IS_EXPORT = true;
                        listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expFilter);

                        foreach (var medi in listExpMestMedicine)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = THUOC;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                            rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                            rdo.BEGIN_AMOUNT = -medi.AMOUNT;
                            rdo.END_AMOUNT = -medi.AMOUNT;
                            rdo.IMP_PRICE = medi.IMP_PRICE;
                            rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                            rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                            ListRdo.Add(rdo);
                        }

                        #endregion
                    }
                    if (filter.IS_MATERIAL == true)
                    {
                        #region tondauvattu

                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.MEDI_STOCK_ID = medistock;
                        impMateFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                        foreach (var mate in listImpMestMaterial)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = VATTU;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                            rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                            rdo.IMP_PRICE = mate.IMP_PRICE;

                            rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                            rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                            rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                            rdo.BEGIN_AMOUNT = mate.AMOUNT;
                            rdo.END_AMOUNT = mate.AMOUNT;
                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMaterialType.Where(o => o.ID == mate.MATERIAL_TYPE_ID).First().CONCENTRA;
                            rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                            ListRdo.Add(rdo);

                        }
                        HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                        expMateFilter.MEDI_STOCK_ID = medistock;
                        expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                        expMateFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                        foreach (var mate in listExpMestMaterial)
                        {
                            var rdo = new Mrs00200RDO();
                            rdo.TYPE = VATTU;
                            rdo.MEDI_STOCK_ID = medistock;
                            rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                            rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                            rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                            rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                            rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                            rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                            rdo.BEGIN_AMOUNT = -mate.AMOUNT;
                            rdo.END_AMOUNT = -mate.AMOUNT;
                            rdo.IMP_PRICE = mate.IMP_PRICE;
                            rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                            rdo.CONCENTRA = ListMaterialType.Where(o => o.ID == mate.MATERIAL_TYPE_ID).First().CONCENTRA;
                            rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                            rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                            rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                            rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;

                            ListRdo.Add(rdo);
                        }
                        #endregion
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessImpMestAmount(long medistock)
        {
            try
            {
                if (filter.IS_MEDICINE == true)
                {
                    #region nhapthuoc

                    HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                    impFilter.IMP_TIME_FROM = filter.TIME_FROM;
                    impFilter.MEDI_STOCK_ID = medistock;
                    impFilter.IMP_TIME_TO = filter.TIME_TO;
                    List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
                    foreach (var medi in listImpMestMedicine)
                    {
                        var rdo = new Mrs00200RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_STOCK_ID = medistock;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        FieldInfo[] f = (typeof(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE)).GetRuntimeFields().ToArray();
                        FieldInfo v = f.ToList().FirstOrDefault(o => medi.IMP_MEST_TYPE_ID == (long)o.GetRawConstantValue());
                        if (v != null)
                        {
                            PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00200RDO>();
                            if (pi != null)
                            {
                                var p = pi.FirstOrDefault(o => o.Name.Contains(v.Name));
                                if (p != null)
                                {
                                    p.SetValue(rdo, medi.AMOUNT);
                                }
                            }
                        }

                        rdo.IMP_TOTAL_AMOUNT = medi.AMOUNT;
                        rdo.END_AMOUNT = medi.AMOUNT;
                        rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.CONCENTRA = medi.CONCENTRA;
                        rdo.MEDICINE_TYPE_PROPRIETARY_NAME = ListMedicineType.Where(o => o.ID == medi.MEDICINE_TYPE_ID).First().MEDICINE_TYPE_PROPRIETARY_NAME;
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        ListRdo.Add(rdo);

                    }

                    #endregion
                }
                if (filter.IS_MATERIAL == true)
                {
                    #region nhapvattu

                    HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMateFilter.IMP_TIME_FROM = filter.TIME_FROM;
                    impMateFilter.IMP_TIME_TO = filter.TIME_TO;
                    impMateFilter.MEDI_STOCK_ID = medistock;
                    List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                    foreach (var mate in listImpMestMaterial)
                    {
                        var rdo = new Mrs00200RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_STOCK_ID = medistock;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        FieldInfo[] f = (typeof(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE)).GetRuntimeFields().ToArray();
                        FieldInfo v = f.ToList().FirstOrDefault(o => mate.IMP_MEST_TYPE_ID == (long)o.GetRawConstantValue());
                        if (v != null)
                        {
                            PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00200RDO>();
                            if (pi != null)
                            {
                                var p = pi.FirstOrDefault(o => o.Name.Contains(v.Name));
                                if (p != null)
                                {
                                    p.SetValue(rdo, mate.AMOUNT);
                                }
                            }
                        }


                        rdo.IMP_TOTAL_AMOUNT = mate.AMOUNT;
                        rdo.END_AMOUNT = mate.AMOUNT;
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                        ListRdo.Add(rdo);

                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessExpMestAmount(long medistock)
        {
            try
            {
                if (filter.IS_MEDICINE == true)
                {
                    #region xuatthuoc

                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                    expMediFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMediFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMediFilter.MEDI_STOCK_ID = medistock;
                    expMediFilter.IS_EXPORT = true;
                    //expMediFilter.IN_EXECUTE = true; 
                    List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter);
                    var treatmentIds = listExpMestMedicine.Where(o => o.TDL_TREATMENT_ID.HasValue).Select(p => p.TDL_TREATMENT_ID.Value).Distinct().ToList();
                    List<HIS_TREATMENT> Treatments = new List<HIS_TREATMENT>();
                    if (treatmentIds != null)
                    {
                        Treatments = new ManagerSql().Get(new HisTreatmentFilterQuery() { IDs = treatmentIds });
                    }
                    List<HIS_EXP_MEST> ExpMests = new List<HIS_EXP_MEST>();

                    var ExpMestIds = listExpMestMedicine.Where(o => o.EXP_MEST_ID.HasValue).Select(p => p.EXP_MEST_ID.Value).Distinct().ToList();
                    if (ExpMestIds != null)
                    {
                        ExpMests = new ManagerSql().Get(new HisExpMestFilterQuery() { IDs = ExpMestIds });
                    }
                    foreach (var medi in listExpMestMedicine)
                    {
                        var rdo = new Mrs00200RDO();

                        rdo.TYPE = THUOC;
                        rdo.MEDI_STOCK_ID = medistock;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.EXP_TOTAL_AMOUNT = medi.AMOUNT;
                        rdo.END_AMOUNT = -medi.AMOUNT;
                        FieldInfo[] f = (typeof(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE)).GetRuntimeFields().ToArray();
                        FieldInfo v = f.ToList().FirstOrDefault(o => medi.EXP_MEST_TYPE_ID == (long)o.GetRawConstantValue());
                        if (v != null)
                        {
                            PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00200RDO>();
                            if (pi != null)
                            {
                                var p = pi.FirstOrDefault(o => o.Name.Contains(v.Name));
                                if (p != null)
                                {
                                    p.SetValue(rdo, medi.AMOUNT);
                                }
                            }
                        }

                        if (medi.TDL_TREATMENT_ID != null)
                        {
                            if (typem(medi, Treatments) == BH_NT) rdo.EXP_PRES_NT_BH_AMOUNT = medi.AMOUNT;
                            if (typem(medi, Treatments) == BH_NGT) rdo.EXP_PRES_NGT_BH_AMOUNT = medi.AMOUNT;
                            if (typem(medi, Treatments) == ND_NT) rdo.EXP_PRES_NT_ND_AMOUNT = medi.AMOUNT;
                            if (typem(medi, Treatments) == ND_NGT) rdo.EXP_PRES_NGT_ND_AMOUNT = medi.AMOUNT;

                            if (typeT(medi, Treatments) == BH_LESS6) rdo.EXP_PRES_LESS6_BH_AMOUNT = medi.AMOUNT;
                            if (typeT(medi, Treatments) == BH_MORE6) rdo.EXP_PRES_MORE6_BH_AMOUNT = medi.AMOUNT;
                            if (typeT(medi, Treatments) == ND_LESS6) rdo.EXP_PRES_LESS6_ND_AMOUNT = medi.AMOUNT;
                            if (typeT(medi, Treatments) == ND_MORE6) rdo.EXP_PRES_MORE6_ND_AMOUNT = medi.AMOUNT;
                        }
                        var expMest = ExpMests.FirstOrDefault(o => o.ID == medi.EXP_MEST_ID);

                        if (expMest != null && expMest.EXP_MEST_REASON_ID != null)
                        {
                            
                            rdo.DIC_EXP_MEST_REASON.Add(ExpMestReasonCode(expMest.EXP_MEST_REASON_ID ?? 0, ListExpMestReason), medi.AMOUNT);
                        }
                        ListRdo.Add(rdo);

                    }

                    #endregion
                }
                if (filter.IS_MATERIAL == true)
                {
                    #region xuatvattu

                    HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                    expMateFilter.EXP_TIME_FROM = filter.TIME_FROM;
                    expMateFilter.EXP_TIME_TO = filter.TIME_TO;
                    expMateFilter.MEDI_STOCK_ID = medistock;
                    //expMateFilter.IN_EXECUTE = true; 
                    expMateFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                    var treatmentIds = listExpMestMaterial.Where(o => o.TDL_TREATMENT_ID.HasValue).Select(p => p.TDL_TREATMENT_ID.Value).Distinct().ToList();
                    List<HIS_TREATMENT> Treatments = new List<HIS_TREATMENT>();
                    if (treatmentIds != null)
                    {
                        Treatments = new ManagerSql().Get(new HisTreatmentFilterQuery() { IDs = treatmentIds });
                    }
                    List<HIS_EXP_MEST> ExpMests = new List<HIS_EXP_MEST>();

                    var ExpMestIds = listExpMestMaterial.Where(o => o.EXP_MEST_ID.HasValue).Select(p => p.EXP_MEST_ID.Value).Distinct().ToList();
                    if (ExpMestIds != null)
                    {
                        ExpMests = new ManagerSql().Get(new HisExpMestFilterQuery() { IDs = ExpMestIds });
                    }
                    foreach (var mate in listExpMestMaterial)
                    {
                        var rdo = new Mrs00200RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_STOCK_ID = medistock;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.IMP_PRICE = mate.IMP_PRICE;

                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.EXP_TOTAL_AMOUNT = mate.AMOUNT;
                        rdo.END_AMOUNT = -mate.AMOUNT;
                        FieldInfo[] f = (typeof(IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE)).GetRuntimeFields().ToArray();
                        FieldInfo v = f.ToList().FirstOrDefault(o => mate.EXP_MEST_TYPE_ID == (long)o.GetRawConstantValue());
                        if (v != null)
                        {
                            PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<Mrs00200RDO>();
                            if (pi != null)
                            {
                                var p = pi.FirstOrDefault(o => o.Name.Contains(v.Name));
                                if (p != null)
                                {
                                    p.SetValue(rdo, mate.AMOUNT);
                                }
                            }
                        }
                        if (mate.TDL_TREATMENT_ID != null)
                        {
                            if (typem(mate, Treatments) == BH_NT) rdo.EXP_PRES_NT_BH_AMOUNT = mate.AMOUNT;
                            if (typem(mate, Treatments) == BH_NGT) rdo.EXP_PRES_NGT_BH_AMOUNT = mate.AMOUNT;
                            if (typem(mate, Treatments) == ND_NT) rdo.EXP_PRES_NT_ND_AMOUNT = mate.AMOUNT;
                            if (typem(mate, Treatments) == ND_NGT) rdo.EXP_PRES_NGT_ND_AMOUNT = mate.AMOUNT;

                            if (typeT(mate, Treatments) == BH_LESS6) rdo.EXP_PRES_LESS6_BH_AMOUNT = mate.AMOUNT;
                            if (typeT(mate, Treatments) == BH_MORE6) rdo.EXP_PRES_MORE6_BH_AMOUNT = mate.AMOUNT;
                            if (typeT(mate, Treatments) == ND_LESS6) rdo.EXP_PRES_LESS6_ND_AMOUNT = mate.AMOUNT;
                            if (typeT(mate, Treatments) == ND_MORE6) rdo.EXP_PRES_MORE6_ND_AMOUNT = mate.AMOUNT;
                        }
                        var expMest = ExpMests.FirstOrDefault(o => o.ID == mate.EXP_MEST_ID);

                        if (expMest != null && expMest.EXP_MEST_REASON_ID != null)
                        {
                           
                            rdo.DIC_EXP_MEST_REASON.Add(ExpMestReasonCode(expMest.EXP_MEST_REASON_ID ?? 0, ListExpMestReason), mate.AMOUNT);
                        }
                        ListRdo.Add(rdo);

                    }

                    #endregion
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private string ExpMestReasonCode(long expMestReasonId, List<HIS_EXP_MEST_REASON> ListExpMestReason)
        {
            string result = "";
            try
            {
                result = ((ListExpMestReason.FirstOrDefault(o => o.ID == expMestReasonId) ?? new HIS_EXP_MEST_REASON()).EXP_MEST_REASON_CODE ?? "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                result = "";
            }
            return result;
        }

        private long typem(V_HIS_EXP_MEST_MEDICINE medi, List<HIS_TREATMENT> treatments)
        {
            long result = 0;
            try
            {
                var treatment = treatments.FirstOrDefault(o => o.ID == medi.TDL_TREATMENT_ID);
                if (treatment != null)
                {

                    if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = BH_NT;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = BH_NGT;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = ND_NT;
                    }
                    else result = ND_NGT;

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        private long typem(V_HIS_EXP_MEST_MATERIAL mate, List<HIS_TREATMENT> treatments)
        {
            long result = 0;
            try
            {
                var treatment = treatments.FirstOrDefault(o => o.ID == mate.TDL_TREATMENT_ID);
                if (treatment != null)
                {

                    if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = BH_NT;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = BH_NGT;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && treatment.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        result = ND_NT;
                    }
                    else result = ND_NGT;

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        private long typeT(V_HIS_EXP_MEST_MEDICINE medi, List<HIS_TREATMENT> treatments)
        {
            long result = 0;
            try
            {
                var treatment = treatments.FirstOrDefault(o => o.ID == medi.TDL_TREATMENT_ID);
                if (treatment != null)
                {

                    if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) < 6)
                    {
                        result = BH_LESS6;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) >= 6)
                    {
                        result = BH_MORE6;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) < 6)
                    {
                        result = ND_LESS6;
                    }
                    else result = ND_MORE6;

                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }

        private long typeT(V_HIS_EXP_MEST_MATERIAL mate, List<HIS_TREATMENT> treatments)
        {
            long result = 0;
            try
            {
                var treatment = treatments.FirstOrDefault(o => o.ID == mate.TDL_TREATMENT_ID);
                if (treatment != null)
                {

                    if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) < 6)
                    {
                        result = BH_LESS6;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) >= 6)
                    {
                        result = BH_MORE6;
                    }
                    else if (treatment.TDL_PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT && Age(treatment.IN_TIME, treatment.TDL_PATIENT_DOB) < 6)
                    {
                        result = ND_LESS6;
                    }
                    else result = ND_MORE6;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return 0;
            }
            return result;
        }
        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        private void ProcessGroup(long medistock)
        {
            try
            {
                GroupByID();
                GroupByPrice();
                AddInfo();
                AddInfoGroup(ListRdo);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupByID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_ID, o.TYPE}).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00200RDO rdo;
                List<Mrs00200RDO> listSub;
               
                PropertyInfo[] pi = Properties.Get<Mrs00200RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00200RDO();
                    listSub = item.ToList<Mrs00200RDO>();
                    foreach (var i in listSub)
                    {
                        if (i.DIC_EXP_MEST_REASON != null)
                        {
                            if (rdo.DIC_EXP_MEST_REASON == null)
                            {
                                rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                            }
                            foreach (var dc in i.DIC_EXP_MEST_REASON )
                            {
                                if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                                {
                                    rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                                }
                                else
                                {
                                    rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                                }
                            }

                        }
                    }
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else if (!field.Name.Contains("DIC_EXP_MEST_REASON"))
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private Mrs00200RDO IsMeaningful(List<Mrs00200RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00200RDO();
        }

        private void GroupByPrice()
        {
            try
            {
                var group = ListRdo.GroupBy(o => new { o.MEDI_MATE_TYPE_ID, o.IMP_PRICE, o.TYPE, o.SUPPLIER_ID }).ToList();
                ListRdo.Clear();
                Decimal sum = 0;
                Mrs00200RDO rdo;
                List<Mrs00200RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00200RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00200RDO();
                    listSub = item.ToList<Mrs00200RDO>();
                    foreach (var i in listSub)
                    {
                        if (i.DIC_EXP_MEST_REASON != null)
                        {
                            if (rdo.DIC_EXP_MEST_REASON == null)
                            {
                                rdo.DIC_EXP_MEST_REASON = new Dictionary<string, decimal>();
                            }
                            foreach (var dc in i.DIC_EXP_MEST_REASON)
                            {
                                if (rdo.DIC_EXP_MEST_REASON.ContainsKey(dc.Key))
                                {
                                    rdo.DIC_EXP_MEST_REASON[dc.Key] += dc.Value;
                                }
                                else
                                {
                                    rdo.DIC_EXP_MEST_REASON.Add(dc.Key, dc.Value);
                                }
                            }

                        }
                    }
                    bool hide = true;
                    foreach (var field in pi)
                    {
                        if (field.Name.Contains("_AMOUNT"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else if (!field.Name.Contains("DIC_EXP_MEST_REASON"))
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void AddInfo()
        {
            try
            {
                foreach (var item in ListRdo)
                {
                    item.SERVICE_TYPE_NAME = item.TYPE == 1 ? "THUỐC" : "VẬT TƯ";
                    item.VAT_RATIO_STR = string.Format("'{0}%", (long)(100 * item.VAT_RATIO ?? 0));
                }
            }
            catch (Exception ex)
            {

                LogSystem.Error(ex);
            }
        }
        private void AddInfoGroup(List<Mrs00200RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicineType = ListMedicineType.FirstOrDefault(o => o.ID == item.MEDI_MATE_TYPE_ID);
                if (item.TYPE == THUOC)
                {
                    if (medicineType != null && medicineType.MEDICINE_LINE_ID != null)
                    {
                        item.MEDICINE_LINE_ID = medicineType.MEDICINE_LINE_ID;
                        item.MEDICINE_LINE_CODE = medicineType.MEDICINE_LINE_CODE;
                        item.MEDICINE_LINE_NAME = medicineType.MEDICINE_LINE_NAME;
                    }
                    else
                    {
                        item.MEDICINE_LINE_ID = 0;
                        item.MEDICINE_LINE_CODE = "DTK";
                        item.MEDICINE_LINE_NAME = "Dòng thuốc khác";
                    }
                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID != null)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Thường";
                    }

                    if (medicineType != null && medicineType.PARENT_ID != null)
                    {
                        var parentMedicineType = ListMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                        if (parentMedicineType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMedicineType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMedicineType.MEDICINE_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMedicineType.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm thuốc khác";
                    }
                }
                if (item.TYPE == VATTU)
                {
                    var materialType = ListMaterialType.FirstOrDefault(o => o.ID == item.MEDI_MATE_TYPE_ID);
                    item.MEDICINE_LINE_CODE = "DVT";
                    item.MEDICINE_LINE_NAME = "Vật tư";
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                    if (materialType != null && materialType.PARENT_ID != null)
                    {
                        var parentMaterialType = ListMaterialType.FirstOrDefault(o => o.ID == materialType.PARENT_ID);
                        if (parentMaterialType != null)
                        {
                            item.PARENT_MEDICINE_TYPE_ID = parentMaterialType.ID;
                            item.PARENT_MEDICINE_TYPE_CODE = parentMaterialType.MATERIAL_TYPE_CODE;
                            item.PARENT_MEDICINE_TYPE_NAME = parentMaterialType.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            item.PARENT_MEDICINE_TYPE_ID = 0;
                            item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                            item.PARENT_MEDICINE_TYPE_NAME = "Nhóm Vật tư khác";
                        }
                    }
                    else
                    {
                        item.PARENT_MEDICINE_TYPE_ID = 0;
                        item.PARENT_MEDICINE_TYPE_CODE = "NVTK";
                        item.PARENT_MEDICINE_TYPE_NAME = "Nhóm Vật tư khác";
                    }
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            #region Cac the Single
            if (filter.TIME_FROM > 0)
            {
                dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM));
            }
            if (filter.TIME_TO > 0)
            {
                dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO));
            }
            dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(",", ListMediStock.Select(o => o.MEDI_STOCK_NAME).ToList()));
            #endregion

            ListRdo = ListRdo.OrderBy(o => o.MEDI_STOCK_ID).ThenBy(t1 => t1.TYPE).ThenBy(t3 => t3.SERVICE_NAME).ToList();


            objectTag.AddObjectData(store, "Services", ListRdo); 
            objectTag.AddObjectData(store, "MedicineGroup", ListRdo.GroupBy(o => o.MEDICINE_GROUP_ID).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "MedicineGroup", "Services", "MEDICINE_GROUP_ID", "MEDICINE_GROUP_ID");


            objectTag.AddObjectData(store, "GrandParent", ListRdo.GroupBy(o => o.PARENT_MEDICINE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => new { o.MEDICINE_LINE_ID, o.PARENT_MEDICINE_TYPE_ID }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "GrandParent", "Parent", "PARENT_MEDICINE_TYPE_ID", "PARENT_MEDICINE_TYPE_ID");

            objectTag.AddRelationship(store, "Parent", "Services", new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" }, new string[] { "MEDICINE_LINE_ID", "PARENT_MEDICINE_TYPE_ID" });

            objectTag.SetUserFunction(store, "Element", new RDOElement());
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncRownumber", new RDOCustomerFuncManyRownumberData());
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
