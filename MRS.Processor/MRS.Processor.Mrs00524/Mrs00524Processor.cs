using ACS.EFMODEL.DataModels;
using ACS.Filter;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using ACS.MANAGER.Core.AcsUser;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisSereServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.MANAGER.Core.AcsUser.Get;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisExpMestMedicine;

namespace MRS.Processor.Mrs00524
{
    class Mrs00524Processor : AbstractProcessor
    {
        Mrs00524Filter castFilter = null;

        public Mrs00524Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_EXP_MEST_MEDICINE> listHisExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
        Dictionary<long, List<HIS_SERVICE_REQ>> dicHisServiceReq = new Dictionary<long, List<HIS_SERVICE_REQ>>();
        List<HIS_MEDICINE_TYPE> listHisMedicineType = new List<HIS_MEDICINE_TYPE>();
        List<ACS_USER> listAcsUser = new List<ACS_USER>();
        List<HIS_EMPLOYEE> listEmployee = new List<HIS_EMPLOYEE>();
        private List<Mrs00524RDO> ListRdo = new List<Mrs00524RDO>();

        public override Type FilterType()
        {
            return typeof(Mrs00524Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00524Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00524: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                if (castFilter.TAKE_MONTH != null && castFilter.TAKE_MONTH != 0)
                {
                    castFilter.TIME_FROM = (castFilter.TAKE_MONTH ?? 0) + 1000000;
                    List<string> _31Days = new List<string>() { "01", "03", "05", "07", "08", "10", "12" };
                    List<string> _30Days = new List<string>() { "04", "06", "09", "11" };
                    List<string> _29Days = new List<string>();
                    List<string> _28Days = new List<string>();
                    string yearStr = castFilter.TAKE_MONTH.ToString().Length >= 4 ? castFilter.TAKE_MONTH.ToString().Substring(0, 4) ?? "0" : "0";
                    int yearNumber = Convert.ToInt32(yearStr);
                    if (yearNumber > 0)
                    {
                        if (yearNumber % 4 == 0)
                        {
                            _28Days.Add("02");
                        }
                        else
                        {
                            _29Days.Add("02");
                        }
                    }
                    string monthStr = castFilter.TAKE_MONTH.ToString().Substring(4, 2) ?? "0";
                    if (monthStr != "0")
                    {
                        if (_31Days.Contains(monthStr))
                        {
                            castFilter.TIME_TO = (castFilter.TAKE_MONTH ?? 0) + 31235959;
                        }
                        else if (_30Days.Contains(monthStr))
                        {
                            castFilter.TIME_TO = (castFilter.TAKE_MONTH ?? 0) + 30235959;
                        }
                        else if (_29Days.Contains(monthStr))
                        {
                            castFilter.TIME_TO = (castFilter.TAKE_MONTH ?? 0) + 29235959;
                        }
                        else if (_28Days.Contains(monthStr))
                        {
                            castFilter.TIME_TO = (castFilter.TAKE_MONTH ?? 0) + 28235959;
                        }
                    }
                }
                LogSystem.Info("TIME_TO:" + castFilter.TIME_TO);
                var listHisRoom = HisRoomCFG.HisRooms ?? new List<V_HIS_ROOM>();
                if ((castFilter.BRANCH_ID ?? 0) != 0)
                    listHisRoom = listHisRoom.Where(o => o.BRANCH_ID == castFilter.BRANCH_ID).ToList();
                else
                    listHisRoom = null;

                HisServiceReqFilterQuery HisServiceReqfilter = new HisServiceReqFilterQuery();
                HisServiceReqfilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                HisServiceReqfilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                HisServiceReqfilter.SERVICE_REQ_TYPE_IDs = new List<long>
                {IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONDT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK,
                    IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM};
                if ((castFilter.BRANCH_ID ?? 0) != 0)
                    HisServiceReqfilter.REQUEST_ROOM_IDs = listHisRoom.Select(o => o.ID).ToList();
                HisServiceReqfilter.HAS_EXECUTE = true;
                var listHisServiceReqSub = new HisServiceReqManager().Get(HisServiceReqfilter);

                if (castFilter.REQUEST_LOGINNAMEs != null)
                {
                    listHisServiceReqSub = listHisServiceReqSub.Where(o => castFilter.REQUEST_LOGINNAMEs.Contains (o.REQUEST_LOGINNAME)).ToList();
                        
                }
                if (listHisServiceReqSub == null)
                    Inventec.Common.Logging.LogSystem.Error("listHisSereServSub Get null");
                else
                    listHisServiceReq.AddRange(listHisServiceReqSub);
                var serviceReqIds = listHisServiceReq.Select(o => o.ID).Distinct().ToList();
                if (serviceReqIds != null && serviceReqIds.Count > 0)
                {
                    var skip = 0;
                    while (serviceReqIds.Count - skip > 0)
                    {
                        var limit = serviceReqIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisExpMestMedicineViewFilterQuery HisExpMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
                        HisExpMestMedicinefilter.TDL_SERVICE_REQ_IDs = limit;
                        if (castFilter.MEDICINE_TYPE_IDs != null) HisExpMestMedicinefilter.MEDICINE_TYPE_IDs = castFilter.MEDICINE_TYPE_IDs;
                        HisExpMestMedicinefilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        var listHisExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(HisExpMestMedicinefilter);
                        if (listHisExpMestMedicineSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisExpMestMedicineSub Get null" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => HisExpMestMedicinefilter), HisExpMestMedicinefilter));
                        else
                            listHisExpMestMedicine.AddRange(listHisExpMestMedicineSub);
                    }
                }
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00524:");
                }

                listEmployee = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EMPLOYEE>("select * from his_employee");
                dicHisServiceReq = listHisServiceReq.GroupBy(g=>g.ID).ToDictionary(p=>p.Key,q=>q.ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                resutl = false;
            }
            return resutl;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdo.AddRange((from r in listHisExpMestMedicine select new Mrs00524RDO(r,dicHisServiceReq.ContainsKey(r.TDL_SERVICE_REQ_ID??0)? dicHisServiceReq[r.TDL_SERVICE_REQ_ID??0]:new List<HIS_SERVICE_REQ>(), listEmployee)).ToList() ?? new List<Mrs00524RDO>());
                ListRdo = ListRdo.Where(o => o.MEDICINE_TYPE_NAME != null).ToList();
                
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", ListRdo);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }


}
