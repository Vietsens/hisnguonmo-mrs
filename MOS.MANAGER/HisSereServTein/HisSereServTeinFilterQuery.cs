using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServTein
{
    public class HisSereServTeinFilterQuery : HisSereServTeinFilter
    {
        public HisSereServTeinFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEIN, bool>>> listHisSereServTeinExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_TEIN, bool>>>();

        

        internal HisSereServTeinSO Query()
        {
            HisSereServTeinSO search = new HisSereServTeinSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisSereServTeinExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisSereServTeinExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisSereServTeinExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisSereServTeinExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion
                if (this.SERE_SERV_IDs != null)
                {
                    search.listHisSereServTeinExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.TEST_INDEX_ID.HasValue)
                {
                    search.listHisSereServTeinExpression.Add(o => o.TEST_INDEX_ID == this.TEST_INDEX_ID.Value);
                }

                search.listHisSereServTeinExpression.AddRange(listHisSereServTeinExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServTeinExpression.Clear();
                search.listHisSereServTeinExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
