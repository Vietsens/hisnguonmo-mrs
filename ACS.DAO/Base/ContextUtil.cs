using System;
using System.Data.Entity;
using System.Reflection;
using Inventec.Common;

namespace ACS.DAO.Base
{
    class ContextUtil<T, C>
        where T : class
        where C : AppContext
    {
        DbSet DbSet;

        internal ContextUtil(C context)
        {
            DbSet = context.GetDbSet<T>();
        }

        internal bool CheckIsActive(object id)
        {
            bool result = false;
            if (id != null)
            {
                T entity = (T)DbSet.Find(id);
                Type t = entity.GetType();
                PropertyInfo p1 = t.GetProperty("IS_ACTIVE"); //Chuan thiet ke CSDL khong bao gio thay doi
                if (p1 != null)
                {
                    result = ((short)(p1.GetValue(entity, null) ?? (short)0) == (short)1); //Chuan thiet ke CSDL khong bao gio thay doi
                }
            }
            return result;
        }
    }
}
