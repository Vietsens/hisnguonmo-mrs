using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00601
{
    class Mrs00601RDO : HIS_TREATMENT
    {
        public string TRANSACTION_CODE { get; set; }
        public string TRANSACTION_TIME_STR { get; set; }
        public long TRANSACTION_TIME { get; set; }
        public decimal KH_PRICE { get; set; }
        public decimal GI_PRICE { get; set; }
        public decimal XN_PRICE { get; set; }
        public decimal CDHA_PRICE { get; set; }
        public decimal NS_PRICE { get; set; }
        public decimal SA_PRICE { get; set; }
        public decimal TDCN_PRICE { get; set; }
        public decimal PT_PRICE { get; set; }
        public decimal TT_PRICE { get; set; }
        public decimal TH_PRICE { get; set; }
        public decimal THTL_PRICE { get; set; }
        public decimal VT_PRICE { get; set; }
        public decimal KHAC_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_GROUP { get; set; }
        public Dictionary<string, decimal> DIC_GROUP_AMOUNT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public short HAS_SBA { get; set; }//1:Co sao benh an - 0: khong sao benh an
        public string TDL_FIRST_EXAM_ROOM_NAME { get; set; }//phong kham dau tien
        public decimal BILL_AMOUNT { get; set; }
        public decimal DEPOSIT_AMOUNT { get; set; }
        public decimal REPAY_AMOUNT { get; set; }

        public string BILL_ACCOUNT_BOOK_CODE { get; set; }

        public string DEPO_ACCOUNT_BOOK_CODE { get; set; }

        public string REPAY_ACCOUNT_BOOK_CODE { get; set; }

        public string BILL_NUM_ORDER { get; set; }
        public string DEPO_NUM_ORDER { get; set; }
        public string REPAY_NUM_ORDER { get; set; }

        public string EINVOICE_NUM_ORDER { get; set; }

        public Mrs00601RDO()
        {
        }

        public Mrs00601RDO(HIS_TREATMENT data, List<HIS_SERE_SERV> listHisSereServ, List<HIS_TRANSACTION> listHisTransaction, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, Mrs00601Filter filter, List<HIS_ACCOUNT_BOOK> ListHisAccountBook, List<HIS_SERVICE_REQ> ListExamHisServiceReq, List<V_HIS_TREATMENT_BED_ROOM> ListHisTreatmentBedRoom)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_TREATMENT>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }
                this.LAST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == data.LAST_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                this.TDL_FIRST_EXAM_ROOM_ID = this.IN_ROOM_ID ?? this.END_ROOM_ID ?? this.TDL_FIRST_EXAM_ROOM_ID ?? 0;
                this.TDL_FIRST_EXAM_DEPARTMENT_ID = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_ID;
                this.TDL_FIRST_EXAM_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == data.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                SetExtendField(this, listHisSereServ, listHisTransaction, listHisServiceRetyCat, filter, ListHisAccountBook, ListExamHisServiceReq, ListHisTreatmentBedRoom);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        public Mrs00601RDO(HIS_TREATMENT data, List<HIS_SERE_SERV> listHisSereServ, List<HIS_TRANSACTION> listHisTransaction, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, Mrs00601Filter filter, List<HIS_ACCOUNT_BOOK> ListHisAccountBook, List<HIS_SERVICE_REQ> ListExamHisServiceReq, List<V_HIS_TREATMENT_BED_ROOM> ListHisTreatmentBedRoom,long requestDepartmentId,long requestRoomId)
        {
            try
            {
                System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<HIS_TREATMENT>();
                foreach (var item in pi)
                {
                    item.SetValue(this, (item.GetValue(data)));
                }
                this.LAST_DEPARTMENT_ID = requestDepartmentId;
                this.LAST_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == requestDepartmentId) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
                this.TDL_FIRST_EXAM_ROOM_ID = requestRoomId;
                this.TDL_FIRST_EXAM_DEPARTMENT_ID = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).DEPARTMENT_ID;
                this.TDL_FIRST_EXAM_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.TDL_FIRST_EXAM_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                SetExtendField(this, listHisSereServ, listHisTransaction, listHisServiceRetyCat, filter, ListHisAccountBook, ListExamHisServiceReq, ListHisTreatmentBedRoom);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void SetExtendField(Mrs00601RDO r, List<HIS_SERE_SERV> listHisSereServ, List<HIS_TRANSACTION> listHisTransaction, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat, Mrs00601Filter filter, List<HIS_ACCOUNT_BOOK> ListHisAccountBook, List<HIS_SERVICE_REQ> ListExamHisServiceReq, List<V_HIS_TREATMENT_BED_ROOM> ListHisTreatmentBedRoom)
        {
            try
            {
                this.DIC_GROUP = new Dictionary<string, decimal>();
                this.DIC_GROUP_AMOUNT = new Dictionary<string, decimal>();
                var sereServSub = listHisSereServ.Where(o => o.TDL_TREATMENT_ID == r.ID).ToList();
                this.DEPOSIT_AMOUNT = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                this.REPAY_AMOUNT = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).Sum(s => s.AMOUNT);
                this.BILL_AMOUNT = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).Sum(s => s.AMOUNT);
                var billSub = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT).ToList();
                
                if (billSub != null)
                {
                    var billLast = billSub.OrderByDescending(p => p.TRANSACTION_TIME).ThenByDescending(q => q.ID).FirstOrDefault();
                    if (billLast != null)
                    {
                        this.TRANSACTION_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(billLast.TRANSACTION_TIME);
                        this.TRANSACTION_TIME = billLast.TRANSACTION_TIME;
                        var examServiceReqSub = ListExamHisServiceReq.Where(o => o.TREATMENT_ID == r.ID).ToList();
                       
                        var examServiceReq = examServiceReqSub.OrderBy(o => o.INTRUCTION_TIME).LastOrDefault(o => o.INTRUCTION_TIME <= this.TRANSACTION_TIME);
                        var treatmentBedRoomSub = ListHisTreatmentBedRoom.Where(o => o.TREATMENT_ID == r.ID).ToList();
                        var treatmentBedRoom = treatmentBedRoomSub.OrderBy(o => o.ADD_TIME).LastOrDefault(o => o.ADD_TIME <= this.TRANSACTION_TIME);
                        if (treatmentBedRoom != null)
                        {
                            this.BILL_ROOM_ID = treatmentBedRoom.BED_ROOM_ID;

                            this.BILL_ROOM_CODE = treatmentBedRoom.BED_ROOM_CODE;

                            this.BILL_ROOM_NAME = treatmentBedRoom.BED_ROOM_NAME;
                        }
                        else if (examServiceReq != null)
                        {
                            this.BILL_ROOM_ID = examServiceReq.EXECUTE_ROOM_ID;

                            this.BILL_ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examServiceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;

                            this.BILL_ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == examServiceReq.EXECUTE_ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
                        }
                    }
                    this.BILL_NUM_ORDER = string.Join(",", billSub.Select(o => o.NUM_ORDER).Distinct().ToList());
                    this.EINVOICE_NUM_ORDER = string.Join(",", billSub.Select(o => o.EINVOICE_NUM_ORDER).Distinct().ToList());
                    this.TRANSACTION_CODE = string.Join(",", billSub.Select(o => o.TRANSACTION_CODE).Distinct().ToList());
                    var accountBook = ListHisAccountBook.Where(o => billSub.Exists(p => p.ACCOUNT_BOOK_ID == o.ID)).ToList();
                    this.BILL_ACCOUNT_BOOK_CODE = string.Join(",", accountBook.Select(o => o.ACCOUNT_BOOK_CODE).Distinct().ToList());
                }
                var depositSub = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).ToList();
                if (depositSub != null)
                {
                    var Last = depositSub.OrderByDescending(p => p.TRANSACTION_TIME).ThenByDescending(q => q.ID).FirstOrDefault();

                    this.DEPO_NUM_ORDER = string.Join(",", depositSub.Select(o => o.NUM_ORDER).Distinct().ToList());
                    var accountBook = ListHisAccountBook.Where(o => depositSub.Exists(p => p.ACCOUNT_BOOK_ID == o.ID)).ToList();
                    this.DEPO_ACCOUNT_BOOK_CODE = string.Join(",", accountBook.Select(o => o.ACCOUNT_BOOK_CODE).Distinct().ToList());
                }

                var repaySub = listHisTransaction.Where(o => o.TREATMENT_ID == r.ID && o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU).ToList();
                if (repaySub != null)
                {
                    var Last = repaySub.OrderByDescending(p => p.TRANSACTION_TIME).ThenByDescending(q => q.ID).FirstOrDefault();

                    this.REPAY_NUM_ORDER = string.Join(",", repaySub.Select(o => o.NUM_ORDER).Distinct().ToList());
                    var accountBook = ListHisAccountBook.Where(o => repaySub.Exists(p => p.ACCOUNT_BOOK_ID == o.ID)).ToList();
                    this.REPAY_ACCOUNT_BOOK_CODE = string.Join(",", accountBook.Select(o => o.ACCOUNT_BOOK_CODE).Distinct().ToList());
                }
                
               

                if (sereServSub != null)
                {

                    this.KH_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.GI_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.XN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.CDHA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.NS_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.SA_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.TDCN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.PT_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.TT_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.TH_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.THTL_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.VT_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.PHCN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PHCN).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.GPBL_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__GPBL).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.MAU_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.AN_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__AN).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.KHAC_PRICE = sereServSub.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KHAC).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0));
                    this.TOTAL_PRICE = sereServSub.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));
                    this.TH_TL_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TH_TL).Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.VT_TL_PRICE = sereServSub.Where(o => o.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TL).Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.TOTAL_PATIENT_PRICE_BHYT_5PERCENT = sereServSub.Where(o => o.HEIN_RATIO == (decimal)0.95).Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.TOTAL_PATIENT_PRICE_BHYT_20PERCENT = sereServSub.Where(o => o.HEIN_RATIO == (decimal)0.8).Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.EXEMPTION = billSub.Sum(s => s.EXEMPTION ?? 0);
                    this.VIR_TOTAL_PATIENT_PRICE_BHYT = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0);
                    this.TOTAL_PATIENT_PRICE = sereServSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0) - sereServSub.Sum(s => (s.ADD_PRICE ?? 0) * (1 + s.VAT_RATIO) * s.AMOUNT);

                    this.DISCOUNT_BH = sereServSub.Where(o => o.DISCOUNT > 0 && o.PATIENT_TYPE_ID == 1).Sum(s => (s.DISCOUNT ?? 0));
                    this.DISCOUNT_VP = sereServSub.Where(o => o.DISCOUNT > 0 && o.PATIENT_TYPE_ID != 1).Sum(s => (s.DISCOUNT ?? 0));
                    if (listHisServiceRetyCat.Count > 0)
                    {
                        this.DIC_GROUP = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0) - (s.VIR_TOTAL_PATIENT_PRICE_BHYT ?? 0)));
                        this.DIC_GROUP_AMOUNT = sereServSub.GroupBy(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
                        if (filter.CATEGORY_CODE__SBA != null && sereServSub.Exists(o => CategoryCode(o.SERVICE_ID, listHisServiceRetyCat) == filter.CATEGORY_CODE__SBA))
                        {
                            this.HAS_SBA = (short)1;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string CategoryCode(long serviceId, List<V_HIS_SERVICE_RETY_CAT> listHisServiceRetyCat)
        {
            try
            {
                return (listHisServiceRetyCat.FirstOrDefault(o => o.SERVICE_ID == serviceId) ?? new V_HIS_SERVICE_RETY_CAT()).CATEGORY_CODE ?? "";
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        public string LAST_DEPARTMENT_NAME { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT_5PERCENT { get; set; }
        public decimal TOTAL_PATIENT_PRICE_BHYT_20PERCENT { get; set; }
        public decimal EXEMPTION { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }
        public decimal TH_TL_PRICE { get; set; }
        public decimal VT_TL_PRICE { get; set; }
        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }
        public decimal VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public decimal DISCOUNT_VP { get; set; }

        public decimal DISCOUNT_BH { get; set; }

        public decimal PHCN_PRICE { get; set; }

        public decimal GPBL_PRICE { get; set; }

        public decimal MAU_PRICE { get; set; }

        public decimal AN_PRICE { get; set; }

        public long TDL_FIRST_EXAM_DEPARTMENT_ID { get; set; }

        public long? BILL_ROOM_ID { get; set; }

        public string BILL_ROOM_CODE { get; set; }

        public string BILL_ROOM_NAME { get; set; }
    }
}
