using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinApproval
{
    partial class HisHeinApprovalGet : BusinessBase
    {
        internal HisHeinApprovalGet()
            : base()
        {

        }

        internal HisHeinApprovalGet(CommonParam paramGet)
            : base(paramGet)
        {

        }

        internal List<HIS_HEIN_APPROVAL> Get(HisHeinApprovalFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinApprovalDAO.Get(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_HEIN_APPROVAL> GetByTreatmentId(long treatmentId)
        {
            try
            {
                HisHeinApprovalFilterQuery filter = new HisHeinApprovalFilterQuery();
                filter.TREATMENT_ID = treatmentId;
                //filter.IS_DELETE = false;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_HEIN_APPROVAL> GetByTreatmentIds(List<long> treatmentIds)
        {
            if (IsNotNullOrEmpty(treatmentIds))
            {
                HisHeinApprovalFilterQuery filter = new HisHeinApprovalFilterQuery();
                filter.TREATMENT_IDs = treatmentIds;
                return this.Get(filter);
            }
            return null;
        }

        internal HIS_HEIN_APPROVAL GetById(long id)
        {
            try
            {
                return GetById(id, new HisHeinApprovalFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal HIS_HEIN_APPROVAL GetById(long id, HisHeinApprovalFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinApprovalDAO.GetById(id, filter.Query());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<HIS_HEIN_APPROVAL> GetExistedData(HIS_HEIN_APPROVAL data, long treatmentId)
        {
            try
            {
                HisHeinApprovalFilterQuery filter = new HisHeinApprovalFilterQuery();
                filter.HEIN_CARD_NUMBER__EXACT = data.HEIN_CARD_NUMBER;
                filter.HEIN_MEDI_ORG_CODE__EXACT = data.HEIN_MEDI_ORG_CODE;
                filter.LIVE_AREA_CODE__EXACT = data.LIVE_AREA_CODE;
                filter.RIGHT_ROUTE_CODE__EXACT = data.RIGHT_ROUTE_CODE;
                filter.PAID_6_MONTH__EXACT = data.PAID_6_MONTH;
                filter.JOIN_5_YEAR__EXACT = data.JOIN_5_YEAR;
                filter.LEVEL_CODE__EXACT = data.LEVEL_CODE;
                filter.TREATMENT_ID = treatmentId;
                return this.Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal List<V_HIS_HEIN_APPROVAL> GetView(HisHeinApprovalViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinApprovalDAO.GetView(filter.Query(), param);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_HEIN_APPROVAL GetViewById(long id)
        {
            try
            {
                return GetViewById(id, new HisHeinApprovalViewFilterQuery());
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }

        internal V_HIS_HEIN_APPROVAL GetViewById(long id, HisHeinApprovalViewFilterQuery filter)
        {
            try
            {
                return DAOWorker.HisHeinApprovalDAO.GetViewById(id, filter.Query());
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
