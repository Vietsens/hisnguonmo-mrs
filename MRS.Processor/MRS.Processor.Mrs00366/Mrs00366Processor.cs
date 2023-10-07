using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisBranch;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
 
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisTreatment; 

namespace MRS.Processor.Mrs00366
{
    class Mrs00366Processor : AbstractProcessor
    {
        private List<Mrs00366RDO> listMrs00366Rdos = new List<Mrs00366RDO>(); 
        Mrs00366Filter castFilter = null; 

        List<HIS_SERE_SERV> listSereServs = new List<HIS_SERE_SERV>();
        List<V_HIS_SERVICE> listServices = new List<V_HIS_SERVICE>(); 
        List<HIS_SERE_SERV_DEPOSIT> listSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>(); 
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
        List<HIS_TREATMENT> treatment = new List<HIS_TREATMENT>(); 
        List<HIS_BRANCH> listBranchs = new List<HIS_BRANCH>(); 

        public Mrs00366Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00366Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00366Filter)this.reportFilter;

                var manuHisSereServViewFilterQuery = new HisSereServFilterQuery()
                {
                    TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM,
                    TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO,
                    TDL_SERVICE_TYPE_ID = castFilter.SERVICE_TYPE_ID,
                    IS_EXPEND = false,
                };
                listSereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(param).Get(manuHisSereServViewFilterQuery);
                listSereServs = listSereServs.Where(o => o.IS_DELETE == IMSys.DbConfig.HIS_RS.COMMON.IS_DELETE__TRUE).ToList();
                if (castFilter.BRANCH_ID != null)
                {
                    listSereServs = listSereServs.Where(o => HisDepartmentCFG.DEPARTMENTs.Exists(p => p.ID == o.TDL_REQUEST_DEPARTMENT_ID && p.BRANCH_ID == castFilter.BRANCH_ID)).ToList();
                }
                if (castFilter.SERVICE_ID != null)
                {
                    HisServiceViewFilterQuery serviceViewFilter = new HisServiceViewFilterQuery();
                    serviceViewFilter.ID = castFilter.SERVICE_ID;
                    listServices = new MOS.MANAGER.HisService.HisServiceManager(param).GetView(serviceViewFilter);
                }
                var skip = 0; 
                while (listSereServs.Count() - skip > 0)
                {
                    var ListDSs = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var menuSereServDepositFilter = new HisSereServDepositFilterQuery()
                    {
                        SERE_SERV_IDs = ListDSs.Select(s=>s.ID).ToList(),
                    }; 
                    var listSereServDepositSub = new HisSereServDepositManager(param).Get(menuSereServDepositFilter); 
                    listSereServDeposit.AddRange(listSereServDepositSub); 
                }
                
                skip = 0; 
                while (listSereServs.Count() - skip > 0)
                {
                    var treatmentId = listSereServs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                    var ListDSs = treatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var treatmentFilter = new HisTreatmentFilterQuery()
                    {
                        IDs = ListDSs,
                    };
                    var treatmentSub = new HisTreatmentManager(param).Get(treatmentFilter);
                    treatment.AddRange(treatmentSub); 
                }
                treatment = treatment.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).ToList();
                HisBranchFilterQuery branchName = new HisBranchFilterQuery(); 
                branchName.IDs = listDepartments.Select(s=>s.BRANCH_ID).ToList(); 
                listBranchs = new MOS.MANAGER.HisBranch.HisBranchManager(param).Get(branchName); 

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {

                var listSereServss = listSereServs.Where(s => treatment.Exists(o=>o.ID==s.TDL_TREATMENT_ID) && (listSereServDeposit == null || !listSereServDeposit.Exists(o => o.SERE_SERV_ID == s.ID))).ToList();
                var listSereServ = castFilter.SERVICE_ID != null ? listSereServss.Where(s => s.PARENT_ID == castFilter.SERVICE_ID) : listSereServss;
                foreach (var SereServ in listSereServ)
                {
                    Mrs00366RDO rdo = new Mrs00366RDO();
                    rdo.SERVICE_NAME = SereServ.TDL_SERVICE_NAME;
                    rdo.AMOUNT = SereServ.AMOUNT;
                    rdo.PRICE = SereServ.PRICE;
                    rdo.DCT_BH = SereServ.VIR_HEIN_PRICE;
                    rdo.DCT_BN = SereServ.VIR_PATIENT_PRICE;
                    rdo.TOTAL_PRICE = SereServ.VIR_TOTAL_PRICE;
                    listMrs00366Rdos.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                //dicSingleTag.Add("BRANCH_NAME", listBranchs.Select(s => s.BRANCH_NAME).First());
                dicSingleTag.Add("SERVICE_NAME", (listServices.FirstOrDefault(o => o.ID == castFilter.SERVICE_ID)??new V_HIS_SERVICE()).SERVICE_NAME); 

                objectTag.AddObjectData(store, "Report", listMrs00366Rdos); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Report", "Report1", "EXP_MEST_CODE", "EXP_MEST_CODE"); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
