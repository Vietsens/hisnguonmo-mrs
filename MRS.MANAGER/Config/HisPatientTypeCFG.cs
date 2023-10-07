using MRS.MANAGER.Base;
using Inventec.Common.Logging;
using Inventec.Core;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisPatientType;

namespace MRS.MANAGER.Config
{
    public class HisPatientTypeCFG
    {
        private const string PATIENT_TYPE_CODE__KSK = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSK";//Doi tuong khám sức khỏe thông thường
        private const string PATIENT_TYPE_CODE__KSKHD = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.KSKHD";//Doi tuong khám sức khỏe theo hợp đồng
        private const string PATIENT_TYPE_CODE__IS_FREE = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.IS_FREE";//Doi tuong mien phi
        private const string PATIENT_TYPE_CODE__FEE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.HOSPITAL_FEE";//Doi tuong vien phi
        private const string PATIENT_TYPE_CODE__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";//bảo hiểm y tế
        private const string PATIENT_TYPE_CODE__QU = "MRS.PATIENT_TYPE.PATIENT_TYPE_CODE.QU";
        private const string PATIENT_TYPE_CODE__DTCS = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.DTCS";// đối tượng chính sách
        private const string PATIENT_TYPE_CODE__GTCA = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.GTCA";// giới thiệu công an
        private const string PATIENT_TYPE_CODE__GTQD = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.GTQD";// giới thiệu quân đội
        private const string PATIENT_TYPE_CODE__DTQT = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.DTQT";// đối tượng quốc tế
        private const string PATIENT_TYPE_CODE__BUYMEDI = "MRS.HIS_RS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BUYMEDI";// đối tượng mua thuốc
        private const string PATIENT_TYPE_CODE__DV = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.SERVICE";//dịch vụ

        private static long patientTypeIdBUYMEDI;
        public static long PATIENT_TYPE_ID__BUYMEDI
        {
            get
            {
                if (patientTypeIdBUYMEDI == 0)
                {
                    patientTypeIdBUYMEDI = GetId(PATIENT_TYPE_CODE__BUYMEDI);
                }
                return patientTypeIdBUYMEDI;
            }
            set
            {
                patientTypeIdDTQT = value;
            }
        }

        private static long patientTypeIdDTQT;
        public static long PATIENT_TYPE_ID__DTQT
        {
            get
            {
                if (patientTypeIdDTQT == 0)
                {
                    patientTypeIdDTQT = GetId(PATIENT_TYPE_CODE__DTQT);
                }
                return patientTypeIdDTQT;
            }
            set
            {
                patientTypeIdDTQT = value;
            }
        }

        private static long patientTypeIdGTQD;
        public static long PATIENT_TYPE_ID__GTQD
        {
            get
            {
                if (patientTypeIdGTQD == 0)
                {
                    patientTypeIdGTQD = GetId(PATIENT_TYPE_CODE__GTQD);
                }
                return patientTypeIdGTQD;
            }
            set
            {
                patientTypeIdGTQD = value;
            }
        }

        private static long patientTypeIdGTCA;
        public static long PATIENT_TYPE_ID__GTCA
        {
            get
            {
                if (patientTypeIdGTCA == 0)
                {
                    patientTypeIdGTCA = GetId(PATIENT_TYPE_CODE__GTCA);
                }
                return patientTypeIdGTCA;
            }
            set
            {
                patientTypeIdGTCA = value;
            }
        }

        private static long patientTypeIdDTCS;
        public static long PATIENT_TYPE_ID__DTCS
        {
            get
            {
                if (patientTypeIdDTCS == 0)
                {
                    patientTypeIdDTCS = GetId(PATIENT_TYPE_CODE__DTCS);
                }
                return patientTypeIdDTCS;
            }
            set
            {
                patientTypeIdDTCS = value;
            }
        }

        private static long patientTypeIdQu;
        public static long PATIENT_TYPE_ID__QU
        {
            get
            {
                if (patientTypeIdQu == 0)
                {
                    patientTypeIdQu = GetId(PATIENT_TYPE_CODE__QU);
                }
                return patientTypeIdQu;
            }
            set
            {
                patientTypeIdQu = value;
            }
        }

        private static long patientTypeIdIsFree;
        public static long PATIENT_TYPE_ID__IS_FREE
        {
            get
            {
                if (patientTypeIdIsFree == 0)
                {
                    patientTypeIdIsFree = GetId(PATIENT_TYPE_CODE__IS_FREE);
                }
                return patientTypeIdIsFree;
            }
            set
            {
                patientTypeIdIsFree = value;
            }
        }

        private static long patientTypeIdKsk;
        public static long PATIENT_TYPE_ID__KSK
        {
            get
            {
                if (patientTypeIdKsk == 0)
                {
                    patientTypeIdKsk = GetId(PATIENT_TYPE_CODE__KSK);
                }
                return patientTypeIdKsk;
            }
            set
            {
                patientTypeIdKsk = value;
            }
        }

        private static long patientTypeIdKskHd;
        public static long PATIENT_TYPE_ID__KSKHD
        {
            get
            {
                if (patientTypeIdKskHd == 0)
                {
                    patientTypeIdKskHd = GetId(PATIENT_TYPE_CODE__KSKHD);
                }
                return patientTypeIdKskHd;
            }
            set
            {
                patientTypeIdKskHd = value;
            }
        }

