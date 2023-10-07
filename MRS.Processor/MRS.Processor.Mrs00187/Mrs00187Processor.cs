using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
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
using MRS.Processor.Mrs00187; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientType;

namespace MRS.Processor.Mrs00187
{
    public class Mrs00187Processor : AbstractProcessor
    {
        private List<Mrs00187RDO> listMrs00187Rdos = new List<Mrs00187RDO>();
        private List<Mrs00187RDO> listMrs00187Rdo = new List<Mrs00187RDO>(); 
        private Mrs00187Filter FilterMrs00187; 

        private List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        private List<V_HIS_EXP_MEST> listExpMest = new List<V_HIS_EXP_MEST>();
        private List<HIS_EXP_MEST_TYPE> listExpMestType = new List<HIS_EXP_MEST_TYPE>();
        private List<HIS_TRANSACTION> listBill = new List<HIS_TRANSACTION>();
        private List<HIS_IMP_MEST> listMobaImpMests = new List<HIS_IMP_MEST>(); 
        private List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        private List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        private List<HIS_PATIENT> listPatient;
        private List<HIS_PATIENT_TYPE> listPatientType;
        public Mrs00187Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            FilterMrs00187 = ((Mrs00187Filter)this.reportFilter); 
            return typeof(Mrs00187Filter); 
        }

        protected override bool GetData()
        {
            var result = true;
            FilterMrs00187 = ((Mrs00187Filter)this.reportFilter);
            var paramGet = new CommonParam(); 
            try
            {
                listExpMestType = new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery());
                var HisExpMestMedicineFilterQuery = new HisExpMestMedicineViewFilterQuery()
                {
                    EXP_TIME_FROM = ((Mrs00187Filter)this.reportFilter).EXP_TIME_FROM,
                    EXP_TIME_TO = ((Mrs00187Filter)this.reportFilter).EXP_TIME_TO,
                    MEDI_STOCK_IDs = ((Mrs00187Filter)this.reportFilter).MEDI_STOCK_IDs,
                    IS_EXPORT = true,
                    //EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE, // phieu da xuat
                    EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK, // xuat don thuoc
                    }
                };
                listExpMestMedicines = new HisExpMestMedicineManager(paramGet).GetView(HisExpMestMedicineFilterQuery);
               
