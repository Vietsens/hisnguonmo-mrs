using MOS.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00826.Base
{
    public class GlobalConfigStore
    {
        public static bool IsInit { get; set; }

        //danh mã ICD ngoại định suất và ngoài đinh suất đối với trẻ em
        public static List<string> ListIcdCode_Nds = new List<string>();
        public static List<string> ListIcdCode_Nds_Te = new List<string>();

        //Đường dẫn thư mục lưu file xml
        public static string PathSaveXml;

        //Thông tin đơn vị
        public static string Signature;
        public static HIS_BRANCH Branch;

        //Thông tin giấy phép hành nghề của Bs
        public static List<HIS_EMPLOYEE> ListEmployees = new List<HIS_EMPLOYEE>();
        public static List<HIS_DEPARTMENT> ListDepartments = new List<HIS_DEPARTMENT>();
        public static List<V_HIS_SERVICE> ListService = new List<V_HIS_SERVICE>();

        public const int MAX_LENGTH = 1024;

        private static List<HIS_MEDI_ORG> HisMediOrg;
        internal static List<HIS_MEDI_ORG> HisHeinMediOrg
        {
            get
            {
                if (HisMediOrg == null || HisMediOrg.Count <= 0)
                {
                    HisMediOrg = ConfigHeinMediOrg.GetMediOrgConst();
                }

                return HisMediOrg;
            }
            set
            {
                HisMediOrg = value;
            }
        }
    }
}
