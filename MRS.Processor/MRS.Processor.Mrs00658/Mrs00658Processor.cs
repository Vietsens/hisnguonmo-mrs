using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisEkipUser;
using MOS.MANAGER.HisEmployee;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00658
{
    class Mrs00658Processor : AbstractProcessor
    {
        Mrs00658Filter castFilter = null;
        List<Mrs00658RDO> ListRdo = new List<Mrs00658RDO>();
        List<Mrs00658RDO> ListRdoDepartment = new List<Mrs00658RDO>();
        List<HIS_SERE_SERV> ListSereServ = new List<HIS_SERE_SERV>();
        List<HIS_EMPLOYEE> ListEmployee = new List<HIS_EMPLOYEE>();
        List<V_HIS_EKIP_USER> ListEkipUser = new List<V_HIS_EKIP_USER>();

        public Mrs00658Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00658Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00658Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                ListSereServ = new ManagerSql().GetSS(castFilter);

                HisEmployeeFilterQuery employeeFilter = new HisEmployeeFilterQuery();
                employeeFilter.IS_ACTIVE = 1;
                ListEmployee = new HisEmployeeManager().Get(employeeFilter);

                if (IsNotNullOrEmpty(ListSereServ))
                {
                    var listEkipIds = ListSereServ.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
                    int skip = 0;
                    while (listEkipIds.Count - skip > 0)
                    {
                        var listIds = listEkipIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisEkipUserViewFilterQuery ekipFilter = new HisEkipUserViewFilterQuery();
                        ekipFilter.EKIP_IDs = listIds;
                        var ekips = new HisEkipUserManager().GetView(ekipFilter);
                        if (IsNotNullOrEmpty(ekips))
                        {
                            ListEkipUser.AddRange(ekips);
                        }
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

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(ListSereServ) && IsNotNullOrEmpty(ListEkipUser))
                {
                    Dictionary<string, HIS_EMPLOYEE> dicEmployee = new Dictionary<string, HIS_EMPLOYEE>();
                    if (IsNotNullOrEmpty(ListEmployee))
                    {
                        dicEmployee = ListEmployee.ToDictionary(o => o.LOGINNAME, o => o);
                    }

                    var grDepartment = ListSereServ.GroupBy(o => o.TDL_EXECUTE_DEPARTMENT_ID).ToList();
                    foreach (var sereServs in grDepartment)
                    {
                        var ekipIds = sereServs.Select(s => s.EKIP_ID ?? 0).Distinct().ToList();
                        if (!IsNotNullOrEmpty(ekipIds)) continue;

                        var ekipUser = ListEkipUser.Where(o => ekipIds.Contains(o.EKIP_ID)).ToList();
                        if (!IsNotNullOrEmpty(ekipUser)) continue;

                        var department = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServs.First().TDL_EXECUTE_DEPARTMENT_ID);
                        if (!IsNotNull(department))
                        {
                            department = new HIS_DEPARTMENT();
                            department.ID = sereServs.First().TDL_EXECUTE_DEPARTMENT_ID;
                        }

                        var groupUser = ekipUser.GroupBy(o => o.LOGINNAME).ToList();
                        foreach (var user in groupUser)
                        {
                            Mrs00658RDO rdo = new Mrs00658RDO();
                            rdo.EXECUTE_DEPARTMENT_ID = department.ID;
                            rdo.EXECUTE_DEPARTMENT_NAME = department.DEPARTMENT_NAME;

                            if (dicEmployee.ContainsKey(user.First().LOGINNAME))
                            {
                                rdo.ACCOUNT_NUMBER = dicEmployee[user.First().LOGINNAME].ACCOUNT_NUMBER;
                                rdo.BANK = dicEmployee[user.First().LOGINNAME].BANK;
                                rdo.DIPLOMA = dicEmployee[user.First().LOGINNAME].DIPLOMA;
                                rdo.IS_ADMIN = dicEmployee[user.First().LOGINNAME].IS_ADMIN;
                                rdo.IS_DOCTOR = dicEmployee[user.First().LOGINNAME].IS_DOCTOR;
                                var edepartment = MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == dicEmployee[user.First().LOGINNAME].DEPARTMENT_ID);
                                if (edepartment != null)
                                {
                                    rdo.EMPLOYEE_DEPARTMENT_NAME = edepartment.DEPARTMENT_NAME;
                                }
                            }

                            rdo.LOGINNAME = user.First().LOGINNAME;
                            rdo.USERNAME = user.First().USERNAME;
                            rdo.EXECUTE_ROLE_NAME = string.Join(",", user.Select(s => s.EXECUTE_ROLE_NAME).Distinct().ToArray());
                            rdo.REMUNERATION_PRICE = user.Sum(s => s.REMUNERATION_PRICE ?? 0);

                            ListRdo.Add(rdo);
                        }

                        ListRdoDepartment.Add(new Mrs00658RDO() { EXECUTE_DEPARTMENT_NAME = department.DEPARTMENT_NAME, EXECUTE_DEPARTMENT_ID = department.ID });
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

                if (castFilter.IS_PT_TT.HasValue)
                {
                    if (castFilter.IS_PT_TT.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_PT_TT", "THỦ THUẬT");
                    }
                    else if (castFilter.IS_PT_TT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_PT_TT", "PHẪU THUẬT");
                    }
                }

                if (castFilter.IS_NT_NGT.HasValue)
                {
                    if (castFilter.IS_NT_NGT.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NGOẠI TRÚ");
                    }
                    else if (castFilter.IS_NT_NGT.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT", "NỘI TRÚ");
                    }
                }

                if (castFilter.IS_NT_NGT_0.HasValue)
                {
                    if (castFilter.IS_NT_NGT_0.Value == 0)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT_0", "NGOẠI TRÚ");
                    }
                    else if (castFilter.IS_NT_NGT_0.Value == 1)
                    {
                        dicSingleTag.Add("TITLE_NT_NGT_0", "NỘI TRÚ");
                    }
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.AddObjectData(store, "Department", ListRdoDepartment);
                objectTag.AddRelationship(store, "Department", "Report", "EXECUTE_DEPARTMENT_ID", "EXECUTE_DEPARTMENT_ID");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
