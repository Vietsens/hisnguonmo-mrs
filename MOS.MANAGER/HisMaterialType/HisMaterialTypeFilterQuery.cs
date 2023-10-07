using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisMaterialType
{
    public class HisMaterialTypeFilterQuery : HisMaterialTypeFilter
    {
        public HisMaterialTypeFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_TYPE, bool>>> listHisMaterialTypeExpression = new List<System.Linq.Expressions.Expression<Func<HIS_MATERIAL_TYPE, bool>>>();

        

        internal HisMaterialTypeSO Query()
        {
            HisMaterialTypeSO search = new HisMaterialTypeSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    search.listHisMaterialTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    search.listHisMaterialTypeExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    search.listHisMaterialTypeExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    search.listHisMaterialTypeExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    search.listHisMaterialTypeExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IS_LEAF.HasValue)
                {
                    if (this.IS_LEAF.Value)
                    {
                        search.listHisMaterialTypeExpression.Add(o => o.IS_LEAF.HasValue && o.IS_LEAF.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        search.listHisMaterialTypeExpression.Add(o => !o.IS_LEAF.HasValue || o.IS_LEAF.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.IS_STOP_IMP.HasValue)
                {
                    if (this.IS_STOP_IMP.Value)
                    {
                        search.listHisMaterialTypeExpression.Add(o => o.IS_STOP_IMP.HasValue && o.IS_STOP_IMP.Value == ManagerConstant.IS_TRUE);
                    }
                    else
                    {
                        search.listHisMaterialTypeExpression.Add(o => !o.IS_STOP_IMP.HasValue || o.IS_STOP_IMP.Value != ManagerConstant.IS_TRUE);
                    }
                }
                if (this.IDs != null)
                {
                    search.listHisMaterialTypeExpression.Add(o => this.IDs.Contains(o.ID));
                }

                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisMaterialTypeExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.MATERIAL_TYPE_NAME.ToLower().Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD) ||
                        o.NATIONAL_NAME.ToLower().Contains(this.KEY_WORD)
                        );
                }

                if (this.IsTree.HasValue && this.IsTree.Value)
                {
                    listHisMaterialTypeExpression.Add(o => (!o.PARENT_ID.HasValue && !this.Node.HasValue) || (o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.Node.Value));
                }
                if (this.PARENT_ID.HasValue)
                {
                    listHisMaterialTypeExpression.Add(o => o.PARENT_ID.HasValue && o.PARENT_ID.Value == this.PARENT_ID.Value);
                }
                if (this.MANUFACTURER_ID.HasValue)
                {
                    listHisMaterialTypeExpression.Add(o => o.MANUFACTURER_ID.HasValue && o.MANUFACTURER_ID.Value == this.MANUFACTURER_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.CN_WORD))
                {
                    this.CN_WORD = this.CN_WORD.ToLower().Trim();
                    listHisMaterialTypeExpression.Add(o => o.MATERIAL_TYPE_CODE.ToLower().Contains(this.CN_WORD) || o.MATERIAL_TYPE_NAME.ToLower().Contains(this.CN_WORD));
                }
                if (this.IS_STENT.HasValue && this.IS_STENT.Value)
                {
                    listHisMaterialTypeExpression.Add(o => o.IS_STENT.HasValue && o.IS_STENT == ManagerConstant.IS_TRUE);
                }
                if (this.IS_STENT.HasValue && !this.IS_STENT.Value)
                {
                    listHisMaterialTypeExpression.Add(o => !o.IS_STENT.HasValue || o.IS_STENT != ManagerConstant.IS_TRUE);
                }
                if (this.MEMA_GROUP_ID.HasValue)
                {
                    listHisMaterialTypeExpression.Add(o => o.MEMA_GROUP_ID.HasValue && o.MEMA_GROUP_ID.Value == this.MEMA_GROUP_ID.Value);
                }

                search.listHisMaterialTypeExpression.AddRange(listHisMaterialTypeExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisMaterialTypeExpression.Clear();
                search.listHisMaterialTypeExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
