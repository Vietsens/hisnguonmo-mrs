using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
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
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisInvoice; 
using MOS.MANAGER.HisInvoiceDetail; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMediStockPeriod; 
using MOS.MANAGER.HisMestPeriodMate; 
using MOS.MANAGER.HisMestPeriodMedi; 
using MOS.MANAGER.HisPatient; 
using MOS.MANAGER.HisPatientType; 
using MOS.MANAGER.HisReportTypeCat; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisService; 
using MOS.MANAGER.HisServiceType; 
using MOS.MANAGER.HisServiceRetyCat; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisBranch; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisServiceReq; 
using MOS.MANAGER.HisTransaction; 
using MOS.MANAGER.HisSereServDeposit; 
using MOS.MANAGER.HisSeseDepoRepay; 

namespace MRS.Processor.Mrs00425
{
    class Mrs00425Processor : AbstractProcessor
    {
        Mrs00425Filter castFilter = null; 

        public Mrs00425Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<Mrs00425RDO> listRdo = new List<Mrs00425RDO>(); 
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 
        List<HIS_SERE_SERV_DEPOSIT> listSereServDeposits = new List<HIS_SERE_SERV_DEPOSIT>(); 
        List<HIS_SESE_DEPO_REPAY> listRepays = new List<HIS_SESE_DEPO_REPAY>(); 

        V_HIS_SERVICE parentService = null; 
        HIS_SERVICE_TYPE serviceType = null; 
        HIS_BRANCH brand = null; 

        public override Type FilterType()
        {
            return typeof(Mrs00425Filter); 
        }

        protected override bool GetData()
        {
            bool valid = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00425Filter)this.reportFilter; 
                var skip = 0; 

                serviceType = new HisServiceTypeManager(paramGet).GetById(this.castFilter.SERVICE_TYPE_ID); 
                brand = new HisBranchManager(paramGet).GetById(this.castFilter.BRANCH_ID); 

                if (this.castFilter.SERVICE_ID.HasValue)
                {
                    HisServiceViewFilterQuery serviceFilter = new HisServiceViewFilterQuery(); 
                    serviceFilter.ID = castFilter.SERVICE_ID; 
                    var listData = new HisServiceManager(paramGet).GetView(serviceFilter); 

                    if (paramGet.HasException)
                    {
                        throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_SERVICE, MRS00293"); 
                    }

                    if (listData == null || listData.Count != 1)
                    {
                        throw new Exception("Khong lay duoc Service theo ServiceId:" + castFilter.SERVICE_ID); 
                    }
                    parentService = listData.FirstOrDefault(); 
                }

                //Lay ID khoa theo co so
                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery(); 
                departmentFilter.BRANCH_ID = this.castFilter.BRANCH_ID; 
                listDepartments = new HisDepartmentManager(paramGet).Get(departmentFilter); 

                var listReqDepartments = listDepartments.Select(s => s.ID).ToList(); 
                //Lay DV tam ung
                HisSereServDepositFilterQuery SereServDepositFilter = new HisSereServDepositFilterQuery(); 
                SereServDepositFilter.CREATE_TIME_FROM = this.castFilter.TIME_FROM; 
                SereServDepositFilter.CREATE_TIME_TO = this.castFilter.TIME_TO; 
                listSereServDeposits = new HisSereServDepositManager(paramGet).Get(SereServDepositFilter); 
                //Lay DV hoan ung
                HisSeseDepoRepayFilterQuery repayFilter = new HisSeseDepoRepayFilterQuery(); 
                repayFilter.CREATE_TIME_FROM = this.castFilter.TIME_FROM; 
                repayFilter.CREATE_TIME_TO = this.castFilter.TIME_TO; 
                listRepays = new HisSeseDepoRepayManager(paramGet).Get(repayFilter); 

                var listSeseDepoRepayIds = listRepays.Select(s => s.SERE_SERV_DEPOSIT_ID).ToList();  //Lay ID hoan ung

                //Lay DV tam ung ma chua hoan ung
                listSereServDeposits = listSereServDeposits.Where(w =>!listSeseDepoRepayIds.Contains(w.ID)).ToList(); 

                var listSereServIds = listSereServDeposits.Select(s => s.SERE_SERV_ID).Distinct().ToList(); 

