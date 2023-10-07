using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePackage
{
    public class HisServicePackageViewFilterQuery : HisServicePackageViewFilter
    {
        public HisServicePackageViewFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PACKAGE, bool>>> listVHisServicePackageExpression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERVICE_PACKAGE, bool>>>();

        

        internal HisServicePackageSO Query()
        {
            HisServicePackageSO search = new HisServicePackageSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listVHisServicePackageExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listVHisServicePackageExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listVHisServicePackageExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listVHisServicePackageExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_ATTACH_ID.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.SERVICE_ATTACH_ID == this.SERVICE_ATTACH_ID.Value);
                }
                if (this.SERVICE_ID.HasValue)
                {
                    search.listVHisServicePackageExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listVHisServicePackageExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ATTACH_SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.ATTACH_SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ATTACH_SERVICE_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.ATTACH_SERVICE_TYPE_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }
                search.listVHisServicePackageExpression.AddRange(listVHisServicePackageExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisServicePackageExpression.Clear();
                search.listVHisServicePackageExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
