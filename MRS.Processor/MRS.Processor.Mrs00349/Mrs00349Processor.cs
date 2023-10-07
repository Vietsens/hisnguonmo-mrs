using MOS.MANAGER.HisService;
using MOS.MANAGER.HisRoom;
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisServiceReq; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00349
{
    class Mrs00349Processor : AbstractProcessor
    {
        Mrs00349Filter castFilter = null; 

        List<Mrs00349RDO> ListRdo = new List<Mrs00349RDO>(); 
        List<Mrs00349RDO> ListRdoGroup = new List<Mrs00349RDO>(); 

        List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>(); 

        public Mrs00349Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00349Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00349Filter)this.reportFilter; 

                HisRoomFilterQuery roomFilter = new HisRoomFilterQuery(); 
                roomFilter.ROOM_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD; 
                var listRoomTDs = new MOS.MANAGER.HisRoom.HisRoomManager(param).Get(roomFilter); 

                var skip = 0; 
                while (listRoomTDs.Count - skip > 0)
                {
                    var listIds = listRoomTDs.Skip(skip).Take(1).ToList(); 
                    skip = skip + 1; 

                    HisServiceReqViewFilterQuery ServiceReqViewFilter = new HisServiceReqViewFilterQuery(); 
                    ServiceReqViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM; 
                    ServiceReqViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO; 
                    ServiceReqViewFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH; 
                    ServiceReqViewFilter.REQUEST_ROOM_ID = listIds.Select(s => s.ID).First();
                    listServiceReqs.AddRange(new HisServiceReqManager(param).GetView(ServiceReqViewFilter)); 
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

        protected override bool ProcessData()
        {
            bool result = true; 
            try
            {
                if (IsNotNullOrEmpty(listServiceReqs))
                {
                    var listSereServss = new List<MRS00349_SERVICE_REQ>(); 
                    foreach (var i in listServiceReqs)
                    {
                        var sereServ = new MRS00349_SERVICE_REQ(); 
                        sereServ.INTRUCTION_DATE = Inventec.Common.DateTime.Convert.TimeNumberToDateString(i.INTRUCTION_TIME); 
                        sereServ.INTRUCTION_TIME = Convert.ToInt32((i.INTRUCTION_TIME).ToString().Substring(8, 6)); 
                        sereServ.SERVICE_REQ = i; 

                        listSereServss.Add(sereServ); 
                    }

                    var listSereServGroupByDates = listSereServss.GroupBy(g => g.INTRUCTION_DATE); 
                    foreach (var listSereServGroupByDate in listSereServGroupByDates)
                    {
                        var listSereServGroupByLoginnames = listSereServGroupByDate.GroupBy(g => g.SERVICE_REQ.REQUEST_LOGINNAME); 
                        foreach (var listSereServGroupByLoginname in listSereServGroupByLoginnames)
                        {
                            var rdo = new Mrs00349RDO(); 
                            rdo.DATETIME = listSereServGroupByDate.First().INTRUCTION_DATE; 
                            rdo.LOGINNAME = listSereServGroupByLoginname.First().SERVICE_REQ.REQUEST_LOGINNAME; 
                            rdo.USERNAME = listSereServGroupByLoginname.First().SERVICE_REQ.REQUEST_USERNAME; 
                            var goodMorning = listSereServGroupByLoginname.Where(s => s.INTRUCTION_TIME > castFilter.MORNING_HOUR_FROM && s.INTRUCTION_TIME < castFilter.MORNING_HOUR_TO).OrderBy(o => o.INTRUCTION_TIME); 
                            if (IsNotNullOrEmpty(goodMorning.ToList()))
                            {
                                //rdo.MORNING_START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(goodMorning.First().SERE_SERV.INTRUCTION_TIME); 
                                //rdo.MORNING_END_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(goodMorning.Last().SERE_SERV.INTRUCTION_TIME); 
                                rdo.MORNING_START_TIME = (goodMorning.First().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(10, 2) + ":" + (goodMorning.First().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(12, 2); 
                                rdo.MORNING_END_TIME = (goodMorning.Last().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(10, 2) + ":" + (goodMorning.Last().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(12, 2); 
                                rdo.MORNING_AMOUNT_REQUEST = goodMorning.Count(); 
                            }
                            var goodAfternoon = listSereServGroupByLoginname.Where(s => s.INTRUCTION_TIME > castFilter.AFTERNOON_HOUR_FROM && s.INTRUCTION_TIME < castFilter.AFTERNOON_HOUR_TO).OrderBy(o => o.INTRUCTION_TIME); 
                            if (IsNotNullOrEmpty(goodAfternoon.ToList()))
                            {
                                //rdo.AFTERNOON_START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(goodAfternoon.First().SERE_SERV.INTRUCTION_TIME); 
                                //rdo.AFTERNOON_END_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(goodAfternoon.Last().SERE_SERV.INTRUCTION_TIME); 
                                rdo.AFTERNOON_START_TIME = (goodAfternoon.First().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(10, 2) + ":" + (goodAfternoon.First().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(12, 2); 
                                rdo.AFTERNOON_END_TIME = (goodAfternoon.Last().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(10, 2) + ":" + (goodAfternoon.Last().SERVICE_REQ.INTRUCTION_TIME).ToString().Substring(12, 2); 
                                rdo.AFTERNOON_AMOUNT_REQUEST = goodAfternoon.Count(); 
                            }

                            ListRdo.Add(rdo); 
                        }
                    }

                    ListRdoGroup = ListRdo.GroupBy(g => g.DATETIME).Select(s => new Mrs00349RDO
                    {
                        DATETIME = s.First().DATETIME,
                        LOGINNAME = s.First().LOGINNAME
                    }).ToList(); 
                }
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                //dicSingleTag.Add("CREATE_TIME", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year); 

                bool exportSuccess = true; 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.USERNAME).ToList()); 
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup.OrderByDescending(s => s.DATETIME).ToList()); 
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "DATETIME", "DATETIME"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
