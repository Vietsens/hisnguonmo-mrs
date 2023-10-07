using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisService;

namespace MRS.Processor.Mrs00617
{
    public class Mrs00617Processor : AbstractProcessor
    {
        Mrs00617Filter castFilter = null;

        private List<Mrs00617RDO> ListRdo = new List<Mrs00617RDO>();
        private List<Mrs00617RDO> ListRdo1 = new List<Mrs00617RDO>();
        private List<Mrs00617NewRDO> ListRdo3 = new List<Mrs00617NewRDO>();
        private List<Mrs00617NewRDO> ListRdo4 = new List<Mrs00617NewRDO>();
        private List<Mrs00617NewRDO> ListRdo2 = new List<Mrs00617NewRDO>();
        List<DATA_GET> ListSereServ = new List<DATA_GET>();


        private long patientTypeIdKsk = 0;
        private List<Mrs00617RDO> ListServiceRdo = new List<Mrs00617RDO>();
        Dictionary<long, V_HIS_ROOM> dicRoom = new Dictionary<long, V_HIS_ROOM>();
        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();
        List<Mrs00617NewRDO> ListParentService = new List<Mrs00617NewRDO>();
        Dictionary<string, Mrs00617NewRDO> dicParent = new Dictionary<string, Mrs00617NewRDO>();
        //List<HIS_SERVICE_REQ> ListServiceReq = new List<HIS_SERVICE_REQ>();
        //List<HIS_TREATMENT> ListTreatment = new List<HIS_TREATMENT>();
        List<HIS_SERVICE_MACHINE> ListServiceMachine = new List<HIS_SERVICE_MACHINE>();
        List<HIS_MACHINE> ListMachine = new List<HIS_MACHINE>();
        Dictionary<long, List<HIS_SERVICE_MACHINE>> dicServiceMachine = new Dictionary<long, List<HIS_SERVICE_MACHINE>>();

        List<Mrs00617NewRDO> listSV = new List<Mrs00617NewRDO>();
        CommonParam paramGet = new CommonParam();
        public Mrs00617Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }
        List<long> SERVICE_TYPE_IDs = new List<long>()
        { 
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN,
        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G,
        };
        public override Type FilterType()
        {
            return typeof(Mrs00617Filter);
        }

