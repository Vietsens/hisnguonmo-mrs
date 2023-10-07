using ACS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using Inventec.Core;
using LIS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00745
{
    class Mrs00745Processor : AbstractProcessor
    {
        List<LIS_MACHINE> listLisMachine = new List<LIS_MACHINE>();
        List<Mrs00745RDO> listData = new List<Mrs00745RDO>();
        List<Mrs00745RDO> listRdo = new List<Mrs00745RDO>();
        List<DEPARTMENT> listDepartment = new List<DEPARTMENT>();
        List<ACS_USER> listUser = new List<ACS_USER>();
        List<HIS_EMPLOYEE> listEmployee = new List<HIS_EMPLOYEE>();
        Mrs00745Filter filter = null;

        public Mrs00745Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00745Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                filter = (Mrs00745Filter)this.reportFilter;
                listLisMachine = new ManagerSql().GetMachine(filter) ?? new List<LIS_MACHINE>();
                listData = new ManagerSql().GetQC(filter) ?? new List<Mrs00745RDO>();
                listDepartment = new ManagerSql().GetDepartment(filter) ?? new List<DEPARTMENT>();
                listUser = new ManagerSql().GetUser() ?? new List<ACS_USER>();
                listEmployee = new ManagerSql().GetEmployee() ?? new List<HIS_EMPLOYEE>();
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (listData != null)
                {
                    foreach (var item in listData)
                    {
                        var machine = listLisMachine.FirstOrDefault(p => p.MACHINE_CODE == item.MACHINE_CODE) ?? new LIS_MACHINE();
                        var employee = listEmployee.FirstOrDefault(p => p.LOGINNAME == (item.MODIFIER ?? item.CREATOR)) ?? new HIS_EMPLOYEE();
                        var department = listDepartment.FirstOrDefault(p => p.ROOM_CODE == machine.EXECUTE_ROOM_CODE) ?? new DEPARTMENT();
                        var employeeDepartment = listDepartment.FirstOrDefault(p => p.ID == employee.DEPARTMENT_ID) ?? new DEPARTMENT();
                        var user = listUser.FirstOrDefault(p => p.LOGINNAME == (item.MODIFIER ?? item.CREATOR)) ?? new ACS_USER();
                        Mrs00745RDO rdo = new Mrs00745RDO();
                        rdo.QC_TYPE_CODE = item.QC_TYPE_CODE;
                        rdo.QC_TYPE_NAME = item.QC_TYPE_NAME;
                        rdo.MACHINE_CODE = item.MACHINE_CODE;
                        rdo.MACHINE_NAME = item.MACHINE_NAME;
                        rdo.USER_NAME = user.USERNAME;
                        rdo.DEPARTMENT_NAME = employeeDepartment.DEPARTMENT_NAME;
                        
                        listRdo.Add(rdo);
                    }
                }
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                LogSystem.Info("listRdo: " + listRdo.Count);
                objectTag.AddObjectData(store, "Report", listRdo.OrderBy(p => p.QC_TYPE_CODE).ThenBy(p => p.MACHINE_CODE).ToList());
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
            }
        }
    }
}
