using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisMaterial;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicine;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisTransaction;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00816
{
    public class Mrs00816Processor : AbstractProcessor
    {
        public Mrs00816Filter filter;
        public List<Mrs00816RDO> listRdo = new List<Mrs00816RDO>();
        public List<HIS_EXP_MEST> listExpMest = new List<HIS_EXP_MEST>();
        public List<HIS_TRANSACTION> listTransaction = new List<HIS_TRANSACTION>();
        public List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        public List<V_HIS_EXP_MEST_MATERIAL> listExpMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        public List<HIS_MEDICINE> listMedicine = new List<HIS_MEDICINE>();
        public List<HIS_MATERIAL> listMaterial = new List<HIS_MATERIAL>();
        public Mrs00816Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00816Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                filter = (Mrs00816Filter)this.reportFilter;
                var skip = 0;
                List<long> exp_mest_type_ids = new List<long>(){
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK,
                IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT,
                };
                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                expMestFilter.FINISH_TIME_FROM = filter.TIME_FROM;
                expMestFilter.FINISH_TIME_TO = filter.TIME_TO;
                expMestFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                expMestFilter.MEDI_STOCK_ID = filter.MEDI_STOCK_BUSINESS_ID;
                if (filter.IS_CASHER == true)
                {
                    listExpMest = new HisExpMestManager().Get(expMestFilter);
                    listExpMest = listExpMest.Where(x => x.TDL_TREATMENT_ID == null).ToList();
                }
                else
                {
                    expMestFilter.EXP_MEST_TYPE_IDs = exp_mest_type_ids;
                    listExpMest = new HisExpMestManager().Get(expMestFilter);
                    listExpMest = listExpMest.Where(x => x.TDL_TREATMENT_ID != null).ToList();
                }
                var expMestIds = listExpMest.Select(x => x.ID).ToList();
                while (expMestIds.Count - skip > 0)
                {
                    var listIds = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMedicineViewFilterQuery expMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                    expMedicineFilter.EXP_MEST_IDs = listIds;
                    expMedicineFilter.IS_EXPORT = true;
                    var lstExpMedicine = new HisExpMestMedicineManager().GetView(expMedicineFilter);
                    listExpMestMedicine.AddRange(lstExpMedicine);
                }
                skip = 0;
                while (expMestIds.Count - skip > 0)
                {
                    var listIds = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestMaterialViewFilterQuery expMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                    expMaterialFilter.EXP_MEST_IDs = listIds;
                    expMaterialFilter.IS_EXPORT = true;
                    var lstExpMaterial = new HisExpMestMaterialManager().GetView(expMaterialFilter);
                    listExpMaterial.AddRange(lstExpMaterial);
                }

                //get lô thuốc vật tư
                //GetMediMate();

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();

                if (listExpMestMedicine != null)
                {
                    medicineIds.AddRange(listExpMestMedicine.Select(o => o.MEDICINE_ID??0).ToList());
                }


                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineFilterQuery Medicinefilter = new HisMedicineFilterQuery();
                        Medicinefilter.IDs = limit;

                        var MedicineSub = new HisMedicineManager().Get(Medicinefilter);
                        listMedicine.AddRange(MedicineSub);
                    }
                }
             

                List<long> materialIds = new List<long>();

                if (listExpMaterial != null)
                {
                    materialIds.AddRange(listExpMaterial.Select(o => o.MATERIAL_ID??0).ToList());
                }
                
                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialFilterQuery Materialfilter = new HisMaterialFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().Get(Materialfilter);
                        listMaterial.AddRange(MaterialSub);
                    }
                }
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

            }
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    foreach (var item in listExpMestMedicine)
                    {
                        Mrs00816RDO rdo = new Mrs00816RDO();
                        //var medicine = listMedicine.Where(x => x.ID == item.MEDICINE_ID).FirstOrDefault();
                        rdo.EXP_TIME = item.EXP_TIME ?? 0;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXP_TIME ?? 0);
                        rdo.AMOUNT = item.AMOUNT;
                        //if (item.VIR_PRICE == 0 || item.VIR_PRICE == null)
                        //{
                        //    if (medicine != null)
                        //    {
                        //        rdo.PRICE = medicine.IMP_PRICE;
                        //    }
                        //}
                        //else
                        //{
                        //    rdo.PRICE = item.VIR_PRICE ?? 0;
                        //}
                        rdo.PRICE = (item.PRICE??0)*(1+(item.VAT_RATIO??0));
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        listRdo.Add(rdo);
                    }
                }
                if (IsNotNullOrEmpty(listExpMaterial))
                {
                    foreach (var item in listExpMaterial)
                    {
                        Mrs00816RDO rdo = new Mrs00816RDO();
                        //var material = listMaterial.Where(x => x.ID == item.MATERIAL_ID).FirstOrDefault();
                        rdo.EXP_TIME = item.EXP_TIME ?? 0;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.EXP_TIME ?? 0);
                        rdo.AMOUNT = item.AMOUNT;
                        //if (item.VIR_PRICE == 0 || item.VIR_PRICE == null)
                        //{
                        //    if (material != null)
                        //    {
                        //        rdo.PRICE = material.IMP_PRICE;
                        //    }
                        //}
                        //else
                        //{
                        //    rdo.PRICE = item.VIR_PRICE ?? 0;
                        //}
                        rdo.PRICE = (item.PRICE ?? 0) * (1 + (item.VAT_RATIO ?? 0));
                        rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                        listRdo.Add(rdo);
                    }
                }
                var group = listRdo.GroupBy(x => x.EXP_TIME_STR).ToList();
                listRdo.Clear();
                foreach (var item in group)
                {
                    Mrs00816RDO rdo = new Mrs00816RDO();
                    rdo.EXP_TIME_STR = item.First().EXP_TIME_STR;
                    rdo.TOTAL_PRICE = item.Sum(x => x.TOTAL_PRICE);
                    listRdo.Add(rdo);
                }
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_FROM));
            dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(filter.TIME_TO));
            objectTag.AddObjectData(store, "Report", listRdo);
        }
    }
}
