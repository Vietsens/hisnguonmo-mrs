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
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisMestPeriodMate;
using MOS.MANAGER.HisMestPeriodMedi;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MRS.Proccessor.Mrs00509;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00509
{
    public class Mrs00509Processor : AbstractProcessor
    {
        private List<Mrs00509RDO> ListRdo = new List<Mrs00509RDO>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        List<long> listImpMediStockId = new List<long>();
        V_HIS_MEDI_STOCK MediStock = new V_HIS_MEDI_STOCK();
        private Dictionary<long, V_HIS_EXP_MEST> dicPrescription = new Dictionary<long, V_HIS_EXP_MEST>();

        private Dictionary<long, List<V_HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<V_HIS_SERE_SERV>>();
        Mrs00509Filter filter = null;
        const long THUOC = 1;
        const long VATTU = 2;

        string thisReportTypeCode = "";
        public Mrs00509Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00509Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00509Filter)this.reportFilter;
            try
            {
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00509: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => filter), filter));
                ///Danh sách kho
                MediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == filter.MEDI_STOCK_ID);

                //Danh sách kho nhận từ kho đó
                HisExpMestFilterQuery chmsExpFilter = new HisExpMestFilterQuery();
                chmsExpFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_ID;
                chmsExpFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__CK;
                chmsExpFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                listImpMediStockId = (new HisExpMestManager().Get(chmsExpFilter) ?? new List<HIS_EXP_MEST>()).Select(o => o.IMP_MEDI_STOCK_ID ?? 0).Distinct().ToList();

                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
                if (IsNotNullOrEmpty(ListMedicineType))
                {
                    foreach (var item in ListMedicineType)
                    {
                        dicMedicineType[item.ID] = item;
                    }
                }

                if (IsNotNullOrEmpty(ListMaterialType))
                {
                    foreach (var item in ListMaterialType)
                    {
                        dicMaterialType[item.ID] = item;
                    }
                }

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

                //tính tồn đầu
                ProcessBeginAmount(MediStock.ID, true);
                // nhập trong kỳ
                ProcessImpMestAmount(MediStock.ID, true);
                // xuất trong kỳ
                ProcessExpMestAmount(MediStock.ID, true);

                foreach (var item in listImpMediStockId)
                {
                    //tính tồn đầu
                    ProcessBeginAmount(item, false);
                    // nhập trong kỳ
                    ProcessImpMestAmount(item, false);
                    // xuất trong kỳ
                    ProcessExpMestAmount(item, false);
                }
                //Gộp theo thuốc, theo giá
                ProcessGroup();

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00509RDO>();
                result = false;
            }
            return result;
        }

        private void ProcessBeginAmount(long medistock, bool main)
        {
            //try
            //{
            HisMediStockPeriodFilterQuery periodFilter = new HisMediStockPeriodFilterQuery();
            //periodFilter.CREATE_TIME_FROM = filter.TIME_FROM;
            periodFilter.CREATE_TIME_TO = filter.TIME_FROM;
            periodFilter.MEDI_STOCK_ID = medistock;
            periodFilter.ORDER_DIRECTION = "DESC";
            periodFilter.ORDER_FIELD = "CREATE_TIME";
            var listPeriod = new HisMediStockPeriodManager(paramGet).Get(periodFilter);
            //Có chốt kì
            if (IsNotNullOrEmpty(listPeriod))
            {
                var lastPeriod = listPeriod.First();
                HisMestPeriodMediViewFilterQuery mestPeriodMediFilter = new HisMestPeriodMediViewFilterQuery();
                mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = lastPeriod.ID;
                List<V_HIS_MEST_PERIOD_MEDI> listMestPeriodMedi = new HisMestPeriodMediManager(paramGet).GetView(mestPeriodMediFilter);
                HisMestPeriodMateViewFilterQuery mestPeriodMateFilter = new HisMestPeriodMateViewFilterQuery();
                mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = lastPeriod.ID;
                List<V_HIS_MEST_PERIOD_MATE> listMestPeriodMate = new HisMestPeriodMateManager(paramGet).GetView(mestPeriodMateFilter);

                #region tondauthuoc
                if (IsNotNullOrEmpty(listMestPeriodMedi))
                {
                    foreach (var medi in listMestPeriodMedi)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = medi.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = medi.AMOUNT;
                        }

                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;

                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;

                        ListRdo.Add(rdo);
                    }
                }

                HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                impFilter.IMP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                impFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                impFilter.MEDI_STOCK_ID = medistock;
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;

                List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
                LogSystem.Info("3: " + listImpMestMedicine.Count);
                if (IsNotNullOrEmpty(listImpMestMedicine))
                {
                    foreach (var medi in listImpMestMedicine)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = medi.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = medi.AMOUNT;
                        }
                        ListRdo.Add(rdo);

                    }
                }

                HisExpMestMedicineViewFilterQuery expFilter = new HisExpMestMedicineViewFilterQuery();
                expFilter.EXP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                expFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                expFilter.MEDI_STOCK_ID = medistock;
                expFilter.IS_EXPORT = true;

                List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expFilter);
                LogSystem.Info("4: " + listExpMestMedicine.Count);
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    foreach (var medi in listExpMestMedicine)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = -medi.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = -medi.AMOUNT;
                        }

                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;

                        ListRdo.Add(rdo);
                    }
                }

                #endregion

                #region tondauvattu
                if (IsNotNullOrEmpty(listMestPeriodMate))
                {
                    foreach (var mate in listMestPeriodMate)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = mate.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = mate.AMOUNT;
                        }

                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;

                        ListRdo.Add(rdo);

                    }
                }

                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.MEDI_STOCK_ID = medistock;
                impMestFilter.IMP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                impMestFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<HIS_IMP_MEST> listImpMest = new HisImpMestManager().Get(impMestFilter);
                var ImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
                List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                var skip1 = 0;
                while (ImpMestId.Count - skip1 > 0)
                {
                    var listIDs = ImpMestId.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMateFilter.IMP_MEST_IDs = listIDs;

                    List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                    if (hisImpMestMaterialSub == null)
                        foreach (var item in listIDs)
                        {
                            HisImpMestMaterialViewFilterQuery impMateFilterSub = new HisImpMestMaterialViewFilterQuery();
                            impMateFilterSub.IMP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                            impMateFilterSub.IMP_TIME_TO = filter.TIME_FROM - 1;
                            impMateFilterSub.MEDI_STOCK_ID = medistock;
                            impMateFilterSub.IMP_MEST_ID = item;
                            impMateFilterSub.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                            hisImpMestMaterialSub = new HisImpMestMaterialManager().GetView(impMateFilterSub);
                            if (hisImpMestMaterialSub == null) Inventec.Common.Logging.LogSystem.Info("Error Medicine_id:" + item.ToString());
                            listImpMestMaterial.AddRange(hisImpMestMaterialSub ?? new List<V_HIS_IMP_MEST_MATERIAL>());
                        }
                    else

                        listImpMestMaterial.AddRange(hisImpMestMaterialSub ?? new List<V_HIS_IMP_MEST_MATERIAL>());
                }

                LogSystem.Info("5: " + listImpMestMaterial.Count);
                if (IsNotNullOrEmpty(listImpMestMaterial))
                {
                    foreach (var mate in listImpMestMaterial)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.IMP_PRICE = mate.IMP_PRICE;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = mate.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = mate.AMOUNT;
                        }

                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                        ListRdo.Add(rdo);

                    }
                }
                //LogSystem.Info("4: " + string.Join(", ", ListRdo.Sum(o => o.IMP_CHMS_AMOUNT)));

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.MEDI_STOCK_ID = medistock;
                expMateFilter.EXP_TIME_FROM = lastPeriod.CREATE_TIME + 1;
                expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                LogSystem.Info("6: " + listExpMestMaterial.Count);
                if (IsNotNullOrEmpty(listExpMestMaterial))
                {
                    foreach (var mate in listExpMestMaterial)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = -mate.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = -mate.AMOUNT;
                        }

                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                        ListRdo.Add(rdo);
                    }
                }
                //LogSystem.Info("5: " + string.Join(", ", ListRdo.Sum(o => o.IMP_CHMS_AMOUNT)));

                #endregion
                //}
            }
            else
            //không có chốt kì
            {
                //if (filter.IS_MEDICINE)
                //{
                #region tondauthuoc

                HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
                impFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                impFilter.MEDI_STOCK_ID = medistock;
                impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
                LogSystem.Info("7: " + listImpMestMedicine.Count);

                if (IsNotNullOrEmpty(listImpMestMedicine))
                {
                    foreach (var medi in listImpMestMedicine)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                        rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                        rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = medi.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = medi.AMOUNT;
                        }

                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        ListRdo.Add(rdo);

                    }
                }
                //LogSystem.Info("6: " + string.Join(", ", ListRdo.Sum(o => o.IMP_CHMS_AMOUNT)));
                List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
                var medicineIds = listImpMestMedicine.Select(o => o.MEDICINE_ID).Distinct().ToList();
                var skip = 0;
                while (medicineIds.Count - skip > 0)
                {
                    var listIDs = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
                    expMediFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                    expMediFilter.MEDI_STOCK_ID = medistock;
                    expMediFilter.MEDICINE_IDs = listIDs;
                    expMediFilter.IS_EXPORT = true;
                    List<V_HIS_EXP_MEST_MEDICINE> hisExpMestMedicineSub = new HisExpMestMedicineManager().GetView(expMediFilter);

                    if (hisExpMestMedicineSub == null)
                        foreach (var item in listIDs)
                        {
                            HisExpMestMedicineViewFilterQuery expMediFilterSub = new HisExpMestMedicineViewFilterQuery();
                            expMediFilterSub.EXP_TIME_TO = filter.TIME_FROM - 1;
                            expMediFilterSub.MEDI_STOCK_ID = medistock;
                            expMediFilterSub.MEDICINE_ID = item;
                            expMediFilterSub.IS_EXPORT = true;
                            hisExpMestMedicineSub = new HisExpMestMedicineManager().GetView(expMediFilter);
                            if (hisExpMestMedicineSub == null) Inventec.Common.Logging.LogSystem.Info("Error Medicine_id:" + item.ToString());
                            listExpMestMedicine.AddRange(hisExpMestMedicineSub ?? new List<V_HIS_EXP_MEST_MEDICINE>());
                        }
                    else
                        listExpMestMedicine.AddRange(hisExpMestMedicineSub);
                }
                LogSystem.Info("8: " + listExpMestMedicine.Count);
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    var amountAsel = listExpMestMedicine.Where(o => o.MEDICINE_TYPE_ID == 764).Sum(s => s.AMOUNT);
                    foreach (var medi in listExpMestMedicine)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = THUOC;
                        rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = -medi.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = -medi.AMOUNT;
                        }

                        rdo.IMP_PRICE = medi.IMP_PRICE;
                        rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                            rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                            rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                        ListRdo.Add(rdo);
                    }
                }
                //LogSystem.Info("7: " + string.Join(", ", ListRdo.Sum(o => o.IMP_CHMS_AMOUNT)));

                #endregion
                //}
                //if (filter.IS_MATERIAL)
                //{
                #region tondauvattu
                HisImpMestFilterQuery impMestFilter = new HisImpMestFilterQuery();
                impMestFilter.MEDI_STOCK_ID = medistock;
                impMestFilter.IMP_TIME_TO = filter.TIME_FROM - 1;
                impMestFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                List<HIS_IMP_MEST> listImpMest = new HisImpMestManager().Get(impMestFilter);
                var ImpMestId = listImpMest.Select(o => o.ID).Distinct().ToList();
                List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                skip = 0;
                while (ImpMestId.Count - skip > 0)
                {
                    var listIDs = ImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                    impMateFilter.IMP_MEST_IDs = listIDs;

                    List<V_HIS_IMP_MEST_MATERIAL> hisImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);
                    if (hisImpMestMaterialSub == null)
                        foreach (var item in listIDs)
                        {
                            HisImpMestMaterialViewFilterQuery impMateFilterSub = new HisImpMestMaterialViewFilterQuery();
                            impMateFilterSub.IMP_TIME_TO = filter.TIME_FROM - 1;
                            impMateFilterSub.MEDI_STOCK_ID = medistock;
                            impMateFilterSub.IMP_MEST_ID = item;
                            impMateFilterSub.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                            hisImpMestMaterialSub = new HisImpMestMaterialManager().GetView(impMateFilterSub);
                            if (hisImpMestMaterialSub == null) Inventec.Common.Logging.LogSystem.Info("Error Medicine_id:" + item.ToString());
                            listImpMestMaterial.AddRange(hisImpMestMaterialSub ?? new List<V_HIS_IMP_MEST_MATERIAL>());
                        }
                    else

                        listImpMestMaterial.AddRange(hisImpMestMaterialSub ?? new List<V_HIS_IMP_MEST_MATERIAL>());

                }
                LogSystem.Info("9: " + listImpMestMaterial.Count);
                if (IsNotNullOrEmpty(listImpMestMaterial))
                {
                    foreach (var mate in listImpMestMaterial)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.IMP_PRICE = mate.IMP_PRICE;

                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = mate.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = mate.AMOUNT;
                        }

                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;
                        ListRdo.Add(rdo);

                    }
                }
                //LogSystem.Info("Ton dau vat tu: " + string.Join(", ", ListRdo.Sum(o => o.BEGIN_AMOUNT)));

                HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
                expMateFilter.MEDI_STOCK_ID = medistock;
                expMateFilter.EXP_TIME_TO = filter.TIME_FROM - 1;
                expMateFilter.IS_EXPORT = true;
                List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter);
                LogSystem.Info("10: " + listExpMestMaterial.Count);
                if (IsNotNullOrEmpty(listExpMestMaterial))
                {
                    foreach (var mate in listExpMestMaterial)
                    {
                        var rdo = new Mrs00509RDO();
                        rdo.TYPE = VATTU;
                        rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                        rdo.VAT_RATIO = mate.IMP_VAT_RATIO;
                        rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                        rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;

                        if (main)
                            rdo.MAIN_BEGIN_AMOUNT = -mate.AMOUNT;
                        else
                        {
                            rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                            rdo.EXTRA_BEGIN_AMOUNT = -mate.AMOUNT;
                        }

                        rdo.IMP_PRICE = mate.IMP_PRICE;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                        {
                            rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                            rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                            rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                            rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                        }
                        rdo.NATIONAL_NAME = mate.NATIONAL_NAME;
                        rdo.SUPPLIER_ID = mate.SUPPLIER_ID;
                        rdo.SUPPLIER_CODE = mate.SUPPLIER_CODE;
                        rdo.SUPPLIER_NAME = mate.SUPPLIER_NAME;

                        ListRdo.Add(rdo);
                    }
                }

                #endregion
            }

        }

        private void ProcessImpMestAmount(long medistock, bool main)
        {

            #region nhapthuoc

            HisImpMestMedicineViewFilterQuery impFilter = new HisImpMestMedicineViewFilterQuery();
            impFilter.IMP_TIME_FROM = filter.TIME_FROM;
            impFilter.MEDI_STOCK_ID = medistock;
            impFilter.IMP_TIME_TO = filter.TIME_TO;
            impFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(impFilter);
            if (IsNotNullOrEmpty(listImpMestMedicine))
            {
                foreach (var medi in listImpMestMedicine)
                {
                    var rdo = new Mrs00509RDO();
                    rdo.TYPE = THUOC;
                    rdo.MEDI_MATE_ID = medi.MEDICINE_ID;
                    rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                    rdo.IMP_PRICE = medi.IMP_PRICE;

                    if (main)
                        rdo.MAIN_IMP_AMOUNT = medi.AMOUNT;
                    else
                    {
                        rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                        rdo.EXTRA_IMP_AMOUNT = medi.AMOUNT;
                    }

                    rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                    rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                    rdo.VAT_RATIO = medi.IMP_VAT_RATIO;
                    if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                    {
                        rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                        rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                        rdo.MANUFACTURER_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MANUFACTURER_NAME;
                        rdo.HEIN_SERVICE_CODE = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    }
                    rdo.NATIONAL_NAME = medi.NATIONAL_NAME;
                    rdo.SUPPLIER_ID = medi.SUPPLIER_ID;
                    rdo.SUPPLIER_CODE = medi.SUPPLIER_CODE;
                    rdo.SUPPLIER_NAME = medi.SUPPLIER_NAME;
                    ListRdo.Add(rdo);

                }
            }
            //LogSystem.Info("10: " + string.Join(", ", ListRdo.Sum(o => o.IMP_CHMS_AMOUNT)));

            #endregion

            #region nhapvattu

            HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
            impMateFilter.IMP_TIME_FROM = filter.TIME_FROM;
            impMateFilter.IMP_TIME_TO = filter.TIME_TO;
            impMateFilter.MEDI_STOCK_ID = medistock;
            impMateFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);

            if (IsNotNullOrEmpty(listImpMestMaterial))
            {
                foreach (var mate in listImpMestMaterial)
                {
                    var rdo = new Mrs00509RDO();
                    rdo.TYPE = VATTU;
                    rdo.MEDI_MATE_ID = mate.MATERIAL_ID;
                    rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                    rdo.IMP_PRICE = mate.IMP_PRICE;

                    if (main)
                        rdo.MAIN_IMP_AMOUNT = mate.AMOUNT;
                    else
                    {
                        rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                        rdo.EXTRA_IMP_AMOUNT = mate.AMOUNT;
                    }

                    if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                    {
                        rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                        rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                        rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    }
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
            }
            #endregion

        }

        private void ProcessExpMestAmount(long medistock, bool main)
        {
            #region xuatthuoc

            HisExpMestMedicineViewFilterQuery expMediFilter = new HisExpMestMedicineViewFilterQuery();
            expMediFilter.EXP_TIME_FROM = filter.TIME_FROM;
            expMediFilter.EXP_TIME_TO = filter.TIME_TO;
            expMediFilter.MEDI_STOCK_ID = medistock;
            expMediFilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(expMediFilter) ?? new List<V_HIS_EXP_MEST_MEDICINE>();

            if (IsNotNullOrEmpty(listExpMestMedicine))
            {
                foreach (var medi in listExpMestMedicine)
                {
                    var rdo = new Mrs00509RDO();
                    rdo.TYPE = THUOC;
                    rdo.MEDI_MATE_ID = medi.MEDICINE_ID ?? 0;
                    rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);
                    rdo.MEDI_MATE_TYPE_ID = medi.MEDICINE_TYPE_ID;
                    rdo.SERVICE_CODE = medi.MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = medi.MEDICINE_TYPE_NAME;
                    rdo.VAT_RATIO = medi.IMP_VAT_RATIO;

                    if (main)
                        rdo.MAIN_EXP_AMOUNT = medi.AMOUNT;
                    else
                    {
                        rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                        rdo.EXTRA_EXP_AMOUNT = medi.AMOUNT;
                        if (bhyt(medi))
                            rdo.EXTRA_EXP_AMOUNT_BHYT = medi.AMOUNT;
                        else
                            rdo.EXTRA_EXP_AMOUNT_DV = medi.AMOUNT;
                    }

                    if (dicMedicineType.ContainsKey(medi.MEDICINE_TYPE_ID))
                    {
                        rdo.CONCENTRA = dicMedicineType[medi.MEDICINE_TYPE_ID].CONCENTRA;
                        rdo.MEDICINE_TYPE_PROPRIETARY_NAME = dicMedicineType[medi.MEDICINE_TYPE_ID].MEDICINE_TYPE_PROPRIETARY_NAME;
                    }
                    ListRdo.Add(rdo);

                }
            }

            #endregion


            #region xuatvattu

            HisExpMestMaterialViewFilterQuery expMateFilter = new HisExpMestMaterialViewFilterQuery();
            expMateFilter.EXP_TIME_FROM = filter.TIME_FROM;
            expMateFilter.EXP_TIME_TO = filter.TIME_TO;
            expMateFilter.MEDI_STOCK_ID = medistock;
            expMateFilter.IS_EXPORT = true;
            List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(expMateFilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>();

            if (IsNotNullOrEmpty(listExpMestMaterial))
            {
                foreach (var mate in listExpMestMaterial)
                {
                    var rdo = new Mrs00509RDO();
                    rdo.TYPE = VATTU;
                    rdo.MEDI_MATE_ID = mate.MATERIAL_ID ?? 0;
                    rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                    rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);
                    rdo.IMP_PRICE = mate.IMP_PRICE;

                    rdo.MEDI_MATE_TYPE_ID = mate.MATERIAL_TYPE_ID;
                    rdo.SERVICE_CODE = mate.MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = mate.MATERIAL_TYPE_NAME;
                    rdo.VAT_RATIO = mate.IMP_VAT_RATIO;

                    if (main)
                        rdo.MAIN_EXP_AMOUNT = mate.AMOUNT;
                    else
                    {
                        rdo.IMP_DEPARTMENT_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medistock).DEPARTMENT_NAME;
                        rdo.EXTRA_EXP_AMOUNT = mate.AMOUNT;
                        if (bhyt(mate))
                            rdo.EXTRA_EXP_AMOUNT_BHYT = mate.AMOUNT;
                        else
                            rdo.EXTRA_EXP_AMOUNT_DV = mate.AMOUNT;
                    }

                    if (dicMaterialType.ContainsKey(mate.MATERIAL_TYPE_ID))
                    {
                        rdo.CONCENTRA = dicMaterialType[mate.MATERIAL_TYPE_ID].CONCENTRA;
                        rdo.MANUFACTURER_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].MANUFACTURER_NAME;
                        rdo.HEIN_SERVICE_CODE = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_CODE;
                        rdo.HEIN_SERVICE_NAME = dicMaterialType[mate.MATERIAL_TYPE_ID].HEIN_SERVICE_BHYT_NAME;
                    }
                    ListRdo.Add(rdo);

                }
            }

            #endregion

        }

        private bool bhyt(V_HIS_EXP_MEST_MEDICINE medi)
        {
            return medi.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        }

        private bool bhyt(V_HIS_EXP_MEST_MATERIAL mate)
        {
            return mate.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
        }

        private void ProcessGroup()
        {
            try
            {
                GroupByID();
                GroupByPrice();
                AddInfo();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void GroupByID()
        {

            List<Mrs00509RDO> x = ListRdo.GroupBy(o => new { o.MEDI_MATE_ID, o.TYPE }).Select(s => new Mrs00509RDO
            {
                MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                TYPE = s.First().TYPE,
                MAIN_BEGIN_AMOUNT = s.Sum(sum => sum.MAIN_BEGIN_AMOUNT),
                EXTRA_BEGIN_AMOUNT = s.Sum(sum => sum.EXTRA_BEGIN_AMOUNT),
                MAIN_IMP_AMOUNT = s.Sum(sum => sum.MAIN_IMP_AMOUNT),
                MAIN_EXP_AMOUNT = s.Sum(sum => sum.MAIN_EXP_AMOUNT),
                EXTRA_IMP_AMOUNT = s.Sum(sum => sum.EXTRA_IMP_AMOUNT),
                EXTRA_EXP_AMOUNT = s.Sum(sum => sum.EXTRA_EXP_AMOUNT),
                EXTRA_EXP_AMOUNT_DV = s.Sum(sum => sum.EXTRA_EXP_AMOUNT_DV),
                EXTRA_EXP_AMOUNT_BHYT = s.Sum(sum => sum.EXTRA_EXP_AMOUNT_BHYT),
                IMP_PRICE = s.Where(i => i.IMP_PRICE != null).ToList().Count > 0 ? s.Where(i => i.IMP_PRICE != null).ToList().First().IMP_PRICE : 0,
                VAT_RATIO = s.Where(i => i.VAT_RATIO != null).ToList().Count > 0 ? s.Where(i => i.VAT_RATIO != null).ToList().First().VAT_RATIO : 0,
                MEDI_MATE_TYPE_ID = s.Where(i => i.MEDI_MATE_TYPE_ID != null).ToList().Count > 0 ? s.Where(i => i.MEDI_MATE_TYPE_ID != null).ToList().First().MEDI_MATE_TYPE_ID : 0,
                SERVICE_CODE = s.Where(i => i.SERVICE_CODE != null).ToList().Count > 0 ? s.Where(i => i.SERVICE_CODE != null).ToList().First().SERVICE_CODE : "----",
                SERVICE_NAME = s.Where(i => i.SERVICE_NAME != null).ToList().Count > 0 ? s.Where(i => i.SERVICE_NAME != null).ToList().First().SERVICE_NAME : "----",
                SERVICE_UNIT_NAME = s.Where(i => i.SERVICE_UNIT_NAME != null).ToList().Count > 0 ? s.Where(i => i.SERVICE_UNIT_NAME != null).ToList().First().SERVICE_UNIT_NAME : "",
                MEDICINE_TYPE_PROPRIETARY_NAME = s.Where(i => i.MEDICINE_TYPE_PROPRIETARY_NAME != null).ToList().Count > 0 ? s.Where(i => i.MEDICINE_TYPE_PROPRIETARY_NAME != null).ToList().First().MEDICINE_TYPE_PROPRIETARY_NAME : "",
                NATIONAL_NAME = s.Where(i => i.NATIONAL_NAME != null).ToList().Count > 0 ? s.Where(i => i.NATIONAL_NAME != null).ToList().First().NATIONAL_NAME : "",
                MANUFACTURER_NAME = s.Where(i => i.MANUFACTURER_NAME != null).ToList().Count > 0 ? s.Where(i => i.MANUFACTURER_NAME != null).ToList().First().MANUFACTURER_NAME : "",
                HEIN_SERVICE_CODE = s.Where(i => i.HEIN_SERVICE_CODE != null).ToList().Count > 0 ? s.Where(i => i.HEIN_SERVICE_CODE != null).ToList().First().HEIN_SERVICE_CODE : "",
                HEIN_SERVICE_NAME = s.Where(i => i.HEIN_SERVICE_NAME != null).ToList().Count > 0 ? s.Where(i => i.HEIN_SERVICE_NAME != null).ToList().First().HEIN_SERVICE_NAME : "",
                PACKAGE_NUMBER = s.Where(i => i.PACKAGE_NUMBER != null).ToList().Count > 0 ? s.Where(i => i.PACKAGE_NUMBER != null).ToList().First().PACKAGE_NUMBER : "",
                EXPIRED_DATE_STR = s.Where(i => i.EXPIRED_DATE_STR != null).ToList().Count > 0 ? s.Where(i => i.EXPIRED_DATE_STR != null).ToList().First().EXPIRED_DATE_STR : "",
                SUPPLIER_CODE = s.Where(i => i.SUPPLIER_CODE != null).ToList().Count > 0 ? s.Where(i => i.SUPPLIER_CODE != null).ToList().First().SUPPLIER_CODE : "",
                SUPPLIER_NAME = s.Where(i => i.SUPPLIER_NAME != null).ToList().Count > 0 ? s.Where(i => i.SUPPLIER_NAME != null).ToList().First().SUPPLIER_NAME : "",
                IMP_DEPARTMENT_NAME = s.Where(i => i.IMP_DEPARTMENT_NAME != null).ToList().Count > 0 ? s.Where(i => i.IMP_DEPARTMENT_NAME != null).ToList().First().IMP_DEPARTMENT_NAME : "",

            }).ToList();
            ListRdo.Clear();
            ListRdo.AddRange(x);
        }

        private void GroupByPrice()
        {
            List<Mrs00509RDO> x = ListRdo.GroupBy(o => new { o.MEDI_MATE_TYPE_ID, o.IMP_PRICE, o.TYPE }).Select(s => new Mrs00509RDO
            {
                MEDI_MATE_ID = s.First().MEDI_MATE_ID,
                TYPE = s.First().TYPE,
                MAIN_BEGIN_AMOUNT = s.Sum(sum => sum.MAIN_BEGIN_AMOUNT),
                EXTRA_BEGIN_AMOUNT = s.Sum(sum => sum.EXTRA_BEGIN_AMOUNT),
                MAIN_IMP_AMOUNT = s.Sum(sum => sum.MAIN_IMP_AMOUNT),
                MAIN_EXP_AMOUNT = s.Sum(sum => sum.MAIN_EXP_AMOUNT),
                EXTRA_IMP_AMOUNT = s.Sum(sum => sum.EXTRA_IMP_AMOUNT),
                EXTRA_EXP_AMOUNT = s.Sum(sum => sum.EXTRA_EXP_AMOUNT),
                EXTRA_EXP_AMOUNT_DV = s.Sum(sum => sum.EXTRA_EXP_AMOUNT_DV),
                EXTRA_EXP_AMOUNT_BHYT = s.Sum(sum => sum.EXTRA_EXP_AMOUNT_BHYT),
                IMP_PRICE = s.First().IMP_PRICE,
                VAT_RATIO = s.First().VAT_RATIO,

                MEDI_MATE_TYPE_ID = s.First().MEDI_MATE_TYPE_ID,
                SERVICE_CODE = s.First().SERVICE_CODE,
                SERVICE_NAME = s.First().SERVICE_NAME,
                SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                MEDICINE_TYPE_PROPRIETARY_NAME = s.First().MEDICINE_TYPE_PROPRIETARY_NAME,
                NATIONAL_NAME = s.First().NATIONAL_NAME,
                MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                HEIN_SERVICE_CODE = s.First().HEIN_SERVICE_CODE,
                HEIN_SERVICE_NAME = s.First().HEIN_SERVICE_NAME,
                PACKAGE_NUMBER = s.First().PACKAGE_NUMBER,
                EXPIRED_DATE_STR = s.First().EXPIRED_DATE_STR,
                SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                IMP_DEPARTMENT_NAME = s.First().IMP_DEPARTMENT_NAME
            }).ToList();
            ListRdo.Clear();
            ListRdo.AddRange(x);

        }

        private void AddInfo()
        {

            List<Mrs00509RDO> x = ListRdo.Select(s => new Mrs00509RDO
            {

                SERVICE_TYPE_NAME = s.TYPE == THUOC ? "THUỐC" : "VẬT TƯ",
                MEDI_MATE_ID = s.MEDI_MATE_ID,
                TYPE = s.TYPE,
                MAIN_BEGIN_AMOUNT = s.MAIN_BEGIN_AMOUNT,
                EXTRA_BEGIN_AMOUNT = s.EXTRA_BEGIN_AMOUNT,
                MAIN_IMP_AMOUNT = s.MAIN_IMP_AMOUNT,
                MAIN_EXP_AMOUNT = s.MAIN_EXP_AMOUNT,
                EXTRA_IMP_AMOUNT = s.EXTRA_IMP_AMOUNT,
                EXTRA_EXP_AMOUNT = s.EXTRA_EXP_AMOUNT,
                EXTRA_EXP_AMOUNT_DV = s.EXTRA_EXP_AMOUNT_DV,
                EXTRA_EXP_AMOUNT_BHYT = s.EXTRA_EXP_AMOUNT_BHYT,
                IMP_PRICE = s.IMP_PRICE,
                VAT_RATIO = s.VAT_RATIO,
                VAT_RATIO_STR = string.Format("'{0}%", (long)(100 * s.VAT_RATIO ?? 0)),
                MEDI_MATE_TYPE_ID = s.MEDI_MATE_TYPE_ID,
                SERVICE_CODE = s.SERVICE_CODE,
                SERVICE_NAME = s.SERVICE_NAME,
                SERVICE_UNIT_NAME = s.SERVICE_UNIT_NAME,
                MEDICINE_TYPE_PROPRIETARY_NAME = s.MEDICINE_TYPE_PROPRIETARY_NAME,
                NATIONAL_NAME = s.NATIONAL_NAME,
                MANUFACTURER_NAME = s.MANUFACTURER_NAME,
                HEIN_SERVICE_CODE = s.HEIN_SERVICE_CODE,
                HEIN_SERVICE_NAME = s.HEIN_SERVICE_NAME,
                PACKAGE_NUMBER = s.PACKAGE_NUMBER,
                EXPIRED_DATE_STR = s.EXPIRED_DATE_STR,
                SUPPLIER_CODE = s.SUPPLIER_CODE,
                SUPPLIER_NAME = s.SUPPLIER_NAME,
                IMP_DEPARTMENT_NAME = s.IMP_DEPARTMENT_NAME,
                MAIN_END_AMOUNT = s.MAIN_BEGIN_AMOUNT + s.MAIN_IMP_AMOUNT - s.MAIN_EXP_AMOUNT,
                EXTRA_END_AMOUNT = s.EXTRA_BEGIN_AMOUNT + s.EXTRA_IMP_AMOUNT - s.EXTRA_EXP_AMOUNT
            }).ToList();
            ListRdo.Clear();
            ListRdo.AddRange(x);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            dicSingleTag.Add("MEDI_STOCK_NAME", MediStock.MEDI_STOCK_NAME);

            objectTag.AddObjectData(store, "Services", ListRdo.Where(o => o.MAIN_BEGIN_AMOUNT != 0 || o.MAIN_END_AMOUNT != 0 || o.EXTRA_BEGIN_AMOUNT != 0 || o.EXTRA_END_AMOUNT != 0 || o.MAIN_IMP_AMOUNT != 0 || o.MAIN_EXP_AMOUNT != 0 || o.EXTRA_IMP_AMOUNT != 0 || o.EXTRA_EXP_AMOUNT != 0).OrderBy(p => p.TYPE).ThenBy(p => p.SERVICE_NAME).ToList());

        }

    }

}
