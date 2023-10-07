using MOS.EFMODEL.DataModels;
using System;
using TYT.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00572
{
    
    class Mrs00572RDO : TYT_HIV
    {
        const string genderMale = "Nam";
        const string genderFeMale = "Nữ";
        public string OLD_MALE { get; set; }
        public string OLD_FEMALE { get; set; }

        public string CREATE_TIME_STR { get; set; }
        public string HIV_DIAGNOSIS_TIME_STR { get; set; }
        public string DEATH_TIME_STR { get; set; }
        public string FETUS_TIME_STR { get; set; }

        public Mrs00572RDO(TYT_HIV data)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT_HIV>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(data)));
            }
            SetExtendField(this);
        }

        private void SetExtendField(Mrs00572RDO mrs00572RDO)
        {
            try
            {
                this.OLD_MALE = this.GENDER_NAME == genderMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.OLD_FEMALE = this.GENDER_NAME == genderFeMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.HIV_DIAGNOSIS_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.HIV_DIAGNOSIS_TIME ?? 0);
                this.CREATE_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.CREATE_TIME ?? 0);
                this.DEATH_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.DEATH_TIME ?? 0);
                this.FETUS_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.FETUS_TIME ?? 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
