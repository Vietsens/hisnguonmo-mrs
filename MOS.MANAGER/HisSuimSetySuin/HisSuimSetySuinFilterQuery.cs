using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSuimSetySuin
{
    public class HisSuimSetySuinFilterQuery : HisSuimSetySuinFilter
    {
        public HisSuimSetySuinFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SUIM_SETY_SUIN, bool>>> listHisSuimSetySuinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUIM_SETY_SUIN, bool>>>();

        

        internal HisSuimSetySuinSO Query()
        {
            HisSuimSetySuinSO search = new HisSuimSetySuinSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisSuimSetySuinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSuimSetySuinExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSuimSetySuinExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSuimSetySuinExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SUIM_INDEX_ID.HasValue)
                {
                    listHisSuimSetySuinExpression.Add(o => o.SUIM_INDEX_ID == this.SUIM_INDEX_ID.Value);
                }

                search.listHisSuimSetySuinExpression.AddRange(listHisSuimSetySuinExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSuimSetySuinExpression.Clear();
                search.listHisSuimSetySuinExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
