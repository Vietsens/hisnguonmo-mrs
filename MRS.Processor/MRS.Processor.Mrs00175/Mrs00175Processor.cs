using MOS.MANAGER.HisService;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using AutoMapper; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisDepartmentTran; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisMediStock; 
using MRS.MANAGER.Config; 
using MOS.MANAGER.HisExpMest; 

namespace MRS.Processor.Mrs00175
{
    public class Mrs00175Processor : AbstractProcessor
    {
        private List<Mrs00175RDO> listMrs00175Rdos = new List<Mrs00175RDO>(); 
        private Mrs00175Filter FilterMrs00175; 
        List<V_HIS_TREATMENT> listTreatment = new List<V_HIS_TREATMENT>(); 

        private List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials; 
        private List<V_HIS_EXP_MEST> listPresCriptions; 
        private List<V_HIS_SERVICE_REQ> listServiceReqs; 
        private List<V_HIS_SERE_SERV> listSereServs; 
        private List<V_HIS_DEPARTMENT_TRAN> listDepartmentTrans; 
        private List<V_HIS_TREATMENT> listTreatments; 
        private List<V_HIS_MEDI_STOCK> listMediStocks; 
        
        public Mrs00175Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            FilterMrs00175 = ((Mrs00175Filter)this.reportFilter); 
            return typeof(Mrs00175Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            var paramGet = new CommonParam(); 
            try
            {
                //--------------------------------------------------------------------------------------------------
                // EXP_MEST_MATERIAL
                var expMestMaterial = new HisExpMestMaterialViewFilterQuery()
                {
                    EXP_TIME_FROM = ((Mrs00175Filter)this.reportFilter).TIME_FROM,
                    EXP_TIME_TO = ((Mrs00175Filter)this.reportFilter).TIME_TO,
                    MEDI_STOCK_IDs = ((Mrs00175Filter)this.reportFilter).MEDI_STOCK_IDs,
                    IS_EXPORT = true
                }; 
                listExpMestMaterials = new HisExpMestMaterialManager(paramGet).GetView(expMestMaterial); 
                //--------------------------------------------------------------------------------------------------
                // PRESCRIPTION
                var listExpMestIds = listExpMestMaterials.Select(s => s.EXP_MEST_ID ?? 0).ToList(); 
                listPresCriptions = new List<V_HIS_EXP_MEST>(); 
                var skip = 0; 
                while (listExpMestIds.Count() - skip > 0)
                {
                    var ListDSs = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var metyFilterPrescription = new HisExpMestViewFilterQuery()
                    {
                        IDs = ListDSs,
                    }; 
                    var listPrescription = new HisExpMestManager(paramGet).GetView(metyFilterPrescription); 
                    listPresCriptions.AddRange(listPrescription); 
                }
                // SERVICE_REQ
                var listServiceReqIds = listPresCriptions.Select(s => s.SERVICE_REQ_ID??0).ToList(); 
                listServiceReqs = new List<V_HIS_SERVICE_REQ>(); 
                skip = 0; 
                while (listServiceReqIds.Count() - skip > 0)
                {
                    var ListDSs = listServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var metyFilterServiceReq = new HisServiceReqViewFilterQuery()
                    {
                        IDs = ListDSs,
                    }; 
                    var listServiceReq = new HisServiceReqManager(paramGet).GetView(metyFilterServiceReq); 
                    listServiceReqs.AddRange(listServiceReq); 
                }
                //--------------------------------------------------------------------------------------------------
                // SERE_SERV

                var ServiceReqIds = listServiceReqs.Select(s => s.ID).ToList(); 
                listSereServs = new List<V_HIS_SERE_SERV>(); 
                skip = 0; 
                while (ServiceReqIds.Count() - skip > 0)
                {
                    var ListDSs = ServiceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var sereServFilter = new HisSereServViewFilterQuery()
                    {
                        SERVICE_REQ_IDs = ListDSs,
                    }; 
                    var sereservs = new HisSereServManager(paramGet).GetView(sereServFilter); 
                    listSereServs.AddRange(sereservs); 
                }
                //--------------------------------------------------------------------------------------------------
                //DEPARTMENT_TRAN               
                var listTreatmentIds = listServiceReqs.Select(s => s.TREATMENT_ID).ToList().Distinct(); 
                listDepartmentTrans = new List<V_HIS_DEPARTMENT_TRAN>(); 
                skip = 0; 
                while (listTreatmentIds.Count() - skip > 0)
                {
                    var ListDSs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var departmentTrantFilter = new HisDepartmentTranViewFilterQuery()
                    {
                        TREATMENT_IDs = ListDSs,
                        DEPARTMENT_ID = ((Mrs00175Filter)this.reportFilter).DEPARTMENT_ID,
                    }; 
                    var departmentTrant = new HisDepartmentTranManager(paramGet).GetView(departmentTrantFilter); 
                    listDepartmentTrans.AddRange(departmentTrant);                     
                }

                //--------------------------------------------------------------------------------------------------
                //TREATMENT
                var departmentID = listDepartmentTrans.Select(s => s.TREATMENT_ID).ToList(); 
                listTreatments = new List<V_HIS_TREATMENT>(); 
                skip = 0; 
                while (departmentID.Count() - skip > 0)
                {
                    var ListDSs = departmentID.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var treatmentFilter = new HisTreatmentViewFilterQuery()
                    {
                        IDs = ListDSs,
                    }; 
                    var treatments = new HisTreatmentManager(paramGet).GetView(treatmentFilter); 
                    listTreatments.AddRange(treatments); 
                }

                //--------------------------------------------------------------------------------------------------
                //Medistock
                var mediStockIds = listExpMestMaterials.Select(s => s.MEDI_STOCK_ID).ToList(); 
                listMediStocks = new List<V_HIS_MEDI_STOCK>(); 
                skip = 0; 
                while (mediStockIds.Count() - skip > 0)
                {
                    var ListDSs = mediStockIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var mediStockFilter = new HisMediStockViewFilterQuery()
                    {
                        IDs = ListDSs,
                    }; 
                    var mediStocks = new HisMediStockManager(paramGet).GetView(mediStockFilter); 
                    listMediStocks.AddRange(mediStocks); 
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
                listTreatment = listTreatments.GroupBy(s => s.ID).Select(o => o.First()).ToList(); 

                foreach (var treatment in listTreatment)
                {                   
                    int Number = 0; 
                    decimal HEIN; 
                    decimal FEE; 
                    decimal EXPEND; 
                    var listSereServsGroupByServiceIds = listSereServs.Where(s => s.TDL_TREATMENT_ID == treatment.ID).ToList().GroupBy(s => s.SERVICE_ID); 
                    foreach (var listSereServsGroupByServiceId in listSereServsGroupByServiceIds)
                    {
                        HEIN = 0; 
                        FEE = 0; 
                        EXPEND = 0; 
                        var MATERIAL_CODE = listSereServsGroupByServiceId.First().TDL_SERVICE_CODE; 
                        var MATERIAL_NAME = listSereServsGroupByServiceId.First().TDL_SERVICE_NAME; 
                        foreach (var sereServ in listSereServsGroupByServiceId)
                        {
                            Number = Number + 1; 
                            if (sereServ.IS_EXPEND != null)
                            {
                                EXPEND = EXPEND + sereServ.AMOUNT; 
                            }
                            else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            {
                                FEE = FEE + sereServ.AMOUNT; 
                            }
                            else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                HEIN = HEIN + sereServ.AMOUNT; 
                            }
                        }

                        var rdo = new Mrs00175RDO
                        {
                            TREATMENT_ID = listSereServsGroupByServiceId.First().TDL_TREATMENT_ID??0,
                            NUMBER = Number,
                            MATERIAL_CODE = MATERIAL_CODE,
                            MATERIAL_NAME = MATERIAL_NAME,
                            EXPEND = EXPEND,
                            FEE = FEE,
                            HEIN = HEIN,
                        }; 
                        listMrs00175Rdos.Add(rdo); 
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("EXP_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00175Filter)this.reportFilter).TIME_FROM)); 
            dicSingleTag.Add("EXP_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00175Filter)this.reportFilter).TIME_TO)); 
            dicSingleTag.Add("DEPARTMENT_NAME", listDepartmentTrans.Select(s => s.DEPARTMENT_NAME).First()); 
            //dicSingleTag.Add("MEDI_STOCK_ID", listMediStocks.Select(s => s.MEDI_STOCK_NAME).First()); 
            string MEDI_STOCK_NAME = ""; 
            foreach (var listMediStock in listMediStocks)
            {
                MEDI_STOCK_NAME = MEDI_STOCK_NAME + " - " + listMediStock.MEDI_STOCK_NAME; 
            }
            dicSingleTag.Add("MEDI_STOCK_IDs", MEDI_STOCK_NAME); 
            objectTag.AddObjectData(store, "Report", listMrs00175Rdos); 
            objectTag.AddObjectData(store, "Report1", listTreatment); 
            objectTag.AddRelationship(store, "Report1", "Report", "ID", "TREATMENT_ID"); 
        }
    }
}
