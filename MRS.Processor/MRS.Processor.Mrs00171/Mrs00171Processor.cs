using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisSereServ;
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
using MOS.Filter;
using MRS.MANAGER.Base;
using FlexCel.Report;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisServiceReq;

namespace MRS.Processor.Mrs00171
{
    internal class Mrs00171Processor : AbstractProcessor
    {
        List<VSarReportMrs00171RDO> _listSarReportMrs00171Rdos = new List<VSarReportMrs00171RDO>();
        Mrs00171Filter CastFilter;
        List<long> listServiceId = new List<long>();
        List<HIS_SERVICE_GROUP> ListServiceGroup = new List<HIS_SERVICE_GROUP>(); ////
        List<HIS_PATIENT_TYPE> ListPatientType = new List<HIS_PATIENT_TYPE>(); ///
        List<HIS_TREATMENT_TYPE> ListTreatmentType = new List<HIS_TREATMENT_TYPE>(); ///
        public Mrs00171Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00171Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00171Filter)this.reportFilter;

                HisServiceGroupFilterQuery listServiceGroupFilter = new HisServiceGroupFilterQuery(); ///
                listServiceGroupFilter.ID = CastFilter.SERVICE_GROUP_ID;
                ListServiceGroup = new MOS.MANAGER.HisServiceGroup.HisServiceGroupManager(paramGet).Get(listServiceGroupFilter);

                HisPatientTypeFilterQuery listPatientTypeFilter = new HisPatientTypeFilterQuery(); ///
                listPatientTypeFilter.ID = CastFilter.PATIENT_TYPE_ID;
                ListPatientType = new MOS.MANAGER.HisPatientType.HisPatientTypeManager(paramGet).Get(listPatientTypeFilter);

                HisTreatmentTypeFilterQuery listTreatmentTypeFilter = new HisTreatmentTypeFilterQuery(); ///
                listTreatmentTypeFilter.ID = CastFilter.TREATMENT_TYPE_ID;
                ListTreatmentType = new MOS.MANAGER.HisTreatmentType.HisTreatmentTypeManager(paramGet).Get(listTreatmentTypeFilter);

                var metyFilterServSegrView = new HisServSegrFilterQuery
                {
                    SERVICE_GROUP_ID = CastFilter.SERVICE_GROUP_ID
                };
                var listServSegr = new MOS.MANAGER.HisServSegr.HisServSegrManager(paramGet).Get(metyFilterServSegrView);

                listServiceId = listServSegr.Select(o => o.SERVICE_ID).Distinct().ToList();

                //---------------------------------------------------------------------------------------HIS_SERE_SERV
                //yeu cau
                var ListServiceReq = new List<HIS_SERVICE_REQ>();
                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.FINISH_TIME_FROM = CastFilter.TIME_FROM;
                reqFilter.FINISH_TIME_TO = CastFilter.TIME_TO;
                ListServiceReq = new HisServiceReqManager().Get(reqFilter);
                var treatmentIds = ListServiceReq.Select(o => o.TREATMENT_ID).Distinct().ToList();
                var listSereservs = new List<HIS_SERE_SERV>();
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

                if (CastFilter.BRANCH_IDs != null)
                {
                    listTreatments = listTreatments.Where(o => CastFilter.BRANCH_IDs.Contains(o.BRANCH_ID )).ToList();
                }

                if (CastFilter.TREATMENT_TYPE_ID != null)
                {
                    listTreatments = listTreatments.Where(o => o.TDL_TREATMENT_TYPE_ID == CastFilter.TREATMENT_TYPE_ID).ToList();
                }
                if (CastFilter.PATIENT_TYPE_ID != null)
                {
                    listTreatments = listTreatments.Where(o => o.TDL_PATIENT_TYPE_ID == CastFilter.PATIENT_TYPE_ID).ToList();
                }

                if (CastFilter.TREATMENT_TYPE_IDs != null)
                {
                    listTreatments = listTreatments.Where(o => CastFilter.TREATMENT_TYPE_IDs.Contains(o.TDL_TREATMENT_TYPE_ID ?? 0)).ToList();
                }
                if (CastFilter.PATIENT_TYPE_IDs != null)
                {
                    listTreatments = listTreatments.Where(o => CastFilter.PATIENT_TYPE_IDs.Contains(o.TDL_PATIENT_TYPE_ID ?? 0)).ToList();
                }
                treatmentIds = listTreatments.Select(o=>o.ID).ToList();
                
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

