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
using MOS.MANAGER.HisCashierRoom;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServBill;
using System.Data;

namespace MRS.Processor.Mrs00330
{
    public partial class ManagerSql : BusinessBase
    {

        //public List<HIS_PTA> GetPta(long MinTreatmentId,long MaxTreatmentId)
        //{
        //    List<HIS_PTA> result = new List<HIS_PTA>();
        //    try
        //    {
        //        string query = "";
        //        query += "SELECT ";
        //        query += "PTA.ID, ";
        //        query += "PTA.LOG_TIME, ";
        //        query += "PTA.TREATMENT_ID, ";
        //        query += "PTA.TREATMENT_TYPE_ID, ";
        //        query += "PTA.PATIENT_TYPE_ID, ";
        //        query += "PTA.HEIN_CARD_NUMBER ";
        //        query += "FROM HIS_RS.HIS_PATIENT_TYPE_ALTER PTA ";
        //        query += "WHERE 1=1 ";
        //        string.Format("AND PTA.TREATMENT_ID BETWEEN {0} AND {1} ", MinTreatmentId, MaxTreatmentId);


        //        Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
        //        var rs = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PTA>(query);

        //        if (rs != null)
        //        {
        //            result = rs;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //        param.HasException = true;
        //        return null;
        //    }

        //    return result;
        //}

