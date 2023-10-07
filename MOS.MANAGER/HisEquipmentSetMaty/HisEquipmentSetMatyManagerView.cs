using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSetMaty
{
    public partial class HisEquipmentSetMatyManager : BusinessBase
    {
        public List<V_HIS_EQUIPMENT_SET_MATY> GetView(HisEquipmentSetMatyViewFilterQuery filter)
        {
            List<V_HIS_EQUIPMENT_SET_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisEquipmentSetMatyGet(param).GetView(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                param.HasException = true;
                result = null;
            }
            return result;
        }
    }
}
