using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MOS.MANAGER.HisBaby;
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

namespace MRS.Processor.Mrs00626
{
    class Mrs00626Processor : AbstractProcessor
    {
        Mrs00626Filter castFilter = null;

        public Mrs00626Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        List<HIS_BORN_TYPE> listHisBornType = new List<HIS_BORN_TYPE>();
        List<HIS_BORN_RESULT> listHisBornResult = new List<HIS_BORN_RESULT>();
        List<V_HIS_BABY> listHisBaby = new List<V_HIS_BABY>();
        List<HIS_TREATMENT> listHisTreatment = new List<HIS_TREATMENT>();
        private List<Mrs00626RDO> ListRdo = new List<Mrs00626RDO>();

        public override Type FilterType()
        {
            return typeof(Mrs00626Filter);
        }

        protected override bool GetData()
        {
            bool resutl = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                this.castFilter = (Mrs00626Filter)this.reportFilter;
                Inventec.Common.Logging.LogSystem.Info("Bat dau lay du lieu MRS00626: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));

                HisTreatmentFilterQuery listHisTreatmentfilter = new HisTreatmentFilterQuery();
                listHisTreatmentfilter = this.MapFilter<Mrs00626Filter, HisTreatmentFilterQuery>(castFilter, listHisTreatmentfilter);
                listHisTreatment = new HisTreatmentManager(new CommonParam()).Get(listHisTreatmentfilter);
                
                var treatmentIds = listHisTreatment.Select(o => o.ID).ToList();
                if (treatmentIds != null && treatmentIds.Count > 0)
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var Ids = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisBabyViewFilterQuery HisBabyfilter = new HisBabyViewFilterQuery();
                        HisBabyfilter.TREATMENT_IDs = Ids;
                        HisBabyfilter.ORDER_DIRECTION = "ID";
                        HisBabyfilter.ORDER_FIELD = "ASC";
                        var listHisBabySub = new HisBabyManager(param).GetView(HisBabyfilter);
                        if (listHisBabySub == null)
                            Inventec.Common.Logging.LogSystem.Error("listHisBabySub GetView null");
                        else
                            listHisBaby.AddRange(listHisBabySub);
                    }
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
                ListRdo.AddRange((from r in listHisBaby select new Mrs00626RDO(listHisTreatment.FirstOrDefault(o=>o.ID==r.TREATMENT_ID),r)).ToList() ?? new List<Mrs00626RDO>());
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
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_FROM ?? castFilter.OUT_TIME_FROM ?? 0));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.IN_TIME_TO ?? castFilter.OUT_TIME_TO ?? 0));
                objectTag.AddObjectData(store, "Report", ListRdo);
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
