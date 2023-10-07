using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisDepartment;
using FlexCel.Report; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisPatientTypeAlter; 
using MOS.MANAGER.HisTreatment; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisDepartmentTran; 

namespace MRS.Processor.Mrs00298
{
    class Mrs00298Processor : AbstractProcessor
    {
        Mrs00298Filter filter = null;
        List<V_HIS_TREATMENT_FEE> ListTreatment = new List<V_HIS_TREATMENT_FEE>(); 
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00298RDO> listRdo = new List<Mrs00298RDO>(); 

        public Mrs00298Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00298Filter); 
        }

        protected override bool GetData()///
        {
            bool result = true; 
            try
            {
                this.filter = (Mrs00298Filter)this.reportFilter;
                HisTreatmentFeeViewFilterQuery filterTreatment = new HisTreatmentFeeViewFilterQuery();
                if (filter.TIME_TO.HasValue)
                {
                    filterTreatment.IN_TIME_TO = filter.TIME_TO;
                }
                else if (filter.TIME.HasValue)
                {
                    filterTreatment.IN_TIME_TO = filter.TIME;
                }
                filterTreatment.IS_PAUSE = false;
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                var listTreatmentSub = new HisTreatmentManager(paramGet).GetFeeView(filterTreatment); 
                ListTreatment.AddRange(listTreatmentSub);
                filterTreatment = new HisTreatmentFeeViewFilterQuery();
                if (filter.TIME_TO.HasValue)
                {
                    filterTreatment.IN_TIME_TO = filter.TIME_TO;
                }
                else if (filter.TIME.HasValue)
                {
                    filterTreatment.IN_TIME_TO = filter.TIME;
                }
                filterTreatment.IS_PAUSE = true;
                if (filter.TIME_FROM.HasValue)
                {
                    filterTreatment.OUT_TIME_FROM = filter.TIME_FROM;
                }
                else if (filter.TIME.HasValue)
                {
                    filterTreatment.OUT_TIME_FROM = filter.TIME;
                }
                filterTreatment.CLINICAL_IN_TIME_FROM = 1;
                var listTreatmentSub1 = new HisTreatmentManager(paramGet).GetFeeView(filterTreatment); 
                ListTreatment.AddRange(listTreatmentSub1); 

                ListTreatment = ListTreatment.Where(o => o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNOITRU
                    || o.TDL_TREATMENT_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_TREATMENT_TYPE.ID__DTNGOAITRU).ToList();
                if (filter.IS_NOT_BILL == true)
                {
                    ListTreatment = ListTreatment.Where(o => o.TOTAL_BILL_AMOUNT < o.TOTAL_PATIENT_PRICE && o.TOTAL_PATIENT_PRICE > 0).ToList();
                }
                if (filter.BRANCH_IDs != null)
                {
                    ListTreatment = ListTreatment.Where(o => filter.BRANCH_IDs.Contains(o.BRANCH_ID)).ToList();
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
            bool valid = true; 
            try
            {
                listRdo.Clear(); 
                if (IsNotNullOrEmpty(ListTreatment))
                {
                    listRdo = (from d in ListTreatment select new Mrs00298RDO(d)).ToList(); 
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                valid = false; 
            }
            return valid; 
        }


        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME_TO ?? 0));
                dicSingleTag.Add("TIME", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(filter.TIME ?? 0)); 

                objectTag.AddObjectData(store, "Report", listRdo); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
        }


    }
}
