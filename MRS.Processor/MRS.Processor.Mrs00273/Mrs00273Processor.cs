using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisImpMest;
//using ACS.EFMODEL.DataModels; 
using AutoMapper; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
using MOS.MANAGER.HisExpMest; 
using MOS.MANAGER.HisExpMestMaterial; 
using MOS.MANAGER.HisExpMestMedicine; 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisMaterialType; 
using MOS.MANAGER.HisMedicineType; 
using MOS.MANAGER.HisMediStock; 
using MRS.SDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using ACS.MANAGER.Manager;
using ACS.EFMODEL.DataModels;
using ACS.MANAGER.Core.AcsUser.Get; 

namespace MRS.Processor.Mrs00273
{
    class Mrs00273Processor : AbstractProcessor
    {
        List<Mrs00273RDO> ListMedicineRdo = new List<Mrs00273RDO>(); 
        List<Mrs00273RDO> ListMaterialRdo = new List<Mrs00273RDO>(); 
        List<V_HIS_IMP_MEST_MEDICINE> listMedicine = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<HIS_EXP_MEST_MEDICINE>(); 
        List<HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_IMP_MEST> listMobaImpMest = new List<V_HIS_IMP_MEST>();
        List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
        List<long> MobaSaleImpMestTypeIds = new List<long>() 
        {
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH
        };
        CommonParam paramGet = new CommonParam(); 
        public Mrs00273Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }
        public override Type FilterType()
        {
            return typeof(Mrs00273Filter); 
        }


