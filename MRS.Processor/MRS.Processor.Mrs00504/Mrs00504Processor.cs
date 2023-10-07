using MOS.MANAGER.HisService;
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
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisDepartment;

namespace MRS.Processor.Mrs00504
{
    class Mrs00504Processor : AbstractProcessor
    {
        Mrs00504Filter castFilter = null;
        List<Mrs00504RDO> listRdo = new List<Mrs00504RDO>();

        List<V_HIS_TREATMENT> listTreatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_SERE_SERV> listSereServs = new List<V_HIS_SERE_SERV>();

        List<long> listServiceTrans = new List<long>();

        const string ICD_CODE__CAPD = "MRS.MRS_504.ICD_CODE__CAPD";          // mã bệnh capd (capd là nhóm bn có diện điều trị là khám + chẩn đoán Z49,Z49.0,Z49.1,Z49.2)
        const string DEPA_CODE__KKB = "MRS.MRS_504.DEPARTMENT_CODE__KKB";    // khoa khám bệnh
        const string DEPA_CODE__TNT = "MRS.MRS_504.DEPARTMENT_CODE__TNT";    // khoa tnt
        const string DEPA_CODE__NTR = "MRS.MRS_504.DEPARTMENT_CODE__NTR";    // khoa nội trú

        List<string> listCAPDs = new List<string>();
        List<long> listKKBs = new List<long>();
        List<long> listTNTs = new List<long>();
        List<long> listNTRs = new List<long>();



        public Mrs00504Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00504Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00504Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay bao cao Mrs00504: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                // get config 
                var listDepartments = new MOS.MANAGER.HisDepartment.HisDepartmentManager(param).Get(new HisDepartmentFilterQuery()) ?? new List<HIS_DEPARTMENT>();
                listCAPDs = GetCode(ICD_CODE__CAPD);
                listKKBs = GetId(listDepartments, DEPA_CODE__KKB) ?? new List<long>();
                listTNTs = GetId(listDepartments, DEPA_CODE__TNT) ?? new List<long>();
                listNTRs = GetId(listDepartments, DEPA_CODE__NTR) ?? new List<long>();
                GetService();
                // get data

