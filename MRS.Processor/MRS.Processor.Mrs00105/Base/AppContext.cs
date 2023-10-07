using MOS.EFMODEL.DataModels;
using System;
using System.Data.Entity;

namespace MRS.Processor.Mrs00105
{
    public class AppContext : MOSEntities
    {
        public AppContext()
            : base()
        {
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = true;
            this.Configuration.ValidateOnSaveEnabled = false;
            this.Configuration.LazyLoadingEnabled = false;
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