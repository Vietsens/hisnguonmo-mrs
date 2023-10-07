using Inventec.Common.FlexCellExport;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisHeinApproval;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisTreatmentLogging;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00546
{
    class Mrs00546Processor : AbstractProcessor
    {
        Mrs00546Filter castFilter = null;
        List<Mrs00546RDO> listRdo = new List<Mrs00546RDO>();

        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();
        Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>> dicMedicine = new Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>>();
        List<HIS_MEDICINE_TYPE> listMedicineType = new List<HIS_MEDICINE_TYPE>();

        public Mrs00546Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00546Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                castFilter = ((Mrs00546Filter)this.reportFilter);
                if (!IsNotNull(castFilter))
                {
                    throw new NullReferenceException("Nguoi dung truyen len khong chin xac");
                }
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu HIS_TREATMENT, Mrs00541, filter: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                listMedicineType = new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(paramGet).Get(new MOS.MANAGER.HisMedicineType.HisMedicineTypeFilterQuery());

                HisHeinApprovalViewFilterQuery approvalFilter = new HisHeinApprovalViewFilterQuery();
                approvalFilter.EXECUTE_TIME_FROM = castFilter.TIME_FROM;
                approvalFilter.EXECUTE_TIME_TO = castFilter.TIME_TO;
                //approvalFilter.BRANCH_ID = castFilter.BRANCH_ID;
                approvalFilter.ORDER_DIRECTION = "ASC";
                approvalFilter.ORDER_FIELD = "EXECUTE_TIME";
                var ListHeinApproval = new HisHeinApprovalManager(paramGet).GetView(approvalFilter);

                List<long> treatmentIds = ListHeinApproval.Select(s => s.TREATMENT_ID).Distinct().ToList();
                List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
                if (IsNotNullOrEmpty(treatmentIds))
                {
                    var skip = 0;
                    while (treatmentIds.Count - skip > 0)
                    {
                        var limit = treatmentIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisTreatmentFilterQuery FilterTreatment = new HisTreatmentFilterQuery();
                        FilterTreatment.IDs = limit;
                        var treatment = new HisTreatmentManager(paramGet).Get(FilterTreatment);
                        if (IsNotNullOrEmpty(treatment))
                            listTreatment.AddRange(treatment);

                        HisExpMestFilterQuery expFilter = new HisExpMestFilterQuery();
                        expFilter.TDL_TREATMENT_IDs = limit;
                        expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                        var expMest = new HisExpMestManager(paramGet).Get(expFilter);
                        if (IsNotNullOrEmpty(expMest))
                            listExpMest.AddRange(expMest);
                    }
                }

                if (IsNotNullOrEmpty(listExpMest))
                {
                    var skip = 0;
                    while (listExpMest.Count - skip > 0)
                    {
                        var limit = listExpMest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                        mediFilter.EXP_MEST_IDs = limit.Select(o => o.ID).ToList();
                        mediFilter.IS_EXPORT = true;
                        var medi = new HisExpMestMedicineManager(paramGet).GetView(mediFilter);
                        if (IsNotNullOrEmpty(medi))
                        {
                            foreach (var item in limit)
                            {
                                if (!medi.Exists(o => o.EXP_MEST_ID == item.ID))
                                    continue;
                                if (!dicMedicine.ContainsKey(item.TDL_TREATMENT_ID ?? 0))
                                    dicMedicine[item.TDL_TREATMENT_ID ?? 0] = new List<V_HIS_EXP_MEST_MEDICINE>();
                                dicMedicine[item.TDL_TREATMENT_ID ?? 0].AddRange(medi.Where(o => o.EXP_MEST_ID == item.ID).ToList());
                            }
                        }
                    }
                }

                if (paramGet.HasException)
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu, Mrs00546." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => paramGet), paramGet));
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
                Dictionary<int, HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<int, HIS_MEDICINE_TYPE>();
                if (IsNotNullOrEmpty(listTreatment))
                {
                    foreach (var treatment in listTreatment)
                    {
                        if (!dicMedicine.ContainsKey(treatment.ID))
                            continue;
                        var medicines = dicMedicine[treatment.ID];
                        var groupExp = medicines.GroupBy(o => o.EXP_MEST_ID).ToList();
                        foreach (var type in groupExp)
                        {
                            Mrs00546RDO rdo = new Mrs00546RDO();
                            rdo.EXP_MEST_CODE = type.First().EXP_MEST_CODE;
                            rdo.TDL_PATIENT_NAME = treatment.TDL_PATIENT_NAME;

                            var medigr = type.GroupBy(o => new { o.PRICE, o.TDL_MEDICINE_TYPE_ID }).ToList();

                            foreach (var item in medigr)
                            {
                                rdo.TOTAL_PRICE += (item.Sum(o => o.AMOUNT) * (item.First().PRICE ?? 0) * (1 + item.First().VAT_RATIO ?? 0));

                                int count = 0;
                                for (int i = 0; i < 60; i++)
                                {
                                    count += 1;
                                    if (!dicMedicineType.ContainsKey(count))
                                        break;
                                    if (dicMedicineType[count].ID == item.First().TDL_MEDICINE_TYPE_ID)
                                        break;
                                }
                                if (count > 60)
                                    break;

                                HIS_MEDICINE_TYPE t = IsNotNullOrEmpty(listMedicineType) ? listMedicineType.FirstOrDefault(o => o.ID == item.First().TDL_MEDICINE_TYPE_ID) : null;
                                dicMedicineType[count] = t ?? new HIS_MEDICINE_TYPE();

                                System.Reflection.PropertyInfo piAmount = typeof(Mrs00546RDO).GetProperty("MEDICINE_AMOUNT_" + count);
                                System.Reflection.PropertyInfo piType = typeof(Mrs00546RDO).GetProperty("MEDICINE_TYPE_ID_" + count);
                                System.Reflection.PropertyInfo piName = typeof(Mrs00546RDO).GetProperty("MEDICINE_TYPE_NAME_" + count);
                                piType.SetValue(rdo, dicMedicineType[count].ID);
                                piName.SetValue(rdo, dicMedicineType[count].MEDICINE_TYPE_NAME);
                                piAmount.SetValue(rdo, item.Sum(s => s.AMOUNT));
                            }

                            listRdo.Add(rdo);
                        }
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

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {
                #region Cac the Single
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                var medistock = new MOS.MANAGER.HisMediStock.HisMediStockManager(new CommonParam()).GetById(castFilter.MEDI_STOCK_ID);
                dicSingleTag.Add("MEDI_STOCK_NAME", IsNotNull(medistock) ? medistock.MEDI_STOCK_NAME : "");
                #endregion

                ProcessName(dicSingleTag);

                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessName(Dictionary<string, object> dicSingleTag)
        {
            try
            {
                if (IsNotNullOrEmpty(listRdo))
                {
                    for (int i = 1; i < 61; i++)
                    {
                        string key = "MEDICINE_TYPE_NAME_" + i;
                        System.Reflection.PropertyInfo piName = typeof(Mrs00546RDO).GetProperty(key);
                        string value = "";
                        foreach (var item in listRdo)
                        {
                            value = "";
                            object v = piName.GetValue(item);
                            if (IsNotNull(v))
                            {
                                value = v.ToString();
                            }

                            if (!String.IsNullOrWhiteSpace(value))
                                break;
                        }
                        dicSingleTag.Add(key, value);
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
