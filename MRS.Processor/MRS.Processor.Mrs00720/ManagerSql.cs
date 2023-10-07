using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00720
{
    internal class ManagerSql
    {
        public class SERESERV
        {
            //sere_Serv_bill
            public long? SS_PATIENT_TYPE_ID { get; set; }//
            public long TDL_SERVICE_TYPE_ID { get; set; }//

            public long SERVICE_ID { get; set; }//
            public long? MEDICINE_ID { get; set; }//
            public long? MATERIAL_ID { get; set; }//
            public decimal? PRICE { get; set; }//
            public decimal? VIR_PRICE { get; set; }//
            public long BILL_ID { get; set; }//
            public decimal? TDL_PRICE { get; set; }//
            public decimal? TDL_TOTAL_PATIENT_PRICE { get; set; }//
            public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }//
            public decimal? TDL_TOTAL_PATIENT_PRICE_BHYT { get; set; }//
            public decimal? TDL_OTHER_SOURCE_PRICE { get; set; }//
            public decimal? VIR_TOTAL_PRICE_NO_EXPEND { get; set; }
            public long? PACKAGE_ID { get; set; }
            //service + parent service 
            public long REQUEST_DEPARTMENT_ID { get; set; }//
            public long REQUEST_ROOM_ID { get; set; }//
            public string REQUEST_LOGINNAME { get; set; }//
            public string REQUEST_USERNAME { get; set; }//

            public string SERVICE_REQ_CODE { get; set; }//
            public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }//
            public decimal? ADD_PRICE { get; set; }//
            public decimal? SS_AMOUNT { get; set; }//
            public decimal SS_PRICE { get; set; }
            public decimal SS_TOTAL_PRICE { get; set; }
            public long? TDL_TREATMENT_ID { get; set; }
        }

        internal List<SERESERV> GetDataSereServ(Mrs00720Filter filter, bool isCancel)
        {
            List<SERESERV> result = new List<SERESERV>();
            try
            {
                string query = "select \n";
                query += "ss.PACKAGE_ID, \n";
                query += "ss.ADD_PRICE, \n";
                query += "ss.tdl_hein_service_type_id, \n";
                query += "ss.service_id, \n";
                query += "ss.medicine_id, \n";
                query += "ss.material_id, \n";
                query += "ssb.tdl_patient_type_id SS_PATIENT_TYPE_ID, \n";

                query += "ssb.tdl_service_type_id, \n";
                query += "ssb.price, \n";
                query += "ss.vir_price, \n";
                query += "ssb.bill_id, \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.price ss_price, \n";
                query += "ss.amount ss_amount, \n";
                query += "ss.vir_total_price ss_total_price, \n";
                query += "ssb.tdl_price,\n";
                query += "ssb.tdl_total_patient_price, \n";
                query += "ssb.tdl_total_patient_price_bhyt, \n";
                query += "ss.vir_total_hein_price, \n";
                query += "ss.vir_total_price_no_expend, \n";
                query += "ssb.tdl_other_source_price, \n";

                query += "sr.request_department_id, \n";
                query += "sr.request_room_id, \n";
                query += "sr.request_loginname, \n";
                query += "sr.request_username, \n";
                query += "sr.service_req_code \n";
                query += "from his_sere_serv_bill ssb  \n";
                query += "join his_sere_serv ss on ssb.sere_serv_id = ss.id \n";
                //query += "left join his_medicine me on ss.medicine_id = me.id \n";
                query += "join his_transaction tran on ssb.bill_id = tran.id\n";
                query += "join his_treatment trea on ssb.tdl_treatment_id = trea.id\n";
                query += "join his_service_req sr on ssb.tdl_service_req_id = sr.id\n";
                query += "where 1=1  and not exists(select 1 from his_sere_serv_deposit ssd join his_transaction de on de.id = ssd.deposit_id where ssd.sere_serv_id = ss.id and (de.is_cancel is null or de.is_cancel=1 and de.cancel_time>=tran.transaction_time))\n";
                query += StrFilterTransaction(filter, isCancel);
                query += StrFilterTreatment(filter);

                if (filter.SS_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ssb.TDL_PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs)); //thanh toan
                }


                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.request_department_id = {0}  \n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.request_department_id in ({0})  \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in ({0})  \n", string.Join(",", filter.ROOM_IDs));
                }
                
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERESERV>(query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        internal List<SERESERV> GetDataBillGoods(Mrs00720Filter filter, bool isCancel)
        {
            List<SERESERV> result = new List<SERESERV>();
            try
            {
                string query = "select distinct \n";
                query += "0 PACKAGE_ID, \n";
                query += "0 ADD_PRICE, \n";
                query += "0 tdl_hein_service_type_id, \n";
                query += "nvl(bg.none_medi_service_id,0) service_id, \n";
                query += "0 medicine_id, \n";
                query += "0 material_id, \n";
                query += "0  SS_PATIENT_TYPE_ID, \n";

                query += "0 tdl_service_type_id, \n";
                query += " bg.amount*bg.price price, \n";
                query += "bg.price vir_price, \n";
                query += "bg.bill_id, \n";
                query += "tran1.treatment_id tdl_treatment_id, \n";
                query += "bg.price ss_price, \n";
                query += "bg.amount ss_amount, \n";
                query += "bg.amount*bg.price ss_total_price, \n";
                query += "bg.price tdl_price,\n";
                query += "bg.amount*bg.price tdl_total_patient_price, \n";
                query += "0 tdl_total_patient_price_bhyt, \n";
                query += "0 vir_total_hein_price, \n";
                query += "bg.amount * bg.price vir_total_price_no_expend, \n";
                query += "0 tdl_other_source_price, \n";

                query += "0 request_department_id, \n";
                query += "0 request_room_id, \n";
                query += "' ' request_loginname, \n";
                query += "' ' request_username, \n";
                query += "' ' service_req_code \n";
                query += "from his_bill_goods bg  \n";
                query += "join his_transaction tran1 on bg.bill_id = tran1.id\n";
                query += "join his_transaction tran on tran1.treatment_id = tran.treatment_id\n";
                query += "join his_treatment trea on tran.treatment_id = trea.id\n";
                query += "where 1=1\n";
                query += StrFilterTransaction(filter, isCancel);
                query += StrFilterTreatment(filter);

                if (filter.SS_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and 1=0 \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs)); //thanh toan
                }
                query += string.Format("and tran1.is_cancel is null \n"); //thanh toan
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERESERV>(query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        internal List<SERESERV> GetDataSereServNoBill(Mrs00720Filter filter)
        {
            List<SERESERV> result = new List<SERESERV>();
            try
            {
                string query = "select \n";
                query += "ss.PACKAGE_ID, \n";
                query += "ss.ADD_PRICE, \n";
                query += "ss.tdl_hein_service_type_id, \n";
                query += "ss.service_id, \n";
                query += "ss.medicine_id, \n";
                query += "ss.material_id, \n";
                query += "ss.patient_type_id SS_PATIENT_TYPE_ID, \n";

                query += "ss.tdl_service_type_id, \n";
                query += "0 price, \n";
                query += "ss.vir_price, \n";
                query += "0 bill_id, \n";
                query += "ss.tdl_treatment_id, \n";
                query += "ss.price ss_price, \n";
                query += "ss.amount ss_amount, \n";
                query += "ss.vir_total_price ss_total_price, \n";
                query += "ss.vir_price tdl_price,\n";
                query += "ss.vir_total_patient_price tdl_total_patient_price, \n";
                query += "ss.vir_total_patient_price_bhyt tdl_total_patient_price_bhyt, \n";
                query += "ss.vir_total_hein_price, \n";
                query += "ss.vir_total_price_no_expend, \n";
                query += "ss.other_source_price tdl_other_source_price, \n";

                query += "sr.request_department_id, \n";
                query += "sr.request_room_id, \n";
                query += "sr.request_loginname, \n";
                query += "sr.request_username, \n";
                query += "sr.service_req_code \n";
                query += "from his_sere_serv ss\n";
                query += "join v_his_treatment_fee trea on ss.tdl_treatment_id = trea.id\n";
                query += "join his_service_req sr on ss.service_req_id = sr.id\n";
                query += "where 1=1 and ss.is_delete=0 and ss.is_no_execute is null and sr.is_no_execute is null and sr.is_delete=0\n";
                query += StrFilterTreatment(filter);
                query += string.Format("and trea.IS_ACTIVE={0}  \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                query += string.Format("and trea.FEE_LOCK_TIME between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("and trea.total_bill_amount = 0 \n");
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and trea.FEE_LOCK_ROOM_ID in ({0})  \n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs)); //phong thu
                }

                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and tran.fee_lock_loginname in ('{0}')  \n", string.Join("','", filter.CASHIER_LOGINNAMEs)); //thu ngan
                }
                if (!string.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                {
                    query += string.Format("and tran.fee_lock_loginname = '{0}'  \n", filter.CASHIER_LOGINNAME);
                }
                if (filter.SS_PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and ss.PATIENT_TYPE_ID in ({0}) \n", string.Join(",", filter.SS_PATIENT_TYPE_IDs)); //thanh toan
                }


                if (filter.DEPARTMENT_ID != null)
                {
                    query += string.Format("and sr.request_department_id = {0}  \n", filter.DEPARTMENT_ID);
                }

                if (filter.DEPARTMENT_IDs != null)
                {
                    query += string.Format("and sr.request_department_id in ({0})  \n", string.Join(",", filter.DEPARTMENT_IDs));
                }
                if (filter.ROOM_IDs != null)
                {
                    query += string.Format("and sr.request_room_id in ({0})  \n", string.Join(",", filter.ROOM_IDs));
                }

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<SERESERV>(query);

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }


        internal string StrFilterTransaction(Mrs00720Filter filter, bool IsCancel)
        {
            string query = "";
            try
            {
                query += "and tran.is_delete=0 \n";
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and tran.cashier_room_id in ({0})  \n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs)); //phong thu
                }

                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and tran.cashier_loginname in ('{0}')  \n", string.Join("','", filter.CASHIER_LOGINNAMEs)); //thu ngan
                }
                if (!string.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                {
                    query += string.Format("and tran.cashier_loginname = '{0}'  \n", filter.CASHIER_LOGINNAME);
                }
                if (filter.ACCOUNT_BOOK_IDs != null)
                {
                    query += string.Format("and tran.account_book_id in ({0})  \n", string.Join(",", filter.ACCOUNT_BOOK_IDs));
                }
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("and (tran.PAY_FORM_ID in ({0}))  \n", string.Join(",", filter.PAY_FORM_IDs));
                }

                if (IsCancel == true)
                {
                    query += "and tran.is_cancel = 1 \n";
                    query += string.Format("and tran.cancel_time between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                }
                else
                {
                    query += string.Format("and tran.transaction_time between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                    //query += "and tran.is_cancel is null \n";
                }
                if (filter.INPUT_DATA_ID_CANCEL_TYPE == 1)
                {
                    query += "and tran.is_cancel = 1 \n";
                }
                if (filter.INPUT_DATA_ID_CANCEL_TYPE == 2)
                {
                    query += "and tran.is_cancel is null \n";
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return "";
            }
            return query;
        }

        internal string StrFilterTreatment(Mrs00720Filter filter)
        {
            string query = "";
            try
            {
                if (!string.IsNullOrWhiteSpace(filter.EXACT_TREATMENT_CODE))
                {
                    query += string.Format("and trea.TREATMENT_CODE = '{0}'  \n", filter.EXACT_TREATMENT_CODE);

                }
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_TREATMENT_TYPE_ID in ({0})  \n", string.Join(",", filter.TREATMENT_TYPE_IDs));

                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_TYPE_ID in ({0})  \n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }

                if (filter.END_DEPARTMENT_ID != null)
                {
                    query += string.Format("and trea.end_department_id = {0} \n", filter.END_DEPARTMENT_ID);
                }

                if (filter.TDL_PATIENT_CLASSIFY_IDs != null)
                {
                    query += string.Format("and trea.TDL_PATIENT_CLASSIFY_ID in ({0})  \n", string.Join(",", filter.TDL_PATIENT_CLASSIFY_IDs)); //dt chi tiet
                }

                if (filter.END_DEPARTMENT_IDs != null)
                {
                    query += string.Format("and trea.end_department_id in ({0}) \n", string.Join(",", filter.END_DEPARTMENT_IDs));
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return "";
            }
            return query;
        }

        internal List<HIS_TRANSACTION> GetTransaction(Mrs00720Filter filter, bool isCancel)
        {
            List<HIS_TRANSACTION> result = new List<HIS_TRANSACTION>();
            try
            {
                string query = "";
                query += "select \n";
                query += "tran.* \n";
                query += "from his_transaction tran \n";
                query += "join his_treatment trea on trea.id=tran.treatment_id \n";
                query += "where 1=1\n";
                query += StrFilterTransaction(filter, isCancel);
                query += StrFilterTreatment(filter);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_TRANSACTION>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        internal List<V_HIS_TREATMENT_FEE> GetTreatmentFee(Mrs00720Filter filter, bool isCancel)
        {
            List<V_HIS_TREATMENT_FEE> result = new List<V_HIS_TREATMENT_FEE>();
            try
            {
                string query = "";
                query += "select distinct\n";
                query += "(SELECT NVL(SUM(DE.AMOUNT),0) FROM HIS_TRANSACTION DE WHERE (DE.TDL_SERE_SERV_DEPOSIT_COUNT IS NULL OR DE.TDL_SERE_SERV_DEPOSIT_COUNT =0) and DE.TREATMENT_ID = trea.ID AND (DE.IS_CANCEL IS NULL OR DE.IS_CANCEL <> 1) AND DE.IS_ACTIVE = 1 AND DE.TRANSACTION_TYPE_ID = 1) TOTAL_DEPOSIT_AMOUNT, \n";
                query += "trea.* \n";
                query += "from his_transaction tran \n";
                query += "join v_his_treatment_fee trea on trea.id=tran.treatment_id \n";
                query += "where 1=1\n";
                query += StrFilterTransaction(filter, isCancel);
                query += StrFilterTreatment(filter);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT_FEE>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        internal List<V_HIS_TREATMENT_FEE> GetTreatmentFeeNoBill(Mrs00720Filter filter)
        {
            List<V_HIS_TREATMENT_FEE> result = new List<V_HIS_TREATMENT_FEE>();
            try
            {
                string query = "";
                query += "select distinct\n";
                query += "(SELECT NVL(SUM(DE.AMOUNT),0) FROM HIS_TRANSACTION DE WHERE (DE.TDL_SERE_SERV_DEPOSIT_COUNT IS NULL OR DE.TDL_SERE_SERV_DEPOSIT_COUNT =0) and DE.TREATMENT_ID = trea.ID AND (DE.IS_CANCEL IS NULL OR DE.IS_CANCEL <> 1) AND DE.IS_ACTIVE = 1 AND DE.TRANSACTION_TYPE_ID = 1) TOTAL_DEPOSIT_AMOUNT, \n";
                query += "trea.* \n";
                query += "from v_his_treatment_fee trea\n";
                query += "where 1=1\n";
                query += string.Format("and trea.IS_ACTIVE={0}  \n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                query += string.Format("and trea.FEE_LOCK_TIME between {0} and {1}  \n", filter.TIME_FROM, filter.TIME_TO);
                query += string.Format("and trea.total_bill_amount = 0 \n");
                if (filter.EXACT_CASHIER_ROOM_IDs != null)
                {
                    query += string.Format("and trea.FEE_LOCK_ROOM_ID in ({0})  \n", string.Join(",", filter.EXACT_CASHIER_ROOM_IDs)); //phong thu
                }

                if (filter.CASHIER_LOGINNAMEs != null)
                {
                    query += string.Format("and tran.fee_lock_loginname in ('{0}')  \n", string.Join("','", filter.CASHIER_LOGINNAMEs)); //thu ngan
                }
                if (!string.IsNullOrEmpty(filter.CASHIER_LOGINNAME))
                {
                    query += string.Format("and tran.fee_lock_loginname = '{0}'  \n", filter.CASHIER_LOGINNAME);
                }
                query += StrFilterTreatment(filter);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);

                result = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_TREATMENT_FEE>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        internal List<Mrs00720ServiceRDO> GetService()
        {
            List<Mrs00720ServiceRDO> result = new List<Mrs00720ServiceRDO>();
            try
            {
                string query = "select distinct pr.service_code, pr.service_name from his_service pr\n";
                query += "join his_service sv on sv.parent_id = pr.id\n";
                query += string.Format("where sv.service_type_id <> {0} and sv.service_type_id <> {1} and sv.service_type_id <> {2}", IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU);
                Inventec.Common.Logging.LogSystem.Info("SQL:" + query);
                result = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00720ServiceRDO>(query);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return null;
            }
            return result;
        }

        public List<PATIENT_CLASSIFY> GetClassify(List<long> classfyIds)
        {
            List<PATIENT_CLASSIFY> result = null;
            try
            {
                string query = string.Format("select * from his_patient_classify where id in ({0})\n", string.Join(",", classfyIds));
                result = new MOS.DAO.Sql.SqlDAO().GetSql<PATIENT_CLASSIFY>(query);
                LogSystem.Info("PATIENT_CLASSIFY: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;
            
            
        }

        public List<CASHIER_USER> GetCashierUser(List<string> cashierLoginnames)
        {
            List<CASHIER_USER> result = null;
            try
            {
                string query = string.Format("select * from acs_rs.acs_user where loginname in ('{0}')\n", string.Join("','", cashierLoginnames));
                result = new MOS.DAO.Sql.SqlDAO().GetSql<CASHIER_USER>(query);
                LogSystem.Info("CASHIER_USER: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;


        }
        public List<HIS_NONE_MEDI_SERVICE> GetNoneMediService()
        {
            List<HIS_NONE_MEDI_SERVICE> result = null;
            try
            {
                string query = string.Format("select * from HIS_NONE_MEDI_SERVICE\n");
                result = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_NONE_MEDI_SERVICE>(query);
                LogSystem.Info("HIS_NONE_MEDI_SERVICE: " + query);
            }
            catch (Exception e)
            {
                LogSystem.Error(e);
                return result;
            }
            return result;


        }
       
    }
}
