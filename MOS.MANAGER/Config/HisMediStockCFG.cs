using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisMediStock;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;

namespace MOS.MANAGER.Config
{
    public class HisMediStockCFG
    {
        public enum ExportOption
        {
            IMP_TIME = 1,//uu tien theo thoi gian nhap truoc
            EXPIRED_DATE = 2,//uu tien theo han su dung
        }

        //Cac cau hinh quan ly thuoc le cua kho duoc (ko tinh tu truc)
        public enum OddManagementOption
        {
            //Cho phep nhap le, xuat le, khong quan tam
            ALLOW = 1,

            //Ko cho phep va khong quan ly phan le (phan le coi nhu la hao phi)
            DISALLOW_AND_NO_MANAGEMENT = 2,

            //Ko cho phep nhung co quan ly phan le
            //- Trong truong hop linh thuoc thi tao phan bu` se chuyen ve kho le,
            //- Trong truong hop tra thuoc thi phan le se chuyen ve kho le
            DISALLOW_AND_MANAGEMENT = 3,
        }

        private const string EXPORT_OPTION_CFG = "MOS.HIS_MEDI_STOCK.EXPORT_OPTION";
        private const string ODD_MEDICINE_MANAGEMENT_OPTION_CFG = "MOS.HIS_MEDI_STOCK.ODD_MEDICINE_MANAGEMENT_OPTION";
        private const string ODD_MATERIAL_MANAGEMENT_OPTION_CFG = "MOS.HIS_MEDI_STOCK.ODD_MATERIAL_MANAGEMENT_OPTION";

        private static int exportOption;
        public static int EXPORT_OPTION
        {
            get
            {
                if (exportOption == 0)
                {
                    exportOption = ConfigUtil.GetIntConfig(EXPORT_OPTION_CFG);
                }
                return exportOption;
            }
            set
            {
                exportOption = value;
            }
        }

        private static int oddMedicineManagementOption;
        public static int ODD_MEDICINE_MANAGEMENT_OPTION
        {
            get
            {
                if (oddMedicineManagementOption == 0)
                {
                    oddMedicineManagementOption = ConfigUtil.GetIntConfig(ODD_MEDICINE_MANAGEMENT_OPTION_CFG);
                }
                return oddMedicineManagementOption;
            }
            set
            {
                oddMedicineManagementOption = value;
            }
        }

        private static int oddMaterialManagementOption;
        public static int ODD_MATERIAL_MANAGEMENT_OPTION
        {
            get
            {
                if (oddMaterialManagementOption == 0)
                {
                    oddMaterialManagementOption = ConfigUtil.GetIntConfig(ODD_MATERIAL_MANAGEMENT_OPTION_CFG);
                }
                return oddMaterialManagementOption;
            }
            set
            {
                oddMaterialManagementOption = value;
            }
        }

        private static List<V_HIS_MEDI_STOCK> data;
        public static List<V_HIS_MEDI_STOCK> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisMediStockGet().GetView(new HisMediStockViewFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        public static void Reload()
        {
            var tmp = new HisMediStockGet().GetView(new HisMediStockViewFilterQuery());
            exportOption = ConfigUtil.GetIntConfig(EXPORT_OPTION_CFG);
            data = tmp;
        }
    }
}
