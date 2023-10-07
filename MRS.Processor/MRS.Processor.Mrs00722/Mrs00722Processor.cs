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

namespace MRS.Processor.Mrs00722
{
    class Mrs00722Processor : AbstractProcessor
    {
        Mrs00722Filter castFilter = null;
        List<ManagerSql.SERVICE_REQ> ListData = new List<ManagerSql.SERVICE_REQ>();
        List<Mrs00722RDO> ListRdo = new List<Mrs00722RDO>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();

        CommonParam paramGet = new CommonParam();
        string title = "";

        public Mrs00722Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00722Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            castFilter = (Mrs00722Filter)reportFilter;
            try
            {
                ListData = new MRS.Processor.Mrs00722.ManagerSql().GetDataServiceReq(castFilter) ?? new List<ManagerSql.SERVICE_REQ>();

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
                    var service = ListData.Select(p => p.MA_DV).Distinct().ToList();
                    if (service.Count() > 0)
                    {
                        foreach (var item in service)
                        {
                           
                            var checkData = ListData.Where(p => p.MA_DV == item).FirstOrDefault();
                            
                            
                            Mrs00722RDO rdo = new Mrs00722RDO();
                            rdo.IN_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(checkData.TGIAN_VAO);
                            rdo.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(checkData.TGIAN_CHIDINH ?? 0);
                            rdo.PATIENT_CODE = checkData.MA_BN;
                            rdo.PATIENT_NAME = checkData.TEN_BN;
                            rdo.PATIENT_GENDER = checkData.GIOI_TINH == 1 ? "Nam" : "Nữ";
                            rdo.PATIENT_DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(checkData.NGAY_SINH);
                            rdo.PATIENT_ADDRESS = checkData.DIA_CHI;
                            if (checkData.MA_KP_CHIDINH != null)
                            {
                                rdo.REQUEST_DEPARTMENT_NAME = checkData.TEN_KP;
                            }
                            rdo.REQUEST_USERNAME = checkData.NGUOI_CHIDINH;
                            rdo.EXECUTE_USERNAME = checkData.NGUOI_THUCHIEN;
                            if (checkData.LOAI_BN != null)
                            {
                                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == checkData.LOAI_BN) ?? new HIS_PATIENT_TYPE();
                                rdo.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                                rdo.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            }
                            
                            rdo.ICD_DIIM = checkData.ICD + ";" + checkData.CHAN_DOAN;
                            rdo.DIIM_RESULT = checkData.KET_LUAN;
                            rdo.SERVICE_NAME = checkData.TEN_DV;
                            rdo.AMOUNT = checkData.SO_LUONG ?? 0;
                            rdo.PRICE = checkData.GIA ?? 0;

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
            
            
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "Parent", ListRdo.GroupBy(o => o.SERVICE_NAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_NAME", "SERVICE_NAME");
        }
        
    }
}
