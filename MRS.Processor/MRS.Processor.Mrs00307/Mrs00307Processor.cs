using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using MRS.MANAGER.Config;
using FlexCel.Report;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Core.MrsReport.RDO;

namespace MRS.Processor.Mrs00307
{
    public class Mrs00307Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();

        private List<Mrs00307RDO> ListRdo = new List<Mrs00307RDO>();
        private List<Mrs00307RDO> ListOutTime = new List<Mrs00307RDO>();
        private List<V_HIS_TREATMENT_4> ListTreatment = new List<V_HIS_TREATMENT_4>();
        private List<V_HIS_PATIENT_TYPE_ALTER> ListPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<MOS.EFMODEL.DataModels.HIS_ICD> icds = new List<HIS_ICD>();
        private int CountTreatment = 0;

        public Mrs00307Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00307Filter);
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00307Filter)reportFilter);
            var result = true;
            try
            {
                //HSDT chuyen tuyen
                HisTreatmentView4FilterQuery filtermain = new HisTreatmentView4FilterQuery();
                filtermain.OUT_TIME_FROM = filter.TIME_FROM;
                filtermain.OUT_TIME_TO = filter.TIME_TO;
                filtermain.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;
                ListTreatment = new HisTreatmentManager(paramGet).GetView4(filtermain);
                var ListTreatmentId = ListTreatment.Select(o => o.ID).ToList();

                //doi tuong dieu tri
                if (IsNotNullOrEmpty(ListTreatmentId))
                {
                    var skip = 0;
                    while (ListTreatmentId.Count - skip > 0)
                    {
                        var listIDs = ListTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery patientTypeAlterFilter = new HisPatientTypeAlterViewFilterQuery()
                        {
                            TREATMENT_IDs = listIDs
                        };
                        var LisPatientTypeAlterLib = new HisPatientTypeAlterManager(paramGet).GetView(patientTypeAlterFilter);
                        ListPatientTypeAlter.AddRange(LisPatientTypeAlterLib);
                    }
                }
                //var LisPatientTypeAlterId = ListPatientTypeAlter.Select(o => o.ID).ToList();

                icds = new HisIcdManager(paramGet).Get(new HisIcdFilterQuery());
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
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    ListRdo.Clear();
                    ListOutTime.Clear();
                    foreach (var tranPati in ListTreatment)
                    {
                        Mrs00307RDO rdo = new Mrs00307RDO();
                        rdo.TREATMENT_CODE = tranPati.TREATMENT_CODE;
                        var listPatientTypeAlterSub = ListPatientTypeAlter.Where(o => o.TREATMENT_ID == tranPati.ID && o.HEIN_CARD_NUMBER != null).ToList();
                        rdo.HEIN_CARD_NUMBER = tranPati.TDL_HEIN_CARD_NUMBER;// IsNotNullOrEmpty(listPatientTypeAlterSub) ? listPatientTypeAlterSub.OrderByDescending(o => o.LOG_TIME).FirstOrDefault().HEIN_CARD_NUMBER : "";
                        rdo.MEDI_ORG_NAME = tranPati.MEDI_ORG_NAME;
                        CalcuatorAge(ref rdo, tranPati);
                        rdo.PATIENT_NAME = tranPati.TDL_PATIENT_NAME;
                        rdo.OUT_TIME = tranPati.OUT_TIME;
                        rdo.ADDRESS = tranPati.TDL_PATIENT_ADDRESS;
                        rdo.GENDER_NAME = tranPati.TDL_PATIENT_GENDER_NAME;
                        rdo.END_USERNAME = tranPati.END_USERNAME;
                        rdo.OUT_DATE = Inventec.Common.TypeConvert.Parse.ToInt64(((rdo.OUT_TIME ?? 0).ToString().Substring(0, 8)));
                        rdo.OUT_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(rdo.OUT_TIME ?? 0);
                        if (String.IsNullOrEmpty(tranPati.ICD_CODE))
                        {
                            var checkIcd = icds.FirstOrDefault(o => o.ICD_CODE == tranPati.ICD_CODE);
                            if (checkIcd != null)
                            {
                                rdo.ICD_NAME = checkIcd.ICD_NAME;
                                rdo.ICD_CODE = checkIcd.ICD_CODE;
                            }
                        }

                        CountTreatment++;
                        rdo.NUM_ORDER = CountTreatment;
                        ListRdo.Add(rdo);
                    }
                    ListOutTime = ListRdo.GroupBy(p => new { p.OUT_DATE }).Select(g => g.First()).OrderBy(o => o.OUT_DATE).ToList();
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void CalcuatorAge(ref Mrs00307RDO rdo, V_HIS_TREATMENT_4 treatment)
        {
            try
            {
                int? tuoi = RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        rdo.AGE = (tuoi >= 1) ? tuoi : 1;
                    }
                    else
                    {
                        rdo.AGE = (tuoi >= 1) ? tuoi : 1;
                    }
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00307Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00307Filter)reportFilter).TIME_FROM));
            }
            if (((Mrs00307Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00307Filter)reportFilter).TIME_TO));
            }
            bool exportSuccess = true;
            dicSingleTag.Add("SUM_TREATMENT", CountTreatment);
            objectTag.AddObjectData(store, "ListOutTime", ListOutTime);
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddRelationship(store, "ListOutTime", "Report", "OUT_DATE", "OUT_DATE");
            exportSuccess = exportSuccess && store.SetCommonFunctions();
        }
    }
}
