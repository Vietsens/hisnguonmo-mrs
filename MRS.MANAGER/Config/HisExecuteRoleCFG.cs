using Inventec.Common.Logging;
using MOS.MANAGER.HisExecuteRole;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.MANAGER.Config
{
    public class HisExecuteRoleCFG
    {
        private const string HIS_EXECUTE_CODE__MAIN = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.MAIN";//bac sy phau thuat
        private const string HIS_EXECUTE_CODE__ANESTHETIST = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.ANESTHETIST";//bac sy gay me
        private const string HIS_EXECUTE_CODE__TT = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.TT";//bac sy thu thuat
        private const string HIS_EXECUTE_CODE__PM1 = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.PM1";//phu mo 1
        private const string HIS_EXECUTE_CODE__PM2 = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.PM2";//phu mo 2
        private const string HIS_EXECUTE_CODE__PMe1 = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.PMe1";//phu me 1
        private const string HIS_EXECUTE_CODE__PMe2 = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.Pme2";//phu me 2
        private const string HIS_EXECUTE_CODE__TYDD = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.YTDD";//y ta dieu duong
        private const string HIS_EXECUTE_CODE__DCVPTTT = "DBCODE.HIS_RS.HIS_EXECUTE_ROLE.EXECUTE_ROLE_CODE.DCVPTTT";//dung cu vien PTTT

        private static long hisExecuteRoleId__Main;
        public static long HisExecuteRoleId__Main
        {
            get
            {
                if (hisExecuteRoleId__Main == 0)
                {
                    hisExecuteRoleId__Main = GetId(HIS_EXECUTE_CODE__MAIN);
                }
                return hisExecuteRoleId__Main;
            }
        }

        private static long hisExecuteRoleId__Anesthetist;
        public static long HisExecuteRoleId__Anesthetist
        {
            get
            {
                if (hisExecuteRoleId__Anesthetist == 0)
                {
                    hisExecuteRoleId__Anesthetist = GetId(HIS_EXECUTE_CODE__ANESTHETIST);
                }
                return hisExecuteRoleId__Anesthetist;
            }
        }

        private static long hisExecuteRoleId__TT;
        public static long HisExecuteRoleId__TT
        {
            get
            {
                if (hisExecuteRoleId__TT == 0)
                {
                    hisExecuteRoleId__TT = GetId(HIS_EXECUTE_CODE__TT);
                }
                return hisExecuteRoleId__TT;
            }
        }

        private static long hisExecuteRoleId__PM1;
        public static long HisExecuteRoleId__PM1
        {
            get
            {
                if (hisExecuteRoleId__PM1 == 0)
                {
                    hisExecuteRoleId__PM1 = GetId(HIS_EXECUTE_CODE__PM1);
                }
                return hisExecuteRoleId__PM1;
            }
        }

        private static long hisExecuteRoleId__PM2;
        public static long HisExecuteRoleId__PM2
        {
            get
            {
                if (hisExecuteRoleId__PM2 == 0)
                {
                    hisExecuteRoleId__PM2 = GetId(HIS_EXECUTE_CODE__PM2);
                }
                return hisExecuteRoleId__PM2;
            }
        }

        private static long hisExecuteRoleId__PMe1;
        public static long HisExecuteRoleId__PMe1
        {
            get
            {
                if (hisExecuteRoleId__PMe1 == 0)
                {
                    hisExecuteRoleId__PMe1 = GetId(HIS_EXECUTE_CODE__PMe1);
                }
                return hisExecuteRoleId__PMe1;
            }
        }

        private static long hisExecuteRoleId__PMe2;
        public static long HisExecuteRoleId__PMe2
        {
            get
            {
                if (hisExecuteRoleId__PMe2 == 0)
                {
                    hisExecuteRoleId__PMe2 = GetId(HIS_EXECUTE_CODE__PMe2);
                }
                return hisExecuteRoleId__PMe2;
            }
        }

        private static long hisExecuteRoleId__YTDD;
        public static long HisExecuteRoleId__YTDD
        {
            get
            {
                if (hisExecuteRoleId__YTDD == 0)
                {
                    hisExecuteRoleId__YTDD = GetId(HIS_EXECUTE_CODE__TYDD);
                }
                return hisExecuteRoleId__YTDD;
            }
        }

        private static long hisExecuteRoleId__DCVPTTT;
        public static long HisExecuteRoleId__DCVPTTT
        {
            get
            {
                if (hisExecuteRoleId__DCVPTTT == 0)
                {
                    hisExecuteRoleId__DCVPTTT = GetId(HIS_EXECUTE_CODE__DCVPTTT);
                }
                return hisExecuteRoleId__DCVPTTT;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_EXECUTE_ROLE> ExecuteRoles;
        public static List<MOS.EFMODEL.DataModels.HIS_EXECUTE_ROLE> EXECUTE_ROLEs
        {
            get
            {
                if (ExecuteRoles == null)
                {
                    ExecuteRoles = GetAll();
                }
                return ExecuteRoles;
            }
            set
            {
                ExecuteRoles = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_EXECUTE_ROLE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_EXECUTE_ROLE> result = null;
            try
            {
                HisExecuteRoleFilterQuery filter = new HisExecuteRoleFilterQuery();
                result = new HisExecuteRoleManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                var config = Loader.dictionaryConfig[code];
                if (config == null) throw new ArgumentNullException(code);
                string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
                if (String.IsNullOrEmpty(value)) throw new ArgumentNullException(code);
                HisExecuteRoleFilterQuery filter = new HisExecuteRoleFilterQuery();
                //filter.DEPARTMENT_CODE = value;//TODO
                var data = new HisExecuteRoleManager().Get(filter).FirstOrDefault(o => o.EXECUTE_ROLE_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                hisExecuteRoleId__Main = 0;
                hisExecuteRoleId__Anesthetist = 0;
                hisExecuteRoleId__TT = 0;
                hisExecuteRoleId__PM1 = 0;
                hisExecuteRoleId__PM2 = 0;
                hisExecuteRoleId__PMe1 = 0;
                hisExecuteRoleId__PMe2 = 0;
                hisExecuteRoleId__YTDD = 0;
                hisExecuteRoleId__DCVPTTT = 0;
                ExecuteRoles = null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
