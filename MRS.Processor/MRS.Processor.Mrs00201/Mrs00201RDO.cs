using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00201
{
    class Mrs00201RDO
    {
        public string PATIENT_CODE { get; set; }//mã bệnh nhân
        public string TREATMENT_CODE { get; set; }//mã điều trị

        public string PATIENT_NAME { get;  set;  }//tên bệnh nhân

        public string DATE_OF_BIRTH_MALE { get;  set;  }//ngày sinh nam

        public string DATE_OF_BIRTH_FEMALE { get;  set;  }//ngày sinh nữ

        public string X_QUANG { get;  set;  }//chụp Xquang

        public decimal? GIA_X_QUANG { get;  set; }//giá chụp Xquang

        public string CDHA_KHAC { get; set; }//cdha khác

        public decimal? GIA_CDHA_KHAC { get; set; }//giá cdha khác

        public string TDCN { get; set; }//thăm dò chức năng

        public decimal? GIA_TDCN { get; set; }//giá thăm dò chức năng

        public string SIEU_AM { get;  set;  }//siêu âm

        public decimal? GIA_SIEU_AM { get;  set;  }//giá siêu âm

        public string DV_XET_NGHEM { get;  set;  }//tên dịch vụ xét nghiệm

        public decimal? GIA_DV_XET_NGHEM { get;  set;  }//giá dịch vụ xét nghiệm

        public string TEN_CHI_SO_XET_NGHIEM { get;  set;  }//tên chỉ số xét nghiệm

        public string CHI_SO_XET_NGHIEM { get;  set;  }//chỉ số xét nghiệm

        public string EXAMINATION { get;  set;  }//khám

        public decimal? PRICE_EXAMINATION { get; set; }//giá khám

        public string GIAIPHAU { get; set; }// giải phẫu bệnh lý

        public decimal? PRICE_GIAIPHAU { get; set; }//giá giải phẫu bệnh lý
        public long ROW_POS { get; set; }

        public string NOI_SOI { get; set; }

        public decimal? GIA_NOI_SOI { get; set; }

        public decimal? GIA_THU_THUAT { get; set; }

        public string THU_THUAT { get; set; }
    }
    public class TreatmentServiceInfo
    {
        public TreatmentServiceInfo() { }
        public TreatmentServiceInfo(List<TreatmentServiceInfo> list)
        {
            var item = list.First();
            this.TREATMENT_CODE = item.TREATMENT_CODE;
            this.TDL_PATIENT_NAME = item.TDL_PATIENT_NAME;
            this.TDL_PATIENT_DOB = item.TDL_PATIENT_DOB;
            this.AMOUNT = list.Sum(s=>s.AMOUNT);
            this.TDL_SERVICE_CODE = item.TDL_SERVICE_CODE;
            this.TDL_SERVICE_NAME = item.TDL_SERVICE_NAME;
            this.VIR_PRICE = item.VIR_PRICE;
            this.VIR_TOTAL_PRICE = list.Sum(s=>s.VIR_TOTAL_PRICE);
            this.DIC_SV_AMOUNT = list.GroupBy(o => string.Format("{0}_{1}", o.TDL_SERVICE_CODE, o.VIR_PRICE)).ToDictionary(p => p.Key, q => q.Sum(s => s.AMOUNT));
        }
        public string TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long TDL_PATIENT_DOB { get; set; }
        public decimal AMOUNT { get; set; }
        public string TDL_SERVICE_CODE { get; set; }
        public string TDL_SERVICE_NAME { get; set; }
        public decimal VIR_PRICE { get; set; }
        public decimal VIR_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_SV_AMOUNT { get; set; }
    }

    public class SereServRdo : HIS_SERE_SERV
    {
        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
    }
}