                if (((Mrs00187Filter)this.reportFilter).IMP_MEDI_STOCK_IDs != null)
                {
                    listExpMestMedicines = listExpMestMedicines.Where(p => ((Mrs00187Filter)this.reportFilter).IMP_MEDI_STOCK_IDs.Contains(p.CK_IMP_MEST_MEDICINE_ID ?? 0)).ToList();
                }
                
              
                var ExpMestIds = listExpMestMedicines != null ? listExpMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).Distinct().ToList() : null;
                //--------------------------------------------------------------------------------------------------V_HIS_MOBA_IMP_MEST
                listMobaImpMests = new List<HIS_IMP_MEST>();
                if (ExpMestIds.Count > 0)
                {
                    var skip = 0;
                    while (ExpMestIds.Count() - skip > 0)
                    {
                        var ListDSs = ExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var MobaImpMestFilter = new HisImpMestFilterQuery()
                        {
                            MOBA_EXP_MEST_IDs = ListDSs,
                        };
                        var MobaImpMests = new HisImpMestManager(paramGet).Get(MobaImpMestFilter);
                        listMobaImpMests.AddRange(MobaImpMests);
                    }
                }
                //
               
                //--------------------------------------------------------------------------------------------------V_HIS__IMP_MEST_MEDICINE
                var MobaImpMestId = listMobaImpMests.Select(s => s.ID).Distinct().ToList();

                listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();

                if (MobaImpMestId.Count > 0)
                {
                    var skip = 0;
                    //HIS_IMP_MEST_MEDICINE
                    while (MobaImpMestId.Count() - skip > 0)
                    {
                        var ListDSs = MobaImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var ImpMestMedicineFilter = new HisImpMestMedicineViewFilterQuery()
                        {
                            IMP_MEST_IDs = ListDSs,

                        };
                        var ImpMestMedicines = new HisImpMestMedicineManager(paramGet).GetView(ImpMestMedicineFilter);
                        listImpMestMedicines.AddRange(ImpMestMedicines);
                    }
                }

                //dữ liệu phiếu xuất
                if (ExpMestIds.Count > 0)
                {
                    var skip = 0;
                    while (ExpMestIds.Count() - skip > 0)
                    {
                        var ListDSs = ExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
                        if (ListDSs.Count > 0)
                        {
                            expFilter.IDs = ListDSs;
                        }
                        //if (((Mrs00187Filter)this.reportFilter).EXP_MEST_TYPE_IDs != null)
                        //{
                        //    expFilter.EXP_MEST_TYPE_IDs = ((Mrs00187Filter)this.reportFilter).EXP_MEST_TYPE_IDs;
                        //}
                        var listExpMestSub = new HisExpMestManager().GetView(expFilter);
                        listExpMest.AddRange(listExpMestSub);
                    }
                }
               
                if (((Mrs00187Filter)this.reportFilter).SS_PATIENT_TYPE_IDs != null)
                {
                    listExpMest = listExpMest.Where(p => ((Mrs00187Filter)this.reportFilter).SS_PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                if (((Mrs00187Filter)this.reportFilter).REQ_LOGINNAMEs != null)
                {
                    listExpMest = listExpMest.Where(p => ((Mrs00187Filter)this.reportFilter).REQ_LOGINNAMEs.Contains(p.REQ_LOGINNAME)).ToList();
                }
                if (((Mrs00187Filter)this.reportFilter).PATIENT_NAME != null)
                {
                    listExpMest = listExpMest.Where(p => p.TDL_PATIENT_NAME.Contains(((Mrs00187Filter)this.reportFilter).PATIENT_NAME)).ToList();
                }
                HisTransactionFilterQuery transFilter = new HisTransactionFilterQuery();
                //if (listExpMest != null)
                //dữ liệu giao dịch
                var billIds = listExpMest.Select(p => p.BILL_ID ?? 0).Distinct().ToList();
                if (billIds.Count > 0)
                {
                    var skip = 0;
                    while (billIds.Count() - skip > 0)
                    {
                        var ListDSs = billIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        //HisTransactionFilterQuery transFilter = new HisTransactionFilterQuery();
                        if (ListDSs != null)
                        {
                            transFilter.IDs = ListDSs;
                        }

                       var listBillSub = new HisTransactionManager().Get(transFilter);
                       listBill.AddRange(listBillSub);
                    }
                }
                HisPatientFilterQuery patientFilter = new HisPatientFilterQuery();
                listPatient = new HisPatientManager(paramGet).Get(patientFilter);
                //
                if (((Mrs00187Filter)this.reportFilter).PATIENT_CLASSIFY_IDs != null)
                {
                    var patientIDs = listPatient.Where(x => ((Mrs00187Filter)this.reportFilter).PATIENT_CLASSIFY_IDs.Contains(x.PATIENT_CLASSIFY_ID ?? 0)).Select(x => x.ID).ToList();
                    listExpMest = listExpMest.Where(x => patientIDs.Contains(x.TDL_PATIENT_ID ?? 0)).ToList();
                    listMobaImpMests = listMobaImpMests.Where(x => patientIDs.Contains(x.TDL_PATIENT_ID ?? 0)).ToList();
                }
                listBill = new HisTransactionManager().Get(transFilter);
                
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                if (listExpMestMedicines != null)
                
                listBill = new HisTransactionManager().Get(transFilter);
               
                //dữ liệu hồ sơ điều trị
                var treatmentIds = listExpMest.Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                if (treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count() - skip > 0)
                    {
                        var ListDSs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        //HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                        if (ListDSs != null)
                        {
                            treatmentFilter.IDs = ListDSs;
                        }

                       var listTreatmentSub = new HisTreatmentManager().Get(treatmentFilter);
                        listTreatment.AddRange(listTreatmentSub);
                    }
                }
                if (((Mrs00187Filter)this.reportFilter).PATIENT_TYPE_IDs != null)
                {
                    listTreatment = listTreatment.Where(p => ((Mrs00187Filter)this.reportFilter).PATIENT_TYPE_IDs.Contains(p.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
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
            bool result = true; 
            try
            {
                ProcessDetail();
                var STT = 0;
                var listeExpMestmedicine = listExpMestMedicines.GroupBy(s => s.MEDICINE_TYPE_ID).ToList();
                foreach (var ExpMestmedicine in listeExpMestmedicine)
                {

                    var expMestmedicineGroupByPrices = ExpMestmedicine.GroupBy(s => s.PRICE).ToList();
                    foreach (var expMestmedicineGroupByPrice in expMestmedicineGroupByPrices)
                    {
                        STT = STT + 1;
                        var listExpMestIds = ExpMestmedicine.Select(s => s.EXP_MEST_ID).ToList();
                        var mobaImpMests = listMobaImpMests.Where(s => listExpMestIds.Contains(s.MOBA_EXP_MEST_ID.Value)).Select(s => s.ID).ToList();
                        var recovery = listImpMestMedicines.Where(s => s.MEDICINE_TYPE_ID == expMestmedicineGroupByPrice.First().MEDICINE_TYPE_ID
                                    && mobaImpMests.Contains(s.IMP_MEST_ID)).Sum(s => s.AMOUNT);
                        var amount = expMestmedicineGroupByPrice.Sum(s => s.AMOUNT) - recovery;
                        var totalAmount = amount * expMestmedicineGroupByPrice.First().PRICE;

                        var rdo = new Mrs00187RDO
                        {
                            STT = STT,
                            MEDICINE_TYPE_NAME = expMestmedicineGroupByPrice.First().MEDICINE_TYPE_NAME,
                            SERVICE_UNIT_NAME = expMestmedicineGroupByPrice.First().SERVICE_UNIT_NAME,
                            PRICE = expMestmedicineGroupByPrice.First().PRICE,
                            AMOUNT = amount,
                            TT = totalAmount,
                        };
                        listMrs00187Rdos.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        private void ProcessDetail()
        {
            
            try
            {
                if (listExpMestMedicines != null)
                {
                    var listExpMestTreatment = listExpMestMedicines.GroupBy(s => string.Format("{0}_{1}", s.TDL_TREATMENT_ID, ((Mrs00187Filter)this.reportFilter).IS_AGGR == true ? s.TDL_AGGR_EXP_MEST_ID : s.ID)).ToList();
                    foreach (var item in listExpMestTreatment)
                    {
                        List<V_HIS_EXP_MEST_MEDICINE> listSub = item.ToList<V_HIS_EXP_MEST_MEDICINE>();
                        var expMestType = listExpMestType.FirstOrDefault(o => o.ID == listSub[0].EXP_MEST_TYPE_ID);
                        var expMest = listExpMest.FirstOrDefault(o => o.ID == listSub[0].EXP_MEST_ID);
                        var bill = expMest != null ? listBill.FirstOrDefault(o => o.ID == expMest.BILL_ID) : null;
                        var treatment = listSub[0].TDL_TREATMENT_ID != null ? listTreatment.FirstOrDefault(o => o.ID == listSub[0].TDL_TREATMENT_ID) : null;
                        Mrs00187RDO rdo = new Mrs00187RDO();
                        rdo.MEDI_STOCK_CODE = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == listSub[0].MEDI_STOCK_ID).MEDI_STOCK_CODE;
                        rdo.MEDI_STOCK_NAME = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == listSub[0].MEDI_STOCK_ID).MEDI_STOCK_NAME;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(listSub[0].EXP_TIME ?? 0);
                        rdo.REGISTER_NUMBER = listSub[0].REGISTER_NUMBER;
                        rdo.EXP_MEST_CODE = listSub[0].EXP_MEST_CODE;
                        rdo.EXP_MEST_TYPE_NAME = expMestType != null ? expMestType.EXP_MEST_TYPE_NAME : "";
                        if (treatment != null)
                        {
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.TREATMENT_CODE = treatment.TREATMENT_CODE;
                        }
                        if (bill != null)
                        {
                            rdo.TRANSACTION_CODE = bill.TRANSACTION_CODE;
                        }
                        if (expMest != null)
                        {
                            rdo.TDL_AGGR_EXP_MEST_CODE = expMest.TDL_AGGR_EXP_MEST_CODE;
                        }
                        rdo.TOTAL_PRICE = listSub.Sum(P => P.AMOUNT*(P.VIR_PRICE??0)*(1+(P.VAT_RATIO??0)));
                        listMrs00187Rdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                
                LogSystem.Error(ex);
            }
            
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("EXP_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00187Filter)this.reportFilter).EXP_TIME_FROM)); 
            dicSingleTag.Add("EXP_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00187Filter)this.reportFilter).EXP_TIME_TO));    
               
            //string MEDI_STOCK_NAME = "";
            //var listMediStocks = HisMediStockCFG.HisMediStocks;
            //if (FilterMrs00187.MEDI_STOCK_IDs != null)
            //{
            //    listMediStocks = listMediStocks != null ? listMediStocks.Where(o => FilterMrs00187.MEDI_STOCK_IDs.Contains( o.ID)).ToList() : new List<V_HIS_MEDI_STOCK>();
            //    foreach (var listMediStock in listMediStocks)
            //    {
            //        MEDI_STOCK_NAME = MEDI_STOCK_NAME + " - " + listMediStock.MEDI_STOCK_NAME;
            //    }
            //    dicSingleTag.Add("MEDI_STOCK", MEDI_STOCK_NAME); 
            //}
            objectTag.AddObjectData(store, "Report", listMrs00187Rdos);
            objectTag.AddObjectData(store, "Report1", listMrs00187Rdo.OrderBy(P => P.PATIENT_CODE).ToList());
            objectTag.AddObjectData(store, "Bill", listMrs00187Rdo.GroupBy(p => p.TRANSACTION_CODE).Select(Q => Q.First()).ToList());
            objectTag.AddRelationship(store, "Bill", "Report1", "TRANSACTION_CODE", "TRANSACTION_CODE");
        }
    }
}
