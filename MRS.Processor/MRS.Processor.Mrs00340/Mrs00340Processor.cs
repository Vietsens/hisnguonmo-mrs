using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisPtttGroup;
using MOS.MANAGER.HisEkipUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisInvoice;
using MOS.MANAGER.HisInvoiceDetail;

using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00340
{
    public class Mrs00340Processor : AbstractProcessor
    {
        List<Mrs00340RDO> ListRdo = new List<Mrs00340RDO>();
        List<DAY> ListDay = new List<DAY>();
        Mrs00340Filter castFilter = new Mrs00340Filter();

        public const long MAX_REQUEST_DAY = 31;

        List<HIS_SERVICE_REQ> listServiceReq = new List<HIS_SERVICE_REQ>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_SERE_SERV_PTTT> listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>();
        List<V_HIS_EKIP_USER> listEkipUsers = new List<V_HIS_EKIP_USER>();
        long PTTT_GROUP_ID__GROUP1 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP1;
        long PTTT_GROUP_ID__GROUP2 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP2;
        long PTTT_GROUP_ID__GROUP3 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP3;
        long PTTT_GROUP_ID__GROUP4 = HisPtttGroupCFG.PTTT_GROUP_ID__GROUP4;
        public Mrs00340Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00340Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = ((Mrs00340Filter)this.reportFilter);
                // yêu cầu:
                HisServiceReqFilterQuery ServiceReqFilter = new HisServiceReqFilterQuery();
                ServiceReqFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;  //EXECUTE_TIME_FROM
                ServiceReqFilter.FINISH_TIME_TO = castFilter.TIME_TO;  //EXECUTE_TIME_TO
                ServiceReqFilter.SERVICE_REQ_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT };
                ServiceReqFilter.REQUEST_ROOM_IDs = castFilter.REQ_ROOM_IDs;
                ServiceReqFilter.EXECUTE_ROOM_IDs = castFilter.EXE_ROOM_IDs;
                listServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(ServiceReqFilter);

                if (castFilter.REQ_DEPARTMENT_IDs != null)
                {
                    listServiceReq = listServiceReq.Where(o => castFilter.REQ_DEPARTMENT_IDs.Contains(o.REQUEST_DEPARTMENT_ID)).ToList();
                }
                if (castFilter.EXE_DEPARTMENT_IDs != null)
                {
                    listServiceReq = listServiceReq.Where(o => castFilter.EXE_DEPARTMENT_IDs.Contains(o.EXECUTE_DEPARTMENT_ID)).ToList();
                }
                if (castFilter.INPUT_DATA_ID_SVT != null)
                {
                    listServiceReq = listServiceReq.Where(o => castFilter.INPUT_DATA_ID_SVT == 1 && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT
                    || castFilter.INPUT_DATA_ID_SVT == 2 && o.SERVICE_REQ_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT).ToList();
                }
                if (IsNotNullOrEmpty(castFilter.LOGINNAMEs))
                    listServiceReq = listServiceReq.Where(o => castFilter.LOGINNAMEs.Contains(o.EXECUTE_LOGINNAME ?? "")).ToList();
                //V_HIS_SERE_SERV
                var skip1 = 0;
                var serviceReqIds = listServiceReq.Select(o => o.ID).ToList();
                while (serviceReqIds.Count - skip1 > 0)
                {
                    var listIds = serviceReqIds.Skip(skip1).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip1 = skip1 + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                    sereServViewFilter.SERVICE_REQ_IDs = listIds;

                    var listSereServSub = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServViewFilter);
                    listSereServs.AddRange(listSereServSub);
                }
                dicServiceReq = listServiceReq.GroupBy(o => o.ID).ToDictionary(p => p.Key, p => p.First());

                // V_HIS_SERE_SERV_PTTT:
                var skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisSereServPtttViewFilterQuery sereServPtttViewFilter = new HisSereServPtttViewFilterQuery();
                    sereServPtttViewFilter.SERE_SERV_IDs = listIds.Select(s => s.ID).ToList();
                    var listSereServPttt = new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(paramGet).GetView(sereServPtttViewFilter);
                    listSereServPttts.AddRange(listSereServPttt);
                }
                // V_HIS_EKIP_USER:
                skip = 0;
                while (listSereServs.Count - skip > 0)
                {
                    var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisEkipUserViewFilterQuery ekipUserViewFilter = new HisEkipUserViewFilterQuery();
                    ekipUserViewFilter.EKIP_IDs = listIds.Select(s => s.EKIP_ID ?? 0).ToList();
                    var listEkipUser = new MOS.MANAGER.HisEkipUser.HisEkipUserManager(paramGet).GetView(ekipUserViewFilter);
                    listEkipUsers.AddRange(listEkipUser);
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
                List<EXECUTE_LOGIN> listExeLogins = new List<EXECUTE_LOGIN>();
                foreach (var listSereServ in listSereServs)
                {
                    if (!dicServiceReq.ContainsKey(listSereServ.SERVICE_REQ_ID ?? 0)) continue;
                    var listSereServPttt = listSereServPttts.Where(s => s.SERE_SERV_ID == listSereServ.ID).ToList();
                    var listEkipUSer = listEkipUsers.Where(s => s.EKIP_ID == listSereServ.EKIP_ID).ToList();
                    if (IsNotNullOrEmpty(listEkipUSer))
                    {
                        HIS_SERVICE_REQ req = dicServiceReq[listSereServ.SERVICE_REQ_ID ?? 0];
                        foreach (var ekipUser in listEkipUSer)
                        {
                            var exe = new EXECUTE_LOGIN();
                            exe.LOGINNAME = ekipUser.LOGINNAME;
                            exe.USERNAME = ekipUser.USERNAME;
                            exe.EXECUTE_ROLE_ID = ekipUser.EXECUTE_ROLE_ID;
                            exe.EXECUTE_ROLE_NAME = ekipUser.EXECUTE_ROLE_NAME;
                            exe.EXECUTE_TIME = req.FINISH_TIME.ToString().Substring(4, 4);     // EXECUTE_TIME
                            exe.PTTT_GROUP_ID = listSereServPttt != null && listSereServPttt.Count > 0 ? listSereServPttt.First().PTTT_GROUP_ID ?? 0 : 0;
                            listExeLogins.Add(exe);
                        }
                    }
                }
                // loại bỏ những dịch vụ ko thuộc pttt lọa 1, 2, 3, đb
                listExeLogins = listExeLogins.Where(s => s.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1 || s.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2 || s.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3 || s.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4).ToList();
                // tính ngày
                List<TIME_LINE> listTimeLines = new List<TIME_LINE>();
                long i = 1;
                var day = (DateTime)Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
                var dayyyy = new DAY();
                while (i < 32)
                {
                    var timeLine = new TIME_LINE();
                    timeLine.ID = i;
                    timeLine.DATE_VALUE = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(day).ToString().Substring(4, 4);
                    timeLine.DATE_STRING = day.Day + "/" + day.Month;
                    switch (i)
                    {
                        case 01: dayyyy.DAY_01 = timeLine.DATE_STRING; break;
                        case 02: dayyyy.DAY_02 = timeLine.DATE_STRING; break;
                        case 03: dayyyy.DAY_03 = timeLine.DATE_STRING; break;
                        case 04: dayyyy.DAY_04 = timeLine.DATE_STRING; break;
                        case 05: dayyyy.DAY_05 = timeLine.DATE_STRING; break;
                        case 06: dayyyy.DAY_06 = timeLine.DATE_STRING; break;
                        case 07: dayyyy.DAY_07 = timeLine.DATE_STRING; break;
                        case 08: dayyyy.DAY_08 = timeLine.DATE_STRING; break;
                        case 09: dayyyy.DAY_09 = timeLine.DATE_STRING; break;
                        case 10: dayyyy.DAY_10 = timeLine.DATE_STRING; break;
                        case 11: dayyyy.DAY_11 = timeLine.DATE_STRING; break;
                        case 12: dayyyy.DAY_12 = timeLine.DATE_STRING; break;
                        case 13: dayyyy.DAY_13 = timeLine.DATE_STRING; break;
                        case 14: dayyyy.DAY_14 = timeLine.DATE_STRING; break;
                        case 15: dayyyy.DAY_15 = timeLine.DATE_STRING; break;
                        case 16: dayyyy.DAY_16 = timeLine.DATE_STRING; break;
                        case 17: dayyyy.DAY_17 = timeLine.DATE_STRING; break;
                        case 18: dayyyy.DAY_18 = timeLine.DATE_STRING; break;
                        case 19: dayyyy.DAY_19 = timeLine.DATE_STRING; break;
                        case 20: dayyyy.DAY_20 = timeLine.DATE_STRING; break;
                        case 21: dayyyy.DAY_21 = timeLine.DATE_STRING; break;
                        case 22: dayyyy.DAY_22 = timeLine.DATE_STRING; break;
                        case 23: dayyyy.DAY_23 = timeLine.DATE_STRING; break;
                        case 24: dayyyy.DAY_24 = timeLine.DATE_STRING; break;
                        case 25: dayyyy.DAY_25 = timeLine.DATE_STRING; break;
                        case 26: dayyyy.DAY_26 = timeLine.DATE_STRING; break;
                        case 27: dayyyy.DAY_27 = timeLine.DATE_STRING; break;
                        case 28: dayyyy.DAY_28 = timeLine.DATE_STRING; break;
                        case 29: dayyyy.DAY_29 = timeLine.DATE_STRING; break;
                        case 30: dayyyy.DAY_30 = timeLine.DATE_STRING; break;
                        case 31: dayyyy.DAY_31 = timeLine.DATE_STRING; break;
                    }
                    listTimeLines.Add(timeLine);
                    day = day.AddDays(1);
                    i++;
                }
                ListDay.Add(dayyyy);

                // tính dịch vụ pttt mà nhân viên thực hiện
                // chỉ tính những dịch vụ có loại là loại 1/2/3/đb
                i = 1;
                var listExxeLoginGroupByExecuteRoleIds = listExeLogins.GroupBy(s => s.EXECUTE_ROLE_ID).ToList();
                foreach (var listExxeLoginGroupByExecuteRoleId in listExxeLoginGroupByExecuteRoleIds)
                {
                    var listExeLoginGroupByLoginnames = listExxeLoginGroupByExecuteRoleId.GroupBy(s => s.LOGINNAME).ToList();
                    foreach (var listExeLoginGroupByLoginname in listExeLoginGroupByLoginnames)
                    {
                        var rdo = new Mrs00340RDO();
                        rdo.NUMBER = i;
                        rdo.LOGINNAME = listExeLoginGroupByLoginname.First().LOGINNAME;
                        rdo.USERNAME = listExeLoginGroupByLoginname.First().USERNAME;
                        rdo.EXECUTE_ROLE_NAME = listExeLoginGroupByLoginname.First().EXECUTE_ROLE_NAME;
                        Dictionary<long, Dictionary<string, int>> dicFreq = new Dictionary<long, Dictionary<string, int>>();

                        foreach (var exeLoginGroupByLoginname in listExeLoginGroupByLoginname.OrderBy(s => s.PTTT_GROUP_ID))
                        {
                            var ids = listTimeLines.Where(s => s.DATE_VALUE.Equals(exeLoginGroupByLoginname.EXECUTE_TIME)).ToList();
                            if (ids == null || ids.Count <= 0) continue;
                            var id = ids.FirstOrDefault().ID;
                            if (!dicFreq.ContainsKey(id)) dicFreq[id] = new Dictionary<string, int>();
                            string key = "";
                            if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) key = "1";
                            if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) key = "2";
                            if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) key = "3";
                            if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) key = "4";
                            if (!dicFreq[id].ContainsKey(key)) dicFreq[id][key] = 0;
                            dicFreq[id][key]++;
                            //nếu không chọn thống kê theo số lần
                            if (castFilter.IS_SHOW_FREQUENCE != true)
                            {
                                switch (id)
                                {
                                    case 1:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_01 = rdo.DAY_01 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_01 = rdo.DAY_01 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_01 = rdo.DAY_01 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_01 = rdo.DAY_01 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 2:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_02 = rdo.DAY_02 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_02 = rdo.DAY_02 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_02 = rdo.DAY_02 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_02 = rdo.DAY_02 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 3:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_03 = rdo.DAY_03 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_03 = rdo.DAY_03 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_03 = rdo.DAY_03 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_03 = rdo.DAY_03 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 4:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_04 = rdo.DAY_04 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_04 = rdo.DAY_04 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_04 = rdo.DAY_04 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_04 = rdo.DAY_04 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 5:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_05 = rdo.DAY_05 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_05 = rdo.DAY_05 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_05 = rdo.DAY_05 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_05 = rdo.DAY_05 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 6:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_06 = rdo.DAY_06 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_06 = rdo.DAY_06 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_06 = rdo.DAY_06 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_06 = rdo.DAY_06 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 7:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_07 = rdo.DAY_07 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_07 = rdo.DAY_07 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_07 = rdo.DAY_07 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_07 = rdo.DAY_07 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 8:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_08 = rdo.DAY_08 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_08 = rdo.DAY_08 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_08 = rdo.DAY_08 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_08 = rdo.DAY_08 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 9:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_09 = rdo.DAY_09 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_09 = rdo.DAY_09 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_09 = rdo.DAY_09 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_09 = rdo.DAY_09 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 10:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_10 = rdo.DAY_10 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_10 = rdo.DAY_10 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_10 = rdo.DAY_10 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_10 = rdo.DAY_10 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;

                                    case 11:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_11 = rdo.DAY_11 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_11 = rdo.DAY_11 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_11 = rdo.DAY_11 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_11 = rdo.DAY_11 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 12:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_12 = rdo.DAY_12 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_12 = rdo.DAY_12 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_12 = rdo.DAY_12 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_12 = rdo.DAY_12 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 13:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_13 = rdo.DAY_13 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_13 = rdo.DAY_13 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_13 = rdo.DAY_13 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_13 = rdo.DAY_13 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 14:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_14 = rdo.DAY_14 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_14 = rdo.DAY_14 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_14 = rdo.DAY_14 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_14 = rdo.DAY_14 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 15:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_15 = rdo.DAY_15 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_15 = rdo.DAY_15 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_15 = rdo.DAY_15 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_15 = rdo.DAY_15 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 16:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_16 = rdo.DAY_16 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_16 = rdo.DAY_16 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_16 = rdo.DAY_16 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_16 = rdo.DAY_16 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 17:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_17 = rdo.DAY_17 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_17 = rdo.DAY_17 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_17 = rdo.DAY_17 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_17 = rdo.DAY_17 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 18:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_18 = rdo.DAY_18 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_18 = rdo.DAY_18 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_18 = rdo.DAY_18 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_18 = rdo.DAY_18 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 19:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_19 = rdo.DAY_19 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_19 = rdo.DAY_19 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_19 = rdo.DAY_19 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_19 = rdo.DAY_19 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 20:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_20 = rdo.DAY_20 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_20 = rdo.DAY_20 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_20 = rdo.DAY_20 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_20 = rdo.DAY_20 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;

                                    case 21:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_21 = rdo.DAY_21 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_21 = rdo.DAY_21 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_21 = rdo.DAY_21 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_21 = rdo.DAY_21 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 22:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_22 = rdo.DAY_22 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_22 = rdo.DAY_22 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_22 = rdo.DAY_22 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_22 = rdo.DAY_22 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 23:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_23 = rdo.DAY_23 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_23 = rdo.DAY_23 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_23 = rdo.DAY_23 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_23 = rdo.DAY_23 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 24:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_24 = rdo.DAY_24 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_24 = rdo.DAY_24 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_24 = rdo.DAY_24 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_24 = rdo.DAY_24 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 25:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_25 = rdo.DAY_25 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_25 = rdo.DAY_25 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_25 = rdo.DAY_25 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_25 = rdo.DAY_25 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 26:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_26 = rdo.DAY_26 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_26 = rdo.DAY_26 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_26 = rdo.DAY_26 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_26 = rdo.DAY_26 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 27:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_27 = rdo.DAY_27 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_27 = rdo.DAY_27 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_27 = rdo.DAY_27 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_27 = rdo.DAY_27 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 28:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_28 = rdo.DAY_28 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_28 = rdo.DAY_28 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_28 = rdo.DAY_28 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_28 = rdo.DAY_28 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 29:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_29 = rdo.DAY_29 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_29 = rdo.DAY_29 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_29 = rdo.DAY_29 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_29 = rdo.DAY_29 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    case 30:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_30 = rdo.DAY_30 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_30 = rdo.DAY_30 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_30 = rdo.DAY_30 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_30 = rdo.DAY_30 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;

                                    case 31:
                                        if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP1) { rdo.DAY_31 = rdo.DAY_31 + "1"; rdo.TOTAL_GROUP_01 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP2) { rdo.DAY_31 = rdo.DAY_31 + "2"; rdo.TOTAL_GROUP_02 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP3) { rdo.DAY_31 = rdo.DAY_31 + "3"; rdo.TOTAL_GROUP_03 += 1; }
                                        else if (exeLoginGroupByLoginname.PTTT_GROUP_ID == PTTT_GROUP_ID__GROUP4) { rdo.DAY_31 = rdo.DAY_31 + "4"; rdo.TOTAL_GROUP_04 += 1; }
                                        break;
                                    default:
                                        break;

                                }
                            }
                        }
                        //thống kê theo số lần
                        if (castFilter.IS_SHOW_FREQUENCE == true)
                        {
                            rdo.TOTAL_GROUP_01 = dicFreq.SelectMany(o => o.Value.Where(p => p.Key == "1").ToDictionary(q => q.Key, r => r.Value).Values).Sum(s => s);
                            rdo.TOTAL_GROUP_02 = dicFreq.SelectMany(o => o.Value.Where(p => p.Key == "2").ToDictionary(q => q.Key, r => r.Value).Values).Sum(s => s);
                            rdo.TOTAL_GROUP_03 = dicFreq.SelectMany(o => o.Value.Where(p => p.Key == "3").ToDictionary(q => q.Key, r => r.Value).Values).Sum(s => s);
                            rdo.TOTAL_GROUP_04 = dicFreq.SelectMany(o => o.Value.Where(p => p.Key == "4").ToDictionary(q => q.Key, r => r.Value).Values).Sum(s => s);
                            if (dicFreq.ContainsKey(1))
                            {
                                rdo.DAY_01 = string.Join(",", dicFreq[1].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(2))
                            {
                                rdo.DAY_02 = string.Join(",", dicFreq[2].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(3))
                            {
                                rdo.DAY_03 = string.Join(",", dicFreq[3].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(4))
                            {
                                rdo.DAY_04 = string.Join(",", dicFreq[4].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(5))
                            {
                                rdo.DAY_05 = string.Join(",", dicFreq[5].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(6))
                            {
                                rdo.DAY_06 = string.Join(",", dicFreq[6].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(7))
                            {
                                rdo.DAY_07 = string.Join(",", dicFreq[7].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(8))
                            {
                                rdo.DAY_08 = string.Join(",", dicFreq[8].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(9))
                            {
                                rdo.DAY_09 = string.Join(",", dicFreq[9].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(10))
                            {
                                rdo.DAY_10 = string.Join(",", dicFreq[10].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(11))
                            {
                                rdo.DAY_11 = string.Join(",", dicFreq[11].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(12))
                            {
                                rdo.DAY_12 = string.Join(",", dicFreq[12].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(13))
                            {
                                rdo.DAY_13 = string.Join(",", dicFreq[13].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(14))
                            {
                                rdo.DAY_14 = string.Join(",", dicFreq[14].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(15))
                            {
                                rdo.DAY_15 = string.Join(",", dicFreq[15].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(16))
                            {
                                rdo.DAY_16 = string.Join(",", dicFreq[16].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(17))
                            {
                                rdo.DAY_17 = string.Join(",", dicFreq[17].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(18))
                            {
                                rdo.DAY_18 = string.Join(",", dicFreq[18].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(19))
                            {
                                rdo.DAY_19 = string.Join(",", dicFreq[19].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(20))
                            {
                                rdo.DAY_20 = string.Join(",", dicFreq[20].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(21))
                            {
                                rdo.DAY_21 = string.Join(",", dicFreq[21].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(22))
                            {
                                rdo.DAY_22 = string.Join(",", dicFreq[22].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(23))
                            {
                                rdo.DAY_23 = string.Join(",", dicFreq[23].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(24))
                            {
                                rdo.DAY_24 = string.Join(",", dicFreq[24].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(25))
                            {
                                rdo.DAY_25 = string.Join(",", dicFreq[25].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(26))
                            {
                                rdo.DAY_26 = string.Join(",", dicFreq[26].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(27))
                            {
                                rdo.DAY_27 = string.Join(",", dicFreq[27].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(28))
                            {
                                rdo.DAY_28 = string.Join(",", dicFreq[28].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(29))
                            {
                                rdo.DAY_29 = string.Join(",", dicFreq[29].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(30))
                            {
                                rdo.DAY_30 = string.Join(",", dicFreq[30].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }
                            if (dicFreq.ContainsKey(31))
                            {
                                rdo.DAY_31 = string.Join(",", dicFreq[31].Select(o => string.Format("{0}({1})", o.Key, o.Value)).ToList());
                            }

                        }
                        ListRdo.Add(rdo);
                        i++;
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }

                if (IsNotNullOrEmpty(castFilter.REQ_ROOM_IDs))
                {
                    dicSingleTag.Add("REQ_ROOM_IDs", "Phòng chỉ định:" + String.Join(",", castFilter.REQ_ROOM_IDs.ToArray()));
                }

                if (IsNotNullOrEmpty(castFilter.EXE_ROOM_IDs))
                {
                    dicSingleTag.Add("EXE_ROOM_IDs", "Phòng thực hiện:" + String.Join(",", castFilter.EXE_ROOM_IDs.ToArray()));
                }

                dicSingleTag.Add("CREATE_TIME", "Ngày " + DateTime.Now.Day + " tháng " + DateTime.Now.Month + " năm " + DateTime.Now.Year);

                objectTag.AddObjectData(store, "Day", ListDay);
                objectTag.AddObjectData(store, "Rdo", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

}
