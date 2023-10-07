using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;

using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServTein;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisTestIndex;
using MOS.MANAGER.HisReportTypeCat;

namespace MRS.Processor.Mrs00484
{
    class Mrs00484Processor : AbstractProcessor
    {
        Mrs00484Filter castFilter = null;

        List<Mrs00484RDO> listRdo = new List<Mrs00484RDO>();
        Dictionary<string, Mrs00484RDO> dicRdo = new Dictionary<string, Mrs00484RDO>();
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();

        public Mrs00484Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        const int MAX_COUNT = 200;
        Dictionary<long, TestIndexNumOrder> dicTestIndex = new Dictionary<long, TestIndexNumOrder>();

        List<SSE> ListSereServExt = new List<SSE>();
        List<Mrs00484RDO> ListSereServ = new List<Mrs00484RDO>();
        List<V_HIS_TEST_INDEX> listTestIndex = new List<V_HIS_TEST_INDEX>();

        HIS_DEPARTMENT department = null;

        string KeyGroup = "{0}_{1}_{2}";//khoi tao gop theo treatment_id, ngay  y lenh, ma y lenh

        string CategoryName = "";
        string CategoryCode = "";

        public override Type FilterType()
        {
            return typeof(Mrs00484Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00484Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao MRS00484: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                ListSereServ = new ManagerSql().GetRdo(castFilter);
                ListSereServExt = new ManagerSql().GetRdoExt(castFilter);

                //get dịch vụ máy
                GetServiceMachine();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();

                GetTestIndex();
                if (castFilter.DEPARTMENT_ID.HasValue)
                {
                    department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == castFilter.DEPARTMENT_ID.Value);
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00484");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = dicServiceMachine.Values.SelectMany(p => p.ToList()).Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                ListSereServ = ListSereServ.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
                var sereServIds = ListSereServ.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServExt = ListSereServExt.Where(o => sereServIds.Contains(o.SERE_SERV_ID ?? 0)).ToList();
            }
            if (castFilter.EXECUTE_MACHINE_IDs != null)
            {
                ListSereServExt = ListSereServExt.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.MACHINE_ID ?? 0)).ToList();
                var sereServIds = ListSereServExt.Select(o => o.SERE_SERV_ID).ToList();
                ListSereServ = ListSereServ.Where(o => sereServIds.Contains(o.SERE_SERV_ID)).ToList();
            }
        }

        private void GetMachine()
        {
            string query = "select * from his_machine where is_delete=0";
            ListMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MACHINE>(query) ?? new List<HIS_MACHINE>();
        }

        private void GetServiceMachine()
        {
            string query = "select * from his_service_machine where is_delete=0";
            var ListServiceMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_MACHINE>(query);
            if (ListServiceMachine != null && ListServiceMachine.Count > 0)
            {
                dicServiceMachine = ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }

        private void GetTestIndex()
        {
            HisTestIndexViewFilterQuery serviceRetyFilter = new HisTestIndexViewFilterQuery();
            listTestIndex = new MOS.MANAGER.HisTestIndex.HisTestIndexManager().GetView(serviceRetyFilter);
            Inventec.Common.Logging.LogSystem.Info("listTestIndex" + listTestIndex.Count);

            //lọc chỉ lấy các chỉ số trong nhóm báo cáo
            if (castFilter.REPORT_TYPE_CAT_ID != null || castFilter.REPORT_TYPE_CAT_IDs != null)
            {
                HisServiceRetyCatViewFilterQuery RetyFilter = new HisServiceRetyCatViewFilterQuery();
                RetyFilter.REPORT_TYPE_CODE__EXACT = "MRS00484";
                RetyFilter.REPORT_TYPE_CAT_ID = castFilter.REPORT_TYPE_CAT_ID;
                RetyFilter.REPORT_TYPE_CAT_IDs = castFilter.REPORT_TYPE_CAT_IDs;
                var listServiceRetyCat = new HisServiceRetyCatManager().GetView(RetyFilter) ?? new List<V_HIS_SERVICE_RETY_CAT>();
                CategoryCode = string.Join(", ", listServiceRetyCat.Select(o => o.CATEGORY_CODE).Distinct().ToList());
                CategoryName = string.Join(", ", listServiceRetyCat.Select(o => o.CATEGORY_NAME).Distinct().ToList());
                var serviceIds = listServiceRetyCat.Select(o => o.SERVICE_ID).Distinct().ToList();
                listTestIndex = listTestIndex.Where(o => serviceIds.Contains(o.TEST_SERVICE_TYPE_ID)).OrderBy(o => o.TEST_SERVICE_TYPE_ID).ToList();

            }
            //lọc chỉ lấy các chỉ số của dịch vụ có trong báo cáo
            if (castFilter.IS_NOT_SHOW_ALL == true)
            {
                var serviceIds = ListSereServExt.Where(o => !string.IsNullOrWhiteSpace(o.VALUE)).Select(o => o.SERVICE_ID).Distinct().ToList();
                listTestIndex = listTestIndex.Where(o => serviceIds.Contains(o.TEST_SERVICE_TYPE_ID)).OrderBy(o => o.TEST_SERVICE_TYPE_ID).ToList();
            }

        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListSereServExt))
                {
                    if (IsNotNullOrEmpty(ListSereServExt))
                    {
                        foreach (var item in ListSereServExt)
                        {
                            if (item.VALUE == null) continue;
                            if (item.TEST_INDEX_ID == null) continue;
                            if (dicTestIndex.ContainsKey(item.TEST_INDEX_ID ?? 0)) continue;
                            var testIndex = listTestIndex.FirstOrDefault(o => o.ID == item.TEST_INDEX_ID);
                            if (testIndex == null) continue;
                            dicTestIndex[item.TEST_INDEX_ID ?? 0] = new TestIndexNumOrder()
                            {
                                TestIndexId = testIndex.ID,
                                TestIndexCode = testIndex.TEST_INDEX_CODE,
                                TestIndexName = testIndex.TEST_INDEX_NAME,
                                TestName = testIndex.SERVICE_NAME
                            };
                        }
                        Inventec.Common.Logging.LogSystem.Info("dicTestIndex" + dicTestIndex.Count);
                    }


                    if (dicTestIndex.Count > 0)
                    {
                        var listIndex = dicTestIndex.Select(s => s.Value).OrderByDescending(o => o.TestName).ToList();
                        long index = 1;
                        string oldName = "";
                        foreach (var item in listIndex)
                        {
                            if (oldName == item.TestName) item.merge = -1;
                            else item.merge = 0;
                            item.Order = index;
                            oldName = item.TestName;
                            index++;
                        }
                        dicTestIndex = new Dictionary<long, TestIndexNumOrder>();
                        dicTestIndex = listIndex.ToDictionary(o => o.TestIndexId);
                    }
                    if (castFilter.IS_MERGE_TREATMENT == true)
                    {
                        //gop theo treatment_id
                        KeyGroup = KeyGroup.Replace("_{1}_{2}", "");
                    }
                    //if (castFilter.IS_SPLIT_SV != true)
                    {
                        //gop theo treatment_id va ngay y lenh
                        //KeyGroup = KeyGroup.Replace("_{2}", "");
                    }

                    this.ProcessDataDetail();
                    this.ProcessListRdo();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        void ProcessDataDetail()
        {
            try
            {
                foreach (var ss in ListSereServ)
                {
                    ss.DIC_VALUE = new Dictionary<string, string>();
                    string key = string.Format(KeyGroup, ss.TREATMENT_ID, ss.INTRUCTION_TIME_NUM - ss.INTRUCTION_TIME_NUM % 1000000, ss.SERVICE_REQ_CODE);
                    if (!dicRdo.ContainsKey(key))
                    {

                        dicRdo[key] = ss;
                    }
                    var sereServTeinSub = ListSereServExt.Where(o => o.SERE_SERV_ID == ss.SERE_SERV_ID).ToList();
                    var machineExt = ListMachine.Where(p => sereServTeinSub.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                    if (machineExt.Count > 0)
                    {
                        if (string.IsNullOrWhiteSpace(dicRdo[key].EXECUTE_MACHINE_CODE))
                        {
                            dicRdo[key].EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            dicRdo[key].EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                        else
                        {
                            dicRdo[key].EXECUTE_MACHINE_NAME = ";" + string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            dicRdo[key].EXECUTE_MACHINE_CODE = ";" + string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                        
                    }
                    AddExtentKey(ss);
                    AddInfoResult(dicRdo[key], sereServTeinSub);

                    AddServiceName(dicRdo[key], ss.SERVICE_NAME);

                    AddMachine(dicRdo[key], ss.SERVICE_ID);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddMachine(Mrs00484RDO rdo, long serviceId)
        {
            var serviceMachine = dicServiceMachine.ContainsKey(serviceId) ? dicServiceMachine[serviceId] : null;
            if (serviceMachine != null && serviceMachine.Count > 0)
            {
                var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                if (machine.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(rdo.MACHINE_CODE))
                    {
                        rdo.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                        rdo.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                    }
                    else
                    {
                        rdo.MACHINE_NAME += ";" + string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                        rdo.MACHINE_CODE += ";" + string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                    }
                }
            }
        }

        private void AddServiceName(Mrs00484RDO rdo, string NewStr)
        {
            if (string.IsNullOrWhiteSpace(NewStr))
            {
                return;
            }
            if (string.IsNullOrWhiteSpace(rdo.SERVICE_NAME))
            {
                rdo.SERVICE_NAME = NewStr;
            }
            if (!string.Format(",{0},", rdo.SERVICE_NAME).Contains(string.Format(",{0},", NewStr)))
            {
                rdo.SERVICE_NAME += ", " + NewStr;
            }
        }

        private void AddExtentKey(Mrs00484RDO ss)
        {
            try
            {
                var reqRoom = HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == ss.REQUEST_ROOM_ID);
                if (reqRoom != null)
                {
                    ss.ROOM_NAME = reqRoom.ROOM_NAME;
                    ss.DEPARTMENT_NAME = reqRoom.DEPARTMENT_NAME;
                }
                //if(!string.IsNullOrWhiteSpace(ss.INTRUCTION_TIME))
                //{
                //ss.INTRUCTION_TIME += ";";
                //}    
                //ss.INTRUCTION_TIME += Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.INTRUCTION_TIME_NUM);

                //if (!string.IsNullOrWhiteSpace(ss.SERVICE_NAME))
                // {
                //  ss.INTRUCTION_TIME += ";";
                //}
                ss.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.INTRUCTION_TIME_NUM);
                ss.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(ss.FINISH_TIME_NUM ?? 0);
                ss.ICD_ALL = ss.ICD_NAME + "; " + ss.ICD_MAIN_TEXT;
                if (ss.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                {
                    ss.FEMALE_YEAR = ss.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                else
                {
                    ss.MALE_YEAR = ss.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                }
                //Inventec.Common.Logging.LogSystem.Info("dicRdo" + dicRdo.Count);
                if (ss.TDL_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    ss.IS_BHYT = "x";
                }
                else
                {
                    ss.IS_BHYT = " ";
                }
                if (ss.RESULT_TIME != null)
                {
                    ss.RESULT_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(ss.RESULT_TIME ?? 0);
                    ss.RESULT_DATE = (ss.RESULT_TIME ?? 0) - (ss.RESULT_TIME ?? 0) % 1000000;
                }

                var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                ss.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                ss.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == ss.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                ss.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_CODE;
                ss.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddInfoResult(Mrs00484RDO rdo, List<SSE> listTein)
        {

            foreach (var tein in listTein)
            {
                if (string.IsNullOrWhiteSpace(tein.VALUE)) continue;
                if (!dicTestIndex.ContainsKey(tein.TEST_INDEX_ID ?? 0)) continue;
                var testIndex = dicTestIndex[tein.TEST_INDEX_ID ?? 0];
                if (testIndex == null) continue;
                if (testIndex.Order <= MAX_COUNT)
                {
                    System.Reflection.PropertyInfo pi = typeof(Mrs00484RDO).GetProperty("VALUE_" + testIndex.Order);
                    pi.SetValue(rdo, tein.VALUE);
                }
                if (rdo.DIC_VALUE.ContainsKey(testIndex.TestIndexCode)) continue;
                rdo.DIC_VALUE[testIndex.TestIndexCode] = tein.VALUE;

            }
        }


        void ProcessListRdo()
        {
            try
            {
                if (dicRdo.Count > 0)
                {
                    foreach (var dic in dicRdo)
                    {
                        listRdo.Add(dic.Value);
                    }

                    listRdo = listRdo.OrderBy(o => o.RESULT_DATE).ThenBy(o => o.TREATMENT_CODE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (dicTestIndex.Count > 0)
                {
                    foreach (var dic in dicTestIndex)
                    {
                        dicSingleTag.Add("INDEX_NAME_" + dic.Value.Order, dic.Value.TestIndexName);
                        dicSingleTag.Add("TEST_NAME_" + dic.Value.Order, dic.Value.TestName);
                        dicSingleTag.Add("MERGE_" + dic.Value.Order, dic.Value.merge);
                    }
                }

                if (department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", department.DEPARTMENT_NAME);
                }
                dicSingleTag.Add("CATEGORY_CODE", CategoryCode);
                dicSingleTag.Add("CATEGORY_NAME", CategoryName);

                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (castFilter.EXECUTE_ROOM_ID != null)
                {
                    var room = HisRoomCFG.HisRooms.FirstOrDefault(o => (castFilter.EXECUTE_ROOM_ID == o.ID));
                    dicSingleTag.Add("EXECUTE_ROOM_NAME", room.ROOM_NAME);
                }
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(o => o.FINISH_TIME_NUM).ToList());
                objectTag.AddObjectData(store, "Indexs", listTestIndex);
                objectTag.SetUserFunction(store, "MergeManyCellFunc", new MergeCellMany());
                objectTag.SetUserFunction(store, "Element", new RDOElement());
                if (castFilter.EXECUTE_MACHINE_IDs != null)
                {
                    var machine = this.ListMachine.Where(o => castFilter.EXECUTE_MACHINE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("EXECUTE_MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
                }
                if (castFilter.MACHINE_IDs != null)
                {
                    var machine = this.ListMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("MACHINE_NAMEs", string.Join(";", machine.Select(o => o.MACHINE_NAME).ToList()));
                }
                if (castFilter.PATIENT_TYPE_IDs != null)
                {
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
                }
                if (castFilter.TDL_PATIENT_TYPE_IDs != null)
                {
                    var patientType = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => castFilter.TDL_PATIENT_TYPE_IDs.Contains(o.ID)).ToList();
                    dicSingleTag.Add("TDL_PATIENT_TYPE_NAMEs", string.Join(";", patientType.Select(o => o.PATIENT_TYPE_NAME).ToList()));
                }
                MRS.MANAGER.Core.MrsReport.AbsProcessDelegate.ProcessMrs = this.SelectSheet;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SelectSheet(ref Inventec.Common.FlexCellExport.Store store, ref System.IO.MemoryStream resultStream)
        {

            //resultStream.Position = 0;
            //FlexCel.XlsAdapter.XlsFile xls = new FlexCel.XlsAdapter.XlsFile(true);
            //xls.Open(resultStream);
            //try
            //{

            //    if (IsNotNullOrEmpty(listReportTypeCat))
            //    {
            //        var listCategoryCode = listReportTypeCat.Select(o => o.CATEGORY_CODE).Distinct().ToList();
            //        if (listCategoryCode.Count == 1)
            //        {
            //            xls.ActiveSheetByName = listCategoryCode.First();
            //        }
            //        else
            //        {
            //            xls.ActiveSheet = 1;
            //        }
            //    }
            //    else
            //    {
            //        xls.ActiveSheet = 1;
            //    }


            //    xls.Save(resultStream);
            //    //resultStream = result;
            //}
            //catch (Exception ex)
            //{
            //    Inventec.Common.Logging.LogSystem.Error(ex);
            //    xls.ActiveSheet = 1;
            //}
        }
    }

    class TestIndexNumOrder
    {
        public long TestIndexId { get; set; }

        public string TestIndexCode { get; set; }
        public string TestIndexName { get; set; }
        public string TestName { get; set; }
        public long Order { get; set; }
        public int merge { get; set; }
    }

    class MergeCellMany : FlexCel.Report.TFlexCelUserFunction
    {
        public MergeCellMany() { }

        long DateResult;

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long date = Convert.ToInt64(parameters[0]);

                if (DateResult == date)
                {
                    return true;
                }
                else
                {
                    DateResult = date;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex);
            }

            return false;
        }
    }
}
