using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisImpSource;
using MOS.MANAGER.HisImpMestMedicine;
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
using HIS.Treatment.DateTime;
using MOS.MANAGER.HisImpMest;

namespace MRS.Processor.Mrs00449
{
    // báo cáo công tác khám chữa bệnh

    class Mrs00449Processor : AbstractProcessor
    {
        Mrs00449Filter castFilter = null;
        List<Mrs00449RDO> listRdo = new List<Mrs00449RDO>();
        List<Mrs00449RDO> listRdoGroup = new List<Mrs00449RDO>();
        List<Mrs00449RDO> listRdoGroupMety = new List<Mrs00449RDO>();

        List<HIS_IMP_MEST> listImpMests = new List<HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST> listManuImpMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_MEDICINE> listMedicines = new List<V_HIS_MEDICINE>();
        List<HIS_MEDICINE_TYPE> listMedicineTypes = new List<HIS_MEDICINE_TYPE>();

        public string SERVICE_GROUP_NAME = "TỔNG HỢP";
        public string MEDI_STOCK_NAME = "";
        public string IMP_SOURCE_NAME = "";

        public bool IS_MEDICINE = false;
        public bool IS_NEUROLOGICAL = false;
        public bool IS_ADDICTIVE = false;

        public Mrs00449Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00449Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00449Filter)this.reportFilter;
                #region blah...blah...
                if (castFilter.IS_MEDICINE)
                {
                    this.IS_MEDICINE = true;
                    SERVICE_GROUP_NAME = "THUỐC THƯỜNG";
                }
                if (castFilter.IS_NEUROLOGICAL)
                {
                    this.IS_NEUROLOGICAL = true;
                    SERVICE_GROUP_NAME = "THUỐC HƯỚNG THẦN";
                }
                if (castFilter.IS_ADDICTIVE)
                {
                    this.IS_ADDICTIVE = true;
                    SERVICE_GROUP_NAME = "THUỐC GÂY NGHIỆN";
                }
                if ((castFilter.IS_MEDICINE && castFilter.IS_ADDICTIVE && castFilter.IS_NEUROLOGICAL) || (!castFilter.IS_ADDICTIVE && !castFilter.IS_MEDICINE && !castFilter.IS_NEUROLOGICAL))
                {
                    this.IS_MEDICINE = true;
                    this.IS_NEUROLOGICAL = true;
                    this.IS_ADDICTIVE = true;
                    SERVICE_GROUP_NAME = "TỔNG HỢP";
                }
                #endregion

