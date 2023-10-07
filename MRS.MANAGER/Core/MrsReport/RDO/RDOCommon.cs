using Inventec.Common.Logging;
using Inventec.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using MRS.MANAGER.Config;
using MOS.EFMODEL.DataModels;

namespace MRS.MANAGER.Core.MrsReport.RDO
{
    public class RDOCommon
    {

        public static string GenerateAddress(string provinceName, string districtName, string communeName, string villageStreet, string houseNumber)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(provinceName))
                {
                    result = result + provinceName;
                }
                if (!String.IsNullOrWhiteSpace(districtName))
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result = result + ",";
                    }
                    result = result + districtName;
                }
                if (!String.IsNullOrWhiteSpace(communeName))
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result = result + ",";
                    }
                    result = result + communeName;
                }
                if (!String.IsNullOrWhiteSpace(villageStreet))
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result = result + ",";
                    }
                    result = result + villageStreet;
                }
                if (!String.IsNullOrWhiteSpace(houseNumber))
                {
                    if (!String.IsNullOrWhiteSpace(result))
                    {
                        result = result + ",";
                    }
                    result = result + houseNumber;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public static string ConvertTimeToDateString(long? time)
        {
            string result = "";
            try
            {
                if (time.HasValue)
                {
                    result = Inventec.Common.DateTime.Convert.TimeNumberToDateString(time.Value);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public static string ConvertTimeToTimeString(long? time)
        {
            string result = "";
            try
            {
                if (time.HasValue)
                {
                    result = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(time.Value);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = "";
            }
            return result;
        }

        public static void TickPatientType(ref string hein, ref string free, ref string service, MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER patientType)
        {
            try
            {
                if (patientType != null)
                {
                    if (patientType.PATIENT_TYPE_ID == MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {
                        hein = RDOConstant.TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__IS_FREE == patientType.PATIENT_TYPE_ID)
                    {
                        free = RDOConstant.TickSymbol;
                    }
                    else
                    {
                        service = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        //public static void GetCurrentPatientTypeAlter(long treatmentId, long instructionTime, ref MOS.EFMODEL.DataModels.V_HIS_PATIENT_TYPE_ALTER hisPatientTypeAlter)
        //{
        //    try
        //    {

        //        MOS.Filter.HisPatientTypeAlterViewAppliedFilter filter = new MOS.Filter.HisPatientTypeAlterViewAppliedFilter();
        //        filter.TreatmentId = treatmentId;
        //        filter.InstructionTime = instructionTime;
        //        MRS.MANAGER.His.HisPatientTypeAlter.HisPatientTypeAlterGet mngUIConcrete = new MRS.MANAGER.His.HisPatientTypeAlter.HisPatientTypeAlterGet(new CommonParam());
        //        hisPatientTypeAlter = mngUIConcrete.GetApplied(filter);
        //    }
        //    catch (Exception ex)
        //    {
        //        Inventec.Common.Logging.LogSystem.Warn(ex);
        //    }
        //}

        //public static void GetCurrentTreatment(long? treatmentId, ref MOS.EFMODEL.DataModels.V_HIS_TREATMENT hisTreatment)
        // {
        //     try
        //     {
        //         MOS.Filter.HisTreatmentViewFilter filter = new MOS.Filter.HisTreatmentViewFilter();
        //         filter.ID = treatmentId;
        //         MRS.MANAGER.His.HisTreatment.HisTreatmentGet mngUIConcrete = new MRS.MANAGER.His.HisTreatment.HisTreatmentGet(new CommonParam());
        //         hisTreatment = mngUIConcrete.GetView(filter).FirstOrDefault();
        //     }
        //     catch (Exception ex)
        //     {
        //         Inventec.Common.Logging.LogSystem.Warn(ex);
        //         hisTreatment = null;
        //     }
        // }

        public static void TickTreatmentEndType(ref string hospitalizedIn, long? treatmentEndTypeId)
        {
            try
            {
                if (treatmentEndTypeId.HasValue)
                {
                    if (HisTreatmentEndTypeCFG.TREATMENT_END_TYPE_ID__HOME == treatmentEndTypeId)
                    {
                        hospitalizedIn = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void TickTranPatiForm(ref string equal, ref string down, ref string up, long? tranPatiFormId)
        {
            try
            {
                if (tranPatiFormId.HasValue && tranPatiFormId > 0)
                {
                    if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__EQUAL == tranPatiFormId.Value)
                    {
                        equal = RDOConstant.TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__UP_DOWN == tranPatiFormId.Value)
                    {
                        down = RDOConstant.TickSymbol;
                    }
                    else if (MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NEXT == tranPatiFormId.Value || MRS.MANAGER.Config.HisTranPatiFormCFG.HIS_TRAN_PATI_FORM_ID__DOWN_UP_NON_NEXT == tranPatiFormId.Value)
                    {
                        up = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void TickTreatmentType(ref string treatmentType_Exam, ref string treatmentType_TreatIn, ref string treatmentType_TreatOut, long? treatmentTypeId)
        {
            try
            {
                if (treatmentTypeId.HasValue && treatmentTypeId > 0)
                {
                    if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__KHAM == treatmentTypeId.Value)
                    {
                        treatmentType_Exam = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU == treatmentTypeId.Value)
                    {
                        treatmentType_TreatIn = RDOConstant.TickSymbol;
                    }
                    else if (IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU == treatmentTypeId.Value)
                    {
                        treatmentType_TreatOut = RDOConstant.TickSymbol;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void TickHeinTreatmentType(ref string heinTreatmentType_Exam, ref string heinTreatmentType_Treat, string heinTreatmentTypeCode)
        {
            try
            {
                if (!String.IsNullOrEmpty(heinTreatmentTypeCode))
                {
                    if (MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM == heinTreatmentTypeCode)
                    {
                        heinTreatmentType_Exam = RDOConstant.TickSymbol;
                    }
                    else if (MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT == heinTreatmentTypeCode)
                    {
                        heinTreatmentType_Treat = RDOConstant.TickSymbol;
                    }
                    else
                    {
                        heinTreatmentType_Exam = heinTreatmentType_Exam = "";
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void GenerateAge(ref int? female, ref int? male, long? dob, long? genderId)
        {
            try
            {
                if (dob.HasValue && genderId.HasValue)
                {
                    int age = Inventec.Common.DateTime.Calculation.Age(dob.Value);
                    if (IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE == genderId.Value)
                    {
                        female = age;
                    }
                    else
                    {
                        male = age;
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void TickTreatment(ref string result, short? isTreatment)
        {
            try
            {
                if (isTreatment.HasValue && isTreatment.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    result = RDOConstant.TickSymbol;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static void TickSpeciality(ref string result, short? isSpeciality)
        {
            try
            {
                if (isSpeciality.HasValue && isSpeciality.Value == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                {
                    result = RDOConstant.TickSymbol;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        public static string GenerateHeinCardSeparate(string heinCardNumber)
        {
            string result = "";
            try
            {
                if (!String.IsNullOrWhiteSpace(heinCardNumber) && heinCardNumber.Length == 15)
                {
                    string separateSymbol = "-";
                    result = new StringBuilder().Append(heinCardNumber.Substring(0, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(2, 1)).Append(separateSymbol).Append(heinCardNumber.Substring(3, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(5, 2)).Append(separateSymbol).Append(heinCardNumber.Substring(7, 3)).Append(separateSymbol).Append(heinCardNumber.Substring(10, 5)).ToString();
                }
                else
                {
                    result = heinCardNumber;
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = heinCardNumber;
            }
            return result;
        }

        public static int? CalculateAge(long ageNumber)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ageNumber) ?? DateTime.MinValue;
                TimeSpan diff = DateTime.Now - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;
                int thang = newDate.Month - 1;
                int ngay = newDate.Day - 1;
                int gio = newDate.Hour;
                int phut = newDate.Minute;
                int giay = newDate.Second;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                    //if (thang > 0)
                    //{
                    //    tuoi = thang.ToString();
                    //    cboAge = "Tháng";
                    //}
                    //else
                    //{
                    //    if (ngay > 0)
                    //    {
                    //        tuoi = ngay.ToString();
                    //        cboAge = "ngày";
                    //    }
                    //    else
                    //    {
                    //        tuoi = "";
                    //        cboAge = "Giờ";
                    //    }
                    //}
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }

        public static int? CalculateAge(long ngaySinh, long ngayVao)
        {
            int tuoi;
            try
            {
                DateTime dtNgSinh = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngaySinh) ?? DateTime.MinValue;
                DateTime dtNgVao = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(ngayVao) ?? DateTime.MinValue;
                TimeSpan diff = dtNgVao - dtNgSinh;
                long tongsogiay = diff.Ticks;
                if (tongsogiay < 0)
                {
                    tuoi = 0;
                    return 0;
                }
                DateTime newDate = new DateTime(tongsogiay);

                int nam = newDate.Year - 1;

                if (nam > 0)
                {
                    tuoi = nam;
                }
                else
                {
                    tuoi = 0;
                }
                return tuoi;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Warn(ex);
                return null;
            }
        }
    }
}
