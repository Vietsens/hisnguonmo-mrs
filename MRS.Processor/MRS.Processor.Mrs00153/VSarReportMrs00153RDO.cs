using AutoMapper;
using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00153
{
    public class VSarReportMrs00153RDO
    {
        public long DEPARTMENT_ID { get; set; }
        public int NUMBER_ORDER { get; set; }
        public string MEDICINE_TYPE_NAME { get; internal set; }
        public string SERVICE_UNIT_NAME { get; internal set; }
        public string NATIONAL_NAME { get; internal set; }
        public decimal? PRICE { get; internal set; }
        public decimal? AMOUNT { get; internal set; }
        public decimal? TOTAL_PRICE { get; internal set; }
    }

    public class TotalPrice
    {
        public long DEPARTMENT_ID { get; set; }

        public decimal? TOTAL_PRICE { get; set; }
    }

    public class MedicineTypeRdo : V_HIS_IMP_MEST_MEDICINE
    {
        public int TYPE { get; set; }
        public decimal TOTAL_PRICE { get; set; }

        public Dictionary<string, decimal> DIC_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_TOTAL_PRICE { get; set; }
        public Dictionary<string, decimal> DIC_ROOM_AMOUNT { get; set; }
        public Dictionary<string, decimal> DIC_ROOM_TOTAL_PRICE { get; set; }
        public string REQ_DEPARTMENT_CODE { get; set; }
        public string REQ_DEPARTMENT_NAME { get; set; }
        public string MEDI_STOCK_NAME { get; set; }
        public string MEDI_STOCK_CODE { get; set; }
        public string CONTAIN_DEPARTMENT_CODE { get; set; }
        public string CONTAIN_DEPARTMENT_NAME { get; set; }
        public long REQ_TIME { get; set; }
        public long APPROVAL_TIME { get; set; }
        public string CHMS_MEDI_STOCK_NAME { get; set; }
        public string CHMS_MEDI_STOCK_CODE { get; set; }
        public string CHMS_DEPARTMENT_NAME { get; set; }
        public string CHMS_DEPARTMENT_CODE { get; set; }
        public string AGGR_IMP_MEST_CODE { get; set; }
        public string START_IMP_MEST_CODE { get; set; }
        public string IMP_MEST_TYPE_NAME { get; set; }
        public string IMP_MEST_SUB_CODE { get; set; }
        public string EXP_MEST_CODE { get; set; }
        public MedicineTypeRdo(V_HIS_IMP_MEST_MEDICINE r, List<HIS_MEDICINE> Medicines)
        {
            TYPE = 1;
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<MOS.EFMODEL.DataModels.V_HIS_IMP_MEST_MEDICINE>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(r)));
            }
            START_IMP_MEST_CODE = r.IMP_MEST_CODE;
            DIC_AMOUNT = new Dictionary<string, decimal>();
            DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_ROOM_AMOUNT = new Dictionary<string, decimal>();
            DIC_ROOM_TOTAL_PRICE = new Dictionary<string, decimal>();

            this.REQ_DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.REQ_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            HIS_MEDICINE Medicine = Medicines.FirstOrDefault(o => o.ID == r.MEDICINE_ID) ?? new HIS_MEDICINE();
            this.BID_GROUP_CODE = Medicine.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Medicine.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Medicine.TDL_BID_NUMBER;
        }

        public MedicineTypeRdo(V_HIS_IMP_MEST_MATERIAL r, List<HIS_MATERIAL> Materials)
        {
            TYPE = 2;
            Mapper.CreateMap<V_HIS_IMP_MEST_MATERIAL, V_HIS_IMP_MEST_MEDICINE>();
            Mapper.Map<V_HIS_IMP_MEST_MATERIAL, V_HIS_IMP_MEST_MEDICINE>(r,this);
            DIC_AMOUNT = new Dictionary<string, decimal>();
            DIC_TOTAL_PRICE = new Dictionary<string, decimal>();
            DIC_ROOM_AMOUNT = new Dictionary<string, decimal>();
            DIC_ROOM_TOTAL_PRICE = new Dictionary<string, decimal>();

            this.START_IMP_MEST_CODE = r.IMP_MEST_CODE;
            this.MEDICINE_TYPE_CODE = r.MATERIAL_TYPE_CODE;
            this.MEDICINE_TYPE_ID = r.MATERIAL_TYPE_ID;
            this.MEDICINE_TYPE_NAME = r.MATERIAL_TYPE_NAME;
            this.MEDICINE_NUM_ORDER = r.MATERIAL_NUM_ORDER;
            this.REQ_DEPARTMENT_CODE = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.REQ_DEPARTMENT_NAME = (MANAGER.Config.HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == r.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;
            HIS_MATERIAL Material = Materials.FirstOrDefault(o => o.ID == r.MATERIAL_ID) ?? new HIS_MATERIAL();
            this.BID_GROUP_CODE = Material.TDL_BID_GROUP_CODE;
            this.BID_NUM_ORDER = Material.TDL_BID_NUM_ORDER;
            this.TDL_BID_NUMBER = Material.TDL_BID_NUMBER;
            this.MEDICINE_GROUP_ID = 0;
            this.MEDICINE_GROUP_NAME = "Vật tư";
            this.MEDICINE_GROUP_CODE = "VTU";
        }

        public string TDL_BID_NUMBER { get; set; }

        public string BID_NUM_ORDER { get; set; }

        public string BID_GROUP_CODE { get; set; }

        public string MOBA_EXP_MEDI_STOCK_NAME { get; set; }

        public string MOBA_EXP_MEDI_STOCK_CODE { get; set; }
        public string MEDICINE_GROUP_NAME { get;  set; }
        public string MEDICINE_GROUP_CODE { get;  set; }
    }
}
