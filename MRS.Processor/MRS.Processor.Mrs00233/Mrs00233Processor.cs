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
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00233
{
    public class Mrs00233Processor : AbstractProcessor
    {
        private List<Mrs00233RDO> ListRdo = new List<Mrs00233RDO>(); 
        HIS_MEDI_STOCK medistock = new HIS_MEDI_STOCK(); 

        List<V_HIS_EXP_MEST_MATERIAL> ExpMestMaterialViews = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<V_HIS_IMP_MEST_MATERIAL> ImpMestMaterialViews = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<V_HIS_EXP_MEST_MEDICINE> ExpMestMedicineViews = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MEDICINE> ImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>(); 

        Dictionary<long, V_HIS_EXP_MEST> dicExpMest = new Dictionary<long, V_HIS_EXP_MEST>(); 
        Dictionary<long, V_HIS_IMP_MEST> dicImpMest = new Dictionary<long, V_HIS_IMP_MEST>(); 
        Decimal BEGIN_AMOUNT = 0; 
        string MAME_NAME = ""; 
        public Mrs00233Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00233Filter); 
        }

        protected override bool GetData()
        {

            var result = false; 
            try
            {
                var paramGet = new CommonParam(); 
                //get dữ liệu:
                //Kho

                    medistock = new HisMediStockManager(paramGet).GetById(((Mrs00233Filter)this.reportFilter).MEDI_STOCK_ID); 
                //Ky cua kho
                HisMediStockPeriodFilterQuery mediStockPeriodFilter = new HisMediStockPeriodFilterQuery(); 
                mediStockPeriodFilter.MEDI_STOCK_ID = medistock.ID; 
                mediStockPeriodFilter.CREATE_TIME_TO = ((Mrs00233Filter)this.reportFilter).TIME_FROM - 1; 
                var listMediStockPeriod = new HisMediStockPeriodManager().Get(mediStockPeriodFilter); 
                //Neu co chot ky
                if (IsNotNullOrEmpty(listMediStockPeriod))
                {
                    //So luong thuoc khi chot ki
                    HisMestPeriodMediFilterQuery mestPeriodMediFilter = new HisMestPeriodMediFilterQuery(); 
                    mestPeriodMediFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriod.OrderByDescending(o => o.CREATE_TIME).First().ID; 
                    var listMestPeriodMedi = new HisMestPeriodMediManager().Get(mestPeriodMediFilter); 

                    //So luong vat tu khi chot ki
                    HisMestPeriodMateFilterQuery mestPeriodMateFilter = new HisMestPeriodMateFilterQuery(); 
                    mestPeriodMateFilter.MEDI_STOCK_PERIOD_ID = listMediStockPeriod.OrderByDescending(o => o.CREATE_TIME).First().ID; 
                    var listMestPeriodMate = new HisMestPeriodMateManager().Get(mestPeriodMateFilter); 
                    //So luong tu khi chot ki den luc lay bao cao
                    GetImpExp(true, null, ((Mrs00233Filter)this.reportFilter).TIME_FROM - 1); 
                    //tinh ton dau
                    BEGIN_AMOUNT = listMestPeriodMedi.Sum(o => o.AMOUNT) + listMestPeriodMate.Sum(o => o.AMOUNT)
                        + ImpMestMedicineViews.Sum(o => o.AMOUNT) + ImpMestMedicineViews.Sum(o => o.AMOUNT)
                        - ExpMestMaterialViews.Sum(o => o.AMOUNT) - ExpMestMedicineViews.Sum(o => o.AMOUNT); 
                }
               //neu khong co chot ky
                else
                {
                    GetImpExp(false, null, ((Mrs00233Filter)this.reportFilter).TIME_FROM - 1); 
                    BEGIN_AMOUNT = ImpMestMedicineViews.Sum(o => o.AMOUNT) + ImpMestMedicineViews.Sum(o => o.AMOUNT)
                        - ExpMestMaterialViews.Sum(o => o.AMOUNT) - ExpMestMedicineViews.Sum(o => o.AMOUNT); 
                }
                //Nhap xuat trong ky
                GetImpExp(null, ((Mrs00233Filter)this.reportFilter).TIME_FROM, ((Mrs00233Filter)this.reportFilter).TIME_TO); 
                List<long> expMestIds = new List<long>();
                expMestIds.AddRange(ExpMestMaterialViews.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList());
                expMestIds.AddRange(ExpMestMedicineViews.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList()); 
                List<long> impMestIds = new List<long>(); 
                impMestIds.AddRange(ImpMestMaterialViews.Select(o => o.IMP_MEST_ID).Distinct().ToList()); 
                impMestIds.AddRange(ImpMestMedicineViews.Select(o => o.IMP_MEST_ID).Distinct().ToList()); 

                var skip = 0; 
                var listExpMest = new List<V_HIS_EXP_MEST>(); 
                while (expMestIds.Count - skip > 0)
                {
                    var listIds = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery(); 
                    expMestFilter.IDs = listIds; 
                    listExpMest.AddRange(new HisExpMestManager(param).GetView(expMestFilter)); 
                }
                dicExpMest = listExpMest.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 

                skip = 0; 
                var listImpMest = new List<V_HIS_IMP_MEST>(); 
                while (impMestIds.Count - skip > 0)
                {
                    var listIds = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisImpMestViewFilterQuery impMestFilter = new HisImpMestViewFilterQuery(); 
                    impMestFilter.IDs = listIds;
                    listImpMest.AddRange(new HisImpMestManager(param).GetView(impMestFilter)); 
                }
                dicImpMest = listImpMest.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 


                result = true; 
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 

                result = false; 
            }
            return result; 
        }

        void GetImpExp(bool? hasPeriod, long? timeFrom, long? timeTo)
        {
            CommonParam paramGet = new CommonParam(); 
            ExpMestMaterialViews = new List<V_HIS_EXP_MEST_MATERIAL>(); 
            ImpMestMaterialViews = new List<V_HIS_IMP_MEST_MATERIAL>(); 
            ExpMestMedicineViews = new List<V_HIS_EXP_MEST_MEDICINE>(); 
            ImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>(); 
            if (((Mrs00233Filter)this.reportFilter).MATERIAL_TYPE_ID != 0)
            {
                //Nhap xuat truoc timeFrom
                HisExpMestMaterialViewFilterQuery metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                {
                    EXP_TIME_FROM = timeFrom,
                    EXP_TIME_TO = timeTo,
                    HAS_MEDI_STOCK_PERIOD = hasPeriod,
                    MATERIAL_TYPE_ID = ((Mrs00233Filter)this.reportFilter).MATERIAL_TYPE_ID,
                    MEDI_STOCK_ID = ((Mrs00233Filter)this.reportFilter).MEDI_STOCK_ID,
                    EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    IS_EXPORT = true
                }; 
                ExpMestMaterialViews = new HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial); 

                HisImpMestMaterialViewFilterQuery metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                {
                    IMP_TIME_FROM = timeFrom,
                    IMP_TIME_TO = timeTo,
                    HAS_MEDI_STOCK_PERIOD = hasPeriod,
                    MATERIAL_TYPE_ID = ((Mrs00233Filter)this.reportFilter).MATERIAL_TYPE_ID,
                    MEDI_STOCK_ID = ((Mrs00233Filter)this.reportFilter).MEDI_STOCK_ID,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                }; 

                ImpMestMaterialViews = new HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial); 

                if (IsNotNullOrEmpty(ExpMestMaterialViews))
                    MAME_NAME = ExpMestMaterialViews.Where(o => o.MATERIAL_TYPE_ID == ((Mrs00233Filter)this.reportFilter).MATERIAL_TYPE_ID).First().MATERIAL_TYPE_NAME; 
            }

            if (((Mrs00233Filter)this.reportFilter).MEDICINE_TYPE_ID != 0)
            {

                HisExpMestMedicineViewFilterQuery metyFilterExpMestMedicine = new HisExpMestMedicineViewFilterQuery
                {
                    EXP_TIME_FROM = timeFrom,
                    EXP_TIME_TO = timeTo,
                    HAS_MEDI_STOCK_PERIOD = hasPeriod,
                    MEDICINE_TYPE_ID = ((Mrs00233Filter)this.reportFilter).MEDICINE_TYPE_ID,
                    MEDI_STOCK_ID = ((Mrs00233Filter)this.reportFilter).MEDI_STOCK_ID,
                    //EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE
                    IS_EXPORT = true
                }; 
                ExpMestMedicineViews = new HisExpMestMedicineManager(paramGet).GetView(metyFilterExpMestMedicine); 

                HisImpMestMedicineViewFilterQuery metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                {
                    IMP_TIME_FROM = timeFrom,
                    IMP_TIME_TO = timeTo,
                    HAS_MEDI_STOCK_PERIOD = hasPeriod,
                    MEDICINE_TYPE_ID = ((Mrs00233Filter)this.reportFilter).MEDICINE_TYPE_ID,
                    MEDI_STOCK_ID = ((Mrs00233Filter)this.reportFilter).MEDI_STOCK_ID,
                    IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT
                }; 
                ImpMestMedicineViews = new HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine); 

                if (IsNotNullOrEmpty(ExpMestMedicineViews))
                    MAME_NAME = ExpMestMedicineViews.Where(o => o.MEDICINE_TYPE_ID == ((Mrs00233Filter)this.reportFilter).MEDICINE_TYPE_ID).First().MEDICINE_TYPE_NAME; 
            }

        
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                ListRdo.Clear(); 
                V_HIS_IMP_MEST  ImpMest = null; 
                V_HIS_EXP_MEST ExpMest = null; 
                if (((Mrs00233Filter)this.reportFilter).MATERIAL_TYPE_ID != 0)
                {
                    foreach (var expMestMaterial in ExpMestMaterialViews)
                    {
                        if (!dicExpMest.ContainsKey(expMestMaterial.EXP_MEST_ID ?? 0)) continue;
                        ExpMest = dicExpMest[expMestMaterial.EXP_MEST_ID ?? 0]; 
                        Mrs00233RDO rdo = new Mrs00233RDO
                        {
                            TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)expMestMaterial.EXP_TIME),
                            MEST_CODE = expMestMaterial.EXP_MEST_CODE,
                            MEST_TYPE_NAME = ExpMest.EXP_MEST_TYPE_NAME,
                            PRICE = expMestMaterial.IMP_PRICE,
                            EXP_AMOUNT = expMestMaterial.AMOUNT,
                            EXP_TOTAL_PRICE = expMestMaterial.IMP_PRICE * expMestMaterial.AMOUNT
                        }; 

                        ListRdo.Add(rdo); 
                    }

                    foreach (var impMestMaterial in ImpMestMaterialViews)
                    {
                        if (!dicImpMest.ContainsKey(impMestMaterial.IMP_MEST_ID)) continue; 
                        ImpMest = dicImpMest[impMestMaterial.IMP_MEST_ID]; 
                        Mrs00233RDO rdo = new Mrs00233RDO
                        {
                            TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)impMestMaterial.IMP_TIME),
                            MEST_CODE = impMestMaterial.IMP_MEST_CODE,
                            MEST_TYPE_NAME = ImpMest.IMP_MEST_TYPE_NAME,
                            PRICE = impMestMaterial.IMP_PRICE,
                            IMP_AMOUNT = impMestMaterial.AMOUNT,
                            IMP_TOTAL_PRICE = impMestMaterial.IMP_PRICE * impMestMaterial.AMOUNT
                        }; 

                        ListRdo.Add(rdo); 
                    }
                }

                if (((Mrs00233Filter)this.reportFilter).MEDICINE_TYPE_ID != 0)
                {
                    foreach (var expMestMedicine in ExpMestMedicineViews)
                    {
                        if (!dicExpMest.ContainsKey(expMestMedicine.EXP_MEST_ID ?? 0)) continue;
                        ExpMest = dicExpMest[expMestMedicine.EXP_MEST_ID ?? 0]; 
                        Mrs00233RDO rdo = new Mrs00233RDO
                        {
                            TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)expMestMedicine.EXP_TIME),
                            MEST_CODE = expMestMedicine.EXP_MEST_CODE,
                            MEST_TYPE_NAME = ExpMest.EXP_MEST_TYPE_NAME,
                            PRICE = expMestMedicine.IMP_PRICE,
                            EXP_AMOUNT = expMestMedicine.AMOUNT,
                            EXP_TOTAL_PRICE = expMestMedicine.IMP_PRICE * expMestMedicine.AMOUNT
                        }; 

                        ListRdo.Add(rdo); 
                    }

                    foreach (var impMestMedicine in ImpMestMedicineViews)
                    {
                        if (!dicImpMest.ContainsKey(impMestMedicine.IMP_MEST_ID)) continue; 
                        ImpMest = dicImpMest[impMestMedicine.IMP_MEST_ID]; 
                        Mrs00233RDO rdo = new Mrs00233RDO
                        {
                            TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)impMestMedicine.IMP_TIME),
                            MEST_CODE = impMestMedicine.IMP_MEST_CODE,
                            MEST_TYPE_NAME = ImpMest.IMP_MEST_TYPE_NAME,
                            PRICE = impMestMedicine.IMP_PRICE,
                            IMP_AMOUNT = impMestMedicine.AMOUNT,
                            IMP_TOTAL_PRICE = impMestMedicine.IMP_PRICE * impMestMedicine.AMOUNT
                        }; 

                        ListRdo.Add(rdo); 
                    }
                }
                var groupByMestCode = ListRdo.GroupBy(o => new {o.MEST_CODE,o.TIME}).ToList(); 
                ListRdo.Clear(); 
                foreach (var item in groupByMestCode)
                {
                    Mrs00233RDO rdo = new Mrs00233RDO(); 
                    rdo = item.First(); 
                    rdo.IMP_AMOUNT = item.Sum(o => o.IMP_AMOUNT); 
                    rdo.EXP_AMOUNT = item.Sum(o => o.EXP_AMOUNT); 
                    rdo.IMP_TOTAL_PRICE = item.Sum(o => o.IMP_TOTAL_PRICE); 
                    rdo.EXP_TOTAL_PRICE = item.Sum(o => o.EXP_TOTAL_PRICE); 
                    ListRdo.Add(rdo); 
                }

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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00233Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00233Filter)this.reportFilter).TIME_TO)); 
            dicSingleTag.Add("MEDI_STOCK_NAME", medistock.MEDI_STOCK_NAME); 

            dicSingleTag.Add("MAME_NAME", MAME_NAME); 
            dicSingleTag.Add("BEGIN_AMOUNT", BEGIN_AMOUNT); 

            objectTag.AddObjectData(store, "Mest", ListRdo); 

        }
    }
}
