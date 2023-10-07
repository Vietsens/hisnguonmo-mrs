using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisBranch;
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

namespace MRS.Processor.Mrs00358
{
    public class Mrs00358Processor : AbstractProcessor
    {
        Mrs00358Filter castFilter = null;

        List<Mrs00358RDO> ListExamRdo_A = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListBedRdo_A = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListTestRdo_A = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListDiimFuexRdo_A = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListSurgMisuRdo_A = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListOtherRdo_A = new List<Mrs00358RDO>();

        List<Mrs00358RDO> ListExamRdo_B = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListBedRdo_B = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListTestRdo_B = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListDiimFuexRdo_B = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListSurgMisuRdo_B = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListOtherRdo_B = new List<Mrs00358RDO>();

        List<Mrs00358RDO> ListExamRdo_C = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListBedRdo_C = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListTestRdo_C = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListDiimFuexRdo_C = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListSurgMisuRdo_C = new List<Mrs00358RDO>();
        List<Mrs00358RDO> ListOtherRdo_C = new List<Mrs00358RDO>();

        private const string EXAM = "EXAM";
        private const string BED = "BED";
        private const string TEST = "TEST";
        private const string DIIMFUEX = "DIIMFUEX";
        private const string SURGMISU = "SURGMISU";
        private const string OTHER = "OTHER";

        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_3> sereServs = new List<V_HIS_SERE_SERV_3>();
        HIS_BRANCH _Branch = null;

        public Mrs00358Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00358Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00358Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, MRS00353, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                this._Branch = MRS.MANAGER.Config.HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == this.castFilter.BRANCH_ID);
                if (this._Branch == null)
                    throw new NullReferenceException("Nguoi dung truyen len branchId khong chin xac");

                List<V_HIS_TREATMENT> tmp = this.GetTreatment();

