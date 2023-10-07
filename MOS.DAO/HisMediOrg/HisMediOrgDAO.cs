using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisMediOrg
{
    public partial class HisMediOrgDAO : EntityBase
    {
        private HisMediOrgGet GetWorker
        {
            get
            {
                return (HisMediOrgGet)Worker.Get<HisMediOrgGet>();
            }
        }
        public List<HIS_MEDI_ORG> Get(HisMediOrgSO search, CommonParam param)
        {
            List<HIS_MEDI_ORG> result = new List<HIS_MEDI_ORG>();
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

        public HIS_MEDI_ORG GetById(long id, HisMediOrgSO search)
        {
            HIS_MEDI_ORG result = null;
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
