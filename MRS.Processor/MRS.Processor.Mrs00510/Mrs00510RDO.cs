using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00510;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Proccessor.Mrs00510
{

    public class Mrs00510RDO : HIS_TREATMENT
    {
        public string PATIENT_NAME { get; set; }
        public string DOB_YEAR { get; set; }
        const int MAX = 40;
        public Decimal AMOUNT_SERVICE_1 { get; set; }
        public Decimal AMOUNT_SERVICE_2 { get; set; }
        public Decimal AMOUNT_SERVICE_3 { get; set; }
        public Decimal AMOUNT_SERVICE_4 { get; set; }
        public Decimal AMOUNT_SERVICE_5 { get; set; }
        public Decimal AMOUNT_SERVICE_6 { get; set; }
        public Decimal AMOUNT_SERVICE_7 { get; set; }
        public Decimal AMOUNT_SERVICE_8 { get; set; }
        public Decimal AMOUNT_SERVICE_9 { get; set; }
        public Decimal AMOUNT_SERVICE_10 { get; set; }
        public Decimal AMOUNT_SERVICE_11 { get; set; }
        public Decimal AMOUNT_SERVICE_12 { get; set; }
        public Decimal AMOUNT_SERVICE_13 { get; set; }
        public Decimal AMOUNT_SERVICE_14 { get; set; }
        public Decimal AMOUNT_SERVICE_15 { get; set; }
        public Decimal AMOUNT_SERVICE_16 { get; set; }
        public Decimal AMOUNT_SERVICE_17 { get; set; }
        public Decimal AMOUNT_SERVICE_18 { get; set; }
        public Decimal AMOUNT_SERVICE_19 { get; set; }

        public Decimal AMOUNT_SERVICE_20 { get; set; }
        public Decimal AMOUNT_SERVICE_21 { get; set; }
        public Decimal AMOUNT_SERVICE_22 { get; set; }
        public Decimal AMOUNT_SERVICE_23 { get; set; }
        public Decimal AMOUNT_SERVICE_24 { get; set; }
        public Decimal AMOUNT_SERVICE_25 { get; set; }
        public Decimal AMOUNT_SERVICE_26 { get; set; }
        public Decimal AMOUNT_SERVICE_27 { get; set; }
        public Decimal AMOUNT_SERVICE_28 { get; set; }
        public Decimal AMOUNT_SERVICE_29 { get; set; }
        public Decimal AMOUNT_SERVICE_30 { get; set; }
        public Decimal AMOUNT_SERVICE_31 { get; set; }
        public Decimal AMOUNT_SERVICE_32 { get; set; }
        public Decimal AMOUNT_SERVICE_33 { get; set; }
        public Decimal AMOUNT_SERVICE_34 { get; set; }
        public Decimal AMOUNT_SERVICE_35 { get; set; }
        public Decimal AMOUNT_SERVICE_36 { get; set; }
        public Decimal AMOUNT_SERVICE_37 { get; set; }
        public Decimal AMOUNT_SERVICE_38 { get; set; }
        public Decimal AMOUNT_SERVICE_39 { get; set; }
        public Decimal AMOUNT_SERVICE_40 { get; set; }
        public Mrs00510RDO()
        {


        }

        public Mrs00510RDO(HIS_TREATMENT data, List<V_HIS_SERE_SERV> sereServs)
        {
            this.PATIENT_NAME = data.TDL_PATIENT_NAME;
            this.DOB_YEAR = data.TDL_PATIENT_DOB.ToString().Substring(0, 4);
            PropertyInfo[] p = Properties.Get<HIS_TREATMENT>();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(data));
            }
            
            int i = 0;
            foreach (var item in HisServiceCFG.getList_SERVICE_CODE__KND)
            {
                i++;
                if (i > MAX) break;
                PropertyInfo s = typeof(Mrs00510RDO).GetProperty(string.Format("AMOUNT_SERVICE_{0}", i));
                s.SetValue(this, sereServs.Where(o => o.TDL_SERVICE_CODE == item).Sum(q => q.AMOUNT));

            }
        }
    }
}
