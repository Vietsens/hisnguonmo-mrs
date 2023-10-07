using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisDataStore
{
    public class HisDataStoreViewFilterQuery : HisDataStoreViewFilter
    {
        public HisDataStoreViewFilterQuery()
            : base()
        {

        }
        
        internal List<System.Linq.Expressions.Expression<Func<V_HIS_DATA_STORE, bool>>> listVHisDataStoreExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_DATA_STORE, bool>>>();

        

        internal HisDataStoreSO Query()
        {
            HisDataStoreSO search = new HisDataStoreSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisDataStoreExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisDataStoreExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisDataStoreExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisDataStoreExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisDataStoreExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisDataStoreExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.DATA_STORE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DATA_STORE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ROOM_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.DEPARTMENT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.G_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PARENT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PARENT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }
                if (this.IsTree.HasValue && this.IsTree.Value)
                {
                    listVHisDataStoreExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
                }

                search.listVHisDataStoreExpression.AddRange(listVHisDataStoreExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisDataStoreExpression.Clear();
                search.listVHisDataStoreExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
