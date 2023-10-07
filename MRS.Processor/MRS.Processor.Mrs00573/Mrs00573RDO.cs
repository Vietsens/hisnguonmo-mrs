using MOS.EFMODEL.DataModels;
using System;
using TYT.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00573
{
    
    class Mrs00573RDO : TYT_TUBERCULOSIS
    {
        const string genderMale = "Nam";
        const string genderFeMale = "Nữ";
        public string OLD_MALE { get; set; }
        public string OLD_FEMALE { get; set; }

        public string TYT_IN_TIME_STR { get; set; }

        const long resultCured = 1;
        const long resultDisease = 2;
        const long resultTran = 3;
        const long resultEscape = 4;
        const long resultDeath = 5;


        public string IS_CURED { get; set; }
        public string ISDISEASE { get; set; }	
        public string ISTRAN { get; set; }
        public string ISESCAPE { get; set; }	
        public string ISDEATH { get; set; }



        public Mrs00573RDO(TYT_TUBERCULOSIS data)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT_TUBERCULOSIS>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(data)));
            }
            SetExtendField(this);
        }

        private void SetExtendField(Mrs00573RDO mrs00573RDO)
        {
            try
            {
                this.OLD_MALE = this.GENDER_NAME == genderMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.OLD_FEMALE = this.GENDER_NAME == genderFeMale ? this.DOB.ToString().Substring(0, 4) : "";
                this.TYT_IN_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.TYT_IN_TIME ?? 0);
                switch (this.TREATMENT_RESULT)
                {
                    case resultCured: IS_CURED = "x";
                        break;
                    case resultDisease: ISDISEASE = "x";
                        break;
                    case resultTran: ISTRAN = "x";
                        break;
                    case resultEscape: ISESCAPE = "x";
                        break;
                    case resultDeath: ISDEATH = "x";
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
