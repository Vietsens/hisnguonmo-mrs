using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00574
{
    class Mrs00574RDO : TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION
    {
        public long? AGE { get; set; }

        public Mrs00574RDO() { }

        public Mrs00574RDO(TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION data)
        {
            try
            {
                if (data != null)
                {
                    System.Reflection.PropertyInfo[] pi = Inventec.Common.Repository.Properties.Get<TYT.EFMODEL.DataModels.TYT_FETUS_ABORTION>();
                    foreach (var item in pi)
                    {
                        item.SetValue(this, (item.GetValue(data)));
                    }

                    if (data.DOB > 0)
                    {
                        var year = DateTime.Now.Year;
                        var dataYear = int.Parse(data.DOB.ToString().Substring(0, 4));

                        this.AGE = year - dataYear;
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
