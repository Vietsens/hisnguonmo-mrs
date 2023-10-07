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
    public class HisHeinApprovalFilterQuery : HisHeinApprovalFilter
    {
        public HisHeinApprovalFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_HEIN_APPROVAL, bool>>> listHisHeinApprovalExpression = new List<System.Linq.Expressions.Expression<Func<HIS_HEIN_APPROVAL, bool>>>();



        internal HisHeinApprovalSO Query()
        {
            HisHeinApprovalSO search = new HisHeinApprovalSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisHeinApprovalExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisHeinApprovalExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisHeinApprovalExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisHeinApprovalExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXECUTE_TIME_FROM.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME.Value >= this.EXECUTE_TIME_FROM.Value);
                }
                if (this.EXECUTE_TIME_TO.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.EXECUTE_TIME.HasValue && o.EXECUTE_TIME.Value <= this.EXECUTE_TIME_TO.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisHeinApprovalExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (!String.IsNullOrEmpty(this.LEVEL_CODE__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.LEVEL_CODE != null && this.LEVEL_CODE__EXACT.Equals(o.LEVEL_CODE));
                }
                if (!String.IsNullOrEmpty(this.JOIN_5_YEAR__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.JOIN_5_YEAR != null && this.JOIN_5_YEAR__EXACT.Equals(o.JOIN_5_YEAR));
                }
                if (!String.IsNullOrEmpty(this.PAID_6_MONTH__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.PAID_6_MONTH != null && this.PAID_6_MONTH__EXACT.Equals(o.PAID_6_MONTH));
                }
                if (!String.IsNullOrEmpty(this.RIGHT_ROUTE_CODE__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.RIGHT_ROUTE_CODE != null && this.RIGHT_ROUTE_CODE__EXACT.Equals(o.RIGHT_ROUTE_CODE));
                }
                if (!String.IsNullOrEmpty(this.LIVE_AREA_CODE__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.LIVE_AREA_CODE != null && this.LIVE_AREA_CODE__EXACT.Equals(o.LIVE_AREA_CODE));
                }
                if (!String.IsNullOrEmpty(this.HEIN_MEDI_ORG_CODE__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.HEIN_MEDI_ORG_CODE != null && this.HEIN_MEDI_ORG_CODE__EXACT.Equals(o.HEIN_MEDI_ORG_CODE));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER__EXACT))
                {
                    listHisHeinApprovalExpression.Add(o => o.HEIN_CARD_NUMBER != null && this.HEIN_CARD_NUMBER__EXACT.Equals(o.HEIN_CARD_NUMBER));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listHisHeinApprovalExpression.Add(o => o.TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listHisHeinApprovalExpression.Add(o => this.TREATMENT_IDs.Contains(o.TREATMENT_ID));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER))
                {
                    this.HEIN_CARD_NUMBER = this.HEIN_CARD_NUMBER.ToLower().Trim();
                    listHisHeinApprovalExpression.Add(o => o.HEIN_CARD_NUMBER.ToLower().Contains(this.HEIN_CARD_NUMBER));
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisHeinApprovalExpression.Add(o =>
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
                    listHisHeinApprovalExpression.Add(o => this.HEIN_CARD_NUMBER_PREFIXs.Where(t => o.HEIN_CARD_NUMBER.StartsWith(t)).Any());
                }
                if (this.HEIN_CARD_NUMBER_PREFIX__NOT_INs != null)
                {
                    listHisHeinApprovalExpression.Add(o => !this.HEIN_CARD_NUMBER_PREFIX__NOT_INs.Where(t => o.HEIN_CARD_NUMBER.StartsWith(t)).Any());
                }

                search.listHisHeinApprovalExpression.AddRange(listHisHeinApprovalExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisHeinApprovalExpression.Clear();
                search.listHisHeinApprovalExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
