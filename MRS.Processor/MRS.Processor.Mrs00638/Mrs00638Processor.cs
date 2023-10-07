using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00638;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceRetyCat;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00638
{
    public class Mrs00638Processor : AbstractProcessor
    {
        private List<Mrs00638RDO> ListRdo = new List<Mrs00638RDO>();
        private List<Mrs00638RDO> ListRdoDepartment = new List<Mrs00638RDO>();
        private List<Mrs00638RDO> ListRdoService = new List<Mrs00638RDO>();
        Mrs00638Filter filter = null;
        string thisReportTypeCode = "";

        List<Mrs00638RDO> listHisSereServ = new List<Mrs00638RDO>();

        public Mrs00638Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00638Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00638Filter)this.reportFilter;
            try
            {
                listHisSereServ = new ManagerSql().GetSereServDO(filter);
                //lay loc theo trạng thái hồ sơ
                if (filter.IS_TREATING_OR_FEE_LOCK != null)
                {
                    listHisSereServ = listHisSereServ.Where(o => o.IS_TREATING_OR_FEE_LOCK != null && (o.IS_TREATING_OR_FEE_LOCK==1) == filter.IS_TREATING_OR_FEE_LOCK).ToList();
                }
                //lay loc theo khoa
                if (filter.DEPARTMENT_ID != null)
                {
                    listHisSereServ = listHisSereServ.Where(o => o.DEPARTMENT_ID == filter.DEPARTMENT_ID).ToList();
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
            bool result = false;
            try
            {
                ListRdo = (from r in listHisSereServ select new Mrs00638RDO(r, this.filter)).ToList();
                GroupByKey();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00638RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByKey()
        {
            string errorField = "";
            try
            {
                string KeyGroup = "{0}";
                //khi có điều kiện lọc từ template thì đổi sang key từ template
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_SV") && this.dicDataFilter["KEY_GROUP_SV"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_SV"].ToString()))
                {
                    KeyGroup = this.dicDataFilter["KEY_GROUP_SV"].ToString();
                }
                var group = ListRdo.GroupBy(o => string.Format(KeyGroup,o.DEPARTMENT_ID,o.ROOM_ID,o.TDL_TREATMENT_ID)).ToList();

                Decimal sum = 0;
                Mrs00638RDO rdo;
                List<Mrs00638RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00638RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00638RDO();
                    listSub = item.ToList<Mrs00638RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE") || field.Name.Contains("AMOUNT"))
                        {
                            sum = listSub.Sum(s => ValueOf(field, s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    rdo.COUNT_TREATMENT = listSub.Select(o => o.TDL_TREATMENT_ID).Distinct().Count();
                    if (!hide) ListRdoDepartment.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private decimal ValueOf(PropertyInfo field, Mrs00638RDO s)
        {
            decimal result = 0;
            try
            {
                if (field != null && s != null)
                {
                    var r = field.GetValue(s);
                    if (r != null)
                    {
                        result = (decimal)r;
                    }
                }
            }
             catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        private Mrs00638RDO IsMeaningful(List<Mrs00638RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00638RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO ?? 0));
            dicSingleTag.Add("DEPARTMENT_NAME", (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == filter.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME);
            dicSingleTag.Add("TREATMENT_TYPE_NAME", (HisTreatmentTypeCFG.HisTreatmentTypes.FirstOrDefault(o => o.ID == filter.TREATMENT_TYPE_ID) ?? new HIS_TREATMENT_TYPE()).TREATMENT_TYPE_NAME);
            dicSingleTag.Add("PATIENT_TYPE_NAME", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == filter.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME);
            objectTag.AddObjectData(store, "ReportDetail", ListRdo);
            objectTag.AddObjectData(store, "Report", ListRdoDepartment);
            objectTag.AddRelationship(store, "Report", "ReportDetail", "DEPARTMENT_ID", "DEPARTMENT_ID");
        }

    }

}
