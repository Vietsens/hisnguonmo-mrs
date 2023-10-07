using MOS.MANAGER.HisService;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisEkip;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServPttt;
using MOS.MANAGER.HisExecuteRole;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00465
{
    class Mrs00465Processor : AbstractProcessor
    {
        Mrs00465Filter castFilter = null;
        List<Mrs00465RDO> listRdo = new List<Mrs00465RDO>();

        public Mrs00465Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<V_HIS_EKIP_USER> listEkipUsers = new List<V_HIS_EKIP_USER>();

        //List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>(); 

        List<HIS_SERVICE> ListService = new List<HIS_SERVICE>();

        List<EXECUTE_ROLE_465> listExecuteRoles = new List<EXECUTE_ROLE_465>();
        List<V_HIS_SERE_SERV_PTTT> listSereServPttts = new List<V_HIS_SERE_SERV_PTTT>();
        Dictionary<long, V_HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, V_HIS_SERVICE_REQ>();

        public string s_Departments = "";

        public override Type FilterType()
        {
            return typeof(Mrs00465Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00465Filter)this.reportFilter;
                var skip = 0;

                // his_department
                List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();
                if (IsNotNullOrEmpty(castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    while (castFilter.REQUEST_DEPARTMENT_IDs.Count - skip > 0)
                    {
                        var listIds = castFilter.REQUEST_DEPARTMENT_IDs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                        filter.IDs = listIds;
                        listDepartments.AddRange(new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(filter));
                    }
                }
                else if (castFilter.REQUEST_DEPARTMENT_IDs == null || castFilter.REQUEST_DEPARTMENT_IDs.Count == 0)
                {
                    HisDepartmentFilterQuery filter = new HisDepartmentFilterQuery();
                    listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(filter);
                }
                List<V_HIS_SERVICE_REQ> listServiceReqs = new List<V_HIS_SERVICE_REQ>();
                if (IsNotNullOrEmpty(listDepartments))
                    this.s_Departments = String.Join(", ", listDepartments.Select(s => s.DEPARTMENT_NAME).ToList());

                #region lấy theo thời gian chỉ định
                listServiceReqs = GetServiceReq();
                dicServiceReq = listServiceReqs.ToDictionary(o => o.ID);
                skip = 0;
                while (listServiceReqs.Count - skip > 0)
                {
                    var listIds = listServiceReqs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisSereServViewFilterQuery sereServFilter = new HisSereServViewFilterQuery();
                    sereServFilter.SERVICE_REQ_IDs = listIds.Select(s => s.ID).ToList();
                    if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                    {
                        sereServFilter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs;
                    }
                    sereServFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT;
                    sereServFilter.HAS_EXECUTE = true;
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(sereServFilter));
                }
                #endregion


                if (IsNotNullOrEmpty(listSereServs))
                {
                    skip = 0;
                    var listEkipIds = listSereServs.Where(w => w.EKIP_ID != null).Select(s => s.EKIP_ID).ToList();
                    while (listEkipIds.Count - skip > 0)
                    {
                        var listIds = listEkipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisEkipUserViewFilterQuery filter = new HisEkipUserViewFilterQuery();
                        filter.EKIP_IDs = listIds.Select(s => s.Value).ToList();
                        var skipUsers = new HisEkipUserManager(paramGet).GetView(filter);
                        listEkipUsers.AddRange(skipUsers);
                    }

                    // ~~~~~~~~~~> bổ sung
                    skip = 0;
                    while (listSereServs.Count - skip > 0)
                    {
                        var listIds = listSereServs.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisSereServPtttViewFilterQuery sereServPtttFilter = new HisSereServPtttViewFilterQuery();
                        sereServPtttFilter.SERE_SERV_IDs = listIds.Select(s => s.ID).ToList();
                        listSereServPttts.AddRange(new MOS.MANAGER.HisSereServPttt.HisSereServPtttManager(param).GetView(sereServPtttFilter));
                    }
                }

                HisExecuteRoleFilterQuery executeRoleFilter = new HisExecuteRoleFilterQuery();
                executeRoleFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE;
                var listExecuteRoless = new MOS.MANAGER.HisExecuteRole.HisExecuteRoleManager(param).Get(executeRoleFilter);

                if (IsNotNullOrEmpty(listExecuteRoless))
                {
                    long i = 1;
                    listExecuteRoless = listExecuteRoless.OrderBy(s => s.ID).ToList();
                    foreach (var executeRole in listExecuteRoless)
                    {
                        var exeRole = new EXECUTE_ROLE_465();
                        exeRole.NUMBER = i;
                        exeRole.EXECUTE_ROLE_ID = executeRole.ID;
                        exeRole.EXECUTE_ROLE_NAME = executeRole.EXECUTE_ROLE_NAME;
                        exeRole.EXECUTE_ROLE_TAG = "EXECUTE_ROLE_" + i;
                        i++;
                        listExecuteRoles.Add(exeRole);
                    }
                }

                //danh sách dịch vụ
                GetService();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<V_HIS_SERVICE_REQ> GetServiceReq()
        {

            var listServiceReqs = new List<V_HIS_SERVICE_REQ>();
            if (this.castFilter.INPUT_DATA_ID_TIME_TYPE == 1)//lọc theo thời gian chỉ định
            {
                HisServiceReqViewFilterQuery serviceReqViewFilter = new HisServiceReqViewFilterQuery();
                serviceReqViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                serviceReqViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                serviceReqViewFilter.HAS_EXECUTE = true;
                if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    serviceReqViewFilter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs;
                }
                listServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(param).GetView(serviceReqViewFilter);
                

               

            }
            else if (this.castFilter.INPUT_DATA_ID_TIME_TYPE == 2)//lọc theo thời gian ra viện
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.OUT_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.OUT_TIME_TO = castFilter.TIME_TO;
                treatmentFilter.IS_PAUSE = true;

                var treatments = new HisTreatmentManager(param).Get(treatmentFilter);
                var skip = 0;
                while (treatments.Count - skip > 0)
                {
                    var listIds = treatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery();
                    serviceReqFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                    {
                        serviceReqFilter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs;
                    }
                    serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT;
                    serviceReqFilter.HAS_EXECUTE = true;
                    listServiceReqs.AddRange(new HisServiceReqManager().GetView(serviceReqFilter));
                }

            }
            else if (this.castFilter.INPUT_DATA_ID_TIME_TYPE == 3)//lọc theo thời gian khóa viện phí
            {
                HisTreatmentFilterQuery treatmentFilter = new HisTreatmentFilterQuery();
                treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
                treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
                var treatments = new HisTreatmentManager(param).Get(treatmentFilter);
                var skip = 0;
                while (treatments.Count - skip > 0)
                {
                    var listIds = treatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisServiceReqViewFilterQuery serviceReqFilter = new HisServiceReqViewFilterQuery();
                    serviceReqFilter.TREATMENT_IDs = listIds.Select(s => s.ID).ToList();
                    if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                    {
                        serviceReqFilter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs;
                    }
                    serviceReqFilter.SERVICE_REQ_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT;
                    serviceReqFilter.HAS_EXECUTE = true;
                    listServiceReqs.AddRange(new HisServiceReqManager().GetView(serviceReqFilter));
                }

            }
            else
            {
                HisServiceReqViewFilterQuery serviceReqViewFilter = new HisServiceReqViewFilterQuery();
                serviceReqViewFilter.INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                serviceReqViewFilter.INTRUCTION_TIME_TO = castFilter.TIME_TO;
                serviceReqViewFilter.HAS_EXECUTE = true;
                if (IsNotNullOrEmpty(this.castFilter.REQUEST_DEPARTMENT_IDs))
                {
                    serviceReqViewFilter.REQUEST_DEPARTMENT_IDs = this.castFilter.REQUEST_DEPARTMENT_IDs;
                }
                listServiceReqs = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(param).GetView(serviceReqViewFilter);
            }
            return listServiceReqs;
        }

        private void GetService()
        {
            this.ListService = new HisServiceManager().Get(new HisServiceFilterQuery());
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    string nursing_user = "";
                    V_HIS_SERVICE_REQ req = null;
                    foreach (var sereServ in listSereServs)
                    {
                        if (!dicServiceReq.ContainsKey(sereServ.SERVICE_REQ_ID ?? 0)) continue;
                        req = dicServiceReq[sereServ.SERVICE_REQ_ID ?? 0];
                        var ekipUsers = listEkipUsers.Where(w => w.EKIP_ID == sereServ.EKIP_ID && w.EXECUTE_ROLE_ID == MANAGER.Config.HisExecuteRoleCFG.HisExecuteRoleId__YTDD).ToList();
                        Mrs00465RDO rdo = new Mrs00465RDO();
                        if (req.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE)
                        {
                            rdo.DOB_FEMALE = req.TDL_PATIENT_DOB;
                        }
                        if (req.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                        {
                            rdo.DOB_MALE = req.TDL_PATIENT_DOB;
                        }
                        rdo.EXECUTE_TIME = req.INTRUCTION_TIME;
                        rdo.EXECUTE_USER = req.EXECUTE_USERNAME;
                        rdo.PATIENT_CODE = req.TDL_PATIENT_CODE;
                        rdo.PATIENT_NAME = req.TDL_PATIENT_NAME;
                        rdo.REQUEST_USER = req.REQUEST_USERNAME;
                        rdo.SERVICE_NAME = sereServ.TDL_SERVICE_NAME;
                        if (IsNotNullOrEmpty(ekipUsers))
                        {
                            foreach (var ekipUser in ekipUsers)
                            {
                                nursing_user += ekipUser.USERNAME + ",";
                            }
                            rdo.NURSING_USER = nursing_user.Substring(0, nursing_user.Length - 1);
                        }

                        // bổ sung
                        rdo.TREATMENT_CODE = sereServ.TDL_TREATMENT_CODE;

                        DateTime now = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(req.INTRUCTION_TIME) ?? DateTime.Now;
                        int age = now.Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(req.TDL_PATIENT_DOB) ?? DateTime.Now).Year;
                        if (age >= 6 && (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(req.TDL_PATIENT_DOB) ?? DateTime.Now) > now.AddYears(-age)) age--;
                        rdo.AGE = age;
                        rdo.AMOUNT = sereServ.AMOUNT;
                        var sereServPttt = listSereServPttts.Where(w => w.SERE_SERV_ID == sereServ.ID).ToList();
                        if (IsNotNullOrEmpty(sereServPttt))
                            rdo.PTTT_GROUP_NAME = sereServPttt.First().PTTT_GROUP_NAME;
                        var service = ListService.FirstOrDefault(o => o.ID == sereServ.SERVICE_ID);
                        if (service != null)
                        {
                            var ptttGroup = HisPtttGroupCFG.PTTT_GROUPs.Where(w => w.ID == service.PTTT_GROUP_ID).ToList();
                            if (IsNotNullOrEmpty(ptttGroup))
                                rdo.SV_PTTT_GROUP_NAME = ptttGroup.First().PTTT_GROUP_NAME;

                        }
                        var ekipId = sereServ.EKIP_ID ?? 0;
                        var sereServEkips = listEkipUsers.Where(w => w.EKIP_ID == ekipId).ToList();
                        if (IsNotNullOrEmpty(sereServEkips))
                        {
                            string role_01 = "";
                            string role_02 = "";
                            string role_03 = "";
                            string role_04 = "";
                            string role_05 = "";
                            string role_06 = "";
                            string role_07 = "";
                            string role_08 = "";
                            string role_09 = "";
                            string role_10 = "";

                            string role_11 = "";
                            string role_12 = "";
                            string role_13 = "";
                            string role_14 = "";
                            string role_15 = "";
                            string role_16 = "";
                            string role_17 = "";
                            string role_18 = "";
                            string role_19 = "";
                            string role_20 = "";

                            foreach (var user in sereServEkips)
                            {
                                var ekip = listExecuteRoles.Where(w => w.EXECUTE_ROLE_ID == user.EXECUTE_ROLE_ID).ToList();
                                if (IsNotNullOrEmpty(ekip))
                                {
                                    if (ekip.First().NUMBER == 1) role_01 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 2) role_02 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 3) role_03 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 4) role_04 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 5) role_05 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 6) role_06 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 7) role_07 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 8) role_08 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 9) role_09 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 10) role_10 += user.USERNAME + ", ";

                                    else if (ekip.First().NUMBER == 11) role_11 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 12) role_12 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 13) role_13 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 14) role_14 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 15) role_15 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 16) role_16 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 17) role_17 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 18) role_18 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 19) role_19 += user.USERNAME + ", ";
                                    else if (ekip.First().NUMBER == 20) role_20 += user.USERNAME + ", ";
                                }
                            }

                            rdo.EXECUTE_ROLE_01 = role_01;
                            rdo.EXECUTE_ROLE_02 = role_02;
                            rdo.EXECUTE_ROLE_03 = role_03;
                            rdo.EXECUTE_ROLE_04 = role_04;
                            rdo.EXECUTE_ROLE_05 = role_05;
                            rdo.EXECUTE_ROLE_06 = role_06;
                            rdo.EXECUTE_ROLE_07 = role_07;
                            rdo.EXECUTE_ROLE_08 = role_08;
                            rdo.EXECUTE_ROLE_09 = role_09;
                            rdo.EXECUTE_ROLE_10 = role_10;

                            rdo.EXECUTE_ROLE_11 = role_11;
                            rdo.EXECUTE_ROLE_12 = role_12;
                            rdo.EXECUTE_ROLE_13 = role_13;
                            rdo.EXECUTE_ROLE_14 = role_14;
                            rdo.EXECUTE_ROLE_15 = role_15;
                            rdo.EXECUTE_ROLE_16 = role_16;
                            rdo.EXECUTE_ROLE_17 = role_17;
                            rdo.EXECUTE_ROLE_18 = role_18;
                            rdo.EXECUTE_ROLE_19 = role_19;
                            rdo.EXECUTE_ROLE_20 = role_20;
                        }

                        listRdo.Add(rdo);
                    }

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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                dicSingleTag.Add("DEPARTMENT", s_Departments);

                foreach (var role in listExecuteRoles)
                {
                    dicSingleTag.Add(role.EXECUTE_ROLE_TAG, role.EXECUTE_ROLE_NAME);
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(s => s.EXECUTE_TIME).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
