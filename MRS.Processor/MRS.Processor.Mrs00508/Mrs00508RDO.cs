using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00508
{
    public class Mrs00508RDO : V_HIS_TREATMENT
    {
        public string DEPARTMENT_NAME { get; set; }
        public string PATIENT_NAME { get; set; }
        public Decimal EXAM_PRICE { get; set; }//kham	12
        public Decimal FUEX_PRICE { get; set; }//tham do chuc nang
        public Decimal ENDO_PRICE { get; set; }//noi soi
        public Decimal SUIM_PRICE { get; set; }//sieu am
        public Decimal OTHER_PRICE { get; set; }//khac
        public Decimal BED_PRICE { get; set; }//giuong
        public Decimal TRAN_PRICE { get; set; }//giuong
        public Decimal BLOOD_PRICE { get; set; }//mau
        public Decimal TEIN_PRICE { get; set; }//xet nghiem
        public Decimal EXPEND_TEIN_PRICE { get; set; }//Hao phi dinh muc xet nghiem
        public Decimal LEFT_TEIN_PRICE { get; set; }//xet nghiem
        public Decimal DIIM_PRICE { get; set; }	//chup xquang
        public Decimal EXPEND_DIIM_PRICE { get; set; }//Hao phi dinh muc CDHA
        public Decimal MISU_PRICE { get; set; }//thu thuat
        public Decimal SURG_PRICE { get; set; }	//phau thuat
        public Decimal MEDI_PRICE { get; set; }//thuoc
        public Decimal MATE_PRICE { get; set; }//vat tu
        public Decimal EXPEND_TICK { get; set; }//Hao phi thuoc vat tu
        public Decimal VIR_TOTAL_PRICE { get; set; }//Tong tien
        public Mrs00508RDO(V_HIS_TREATMENT data)
        {
            PropertyInfo[] p = Properties.Get<V_HIS_TREATMENT>();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(data));
            }
        }
    }
}
