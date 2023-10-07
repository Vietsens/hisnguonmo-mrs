using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using System.Data;

namespace MRS.Processor.Mrs00708
{
    internal class ManagerSql
    {

        internal List<Mrs00708RDO> GetRdo(Mrs00708Filter filter)
        {
            List<Mrs00708RDO> result = new List<Mrs00708RDO>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select \n";
            query += "trea.treatment_code,\n";
            query += "trea.tdl_patient_code,\n";
            query += "trea.tdl_patient_name,\n";
            query += "trea.tdl_patient_dob,\n";
            query += "trea.tdl_patient_address,\n";
            query += "trea.json_form_id\n";
           
            query += "from his_treatment trea\n";

            query += "where 1 = 1\n";
            query += "and trea.is_pause = 1\n";
            
            query += string.Format("and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00708RDO>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00708");

            return result;
        }

        internal List<FormData> GetFormData(Mrs00708Filter filter,List<long> FormIds)
        {
            List<FormData> result = new List<FormData>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select\n";
            query += "fd.form_id,\n";
            query += "fd.key,\n";
            query += "fd.value\n";
            query += "from sar_form_data fd\n";
            query += "join sar_form fo on fo.id = fd.form_id\n";
            query += "join sar_form_type ft on ft.id = fo.form_type_id\n";
            query += "where 1=1\n";
            if (filter.FORM_TYPE_IDs != null)
            {
                query += string.Format("and ft.ID in ({0})\n", string.Join(",", filter.FORM_TYPE_IDs));
            }
            if (FormIds != null)
            {
                query += string.Format("and fd.FORM_ID in ({0})\n", string.Join(",", FormIds));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SAR.DAO.Sql.SqlDAO().GetSql<FormData>(query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00708");

            return result;
        }

        internal List<Form> GetForm(Mrs00708Filter filter, List<long> FormIds)
        {
            List<Form> result = new List<Form>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += "select\n";
            query += "fo.id,\n";
            query += "fo.create_time,\n";
            query += "fo.creator\n";
            query += "from sar_form fo\n";
            query += "join sar_form_type ft on ft.id = fo.form_type_id\n";
            query += "where 1=1\n";
            if (filter.FORM_TYPE_IDs != null)
            {
                query += string.Format("and ft.ID in ({0})\n", string.Join(",", filter.FORM_TYPE_IDs));
            }
            if (FormIds != null)
            {
                query += string.Format("and fo.ID in ({0})\n", string.Join(",", FormIds));
            }
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new SAR.DAO.Sql.SqlDAO().GetSql<Form>(query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00708");

            return result;
        }
    }
}
