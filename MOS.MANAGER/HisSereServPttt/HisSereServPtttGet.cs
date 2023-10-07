using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServPttt
{
    partial class HisSereServPtttGet : BusinessBase
    {
        internal HisSereServPtttGet()
            : base()
        {

        }

        internal HisSereServPtttGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_SERE_SERV_PTTT> Get(HisSereServPtttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServPtttDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_SERE_SERV_PTTT> GetView(HisSereServPtttViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServPtttDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_PTTT GetById(long id)
        {
            try
            {
                return GetById(id, new HisSereServPtttFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_SERE_SERV_PTTT GetById(long id, HisSereServPtttFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServPtttDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_PTTT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisSereServPtttViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_SERE_SERV_PTTT GetViewById(long id, HisSereServPtttViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisSereServPtttDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByBloodId(long bloodId)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.BLOOD_ID = bloodId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByBloodRhId(long bloodRhId)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.BLOOD_RH_ID = bloodRhId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByEmotionlessMethodId(long emotionlessMethodId)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.EMOTIONLESS_METHOD_ID = emotionlessMethodId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByPtttCatastropheId(long id)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.PTTT_CATASTROPHE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByPtttConditionId(long id)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.PTTT_CONDITION_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByPtttGroupId(long id)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.PTTT_GROUP_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetByPtttMethodId(long id)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.PTTT_METHOD_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_SERE_SERV_PTTT> GetBySereServIds(List<long> sereServIds)
        {
            try
            {
                HisSereServPtttFilterQuery filter = new HisSereServPtttFilterQuery();
                filter.SERE_SERV_IDs = sereServIds;
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
