using Inventec.Core;
using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSet
{
    public partial class HisEquipmentSetManager : BusinessBase
    {
        public HisEquipmentSetManager()
            : base()
        {

        }

        public HisEquipmentSetManager(CommonParam param)
            : base(param)
        {

        }

        public List<HIS_EQUIPMENT_SET> Get(HisEquipmentSetFilterQuery filter)
        {
            List<HIS_EQUIPMENT_SET> result = null;
            try
            {
                bool valid = true;
                valid = valid && IsNotNull(param);
                valid = valid && IsNotNull(filter);
                if (valid)
                {
                    result = new HisEquipmentSetGet(param).Get(filter);
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
