using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDataStore
{
    public class HisDataStoreFilterQuery : HisDataStoreFilter
    {
        public HisDataStoreFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_DATA_STORE, bool>>> listHisDataStoreExpression = new List<System.Linq.Expressions.Expression<Func<HIS_DATA_STORE, bool>>>();

        

        internal HisDataStoreSO Query()
        {
            HisDataStoreSO search = new HisDataStoreSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisDataStoreExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisDataStoreExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisDataStoreExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisDataStoreExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.IsTree.HasValue && this.IsTree.Value)
                {
                    listHisDataStoreExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
                }

                if (this.PARENT_ID.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.PARENT_ID.Value);
                }
                if (this.ROOM_ID.HasValue)
                {
                    listHisDataStoreExpression.Add(o => o.ROOM_ID == this.ROOM_ID.Value);
                }
                if (this.ROOM_IDs != null)
                {
                    listHisDataStoreExpression.Add(o => this.ROOM_IDs.Contains(o.ROOM_ID));
                }

                search.listHisDataStoreExpression.AddRange(listHisDataStoreExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisDataStoreExpression.Clear();
                search.listHisDataStoreExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
