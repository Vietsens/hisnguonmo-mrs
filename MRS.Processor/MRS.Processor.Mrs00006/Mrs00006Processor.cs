using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisMediStock;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00006
{
    public class Mrs00006Processor : AbstractProcessor
    {
        Mrs00006Filter castFilter = null;
        const int REPORT_DAY_COUNT = 14;  //15 ngay
        List<Mrs00006RDO> listRdo = new List<Mrs00006RDO>();
        List<HIS_MEDICINE_TYPE> ListMedicineType = new List<HIS_MEDICINE_TYPE>();
        List<HIS_MATERIAL_TYPE> LisMaterialType = new List<HIS_MATERIAL_TYPE>();

        Dictionary<long, Mrs00006RDO> dicRdoMedicine = new Dictionary<long, Mrs00006RDO>();
        Dictionary<long, Mrs00006RDO> dicRdoMaterial = new Dictionary<long, Mrs00006RDO>();
    
        List<long> MobaImpMestTypeIds = new List<long>() 
        {
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DMTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DTTTL,
        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH
        };
        public Mrs00006Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00006Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                castFilter = ((Mrs00006Filter)this.reportFilter);
                GetMedicine();
                GetMaterial();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void GetMedicine()
        {
            CommonParam getParam = new CommonParam();
            HisExpMestMedicineViewFilterQuery expMedicineFilter = new HisExpMestMedicineViewFilterQuery();
            
            expMedicineFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID ;

            expMedicineFilter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;

            expMedicineFilter.IS_EXPORT = true;
            expMedicineFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            System.DateTime? expFromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
            if (expFromTime.HasValue)
            {
                expMedicineFilter.EXP_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(expFromTime.Value.AddDays(REPORT_DAY_COUNT));
            }
            else
            {
                expMedicineFilter.EXP_TIME_TO = -1;  //Voi muc dich de khong get ra duoc du lieu nao
            }
            expMedicineFilter.ORDER_FIELD = "EXP_TIME";
            expMedicineFilter.ORDER_DIRECTION = "ACS";
            var listExpMedicine = new HisExpMestMedicineManager(getParam).GetView(expMedicineFilter);
          //  var listExpMedicine = ListMedicineType.GroupBy(o => o.ID).ToList();
            
            if (listExpMedicine != null)
            {

             
                foreach (var item in listExpMedicine)
                    
                {
                    var medicineType = ListMedicineType.Where(w => w.ID == item.MEDICINE_TYPE_ID ).ToList();
                  
                    Mrs00006RDO temp = null;
                    if (dicRdoMedicine.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        temp = dicRdoMedicine[item.MEDICINE_TYPE_ID];
                    }
                  
                    else
                    {
                        temp = new Mrs00006RDO(item.MEDICINE_TYPE_NAME, item.SERVICE_UNIT_NAME);
                       
                        if (IsNotNullOrEmpty(medicineType))
                        {
                        temp.PACKING_TYPE_NAME = medicineType.First().PACKING_TYPE_NAME;
                           
                        }
                        temp.GenerateDate(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM));
                        dicRdoMedicine[item.MEDICINE_TYPE_ID] = temp;
                    }
                    if (!temp.Calculate(item.EXP_TIME, item.AMOUNT))
                    {

                    }
                }
            }

            HisImpMestMedicineViewFilterQuery impMedicineFilter = new HisImpMestMedicineViewFilterQuery();
            impMedicineFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            
            //impMedicineFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH;
            impMedicineFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            impMedicineFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            System.DateTime? impFromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
            if (impFromTime.HasValue)
            {
                impMedicineFilter.IMP_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(impFromTime.Value.AddDays(REPORT_DAY_COUNT));
            }
            else
            {
                impMedicineFilter.IMP_TIME_TO = -1;  //Voi muc dich de khong get ra duoc du lieu nao
            }
            impMedicineFilter.ORDER_FIELD = "IMP_TIME";
            impMedicineFilter.ORDER_DIRECTION = "ACS";

            


            var listImpMedicine = new HisImpMestMedicineManager(getParam).GetView(impMedicineFilter);
            //////

            if (castFilter.DEPARTMENT_ID != null)
            {
                listImpMedicine = listImpMedicine.Where(o => o.REQ_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
            }


            if (listImpMedicine != null)
            {
                listImpMedicine = listImpMedicine.Where(o => this.MobaImpMestTypeIds.Contains(o.IMP_MEST_TYPE_ID)).ToList();
            }
            if (listImpMedicine != null)
            {
                foreach (var item in listImpMedicine)
                {
                    Mrs00006RDO temp = null;
                    if (dicRdoMedicine.ContainsKey(item.MEDICINE_TYPE_ID))
                    {
                        temp = dicRdoMedicine[item.MEDICINE_TYPE_ID];
                    }
                    else
                    {
                        temp = new Mrs00006RDO(item.MEDICINE_TYPE_NAME, item.SERVICE_UNIT_NAME);
                        temp = new Mrs00006RDO(item.PACKING_TYPE_NAME, item.PACKING_TYPE_NAME);

                        temp.GenerateDate(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM));
                        dicRdoMedicine[item.MEDICINE_TYPE_ID] = temp;
                    }
                    if (!temp.Calculate(item.IMP_TIME, -item.AMOUNT))
                    {
                            
                    }
                }
            }
        }

        private void GetMaterial()
        {
            CommonParam getParam = new CommonParam();
            HisExpMestMaterialViewFilterQuery expMaterialFilter = new HisExpMestMaterialViewFilterQuery();
            expMaterialFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;

            expMaterialFilter.REQ_DEPARTMENT_ID = castFilter.DEPARTMENT_ID;

            expMaterialFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
            //expMaterialFilter.IN_EXECUTE = true; 
            expMaterialFilter.EXP_TIME_FROM = castFilter.TIME_FROM;
            expMaterialFilter.IS_EXPORT = true; 
            System.DateTime? expFromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
            if (expFromTime.HasValue)
            {
                expMaterialFilter.EXP_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(expFromTime.Value.AddDays(REPORT_DAY_COUNT));
            }
            else
            {
                expMaterialFilter.EXP_TIME_TO = -1;  //Voi muc dich de khong get ra duoc du lieu nao
            }
            expMaterialFilter.ORDER_FIELD = "EXP_TIME";
            expMaterialFilter.ORDER_DIRECTION = "ACS";
            var listExpMaterial = new HisExpMestMaterialManager(getParam).GetView(expMaterialFilter);

            if (listExpMaterial != null)
            {
                foreach (var item in listExpMaterial)
                {
                    var materialType = LisMaterialType.Where(w => w.ID == item.MATERIAL_TYPE_ID).ToList();
                    Mrs00006RDO temp = null;
                    if (dicRdoMaterial.ContainsKey(item.MATERIAL_TYPE_ID))
                    {
                        temp = dicRdoMaterial[item.MATERIAL_TYPE_ID];
                    }
                    else
                    {
                        temp = new Mrs00006RDO(item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME);
                      
                        if (IsNotNullOrEmpty(materialType))
                        {
                            temp.PACKING_TYPE_NAME = materialType.First().PACKING_TYPE_NAME;

                        }
                        temp.GenerateDate(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM));
                        dicRdoMaterial[item.MATERIAL_TYPE_ID] = temp;
                    }
                    if (!temp.Calculate(item.EXP_TIME, item.AMOUNT))
                    {

                    }
                }
            }

            HisImpMestMaterialViewFilterQuery impMaterialFilter = new HisImpMestMaterialViewFilterQuery();
            impMaterialFilter.MEDI_STOCK_ID = castFilter.MEDI_STOCK_ID;
            
            impMaterialFilter.IMP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH;
            impMaterialFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
            impMaterialFilter.IMP_TIME_FROM = castFilter.TIME_FROM;
            System.DateTime? impFromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
            if (impFromTime.HasValue)
            {
                impMaterialFilter.IMP_TIME_TO = Inventec.Common.DateTime.Convert.SystemDateTimeToTimeNumber(impFromTime.Value.AddDays(REPORT_DAY_COUNT));
            }
            else
            {
                impMaterialFilter.IMP_TIME_TO = -1;  //Voi muc dich de khong get ra duoc du lieu nao
            }
            impMaterialFilter.ORDER_FIELD = "IMP_TIME";
            impMaterialFilter.ORDER_DIRECTION = "ACS";
            var listImpMaterial = new HisImpMestMaterialManager(getParam).GetView(impMaterialFilter);

            if (castFilter.DEPARTMENT_ID != null)
            {
                listImpMaterial = listImpMaterial.Where(o => o.REQ_DEPARTMENT_ID == castFilter.DEPARTMENT_ID).ToList();
            }

            foreach (var item in listImpMaterial)
            {
                Mrs00006RDO temp = null;
                if (dicRdoMaterial.ContainsKey(item.MATERIAL_TYPE_ID))
                {
                    temp = dicRdoMaterial[item.MATERIAL_TYPE_ID];
                }
                else
                {
                    temp = new Mrs00006RDO(item.MATERIAL_TYPE_NAME, item.SERVICE_UNIT_NAME);
                    temp = new Mrs00006RDO(item.PACKING_TYPE_NAME, item.PACKING_TYPE_NAME);
                    temp.GenerateDate(Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM));
                    dicRdoMaterial[item.MATERIAL_TYPE_ID] = temp;
                }
                if (!temp.Calculate(item.IMP_TIME, -item.AMOUNT))
                {

                }
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                listRdo.AddRange(dicRdoMedicine.Values);
                listRdo.AddRange(dicRdoMaterial.Values);
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
                var resultRoom = new HisMediStockManager().GetById(castFilter.MEDI_STOCK_ID ??0);
                 resultRoom = new HisMediStockManager().GetById(castFilter.DEPARTMENT_ID ??0  );
                HIS_MEDI_STOCK stock = resultRoom != null ? resultRoom : null;
                if (stock != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", stock.MEDI_STOCK_NAME);
                }
                System.DateTime? fromTime = Inventec.Common.DateTime.Convert.TimeNumberToSystemDateTime(castFilter.TIME_FROM);
                if (fromTime.HasValue)
                {
                    dicSingleTag.Add("EXP_DATE_FROM_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value));
                    dicSingleTag.Add("EXP_DATE_TO_STR", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(REPORT_DAY_COUNT)));

                    dicSingleTag.Add("DATE1", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value));
                    dicSingleTag.Add("DATE2", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(1)));
                    dicSingleTag.Add("DATE3", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(2)));
                    dicSingleTag.Add("DATE4", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(3)));
                    dicSingleTag.Add("DATE5", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(4)));
                    dicSingleTag.Add("DATE6", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(5)));
                    dicSingleTag.Add("DATE7", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(6)));
                    dicSingleTag.Add("DATE8", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(7)));
                    dicSingleTag.Add("DATE9", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(8)));
                    dicSingleTag.Add("DATE10", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(9)));
                    dicSingleTag.Add("DATE11", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(10)));
                    dicSingleTag.Add("DATE12", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(11)));
                    dicSingleTag.Add("DATE13", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(12)));
                    dicSingleTag.Add("DATE14", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(13)));
                    dicSingleTag.Add("DATE15", Inventec.Common.DateTime.Convert.SystemDateTimeToDateString(fromTime.Value.AddDays(14)));
                }
                objectTag.AddObjectData(store, "Report", listRdo);
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
