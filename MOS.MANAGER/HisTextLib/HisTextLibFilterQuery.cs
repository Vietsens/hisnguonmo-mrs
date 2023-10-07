using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisTextLib
{
    public class HisTextLibFilterQuery : HisTextLibFilter
    {
        public HisTextLibFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_TEXT_LIB, bool>>> listHisTextLibExpression = new List<System.Linq.Expressions.Expression<Func<HIS_TEXT_LIB, bool>>>();

        

        internal HisTextLibSO Query()
        {
            HisTextLibSO search = new HisTextLibSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisTextLibExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisTextLibExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisTextLibExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisTextLibExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisTextLibExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisTextLibExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.TITLE.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (!string.IsNullOrWhiteSpace(this.HASHTAG))
                {
                    this.HASHTAG = string.Format(",{0},", this.HASHTAG.ToLower().Trim());
                    listHisTextLibExpression.Add(o => o.HASHTAG != null && ("," + o.HASHTAG.ToLower() + ",").Contains(this.HASHTAG));
                }
                if (this.HASHTAGs != null)
                {
                    foreach (string s in this.HASHTAGs)
                    {
                        string t = string.Format(",{0},", s.ToLower().Trim());
                        listHisTextLibExpression.Add(o => o.HASHTAG != null && ("," + o.HASHTAG.ToLower() + ",").Contains(t));
                    }
                }
                if (this.CAN_VIEW.HasValue && this.CAN_VIEW.Value)
                {
                    string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();

                    listHisTextLibExpression.Add(o => o.CREATOR == loginName || o.IS_PUBLIC.HasValue && o.IS_PUBLIC.Value == ManagerConstant.IS_TRUE);
                }
                if (this.CAN_VIEW.HasValue && !this.CAN_VIEW.Value)
                {
                    string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();

                    listHisTextLibExpression.Add(o => o.CREATOR != loginName && (!o.IS_PUBLIC.HasValue || o.IS_PUBLIC.Value == ManagerConstant.IS_TRUE));
                }

                search.listHisTextLibExpression.AddRange(listHisTextLibExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisTextLibExpression.Clear();
                search.listHisTextLibExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
