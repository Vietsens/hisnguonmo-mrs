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

namespace MRS.Processor.Mrs00716
{
   internal class ManagerSql
    {
       internal List<TREATMENT> GetTreatment(Mrs00716Filter filter)
       {
           List<TREATMENT> result = new List<TREATMENT>();
         CommonParam paramGet = new CommonParam();
         try
         {
             string query = "";
             
             query += "select \n";
             query += "TREA.OUT_TIME,\n";
             query += "TREA.IN_TIME,\n";
             query += "TREA.TDL_PATIENT_GENDER_ID,\n";
             query += "TREA.TDL_PATIENT_GENDER_NAME,\n";
             query += "TREA.TDL_PATIENT_DOB ,\n";
             query += "TREA.TDL_PATIENT_TYPE_ID,\n";
             query += "TREA.TDL_TREATMENT_TYPE_ID,\n"; // DIỆN ĐỐI TƯỢNG
             query += "TREA.TREATMENT_END_TYPE_ID\n"; // lOẠI RA VIỆN
             query += "FROM HIS_TREATMENT TREA\n";
             query += "WHERE 1=1\n";
             if (filter.TIME_FROM > 0)
             {
                 query += string.Format("AND TREA.OUT_TIME>={0}\n", filter.TIME_FROM);
             }
             if (filter.TIME_TO > 0)
             {
                 query += string.Format("AND TREA.OUT_TIME<={0}\n", filter.TIME_TO);
             }
            
             
             Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
             result = new MOS.DAO.Sql.SqlDAO().GetSql<TREATMENT>(paramGet, query);
             if (paramGet.HasException)
                 throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00716");

             
         }
         catch (Exception ex)
         {
             Inventec.Common.Logging.LogSystem.Error(ex);
             result = null;
         }
         return result;
       }

       
       public class TREATMENT
       {
           public long? OUT_TIME { get; set; }
           public long IN_TIME { get; set; }
           public string TDL_PATIENT_GENDER_NAME { get; set; }
           public long TDL_PATIENT_GENDER_ID { get; set; }
           public long TDL_PATIENT_DOB { get; set; }
           public long? TDL_PATIENT_TYPE_ID { get; set; }
           public long? TDL_TREATMENT_TYPE_ID { get; set; }
           public long? TREATMENT_END_TYPE_ID { get; set; }

       }
    }
}
