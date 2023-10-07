using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using MRS.MANAGER.Config;
using Mrs.Bhyt.PayRateAndTotalPrice;
using MOS.MANAGER.HisBranch;

namespace MRS.Processor.Mrs00810
{
    public class Mrs00810Processor : AbstractProcessor
    {
        public Mrs00810Filter filter;
        public List<Mrs00810RDO> listRdo = new List<Mrs00810RDO>();
        public List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        public List<HIS_SERE_SERV> listSereServ = new List<HIS_SERE_SERV>();
        public List<V_HIS_HEIN_APPROVAL> ListHeinApproval = new List<V_HIS_HEIN_APPROVAL>();
        public List<HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<HIS_EXP_MEST_MEDICINE>();
        public List<HIS_MEDICINE_TYPE> listMedicieType = new List<HIS_MEDICINE_TYPE>();

        public List<HIS_BRANCH> ListBranch = new List<HIS_BRANCH>();
        public CommonParam commonParam = new CommonParam();
        public Mrs00810Processor(CommonParam param, string reportTypeCode):base (param,reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00810Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            filter= (Mrs00810Filter)this.reportFilter;
            try
            {
                #region câu lệnh truy vấn
                //select trea.TDL_PATIENT_NAME, trea.TDL_PATIENT_DOB,trea.TDL_PATIENT_GENDER_NAME,
                //trea.TDL_HEIN_CARD_NUMBER,mety.MEDICINE_TYPE_NAME,brand.BRANCH_NAME,trea.IN_DATE,
                //trea.ICD_CODE,serv.PRICE as PRICE_TREATMENT,medi.IMP_PRICE* serv.AMount as TOTAL_PRICE_MEDI
                //from HIS_SERE_SERV serv join HIS_SERVICE se on serv.SERVICE_ID = se.ID
                //join HIS_MEDICINE medi on medi.id = medicine_id
                //join HIS_MEDICINE_TYPE mety on mety.ID = medi.MEDICINE_TYPE_ID
                //join HIS_TREATMENT trea on trea.ID = serv.TDL_TREATMENT_ID
                //join HIS_BRANCH brand on brand.ID = trea.BRANCH_ID
                //where trea.ICD_CODE = 'B20' and mety.MEDICINE_TYPE_NAME like'%ARV%'
                #endregion
                HisHeinApprovalViewFilterQuery heinApprovalFilter = new HisHeinApprovalViewFilterQuery();
                heinApprovalFilter.BRANCH_ID = filter.BRANCH_ID;
                heinApprovalFilter.EXECUTE_TIME_FROM = filter.TIME_FROM;
                heinApprovalFilter.EXECUTE_TIME_TO = filter.TIME_TO;
                ListHeinApproval = new HisHeinApprovalManager(commonParam).GetView(heinApprovalFilter);

                ListBranch = new HisBranchManager(commonParam).Get(new HisBranchFilterQuery());

                var heinApprovalIds = ListHeinApproval.Select(x => x.ID).ToList();
                var treatmentIds = ListHeinApproval.Select(x => x.TREATMENT_ID).ToList();
                HisTreatmentFilterQuery treatmentFliter = new HisTreatmentFilterQuery();
                treatmentFliter.IDs = treatmentIds;
                listTreatment = new HisTreatmentManager(commonParam).Get(treatmentFliter);

                HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                listMedicieType = new HisMedicineTypeManager(commonParam).Get(medicineTypeFilter);
                var medicineTypeIds = listMedicieType.Where(x => x.MEDICINE_TYPE_NAME.Contains("ARV")).Select(x=>x.ID).ToList();
                HisExpMestMedicineFilterQuery medicineFilter = new HisExpMestMedicineFilterQuery();
                medicineFilter.TDL_MEDICINE_TYPE_IDs = medicineTypeIds;
                listExpMestMedicine = new HisExpMestMedicineManager(commonParam).Get(medicineFilter);
                var listExpMestMedicineIds = listExpMestMedicine.Select(x => x.ID).ToList();
                HisSereServFilterQuery sereServFilter = new HisSereServFilterQuery();
                sereServFilter.TREATMENT_IDs = treatmentIds;
                sereServFilter.HEIN_APPROVAL_IDs = heinApprovalIds;
                listSereServ = new HisSereServManager(commonParam).Get(sereServFilter);
                listSereServ = listSereServ.Where(x => listExpMestMedicineIds.Contains(x.EXP_MEST_MEDICINE_ID??0)).ToList();
                ListBranch = new HisBranchManager(commonParam).Get(new HisBranchFilterQuery());
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
           return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                result = true;
                var groups = listSereServ.GroupBy(x => x.TDL_TREATMENT_ID).ToList();
                foreach (var item in listSereServ)
                {
                    Mrs00810RDO rdo = new Mrs00810RDO();
                    var trea = listTreatment.Where(x => x.ID == item.TDL_TREATMENT_ID).FirstOrDefault();
                    if (trea!=null)
                    {
                        rdo.PATIENT_NAME = trea.TDL_PATIENT_NAME;
                        rdo.PATIENT_CODE = trea.TDL_PATIENT_CODE;
                        rdo.PATIENT_ID = trea.PATIENT_ID;
                        rdo.DOB = trea.TDL_PATIENT_DOB.ToString().Substring(0, 4);
                        rdo.GENDER = trea.TDL_PATIENT_GENDER_NAME;
                        rdo.HEIN_CARD_NUMBER = trea.TDL_HEIN_CARD_NUMBER;
                        rdo.IN_DATE = trea.IN_DATE;
                        rdo.ICD_CODE = trea.ICD_CODE;
                        rdo.END_CODE = trea.END_CODE;
                    }
                    var heinApproval = ListHeinApproval.Where(x => x.ID == item.HEIN_APPROVAL_ID).FirstOrDefault();
                    if (heinApproval!=null)
                    {
                        rdo.HEIN_ORG_NAME = heinApproval.HEIN_MEDI_ORG_NAME;
                    }
                    var expMestMedicine = listExpMestMedicine.Where(x => x.ID == item.EXP_MEST_MEDICINE_ID).FirstOrDefault();
                    if (expMestMedicine!=null)
                    {
                        rdo.TOTAL_PRICE_ARV = expMestMedicine.AMOUNT * expMestMedicine.PRICE ?? 0;
                        //var TotalPriceTreatment = Mrs.Bhyt.PayRateAndTotalPrice.Caculator.TotalPrice(item, banch,true);
                        //LogSystem.Info(TotalPriceTreatment.ToString());
                        
                    }
                    rdo.HEIN_RATIO = item.HEIN_RATIO??0;
                    rdo.HEIN_PRICE = item.VIR_HEIN_PRICE??0;// tiên bảo hiểm thanh toán
                    rdo.OTHER_SOURCE_PRICE = item.OTHER_SOURCE_PRICE??0;// nguồn khác trả
                    //rdo.TOTAL_PRICE = item.VIR_TOTAL_PRICE ?? 0;
                    rdo.TOTAL_PATIENT_PRICE = item.PATIENT_PRICE_BHYT ?? 0;
                    var ora = item.OTHER_SOURCE_PRICE;
                    listRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }
        public List<Mrs00810RDO> SumPrice(List<Mrs00810RDO> listRdo) {

            var groups = listRdo.GroupBy(x => new { x.PATIENT_CODE, x.PATIENT_ID, x.PATIENT_NAME, x.HEIN_ORG_NAME,x.IN_DATE ,x.END_CODE}).ToList();
            List<Mrs00810RDO> ListSumRdo = new List<Mrs00810RDO>();
            foreach (var item in groups)
            {
                Mrs00810RDO rdo = new Mrs00810RDO();
                rdo.PATIENT_ID = item.First().PATIENT_ID;
                rdo.PATIENT_CODE = item.First().PATIENT_CODE;
                rdo.PATIENT_NAME = item.First().PATIENT_NAME;
                rdo.HEIN_ORG_NAME = item.First().HEIN_ORG_NAME;
                rdo.HEIN_CARD_NUMBER = item.First().HEIN_CARD_NUMBER;
                rdo.IN_DATE = item.First().IN_DATE;
                rdo.ICD_CODE = item.First().ICD_CODE;
                rdo.DOB = item.First().DOB;
                rdo.GENDER = item.First().GENDER;
                rdo.TOTAL_PATIENT_PRICE = item.Sum(x => x.TOTAL_PATIENT_PRICE);
                rdo.TOTAL_PRICE_ARV = item.Sum(x => x.TOTAL_PRICE_ARV);
                rdo.TOTAL_PRICE = item.Sum(x => x.TOTAL_PRICE);
                rdo.BLOOD_PRICE = item.Sum(x => x.BLOOD_PRICE);
                rdo.CDHA_PRICE = item.Sum(x => x.CDHA_PRICE);
                rdo.CPVC_PRICE = item.Sum(x => x.CPVC_PRICE);
                rdo.DVKT_NC_PRICE = item.Sum(x => x.DVKT_NC_PRICE);
                rdo.DVKT_TT_PRICE = item.Sum(x => x.DVKT_TT_PRICE);
                rdo.MEDI_PRICE = item.Sum(x => x.MEDI_PRICE);
                rdo.VTTHYT_PRICE = item.Sum(x => x.VTTHYT_PRICE);
                rdo.XN_PRICE = item.Sum(x => x.XN_PRICE);
                rdo.TOTAL_PRICE =( rdo.TOTAL_PATIENT_PRICE + rdo.TOTAL_PRICE_ARV + rdo.BLOOD_PRICE + rdo.CDHA_PRICE + 
                    rdo.CPVC_PRICE + rdo.DVKT_NC_PRICE + rdo.DVKT_TT_PRICE + rdo.VTTHYT_PRICE + rdo.XN_PRICE);
                rdo.HEIN_PRICE = rdo.TOTAL_PRICE * item.First().HEIN_RATIO;
                rdo.OTHER_SOURCE_PRICE = item.Sum(x => x.OTHER_SOURCE_PRICE);
                rdo.HEIN_PRICE = item.Sum(x => x.HEIN_PRICE);
                ListSumRdo.Add(rdo);
            }
            return ListSumRdo;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var sumPrice = SumPrice(listRdo);
            objectTag.AddObjectData(store, "Report",SumPrice(listRdo));
            var branch = ListBranch.Where(x=>x.ID == filter.BRANCH_ID).FirstOrDefault().BRANCH_NAME;
            dicSingleTag.Add("ORGANIZATION", branch);
            dicSingleTag.Add("TOTAL_HEIN_PRICE", sumPrice.Sum(x => x.OTHER_SOURCE_PRICE));
        }
    }
}
