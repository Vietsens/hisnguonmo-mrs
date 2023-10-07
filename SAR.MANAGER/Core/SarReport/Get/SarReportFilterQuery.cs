using SAR.DAO.StagingObject;
using SAR.EFMODEL.DataModels;
using SAR.Filter;
using SAR.MANAGER.Base;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;

namespace SAR.MANAGER.Core.SarReport.Get
{
    public class SarReportFilterQuery : SarReportFilter
    {
        public SarReportFilterQuery()
            : base()
        {

        }

        internal List<System.Linq.Expressions.Expression<Func<SAR_REPORT, bool>>> listExpression = new List<System.Linq.Expressions.Expression<Func<SAR_REPORT, bool>>>();

        internal OrderProcessorBase OrderProcess = new OrderProcessorBase();

        internal SarReportSO Query()
        {
            SarReportSO search = new SarReportSO();
            try
            {
                #region Abstract Base
                if (this.ID.HasValue)
                {
                    listExpression.Add(o => o.ID == this.ID.Value);
                }
                if (this.IS_ACTIVE.HasValue)
                {
                    listExpression.Add(o => o.IS_ACTIVE == this.IS_ACTIVE.Value);
                }
                if (this.CREATE_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value >= this.CREATE_TIME_FROM.Value);
                }
                if (this.CREATE_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.CREATE_TIME.Value <= this.CREATE_TIME_TO.Value);
                }
                if (this.MODIFY_TIME_FROM.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value >= this.MODIFY_TIME_FROM.Value);
                }
                if (this.MODIFY_TIME_TO.HasValue)
                {
                    listExpression.Add(o => o.MODIFY_TIME.Value <= this.MODIFY_TIME_TO.Value);
                }
                if (!String.IsNullOrEmpty(this.CREATOR))
                {
                    listExpression.Add(o => o.CREATOR == this.CREATOR);
                }
                if (!String.IsNullOrEmpty(this.MODIFIER))
                {
                    listExpression.Add(o => o.MODIFIER == this.MODIFIER);
                }
                if (!String.IsNullOrEmpty(this.GROUP_CODE))
                {
                    listExpression.Add(o => o.GROUP_CODE == this.GROUP_CODE);
                }
                if (this.IDs != null && this.IDs.Count > 0)
                {
                    listExpression.Add(o => this.IDs.Contains(o.ID));
                }
                #endregion

                /////Fix dieu kien loc luon dam bao chi loc ra cac bao cao theo cac dieu kien sau (HOAC)
                /////Do nguoi do tao ra
                /////Nguoi do nhan duoc
                /////Bao cao chia se cong cong
                //string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
                //if (String.IsNullOrWhiteSpace(loginName))
                //{
                //    Inventec.Common.Logging.LogSystem.Error("Khong lay duoc thong tin loginName.");
                //    listExpression.Add(o => o.ID == NEGATIVE_ID);
                //}
                //listExpression.Add(o => (o.CREATOR == loginName) || (o.IS_PUBLIC == 1) || (o.JSON_READER.Contains(loginName))); // 

                //switch (FilterMode)
                //{
                //    case FilterModeEnum.ALL:
                //        break;
                //    case FilterModeEnum.CREATE:
                //        listExpression.Add(o => (o.CREATOR == loginName));
                //        break;
                //    case FilterModeEnum.RECEIVE:
                //        listExpression.Add(o => (o.IS_PUBLIC == 1) || (o.JSON_READER.Contains(loginName)));
                //        break;
                //    default:
                //        break;
                //}

                if (!String.IsNullOrEmpty(this.KEY_WORD))
                {
                    this.KEY_WORD = this.KEY_WORD.ToLower().Trim();
                    listExpression.Add(o =>
                        o.CREATOR.ToLower().Contains(this.KEY_WORD) ||
                        o.MODIFIER.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_CODE.ToLower().Contains(this.KEY_WORD) ||
                        o.REPORT_NAME.ToLower().Contains(this.KEY_WORD));
                }

                if (this.REPORT_STT_ID.HasValue)
                {
                    listExpression.Add(o => o.REPORT_STT_ID == this.REPORT_STT_ID.Value);
                }

                if (this.REPORT_STT_IDs != null && this.REPORT_STT_IDs.Count > 0)
                {
                    listExpression.Add(o => this.REPORT_STT_IDs.Contains(o.REPORT_STT_ID));
                }

                search.listSarReportExpression.AddRange(listExpression);
                search.OrderField = OrderProcess.GetOrderField<SAR_REPORT>(ORDER_FIELD);
                search.OrderDirection = OrderProcess.GetOrderDirection(ORDER_DIRECTION);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                search.listSarReportExpression.Clear();
                search.listSarReportExpression.Add(o => o.ID == NEGATIVE_ID);
            }
            return search;
        }
    }
}
