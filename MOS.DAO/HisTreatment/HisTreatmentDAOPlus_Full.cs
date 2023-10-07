using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisTreatment
{
    public partial class HisTreatmentDAO : EntityBase
    {
        public List<V_HIS_TREATMENT> GetView(HisTreatmentSO search, CommonParam param)
        {
            List<V_HIS_TREATMENT> result = new List<V_HIS_TREATMENT>();

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

        public HIS_TREATMENT GetByCode(string code, HisTreatmentSO search)
        {
            HIS_TREATMENT result = null;

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
        
        public V_HIS_TREATMENT GetViewById(long id, HisTreatmentSO search)
        {
            V_HIS_TREATMENT result = null;

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

        public V_HIS_TREATMENT GetViewByCode(string code, HisTreatmentSO search)
        {
            V_HIS_TREATMENT result = null;

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

        public Dictionary<string, HIS_TREATMENT> GetDicByCode(HisTreatmentSO search, CommonParam param)
        {
            Dictionary<string, HIS_TREATMENT> result = new Dictionary<string, HIS_TREATMENT>();
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
