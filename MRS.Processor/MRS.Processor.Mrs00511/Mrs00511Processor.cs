using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using MOS.MANAGER.HisDepartment; 
using MOS.MANAGER.HisSereServ; 
using MOS.MANAGER.HisBranch; 
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisTreatment; 

namespace MRS.Processor.Mrs00511
{
    class Mrs00511Processor : AbstractProcessor
    {
        Mrs00511Filter castFilter = null; 

        public Mrs00511Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        List<Mrs00511RDO> listRdo = new List<Mrs00511RDO>();

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<HIS_TREATMENT> listTreatments = new List<HIS_TREATMENT>();

        List<long> listServiceTntIds = new List<long>();
        List<long> listServiceHdfIds = new List<long>();

        string thisReportTypeCode = "";

        public override Type FilterType()
        {
            return typeof(Mrs00511Filter); 
        }

        protected override bool GetData()
        {
            bool valid = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00511Filter)this.reportFilter; 
                // getData
                HisServiceRetyCatViewFilterQuery serviceRetyCatFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatFilter.REPORT_TYPE_CODE__EXACT = this.thisReportTypeCode;
                var listServiceRetyCats = new HisServiceRetyCatManager(param).GetView(serviceRetyCatFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>();

                listServiceTntIds = listServiceRetyCats.Where(w => w.CATEGORY_CODE.Equals("TNT")).Select(s => s.SERVICE_ID).ToList();
                listServiceHdfIds = listServiceRetyCats.Where(w => w.CATEGORY_CODE.Equals("HDF")).Select(s => s.SERVICE_ID).ToList();

                List<long> listAllServices = new List<long>();
                listAllServices.AddRange(listServiceTntIds);
                listAllServices.AddRange(listServiceHdfIds);

                HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                sereServFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                sereServFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                sereServFilter.SERVICE_IDs = listAllServices;
                listSereServs = new HisSereServManager(param).GetView(sereServFilter) ?? new List<V_HIS_SERE_SERV>();

                List<long> listTreatmentIds = listSereServs.Select(s => s.TDL_TREATMENT_ID.Value).Distinct().ToList();
                var skip = 0;
                while (listTreatmentIds.Count - skip > 0)
                {
                    var listIDs = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                    treatmentFilter.IDs = listIDs;
                    listTreatments.AddRange(new HisTreatmentManager(param).Get(treatmentFilter) ?? new List<HIS_TREATMENT>());
                }
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
                listRdo.Clear(); 

                //process
                foreach (var sereServ in listSereServs)
                {
                    var rdo = new Mrs00511RDO();
                    rdo.TREATMENT_ID = sereServ.TDL_TREATMENT_ID.Value;
                    rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;

                    var treatments = listTreatments.Where(w => w.ID == sereServ.TDL_TREATMENT_ID.Value).ToList();
                    if(IsNotNullOrEmpty(treatments))
                    {
                        rdo.FIRST_NAME = treatments.First().TDL_PATIENT_FIRST_NAME;
                        rdo.LAST_NAME = treatments.First().TDL_PATIENT_LAST_NAME;
                        rdo.DOB = treatments.First().TDL_PATIENT_DOB;
                    }

                    if (IsNotNullOrEmpty(listServiceTntIds.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        if (sereServ.REQUEST_ROOM_CODE == sereServ.EXECUTE_ROOM_CODE)
                            rdo.TNT_TCK = sereServ.AMOUNT;
                        else
                            rdo.TNT_NCK = sereServ.AMOUNT;
                    }
                    else if (IsNotNullOrEmpty(listServiceHdfIds.Where(w => w == sereServ.SERVICE_ID).ToList()))
                    {
                        if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            rdo.HDF_BH = sereServ.AMOUNT;
                        else if (sereServ.PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE)
                            rdo.HDF_ND = sereServ.AMOUNT;
                    }

                    rdo.PRICE_TNT = sereServ.PRICE;
                    listRdo.Add(rdo);
                }

                listRdo = listRdo.GroupBy(g => new { g.TREATMENT_ID, g.PRICE_TNT }).Select(s => new Mrs00511RDO()
                {
                    TREATMENT_ID = s.First().TREATMENT_ID,
                    TREATMENT_CODE = s.First().TREATMENT_CODE,

                    FIRST_NAME = s.First().FIRST_NAME,
                    LAST_NAME = s.First().LAST_NAME,
                    DOB = s.First().DOB,

                    TNT_TCK = s.Sum(su => su.TNT_TCK),
                    TNT_NCK = s.Sum(su => su.TNT_NCK),
                    HDF_ND = s.Sum(su => su.HDF_ND),
                    HDF_BH = s.Sum(su => su.HDF_BH),

                    PRICE_TNT = s.First().PRICE_TNT
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);

                dicSingleTag.Add("TOTAL_PRICE", listRdo.Sum(s => (s.TNT_NCK + s.TNT_TCK) * s.PRICE_TNT));

                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(o => o.LAST_NAME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
