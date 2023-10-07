using MOS.EFMODEL.DataModels;
using System;
using TYT.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00567
{
    
    class Mrs00567RDO : TYT_UNINFECT
    {
        const string genderMale = "Nam";
        const string genderFeMale = "Nữ";
        public string OLD_MALE { get; set; }
        public string OLD_FEMALE { get; set; }

        public string DIAGNOSIS_TIME_STR { get; set; }

        public Mrs00567RDO(TYT_UNINFECT data)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT_UNINFECT>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(data)));
            }
            SetExtendField(this);
        }

        private void SetExtendField(Mrs00567RDO mrs00567RDO)
        {
            try
            {
                this.OLD_MALE = this.GENDER_NAME == genderMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.OLD_FEMALE = this.GENDER_NAME == genderFeMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.DIAGNOSIS_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.DIAGNOSIS_TIME??0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
