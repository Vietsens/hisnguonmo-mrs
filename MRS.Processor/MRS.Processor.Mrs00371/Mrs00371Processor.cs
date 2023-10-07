using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using FlexCel.Report;
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
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00371;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using Inventec.Common.Logging;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;

namespace MRS.Processor.Mrs00371
{
    public class Mrs00371Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<Mrs00371RDO> ListRdo = new List<Mrs00371RDO>();
        List<Mrs00371ServiceRDO> listParentService = new List<Mrs00371ServiceRDO>();

        List<Mrs00371RDO> ListGroupRdo = new List<Mrs00371RDO>();
        List<Mrs00371RDO> ListGroupRdo1 = new List<Mrs00371RDO>();
        List<V_HIS_ROOM> listRoom = new List<V_HIS_ROOM>();
        List<HIS_REPORT_TYPE_CAT> listReportTypeCatAll = new List<HIS_REPORT_TYPE_CAT>();
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        Mrs00371Filter castFilter;

        public Mrs00371Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00371Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                castFilter = (Mrs00371Filter)this.reportFilter;

                Inventec.Common.Logging.LogSystem.Info("castFilter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => ((Mrs00371Filter)this.reportFilter)), ((Mrs00371Filter)this.reportFilter)));
                //Get dịch vụ cha
                GetParentService();

                //get dữ liệu chính
                ListRdo = GetMainData(((Mrs00371Filter)this.reportFilter));

                //Get danh sách phòng
                GetRoom();

                //Get danh sách nhóm báo cáo
                GetReportTypeCat();

                //get dịch vụ máy
                GetServiceMachine();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();
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
                ListRdo = ListRdo.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
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

        private void GetReportTypeCat()
        {
            HisReportTypeCatFilterQuery filter = new HisReportTypeCatFilterQuery();
            filter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
            filter.REPORT_TYPE_CODE__EXACT = this.reportType.REPORT_TYPE_CODE;
            listReportTypeCatAll = new HisReportTypeCatManager().Get(filter);
        }

        private void GetRoom()
        {
            listRoom = HisRoomCFG.HisRooms;
        }

        private List<Mrs00371RDO> GetMainData(Mrs00371Filter filter)
        {
            string query = " --chi tiet dich vu cls\n";
            query += "select\n";
            query += "sr.tdl_patient_name PATIENT_NAME,\n";
            query += "sr.FINISH_TIME FINISH_TIME_NUM,\n";
            query += "sr.tdl_patient_code PATIENT_CODE,\n";
            query += "sr.INTRUCTION_TIME INTRUCTION_TIME_NUM,\n";
            query += "sr.tdl_patient_address VIR_ADDRESS,\n";
            query += "ss.PATIENT_TYPE_ID,\n";
            query += "sr.ICD_NAME,\n";
            query += "ss.REQUEST_DEPARTMENT_CODE,\n";
            query += "ss.REQUEST_DEPARTMENT_NAME,\n";
            query += "src.REPORT_TYPE_CAT_ID,\n";
            query += "ss.TDL_REQUEST_DEPARTMENT_ID,\n";
            query += "src.CATEGORY_NAME,\n";
            query += "src.CATEGORY_CODE,\n";
            query += "ss.AMOUNT,\n";
            query += "ss.TDL_REQUEST_ROOM_ID,\n";
            query += "ss.REQUEST_ROOM_CODE,\n";
            query += "ss.REQUEST_ROOM_NAME,\n";
            query += "sr.REQUEST_LOGINNAME,\n";
            query += "sr.REQUEST_USERNAME,\n";
            query += "sr.START_TIME START_TIME_NUM,\n";
            query += "trea.tdl_treatment_type_id TREATMENT_TYPE_ID,\n";
            query += "trea.tdl_patient_type_id,\n";
            query += "sr.service_req_stt_id,\n";
            query += "sr.tdl_patient_gender_id,\n";
            query += "ss.TDL_SERVICE_REQ_CODE,\n";
            query += "ss.tdl_service_code SERVICE_CODE,\n";
            query += "ss.tdl_service_name SERVICE_NAME,\n";
            query += "ss.tdl_service_type_id SERVICE_TYPE_ID,\n";
            query += "sse.machine_id,\n";
            query += "ss.SERVICE_ID\n";
            query += "FROM HIS_RS.V_HIS_SERE_SERV SS\n";
            query += "join HIS_RS.V_HIS_SERVICE_REQ SR on sr.id=ss.service_req_id\n";
            query += "left join HIS_RS.HIS_SERE_SERV_EXT SSE on SS.id=SSE.SERE_SERV_ID\n";
            query += "join HIS_RS.his_treatment trea on trea.id=sr.treatment_id\n";
            query += "join HIS_RS.V_HIS_SERVICE_RETY_CAT SRC on SRC.SERVICE_ID=SS.SERVICE_ID AND SRC.REPORT_TYPE_CODE='MRS00371'\n";
            query += "WHERE SS.IS_DELETE = 0\n";
            query += "AND SR.IS_DELETE = 0\n";
            query += "AND SS.IS_NO_EXECUTE IS NULL\n";
            query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";

            //phòng thực hiện
            if (filter.EXECUTE_ROOM_ID != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID = {0}\n", filter.EXECUTE_ROOM_ID);
            }
            if (filter.EXECUTE_ROOM_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_ROOM_ID in ({0})\n", string.Join(",", filter.EXECUTE_ROOM_IDs));
            }
            //khoa thực hiện
            if (filter.EXECUTE_DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID = {0}\n", filter.EXECUTE_DEPARTMENT_ID);
            }
            if (filter.EXECUTE_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.EXECUTE_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.EXECUTE_DEPARTMENT_IDs));
            }

