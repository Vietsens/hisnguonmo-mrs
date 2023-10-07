using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisSereServ;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MRS.Proccessor.Mrs00530;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MRS.MANAGER.Core.MrsReport.RDO;
using MOS.MANAGER.HisServiceMety;
using MOS.MANAGER.HisServiceMaty;
using MOS.MANAGER.HisServiceRetyCat;
using System.Reflection;
using Inventec.Common.Repository;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExecuteRoom;
using MOS.MANAGER.HisRoom;


namespace MRS.Processor.Mrs00530
{
    public class Mrs00530Processor : AbstractProcessor
    {
        private List<Mrs00530RDO> ListRdo = new List<Mrs00530RDO>();
        private List<Mrs00530RDO> ListRdoTreatmentEndRoom = new List<Mrs00530RDO>();
        private List<Mrs00530RDO> ListRdoTreatmentExecuteRoom = new List<Mrs00530RDO>();
        private List<Mrs00530RDO> ListRdoTreatmentBhytEndRoom = new List<Mrs00530RDO>();
        private List<V_HIS_TREATMENT_4> Treatments = new List<V_HIS_TREATMENT_4>();
        Dictionary<long, List<HIS_SERVICE_METY>> dicServiceMety = new Dictionary<long, List<HIS_SERVICE_METY>>();
        Dictionary<long, List<HIS_SERVICE_MATY>> dicServiceMaty = new Dictionary<long, List<HIS_SERVICE_MATY>>();
        Mrs00530Filter filter = null;
        List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new List<V_HIS_SERVICE_RETY_CAT>();
        List<V_HIS_ROOM> listHisRoom = new List<V_HIS_ROOM>();
        List<HIS_EXECUTE_ROOM> listHisExecuteRoom = new List<HIS_EXECUTE_ROOM>();
        string thisReportTypeCode = "";
		
		List<V_HIS_SERE_SERV> listHisSereServ = new List<V_HIS_SERE_SERV>();
		
        public Mrs00530Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            thisReportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00530Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            filter = (Mrs00530Filter)this.reportFilter;
            try
            {
                //Danh sach phong
                HisRoomViewFilterQuery listHisRoomfilter = new HisRoomViewFilterQuery();
                listHisRoom = new HisRoomManager(new CommonParam()).GetView(listHisRoomfilter);
                //Danh sach phong thuc hien
                HisExecuteRoomFilterQuery listHisExecuteRoomfilter = new HisExecuteRoomFilterQuery();
                listHisExecuteRoom = new HisExecuteRoomManager(new CommonParam()).Get(listHisExecuteRoomfilter);

                // Ho so dieu tri
                HisTreatmentView4FilterQuery filtermain = new HisTreatmentView4FilterQuery();
                filtermain.FEE_LOCK_TIME_FROM = filter.TIME_FROM;
                filtermain.FEE_LOCK_TIME_TO = filter.TIME_TO;
                filtermain.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                Treatments = new HisTreatmentManager(new CommonParam()).GetView4(filtermain);
                // dien dieu tri cuoi cung
                if (Treatments != null && Treatments.Count > 0)
                {
                    var skip = 0;
                    while (Treatments.Count - skip > 0)
                    {
                        var Ids = Treatments.Select(o => o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }
                lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                //Lay danh sach BN theo dien dieu tri
                if (filter.TREATMENT_TYPE_ID != null)
                    Treatments = Treatments.Where(o => lastHisPatientTypeAlter.Exists(p => p.TREATMENT_ID == o.ID && p.TREATMENT_TYPE_ID == filter.TREATMENT_TYPE_ID)).ToList();
                if (filter.TREATMENT_STT_ID.HasValue)
                {
                    if (filter.TREATMENT_STT_ID.Value == 0)
                    {
                        // trạng thái điều trị : Ðang điều trị
                        Treatments = Treatments.Where(o => o.TREATMENT_STT_ID == 1).ToList();
                       
                    }
                    else if (filter.TREATMENT_STT_ID.Value == 1)
                    {
                        // trạng thái điều trị : Kết thúc điều trị
                        Treatments = Treatments.Where(o => o.TREATMENT_STT_ID == 2).ToList();
                    }
                    else
                    {
                    // trạng thái điều trị : tất cả
                        Treatments = Treatments.ToList();
                        
                     
                    }
                }
                else
                {
                    Treatments = Treatments.ToList();
                }

                // sere_serv tuong ung
                if (Treatments != null && Treatments.Count > 0)
                {
                    var skip = 0;
                    while (Treatments.Count - skip > 0)
                    {
                        var Ids = Treatments.Select(o => o.ID).Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                        sereServFilter.TREATMENT_IDs = Ids;
                        sereServFilter.REQUEST_ROOM_IDs = this.filter.EXAM_ROOM_IDs;
                        sereServFilter.PATIENT_TYPE_ID = this.filter.PATIENT_TYPE_ID;
                        sereServFilter.HAS_EXECUTE = true;
                        var listSereServSub = new HisSereServManager(param).GetView(sereServFilter);
                        if (listSereServSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listSereServSub GetView null");
                        else
                        listHisSereServ.AddRange(listSereServSub);
                    }
                }
               

                //Dinh muc thuoc hao phi
                List<HIS_SERVICE_METY> listServiceMety = new List<HIS_SERVICE_METY>();
                HisServiceMetyFilterQuery serviceMetyFilter = new HisServiceMetyFilterQuery();
                listServiceMety = new HisServiceMetyManager().Get(serviceMetyFilter);

                ////neu la kham thi lay phong chi dinh la phong thuc hien
                //foreach (var item in listHisSereServ)
                //{
                //    if (item.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH)
                //    {
                //        item.TDL_REQUEST_DEPARTMENT_ID = item.TDL_EXECUTE_DEPARTMENT_ID;
                //        item.TDL_REQUEST_ROOM_ID = item.TDL_EXECUTE_ROOM_ID;
                //        item.REQUEST_DEPARTMENT_CODE = item.EXECUTE_DEPARTMENT_CODE;
                //        item.REQUEST_ROOM_CODE = item.EXECUTE_ROOM_CODE;
                //        item.REQUEST_DEPARTMENT_NAME = item.EXECUTE_DEPARTMENT_NAME;
                //        item.REQUEST_ROOM_NAME = item.EXECUTE_ROOM_NAME;
                //    }

                //}
                foreach (var item in listServiceMety)
                {
                    if (!dicServiceMety.ContainsKey(item.SERVICE_ID)) dicServiceMety[item.SERVICE_ID] = new List<HIS_SERVICE_METY>();
                    dicServiceMety[item.SERVICE_ID].Add(item);
                }

                //Dinh muc vat tu hao phi
                List<HIS_SERVICE_MATY> listServiceMaty = new List<HIS_SERVICE_MATY>();
                HisServiceMatyFilterQuery serviceMatyFilter = new HisServiceMatyFilterQuery();
                listServiceMaty = new HisServiceMatyManager().Get(serviceMatyFilter);
                foreach (var item in listServiceMaty)
                {
                    if (!dicServiceMaty.ContainsKey(item.SERVICE_ID)) dicServiceMaty[item.SERVICE_ID] = new List<HIS_SERVICE_MATY>();
                    dicServiceMaty[item.SERVICE_ID].Add(item);
                }
                HisServiceRetyCatViewFilterQuery HisServiceRetyCatViewfilter = new HisServiceRetyCatViewFilterQuery();
                HisServiceRetyCatViewfilter.REPORT_TYPE_CODE__EXACT = this.ReportTypeCode;
                listServiceRetyCat = new HisServiceRetyCatManager().GetView(HisServiceRetyCatViewfilter);

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
                ListRdo = (from r in listHisSereServ select new Mrs00530RDO(r, dicServiceMety, dicServiceMaty, listServiceRetyCat ?? new List<V_HIS_SERVICE_RETY_CAT>(), this.filter, Treatments, lastHisPatientTypeAlter, listHisRoom, listHisExecuteRoom)).ToList();
                ListRdo = ListRdo.OrderByDescending(o=>o.ROOM_NUM_ORDER).ToList();
                GroupByTreatmentEndRoom();
                GroupByTreatmentExecuteRoom();
                GroupByTreatmentBhytEndRoom();
                GroupByEndRoomID();
                AddInfoExecuteRoomAndBhytEndRoom();
			result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                ListRdo = new List<Mrs00530RDO>();
                result = false;
            }
            return result;
        }
        private void GroupByTreatmentEndRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.END_ROOM_ID }).ToList();

