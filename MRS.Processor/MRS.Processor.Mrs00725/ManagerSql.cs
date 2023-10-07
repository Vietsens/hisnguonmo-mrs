using System;
using System.Collections.Generic;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.DAO.Sql;
using MRS.Processor.Mrs00725;
using MOS.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00725
{
	internal class ManagerSql
	{
		public class SERVICE_REQ 
		{
			public string PR_CODE { get; set; }
			public string PR_NAME { get; set; }

			public string SERVICE_CODE { get; set; }
			public string SERVICE_NAME { get; set; }
			public long TREATMENT_ID { get; set; }

            public long? OUT_TIME { get; set; }

			public long? EXECUTE_ROOM_ID { get; set; }

            public long? REQUEST_ROOM_ID { get; set; }

            public long? EXAM_ROOM_ID { get; set; }

            public long? EXECUTE_DEPARTMENT_ID { get; set; }

			public long? REQUEST_DEPARTMENT_ID { get; set; }

            public string REQUEST_LOGINNAME { get; set; }

            public string REQUEST_USERNAME { get; set; }

			public long SERE_SERV_ID { get; set; }

			public long INTRUCTION_TIME { get; set; }

            public string TDL_PATIENT_CODE { get; set; }

            public string TDL_PATIENT_NAME { get; set; }

            public long? TDL_PATIENT_DOB { get; set; }

            public long? TDL_PATIENT_GENDER_ID { get; set; }

            public long IN_TIME { get; set; }

            public decimal? TREATMENT_DAY_COUNT { get; set; }

            public string ICD_CODE { get; set; }

            public string ICD_NAME { get; set; }

            public string ICD_SUB_CODE { get; set; }

            public long TDL_SERVICE_TYPE_ID { get; set; }

            public short? IS_SENT_EXT { get; set; }


            public string TREATMENT_CODE { get; set; }

            public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }

            public decimal AMOUNT { get; set; }

            public decimal? VIR_PRICE { get; set; }

			public long PATIENT_TYPE_ID { get; set; }

			public long? OTHER_PAY_SOURCE_ID { get; set; }

			public decimal? HEIN_PRICE { get; set; }

            public decimal PRICE { get; set; }

            public decimal? VAT_RATIO { get; set; }

            public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }

            public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

            public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }

            public short? IS_EXPEND { get; set; }

            public decimal? VIR_TOTAL_PRICE_NO_EXPEND { get; set; }

            public long? EXPEND_TYPE_ID { get; set; }

            public decimal? DISCOUNT { get; set; }

            public string SERVICE_REQ_CODE { get; set; }

            public string EXECUTE_LOGINNAME { get; set; }

            public string EXECUTE_USERNAME { get; set; }

            public long SERVICE_REQ_ID { get; set; }

            public decimal? VIR_TOTAL_PRICE { get; set; }

            public long SERVICE_ID { get; set; }
        }

		internal List<SERVICE_REQ> GetServiceReq(Mrs00725Filter filter)
		{

			List<SERVICE_REQ> list = new List<SERVICE_REQ>();
			CommonParam val = new CommonParam();
			try
			{
				string text = "--danh sach y lenh\n";
				text += "select \n";
                text += "SS.SERVICE_ID,";
				text += "SV.SERVICE_CODE,\n";
				text += "SV.SERVICE_NAME,\n";

				text += "PR.SERVICE_CODE PR_CODE,\n";
                text += "PR.SERVICE_NAME PR_NAME,\n";
                text += "trea.TREATMENT_CODE,\n";
                text += "trea.TDL_PATIENT_CODE,\n";
                text += "trea.TDL_PATIENT_NAME,\n";
                text += "trea.TDL_PATIENT_DOB,\n";
                text += "trea.TDL_PATIENT_GENDER_ID,\n";
                text += "trea.id TREATMENT_ID,\n";
                text += "trea.in_time, \n";
                text += "trea.out_time, \n";
                text += "trea.treatment_day_count, \n";
                text += "trea.icd_code, \n";
                text += "trea.icd_name, \n";
                text += "trea.icd_sub_code, \n";
				text += "sr.intruction_time,\n";
				text += "sr.request_room_id, \n";
				text += "sr.execute_room_id, \n";
				text += "sr.execute_department_id, \n";
                text += "sr.request_department_id, \n";
                text += "sr.execute_loginname, \n";
                text += "sr.execute_username, \n";
                text += "sr.request_loginname, \n";
                text += "sr.request_username, \n";
                text += "sr.SERVICE_REQ_CODE, \n";
                text += "sr.id SERVICE_REQ_ID, \n";
                text += "ss.vir_price, \n";
                text += "ss.hein_price, \n";
                text += "ss.price, \n";
                text += "ss.vat_ratio, \n";
                text += "ss.tdl_service_type_id, \n";
                text += "ss.TDL_HEIN_SERVICE_BHYT_CODE, \n";
                text += "ss.patient_type_id, \n";
                text += "ss.EXPEND_TYPE_ID, \n";
                text += "ss.exp_mest_medicine_id, \n";
                text += "ss.IS_SENT_EXT, \n";
                text += "ss.IS_EXPEND, \n";
                text += "ss.id SERE_SERV_ID, \n";
                text += "ss.other_pay_source_id, \n";
                text += "(case when ss.tdl_service_type_id=1 then sr.execute_room_id else sr.request_room_id end) exam_room_id, \n";
                text += "sum(ss.AMOUNT) AMOUNT, \n";
				text += "sum(ss.vir_total_price) vir_total_price, \n";
                text += "sum(ss.VIR_TOTAL_HEIN_PRICE) VIR_TOTAL_HEIN_PRICE, \n";
                text += "sum(ss.VIR_TOTAL_PATIENT_PRICE_BHYT) VIR_TOTAL_PATIENT_PRICE_BHYT, \n";
                text += "sum(ss.VIR_TOTAL_PATIENT_PRICE) VIR_TOTAL_PATIENT_PRICE, \n";
                text += "sum(ss.VIR_TOTAL_PRICE_NO_EXPEND) VIR_TOTAL_PRICE_NO_EXPEND, \n";
                text += "sum(ss.DISCOUNT) DISCOUNT, \n";
				text += "1 \n";
				text += "from his_service_req sr\n";
				text += "join his_treatment trea on sr.treatment_id = trea.id\n";
				text += "join his_sere_serv ss on sr.id = ss.service_req_id \n";
                text += "join his_service sv on ss.service_id = sv.id \n";
                text += "left join his_service pr on sv.parent_id = pr.id \n";
                text += "left join his_execute_room exr on exr.room_id = sr.execute_room_id \n";
				text += "where 1=1\n";
                if (filter.INPUT_DATA_ID_TIME_TYPE != null)
                {
                    if (filter.INPUT_DATA_ID_TIME_TYPE == 1) //vào viện
                    {
						text += string.Format("and trea.in_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
					}
                    else if (filter.INPUT_DATA_ID_TIME_TYPE == 2) //ra viện
					{
						text += string.Format("and trea.out_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
					}
                    else if (filter.INPUT_DATA_ID_TIME_TYPE == 3) //chỉ định
                    {
						text += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
					}
                }
                else
                {
					text += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
				}
				if(filter.REQUEST_ROOM_IDs != null)
                {
					text += string.Format("and sr.request_room_id in ({0}) \n", string.Join(",", filter.REQUEST_ROOM_IDs));
                }
				if (filter.TREATMENT_TYPE_IDs != null)
				{
					text += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
				}
				if (filter.SERVICE_REQ_STT_ID.HasValue)
				{
					text += string.Format("and sr.service_req_stt_id = {0}\n", filter.SERVICE_REQ_STT_ID);
				}
				if (filter.EXACT_EXECUTE_ROOM_IDs != null)
				{
                    text += string.Format("and exr.id in ({0})\n", string.Join(",", filter.EXACT_EXECUTE_ROOM_IDs));
				}
				if (filter.CURRENTBRANCH_DEPARTMENT_IDs != null)
				{
					text += string.Format("and sr.execute_DEPARTMENT_id in ({0})\n", string.Join(",", filter.CURRENTBRANCH_DEPARTMENT_IDs));
				}
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    text += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
				if(filter.SERVICE_TYPE_IDs != null)
                {
					text += string.Format("and ss.tdl_service_type_id in ({0})\n", string.Join(",", filter.SERVICE_TYPE_IDs));
				}
				if (filter.PARENT_SERVICE_IDs != null)
				{
					text += string.Format("and pr.id in ({0})\n", string.Join(",", filter.PARENT_SERVICE_IDs));
				}
				if (filter.PATIENT_TYPE_IDs != null)
				{
					text += string.Format("and ss.patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
				}
				if (filter.TDL_PATIENT_TYPE_IDs != null)
				{
					text += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
				}
                if (filter.INPUT_DATA_ID_FINANCE_TYPE != null)
                {
                    if (filter.INPUT_DATA_ID_FINANCE_TYPE == 1)
                    {
                        text += string.Format("and trea.IS_ACTIVE = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE);
                    }
                    if (filter.INPUT_DATA_ID_FINANCE_TYPE == 2)
                    {
                        text += string.Format("and trea.IS_ACTIVE = {0}\n", IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE);
                    }
                }

                text += "group by \n";
                text += "SS.SERVICE_ID,";
                text += "SV.SERVICE_CODE,\n";
                text += "SV.SERVICE_NAME,\n";

                text += "PR.SERVICE_CODE,\n";
                text += "PR.SERVICE_NAME,\n";
                text += "trea.TREATMENT_CODE,\n";
                text += "trea.TDL_PATIENT_CODE,\n";
                text += "trea.TDL_PATIENT_NAME,\n";
                text += "trea.TDL_PATIENT_DOB,\n";
                text += "trea.TDL_PATIENT_GENDER_ID,\n";
                text += "trea.id,\n";
                text += "trea.in_time, \n";
                text += "trea.out_time, \n";
                text += "trea.treatment_day_count, \n";
                text += "trea.icd_code, \n";
                text += "trea.icd_name, \n";
                text += "trea.icd_sub_code, \n";
                text += "sr.intruction_time,\n";
                text += "sr.request_room_id, \n";
                text += "sr.execute_room_id, \n";
                text += "sr.execute_department_id, \n";
                text += "sr.request_department_id, \n";
                text += "sr.execute_loginname, \n";
                text += "sr.execute_username, \n";
                text += "sr.request_loginname, \n";
                text += "sr.request_username, \n";
                text += "sr.SERVICE_REQ_CODE, \n";
                text += "sr.id, \n";
                text += "ss.vir_price, \n";
                text += "ss.hein_price, \n";
                text += "ss.price, \n";
                text += "ss.vat_ratio, \n";
                text += "ss.tdl_service_type_id, \n";
                text += "ss.TDL_HEIN_SERVICE_BHYT_CODE, \n";
                text += "ss.patient_type_id, \n";
                text += "ss.EXPEND_TYPE_ID, \n";
                text += "ss.exp_mest_medicine_id, \n";
                text += "ss.IS_SENT_EXT, \n";
                text += "ss.IS_EXPEND, \n";
                text += "ss.id, \n";
                text += "ss.other_pay_source_id, \n";
                text += "(case when ss.tdl_service_type_id=1 then sr.execute_room_id else sr.request_room_id end), \n";
                text += "1 \n";
                LogSystem.Info("SQL: " + text);
				list = new SqlDAO().GetSql<SERVICE_REQ>(text);
				LogSystem.Info("Result: " + list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				list = null;
			}
			return list;
		}

		internal List<BED_LOG> GetBedLog(Mrs00725Filter filter)
		{

			List<BED_LOG> list = new List<BED_LOG>();
			CommonParam val = new CommonParam();
			try
			{
				string text = "--danh sach nhat ky giuong\n";
				text += "select \n";
				text += "trea.id TREATMENT_ID,\n";
				text += "tbr.bed_room_id,\n";
				text += "bl.start_time,\n";
				text += "bl.bed_id\n";
				
				text += "from his_service_req sr\n";
				text += "join his_treatment trea on sr.treatment_id = trea.id\n";
				text += "join his_treatment_bed_room tbr on tbr.treatment_id = trea.id\n";
                text += "join his_bed_log bl on tbr.id = bl.treatment_bed_room_id \n";
                text += "left join his_execute_room exr on exr.room_id = sr.execute_room_id \n";
				text += "where 1=1\n";
                text += string.Format("and sr.intruction_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                text += string.Format("and sr.service_req_type_id = 3\n");
                if (filter.TREATMENT_TYPE_IDs != null)
                {
                    text += string.Format("and trea.tdl_treatment_type_id in ({0})\n", string.Join(",", filter.TREATMENT_TYPE_IDs));
                }
                if (filter.SERVICE_REQ_STT_ID.HasValue)
                {
                    text += string.Format("and sr.service_req_stt_id = {0}\n", filter.SERVICE_REQ_STT_ID);
                }
				if (filter.EXACT_EXECUTE_ROOM_IDs != null)
                {
                    text += string.Format("and exr.id in ({0})\n", string.Join(",", filter.EXACT_EXECUTE_ROOM_IDs));
				}
				if (filter.CURRENTBRANCH_DEPARTMENT_IDs != null)
				{
					text += string.Format("and sr.execute_DEPARTMENT_id in ({0})\n", string.Join(",", filter.CURRENTBRANCH_DEPARTMENT_IDs));
				}
                if (filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    text += string.Format("and sr.REQUEST_DEPARTMENT_ID in ({0})\n", string.Join(",", filter.REQUEST_DEPARTMENT_IDs));
                }
				text += "and sr.is_sent_ext = 1\n";
				text += "order by bl.start_time \n";
				LogSystem.Info("SQL: " + text);
				list = new SqlDAO().GetSql<BED_LOG>(text);
				LogSystem.Info("Result: " + list);
			}
			catch (Exception ex)
			{
				LogSystem.Error(ex);
				list = null;
			}
			return list;
		}
		public class BED_LOG
		{
			public int TREATMENT_ID { get; set; }

			public int BED_ROOM_ID { get; set; }

			public long BED_ID { get; set; }

			public long START_TIME { get; set; }
		}

        
	}
}
