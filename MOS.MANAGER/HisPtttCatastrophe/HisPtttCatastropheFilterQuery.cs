using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisPtttCatastrophe
{
    public class HisPtttCatastropheFilterQuery : HisPtttCatastropheFilter
    {
        public HisPtttCatastropheFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_PTTT_CATASTROPHE, bool>>> listHisPtttCatastropheExpression = new List<System.Linq.Expressions.Expression<Func<HIS_PTTT_CATASTROPHE, bool>>>();

        

        internal HisPtttCatastropheSO Query()
        {
            HisPtttCatastropheSO search = new HisPtttCatastropheSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.ID == this.ID.Value);
                }
				if (this.IDs != null)
                {
                    listHisPtttCatastropheExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisPtttCatastropheExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisPtttCatastropheExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisPtttCatastropheExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisPtttCatastropheExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisPtttCatastropheExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisPtttCatastropheExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.PTTT_CATASTROPHE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PTTT_CATASTROPHE_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisPtttCatastropheExpression.AddRange(listHisPtttCatastropheExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisPtttCatastropheExpression.Clear();
                search.listHisPtttCatastropheExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
