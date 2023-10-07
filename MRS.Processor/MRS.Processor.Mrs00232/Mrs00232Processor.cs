using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMestType;
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
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisIcd; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientType; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00232
{
    public class Mrs00232Processor : AbstractProcessor
    {
        private List<Mrs00232RDO> listMrs00232RDOs = new List<Mrs00232RDO>(); 
        CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
        List<Mrs00232RDO> ListRdo = new List<Mrs00232RDO>(); 
        HIS_MEDI_STOCK medistock = new HIS_MEDI_STOCK(); 
        List<HIS_EXP_MEST_TYPE> listHisExpMestType = new List<HIS_EXP_MEST_TYPE>(); 
        List<V_HIS_EXP_MEST_MEDICINE> listHisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>(); 
        List<V_HIS_EXP_MEST_MATERIAL> listHisExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>(); 
        List<Mrs00232RDO> ListExpMestType = new List<Mrs00232RDO>(); 
        List<Mrs00232RDO> ListSumDepartment = new List<Mrs00232RDO>(); 
        Dictionary<long, V_HIS_EXP_MEST> dicExpMest = new Dictionary<long, V_HIS_EXP_MEST>(); 
        public Mrs00232Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00232Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                //get dữ liệu:
                HisMediStockFilterQuery stock = new HisMediStockFilterQuery(); 
                stock.ID = ((Mrs00232Filter)this.reportFilter).MEDI_STOCK_ID; 
                medistock = new HisMediStockManager(paramGet).Get(stock).First(); 





                HisExpMestTypeFilterQuery sft = new HisExpMestTypeFilterQuery(); 
                listHisExpMestType = new MOS.MANAGER.HisExpMestType.HisExpMestTypeManager(paramGet).Get(sft); 
                List<long> expMestIds = new List<long>(); 
                HisExpMestMedicineViewFilterQuery medifilter = new HisExpMestMedicineViewFilterQuery()
                {
                    EXP_TIME_FROM = ((Mrs00232Filter)this.reportFilter).TIME_FROM,
                    EXP_TIME_TO = ((Mrs00232Filter)this.reportFilter).TIME_TO,
                    TDL_MEDI_STOCK_ID = ((Mrs00232Filter)this.reportFilter).MEDI_STOCK_ID,
                    IS_EXPORT = true
                }; 
                listHisExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(medifilter);
                expMestIds.AddRange(listHisExpMestMedicine.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList() ?? new List<long>()); 
                HisExpMestMaterialViewFilterQuery matefilter = new HisExpMestMaterialViewFilterQuery()
                {
                    EXP_TIME_FROM = ((Mrs00232Filter)this.reportFilter).TIME_FROM,
                    EXP_TIME_TO = ((Mrs00232Filter)this.reportFilter).TIME_TO,
                    TDL_MEDI_STOCK_ID = ((Mrs00232Filter)this.reportFilter).MEDI_STOCK_ID,
                    IS_EXPORT = true
                }; 
                listHisExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(matefilter);
                expMestIds.AddRange(listHisExpMestMaterial.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList() ?? new List<long>()); 
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
                dicExpMest = listExpMest.GroupBy(o=>o.ID).ToDictionary(p=>p.Key,p=>p.First()); 

                result = true; 
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

                if (IsNotNullOrEmpty(listHisExpMestMedicine))
                {
                    V_HIS_EXP_MEST expMest = null; 
                    foreach (var item in listHisExpMestMedicine)
                    {
                        expMest = dicExpMest.ContainsKey(item.EXP_MEST_ID ?? 0) ? dicExpMest[item.EXP_MEST_ID ?? 0] : new V_HIS_EXP_MEST(); 
                        Mrs00232RDO rdo = new Mrs00232RDO()
                                           {
                                               DEPARTMENT_ID = item.REQ_DEPARTMENT_ID,
                                               DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME,
                                               EXP_MEST_ID = item.EXP_MEST_ID ?? 0,
                                               EXP_MEST_CODE = item.EXP_MEST_CODE,
                                               TOTAL_PRICE = item.AMOUNT*item.PRICE??0,
                                               EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID,
                                               EXP_MEST_TYPE_NAME = expMest.EXP_MEST_TYPE_NAME,
                                               EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)item.EXP_TIME),
                                               VIR_PATIENT_NAME =expMest.TDL_PATIENT_NAME,
                                               REQ_USERNAME = expMest.REQ_USERNAME,
                                           }; 
                        ListRdo.Add(rdo); 
                    }
                }

                if (IsNotNullOrEmpty(listHisExpMestMaterial))
                {
                    V_HIS_EXP_MEST expMest = null; 
                    foreach (var item in listHisExpMestMaterial)
                    {
                        expMest = dicExpMest.ContainsKey(item.EXP_MEST_ID ?? 0) ? dicExpMest[item.EXP_MEST_ID ?? 0] : new V_HIS_EXP_MEST(); 
                        Mrs00232RDO rdo = new Mrs00232RDO()
                        {
                            DEPARTMENT_ID = item.REQ_DEPARTMENT_ID,
                            DEPARTMENT_NAME = expMest.REQ_DEPARTMENT_NAME,
                            EXP_MEST_ID = item.EXP_MEST_ID ?? 0,
                            EXP_MEST_CODE = item.EXP_MEST_CODE,
                            TOTAL_PRICE = item.AMOUNT * item.PRICE ?? 0,
                            EXP_MEST_TYPE_ID = item.EXP_MEST_TYPE_ID,
                            EXP_MEST_TYPE_NAME = expMest.EXP_MEST_TYPE_NAME,
                            EXP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)item.EXP_TIME),
                            VIR_PATIENT_NAME = expMest.TDL_PATIENT_NAME,
                            REQ_USERNAME = expMest.REQ_USERNAME,
                        }; 
                        ListRdo.Add(rdo); 
                    }
                }
                var groupByExpMest = ListRdo.GroupBy(o => new { o.EXP_MEST_ID,o.EXP_TIME}).ToList(); 
                ListRdo.Clear(); 
                foreach (var item in groupByExpMest)
                {
                    Mrs00232RDO rdo = new Mrs00232RDO(); 
                    rdo = item.First(); 
                    rdo.TOTAL_PRICE = item.Sum(o => o.TOTAL_PRICE); 
                    ListRdo.Add(rdo); 
                }
                ListSumDepartment = ListRdo.GroupBy(o => new { o.EXP_MEST_TYPE_ID, o.DEPARTMENT_ID }).Select(p => p.First()).ToList(); 
                ListExpMestType = ListSumDepartment.GroupBy(o => o.EXP_MEST_TYPE_ID).Select(p => p.First()).ToList(); 
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00232Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00232Filter)this.reportFilter).TIME_TO)); 
            dicSingleTag.Add("MEDI_STOCK_NAME", medistock.MEDI_STOCK_NAME); 
            objectTag.AddObjectData(store, "ExpMestType", ListExpMestType); 
            objectTag.AddObjectData(store, "SumDepartment", ListSumDepartment); 
            objectTag.AddObjectData(store, "ExpMest", ListRdo); 
            objectTag.AddRelationship(store, "ExpMestType", "SumDepartment", "EXP_MEST_TYPE_ID", "EXP_MEST_TYPE_ID"); 
            string[] ship = { "DEPARTMENT_ID", "EXP_MEST_TYPE_ID" }; 
            objectTag.AddRelationship(store, "SumDepartment", "ExpMest", ship, ship); 
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
                    Inventec.Common.Logging.LogSystem.Error(ex); 
                }
                return result; 
            }
        }
    }
}