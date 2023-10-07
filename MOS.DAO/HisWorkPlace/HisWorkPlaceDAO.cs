using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisWorkPlace
{
    public partial class HisWorkPlaceDAO : EntityBase
    {
        private HisWorkPlaceGet GetWorker
        {
            get
            {
                return (HisWorkPlaceGet)Worker.Get<HisWorkPlaceGet>();
            }
        }
        public List<HIS_WORK_PLACE> Get(HisWorkPlaceSO search, CommonParam param)
        {
            List<HIS_WORK_PLACE> result = new List<HIS_WORK_PLACE>();
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

        public HIS_WORK_PLACE GetById(long id, HisWorkPlaceSO search)
        {
            HIS_WORK_PLACE result = null;
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
