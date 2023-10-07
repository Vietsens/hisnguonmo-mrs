using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisBaby;
using MOS.MANAGER.HisDepartmentTran;
using MOS.MANAGER.HisIcd;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisTreatment;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00592
{
    class Mrs00592Processor : AbstractProcessor
    {
        Mrs00592Filter castFilter = null;

        public Mrs00592Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<HIS_BABY> listHisBaby = new List<HIS_BABY>();
        List<HIS_DEPARTMENT_TRAN> lastHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
        List<HIS_PATIENT_TYPE_ALTER> lastHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();

        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        private List<Mrs00592RDO> ListRdo = new List<Mrs00592RDO>();

        private List<Mrs00592RDO> sumRdo = new List<Mrs00592RDO>();
        public override Type FilterType()
        {
            return typeof(Mrs00592Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00592Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00592: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter = this.MapFilter<Mrs00592Filter, HisTreatmentFilterQuery>(castFilter, listHisTreatmentfilter);
                listHisTreatment = new HisTreatmentManager(new CommonParam()).Get(listHisTreatmentfilter);
                
                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {

                    List<HIS_DEPARTMENT_TRAN> listHisDepartmentTran = new List<HIS_DEPARTMENT_TRAN>();
                    List<HIS_PATIENT_TYPE_ALTER> listHisPatientTypeAlter = new List<HIS_PATIENT_TYPE_ALTER>();
                    List<HIS_SERVICE_REQ> listHisServiceReq = new List<HIS_SERVICE_REQ>();
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisBabyFilterQuery HisBabyfilter = new HisBabyFilterQuery();
                        HisBabyfilter.TREATMENT_IDs = Ids;
                        HisBabyfilter.ORDER_DIRECTION = "ID";
                        HisBabyfilter.ORDER_FIELD = "ASC";
                        var listHisBabySub = new HisBabyManager(param).Get(HisBabyfilter);
                        if (listHisBabySub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisBabySub GetView null");
                        else
                            listHisBaby.AddRange(listHisBabySub);
                        HisDepartmentTranFilterQuery HisDepartmentTranfilter = new HisDepartmentTranFilterQuery();
                        HisDepartmentTranfilter.TREATMENT_IDs = Ids;
                        HisDepartmentTranfilter.ORDER_DIRECTION = "ID";
                        HisDepartmentTranfilter.ORDER_FIELD = "ASC";
                        var listHisDepartmentTranSub = new HisDepartmentTranManager(param).Get(HisDepartmentTranfilter);
                        if (listHisDepartmentTranSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisDepartmentTranSub GetView null");
                        else
                            listHisDepartmentTran.AddRange(listHisDepartmentTranSub);

                        HisPatientTypeAlterFilterQuery HisPatientTypeAlterfilter = new HisPatientTypeAlterFilterQuery();
                        HisPatientTypeAlterfilter.TREATMENT_IDs = Ids;
                        HisPatientTypeAlterfilter.ORDER_DIRECTION = "ID";
                        HisPatientTypeAlterfilter.ORDER_FIELD = "ASC";
                        var listHisPatientTypeAlterSub = new HisPatientTypeAlterManager(param).Get(HisPatientTypeAlterfilter);
                        if (listHisPatientTypeAlterSub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisPatientTypeAlterSub GetView null");
                        else
                            listHisPatientTypeAlter.AddRange(listHisPatientTypeAlterSub);

                    }

                    lastHisDepartmentTran = listHisDepartmentTran.Where(p => p.DEPARTMENT_IN_TIME.HasValue).OrderBy(o => o.DEPARTMENT_IN_TIME.Value).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();
                    lastHisPatientTypeAlter = listHisPatientTypeAlter.OrderBy(o => o.LOG_TIME).ThenBy(q => q.ID).GroupBy(p => p.TREATMENT_ID).Select(q => q.Last()).ToList();

                  
                }
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    listHisTreatment = listHisTreatment.Where(p => lastHisDepartmentTran.Exists(o => o.TREATMENT_ID == p.ID && castFilter.DEPARTMENT_IDs.Contains(o.DEPARTMENT_ID))).ToList();

                }
               
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                resutl = false;
            }
            return resutl;
        }
        private TDest MapFilter<TSource, TDest>(TSource filterS, TDest filterD)
        {
            try
            {

                PropertyInfo[] piSource = typeof(TSource).GetProperties();
                PropertyInfo[] piDest = typeof(TDest).GetProperties();
                foreach (var item in piDest)
                {
                    if (piSource.ToList().Exists(o => o.Name == item.Name && o.GetType() == item.GetType()))
                    {
                        PropertyInfo sField = piSource.FirstOrDefault(o => o.Name == item.Name && o.GetType() == item.GetType());
                        if (sField.GetValue(filterS) != null)
                        {
                            item.SetValue(filterD, sField.GetValue(filterS));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                return filterD;
            }

            return filterD;

        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                ListRdo.AddRange((from r in listHisTreatment select new Mrs00592RDO(r, lastHisPatientTypeAlter, listHisBaby, this.castFilter)).ToList() ?? new List<Mrs00592RDO>());
                ListRdo = ListRdo.Where(o => o.TREATMENT_ID != 0).ToList();
                SumRdo();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void SumRdo()
        {
            string errorField = "";
            try
            {
                decimal sum = 0;
                Mrs00592RDO rdo =new Mrs00592RDO();
                ListRdo = ListRdo.Distinct().ToList();
                PropertyInfo[] pi = Properties.Get<Mrs00592RDO>();
                    foreach (var field in pi)
                    {
                        if (field.Name == "TREATMENT_ID") continue;
                        errorField = field.Name;

                        sum = ListRdo.Sum(s => (decimal)field.GetValue(s));
                            field.SetValue(rdo, sum);
                        
                    }
                    sumRdo.Add(rdo);
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_FROM ?? castFilter.OUT_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_TO ?? castFilter.OUT_TIME_TO ?? 0));
                objectTag.AddObjectData(store, "Report", sumRdo);
                if (castFilter.DEPARTMENT_IDs != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAME", string.Join(",", HisDepartmentCFG.DEPARTMENTs.Where(o => castFilter.DEPARTMENT_IDs.Contains(o.ID)).Select(p => p.DEPARTMENT_NAME).ToList()));
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

   
}
