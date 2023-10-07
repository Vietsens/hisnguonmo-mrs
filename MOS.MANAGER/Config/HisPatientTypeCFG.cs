using Inventec.Common.Logging;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientType;
using SDA.EFMODEL.DataModels;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.Base;

namespace MOS.MANAGER.Config
{
    public class HisPatientTypeCFG
    {
        private const string PATIENT_TYPE_CODE__BHYT = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.BHYT";
        private const string PATIENT_TYPE_CODE__HOSPITAL_FEE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.HOSPITAL_FEE";
        private const string PATIENT_TYPE_CODE__SERVICE = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.SERVICE";
        private const string PATIENT_TYPE_CODE__OVER_EXPEND = "MOS.HIS_PATIENT_TYPE.PATIENT_TYPE_CODE.OVER_EXPEND";

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

        private static long patientTypeIdHospitalFee;
        public static long PATIENT_TYPE_ID__HOSPITAL_FEE
        {
            get
            {
                if (patientTypeIdHospitalFee == 0)
                {
                    patientTypeIdHospitalFee = GetId(PATIENT_TYPE_CODE__HOSPITAL_FEE);
                }
                return patientTypeIdHospitalFee;
            }
            set
            {
                patientTypeIdHospitalFee = value;
            }
        }

        private static long patientTypeIdService;
        public static long PATIENT_TYPE_ID__SERVICE
        {
            get
            {
                if (patientTypeIdService == 0)
                {
                    patientTypeIdService = GetId(PATIENT_TYPE_CODE__SERVICE);
                }
                return patientTypeIdService;
            }
            set
            {
                patientTypeIdService = value;
            }
        }

        private static long patientTypeIdOverExpend;
        public static long PATIENT_TYPE_ID__OVER_EXPEND
        {
            get
            {
                if (patientTypeIdOverExpend == 0)
                {
                    patientTypeIdOverExpend = GetId(PATIENT_TYPE_CODE__OVER_EXPEND);
                }
                return patientTypeIdOverExpend;
            }
            set
            {
                patientTypeIdOverExpend = value;
            }
        }

        private static List<HIS_PATIENT_TYPE> data;
        public static List<HIS_PATIENT_TYPE> DATA
        {
            get
            {
                if (data == null)
                {
                    data = new HisPatientTypeGet().Get(new HisPatientTypeFilterQuery());
                }
                return data;
            }
            set
            {
                data = value;
            }
        }

        private static List<HIS_PATIENT_TYPE> noCoPayment;
        public static List<HIS_PATIENT_TYPE> NO_CO_PAYMENT
        {
            get
            {
                if (noCoPayment == null)
                {
                    noCoPayment = HisPatientTypeCFG.DATA.Where(o => o.IS_COPAYMENT == null || o.IS_COPAYMENT != ManagerConstant.IS_TRUE).ToList();
                }
                return noCoPayment;
            }
            set
            {
                noCoPayment = value;
            }
        }

        private static long GetId(string code)
        {
            long result = -1;//de chi thuc hien load 1 lan
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisPatientType.HisPatientTypeGet().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
            return result;
        }

        public static void Reload()
        {
            var tmp = new HisPatientTypeGet().Get(new HisPatientTypeFilterQuery()); ;
            data = tmp;
            var payment = DATA.Where(o => o.IS_COPAYMENT == null || o.IS_COPAYMENT != ManagerConstant.IS_TRUE).ToList();
            noCoPayment = payment;
            var idHospitalFee = GetId(PATIENT_TYPE_CODE__HOSPITAL_FEE);
            var idService = GetId(PATIENT_TYPE_CODE__SERVICE);
            var idOverExpend = GetId(PATIENT_TYPE_CODE__OVER_EXPEND);

            patientTypeIdHospitalFee = idHospitalFee;
            patientTypeIdService = idService;
            patientTypeIdOverExpend = idOverExpend;
        }
    }
}
