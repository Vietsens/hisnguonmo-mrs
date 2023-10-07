using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatientTypeAlter
{
    public partial class HisPatientTypeAlterManager : BusinessBase
    {
        public List<D_HIS_PATIENT_TYPE_ALTER_1> GetDHisPatientTypeAlter1(DHisPatientTypeAlter1Filter filter)
        {
            List<D_HIS_PATIENT_TYPE_ALTER_1> result = null;

            try
            {
                result = new HisPatientTypeAlterGet(param).GetDHisPatientTypeAlter1(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }

            return result;
        }
    }
}
