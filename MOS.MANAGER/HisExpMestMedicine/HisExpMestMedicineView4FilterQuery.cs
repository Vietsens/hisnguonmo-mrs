using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExpMestMedicine
{
    public class HisExpMestMedicineView4FilterQuery : HisExpMestMedicineView4Filter
    {
        public HisExpMestMedicineView4FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_4, bool>>> listVHisExpMestMedicine4Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_EXP_MEST_MEDICINE_4, bool>>>();

        internal HisExpMestMedicineSO Query()
        {
            HisExpMestMedicineSO search = new HisExpMestMedicineSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.EXP_MEST_ID.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.EXP_MEST_ID == this.EXP_MEST_ID.Value);
                }
                if (this.EXP_MEST_IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.EXP_MEST_ID != null && this.EXP_MEST_IDs.Contains(o.EXP_MEST_ID.Value));
                }
                if (this.MEDICINE_IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.MEDICINE_ID != null && this.MEDICINE_IDs.Contains(o.MEDICINE_ID.Value));
                }
                if (this.MEDICINE_ID.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => this.MEDICINE_ID.Value == o.MEDICINE_ID);
                }
                if (this.TDL_MEDICINE_TYPE_ID.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.TDL_MEDICINE_TYPE_ID == this.TDL_MEDICINE_TYPE_ID.Value);
                }
                if (this.TDL_MEDICINE_TYPE_IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.TDL_MEDICINE_TYPE_ID != null && this.TDL_MEDICINE_TYPE_IDs.Contains(o.TDL_MEDICINE_TYPE_ID.Value));
                }
                if (this.EXP_TIME_FROM.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.EXP_TIME.Value >= this.EXP_TIME_FROM.Value);
                }
                if (this.EXP_TIME_TO.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.EXP_TIME.Value <= this.EXP_TIME_TO.Value);
                }
                if (this.EXP_MEST_TYPE_ID.HasValue)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.EXP_MEST_TYPE_ID == this.EXP_MEST_TYPE_ID.Value);
                }
                if (this.EXP_MEST_TYPE_IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => this.EXP_MEST_TYPE_IDs.Contains(o.EXP_MEST_TYPE_ID));
                }
                if (this.TDL_SERVICE_REQ_IDs != null)
                {
                    listVHisExpMestMedicine4Expression.Add(o => o.TDL_SERVICE_REQ_ID.HasValue && this.TDL_SERVICE_REQ_IDs.Contains(o.TDL_SERVICE_REQ_ID.Value));
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower();
                    listVHisExpMestMedicine4Expression.Add(o => o.APP_CREATOR.Contains(this.KEY_WORD) ||
                        o.APP_MODIFIER.Contains(this.KEY_WORD) ||
                        o.CREATOR.Contains(this.KEY_WORD) ||
                        o.DESCRIPTION.Contains(this.KEY_WORD) ||
                        o.TUTORIAL.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_CODE.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.EXP_MEST_TYPE_NAME.Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_CODE.Contains(this.KEY_WORD) ||
                        o.MEDI_STOCK_NAME.Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_CODE.Contains(this.KEY_WORD) ||
                        o.MEDICINE_TYPE_NAME.Contains(this.KEY_WORD)
                        );
                }
                search.listVHisExpMestMedicine4Expression.AddRange(listVHisExpMestMedicine4Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisExpMestMedicine4Expression.Clear();
                search.listVHisExpMestMedicine4Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
