using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisEquipmentSetMaty
{
    public partial class HisEquipmentSetMatyDAO : EntityBase
    {
        private HisEquipmentSetMatyGet GetWorker
        {
            get
            {
                return (HisEquipmentSetMatyGet)Worker.Get<HisEquipmentSetMatyGet>();
            }
        }

        public List<HIS_EQUIPMENT_SET_MATY> Get(HisEquipmentSetMatySO search, CommonParam param)
        {
            List<HIS_EQUIPMENT_SET_MATY> result = new List<HIS_EQUIPMENT_SET_MATY>();
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

        public HIS_EQUIPMENT_SET_MATY GetById(long id, HisEquipmentSetMatySO search)
        {
            HIS_EQUIPMENT_SET_MATY result = null;
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
