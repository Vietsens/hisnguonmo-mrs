using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisDepartmentCFG
    {
        //department_code cua cac khoa hoi suc cap cuu
        private const string RESUSCITATION_DEPARTMENT_CODES = "MOS.HIS_DEPARTMENT.RESUSCITATION_DEPARTMENT_CODES";
        //department_code cua cac khoa dieu tri
        private const string TREATMENT_DEPARTMENT_CODES = "MOS.HIS_DEPARTMENT.TREATMENT_DEPARTMENT_CODES";

        private static List<long> treatmentDepartmentIds;
        public static List<long> TREATMENT_DEPARTMENT_IDS
        {
            get
            {
                if (treatmentDepartmentIds == null)
                {
                    treatmentDepartmentIds = GetIds(TREATMENT_DEPARTMENT_CODES);
                }
                return treatmentDepartmentIds;
            }
            set
            {
                treatmentDepartmentIds = value;
            }
        }

        private static List<long> resuscitationDepartmentIds;
        public static List<long> RESUSCITATION_DEPARTMENT_IDS
        {
            get
            {
                if (resuscitationDepartmentIds == null)
                {
                    resuscitationDepartmentIds = GetIds(RESUSCITATION_DEPARTMENT_CODES);
                }
                return resuscitationDepartmentIds;
            }
            set
            {
                resuscitationDepartmentIds = value;
            }
        }

        private static List<HIS_DEPARTMENT> data;
        public static List<HIS_DEPARTMENT> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisDepartmentGet().Get(new HisDepartmentFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        private static List<long> GetIds(string code)
        {
            List<long> result = null;
            try
            {
                result = new List<long>();
                List<string> codes = ConfigUtil.GetStrConfigs(code);
                foreach (string t in codes)
                {
                    var data = new HisDepartment.HisDepartmentGet().GetByCode(t);
                    if (data != null)
                    {
                        result.Add(data.ID);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public static void Reload()
        {
            var tmp = new HisDepartmentGet().Get(new HisDepartmentFilterQuery());
            data = tmp;

            var treatDepartmentIds = GetIds(TREATMENT_DEPARTMENT_CODES);
            var resusDepartmentIds = GetIds(RESUSCITATION_DEPARTMENT_CODES);
            treatmentDepartmentIds = treatDepartmentIds;
            resuscitationDepartmentIds = resusDepartmentIds;
        }
    }
}
