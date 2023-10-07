using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using SAR.EFMODEL.DataModels;

namespace SAR.DAO.Base
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
        public BridgeDAO()
        {

        }

        public bool IsUnLock(long id)
        {
            bool result = false;
            using (var ctx = new AppContext())
            {
                ContextUtil<RAW, AppContext> ctxUtil = new ContextUtil<RAW, AppContext>(ctx);
                result = ctxUtil.CheckIsActive(id);
            }
            return result;
        }

        public bool Create(RAW data)
        {
            bool result = false;
            try
            {
                if (IsNotNull(data))
                {
                    SAR.EFMODEL.Decorator.AppCreatorDecorator.Set<RAW>(data);
                    SAR.EFMODEL.Decorator.CreatorDecorator.Set<RAW>(data, Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName());
                    SAR.EFMODEL.Decorator.GroupCodeDecorator.Set<RAW>(data, Inventec.Token.ResourceSystem.ResourceTokenManager.GetGroupCode());
                    SAR.EFMODEL.Decorator.DummyDecorator.Set<RAW>(data);
                    SAR.EFMODEL.Decorator.IsActiveDecorator.Set<RAW>(data);
                    SAR.EFMODEL.Decorator.IsDeleteDecorator.Set<RAW>(data);

                    using (var ctx = new AppContext())
                    {
                        ctx.GetDbSet<RAW>().Add(data);
                        result = (ctx.SaveChanges() > 0);
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }

            return result;
        }

        public bool CreateList(List<RAW> listData)
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listData))
                {
                    string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
                    string groupCode = Inventec.Token.ResourceSystem.ResourceTokenManager.GetGroupCode();

                    using (var ctx = new AppContext())
                    {
                        var dbSet = ctx.GetDbSet<RAW>();
                        foreach (var data in listData)
                        {
                            SAR.EFMODEL.Decorator.AppCreatorDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.CreatorDecorator.Set<RAW>(data, loginName);
                            SAR.EFMODEL.Decorator.GroupCodeDecorator.Set<RAW>(data, groupCode);
                            SAR.EFMODEL.Decorator.DummyDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.IsActiveDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.IsDeleteDecorator.Set<RAW>(data);

                            dbSet.Add(data);
                        }
                        result = (ctx.SaveChanges() > 0);
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Newtonsoft.Json.JsonConvert.SerializeObject(listData), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Newtonsoft.Json.JsonConvert.SerializeObject(listData), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool Update(RAW data)
        {
            bool result = false;
            try
            {
                if (IsNotNull(data))
                {
                    using (var ctx = new AppContext())
                    {
                        SAR.EFMODEL.Decorator.IsActiveDecorator.Set<RAW>(data);
                        SAR.EFMODEL.Decorator.IsDeleteDecorator.Set<RAW>(data);
                        SAR.EFMODEL.Decorator.AppModifierDecorator.Set<RAW>(data);
                        SAR.EFMODEL.Decorator.ModifierDecorator.Set<RAW>(data, Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName());
                        var entry = ctx.Entry<RAW>(data);
                        ctx.GetDbSet<RAW>().Attach(data);
                        //entry.State = EntityState.Modified;
                        ctx.Entry<RAW>(data).State = System.Data.EntityState.Modified;

                        List<string> notChangeFields = SAR.EFMODEL.Decorator.DenyUpdateDecorator.Get<RAW>();
                        if (notChangeFields != null && notChangeFields.Count > 0)
                        {
                            foreach (string field in notChangeFields)
                            {
                                entry.Property(field).IsModified = false;
                            }
                        }

                        ctx.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        public bool UpdateList(List<RAW> listData)
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listData))
                {
                    using (var ctx = new AppContext())
                    {
                        var dbSet = ctx.GetDbSet<RAW>();
                        string loginName = Inventec.Token.ResourceSystem.ResourceTokenManager.GetLoginName();
                        List<string> notChangeFields = SAR.EFMODEL.Decorator.DenyUpdateDecorator.Get<RAW>();

                        foreach (RAW data in listData)
                        {
                            SAR.EFMODEL.Decorator.IsActiveDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.IsDeleteDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.AppModifierDecorator.Set<RAW>(data);
                            SAR.EFMODEL.Decorator.ModifierDecorator.Set<RAW>(data, loginName);
                            ctx.Entry(data).State = EntityState.Detached;
                            dbSet.Attach(data);

                            var entry = ctx.Entry(data);
                            entry.State = EntityState.Modified;

                            if (notChangeFields != null && notChangeFields.Count > 0)
                            {
                                foreach (string field in notChangeFields)
                                {
                                    entry.Property(field).IsModified = false;
                                }
                            }
                        }
                        ctx.SaveChanges();
                        result = true;
                    }
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                Logging(LogUtil.TraceDbException(ex), LogType.Error);
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listData), listData), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            catch (Exception ex)
            {
                Logging(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => listData), listData), LogType.Error);
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }
    }
}
