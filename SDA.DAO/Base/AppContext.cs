using System;
using System.Data.Entity;
using SDA.EFMODEL.DataModels;

namespace SDA.DAO.Base
{
    class AppContext : SDAEntities
    {
        public AppContext()
            : base()
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet GetDbSet<RAW>() where RAW : class
        {
            DbSet result = null;
            Type typeRaw = typeof(RAW);

            var properties = typeof(AppContext).GetProperties();
            foreach (var pr in properties)
            {
                Type propertyType = pr.PropertyType.GenericTypeArguments != null
                    && pr.PropertyType.GenericTypeArguments.Length > 0 ? pr.PropertyType.GenericTypeArguments[0] : null;
                if (propertyType == typeRaw)
                {
                    result = (DbSet<RAW>)pr.GetValue(this);
                    break;
                }
            }
            return result;
        }
    }
}