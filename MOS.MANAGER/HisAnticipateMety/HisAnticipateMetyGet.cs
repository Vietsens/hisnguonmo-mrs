using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMety
{
    partial class HisAnticipateMetyGet : BusinessBase
    {
        internal HisAnticipateMetyGet()
            : base()
        {

        }

        internal HisAnticipateMetyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ANTICIPATE_METY> Get(HisAnticipateMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMetyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_METY GetById(long id)
        {
            try
            {
                return GetById(id, new HisAnticipateMetyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_METY GetById(long id, HisAnticipateMetyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMetyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ANTICIPATE_METY> GetByMaterialTypeId(long id)
        {
            try
            {
                HisAnticipateMetyFilterQuery filter = new HisAnticipateMetyFilterQuery();
                filter.MEDICINE_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ANTICIPATE_METY> GetByAnticipateId(long id)
        {
            try
            {
                HisAnticipateMetyFilterQuery filter = new HisAnticipateMetyFilterQuery();
                filter.ANTICIPATE_ID = id;
                return this.Get(filter);
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
