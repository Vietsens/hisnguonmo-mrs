using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisService
{
    public class HisServiceFilterQuery : HisServiceFilter
    {
        public HisServiceFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_SERVICE, bool>>> listHisServiceExpression = new List<System.Linq.Expressions.Expression<Func<HIS_SERVICE, bool>>>();

        internal HisServiceSO Query()
        {
            HisServiceSO search = new HisServiceSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisServiceExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisServiceExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisServiceExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisServiceExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisServiceExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_SERVICE_BHYT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.HEIN_SERVICE_BHYT_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.SERVICE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.SPECIALITY_CODE.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.SERVICE_TYPE_ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.SERVICE_TYPE_ID == this.SERVICE_TYPE_ID.Value);
                }
                if (this.SERVICE_UNIT_ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.SERVICE_UNIT_ID == this.SERVICE_UNIT_ID.Value);
                }
                if (this.PTTT_GROUP_ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.PTTT_GROUP_ID.HasValue && o.PTTT_GROUP_ID.Value == this.PTTT_GROUP_ID.Value);
                }
                if (this.PTTT_GROUP_IDs != null)
                {
                    search.listHisServiceExpression.Add(o => o.PTTT_GROUP_ID.HasValue && this.PTTT_GROUP_IDs.Contains(o.PTTT_GROUP_ID.Value));
                }
                if (this.ICD_CM_ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.ICD_CM_ID.HasValue && o.ICD_CM_ID.Value == this.ICD_CM_ID.Value);
                }
                if (this.ICD_CM_IDs != null)
                {
                    search.listHisServiceExpression.Add(o => o.ICD_CM_ID.HasValue && this.ICD_CM_IDs.Contains(o.ICD_CM_ID.Value));
                }
                if (!String.IsNullOrEmpty(this.SERVICE_CODE__EXACT))
                {
                    search.listHisServiceExpression.Add(o => o.SERVICE_CODE == this.SERVICE_CODE__EXACT);
                }
                if (this.ID_NOT__EQUAL.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.ID != this.ID_NOT__EQUAL.Value);
                }
                if (this.PARENT_ID.HasValue)
                {
                    search.listHisServiceExpression.Add(o => o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.PARENT_ID.Value);
                }
                if (this.SERVICE_CODEs != null)
                {
                    search.listHisServiceExpression.Add(o => this.SERVICE_CODEs.Contains(o.SERVICE_CODE));
                }

                search.listHisServiceExpression.AddRange(listHisServiceExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisServiceExpression.Clear();
                search.listHisServiceExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
