using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisRegisterReq
{
    public class HisRegisterReqFilterQuery : HisRegisterReqFilter
    {
        public HisRegisterReqFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_REQ, bool>>> listHisRegisterReqExpression = new List<System.Linq.Expressions.Expression<Func<HIS_REGISTER_REQ, bool>>>();

        

        internal HisRegisterReqSO Query()
        {
            HisRegisterReqSO search = new HisRegisterReqSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisRegisterReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisRegisterReqExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisRegisterReqExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisRegisterReqExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisRegisterReqExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.REGISTER_GATE_ID.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.REGISTER_GATE_ID == this.REGISTER_GATE_ID.Value);
                }
                if (this.REGISTER_GATE_IDs != null)
                {
                    listHisRegisterReqExpression.Add(o => this.REGISTER_GATE_IDs.Contains(o.REGISTER_GATE_ID));
                }
                if (this.PATIENT_ID.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.PATIENT_ID.HasValue && o.PATIENT_ID.Value == this.PATIENT_ID.Value);
                }
                if (this.REGISTER_TIME_FROM.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.REGISTER_TIME >= this.REGISTER_TIME_FROM.Value);
                }
                if (this.REGISTER_TIME_TO.HasValue)
                {
                    listHisRegisterReqExpression.Add(o => o.REGISTER_TIME <= this.REGISTER_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CARD_CODE__EXACT))
                {
                    listHisRegisterReqExpression.Add(o => o.CARD_CODE == this.CARD_CODE__EXACT);
                }
                if (!String.IsNullOrEmpty(this.SERVICE_CODE__EXACT))
                {
                    listHisRegisterReqExpression.Add(o => o.SERVICE_CODE == this.SERVICE_CODE__EXACT);
                }
                
                search.listHisRegisterReqExpression.AddRange(listHisRegisterReqExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisRegisterReqExpression.Clear();
                search.listHisRegisterReqExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
