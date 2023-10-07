using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialBean
{
    public class HisMaterialBeanFilterQuery : HisMaterialBeanFilter
    {
        public HisMaterialBeanFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_BEAN, bool>>> listHisMaterialBeanExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_BEAN, bool>>>();

        

        internal HisMaterialBeanSO Query()
        {
            HisMaterialBeanSO search = new HisMaterialBeanSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMaterialBeanExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMaterialBeanExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMaterialBeanExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMaterialBeanExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }                
                #endregion

                if (this.MATERIAL_ID.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.MATERIAL_ID == this.MATERIAL_ID.Value);
                }
                if (this.MEDI_STOCK_ID.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.MEDI_STOCK_ID.HasValue && o.MEDI_STOCK_ID.Value == this.MEDI_STOCK_ID.Value);
                }
                if (this.MATERIAL_IDs != null)
                {
                    search.listHisMaterialBeanExpression.Add(o => this.MATERIAL_IDs.Contains(o.MATERIAL_ID));
                }
                if (this.MATERIAL_TYPE_IDs != null)
                {
                    search.listHisMaterialBeanExpression.Add(o => this.MATERIAL_TYPE_IDs.Contains(o.TDL_MATERIAL_TYPE_ID));
                }
                if (this.DETACH_KEYs != null)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.DETACH_KEY != null && this.DETACH_KEYs.Contains(o.DETACH_KEY));
                }
                if (this.MATERIAL_IS_ACTIVE.HasValue)
                {
                    search.listHisMaterialBeanExpression.Add(o => o.TDL_MATERIAL_IS_ACTIVE == this.MATERIAL_IS_ACTIVE);
                }

                search.listHisMaterialBeanExpression.AddRange(listHisMaterialBeanExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMaterialBeanExpression.Clear();
                search.listHisMaterialBeanExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
