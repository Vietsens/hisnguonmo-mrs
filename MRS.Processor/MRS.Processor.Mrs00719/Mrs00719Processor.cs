using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using Inventec.Common.FlexCellExport;
using Inventec.Common.DateTime;
using Inventec.Common.Number;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServExt;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Core.MrsReport.RDO;
using MRS.Processor.Mrs00719;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00719
{
    public class Mrs00719Processor : AbstractProcessor
    {
        Mrs00719Filter castFilter = null;
        
        List<Mrs00719RDO> ListRdo = new List<Mrs00719RDO>();
        CommonParam paramGet = new CommonParam();
        public Mrs00719Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00719Filter);
        }

        protected override bool GetData()
        {
            
            var result = true;
            castFilter = (Mrs00719Filter)reportFilter;
            try
            {
                ListRdo = new MRS.Processor.Mrs00719.ManagerSql().GetMachine(castFilter);
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
                if (IsNotNullOrEmpty(ListRdo))
                {
                    Dictionary<string, Mrs00719RDO> dicRdo = new Dictionary<string, Mrs00719RDO>();
                    foreach (var item in ListRdo)
                    {
                        Mrs00719RDO rdo = new Mrs00719RDO();
                        if (!dicRdo.ContainsKey(item.MACHINE_CODE))
                        {
                            rdo.DEPARTMENT_CODE = item.DEPARTMENT_CODE;
                            rdo.DEPARTMENT_NAME = item.DEPARTMENT_NAME;
                            rdo.MACHINE_CODE = item.MACHINE_CODE;
                            rdo.MACHINE_NAME = item.MACHINE_NAME;
                            rdo.MACHINE_PRICE = item.MACHINE_PRICE;
                            rdo.PATIENT_TYPE = item.PATIENT_TYPE;
                        }
                        else
                        {
                            dicRdo[item.MACHINE_CODE].MACHINE_PRICE += item.MACHINE_PRICE;
                        }

                        if (!dicRdo[item.MACHINE_CODE].DIC_PATIENT_TYPE.ContainsKey(item.PATIENT_TYPE))
                        {
                            dicRdo[item.MACHINE_CODE].DIC_PATIENT_TYPE[item.PATIENT_TYPE] = Inventec.Common.TypeConvert.Parse.ToInt32(item.MACHINE_PRICE.ToString());
                        }
                        else
                        {
                            dicRdo[item.MACHINE_CODE].DIC_PATIENT_TYPE[item.PATIENT_TYPE] += Inventec.Common.TypeConvert.Parse.ToInt32(item.MACHINE_PRICE.ToString());
                        }
                    }
                    ListRdo = dicRdo.Values.ToList();
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
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO ?? 0));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.DEPARTMENT_CODE).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "DEPARTMENT_CODE", "DEPARTMENT_CODE");
        }
    }
}
