using Inventec.Common.Logging;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.Base;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.HisExamServiceTemp
{
    public class HisExamServiceTempFilterQuery : HisExamServiceTempFilter
    {
        public HisExamServiceTempFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERVICE_TEMP, bool>>> listHisExamServiceTempExpression = new List<System.Linq.Expressions.Expression<Func<HIS_EXAM_SERVICE_TEMP, bool>>>();



        internal HisExamServiceTempSO Query()
        {
            HisExamServiceTempSO search = new HisExamServiceTempSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IDs != null)
                {
                    listHisExamServiceTempExpression.Add(o => this.IDs.Contains(o.ID));
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listHisExamServiceTempExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listHisExamServiceTempExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listHisExamServiceTempExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listHisExamServiceTempExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null)
                {
                    listHisExamServiceTempExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                if (!String.IsNullOrEmpty(this.KEY_WORD__CODE__NAME))
                {
                    this.KEY_WORD__CODE__NAME = this.KEY_WORD__CODE__NAME.ToLower().Trim();
                    listHisExamServiceTempExpression.Add(o =>
                        o.EXAM_SERVICE_TEMP_CODE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.EXAM_SERVICE_TEMP_NAME.ToLower().Contains(this.KEY_WORD__CODE__NAME)
                        );
                }

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listHisExamServiceTempExpression.Add(o =>
                        o.CONCLUDE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.CREATOR.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.DESCRIPTION.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.EXAM_SERVICE_TEMP_CODE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.EXAM_SERVICE_TEMP_NAME.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.FULL_EXAM.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.HOSPITALIZATION_REASON.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.NOTE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_CIRCULATION.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_DIGESTION.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_ENT.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_EYE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_KIDNEY_UROLOGY.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_MENTAL.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_MOTION.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_MUSCLE_BONE.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_NEUROLOGICAL.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_NUTRITION.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_OBSTETRIC.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_OEND.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_RESPIRATORY.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PART_EXAM_STOMATOLOGY.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PATHOLOGICAL_HISTORY.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PATHOLOGICAL_HISTORY_FAMILY.ToLower().Contains(this.KEY_WORD__CODE__NAME) ||
                        o.PATHOLOGICAL_PROCESS.ToLower().Contains(this.KEY_WORD__CODE__NAME)
                        );
                }

                search.listHisExamServiceTempExpression.AddRange(listHisExamServiceTempExpression);
                search.OrderField = ORDER_FIELD;
                search.OrderDirection = ORDER_DIRECTION;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listHisExamServiceTempExpression.Clear();
                search.listHisExamServiceTempExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
