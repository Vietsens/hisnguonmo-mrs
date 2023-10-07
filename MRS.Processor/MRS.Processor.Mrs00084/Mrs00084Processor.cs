using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using FlexCel.Report;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00084
{
    public class Mrs00084Processor : AbstractProcessor
    {
        Mrs00084Filter castFilter = null;
        List<Mrs00084RDO> ListRdo = new List<Mrs00084RDO>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        CommonParam paramGet = new CommonParam();
        List<V_HIS_TREATMENT> hisTreatment;

        public Mrs00084Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00084Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00084Filter)this.reportFilter);

                HisTreatmentViewFilterQuery filter = new HisTreatmentViewFilterQuery();
                filter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                filter.CREATE_TIME_TO = castFilter.TIME_TO;

                hisTreatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(filter);

                if (!paramGet.HasException)
                {
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(hisTreatment))
                {
                    int start = 0;
                    int count = hisTreatment.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_TREATMENT> treatments = hisTreatment.Skip(start).Take(limit).ToList();
                        ProcessListTreatment(paramGet, treatments);
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                    }
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private HIS_SERVICE_REQ req(V_HIS_SERE_SERV o)
        {
            HIS_SERVICE_REQ result = new HIS_SERVICE_REQ();
            try
            {
                if (dicServiceReq.ContainsKey(o.SERVICE_REQ_ID ?? 0))
                {
                    result = dicServiceReq[o.SERVICE_REQ_ID ?? 0];
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new HIS_SERVICE_REQ();
            }
            return result;
        }

        private void ProcessListTreatment(CommonParam paramGet, List<V_HIS_TREATMENT> listTreatment)
        {
            try
            {
                HisPatientTypeAlterViewFilterQuery ptaFilter = new HisPatientTypeAlterViewFilterQuery();
                ptaFilter.TREATMENT_IDs = listTreatment.Select(s => s.ID).ToList();
                List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(ptaFilter);

                HisSereServViewFilterQuery ssvFilter = new HisSereServViewFilterQuery();
                ssvFilter.TREATMENT_IDs = listTreatment.Select(s => s.ID).ToList();
                List<V_HIS_SERE_SERV> hisSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssvFilter);

                HisServiceReqFilterQuery reqFilter = new HisServiceReqFilterQuery();
                reqFilter.TREATMENT_IDs = listTreatment.Select(s => s.ID).ToList();
                dicServiceReq = new MOS.MANAGER.HisServiceReq.HisServiceReqManager(paramGet).Get(reqFilter).ToDictionary(o => o.ID);

                Dictionary<V_HIS_TREATMENT, InfoPatient> dicTreatment = new Dictionary<V_HIS_TREATMENT, InfoPatient>();

                if (hisPatientTypeAlter != null && hisPatientTypeAlter.Count > 0)
                {
                    foreach (var treatment in listTreatment)
                    {
                        InfoPatient infoPatient = new InfoPatient();
                        if (CheckHeinCardNumber(paramGet, hisPatientTypeAlter, treatment, infoPatient))
                        {
                            infoPatient.treatmentCode = treatment.TREATMENT_CODE;
                            infoPatient.patientCode = treatment.TDL_PATIENT_CODE;
                            infoPatient.patientName = treatment.TDL_PATIENT_NAME;
                            infoPatient.vir_address = treatment.TDL_PATIENT_ADDRESS;
                            CalcuatorAge(infoPatient, treatment);
                            dicTreatment.Add(treatment, infoPatient);
                        }
                    }
                    if (dicTreatment.Count > 0)
                    {
                        ProcessDicTreatment(hisSereServ, dicTreatment);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDicTreatment(List<V_HIS_SERE_SERV> hisSereServs, Dictionary<V_HIS_TREATMENT, InfoPatient> dicTreatment)
        {
            try
            {
                foreach (var dic in dicTreatment)
                {
                    var listSereServ = hisSereServs.Where(o => o.TDL_TREATMENT_ID == dic.Key.ID && o.AMOUNT > 0).ToList();
                    if (listSereServ != null && listSereServ.Count > 0)
                    {
                        foreach (var sereserv in listSereServ)
                        {
                            string x = dic.Value.heincardnumber.Substring(0, 2) + dic.Value.heincardnumber.Substring(3, 1);
                            int v = int.Parse(castFilter.KEY_HEIN_CARD);
                            if (x == MOS.LibraryHein.Bhyt.HeinObject.HeinObjectBenefitStore.GetObjectCodeWithBenefitCodes()[v])
                            {
                                Mrs00084RDO rdo = new Mrs00084RDO();
                                rdo.TREATMENT_CODE = dic.Value.treatmentCode;
                                rdo.HEIN_CARD_NUMBER = dic.Value.heincardnumber;
                                rdo.PATIENT_CODE = dic.Value.patientCode;
                                rdo.PATIENT_NAME = dic.Value.patientName;
                                rdo.VIR_ADDRESS = dic.Value.vir_address;
                                rdo.TREATMENT_ID = dic.Key.ID;
                                rdo.MALE_AGE = dic.Value.maleage;
                                rdo.MALE_YEAR = dic.Value.maleyear;
                                rdo.FEMALE_AGE = dic.Value.femaleage;
                                rdo.FEMALE_YEAR = dic.Value.femaleyear;
                                rdo.SERVICE_CODE = sereserv.TDL_SERVICE_CODE;
                                rdo.SERVICE_NAME = sereserv.TDL_SERVICE_NAME;
                                rdo.AMOUNT = sereserv.AMOUNT;
                                rdo.EXECUTE_ROOM_NAME = sereserv.EXECUTE_ROOM_NAME;
                                rdo.EXECUTE_USERNAME = req(sereserv).EXECUTE_USERNAME;
                                rdo.VIR_TOTAL_PATIENT_PRICE = sereserv.VIR_TOTAL_PATIENT_PRICE ?? 0;
                                rdo.VIR_TOTAL_HEIN_PRICE = sereserv.VIR_TOTAL_HEIN_PRICE ?? 0;
                                ListRdo.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private bool CheckHeinCardNumber(CommonParam paramGet, List<V_HIS_PATIENT_TYPE_ALTER> PatientTypeAlters, V_HIS_TREATMENT treatment, InfoPatient info)
        {
            bool result = false;
            try
            {
                var PatientTypeAlter = PatientTypeAlters.Where(o => o.TREATMENT_ID == treatment.ID).ToList();
                if (PatientTypeAlter != null && PatientTypeAlter.Count > 0)
                {
                    PatientTypeAlter = PatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                    if (PatientTypeAlter[0].PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                    {

                        if (PatientTypeAlter[0].HEIN_CARD_NUMBER != null)
                        {
                            info.heincardnumber = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.GenerateHeinCardSeparate(PatientTypeAlter[0].HEIN_CARD_NUMBER);
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void CalcuatorAge(InfoPatient info, V_HIS_TREATMENT treatment)
        {
            try
            {
                int? tuoi = MRS.MANAGER.Core.MrsReport.RDO.RDOCommon.CalculateAge(treatment.TDL_PATIENT_DOB);
                if (tuoi >= 0)
                {
                    if (treatment.TDL_PATIENT_GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE)
                    {
                        info.maleage = (tuoi >= 1) ? tuoi : 1;
                        info.maleyear = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                    else
                    {
                        info.femaleage = (tuoi >= 1) ? tuoi : 1;
                        info.femaleyear = ProcessYearDob(treatment.TDL_PATIENT_DOB);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string ProcessYearDob(long dob)
        {
            try
            {
                if (dob > 0)
                {
                    return dob.ToString().Substring(0, 4);
                }
                return null;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }
        }

        private bool checkKeyHeinCard(string heincardnumber)
        {
            bool result = false;
            try
            {
                if (!String.IsNullOrEmpty(heincardnumber))
                {
                    if (heincardnumber.Contains(castFilter.KEY_HEIN_CARD))
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("CREATE_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ListRdo = ListRdo.OrderBy(o => o.TREATMENT_ID).ThenBy(t => t.SERVICE_CODE).ToList();

                objectTag.AddObjectData(store, "Report", ListRdo);
                objectTag.SetUserFunction(store, "FuncSameTitleCol1", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol2", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol3", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol4", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol5", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol6", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol7", new CustomerFuncMergeSameData());
                objectTag.SetUserFunction(store, "CountTreatment", new CountTreatment());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    internal class InfoPatient
    {
        internal string treatmentCode { get; set; }
        internal string patientCode { get; set; }
        internal string patientName { get; set; }
        internal string heincardnumber { get; set; }
        internal string vir_address { get; set; }
        internal int? maleage { get; set; }
        internal int? femaleage { get; set; }
        internal string maleyear { get; set; }
        internal string femaleyear { get; set; }
    }
    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long treatmentId;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            bool result = false;
            try
            {
                long treatmentIdGet = Convert.ToInt64(parameters[0]);

                if (treatmentIdGet > 0)
                {
                    if (treatmentIdGet == treatmentId)
                    {
                        return true;
                    }
                    else
                    {
                        treatmentId = treatmentIdGet;
                        return false;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return result;
        }
    }
    class CountTreatment : TFlexCelUserFunction
    {
        long treatmentId;
        long num_order = 0;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length <= 0)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                long treatmentIdGet = Convert.ToInt64(parameters[0]);

                if (treatmentIdGet > 0)
                {
                    if (treatmentIdGet == treatmentId)
                    {

                    }
                    else
                    {
                        treatmentId = treatmentIdGet;
                        num_order = num_order + 1;
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return num_order;
        }
    }
}
