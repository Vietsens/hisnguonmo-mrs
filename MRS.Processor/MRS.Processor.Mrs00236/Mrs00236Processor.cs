using MOS.MANAGER.HisMedicine;
using AutoMapper;
using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMediStockPeriod;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisMaterialType;

namespace MRS.Processor.Mrs00236
{
    public class Mrs00236Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<Mrs00236RDO> ListRdo = new List<Mrs00236RDO>();
        List<Mrs00236RDO> ListReportTypeCats = new List<Mrs00236RDO>();
        List<Mrs00236RDO> SumMedicineTypes = new List<Mrs00236RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> listHisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<HIS_MEDI_STOCK> ListMediStock = new List<HIS_MEDI_STOCK>();

        Dictionary<long, HIS_REPORT_TYPE_CAT> dicReportTypeCat = new Dictionary<long, HIS_REPORT_TYPE_CAT>();
        Dictionary<long, HIS_SERVICE_RETY_CAT> dicServiceRetyCat = new Dictionary<long, HIS_SERVICE_RETY_CAT>();
        //Dictionary<long, V_HIS_MANU_IMP_MEST> dicManuImpMest = new Dictionary<long, V_HIS_MANU_IMP_MEST>(); 
        Dictionary<long, HIS_IMP_MEST> dicManuImpMest = new Dictionary<long, HIS_IMP_MEST>();

        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        int THUOC = 1;
        int VAT_TU = 2;

