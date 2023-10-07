using Inventec.Common.Repository;
using MOS.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Inventec.Common.Logging;

namespace MRS.Processor.Mrs00208
{
    public class Mrs00208RDO : V_HIS_EXP_MEST_MATERIAL
    {
        public Decimal AMOUNT_TRUST { get; set; }
        public long PATIENT_ID { get; set; }
        public string VIR_PATIENT_NAME { get; set; }
        public string TDL_TREATMENT_CODE { get; set; }
        public string TDL_PATIENT_CODE { get; set; }
        public string EXP_TIME_STR { get; set; }
        public Decimal TOTAL_PRICE { get; set; }
        public long? INTRUCTION_TIME { get; set; }
        public long? INTRUCTION_DATE { get; set; }
        public long? OUT_TIME { get; set; }
        public string SERVICE_REQ_CODE { get; set; }
        public string REQUEST_USERNAME { get; set; }
        public string REQUEST_LOGINNAME { get; set; }
        public string TDL_PATIENT_ADDRESS { get; set; }
        public string TREATMENT_TYPE_CODE { get; set; }
        public string TREATMENT_TYPE_NAME { get; set; }
        public string TDL_PATIENT_TYPE_CODE { get; set; }
        public string TDL_PATIENT_TYPE_NAME { get; set; }
        public string REQUEST_DEPARTMENT_CODE { get; set; }
        public string REQUEST_DEPARTMENT_NAME { get; set; }
        public string REQUEST_ROOM_CODE { get; set; }
        public string REQUEST_ROOM_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string TREATMENT_END_TYPE_CODE { get; set; }
        public string TREATMENT_END_TYPE_NAME { get; set; }
        public string EXP_MEST_TYPE_CODE { get; set; }
        public string EXP_MEST_TYPE_NAME { get; set; }
        public string CATEGORY_CODE { get; set; }
        public string CATEGORY_NAME { get; set; }
        public long TREATMENT_TYPE_ID { get; set; }
        public long TDL_PATIENT_TYPE_ID { get; set; }
        public long TREATMENT_END_TYPE_ID { get; set; }

        //public Mrs00208RDO(V_HIS_EXP_MEST_MATERIAL data, List<V_HIS_EXP_MEST> pres, List<V_HIS_IMP_MEST> impMoba, List<V_HIS_IMP_MEST_MATERIAL> impMedi,List<HIS_SERVICE_REQ> ListServiceReq)
        //{
        //    PropertyInfo[] pi = Properties.Get<V_HIS_EXP_MEST_MATERIAL>();
        //    foreach (var item in pi)
        //    {
        //        item.SetValue(this, item.GetValue(data));
        //    }
        //    extentKey(data, pres, impMoba, impMedi, ListServiceReq);
        //}

        //private void extentKey(V_HIS_EXP_MEST_MATERIAL data, List<V_HIS_EXP_MEST> pres, List<V_HIS_IMP_MEST> impMoba, List<V_HIS_IMP_MEST_MATERIAL> impMedi, List<HIS_SERVICE_REQ> ListServiceReq)
        //{
        //    try
        //    {
        //        var presSub = (pres != null && pres.Count > 0) ? pres.Where(o => o.ID == data.EXP_MEST_ID).ToList() : new List<V_HIS_EXP_MEST>();
        //        var mobaSub = (impMoba != null && impMoba.Count > 0) ? impMoba.Where(o => (o.MOBA_EXP_MEST_ID ?? 0) == data.EXP_MEST_ID).ToList() : new List<V_HIS_IMP_MEST>();
        //        var mediSub = (impMedi != null && impMedi.Count > 0 && mobaSub.Count > 0) ? impMedi.Where(o => mobaSub.Select(p => p.ID).Contains(o.IMP_MEST_ID) && o.MATERIAL_ID == data.MATERIAL_ID).ToList() : new List<V_HIS_IMP_MEST_MATERIAL>();
        //        this.AMOUNT_TRUST = mediSub.Count > 0 ? this.AMOUNT - mediSub.Sum(s => s.AMOUNT) : this.AMOUNT;
        //        this.TOTAL_PRICE = AMOUNT_TRUST*(this.PRICE??0);
        //        this.PATIENT_ID = presSub.Count > 0 ? (presSub.First().TDL_PATIENT_ID ?? 0) : 0;
        //        this.VIR_PATIENT_NAME = presSub.Count > 0 ? presSub.First().TDL_PATIENT_NAME : "";
        //        this.TDL_TREATMENT_CODE = presSub.Count > 0 ? presSub.First().TDL_TREATMENT_CODE : "";
        //        this.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.EXP_TIME ?? 0);
        //        var serviceReq = ListServiceReq.FirstOrDefault(o => o.ID == data.TDL_SERVICE_REQ_ID);
        //        if (serviceReq != null)
        //        {
        //            this.INTRUCTION_TIME = serviceReq.INTRUCTION_TIME;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogSystem.Error(ex);
        //    }
        //}
    }
}
