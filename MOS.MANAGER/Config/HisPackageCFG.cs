using Inventec.Common.Logging;
using MOS.DAO.HisExpMestStt;
using MOS.DAO.StagingObject;
using MOS.EFMODEL.DataModels;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPackage;

namespace MOS.MANAGER.Config
{
    class HisPackageCFG
    {
        private const string PACKAGE_CODE__3DAY7DAY = "MOS.HIS_PACKAGE.PACKAGE_CODE.3DAY7DAY"; //cua loai chinh sach "3 ngay 7 ngay"
        
        private static List<HIS_PACKAGE> activeData;
        public static List<HIS_PACKAGE> ACTIVE_DATA
        {
            get
            {
                if (activeData == null)
                {
                    activeData = new HisPackageGet().GetActive();
                }
                return activeData;
            }
            set
            {
                activeData = value;
            }
        }

        private static long? packageId3day7day;
        public static long? PACKAGE_ID__3DAY7DAY
        {
            get
            {
                if (!packageId3day7day.HasValue)
                {
                    packageId3day7day = GetId(PACKAGE_CODE__3DAY7DAY);
                }
                return packageId3day7day;
            }
            set
            {
                packageId3day7day = value;
            }
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                if (value != null)
                {
                    var data = ACTIVE_DATA.Where(o => o.PACKAGE_CODE == code).FirstOrDefault();
                    if (data == null) throw new ArgumentNullException(code);
                    result = data.ID;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        public static void Reload()
        {
            var data = new HisPackageGet().GetActive();
            activeData = data;
            var id3day7day = GetId(PACKAGE_CODE__3DAY7DAY);
            packageId3day7day = id3day7day;
        }
    }
}
