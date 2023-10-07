using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestBlood;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00367
{
    class Mrs00367Processor : AbstractProcessor
    {
        Mrs00367Filter castFilter = null;

        List<Mrs00367RDO> ListRdoGroup = new List<Mrs00367RDO>();
        List<Mrs00367RDO> ListRdo = new List<Mrs00367RDO>();
        List<Mrs00367RDO> ListRdoOrderByServiceTypeName = new List<Mrs00367RDO>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_BLOOD> listExpMestBloods = new List<V_HIS_EXP_MEST_BLOOD>();

        public string MEDI_STOCK_NAME = "";
        public string EXP_MEST_REASON_NAME = "";

        public const long GROUP_MEDI_ID = 1;
        public const long GROUP_MATE_ID = 2;
        public const long GROUP_BLOD_ID = 3;

        public Mrs00367Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00367Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                this.castFilter = (Mrs00367Filter)this.reportFilter;

                HisExpMestFilterQuery expFilter = new HisExpMestFilterQuery();
                expFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                expFilter.EXP_MEST_REASON_ID = castFilter.EXP_MEST_REASON_ID;
                expFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC;
                var listExpMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(param).Get(expFilter);
                if (IsNotNullOrEmpty(listExpMest))
                {
                    var skip = 0;
                    while (listExpMest.Count - skip > 0)
                    {
                        var Ids = listExpMest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                        expMestMedicineViewFilter.EXP_MEST_IDs = Ids.Select(o => o.ID).ToList();
                        expMestMedicineViewFilter.IS_EXPORT = true;
                        var medicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter);
                        if (IsNotNullOrEmpty(medicines))
                            listExpMestMedicines.AddRange(medicines);

                        HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                        expMestMaterialViewFilter.EXP_MEST_IDs = Ids.Select(o => o.ID).ToList();
                        expMestMaterialViewFilter.IS_EXPORT = true;
                        var materials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter);
                        if (IsNotNullOrEmpty(materials))
                            listExpMestMaterials.AddRange(materials);

                        HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                        expMestBloodViewFilter.EXP_MEST_IDs = Ids.Select(o => o.ID).ToList();
                        var bloods = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter);
                        if (IsNotNullOrEmpty(bloods))
                            listExpMestBloods.AddRange(bloods);
                    }
                }

                //HisExpMestMedicineViewFilterQuery expMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery();
                //expMestMedicineViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                //expMestMedicineViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                //expMestMedicineViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                //expMestMedicineViewFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC;
                //listExpMestMedicines = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineViewFilter) ?? new List<V_HIS_EXP_MEST_MEDICINE>();

                //HisExpMestMaterialViewFilterQuery expMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery();
                //expMestMaterialViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                //expMestMaterialViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                //expMestMaterialViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                //expMestMaterialViewFilter.EXP_MEST_TYPE_ID = castFilter.EXP_MEST_TYPE_ID;
                //listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialViewFilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>();

                //HisExpMestBloodViewFilterQuery expMestBloodViewFilter = new HisExpMestBloodViewFilterQuery();
                //expMestBloodViewFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
                //expMestBloodViewFilter.EXP_TIME_TO = castFilter.TIME_TO;
                //expMestBloodViewFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                //expMestBloodViewFilter.EXP_MEST_TYPE_ID = castFilter.EXP_MEST_TYPE_ID;
                //listExpMestBloods = new MOS.MANAGER.HisExpMestBlood.HisExpMestBloodManager(param).GetView(expMestBloodViewFilter) ?? new List<V_HIS_EXP_MEST_BLOOD>();

                HisMediStockFilterQuery mediStockFilter = new HisMediStockFilterQuery();
                mediStockFilter.ID = castFilter.MEDI_STOCK_ID;
                MEDI_STOCK_NAME = new MOS.MANAGER.HisMediStock.HisMediStockManager(param).Get(mediStockFilter).First().MEDI_STOCK_NAME.ToUpperInvariant();

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
                if (IsNotNullOrEmpty(listExpMestMedicines))
                {
                    foreach (var expMestMedicine in listExpMestMedicines)
                    {
                        var rdo = new Mrs00367RDO();
                        rdo.GROUP_ID = GROUP_MEDI_ID;
                        rdo.GROUP_NAME = "THUỐC";
                        rdo.SERVICE_TYPE_ID = expMestMedicine.MEDICINE_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = "TH" + expMestMedicine.MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = expMestMedicine.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = expMestMedicine.SERVICE_UNIT_NAME;
                        rdo.MANUFACTURER_NAME = expMestMedicine.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = expMestMedicine.NATIONAL_NAME;
                        rdo.AMOUNT = expMestMedicine.AMOUNT;
                        rdo.IMP_PRICE = expMestMedicine.IMP_PRICE;

                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestMaterials))
                {
                    foreach (var expMestMaterial in listExpMestMaterials)
                    {
                        var rdo = new Mrs00367RDO();
                        rdo.GROUP_ID = GROUP_MATE_ID;
                        rdo.GROUP_NAME = "VẬT TƯ";
                        rdo.SERVICE_TYPE_ID = expMestMaterial.MATERIAL_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = "VT" + expMestMaterial.MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = expMestMaterial.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = expMestMaterial.SERVICE_UNIT_NAME;
                        rdo.MANUFACTURER_NAME = expMestMaterial.MANUFACTURER_NAME;
                        rdo.NATIONAL_NAME = expMestMaterial.NATIONAL_NAME;
                        rdo.AMOUNT = expMestMaterial.AMOUNT;
                        rdo.IMP_PRICE = expMestMaterial.IMP_PRICE;

                        ListRdo.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listExpMestBloods))
                {
                    foreach (var expMestBlood in listExpMestBloods)
                    {
                        var rdo = new Mrs00367RDO();
                        rdo.GROUP_ID = GROUP_BLOD_ID;
                        rdo.GROUP_NAME = "MÁU";
                        rdo.SERVICE_TYPE_ID = expMestBlood.BLOOD_TYPE_ID;
                        rdo.SERVICE_TYPE_CODE = "MA" + expMestBlood.BLOOD_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = expMestBlood.BLOOD_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = expMestBlood.SERVICE_UNIT_NAME;
                        rdo.AMOUNT = 1; //expMestBlood.AMOUNT; 
                        rdo.IMP_PRICE = expMestBlood.IMP_PRICE;

                        ListRdo.Add(rdo);
                    }
                }

                ListRdo = ListRdo.GroupBy(g => g.SERVICE_TYPE_CODE).Select(s => new Mrs00367RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME,
                    SERVICE_TYPE_ID = s.First().SERVICE_TYPE_ID,
                    SERVICE_TYPE_CODE = s.First().SERVICE_TYPE_CODE,
                    SERVICE_TYPE_NAME = s.First().SERVICE_TYPE_NAME,
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    MANUFACTURER_NAME = s.First().MANUFACTURER_NAME,
                    NATIONAL_NAME = s.First().NATIONAL_NAME,
                    AMOUNT = s.Sum(su => su.AMOUNT),
                    IMP_PRICE = s.First().IMP_PRICE
                }).ToList();

                ListRdoGroup = ListRdo.GroupBy(g => g.GROUP_ID).Select(s => new Mrs00367RDO
                {
                    GROUP_ID = s.First().GROUP_ID,
                    GROUP_NAME = s.First().GROUP_NAME
                }).ToList();
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
                    dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
                dicSingleTag.Add("EXP_MEST_REASON_NAME", EXP_MEST_REASON_NAME);

                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Group", ListRdoGroup);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Rdo", ListRdo.OrderBy(s => s.SERVICE_TYPE_NAME).ToList());
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Group", "Rdo", "GROUP_ID", "GROUP_ID");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
