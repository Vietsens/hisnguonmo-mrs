using System;
using System.Data.Entity;
using System.Data.Metadata.Edm;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MOS.DAO.Base
{
    class ContextUtil<T, C>
        where T : class
        where C : AppContext
    {
        DbSet DbSet;
        AppContext appContext;

        internal ContextUtil(C context)
        {
            appContext = context;
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

        public static int GetMaxLength<TEntity>(ObjectContext oc, Expression<Func<TEntity, string>> property)
            where TEntity : EntityObject
        {
            var test = oc.MetadataWorkspace.GetItems(DataSpace.CSpace);

            if (test == null)
                return -1;

            Type entType = typeof(TEntity);
            string propertyName = ((MemberExpression)property.Body).Member.Name;

            var q = test
                .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                .SelectMany(meta => ((EntityType)meta).Properties
                .Where(p => p.Name == propertyName && p.TypeUsage.EdmType.Name == "String"));

            var queryResult = q.Where(p =>
            {
                var match = p.DeclaringType.Name == entType.Name;
                if (!match)
                    match = entType.Name == p.DeclaringType.Name;
                return match;
            })
                .Select(sel => sel.TypeUsage.Facets["MaxLength"].Value)
                .ToList();

            if (queryResult.Any())
            {
                int result = Convert.ToInt32(queryResult.First());
                return result;
            }
            return -1;
        }
    }
}
