using MOS.EFMODEL.DataModels;
using System;
using TYT.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00569
{
    
    class Mrs00569RDO : TYT_MALARIA
    {
        const string genderMale = "Nam";
        const string genderFeMale = "Nữ";
        const long testTypeLam = 1;
        const long testTypeQueThu = 2;
        public string OLD_MALE { get; set; }
        public string OLD_FEMALE { get; set; }

        public string DIAGNOSIS_TIME_STR { get; set; }
        public string IS_HAS_FETUS_STR { get; set; }
        public string IS_FEVER_STR { get; set; }
        public string TEST_TYPE_STR { get; set; }

        public Mrs00569RDO(TYT_MALARIA data)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT_MALARIA>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(data)));
            }
            SetExtendField(this);
        }

        private void SetExtendField(Mrs00569RDO mrs00569RDO)
        {
            try
            {
                this.OLD_MALE = this.GENDER_NAME == genderMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.OLD_FEMALE = this.GENDER_NAME == genderFeMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.DIAGNOSIS_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.DIAGNOSE_TIME ?? 0);
                this.IS_HAS_FETUS_STR = this.IS_HAS_FETUS==IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE?"x":"";
                this.IS_FEVER_STR = this.IS_FEVER == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE ? "x" : "";
                this.TEST_TYPE_STR = this.TEST_TYPE == testTypeLam ? "Lam" : (this.TEST_TYPE == testTypeQueThu?"Que thử":"");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
