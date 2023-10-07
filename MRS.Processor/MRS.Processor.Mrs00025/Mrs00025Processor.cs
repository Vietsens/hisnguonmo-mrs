using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisSereServBill;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00025
{
    public class Mrs00025Processor : AbstractProcessor
    {
        Mrs00025Filter castFilter = null;
        List<Mrs00025RDO> ListRdo = new List<Mrs00025RDO>();
        List<HIS_SERE_SERV_BILL> ListSereServBill = new List<HIS_SERE_SERV_BILL>();
        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> ListCurrentSereServ = new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
        CommonParam paramGet = new CommonParam();

        public Mrs00025Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00025Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00025Filter)this.reportFilter);
                LoadDataToRam();
                result = true;
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
                    ListCurrentSereServ = ListCurrentSereServ.Where(o => ListSereServBill.Select(p => p.SERE_SERV_ID).ToList().Contains(o.ID) || o.VIR_TOTAL_PATIENT_PRICE == 0).ToList();
                    var Groups = ListCurrentSereServ.GroupBy(g => g.PATIENT_TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        List<MOS.EFMODEL.DataModels.HIS_SERE_SERV> listSub = group.ToList<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
                        var patientType = HisPatientTypeCFG.PATIENT_TYPEs.FirstOrDefault(o => o.ID == listSub[0].PATIENT_TYPE_ID) ?? new HIS_PATIENT_TYPE();
                        if (listSub != null && listSub.Count > 0)
                        {
                            Mrs00025RDO dataRDO = new Mrs00025RDO();
                            dataRDO.PATIENT_TYPE_NAME = patientType.PATIENT_TYPE_NAME;
                            dataRDO.PATIENT_TYPE_CODE = patientType.PATIENT_TYPE_CODE;
                            dataRDO.SERVICE_AMOUNT = listSub.Sum(s => s.AMOUNT);
                            dataRDO.VIR_TOTAL_PATIENT_PRICE = listSub.Sum(s => s.VIR_TOTAL_PATIENT_PRICE ?? 0);
                            dataRDO.VIR_TOTAL_HEIN_PRICE = listSub.Sum(s => s.VIR_TOTAL_HEIN_PRICE ?? 0);
                            ListRdo.Add(dataRDO);
                        }
                    }
                    ListRdo = ListRdo.OrderBy(o => o.PATIENT_TYPE_CODE).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();
            }
        }

        private void LoadDataToRam()
        {
            try
            {
                HisSereServFilterQuery filter = new HisSereServFilterQuery();
                filter.TDL_INTRUCTION_TIME_FROM = castFilter.TIME_FROM;
                filter.TDL_INTRUCTION_TIME_TO = castFilter.TIME_TO;
                ListCurrentSereServ = new HisSereServManager().Get(filter);
                //DV - thanh toan
                var listTreatmentId = ListCurrentSereServ.Select(s => s.TDL_TREATMENT_ID??0).Distinct().ToList();

                if (IsNotNullOrEmpty(listTreatmentId))
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var listTreatmentIDs = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisSereServBillFilterQuery filterSereServBill = new HisSereServBillFilterQuery();
                        filterSereServBill.TDL_TREATMENT_IDs = listTreatmentIDs;
                        var listSereServBillSub = new HisSereServBillManager(paramGet).Get(filterSereServBill);
                        ListSereServBill.AddRange(listSereServBillSub);
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListCurrentSereServ = new List<MOS.EFMODEL.DataModels.HIS_SERE_SERV>();
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("BILL_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("BILL_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
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
