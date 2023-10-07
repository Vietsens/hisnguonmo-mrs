using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00650
{
    class Mrs00650Processor : AbstractProcessor
    {
        Mrs00650Filter castFilter = null;
        List<Mrs00650RDO> ListRdo = new List<Mrs00650RDO>();

        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicinesMoba = new List<V_HIS_IMP_MEST_MEDICINE>();

        public Mrs00650Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00650Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00650Filter)this.reportFilter;

                // v_his_imp_mest
                HisImpMestViewFilterQuery impMestViewFilter = new HisImpMestViewFilterQuery();
                impMestViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                impMestViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestViewFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                impMestViewFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                listManuImpMests = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestViewFilter);

                if (IsNotNullOrEmpty(listManuImpMests))
                {
                    var skip = 0;
                    while (listManuImpMests.Count - skip > 0)
                    {
                        var listIDs = listManuImpMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery()
                        {
                            IMP_MEST_IDs = listIDs.Select(s => s.ID).ToList()
                        };
                        var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                        listImpMestMedicines.AddRange(listImpMestMedicine);
                    }
                }

                if (IsNotNullOrEmpty(listImpMestMedicines))
                {
                    var skip = 0;
                    while (listImpMestMedicines.Count - skip > 0)
                    {
                        var listIDs = listImpMestMedicines.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                        expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                        expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                        expMestMedicineViewFilter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                        expMestMedicineViewFilter.IS_EXPORT = true;
                        expMestMedicineViewFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        expMestMedicineViewFilter.MEDICINE_IDs = listIDs.Select(s => s.MEDICINE_ID).ToList();
                        var lstExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);

                        listExpMestMedicines.AddRange(lstExpMestMedicine);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    var expMestIds = listExpMestMedicines.Select(s => s.EXP_MEST_ID ?? 0).Distinct().ToList();
                    var skip = 0;
                    while (expMestIds.Count - skip > 0)
                    {
                        var listIDs = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        //trả khác kho không tính 
                        HisImpMestViewFilterQuery impMestMobaFilter = new HisImpMestViewFilterQuery();
                        impMestMobaFilter.MOBA_EXP_MEST_IDs = listIDs;
                        impMestMobaFilter.MEDI_STOCK_IDs = castFilter.MEDI_STOCK_IDs;
                        impMestMobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        var mobaImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(param).GetView(impMestMobaFilter);

                        if (IsNotNullOrEmpty(mobaImpMest))
                        {
                            var impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery()
                            {
                                IMP_MEST_IDs = mobaImpMest.Select(s => s.ID).ToList()
                            };
                            var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);
                            listImpMestMedicinesMoba.AddRange(listImpMestMedicine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                if (IsNotNullOrEmpty(listManuImpMests))
                {
                    Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>> dicImpMestMedicine = new Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>>();
                    Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>> dicExpMestMedicine = new Dictionary<long, List<V_HIS_EXP_MEST_MEDICINE>>();
                    Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>> dicMobaImpMestMedicine = new Dictionary<long, List<V_HIS_IMP_MEST_MEDICINE>>();

                    if (IsNotNullOrEmpty(listImpMestMedicines))
                    {
                        foreach (var item in listImpMestMedicines)
                        {
                            if (!dicImpMestMedicine.ContainsKey(item.IMP_MEST_ID))
                                dicImpMestMedicine[item.IMP_MEST_ID] = new List<V_HIS_IMP_MEST_MEDICINE>();
                            dicImpMestMedicine[item.IMP_MEST_ID].Add(item);
                        }
                    }

                    if (IsNotNullOrEmpty(listExpMestMedicines))
                    {
                        foreach (var item in listExpMestMedicines)
                        {
                            if (!dicExpMestMedicine.ContainsKey(item.MEDICINE_ID ?? 0))
                                dicExpMestMedicine[item.MEDICINE_ID ?? 0] = new List<V_HIS_EXP_MEST_MEDICINE>();
                            dicExpMestMedicine[item.MEDICINE_ID ?? 0].Add(item);
                        }
                    }

                    if (IsNotNullOrEmpty(listImpMestMedicinesMoba))
                    {
                        foreach (var item in listImpMestMedicinesMoba)
                        {
                            if (!dicMobaImpMestMedicine.ContainsKey(item.MEDICINE_ID))
                                dicMobaImpMestMedicine[item.MEDICINE_ID] = new List<V_HIS_IMP_MEST_MEDICINE>();
                            dicMobaImpMestMedicine[item.MEDICINE_ID].Add(item);
                        }
                    }
                    ListRdo.Clear();

                    foreach (var impMest in listManuImpMests)
                    {
                        if (!dicImpMestMedicine.ContainsKey(impMest.ID)) continue;

                        foreach (var impMestMedicine in dicImpMestMedicine[impMest.ID])
                        {
                            Mrs00650RDO rdo = new Mrs00650RDO();
                            rdo.ACTIVE_INGR_BHYT_CODE = impMestMedicine.ACTIVE_INGR_BHYT_CODE;
                            rdo.ACTIVE_INGR_BHYT_NAME = impMestMedicine.ACTIVE_INGR_BHYT_NAME;
                            rdo.DOCUMENT_DATE = impMest.DOCUMENT_DATE;
                            rdo.DOCUMENT_NUMBER = impMest.DOCUMENT_NUMBER;
                            rdo.DOCUMENT_PRICE = impMest.DOCUMENT_PRICE;
                            rdo.EXPIRED_DATE_STR = impMestMedicine.EXPIRED_DATE.HasValue ? Inventec.Common.DateTime.Convert.TimeNumberToDateString(impMestMedicine.EXPIRED_DATE.Value) : "";
                            rdo.IMP_AMOUNT = impMestMedicine.AMOUNT;
                            rdo.IMP_MEST_CODE = impMestMedicine.IMP_MEST_CODE;
                            rdo.IMP_PRICE = impMestMedicine.IMP_PRICE;
                            rdo.IMP_TIME = impMestMedicine.IMP_TIME;
                            rdo.IMP_VAT_RATIO = impMestMedicine.IMP_VAT_RATIO;
                            rdo.MEDI_MATE_CODE = impMestMedicine.MEDICINE_TYPE_CODE;
                            rdo.MEDI_MATE_ID = impMestMedicine.MEDICINE_TYPE_ID;
                            rdo.MEDI_MATE_NAME = impMestMedicine.MEDICINE_TYPE_NAME;
                            rdo.PACKAGE_NUMBER = impMestMedicine.PACKAGE_NUMBER;
                            rdo.REGISTER_NUMBER = impMestMedicine.REGISTER_NUMBER;
                            rdo.SUPPLIER_CODE = impMestMedicine.SUPPLIER_CODE;
                            rdo.SUPPLIER_ID = impMestMedicine.SUPPLIER_ID;
                            rdo.SUPPLIER_NAME = impMestMedicine.SUPPLIER_NAME;

                            rdo.EXP_AMOUNT = 0;
                            if (dicExpMestMedicine.ContainsKey(impMestMedicine.MEDICINE_ID))
                            {
                                rdo.EXP_AMOUNT += dicExpMestMedicine[impMestMedicine.MEDICINE_ID].Sum(s => s.AMOUNT);

                                if (dicMobaImpMestMedicine.ContainsKey(impMestMedicine.MEDICINE_ID))
                                {
                                    rdo.EXP_AMOUNT -= dicMobaImpMestMedicine[impMestMedicine.MEDICINE_ID].Sum(s => s.AMOUNT);
                                }
                            }

                            ListRdo.Add(rdo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                ListRdo = ListRdo.OrderBy(s => s.IMP_TIME).ToList();
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Report", ListRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
