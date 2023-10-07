using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00825
{
    public class Mrs00825Processor : AbstractProcessor
    {
        public Mrs00825Filter filter;
        public List<Mrs00825RDO> ListRdo = new List<Mrs00825RDO>();
        public List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        public List<V_HIS_ROOM> ListRoom = new List<V_HIS_ROOM>();
        public List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        public List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran = new List<V_HIS_DEPARTMENT_TRAN>();
        public List<V_HIS_SERE_SERV> ListSereServ = new List<V_HIS_SERE_SERV>();
        public Mrs00825Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
       
        public override Type FilterType()
        {
            return typeof(Mrs00825Filter);
        }

        protected override bool GetData()
        {
            filter = (Mrs00825Filter)this.reportFilter;
            bool result = false;
            try 
	        {	        
                HisTreatmentFilterQuery treaFilter = new HisTreatmentFilterQuery();
                treaFilter.IN_TIME_FROM = filter.TIME_FROM;
                treaFilter.IN_TIME_TO = filter.TIME_TO;
                ListTreatment = new HisTreatmentManager().Get(treaFilter);
                var treatmentIds = ListTreatment.Select(x => x.ID).Distinct().ToList();

                var skip = 0;
                while (treatmentIds.Count-skip>0)
                {
                    var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisDepartmentTranViewFilterQuery departmentTranFilter = new HisDepartmentTranViewFilterQuery();
                    departmentTranFilter.TREATMENT_IDs = limit;
                    var departmentTrans = new HisDepartmentTranManager().GetView(departmentTranFilter);
                    ListDepartmentTran.AddRange(departmentTrans);
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                    ssFilter.TREATMENT_IDs = limit;
                    ssFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH;
                    var sereServs = new HisSereServManager().GetView(ssFilter);
                    ListSereServ.AddRange(sereServs);
                }
		        result = true;
	        }
	        catch (Exception ex)
	        {
                Inventec.Common.Logging.LogSystem.Error(ex);
		        result= false;
	        }
            return result;
            
        }

        protected override bool ProcessData()
        {
           bool result = false;
            try 
	        {
                foreach (var item in ListTreatment)
                {
                    var Rooms = ListSereServ.Where(x=>x.TDL_TREATMENT_ID==item.ID).Select(x => x.EXECUTE_ROOM_NAME).Distinct().ToList();
                    var Departmets = ListDepartmentTran.Where(x => x.TREATMENT_ID == item.ID).Select(x => x.DEPARTMENT_NAME).Distinct().ToList();
                    Mrs00825RDO rdo = new Mrs00825RDO();
                    rdo.PATIENT_CODE = item.TDL_PATIENT_CODE;
                    rdo.PATIENT_NAME = item.TDL_PATIENT_NAME;
                    rdo.PATIENT_DOB = item.TDL_PATIENT_DOB;
                    rdo.PATIENT_DOB_STR =Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB);
                    //rdo.PATIENT_AGE = Inventec.Common.DateTime.Calculation.AgeString(item.TDL_PATIENT_DOB);
                    rdo.PATIENT_GENDER_NAME = item.TDL_PATIENT_GENDER_NAME;
                    rdo.PATIENT_ADDRESS = item.TDL_PATIENT_ADDRESS;
                    rdo.TREATMENT_CODE = item.TREATMENT_CODE;
                    rdo.CCCD_NUMBER = item.TDL_PATIENT_CCCD_NUMBER;
                    rdo.CMND_NUMBER = item.TDL_PATIENT_CMND_NUMBER;
                    rdo.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID ?? 0;
                    if (Rooms!=null)
                    {
                        rdo.ROOM_NAMEs = string.Join(",", Rooms.ToList());
                    }
                    if (Departmets!=null)
                    {
                        rdo.DEPARTMENT_NAMEs = string.Join(",", Departmets.ToList());
                    }
                    ListRdo.Add(rdo);
                }
		        result = true;
	        }
	        catch (Exception ex)
	        {
                Inventec.Common.Logging.LogSystem.Error(ex);
		        result= false;
	        }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store, "Report", ListRdo);
        }
    }
}