        public Mrs00236Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00236Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                //get dữ liệu:
                HisReportTypeCatFilterQuery filterReportTypeCat = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = this.ReportTypeCode
                };
                var listReportTypeCat = new HisReportTypeCatManager(paramGet).Get(filterReportTypeCat);
                foreach (var o in listReportTypeCat) if (!dicReportTypeCat.ContainsKey(o.ID)) dicReportTypeCat[o.ID] = o;
                var skip = 0;
                List<HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>();

                while (dicReportTypeCat.Keys.Count - skip > 0)
                {
                    var listIds = dicReportTypeCat.Keys.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceRetyCatFilterQuery filterServiceRetyCat = new HisServiceRetyCatFilterQuery()
                    {
                        REPORT_TYPE_CAT_IDs = listIds
                    };
                    var HisServiceRetyCats = new HisServiceRetyCatManager(paramGet).Get(filterServiceRetyCat);
                    listHisServiceRetyCat.AddRange(HisServiceRetyCats);
                }

                foreach (var o in listHisServiceRetyCat) if (!dicServiceRetyCat.ContainsKey(o.SERVICE_ID)) dicServiceRetyCat[o.SERVICE_ID] = o;

                HisMediStockFilterQuery stock = new HisMediStockFilterQuery();
                stock.IDs = ((Mrs00236Filter)this.reportFilter).MEDI_STOCK_IDs;
                ListMediStock = new HisMediStockManager(paramGet).Get(stock);
                //if (IsNotNullOrEmpty(((Mrs00236Filter)this.reportFilter).MEDI_STOCK_IDs))
                {
                    //List<V_HIS_MANU_IMP_MEST> listHisManuImpMest = new List<V_HIS_MANU_IMP_MEST>(); 
                    List<HIS_IMP_MEST> listHisManuImpMest = new List<HIS_IMP_MEST>();

                    HisImpMestFilterQuery metyFilterHisManuImpMest = new HisImpMestFilterQuery()
                    {
                        IMP_TIME_FROM = ((Mrs00236Filter)this.reportFilter).TIME_FROM,
                        IMP_TIME_TO = ((Mrs00236Filter)this.reportFilter).TIME_TO,
                        MEDI_STOCK_IDs = ((Mrs00236Filter)this.reportFilter).MEDI_STOCK_IDs,
                        IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC
                    };
                    listHisManuImpMest = new HisImpMestManager(paramGet).Get(metyFilterHisManuImpMest);
                    foreach (var o in listHisManuImpMest) if (!dicManuImpMest.ContainsKey(o.ID)) dicManuImpMest[o.ID] = o;

                    List<long> listHisImpMestId = listHisManuImpMest.Select(o => o.ID).ToList();
                    //Chi tiết thuốc
                    skip = 0;
                    while (listHisImpMestId.Count - skip > 0)
                    {
                        var listIds = listHisImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestMedicineViewFilterQuery medifilter = new HisImpMestMedicineViewFilterQuery()
                        {
                            IMP_MEST_IDs = listIds
                        };
                        var HisImpMestMedicine = new HisImpMestMedicineManager(paramGet).GetView(medifilter);
                        listHisImpMestMedicine.AddRange(HisImpMestMedicine);
                    }

                    //chi tiết vật tư
                    skip = 0;
                    while (listHisImpMestId.Count - skip > 0)
                    {
                        var listIds = listHisImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisImpMestMaterialViewFilterQuery matefilter = new HisImpMestMaterialViewFilterQuery()
                        {
                            IMP_MEST_IDs = listIds
                        };
                        var HisImpMestMaterial = new HisImpMestMaterialManager(paramGet).GetView(matefilter);
                        listHisImpMestMaterial.AddRange(HisImpMestMaterial);
                    }
                }

                ListMedicineType = new HisMedicineTypeManager(paramGet).GetView(new HisMedicineTypeViewFilterQuery());
                ListMaterialType = new HisMaterialTypeManager(paramGet).GetView(new HisMaterialTypeViewFilterQuery());
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
                ListRdo.Clear();

                if (IsNotNullOrEmpty(listHisImpMestMedicine))
                {
                    foreach (var needRDO in listHisImpMestMedicine)
                    {
                        var reportTypeCatId = dicServiceRetyCat.ContainsKey(needRDO.SERVICE_ID) ? dicServiceRetyCat[needRDO.SERVICE_ID].REPORT_TYPE_CAT_ID : 0;
                        var catagoryName = dicReportTypeCat.ContainsKey(reportTypeCatId) ? dicReportTypeCat[reportTypeCatId].CATEGORY_NAME : "";
                        var documentDate = dicManuImpMest.ContainsKey(needRDO.IMP_MEST_ID) ? dicManuImpMest[needRDO.IMP_MEST_ID].DOCUMENT_DATE : 0;
                        var documentNumber = dicManuImpMest.ContainsKey(needRDO.IMP_MEST_ID) ? dicManuImpMest[needRDO.IMP_MEST_ID].DOCUMENT_NUMBER : "";
                        var type = ListMedicineType.FirstOrDefault(o => o.ID == needRDO.MEDICINE_TYPE_ID);
                        var parent = ListMedicineType.FirstOrDefault(o => o.ID == (type != null ? type.PARENT_ID : 0));

                        Mrs00236RDO rdo = new Mrs00236RDO()
                        {
                            REPORT_TYPE_CAT_ID = reportTypeCatId,
                            CATEGORY_NAME = catagoryName,
                            MEDICINE_TYPE_ID = needRDO.MEDICINE_TYPE_ID,
                            MEDICINE_TYPE_NAME = needRDO.MEDICINE_TYPE_NAME,
                            MEDICINE_TYPE_CODE = needRDO.MEDICINE_TYPE_CODE,
                            UNIT = needRDO.SERVICE_UNIT_NAME,//
                            IMP_MEST_CODE = needRDO.IMP_MEST_CODE,
                            MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == needRDO.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME,
                            IMP_PRICE = needRDO.IMP_PRICE,
                            IMP_VAT_RATIO = needRDO.IMP_VAT_RATIO,
                            TOTAL_IMP_VAT = needRDO.IMP_VAT_RATIO * needRDO.AMOUNT,
                            AMOUNT = needRDO.AMOUNT,
                            TOTAL_PRICE = needRDO.IMP_PRICE * needRDO.AMOUNT,
                            IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(needRDO.IMP_TIME ?? 0),
                            SUPPLIER_CODE = needRDO.SUPPLIER_CODE,
                            SUPPLIER_NAME = needRDO.SUPPLIER_NAME,
                            MANUFACTURER_NAME = needRDO.MANUFACTURER_NAME,
                            NATIONAL_NAME = needRDO.NATIONAL_NAME,
                            BID_NUMBER = needRDO.BID_NUMBER,
                            DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(documentDate ?? 0),
                            DOCUMENT_NUMBER = documentNumber,
                            TYPE = THUOC,
                            MEDI_MATE_PARENT_CODE = parent != null ? parent.MEDICINE_TYPE_CODE : "",
                            MEDI_MATE_PARENT_NAME = parent != null ? parent.MEDICINE_TYPE_NAME : ""
                        };

                        rdo.V_HIS_IMP_MEST_MEDICINE = needRDO;
                        rdo.V_HIS_IMP_MEST_MATERIAL = new V_HIS_IMP_MEST_MATERIAL();
                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listHisImpMestMaterial))
                {
                    foreach (var needRDO in listHisImpMestMaterial)
                    {
                        var reportTypeCatId = dicServiceRetyCat.ContainsKey(needRDO.SERVICE_ID) ? dicServiceRetyCat[needRDO.SERVICE_ID].REPORT_TYPE_CAT_ID : 0;
                        var catagoryName = dicReportTypeCat.ContainsKey(reportTypeCatId) ? dicReportTypeCat[reportTypeCatId].CATEGORY_NAME : "";
                        var documentDate = dicManuImpMest.ContainsKey(needRDO.IMP_MEST_ID) ? dicManuImpMest[needRDO.IMP_MEST_ID].DOCUMENT_DATE : 0;
                        var documentNumber = dicManuImpMest.ContainsKey(needRDO.IMP_MEST_ID) ? dicManuImpMest[needRDO.IMP_MEST_ID].DOCUMENT_NUMBER : "";
                        var type = ListMaterialType.FirstOrDefault(o => o.ID == needRDO.MATERIAL_TYPE_ID);
                        var parent = ListMaterialType.FirstOrDefault(o => o.ID == (type != null ? type.PARENT_ID : 0));

                        Mrs00236RDO rdo = new Mrs00236RDO()
                        {
                            REPORT_TYPE_CAT_ID = reportTypeCatId,
                            CATEGORY_NAME = catagoryName,
                            MEDICINE_TYPE_ID = needRDO.MATERIAL_TYPE_ID,
                            MEDICINE_TYPE_NAME = needRDO.MATERIAL_TYPE_NAME,
                            MEDICINE_TYPE_CODE = needRDO.MATERIAL_TYPE_CODE,
                            UNIT = needRDO.SERVICE_UNIT_NAME,//
                            IMP_MEST_CODE = needRDO.IMP_MEST_CODE,
                            MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == needRDO.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME,
                            IMP_PRICE = needRDO.IMP_PRICE,
                            IMP_VAT_RATIO = needRDO.IMP_VAT_RATIO,
                            TOTAL_IMP_VAT = needRDO.IMP_VAT_RATIO * needRDO.AMOUNT,
                            AMOUNT = needRDO.AMOUNT,
                            TOTAL_PRICE = needRDO.IMP_PRICE * needRDO.AMOUNT,
                            IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(needRDO.IMP_TIME ?? 0),
                            SUPPLIER_CODE = needRDO.SUPPLIER_CODE,
                            SUPPLIER_NAME = needRDO.SUPPLIER_NAME,
                            MANUFACTURER_NAME = needRDO.MANUFACTURER_NAME,
                            NATIONAL_NAME = needRDO.NATIONAL_NAME,
                            BID_NUMBER = needRDO.BID_NUMBER,
                            DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(documentDate ?? 0),
                            DOCUMENT_NUMBER = documentNumber,
                            TYPE = VAT_TU,
                            MEDI_MATE_PARENT_CODE = parent != null ? parent.MATERIAL_TYPE_CODE : "",
                            MEDI_MATE_PARENT_NAME = parent != null ? parent.MATERIAL_TYPE_NAME : ""
                        };

                        rdo.V_HIS_IMP_MEST_MATERIAL = needRDO;
                        rdo.V_HIS_IMP_MEST_MEDICINE = new V_HIS_IMP_MEST_MEDICINE();
                        ListRdo.Add(rdo);
                    }
                }

                SumMedicineTypes = ListRdo.GroupBy(o => new { o.REPORT_TYPE_CAT_ID, o.MEDICINE_TYPE_ID }).Select(p => p.First()).ToList();
                ListReportTypeCats = ListRdo.GroupBy(o => o.REPORT_TYPE_CAT_ID).Select(p => p.First()).ToList();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00236Filter)this.reportFilter).TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00236Filter)this.reportFilter).TIME_TO));
            if (IsNotNullOrEmpty(((Mrs00236Filter)this.reportFilter).MEDI_STOCK_IDs))
            {
                string Stocks = "";
                for (int i = 0; i < ListMediStock.Count; i++) Stocks = Stocks + ", " + ListMediStock[i].MEDI_STOCK_NAME;
                dicSingleTag.Add("ListMediStock", Stocks);
            }

            objectTag.AddObjectData(store, "ImpMestMedicine", ListRdo);
            objectTag.AddObjectData(store, "SumMedicineType", SumMedicineTypes);
            objectTag.AddObjectData(store, "ReportTypeCat", ListReportTypeCats);
            objectTag.AddRelationship(store, "ReportTypeCat", "SumMedicineType", "REPORT_TYPE_CAT_ID", "REPORT_TYPE_CAT_ID");
            string[] ship = { "REPORT_TYPE_CAT_ID", "MEDICINE_TYPE_ID" };
            objectTag.AddRelationship(store, "SumMedicineType", "ImpMestMedicine", ship, ship);

            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());

            var groupRdo = ListRdo.GroupBy(g => new { g.TYPE, g.MEDI_MATE_PARENT_CODE, g.MEDICINE_TYPE_ID, g.IMP_PRICE, g.SUPPLIER_CODE }).Select(s =>
                new Mrs00236RDO()
                        {
                            REPORT_TYPE_CAT_ID = s.First().REPORT_TYPE_CAT_ID,
                            CATEGORY_NAME = s.First().CATEGORY_NAME,
                            MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID,
                            MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME,
                            MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE,
                            UNIT = s.First().UNIT,
                            IMP_MEST_CODE = s.First().IMP_MEST_CODE,
                            MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                            IMP_PRICE = s.First().IMP_PRICE,
                            IMP_VAT_RATIO = s.First().IMP_VAT_RATIO,
                            TOTAL_IMP_VAT = s.Sum(o => o.IMP_VAT_RATIO),
                            AMOUNT = s.Sum(o => o.AMOUNT),
                            TOTAL_PRICE = s.Sum(o => o.TOTAL_PRICE),
                            IMP_TIME = s.First().IMP_TIME,
                            SUPPLIER_CODE = s.First().SUPPLIER_CODE,
                            SUPPLIER_NAME = s.First().SUPPLIER_NAME,
                            MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                            NATIONAL_NAME = s.First().NATIONAL_NAME,
                            BID_NUMBER = s.First().BID_NUMBER,
                            DOCUMENT_DATE = s.First().DOCUMENT_DATE,
                            DOCUMENT_NUMBER = s.First().DOCUMENT_NUMBER,
                            TYPE = s.First().TYPE,
                            MEDI_MATE_PARENT_CODE = s.First().MEDI_MATE_PARENT_CODE,
                            MEDI_MATE_PARENT_NAME = s.First().MEDI_MATE_PARENT_NAME,
                            V_HIS_IMP_MEST_MATERIAL = s.First().V_HIS_IMP_MEST_MATERIAL,
                            V_HIS_IMP_MEST_MEDICINE = s.First().V_HIS_IMP_MEST_MEDICINE
                        }).ToList();

            var groupType = ListRdo.GroupBy(g => g.TYPE).Select(s => s.First()).ToList();
            var groupParent = ListRdo.GroupBy(g => new { g.TYPE, g.MEDI_MATE_PARENT_CODE }).Select(s => s.First()).ToList();

            objectTag.AddObjectData(store, "MediMateGroupRdo", groupRdo);
            objectTag.AddObjectData(store, "MediMateGroupType", groupType);
            objectTag.AddObjectData(store, "MediMateGroupParent", groupParent);
            objectTag.AddRelationship(store, "MediMateGroupType", "MediMateGroupRdo", "TYPE", "TYPE");
            objectTag.AddRelationship(store, "MediMateGroupType", "MediMateGroupParent", "TYPE", "TYPE");
            objectTag.AddRelationship(store, "MediMateGroupParent", "MediMateGroupRdo", "MEDI_MATE_PARENT_CODE", "MEDI_MATE_PARENT_CODE");
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
}