        protected override bool GetData()
        {
            castFilter = ((Mrs00617Filter)reportFilter);
            var result = true;
            try
            {
                //listSV = new ManagerSql().GetParent(castFilter);
                patientTypeIdKsk = HisPatientTypeCFG.PATIENT_TYPE_ID__KSK;
                var serviceType = HisServiceTypeCFG.HisServiceTypes;
                if (castFilter.SVT_LIMIT_CODE != null)
                {
                    serviceType = serviceType.Where(o => string.Format(",{0},", castFilter.SVT_LIMIT_CODE).Contains(string.Format(",{0},", o.SERVICE_TYPE_CODE))).ToList();
                }
                this.SERVICE_TYPE_IDs = serviceType.Select(o => o.ID).ToList();
                if (castFilter.SERVICE_TYPE_IDs != null)
                {
                    this.SERVICE_TYPE_IDs = this.SERVICE_TYPE_IDs.Where(o => castFilter.SERVICE_TYPE_IDs.Contains(o)).ToList();
                }
                ListSereServ = new ManagerSql().GetMain(castFilter, this.SERVICE_TYPE_IDs);
                if (castFilter.PATIENT_TYPE_IS_THU_PHI == true & castFilter.PATIENT_TYPE_IS_THU_PHI.HasValue)
                {
                    ListSereServ = ListSereServ.Where(x => x.PATIENT_TYPE_NAME == "Thu phí").ToList();
                }

                //get dịch vụ máy
                GetServiceMachine();

                //get dịch vụ
                GetService();

                //get máy
                GetMachine();

                //lọc theo máy
                FilterByMachine();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void FilterByMachine()
        {
            if (castFilter.MACHINE_IDs != null)
            {
                var serviceIds = ListServiceMachine.Where(o => castFilter.MACHINE_IDs.Contains(o.MACHINE_ID)).Select(q => q.SERVICE_ID).Distinct().ToList();
                ListSereServ = ListSereServ.Where(o => serviceIds.Contains(o.SERVICE_ID)).ToList();
            }
        }

        private void GetService()
        {
            this.ListService = new ManagerSql().GetService();
            this.ListParentService= new ManagerSql().GetParentService();
            this.dicParent = this.ListService.GroupBy(o => o.SERVICE_CODE).ToDictionary(p => p.Key, q => this.ListParentService.FirstOrDefault(r => r.PARENT_SERVICE_ID == q.First().PARENT_ID));
        }

        private void GetMachine()
        {
            string query = "select * from his_machine where is_delete=0";
            ListMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MACHINE>(query) ?? new List<HIS_MACHINE>();
        }

        private void GetServiceMachine()
        {
            string query = "select * from his_service_machine where is_delete=0";
            ListServiceMachine = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_MACHINE>(query);
            if (ListServiceMachine != null && ListServiceMachine.Count > 0)
            {
                dicServiceMachine = ListServiceMachine.GroupBy(o => o.SERVICE_ID).ToDictionary(p => p.Key, q => q.ToList());
            }
        }

        protected override bool ProcessData()
        {
            var result = true;
            try
            {
                /*
                 mô tả các list:
                 * ListSereServ sau khi được truy vấn về, 
                 * sinh ra 3 list: listRdo, listSV, listCheck
                 * listRdo sinh ra 3 list: ListRdo, ListServiceRdo, ListRdo1
                 * ListRdo group listRdo theo khoa chỉ định và dịch vụ
                 * ListServiceRdo group listRdo theo dịch vụ
                 * ListRdo1 gán bằng listRdo luôn
                 * listSV sinh ra ListRdo2 (ListRdo2 group listSV theo tên dịch vụ cha và loại dịch vụ)
                 * listCheck sinh ra ListRdo3 (ListRdo3 group listCheck theo người chỉ định và phòng chỉ định)
                 * listCheck sinh ra ListRdo4 (ListRdo4 group listCheck theo phòng chỉ định)
                 * ListRdo sinh ra service (service group ListRdo theo dịch vụ)
                 */
                List<Mrs00617RDO> listRdo = new List<Mrs00617RDO>();
                List<Mrs00617NewRDO> listCheck = new List<Mrs00617NewRDO>();
                dicRoom = HisRoomCFG.HisRooms.ToDictionary(o => o.ID, p => p);
                LogSystem.Info("TotalGet:" + GC.GetTotalMemory(true) / (1024 * 1024));
                if (ListSereServ != null)
                {
                    foreach (var item in ListSereServ)
                    {
                        //var Treatment = ListTreatment.FirstOrDefault(o => o.ID == item.TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                        Mrs00617RDO rdo = new Mrs00617RDO(item, dicRoom, patientTypeIdKsk, ListMachine, dicServiceMachine,castFilter.IS_AMOUNT==true?item.AMOUNT:1);

                        listRdo.Add(rdo);
                        if (item.HAS_TEST_VALUE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                        {
                            var rdo1 = new Mrs00617NewRDO();
                            rdo1.AMOUNT = item.AMOUNT;
                            rdo1.NUMBER_OF_FILM = item.NUMBER_OF_FILM;
                            rdo1.IN_TIME = item.IN_TIME;

                            rdo1.PARENT_SERVICE_CODE = item.PARENT_SERVICE_CODE;
                            rdo1.PARENT_SERVICE_NAME = item.PARENT_SERVICE_NAME;
                            rdo1.SERVICE_CODE = item.TDL_SERVICE_CODE;
                            rdo1.SERVICE_NAME = item.TDL_SERVICE_NAME;
                            rdo1.PRICE = item.PRICE;
                            rdo1.SERVICE_ID = item.SERVICE_ID;
                            rdo1.TDL_TREATMENT_ID = item.TDL_TREATMENT_ID;
                            rdo1.TDL_SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID;
                            rdo1.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID;
                            rdo1.TDL_PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID;
                            rdo1.TDL_PATIENT_TYPE_NAME = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == item.TDL_PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
                            rdo1.PATIENT_TYPE_NAME = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == item.PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
                            rdo1.PATIENT_TYPE_ID = item.PATIENT_TYPE_ID;
                            rdo1.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB ?? 0;
                            rdo1.SERVICE_REQ_ID = item.SERVICE_REQ_ID;
                            listSV.Add(rdo1);
                        }
                        var rdo2 = new Mrs00617NewRDO();
                        rdo2.TDL_REQUEST_ROOM_CODE = dicRoom.ContainsKey(item.TDL_REQUEST_ROOM_ID) ? dicRoom[item.TDL_REQUEST_ROOM_ID].ROOM_CODE : null;
                        rdo2.TDL_REQUEST_ROOM_NAME = dicRoom.ContainsKey(item.TDL_REQUEST_ROOM_ID) ? dicRoom[item.TDL_REQUEST_ROOM_ID].ROOM_NAME : null;
                        rdo2.REQUEST_LOGINNAME = item.TDL_REQUEST_LOGINNAME;
                        rdo2.REQUEST_USERNAME = item.TDL_REQUEST_USERNAME;
                        rdo2.AMOUNT = item.AMOUNT;
                        rdo2.NUMBER_OF_FILM=item.NUMBER_OF_FILM;

                        rdo2.PARENT_SERVICE_CODE = item.PARENT_SERVICE_CODE;
                        rdo2.PARENT_SERVICE_NAME = item.PARENT_SERVICE_NAME;

                        rdo2.SERVICE_ID = item.SERVICE_ID;
                        rdo2.SERVICE_CODE = item.TDL_SERVICE_CODE;
                        rdo2.SERVICE_NAME = item.TDL_SERVICE_NAME;
                        rdo2.TDL_TREATMENT_ID = item.TDL_TREATMENT_ID;
                        rdo2.TDL_SERVICE_TYPE_ID = item.TDL_SERVICE_TYPE_ID; //loại dịch vụ
                        rdo2.TDL_TREATMENT_TYPE_ID = item.TDL_TREATMENT_TYPE_ID; //loại điều trị
                        rdo2.TDL_PATIENT_TYPE_ID = item.TDL_PATIENT_TYPE_ID;
                        rdo2.TDL_PATIENT_TYPE_NAME = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == item.TDL_PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
                        rdo2.PATIENT_TYPE_NAME = ((HIS_PATIENT_TYPE)((HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault((HIS_PATIENT_TYPE o) => o.ID == item.PATIENT_TYPE_ID)) ?? (new HIS_PATIENT_TYPE()))).PATIENT_TYPE_NAME;
                        rdo2.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB ?? 0;
                        rdo2.TDL_TREATMENT_END_TYPE_ID = item.TREATMENT_END_TYPE_ID;
                        rdo2.SERVICE_REQ_ID = item.SERVICE_REQ_ID;
                        listCheck.Add(rdo2);

                    }
                }


                LogSystem.Info("TotalGet1:" + GC.GetTotalMemory(true) / (1024 * 1024));

                ListRdo = listRdo.GroupBy(o => new { o.HisSereServ.SERVICE_ID, o.HisSereServ.TDL_REQUEST_DEPARTMENT_ID, o.HisSereServ.TDL_SERVICE_NAME }).Select(p => new Mrs00617RDO(p.First().HisSereServ, dicRoom, patientTypeIdKsk, ListMachine, dicServiceMachine, castFilter.IS_AMOUNT == true ? p.Sum(s => s.AMOUNT) : p.Sum(s => 1))
                {
                    TDL_REQUEST_DEPARTMENT_ID = p.First().TDL_REQUEST_DEPARTMENT_ID,
                    TDL_REQUEST_DEPARTMENT_CODE = p.First().TDL_REQUEST_DEPARTMENT_CODE,
                    TDL_REQUEST_DEPARTMENT_NAME = p.First().TDL_REQUEST_DEPARTMENT_NAME,
                    SERVICE_TYPE_ID = p.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME,
                    SERVICE_UNIT_NAME = p.First().SERVICE_UNIT_NAME,
                    AMOUNT = p.Sum(s => s.AMOUNT),
                    AMOUNT_KSK = p.Sum(s => s.AMOUNT_KSK),
                    AMOUNT_IN = p.Sum(s => s.AMOUNT_IN),
                    AMOUNT_EXAM_ROOM = p.Sum(s => s.AMOUNT_EXAM_ROOM),
                    AMOUNT_IN_BHYT = p.Sum(s => s.AMOUNT_IN_BHYT),
                    AMOUNT_IN_VP = p.Sum(s => s.AMOUNT_IN_VP),
                    AMOUNT_IN_CA = p.Sum(s => s.AMOUNT_IN_CA),
                    AMOUNT_IN_NN = p.Sum(s => s.AMOUNT_IN_NN),
                    AMOUNT_OUT = p.Sum(s => s.AMOUNT_OUT),
                    AMOUNT_OUT_BHYT = p.Sum(s => s.AMOUNT_OUT_BHYT),
                    AMOUNT_OUT_VP = p.Sum(s => s.AMOUNT_OUT_VP),
                    AMOUNT_OUT_CA = p.Sum(s => s.AMOUNT_OUT_CA),
                    AMOUNT_OUT_NN = p.Sum(s => s.AMOUNT_OUT_NN),
                    TOTAL_PRICE = p.Sum(s => s.TOTAL_PRICE),
                    VIR_TOTAL_PRICE = p.Sum(s => s.VIR_TOTAL_PRICE),
                    TOTAL_NUMBER_OF_FILM = p.Sum(s => s.TOTAL_NUMBER_OF_FILM)
                }).ToList();


                LogSystem.Info("TotalGet2:" + GC.GetTotalMemory(true) / (1024 * 1024));

                ListServiceRdo = listRdo.GroupBy(o => new { o.HisSereServ.SERVICE_ID }).Select(p => new Mrs00617RDO(p.First().HisSereServ, dicRoom, patientTypeIdKsk, ListMachine, dicServiceMachine, castFilter.IS_AMOUNT == true ? p.Sum(s => s.AMOUNT) : p.Sum(s => 1))
                {
                    SERVICE_TYPE_ID = p.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME,
                    SERVICE_UNIT_NAME = p.First().SERVICE_UNIT_NAME,
                    AMOUNT = p.Sum(s => s.AMOUNT),
                    AMOUNT_KSK = p.Sum(s => s.AMOUNT_KSK),
                    AMOUNT_IN = p.Sum(s => s.AMOUNT_IN),
                    AMOUNT_EXAM_ROOM = p.Sum(s => s.AMOUNT_EXAM_ROOM),
                    AMOUNT_IN_BHYT = p.Sum(s => s.AMOUNT_IN_BHYT),
                    AMOUNT_IN_VP = p.Sum(s => s.AMOUNT_IN_VP),
                    AMOUNT_IN_CA = p.Sum(s => s.AMOUNT_IN_CA),
                    AMOUNT_IN_NN = p.Sum(s => s.AMOUNT_IN_NN),
                    AMOUNT_OUT = p.Sum(s => s.AMOUNT_OUT),
                    AMOUNT_OUT_BHYT = p.Sum(s => s.AMOUNT_OUT_BHYT),
                    AMOUNT_OUT_VP = p.Sum(s => s.AMOUNT_OUT_VP),
                    AMOUNT_OUT_CA = p.Sum(s => s.AMOUNT_OUT_CA),
                    AMOUNT_OUT_NN = p.Sum(s => s.AMOUNT_OUT_NN),
                    TOTAL_PRICE = p.Sum(s => s.TOTAL_PRICE),
                    VIR_TOTAL_PRICE = p.Sum(s => s.VIR_TOTAL_PRICE),
                    TOTAL_NUMBER_OF_FILM = p.Sum(s => s.TOTAL_NUMBER_OF_FILM),
                    DIC_AMOUNT_REQUEST_ROOM = p.GroupBy(o => o.TDL_REQUEST_ROOM_CODE).ToDictionary(r => r.Key, q => q.Sum(s => s.AMOUNT)),
                    DIC_AMOUNT_REQUEST_DEPA = p.GroupBy(o => o.TDL_REQUEST_DEPARTMENT_CODE).ToDictionary(r => r.Key, q => q.Sum(s => s.AMOUNT)),
                }).ToList();

                LogSystem.Info("TotalGet3:" + GC.GetTotalMemory(true) / (1024 * 1024));
                ListRdo1 = listRdo;
                if (listSV != null)
                {
                    var group = listSV.GroupBy(p => new { p.TDL_SERVICE_TYPE_ID, p.PARENT_SERVICE_NAME/*,p.PRICE*/ }).ToList();
                    foreach (var item in group)
                    {
                        List<Mrs00617NewRDO> listSub = item.ToList<Mrs00617NewRDO>();
                        var rdo1 = new Mrs00617NewRDO();
                        rdo1.TDL_SERVICE_TYPE_ID = listSub.First().TDL_SERVICE_TYPE_ID;
                        rdo1.TDL_SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == listSub.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;

                        //rdo1.PRICE = listSub.First().PRICE;
                        rdo1.PARENT_SERVICE_CODE = listSub.First().PARENT_SERVICE_CODE;
                        rdo1.PARENT_SERVICE_NAME = listSub.First().PARENT_SERVICE_NAME;
                        var grandParent = dicParent.ContainsKey(listSub.First().PARENT_SERVICE_CODE ?? "") ? dicParent[listSub.First().PARENT_SERVICE_CODE ?? ""] : null;
                        if (grandParent != null)
                        {
                            rdo1.GRPR_SERVICE_CODE = grandParent.PARENT_SERVICE_CODE;
                            rdo1.GRPR_SERVICE_NAME = grandParent.PARENT_SERVICE_NAME;
                        }
                        rdo1.AMOUNT_NT = listSub.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                        rdo1.AMOUNT_NGT = listSub.Where(o => o.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Sum(s => s.AMOUNT);
                        rdo1.COUNT_TREA_NGT = listSub.Where(P => P.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p).Distinct().Count();
                        rdo1.COUNT_TREA_NT = listSub.Where(P => P.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.TDL_TREATMENT_ID ?? 0).Distinct().Count();
                        ListRdo2.Add(rdo1);
                    }

                }

                LogSystem.Info("TotalGet4:" + GC.GetTotalMemory(true) / (1024 * 1024));
                var list = listCheck.Select(p => new { p.REQUEST_LOGINNAME, p.TDL_REQUEST_ROOM_ID }).Distinct().ToList();
                if (list != null)
                {

                    foreach (var item in list)
                    {
                        var check = listCheck.Where(p => p.TDL_REQUEST_ROOM_ID == item.TDL_REQUEST_ROOM_ID && p.REQUEST_LOGINNAME == item.REQUEST_LOGINNAME).ToList();
                        Mrs00617NewRDO rdo = new Mrs00617NewRDO();
                        rdo.REQUEST_LOGINNAME = item.REQUEST_LOGINNAME;
                        if (check != null)
                        {
                            rdo.REQUEST_USERNAME = check.First().REQUEST_USERNAME;
                            rdo.TDL_REQUEST_ROOM_CODE = check.First().TDL_REQUEST_ROOM_CODE;
                            rdo.TDL_REQUEST_ROOM_NAME = check.First().TDL_REQUEST_ROOM_NAME;
                            rdo.AMOUNT_KHAM = check.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                            rdo.AMOUNT_KHAM_BHYT = check.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                            rdo.AMOUNT_KHAM_VP = check.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                            rdo.AMOUNT_NGT = check.Where(P => P.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                            rdo.AMOUNT_NT = check.Where(P => P.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                            rdo.AMOUNT_CHUYEN = check.Where(P => P.TDL_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                            rdo.DIC_SERVICE_AMOUNT = new Dictionary<string, decimal>();
                            rdo.DIC_SERVICE_AMOUNT = check.Where(o => !string.IsNullOrWhiteSpace(o.SERVICE_CODE)).GroupBy(g => g.SERVICE_CODE).ToDictionary(p => p.Key, y => y.Sum(p => p.AMOUNT));

                            rdo.DIC_PARENT_AMOUNT = new Dictionary<string, decimal>();
                            rdo.DIC_PARENT_AMOUNT = check.Where(o => !string.IsNullOrWhiteSpace(o.PARENT_SERVICE_CODE)).GroupBy(g => g.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, y => y.Sum(p => p.AMOUNT));
                            ListRdo3.Add(rdo);
                        }
                    }
                }

                LogSystem.Info("TotalGet5:" + GC.GetTotalMemory(true) / (1024 * 1024));
                var groupByRoom = listCheck.GroupBy(x => x.TDL_REQUEST_ROOM_ID).ToList();
                if (list != null)
                {

                    foreach (var item in groupByRoom)
                    {
                        Mrs00617NewRDO rdo = new Mrs00617NewRDO();
                        rdo.REQUEST_USERNAME = string.Join(",", item.Select(x => x.REQUEST_USERNAME).Distinct().ToList());
                        rdo.REQUEST_LOGINNAME = string.Join(",", item.Select(x => x.REQUEST_LOGINNAME).Distinct().ToList());
                        rdo.TDL_REQUEST_ROOM_CODE = item.First().TDL_REQUEST_ROOM_CODE;
                        rdo.TDL_REQUEST_ROOM_NAME = item.First().TDL_REQUEST_ROOM_NAME;
                        rdo.AMOUNT_KHAM = item.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.AMOUNT_KHAM_BHYT = item.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.AMOUNT_KHAM_VP = item.Where(p => p.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH && p.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__FEE).Select(p => p.SERVICE_REQ_ID).Distinct().Count();
                        rdo.AMOUNT_NGT = item.Where(P => P.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.AMOUNT_NT = item.Where(P => P.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.AMOUNT_CHUYEN = item.Where(P => P.TDL_TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN).Select(p => p.TDL_TREATMENT_ID).Distinct().Count();
                        rdo.DIC_SERVICE_AMOUNT = new Dictionary<string, decimal>();
                        rdo.DIC_SERVICE_AMOUNT = item.Where(o => !string.IsNullOrWhiteSpace(o.SERVICE_CODE)).GroupBy(g => g.SERVICE_CODE).ToDictionary(p => p.Key, y => y.Sum(p => p.AMOUNT));
                        rdo.DIC_PARENT_AMOUNT = new Dictionary<string, decimal>();
                        rdo.DIC_PARENT_AMOUNT = item.Where(o => !string.IsNullOrWhiteSpace(o.PARENT_SERVICE_CODE)).GroupBy(g => g.PARENT_SERVICE_CODE).ToDictionary(p => p.Key, y => y.Sum(p => p.AMOUNT));
                        rdo.AMOUNT_XUYEN_SO = item.Where(x => x.SERVICE_NAME.Contains("xuyên sọ")).Sum(x => x.AMOUNT);
                        ListRdo4.Add(rdo);
                    }
                }

                LogSystem.Info("TotalGet6:" + GC.GetTotalMemory(true) / (1024 * 1024));
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00617Filter)this.reportFilter).TIME_FROM ?? 0));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00617Filter)this.reportFilter).TIME_TO ?? 0));

            var service = ListRdo.GroupBy(o => o.HisSereServ.SERVICE_ID).Select(p => new Mrs00617RDO(p.First().HisSereServ, dicRoom, patientTypeIdKsk, ListMachine, dicServiceMachine, castFilter.IS_AMOUNT == true ? p.Sum(s => s.AMOUNT) : p.Sum(s => 1))
            {
                SERVICE_TYPE_ID = p.First().SERVICE_TYPE_ID,
                SERVICE_TYPE_NAME = p.First().SERVICE_TYPE_NAME,
                SERVICE_UNIT_NAME = p.First().SERVICE_UNIT_NAME,
                AMOUNT = p.Sum(s => s.AMOUNT),
                AMOUNT_KSK = p.Sum(s => s.AMOUNT_KSK),
                AMOUNT_IN = p.Sum(s => s.AMOUNT_IN),
                AMOUNT_EXAM_ROOM = p.Sum(s => s.AMOUNT_EXAM_ROOM),
                AMOUNT_IN_BHYT = p.Sum(s => s.AMOUNT_IN_BHYT),
                AMOUNT_IN_VP = p.Sum(s => s.AMOUNT_IN_VP),
                AMOUNT_IN_CA = p.Sum(s => s.AMOUNT_IN_CA),
                AMOUNT_IN_NN = p.Sum(s => s.AMOUNT_IN_NN),
                AMOUNT_OUT = p.Sum(s => s.AMOUNT_OUT),
                AMOUNT_OUT_BHYT = p.Sum(s => s.AMOUNT_OUT_BHYT),
                AMOUNT_OUT_VP = p.Sum(s => s.AMOUNT_OUT_VP),
                AMOUNT_OUT_CA = p.Sum(s => s.AMOUNT_OUT_CA),
                AMOUNT_OUT_NN = p.Sum(s => s.AMOUNT_OUT_NN),
                TOTAL_PRICE = p.Sum(s => s.TOTAL_PRICE),
                VIR_TOTAL_PRICE = p.Sum(s => s.VIR_TOTAL_PRICE),
                TOTAL_NUMBER_OF_FILM = p.Sum(s => s.TOTAL_NUMBER_OF_FILM)
            }).ToList();

            objectTag.AddObjectData(store, "Report", service.OrderBy(o => o.HisSereServ.TDL_SERVICE_TYPE_ID).ThenBy(p => p.HisSereServ.TDL_SERVICE_CODE).ToList());

            objectTag.AddObjectData(store, "ReportService", ListServiceRdo.OrderBy(o => o.HisSereServ.TDL_SERVICE_TYPE_ID).ThenBy(p => p.HisSereServ.TDL_SERVICE_CODE).ToList());

            objectTag.AddObjectData(store, "Parent", service.GroupBy(o => o.SERVICE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_TYPE_ID", "SERVICE_TYPE_ID");
            objectTag.AddObjectData(store, "ReportDepa", ListRdo);
            objectTag.AddObjectData(store, "ReportDetail", ListRdo1.OrderBy((Mrs00617RDO q) => q.INTRUCTION_TIME).ToList());
            objectTag.AddObjectData(store, "ParentDetail", (from o in ListRdo1
                                                            group o by o.TDL_REQUEST_DEPARTMENT_CODE into p
                                                            select p.First()).ToList());
            objectTag.AddRelationship(store, "ParentDetail", "ReportDetail", "TDL_REQUEST_DEPARTMENT_CODE", "TDL_REQUEST_DEPARTMENT_CODE");
            objectTag.AddObjectData(store, "ReportParent", ListRdo2.OrderBy(p => p.TDL_SERVICE_TYPE_ID).ThenBy(p => p.PARENT_SERVICE_CODE).ToList());
            objectTag.AddObjectData(store, "ServiceParent", ListRdo2.GroupBy(p => p.TDL_SERVICE_TYPE_ID).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ServiceParent", "ReportParent", "TDL_SERVICE_TYPE_ID", "TDL_SERVICE_TYPE_ID");


            LogSystem.Info("TotalGet7:" + GC.GetTotalMemory(true) / (1024 * 1024));
            string x = "";
            var Service = ListService;
            var ParentService = this.ListParentService;
            if (ListSereServ != null)
            {
                var serviceIds = ListSereServ.Select(o => o.SERVICE_ID).Distinct().ToList();
                var listService = Service.Where(p => serviceIds.Contains(p.ID)).OrderBy(p => p.SERVICE_CODE).ToList();
                int a = 1;
                if (listService != null)
                {
                    foreach (var item in listService)
                    {
                        dicSingleTag.Add("SERVICE_CODE_" + a, item.SERVICE_CODE);
                        dicSingleTag.Add("SERVICE_NAME_" + a, item.SERVICE_NAME);

                        a++;
                    }
                    for (int i = a; i < 200; i++)
                    {
                        dicSingleTag.Add("SERVICE_CODE_" + i, x);
                        dicSingleTag.Add("SERVICE_NAME_" + i, x);
                    }
                }

                var serviceParIds = ListSereServ.Select(o => o.PARENT_SERVICE_ID ?? 0).Distinct().ToList();
                var listParent = ParentService.Where(p => serviceParIds.Contains(p.PARENT_SERVICE_ID ?? 0)).OrderBy(p => p.PARENT_SERVICE_CODE).ToList();
                int b = 1;
                if (listParent != null)
                {
                    foreach (var item in listParent)
                    {
                        dicSingleTag.Add("PR_SERVICE_CODE_" + b, item.PARENT_SERVICE_CODE);
                        dicSingleTag.Add("PR_SERVICE_NAME_" + b, item.PARENT_SERVICE_NAME);

                        b++;
                    }
                    for (int i = b; i < 200; i++)
                    {
                        dicSingleTag.Add("PR_SERVICE_CODE_" + i, x);
                        dicSingleTag.Add("PR_SERVICE_NAME_" + i, x);
                    }
                }
            }

            objectTag.AddObjectData(store, "CLS", ListRdo3.OrderBy(p => p.TDL_REQUEST_ROOM_CODE).ThenBy(p => p.REQUEST_LOGINNAME).ToList());
            objectTag.AddObjectData(store, "ReportCLS", ListRdo4.OrderBy(p => p.TDL_REQUEST_ROOM_CODE).ToList());

            LogSystem.Info("TotalGet8:" + GC.GetTotalMemory(true) / (1024 * 1024));

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

    }

}
