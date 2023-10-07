using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisEmotionlessMethod
{
    public class HisEmotionlessMethodFilterQuery : HisEmotionlessMethodFilter
    {
        public HisEmotionlessMethodFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_METHOD, bool>>> listHisEmotionlessMethodExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EMOTIONLESS_METHOD, bool>>>();

        

        internal HisEmotionlessMethodSO Query()
        {
            HisEmotionlessMethodSO search = new HisEmotionlessMethodSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisEmotionlessMethodExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisEmotionlessMethodExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisEmotionlessMethodExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisEmotionlessMethodExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisEmotionlessMethodExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisEmotionlessMethodExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.EMOTIONLESS_METHOD_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.EMOTIONLESS_METHOD_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisEmotionlessMethodExpression.AddRange(listHisEmotionlessMethodExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisEmotionlessMethodExpression.Clear();
                search.listHisEmotionlessMethodExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
