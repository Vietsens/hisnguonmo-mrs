using MOS.MANAGER.HisService;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisServiceReq;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisTreatment;

namespace MRS.Processor.Mrs00049
{
    public class Mrs00049Processor : AbstractProcessor
    {
        Mrs00049Filter castFilter = null;
        List<Mrs00049RDO> ListRdo = new List<Mrs00049RDO>();
        List<HIS_SERE_SERV> ListCurrentSereServ = new List<HIS_SERE_SERV>();
        Dictionary<long, HIS_SERVICE_REQ> dicServiceReq = new Dictionary<long, HIS_SERVICE_REQ>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        public Mrs00049Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00049Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00049Filter)this.reportFilter);
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00049: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                if (IsNotNull(castFilter) && !String.IsNullOrEmpty(castFilter.TREATMENT_CODE))
                {
                    LoadDataToRam();
                }
                else
                {
                    Inventec.Common.Logging.LogSystem.Debug(Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                    throw new DataMisalignedException("Filter khong truyen vao treatment_id");
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
                ProcessListCurrentSereServ();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListCurrentSereServ()
        {
            try
            {
                if (ListCurrentSereServ != null && ListCurrentSereServ.Count > 0)
                {
                    ListCurrentSereServ = ListCurrentSereServ.OrderBy(o => o.TDL_INTRUCTION_TIME).ThenBy(o => o.TDL_SERVICE_TYPE_ID).ToList();
                    ListRdo = (from r in ListCurrentSereServ select new Mrs00049RDO(r)).ToList();
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
                listHisTreatment = new HisTreatmentManager().Get(treatFilter);

                if (listHisTreatment != null && listHisTreatment.Count == 1)
                {
                    HisSereServFilterQuery filter = new HisSereServFilterQuery();
                    filter.TREATMENT_ID = listHisTreatment.FirstOrDefault().ID;
                    ListCurrentSereServ = new HisSereServManager().Get(filter);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (ListRdo.Count > 0)
                {
                    dicSingleTag.Add("TREATMENT_CODE", ListRdo[0].TDL_TREATMENT_CODE);
                    dicSingleTag.Add("VIR_PATIENT_NAME", listHisTreatment[0].TDL_PATIENT_NAME);
                    dicSingleTag.Add("GENDER_NAME", listHisTreatment[0].TDL_PATIENT_GENDER_NAME);
                    dicSingleTag.Add("DOB_DATE_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(listHisTreatment[0].TDL_PATIENT_DOB));
                }

                objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
