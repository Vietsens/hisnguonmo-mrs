using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisIcd
{
    public class HisIcdFilterQuery : HisIcdFilter
    {
        public HisIcdFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_ICD, bool>>> listHisIcdExpression = new List<System.Linq.Expressions.Expression<Func<HIS_ICD, bool>>>();

        

        internal HisIcdSO Query()
        {
            HisIcdSO search = new HisIcdSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisIcdExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisIcdExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisIcdExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisIcdExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisIcdExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisIcdExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisIcdExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisIcdExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisIcdExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisIcdExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisIcdExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_NAME_COMMON.ToLower().Contains(this.KEY_WORD) ||
                        o.ICD_NAME_EN.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.ICD_GROUP_ID.HasValue)
                {
                    listHisIcdExpression.Add(o => o.ICD_GROUP_ID.HasValue && o.ICD_GROUP_ID.Value == this.ICD_GROUP_ID.Value);
                }
                if (this.ICD_CODEs != null)
                {
                    listHisIcdExpression.Add(o => this.ICD_CODEs.Contains(o.ICD_CODE));
                }

                search.listHisIcdExpression.AddRange(listHisIcdExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisIcdExpression.Clear();
                search.listHisIcdExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
