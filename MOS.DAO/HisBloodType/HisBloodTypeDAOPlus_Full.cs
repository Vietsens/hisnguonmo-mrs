using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisBloodType
{
    public partial class HisBloodTypeDAO : EntityBase
    {
        public List<V_HIS_BLOOD_TYPE> GetView(HisBloodTypeSO search, CommonParam param)
        {
            List<V_HIS_BLOOD_TYPE> result = new List<V_HIS_BLOOD_TYPE>();

            try
            {
                result = GetWorker.GetView(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }

        public HIS_BLOOD_TYPE GetByCode(string code, HisBloodTypeSO search)
        {
            HIS_BLOOD_TYPE result = null;

            try
            {
                result = GetWorker.GetByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
        
        public V_HIS_BLOOD_TYPE GetViewById(long id, HisBloodTypeSO search)
        {
            V_HIS_BLOOD_TYPE result = null;

            try
            {
                result = GetWorker.GetViewById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }

        public V_HIS_BLOOD_TYPE GetViewByCode(string code, HisBloodTypeSO search)
        {
            V_HIS_BLOOD_TYPE result = null;

            try
            {
                result = GetWorker.GetViewByCode(code, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        public Dictionary<string, HIS_BLOOD_TYPE> GetDicByCode(HisBloodTypeSO search, CommonParam param)
        {
            Dictionary<string, HIS_BLOOD_TYPE> result = new Dictionary<string, HIS_BLOOD_TYPE>();
            try
            {
                result = GetWorker.GetDicByCode(search, param);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }

            return result;
        }
    }
}
