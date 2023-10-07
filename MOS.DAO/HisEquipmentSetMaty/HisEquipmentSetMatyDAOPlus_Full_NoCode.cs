using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEquipmentSetMaty
{
    public partial class HisEquipmentSetMatyDAO : EntityBase
    {
        public List<V_HIS_EQUIPMENT_SET_MATY> GetView(HisEquipmentSetMatySO search, CommonParam param)
        {
            List<V_HIS_EQUIPMENT_SET_MATY> result = new List<V_HIS_EQUIPMENT_SET_MATY>();
            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public V_HIS_EQUIPMENT_SET_MATY GetViewById(long id, HisEquipmentSetMatySO search)
        {
            V_HIS_EQUIPMENT_SET_MATY result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