        private static long patientTypeIdFee;
        public static long PATIENT_TYPE_ID__FEE
        {
            get
            {
                if (patientTypeIdFee == 0)
                {
                    patientTypeIdFee = GetId(PATIENT_TYPE_CODE__FEE);
                }
                return patientTypeIdFee;
            }
            set
            {
                patientTypeIdFee = value;
            }
        }

        private static long patientTypeIdBhyt;
        public static long PATIENT_TYPE_ID__BHYT
        {
            get
            {
                if (patientTypeIdBhyt == 0)
                {
                    patientTypeIdBhyt = GetId(PATIENT_TYPE_CODE__BHYT);
                }
                return patientTypeIdBhyt;
            }
            set
            {
                patientTypeIdBhyt = value;
            }
        }

        private static long patientTypeIdDv;
        public static long PATIENT_TYPE_ID__DV
        {
            get
            {
                if (patientTypeIdDv == 0)
                {
                    patientTypeIdDv = GetId(PATIENT_TYPE_CODE__DV);
                }
                return patientTypeIdDv;
            }
            set
            {
                patientTypeIdDv = value;
            }
        }

        /// <summary>
        /// Nhóm đối tượng BHYT
        /// </summary>
        /// <returns></returns>
        private static List<long> patientTypeIdGroup1s;
        public static List<long> PATIENT_TYPE_IDs__REPORT_GROUP1
        {
            get
            {
                if (patientTypeIdGroup1s == null || patientTypeIdGroup1s.Count == 0)
                {
                    patientTypeIdGroup1s = GetReportGroup1();
                }
                return patientTypeIdGroup1s;
            }
            set
            {
                patientTypeIdGroup1s = value;
            }
        }

        /// <summary>
        /// Nhóm đối tượng thu phí
        /// </summary>
        /// <returns></returns>
        private static List<long> patientTypeIdGroup2s;
        public static List<long> PATIENT_TYPE_IDs__REPORT_GROUP2
        {
            get
            {
                if (patientTypeIdGroup2s == null || patientTypeIdGroup2s.Count == 0)
                {
                    patientTypeIdGroup2s = GetReportGroup2();
                }
                return patientTypeIdGroup2s;
            }
            set
            {
                patientTypeIdGroup2s = value;
            }
        }

        /// <summary>
        /// Nhóm đối tượng miễn phí
        /// </summary>
        /// <returns></returns>
        private static List<long> patientTypeIdGroup3s;
        public static List<long> PATIENT_TYPE_IDs__REPORT_GROUP3
        {
            get
            {
                if (patientTypeIdGroup3s == null || patientTypeIdGroup3s.Count == 0)
                {
                    patientTypeIdGroup3s = GetReportGroup3();
                }
                return patientTypeIdGroup3s;
            }
            set
            {
                patientTypeIdGroup3s = value;
            }
        }

        private static List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE> patientTypes;
        public static List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE> PATIENT_TYPEs
        {
            get
            {
                if (patientTypes == null)
                {
                    patientTypes = GetAll();
                }
                return patientTypes;
            }
            set
            {
                patientTypes = value;
            }
        }

        /// <summary>
        /// Bhyt
        /// </summary>
        /// <returns></returns>
        private static List<long> GetReportGroup1()
        {
            List<long> result = null;
            try
            {
                var data = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => o.ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                if (!(data != null && data.Count() > 0)) throw new ArgumentNullException(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }

        /// <summary>
        /// Nhóm đối tượng thu phí
        /// </summary>
        /// <returns></returns>
        private static List<long> GetReportGroup2()
        {
            List<long> result = null;
            try
            {
                var data = HisPatientTypeCFG.PATIENT_TYPEs.Where(o => o.ID != MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                if (!(data != null && data.Count() > 0)) throw new ArgumentNullException(LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data));
                result = data.Select(o => o.ID).ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }

        /// <summary>
        /// Nhóm đối tượng miễn phí
        /// </summary>
        /// <returns></returns>
        private static List<long> GetReportGroup3()
        {
            List<long> result = new List<long>();
            try
            {
                result.Add(HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = new List<long>();
            }
            return result;
        }

        /// <summary>
        /// Danh sách đối tượng BN
        /// </summary>
        /// <returns></returns>
        private static List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE> GetAll()
        {
            List<MOS.EFMODEL.DataModels.HIS_PATIENT_TYPE> result = null;
            try
            {
                HisPatientTypeFilterQuery filter = new HisPatientTypeFilterQuery();
                result = new HisPatientTypeManager().Get(filter);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private static long GetHeinId(string code)
        {
            long result = 0;
            try
            {
                var data = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT);
                if (!(data != null && data.ID > 0)) throw new ArgumentNullException(code + LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => data), data));
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = 0;
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
                //filter.SERVICE_REQ_TYPE_CODE = code;
                var data = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.PATIENT_TYPE_CODE == value);
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
                patientTypeIdBUYMEDI = 0;
                patientTypeIdDTQT = 0;
                patientTypeIdGTQD = 0;
                patientTypeIdGTCA = 0;
                patientTypeIdDTCS = 0;
                patientTypeIdQu = 0;
                patientTypeIdIsFree = 0;
                patientTypeIdKsk = 0;
                patientTypeIdKskHd = 0;
                patientTypeIdFee = 0;
                patientTypeIdBhyt = 0;
                patientTypeIdGroup1s = null;
                patientTypeIdGroup2s = null;
                patientTypeIdGroup3s = null;
                patientTypes = null;
                patientTypeIdDv = 0;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
