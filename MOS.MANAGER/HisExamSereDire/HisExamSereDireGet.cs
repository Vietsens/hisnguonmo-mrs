using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamSereDire
{
    partial class HisExamSereDireGet : BusinessBase
    {
        internal HisExamSereDireGet()
            : base()
        {

        }

        internal HisExamSereDireGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_EXAM_SERE_DIRE> Get(HisExamSereDireFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamSereDireDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXAM_SERE_DIRE> GetView(HisExamSereDireViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamSereDireDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXAM_SERE_DIRE GetById(long id)
        {
            try
            {
                return GetById(id, new HisExamSereDireFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_EXAM_SERE_DIRE GetById(long id, HisExamSereDireFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamSereDireDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_EXAM_SERE_DIRE GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisExamSereDireViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_EXAM_SERE_DIRE GetViewById(long id, HisExamSereDireViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisExamSereDireDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_EXAM_SERE_DIRE> GetViewByIds(List<long> ids)
        {
            try
            {
                if (ids != null)
                {
                    HisExamSereDireViewFilterQuery filter = new HisExamSereDireViewFilterQuery();
                    filter.IDs = ids;
                    return this.GetView(filter);
                }
                return null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_EXAM_SERE_DIRE> GetByDiseaseRelationId(long id)
        {
            try
            {
                HisExamSereDireFilterQuery filter = new HisExamSereDireFilterQuery();
                filter.DISEASE_RELATION_ID = id;
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
