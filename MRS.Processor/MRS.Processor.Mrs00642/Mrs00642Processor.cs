using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIS.Treatment.DateTime;
using MRS.MANAGER.Core.MrsReport.RDO;
using System.Reflection;
using Inventec.Common.Logging;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisService;


namespace MRS.Processor.Mrs00642
{

    class Mrs00642Processor : AbstractProcessor
    {
        Mrs00642Filter castFilter = null;
        List<Mrs00642RDO> listRdo = new List<Mrs00642RDO>();
        Dictionary<string, int> DIC_ICD_CAREER = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_MALE_15_50 = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_FEMALE_15_50 = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_MALE_LESS15 = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_FEMALE_LESS15 = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_MALE_MORE50 = new Dictionary<string, int>();
        Dictionary<string, int> DIC_ICD_FEMALE_MORE50 = new Dictionary<string, int>();

        public Mrs00642Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00642Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00642Filter)this.reportFilter;
                listRdo = new ManagerSql().Get(this.castFilter);
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
            bool result = true;
            try
            {

                if (listRdo != null && listRdo.Count > 0)
                {

                    DIC_ICD_CAREER = listRdo.GroupBy(o => IcdCareerCode(o.ICD_CODE, o.CAREER_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_MALE_15_50 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && Age(q.OUT_TIME, q.DOB) >= 15 && Age(q.OUT_TIME, q.DOB) < 50).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_FEMALE_15_50 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Age(q.OUT_TIME, q.DOB) >= 15 && Age(q.OUT_TIME, q.DOB) < 50).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_MALE_LESS15 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && Age(q.OUT_TIME, q.DOB) < 15).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_FEMALE_LESS15 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Age(q.OUT_TIME, q.DOB) < 15).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_MALE_MORE50 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__MALE && Age(q.OUT_TIME, q.DOB) >= 50).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                    DIC_ICD_FEMALE_MORE50 = listRdo.Where(q => q.GENDER_ID == IMSys.DbConfig.HIS_RS.HIS_GENDER.ID__FEMALE && Age(q.OUT_TIME, q.DOB) >=50).GroupBy(o => IcdCode(o.ICD_CODE)).ToDictionary(p => p.Key, p => p.ToList().Count);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


        private int Age(long IN_TIME, long TDL_PATIENT_DOB)
        {
            return (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(IN_TIME) ?? DateTime.Now).Year - (Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(TDL_PATIENT_DOB) ?? DateTime.Now).Year;
        }

        private string IcdCareerCode(string IcdCode, string CareerCode)
        {
            try
            {
                return (IcdCode ?? "")
                    + "_" + (CareerCode ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        private string IcdCode(string IcdCode)
        {
            try
            {
                return (IcdCode ?? "");

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return "";
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM ?? 0));

            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO ?? 0));

            dicSingleTag.Add("DIC_ICD_CAREER", DIC_ICD_CAREER);

            dicSingleTag.Add("DIC_ICD_MALE_15_50", DIC_ICD_MALE_15_50);

            dicSingleTag.Add("DIC_ICD_FEMALE_15_50", DIC_ICD_FEMALE_15_50);

            dicSingleTag.Add("DIC_ICD_MALE_LESS15", DIC_ICD_MALE_LESS15);

            dicSingleTag.Add("DIC_ICD_FEMALE_LESS15", DIC_ICD_FEMALE_LESS15);

            dicSingleTag.Add("DIC_ICD_MALE_MORE50", DIC_ICD_MALE_MORE50);

            dicSingleTag.Add("DIC_ICD_FEMALE_MORE50", DIC_ICD_FEMALE_MORE50);

            objectTag.SetUserFunction(store, "SumKeys", new RDOSumKeys());
            objectTag.SetUserFunction(store, "Element", new RDOElement());
        }
    }
}
