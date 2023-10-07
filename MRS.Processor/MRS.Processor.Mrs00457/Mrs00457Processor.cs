using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Core.MrsReport; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00457
{
    class Mrs00457Processor : AbstractProcessor
    {
        Mrs00457Filter castFilter = null; 
        List<Mrs00457RDO> listRdo = new List<Mrs00457RDO>(); 
        List<Mrs00457RDO> listGroupService = new List<Mrs00457RDO>(); 
        List<Mrs00457RDO> listGroupDepartment = new List<Mrs00457RDO>(); 
        List<Mrs00457RDO> listGroupServiceType = new List<Mrs00457RDO>(); 


        public Mrs00457Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>(); 

        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 

        public string s_Departments = ""; 

        public override Type FilterType()
        {
            return typeof(Mrs00457Filter); 
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                CommonParam paramGet = new CommonParam(); 
                this.castFilter = (Mrs00457Filter)this.reportFilter; 
                var skip = 0; 

                if(IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery(); 
                    filter.IDs = this.castFilter.REQUEST_DEPARTMENT_IDs; 
                    listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(filter); 

                    foreach(var department in listDepartments)
                    {
                        s_Departments += department.DEPARTMENT_NAME + ","; 
                    }
                    s_Departments = s_Departments.Substring(0,s_Departments.Length - 1); 
                }

                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery(); 
                treatmentFilter.FEE_LOCK_TIME_FROM = this.castFilter.TIME_FROM; 
                treatmentFilter.FEE_LOCK_TIME_TO = this.castFilter.TIME_TO; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatmentFilter); 

                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList(); 
                while(listTreatmentIds.Count - skip > 0 )
                {
                    var listIds = listTreatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    HisSereServViewFilterQuery filter = new HisSereServViewFilterQuery(); 
                    filter.TREATMENT_IDs = listIds; 
                    if(IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                    {
                        filter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs; 
                    }
                    var sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filter); 
                    listSereServs.AddRange(sereServs); 
                }
                listSereServs = listSereServs.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList(); 
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
                if(IsNotNullOrEmpty(listSereServs))
                {
                    foreach(var sereServ  in listSereServs)
                    {
                        Mrs00457RDO rdo = new Mrs00457RDO(); 
                        rdo.AMOUNT = sereServ.AMOUNT; 
                        rdo.DEPARTMENT_ID = sereServ.TDL_REQUEST_DEPARTMENT_ID; 
                        rdo.DEPARTMENT_NAME = sereServ.REQUEST_DEPARTMENT_NAME; 
                        rdo.PRICE = sereServ.PRICE; 
                        rdo.SERVICE_ID = sereServ.SERVICE_ID; 
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME; 
                        rdo.SERVICE_TYPE_ID = sereServ.TDL_SERVICE_TYPE_ID; 
                        rdo.SERVICE_TYPE_NAME = sereServ.SERVICE_TYPE_NAME; 
                        rdo.TOTAL_PRICE = sereServ.AMOUNT * sereServ.PRICE; 
                        rdo.TOTAL_PRICE_BILL = sereServ.VIR_TOTAL_PRICE??0; 

                        listRdo.Add(rdo); 
                    }

                    listGroupService = listRdo.GroupBy(gr => new
                    {
                        gr.DEPARTMENT_ID,
                        gr.SERVICE_TYPE_ID,
                        gr.SERVICE_ID,
                        gr.PRICE
                    }).Select(s => new Mrs00457RDO
                    {
                        AMOUNT = s.Sum(su => su.AMOUNT),
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        PRICE = s.First().PRICE,
                        SERVICE_ID = s.First().SERVICE_ID,
                        SERVICE_NAME = s.First().SERVICE_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        TOTAL_PRICE = s.Sum(su => su.TOTAL_PRICE),
                        TOTAL_PRICE_BILL = s.Sum(su => su.TOTAL_PRICE_BILL)
                    }).ToList(); 

                    listGroupServiceType = listGroupService.GroupBy(gr => new
                    {
                        gr.SERVICE_TYPE_ID,
                        gr.DEPARTMENT_ID
                    }).Select(s => new Mrs00457RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME,
                        SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                        SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                        SERVICE_ID = s.First().SERVICE_ID
                    }).ToList(); 

                    listGroupDepartment = listGroupServiceType.GroupBy(gr => new
                    {
                        gr.DEPARTMENT_ID,
                    }).Select(s => new Mrs00457RDO
                    {
                        DEPARTMENT_ID = s.First().DEPARTMENT_ID,
                        DEPARTMENT_NAME = s.First().DEPARTMENT_NAME
                    }).ToList(); 
                
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                dicSingleTag.Add("DEPARTMENT", s_Departments); 
                objectTag.AddObjectData(store, "Report", listGroupService); 
                objectTag.AddObjectData(store, "ServiceType", listGroupServiceType); 
                objectTag.AddObjectData(store, "Department", listGroupDepartment); 
                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Department", "ServiceType", "DEPARTMENT_ID", "DEPARTMENT_ID"); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "ServiceType", "Report", new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID" }, new string[] { "SERVICE_TYPE_ID", "DEPARTMENT_ID" }); 
                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

    }
}
