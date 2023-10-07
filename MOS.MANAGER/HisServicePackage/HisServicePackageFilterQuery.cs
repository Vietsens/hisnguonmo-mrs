using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisServicePackage
{
    public class HisServicePackageFilterQuery : HisServicePackageFilter
    {
        public HisServicePackageFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PACKAGE, bool>>> listHisServicePackageExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE_PACKAGE, bool>>>();

        

        internal HisServicePackageSO Query()
        {
            HisServicePackageSO search = new HisServicePackageSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisServicePackageExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisServicePackageExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisServicePackageExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisServicePackageExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion
                if (this.SERVICE_ID.HasValue)
                {
                    search.listHisServicePackageExpression.Add(o => o.SERVICE_ID == this.SERVICE_ID.Value);
                }

                search.listHisServicePackageExpression.AddRange(listHisServicePackageExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServicePackageExpression.Clear();
                search.listHisServicePackageExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
