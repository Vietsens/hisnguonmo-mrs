using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00354
{
    public class Mrs00354RDO
    {
        public long DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }
        public long ROOM_ID { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }

        public decimal COUNT_TREATMENT { get; set; }
        public decimal TOTAL_PRICE { get; set; }
        public decimal MEDICINE_PRICE { get; set; }
        public decimal TOTAL_PATIENT_PRICE_HEIN { get; set; }
        public decimal TOTAL_PATIENT_PRICE { get; set; }

    }
    public class TongKhoaPhong
    {
        public long ID { get; set; }
        public string Ma { get; set; }
        public string Khoa { get; set; }
        //public bool IsRoom { get; set; }

        public decimal SoLuot_NT_BH { get; set; }
        public decimal SoLuot_NT_TP { get; set; }
        public decimal SoLuot_NGT_BH { get; set; }
        public decimal SoLuot_NGT_TP { get; set; }
        public decimal SoLuot_KH_BH { get; set; }
        public decimal SoLuot_KH_TP { get; set; }

        public decimal Sotien_NT_BHTT { get; set; }
        public decimal Sotien_NT_BNTT { get; set; }
        public decimal Sotien_NT_TP { get; set; }
        public decimal Sotien_NGT_BHTT { get; set; }
        public decimal Sotien_NGT_BNTT { get; set; }
        public decimal Sotien_NGT_TP { get; set; }
        public decimal Sotien_KH_BHTT { get; set; }
        public decimal Sotien_KH_BNTT { get; set; }
        public decimal Sotien_KH_TP { get; set; }
        public decimal Sotien_BHTT { get; set; }
        public decimal Sotien_BNTT { get; set; }
        public decimal Sotien_TP { get; set; }
        public decimal Sotien { get; set; }
        public decimal Sotien_giuong { get; set; }
        public decimal Sotien_XN { get; set; }
        public decimal Sotien_Thuoc { get; set; }
        public decimal Sotien_Vattu { get; set; }
        public decimal Sotien_pttt { get; set; }
        public decimal Sotien_kham { get; set; }
        public decimal Sotien_NS { get; set; }
        public decimal Sotien_sieuam { get; set; }
        public decimal Sotien_xq { get; set; }
        public decimal Sotien_DIENTIM { get; set; }
        public decimal Sotien_khac { get; set; }
        public decimal Sotien_HpThuoc { get; set; }
        public decimal Sotien_HpVattu { get; set; }


        public decimal SoLuot_BNG_TP { get; set; }

        public decimal SoLuot_BNG_BH { get; set; }

        public decimal Sotien_BNG_TP { get; set; }

        public decimal Sotien_BNG_BNTT { get; set; }

        public decimal Sotien_BNG_BHTT { get; set; }

        public decimal Sotien_DTT { get; set; }

        public Dictionary<string, decimal> DIC_SV_TOTAL_PRICE { get; set; }
    }
    public class DataGet
    {

        public long? TDL_TREATMENT_ID { get; set; }

        public long? BRANCH_ID { get; set; }

        public short? IS_NO_EXECUTE { get; set; }

        public short? IS_NO_PAY { get; set; }

        public long? TDL_TREATMENT_TYPE_ID { get; set; }

        public string TDL_SERVICE_CODE { get; set; }

        public decimal? VIR_TOTAL_PRICE { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE { get; set; }

        public long? TDL_SERVICE_TYPE_ID { get; set; }

        public long? TDL_REQUEST_ROOM_ID { get; set; }

        public long? TDL_EXECUTE_ROOM_ID { get; set; }

        public string REQUEST_ROOM_CODE { get; set; }

        public string EXECUTE_ROOM_CODE { get; set; }

        public string REQUEST_ROOM_NAME { get; set; }

        public string EXECUTE_ROOM_NAME { get; set; }

        public long? TDL_EXECUTE_DEPARTMENT_ID { get; set; }

        public long? TDL_REQUEST_DEPARTMENT_ID { get; set; }

        public string REQUEST_DEPARTMENT_CODE { get; set; }

        public string EXECUTE_DEPARTMENT_CODE { get; set; }

        public string REQUEST_DEPARTMENT_NAME { get; set; }

        public string EXECUTE_DEPARTMENT_NAME { get; set; }

        public long? MEDICINE_ID { get; set; }

        public long PATIENT_TYPE_ID { get; set; }

        public long? TDL_PATIENT_TYPE_ID { get; set; }

        public long? END_DEPARTMENT_ID { get; set; }

        public string END_DEPARTMENT_CODE { get; set; }

        public string END_DEPARTMENT_NAME { get; set; }

        public decimal? VIR_TOTAL_HEIN_PRICE { get; set; }

        public decimal? VIR_TOTAL_PATIENT_PRICE_BHYT { get; set; }

        public long? END_ROOM_ID { get; set; }

        public string END_ROOM_CODE { get; set; }

        public string END_ROOM_NAME { get; set; }

        public decimal? BILL_PRICE { get; set; }

        public long ID { get; set; }
    }
}
