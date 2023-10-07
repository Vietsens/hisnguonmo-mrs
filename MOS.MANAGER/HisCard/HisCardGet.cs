using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisCard
{
    partial class HisCardGet : GetBase
    {
        internal HisCardGet()
            : base()
        {

        }

        internal HisCardGet(Inventec.Core.CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_CARD> Get(HisCardFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCardDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARD GetById(long id)
        {
            try
            {
                return GetById(id, new HisCardFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARD GetById(long id, HisCardFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCardDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_CARD> GetView(HisCardViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCardDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARD GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisCardViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARD GetViewById(long id, HisCardViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisCardDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARD GetViewByCode(string code)
        {
            try
            {
                HisCardViewFilterQuery filter = new HisCardViewFilterQuery();
                filter.CODE__EXACT = code;
                List<V_HIS_CARD> data = this.GetView(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARD GetByCode(string code)
        {
            try
            {
                HisCardFilterQuery filter = new HisCardFilterQuery();
                filter.CODE__EXACT = code;
                List<HIS_CARD> data = this.Get(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_CARD GetByCardCode(string cardCode)
        {
            try
            {
                HisCardFilterQuery filter = new HisCardFilterQuery();
                filter.CARD_CODE__EXACT = cardCode;
                List<HIS_CARD> data = this.Get(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARD GetViewByCardCode(string cardCode)
        {
            try
            {
                HisCardViewFilterQuery filter = new HisCardViewFilterQuery();
                filter.CARD_CODE__EXACT = cardCode;
                List<V_HIS_CARD> data = this.GetView(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_CARD GetViewByServiceCode(string serviceCode)
        {
            try
            {
                HisCardViewFilterQuery filter = new HisCardViewFilterQuery();
                filter.SERVICE_CODE__EXACT = serviceCode;
                List<V_HIS_CARD> data = this.GetView(filter);
                return IsNotNullOrEmpty(data) ? data[0] : null;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_CARD> GetByPatientId(long id)
        {
            try
            {
                HisCardFilterQuery filter = new HisCardFilterQuery();
                filter.PATIENT_ID = id;
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