        public List<D_HIS_TREATMENT> GetTrea(Mrs00330Filter filter)
        {
            List<D_HIS_TREATMENT> result = new List<D_HIS_TREATMENT>();
            try
            {
                string query = "";
                query += "SELECT ";
                query += "TREA.*, ";
                query += "RO.ROOM_NAME, ";
                query += "RO.DEPARTMENT_NAME ";
                query += "FROM HIS_RS.HIS_TREATMENT TREA ";
                query += "LEFT JOIN HIS_RS.V_HIS_ROOM RO ON RO.ID=TREA.END_ROOM_ID ";
                query += "JOIN (select distinct treatment_id from his_rs.his_transaction where 1=1  ";


                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND PAY_FORM_ID = {0} ", filter.PAY_FORM_ID);
                }

                query += string.Format("AND (IS_CANCEL IS NULL AND TRANSACTION_TIME BETWEEN {0} AND {1} OR IS_CANCEL = 1 AND CANCEL_TIME BETWEEN {0} AND {1}) ", filter.TIME_FROM, filter.TIME_TO);

                query += ") TRAN ON TREA.ID=TRAN.TREATMENT_ID ";
                query += "WHERE 1=1 ";
                

                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_TREATMENT>(query);

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

       
        public List<D_HIS_SERE_SERV_BILL> GetSSB(Mrs00330Filter filter)
        {
            List<D_HIS_SERE_SERV_BILL> result = new List<D_HIS_SERE_SERV_BILL>();
            try
            {
                string query = "";
                query += " --chi tiet thu tien cac dich vu\n";
                query += "select\n";
                query += "tran.is_cancel,\n";
                query += "ssb.sere_serv_id id,\n";
                query += "ssb.tdl_patient_type_id patient_type_id,\n";
                query += "ssb.tdl_service_type_id,\n";
                query += "ssb.tdl_hein_service_type_id,\n";
                query += "(nvl(ssb.tdl_total_hein_price,0)+nvl(ssb.tdl_total_patient_price,0)) vir_total_price,\n";
                query += "ssb.tdl_amount amount,\n";
                query += "ssb.tdl_real_price vir_price,\n";
                query += "ssb.tdl_service_id service_id,\n";
                query += "ssb.tdl_service_code,\n";
                query += "ssb.tdl_service_name,\n";
                query += "nvl(sr.request_room_id,0) tdl_request_room_id,\n";
                query += "ssb.tdl_treatment_id,\n";
                query += "trea.tdl_hein_card_number hein_card_number,\n";
                query += "ssb.tdl_total_hein_price vir_total_hein_price,\n";
                query += "sr.request_loginname tdl_request_loginname,\n";
                query += "sr.request_username tdl_request_username,\n";
                query += "ssb.tdl_total_patient_price_bhyt vir_total_patient_price_bhyt,\n";
                query += "nvl(sr.execute_room_id,0) tdl_execute_room_id,\n";
                query += "nvl(sr.execute_department_id,0) tdl_execute_department_id,\n";
                query += "trea.fee_lock_time,\n";
                query += "trea.end_room_id,\n";
                query += "trea.last_department_id,\n";
                query += "trea.last_department_id end_department_id,\n";
                query += "sv.parent_id parent_service_id,\n";
                //query += "(case when svpr.id is not null then  svpr.service_code else svt.service_type_code end) as parent_service_code,\n";
                //query += "(case when svpr.id is not null then  svpr.service_name else svt.service_type_name end) as parent_service_name,\n";
                query += "ssb.bill_id,\n";
                //query += "svt.service_type_name,\n";
                query += "tran.transaction_time,\n";
                query += "tran.bill_type_id,\n";
                query += "tran.num_order as transaction_num_order,\n";
                query += "tran.cancel_time,\n";
                query += "tran.tdl_patient_address,\n";
                query += "tran.tdl_patient_code,\n";
                query += "tran.tdl_patient_dob,\n";
                query += "tran.tdl_patient_first_name,\n";
                query += "tran.tdl_patient_gender_name,\n";
                query += "tran.tdl_patient_last_name,\n";
                query += "tran.tdl_patient_name,\n";
                query += "sr.execute_loginname,\n";
                query += "sr.execute_username,\n";
                query += "ssb.price as bill_price\n";
                query += "from his_rs.his_sere_serv_bill ssb\n";
                query += "join his_rs.his_transaction tran on tran.id = ssb.bill_id\n";
                //query += "join his_rs.his_sere_serv ss on ss.id = ssb.sere_serv_id\n";
                query += "left join his_rs.his_service_req sr on sr.id = ssb.tdl_service_req_id\n";
                query += "left join his_rs.his_service sv on sv.id = ssb.tdl_service_id\n";
                //query += "join his_rs.his_service_type svt on svt.id = ssb.tdl_service_type_id\n";
                //query += "left join his_rs.his_service svpr on svpr.id = sv.parent_id\n";
                query += "left join his_rs.his_treatment trea on trea.id = ssb.tdl_treatment_id\n";
                query += "where 1=1\n";

                query += "AND SSB.IS_DELETE =0\n ";
                //query += "AND TRAN.IS_CANCEL IS NULL AND TRAN.IS_DELETE =0 ";
                query += "AND TRAN.IS_DELETE =0\n ";

                //if (filter.TIME_TO != null)
                //{
                //    query += string.Format("AND TRAN.TRANSACTION_TIME <= {0} ", filter.TIME_TO);
                //}

                //if (filter.TIME_FROM != null)
                //{
                //    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                //}
                if (filter.PAY_FORM_IDs != null)
                {
                    query += string.Format("AND TRAN.PAY_FORM_ID in ({0})\n ", string.Join(",",filter.PAY_FORM_IDs));
                }
                if (filter.PAY_FORM_ID != null)
                {
                    query += string.Format("AND TRAN.PAY_FORM_ID = {0}\n ", filter.PAY_FORM_ID);
                }

                if (filter.SERE_SERV_PATIENT_TYPE_ID != null)
                {
                    query += string.Format("AND ssb.TDL_PATIENT_TYPE_ID = {0} \n", filter.SERE_SERV_PATIENT_TYPE_ID);
                }

                if (filter.REQUEST_DEPARTMENT_IDs != null && filter.REQUEST_DEPARTMENT_IDs.Count > 0)
                {
                    query += string.Format("AND sr.REQUEST_DEPARTMENT_ID IN ({0})\n ", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }

                if (filter.EXACT_PARENT_SERVICE_IDs != null && filter.EXACT_PARENT_SERVICE_IDs.Count > 0)
                {
                    query += string.Format("AND SV.PARENT_ID IN ({0}) \n", string.Join(",", filter.EXACT_PARENT_SERVICE_IDs));
                }

                if (filter.SERVICE_TYPE_IDs != null && filter.SERVICE_TYPE_IDs.Count > 0)
                {
                    query += string.Format("AND ssb.TDL_SERVICE_TYPE_ID IN ({0}) \n", string.Join(",", filter.SERVICE_TYPE_IDs));
                }

                ////lấy các giao dịch ko bị hủy hoặc có thời gian hủy trong thời gian báo cáo
                //query += string.Format("AND TRAN.IS_CANCEL IS NULL OR ({0} AND {1}) ", string.Format("TRAN.CANCEL_TIME <= {0} ", filter.TIME_TO), string.Format("TRAN.CANCEL_TIME >= {0} ", filter.TIME_FROM));

                query += string.Format("AND (TRAN.TRANSACTION_TIME BETWEEN {0} AND {1}) \n", filter.TIME_FROM, filter.TIME_TO);
                Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
                var rs = new MOS.DAO.Sql.SqlDAO().GetSql<D_HIS_SERE_SERV_BILL>(query);

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


        public DataTable GetTransactionDetail(Mrs00330Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "select tran.tdl_patient_code,tran.tdl_patient_name,acc.TEMPLATE_CODE, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME,sum(ss.vir_total_patient_price) as vir_total_patient_price, sum(ssb.price) as bill_amount,MAX(dp.department_name) KEEP (DENSE_RANK LAST ORDER BY dpt.id) as curent_department_name from  his_rs.his_sere_serv_bill ssb join his_rs.his_sere_serv ss on ss.id = ssb.sere_serv_id join his_rs.his_transaction tran on tran.id = ssb.bill_id left join his_rs.his_department_tran dpt on (dpt.department_in_time<tran.transaction_time  and dpt.treatment_id = tran.treatment_id) left join his_rs.his_department dp on dp.id = dpt.department_id left join his_rs.his_account_book acc on acc.id = tran.account_book_id where tran.is_cancel is null and transaction_type_id =3 ";

                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME >= {0} ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TRAN.TRANSACTION_TIME < {0} ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CASHIER_LOGINNAME ='{0}' ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CASHIER_ROOM_ID ='{0}' ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2 ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID =1 ";
                    }
                }

                query += "group by tran.tdl_patient_code,tran.tdl_patient_name,acc.TEMPLATE_CODE, tran.num_order, tran.TRANSACTION_CODE, tran.TRANSACTION_TIME  ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public DataTable GetFeeByNbill(Mrs00330Filter filter)
        {
            DataTable result = null;
            try
            {
                string query = "";
                query += "SELECT nbilldep.*, trea.*, dp.department_name end_department_name\r\n ";
                query += "FROM (\r\n ";
                query += "SELECT nbill.treatment_id, nbill.transaction_time, nbill.curent_department_id, SUM(dep.amount) AS deposit_amount, nbill.total_fee, nbill.total_service, nbill.total_exemption, nbill.prev_transaction_time, nbill.vir_total_hein_price, nbill.total_nutrition\r\n ";
                query += "FROM (\r\n ";
                query += "SELECT bill.treatment_id, bill.transaction_time, bill.total_fee, bill.total_service, bill.total_exemption, bill.total_nutrition, bill.vir_total_hein_price, bill.curent_department_id, MAX(mbill.transaction_time) AS prev_transaction_time\r\n ";
                query += "FROM (\r\n ";
                query += "SELECT tran.treatment_id, tran.transaction_time, (SUM(CASE WHEN tran.bill_type_id = 2 THEN 0 ELSE tran.amount END)-ssb.total_nutrition_fee) AS total_fee, (SUM(CASE WHEN tran.bill_type_id = 2 THEN tran.amount ELSE 0 END)-ssb.total_nutrition_service) AS total_service, SUM(nvl(tran.exemption, 0)) AS total_exemption, dpt.curent_department_id, (ssb.total_nutrition_fee+ssb.total_nutrition_service) as total_nutrition, ssb.vir_total_hein_price ";
                query += "FROM his_rs.his_transaction tran\r\n ";
                query += "LEFT JOIN ( SELECT treatment_id, MAX(department_id) KEEP(DENSE_RANK LAST ORDER BY id) AS curent_department_id FROM his_rs.his_department_tran GROUP BY treatment_id ) dpt ON dpt.treatment_id = tran.treatment_id\r\n ";
                query += "JOIN ( SELECT tdl_treatment_id, SUM(CASE WHEN tdl_service_type_id = 16 and tdl_bill_type_id is null THEN price ELSE 0 END) AS total_nutrition_fee, SUM(CASE WHEN tdl_service_type_id = 16 and tdl_bill_type_id=2 THEN price ELSE 0 END) AS total_nutrition_service, SUM(CASE WHEN tdl_total_hein_price > 0 THEN price ELSE 0 END) AS vir_total_hein_price FROM his_rs.his_sere_serv_bill GROUP BY tdl_treatment_id ) ssb ON ssb.tdl_treatment_id = tran.treatment_id\r\n ";
                query += "WHERE tran.transaction_type_id = 3 AND tran.Sale_type_id is null\r\n ";
                query += string.Format("AND ( tran.is_cancel IS NULL OR ( tran.cancel_time > {0} AND tran.is_cancel IS NOT NULL)\r\n ", filter.TIME_TO);

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("OR (TRAN.CANCEL_LOGINNAME <>'{0}' AND tran.is_cancel IS NOT NULL)\r\n ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("OR (TRAN.CANCEL_CASHIER_ROOM_ID <>{0} AND tran.is_cancel IS NOT NULL)\r\n ", filter.EXACT_CASHIER_ROOM_ID);
                }
                query += ")\r\n ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND tran.TRANSACTION_TIME >= {0}\r\n ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND tran.TRANSACTION_TIME < {0}\r\n ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND tran.CASHIER_LOGINNAME ='{0}'\r\n ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND tran.CASHIER_ROOM_ID ={0}\r\n ", filter.EXACT_CASHIER_ROOM_ID);
                }

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND tran.BILL_TYPE_ID=2\r\n ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND tran.BILL_TYPE_ID is null\r\n ";
                    }
                }

                query += "GROUP BY tran.treatment_id, tran.transaction_time, dpt.curent_department_id, ssb.vir_total_hein_price, ssb.total_nutrition_fee, ssb.total_nutrition_service ) bill\r\n ";
                query += "LEFT JOIN his_rs.his_transaction mbill ON ( mbill.treatment_id = bill.treatment_id AND mbill.transaction_time < bill.transaction_time AND mbill.transaction_type_id = 3 AND mbill.is_cancel IS NULL AND mbill.Sale_type_id is null)\r\n ";
                query += "GROUP BY bill.treatment_id, bill.transaction_time, bill.total_fee, bill.total_service, bill.total_exemption, bill.curent_department_id, bill.total_nutrition, bill.vir_total_hein_price ) nbill\r\n ";
                query += "LEFT JOIN his_rs.his_transaction dep ON ( dep.transaction_type_id = 1 AND dep.is_cancel IS NULL AND dep.treatment_id = nbill.treatment_id AND dep.transaction_time <= nbill.transaction_time AND ( dep.transaction_time > nbill.prev_transaction_time OR nbill.prev_transaction_time IS NULL ) )\r\n ";
                query += "GROUP BY nbill.treatment_id, nbill.transaction_time, nbill.curent_department_id, nbill.total_nutrition, nbill.vir_total_hein_price, nbill.total_fee, nbill.total_service, nbill.total_exemption, nbill.prev_transaction_time ";
                query += "UNION ALL (\r\n ";
                query += "SELECT tran.treatment_id, tran.cancel_time, dpt.curent_department_id, NULL deposit_amount, SUM(CASE WHEN tran.bill_type_id = 2 THEN 0 ELSE - tran.amount END)+ssb.total_nutrition_fee AS total_fee, SUM(CASE WHEN tran.bill_type_id = 2 THEN - tran.amount ELSE 0 END)+ssb.total_nutrition_service AS total_service, SUM(nvl(-tran.exemption, 0)) AS total_exemption, NULL prev_transaction_time, NULL vir_total_hein_price, -(ssb.total_nutrition_fee+ssb.total_nutrition_service) AS total_nutrition \r\n ";
                query += "FROM his_rs.his_transaction tran\r\n ";
                query += "LEFT JOIN ( SELECT treatment_id, MAX(department_id) KEEP(DENSE_RANK LAST ORDER BY id) AS curent_department_id FROM his_rs.his_department_tran GROUP BY treatment_id ) dpt ON dpt.treatment_id = tran.treatment_id\r\n ";
                query += "JOIN ( SELECT tdl_treatment_id, SUM(CASE WHEN tdl_service_type_id = 16 and tdl_bill_type_id is null THEN price ELSE 0 END) AS total_nutrition_fee, SUM(CASE WHEN tdl_service_type_id = 16 and tdl_bill_type_id=2 THEN price ELSE 0 END) AS total_nutrition_service, SUM(CASE WHEN tdl_total_hein_price > 0 THEN price ELSE 0 END) AS vir_total_hein_price FROM his_rs.his_sere_serv_bill GROUP BY tdl_treatment_id ) ssb ON ssb.tdl_treatment_id = tran.treatment_id\r\n ";
                query += "WHERE tran.transaction_type_id = 3 AND tran.cancel_time IS NOT NULL AND tran.Sale_type_id is null\r\n ";
                if (filter.TIME_FROM != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME >= {0}\r\n ", filter.TIME_FROM);
                }
                if (filter.TIME_TO != null)
                {
                    query += string.Format("AND TRAN.CANCEL_TIME < {0}\r\n ", filter.TIME_TO);
                }

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("AND TRAN.CANCEL_LOGINNAME ='{0}'\r\n ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("AND TRAN.CANCEL_CASHIER_ROOM_ID ='{0}'\r\n ", filter.EXACT_CASHIER_ROOM_ID);
                }

                query += "and ( ";

                query += string.Format("TRAN.TRANSACTION_TIME <= {0}\r\n ", filter.TIME_FROM);

                if (filter.CASHIER_LOGINNAME != null)
                {
                    query += string.Format("OR TRAN.CASHIER_LOGINNAME <>'{0}'\r\n ", filter.CASHIER_LOGINNAME);
                }

                if (filter.EXACT_CASHIER_ROOM_ID != null)
                {
                    query += string.Format("OR TRAN.CASHIER_ROOM_ID <>'{0}'\r\n ", filter.EXACT_CASHIER_ROOM_ID);
                }
                query += ")\r\n ";

                if (filter.IS_BILL_NORMAL.HasValue)
                {
                    if (!filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID=2\r\n ";
                    }
                    else if (filter.IS_BILL_NORMAL.Value)
                    {
                        query += "AND TRAN.BILL_TYPE_ID is null\r\n ";
                    }
                }

                query += "GROUP BY tran.treatment_id, tran.cancel_time, dpt.curent_department_id, ssb.total_nutrition_fee, ssb.total_nutrition_service ) ) nbilldep\r\n ";

                query += "JOIN his_rs.his_treatment trea ON trea.id = nbilldep.treatment_id LEFT JOIN his_rs.his_department dp ON dp.id = nbilldep.curent_department_id	ORDER BY trea.id, nbilldep.transaction_time\r\n ";

                List<string> errors = new List<string>();
                result = new MOS.DAO.Sql.SqlDAO().Execute(query, ref errors);
                Inventec.Common.Logging.LogSystem.Info(string.Join(", ", errors));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }
    }

