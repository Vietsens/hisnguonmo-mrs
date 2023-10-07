using MOS.MANAGER.HisService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Logging;
using MOS.Filter;
using MOS.MANAGER.HisIcd;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;

namespace MRS.Processor.Mrs00710
{
    public class Mrs00710RDOCountTreatment
    {
        public decimal COUNT_TREATMENT_NT_CV { get; set; } // so benh nhan noi tru chuyen vien
        public decimal TREATMENT_DETHUONG { get; set; } // so ho so dieu tri de thuong
        public decimal TREATMENT_DEKHO { get; set; } // so ho so dieu tri de kho
        public decimal TREATMENT_MODE { get; set; } // so ho so dieu tri mo de
        public decimal TREATMENT_DAY_COUNT { get; set; } // so ngay dieu tri theo bang ke
        public decimal COUNT_TREATMENT { get; set; } // so benh nhan den kham
        public decimal COUNT_TREATMENT_FEMALE { get; set; } //so benh nhan nu den kham
        public decimal COUNT_TREATMENT_BH { get; set; } //so benh nhan bhyt den kham
        public decimal COUNT_TREATMENT_YHCT { get; set; } // so benh nhan kham yhct
        public decimal COUNT_TREATMENT_LESS15 { get; set; } //so benh nhan kham duoi 15 tuoi
        public decimal COUNT_TREATMENT_NT { get; set; } //so benh nhan den kham dieu tri noi tru
        public decimal COUNT_TREATMENT_NT_FEMALE { get; set; } // so benh nhan nu den kham va dieu tri noi tru
        public decimal COUNT_TREATMENT_NT_BH { get; set; } //so benh nhan bhyt den kham va dieu tri noi tru
        public decimal COUNT_TREATMENT_NT_YHCT { get; set; } // so benh nhan kham yhct va dieu tri noi tru
        public decimal COUNT_TREATMENT_NT_LESS15 { get; set; } //so benh nhan kham duoi 15 tuoi va dieu tri noi tru
        public decimal COUNT_TREATMENT_NGT { get; set; } //so benh nhan den kham dieu tri ngoai tru
        public decimal COUNT_TREATMENT_NGT_FEMALE { get; set; } // so benh nhan nu den kham va dieu tri ngoai tru
        public decimal COUNT_TREATMENT_NGT_BH { get; set; } //so benh nhan bhyt den kham va dieu tri ngoai tru
        public decimal COUNT_TREATMENT_NGT_YHCT { get; set; } // so benh nhan kham yhct va dieu tri ngoai tru
        public decimal COUNT_TREATMENT_NGT_LESS15 { get; set; } //so benh nhan kham duoi 15 tuoi va dieu tri ngoai tru
    }
    public class Mrs00710RDOCountService
    {
        public decimal COUNT_SV_DIENTIM { get; set; } //tong so dich vu dien tim chi dinh
        public decimal COUNT_SV_DIENNAO { get; set; } //tong so dich vu dien nao chi dinh
        public decimal COUNT_SV_CTSCAN { get; set; } //tong so dich vu ct scan chi dinh
        public long TDL_SERVICE_TYPE_ID { get; set; } //tach theo loai dich vu
        public decimal COUNT_SV { get; set; } //tong so dich vu cac loai
        public decimal COUNT_SV_NT { get; set; } //tong so dich vu chi dinh dien noi tru
        public decimal COUNT_SV_NGT { get; set; } //tong so dich vu chi dinh dien kham va ngoai tru
        public decimal COUNT_SV_BH { get; set; } //tong so dich vu chi dinh cua benh nhan bhyt

    }
}
