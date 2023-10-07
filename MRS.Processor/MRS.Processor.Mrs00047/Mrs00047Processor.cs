using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00047
{
    public class Mrs00047Processor : AbstractProcessor
    {
        Mrs00047Filter castFilter = null;
        List<Mrs00047RDO> ListRdo = new List<Mrs00047RDO>();
        List<V_HIS_TREATMENT> ListTreatment = new List<V_HIS_TREATMENT>();

        string department_name;

        public Mrs00047Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00047Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00047Filter)this.reportFilter);
                LoadDataToRam();
                result = true;
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
                ProcessListTreatment();
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListTreatment()
        {
            try
            {
                if (ListTreatment != null && ListTreatment.Count > 0)
                {
                    CommonParam paramGet = new CommonParam();
                    ListTreatment = ListTreatment.Where(o => o.IS_PAUSE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && o.TREATMENT_END_TYPE_ID != null && o.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();

                    if (castFilter.DEPARTMENT_ID.HasValue)
                    {
                        ListTreatment = ListTreatment.Where(o => o.END_DEPARTMENT_ID == castFilter.DEPARTMENT_ID.Value).ToList();
                        if (IsNotNullOrEmpty(ListTreatment))
                        {
                            department_name = ListTreatment.First().END_DEPARTMENT_NAME;
                        }
                    }

                    if (ListTreatment != null && ListTreatment.Count > 0)
                    {

                        int start = 0;
                        int count = ListTreatment.Count;
                        while (count > 0)
                        {
                            int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM);
                            List<V_HIS_TREATMENT> treatments = ListTreatment.Skip(start).Take(limit).ToList();

                            HisTreatmentFeeViewFilterQuery treatmentFeeFilter = new HisTreatmentFeeViewFilterQuery();
                            treatmentFeeFilter.IDs = treatments.Select(s => s.ID).ToList();
                            var listTreatmentFee = new HisTreatmentManager(paramGet).GetFeeView(treatmentFeeFilter);
                            if (listTreatmentFee != null && listTreatmentFee.Count > 0)
                            {
                                ProcessListTreatmentFee(paramGet, listTreatmentFee);
                            }
                            start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                            count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        }
                        if (paramGet.HasException)
                        {
                            throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessListTreatmentFee(CommonParam paramGet, List<V_HIS_TREATMENT_FEE> listTreatmentFee)
        {
            try
            {
                foreach (var treatmentFee in listTreatmentFee)
                {
                    Mrs00047RDO rdo = new Mrs00047RDO();
                    rdo.PATIENT_CODE = treatmentFee.TDL_PATIENT_CODE;
                    rdo.PATIENT_NAME = treatmentFee.TDL_PATIENT_NAME;
                    rdo.VIR_TOTAL_PRICE = treatmentFee.TOTAL_PATIENT_PRICE ?? 0;
                    rdo.TOTAL_PAID = (treatmentFee.TOTAL_DEPOSIT_AMOUNT ?? 0) + (treatmentFee.TOTAL_BILL_AMOUNT ?? 0) - (treatmentFee.TOTAL_BILL_TRANSFER_AMOUNT ?? 0) - (treatmentFee.TOTAL_REPAY_AMOUNT ?? 0) - (treatmentFee.TOTAL_BILL_EXEMPTION ?? 0);
                    rdo.TOTAL_EXEMPTION = treatmentFee.TOTAL_BILL_EXEMPTION ?? 0;

                    ListRdo.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void CheckInOrOutTreatment(CommonParam paramGet, V_HIS_TREATMENT_FEE treatmentFee, Mrs00047RDO rdo)
        {
            try
            {
                var currentPatientTypeAlter = new HisPatientTypeAlterManager(paramGet).GetApplied(treatmentFee.ID, null);
                if (IsNotNull(currentPatientTypeAlter))
                {
                    if (currentPatientTypeAlter.TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU)
                    {
                        rdo.TOTAL_NOITRU = rdo.VIR_TOTAL_PRICE - (rdo.TOTAL_PAID + rdo.TOTAL_EXEMPTION);
                    }
                    else
                    {
                        rdo.TOTAL_NGOAITRU = rdo.VIR_TOTAL_PRICE - (rdo.TOTAL_PAID + rdo.TOTAL_EXEMPTION);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
                treatmentFilter.CREATE_TIME_FROM = castFilter.TIME_FROM;
                treatmentFilter.CREATE_TIME_TO = castFilter.TIME_TO;
                treatmentFilter.IS_PAUSE = true;
                ListTreatment = new HisTreatmentManager().GetView(treatmentFilter);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListTreatment.Clear();
            }
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

                dicSingleTag.Add("DEPARTMENT_NAME", department_name);

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
