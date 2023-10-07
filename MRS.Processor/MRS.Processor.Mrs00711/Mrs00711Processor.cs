using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
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
using Inventec.Common.DateTime; 
using MRS.MANAGER.Config; 
using FlexCel.Report; 
 
using MOS.MANAGER.HisImpMest; 
using MOS.MANAGER.HisMediStock; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMedicineTypeAcin; 
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisSupplier;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisIcd;
using SDA.EFMODEL.DataModels;
using SDA.MANAGER.Manager;
using SDA.MANAGER.Core.SdaDistrict.Get;
using MOS.MANAGER.HisBranch; 

namespace MRS.Processor.Mrs00711
{
    public class Mrs00711Processor : AbstractProcessor
    {
        private Mrs00711Filter filter;
        List<Mrs00711RDO> listCountTreatment = new List<Mrs00711RDO>();
        List<V_SDA_DISTRICT> listDistrict = new List<V_SDA_DISTRICT>();
        List<HIS_BRANCH> listBranch = new List<HIS_BRANCH>();
        CommonParam paramGet = new CommonParam(); 
        public Mrs00711Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00711Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            filter = (Mrs00711Filter)reportFilter; 
            try
            {
                List<string> IcdCodeDttts = new List<string>();
                List<string> IcdCodeMos = new List<string>();
                List<string> IcdCodeGls = new List<string>();
                List<string> IcdCodeQus = new List<string>();
                List<HIS_ICD> listIcd = new HisIcdManager().Get(new HisIcdFilterQuery());
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__DTTTs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__DTTTs.Split(',');
                    IcdCodeDttts = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q=>q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__MOs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__MOs.Split(',');
                    IcdCodeMos = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__GLs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__GLs.Split(',');
                    IcdCodeGls = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                if (!string.IsNullOrWhiteSpace(filter.PREFIX_ICD_CODE__QUs))
                {
                    string[] IcdCodes = filter.PREFIX_ICD_CODE__QUs.Split(',');
                    IcdCodeQus = listIcd.Where(o => IcdCodes.ToList().Exists(p => o.ICD_CODE.StartsWith(p))).Select(q => q.ICD_CODE).ToList();
                }
                listDistrict = new SdaDistrictManager(new CommonParam()).Get<List<V_SDA_DISTRICT>>(new SdaDistrictViewFilterQuery()) ?? new List<V_SDA_DISTRICT>();
                listBranch = new HisBranchManager().Get(new HisBranchFilterQuery()) ?? new List<HIS_BRANCH>(); 
                listCountTreatment = new MRS.Processor.Mrs00711.ManagerSql().GetCountTreatment(filter,IcdCodeDttts,IcdCodeMos,IcdCodeQus,IcdCodeGls,listDistrict) ?? new List<Mrs00711RDO>();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 

                result = false; 
            }
            return result; 
        }
        
        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                if (listCountTreatment != null)
                {
                    foreach (var item in listCountTreatment)
                    {
                        var district = listDistrict.FirstOrDefault(o=>o.DISTRICT_CODE==item.TDL_PATIENT_DISTRICT_CODE);
                        
                        if (district != null)
                        {
                            var branch = listBranch.FirstOrDefault(o => o.PROVINCE_CODE == district.PROVINCE_CODE);
                            if (branch != null)
                            {
                                item.TDL_PATIENT_DISTRICT_NAME = district.DISTRICT_NAME;
                            }
                            else
                            {
                                item.TDL_PATIENT_DISTRICT_CODE = "ZKHAC";
                                item.TDL_PATIENT_DISTRICT_NAME = "Khác";
                            }
                        }
                        else
                        {
                            item.TDL_PATIENT_DISTRICT_CODE = "ZKHAC";
                            item.TDL_PATIENT_DISTRICT_NAME = "Khác";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                result = false; 
                Inventec.Common.Logging.LogSystem.Error(ex); 
            }
            return result; 
        }

       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            dicSingleTag.Add("TIME_FROM", filter.TIME_FROM);
            dicSingleTag.Add("TIME_TO", filter.TIME_TO);
            objectTag.AddObjectData(store,"Report",listCountTreatment.OrderBy(o=>o.TDL_PATIENT_DISTRICT_CODE).ToList());
        }

       
    }
}
