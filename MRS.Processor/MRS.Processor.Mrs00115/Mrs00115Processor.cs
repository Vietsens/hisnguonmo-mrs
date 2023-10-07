using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServBill;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00115
{
    public class Mrs00115Processor : AbstractProcessor
    {
        List<Mrs00115RDO> ListSereServRdo = new List<Mrs00115RDO>();
        Mrs00115Filter CastFilter = null;
        private string TITLE_REPOST;
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        List<V_HIS_SERE_SERV> listServiceMety = new List<V_HIS_SERE_SERV>();

        public Mrs00115Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00115Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                this.CastFilter = (Mrs00115Filter)this.reportFilter;
                var paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_SERE_SERV, filter: " +
                    Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => CastFilter), CastFilter));
                var sereServBillFilter = new HisSereServBillViewFilterQuery
                {
                    CREATE_TIME_FROM = CastFilter.CREATE_TIME_FROM,
                    CREATE_TIME_TO = CastFilter.CREATE_TIME_TO,
                };
                var listSereServBill = new MOS.MANAGER.HisSereServBill.HisSereServBillManager(paramGet).GetView(sereServBillFilter);
                //DV - thanh toan
                var listSereServId = listSereServBill.Select(s => s.SERE_SERV_ID).Distinct().ToList();
                if (IsNotNullOrEmpty(listSereServId))
                {
                    var skip = 0;
                    while (listSereServId.Count - skip > 0)
                    {
                        var listIDs = listSereServId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery filterSereServ = new HisSereServViewFilterQuery();
                        filterSereServ.IDs = listIDs;
                        filterSereServ.SERVICE_TYPE_IDs = CastFilter.LIST_SERVICE_TYPE_ID;
                        var listSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(filterSereServ);
                        listServiceMety.AddRange(listSereServSub);
                    }
                }
                //yeu cau
                var listServiceReqId = listServiceMety.Select(o => o.SERVICE_REQ_ID ?? 0).Distinct().ToList();
                if (IsNotNullOrEmpty(listServiceReqId)) dicServiceReq = GetServiceReq(listServiceReqId).GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                    throw new DataMisalignedException(
                        "Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_SERE_SERV, MRS00115." +
                        Inventec.Common.Logging.LogUtil.TraceData(
                            Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ();
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new HIS_SERVICE_REQ();
            }
            return result;
        }

        private List<HIS_SERVICE_REQ> GetServiceReq(List<long> listServiceReqId)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                var skip = 0;
                while (listServiceReqId.Count - skip > 0)
                {
                    var listIDs = listServiceReqId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisServiceReqFilterQuery filterServiceReq = new HisServiceReqFilterQuery();
                    filterServiceReq.IDs = listIDs;
                    var listServiceReqSub = new MOS.MANAGER.HisServiceReq.HisServiceReqManager().Get(filterServiceReq);
                    result.AddRange(listServiceReqSub);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<HIS_SERVICE_REQ>();
            }
            return result;
        }

        protected override bool ProcessData()
        {
            var result = false;
            try
            {
                var serviceTypeName = listServiceMety.Select(s => s.SERVICE_TYPE_NAME).Distinct().OrderBy(s => s).ToList();
                TITLE_REPOST = string.Join(", ", serviceTypeName).ToUpper();
                foreach (var lstSereServ in listServiceMety)
                {
                    var rdo = new Mrs00115RDO(lstSereServ, req(lstSereServ));
                    ListSereServRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TITLE_SERVICE", TITLE_REPOST);
                dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.CREATE_TIME_FROM));
                dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.CREATE_TIME_TO));

                objectTag.AddObjectData(store, "SereServs", ListSereServRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
