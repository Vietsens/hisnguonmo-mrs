using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisServiceType;
using MOS.MANAGER.HisServiceGroup;
using MOS.MANAGER.HisTreatmentType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSeseDepoRepay;
using MOS.MANAGER.HisServSegr;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServDeposit;
using MOS.MANAGER.HisPatientTypeAlter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00178
{
    internal class Mrs00178Processor : AbstractProcessor
    {
        List<VSarReportMrs00178RDO> _listSarReportMrs00178Rdos = new List<VSarReportMrs00178RDO>();
        List<HIS_SERVICE_GROUP> ListServiceGroup = new List<HIS_SERVICE_GROUP>(); ////
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>(); ///
        List<HIS_TREATMENT_TYPE> ListTreatmentType = new List<HIS_TREATMENT_TYPE>(); ///
        Dictionary<long, V_HIS_TREATMENT> dicTreatment = new Dictionary<long, V_HIS_TREATMENT>();
        Mrs00178Filter CastFilter;

        public Mrs00178Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00178Filter);
        }
        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00178Filter)this.reportFilter;

                //--------------------------------------------------------------------------------------------------
                HisServiceGroupFilterQuery listServiceGroupFilter = new HisServiceGroupFilterQuery(); ///
                listServiceGroupFilter.ID = CastFilter.SERVICE_GROUP_ID;
                ListServiceGroup = new MOS.MANAGER.HisServiceGroup.HisServiceGroupManager(paramGet).Get(listServiceGroupFilter);

                HisPatientTypeFilterQuery listPatientTypeFilter = new HisPatientTypeFilterQuery(); ///
                listPatientTypeFilter.ID = CastFilter.PATIENT_TYPE_ID;
                ListPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(listPatientTypeFilter);

                HisTreatmentTypeFilterQuery listTreatmentTypeFilter = new HisTreatmentTypeFilterQuery(); ///
                listTreatmentTypeFilter.ID = CastFilter.TREATMENT_TYPE_ID;
                ListTreatmentType = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager(paramGet).Get(listTreatmentTypeFilter);

                List<long> listServiceId = new List<long>();
                if (CastFilter.SERVICE_GROUP_ID != null)
                {
                    var metyFilterServSegrView = new HisServSegrViewFilterQuery
                    {
                        SERVICE_GROUP_ID = CastFilter.SERVICE_GROUP_ID
                    };
                    var listServSegr = new MOS.MANAGER.HisServSegr.HisServSegrManager(paramGet).GetView(metyFilterServSegrView);

                    listServiceId = listServSegr.Select(o => o.SERVICE_ID).Distinct().ToList();
                }
                else
                {
                    throw new NullReferenceException(" Chua co filter CastFilter.SERVICE_GROUP_ID!");
                }

                //---------------------------------------------------------------------------------------HIS_SERE_SERV
                //yeu cau
                var ListServiceReq = new List<HIS_SERVICE_REQ>();
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.INTRUCTION_TIME_FROM = CastFilter.TIME_FROM;
                reqFilter.INTRUCTION_TIME_TO = CastFilter.TIME_TO;
                ListServiceReq = new HisServiceReqManager().Get(reqFilter);

                var listSereservs = new List<HIS_SERE_SERV>();

                //sere serv
                if (IsNotNullOrEmpty(listServiceId))
                {
                    var skip = 0;
                    while (listServiceId.Count - skip > 0)
                    {
                        var listIDs = listServiceId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.SERVICE_IDs = listIDs;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                        if (IsNotNullOrEmpty(listSereServSub))
                            listSereservs.AddRange(listSereServSub);
                    }
                    listSereservs = listSereservs.Where(o => ListServiceReq.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();
                }
                var treatmentIds = listSereservs.Select(o => o.TDL_TREATMENT_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    var listSereServDeposit = new List<HIS_SERE_SERV_DEPOSIT>();
                    var listSeseDepoRepay = new List<HIS_SESE_DEPO_REPAY>();
                    var listSereServBill = new List<HIS_SERE_SERV_BILL>();

                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServDepositFilterQuery HisSereServDepositfilter = new HisSereServDepositFilterQuery();
                        HisSereServDepositfilter.TDL_TREATMENT_IDs = treatmentIds;
                        var listSereServDepositSub = new HisSereServDepositManager(paramGet).Get(HisSereServDepositfilter);
                        if (IsNotNullOrEmpty(listSereServDepositSub))
                            listSereServDeposit.AddRange(listSereServDepositSub);

                        HisSeseDepoRepayFilterQuery HisSeseDepoRepayfilter = new HisSeseDepoRepayFilterQuery();
                        HisSeseDepoRepayfilter.TDL_TREATMENT_IDs = treatmentIds;
                        var listSeseDepoRepaySub = new HisSeseDepoRepayManager(paramGet).Get(HisSeseDepoRepayfilter);
                        if (IsNotNullOrEmpty(listSeseDepoRepaySub))
                            listSeseDepoRepay.AddRange(listSeseDepoRepaySub);

                        HisSereServBillFilterQuery HisSereServBillfilter = new HisSereServBillFilterQuery();
                        HisSereServBillfilter.TDL_TREATMENT_IDs = treatmentIds;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(HisSereServBillfilter);
                        if (IsNotNullOrEmpty(listSereServBillSub))
                            listSereServBill.AddRange(listSereServBillSub);
                    }


                    //DV tm đk: 
                    //- DV da thanh toan
                    listSereservs = listSereservs.Where(o => listSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) ||(listSereServDeposit.Select(p => p.SERE_SERV_ID).Contains(o.ID)&&listSeseDepoRepay.Where(q => q.TDL_SERE_SERV_PARENT_ID == o.ID).ToList().Count() == 0)).ToList();
                    // - DV chua thuc hien
                    listSereservs = listSereservs.Where(o => ListServiceReq.Exists(p=>p.ID==o.SERVICE_REQ_ID&&p.SERVICE_REQ_STT_ID ==IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL)).ToList();
                }

                var listTreatmentIds = listSereservs.Select(s => s.TDL_TREATMENT_ID ?? 0).Distinct().ToList();

                //---------------------------------------------------------------------------------------V_HIS_TREATMENT
                var listTreatments = new List<HIS_TREATMENT>();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var metyFilterTreatment = new HisTreatmentFilterQuery
                        {
                            IDs = listIDs
                        };
                        var listTreatment = new HisTreatmentManager(paramGet).Get(metyFilterTreatment);
                        listTreatments.AddRange(listTreatment);
                    }
                }
                //---------------------------------------------------------------------------------------V_HIS_PATIENT_TYPE_ALTER
                var listPatientTypeAlters = new List<HIS_PATIENT_TYPE_ALTER>();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var listIDs = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
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

                ProcessFilterData(listSereservs, listTreatments,listPatientTypeAlters );
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

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_TO));
            if (ListServiceGroup.Count > 0) dicSingleTag.Add("SERVICE_GROUP_NAME", ListServiceGroup.First().SERVICE_GROUP_NAME);
            if (ListTreatmentType.Count > 0) dicSingleTag.Add("TREATMENT_TYPE_NAME", ListTreatmentType.First().TREATMENT_TYPE_NAME);
            if (ListPatientType.Count > 0) dicSingleTag.Add("PATIENT_TYPE_NAME", ListPatientType.First().PATIENT_TYPE_NAME);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00178Rdos);

        }


        private void ProcessFilterData(List<HIS_SERE_SERV> listSereservs,
            List<HIS_TREATMENT> listTreatments, List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters)
        {
            try
            {

                if (IsNotNullOrEmpty(listSereservs))
                {
                    var Groups = listSereservs.GroupBy(s => s.SERVICE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<HIS_SERE_SERV>();
                        var listGroupTreatment = listSub.GroupBy(s => s.TDL_TREATMENT_ID).ToList();
                        foreach (var item in listGroupTreatment)
                        {
                            var treatment = listTreatments.FirstOrDefault(o => o.ID == item.Key) ?? new HIS_TREATMENT();
                            var listSubTreatment = item.ToList<HIS_SERE_SERV>();
                            var rdo = new VSarReportMrs00178RDO
                            {
                                SERVICE_NAME = listSub.First().TDL_SERVICE_NAME,
                                SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == listSub.First().TDL_SERVICE_UNIT_ID)??new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME,
                                TOTAL_AMOUNT = listSub.Count,
                                VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME,
                                AMOUNT = listSubTreatment.Count
                            };
                            _listSarReportMrs00178Rdos.Add(rdo);
                        };
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
/*
 
*/