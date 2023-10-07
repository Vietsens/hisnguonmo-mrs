using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisSereServ
{
    public class HisSereServView9FilterQuery : HisSereServView9Filter
    {
        public HisSereServView9FilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_9, bool>>> listVHisSereServ9Expression = new List<System.Linq.Expressions.Expression<Func<V_HIS_SERE_SERV_9, bool>>>();

        

        internal HisSereServSO Query()
        {
            HisSereServSO search = new HisSereServSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listVHisSereServ9Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listVHisSereServ9Expression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listVHisSereServ9Expression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listVHisSereServ9Expression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                #endregion

                if (this.SERVICE_REQ_ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.SERVICE_REQ_ID == this.SERVICE_REQ_ID.Value);
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER__EXACT))
                {
                    listVHisSereServ9Expression.Add(o => o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.Equals(this.HEIN_CARD_NUMBER__EXACT));
                }
                if (!String.IsNullOrEmpty(this.HEIN_CARD_NUMBER))
                {
                    listVHisSereServ9Expression.Add(o => o.HEIN_CARD_NUMBER != null && o.HEIN_CARD_NUMBER.ToLower().Contains(this.HEIN_CARD_NUMBER.ToLower().Trim()));
                }
                if (this.PATIENT_TYPE_ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.PATIENT_TYPE_ID == this.PATIENT_TYPE_ID.Value);
                }
                if (this.SERVICE_REQ_IDs != null)
                {
                    listVHisSereServ9Expression.Add(o => o.SERVICE_REQ_ID.HasValue && this.SERVICE_REQ_IDs.Contains(o.SERVICE_REQ_ID.Value));
                }
                if (this.IS_SPECIMEN.HasValue && this.IS_SPECIMEN.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.IS_SPECIMEN.HasValue && o.IS_SPECIMEN.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_SPECIMEN.HasValue && !this.IS_SPECIMEN.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.IS_SPECIMEN.HasValue || o.IS_SPECIMEN.Value != ManagerConstant.IS_TRUE);
                }
                if (this.ID__NOT_EQUAL.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.ID != this.ID__NOT_EQUAL.Value);
                }
                if (this.IS_EXPEND.HasValue && this.IS_EXPEND.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.IS_EXPEND.HasValue && o.IS_EXPEND.Value == ManagerConstant.IS_TRUE);
                }
                if (this.IS_EXPEND.HasValue && !this.IS_EXPEND.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.IS_EXPEND.HasValue || o.IS_EXPEND.Value != ManagerConstant.IS_TRUE);
                }
                if (this.INVOICE_ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.INVOICE_ID.HasValue && o.INVOICE_ID.Value == this.INVOICE_ID.Value);
                }
                if (this.HAS_INVOICE.HasValue && !this.HAS_INVOICE.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.INVOICE_ID.HasValue);
                }
                if (this.HAS_INVOICE.HasValue && this.HAS_INVOICE.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.INVOICE_ID.HasValue);
                }
                if (this.HAS_EXECUTE.HasValue && !this.HAS_EXECUTE.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.IS_NO_EXECUTE.HasValue && o.IS_NO_EXECUTE.Value == ManagerConstant.IS_TRUE);
                }
                if (this.HAS_EXECUTE.HasValue && this.HAS_EXECUTE.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.IS_NO_EXECUTE.HasValue || o.IS_NO_EXECUTE.Value != ManagerConstant.IS_TRUE);
                }
                if (this.HEIN_APPROVAL_ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.HEIN_APPROVAL_ID.HasValue && o.HEIN_APPROVAL_ID.Value == this.HEIN_APPROVAL_ID.Value);
                }
                if (this.HEIN_APPROVAL_IDs != null)
                {
                    listVHisSereServ9Expression.Add(o => o.HEIN_APPROVAL_ID.HasValue && this.HEIN_APPROVAL_IDs.Contains(o.HEIN_APPROVAL_ID.Value));
                }
                if (this.TREATMENT_ID.HasValue)
                {
                    listVHisSereServ9Expression.Add(o => o.TDL_TREATMENT_ID == this.TREATMENT_ID.Value);
                }
                if (this.TREATMENT_IDs != null)
                {
                    listVHisSereServ9Expression.Add(o => o.TDL_TREATMENT_ID.HasValue && this.TREATMENT_IDs.Contains(o.TDL_TREATMENT_ID.Value));
                }
                if (this.IDs != null)
                {
                    listVHisSereServ9Expression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_MEDICINE.HasValue && this.IS_MEDICINE.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.MEDICINE_ID.HasValue);
                }
                if (this.IS_MEDICINE.HasValue && !this.IS_MEDICINE.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.MEDICINE_ID.HasValue);
                }
                if (this.IS_MATERIAL.HasValue && this.IS_MATERIAL.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.MATERIAL_ID.HasValue);
                }
                if (this.IS_MATERIAL.HasValue && !this.IS_MATERIAL.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.MATERIAL_ID.HasValue);
                }
                if (this.IS_BLOOD.HasValue && this.IS_BLOOD.Value)
                {
                    listVHisSereServ9Expression.Add(o => o.BLOOD_ID.HasValue);
                }
                if (this.IS_BLOOD.HasValue && !this.IS_BLOOD.Value)
                {
                    listVHisSereServ9Expression.Add(o => !o.BLOOD_ID.HasValue);
                }

                search.listVHisSereServ9Expression.AddRange(listVHisSereServ9Expression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listVHisSereServ9Expression.Clear();
                search.listVHisSereServ9Expression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
