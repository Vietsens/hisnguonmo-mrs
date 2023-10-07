using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Linq;
using Inventec.Common.Logging;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00205
{
    class Mrs00205RDO
    {
        public HIS_TREATMENT HIS_TREATMENT { get; set; }
        public string VIR_PATIENT_NAME { get; set; }//

        public string PATIENT_CODE { get; set; }

        public string DOB { get; set; }//

        public string GENDER { get; set; }

        public string IN_CODE { get; set; }

        public string PHONE { get; set; }

        public long? TOTAL_DATE { get; set; }

        public string TREATMENT_CODE { get; set; }//

        public string SERVICE_REQ_CODE { get; set; }

        public string ICD_NAME { get; set; }

        public string ICD_CODE { get; set; }//

        public long SERVICE_ID { get; set; }

        public decimal AMOUNT { get; set; }//

        public decimal PRICE { get; set; }//

        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }//

        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }//

        public decimal? VIR_TOTAL_PRICE { get; set; }

        public string VIR_ADDRESS { get; set; }//

        public string SERVICE_CODE { get; set; }

        public string SERVICE_NAME { get; set; }

        public string SERVICE_UNIT_NAME { get; set; }

        public string TRANSACTION_DATE_STR { get; set; }

        public string TRANSACTION_TIME_STR { get; set; }

        public string FEE_LOCK_DATE_STR { get; set; }

        public string FEE_LOCK_TIME_STR { get; set; }

        public string TRANSACTION_CODE { get; set; }

        public string CASHIER_LOGINNAME_TRANSACTION_NUM_ORDER { get; set; }

        public string TDL_HEIN_SERVICE_BHYT_CODE { get; set; }

        public string PATIENT_TYPE_NAME_HEIN_RATIO_STR { get; set; }

        public string TDL_REQUEST_DEPARTMENT_NAME { get; set; }

        public string TDL_REQUEST_ROOM_NAME { get; set; }//

        public string TDL_REQUEST_USERNAME { get; set; }//

        public string TDL_EXECUTE_USERNAME { get; set; }//

        public long TREATMENT_RESULT_ID { set; get; }

        public string TREATMENT_RESULT_NAME { set; get; }

        public string PATIENT_CMND_NUMBER { set; get; }

        public string  INTRUCTION_TIME { get; set; }
        public string START_TIME { set; get; }
        public string FINISH_TIME { set; get; }

        public string START_TIME_GIUONG { set; get; }   // THỜI GIAN BẮT ĐẦU VÀO GIƯỜNG
        public string FINISH_TIME_GIUONG { set; get; } //THỜI GIAN KẾT THÚC GIƯỜNG
        public string TREATMENT_END_TYPE_NAME { get; set; }

        public string ICD_TEXT { get; set; }

        public string ICD_SUB_CODE { get; set; }
      


        public Mrs00205RDO(HIS_TREATMENT treat,List<HIS_TREATMENT_END_TYPE> listTreatmentEndType,List<V_HIS_BED_LOG> listBedLog ,List<HIS_SERE_SERV> sereServs, List<HIS_TRANSACTION> listBill, List<HIS_PATIENT> listHisPatient,List<HIS_TREATMENT_RESULT> listHisTreamentResult,List<HIS_SERVICE_REQ> listHisServiceReq, Mrs00205Filter filter)
        {
            try
            {
                HIS_TREATMENT = treat;
                var lst_TreatmentResult = listHisTreamentResult.ToList();
                var billSub = listBill.Where(o => sereServs.Exists(p => p.TDL_TREATMENT_ID == o.TREATMENT_ID)).ToList();
                //var transactionSub = new List<HIS_TRANSACTION>();
                //if (sereServBillSub != null)
                //{
                //    transactionSub.AddRange(listHisTransaction.Where(o => sereServBillSub.Exists(p => p.BILL_ID == o.ID)).ToList());
                //}
                
                var patient = listHisPatient.FirstOrDefault(o => o.ID==treat.PATIENT_ID);
                if (patient != null)
                {
                    PHONE = patient.PHONE;
                }

                VIR_PATIENT_NAME = treat.TDL_PATIENT_NAME;//
                PATIENT_CODE = treat.TDL_PATIENT_CODE;
                DOB = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treat.TDL_PATIENT_DOB);//
                TREATMENT_CODE = treat.TREATMENT_CODE;//
                PATIENT_CMND_NUMBER = treat.TDL_PATIENT_CCCD_NUMBER;

                ICD_TEXT = treat.ICD_TEXT;

                ICD_SUB_CODE = string.Format("{0}-{1}", treat.ICD_SUB_CODE, treat.ICD_TEXT);

                ICD_NAME = treat.ICD_NAME;


                ICD_CODE = string.Format("{0}-{1}", treat.ICD_CODE, treat.ICD_NAME);//
                INTRUCTION_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServs.First().TDL_INTRUCTION_TIME);
                VIR_ADDRESS = treat.TDL_PATIENT_ADDRESS;//

                GENDER = treat.TDL_PATIENT_GENDER_NAME;

                IN_CODE = treat.IN_CODE;
                TREATMENT_RESULT_ID = (long)treat.TREATMENT_RESULT_ID;
                TREATMENT_RESULT_NAME = listHisTreamentResult.Where(x => x.ID.Equals(treat.TREATMENT_RESULT_ID)).First().TREATMENT_RESULT_NAME;
                TOTAL_DATE = Calculation.DayOfTreatment(treat.IN_TIME, treat.OUT_TIME, treat.TREATMENT_END_TYPE_ID, treat.TREATMENT_RESULT_ID, (treat.TDL_PATIENT_TYPE_ID == HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT) ? PatientTypeEnum.TYPE.BHYT : PatientTypeEnum.TYPE.THU_PHI);
                FEE_LOCK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(treat.FEE_LOCK_TIME ?? 0);
                FEE_LOCK_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treat.FEE_LOCK_TIME ?? 0);
                SERVICE_ID = sereServs.First().SERVICE_ID;

                AMOUNT = sereServs.Sum(s => s.AMOUNT);//

                PRICE = sereServs.First().PRICE;//

                VIR_TOTAL_PATIENT_PRICE = sereServs.Sum(s => s.VIR_TOTAL_PATIENT_PRICE);//

                VIR_TOTAL_HEIN_PRICE = sereServs.Sum(s => s.VIR_TOTAL_HEIN_PRICE);//

                VIR_TOTAL_PRICE = sereServs.Sum(s => s.VIR_TOTAL_PRICE);

                
                SERVICE_CODE = sereServs.First().TDL_SERVICE_CODE;
                SERVICE_NAME = sereServs.First().TDL_SERVICE_NAME;
                SERVICE_TYPE_CODE = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o => o.ID == sereServs.First().TDL_SERVICE_TYPE_ID) ?? new HIS_SERVICE_TYPE()).SERVICE_TYPE_CODE;
                SERVICE_TYPE_NAME = (HisServiceTypeCFG.HisServiceTypes.FirstOrDefault(o=>o.ID==sereServs.First().TDL_SERVICE_TYPE_ID)??new HIS_SERVICE_TYPE()).SERVICE_TYPE_NAME;
                SERVICE_UNIT_NAME = (HisServiceUnitCFG.HisServiceUnits.FirstOrDefault(o => o.ID == sereServs.First().TDL_SERVICE_UNIT_ID) ?? new HIS_SERVICE_UNIT()).SERVICE_UNIT_NAME;
                SERVICE_REQ_CODE = sereServs.First().TDL_SERVICE_REQ_CODE;

                TDL_HEIN_SERVICE_BHYT_CODE = sereServs.First().TDL_HEIN_SERVICE_BHYT_CODE;
                if (billSub.Count>0&&filter.TRANSACTION_TIME_FROM > 0 && (filter.HAS_BILL == true || (filter.HAS_BILL == false && treat.TDL_TREATMENT_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)))
                {
                    TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(billSub.OrderBy(o => o.TRANSACTION_TIME).Last().TRANSACTION_TIME);

                    TRANSACTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(billSub.OrderBy(o=>o.TRANSACTION_TIME).Last().TRANSACTION_DATE);
                }
                else
                {
                    TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(sereServs.First().TDL_INTRUCTION_TIME);

                    TRANSACTION_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(sereServs.First().TDL_INTRUCTION_TIME);
                }
                
                TRANSACTION_CODE = string.Join(", ", billSub.Select(o => (o.TRANSACTION_CODE)).ToList());

                CASHIER_LOGINNAME_TRANSACTION_NUM_ORDER = string.Join(", ", billSub.Select(o => o.CASHIER_LOGINNAME + "\\" + o.NUM_ORDER).ToList());

                if (sereServs.First().PATIENT_TYPE_ID != HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                {
                    PATIENT_TYPE_NAME_HEIN_RATIO_STR = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServs.First().PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME;
                }
                else
                {
                    PATIENT_TYPE_NAME_HEIN_RATIO_STR = string.Format("{0} {1} ({2}%)", (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == sereServs.First().PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE()).PATIENT_TYPE_NAME, (sereServs.First().HEIN_CARD_NUMBER ?? "xxxx").Substring(2, 1), (sereServs.First().HEIN_RATIO ?? 0) * 100);
                }
                TDL_REQUEST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == sereServs.First().TDL_REQUEST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                TDL_REQUEST_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == sereServs.First().TDL_REQUEST_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;//
                TDL_REQUEST_USERNAME = sereServs.First().TDL_REQUEST_USERNAME;////
                var serviceReq = listHisServiceReq.Where(x => x.ID == sereServs.Where(y=>y.SERVICE_REQ_ID!=null).First().SERVICE_REQ_ID).FirstOrDefault();
                if (serviceReq!=null)
                {
                    TDL_EXECUTE_USERNAME = serviceReq.EXECUTE_USERNAME;////
                    START_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.START_TIME??0);
                    FINISH_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(serviceReq.FINISH_TIME??0);
                }

                var treatmentEndType = listTreatmentEndType.FirstOrDefault(o => o.ID == treat.TREATMENT_END_TYPE_ID);
                if (treatmentEndType != null)
                {
                    TREATMENT_END_TYPE_NAME = treatmentEndType.TREATMENT_END_TYPE_NAME;
                }
                var bedLog = listBedLog.FirstOrDefault(o => o.TREATMENT_ID == treat.ID && sereServs.First().SERVICE_ID == o.BED_SERVICE_TYPE_ID && (o.SERVICE_REQ_ID == null || o.SERVICE_REQ_ID == sereServs.First().SERVICE_REQ_ID )) ;             
                if (bedLog!=null)
                {
                    START_TIME_GIUONG = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(bedLog.START_TIME );
                    FINISH_TIME_GIUONG = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(bedLog.FINISH_TIME ??0);
                    
                }
                
            }
            catch (System.Exception ex)
            {
                LogSystem.Error(ex);
            }

        }

        public string SERVICE_TYPE_CODE { get; set; }

        public string SERVICE_TYPE_NAME { get; set; }
    }
}
