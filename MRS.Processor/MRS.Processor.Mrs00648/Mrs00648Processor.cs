using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00648
{
    class Mrs00648Processor : AbstractProcessor
    {
        Mrs00648Filter castFilter = null;
        List<Mrs00648RDO> ListRdo = new List<Mrs00648RDO>();

        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();
        List<HIS_ICD> ListIcd = new List<HIS_ICD>();

        public Mrs00648Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00648Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = (Mrs00648Filter)this.reportFilter;

                HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                filter.IN_TIME_FROM = castFilter.TIME_FROM;
                filter.IN_TIME_TO = castFilter.TIME_TO;
                if (castFilter.IS_NOT_ACTIVE == true)
                {
                    filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                }
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetView(filter);

                ListIcd = new MOS.MANAGER.HisIcd.HisIcdManager().Get(new HisIcdFilterQuery());
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListTreatment) && IsNotNullOrEmpty(ListIcd))
                {
                    List<long> treatmentIds = ListTreatment.Select(s => s.ID).ToList();

                    HisDepartmentTranViewFilterQuery filter = new HisDepartmentTranViewFilterQuery();
                    filter.TREATMENT_IDs = treatmentIds;
                    filter.DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;

                    List<V_HIS_DEPARTMENT_TRAN> listDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager().GetView(filter);

                    Dictionary<long, V_HIS_DEPARTMENT_TRAN> dicDepartmentTran = new Dictionary<long, V_HIS_DEPARTMENT_TRAN>();
                    if (IsNotNullOrEmpty(listDepartmentTran))
                    {
                        //sắp xếp lấy bản tin vào khoa đầu tiên
                        listDepartmentTran = listDepartmentTran.OrderByDescending(o => o.DEPARTMENT_IN_TIME ?? 99999999999999).ToList();
                        foreach (var item in listDepartmentTran)
                        {
                            dicDepartmentTran[item.TREATMENT_ID] = item;
                        }
                    }

                    foreach (var item in ListTreatment)
                    {
                        if (!dicDepartmentTran.ContainsKey(item.ID)) continue;

                        Mrs00648RDO rdo = new Mrs00648RDO(item);
                        var icd = ListIcd.FirstOrDefault(o => o.ICD_CODE == item.ICD_CODE);
                        if (icd != null)
                        {
                            rdo.ICD = icd;
                        }

                        rdo.DEPARTMENT_IN_CODE = dicDepartmentTran[item.ID].DEPARTMENT_CODE;
                        rdo.DEPARTMENT_IN_ID = dicDepartmentTran[item.ID].DEPARTMENT_ID;
                        rdo.DEPARTMENT_IN_NAME = dicDepartmentTran[item.ID].DEPARTMENT_NAME;

                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                ListRdo.Clear();
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                var ListDepartment = ListRdo.GroupBy(g => g.DEPARTMENT_IN_ID).Select(s => s.First()).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "ReportDepartment", ListDepartment);
                objectTag.AddRelationship(store, "ReportDepartment", "Report", "DEPARTMENT_IN_ID", "DEPARTMENT_IN_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
