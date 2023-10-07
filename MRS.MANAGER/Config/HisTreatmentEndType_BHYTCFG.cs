using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using MOS.MANAGER.HisTreatmentEndType;

namespace MRS.MANAGER.Config
{
    public class HisTreatmentEndType_BHYTCFG
    {
        //private const string TREATMENT_END_TYPE_CODE__RAVIEN = "EXE.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.LEAVE";
        //private const string TREATMENT_END_TYPE_CODE__CHUYENVIEN = "EXE.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.TRAN_PATI";
        //private const string TREATMENT_END_TYPE_CODE__TRONVIEN = "EXE.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.ESC";
        //private const string TREATMENT_END_TYPE_CODE__XINRAVIEN = "EXE.HIS_TREATMENT_END_TYPE.TREATMENT_END_TYPE_CODE.P_LEAVE";

        private static long treatmentEndTypeIdRaVien;
        public static long TREATMENT_END_TYPE_ID__RAVIEN
        {
            get
            {
                if (treatmentEndTypeIdRaVien == 0)
                {
                    treatmentEndTypeIdRaVien = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__RAVIEN;// GetId(TREATMENT_END_TYPE_CODE__RAVIEN);
                }
                return treatmentEndTypeIdRaVien;
            }
            set
            {
                treatmentEndTypeIdRaVien = value;
            }
        }

        private static long treatmentEndTypeIdChuyenVien;
        public static long TREATMENT_END_TYPE_ID__CHUYENVIEN
        {
            get
            {
                if (treatmentEndTypeIdChuyenVien == 0)
                {
                    treatmentEndTypeIdChuyenVien = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN;// GetId(TREATMENT_END_TYPE_CODE__CHUYENVIEN);
                }
                return treatmentEndTypeIdChuyenVien;
            }
            set
            {
                treatmentEndTypeIdChuyenVien = value;
            }
        }

        private static long treatmentEndTypeIdTronVien;
        public static long TREATMENT_END_TYPE_ID__TRONVIEN
        {
            get
            {
                if (treatmentEndTypeIdTronVien == 0)
                {
                    treatmentEndTypeIdTronVien = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__TRON;// GetId(TREATMENT_END_TYPE_CODE__TRONVIEN);
                }
                return treatmentEndTypeIdTronVien;
            }
            set
            {
                treatmentEndTypeIdTronVien = value;
            }
        }

        private static long treatmentEndTypeIdXinRaVien;
        public static long TREATMENT_END_TYPE_ID__XINRAVIEN
        {
            get
            {
                if (treatmentEndTypeIdXinRaVien == 0)
                {
                    treatmentEndTypeIdXinRaVien = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__XINRAVIEN;// GetId(TREATMENT_END_TYPE_CODE__XINRAVIEN);
                }
                return treatmentEndTypeIdXinRaVien;
            }
            set
            {
                treatmentEndTypeIdXinRaVien = value;
            }
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
                HisTreatmentEndTypeFilterQuery filter = new HisTreatmentEndTypeFilterQuery();
                //filter.KEY_WORD = value;
                var data = new HisTreatmentEndTypeManager().Get(filter).FirstOrDefault(o => o.TREATMENT_END_TYPE_CODE == value);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => config), config));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
            }
            return result;
        }

        public static void Refresh()
        {
            try
            {
                treatmentEndTypeIdRaVien = 0;
                treatmentEndTypeIdChuyenVien = 0;
                treatmentEndTypeIdTronVien = 0;
                treatmentEndTypeIdXinRaVien = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
