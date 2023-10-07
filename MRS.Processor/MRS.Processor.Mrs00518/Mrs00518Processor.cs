using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00518;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTransaction;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisReceptionRoom;
using MOS.MANAGER.HisCashierRoom;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisSereServBill;

namespace MRS.Processor.Mrs00518
{
    public class Mrs00518Processor : AbstractProcessor
    {
        private List<Mrs00518RDO> ListRdoDetail = new List<Mrs00518RDO>();
        private List<Mrs00518RDO> ListRdo = new List<Mrs00518RDO>();

        Mrs00518Filter filter = null;

        string thisReportTypeCode = "";

        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        List<HIS_EXECUTE_ROOM> listExecuteRoom = new List<HIS_EXECUTE_ROOM>();
        List<SERE_SERV> listHisSereServ = new List<SERE_SERV>();

        public Mrs00518Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00518Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00518Filter)this.reportFilter;
            try
            {
                //Danh sách phòng
                listRoom = new HisRoomManager().GetView(new HisRoomViewFilterQuery());
                //Danh sách phòng khám
                listExecuteRoom = new HisExecuteRoomManager().Get(new HisExecuteRoomFilterQuery());
                //Danh sách phòng thu ngân
                var listCashierRoom = new HisCashierRoomManager().Get(new HisCashierRoomFilterQuery());

                // Lay cac bn da thanh toan va cac bn BHYT da khoa vien phi (vi mot so bn bhyt ko can thanh toan cung dc lay)
                listHisSereServ = new ManagerSql().GetSereServ(filter);
                var treatmentIds = listHisSereServ.Select(o=>o.TDL_TREATMENT_ID??0).Distinct().ToList();

                //chỉ lấy dịch vụ khám do phòng tiếp đón chỉ định hoặc pttt
                listHisSereServ = listHisSereServ.Where(o => listRoom.Exists(s => s.ID == o.TDL_REQUEST_ROOM_ID
                         && s.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD)
                         || o.TDL_SERVICE_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();

                foreach (var item in listHisSereServ)
                {
                    //neu la kham thi lay phong chi dinh la phong thuc hien
                    //neu la phong tiep don chi dinh thi lay phong thuc hien
                    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH
                        || listRoom.Exists(o => o.ID == item.TDL_REQUEST_ROOM_ID&& o.ROOM_TYPE_ID==IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__TD))
                    {
                        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID??0;
                        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                    }
                }
                //lastPatienttypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                
                //tach cac dich vu o phong kham va phong xu ly khac
                var other = listHisSereServ.Where(o => !listExecuteRoom.Exists(s => s.ROOM_ID == o.TDL_REQUEST_ROOM_ID&&s.IS_EXAM ==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)).ToList();
                listHisSereServ = listHisSereServ.Where(o => listExecuteRoom.Exists(s => s.ROOM_ID == o.TDL_REQUEST_ROOM_ID && s.IS_EXAM == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)).ToList();
                listHisSereServ.AddRange(other);

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
            bool result = false;
            try
            {
                ListRdoDetail = (from r in listHisSereServ select new Mrs00518RDO(r, listRoom)).ToList();
                ListRdoDetail = ListRdoDetail.GroupBy(o => new { o.TDL_TREATMENT_ID, o.ROOM_NAME, o.SERE_SERV_ID }).Select(p => p.First()).ToList();
                GroupByRoom();
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdoDetail = new List<Mrs00518RDO>();
                result = false;
            }
            return result;
        }

        private void GroupByRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdoDetail.GroupBy(o => new { o.TDL_REQUEST_ROOM_ID }).ToList();
                //ListRdoDetail.Clear();
                decimal sum = 0;
                Mrs00518RDO rdo;
                List<Mrs00518RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00518RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00518RDO();
                    listSub = item.ToList<Mrs00518RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("DAY_OUT"))
                        {
                            sum = listSub.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else if (field.Name.Contains("COUNT"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private Mrs00518RDO IsMeaningful(List<Mrs00518RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00518RDO();
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "ReportDetail", ListRdoDetail);
            objectTag.AddObjectData(store, "Report", ListRdo);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }



    }

}
