namespace MRS.Processor.Mrs00150
{
    //Báo cáo sử dụng thuốc trong nước
    public class VSarReportMrs00150RDO
    {
        public string MEDICINE_TYPE_CODE { get;  set;  }

        public string MEDICINE_TYPE_NAME { get;  set;  }

        public string ACTIVE_INGR_BHYT_CODE { get;  set;  }

        public string ACTIVE_INGR_BHYT_NAME { get;  set;  }

        public string MANUFACTURER_NAME { get;  set;  }

        public string NATIONAL_NAME { get;  set;  }

        public decimal? PRICE { get;  set;  }

        public decimal? AMOUNT_USED { get;  set;  }

        public decimal? TOTAL_PRICE { get;  set;  }
    }
}
