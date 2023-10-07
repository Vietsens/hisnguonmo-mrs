using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSetMaty
{
    partial class HisEquipmentSetMatyGet : BusinessBase
    {
        internal List<V_HIS_EQUIPMENT_SET_MATY> GetView(HisEquipmentSetMatyViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEquipmentSetMatyDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
