using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00612
{
    class Mrs00612RDO
    {
        public string BRANCH_NAME { get; set; }
        public long BRANCH_ID { get; set; }

        public long? COUNT_0_5 { get; set; }
        public long? COUNT_6_12 { get; set; }
        public long? COUNT_12_36 { get; set; }
        public long? COUNT_37_60 { get; set; }
        public long? COUNT_MOTHER { get; set; }

        public long? COUNT_VTM_A_6_36 { get; set; }
        public long? COUNT_VTM_A_37_60 { get; set; }
        public long? COUNT_VTM_A_MOTHER { get; set; }
        public long? COUNT_VTM_A_SICK_5Y { get; set; }
        public long? COUNT_VTM_A_OTHER { get; set; }

        public decimal? AMOUNT_VTM_A_BEGIN { get; set; }
        public decimal? AMOUNT_VTM_A_IN { get; set; }
        public decimal? AMOUNT_VTM_A_OUT { get; set; }
        public decimal? AMOUNT_VTM_A_END { get; set; }

        public Mrs00612RDO(HIS_BRANCH branch, List<VitaminAADO> listVitaminA, List<TreatmentADO> lstTreatment, List<HIS_BABY> ListBaby, decimal? beginAmount, decimal? inAmount, decimal? outAmount, decimal? endAmount)
        {
            try
            {
                this.AMOUNT_VTM_A_BEGIN = beginAmount;
                this.AMOUNT_VTM_A_END = endAmount;
                this.AMOUNT_VTM_A_IN = inAmount;
                this.AMOUNT_VTM_A_OUT = outAmount;

                if (branch != null)
                {
                    this.BRANCH_ID = branch.ID;
                    this.BRANCH_NAME = branch.BRANCH_NAME;
                }

                if (listVitaminA != null && listVitaminA.Count > 0)
                {

                    var VTM_A_6_36 = listVitaminA.Count(o => o.CASE_TYPE == 1 && !o.IS_SICK.HasValue && o.MONTH_OLD <= 36 && o.MONTH_OLD >= 6 && o.EXECUTE_TIME.HasValue);
                    if (VTM_A_6_36 > 0) COUNT_VTM_A_6_36 = VTM_A_6_36;

                    var VTM_A_37_60 = listVitaminA.Count(o => o.CASE_TYPE == 1 && !o.IS_SICK.HasValue && o.MONTH_OLD >= 37 && o.MONTH_OLD <= 60 && o.EXECUTE_TIME.HasValue);
                    if (VTM_A_37_60 > 0) COUNT_VTM_A_37_60 = VTM_A_37_60;

                    var VTM_A_MOTHER_6M = listVitaminA.Count(o => o.CASE_TYPE == 2 && o.IS_ONE_MONTH_BORN == 1 && o.EXECUTE_TIME.HasValue);
                    if (VTM_A_MOTHER_6M > 0) COUNT_VTM_A_MOTHER = VTM_A_MOTHER_6M;

                    var VTM_A_SICK_5Y = listVitaminA.Count(o => o.MONTH_OLD <= 60 && o.IS_SICK == 1 && o.EXECUTE_TIME.HasValue);
                    if (VTM_A_SICK_5Y > 0) COUNT_VTM_A_SICK_5Y = VTM_A_SICK_5Y;

                    var VTM_A_OTHER = listVitaminA.Count(o => o.CASE_TYPE == 3 && o.EXECUTE_TIME.HasValue);
                    if (VTM_A_OTHER > 0) COUNT_VTM_A_OTHER = VTM_A_OTHER;
                }

                if (lstTreatment != null && lstTreatment.Count > 0)
                {
                    int c_0_5 = lstTreatment.Count(o => o.MONTH_OLD <= 5);
                    if (c_0_5 > 0) COUNT_0_5 = c_0_5;

                    int c_6_12 = lstTreatment.Count(o => o.MONTH_OLD >= 6 && o.MONTH_OLD <= 12);
                    if (c_6_12 > 0) COUNT_6_12 = c_6_12;

                    int c_12_36 = lstTreatment.Count(o => o.MONTH_OLD > 12 && o.MONTH_OLD <= 36);
                    if (c_12_36 > 0) COUNT_12_36 = c_12_36;

                    int c_37_60 = lstTreatment.Count(o => o.MONTH_OLD >= 37 && o.MONTH_OLD <= 60);
                    if (c_37_60 > 0) COUNT_37_60 = c_37_60;

                    if (ListBaby != null && ListBaby.Count > 0)
                    {
                        int c_mother = lstTreatment.Count(o => ListBaby.Select(s => s.TREATMENT_ID).Contains(o.ID));
                        if (c_mother > 0) COUNT_MOTHER = c_mother;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
