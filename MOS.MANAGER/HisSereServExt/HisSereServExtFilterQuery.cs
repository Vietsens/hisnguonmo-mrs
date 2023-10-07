using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServExt
{
    public class HisSereServExtFilterQuery : HisSereServExtFilter
    {
        public HisSereServExtFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_EXT, bool>>> listHisSereServExtExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERE_SERV_EXT, bool>>>();

        internal HisSereServExtSO Query()
        {
            HisSereServExtSO search = new HisSereServExtSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisSereServExtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisSereServExtExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisSereServExtExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisSereServExtExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisSereServExtExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (this.SERE_SERV_ID.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.SERE_SERV_ID == this.SERE_SERV_ID.Value);
                }
                if (this.SERE_SERV_IDs != null)
                {
                    listHisSereServExtExpression.Add(o => this.SERE_SERV_IDs.Contains(o.SERE_SERV_ID));
                }
                if (this.BEGIN_TIME_FROM.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.BEGIN_TIME.Value >= this.BEGIN_TIME_FROM.Value);
                }
                if (this.BEGIN_TIME_TO.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.BEGIN_TIME.Value <= this.BEGIN_TIME_TO.Value);
                }
                if (this.END_TIME_FROM.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.END_TIME.Value >= this.END_TIME_FROM.Value);
                }
                if (this.END_TIME_TO.HasValue)
                {
                    listHisSereServExtExpression.Add(o => o.END_TIME.Value <= this.END_TIME_TO.Value);
                }
                if (this.IS_NOT_FEE.HasValue)
                {
                    if (this.IS_NOT_FEE.Value)
                    {
                        listHisSereServExtExpression.Add(o => !o.IS_FEE.HasValue);
                    }
                    else
                    {
                        listHisSereServExtExpression.Add(o => o.IS_FEE == 1);
                    }
                }
                if (this.IS_NOT_GATHER_DATA.HasValue)
                {
                    if (this.IS_NOT_GATHER_DATA.Value)
                    {
                        listHisSereServExtExpression.Add(o => !o.IS_GATHER_DATA.HasValue);
                    }
                    else
                    {
                        listHisSereServExtExpression.Add(o => o.IS_GATHER_DATA == 1);
                    }
                }

                search.listHisSereServExtExpression.AddRange(listHisSereServExtExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSereServExtExpression.Clear();
                search.listHisSereServExtExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
