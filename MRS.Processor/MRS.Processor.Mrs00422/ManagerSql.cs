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
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00422
{
    public partial class ManagerSql : BusinessBase
    {
        public List<Mrs00422RDO> GetSereServDO(Mrs00422Filter filter)
        {
            List<Mrs00422RDO> result = new List<Mrs00422RDO>();
            try
            {
               
                string query = " --chi tiet dich vu thuc hien\n";
                query += "SELECT\n";
                query += "SS.SERVICE_ID,\n";
                query += "SS.TDL_SERVICE_NAME AS SERVICE_NAME,\n";
                query += "SS.TDL_SERVICE_CODE AS SERVICE_CODE,\n";
                query += "SS.VIR_PRICE AS PRICE,\n";
                query += "SS.AMOUNT,\n";
                query += "SR.REQUEST_LOGINNAME,\n";
                query += "SR.REQUEST_USERNAME,\n";
                query += "SR.EXECUTE_LOGINNAME,\n";
                query += "SR.EXECUTE_USERNAME,\n";
                query += "NVL(SS.IS_NO_EXECUTE,0) AS IS_NO_EXECUTE,\n";
                query += "NVL(SS.IS_EXPEND,0) AS IS_EXPEND,\n";
                query += "SR.EXECUTE_ROOM_ID,\n";
                query += "TREA.TDL_PATIENT_DOB,\n";
                query += "TREA.IN_TIME,\n";
                query += "TREA.TDL_PATIENT_TYPE_ID,\n";
                query += "SS.TDL_SERVICE_TYPE_ID AS SERVICE_TYPE_ID,\n";
                query += "TREA.TDL_PATIENT_GENDER_ID,\n";
                query += "TREA.TDL_PATIENT_NAME,\n";
                query += "TREA.TREATMENT_CODE,\n";
                query += "ro.room_type_id REQUEST_ROOM_TYPE_ID,\n";
                query += "SR.SERVICE_REQ_STT_ID\n";
                query += "FROM HIS_SERE_SERV SS\n";
                query += "join HIS_SERVICE_REQ SR on sr.id=ss.service_req_id\n";
                query += "join HIS_TREATMENT TREA on trea.id=sr.treatment_id\n";
                query += "left join his_room ro on ro.id=sr.request_room_id\n";
                query += "WHERE SS.IS_DELETE = 0\n";
                query += "AND SR.IS_DELETE = 0\n";
                //query += "AND SS.IS_NO_EXECUTE IS NULL\n";
                query += "AND SS.SERVICE_REQ_ID IS NOT NULL\n";

                if (filter.PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID = {0}\n", filter.PATIENT_TYPE_ID);
                }

                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.PATIENT_TYPE_ID in ({0})", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                if (filter.IS_FINISH_TIME.HasValue && filter.IS_FINISH_TIME.Value)
                {
                    query += string.Format("AND SR.FINISH_TIME >= {0}\n", filter.TIME_FROM);
                    query += string.Format("AND SR.FINISH_TIME < {0} ", filter.TIME_TO);
                }
                else
                {
                    query += string.Format("AND SR.INTRUCTION_TIME >= {0}\n", filter.TIME_FROM);
                    query += string.Format("AND SR.INTRUCTION_TIME < {0}\n", filter.TIME_TO);
                }

                if (filter.SERVICE_TYPE_ID != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID = {0}\n", filter.SERVICE_TYPE_ID);
                }

                if (filter.SERVICE_TYPE_IDs != null)
                {
                    query += string.Format("AND SS.TDL_SERVICE_TYPE_ID IN ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                if (filter.SERVICE_REQ_STT_ID != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID = {0}\n", filter.SERVICE_REQ_STT_ID);
                }

                if (filter.SERVICE_REQ_STT_IDs != null)
                {
                    query += string.Format("AND SR.SERVICE_REQ_STT_ID IN ({0})\n", string.Join(",", filter.SERVICE_REQ_STT_IDs));
                }
                if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                {
                    query += string.Format("AND SR.EXECUTE_LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                }
                if (filter.REQUEST_LOGINNAME_DOCTORs != null)
                {
                    query += string.Format("AND SR.REQUEST_LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAME_DOCTORs));
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    query += string.Format("AND SR.REQUEST_DEPARTMENT_ID IN ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }


                if (filter.IS_PAYED != false)
                {
                    query += string.Format("AND exists(select 1 from his_sere_serv_bill where sere_serv_id = ss.id and is_cancel is null)\n");
                }


                if (filter.IS_DEPO_SERVICE != false)
                {
                    query += string.Format("AND exists(select 1 from his_sere_serv_deposit where sere_serv_id = ss.id and is_cancel is null)\n");
                }
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00422RDO>(query);
               
                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
        public List<ACS_USER> GetAcsUser(Mrs00422Filter filter)
        {
            List<ACS_USER> result = new List<ACS_USER>();
            try
            {

                string query = " --danh sach nguoi dung from QCS\n";
                query += "SELECT\n";
                query += "AU.*\n";
              
                query += "FROM ACS_RS.ACS_USER AU\n";
               
                query += "JOIN HIS_RS.HIS_EMPLOYEE EMP ON EMP.LOGINNAME = AU.LOGINNAME\n";
                query += "WHERE 1=1 and EMP.IS_DOCTOR=1\n";
               
                //if (filter.EXECUTE_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND AU.LOGINNAME IN ('{0}')\n", string.Join("','", filter.EXECUTE_LOGINNAME_DOCTORs));
                //}
                //if (filter.REQUEST_LOGINNAME_DOCTORs != null)
                //{
                //    query += string.Format("AND AU.LOGINNAME IN ('{0}')\n", string.Join("','", filter.REQUEST_LOGINNAME_DOCTORs));
                //}

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new  MOS.DAO.Sql.MyAppContext().GetSql<ACS_USER>(query);

                if (rs != null)
                {
                    result = rs;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
    }
}
