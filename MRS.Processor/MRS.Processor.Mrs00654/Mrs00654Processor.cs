using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisBranch;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00654
{
    class Mrs00654Processor : AbstractProcessor
    {
        List<Mrs00654RDO> ListRdo = new List<Mrs00654RDO>();
        Mrs00654Filter castFilter;
        List<V_HIS_TREATMENT> ListTreatment;
        List<HIS_PATIENT> ListPatient;

        //lấy số lần in
        const string PrintTypeCode = "Mps000298";

        public Mrs00654Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00654Filter);
        }

        protected override bool GetData()
        {
            castFilter = (Mrs00654Filter)reportFilter;
            bool result = true;
            try
            {
                HisTreatmentViewFilterQuery treatFilter = new HisTreatmentViewFilterQuery();
                treatFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatFilter.OUT_TIME_TO = castFilter.TIME_TO;

                var treatments = new HisTreatmentManager().GetView(treatFilter);
                if (treatments != null && treatments.Count > 0)
                {
                    ListTreatment = treatments.Where(o => o.TREATMENT_END_TYPE_EXT_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE_EXT.ID__NGHI_OM).ToList();
                }

                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ListPatient = new List<HIS_PATIENT>();
                    var lstPatient = ListTreatment.Select(o => o.PATIENT_ID).Distinct().ToList();
                    int skip = 0;
                    while (lstPatient.Count - skip > 0)
                    {
                        var listId = lstPatient.Skip(skip).Take(MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += MANAGER.Base.ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientFilterQuery patFilter = new HisPatientFilterQuery();
                        patFilter.IDs = listId;
                        var patient = new HisPatientManager().Get(patFilter);
                        if (IsNotNullOrEmpty(patient))
                        {
                            ListPatient.AddRange(patient);
                        }
                    }
                }
                Inventec.Common.Logging.LogSystem.Info("ListTreatment: " + ListTreatment.Count);
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
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    var ListPrintlog = ManagerSql.GetPrintLog(PrintTypeCode);
                    foreach (var item in ListTreatment)
                    {
                        Mrs00654RDO rdo = new Mrs00654RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00654RDO>(rdo, item);
                        rdo.IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(item.IN_TIME);
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeStringWithoutSecond(item.OUT_TIME ?? 0);

                        rdo.DOB_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.TDL_PATIENT_DOB);
                        rdo.SICK_LEAVE_FROM_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.SICK_LEAVE_FROM ?? 0);
                        rdo.SICK_LEAVE_TO_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.SICK_LEAVE_TO ?? 0);

                        HIS_PATIENT patient = ListPatient != null ? ListPatient.FirstOrDefault(o => o.ID == item.PATIENT_ID) : null;
                        if (IsNotNull(patient))
                        {
                            rdo.FATHER_NAME = patient.FATHER_NAME;
                            rdo.MOTHER_NAME = patient.MOTHER_NAME;
                        }

                        //api trả về decimal đc json có chứa phần thập phân (.0)
                        //báo cáo lấy trực tiếp db sẽ không có phần thập phân (.0) nên uniqueCode sẽ bị khác.
                        string leaveDay = Newtonsoft.Json.JsonConvert.SerializeObject(item.SICK_LEAVE_DAY);
                        leaveDay = leaveDay.Replace('.', ',');
                        String uniqueCode = String.Format("{0}_{1}_{2}_{3}_{4}_{5}", PrintTypeCode, item.TREATMENT_CODE, item.IN_TIME, leaveDay, item.TREATMENT_END_TYPE_ID, item.TREATMENT_RESULT_ID);
                        Inventec.Common.Logging.LogSystem.Debug("uniqueCode:" + uniqueCode);

                        if (IsNotNullOrEmpty(ListPrintlog))
                        {
                            var lstlog = ListPrintlog.Where(o => o.UNIQUE_CODE == uniqueCode).ToList();
                            if (IsNotNullOrEmpty(lstlog))
                            {
                                rdo.CURRENT_NUM_ORDER_PRINT = lstlog.Max(m => m.NUM_ORDER);
                            }
                        }

                        ListRdo.Add(rdo);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));

                var branch = new HisBranchManager().GetById(this.branch_id)??new HIS_BRANCH();
                dicSingleTag.Add("MEDI_ORG_CODE", branch.HEIN_MEDI_ORG_CODE);

                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.EXTRA_END_CODE).ThenBy(p=>p.SICK_LEAVE_FROM).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
