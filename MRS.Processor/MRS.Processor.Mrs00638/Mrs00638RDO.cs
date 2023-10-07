using MRS.Processor.Mrs00638;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.EFMODEL.DataModels;
using System.Reflection;
using MRS.MANAGER.Config;

namespace MRS.Proccessor.Mrs00638
{
    public class Mrs00638RDO:HIS_SERE_SERV
    {
        public int COUNT_TREATMENT { get; set; }

        public decimal? TEIN_AMOUNT { get; set; }
        public decimal? TEIN_EXPEND_TOTAL_PRICE { get; set; }
        public decimal? TEIN_TOTAL_PRICE { get; set; }

        public decimal? DIIM_AMOUNT { get; set; }
        public decimal? DIIM_EXPEND_TOTAL_PRICE { get; set; }
        public decimal? DIIM_TOTAL_PRICE { get; set; }

        public decimal? SUIM_AMOUNT { get; set; }
        public decimal? SUIM_EXPEND_TOTAL_PRICE { get; set; }
        public decimal? SUIM_TOTAL_PRICE { get; set; }

        public decimal? ENDO_AMOUNT { get; set; }
        public decimal? ENDO_EXPEND_TOTAL_PRICE { get; set; }
        public decimal? ENDO_TOTAL_PRICE { get; set; }

        public decimal? FUEX_AMOUNT { get; set; }
        public decimal? FUEX_EXPEND_TOTAL_PRICE { get; set; }
        public decimal? FUEX_TOTAL_PRICE { get; set; }

        public decimal? PTTT_AMOUNT { get; set; }
        public decimal? PTTT_TOTAL_PRICE { get; set; }
        public decimal? PTTT_FOLLOW_TOTAL_PRICE { get; set; }
        public decimal? PTTT_DIFF_TOTAL_PRICE { get; set; }

        public decimal? BED_TOTAL_PRICE { get; set; }
        public decimal? BED_EXPEND_TOTAL_PRICE { get; set; }

        public decimal? EXAM_TOTAL_PRICE { get; set; }
        
        public decimal? MEDICINE_TOTAL_PRICE { get; set; }
        
        public decimal? MATERIAL_TOTAL_PRICE { get; set; }
        
        public decimal? BLOOD_TOTAL_PRICE { get; set; }

        public decimal? TRANS_TOTAL_PRICE { get; set; }

        public decimal? DIFF_TOTAL_PRICE { get; set; }

        public short? IS_TREATING_OR_FEE_LOCK { get; set; }

        public long? ROOM_ID { get; set; }
        public string ROOM_CODE { get; set; }
        public string ROOM_NAME { get; set; }

        public long? DEPARTMENT_ID { get; set; }
        public string DEPARTMENT_CODE { get; set; }
        public string DEPARTMENT_NAME { get; set; }


        public string TDL_PATIENT_CODE { get; set; }
        public string TDL_PATIENT_NAME { get; set; }

        public Mrs00638RDO(Mrs00638RDO r, Mrs00638Filter mrs00638Filter)
        {
            PropertyInfo[] p = typeof(Mrs00638RDO).GetProperties();
            foreach (var item in p)
            {
                item.SetValue(this, item.GetValue(r));
            }
            SetExtendField(r, mrs00638Filter);
        }

        public Mrs00638RDO()
        {

        }

        private void SetExtendField(Mrs00638RDO r, Mrs00638Filter mrs00638Filter)
        {
            this.ROOM_CODE = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.ROOM_ID) ?? new V_HIS_ROOM()).ROOM_CODE;
            this.ROOM_NAME = (HisRoomCFG.HisRooms.FirstOrDefault(o => o.ID == this.ROOM_ID) ?? new V_HIS_ROOM()).ROOM_NAME;
            this.DEPARTMENT_CODE = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_CODE;
            this.DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == this.DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;

            var ToTalPatientPrice = this.VIR_TOTAL_PRICE ?? 0;
            var Amount = this.AMOUNT;
            var PtttFollowTotalPrice = this.PTTT_FOLLOW_TOTAL_PRICE??0;

            #region process with issue
            
            switch (this.TDL_SERVICE_TYPE_ID)
            {
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN:
                    this.TEIN_AMOUNT = Amount;
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.TEIN_EXPEND_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    else
                    {
                        this.TEIN_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA:
                   this.DIIM_AMOUNT = Amount;
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.DIIM_EXPEND_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    else
                    {
                        this.DIIM_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__SA:
                    this.SUIM_AMOUNT = Amount;
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.SUIM_EXPEND_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    else
                    {
                        this.SUIM_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN:
                    this.FUEX_AMOUNT = Amount;
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.FUEX_EXPEND_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    else
                    {
                        this.FUEX_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__NS:
                    this.ENDO_AMOUNT = Amount;
                    if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                    {
                        this.ENDO_EXPEND_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    else
                    {
                        this.ENDO_TOTAL_PRICE = ToTalPatientPrice;
                    }
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT:
                   this.PTTT_AMOUNT = Amount;
                   this.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                   this.PTTT_FOLLOW_TOTAL_PRICE = PtttFollowTotalPrice;
                   this.PTTT_DIFF_TOTAL_PRICE = ToTalPatientPrice - PtttFollowTotalPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT:
                     this.PTTT_AMOUNT = Amount;
                   this.PTTT_TOTAL_PRICE = ToTalPatientPrice;
                   this.PTTT_FOLLOW_TOTAL_PRICE = PtttFollowTotalPrice;
                   this.PTTT_DIFF_TOTAL_PRICE = ToTalPatientPrice - PtttFollowTotalPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G:
                   this.BED_TOTAL_PRICE = ToTalPatientPrice;
                   this.BED_EXPEND_TOTAL_PRICE = PtttFollowTotalPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH:
                    this.EXAM_TOTAL_PRICE = ToTalPatientPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC:
                    this.MEDICINE_TOTAL_PRICE = ToTalPatientPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT:
                    this.MATERIAL_TOTAL_PRICE = ToTalPatientPrice;
                    break;
                case IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU:
                    this.BLOOD_TOTAL_PRICE = ToTalPatientPrice;
                    break;
            }
            if (this.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
            {
                this.DIFF_TOTAL_PRICE = -ToTalPatientPrice - PtttFollowTotalPrice;
            }
            else
            {
                this.DIFF_TOTAL_PRICE = ToTalPatientPrice - PtttFollowTotalPrice;
            }
            #endregion
        }
    }
}
