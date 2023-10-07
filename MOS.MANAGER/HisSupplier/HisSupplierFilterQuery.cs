using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSupplier
{
    public class HisSupplierFilterQuery : HisSupplierFilter
    {
        public HisSupplierFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SUPPLIER, bool>>> listHisSupplierExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SUPPLIER, bool>>>();

        

        internal HisSupplierSO Query()
        {
            HisSupplierSO search = new HisSupplierSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisSupplierExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisSupplierExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisSupplierExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisSupplierExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisSupplierExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisSupplierExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.ADDRESS.ToLower().Contains(this.KEY_WORD) ||
                        o.PHONE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SUPPLIER_SHORT_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisSupplierExpression.AddRange(listHisSupplierExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisSupplierExpression.Clear();
                search.listHisSupplierExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
