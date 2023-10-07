using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExpMest;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00104
{
    public class Mrs00104Processor : AbstractProcessor
    {
        Mrs00104Filter castFilter = null;
        List<Mrs00104RDO> ListRdo = new List<Mrs00104RDO>();
        List<V_HIS_SERE_SERV> ListSereServ;
        CommonParam paramGet = new CommonParam();
        HIS_TREATMENT currentTreatment;

        public Mrs00104Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00104Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00104Filter)this.reportFilter;
                if (castFilter == null)
                {
                    Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_SERE_SERV, MRS00104 Filter." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => reportFilter), reportFilter));
                }

                HisTreatmentFilterQuery treatFilter = new HisTreatmentFilterQuery();
                if (!String.IsNullOrEmpty(castFilter.TREATMENT_CODE.Trim()))
                {
                    string code = castFilter.TREATMENT_CODE.Trim();
                    if (code.Length < 12 && checkDigit(code))
                    {
                        code = string.Format("{0:000000000000}", Convert.ToInt64(code));
                    }
                    treatFilter.TREATMENT_CODE__EXACT = code;
                }
                var treatment = new HisTreatmentManager().Get(treatFilter);

                if (treatment != null && treatment.Count == 1)
                {
                    currentTreatment = treatment.FirstOrDefault();
                    HisSereServViewFilterQuery ssFilter = new HisSereServViewFilterQuery();
                    ssFilter.TREATMENT_ID = currentTreatment.ID;
                    ssFilter.SERVICE_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT;
                    //Config.IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VT_TDM; 
                    ssFilter.IS_EXPEND = true;
                    ListSereServ = new MOS.MANAGER.HisSereServ.HisSereServManager(paramGet).GetView(ssFilter);
                    if (!paramGet.HasException)
                    {
                        result = true;
                    }
                    else
                    {
                        Inventec.Common.Logging.LogSystem.Info("Co exception xay ra trong qua tinh lay du lieu MRS00104." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
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

        private bool checkDigit(string s)
        {
            bool result = false;
            try
            {
                for (int i = 0; i < s.Length; i++)
                {
                    if (char.IsDigit(s[i]) == true) result = true;
                    else result = false;
                }
                return result;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return result;
            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ProcessListSereServ(ListSereServ);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListSereServ(List<V_HIS_SERE_SERV> ListSereServ)
        {
            try
            {
                if (IsNotNullOrEmpty(ListSereServ))
                {
                    CommonParam paramGet = new CommonParam();
                    ListSereServ = ListSereServ.Where(o => o.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT && o.IS_EXPEND == 1 && o.AMOUNT > 0).ToList();
                    var Groups = ListSereServ.GroupBy(g => g.SERVICE_REQ_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<V_HIS_SERE_SERV> hisSereServs = group.ToList<V_HIS_SERE_SERV>();
                        ProcessDetailListSereServ(paramGet, hisSereServs);
                    }
                    if (paramGet.HasException)
                    {
                        throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu, MRS00104.");
                    }
                    ListRdo = ListRdo.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).Select(s => new Mrs00104RDO { SERVICE_ID = s.First().SERVICE_ID, VIR_PRICE = s.First().VIR_PRICE, SERVICE_CODE = s.First().SERVICE_CODE, SERVICE_NAME = s.First().SERVICE_NAME, SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME, TOTAL_AMOUNT = s.Sum(s1 => s1.TOTAL_AMOUNT) }).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void ProcessDetailListSereServ(CommonParam paramGet, List<V_HIS_SERE_SERV> hisSereServs)
        {
            try
            {
                if (IsNotNullOrEmpty(hisSereServs))
                {
                    HisExpMestFilterQuery presFilter = new HisExpMestFilterQuery();
                    presFilter.SERVICE_REQ_ID = hisSereServs.First().SERVICE_REQ_ID;
                    presFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; // IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE; 
                    var Prescription = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(presFilter);
                    if (Prescription != null && Prescription.First().EXP_MEST_STT_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                    //IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE)
                    {
                        var Groups = hisSereServs.GroupBy(g => new { g.SERVICE_ID, g.VIR_PRICE }).ToList();
                        foreach (var group in Groups)
                        {
                            List<V_HIS_SERE_SERV> listSub = group.ToList<V_HIS_SERE_SERV>();
                            ListRdo.Add(new Mrs00104RDO(listSub));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void GetTreatmentById(CommonParam paramGet, Dictionary<string, object> dicSingleData)
        {
            try
            {
                if (IsNotNull(currentTreatment))
                {
                    var Treatment = new MOS.MANAGER.HisTreatment.HisTreatmentManager(paramGet).GetViewById(currentTreatment.ID);
                    if (IsNotNull(Treatment))
                    {
                        dicSingleData.Add("TREATMENT_CODE", Treatment.TREATMENT_CODE);
                        dicSingleData.Add("PATIENT_CODE", Treatment.TDL_PATIENT_CODE);
                        dicSingleData.Add("VIR_PATIENT_NAME", Treatment.TDL_PATIENT_NAME);
                        dicSingleData.Add("DOB_YEAR", GenerateDobYear(Treatment.TDL_PATIENT_DOB));
                        dicSingleData.Add("GENDER_NAME", Treatment.TDL_PATIENT_GENDER_NAME);

                        HisPatientTypeAlterViewFilterQuery appFilter = new HisPatientTypeAlterViewFilterQuery();
                        appFilter.TREATMENT_ID = currentTreatment.ID;
                        var currentPatientTypeAlter = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetView(appFilter).OrderBy(o => o.LOG_TIME).ThenBy(p => p.ID).Last();

                        if (currentPatientTypeAlter.HEIN_CARD_NUMBER != null)
                        {
                            dicSingleData.Add("HEIN_CARD_NUMBER", currentPatientTypeAlter.HEIN_CARD_NUMBER);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private string GenerateDobYear(long dob)
        {
            string result = null;
            try
            {
                if (dob > 0)
                {
                    result = dob.ToString().Substring(0, 4);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                GetTreatmentById(paramGet, dicSingleTag);
                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
