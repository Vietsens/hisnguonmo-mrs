using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisTreatmentType;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00727
{
    class Mrs00727Processor : AbstractProcessor
    {
        Mrs00727Filter filter = null;
        List<ManagerSql.DEBATE> ListData = new List<ManagerSql.DEBATE>();
        List<Mrs00727RDO> ListRdo = new List<Mrs00727RDO>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();
        List<HIS_EXECUTE_ROLE> ListRole = new List<HIS_EXECUTE_ROLE>();
        List<HIS_DEBATE_EKIP_USER> ListEkip = new List<HIS_DEBATE_EKIP_USER>();
        List<HIS_DEBATE_TYPE> ListDebateType = new List<HIS_DEBATE_TYPE>();
        
        CommonParam paramGet = new CommonParam();

        public Mrs00727Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00727Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            filter = (Mrs00727Filter)base.reportFilter;
            try
            {
                string query = "select * from his_debate_ekip_user";
                ListEkip = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEBATE_EKIP_USER>(query);

                string query1 = "select * from his_debate_type";
                ListDebateType = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEBATE_TYPE>(query1);
                ListData = new ManagerSql().GetRdo(filter);
                
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
                if (IsNotNullOrEmpty(ListData))
                {
                    var listData = ListData.Select(p => p.ID).Distinct().ToList();
                    foreach (var item in listData)
                    {
                        var ekip = ListEkip.Where(o => o.DEBATE_ID == item);
                        var type = ListDebateType.FirstOrDefault(o => o.ID == ListData.Where(p => p.ID==item).First().DEBATE_TYPE_ID);
                        Mrs00727RDO rdo = new Mrs00727RDO();
                        rdo.DEBATE_TIME = ListData.Where(p => p.ID == item).First().DEBATE_TIME;
                        rdo.DEBATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.DEBATE_TIME);
                        rdo.PATIENT_NAME = ListData.Where(p => p.ID == item).First().PATIENT_NAME;
                        rdo.PATIENT_CODE = ListData.Where(p => p.ID == item).First().PATIENT_CODE;
                        rdo.TREATMENT_CODE = ListData.Where(p => p.ID == item).First().TREATMENT_CODE;
                        rdo.REQUEST_DEPARTMENT_ID = ListData.Where(p => p.ID == item).First().REQUEST_DEPARTMENT_ID;
                        rdo.REQUEST_DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ListData.Where(p => p.ID == item).First().REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                        rdo.REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == ListData.Where(p => p.ID == item).First().REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                        rdo.TREATMENT_TYPE_NAME = ListData.Where(x => x.ID == item).First().TREATMENT_TYPE_NAME;
                        rdo.PATIENT_TYPE_NAME = ListData.Where(x => x.ID == item).First().PATIENT_TYPE_NAME;
                        if (ekip != null)
                        {
                            rdo.DEBATE_DOCTORs = string.Join(";", ekip.OrderBy(p => p.EXECUTE_ROLE_ID).Select(p => p.USERNAME));
                            
                        }

                        if (type != null)
                        {
                            rdo.DEBATE_TYPE_NAME = type.DEBATE_TYPE_NAME;
                        }
                        if (ListData.Where(p => p.ID == item).First().CONTENT_TYPE == 1)
                        {
                            rdo.CONTENT_TYPE_NAME = "Khác";
                        }
                        else if (ListData.Where(p => p.ID == item).First().CONTENT_TYPE == 2)
                        {
                            rdo.CONTENT_TYPE_NAME = "Hội chẩn thuốc";
                        }
                        else if (ListData.Where(p => p.ID == item).First().CONTENT_TYPE == 3)
                        {
                            rdo.CONTENT_TYPE_NAME = "Hội chẩn trước phẫu thuật";
                        }
                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(p => p.REQUEST_DEPARTMENT_ID).ThenBy(p => p.DEBATE_TIME).ToList());
            objectTag.AddObjectData(store, "Department", ListRdo.GroupBy(p => p.REQUEST_DEPARTMENT_NAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Department", "Report", "REQUEST_DEPARTMENT_NAME", "REQUEST_DEPARTMENT_NAME");
            dicSingleTag.Add("DepartmentNames", string.Join(",", ListRdo.Select(x => x.REQUEST_DEPARTMENT_NAME).Distinct().ToList()));
        }
    }
}
