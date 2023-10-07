using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisPatient()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_PATIENT).GetProperty("PATIENT_CODE"));

            properties[typeof(HIS_PATIENT)] = pies;
        }
    }
}
