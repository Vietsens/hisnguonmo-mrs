using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00702
{
    class Mrs00702Processor : AbstractProcessor
    {
        Mrs00702Filter castFilter = null;
        List<Mrs00702RDO> ListRdo = new List<Mrs00702RDO>();
        List<V_LIS_SAMPLE> ListSample = new List<V_LIS_SAMPLE>();
        List<V_LIS_RESULT> ListResult = new List<V_LIS_RESULT>();
        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();

        public Mrs00702Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00702Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00702Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("_______Mrs00702Filter____" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                CommonParam paramGet = new CommonParam();

                ListSample = new ManagerSql().GetLisSample(castFilter);

                ListServiceReq = new ManagerSql().GetLisServiceReq(ListSample.Select(s => s.SERVICE_REQ_CODE).Distinct().ToList(), castFilter.PATIENT_TYPE_ID);
                if (castFilter.EXECUTE_DEPARTMENT_IDs != null)
                {
                    ListServiceReq = ListServiceReq.Where(p => castFilter.EXECUTE_DEPARTMENT_IDs.Contains(p.EXECUTE_DEPARTMENT_ID)).ToList();
                }
                if (castFilter.EXECUTE_ROOM_IDs != null)
                {
                    ListServiceReq = ListServiceReq.Where(p => castFilter.EXECUTE_ROOM_IDs.Contains(p.EXECUTE_ROOM_ID)).ToList();
                }

                ListSample = ListSample.Where(o => ListServiceReq.Select(s => s.SERVICE_REQ_CODE).Contains(o.SERVICE_REQ_CODE)).ToList();

                var listResult = new ManagerSql().GetLisResult(ListSample.Select(s => s.ID).ToList());
                if (IsNotNullOrEmpty(listResult))
                {
                    ListResult.AddRange(listResult);
                }
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
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    Dictionary<string, List<V_LIS_SAMPLE>> dicSample = new Dictionary<string, List<V_LIS_SAMPLE>>();
                    foreach (var item in ListSample)
                    {
                        if (String.IsNullOrWhiteSpace(item.SERVICE_REQ_CODE)) continue;

                        if (!dicSample.ContainsKey(item.SERVICE_REQ_CODE))
                            dicSample[item.SERVICE_REQ_CODE] = new List<V_LIS_SAMPLE>();

                        dicSample[item.SERVICE_REQ_CODE].Add(item);
                    }

                    Dictionary<long, List<V_LIS_RESULT>> dicResult = new Dictionary<long, List<V_LIS_RESULT>>();
                    foreach (var item in ListResult)
                    {
                        if (!item.SAMPLE_ID.HasValue) continue;

                        if (!dicResult.ContainsKey(item.SAMPLE_ID ?? 0))
                            dicResult[item.SAMPLE_ID ?? 0] = new List<V_LIS_RESULT>();

                        dicResult[item.SAMPLE_ID ?? 0].Add(item);
                    }

                    foreach (var req in ListServiceReq)
                    {
                        Mrs00702RDO rdo = new Mrs00702RDO();
                        Inventec.Common.Mapper.DataObjectMapper.Map<HIS_SERVICE_REQ>(rdo, req);

                        string ketqua = "";
                        if (dicSample.ContainsKey(req.SERVICE_REQ_CODE))
                        {
                            List<V_LIS_RESULT> rs = new List<V_LIS_RESULT>();
                            foreach (var item in dicSample[req.SERVICE_REQ_CODE])
                            {
                                if (dicResult.ContainsKey(item.ID))
                                {
                                    rs.AddRange(dicResult[item.ID]);
                                }
                            }

                            var gr = rs.GroupBy(g => g.SERVICE_CODE).ToList();
                            List<string> lstValue = new List<string>();
                            foreach (var item in gr)
                            {
                                var kq = string.Format("{0}:{1}", item.First().SERVICE_NAME, string.Join("-", item.Select(s => s.VALUE).Distinct().ToList()));
                                lstValue.Add(kq);
                            }

                            ketqua = string.Join(";", lstValue);
                        }

                        rdo.KET_QUA = ketqua;
                        ListRdo.Add(rdo);
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
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

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
