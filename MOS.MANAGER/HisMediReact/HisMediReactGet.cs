using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediReact
{
    class HisMediReactGet : BusinessBase
    {
        internal HisMediReactGet()
            : base()
        {

        }

        internal HisMediReactGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_MEDI_REACT> Get(HisMediReactFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_MEDI_REACT> GetView(HisMediReactViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT GetById(long id)
        {
            try
            {
                return GetById(id, new HisMediReactFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_MEDI_REACT GetById(long id, HisMediReactFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
        
        internal V_HIS_MEDI_REACT GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisMediReactViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_MEDI_REACT GetViewById(long id, HisMediReactViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisMediReactDAO.GetViewById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_REACT> GetByMediReactTypeId(long id)
        {
            try
            {
                HisMediReactFilterQuery filter = new HisMediReactFilterQuery();
                filter.MEDI_REACT_TYPE_ID = id;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_MEDI_REACT> GetByMedicineId(long id)
        {
            try
            {
                HisMediReactFilterQuery filter = new HisMediReactFilterQuery();
                filter.MEDICINE_ID = id;
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
