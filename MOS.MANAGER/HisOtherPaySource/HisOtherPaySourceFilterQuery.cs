using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisOtherPaySource
{
    public class HisOtherPaySourceFilterQuery : HisOtherPaySourceFilter
    {
        public HisOtherPaySourceFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_OTHER_PAY_SOURCE, bool>>> listHisOtherPaySourceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_OTHER_PAY_SOURCE, bool>>>();

        

        internal HisOtherPaySourceSO Query()
        {
            HisOtherPaySourceSO search = new HisOtherPaySourceSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisOtherPaySourceExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisOtherPaySourceExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisOtherPaySourceExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisOtherPaySourceExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisOtherPaySourceExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisOtherPaySourceExpression.Add(o =>
                        o.OTHER_PAY_SOURCE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.OTHER_PAY_SOURCE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisOtherPaySourceExpression.AddRange(listHisOtherPaySourceExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisOtherPaySourceExpression.Clear();
                search.listHisOtherPaySourceExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
