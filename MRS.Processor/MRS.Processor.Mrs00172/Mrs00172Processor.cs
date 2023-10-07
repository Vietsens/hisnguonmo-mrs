using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatient;
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
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00172
{
    internal class Mrs00172Processor : AbstractProcessor
    {
        List<VSarReportMrs00172RDO> _listSarReportMrs00172Rdos = new List<VSarReportMrs00172RDO>();
        List<HIS_SERVICE_GROUP> ListServiceGroup = new List<HIS_SERVICE_GROUP>(); ////
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>(); ///
        List<HIS_TREATMENT_TYPE> ListTreatmentType = new List<HIS_TREATMENT_TYPE>(); ///
        Mrs00172Filter CastFilter;
        List<long> listServiceId = new List<long>();
        public Mrs00172Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00172Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00172Filter)this.reportFilter;

                HisServiceGroupFilterQuery listServiceGroupFilter = new HisServiceGroupFilterQuery(); ///
                listServiceGroupFilter.ID = CastFilter.SERVICE_GROUP_ID;
                ListServiceGroup = new MOS.MANAGER.HisServiceGroup.HisServiceGroupManager(paramGet).Get(listServiceGroupFilter);

                HisPatientTypeFilterQuery listPatientTypeFilter = new HisPatientTypeFilterQuery(); ///
                listPatientTypeFilter.ID = CastFilter.PATIENT_TYPE_ID;
                ListPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(listPatientTypeFilter);

                HisTreatmentTypeFilterQuery listTreatmentTypeFilter = new HisTreatmentTypeFilterQuery(); ///
                listTreatmentTypeFilter.ID = CastFilter.TREATMENT_TYPE_ID;
                ListTreatmentType = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager(paramGet).Get(listTreatmentTypeFilter);

                if (CastFilter.SERVICE_GROUP_ID != null)
                {
                    var metyFilterServSegrView = new HisServSegrFilterQuery
                    {
                        SERVICE_GROUP_ID = CastFilter.SERVICE_GROUP_ID
                    };
                    var listServSegr = new MOS.MANAGER.HisServSegr.HisServSegrManager(paramGet).Get(metyFilterServSegrView);

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
                reqFilter.FINISH_TIME_FROM = CastFilter.DATE_FROM;
                reqFilter.FINISH_TIME_TO = CastFilter.DATE_TO;
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

                    //DV tm ðk: 
                    listSereservs = listSereservs.Where(o => !((o.HEIN_RATIO ?? 0) < 1 && (!listSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID)) && ((!(listSereServDeposit.Select(p => p.SERE_SERV_ID).Contains(o.ID))) || listSeseDepoRepay.Where(q => q.TDL_SERE_SERV_PARENT_ID == o.ID).ToList().Count() > 0))).ToList();
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

                ProcessFilterData(listPatientTypeAlters, listTreatments, listSereservs);
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
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM ?? 0));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO ?? 0));
            if (ListServiceGroup.Count > 0) dicSingleTag.Add("SERVICE_GROUP_NAME", ListServiceGroup.First().SERVICE_GROUP_NAME);
            if (ListTreatmentType.Count > 0) dicSingleTag.Add("TREATMENT_TYPE_NAME", ListTreatmentType.First().TREATMENT_TYPE_NAME);
            if (ListPatientType.Count > 0) dicSingleTag.Add("PATIENT_TYPE_NAME", ListPatientType.First().PATIENT_TYPE_NAME);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00172Rdos);
        }

        private void ProcessFilterData(List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters,
            List<HIS_TREATMENT> listTreatments, List<HIS_SERE_SERV> listSereservs)
        {
            try
            {
                var listSereserv = listSereservs.Where(s => (s.HEIN_RATIO ?? 0) != 1 /*HisServiceTypeCFG.SERVICE_TYPE_ID__HEIN_RATIO_0*/
                         && listPatientTypeAlters.Exists(o => o.TREATMENT_ID == s.TDL_TREATMENT_ID)).ToList();

                if (IsNotNullOrEmpty(listSereserv))
                {
                    var Groups = listSereserv.GroupBy(s => s.SERVICE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<HIS_SERE_SERV>();
                        var listGroupTreatment = listSub.GroupBy(g => g.TDL_TREATMENT_ID).ToList();
                        foreach (var xxx in listGroupTreatment)
                        {
                            var treatment = listTreatments.FirstOrDefault(o => o.ID == xxx.Key) ?? new HIS_TREATMENT();
                            var listSubTreatment = xxx.ToList<HIS_SERE_SERV>();
                            var rdo = new VSarReportMrs00172RDO
                            {
                                DV = listSub.First().TDL_SERVICE_NAME,
                                VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME,
                                TREATMENT_CODE = treatment.TREATMENT_CODE,
                                AMOUNT = listSubTreatment.Select(o => o.AMOUNT).Sum(),
                                PRICE = listSubTreatment.First().PRICE,
                                TOTAL_PRICE = listSubTreatment.Select(o => o.PRICE * o.AMOUNT).Sum(),
                                PATIENT_CODE = treatment.TDL_PATIENT_CODE,
                                VIR_PATIENT_PRICE = listSubTreatment.Select(o => o.VIR_PATIENT_PRICE).Sum(),
                                HEIN_CARD_NUMBER = listSubTreatment.First().HEIN_CARD_NUMBER
                            };
                            _listSarReportMrs00172Rdos.Add(rdo);
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