                ///lay du lieu patient_type_alter va treament
                this.GetPatientTypeAlterAndTreatment(tmp, ref this.treatments, ref this.patientTypeAlters);
                this.sereServs = this.GetSereServ(this.treatments);

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co loi xay ra trong qua trinh lay du lieu, MRS00353");
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
                if (IsNotNullOrEmpty(this.patientTypeAlters))
                {

                    GeneralDataByListPatientTypeAlter(this.patientTypeAlters, this.sereServs);

                    ListExamRdo_A = ProcessListRDO(ListExamRdo_A);
                    ListBedRdo_A = ProcessListRDO(ListBedRdo_A);
                    ListTestRdo_A = ProcessListRDO(ListTestRdo_A);
                    ListDiimFuexRdo_A = ProcessListRDO(ListDiimFuexRdo_A);
                    ListSurgMisuRdo_A = ProcessListRDO(ListSurgMisuRdo_A);
                    ListOtherRdo_A = ProcessListRDO(ListOtherRdo_A);
                    ListExamRdo_B = ProcessListRDO(ListExamRdo_B);
                    ListBedRdo_B = ProcessListRDO(ListBedRdo_B);
                    ListTestRdo_B = ProcessListRDO(ListTestRdo_B);
                    ListDiimFuexRdo_B = ProcessListRDO(ListDiimFuexRdo_B);
                    ListSurgMisuRdo_B = ProcessListRDO(ListSurgMisuRdo_B);
                    ListOtherRdo_B = ProcessListRDO(ListOtherRdo_B);
                    ListExamRdo_C = ProcessListRDO(ListExamRdo_C);
                    ListBedRdo_C = ProcessListRDO(ListBedRdo_C);
                    ListTestRdo_C = ProcessListRDO(ListTestRdo_C);
                    ListDiimFuexRdo_C = ProcessListRDO(ListDiimFuexRdo_C);
                    ListSurgMisuRdo_C = ProcessListRDO(ListSurgMisuRdo_C);
                    ListOtherRdo_C = ProcessListRDO(ListOtherRdo_C);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
                ListExamRdo_A.Clear();
                ListBedRdo_A.Clear();
                ListTestRdo_A.Clear();
                ListDiimFuexRdo_A.Clear();
                ListSurgMisuRdo_A.Clear();
                ListOtherRdo_A.Clear();
                ListExamRdo_B.Clear();
                ListBedRdo_B.Clear();
                ListTestRdo_B.Clear();
                ListDiimFuexRdo_B.Clear();
                ListSurgMisuRdo_B.Clear();
                ListOtherRdo_B.Clear();
                ListExamRdo_C.Clear();
                ListBedRdo_C.Clear();
                ListTestRdo_C.Clear();
                ListDiimFuexRdo_C.Clear();
                ListSurgMisuRdo_C.Clear();
                ListOtherRdo_C.Clear();
            }
            return result;
        }

        private void GeneralDataByListPatientTypeAlter(List<V_HIS_PATIENT_TYPE_ALTER> hisPatientTypeAlters, List<V_HIS_SERE_SERV_3> listSereServ)
        {
            try
            {
                Dictionary<long, V_HIS_PATIENT_TYPE_ALTER> dicPatientTypeAlter = new Dictionary<long, V_HIS_PATIENT_TYPE_ALTER>();
                if (IsNotNullOrEmpty(hisPatientTypeAlters))
                {
                    foreach (var hisPatientTypeAlter in hisPatientTypeAlters)
                    {
                        dicPatientTypeAlter[hisPatientTypeAlter.TREATMENT_ID] = hisPatientTypeAlter;
                    }
                }

                if (IsNotNullOrEmpty(listSereServ))
                {
                    foreach (var item in listSereServ)
                    {
                        if (item.TDL_HEIN_SERVICE_TYPE_ID == null || item.AMOUNT <= 0 || item.VIR_TOTAL_HEIN_PRICE == 0 || item.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || item.IS_NO_EXECUTE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            continue;
                        if (!dicPatientTypeAlter.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                            continue;
                        var patientTypeAlter = dicPatientTypeAlter[item.TDL_TREATMENT_ID ?? 0];
                        Mrs00358RDO rdo = new Mrs00358RDO();
                        rdo.SERVICE_ID = item.SERVICE_ID;
                        rdo.SERVICE_CODE_DMBYT = item.TDL_HEIN_SERVICE_BHYT_CODE;
                        rdo.SERVICE_STT_DMBYT = item.TDL_HEIN_ORDER;
                        rdo.SERVICE_TYPE_NAME = item.TDL_HEIN_SERVICE_BHYT_NAME;
                        rdo.TOTAL_HEIN_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        rdo.PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO);
                        bool valid = false;

                        if (castFilter.IS_TREAT.HasValue)
                        {
                            if (castFilter.IS_TREAT.Value && patientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                valid = true;
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO) * item.AMOUNT;
                            }
                            else if (!castFilter.IS_TREAT.Value && patientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                valid = true;
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                                rdo.TOTAL_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO) * item.AMOUNT;
                            }
                        }
                        else
                        {
                            valid = true;
                            if (patientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.TREAT)
                            {
                                rdo.AMOUNT_NOITRU = item.AMOUNT;
                            }
                            else if (patientTypeAlter.HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = item.AMOUNT;
                            }

                            rdo.TOTAL_PRICE = item.ORIGINAL_PRICE * (1 + item.VAT_RATIO) * item.AMOUNT;
                        }
                        if (valid)
                        {
                            if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                            {
                                rdo.TOTAL_HEIN_PRICE = item.PRICE;
                                ProcessRdoToList(rdo, patientTypeAlter, EXAM);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, BED);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, TEST);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, DIIMFUEX);
                            }
                            else if (item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, SURGMISU);
                            }
                            else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT && item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, OTHER);
                            }
                            else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT && item.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                            {
                                ProcessRdoToList(rdo, patientTypeAlter, OTHER);
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

        private void ProcessRdoToList(Mrs00358RDO rdo, V_HIS_PATIENT_TYPE_ALTER patientTypeAlter, string code)
        {
            try
            {
                if (!String.IsNullOrWhiteSpace(this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE) && this._Branch.ACCEPT_HEIN_MEDI_ORG_CODE.Contains(patientTypeAlter.HEIN_MEDI_ORG_CODE) && checkBhytProvinceCode(patientTypeAlter.HEIN_CARD_NUMBER))
                {
                    ProcessAddListRdo_A(rdo, code);
                }
                else if (checkBhytProvinceCode(patientTypeAlter.HEIN_CARD_NUMBER))
                {
                    ProcessAddListRdo_B(rdo, code);
                }
                else
                {
                    ProcessAddListRdo_C(rdo, code);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_A(Mrs00358RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_A.Add(rdo); break;
                    case BED: ListBedRdo_A.Add(rdo); break;
                    case TEST: ListTestRdo_A.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_A.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_A.Add(rdo); break;
                    case OTHER: ListOtherRdo_A.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_B(Mrs00358RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_B.Add(rdo); break;
                    case BED: ListBedRdo_B.Add(rdo); break;
                    case TEST: ListTestRdo_B.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_B.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_B.Add(rdo); break;
                    case OTHER: ListOtherRdo_B.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessAddListRdo_C(Mrs00358RDO rdo, string Code)
        {
            try
            {
                switch (Code)
                {
                    case EXAM: ListExamRdo_C.Add(rdo); break;
                    case BED: ListBedRdo_C.Add(rdo); break;
                    case TEST: ListTestRdo_C.Add(rdo); break;
                    case DIIMFUEX: ListDiimFuexRdo_C.Add(rdo); break;
                    case SURGMISU: ListSurgMisuRdo_C.Add(rdo); break;
                    case OTHER: ListOtherRdo_C.Add(rdo); break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<Mrs00358RDO> ProcessListRDO(List<Mrs00358RDO> listRdo)
        {
            List<Mrs00358RDO> listCurrent = new List<Mrs00358RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00358RDO> listsub = group.ToList<Mrs00358RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00358RDO rdo = new Mrs00358RDO();
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.TOTAL_HEIN_PRICE = listsub[0].TOTAL_HEIN_PRICE;
                            rdo.PRICE = listsub[0].PRICE;
                            rdo.AMOUNT_NOITRU = listsub.Sum(s => s.AMOUNT_NOITRU);
                            rdo.AMOUNT_NGOAITRU = listsub.Sum(s => s.AMOUNT_NGOAITRU);
                            rdo.TOTAL_PRICE = listsub.Sum(s => s.TOTAL_PRICE);
                            if (rdo.AMOUNT_NGOAITRU > 0 || rdo.AMOUNT_NOITRU > 0)
                            {
                                listCurrent.Add(rdo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.SERVICE_ID).ThenByDescending(o => o.PRICE).ToList();
        }

        private bool checkBhytProvinceCode(string HeinNumber)
        {
            bool result = false;
            try
            {
                if (!string.IsNullOrEmpty(HeinNumber) && HeinNumber.Length == 15)
                {
                    string provinceCode = HeinNumber.Substring(3, 2);
                    if (this._Branch.HEIN_PROVINCE_CODE.Equals(provinceCode))
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
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListExamRdo_A", ListExamRdo_A);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListBedRdo_A", ListBedRdo_A);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListTestRdo_A", ListTestRdo_A);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListDiimFuexRdo_A", ListDiimFuexRdo_A);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListSurgMisuRdo_A", ListSurgMisuRdo_A);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListOtherRdo_A", ListOtherRdo_A);

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListExamRdo_B", ListExamRdo_B);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListBedRdo_B", ListBedRdo_B);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListTestRdo_B", ListTestRdo_B);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListDiimFuexRdo_B", ListDiimFuexRdo_B);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListSurgMisuRdo_B", ListSurgMisuRdo_B);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListOtherRdo_B", ListOtherRdo_B);

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListExamRdo_C", ListExamRdo_C);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListBedRdo_C", ListBedRdo_C);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListTestRdo_C", ListTestRdo_C);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListDiimFuexRdo_C", ListDiimFuexRdo_C);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListSurgMisuRdo_C", ListSurgMisuRdo_C);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ListOtherRdo_C", ListOtherRdo_C);

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<V_HIS_TREATMENT> GetTreatment()
        {
            ///lay du lieu treatment
            HisTreatmentViewFilterQuery treatmentFilter = new HisTreatmentViewFilterQuery();
            treatmentFilter.FEE_LOCK_TIME_FROM = castFilter.TIME_FROM;
            treatmentFilter.FEE_LOCK_TIME_TO = castFilter.TIME_TO;
            treatmentFilter.IS_PAUSE = true;
            treatmentFilter.IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE;
            return new MOS.MANAGER.HisTreatment.HisTreatmentManager().GetView(treatmentFilter);
        }

        private List<V_HIS_SERE_SERV_3> GetSereServ(List<V_HIS_TREATMENT> treatments)
        {
            List<V_HIS_SERE_SERV_3> result = new List<V_HIS_SERE_SERV_3>();
            int start = 0;
            int count = treatments.Count;
            while (count > 0)
            {
                int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                var tmp = treatments.Skip(start).Take(limit).ToList();

                HisSereServView3FilterQuery ssFilter = new HisSereServView3FilterQuery();
                ssFilter.TREATMENT_IDs = tmp.Select(s => s.ID).ToList();
                ssFilter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                List<V_HIS_SERE_SERV_3> sereServs = new MOS.MANAGER.HisSereServ.HisSereServManager().GetView3(ssFilter);
                if (IsNotNullOrEmpty(sereServs))
                {
                    result.AddRange(sereServs);
                }
                start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
            }
            return result;
        }

        /// <summary>
        /// Lay patient_type_alter dua vao treatmen_id
        /// </summary>
        /// <param name="treatmentIds"></param>
        /// <returns></returns>
        private void GetPatientTypeAlterAndTreatment(List<V_HIS_TREATMENT> treatments, ref List<V_HIS_TREATMENT> treatmentsToUse, ref List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlterToUse)
        {
            patientTypeAlterToUse = new List<V_HIS_PATIENT_TYPE_ALTER>();
            treatmentsToUse = new List<V_HIS_TREATMENT>();
            try
            {
                if (IsNotNullOrEmpty(treatments))
                {
                    CommonParam paramGet = new CommonParam();
                    int start = 0;
                    int count = treatments.Count;
                    List<V_HIS_PATIENT_TYPE_ALTER> tmp = new List<V_HIS_PATIENT_TYPE_ALTER>();
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var lst = treatments.Skip(start).Take(limit).ToList();

                        HisPatientTypeAlterViewFilterQuery filter = new HisPatientTypeAlterViewFilterQuery();
                        filter.TREATMENT_IDs = lst.Select(o => o.ID).ToList();
                        filter.PATIENT_TYPE_ID = HisPatientTypeCFG.PATIENT_TYPE_ID__BHYT;
                        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(filter);

                        if (IsNotNullOrEmpty(patientTypeAlters))
                        {
                            tmp.AddRange(patientTypeAlters);
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }

                    if (IsNotNullOrEmpty(tmp))
                    {
                        foreach (V_HIS_TREATMENT treatment in treatments)
                        {
                            V_HIS_PATIENT_TYPE_ALTER p = tmp
                                .Where(o => o.TREATMENT_ID == treatment.ID)
                                .OrderByDescending(o => o.LOG_TIME).ThenByDescending(o => o.ID).FirstOrDefault();
                            if (p != null)
                            {
                                patientTypeAlterToUse.Add(p);
                                treatmentsToUse.Add(treatment);
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


    }
}
