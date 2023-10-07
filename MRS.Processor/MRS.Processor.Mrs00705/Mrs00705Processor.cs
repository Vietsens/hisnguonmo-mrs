using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00705
{
    class Mrs00705Processor : AbstractProcessor
    {
        Mrs00705Filter castFilter = null;
        List<Mrs00705RDO> ListMrs00705RDO = new List<Mrs00705RDO>();
        List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> ListNewTreatment = new List<HIS_TREATMENT>();
        List<HIS_DEPARTMENT> ListDepartment = new List<HIS_DEPARTMENT>();

        public Mrs00705Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00705Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                this.castFilter = (Mrs00705Filter)this.reportFilter;
                HisTreatmentFilterQuery filter = new HisTreatmentFilterQuery();
                filter.IN_TIME_FROM = castFilter.TIME_FROM;
                filter.IN_TIME_TO = castFilter.TIME_TO;
                filter.END_DEPARTMENT_IDs = castFilter.DEPARTMENT_IDs;
                ListTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager().Get(filter);

                int skip = 0;
                while (ListTreatment.Count - skip > 0)
                {
                    var listIds = ListTreatment.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var reqFilter = new HisServiceReqFilterQuery();
                    reqFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH;
                    var reqs = new HisServiceReqManager().Get(reqFilter);
                    if (IsNotNullOrEmpty(reqs))
                    {
                        ListServiceReq.AddRange(reqs);
                    }
                }

                ListServiceReq = ListServiceReq.Where(o => !String.IsNullOrWhiteSpace(o.PART_EXAM_EYESIGHT_LEFT) || !String.IsNullOrWhiteSpace(o.PART_EXAM_EYESIGHT_RIGHT)).ToList();

                ListTreatment = ListTreatment.Where(o => ListServiceReq.Exists(e => e.TREATMENT_ID == o.ID)).ToList();

                List<long> listPatient = new List<long>();
                listPatient = ListTreatment.Select(s => s.PATIENT_ID).Distinct().ToList();
                skip = 0;
                while (listPatient.Count - skip > 0)
                {
                    var listIds = listPatient.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    var treatFilter = new HisTreatmentFilterQuery();
                    treatFilter.PATIENT_IDs = listIds;
                    var treatment = new HisTreatmentManager().Get(treatFilter);
                    if (IsNotNullOrEmpty(treatment))
                    {
                        ListNewTreatment.AddRange(treatment);
                    }
                }

                if (IsNotNullOrEmpty(castFilter.DEPARTMENT_IDs))
                    ListDepartment = MRS.MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).ToList();
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
                    Dictionary<long, List<HIS_SERVICE_REQ>> dicServiceReq = new Dictionary<long, List<HIS_SERVICE_REQ>>();
                    Dictionary<long, List<HIS_TREATMENT>> dicTreatment = new Dictionary<long, List<HIS_TREATMENT>>();
                    if (IsNotNullOrEmpty(ListServiceReq))
                    {
                        foreach (var item in ListServiceReq)
                        {
                            if (!dicServiceReq.ContainsKey(item.TREATMENT_ID))
                            {
                                dicServiceReq[item.TREATMENT_ID] = new List<HIS_SERVICE_REQ>();
                            }

                            dicServiceReq[item.TREATMENT_ID].Add(item);
                        }
                    }

                    if (IsNotNullOrEmpty(ListNewTreatment))
                    {
                        foreach (var item in ListNewTreatment)
                        {
                            if (!dicTreatment.ContainsKey(item.PATIENT_ID))
                            {
                                dicTreatment[item.PATIENT_ID] = new List<HIS_TREATMENT>();
                            }

                            dicTreatment[item.PATIENT_ID].Add(item);
                        }
                    }

                    foreach (var treatment in ListTreatment)
                    {
                        Mrs00705RDO ado = new Mrs00705RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00705RDO>(ado, treatment);
                        if (dicServiceReq.ContainsKey(treatment.ID))
                        {
                            var serviceReq = dicServiceReq[treatment.ID].OrderByDescending(o => o.INTRUCTION_TIME).ToList();
                            ado.PART_EXAM_EYESIGHT_LEFT = serviceReq.First().PART_EXAM_EYESIGHT_LEFT;
                            ado.PART_EXAM_EYESIGHT_RIGHT = serviceReq.First().PART_EXAM_EYESIGHT_RIGHT;
                        }

                        if (dicTreatment.ContainsKey(treatment.PATIENT_ID) && dicTreatment[treatment.PATIENT_ID].Count > 1)
                        {
                            var newTreatment = dicTreatment[treatment.PATIENT_ID].Where(o => (o.OUT_TIME ?? 0) > (treatment.OUT_TIME ?? 99999999999999)).OrderBy(o => o.IN_TIME).ToList();
                            if (IsNotNullOrEmpty(newTreatment))
                            {
                                ado.NOTE = "Đã tái khám";
                                ado.EYESIGHT_LEFT_NEW_1 = newTreatment[0].EYESIGHT_LEFT;
                                ado.EYESIGHT_RIGHT_NEW_1 = newTreatment[0].EYESIGHT_RIGHT;

                                if (newTreatment.Count > 1)
                                {
                                    ado.EYESIGHT_LEFT_NEW_2 = newTreatment[1].EYESIGHT_LEFT;
                                    ado.EYESIGHT_RIGHT_NEW_2 = newTreatment[1].EYESIGHT_RIGHT;
                                }

                                if (newTreatment.Count > 2)
                                {
                                    ado.EYESIGHT_LEFT_NEW_3 = newTreatment[2].EYESIGHT_LEFT;
                                    ado.EYESIGHT_RIGHT_NEW_3 = newTreatment[2].EYESIGHT_RIGHT;
                                }
                            }
                        }

                        ListMrs00705RDO.Add(ado);
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
            try
            {
                if (castFilter.TIME_FROM.HasValue)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                }

                if (castFilter.TIME_TO.HasValue)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));
                }

                if (IsNotNullOrEmpty(ListDepartment))
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", string.Join(", ", ListDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(s => s.DEPARTMENT_NAME)));
                }
                else
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", "Tất cả");
                }

                if (IsNotNullOrEmpty(ListMrs00705RDO))
                {
                    bool has2 = ListMrs00705RDO.Exists(o => !string.IsNullOrWhiteSpace(o.EYESIGHT_LEFT_NEW_2) || !string.IsNullOrWhiteSpace(o.EYESIGHT_RIGHT_NEW_2));
                    bool has3 = ListMrs00705RDO.Exists(o => !string.IsNullOrWhiteSpace(o.EYESIGHT_LEFT_NEW_3) || !string.IsNullOrWhiteSpace(o.EYESIGHT_RIGHT_NEW_3));

                    if (has2)
                    {
                        dicSingleTag.Add("NEW_2", 1);
                    }

                    if (has3)
                    {
                        dicSingleTag.Add("NEW_3", 1);
                    }
                }

                objectTag.AddObjectData(store, "Report", ListMrs00705RDO);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
