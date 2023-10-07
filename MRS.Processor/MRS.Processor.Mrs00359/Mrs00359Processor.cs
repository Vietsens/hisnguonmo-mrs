using MOS.MANAGER.HisService;
using MOS.MANAGER.HisImpMestType;
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
using MRS.Processor.Mrs00359; 
using Inventec.Common.FlexCellExport; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisReportTypeCat; 
using Inventec.Common.Logging; 

namespace MRS.Processor.Mrs00359
{
    public class Mrs00359Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00359RDO> ListRdo = new List<Mrs00359RDO>(); 
        List<Mrs00359RDO> ListReportTypeCats = new List<Mrs00359RDO>(); 
        List<Mrs00359RDO> SumMaterialTypes = new List<Mrs00359RDO>(); 
        List<HIS_IMP_MEST_TYPE> listHisImpMestType = new List<HIS_IMP_MEST_TYPE>(); 
        List<V_HIS_IMP_MEST> listHisImpMest = new List<V_HIS_IMP_MEST>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<HIS_MEDI_STOCK> ListMediStock = new List<HIS_MEDI_STOCK>(); 

        List<HIS_SERVICE_RETY_CAT> listHisServiceRetyCat = new List<HIS_SERVICE_RETY_CAT>(); 
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>(); 
        public Mrs00359Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00359Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {

                //get report type cat
             HisReportTypeCatFilterQuery filterReportTypeCat = new HisReportTypeCatFilterQuery()
                {
                    REPORT_TYPE_CODE__EXACT = this.ReportTypeCode
                }; 
                listReportTypeCat = new HisReportTypeCatManager(paramGet).Get(filterReportTypeCat); 
                List<long> ReportTypeCatIds = listReportTypeCat.Select(o => o.ID).ToList(); 
                var skip = 0; 
                while (ReportTypeCatIds.Count - skip > 0)
                {
                    var listIds = ReportTypeCatIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisServiceRetyCatFilterQuery filterServiceRetyCat = new HisServiceRetyCatFilterQuery()
                    {
                        REPORT_TYPE_CAT_IDs = listIds
                    }; 
                    var HisServiceRetyCats = new HisServiceRetyCatManager(paramGet).Get(filterServiceRetyCat); 
                    listHisServiceRetyCat.AddRange(HisServiceRetyCats); 
                }
                 skip = 0; 
                HisMediStockFilterQuery stock = new HisMediStockFilterQuery(); 
                stock.IDs = ((Mrs00359Filter)this.reportFilter).MEDI_STOCK_IDs; 
                ListMediStock = new HisMediStockManager(paramGet).Get(stock); 
              
                if (IsNotNullOrEmpty(((Mrs00359Filter)this.reportFilter).MEDI_STOCK_IDs))
                {
                    //LỌC CÁC PHIẾU NHẬP
                    List<long> ads = new List<long>(); 
                    ads.Add(IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC); 
                    HisImpMestViewFilterQuery metyFilterHisImpMest = new HisImpMestViewFilterQuery()
                    {
                        IMP_TIME_FROM = ((Mrs00359Filter)this.reportFilter).TIME_FROM,
                        IMP_TIME_TO = ((Mrs00359Filter)this.reportFilter).TIME_TO,
                        MEDI_STOCK_IDs = ((Mrs00359Filter)this.reportFilter).MEDI_STOCK_IDs,
                        IMP_MEST_TYPE_IDs = ads
                    }; 
                    listHisImpMest = new HisImpMestManager(paramGet).GetView(metyFilterHisImpMest); 

                    List<long> listHisImpMestId = listHisImpMest.Select(o => o.ID).ToList(); 
                    HisImpMestTypeFilterQuery sft = new HisImpMestTypeFilterQuery(); 
                    listHisImpMestType = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager(paramGet).Get(sft); 
                  
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

                    result = true; 
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
                ListRdo.Clear(); 




                if (IsNotNullOrEmpty(listHisImpMestMaterial))
                {

                    foreach (var needRDO in listHisImpMestMaterial)
                    {

                        Mrs00359RDO rdo = new Mrs00359RDO()
                        {
                            REPORT_TYPE_CAT_ID = IsNotNullOrEmpty(listHisServiceRetyCat.Where(o => o.SERVICE_ID == needRDO.SERVICE_ID).Select(p => p.REPORT_TYPE_CAT_ID).ToList()) ? 
                            listHisServiceRetyCat.Where(o => o.SERVICE_ID == needRDO.SERVICE_ID).Select(p => p.REPORT_TYPE_CAT_ID).First() : 0,
                            CATEGORY_NAME = IsNotNullOrEmpty(listHisServiceRetyCat.Where(o => o.SERVICE_ID == needRDO.SERVICE_ID).Select(p => p.REPORT_TYPE_CAT_ID).ToList()) ? listReportTypeCat.Where(q => q.ID == listHisServiceRetyCat.Where(o => o.SERVICE_ID == needRDO.SERVICE_ID).Select(p => p.REPORT_TYPE_CAT_ID).First()).First().CATEGORY_NAME : "",
                            MATERIAL_TYPE_ID = needRDO.MATERIAL_TYPE_ID,
                            MATERIAL_TYPE_NAME = needRDO.MATERIAL_TYPE_NAME,
                            UNIT = needRDO.SERVICE_UNIT_NAME,
                            IMP_MEST_CODE = needRDO.IMP_MEST_CODE,
                            IMP_PRICE = needRDO.IMP_PRICE,
                            IMP_VAT_RATIO = needRDO.IMP_VAT_RATIO,
                            AMOUNT = needRDO.AMOUNT,
                            TOTAL_PRICE = needRDO.IMP_PRICE * needRDO.AMOUNT,

                            IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)needRDO.IMP_TIME)

                        }; 
                        ListRdo.Add(rdo); 
                    }
                }
                SumMaterialTypes = ListRdo.GroupBy(o => new { o.REPORT_TYPE_CAT_ID, o.MATERIAL_TYPE_ID }).Select(p => p.First()).ToList(); 
                ListReportTypeCats = ListRdo.GroupBy(o => o.REPORT_TYPE_CAT_ID).Select(p => p.First()).ToList(); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                ListRdo.Clear(); 

            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00359Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00359Filter)this.reportFilter).TIME_TO)); 
            if (IsNotNullOrEmpty(((Mrs00359Filter)this.reportFilter).MEDI_STOCK_IDs))
            {
                string Stocks = ""; 
                for (int i = 0;  i < ListMediStock.Count;  i++) Stocks = Stocks + ", " + ListMediStock[i].MEDI_STOCK_NAME; 
                dicSingleTag.Add("ListMediStock", Stocks); 
            }
            objectTag.AddObjectData(store, "ImpMestMaterial", ListRdo); 
            objectTag.AddObjectData(store, "SumMaterialType", SumMaterialTypes); 
            objectTag.AddObjectData(store, "ReportTypeCat", ListReportTypeCats); 
           objectTag.AddRelationship(store, "ReportTypeCat", "SumMaterialType", "REPORT_TYPE_CAT_ID", "REPORT_TYPE_CAT_ID"); 
           
           string[] ship = { "REPORT_TYPE_CAT_ID", "MATERIAL_TYPE_ID" }; 
     
            objectTag.AddRelationship(store, "SumMaterialType", "ImpMestMaterial", ship, ship); 
         

            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData()); 
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
