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

namespace MRS.Processor.Mrs00588
{
    public partial class ManagerSql : BusinessBase
    {
        public List<HIS_TREATMENT> GetTreatment(Mrs00588Filter filter)
        {
            List<HIS_TREATMENT> result = new List<HIS_TREATMENT>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.* ";
                query += "FROM HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TREATMENT>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_PATIENT_TYPE_ALTER> GetPatientTypeAlter(Mrs00588Filter filter)
        {
            List<HIS_PATIENT_TYPE_ALTER> result = new List<HIS_PATIENT_TYPE_ALTER>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PTA.* ";
                query += "FROM HIS_PATIENT_TYPE_ALTER PTA, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID=PTA.TREATMENT_ID ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_TYPE_ALTER>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_DEPARTMENT_TRAN> GetDepartmentTran(Mrs00588Filter filter)
        {
            List<HIS_DEPARTMENT_TRAN> result = new List<HIS_DEPARTMENT_TRAN>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "DEPT.* ";
                query += "FROM HIS_DEPARTMENT_TRAN DEPT, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.ID=DEPT.TREATMENT_ID ";
                query += "AND TREA.CLINICAL_IN_TIME IS NOT NULL ";
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_DEPARTMENT_TRAN>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_SERVICE_REQ> GetServiceReq(Mrs00588Filter filter)
        {
            List<HIS_SERVICE_REQ> result = new List<HIS_SERVICE_REQ>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SR.* ";
                query += "FROM HIS_SERVICE_REQ SR, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND SR.IS_DELETE=0 ";
                query += "AND SR.IS_NO_EXECUTE IS NULL ";
                query += "AND TREA.ID=SR.TREATMENT_ID ";
                query += string.Format("AND SR.SERVICE_REQ_TYPE_ID IN ({0},{1},{2}) ", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__KH,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT);
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND TREA.IN_TIME >={0}  ", filter.IN_TIME_FROM);
                }

                query += "ORDER BY SR.ID ASC ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERVICE_REQ>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }

        public List<HIS_PATIENT_D> GetPatient(Mrs00588Filter filter)
        {
            List<HIS_PATIENT_D> result = new List<HIS_PATIENT_D>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "PT.ID, ";
                query += "PT.ETHNIC_NAME ";
                query += "FROM HIS_PATIENT PT, ";
                query += "HIS_TREATMENT TREA ";

                query += "WHERE 1=1 ";
                query += "AND TREA.PATIENT_ID=PT.ID ";

                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND TREA.IN_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND ((TREA.OUT_TIME >={0} AND TREA.IS_PAUSE =1) OR (TREA.IS_PAUSE IS NULL))  ", filter.IN_TIME_FROM);
                }

                query += "ORDER BY PT.ID ASC ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_D>(query);
                if (rs != null)
                {
                    result = rs.GroupBy(o => o.ID).Select(p => p.First()).ToList();
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

        internal List<HIS_SERE_SERV> GetSereServPTTTByIntructionTime(Mrs00588Filter filter)
        {
            List<HIS_SERE_SERV> result = new List<HIS_SERE_SERV>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "SR.* ";
                query += "FROM HIS_SERE_SERV SR ";

                query += "WHERE 1=1 ";
                query += "AND SR.IS_DELETE=0 AND SR.SERVICE_REQ_ID IS NOT NULL ";
                query += "AND SR.IS_NO_EXECUTE IS NULL ";
                query += "AND SR.TDL_TREATMENT_ID IS NOT NULL ";
                query += string.Format("AND SR.TDL_SERVICE_REQ_TYPE_ID IN ({0},{1}) ",
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__PT,
                             IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__TT);
                if (filter.IN_TIME_TO != null)
                {
                    query += string.Format("AND SR.TDL_INTRUCTION_TIME < {0} ", filter.IN_TIME_TO);
                }

                if (filter.IN_TIME_FROM != null)
                {
                    query += string.Format("AND SR.TDL_INTRUCTION_TIME >={0}  ", filter.IN_TIME_FROM);
                }

                query += "ORDER BY SR.ID ASC ";
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_SERE_SERV>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }

            return result;
        }
        internal List<Mrs00588RDO> GetListCls(Mrs00588Filter filter)
        {
            List<Mrs00588RDO> result = new List<Mrs00588RDO>();
            try
            {
                string query = "select \n";
                query += "sr.request_department_id id, \n";
                query += "nvl(cat.category_code,sv.service_code) category_code, \n";
                query += string.Format("sum(case when sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT, \n",0, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_TEST, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when cat.category_code = 'XQ' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_XQUANG, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when cat.category_code = 'MRI' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_MRI, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when cat.category_code = 'CT' and sr.service_req_stt_id = {0} then 1 else 0 end) as AMOUNT_CT, \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += string.Format("sum(case when ss.tdl_service_type_id = {0} and sr.service_req_stt_id = {1} then 1 else 0 end) as AMOUNT_SIEUAM \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA, IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                //query += string.Format("sum(case when t.treatment_end_type_id = {0} then 1 else 0 end) as AMOUNT_CHUYEN_TUYEN \n", IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN);
                query += "from his_sere_serv ss \n";
                query += "join his_service_req sr on ss.service_req_id = sr.id \n";
                query += "left join lateral ( \n";
                query += "select src.service_id, rtc.category_code, rtc.category_name \n";
                query += "from his_service_rety_cat src \n";
                query += "join his_report_type_cat rtc on src.report_type_cat_id = rtc.id \n";
                query += "where rtc.report_type_code = 'MRS00588' \n";
                query += "and ss.service_id = src.service_id \n";
                query += ") cat on ss.service_id = cat.service_id \n";
                query += "left join lateral ( \n";
                query += "select sv.id, pr.service_code, pr.service_name \n";
                query += "from his_service sv \n";
                query += "join his_service pr on sv.parent_id = pr.id \n";
                query += "where 1=1 \n";
                query += "and ss.service_id = sv.id \n";
                query += ") sv on sv.id = ss.service_id \n";
                query += "join his_treatment t on ss.tdl_treatment_id = t.id \n";
                query += "where 1=1 and sr.service_req_type_id in (2,3,5,8,9)\n";
                query += string.Format("and sr.intruction_time between {0} and {1} \n", filter.IN_TIME_FROM, filter.IN_TIME_TO);
                query += "and sr.is_delete = 0 \n";
                query += string.Format("and sr.service_req_stt_id = {0} \n", IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_STT.ID__HT);
                query += "group by nvl(cat.category_code,sv.service_code),sr.request_department_id \n";
                LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00588RDO>(query);

                result = result.GroupBy(g => g.ID).Select(p => new Mrs00588RDO() { 
                    ID = p.First().ID, 
                    AMOUNT_TEST = p.Sum(s => s.AMOUNT_TEST),
                    AMOUNT_XQUANG = p.Sum(s => s.AMOUNT_XQUANG),
                    AMOUNT_MRI = p.Sum(s => s.AMOUNT_MRI),
                    AMOUNT_CT = p.Sum(s => s.AMOUNT_CT),
                    AMOUNT_SIEUAM = p.Sum(s => s.AMOUNT_SIEUAM),
                    DIC_CATE_AMOUNT = p.GroupBy(o=>o.CATEGORY_CODE??"NONE").ToDictionary(q=>q.Key,r=>r.Sum(s=>s.AMOUNT))
                }).ToList();
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                result = null;
            }
            return result;
        }
    }
}
