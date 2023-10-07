using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDhst;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00545
{
    class Mrs00545Processor : AbstractProcessor
    {
        Mrs00545Filter castFilter = null;
        List<Mrs00545RDO> ListRdoDetail = new List<Mrs00545RDO>();
        List<Mrs00545RDO> ListRdo = new List<Mrs00545RDO>();
        List<V_HIS_TREATMENT_4> listTreatment = new List<V_HIS_TREATMENT_4>();
        List<HIS_PATIENT_TYPE_ALTER> LastPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        public Mrs00545Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00545Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = ((Mrs00545Filter)this.reportFilter);
               
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu HIS_TREATMENT, Mrs00545, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                //danh sách bệnh nhân
                HisTreatmentView4FilterQuery HisTreatmentfilter = new HisTreatmentView4FilterQuery();
                HisTreatmentfilter = RDOCustomerFuncMapFilter.MapFilter<Mrs00545Filter, HisTreatmentView4FilterQuery>(castFilter, HisTreatmentfilter);
                if (HisTreatmentfilter.IN_TIME_FROM == null && HisTreatmentfilter.IN_TIME_TO == null)
                {
                    HisTreatmentfilter.IN_TIME_FROM = castFilter.TIME_FROM;
                    HisTreatmentfilter.IN_TIME_TO = castFilter.TIME_TO;
                }
                listTreatment = new HisTreatmentManager(paramGet).GetView4(HisTreatmentfilter);

                //danh sách đối tượng
                LastPatientTypeAlter = GetPatientTypeAlter(listTreatment.Select(s => s.ID).ToList());

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

     

        private List<HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(List<long> list)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = null;
            try
            {
                if (IsNotNullOrEmpty(list))
                {
                    result = new List<HIS_PATIENT_TYPE_ALTER>();
                    var skip = 0;
                    while (list.Count - skip > 0)
                    {
                        var listIDs = list.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = listIDs;
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ID";
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ASC";
                        var paty = new HisPatientTypeAlterManager(new CommonParam()).Get(HisPatientTypeAlterfilter);
                        if (paty == null)
                            Inventec.Common.Logging.LogSystem.Debug("HIS_PATIENT_TYPE_ALTER is null");
                        result.AddRange(paty);

                    }
                    result = result.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listTreatment))
                {

                    ListRdoDetail.Clear();
                    foreach (var item in listTreatment)
                    {
                        var patientTypeAlter = LastPatientTypeAlter.FirstOrDefault(o=>o.TREATMENT_ID==item.ID)??new HIS_PATIENT_TYPE_ALTER();
                        Mrs00545RDO rdo = new Mrs00545RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00545RDO>(rdo, item);
                        if (HisBranchCFG.HisBranchs.Select(o => o.HEIN_MEDI_ORG_CODE ?? "").Contains(patientTypeAlter.HEIN_MEDI_ORG_CODE ?? ""))
                        {
                            rdo.SUM_COUNT = 1;
                            if ((this.castFilter.ADDRESS_EMPLOYEEs ?? new List<string>()).Contains(patientTypeAlter.ADDRESS))
                            {
                                rdo.SUM_COUNT_EM = 1;
                            }
                        }
                        if (rdo.SUM_COUNT == 1)
                        {
                            ListRdoDetail.Add(rdo);
                        }
                    }
                }
                List<string> keySums = new List<string>() { "SUM" };
                ListRdo = RDOCustomerFuncGroupSum.GroupSum(ListRdoDetail.GroupBy(o => new { o.SUM_COUNT }).ToList().ToDictionary(p => p, p => new Mrs00545RDO()), keySums);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdoDetail.Clear();
            }
            return result;
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_FROM ?? this.castFilter.OUT_TIME_FROM ?? this.castFilter.FEE_LOCK_TIME_FROM??0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(this.castFilter.TIME_TO ?? this.castFilter.OUT_TIME_TO ?? this.castFilter.FEE_LOCK_TIME_TO??0));

                objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