                        HisSereServFilterQuery filterSs = new HisSereServFilterQuery();
                        filterSs.TREATMENT_IDs = listIDs;
                        filterSs.IS_EXPEND = false;
                        filterSs.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(paramGet).Get(filterSs);
                        if (IsNotNullOrEmpty(listSereServSub))
                            listSereservs.AddRange(listSereServSub);
                    }

                    var serviceReqIds = ListServiceReq.Select(o => o.ID).ToList();
                    listSereservs = listSereservs.Where(o => listServiceId.Contains(o.SERVICE_ID) && serviceReqIds.Contains(o.SERVICE_REQ_ID ?? 0)).ToList();
                    listSereServBill = listSereServBill.Where(o => serviceReqIds.Contains(o.TDL_SERVICE_REQ_ID ?? 0)).ToList();
                    listSereServDeposit = listSereServDeposit.Where(o => serviceReqIds.Contains(o.TDL_SERVICE_REQ_ID ?? 0)).ToList();
                    listSeseDepoRepay = listSeseDepoRepay.Where(o => serviceReqIds.Contains(o.TDL_SERVICE_REQ_ID ?? 0)).ToList();
                    var ssbIds = listSereServBill.Select(p => p.SERE_SERV_ID).ToList();
                    var ssdIds = listSereServDeposit.Select(p => p.SERE_SERV_ID).ToList();
                    var ssrIds = listSeseDepoRepay.Select(p => p.TDL_SERE_SERV_PARENT_ID??0).ToList();
                        //DV tm đk: 
                    listSereservs = listSereservs.Where(o => o.IS_DELETE == 0 && o.VIR_TOTAL_PATIENT_PRICE>0 && (!ssbIds.Contains(o.ID)) && ((!ssdIds.Contains(o.ID)) || ssrIds.Count() > 0)).ToList();
                }
                
                ProcessFilterData(listSereservs, listTreatments);


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

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00171Rdos);
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            objectTag.SetUserFunction(store, "FuncSameTitleCol1", new CustomerFuncMergeSameData1());
            objectTag.SetUserFunction(store, "FuncSameTitleCol2", new CustomerFuncMergeSameData2());

        }

        private void ProcessFilterData(List<HIS_SERE_SERV> listSereservs,
            List<HIS_TREATMENT> listTreatments)
        {
            try
            {


                var listSereserv = listSereservs.Where(s => (s.HEIN_RATIO??0) != 1 ).ToList();

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
                            var rdo = new VSarReportMrs00171RDO
                            {
                                SERVICE_NAME = listSub.First().TDL_SERVICE_NAME,
                                SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o=>o.ID==listSub.First().TDL_SERVICE_UNIT_ID)??new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME,
                                TOTAL_AMOUNT = listSub.Select(o => o.AMOUNT).Sum(),
                                VIR_PATIENT_NAME = treatment.TDL_PATIENT_NAME,
                                TREATMENT_CODE = treatment.TREATMENT_CODE,
                                AMOUNT = listSubTreatment.Select(o => o.AMOUNT).Sum()
                            };
                            _listSarReportMrs00171Rdos.Add(rdo);
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        class CustomerFuncMergeSameData : TFlexCelUserFunction
        {
            string ServiceName;

            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {

                    string SameType = System.Convert.ToString(parameters[0]);

                    if (SameType != "")
                    {
                        if (SameType == ServiceName)
                        {
                            return true;
                        }
                        else
                        {

                            ServiceName = SameType;
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }
        class CustomerFuncMergeSameData1 : TFlexCelUserFunction
        {
            string ServiceName1;

            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {

                    string SameType = System.Convert.ToString(parameters[0]);

                    if (SameType != "")
                    {
                        if (SameType == ServiceName1)
                        {
                            return true;
                        }
                        else
                        {

                            ServiceName1 = SameType;
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }
        class CustomerFuncMergeSameData2 : TFlexCelUserFunction
        {
            string ServiceName2;

            public override object Evaluate(object[] parameters)
            {
                if (parameters == null || parameters.Length <= 0)
                    throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

                bool result = false;
                try
                {

                    string SameType = System.Convert.ToString(parameters[0]);

                    if (SameType != "")
                    {
                        if (SameType == ServiceName2)
                        {
                            return true;
                        }
                        else
                        {

                            ServiceName2 = SameType;
                            return false;
                        }
                    }

                }
                catch (Exception ex)
                {

                }

                return result;
            }
        }

    }
}
