using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisAnticipateMaty
{
    partial class HisAnticipateMatyGet : BusinessBase
    {
        internal HisAnticipateMatyGet()
            : base()
        {

        }

        internal HisAnticipateMatyGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_ANTICIPATE_MATY> Get(HisAnticipateMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMatyDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_MATY GetById(long id)
        {
            try
            {
                return GetById(id, new HisAnticipateMatyFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_ANTICIPATE_MATY GetById(long id, HisAnticipateMatyFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisAnticipateMatyDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ANTICIPATE_MATY> GetByMaterialTypeId(long id)
        {
            try
            {
                HisAnticipateMatyFilterQuery filter = new HisAnticipateMatyFilterQuery();
                filter.MATERIAL_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_ANTICIPATE_MATY> GetByAnticipateId(long id)
        {
            try
            {
                HisAnticipateMatyFilterQuery filter = new HisAnticipateMatyFilterQuery();
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
