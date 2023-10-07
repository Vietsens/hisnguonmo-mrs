using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00523
{
    class Mrs00523Processor : AbstractProcessor
    {
        Mrs00523Filter castFilter = null;

        public Mrs00523Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<V_HIS_EXP_MEST> listHisExpMest = new List<V_HIS_EXP_MEST>();
        List<V_HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<V_HIS_PATIENT_TYPE_ALTER>();
        private List<Mrs00523RDO> ListRdo = new List<Mrs00523RDO>();

        public override Type FilterType()
        {
            return typeof(Mrs00523Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00523Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00523: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisExpMestViewFilterQuery HisExpMestfilter = new HisExpMestViewFilterQuery();
                HisExpMestfilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                HisExpMestfilter.FINISH_TIME_TO = castFilter.TIME_TO;
                HisExpMestfilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                HisExpMestfilter.ORDER_DIRECTION = "ASC";
                HisExpMestfilter.ORDER_FIELD = "CREATE_TIME";
                HisExpMestfilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                HisExpMestfilter.EXP_MEST_TYPE_IDs = new List<long>(){
                    IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                   IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                   IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DTT,
                   IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DM};
                listHisExpMest = new HisExpMestManager(paramGet).GetView(HisExpMestfilter);

                var listTreatmentId = listHisExpMest.Select(o => o.TDL_TREATMENT_ID??0).Distinct().ToList();
                // Doi tuong tuong ung
                if (listTreatmentId != null && listTreatmentId.Count > 0)
                {
                    var skip = 0;
                    while (listTreatmentId.Count - skip > 0)
                    {
                        var limit = listTreatmentId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisPatientTypeAlterViewFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterViewFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = limit;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "DESC";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ID";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager().GetView(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub Get null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);
                    }
                }
                listHisPatientTypeAlter = listHisPatientTypeAlter.OrderByDescending(o => o.LOG_TIME).ToList();
                var lastPatientTypeAlters = listHisPatientTypeAlter.GroupBy(o=>o.TREATMENT_ID).Select(p=>p.First()).ToList();
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00523:");
                }
                //Dien dieu tri kham
                listHisExpMest = listHisExpMest.Where(o => lastPatientTypeAlters.Exists(p => p.TREATMENT_ID == o.TDL_TREATMENT_ID)).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                resutl = false;
            }
            return resutl;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdo.AddRange((from r in listHisExpMest select new Mrs00523RDO(r)).ToList()??new List<Mrs00523RDO>());
                ListRdo = ListRdo.GroupBy(o => o.TDL_TREATMENT_ID).Select(p => p.First()).ToList();
                groupByDay();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void groupByDay()
        {
            string errorField = "";
            try
            {
                var group = ListRdo.GroupBy(o => new { o.EXP_DATE_STR }).ToList();
                ListRdo.Clear();
                decimal sum = 0;
                Mrs00523RDO rdo;
                List<Mrs00523RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00523RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00523RDO();
                    listSub = item.ToList<Mrs00523RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("COUNT_TREATMENT"))
                        {
                            sum = listSub.Sum(s => (decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) ListRdo.Add(rdo);
                }

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                LogSystem.Info(errorField);
            }
        }
        private Mrs00523RDO IsMeaningful(List<Mrs00523RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00523RDO();
        }
       
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                objectTag.AddObjectData(store, "Report", ListRdo);
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

   
}