        protected override bool GetData()
        {
            var filter = ((Mrs00273Filter)reportFilter); 
            bool result = true; 
            try
            {
                HisImpMestViewFilterQuery mobafilter = new HisImpMestViewFilterQuery(); 
                mobafilter.IMP_TIME_FROM = filter.TIME_FROM; 
                mobafilter.IMP_TIME_TO = filter.TIME_TO; 
                //mobafilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH; 
                mobafilter.MEDI_STOCK_IDs = filter.MEDI_STOCK_BUSINESS_IDs;
                listMobaImpMest = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(mobafilter);
                if (listMobaImpMest != null)
                {
                    listMobaImpMest = listMobaImpMest.Where(o => MobaSaleImpMestTypeIds.Contains(o.IMP_MEST_TYPE_ID)).ToList();
                }
                var mobaExpMestId = listMobaImpMest.Select(o => o.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList();


                if (mobaExpMestId.Count > 0)
                {
                    var skip = 0;
                    while (mobaExpMestId.Count - skip > 0)
                    {
                        var lists = mobaExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList(); 
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM; 

                        HisExpMestFilterQuery salefilter = new HisExpMestFilterQuery();
                        salefilter.IDs = lists; 
                        salefilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                        var listExpMestSub = new HisExpMestManager(paramGet).Get(salefilter);
                        if (listExpMestSub == null)
                        {
                            throw new NullReferenceException("HIS_EXP_MEST is null");
                        }
                        listExpMest.AddRange(listExpMestSub);
                    }
                    if (!String.IsNullOrEmpty(((Mrs00273Filter)reportFilter).LOGINNAME_SALE))
                    {
                        listExpMest = listExpMest.Where(o => o.CREATOR == filter.LOGINNAME_SALE).ToList();
                        mobaExpMestId = mobaExpMestId.Where(o => listExpMest.Exists(p => p.ID == o)).ToList();
                        listMobaImpMest = listMobaImpMest.Where(o => listExpMest.Exists(p => p.ID == o.MOBA_EXP_MEST_ID)).ToList();

                    }
                }

                if (mobaExpMestId.Count > 0)
                {
                    var skip = 0;
                    while (mobaExpMestId.Count - skip > 0)
                    {
                        var lists = mobaExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisExpMestMedicineFilterQuery ExpMestMedicinefilter = new HisExpMestMedicineFilterQuery();
                        ExpMestMedicinefilter.EXP_MEST_IDs = lists;
                        ExpMestMedicinefilter.IS_EXPORT = true;
                        var listExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).Get(ExpMestMedicinefilter);
                        if (listExpMestMedicineSub == null)
                        {
                            throw new NullReferenceException("HIS_EXP_MEST_MEDICINE is null");
                        }
                        listExpMestMedicine.AddRange(listExpMestMedicineSub);

                        HisExpMestMaterialFilterQuery ExpMestMaterialfilter = new HisExpMestMaterialFilterQuery();
                        ExpMestMaterialfilter.EXP_MEST_IDs = lists;
                        ExpMestMaterialfilter.IS_EXPORT = true;
                        var listExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).Get(ExpMestMaterialfilter);
                        if (listExpMestMaterialSub == null)
                        {
                            throw new NullReferenceException("HIS_EXP_MEST_MATERIAL is null");
                        }
                        listExpMestMaterial.AddRange(listExpMestMaterialSub);
                    }
                }

                var impMestId = listMobaImpMest.Select(o => o.ID).ToList();

                if (impMestId.Count > 0)
                {
                    var skip = 0;
                    while (impMestId.Count - skip > 0)
                    {
                        var lists = impMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        HisImpMestMedicineViewFilterQuery ImpMestMedicinefilter = new HisImpMestMedicineViewFilterQuery();
                        ImpMestMedicinefilter.IMP_MEST_IDs = lists;
                        var listImpMestMedicineSub = new HisImpMestMedicineManager(paramGet).GetView(ImpMestMedicinefilter);
                        if (listImpMestMedicineSub == null)
                        {
                            throw new NullReferenceException("V_HIS_IMP_MEST_MEDICINE is null");
                        }
                        listMedicine.AddRange(listImpMestMedicineSub);

                        HisImpMestMaterialViewFilterQuery ImpMestMaterialfilter = new HisImpMestMaterialViewFilterQuery();
                        ImpMestMaterialfilter.IMP_MEST_IDs = lists;
                        var listImpMestMaterialSub = new HisImpMestMaterialManager(paramGet).GetView(ImpMestMaterialfilter);
                        if (listImpMestMaterialSub == null)
                        {
                            throw new NullReferenceException("V_HIS_IMP_MEST_MATERIAL is null");
                        }
                        listMaterial.AddRange(listImpMestMaterialSub);
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

        protected override bool ProcessData()
        {
            var result = true; 
            try
            {
                ListMedicineRdo.Clear(); 
                ListMaterialRdo.Clear(); 

                if (IsNotNullOrEmpty(listMedicine))
                {
                    //Dictionary<long ,List<V_HIS_IMP_MEST_MEDICINE>> DicMedicine = new Dictionary<long ,List<V_HIS_IMP_MEST_MEDICINE>>(); 
                    foreach (var medicine in listMedicine)
                    {

                        Mrs00273RDO rdo = new Mrs00273RDO(); 
                        rdo.MEDICINE_TYPE_NAME = medicine.MEDICINE_TYPE_NAME; 
                        rdo.NATIONAL_NAME = medicine.NATIONAL_NAME; 
                        //rdo.PRICE = listExpMestMedicine
                        //.Where(o => o.EXP_MEST_ID == listMobaImpMest
                        //           .Where(p => p.IMP_MEST_ID == medicine.IMP_MEST_ID).First().EXP_MEST_ID && o.MEDICINE_ID == medicine.MEDICINE_ID).First().PRICE ?? medicine.IMP_PRICE; 
                        var expMedi = listExpMestMedicine.FirstOrDefault(o => listMobaImpMest.Exists(p => p.ID == medicine.IMP_MEST_ID && p.MOBA_EXP_MEST_ID == o.EXP_MEST_ID) && o.MEDICINE_ID == medicine.MEDICINE_ID); 
                        if (expMedi != null && expMedi.PRICE.HasValue)
                        {
                            rdo.PRICE = expMedi.PRICE.Value * (1 + (expMedi.VAT_RATIO ?? 0)); 
                            rdo.VAT_RATIO = expMedi.VAT_RATIO ?? 0; 
                        }
                        else
                        {
                            rdo.PRICE = medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO); 
                            rdo.VAT_RATIO = medicine.IMP_VAT_RATIO; 
                        }
                        rdo.AMOUNT = medicine.AMOUNT; 
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT; 
                        ListMedicineRdo.Add(rdo); 

                    }
                    ListMedicineRdo = ListMedicineRdo.GroupBy(o => new { MEDICINE_TYPE_NAME = o.MEDICINE_TYPE_NAME, PRICE = o.PRICE })
                               .Select(p => new Mrs00273RDO
                    {
                        MEDICINE_TYPE_NAME = p.First().MEDICINE_TYPE_NAME,
                        NATIONAL_NAME = p.First().NATIONAL_NAME,
                        PRICE = p.First().PRICE,
                        AMOUNT = p.Sum(q => q.AMOUNT),
                        TOTAL_PRICE = p.First().PRICE * p.Sum(q => q.AMOUNT)
                    }
                    ).ToList(); 
                }
                if (IsNotNullOrEmpty(listMaterial))
                {
                    //Dictionary<long ,List<V_HIS_IMP_MEST_MEDICINE>> DicMedicine = new Dictionary<long ,List<V_HIS_IMP_MEST_MEDICINE>>(); 
                    foreach (var material in listMaterial)
                    {

                        Mrs00273RDO rdo = new Mrs00273RDO(); 
                        rdo.MATERIAL_TYPE_NAME = material.MATERIAL_TYPE_NAME; 
                        rdo.NATIONAL_NAME = material.NATIONAL_NAME; 
                        //rdo.PRICE = listExpMestMaterial
                        //           .Where(o => o.EXP_MEST_ID == listMobaImpMest
                        //                    .Where(p => p.IMP_MEST_ID == material.IMP_MEST_ID).First().EXP_MEST_ID && o.MATERIAL_ID == material.MATERIAL_ID).First().PRICE ?? material.IMP_PRICE; 
                        var expMate = listExpMestMaterial.FirstOrDefault(o => listMobaImpMest.Exists(p => p.ID == material.IMP_MEST_ID && p.MOBA_EXP_MEST_ID == o.EXP_MEST_ID) && o.MATERIAL_ID == material.MATERIAL_ID); 
                        if (expMate != null && expMate.PRICE.HasValue)
                        {
                            rdo.PRICE = expMate.PRICE.Value * (1 + (expMate.VAT_RATIO ?? 0)); 
                            rdo.VAT_RATIO = expMate.VAT_RATIO ?? 0; 
                        }
                        else
                        {
                            rdo.PRICE = material.IMP_PRICE * (1 + material.IMP_VAT_RATIO); 
                            rdo.VAT_RATIO = material.IMP_VAT_RATIO; 
                        }
                        rdo.AMOUNT = material.AMOUNT; 
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT; 
                        ListMaterialRdo.Add(rdo); 

                    }
                    ListMaterialRdo = ListMaterialRdo.GroupBy(o => new { MATERIAL_TYPE_NAME = o.MATERIAL_TYPE_NAME, PRICE = o.PRICE })
                             .Select(p => new Mrs00273RDO
                             {
                                 MATERIAL_TYPE_NAME = p.First().MATERIAL_TYPE_NAME,
                                 NATIONAL_NAME = p.First().NATIONAL_NAME,
                                 PRICE = p.First().PRICE,
                                 AMOUNT = p.Sum(q => q.AMOUNT),
                                 TOTAL_PRICE = p.First().PRICE * p.Sum(q => q.AMOUNT)
                             }
                    ).ToList(); 
                }

            }
            catch (Exception ex)
            {
                result = false; 
                LogSystem.Error(ex); 
            }
            return result; 
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            var acsUser = new AcsUserManager(paramGet).Get<List<ACS_USER>>(new AcsUserFilterQuery());
            if (((Mrs00273Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00273Filter)reportFilter).TIME_FROM)); 
            }
            if (IsNotNullOrEmpty(((Mrs00273Filter)reportFilter).LOGINNAME_SALE))
            {
                dicSingleTag.Add("SALE_USERNAME", acsUser.Where(o => o.LOGINNAME == ((Mrs00273Filter)reportFilter).LOGINNAME_SALE).ToList().First().USERNAME); 
            }
            if (((Mrs00273Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(((Mrs00273Filter)reportFilter).TIME_TO)); 
            }
            if (IsNotNullOrEmpty(((Mrs00273Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs))
            {
                dicSingleTag.Add("MEDI_STOCK_NAME", String.Join(", ", new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00273Filter)reportFilter).MEDI_STOCK_BUSINESS_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToArray())); 
            }

            objectTag.AddObjectData(store, "Medicine", ListMedicineRdo); 
            objectTag.AddObjectData(store, "Material", ListMaterialRdo);

            objectTag.AddObjectData(store, "MedicineDetail", listMedicine);
            objectTag.AddObjectData(store, "MaterialDetail", listMaterial);
            objectTag.AddObjectData(store, "Sale", listMobaImpMest.GroupBy(o => o.CREATOR).Select(p => new SALE() { CREATOR = p.Key, USERNAME = acsUser.Where(o => o.LOGINNAME == p.Key).ToList().First().USERNAME }).OrderBy(r=>r.USERNAME).ToList());
            objectTag.AddObjectData(store, "ImpMest", listMobaImpMest);
            objectTag.AddRelationship(store, "ImpMest", "MedicineDetail", "ID", "IMP_MEST_ID");
            objectTag.AddRelationship(store, "ImpMest", "MaterialDetail", "ID", "IMP_MEST_ID");
            objectTag.AddRelationship(store, "Sale", "ImpMest", "CREATOR", "CREATOR");
        }
    }
}
