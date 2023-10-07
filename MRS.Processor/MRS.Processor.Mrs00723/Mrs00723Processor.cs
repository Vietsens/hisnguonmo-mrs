using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00723
{
    class Mrs00723Processor : AbstractProcessor
    {
        Mrs00723Filter castFilter = null;
        List<ManagerSql.TREATMENT> ListData = new List<ManagerSql.TREATMENT>();
        List<Mrs00723RDO> ListRdo = new List<Mrs00723RDO>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();

        CommonParam paramGet = new CommonParam();
        string title = "";

        public Mrs00723Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00723Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00723Filter)reportFilter;
            try
            {
                ListData = new MRS.Processor.Mrs00723.ManagerSql().GetDataTreatment(castFilter) ?? new List<ManagerSql.TREATMENT>();

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
                if (IsNotNullOrEmpty(ListData))
                {
                    var patient = ListData.Select(p => p.ID_DT).Distinct().ToList();
                    if (patient.Count() > 0)
                    {
                        foreach (var item in patient)
                        {
                            var checkData = ListData.FirstOrDefault(p => p.ID_DT == item);
                            Mrs00723RDO rdo = new Mrs00723RDO();
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(checkData.TGIAN_VAO);
                            rdo.TREATMENT_CODE = checkData.MA_DT;
                            rdo.PATIENT_NAME = checkData.TEN_BN;
                            if (checkData.NGAY_SINH != null)
                                rdo.PATIENT_DOB = checkData.NGAY_SINH.ToString().Substring(0, 4);
                            rdo.PATIENT_CAREER = checkData.NGHE_NGHIEP;
                            if (checkData.LOAI_BN != null)
                            {
                                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(p => p.ID == checkData.LOAI_BN);
                                rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;

                            }
                            rdo.HEIN_CARD_NUMBER = checkData.SO_THE;
                            if (checkData.ID_KP != null)
                                rdo.DEPARTMENT_NAME = checkData.TEN_KP;
                            ListRdo.Add(rdo);

                        }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
            
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));

            if (IsNotNullOrEmpty(ListData))
            {
                var patient = ListData.Select(p => p.ID_DT).Distinct().ToList();
                if(patient.Count() > 0)
                    dicSingleTag.Add("TOTAL_PATIENT", patient.Count());
                else
                    dicSingleTag.Add("TOTAL_PATIENT", 0);
            }
            else
                dicSingleTag.Add("TOTAL_PATIENT", 0);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.DEPARTMENT_NAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "DEPARTMENT_NAME", "DEPARTMENT_NAME");
        }
        
    }
}
