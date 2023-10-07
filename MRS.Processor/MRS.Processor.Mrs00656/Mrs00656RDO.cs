using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00656
{
    class Mrs00656RDO : HIS_SERE_SERV
    {
        public string EXECUTE_DEPARTMENT_CODE { get; set; }
        public string EXECUTE_DEPARTMENT_NAME { get; set; }

        public decimal COUNT_TOTAL { get; set; }
        public decimal COUNT_CC { get; set; }
        public decimal COUNT_KH { get; set; }
        public decimal COUNT_YC { get; set; }
        public decimal COUNT_DV { get; set; }
        public decimal COUNT_OTHER { get; set; }

        public string PTTT_GROUP_CODE { get; set; }
        public long PTTT_GROUP_ID { get; set; }
        public string PTTT_GROUP_NAME { get; set; }

        public Mrs00656RDO(HIS_SERE_SERV data, HIS_SERE_SERV_PTTT pttt)
        {
            if (data != null)
            {
                Inventec.Common.Mapper.DataObjectMapper.Map<Mrs00656RDO>(this, data);

                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == data.TDL_EXECUTE_DEPARTMENT_ID);
                if (department != null)
                {
                    this.EXECUTE_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                    this.EXECUTE_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                }

                if (data.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV || data.PRIMARY_PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__DV)
                {
                    COUNT_DV += 1;
                }
            }

            if (pttt != null)
            {
                var ptttGr = HisPtttGroupCFG.PTTT_GROUPs.FirstOrDefault(o => o.ID == pttt.PTTT_GROUP_ID);
                if (ptttGr != null)
                {
                    this.PTTT_GROUP_CODE = ptttGr.PTTT_GROUP_CODE;
                    this.PTTT_GROUP_ID = ptttGr.ID;
                    this.PTTT_GROUP_NAME = ptttGr.PTTT_GROUP_NAME;
                }

                if (pttt.PTTT_PRIORITY_ID.HasValue)
                {
                    COUNT_TOTAL += 1;
                }

                //them cau hinh loai(de sau)
                if (pttt.PTTT_PRIORITY_ID == MANAGER.Config.HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__P)
                {
                    COUNT_KH += 1;
                }
                else if (pttt.PTTT_PRIORITY_ID == MANAGER.Config.HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__CC)
                {
                    COUNT_CC += 1;
                }
                else if (pttt.PTTT_PRIORITY_ID == MANAGER.Config.HisPtttPriorityCFG.PTTT_PRIORITY_ID__GROUP__YC)
                {
                    COUNT_YC += 1;
                }
                else
                {
                    COUNT_OTHER += 1;
                }
            }
        }

        public Mrs00656RDO()
        {
            // TODO: Complete member initialization
        }
    }
}
