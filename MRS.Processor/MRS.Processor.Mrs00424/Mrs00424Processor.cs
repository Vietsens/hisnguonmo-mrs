using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisExpMestMedicine;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisImpMest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisImpMestMaterial;
using Inventec.Common.Logging;
using MOS.MANAGER.HisExpMestType;

namespace MRS.Processor.Mrs00424
{
    class Mrs00424Processor : AbstractProcessor
    {
        Mrs00424Filter castFilter = null;
        List<Mrs00424RDO> listRdo = new List<Mrs00424RDO>();
        List<HIS_EXP_MEST_TYPE> listExpMestTypes = new List<HIS_EXP_MEST_TYPE>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST> listMobaImMests = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();

        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();

        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();

        public Mrs00424Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00424Filter);
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                bool exportSuccess = true;
                objectTag.AddObjectData(store, "Report", listRdo.Where(s => s.TOTAL_AMOUNT > 0).ToList());

                dicSingleTag.Add("MEDI_STOCK_NAME", string.Join(", ", listRdo.Select(o => o.MEDI_STOCK_NAME).Distinct().ToList()));
                dicSingleTag.Add("REQ_DEPARTMENT_NAME", string.Join(", ", listRdo.Select(o => o.REQ_DEPARTMENT_NAME).Distinct().ToList()));

                
                dicSingleTag.Add("EXP_MEST_TYPE_NAME", string.Join(", ",listRdo.Select(o=>o.EXP_MEST_TYPE_NAME).Distinct().ToList()));
                LogSystem.Info("check");
                exportSuccess = exportSuccess && store.SetCommonFunctions();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00424Filter)this.reportFilter;
                HisExpMestMedicineViewFilterQuery expMestMedicineFillter = new HisExpMestMedicineViewFilterQuery();
                expMestMedicineFillter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestMedicineFillter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestMedicineFillter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                expMestMedicineFillter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMestMedicineFillter.IS_EXPORT = true;
                expMestMedicineFillter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                var listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(param).GetView(expMestMedicineFillter);
                listExpMestMedicines.AddRange(listExpMestMedicine);

                HisExpMestMaterialViewFilterQuery expMestMaterialFillter = new HisExpMestMaterialViewFilterQuery();
                expMestMaterialFillter.EXP_TIME_FROM = castFilter.TIME_FROM;
                expMestMaterialFillter.EXP_TIME_TO = castFilter.TIME_TO;
                expMestMaterialFillter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;
                expMestMaterialFillter.EXP_MEST_TYPE_IDs = castFilter.EXP_MEST_TYPE_IDs;
                expMestMaterialFillter.IS_EXPORT = true;
                expMestMaterialFillter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
                var listExpMestMaterial = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(expMestMaterialFillter);
                listExpMestMaterials.AddRange(listExpMestMaterial);

                listExpMestTypes = new HisExpMestTypeManager().Get(new HisExpMestTypeFilterQuery() { IDs = castFilter.EXP_MEST_TYPE_IDs });
               
                var skip = 0;
                var mobaExpMestIds = listExpMestMedicines.Where(o=>o.TH_AMOUNT>0).Select(s => s.EXP_MEST_ID ?? 0).Distinct().ToList();
                while (mobaExpMestIds.Count() - skip > 0)
                {
                    var listIds = mobaExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = +ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestViewFilterQuery mobaImMestFillter = new HisImpMestViewFilterQuery();
                    mobaImMestFillter.IMP_TIME_FROM = castFilter.TIME_FROM;
                    mobaImMestFillter.IMP_TIME_TO = castFilter.TIME_TO;
                    mobaImMestFillter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    mobaImMestFillter.MOBA_EXP_MEST_IDs = listIds;
                    var listMobaImMest = new HisImpMestManager(param).GetView(mobaImMestFillter);
                    listMobaImMests.AddRange(listMobaImMest);
                }

                skip = 0;
                while (listMobaImMests.Count() - skip > 0)
                {
                    var listIds = listMobaImMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = +ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMedicineViewFilterQuery ImpMestMedicineFillter = new HisImpMestMedicineViewFilterQuery();
                    ImpMestMedicineFillter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    ImpMestMedicineFillter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var listImpMestMedicine = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(param).GetView(ImpMestMedicineFillter);
                    listImpMestMedicines.AddRange(listImpMestMedicine);
                }

                skip = 0;
                while (listMobaImMests.Count() - skip > 0)
                {
                    var listIds = listMobaImMests.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = +ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisImpMestMaterialViewFilterQuery ImpMestMaterialFillter = new HisImpMestMaterialViewFilterQuery();
                    ImpMestMaterialFillter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                    ImpMestMaterialFillter.IMP_MEST_IDs = listIds.Select(s => s.ID).ToList();
                    var listImpMestMaterial = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(param).GetView(ImpMestMaterialFillter);
                    listImpMestMaterials.AddRange(listImpMestMaterial);
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

                //var listGroupMedicines = listExpMestMedicines.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.PRICE }).ToList(); 
                foreach (var listExpMestMedicine in listExpMestMedicines)
                {
                    var expMestType = listExpMestTypes.FirstOrDefault(o => o.ID == listExpMestMedicine.EXP_MEST_TYPE_ID);
                    Mrs00424RDO rdo = new Mrs00424RDO();
                    rdo.MEDI_MATE_TYPE_NAME = listExpMestMedicine.MEDICINE_TYPE_NAME;  // ten thuoc
                    if (expMestType != null)
                    {
                        rdo.EXP_MEST_TYPE_NAME = expMestType.EXP_MEST_TYPE_NAME;  // loại xuất
                    }
                    rdo.MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == listExpMestMedicine.MEDI_STOCK_ID)??new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;  // ten kho
                    rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listExpMestMedicine.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;  // ten khoa
                    rdo.SERVICE_UNIT_NAME = listExpMestMedicine.SERVICE_UNIT_NAME;    // don vi
                    rdo.NATIONAL_NAME = listExpMestMedicine.NATIONAL_NAME;            // nuoc sx
                    rdo.PRICE = listExpMestMedicine.PRICE;                            // don gia xuat
                    var EXP_AMOUNT = listExpMestMedicine.AMOUNT;                  // tong xuat
                    rdo.IMP_PRICE = listExpMestMedicine.IMP_PRICE;
                    rdo.VAT_RATIO = listExpMestMedicine.VAT_RATIO;
                    rdo.IMP_VAT_RATIO = listExpMestMedicine.IMP_VAT_RATIO;
                    rdo.CONCENTRA = listExpMestMedicine.CONCENTRA; // hàm lượng nồng độ
                    rdo.ACTIVE_INGR_BHYT_NAME = listExpMestMedicine.ACTIVE_INGR_BHYT_NAME; //tên hoạt chất
                    decimal? TOTAL_EXP_PRICE = EXP_AMOUNT * listExpMestMedicine.PRICE;

                    var listMobaImMest = listMobaImMests.Where(s => s.MOBA_EXP_MEST_ID == listExpMestMedicine.EXP_MEST_ID).Select(s => s.ID).ToList();
                    var listImpMestMedicine = listImpMestMedicines.Where(s => s.TH_EXP_MEST_MEDICINE_ID == listExpMestMedicine.ID).ToList();

                    decimal IMP_AMOUNT = 0;
                    decimal? TOTAL_IMP_PRICE = 0;
                    foreach (var ImpMestMedicine in listImpMestMedicine)
                    {
                        IMP_AMOUNT = ImpMestMedicine.AMOUNT;
                        TOTAL_IMP_PRICE = IMP_AMOUNT * listExpMestMedicine.PRICE;
                    }

                    rdo.TOTAL_AMOUNT = EXP_AMOUNT - IMP_AMOUNT;
                    rdo.TOTAL_PRICE = TOTAL_EXP_PRICE - TOTAL_IMP_PRICE;
                    listRdo.Add(rdo);
                }

                foreach (var listExpMestMaterial in listExpMestMaterials)
                {
                    var expMestType = listExpMestTypes.FirstOrDefault(o => o.ID == listExpMestMaterial.EXP_MEST_TYPE_ID);
                    Mrs00424RDO rdo = new Mrs00424RDO();
                    rdo.MEDI_MATE_TYPE_NAME = listExpMestMaterial.MATERIAL_TYPE_NAME;  // ten vat tu
                    if (expMestType != null)
                    {
                        rdo.EXP_MEST_TYPE_NAME = expMestType.EXP_MEST_TYPE_NAME;  // loại xuất
                    }
                    rdo.MEDI_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == listExpMestMaterial.MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;  // ten kho
                    rdo.REQ_DEPARTMENT_NAME = (HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == listExpMestMaterial.REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT()).DEPARTMENT_NAME;  // ten khoa
                    rdo.SERVICE_UNIT_NAME = listExpMestMaterial.SERVICE_UNIT_NAME;    // don vi
                    rdo.NATIONAL_NAME = listExpMestMaterial.NATIONAL_NAME;            // nuoc sx
                    rdo.PRICE = listExpMestMaterial.PRICE;                            // don gia xuat
                    var EXP_AMOUNT = listExpMestMaterial.AMOUNT;                  // tong xuat
                    decimal? TOTAL_EXP_PRICE = EXP_AMOUNT * listExpMestMaterial.PRICE;
                    rdo.IMP_PRICE = listExpMestMaterial.IMP_PRICE;
                    rdo.VAT_RATIO = listExpMestMaterial.VAT_RATIO;
                    rdo.IMP_VAT_RATIO = listExpMestMaterial.IMP_VAT_RATIO;
                    var listMobaImMest = listMobaImMests.Where(s => s.MOBA_EXP_MEST_ID == listExpMestMaterial.EXP_MEST_ID).Select(s => s.ID).ToList();
                    var listImpMestMaterial = listImpMestMaterials.Where(s => s.TH_EXP_MEST_MATERIAL_ID == listExpMestMaterial.ID).ToList();

                    decimal IMP_AMOUNT = 0;
                    decimal? TOTAL_IMP_PRICE = 0;
                    foreach (var ImpMestMateral in listImpMestMaterial)
                    {
                        IMP_AMOUNT = ImpMestMateral.AMOUNT;
                        TOTAL_IMP_PRICE = IMP_AMOUNT * listExpMestMaterial.PRICE;
                    }

                    rdo.TOTAL_AMOUNT = EXP_AMOUNT - IMP_AMOUNT;
                    rdo.TOTAL_PRICE = TOTAL_EXP_PRICE - TOTAL_IMP_PRICE;
                    listRdo.Add(rdo);
                }
                string keyGroup = "{0}_{1}";
                if (castFilter.INPUT_DATA_ID_GROUP_TYPE == 1)
                {
                    keyGroup = "{0}_{1}";//gộp theo giá bán
                }
                if (castFilter.INPUT_DATA_ID_GROUP_TYPE == 2)
                {
                    keyGroup = "{0}_{2}";//gộp theo giá nhập
                }
                listRdo = listRdo.GroupBy(g =>string.Format(keyGroup, g.MEDI_MATE_TYPE_NAME, (g.PRICE ?? 0) * (1 + (g.VAT_RATIO ?? 0)), g.IMP_PRICE * (1 + g.IMP_VAT_RATIO))).Select(s => new Mrs00424RDO
                {
                    SERVICE_UNIT_NAME = s.First().SERVICE_UNIT_NAME,
                    NATIONAL_NAME = s.First().NATIONAL_NAME,
                    MEDI_MATE_TYPE_NAME = s.First().MEDI_MATE_TYPE_NAME,
                    CONCENTRA = s.First().CONCENTRA,
                    ACTIVE_INGR_BHYT_NAME = s.First().ACTIVE_INGR_BHYT_NAME,
                    EXP_MEST_TYPE_NAME = s.First().EXP_MEST_TYPE_NAME,
                    MEDI_STOCK_NAME = s.First().MEDI_STOCK_NAME,
                    REQ_DEPARTMENT_NAME = s.First().REQ_DEPARTMENT_NAME,
                    PRICE = s.First().PRICE,
                    IMP_PRICE = s.First().IMP_PRICE,
                    VAT_RATIO = s.First().VAT_RATIO,
                    IMP_VAT_RATIO = s.First().IMP_VAT_RATIO,
                    TOTAL_AMOUNT = s.Sum(o => o.TOTAL_AMOUNT??0),
                    TOTAL_PRICE = s.Sum(o => o.TOTAL_PRICE??0)
                }).ToList();

                
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }


    }
}
