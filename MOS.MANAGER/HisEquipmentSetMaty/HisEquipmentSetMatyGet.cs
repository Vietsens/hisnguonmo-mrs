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
        internal HisEquipmentSetMatyGet()
            : base()
        {

        }

        internal HisEquipmentSetMatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EQUIPMENT_SET_MATY> Get(HisEquipmentSetMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEquipmentSetMatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EQUIPMENT_SET_MATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisEquipmentSetMatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EQUIPMENT_SET_MATY GetById(long id, HisEquipmentSetMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisEquipmentSetMatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EQUIPMENT_SET_MATY> GetByEquipmentSetId(long equipmentSetId)
        {
            try
            {
                HisEquipmentSetMatyFilterQuery filter = new HisEquipmentSetMatyFilterQuery();
                filter.EQUIPMENT_SET_ID = equipmentSetId;
                return Get(filter);
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
