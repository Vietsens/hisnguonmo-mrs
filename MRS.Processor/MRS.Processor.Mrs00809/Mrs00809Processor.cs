using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00809
{
    public class Mrs00809Processor : AbstractProcessor
    {
        public Mrs00809Filter Filter;
        public List<Mrs00809RDO> ListRdo = new List<Mrs00809RDO>();
        public List<Mrs00809TreatmentRdo> ListTreatRdo = new List<Mrs00809TreatmentRdo>();
        public List<Mrs0809ServiceRdo> ListSerRdo = new List<Mrs0809ServiceRdo>();
        public List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        public List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        public List<HIS_ROOM> ListRoom = new List<HIS_ROOM>();
        public List<HIS_DEPARTMENT> ListDeparrtment = new List<HIS_DEPARTMENT>();
        public List<V_HIS_TREATMENT> treat = new List<V_HIS_TREATMENT>();
        public List<V_HIS_SERVICE_REQ> listServiceReq = new List<V_HIS_SERVICE_REQ>();
        public CommonParam commonParam = new CommonParam();
        public Mrs00809Processor(CommonParam param, string reportTypeName):base(param,reportTypeName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00809Filter);
        }

        protected override bool GetData()
        {
            Filter = (Mrs00809Filter)this.reportFilter;
            bool result = false;
            try
            {
                ListDeparrtment = new HisDepartmentManager(commonParam).Get(new HisDepartmentFilterQuery());
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                if (Filter.DEPARTMENT_IDs!=null)
                {
                    treatmentFilter.END_DEPARTMENT_IDs = Filter.DEPARTMENT_IDs;
                }
                else
                {
                    treatmentFilter.END_DEPARTMENT_IDs = ListDeparrtment.Select(x => x.ID).ToList();
                }
                treatmentFilter.IS_PAUSE = false;
                treat = new HisTreatmentManager(commonParam).GetView(treatmentFilter); 
                treatmentFilter.IN_TIME_FROM = Filter.TIME_FROM;
                treatmentFilter.IN_TIME_TO = Filter.TIME_TO;
                //if (Filter.BRANCH_IDs!=null)
                //{
                //    treatmentFilter.BRANCH_IDs = Filter.BRANCH_IDs;
                //}
                ListTreatment = new HisTreatmentManager(commonParam).GetView(treatmentFilter);
                var treatmentIDs = ListTreatment.Select(x => x.ID).ToList();
                //HisSereServViewFilterQuery seveServFilter = new HisSereServViewFilterQuery();
                //seveServFilter.SERVICE_TYPE_IDs = new List<long>
                //{
                //    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                //    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU,
                //    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                //    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                //};
                //if (Filter.CHECK_TYPE_TIME == true)
                //{
                //    seveServFilter.IN_TIME_FROM = Filter.TIME_FROM;
                //    seveServFilter.IN_TIME_TO = Filter.TIME_TO;
                //}
                //else
                //{
                //    seveServFilter.OUT_TIME_FROM = Filter.TIME_FROM;
                //    seveServFilter.OUT_TIME_FROM = Filter.TIME_TO;
                //}
                ////seveServFilter.TREATMENT_IDs = treatmentIDs;
                //seveServFilter.EXECUTE_ROOM_IDs = Filter.EXECUTE_ROOM_IDs;
                //ListSereServ = new HisSereServManager(commonParam).GetView(seveServFilter);
                HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery();
                serviceReqFilter.SERVICE_REQ_TYPE_IDs = new List<long>
                {
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA
                };
                if (Filter.CHECK_TYPE_TIME == true)
                {
                    serviceReqFilter.INTRUCTION_TIME_FROM = Filter.TIME_FROM;
                    serviceReqFilter.INTRUCTION_TIME_TO = Filter.TIME_TO;
                }
                else
                {
                    serviceReqFilter.FINISH_TIME_FROM = Filter.TIME_FROM;
                    serviceReqFilter.FINISH_TIME_TO = Filter.TIME_TO;
                }
                listServiceReq = new HisServiceReqManager(commonParam).GetView(serviceReqFilter);
                HisSereServViewFilterQuery seveServFilter = new HisSereServViewFilterQuery();
                seveServFilter.SERVICE_REQ_IDs = listServiceReq.Select(x => x.ID).ToList();
                ListSereServ = new HisSereServManager(commonParam).GetView(seveServFilter);
                result = true;
            }
            catch (Exception ex)
            {
                result = true;
                LogSystem.Error(ex);
            }
            return result;

        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                //
                var groups = ListTreatment.GroupBy(x =>new { x.END_ROOM_NAME}).ToList();
                foreach (var item in groups)
                {
                    Mrs00809TreatmentRdo rdo = new Mrs00809TreatmentRdo();
                    rdo.EXECUTE_ROOM_CODE = item.First().END_ROOM_NAME;
                    rdo.EXECUTE_ROOM_NAME = item.First().END_ROOM_CODE;
                    rdo.AMOUNT = item.Count();
                    rdo.PRINT = rdo.EXECUTE_ROOM_NAME.ToString() + ": " + rdo.AMOUNT.ToString();
                    ListTreatRdo.Add(rdo);
                }
                var groupSers = ListSereServ.GroupBy(x =>new{ x.SERVICE_TYPE_CODE,x.SERVICE_TYPE_NAME,x.TDL_SERVICE_TYPE_ID}).ToList();
                foreach (var item in groupSers)
                {
                    Mrs0809ServiceRdo rdo = new Mrs0809ServiceRdo();
                    rdo.SERVICE_NAME = item.First().SERVICE_TYPE_NAME;
                    rdo.SERVICE_CODE = item.First().SERVICE_TYPE_CODE;
                    rdo.TOTAL_AMOUNT = item.Sum(x => x.AMOUNT);
                    rdo.SERVICE_PRINT = rdo.SERVICE_NAME.ToString() + ": " + rdo.TOTAL_AMOUNT.ToString();
                    ListSerRdo.Add(rdo);
                }
                // treatment
                var groupByDepa = ListTreatment.GroupBy(x => new { x.LAST_DEPARTMENT_ID }).ToList();
                foreach (var item in groupByDepa)
                {
                    Mrs00809RDO rdoA = new Mrs00809RDO();
                    rdoA.AMOUNT_CV = item.Where(x => x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Count();
                    rdoA.AMOUNT_DIE = item.Where(x => x.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHET).Count();
                    rdoA.AMOUNT_RV = item.Where(x => x.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN).Count();
                    rdoA.AMOUNT_OLD = treat.Where(x => x.IN_TIME < Filter.TIME_FROM&& x.LAST_DEPARTMENT_ID==item.First().LAST_DEPARTMENT_ID).Count();
                    rdoA.AMOUNT_NEW = item.Where(x=>x.IN_TIME>= Filter.TIME_FROM&& x.IN_TIME<=Filter.TIME_TO).Count();
                    rdoA.AMOUNT_CD = item.Where(x => x.IS_TRANSFER_IN == 1).Count();
                    rdoA.AMOUNT_CK = item.Where(x => x.END_DEPARTMENT_ID !=x.LAST_DEPARTMENT_ID).Count();
                    var depa = ListDeparrtment.Where(x => x.ID == item.First().LAST_DEPARTMENT_ID).FirstOrDefault();
                    if (depa!= null)
                    {
                        rdoA.DEPARTMENT_NAME = depa.DEPARTMENT_NAME;
                        rdoA.DEPARTMENT_CODE = depa.DEPARTMENT_CODE;
                    }
                    rdoA.AMOUNT_TREATMENT = item.Where(x => x.IS_PAUSE == null).Count()+ rdoA.AMOUNT_OLD- rdoA.AMOUNT_RV- rdoA.AMOUNT_DIE- rdoA.AMOUNT_CV;
                    ListRdo.Add(rdoA);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = true;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", Filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", ListRdo);

            string SERVICE = "";
            foreach (var item in ListSerRdo)
            {
                SERVICE += item.SERVICE_PRINT + ";";
            }
            string trea = "";
            foreach (var item in ListTreatRdo)
            {
                trea += item.PRINT + ";";
            }
            dicSingleTag.Add("SERVICE", SERVICE);
            dicSingleTag.Add("TREA", trea);
            dicSingleTag.Add("TOTAL_SERVICE", ListSerRdo.Sum(x => x.TOTAL_AMOUNT));
            dicSingleTag.Add("TOTAL_TREATMENT", ListTreatRdo.Sum(x => x.AMOUNT));
        }
    }
}
