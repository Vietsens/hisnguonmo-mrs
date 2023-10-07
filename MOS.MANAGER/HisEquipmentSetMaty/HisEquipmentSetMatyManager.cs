using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSetMaty
{
    public partial class HisEquipmentSetMatyManager : BusinessBase
    {
        public HisEquipmentSetMatyManager()
            : base()
        {

        }

        public HisEquipmentSetMatyManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_EQUIPMENT_SET_MATY> Get(HisEquipmentSetMatyFilterQuery filter)
        {
            List<HIS_EQUIPMENT_SET_MATY> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisEquipmentSetMatyGet(param).Get(filter);
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
