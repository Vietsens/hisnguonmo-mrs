using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisHeinServiceType;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
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

namespace MRS.Processor.Mrs00357
{
    public class Mrs00357Processor : AbstractProcessor
    {
        Mrs00357Filter castFilter = null;

        List<Mrs00357RDO> ListExamRdo = new List<Mrs00357RDO>();
        List<Mrs00357RDO> ListBedRdo = new List<Mrs00357RDO>();
        List<Mrs00357RDO> ListTestRdo = new List<Mrs00357RDO>();
        List<Mrs00357RDO> ListDiimFuexRdo = new List<Mrs00357RDO>();
        List<Mrs00357RDO> ListSurgMisuRdo = new List<Mrs00357RDO>();
        List<Mrs00357RDO> ListOtherRdo = new List<Mrs00357RDO>();

        List<V_HIS_TREATMENT> treatments = new List<V_HIS_TREATMENT>();
        List<V_HIS_PATIENT_TYPE_ALTER> patientTypeAlters = new List<V_HIS_PATIENT_TYPE_ALTER>();
        List<V_HIS_SERE_SERV_3> sereServs = new List<V_HIS_SERE_SERV_3>();

        public Mrs00357Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00357Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            CommonParam paramGet = new CommonParam();
            try
            {
                castFilter = (Mrs00357Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu, MRS00353, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
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

                    if (IsNotNullOrEmpty(ListBedRdo))
                    {
                        ListBedRdo = ProcessListRDO(ListBedRdo);
                    }
                    if (IsNotNullOrEmpty(ListDiimFuexRdo))
                    {
                        ListDiimFuexRdo = ProcessListRDO(ListDiimFuexRdo);
                    }
                    if (IsNotNullOrEmpty(ListExamRdo))
                    {
                        ListExamRdo = ProcessListRDO(ListExamRdo);
                    }
                    if (IsNotNullOrEmpty(ListOtherRdo))
                    {
                        ListOtherRdo = ProcessListRDO(ListOtherRdo);
                    }
                    if (IsNotNullOrEmpty(ListSurgMisuRdo))
                    {
                        ListSurgMisuRdo = ProcessListRDO(ListSurgMisuRdo);
                    }
                    if (IsNotNullOrEmpty(ListTestRdo))
                    {
                        ListTestRdo = ProcessListRDO(ListTestRdo);
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
                    foreach (var sere in listSereServ)
                    {
                        if (sere.IS_EXPEND == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE || sere.AMOUNT <= 0 || sere.TDL_HEIN_SERVICE_TYPE_ID == null)
                            continue;

                        bool valid = false;
                        Mrs00357RDO rdo = new Mrs00357RDO(sere);
                        if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__KH)
                        {
                            valid = true;
                            ListExamRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_NGT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_BN || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__GI_L)
                        {
                            valid = true;
                            ListBedRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__XN)
                        {
                            valid = true;
                            ListTestRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__CDHA || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__TDCN)
                        {
                            valid = true;
                            ListDiimFuexRdo.Add(rdo);
                        }
                        else if (sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__PTTT || sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__DVKTC)
                        {
                            valid = true;
                            ListSurgMisuRdo.Add(rdo);
                        }
                        else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_BLOOD__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__BLOOD__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU)
                        {
                            valid = true;
                            ListOtherRdo.Add(rdo);
                        }
                        else if (MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE_TRAN__SELECTBHYT == MRS.MANAGER.Config.HisHeinServiceTypeCFG.HEIN_SERVICE_TYPE__TRAN__IN__DVKT && sere.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC)
                        {
                            valid = true;
                            ListOtherRdo.Add(rdo);
                        }
                        if (valid)
                        {
                            if (dicPatientTypeAlter[sere.TDL_TREATMENT_ID ?? 0].HEIN_TREATMENT_TYPE_CODE == MOS.LibraryHein.Bhyt.HeinTreatmentType.HeinTreatmentTypeCode.EXAM)
                            {
                                rdo.AMOUNT_NGOAITRU = sere.AMOUNT;
                            }
                            else
                            {
                                rdo.AMOUNT_NOITRU = sere.AMOUNT;
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

        private List<Mrs00357RDO> ProcessListRDO(List<Mrs00357RDO> listRdo)
        {
            List<Mrs00357RDO> listCurrent = new List<Mrs00357RDO>();
            try
            {
                if (listRdo.Count > 0)
                {
                    var groupExams = listRdo.GroupBy(o => new { o.SERVICE_ID, o.VIR_PRICE }).ToList();
                    foreach (var group in groupExams)
                    {
                        List<Mrs00357RDO> listsub = group.ToList<Mrs00357RDO>();
                        if (listsub != null && listsub.Count > 0)
                        {
                            Mrs00357RDO rdo = new Mrs00357RDO();
                            rdo.SERVICE_ID = listsub[0].SERVICE_ID;
                            rdo.SERVICE_CODE_DMBYT = listsub[0].SERVICE_CODE_DMBYT;
                            rdo.SERVICE_STT_DMBYT = listsub[0].SERVICE_STT_DMBYT;
                            rdo.SERVICE_TYPE_NAME = listsub[0].SERVICE_TYPE_NAME;
                            rdo.VIR_PRICE = listsub[0].VIR_PRICE;
                            foreach (var item in listsub)
                            {
                                rdo.AMOUNT_NOITRU += item.AMOUNT_NOITRU;
                                rdo.AMOUNT_NGOAITRU += item.AMOUNT_NGOAITRU;
                                rdo.VIR_TOTAL_PRICE += item.VIR_TOTAL_PRICE;
                            }

                            listCurrent.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listCurrent.Clear();
            }
            return listCurrent.OrderBy(o => o.SERVICE_STT_DMBYT).ToList();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_FROM));
                }

                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXECUTE_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(castFilter.TIME_TO));
                }
                Decimal Total = 0;
                if (ListExamRdo.Count > 0) Total += ListExamRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListBedRdo.Count > 0) Total += ListBedRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListTestRdo.Count > 0) Total += ListTestRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListDiimFuexRdo.Count > 0) Total += ListDiimFuexRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListSurgMisuRdo.Count > 0) Total += ListSurgMisuRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (ListOtherRdo.Count > 0) Total += ListOtherRdo.Sum(o => o.VIR_TOTAL_PRICE);
                if (Total > 0) dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: " + Inventec.Common.String.Convert.CurrencyToVneseString(Total.ToString()) + " đồng");
                else dicSingleTag.Add("TOTAL_MONEY_STR", "Tổng: Không đồng");

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ExamSereServ", ListExamRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "BedSereServ", ListBedRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TestSereServ", ListTestRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "DiimSereServ", ListDiimFuexRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "SurgSereServ", ListSurgMisuRdo);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "OtherSereServ", ListOtherRdo);

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
