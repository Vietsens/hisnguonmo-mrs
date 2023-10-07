using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 
using AutoMapper; 
using Inventec.Common.DateTime; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MRS.MANAGER.Config; 
using MOS.Filter; 
using MOS.EFMODEL.DataModels; 
using MOS.MANAGER.HisTreatment; 
using MOS.MANAGER.HisMediOrg; 
using MOS.MANAGER.HisTranPatiForm; 
using MRS.MANAGER.Core.MrsReport; 

namespace MRS.Processor.Mrs00176
{
    internal class Mrs00176Processor : AbstractProcessor
    {
        List<Mrs00176RDO> listRdo = new List<Mrs00176RDO>(); //giống 
        Mrs00176Filter CastFilter; 
        private const string provinceCode_HN = "101"; 
        private const string Region_HN = "Khu vực Hà Nội"; 
        private const string Region_Other = "Khu vực khác"; 

        public Mrs00176Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00176Filter); 
        }

        protected override bool GetData()
        {
            var result = true; 
            try
            {
                var paramGet = new CommonParam(); 
                CastFilter = (Mrs00176Filter)this.reportFilter; 

                HisTreatmentFilterQuery Filter = new HisTreatmentFilterQuery(); 
                Filter.IN_TIME_FROM = CastFilter.DATE_FROM;
                Filter.IN_TIME_TO = CastFilter.DATE_TO; 
                Filter.TREATMENT_END_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_TREATMENT_END_TYPE.ID__CHUYEN; 
                List<HIS_TREATMENT> ListTreatmentTranPati = new HisTreatmentManager(paramGet).Get(Filter); 
                long year = CastFilter.DATE_FROM; 
                year = (long)10000000000 * (int)(CastFilter.DATE_FROM / 10000000000); 
                ProcessFilterData(ListTreatmentTranPati, year); 
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
            return true; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM)); 
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO)); 
            dicSingleTag.Add("YEAR", ((int)(CastFilter.DATE_FROM / 10000000000)).ToString()); 
            objectTag.AddObjectData(store, "Report1", listRdo); 
            objectTag.AddObjectData(store, "Report2", listRdo.GroupBy(o => new { o.PLACE_NAME, o.TRAN_PATI_FORM_ID }).Select(p => p.First()).ToList()); 
            objectTag.AddObjectData(store, "Report3", listRdo.GroupBy(o => o.TRAN_PATI_FORM_ID).Select(p => p.First()).ToList()); 
            string[] ship = { "PLACE_NAME", "TRAN_FORM" }; 
            objectTag.AddRelationship(store, "Report2", "Report1", ship, ship); 
            objectTag.AddRelationship(store, "Report3", "Report2", "TRAN_FORM", "TRAN_FORM"); 
        }

        private void ProcessFilterData(List<HIS_TREATMENT> ListTreatmentTranPati, long year)
        {
            try
            {
                Dictionary<string, HIS_MEDI_ORG> dicMediOrg = LoadDicHisMediOrg(); 
                Dictionary<long, HIS_TRAN_PATI_FORM> dicTranPatiForm = LoadTranPatiForm(); 

                var GroupBy = ListTreatmentTranPati.GroupBy(o => o.MEDI_ORG_CODE).ToList(); 
                foreach (var item in GroupBy)
                {
                    if (!IsNotNullOrEmpty(item.Key)) continue; 
                    Mrs00176RDO rdo = new Mrs00176RDO(); 
                    rdo.MEDI_ORG_CODE = item.Key; 
                    rdo.MEDI_ORG_NAME = item.Last().MEDI_ORG_NAME;
                    if (dicMediOrg.ContainsKey(item.Key))
                    {
                        rdo.PLACE_NAME = (dicMediOrg[item.Key].PROVINCE_CODE == provinceCode_HN) ? Region_HN : Region_Other;
                    }
                    rdo.TRAN_PATI_FORM_ID = item.First().TRAN_PATI_FORM_ID ?? 0; 
                    if(dicTranPatiForm.ContainsKey(item.First().TRAN_PATI_FORM_ID ?? 0))
                    {
                    rdo.TRAN_FORM =dicTranPatiForm[item.First().TRAN_PATI_FORM_ID ?? 0].TRAN_PATI_FORM_NAME; 
                    }
                    rdo.SUM = item.Count(); 

                    listRdo.Add(rdo); 
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
            }
        }

        private Dictionary<long, HIS_TRAN_PATI_FORM> LoadTranPatiForm()
        {
            Dictionary<long, HIS_TRAN_PATI_FORM> result = new Dictionary<long,HIS_TRAN_PATI_FORM>(); 
            try
            {
                CommonParam param = new CommonParam(); 
                HisTranPatiFormFilterQuery filter = new HisTranPatiFormFilterQuery(); 
                var listTranPatiForm = new HisTranPatiFormManager(param).Get(filter); 
                foreach (var item in listTranPatiForm)
                {
                    if (!result.ContainsKey(item.ID)) result[item.ID] = item; 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                return null; 
            }
            return result; 
        }

        private Dictionary<string, HIS_MEDI_ORG> LoadDicHisMediOrg()
        {
            Dictionary<string, HIS_MEDI_ORG> result = new Dictionary<string,HIS_MEDI_ORG>(); 
            try
            {
                CommonParam param = new CommonParam(); 
                HisMediOrgFilterQuery filter = new HisMediOrgFilterQuery(); 
                var listMediOrg = new HisMediOrgManager(param).Get(filter); 
                foreach (var item in listMediOrg)
                {
                    if (!result.ContainsKey(item.MEDI_ORG_CODE)) result[item.MEDI_ORG_CODE] = item; 
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex); 
                return null; 
            }
            return result; 
        }
    }
}
