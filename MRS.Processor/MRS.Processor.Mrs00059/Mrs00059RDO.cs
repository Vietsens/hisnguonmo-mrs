using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00059
{
    class Mrs00059RDO
    {
        public string PATIENT_TYPE_CODE { get;  set;  }
        public string PATIENT_TYPE_NAME { get;  set;  }

        public decimal VIR_TOTAL_PATIENT_PRICE { get;  set;  }
        public decimal VIR_TOTAL_HEIN_PRICE { get;  set;  }
        public decimal SERVICE_AMOUNT { get;  set;  }
    }

    class BillRDO
    {
        public string TDL_PATIENT_CODE { get; set; }	//mã bệnh nhân
        public string TREATMENT_CODE { get; set; }	//mã điều trị
        public string TDL_PATIENT_NAME { get; set; }	//tên bệnh nhân
        public long TDL_PATIENT_DOB { get; set; }	//ngày sinh
        public string TDL_PATIENT_GENDER_NAME { get; set; }	//giới tính
        public string TDL_HEIN_CARD_NUMBER { get; set; }	//số thẻ bhyt
        public string TDL_HEIN_MEDI_ORG_CODE { get; set; }	//nơi đăng ký kcbbđ
        public long IN_TIME { get; set; } //thời gian vào viện
        public long? OUT_TIME { get; set; } //thời gian ra viện
        public string ICD_CODE { get; set; } // mã bệnh
        public long? FEE_LOCK_ORDER { get; set; }// số thứ tự duyệt khóa vp
        public string DEPARTMENT_NAME { get;  set;  }	//khoa kết thúc
        public string ACCOUNT_BOOK_CODE { get;  set;  }	//mã sổ thu
        public long NUM_ORDER { get;  set;  }	//số biên lai
        public decimal TOTAL_PRICE { get;  set;  }	//tổng chi phí
        public decimal TOTAL_HEIN_PRICE { get;  set;  }	//bảo hiểm chi trả
        public decimal TOTAL_PATIENT_PRICE { get;  set;  }	//bệnh nhân chi trả
        public decimal TOTAL_PRICE_EXPEND { get; set; }	//tổng tiền hao phí
        public decimal TOTAL_DISCOUNT { get; set; }	//tổng chiết khấu, miễn giảm
        public decimal TOTAL_OTHER_SOURCE_PRICE { get; set; }	//tổng nguồn khác
        public decimal DIFF_PRIMARY_PRICE { get; set; }	//tổng phụ thu
        public decimal TOTAL_DEPOSIT_AMOUNT { get; set; }	//tổng tạm ứng
        public decimal TOTAL_REPAY_AMOUNT { get; set; }	//tổng hoàn ứng
        public decimal TOTAL_DEPOSIT_ONTIME { get; set; }	//tổng tạm ứng trong kỳ
        public decimal TOTAL_REPAY_ONTIME { get; set; }	//tổng hoàn ứng trong kỳ
        public decimal TOTAL_BILL_ONTIME { get; set; }	//tổng thanh toán trong kỳ
        public decimal TOTAL_KC_ONTIME { get; set; }	//tổng kết chuyển trong kỳ
        public string TRANSACTION_INFOS { get; set; } //thông tin giao dịch
        public decimal BILL_AMOUNT { get; set; } //tổng tiền hóa đơn
        public string FEE_LOCK_LOGINNAME { get; set; } //tài khoản khóa viện phí
        public string FEE_LOCK_USERNAME { get; set; } //username khóa viện phí
        public string PAY_FORM_CODE { get; set; } //mã hình thức thanh toán
        public string PAY_FORM_NAME { get; set; } //tên hình thức thanh toán
        public long FEE_LOCK_DATE { get; set; } //thời gian khóa viện phí

        //các thông tin bổ sung
        public long? TDL_TREATMENT_TYPE_ID { get; set; } //diện điều trị
        public string TDL_PATIENT_TYPE_CODE { get; set; } //mã đối tượng bệnh nhân

        public Dictionary<string, decimal> DIC_SVT_EXPEND_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_SVT_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_HSVT_TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_CATE_TOTAL_PRICE { get; set; }

        public BillRDO()
        {
            //
        }
        public BillRDO(V_HIS_TREATMENT treatment, V_HIS_TRANSACTION bill, List<HIS_SERE_SERV_BILL> ssbSub, List<HIS_SERE_SERV_BILL> ssbDirectSub, List<V_HIS_TRANSACTION> transactionSub, List<V_HIS_SERE_SERV> listSereServSub, List<HIS_SERE_SERV_DEPOSIT> ssdSub, List<HIS_SESE_DEPO_REPAY> ssrSub, Dictionary<long, string> dicCategory, Mrs00059Filter castFilter)
        {
            //thong tin ho so dieu tri
            AddInfoTreatment(treatment);
            //thong tin hoa don
            AddInfoBill(bill);
            //thong tin giao dich
            AddInfoTransaction(transactionSub,castFilter);
            //thong tin chi tiet hoa don
            AddInfoBillDetail(ssbSub, listSereServSub, ssdSub, ssrSub, ssbDirectSub, dicCategory);

        }

        private void AddInfoBillDetail(List<HIS_SERE_SERV_BILL> ssbSub, List<V_HIS_SERE_SERV> listSereServSub, List<HIS_SERE_SERV_DEPOSIT> ssdSub, List<HIS_SESE_DEPO_REPAY> ssrSub, List<HIS_SERE_SERV_BILL> ssbDirectSub, Dictionary<long, string> dicCategory)
        {
            if(listSereServSub != null)
            {
                this.TOTAL_PRICE = listSereServSub.Sum(o => o.VIR_TOTAL_PRICE ?? 0);
                this.DIC_CATE_TOTAL_PRICE = listSereServSub.GroupBy(o => dicCategory.ContainsKey(o.SERVICE_ID) ? dicCategory[o.SERVICE_ID] : "NONE").ToDictionary(p => p.Key, q => q.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                this.DIC_SVT_TOTAL_PRICE = listSereServSub.GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => q.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                var expends = listSereServSub.Where(o=>o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
                this.DIC_SVT_EXPEND_PRICE = listSereServSub.Where(o => o.IS_EXPEND != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).GroupBy(o => o.SERVICE_TYPE_CODE ?? "NONE").ToDictionary(p => p.Key, q => expends.Where(p=>q.Select(y=>y.ID).Contains(p.PARENT_ID??0)).Sum(o => o.VIR_TOTAL_PRICE_NO_EXPEND ?? 0));
                this.DIC_HSVT_TOTAL_PRICE = listSereServSub.GroupBy(o => o.HEIN_SERVICE_TYPE_CODE?? "NONE").ToDictionary(p => p.Key, q => q.Sum(o => o.VIR_TOTAL_PRICE ?? 0));
                this.TOTAL_HEIN_PRICE = listSereServSub.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0);
                this.TOTAL_PATIENT_PRICE = listSereServSub.Sum(o => o.VIR_TOTAL_PATIENT_PRICE ?? 0);
                this.TOTAL_PRICE_EXPEND = listSereServSub.Sum(o => o.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? (o.VIR_TOTAL_PRICE_NO_EXPEND ?? 0) : 0);
                this.TOTAL_DISCOUNT = listSereServSub.Sum(o => o.DISCOUNT ?? 0);
                this.TOTAL_HEIN_PRICE = listSereServSub.Sum(o => o.VIR_TOTAL_HEIN_PRICE ?? 0);
                this.TOTAL_OTHER_SOURCE_PRICE = listSereServSub.Sum(o => (o.OTHER_SOURCE_PRICE ?? 0) * o.AMOUNT);
                this.DIFF_PRIMARY_PRICE = listSereServSub.Where(o => o.PRIMARY_PATIENT_TYPE_ID > 0 && o.PRIMARY_PRICE > o.LIMIT_PRICE).Sum(o => ((o.PRIMARY_PRICE??0)-(o.LIMIT_PRICE??0)) * o.AMOUNT);
                //tinh tong tam ung dich vu va hoan ung dich vu
                var ssdSs = ssdSub.Where(o => listSereServSub.Exists(p => p.ID == o.SERE_SERV_ID)).ToList();
                this.TOTAL_DEPOSIT_SV_AMOUNT = ssdSs.Sum(s => s.AMOUNT);
                this.TOTAL_REPAY_SV_AMOUNT = ssrSub.Where(o => ssdSs.Exists(p => p.ID == o.SERE_SERV_DEPOSIT_ID)).Sum(s => s.AMOUNT);
                this.TOTAL_DIRECT_BILL_AMOUNT = ssbDirectSub.Sum(s => s.PRICE);
            }
        }

        private void AddInfoBill(V_HIS_TRANSACTION bill)
        {
            if(bill !=null)
            {
                this.ACCOUNT_BOOK_CODE = bill.ACCOUNT_BOOK_CODE;
                this.NUM_ORDER = bill.NUM_ORDER;
                this.EINVOICE_NUM_ORDER = bill.EINVOICE_NUM_ORDER;
                this.BILL_AMOUNT = bill.AMOUNT;
                this.BILL_KC_AMOUNT = (bill.KC_AMOUNT ?? 0);
                this.PAY_FORM_CODE = bill.PAY_FORM_CODE;
                this.PAY_FORM_NAME = bill.PAY_FORM_NAME;
                
            }
        }

        private void AddInfoTransaction(List<V_HIS_TRANSACTION> transactionSub, Mrs00059Filter castFilter)
        {
            if(transactionSub!=null)
            {
                //chỉ lấy giao dịch tạm ứng và hoàn ứng không dịch vụ
                this.TOTAL_DEPOSIT_AMOUNT = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU && (o.TDL_SERE_SERV_DEPOSIT_COUNT??0) == 0).Sum(s => s.AMOUNT);
                this.TOTAL_REPAY_AMOUNT = transactionSub.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Sum(s => s.AMOUNT);
               
                //tinh tong mien giam
                this.EXEMPTION = transactionSub.Sum(s => s.EXEMPTION ?? 0);
                this.TRANSACTION_INFOS = string.Join(";", transactionSub.Where(o => (o.TDL_SERE_SERV_DEPOSIT_COUNT ?? 0) == 0 && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Select(s => string.Format("{0}_{1}: {2}", s.ACCOUNT_BOOK_CODE, s.NUM_ORDER, s.AMOUNT)).ToList());
                this.TRANSACTION_EINFOS = string.Join(";", transactionSub.Where(o => (o.TDL_SERE_SERV_DEPOSIT_COUNT ?? 0) == 0 && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Select(s => string.Format("{0}_{1}: {2}", s.ACCOUNT_BOOK_CODE, s.EINVOICE_NUM_ORDER, s.AMOUNT)).ToList());
                var transactionOnTime = transactionSub.Where(o => o.TRANSACTION_TIME >= castFilter.TIME_FROM && o.TRANSACTION_TIME <= castFilter.TIME_TO && (o.TDL_SERE_SERV_DEPOSIT_COUNT ?? 0) == 0 && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).ToList();
                this.TOTAL_DEPOSIT_ONTIME = transactionOnTime.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TU).Sum(s => s.AMOUNT);
                this.TOTAL_REPAY_ONTIME = transactionOnTime.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__HU && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Sum(s => s.AMOUNT);
                this.TOTAL_BILL_ONTIME = transactionOnTime.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Sum(s => s.AMOUNT);
                this.TOTAL_KC_ONTIME = transactionOnTime.Where(o => o.TRANSACTION_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TRANSACTION_TYPE.ID__TT && (o.TDL_SESE_DEPO_REPAY_COUNT ?? 0) == 0).Sum(s => s.KC_AMOUNT??0);
            }    
        }

        private void AddInfoTreatment(V_HIS_TREATMENT treatment)
        {
            this.TREATMENT_CODE = treatment.TREATMENT_CODE;
            this.TDL_PATIENT_CODE = treatment.TDL_PATIENT_CODE;
            this.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;
            this.TDL_PATIENT_DOB = treatment.TDL_PATIENT_DOB;
            this.TDL_PATIENT_GENDER_NAME = treatment.TDL_PATIENT_GENDER_NAME;
            this.TDL_HEIN_CARD_NUMBER = treatment.TDL_HEIN_CARD_NUMBER;
            this.TDL_HEIN_MEDI_ORG_CODE = treatment.TDL_HEIN_MEDI_ORG_CODE;
            this.IN_TIME = treatment.IN_TIME;
            this.OUT_TIME = treatment.OUT_TIME;
            this.ICD_CODE = treatment.ICD_CODE;
            this.FEE_LOCK_ORDER = treatment.FEE_LOCK_ORDER;
            var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o=>o.ID==treatment.LAST_DEPARTMENT_ID);
            if(department!= null)
            {
                this.DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                this.DEPARTMENT_NAME = department.DEPARTMENT_NAME;
            }
            this.FEE_LOCK_LOGINNAME = treatment.FEE_LOCK_LOGINNAME;
            this.FEE_LOCK_USERNAME = treatment.FEE_LOCK_USERNAME;
            this.FEE_LOCK_DATE = treatment.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE ? (treatment.FEE_LOCK_TIME ?? 0) - (treatment.FEE_LOCK_TIME ?? 0) % 1000000 : 
                (treatment.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE?treatment.OUT_DATE??0:treatment.IN_DATE);
            this.TDL_TREATMENT_TYPE_ID = treatment.TDL_TREATMENT_TYPE_ID;
            this.TDL_PATIENT_TYPE_CODE = (HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o=>o.ID== treatment.TDL_PATIENT_TYPE_ID) ??new HIS_PATIENT_TYPE()).PATIENT_TYPE_CODE;
        }

        public decimal BILL_KC_AMOUNT { get; set; }

        public decimal TOTAL_DEPOSIT_SV_AMOUNT { get; set; }

        public decimal TOTAL_REPAY_SV_AMOUNT { get; set; }

        public decimal TOTAL_DIRECT_BILL_AMOUNT { get; set; }

        public decimal EXEMPTION { get; set; }

        public string DEPARTMENT_CODE { get; set; }

        public string TRANSACTION_EINFOS { get; set; }

        public string EINVOICE_NUM_ORDER { get; set; }
    }

}
