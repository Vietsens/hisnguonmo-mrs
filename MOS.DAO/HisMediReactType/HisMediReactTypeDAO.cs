using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediReactType
{
    public partial class HisMediReactTypeDAO : EntityBase
    {
        private HisMediReactTypeGet GetWorker
        {
            get
            {
                return (HisMediReactTypeGet)Worker.Get<HisMediReactTypeGet>();
            }
        }
        public List<HIS_MEDI_REACT_TYPE> Get(HisMediReactTypeSO search, CommonParam param)
        {
            List<HIS_MEDI_REACT_TYPE> result = new List<HIS_MEDI_REACT_TYPE>();
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

        public HIS_MEDI_REACT_TYPE GetById(long id, HisMediReactTypeSO search)
        {
            HIS_MEDI_REACT_TYPE result = null;
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
