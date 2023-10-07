using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceGroup;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServSegr;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.SDO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00174
{
    internal class Mrs00174Processor : AbstractProcessor
    {
        List<VSarReportMrs00174RDO> listRdo = new List<VSarReportMrs00174RDO>();
        List<HIS_SERVICE_GROUP> ListServiceGroup = new List<HIS_SERVICE_GROUP>(); ////
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>(); ///
        List<HIS_TREATMENT_TYPE> ListTreatmentType = new List<HIS_TREATMENT_TYPE>(); ///
        Mrs00174Filter CastFilter;

        public Mrs00174Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00174Filter);
        }
        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00174Filter)this.reportFilter;

                HisServiceGroupFilterQuery listServiceGroupFilter = new HisServiceGroupFilterQuery(); ///
                listServiceGroupFilter.ID = CastFilter.SERVICE_GROUP_ID;
                ListServiceGroup = new HisServiceGroupManager(paramGet).Get(listServiceGroupFilter);

                HisPatientTypeFilterQuery listPatientTypeFilter = new HisPatientTypeFilterQuery(); ///
                listPatientTypeFilter.ID = CastFilter.PATIENT_TYPE_ID;
                ListPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(listPatientTypeFilter);

                HisTreatmentTypeFilterQuery listTreatmentTypeFilter = new HisTreatmentTypeFilterQuery(); ///
                listTreatmentTypeFilter.ID = CastFilter.TREATMENT_TYPE_ID;
                ListTreatmentType = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager(paramGet).Get(listTreatmentTypeFilter);
                //--------------------------------------------------------------------------------------------------

                //select
                List<long> listServiceId = new List<long>();
                if (CastFilter.SERVICE_GROUP_ID != null)
                {
                    var metyFilterServSegr = new HisServSegrFilterQuery
                    {
                        SERVICE_GROUP_ID = CastFilter.SERVICE_GROUP_ID
                    };
                    var listServSegr = new MOS.MANAGER.HisServSegr.HisServSegrManager(paramGet).Get(metyFilterServSegr);

                    listServiceId = listServSegr.Select(o => o.SERVICE_ID).Distinct().ToList();

                }
                //---------------------------------------------------------------------------------------V_HIS_SERE_SERV
                var listSereservs = new List<HIS_SERE_SERV>();

                var metyFilterSereserv = new HisSereServFilterQuery();
                {
                    metyFilterSereserv.TDL_INTRUCTION_TIME_FROM = CastFilter.INSTRUCT_TIME_FROM;
                    metyFilterSereserv.TDL_INTRUCTION_TIME_TO = CastFilter.INSTRUCT_TIME_TO;
                }
                listSereservs = new HisSereServManager(paramGet).Get(metyFilterSereserv);
                if (listSereservs != null) listSereservs = listSereservs.Where(o => listServiceId.Contains(o.SERVICE_ID)).ToList();

                var listTreatmentIDs = listSereservs.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                //---------------------------------------------------------------------------------------HIS_TREATMENT
                var listTreatments = new List<HIS_TREATMENT>();
                var skip = 0;
                while (listTreatmentIDs.Count - skip > 0)
                {
                    var listIDs = listTreatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterTreatment = new HisTreatmentFilterQuery
                    {
                        IDs = listIDs
                    };
                    var listTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                    listTreatments.AddRange(listTreatment);
                }
                //---------------------------------------------------------------------------------------HIS_PATIENT_TYPE_ALTER
                var listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
                skip = 0;
                while (listTreatmentIDs.Count - skip > 0)
                {
                    var listIDs = listTreatmentIDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterPatientTypeAlter = new HisPatientTypeAlterFilterQuery
                    {
                        TREATMENT_IDs = listIDs,
                        ORDER_DIRECTION = "ASC",
                        ORDER_FIELD = "ID"
                    };
                    var listPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).Get(metyFilterPatientTypeAlter);
                    listPatientTypeAlters.AddRange(listPatientTypeAlter);
                }
                listPatientTypeAlters = listPatientTypeAlters.OrderBy(q => q.LOG_TIME).GroupBy(o => o.TREATMENT_ID).Select(p => p.Last()).ToList();

                if (CastFilter.TREATMENT_TYPE_ID != null)
                {
                    listPatientTypeAlters = listPatientTypeAlters.Where(o => o.TREATMENT_TYPE_ID == CastFilter.TREATMENT_TYPE_ID).ToList();
                }
                if (CastFilter.PATIENT_TYPE_ID != null)
                {
                    listPatientTypeAlters = listPatientTypeAlters.Where(o => o.PATIENT_TYPE_ID == CastFilter.PATIENT_TYPE_ID).ToList();
                }

                var listPatientTypeAlterIds = listPatientTypeAlters.Select(s => s.ID).ToList();
                listTreatments = listTreatments.Where(o => o.ID != null && listPatientTypeAlters.Select(p => p.TREATMENT_ID).Contains(o.ID)).ToList();



                ProcessFilterData(listPatientTypeAlters, listSereservs, listTreatments);


                result = true;

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
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.INSTRUCT_TIME_FROM.Value));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.INSTRUCT_TIME_TO.Value));
            if (ListServiceGroup.Count > 0) dicSingleTag.Add("SERVICE_GROUP_NAME", ListServiceGroup.First().SERVICE_GROUP_NAME);
            if (ListTreatmentType.Count > 0) dicSingleTag.Add("TREATMENT_TYPE_NAME", ListTreatmentType.First().TREATMENT_TYPE_NAME);
            if (ListPatientType.Count > 0) dicSingleTag.Add("PATIENT_TYPE_NAME", ListPatientType.First().PATIENT_TYPE_NAME);

            objectTag.AddObjectData(store, "Report", listRdo);

        }

        private void ProcessFilterData(List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters, List<HIS_SERE_SERV> listSereservs, List<HIS_TREATMENT> listTreatments)
        {
            try
            {
                foreach (var treatment in listTreatments)
                {
                    var listSereserv = listSereservs.Where(s => s.TDL_TREATMENT_ID == treatment.ID).ToList() ?? new List<HIS_SERE_SERV>();
                    var groupByTreatmentAndService = listSereserv.GroupBy(s => new { s.SERVICE_ID, s.TDL_TREATMENT_ID, s.PRICE }).ToList();
                    foreach (var item in groupByTreatmentAndService)
                    {
                        List<HIS_SERE_SERV> listSub = item.ToList<HIS_SERE_SERV>();
                        var patientTypeAlter = listPatientTypeAlters.FirstOrDefault(o => o.TREATMENT_ID == treatment.ID) ?? new HIS_PATIENT_TYPE_ALTER();
                        VSarReportMrs00174RDO rdo = new VSarReportMrs00174RDO
                        {
                            HEIN_CARD_NUMBER = patientTypeAlter.HEIN_CARD_NUMBER,
                            PATIENT_CODE = treatment.TDL_PATIENT_CODE,
                            VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME,
                            TREATMENT_CODE = treatment.TREATMENT_CODE,
                            AMOUNT = listSub.Sum(o => o.AMOUNT),
                            PRICE = item.Key.PRICE,
                            TOTAL_PRICE = listSub.Sum(o => o.AMOUNT) * item.Key.PRICE,
                            DV = listSub.First().TDL_SERVICE_NAME
                        };
                        listRdo.Add(rdo);
                    }
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }
    }
}
