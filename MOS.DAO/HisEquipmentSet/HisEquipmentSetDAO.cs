using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEquipmentSet
{
    public partial class HisEquipmentSetDAO : EntityBase
    {
        private HisEquipmentSetGet GetWorker
        {
            get
            {
                return (HisEquipmentSetGet)Worker.Get<HisEquipmentSetGet>();
            }
        }

        public List<HIS_EQUIPMENT_SET> Get(HisEquipmentSetSO search, CommonParam param)
        {
            List<HIS_EQUIPMENT_SET> result = new List<HIS_EQUIPMENT_SET>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_EQUIPMENT_SET GetById(long id, HisEquipmentSetSO search)
        {
            HIS_EQUIPMENT_SET result = null;
            try
            {
                result = GetWorker.GetById(id, search);
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
