using FlexCel.Report;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Common.Repository;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisBidMaterialType;
using MOS.MANAGER.HisBidMedicineType;

namespace MRS.Processor.Mrs00507
{
    class Mrs00507Processor : AbstractProcessor
    {
        Mrs00507Filter castFilter = null;
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineOnBid = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialOnBid = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestChemistryOnBid = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicineBuyYourSelf = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialBuyYourSelf = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestChemistryBuyYourSelf = new List<V_HIS_EXP_MEST_MATERIAL>();

        Dictionary<long, V_HIS_MEDICINE_TYPE> dicMedicineType = new Dictionary<long, V_HIS_MEDICINE_TYPE>();
        Dictionary<string,HIS_BID_MEDICINE_TYPE> dicBidMedicineType = new Dictionary<string,HIS_BID_MEDICINE_TYPE>();

        Dictionary<long, V_HIS_MATERIAL_TYPE> dicMaterialType = new Dictionary<long, V_HIS_MATERIAL_TYPE>();
        Dictionary<string,HIS_BID_MATERIAL_TYPE> dicBidMaterialType = new Dictionary<string,HIS_BID_MATERIAL_TYPE>();
        List<Mrs00507RDO> listMedicineOnBidRdo = new List<Mrs00507RDO>();
        List<Mrs00507RDO> listMaterialOnBidRdo = new List<Mrs00507RDO>();
        List<Mrs00507RDO> listChemistryOnBidRdo = new List<Mrs00507RDO>();
        List<Mrs00507RDO> listMedicineBuyYourSelfRdo = new List<Mrs00507RDO>();
        List<Mrs00507RDO> listMaterialBuyYourSelfRdo = new List<Mrs00507RDO>();
        List<Mrs00507RDO> listChemistryBuyYourSelfRdo = new List<Mrs00507RDO>();


        public Mrs00507Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }


