using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPatient
{
    partial class HisPatientGet : GetBase
    {
        internal HisPatientGet()
            : base()
        {

        }

        internal HisPatientGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_PATIENT> Get(HisPatientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_PATIENT> GetView(HisPatientViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT> GetByIds(List<long> ids)
        {
            try
            {
                HisPatientFilterQuery filter = new HisPatientFilterQuery();
                filter.IDs = ids;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT GetById(long id)
        {
            try
            {
                return GetById(id, new HisPatientFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT GetById(long id, HisPatientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisPatientViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT GetViewById(long id, HisPatientViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT GetByCode(string code)
        {
            try
            {
                return GetByCode(code, new HisPatientFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_PATIENT GetByCode(string code, HisPatientFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.GetByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT GetViewByCode(string code)
        {
            try
            {
                return GetViewByCode(code, new HisPatientViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_PATIENT GetViewByCode(string code, HisPatientViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisPatientDAO.GetViewByCode(code, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT> GetByCareerId(long careerId)
        {
            try
            {
                HisPatientFilterQuery filter = new HisPatientFilterQuery();
                filter.CAREER_ID = careerId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT> GetByMilitaryRankId(long militaryRankId)
        {
            try
            {
                HisPatientFilterQuery filter = new HisPatientFilterQuery();
                filter.MILITARY_RANK_ID = militaryRankId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_PATIENT> GetByWorkPlaceId(long id)
        {
            try
            {
                HisPatientFilterQuery filter = new HisPatientFilterQuery();
                filter.WORK_PLACE_ID = id;
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
