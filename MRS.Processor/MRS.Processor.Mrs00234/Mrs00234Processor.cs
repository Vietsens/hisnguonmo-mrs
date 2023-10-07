using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisImpMestType;
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
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisImpMestMaterial; 
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

namespace MRS.Processor.Mrs00234
{
    public class Mrs00234Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00234RDO> ListRdo = new List<Mrs00234RDO>(); 
        List<V_HIS_IMP_MEST> listHisMobaImpMest = new List<V_HIS_IMP_MEST>(); 
        List<V_HIS_EXP_MEST> listHisExpMest = new List<V_HIS_EXP_MEST>(); 
        List<HIS_IMP_MEST_TYPE> listHisImpMestType = new List<HIS_IMP_MEST_TYPE>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listHisImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<Mrs00234RDO> ListExpMestType = new List<Mrs00234RDO>(); 
        List<HIS_MEDI_STOCK> ListMediStock = new List<HIS_MEDI_STOCK>(); 
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>(); 
        List<Mrs00234RDO> ListSumDepartment = new List<Mrs00234RDO>(); 
        public Mrs00234Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00234Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                //get dữ liệu:
                HisMediStockFilterQuery stock = new HisMediStockFilterQuery(); 
                stock.IDs = ((Mrs00234Filter)this.reportFilter).MEDI_STOCK_IDs; 
                ListMediStock = new HisMediStockManager(paramGet).Get(stock); 
                HisDepartmentFilterQuery Departmentfilter = new HisDepartmentFilterQuery(); 
                ListDepartment = new HisDepartmentManager(paramGet).Get(Departmentfilter); 
                if (IsNotNullOrEmpty(((Mrs00234Filter)this.reportFilter).MEDI_STOCK_IDs))
                {
                    HisImpMestViewFilterQuery metyFilterHisMobaImpMest = new HisImpMestViewFilterQuery()
                    {
                        IMP_TIME_FROM = ((Mrs00234Filter)this.reportFilter).TIME_FROM,
                        IMP_TIME_TO = ((Mrs00234Filter)this.reportFilter).TIME_TO,
                        MEDI_STOCK_IDs = ((Mrs00234Filter)this.reportFilter).MEDI_STOCK_IDs,
                        IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH
                    }; 
                    var HisMobaImpMest = new HisImpMestManager(paramGet).GetView(metyFilterHisMobaImpMest); 
                    listHisMobaImpMest.AddRange(HisMobaImpMest); 

                    List<long> listHisImpMestId = listHisMobaImpMest.Select(o => o.ID).ToList(); 
                    var metyFilterHisExpMest = new HisExpMestViewFilterQuery
                    {
                        IDs = listHisMobaImpMest.Select(o => o.MOBA_EXP_MEST_ID ?? 0).ToList()
                    }; 
                    listHisExpMest = new HisExpMestManager(paramGet).GetView(metyFilterHisExpMest); 
                    HisImpMestTypeFilterQuery sft = new HisImpMestTypeFilterQuery(); 
                    listHisImpMestType = new MOS.MANAGER.HisImpMestType.HisImpMestTypeManager(paramGet).Get(sft); 
                    //Sửa vòng while để lấy api tốt hơn
                    var skip = 0; 
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
                    //Sửa vòng while để lấy api tốt hơn
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
                if (IsNotNullOrEmpty(listHisMobaImpMest))
                {
                    foreach (var needRDO in listHisMobaImpMest)
                    {
                        decimal sum = 0; 
                        var medis = listHisImpMestMedicine.Where(o => o.IMP_MEST_ID == needRDO.ID).ToList(); 
                        var mates = listHisImpMestMaterial.Where(o => o.IMP_MEST_ID == needRDO.ID).ToList(); 
                        if (mates != null)
                            foreach (var imate in mates)
                            {
                                sum += imate.IMP_PRICE * imate.AMOUNT; 
                            }
                        if (medis != null)
                            foreach (var imedi in medis)
                            {
                                sum += imedi.IMP_PRICE * imedi.AMOUNT; 
                            }
                        Dictionary<long, V_HIS_EXP_MEST> dicExpMest = new Dictionary<long, V_HIS_EXP_MEST>(); 
                        foreach (var item in listHisExpMest)
                        {
                            if (!dicExpMest.ContainsKey(item.ID))
                                dicExpMest.Add(item.ID, item); 
                        }

                        Mrs00234RDO rdo = new Mrs00234RDO()
                        {
                            REQ_DEPARTMENT_ID = needRDO.REQ_DEPARTMENT_ID,
                            REQ_DEPARTMENT_NAME = needRDO.REQ_DEPARTMENT_ID != null ? ListDepartment.Where(o => o.ID == needRDO.REQ_DEPARTMENT_ID).First().DEPARTMENT_NAME : "",//
                            IMP_MEST_ID = needRDO.ID,
                            IMP_MEST_CODE = needRDO.IMP_MEST_CODE,
                            TOTAL_PRICE = sum,
                            EXP_MEST_TYPE_ID = dicExpMest.ContainsKey(needRDO.MOBA_EXP_MEST_ID ?? 0) ? dicExpMest[needRDO.MOBA_EXP_MEST_ID ?? 0].EXP_MEST_TYPE_ID : 0,
                            //listHisExpMest.Where(o => o.ID == needRDO.MOBA_EXP_MEST_ID).FirstOrDefault().EXP_MEST_TYPE_ID,
                            EXP_MEST_TYPE_NAME = dicExpMest.ContainsKey(needRDO.MOBA_EXP_MEST_ID ?? 0) ? dicExpMest[needRDO.MOBA_EXP_MEST_ID ?? 0].EXP_MEST_TYPE_NAME : "",
                            IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)needRDO.IMP_TIME)
                        }; 
                        ListRdo.Add(rdo); 
                    }
                }

                ListSumDepartment = ListRdo.GroupBy(o => new { o.EXP_MEST_TYPE_ID, o.REQ_DEPARTMENT_ID }).Select(p => p.First()).ToList(); 
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00234Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00234Filter)this.reportFilter).TIME_TO)); 
            if (IsNotNullOrEmpty(((Mrs00234Filter)this.reportFilter).MEDI_STOCK_IDs))
            {
                string Stocks = ""; 
                for (int i = 0;  i < ListMediStock.Count;  i++) Stocks = Stocks + ", " + ListMediStock[i].MEDI_STOCK_NAME; 
                dicSingleTag.Add("ListMediStock", Stocks); 
            }

            objectTag.AddObjectData(store, "ExpMestType", ListExpMestType); 
            objectTag.AddObjectData(store, "Department", ListSumDepartment); 
            objectTag.AddObjectData(store, "MobaImpMest", ListRdo); 
            objectTag.AddRelationship(store, "ExpMestType", "Department", "EXP_MEST_TYPE_ID", "EXP_MEST_TYPE_ID"); 
            string[] ship = { "REQ_DEPARTMENT_ID", "EXP_MEST_TYPE_ID" }; 
            objectTag.AddRelationship(store, "Department", "MobaImpMest", ship, ship); 
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