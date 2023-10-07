using LIS.EFMODEL.Decorator;
using Inventec.Backend.EFMODEL;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace LIS.DAO.Base
{
    /// <summary>
    /// CREATE
    /// - Thanh cong khi co du lieu duoc insert vao DB thuc (savechange > 0 && == so luong can insert)
    /// UPDATE
    /// - Thanh cong khi tim thay du lieu, cap nhat & savechange ma khong co exception gi. Khong quan tam toi viec thuc te du lieu trong database co thay doi gi hay khong (truong hop du lieu update giong het du lieu dang ton tai).
    /// - That bai khi khong tim duoc du lieu, hoac savechange co exception
    /// DELETE
    /// - Tuong tu nhu update, can phai tim thay du lieu (can delete), thuc hien set IS_DELETE = 1 & savechange thanh cong
    /// - That bai khi khong tim duoc du lieu, hoac savechange co exception
    /// - Tuong tu truong hop can delete 1 list danh sach, chi can 1 du lieu trong list khong duoc tim thay, return false khong thuc hien xoa
    /// TRUNCATE
    /// - Tuong tu delete
    /// </summary>
    /// <typeparam name="DTO"></typeparam>
    /// <typeparam name="RAW"></typeparam>
    class BridgeDAO<RAW> : Inventec.Core.EntityBase
        where RAW : class
    {
        const string APP_CODE = "LIS";

        public BridgeDAO()
        {

        }

        public bool ExecuteSql(string sql)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(sql))
                {
                    using (var ctx = new AppContext())
                    {
                        var count = ctx.Database.ExecuteSqlCommand(sql);
                        if (count == 0)
                        {
                            Inventec.Common.Logging.LogSystem.Info("SQL thuc hien thanh cong tuy nhien khong co du lieu duoc tac dong." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql));
                        }
                        result = true;
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => sql), sql), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}