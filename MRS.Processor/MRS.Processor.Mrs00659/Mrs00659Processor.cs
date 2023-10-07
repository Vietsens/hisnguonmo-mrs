using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00659
{
    class Mrs00659Processor : AbstractProcessor
    {
        Mrs00659Filter castFilter = null;
        List<Mrs00659RDO> ListRdo = new List<Mrs00659RDO>();

        List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        List<LIS_SAMPLE> ListSample = new List<LIS_SAMPLE>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();

        bool SameDay = false;

        public Mrs00659Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00659Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00659Filter)reportFilter);

                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                reqFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                reqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__XN;
                reqFilter.IS_SEND_LIS = true;
                ListServiceReq = new HisServiceReqManager().Get(reqFilter);
                Inventec.Common.Logging.LogSystem.Info("ListServiceReq:" + ListServiceReq.Count);
                if (IsNotNullOrEmpty(ListServiceReq))
                {
                    ListSample = new ManagerSql().GetLisSample(castFilter);

                    List<long> reqIds = ListServiceReq.Select(s => s.ID).ToList();
                    int skip = 0;
                    while (reqIds.Count - skip > 0)
                    {
                        var listId = reqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServFilterQuery ssFilter = new HisSereServFilterQuery();
                        ssFilter.SERVICE_REQ_IDs = listId;
                        var sereServ = new HisSereServManager().Get(ssFilter);
                        if (IsNotNullOrEmpty(sereServ))
                        {
                            ListSereServ.AddRange(sereServ);
                        }
                    }
                }

                SameDay = CheckSameDay(castFilter.TIME_FROM, castFilter.TIME_TO);
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        //cung ngay se hien thi gio, khac ngay se hien thi day du
        private bool CheckSameDay(long? timeFrom, long? timeTo)
        {
            bool result = false;
            try
            {
                if (timeFrom.HasValue && timeTo.HasValue)
                {
                    long dayFrom = long.Parse(timeFrom.ToString().Substring(0, 8));
                    long dayTo = long.Parse(timeTo.ToString().Substring(0, 8));

                    if (dayFrom - dayTo == 0)
                    {
                        result = true;
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSample))
                {
                    Dictionary<long, List<HIS_SERE_SERV>> dicSereServ = new Dictionary<long, List<HIS_SERE_SERV>>();

                    if (IsNotNullOrEmpty(ListSereServ))
                    {
                        foreach (var item in ListSereServ)
                        {
                            if (!dicSereServ.ContainsKey(item.SERVICE_REQ_ID ?? 0))
                                dicSereServ[item.SERVICE_REQ_ID ?? 0] = new List<HIS_SERE_SERV>();

                            dicSereServ[item.SERVICE_REQ_ID ?? 0].Add(item);
                        }
                    }

                    foreach (var sample in ListSample)
                    {
                        var serviceReq = ListServiceReq.FirstOrDefault(o => o.SERVICE_REQ_CODE == sample.SERVICE_REQ_CODE);
                        if (serviceReq == null) continue;

                        if (!dicSereServ.ContainsKey(serviceReq.ID)) continue;

                        Mrs00659RDO rdo = new Mrs00659RDO(sample, SameDay);
                        rdo.SERVICE_NAME = string.Join(",", dicSereServ[serviceReq.ID].Select(s => s.TDL_SERVICE_NAME));
                        rdo.TDL_PATIENT_NAME = serviceReq.TDL_PATIENT_NAME;
                        rdo.TDL_PATIENT_GENDER_NAME = serviceReq.TDL_PATIENT_GENDER_NAME;

                        //+ Đối với phòng khám: Người nhận kq là bác sĩ chỉ định, giờ nhận KQ= giờ trả KQ
                        //+ Đối với khoa lâm sàng: lấy tên người nhận kết quả và giờ vào báo cáo
                        //chưa có thông tin ng nhân nên lấy thông tin ng chỉ định
                        //rdo.RESULT_USERNAME = serviceReq.REQUEST_USERNAME;

                        var intructionTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(serviceReq.INTRUCTION_TIME) ?? DateTime.Now;
                        var cancelTime = intructionTime.AddDays(7);
                        rdo.CANCEL_TIME_STR = cancelTime.ToString("HH:mm dd/MM/yyyy");

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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                ListRdo = ListRdo.OrderBy(o => o.INTRUCTION_TIME).ToList();
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