                Decimal sum = 0;
                Mrs00530RDO rdo;
                List<Mrs00530RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00530RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00530RDO();
                    listSub = item.ToList<Mrs00530RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoTreatmentEndRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByTreatmentExecuteRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.TDL_EXECUTE_ROOM_ID }).ToList();

                Decimal sum = 0;
                Mrs00530RDO rdo;
                List<Mrs00530RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00530RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00530RDO();
                    listSub = item.ToList<Mrs00530RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoTreatmentExecuteRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByTreatmentBhytEndRoom()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.TDL_TREATMENT_ID, o.TREATMENT_BHYT_END_ROOM_ID }).ToList();

                Decimal sum = 0;
                Mrs00530RDO rdo;
                List<Mrs00530RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00530RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00530RDO();
                    listSub = item.ToList<Mrs00530RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdoTreatmentBhytEndRoom.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }

        private void GroupByEndRoomID()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.END_ROOM_ID }).ToList();
                ListRdo.Clear();

                Decimal sum = 0;
                Mrs00530RDO rdo;
                List<Mrs00530RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00530RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00530RDO();
                    listSub = item.ToList<Mrs00530RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("TOTAL_PRICE"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
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
        private Mrs00530RDO IsMeaningful(List<Mrs00530RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00530RDO();
        }

        private void AddInfoExecuteRoomAndBhytEndRoom()
        {
            try
            {
                foreach (var item in ListRdo)
                {
                    item.COUNT_TREATMENT_EXECUTE_ROOM = ListRdoTreatmentExecuteRoom.Where(o => o.TDL_EXECUTE_ROOM_ID == item.END_ROOM_ID).ToList().Count;
                    item.COUNT_TREATMENT_BHYT_END_ROOM = ListRdoTreatmentBhytEndRoom.Where(o => o.TREATMENT_BHYT_END_ROOM_ID == item.END_ROOM_ID).ToList().Count;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", ListRdo);
            objectTag.AddObjectData(store, "ReportTreatment", ListRdoTreatmentEndRoom);
            //objectTag.SetUserFunction(store, "SumData", new RDOSumData());
        }

    }

}
