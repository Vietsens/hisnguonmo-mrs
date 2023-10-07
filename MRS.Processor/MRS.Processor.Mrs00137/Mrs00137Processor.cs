using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisDepartmentTran;
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

namespace MRS.Processor.Mrs00137
{
    public class Mrs00137Processor : AbstractProcessor
    {
        Mrs00137Filter castFilter = null; 

        decimal? EXAM_AMOUNT = null; 
        decimal? EXAM_HEIN_TOTAL_PRICE = null; 
        decimal? TRAN_AMOUNT = null; 
        decimal? IN_TREAT_AMOUNT = null; 
        decimal? OUT_TREAT_AMOUNT = null; 
        decimal? TREAT_HEIN_TOTAL_PRICE = null; 

        List<V_HIS_TREATMENT> listTreatmentEXAM = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TREATMENT> listTreatmentTREAT = new List<V_HIS_TREATMENT>(); 
        List<V_HIS_TREATMENT> ListTreatmentOut; 
        List<V_HIS_TREATMENT> ListTreatmentIn; 
        List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran; 

        public Mrs00137Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00137Filter); 
        }

        protected override bool GetData()
        {
            bool result = false; 
            try
            {
                this.castFilter = (Mrs00137Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam(); 
                Inventec.Common.Logging.LogSystem.Debug("Bat dau get du lieu V_HIS_TREATMENT, V_HIS_DEPARTMENT_TRAN, MRS00137 Filter:" + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter)); 

                //Hồ sơ điều trị duyệt khóa HFS (ra viện)
                HisTreatmentViewFilterQuery treatFilterOut = new HisTreatmentViewFilterQuery(); 
                treatFilterOut.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM; 
                treatFilterOut.FEE_LOCK_TIME_TO = castFilter.TIME_TO; 
                ListTreatmentOut = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilterOut); 

                //Hồ sơ điều trị vào viện trong khoản thời gian
                HisTreatmentViewFilterQuery treatFilterIn = new HisTreatmentViewFilterQuery(); 
                treatFilterIn.IN_TIME_FROM = castFilter.TIME_FROM; 
                treatFilterIn.IN_TIME_TO = castFilter.TIME_TO; 
                ListTreatmentIn = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetView(treatFilterIn); 

                //Chuyen khoa (dung de tinh benh nhan noi tru)
                HisDepartmentTranViewFilterQuery departFilter = new HisDepartmentTranViewFilterQuery(); 
                departFilter.DEPARTMENT_IDs = MRS.MANAGER.Config.HisDepartmentCFG.HIS_DEPARTMENT_ID__LIST_INTERNAL_MEDICINE; 
                departFilter.DEPARTMENT_IN_TIME_FROM = castFilter.TIME_FROM; 
                departFilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO; 
                ListDepartmentTran = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(departFilter); 
                if (!paramGet.HasException)
                {
                    result = true; 
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu V_HIS_TREATMENT, V_HIS_DEPARTMENT_TRAN, MRS00137."); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected override bool ProcessData()
        {
            bool result = false; 
            try
            {
                ProcessListTreatAndDepartTran(ListTreatmentIn, ListTreatmentOut, ListDepartmentTran); 
                result = true; 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void ProcessListTreatAndDepartTran(
            List<V_HIS_TREATMENT> ListTreatmentIn,
            List<V_HIS_TREATMENT> ListTreatmentOut,
            List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran)
        {
            try
            {
                CommonParam paramGet = new CommonParam(); 

                //Tinh so luong thanh toan ra vien va so luong chuyen vien
                if (IsNotNullOrEmpty(ListTreatmentOut))
                {
                    ProcessListTreatmentOut(ListTreatmentOut, paramGet); 
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00137."); 
                }

                //Tinh so luong vao vien tu khoa kham benh
                if (IsNotNullOrEmpty(ListTreatmentIn))
                {
                    ProcessListTreatmentIn(ListTreatmentIn, paramGet); 
                }
                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00137."); 
                }

                //Tinh so tien benh nhan ngoai tru bhyt phai thanh toan
                ProcessPriceListTreatmentExam(paramGet); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00137."); 
                }

                //tinh so tien benh nhan noi tru bhyt phai thanh toan
                ProcessPriceListTreatmentTreat(paramGet); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00137."); 
                }

                //Tinh so luong chuyen noi tru vao (chuyen vao tu khoa kham benh va vao truc tiep)
                ProcessListDepartmentTran(ListDepartmentTran, paramGet); 

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, Mrs00137."); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                EXAM_AMOUNT = null; 
                EXAM_HEIN_TOTAL_PRICE = null; 
                TRAN_AMOUNT = null; 
                IN_TREAT_AMOUNT = null; 
                OUT_TREAT_AMOUNT = null; 
                TREAT_HEIN_TOTAL_PRICE = null; 

            }
        }

        private void ProcessListDepartmentTran(List<V_HIS_DEPARTMENT_TRAN> ListDepartmentTran, CommonParam paramGet)
        {
            try
            {
                if (IsNotNullOrEmpty(ListDepartmentTran))
                {
                    IN_TREAT_AMOUNT = 0; 
                    var listTreatmentId = ListDepartmentTran.Select(s => s.TREATMENT_ID).Distinct().ToList(); 
                    var listPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetViewByTreatmentIds(listTreatmentId); 
                    if (IsNotNullOrEmpty(listPatientTypeAlter))
                    {
                        foreach (var treatmentId in listTreatmentId)
                        {
                            HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery(); 
                            appFilter.TREATMENT_ID = treatmentId;
                            var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last(); 
                            if (IsNotNull(currentPatientTypeAlter) && currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                var hisPatientAlters = listPatientTypeAlter.Where(o => o.TREATMENT_ID == treatmentId && o.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT).ToList(); 
                                if (IsNotNullOrEmpty(hisPatientAlters))
                                {
                                    var firstPatientTypeAlter = hisPatientAlters.OrderBy(o => o.LOG_TIME).FirstOrDefault(); 
                                    if (IsNotNull(firstPatientTypeAlter) && firstPatientTypeAlter.LOG_TIME >= castFilter.TIME_FROM && firstPatientTypeAlter.LOG_TIME <= castFilter.TIME_TO)
                                    {
                                        IN_TREAT_AMOUNT += 1; 
                                    }
                                }
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

        private void ProcessPriceListTreatmentTreat(CommonParam paramGet)
        {
            try
            {
                if (IsNotNullOrEmpty(listTreatmentTREAT))
                {
                    TREAT_HEIN_TOTAL_PRICE = 0; 
                    int start = 0; 
                    int count = listTreatmentTREAT.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listTreatmentSkip = listTreatmentTREAT.Skip(start).Take(limit).ToList(); 
                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery(); 
                        feeFilter.IDs = listTreatmentSkip.Select(s => s.ID).ToList();
                        var ListTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(feeFilter); 
                        if (!paramGet.HasException)
                        {
                            if (IsNotNullOrEmpty(ListTreatmentFee))
                            {
                                TREAT_HEIN_TOTAL_PRICE += ListTreatmentFee.Sum(s => s.TOTAL_HEIN_PRICE.Value); 
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessPriceListTreatmentExam(CommonParam paramGet)
        {
            try
            {
                if (IsNotNullOrEmpty(listTreatmentEXAM))
                {
                    EXAM_HEIN_TOTAL_PRICE = 0; 
                    int start = 0; 
                    int count = listTreatmentEXAM.Count; 
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        var listTreatmentSkip = listTreatmentEXAM.Skip(start).Take(limit).ToList(); 
                        HisTreatmentFeeViewFilterQuery feeFilter = new HisTreatmentFeeViewFilterQuery(); 
                        feeFilter.IDs = listTreatmentSkip.Select(s => s.ID).ToList();
                        var ListTreatmentFee = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetFeeView(feeFilter); 
                        if (!paramGet.HasException)
                        {
                            if (IsNotNullOrEmpty(ListTreatmentFee))
                            {
                                EXAM_HEIN_TOTAL_PRICE += ListTreatmentFee.Sum(s => s.TOTAL_HEIN_PRICE.Value); 
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        private void ProcessListTreatmentIn(List<V_HIS_TREATMENT> ListTreatmentIn, CommonParam paramGet)
        {
            try
            {
                EXAM_AMOUNT = 0; 
                List<V_HIS_DEPARTMENT_TRAN> hisDepartmentTranIns = new List<V_HIS_DEPARTMENT_TRAN>(); 
                int start = 0; 
                int count = ListTreatmentIn.Count; 
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    var hisTreatments = ListTreatmentIn.Skip(start).Take(limit).ToList(); 
                    HisDepartmentTranViewFilterQuery depaTranFilter = new HisDepartmentTranViewFilterQuery(); 
                    depaTranFilter.TREATMENT_IDs = hisTreatments.Select(s => s.ID).ToList(); 
                    //depaTranFilter.IN_OUT = IMSys.DbConfig.HIS_RS.HIS_DEPARTMENT_TRAN.IN_OUT__IN; 
                    depaTranFilter.DEPARTMENT_IN_TIME_FROM = castFilter.TIME_FROM; 
                    depaTranFilter.DEPARTMENT_IN_TIME_TO = castFilter.TIME_TO; 
                    depaTranFilter.DEPARTMENT_IDs = MRS.MANAGER.Config.HisDepartmentCFG.HIS_DEPARTMENT_ID__EXAM; 
                    var listData = new MOS.MANAGER.HisDepartmentTran.HisDepartmentTranManager(paramGet).GetView(depaTranFilter); 
                    if (!paramGet.HasException)
                    {
                        if (IsNotNullOrEmpty(listData))
                        {
                            hisDepartmentTranIns.AddRange(listData); 
                        }
                    }
                    start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 
                }
                if (IsNotNullOrEmpty(hisDepartmentTranIns))
                {
                    foreach (var treatment in ListTreatmentIn)
                    {
                        var departTrans = hisDepartmentTranIns.Where(o => o.TREATMENT_ID == treatment.ID).ToList(); 
                        if (IsNotNullOrEmpty(departTrans))
                        {
                            HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                            appFilter.TREATMENT_ID = treatment.ID;
                            var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last(); 
                            if (IsNotNull(currentPatientTypeAlter) && currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                            {
                                EXAM_AMOUNT += 1; 
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

        private void ProcessListTreatmentOut(List<V_HIS_TREATMENT> ListTreatmentOut, CommonParam paramGet)
        {
            try
            {
                TRAN_AMOUNT = 0; 
                OUT_TREAT_AMOUNT = 0; 
                foreach (var treatment in ListTreatmentOut)
                {
                    HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery(); 
                    appFilter.TREATMENT_ID = treatment.ID;
                    var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o=>o.LOG_TIME).ThenBy(p=>p.ID).Last(); 
                    if (IsNotNull(currentPatientTypeAlter))
                    {
                        if (currentPatientTypeAlter.PATIENT_TYPE_ID == MRS.MANAGER.Config.HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT)
                        {
                            if (treatment.TREATMENT_END_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN)
                            {
                                TRAN_AMOUNT += 1; 
                            }
                            if (currentPatientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                OUT_TREAT_AMOUNT += 1; 
                                listTreatmentTREAT.Add(treatment); 
                            }
                            else
                            {
                                listTreatmentEXAM.Add(treatment); 
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

        private void ProcessDicSingleData(Dictionary<string, object> dicSingData)
        {
            try
            {
                if (IsNotNull(dicSingData))
                {
                    dicSingData.Add("EXAM_AMOUNT", EXAM_AMOUNT); 
                    dicSingData.Add("EXAM_HEIN_TOTAL_PRICE", EXAM_HEIN_TOTAL_PRICE); 
                    dicSingData.Add("TRAN_AMOUNT", TRAN_AMOUNT); 
                    dicSingData.Add("IN_TREAT_AMOUNT", IN_TREAT_AMOUNT); 
                    dicSingData.Add("OUT_TREAT_AMOUNT", OUT_TREAT_AMOUNT); 
                    dicSingData.Add("TREAT_HEIN_TOTAL_PRICE", TREAT_HEIN_TOTAL_PRICE); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                #region Cac the Single
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM)); 
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO)); 
                }
                #endregion
                ProcessDicSingleData(dicSingleTag); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }
    }
}
