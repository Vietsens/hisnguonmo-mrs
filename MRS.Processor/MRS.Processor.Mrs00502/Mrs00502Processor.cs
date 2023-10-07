using MOS.MANAGER.HisService;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisDepartment;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisTreatment;
using SDA.EFMODEL.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MRS.Processor.Mrs00502
{
    class Mrs00502Processor : AbstractProcessor
    {
        Mrs00502Filter castFilter = new Mrs00502Filter();
        List<Mrs00502RDO> listRdo = new List<Mrs00502RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();
        List<HIS_DEPARTMENT> listDepartments = new List<HIS_DEPARTMENT>();

        long TIME_FROM = 0;
        long TIME_TO = 0;

        long YEAR = DateTime.Now.Year;
        decimal TOTAL_ACCUMULATED = -1;

        string RETY_CAT_CODE__TRANS = "TRANS";

        public Mrs00502Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00502Filter);
        }

        protected override bool GetData()
        {
            bool result = true; 
            try
            {
                this.castFilter = (Mrs00502Filter)this.reportFilter; 
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00502: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                // get time
                if (castFilter.CREATE_TIME != null) YEAR = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.CREATE_TIME.Value).Value.Year; 
                this.TIME_FROM = (long)Convert.ToDecimal(YEAR + "0101000000"); 
                this.TIME_TO = (long)Convert.ToDecimal(YEAR + "1231235959"); 
                // get treatment and service
                HisTreatmentViewFilterQuery treatmentViewfilter = new HisTreatmentViewFilterQuery(); 
                treatmentViewfilter.END_DEPARTMENT_IDs = new List<long>() { castFilter.DEPARTMENT_ID }; 
                treatmentViewfilter.OUT_TIME_FROM = this.TIME_FROM; 
                treatmentViewfilter.OUT_TIME_TO = this.TIME_TO; 
                treatmentViewfilter.IS_LOCK_HEIN = true; 
                listTreatments = new MOS.MANAGER.HisTreatment.HisTreatmentManager(param).GetView(treatmentViewfilter); 

                var skip = 0; 
                while (listTreatments.Count - skip > 0)
                {
                    var listIDs = listTreatments.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                    HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery(); 
                    sereServViewFilter.TREATMENT_IDs = listIDs.Select(s => s.ID).ToList(); 
                    sereServViewFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE; 
                    listSereServs.AddRange(new MOS.MANAGER.HisSereServ.HisSereServManager(param).GetView(sereServViewFilter)); 
                }

                listSereServs = listSereServs.Where(w => w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE && w.IS_ACTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Distinct().ToList(); 
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        protected List<long> GetListConfig(string key)
        {
            var config = Loader.dictionaryConfig[key];
            if (config == null) throw new ArgumentNullException(key);
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;

            var departmentCode = value.Split(',').ToList();

            var result = listDepartments.Where(w => departmentCode.Contains(w.DEPARTMENT_CODE)).Select(s => s.ID).ToList() ?? new List<long>();

            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listSereServs))
                {
                    ProcessTotalAccumulated();
                }
                //
                ProcessCountTreatmentExamOrClinical(01, "Số lượt khám", listTreatments);
                ProcessCountServicePrice(02, "Tiền khám", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH }, "");
                ProcessCountServicePrice(03, "Tiền xét nghiệm", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN }, "");
                ProcessCountServicePrice(04, "Tiền chẩn đoán hình ảnh", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA }, "");
                ProcessCountServicePrice(05, "Tiền thủ thuật, phẫu thuật", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT }, "");
                ProcessCountServicePrice(06, "Tiền máu, chế phẩm máu", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU }, "");
                ProcessCountServicePrice(07, "Tiền vận chuyển", new List<long>(), this.RETY_CAT_CODE__TRANS);
                ProcessCountServicePrice(08, "Tiền vật tư ý tế", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT }, "");
                ProcessCountServicePrice(09, "Tiền thuốc, dịch", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC }, "");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex);
                result = false;
            }
            return result;
        }

        protected void ProcessTotalAccumulated()
        {
            var sereServs = new List<V_HIS_SERE_SERV>();
            // lấy những dịch vụ có nhóm loại dịch vụ
            List<long> listServiceTypes = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC
                    };
            sereServs = listSereServs.Where(w => listServiceTypes.Contains(w.TDL_SERVICE_TYPE_ID)).ToList();
            // lấy dịch vụ vận chuyển (ko có nhóm loại vận chuyển)
            HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatViewFilter.REPORT_TYPE_CODE__EXACT = "MRS00485";
            var listRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter);

            listRetyCats = listRetyCats.Where(w => w.CATEGORY_CODE == this.RETY_CAT_CODE__TRANS).ToList();

            sereServs.AddRange(listSereServs.Where(w => listRetyCats.Select(s => s.SERVICE_ID).Contains(w.SERVICE_ID)).ToList());
            // 
            sereServs.Distinct();

            this.TOTAL_ACCUMULATED = sereServs.Sum(su => su.VIR_TOTAL_PRICE ?? 0);
        }

        protected void ProcessCountTreatmentExamOrClinical(long id, string index, List<V_HIS_TREATMENT> list)
        {
            var rdo = new Mrs00502RDO();
            rdo.ID = id;
            rdo.INDEX = index;
            rdo.JAN = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0101000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0131235959")).Count();
            rdo.FEB = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0201000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0229235959")).Count();
            rdo.MAR = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0301000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0331235959")).Count();
            rdo.APR = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0401000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0430235959")).Count();
            rdo.MAY = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0501000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0531235959")).Count();
            rdo.JUN = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0601000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0630235959")).Count();
            rdo.JUL = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0701000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0731235959")).Count();
            rdo.AUG = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0801000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0831235959")).Count();
            rdo.SEP = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0901000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0930235959")).Count();
            rdo.OCT = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1001000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1031235959")).Count();
            rdo.NOV = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1101000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1130235959")).Count();
            rdo.DEC = list.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1201000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1231235959")).Count();
            rdo.ACCUMULATED = listTreatments.Distinct().Count();
            //rdo.PERCEN = TOTAL_ACCUMULATED > 0 ? (rdo.ACCUMULATED / this.TOTAL_ACCUMULATED) * 100 : 0; 
            listRdo.Add(rdo);
        }

        protected void ProcessCountServicePrice(long id, string index, List<long> serviceTypeId, string categoryCode)
        {
            var sereServs = new List<V_HIS_SERE_SERV>();
            if (categoryCode.Trim() != "")
            {
                HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery();
                serviceRetyCatViewFilter.REPORT_TYPE_CODE__EXACT = "MRS00485";
                var listRetyCats = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter);

                listRetyCats = listRetyCats.Where(w => w.CATEGORY_CODE == categoryCode).ToList();

                sereServs = listSereServs.Where(w => listRetyCats.Select(s => s.SERVICE_ID).Contains(w.SERVICE_ID)).ToList();
            }
            else
            {
                sereServs = listSereServs.Where(w => serviceTypeId.Contains(w.TDL_SERVICE_TYPE_ID)).ToList();
            }

            var rdo = new Mrs00502RDO();
            rdo.ID = id;
            rdo.INDEX = index;

            var sub = new List<long>();
            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0101000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0131235959")).Select(s => s.ID).ToList();
            rdo.JAN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_JAN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_JAN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0201000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0229235959")).Select(s => s.ID).ToList();
            rdo.FEB = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_FEB = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_FEB = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0301000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0331235959")).Select(s => s.ID).ToList();
            rdo.MAR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_MAR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_MAR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0401000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0430235959")).Select(s => s.ID).ToList();
            rdo.APR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_APR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_APR = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0501000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0531235959")).Select(s => s.ID).ToList();
            rdo.MAY = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_MAY = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_MAY = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0601000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0630235959")).Select(s => s.ID).ToList();
            rdo.JUN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_JUN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_JUN = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0701000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0731235959")).Select(s => s.ID).ToList();
            rdo.JUL = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_JUL = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_JUL = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0801000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0831235959")).Select(s => s.ID).ToList();
            rdo.AUG = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_AUG = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_AUG = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "0901000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "0930235959")).Select(s => s.ID).ToList();
            rdo.SEP = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_SEP = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_SEP = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1001000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1031235959")).Select(s => s.ID).ToList();
            rdo.OCT = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_OCT = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_OCT = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1101000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1130235959")).Select(s => s.ID).ToList();
            rdo.NOV = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_NOV = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_NOV = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            sub = listTreatments.Where(w => w.FEE_LOCK_TIME >= Convert.ToDecimal(YEAR + "1201000000") && w.FEE_LOCK_TIME <= Convert.ToDecimal(YEAR + "1231235959")).Select(s => s.ID).ToList();
            rdo.DEC = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PRICE ?? 0));
            rdo.HEIN_DEC = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_HEIN_PRICE ?? 0));
            rdo.FEE_DEC = sereServs.Where(w => sub.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => (s.VIR_TOTAL_PATIENT_PRICE ?? 0));

            rdo.ACCUMULATED = sereServs.Sum(su => su.VIR_TOTAL_PRICE ?? 0);
            rdo.PERCEN = TOTAL_ACCUMULATED > 0 ? (rdo.ACCUMULATED / this.TOTAL_ACCUMULATED) * 100 : 0;
            listRdo.Add(rdo);
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("CREATE_TIME", castFilter.CREATE_TIME);
                dicSingleTag.Add("YEAR", this.YEAR);

                HisDepartmentFilterQuery departmentFilter = new HisDepartmentFilterQuery();
                departmentFilter.ID = castFilter.DEPARTMENT_ID;
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(departmentFilter) ?? new List<HIS_DEPARTMENT>();

                dicSingleTag.Add("DEPARTMENT_NAME", String.Join(",", listDepartments.Select(S => S.DEPARTMENT_NAME.ToUpper()).ToList()));

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.ID).ToList());
                //objectTag.AddObjectData(store, "RdoParent", listRdoParent.OrderBy(s => s.PARENT_ID).ToList()); 
                //exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoParent", "Rdo", "PARENT_ID", "PARENT_ID"); 
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
