using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Linq;
using System.Collections.Generic;

namespace MOS.MANAGER.HisHeinApproval
{
    public class HisHeinApprovalViewFilterQuery : HisHeinApprovalViewFilter
    {
        public HisHeinApprovalViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_HEIN_APPROVAL, bool>>> listVHisHeinApprovalExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_HEIN_APPROVAL, bool>>>();



        internal HisHeinApprovalSO Query()
        {
            HisHeinApprovalSO search = new HisHeinApprovalSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisHeinApprovalExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisHeinApprovalExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisHeinApprovalExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXECUTE_TIME_FROM.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME.Value >= this.EXECUTE_TIME_FROM.Value);
                }
                if (this.EXECUTE_TIME_TO.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME.Value <= this.EXECUTE_TIME_TO.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (!String.IsNullOrEmpty(this.LEVEL_CODE__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.LEVEL_CODE != null && this.LEVEL_CODE__EXACT.Equals(o.LEVEL_CODE));
                }
                if (!String.IsNullOrEmpty(this.JOIN_5_YEAR__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.JOIN_5_YEAR != null && this.JOIN_5_YEAR__EXACT.Equals(o.JOIN_5_YEAR));
                }
                if (!String.IsNullOrEmpty(this.PAID_6_MONTH__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.PAID_6_MONTH != null && this.PAID_6_MONTH__EXACT.Equals(o.PAID_6_MONTH));
                }
                if (!String.IsNullOrEmpty(this.RIGHT_ROUTE_CODE__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.RIGHT_ROUTE_CODE != null && this.RIGHT_ROUTE_CODE__EXACT.Equals(o.RIGHT_ROUTE_CODE));
                }
                if (!String.IsNullOrEmpty(this.LIVE_AREA_CODE__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.LIVE_AREA_CODE != null && this.LIVE_AREA_CODE__EXACT.Equals(o.LIVE_AREA_CODE));
                }
                if (!String.IsNullOrEmpty(this.HEIN_MEDI_ORG_CODE__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.HEIN_MEDI_ORG_CODE != null && this.HEIN_MEDI_ORG_CODE__EXACT.Equals(o.HEIN_MEDI_ORG_CODE));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => o.HEIN_CARD_NUMBER != null && this.HEIN_CARD_NUMBER__EXACT.Equals(o.HEIN_CARD_NUMBER));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (this.BRANCH_ID.HasValue)
                {
                    listVHisHeinApprovalExpression.Add(o => o.BRANCH_ID == this.BRANCH_ID.Value);
                }
                if (this.BRANCH_IDs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => this.BRANCH_IDs.Contains(o.BRANCH_ID));
                }
                if (!String.IsNullOrEmpty(this.TREATMENT_CODE__EXACT))
                {
                    listVHisHeinApprovalExpression.Add(o => this.TREATMENT_CODE__EXACT.Equals(o.TREATMENT_CODE));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER))
                {
                    this.HEIN_CARD_NUMBER = this.HEIN_CARD_NUMBER.ToLower().Trim();
                    listVHisHeinApprovalExpression.Add(o => o.HEIN_CARD_NUMBER.ToLower().Contains(this.HEIN_CARD_NUMBER));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisHeinApprovalExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ADDRESS.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_LOGINNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.EXECUTE_USERNAME.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_APPROVAL_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_CARD_NUMBER.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_MEDI_ORG_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_MEDI_ORG_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.LEVEL_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.LIVE_AREA_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.RIGHT_ROUTE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.RIGHT_ROUTE_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.HEIN_CARD_NUMBER_PREFIXs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => this.HEIN_CARD_NUMBER_PREFIXs.Where(t => o.HEIN_CARD_NUMBER.StartsWith(t)).Any());
                }
                if (this.HEIN_CARD_NUMBER_PREFIX__NOT_INs != null)
                {
                    listVHisHeinApprovalExpression.Add(o => !this.HEIN_CARD_NUMBER_PREFIX__NOT_INs.Where(t => o.HEIN_CARD_NUMBER.StartsWith(t)).Any());
                }

                search.listVHisHeinApprovalExpression.AddRange(listVHisHeinApprovalExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisHeinApprovalExpression.Clear();
                search.listVHisHeinApprovalExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
