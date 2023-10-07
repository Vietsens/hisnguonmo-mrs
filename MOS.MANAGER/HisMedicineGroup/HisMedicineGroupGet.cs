using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMedicineGroup
{
    partial class HisMedicineGroupGet : BusinessBase
    {
        internal HisMedicineGroupGet()
            : base()
        {

        }

        internal HisMedicineGroupGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDICINE_GROUP> Get(HisMedicineGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineGroupDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_GROUP GetById(long id)
        {
            try
            {
                return GetById(id, new HisMedicineGroupFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDICINE_GROUP GetById(long id, HisMedicineGroupFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMedicineGroupDAO.GetById(id, filter.Query());
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
