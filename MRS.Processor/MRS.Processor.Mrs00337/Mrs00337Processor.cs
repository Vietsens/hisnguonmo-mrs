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
using Inventec.Common.DateTime;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisIcdGroup;

namespace MRS.Processor.Mrs00337
{
    public class Mrs00337Processor : AbstractProcessor
    {
        private Mrs00337Filter filter;
        private CommonParam paramGet = new CommonParam();
        private List<RdoGet> listRdoGet = new List<RdoGet>();
        private List<Mrs00337RDO> listRdo = new List<Mrs00337RDO>();

        List<long> DepartmentIdExam = null;
        public Mrs00337Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00337Filter);
        }

        protected override bool GetData()///
        {
            var result = true;
            filter = (Mrs00337Filter)reportFilter;
            try
            {
                DepartmentIdExam = HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM;
                listRdoGet = new ManagerSql().GetRdo(filter, string.Join("','", DepartmentIdExam ?? new List<long>()));
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
            var result = true;
            try
            {


                foreach (var item in listRdoGet)
                {
                    Mrs00337RDO rdo = new Mrs00337RDO();
                    rdo.ICD_NAME = item.ICD_NAME;
                    rdo.ICD_CODE = item.ICD_CODE;
                    rdo.ICD_GROUP_ID = item.ICD_GROUP_ID;
                    rdo.ICD_GROUP_CODE = item.ICD_GROUP_CODE;
                    rdo.ICD_GROUP_NAME = item.ICD_GROUP_NAME;
                    rdo.TOTAL_EXAMINATION = item.TOTAL_EXAM;
                    rdo.FEMALE_EXAMINATION = item.FEMALE_EXAM;
                    rdo.CHILDREN_UNDER_15_AGE_EXAMINATION = item.CHILD_UNDER_15_AGE_EXAM;
                    rdo.TOTAL_DEAD_EXAMINATION = item.TOTAL_DEAD_EXAM;
                    rdo.TOTAL_SICK_BOARDING = item.TOTAL_SICK;
                    rdo.TOTAL_FEMALE_SICK_BOARDING = item.TOTAL_FEMALE_SICK;
                    rdo.TOTAL_DEAD_BOARDING = item.TOTAL_DEAD;
                    rdo.TOTAL_FEMALE_DEAD_BOARDING = item.TOTAL_FEMALE_DEAD;
                    rdo.TOTAL_CHILDREN_UNDER_15_AGE_SICK_BOARDING = item.TOTAL_CHILD_UNDER_15_AGE_SICK;
                    rdo.TOTAL_CHILDREN_UNDER_5_AGE_SICK_BOARDING = item.TOTAL_CHILD_UNDER_5_AGE_SICK;
                    rdo.TOTAL_CHILDREN_UNDER_15_AGE_DEAD_BOARDING = item.TOTAL_CHILD_UNDER_15_AGE_DEAD;
                    rdo.TOTAL_CHILDREN_UNDER_5_AGE_DEAD_BOARDING = item.TOTAL_CHILD_UNDER_5_AGE_DEAD;
                    rdo.TOTAL_DEAD_BEFORE = item.TOTAL_DEAD_BEFORE;
                    rdo.TOTAL_NANG_XINVE = item.TOTAL_NANG_XINVE;

                    listRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

      
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("DATE_TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_TIME_FROM));
            dicSingleTag.Add("DATE_TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.DATE_TIME_TO));

            objectTag.AddObjectData(store, "Report", listRdo);
            objectTag.AddObjectData(store, "ReportParent", listRdo.GroupBy(o => o.ICD_GROUP_ID).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ReportParent", "Report", "ICD_GROUP_ID", "ICD_GROUP_ID");
        }

    }
}
