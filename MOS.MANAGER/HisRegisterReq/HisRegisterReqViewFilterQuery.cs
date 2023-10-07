using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterReq
{
    public class HisRegisterReqViewFilterQuery : HisRegisterReqViewFilter
    {
        public HisRegisterReqViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_REGISTER_REQ, bool>>> listVHisRegisterReqExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_REGISTER_REQ, bool>>>();

        

        internal HisRegisterReqSO Query()
        {
            HisRegisterReqSO search = new HisRegisterReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listVHisRegisterReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisRegisterReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisRegisterReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisRegisterReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listVHisRegisterReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.REGISTER_GATE_ID.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.REGISTER_GATE_ID == this.REGISTER_GATE_ID.Value);
                }
                if (this.PATIENT_ID.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.PATIENT_ID.HasValue && o.PATIENT_ID.Value == this.PATIENT_ID.Value);
                }
                if (this.REGISTER_TIME_FROM.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.REGISTER_TIME >= this.REGISTER_TIME_FROM.Value);
                }
                if (this.REGISTER_TIME_TO.HasValue)
                {
                    listVHisRegisterReqExpression.Add(o => o.REGISTER_TIME <= this.REGISTER_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CARD_CODE__EXACT))
                {
                    listVHisRegisterReqExpression.Add(o => o.CARD_CODE == this.CARD_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.SERVICE_CODE__EXACT))
                {
                    listVHisRegisterReqExpression.Add(o => o.SERVICE_CODE == this.SERVICE_CODE__EXACT);
                }

                search.listVHisRegisterReqExpression.AddRange(listVHisRegisterReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisRegisterReqExpression.Clear();
                search.listVHisRegisterReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