            //phòng chỉ định
            if (filter.REQUEST_ROOM_ID != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID = {0}\n", filter.REQUEST_ROOM_ID);
            }
            if (filter.REQUEST_ROOM_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_ROOM_ID in ({0})\n", string.Join(",", filter.REQUEST_ROOM_IDs));
            }
            //khoa chỉ định
            if (filter.REQUEST_DEPARTMENT_IDs != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
            }
            //bác sĩ chỉ định
            if (filter.REQUEST_LOGINNAMEs != null)
            {
                query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAMEs));
            }

            if (filter.REQUEST_DEPARTMENT_ID != null)
            {
                query += string.Format("AND SR.REQUEST_DEPARTMENT_ID = {0}\n", filter.REQUEST_DEPARTMENT_ID);
            }

            //đối tượng thanh toán
            if (filter.PATIENT_TYPE_ID != null)
            {
                query += string.Format("AND SS.PATIENT_TYPE_ID = {0}\n", filter.PATIENT_TYPE_ID);
            }

            //đối tượng bệnh nhân
            if (filter.TDL_PATIENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID = {0}\n", filter.TDL_PATIENT_TYPE_ID);
            }
            //thời gian và trạng thái y lệnh
            if (filter.SERVICE_REQ_STT_ID != null)
            {
                if (filter.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL)
                {
                    query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.START_TIME_FROM, filter.TIME_TO ?? filter.START_TIME_TO);
                }
                else if (filter.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL)
                {
                    query += string.Format("AND SR.START_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID<>{2}\n", filter.TIME_FROM ?? filter.START_TIME_FROM, filter.TIME_TO ?? filter.START_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL);
                }
                else
                {
                    query += string.Format("AND SR.FINISH_TIME BETWEEN {0} and {1} AND SR.SERVICE_REQ_STT_ID ={2} \n", filter.TIME_FROM ?? filter.START_TIME_FROM, filter.TIME_TO ?? filter.START_TIME_TO, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                }
            }
            else
            {
                query += string.Format("AND SR.INTRUCTION_TIME BETWEEN {0} and {1} \n", filter.TIME_FROM ?? filter.START_TIME_FROM, filter.TIME_TO ?? filter.START_TIME_TO);
            }

            //loại dịch vụ
            if (filter.SERVICE_TYPE_ID != null)
            {
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", filter.SERVICE_TYPE_ID);
            }

            if (filter.SERVICE_TYPE_IDs != null)
            {
                query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
            }
            //dịch vụ
            if (filter.SERVICE_ID != null)
            {
                query += string.Format("AND SS.SERVICE_ID = {0}\n", filter.SERVICE_ID);
            }
            if (filter.SERVICE_IDs != null)
            {
                query += string.Format("AND SS.SERVICE_ID IN ({0})\n", string.Join(",", filter.SERVICE_IDs));
            }
            if (!string.IsNullOrEmpty(filter.SERVICE_NAME))
            {
                query += string.Format("AND lower(SS.TDL_SERVICE_NAME) like '%{0}%'\n", filter.SERVICE_NAME.ToLower());
            }
            //nhóm dịch vụ
            if (filter.EXACT_PARENT_SERVICE_ID != null)
            {
                query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_RS.HIS_SERVICE WHERE PARENT_ID= {0})\n", filter.EXACT_PARENT_SERVICE_ID);
            }
            if (filter.EXACT_PARENT_SERVICE_IDs != null)
            {
                query += string.Format("AND SS.SERVICE_ID IN (SELECT ID FROM HIS_RS.HIS_SERVICE WHERE PARENT_ID IN ({0}))\n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
            }

            //diện điều trị
            if (filter.TREATMENT_TYPE_ID != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID = {0}\n", filter.TREATMENT_TYPE_ID);
            }
            if (filter.TREATMENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_TREATMENT_TYPE_ID in ({0})", string.Join(",", filter.TREATMENT_TYPE_IDs));
            }

            // chi nhánh
            if (filter.BRANCH_IDs != null)
            {
                query += string.Format("AND TREA.BRANCH_ID in ({0})", string.Join(",", filter.BRANCH_IDs));
            }

            if (filter.BRANCH_ID != null)
            {
                query += string.Format("AND TREA.BRANCH_ID = {0}\n", filter.BRANCH_ID);
            }

            //trạng thái y lệnh
            if (filter.SERVICE_REQ_STT_ID != null)
            {
                query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
            }
            if (filter.SERVICE_REQ_STT_IDs != null)
            {
                query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
            }
            
            //đối tượng bệnh nhân
            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND TREA.TDL_PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }

            //đối tượng thanh toán
            if (filter.PATIENT_TYPE_IDs != null)
            {
                query += string.Format("AND SS.PATIENT_TYPE_ID IN ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
            }

            //đối tượng chi tiết
            if (filter.PATIENT_CLASSIFY_ID != null)
            {
                query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID = {0}\n", filter.PATIENT_CLASSIFY_ID);
            }
            if (filter.PATIENT_CLASSIFY_IDs != null)
            {
                query += string.Format("AND SR.TDL_PATIENT_CLASSIFY_ID IN ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
            }

            //lọc theo máy thực hiện
            if (filter.EXECUTE_MACHINE_IDs != null)
            {
                query += string.Format("AND sse.execute_machine_id in ({0})\n", string.Join(",", filter.EXECUTE_MACHINE_IDs));
            }

            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            return new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00371RDO>(query);

        }

        private void GetParentService()
        {
            string query = "select sv.id, sv.service_type_id, sv.service_code, sv.service_name, (case when pr.id is null then 0 else pr.id end) as PR_ID, (case when pr.service_code is null then 'OTHER' else pr.service_code end) as pr_service_code, (case when pr.service_name is null then 'NHÓM KHÁC' else pr.service_name end) as pr_service_name from his_service pr right join his_service sv on pr.id = sv.parent_id";
            listParentService = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00371ServiceRDO>(query);
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {

                if (IsNotNullOrEmpty(ListRdo))
                {
                    foreach (var serser in ListRdo)
                    {
                        var prService = listParentService.FirstOrDefault(p => p.ID == serser.SERVICE_ID);
                        serser.DIC_PARENT_SERVICE_AMOUNT = new Dictionary<string, decimal>();

                        if (prService != null)
                        {
                            serser.SERVICE_TYPE_ID = prService.SERVICE_TYPE_ID;
                            serser.PARENT_SERVICE_CODE = prService.PR_SERVICE_CODE;
                            serser.PARENT_SERVICE_NAME = prService.PR_SERVICE_NAME;
                        }

                        serser.INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serser.INTRUCTION_TIME_NUM);
                        serser.START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serser.START_TIME_NUM ?? 0);
                        serser.FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serser.FINISH_TIME_NUM ?? 0);


                        if (serser.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            serser.MALE_AGE = Inventec.Common.DateTime.Calculation.AgeCaption(serser.TDL_PATIENT_DOB ?? 0);
                        }
                        else // Tuoi nu
                        {
                            serser.FEMALE_AGE = Inventec.Common.DateTime.Calculation.AgeCaption(serser.TDL_PATIENT_DOB ?? 0);
                        }
                        //var lastPatientTypeAlter = listPatientTypeAlter.LastOrDefault(o => o.TREATMENT_ID == serser.TDL_TREATMENT_ID) ?? new V_HIS_PATIENT_TYPE_ALTER();
                        if (serser.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            serser.IS_BHYRT = 'x';
                        }
                        serser.ROOM_TYPE_ID_STR = (HisRoomCFG.HisRooms.FirstOrDefault(p => p.ID == serser.TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_TYPE_ID.ToString();

                        serser.TDL_SERVICE_REQ_CODE = serser.TDL_SERVICE_REQ_CODE;
                        if (serser.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT)
                        {
                            serser.TDL_SERVICE_REQ_STT = "Hoàn thành";
                        }
                        else if (serser.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__DXL)
                        {
                            serser.TDL_SERVICE_REQ_STT = "Đang xử lý";
                        }
                        else if (serser.SERVICE_REQ_STT_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__CXL)
                        {
                            serser.TDL_SERVICE_REQ_STT = "Chưa xử lý";
                        }

                        var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == serser.PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                        serser.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                        serser.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;

                        var patientType1 = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == serser.TDL_PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                        serser.TDL_PATIENT_TYPE_CODE = patientType1.PATIENT_TYPE_CODE;
                        serser.TDL_PATIENT_TYPE_NAME = patientType1.PATIENT_TYPE_NAME;

                        var serviceMachine = dicServiceMachine.ContainsKey(serser.SERVICE_ID) ? dicServiceMachine[serser.SERVICE_ID] : null;
                        if (serviceMachine != null && serviceMachine.Count > 0)
                        {
                            var machine = ListMachine.Where(p => serviceMachine.Exists(o => p.ID == o.MACHINE_ID)).ToList();
                            if (machine.Count > 0)
                            {
                                serser.MACHINE_NAME = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                                serser.MACHINE_CODE = string.Join(";", machine.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                            }
                        }
                        var machineExt = ListMachine.Where(p => p.ID == serser.MACHINE_ID).ToList();
                        if (machineExt.Count > 0)
                        {
                            serser.EXECUTE_MACHINE_NAME = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_NAME)).Select(p => p.MACHINE_NAME).ToList());
                            serser.EXECUTE_MACHINE_CODE = string.Join(";", machineExt.Where(o => !string.IsNullOrWhiteSpace(o.MACHINE_CODE)).Select(p => p.MACHINE_CODE).ToList());
                        }
                    }
                    GroupListRdo(ListRdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        private void GroupListRdo(List<Mrs00371RDO> listRdo)
        {
            if (listRdo != null)
            {
                var group = listRdo.GroupBy(p => new { p.TDL_REQUEST_DEPARTMENT_ID, p.PARENT_SERVICE_CODE }).Select(q => new Mrs00371RDO
                {
                    TDL_REQUEST_DEPARTMENT_ID = q.First().TDL_REQUEST_DEPARTMENT_ID,
                    REQUEST_DEPARTMENT_CODE = q.First().REQUEST_DEPARTMENT_CODE,
                    REQUEST_DEPARTMENT_NAME = q.First().REQUEST_DEPARTMENT_NAME,
                    PARENT_SERVICE_CODE = q.First().PARENT_SERVICE_CODE,
                    PARENT_SERVICE_NAME = q.First().PARENT_SERVICE_NAME,
                    COUNT_REQ = q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE)).Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count(),
                    COUNT_REQ_COMPLETE = q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE) && p.TDL_SERVICE_REQ_STT == "Hoàn thành").Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count(),
                }).ToList();

                var group1 = listRdo.GroupBy(p => new { p.TDL_REQUEST_DEPARTMENT_ID, p.CATEGORY_CODE }).Select(q => new Mrs00371RDO
                {
                    TDL_REQUEST_DEPARTMENT_ID = q.First().TDL_REQUEST_DEPARTMENT_ID,
                    REQUEST_DEPARTMENT_CODE = q.First().REQUEST_DEPARTMENT_CODE,
                    REQUEST_DEPARTMENT_NAME = q.First().REQUEST_DEPARTMENT_NAME,
                    CATEGORY_CODE = q.First().CATEGORY_CODE,
                    CATEGORY_NAME = q.First().CATEGORY_NAME,
                    COUNT_REQ = q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE)).Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count() > 0 ? q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE)).Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count() : 0,
                    COUNT_REQ_COMPLETE = q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE) && p.TDL_SERVICE_REQ_STT == "Hoàn thành").Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count() > 0 ? q.Where(p => !string.IsNullOrEmpty(p.TDL_SERVICE_REQ_CODE) && p.TDL_SERVICE_REQ_STT == "Hoàn thành").Select(p => p.TDL_SERVICE_REQ_CODE).Distinct().Count() : 0,
                }).ToList();

                if (group != null) ListGroupRdo = group;
                if (group1 != null) ListGroupRdo1 = group1;
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00371Filter)this.reportFilter).TIME_FROM ?? ((Mrs00371Filter)this.reportFilter).START_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00371Filter)this.reportFilter).TIME_TO ?? ((Mrs00371Filter)this.reportFilter).START_TIME_TO ?? 0));
                var listReportTypeCat = ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).ToList();
                if (IsNotNullOrEmpty(listReportTypeCat))
                {
                    dicSingleTag.Add("CATEGORY_NAMEs", String.Join(",", listReportTypeCat.Select(o => o.CATEGORY_NAME).ToList()));
                }

                var listParentService = ListRdo.GroupBy(o => o.CATEGORY_CODE).Select(p => p.First()).OrderBy(q => q.CATEGORY_CODE).ToList();
                for (int i = 0; i < 1000; i++)
                {
                    if (i < listReportTypeCat.Count)
                    {
                        dicSingleTag.Add(string.Format("CATEGORY_CODE_{0}", i + 1), listReportTypeCat[i].CATEGORY_CODE);
                        dicSingleTag.Add(string.Format("CATEGORY_NAME_{0}", i + 1), listReportTypeCat[i].CATEGORY_NAME);
                    }
                }
                var listCategory = ListRdo.Select(p => new { p.CATEGORY_CODE, p.CATEGORY_NAME }).Distinct().OrderBy(p => p.CATEGORY_NAME).ToList();
                for (int i = 0; i < 1000; i++)
                {
                    if (i < listCategory.Count())
                    {
                        dicSingleTag.Add(string.Format("CATEGORY_CODE___{0}", i + 1), listCategory[i].CATEGORY_CODE);
                        dicSingleTag.Add(string.Format("CATEGORY_NAME___{0}", i + 1), listCategory[i].CATEGORY_NAME);
                    }
                }
                dicSingleTag.Add(string.Format("CATEGORY_CODE___{0}", listCategory.Count() + 1), "SUM_ALL");
                dicSingleTag.Add(string.Format("CATEGORY_NAME___{0}", listCategory.Count() + 1), "Tổng");
                dicSingleTag.Add("COUNT_DATA", ListRdo.Count());
                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "RoomAll", listRoom);
                objectTag.AddObjectData(store, "RptcAll", listReportTypeCatAll.OrderBy(o=>o.NUM_ORDER).ToList());
                objectTag.AddObjectData(store, "ReportPK", ListRdo.Where(p => p.ROOM_TYPE_ID_STR == "1").ToList());
                objectTag.AddObjectData(store, "ParentXN", ListRdo.Where(p => p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Select(p => new { p.PARENT_SERVICE_CODE, p.PARENT_SERVICE_NAME }).Distinct().ToList());
                objectTag.AddObjectData(store, "ParentXNPK", ListRdo.Where(p => p.SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN && p.ROOM_TYPE_ID_STR == "1").Select(p => new { p.PARENT_SERVICE_CODE, p.PARENT_SERVICE_NAME }).Distinct().ToList());
                objectTag.AddObjectData(store, "ReportSADoppler", ListRdo.Where(p => p.SERVICE_NAME.ToLower().Contains("siêu âm doppler")).ToList());
                objectTag.AddObjectData(store, "ReportGroup", ListGroupRdo.Where(p => p.COUNT_REQ > 0 || p.COUNT_REQ_COMPLETE > 0).ToList());
                objectTag.AddObjectData(store, "ReportGroup1", ListGroupRdo1.Where(p => p.REQUEST_ROOM_CODE != "KKB").ToList());

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
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}