using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using Inventec.Common.Repository;
using Inventec.Core;
using System;
using System.Collections.Generic;

namespace MOS.DAO.HisHeinApproval
{
    public partial class HisHeinApprovalDAO : EntityBase
    {
        private HisHeinApprovalGet GetWorker
        {
            get
            {
                return (HisHeinApprovalGet)Worker.Get<HisHeinApprovalGet>();
            }
        }
        public List<HIS_HEIN_APPROVAL> Get(HisHeinApprovalSO search, CommonParam param)
        {
            List<HIS_HEIN_APPROVAL> result = new List<HIS_HEIN_APPROVAL>();
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

        public HIS_HEIN_APPROVAL GetById(long id, HisHeinApprovalSO search)
        {
            HIS_HEIN_APPROVAL result = null;
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
