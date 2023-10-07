using Inventec.Common.Logging;

using MOS.EFMODEL.DataModels;
using SDA.EFMODEL.DataModels;
using System;

namespace MOS.MANAGER.Config
{
    class HisTreatmentResultCFG
    {
        //ma ket qua dieu tri mac dinh doi voi loai la kham
        private const string TREATMENT_RESULT_CODE__DEFAULT_OF_EXAM = "MOS.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.DEFAULT_OF_EXAM";
        //ma ket qua dieu tri la 'tu vong'
        private const string TREATMENT_RESULT_CODE__DEATH = "EXE.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.DEATH";
        //ma ket qua dieu tri la 'nang hon'
        private const string TREATMENT_RESULT_CODE__HEAVIER = "EXE.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.HEAVIER";
        //ma ket qua dieu tri la 'khong thay doi'
        private const string TREATMENT_RESULT_CODE__CONSTANT = "EXE.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.CONSTANT";
        //ma ket qua dieu tri la 'do~'
        private const string TREATMENT_RESULT_CODE__REDUCE = "EXE.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.REDUCE";
        //ma ket qua dieu tri la 'khoi'
        private const string TREATMENT_RESULT_CODE__CURED = "EXE.HIS_TREATMENT_RESULT.TREATMENT_RESULT_CODE.CURED";

        private static long treatmentResultIdDefaultOfExam;
        public static long TREATMENT_RESULT_ID__DEFAULT_OF_EXAM
        {
            get
            {
                if (treatmentResultIdDefaultOfExam == 0)
                {
                    treatmentResultIdDefaultOfExam = GetId(TREATMENT_RESULT_CODE__DEFAULT_OF_EXAM);
                }
                return treatmentResultIdDefaultOfExam;
            }
            set
            {
                treatmentResultIdDefaultOfExam = value;
            }
        }

        private static long treatmentResultIdDeath;
        public static long TREATMENT_RESULT_ID__DEATH
        {
            get
            {
                if (treatmentResultIdDeath == 0)
                {
                    treatmentResultIdDeath = GetId(TREATMENT_RESULT_CODE__DEATH);
                }
                return treatmentResultIdDeath;
            }
            set
            {
                treatmentResultIdDeath = value;
            }
        }

        private static long treatmentResultIdHeavier;
        public static long TREATMENT_RESULT_ID__HEAVIER
        {
            get
            {
                if (treatmentResultIdHeavier == 0)
                {
                    treatmentResultIdHeavier = GetId(TREATMENT_RESULT_CODE__HEAVIER);
                }
                return treatmentResultIdHeavier;
            }
            set
            {
                treatmentResultIdHeavier = value;
            }
        }

        private static long treatmentResultIdConstant;
        public static long TREATMENT_RESULT_ID__CONSTANT
        {
            get
            {
                if (treatmentResultIdConstant == 0)
                {
                    treatmentResultIdConstant = GetId(TREATMENT_RESULT_CODE__CONSTANT);
                }
                return treatmentResultIdConstant;
            }
            set
            {
                treatmentResultIdConstant = value;
            }
        }

        private static long treatmentResultIdReduce;
        public static long TREATMENT_RESULT_ID__REDUCE
        {
            get
            {
                if (treatmentResultIdReduce == 0)
                {
                    treatmentResultIdReduce = GetId(TREATMENT_RESULT_CODE__REDUCE);
                }
                return treatmentResultIdReduce;
            }
            set
            {
                treatmentResultIdReduce = value;
            }
        }

        private static long treatmentResultIdCured;
        public static long TREATMENT_RESULT_ID__CURED
        {
            get
            {
                if (treatmentResultIdCured == 0)
                {
                    treatmentResultIdCured = GetId(TREATMENT_RESULT_CODE__CURED);
                }
                return treatmentResultIdCured;
            }
            set
            {
                treatmentResultIdCured = value;
            }
        }
        private static long GetId(string code)
        {
            long result = 0;
            try
            {
                string value = ConfigUtil.GetStrConfig(code);
                var data = new HisTreatmentResult.HisTreatmentResultGet().GetByCode(value);
                if (data == null) throw new ArgumentNullException(code);
                result = data.ID;
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
            var idDefaultOfExam = GetId(TREATMENT_RESULT_CODE__DEFAULT_OF_EXAM);
            var idDeath = GetId(TREATMENT_RESULT_CODE__DEATH);
            var idHeavier = GetId(TREATMENT_RESULT_CODE__HEAVIER);
            var idConstant = GetId(TREATMENT_RESULT_CODE__CONSTANT);
            var idReduce = GetId(TREATMENT_RESULT_CODE__REDUCE);
            var idCured = GetId(TREATMENT_RESULT_CODE__CURED);

            treatmentResultIdDefaultOfExam = idDefaultOfExam;
            treatmentResultIdDeath = idDeath;
            treatmentResultIdHeavier = idHeavier;
            treatmentResultIdConstant = idConstant;
            treatmentResultIdReduce = idReduce;
            treatmentResultIdCured = idCured;
        }
    }
}
