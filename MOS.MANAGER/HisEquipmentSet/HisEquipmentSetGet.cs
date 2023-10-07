using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEquipmentSet
{
    partial class HisEquipmentSetGet : BusinessBase
    {
        internal HisEquipmentSetGet()
            : base()
        {

        }

        internal HisEquipmentSetGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EQUIPMENT_SET> Get(HisEquipmentSetFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEquipmentSetDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EQUIPMENT_SET GetById(long id)
        {
            try
            {
                return GetById(id, new HisEquipmentSetFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EQUIPMENT_SET GetById(long id, HisEquipmentSetFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEquipmentSetDAO.GetById(id, filter.Query());
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
