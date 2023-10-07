using MOS.EFMODEL.DataModels;
using System;
using TYT.EFMODEL.DataModels;

namespace MRS.Processor.Mrs00570
{
    
    class Mrs00570RDO : TYT_GDSK
    {

        public string GDSK_TIME_STR { get; set; }

        public Mrs00570RDO(TYT_GDSK data)
        {
            System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT_GDSK>();
            foreach (var item in pi)
            {
                item.SetValue(this, (item.GetValue(data)));
            }
            SetExtendField(this);
        }

        private void SetExtendField(Mrs00570RDO mrs00570RDO)
        {
            try
            {
                this.GDSK_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(this.GDSK_TIME ?? 0);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

    }
}