        public override Type FilterType()
        {
            return typeof(Mrs00507Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                //Hoa chat vat tu
                this.castFilter = (Mrs00507Filter)this.reportFilter;
                if (((castFilter.IS_MATERIAL ?? false) == (castFilter.IS_MEDICINE ?? false) == (castFilter.IS_CHEMISTRY ?? false) == false)
                    || (castFilter.IS_MATERIAL ?? false) == true || (castFilter.IS_CHEMISTRY ?? false) == true)
                {
                    HisExpMestMaterialViewFilterQuery expMestMaterialfilter = new HisExpMestMaterialViewFilterQuery();
                    expMestMaterialfilter.EXP_TIME_FROM = this.castFilter.TIME_FROM;
                    expMestMaterialfilter.EXP_TIME_TO = this.castFilter.TIME_TO;
                    expMestMaterialfilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID;
                    expMestMaterialfilter.EXP_MEST_TYPE_IDs = this.castFilter.EXP_MEST_TYPE_IDs;
                    expMestMaterialfilter.IS_EXPORT = true;
                    var listExpMestMaterialAndChermistry = new HisExpMestMaterialManager(new CommonParam()).GetView(expMestMaterialfilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>();
                    dicMaterialType = (new HisMaterialTypeManager(new CommonParam()).GetView(new HisMaterialTypeViewFilterQuery()) ?? new List<V_HIS_MATERIAL_TYPE>()).ToDictionary(o => o.ID);
                    dicBidMaterialType = new HisBidMaterialTypeManager().Get(new HisBidMaterialTypeFilterQuery()).GroupBy(o => (o.MATERIAL_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());

                    if (!((castFilter.IS_MATERIAL ?? false) == true && (castFilter.IS_CHEMISTRY ?? false) == false))
                    {
                        listExpMestChemistryOnBid = listExpMestMaterialAndChermistry.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE&&dicBidMaterialType.ContainsKey(o.MATERIAL_TYPE_ID + "_" + (o.BID_ID??0))).ToList();
                        listExpMestChemistryBuyYourSelf = listExpMestMaterialAndChermistry.Where(o => o.IS_CHEMICAL_SUBSTANCE == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE&&(!dicBidMaterialType.ContainsKey(o.MATERIAL_TYPE_ID + "_" + (o.BID_ID??0)))).ToList();
                    }
                    if (!((castFilter.IS_MATERIAL ?? false) == false && (castFilter.IS_CHEMISTRY ?? false) == true))
                    {

                         listExpMestMaterialOnBid = listExpMestMaterialAndChermistry.Where(o => (o.IS_CHEMICAL_SUBSTANCE??0) != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE&&dicBidMaterialType.ContainsKey(o.MATERIAL_TYPE_ID + "_" + (o.BID_ID??0))).ToList();
                        listExpMestMaterialBuyYourSelf = listExpMestMaterialAndChermistry.Where(o => (o.IS_CHEMICAL_SUBSTANCE??0) != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE&&(!dicBidMaterialType.ContainsKey(o.MATERIAL_TYPE_ID + "_" + (o.BID_ID??0)))).ToList();
                    }

                    listMaterialOnBidRdo = (from r in listExpMestMaterialOnBid select new Mrs00507RDO(r, dicMaterialType, dicBidMaterialType)).ToList();

                    listChemistryOnBidRdo = (from r in listExpMestChemistryOnBid select new Mrs00507RDO(r, dicMaterialType, dicBidMaterialType)).ToList();

                    listMaterialBuyYourSelfRdo = (from r in listExpMestMaterialBuyYourSelf select new Mrs00507RDO(r, dicMaterialType)).ToList();

                    listChemistryBuyYourSelfRdo = (from r in listExpMestMaterialBuyYourSelf select new Mrs00507RDO(r, dicMaterialType)).ToList();
                }
                //thuoc
                if (((castFilter.IS_MATERIAL ?? false) == (castFilter.IS_MEDICINE ?? false) == (castFilter.IS_CHEMISTRY ?? false) == false)
                    || (castFilter.IS_MEDICINE?? false) == true)
                {
                    HisExpMestMedicineViewFilterQuery expMestMedicinefilter = new HisExpMestMedicineViewFilterQuery();
                    expMestMedicinefilter.EXP_TIME_FROM = this.castFilter.TIME_FROM;
                    expMestMedicinefilter.EXP_TIME_TO = this.castFilter.TIME_TO;
                    expMestMedicinefilter.MEDI_STOCK_ID = this.castFilter.MEDI_STOCK_ID;
                    expMestMedicinefilter.EXP_MEST_TYPE_IDs = this.castFilter.EXP_MEST_TYPE_IDs;
                    expMestMedicinefilter.IS_EXPORT = true;
                    var listExpMestMedicine = new HisExpMestMedicineManager(new CommonParam()).GetView(expMestMedicinefilter) ?? new List<V_HIS_EXP_MEST_MEDICINE>();

                    dicMedicineType = (new HisMedicineTypeManager(new CommonParam()).GetView(new HisMedicineTypeViewFilterQuery()) ?? new List<V_HIS_MEDICINE_TYPE>()).ToDictionary(o => o.ID);
                    dicBidMedicineType = new HisBidMedicineTypeManager().Get(new HisBidMedicineTypeFilterQuery()).GroupBy(o => (o.MEDICINE_TYPE_ID + "_" + o.BID_ID)).ToDictionary(p => p.Key, p => p.First());
                    listExpMestMedicineOnBid = listExpMestMedicine.Where(o => dicBidMedicineType.ContainsKey(o.MEDICINE_TYPE_ID + "_" + (o.BID_ID ?? 0))).ToList(); 
                    listExpMestMedicineBuyYourSelf = listExpMestMedicine.Where(o => dicBidMedicineType.ContainsKey(o.MEDICINE_TYPE_ID + "_" + (o.BID_ID ?? 0))).ToList();
                    listMedicineOnBidRdo = (from r in listExpMestMedicineOnBid select new Mrs00507RDO(r, dicMedicineType,dicBidMedicineType)).ToList();
                    listMedicineBuyYourSelfRdo = (from r in listExpMestMedicineBuyYourSelf select new Mrs00507RDO(r, dicMedicineType)).ToList();
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

                if (IsNotNullOrEmpty(listMedicineOnBidRdo))
                {
                    listMedicineOnBidRdo = GroupByServiceAndPrice(listMedicineOnBidRdo);
                }
                if (IsNotNullOrEmpty(listMaterialOnBidRdo))
                {
                    listMaterialOnBidRdo = GroupByServiceAndPrice(listMaterialOnBidRdo);
                }
                if (IsNotNullOrEmpty(listChemistryOnBidRdo))
                {
                    listChemistryOnBidRdo = GroupByServiceAndPrice(listChemistryOnBidRdo);
                }
                if (IsNotNullOrEmpty(listMedicineBuyYourSelfRdo))
                {
                    listMedicineBuyYourSelfRdo = GroupByServiceAndPrice(listMedicineBuyYourSelfRdo);
                }
                if (IsNotNullOrEmpty(listMaterialBuyYourSelfRdo))
                {
                    listMaterialBuyYourSelfRdo = GroupByServiceAndPrice(listMaterialBuyYourSelfRdo);
                }
                if (IsNotNullOrEmpty(listChemistryBuyYourSelfRdo))
                {
                    listChemistryBuyYourSelfRdo = GroupByServiceAndPrice(listChemistryBuyYourSelfRdo);
                }

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private List<Mrs00507RDO> GroupByServiceAndPrice(List<Mrs00507RDO> listRdo)
        {
            string errorField = "";
            List<Mrs00507RDO> result = new List<Mrs00507RDO>();
            try
            {
                var group = listRdo.GroupBy(o => new { o.SERVICE_NAME,o.PRICE}).ToList();
                Decimal sum = 0;
                Mrs00507RDO rdo;
                List<Mrs00507RDO> listSub;
                PropertyInfo[] pi = Properties.Get<Mrs00507RDO>();
                foreach (var item in group)
                {
                    rdo = new Mrs00507RDO();
                    listSub = item.ToList<Mrs00507RDO>();

                    bool hide = true;
                    foreach (var field in pi)
                    {
                        errorField = field.Name;
                        if (field.Name.Contains("AMOUNT") || field.Name.Contains("TOTAL"))
                        {
                            sum = listSub.Sum(s => (Decimal)field.GetValue(s));
                            if (hide && sum != 0) hide = false;
                            field.SetValue(rdo, sum);
                        }
                        else
                        {
                            field.SetValue(rdo, field.GetValue(IsMeaningful(listSub, field)));
                        }
                    }
                    if (!hide) result.Add(rdo);
                }
            }
            catch (Exception ex)
            {
                LogSystem.Info(errorField);
                Inventec.Common.Logging.LogSystem.Error(ex);
                return new List<Mrs00507RDO>();
            }
            return result;
        }

        private Mrs00507RDO IsMeaningful(List<Mrs00507RDO> listSub, PropertyInfo field)
        {
            return listSub.Where(o => field.GetValue(o) != null && field.GetValue(o).ToString() != "").FirstOrDefault() ?? new Mrs00507RDO();
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));

                objectTag.AddObjectData(store, "ReportMedicineOnBid", listMedicineOnBidRdo);
                objectTag.AddObjectData(store, "ReportMaterialOnBid", listMaterialOnBidRdo);
                objectTag.AddObjectData(store, "ReportChemistryOnBid", listChemistryOnBidRdo);
                objectTag.AddObjectData(store, "ReportMedicineBuyYourSelf", listMedicineBuyYourSelfRdo);
                objectTag.AddObjectData(store, "ReportMaterialBuyYourSelf", listMaterialBuyYourSelfRdo);
                objectTag.AddObjectData(store, "ReportChemistryBuyYourSelf", listChemistryBuyYourSelfRdo);
           
        }
       
    }
    /*select mate.id,(select bid_num_order from his_bid_material_type bm where bm.id = mate.bid_id and bm.Material_type_id = mate.Material_type_id) as bid_num_order, mate.material_type_name,MATE.SERVICE_UNIT_NAME,MATE.MANUFACTURER_NAME,mtt.PACKING_type_NAME from v_his_exp_mest_material mate
 join v_his_material_type mtt on mtt.id = MATE.MATERIAL_TYPE_ID order by mate.id;
 */
}