    public class D_HIS_SERE_SERV_BILL
    {
        public long BILL_ID { get; set; }
        public decimal BILL_PRICE { get; set; }
        //public string SERVICE_TYPE_NAME { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public long? PARENT_SERVICE_ID { get; set; }
        public string PARENT_SERVICE_CODE { get; set; }
        public string PARENT_SERVICE_NAME { get; set; }
        //public string CATEGORY_CODE { get; set; }
        //public string CATEGORY_NAME { get; set; }
        public string SERVICE_TYPE_CODE { get; set; }
        public string SERVICE_TYPE_NAME { get; set; }
        public string HEIN_SERVICE_TYPE_CODE { get; set; }
        public string HEIN_SERVICE_TYPE_NAME { get; set; }
        public long? BILL_TYPE_ID { get; set; }//hóa đơn thường/dịch vụ

        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public string TDL_PATIENT_FIRST_NAME { get; set; }
        public string TDL_PATIENT_GENDER_NAME { get; set; }
        public string TDL_PATIENT_LAST_NAME { get; set; }
        public string TDL_PATIENT_NAME { get; set; }

        public string EXECUTE_LOGINNAME { get; set; }
        public string EXECUTE_USERNAME { get; set; }

        public long TRANSACTION_NUM_ORDER { get; set; }
        public long? CANCEL_TIME { get; set; }
        public long? FEE_LOCK_TIME { get; set; }
        //public string TRANSACTION_SYMBOL_CODE { get; set; }
        //public string TRANSACTION_TEMPLATE_CODE { get; set; }
        public long PATIENT_TYPE_ID { get; set; }//query += "ss.patient_type_id,\n";
        public long TDL_SERVICE_TYPE_ID { get; set; }//      query += "ss.tdl_service_type_id,\n";
        public long? TDL_HEIN_SERVICE_TYPE_ID { get; set; }//      query += "ss.tdl_hein_service_type_id,\n";
        public decimal? VIR_TOTAL_PRICE { get; set; }//      query += "ss.vir_total_price,\n";
        public decimal AMOUNT { get; set; }//      query += "ss.amount,\n";
        public decimal? VIR_PRICE { get; set; }//      query += "ss.vir_price,\n";
        public long SERVICE_ID { get; set; }//      query += "ss.service_id,\n";
        public string TDL_SERVICE_CODE { get; set; }//      query += "ss.tdl_service_code,\n";
        public string TDL_SERVICE_NAME { get; set; }//      query += "ss.tdl_service_name,\n";
        public long? TDL_REQUEST_ROOM_ID { get; set; }//      query += "ss.tdl_request_room_id,\n";
        public long? TDL_TREATMENT_ID { get; set; } //      query += "ss.tdl_treatment_id,\n";
        public string HEIN_CARD_NUMBER { get; set; }//      query += "ss.hein_card_number,\n";
        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }//      query += "ss.vir_total_hein_price,\n";
        public string TDL_REQUEST_LOGINNAME { get; set; }
        public string TDL_REQUEST_USERNAME { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public long? TDL_EXECUTE_DEPARTMENT_ID { get; set; }

        public long? TDL_REQUEST_DEPARTMENT_ID { get; set; }

        public long ID { get; set; }
        //phần bổ sung sau
        public decimal BHYT_PRICE { get; set; }
        public decimal VP_PRICE { get; set; }
        public decimal DV_PRICE { get; set; }
        public long BHYT_NUM_ORDER { get; set; }
        public long VP_NUM_ORDER { get; set; }
        public long DV_NUM_ORDER { get; set; }

        public decimal TOTAL_BHYT_PRICE { get; set; }
        public decimal TOTAL_VP_PRICE { get; set; }
        public decimal TOTAL_DV_PRICE { get; set; }

        public string DOCTOR_LOGINNAME { get; set; }
        public string DOCTOR_USERNAME { get; set; }

        public string TDL_REQUEST_DEPARTMENT_CODE { get; set; }
        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_CODE { get; set; }
        public string TDL_EXECUTE_DEPARTMENT_NAME { get; set; }

        public string CANCEL_TIME_STR { get; set; }
        public string FEE_LOCK_TIME_STR { get; set; }

        public long? END_ROOM_ID { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }
    }

}