                while (listSereServIds.Count - skip > 0)
                {
                    var listSereServId = listSereServIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    //Lay DV
                    HisSereServViewFilterQuery serServFilter = new HisSereServViewFilterQuery(); 
                    serServFilter.SERVICE_TYPE_ID = this.castFilter.SERVICE_TYPE_ID; 
                    
                    serServFilter.REQUEST_DEPARTMENT_IDs = listReqDepartments; 
                    serServFilter.IDs = listSereServId; 

                    var listSereServ = new HisSereServManager(paramGet).GetView(serServFilter); 
                    listSereServs.AddRange(listSereServ); 
                }
                if (this.castFilter.SERVICE_ID.HasValue)
                {
                    listSereServs = listSereServs.Where(w => w.PARENT_ID == parentService.ID).ToList(); 
                }
                List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>(); 
                //yeu cau
                var treatmentIds = listSereServs.Select(o => o.TDL_TREATMENT_ID.Value).Distinct().ToList(); 
                var skiprq = 0; 
                while (treatmentIds.Count - skiprq > 0)
                {
                    var listIDs = treatmentIds.Skip(skiprq).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skiprq = skiprq + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisServiceReqViewFilterQuery FilterSeq = new HisServiceReqViewFilterQuery()
                    {
                        TREATMENT_IDs = listIDs
                    }; 
                    var LisServiceReqLib = new HisServiceReqManager(paramGet).GetView(FilterSeq); 
                    listServiceReq.AddRange(LisServiceReqLib); 
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }

        protected override bool ProcessData()
        {
            bool valid = true; 
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    foreach (var sereServ in listSereServs)
                    {
                        var dereDetail = listSereServDeposits.Where(w => w.SERE_SERV_ID == sereServ.ID); 
                        Mrs00425RDO rdo = new Mrs00425RDO(); 
                        rdo.INSTRUCTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateStringSeparateString(req(sereServ).INTRUCTION_TIME); 
                        rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE; 
                        rdo.PATIENT_CODE = req(sereServ).TDL_PATIENT_CODE; 
                        rdo.VIR_PATIENT_NAME = req(sereServ).TDL_PATIENT_NAME; 
                        if (req(sereServ).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.FEMALE_AGE = req(sereServ).TDL_PATIENT_DOB; 
                        }
                        if (req(sereServ).TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.MALE_AGE = req(sereServ).TDL_PATIENT_DOB; 
                        }
                        rdo.HEIN_CARD_NUMBER = sereServ.HEIN_CARD_NUMBER; 
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                        rdo.AMOUNT = dereDetail.FirstOrDefault().TDL_AMOUNT; 
                        rdo.PRICE = sereServ.PRICE; 
                        rdo.HEIN_PRICE = sereServ.HEIN_PRICE; 
                        rdo.TOTAL_PATIENT_PRICE = sereServ.VIR_TOTAL_PATIENT_PRICE; 
                        rdo.TOTAL_HEIN_PRICE = sereServ.VIR_TOTAL_HEIN_PRICE; 

                        listRdo.Add(rdo); 
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }

        private V_HIS_SERVICE_REQ req(V_HIS_SERE_SERV sereServ)
        {
            return dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0) ? dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0] : new V_HIS_SERVICE_REQ(); 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {

                dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                if (parentService != null)
                {
                    dicSingleTag.Add("SERVICE_CODE", parentService.SERVICE_CODE); 
                    dicSingleTag.Add("SERVICE_NAME", parentService.SERVICE_NAME); 
                    dicSingleTag.Add("SERVICE_TYPE_NAME", parentService.SERVICE_TYPE_NAME); 
                }
                else
                {
                    dicSingleTag.Add("SERVICE_TYPE_NAME", serviceType.SERVICE_TYPE_NAME); 
                }

                dicSingleTag.Add("BRANCH_NAME", brand.BRANCH_NAME); 
                objectTag.AddObjectData(store, "Report", listRdo); 
                objectTag.SetUserFunction(store, "FuncSameTitleCol", new MergeManyRowData()); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
        public V_HIS_SERVICE GetById(long Id)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 
                HisServiceViewFilterQuery filter = new HisServiceViewFilterQuery(); 
                filter.ID = Id; 
                return new HisServiceManager(paramGet).GetView(filter).SingleOrDefault(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return null; 
            }
        }
    }

    class MergeManyRowData : FlexCel.Report.TFlexCelUserFunction
    {
        long currentInstructionData; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            try
            {
                long instructionDate = Convert.ToInt64(parameters[0]); 
                if (instructionDate == currentInstructionData)
                {
                    return true; 
                }
                else
                {
                    currentInstructionData = instructionDate; 
                    return false; 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                return false; 
            }
            throw new NotImplementedException(); 
        }
    }
}
