using MOS.EFMODEL.DataModels;
using System.Collections.Generic;
using System.Reflection;

namespace MOS.EFMODEL.Decorator
{
    public partial class DummyDecorator
    {
        private static void LoadHisPatientProgram()
        {
            List<PropertyInfo> pies = new List<PropertyInfo>();
            pies.Add(typeof(HIS_PATIENT_PROGRAM).GetProperty("PATIENT_PROGRAM_CODE"));

            properties[typeof(HIS_PATIENT_PROGRAM)] = pies;
        }
    }
}
