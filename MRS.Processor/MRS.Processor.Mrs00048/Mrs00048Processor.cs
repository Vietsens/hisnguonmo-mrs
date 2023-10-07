using MOS.MANAGER.HisService;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00048
{
    public class Mrs00048Processor : AbstractProcessor
    {
        Mrs00048Filter castFilter = null;
        List<Mrs00048RDO> ListRdo = new List<Mrs00048RDO>();

        List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_REQ> ListCurrentServiceReq = new List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_REQ>();

        public Mrs00048Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00048Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00048Filter)this.reportFilter);
                LoadDataToRam();
                if (!IsNotNullOrEmpty(ListCurrentServiceReq))
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ProcessListCurrentServiceReq();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentServiceReq()
        {
            try
            {
                if (ListCurrentServiceReq != null && ListCurrentServiceReq.Count > 0)
                {
                    CommonParam paramGet = new CommonParam();
                    var Groups = ListCurrentServiceReq.OrderBy(o => o.EXECUTE_DEPARTMENT_ID).ToList().GroupBy(g => g.EXECUTE_ROOM_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<MOS.EFMODEL.DataModels.V_HIS_SERVICE_REQ> listSub = group.ToList<V_HIS_SERVICE_REQ>();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00048RDO rdo = new Mrs00048RDO();
                            rdo.EXECUTE_DEPARTMENT_NAME = listSub[0].EXECUTE_DEPARTMENT_NAME;
                            rdo.EXECUTE_ROOM_CODE = listSub[0].EXECUTE_ROOM_CODE;
                            rdo.EXECUTE_ROOM_NAME = listSub[0].EXECUTE_ROOM_NAME;
                            foreach (var serviceReq in listSub)
                            {
                                if (serviceReq.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK ||
                                    serviceReq.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM ||
                                    serviceReq.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT ||
                                    serviceReq.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT)
                                {
                                    HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                                    expMestFilter.SERVICE_REQ_ID = serviceReq.ID;
                                    var prescription = new HisExpMestManager(paramGet).Get(expMestFilter);
                                    if (IsNotNullOrEmpty(prescription))
                                    {
                                        if (prescription.First().EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                                        {
                                            rdo.AMOUNT_DONE += 1;
                                        }
                                        else
                                        {
                                            rdo.AMOUNT_PROCESSING += 1;
                                        }
                                    }
                                }
                                else
                                {
                                    if (serviceReq.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL)
                                    {
                                        rdo.AMOUNT_NEW += 1;
                                    }
                                    else if (serviceReq.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL)
                                    {
                                        rdo.AMOUNT_PROCESSING += 1;
                                    }
                                    else if (serviceReq.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                                    {
                                        rdo.AMOUNT_DONE += 1;
                                    }
                                }
                            }
                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisServiceReqViewFilterQuery filter = new HisServiceReqViewFilterQuery();
                filter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ListCurrentServiceReq = new HisServiceReqManager().GetView(filter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentServiceReq.Clear();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("INTRUCTION_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
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