                HisHeinApprovalFilterQuery heinApprovalFilter = new HisHeinApprovalFilterQuery();
                heinApprovalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                heinApprovalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                List<HIS_HEIN_APPROVAL> listHeinApprovals = new HisHeinApprovalManager(param).Get(heinApprovalFilter) ?? new List<HIS_HEIN_APPROVAL>();
                if (IsNotNullOrEmpty(listHeinApprovals))
                {
                    var skip = 0;
                    while (listHeinApprovals.Count - skip > 0)
                    {
                        var listIDs = listHeinApprovals.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentViewFilterQuery treatmentViewFilter = new HisTreatmentViewFilterQuery();
                        treatmentViewFilter.IDs = listIDs.Select(s => s.TREATMENT_ID).ToList();
                        treatmentViewFilter.IS_LOCK_HEIN = true;
                        listTreatments.AddRange(new HisTreatmentManager(param).GetView(treatmentViewFilter));

                        HisSereServViewFilterQuery sereServViewFilter = new HisSereServViewFilterQuery();
                        sereServViewFilter.TREATMENT_IDs = listIDs.Select(s => s.TREATMENT_ID).ToList();
                        listSereServs.AddRange(new HisSereServManager(param).GetView(sereServViewFilter));
                        if (listSereServs != null)
                        {
                            listSereServs = listSereServs.Where(o => o.VIR_TOTAL_HEIN_PRICE > 0).ToList();
                        }
                    }

                    listSereServs = listSereServs.Where(w => w.IS_NO_EXECUTE != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).ToList();
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
            bool result = true;
            try
            {
                var listTreatKKBs = listTreatments.Where(w => listKKBs.Contains(w.END_DEPARTMENT_ID.Value) && !listCAPDs.Contains(w.ICD_CODE)).Select(s => s.ID).ToList() ?? new List<long>();
                var listTreatCAPDs = listTreatments.Where(w => listKKBs.Contains(w.END_DEPARTMENT_ID.Value) && listCAPDs.Contains(w.ICD_CODE)).Select(s => s.ID).ToList() ?? new List<long>();
                var listTreatTNTs = listTreatments.Where(w => listTNTs.Contains(w.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList() ?? new List<long>();
                var listTreatNTRs = listTreatments.Where(w => listNTRs.Contains(w.END_DEPARTMENT_ID.Value)).Select(s => s.ID).ToList() ?? new List<long>();
                listRdo.Add(ProcessCountTreatment(01, "Số lượt khám/lượt điều trị", listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(02, "Tiền khám/Tiền giường", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(03, "Tiền xét nghiệm", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(04, "Tiền chẩn đoán hình ảnh", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(05, "Tiền thủ thuật, phẫu thuật", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT, IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(06, "Tiền máu, chế phẩm máu", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(07, "Tiền vận chuyển", new List<long>(), true, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(08, "Tiền vật tư y tế", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
                listRdo.Add(ProcessPriceService(09, "Tiền thuốc, dịch", new List<long>() { IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC }, false, listTreatKKBs, listTreatCAPDs, listTreatTNTs, listTreatNTRs));
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error("Lỗi xảy ra tại ProcessData: " + ex);
                result = false;
            }
            return result;
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);

                try
                {
                    var mounthFrom = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM).Value.Month;
                    var mounthTo = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_TO).Value.Month;
                    List<int> mounth = new List<int>();
                    if (mounthTo >= mounthFrom)
                    {
                        for (int i = mounthFrom; i <= mounthTo; i++)
                        {
                            mounth.Add(i);
                        }
                    }
                    else
                    {
                        for (int i = mounthTo; i <= mounthFrom; i++)
                        {
                            mounth.Add(i);
                        }
                    }
                    dicSingleTag.Add("MOUNTH", String.Join(", ", mounth));
                }
                catch { Inventec.Common.Logging.LogSystem.Error("Lỗi khi lấy tháng!"); }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.ID).ToList());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        // my function \( * v*)/ ~*
        private List<string> GetCode(string key)
        {
            var config = Loader.dictionaryConfig[key];
            if (config == null) throw new ArgumentNullException(key);
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;

            List<string> rs = new List<string>() ; 
            if (value != null && value.Contains(","))
                rs.AddRange(value.Split(',').ToList());
            else
                rs.Add(value);
            return rs;
        }

        private List<long> GetId(List<HIS_DEPARTMENT> listDepartments, string key)
        {
            var config = Loader.dictionaryConfig[key];
            if (config == null) throw new ArgumentNullException(key);
            string value = String.IsNullOrEmpty(config.VALUE) ? (String.IsNullOrEmpty(config.DEFAULT_VALUE) ? "" : config.DEFAULT_VALUE) : config.VALUE;
            List<string> departmentCode = new List<string>();
            if (value != null && value.Contains(","))
                departmentCode.AddRange(value.Split(',').ToList());
            else
                departmentCode.Add(value);
            var res = listDepartments.Where(w => departmentCode.Contains(w.DEPARTMENT_CODE)).Select(s => s.ID).ToList() ?? new List<long>();
            return res;
        }

        private void GetService()
        {
            HisServiceRetyCatViewFilterQuery serviceRetyCatViewFilter = new HisServiceRetyCatViewFilterQuery();
            serviceRetyCatViewFilter.REPORT_TYPE_CODE__EXACT = "MRS00504";
            var listRetyCats = new HisServiceRetyCatManager(param).GetView(serviceRetyCatViewFilter);

            listServiceTrans = listRetyCats.Where(w => w.CATEGORY_CODE == "TRANS").Select(s => s.SERVICE_ID).ToList() ?? new List<long>();
        }

        private Mrs00504RDO ProcessCountTreatment(long id, string index, List<long> listTreatKKBs, List<long> listTreatCAPDs, List<long> listTreatTNTs, List<long> listTreatNTRs)
        {
            Mrs00504RDO rdo = new Mrs00504RDO();
            rdo.ID = id;
            rdo.INDEX = index;
            rdo.KKB = listTreatKKBs.Count();
            rdo.CAPD = listTreatCAPDs.Count();
            rdo.TNT = listTreatTNTs.Count();
            rdo.NTR = listTreatNTRs.Count();
            return rdo;
        }

        private Mrs00504RDO ProcessPriceService(long id, string index, List<long> serviceTypeId, bool trans, List<long> listTreatKKBs, List<long> listTreatCAPDs, List<long> listTreatTNTs, List<long> listTreatNTRs)
        {
            Mrs00504RDO rdo = new Mrs00504RDO();
            rdo.ID = id;
            rdo.INDEX = index;
            var sub = new List<V_HIS_SERE_SERV>();
            sub = listSereServs.Where(w => serviceTypeId.Contains(w.TDL_SERVICE_TYPE_ID)).ToList() ?? new List<V_HIS_SERE_SERV>();
            if (trans) sub.AddRange(listSereServs.Where(w => listServiceTrans.Contains(w.SERVICE_ID)).ToList() ?? new List<V_HIS_SERE_SERV>());

            rdo.KKB = sub.Where(w => listTreatKKBs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            rdo.CAPD = sub.Where(w => listTreatCAPDs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            rdo.TNT = sub.Where(w => listTreatTNTs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            rdo.NTR = sub.Where(w => listTreatNTRs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);

            rdo.HEIN_KKB = sub.Where(w => listTreatKKBs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
            rdo.HEIN_CAPD = sub.Where(w => listTreatCAPDs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
            rdo.HEIN_TNT = sub.Where(w => listTreatTNTs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
            rdo.HEIN_NTR = sub.Where(w => listTreatNTRs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);

            rdo.FEE_KKB = sub.Where(w => listTreatKKBs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
            rdo.FEE_CAPD = sub.Where(w => listTreatCAPDs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
            rdo.FEE_TNT = sub.Where(w => listTreatTNTs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
            rdo.FEE_NTR = sub.Where(w => listTreatNTRs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);

            List<long> sub2 = sub.Where(w => w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__MAU || w.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT || listServiceTrans.Contains(w.SERVICE_ID)).Select(s => s.ID).ToList() ?? new List<long>();
            sub = sub.Where(w => !sub2.Contains(w.ID)).ToList();
            if (IsNotNullOrEmpty(sub))
            {
                rdo.PRICE_KKB = sub.Where(w => listTreatKKBs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                rdo.PRICE_CAPD = sub.Where(w => listTreatCAPDs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                rdo.PRICE_TNT = sub.Where(w => listTreatTNTs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
                rdo.PRICE_NTR = sub.Where(w => listTreatNTRs.Contains(w.TDL_TREATMENT_ID.Value)).Sum(s => s.VIR_TOTAL_PRICE ?? 0);
            }
            return rdo;
        }
    }
}
