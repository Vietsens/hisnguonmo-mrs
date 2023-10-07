using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDiseaseRelation
{
    class HisDiseaseRelationGet : GetBase
    {
        internal HisDiseaseRelationGet()
            : base()
        {

        }

        internal HisDiseaseRelationGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_DISEASE_RELATION> Get(HisDiseaseRelationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDiseaseRelationDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DISEASE_RELATION GetById(long id)
        {
            try
            {
                return GetById(id, new HisDiseaseRelationFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DISEASE_RELATION GetById(long id, HisDiseaseRelationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDiseaseRelationDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DISEASE_RELATION GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisDiseaseRelationFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_DISEASE_RELATION GetByCode(string code, HisDiseaseRelationFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisDiseaseRelationDAO.GetByCode(code, filter.Query());
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
