using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMaterialBean
{
    public partial class HisMaterialBeanDAO : EntityBase
    {
        private HisMaterialBeanGet GetWorker
        {
            get
            {
                return (HisMaterialBeanGet)Worker.Get<HisMaterialBeanGet>();
            }
        }
        public List<HIS_MATERIAL_BEAN> Get(HisMaterialBeanSO search, CommonParam param)
        {
            List<HIS_MATERIAL_BEAN> result = new List<HIS_MATERIAL_BEAN>();
            try
            {
                result = GetWorker.Get(search, param);
            }
            catch (Exception ex)
            {
                param.HasException = true;
                Inventec.Common.Logging.LogSystem.Error(ex);
                result.Clear();
            }
            return result;
        }

        public HIS_MATERIAL_BEAN GetById(long id, HisMaterialBeanSO search)
        {
            HIS_MATERIAL_BEAN result = null;
            try
            {
                result = GetWorker.GetById(id, search);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }

            return result;
        }
    }
}
