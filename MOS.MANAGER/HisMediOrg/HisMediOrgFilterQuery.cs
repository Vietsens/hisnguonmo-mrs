using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMediOrg
{
    public class HisMediOrgFilterQuery : HisMediOrgFilter
    {
        public HisMediOrgFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MEDI_ORG, bool>>> listHisMediOrgExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MEDI_ORG, bool>>>();



        internal HisMediOrgSO Query()
        {
            HisMediOrgSO search = new HisMediOrgSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisMediOrgExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisMediOrgExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisMediOrgExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisMediOrgExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisMediOrgExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisMediOrgExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisMediOrgExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_ORG_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MEDI_ORG_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.LEVEL_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.PROVINCE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.RANK_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }

                search.listHisMediOrgExpression.AddRange(listHisMediOrgExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMediOrgExpression.Clear();
                search.listHisMediOrgExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
