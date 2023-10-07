using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOS.MANAGER.HisVitaminA
{
    class HisVitaminAGet : GetBase
    {
        internal HisVitaminAGet()
            : base()
        {

        }

        internal HisVitaminAGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_VITAMIN_A> Get(HisVitaminAFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisVitaminADAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_VITAMIN_A> GetView(HisVitaminAViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisVitaminADAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_VITAMIN_A GetById(long id)
        {
            try
            {
                return GetById(id, new HisVitaminAFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_VITAMIN_A GetById(long id, HisVitaminAFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisVitaminADAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_VITAMIN_A GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisVitaminAViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_VITAMIN_A GetViewById(long id, HisVitaminAViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisVitaminADAO.GetViewById(id, filter.Query());
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