                HisImpSourceFilterQuery impSourceFilter = new HisImpSourceFilterQuery();
                impSourceFilter.ID = castFilter.IMP_SOURCE_ID;
                var listImpSources = new MOS.MANAGER.HisImpSource.HisImpSourceManager(param).Get(impSourceFilter);
                if (IsNotNullOrEmpty(listImpSources))
                    this.IMP_SOURCE_NAME = listImpSources.First().IMP_SOURCE_NAME;

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                var listMediStockIds = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter);
                if (IsNotNullOrEmpty(listMediStockIds))
                    this.MEDI_STOCK_NAME = listMediStockIds.First().MEDI_STOCK_NAME;

                HisImpMestMedicineViewFilterQuery impMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery();
                impMestMedicineViewFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
                impMestMedicineViewFilter.IMP_TIME_TO = castFilter.TIME_TO;
                impMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                impMestMedicineViewFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC;
                listImpMestMedicines = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(impMestMedicineViewFilter);

                var listImpMestIds = listImpMestMedicines.Select(s => s.IMP_MEST_ID).Distinct().ToList();

                var skip = 0;
                while (listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisImpMestViewFilterQuery manuImpMestViewFilter = new HisImpMestViewFilterQuery();
                    manuImpMestViewFilter.IDs = listIds;
                    listManuImpMests.AddRange(new HisImpMestManager(param).GetView(manuImpMestViewFilter));
                }

                var listMedicineIds = listImpMestMedicines.Select(s => s.MEDICINE_ID).Distinct().ToList();

                skip = 0;
                var listAllMedicines = new List<V_HIS_MEDICINE>();
                while (listMedicineIds.Count - skip > 0)
                {
                    var listIds = listMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisMedicineViewFilterQuery medicineFilter = new HisMedicineViewFilterQuery();
                    medicineFilter.IDs = listIds;
                    listAllMedicines.AddRange(new MOS.MANAGER.HisMedicine.HisMedicineManager(param).GetView(medicineFilter));
                }

                // lọc theo nguồn nhập
                listMedicines = listAllMedicines.Where(s => s.IMP_SOURCE_ID == castFilter.IMP_SOURCE_ID).ToList();

                var listMedicineTypeIds = listImpMestMedicines.Select(s => s.MEDICINE_TYPE_ID).Distinct().ToList();

                skip = 0;
                var listAllMedicineTypes = new List<HIS_MEDICINE_TYPE>();
                while (listMedicineTypeIds.Count - skip > 0)
                {
                    var listIds = listMedicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                    HisMedicineTypeFilterQuery medicineTypeFilter = new HisMedicineTypeFilterQuery();
                    medicineTypeFilter.IDs = listIds;
                    listAllMedicineTypes.AddRange(new MOS.MANAGER.HisMedicineType.HisMedicineTypeManager(param).Get(medicineTypeFilter));
                }

                if (this.IS_ADDICTIVE && this.IS_MEDICINE && this.IS_NEUROLOGICAL)
                    listMedicineTypes = listAllMedicineTypes;
                else
                {
                    if (this.IS_ADDICTIVE) listMedicineTypes.AddRange(listAllMedicineTypes.Where(s => s.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN).ToList());
                    if (this.IS_NEUROLOGICAL) listMedicineTypes.AddRange(listAllMedicineTypes.Where(s => s.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT).ToList());
                    if (this.IS_MEDICINE) listMedicineTypes.AddRange(listAllMedicineTypes.Where(s => s.MEDICINE_GROUP_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN && s.MEDICINE_GROUP_ID != IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT).ToList());
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
                CommonParam paramGet = new CommonParam();

                foreach (var imp in listImpMestMedicines)
                {
                    var medicineType = listMedicineTypes.Where(s => s.ID == imp.MEDICINE_TYPE_ID).ToList();
                    if (IsNotNullOrEmpty(medicineType))
                    {
                        var medicine = listMedicines.Where(s => s.ID == imp.MEDICINE_ID).ToList();
                        if (IsNotNullOrEmpty(medicine))
                        {
                            var rdo = new Mrs00449RDO();
                            //if (medicineType.First().IS_NEUROLOGICAL == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT)
                                rdo.SERVICE_GROUP_NAME = "THUỐC HƯỚNG THẦN";
                            //else if (medicineType.First().IS_ADDICTIVE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE)
                            else if (medicineType.First().MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN)
                                rdo.SERVICE_GROUP_NAME = "THUỐC GÂY NGHIỆN";
                            else rdo.SERVICE_GROUP_NAME = "THUỐC THƯỜNG";

                            rdo.MEDICINE_TYPE_ID = imp.MEDICINE_TYPE_ID;
                            rdo.MEDICINE_TYPE_CODE = imp.MEDICINE_TYPE_CODE;
                            rdo.MEDICINE_TYPE_NAME = imp.MEDICINE_TYPE_NAME;

                            rdo.MEDICINE_ID = imp.MEDICINE_ID;

                            rdo.SERVICE_UNIT_NAME = imp.SERVICE_UNIT_NAME ?? ". ";

                            rdo.PACKING_TYPE_NAME = imp.PACKING_TYPE_NAME ?? ". ";
                            rdo.NATIONAL_NAME = imp.NATIONAL_NAME ?? ". ";
                            rdo.MANUFACTURER_NAME = imp.MANUFACTURER_NAME ?? ". ";

                            rdo.IMP_MEST_CODE = imp.IMP_MEST_CODE;

                            rdo.AMOUNT = imp.AMOUNT;

                            rdo.IMP_VAT_RATIO = imp.IMP_VAT_RATIO;
                            rdo.IMP_PRICE = imp.IMP_PRICE;

                            rdo.PACKAGE_NUMBER = imp.PACKAGE_NUMBER;
                            rdo.EXPIRED_DATE = imp.EXPIRED_DATE ?? 0;

                            listRdo.Add(rdo);
                        }
                    }
                }

                var listRdoGroupBySGNs = listRdo.GroupBy(g => g.SERVICE_GROUP_NAME);
                foreach (var listRdoGroupBySGN in listRdoGroupBySGNs)
                {
                    var listRdoOrderByMTNs = listRdoGroupBySGN.OrderBy(s => s.MEDICINE_TYPE_NAME);
                    int NUMBER = 1;
                    long curent_ID = 0;
                    foreach (var listRdoOrderByMTN in listRdoOrderByMTNs)
                    {
                        if (curent_ID == 0)
                            curent_ID = listRdoOrderByMTN.MEDICINE_TYPE_ID;
                        if (listRdoOrderByMTN.MEDICINE_TYPE_ID == curent_ID)
                        {
                            listRdoOrderByMTN.NUMBER = NUMBER;
                        }
                        else
                        {
                            NUMBER++;
                            curent_ID = listRdoOrderByMTN.MEDICINE_TYPE_ID;
                            listRdoOrderByMTN.NUMBER = NUMBER;
                        }
                    }
                }

                listRdo = listRdo.OrderBy(o => o.NUMBER).ToList();
                listRdoGroup = listRdo.GroupBy(g => g.SERVICE_GROUP_NAME).Select(s => new Mrs00449RDO { SERVICE_GROUP_NAME = s.First().SERVICE_GROUP_NAME }).ToList();
                listRdoGroupMety = listRdo.GroupBy(g => g.MEDICINE_TYPE_ID).Select(s => new Mrs00449RDO { SERVICE_GROUP_NAME = s.First().SERVICE_GROUP_NAME, MEDICINE_TYPE_ID = s.First().MEDICINE_TYPE_ID, MEDICINE_TYPE_CODE = s.First().MEDICINE_TYPE_CODE, MEDICINE_TYPE_NAME = s.First().MEDICINE_TYPE_NAME }).ToList();
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
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", castFilter.TIME_FROM);
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", castFilter.TIME_TO);
                }

                dicSingleTag.Add("SERVICE_GROUP_NAME", this.SERVICE_GROUP_NAME);
                dicSingleTag.Add("MEDI_STOCK_NAME", this.MEDI_STOCK_NAME.ToUpper());
                dicSingleTag.Add("IMP_SOURCE_NAME", this.IMP_SOURCE_NAME.ToUpper());

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "RdoGroup", listRdoGroup.OrderBy(o => o.SERVICE_GROUP_NAME).ToList());
                objectTag.AddObjectData(store, "RdoGroupMety", listRdoGroupMety);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroup", "RdoGroupMety", "SERVICE_GROUP_NAME", "SERVICE_GROUP_NAME");
                objectTag.AddObjectData(store, "Rdo", listRdo.OrderBy(s => s.NUMBER).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "RdoGroupMety", "Rdo", "MEDICINE_TYPE_ID", "MEDICINE_TYPE_ID");
                objectTag.SetUserFunction(store, "FuncSameTitleCol1", new MergeManyRowData());
                objectTag.SetUserFunction(store, "FuncSameTitleCol2", new MergeManyRowData1());
                objectTag.SetUserFunction(store, "FuncSameTitleCol3", new MergeManyRowData2());
                objectTag.SetUserFunction(store, "FuncSameTitleCol4", new MergeManyRowData3());
                objectTag.SetUserFunction(store, "FuncSameTitleCol5", new MergeManyRowData4());
                objectTag.SetUserFunction(store, "FuncSameTitleCol6", new MergeManyRowData5());
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    class MergeManyRowData : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }

    class MergeManyRowData1 : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }

    class MergeManyRowData2 : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }

    class MergeManyRowData3 : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }

    class MergeManyRowData4 : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }

    class MergeManyRowData5 : FlexCel.Report.TFlexCelUserFunction
    {
        string s_CurrentData;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");
            try
            {
                string s_Data = parameters[0].ToString();
                if (s_Data == s_CurrentData)
                {
                    return true;
                }
                else
                {
                    s_CurrentData = s_Data;
                    return false;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return false;
            }
            throw new NotImplementedException();
        }
    }
}
