using ACS.EFMODEL.DataModels;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00524
{
    public class Mrs00524RDO : V_HIS_EXP_MEST_MEDICINE
    {

        public string CREATE_DATE_STR { get; set; }
        public long? TAKE_MONTH { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string EMPLOYEE_ID_STR { get; set; }
        public string EMPLOYEE_DEPARTMENT_NAME { get; set; }
        public string EMPLOYEE_DEPARTMENT_SHORT_NAME { get; set; }
        public string POSITION { get; set; }

        public Mrs00524RDO(V_HIS_EXP_MEST_MEDICINE r, List<HIS_SERVICE_REQ> listHisServiceReq, List<HIS_EMPLOYEE> listEmployee)
        {
            PropertyInfo[] p = typeof(V_HIS_EXP_MEST_MEDICINE).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            var serviceReq = listHisServiceReq.FirstOrDefault(o => o.ID == r.TDL_SERVICE_REQ_ID) ?? new HIS_SERVICE_REQ();
            var employee = listEmployee.FirstOrDefault(o => o.LOGINNAME == serviceReq.REQUEST_LOGINNAME);
            this.CREATE_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(serviceReq.INTRUCTION_DATE);
            this.REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
            this.REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
            if (employee != null)
            {
                this.EMPLOYEE_ID_STR = employee.ID.ToString();
                this.EMPLOYEE_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(q => q.ID == (employee.DEPARTMENT_ID ?? 0)) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                this.EMPLOYEE_DEPARTMENT_SHORT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(q => q.ID == (employee.DEPARTMENT_ID ?? 0)) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
                this.POSITION = employee.TITLE;
            }
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID);
            if (department != null)
            {
                this.REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                this.REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
            }
        }
    }
}
