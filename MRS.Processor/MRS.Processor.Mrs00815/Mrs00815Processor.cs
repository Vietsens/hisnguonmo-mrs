using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisDepartment;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisMaterialBean;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisMedicineBean;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisRoom;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00815
{
    public class Mrs00815Processor : AbstractProcessor
    {
        public Mrs00815Filter filter;
        public List<V_HIS_MATERIAL_BEAN> listMaterialBean = new List<V_HIS_MATERIAL_BEAN>();
        public List<V_HIS_MATERIAL_BEAN> listChemicalBean = new List<V_HIS_MATERIAL_BEAN>();
        public List<V_HIS_MEDICINE_BEAN> listMedicineBean = new List<V_HIS_MEDICINE_BEAN>();
        public List<V_HIS_MEDICINE_TYPE> listMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        public List<V_HIS_MATERIAL_TYPE> listMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        public List<HIS_MEDI_STOCK> listMediStock = new List<HIS_MEDI_STOCK>();
        public List<HIS_DEPARTMENT> listDepartment = new List<HIS_DEPARTMENT>();
        public List<HIS_ROOM> listRoom = new List<HIS_ROOM>();
        public List<Mrs00815RDO> listRdo = new List<Mrs00815RDO>();
        Title Title = new Title();
        AMOUNT Amount = new AMOUNT();
        CommonParam paramGet = new CommonParam();
        const int MAX_EXAM_SERVICE_TYPE_NUM = 50;
        public Mrs00815Processor(CommonParam param,string reportTypeName):base(param,reportTypeName)
        {

        }
        public override Type FilterType()
        {
            return typeof(Mrs00815Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            filter = (Mrs00815Filter)this.reportFilter;
            try
            {
                //listDepartment = new HisDepartmentManager().Get(new HisDepartmentFilterQuery());
                //listRoom = new HisRoomManager().Get(new HisRoomFilterQuery());
                listMediStock = new HisMediStockManager().Get(new HisMediStockFilterQuery());
                listMediStock = listMediStock.Where(x => x.IS_CABINET == 1).ToList();
                var mediStockIds = listMediStock.Select(x => x.ID).ToList();
                listMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());

                listMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                if (filter.IS_MATERIAL == true || filter.IS_CHEMICAL_SUBSTANCE == true)
                {
                    HisMaterialBeanViewFilterQuery materialBeanFilter = new HisMaterialBeanViewFilterQuery();
                    materialBeanFilter.MEDI_STOCK_IDs = mediStockIds;
                    //if (filter.TIME_FROM != null)
                    //{
                    //    materialBeanFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    //}
                    if (filter.TIME_TO != null)
                    {
                        materialBeanFilter.CREATE_TIME_TO = filter.TIME_TO;
                    }
                    listMaterialBean = new HisMaterialBeanManager().GetView(materialBeanFilter);
                    
                    GetMaterial();
                }
                if (filter.IS_MEDICINE == true)
                {
                    HisMedicineBeanViewFilterQuery medicineBeanFilter = new HisMedicineBeanViewFilterQuery();
                    medicineBeanFilter.MEDI_STOCK_IDs = mediStockIds;
                    //if (filter.TIME_FROM != null)
                    //{
                    //    medicineBeanFilter.CREATE_TIME_FROM = filter.TIME_FROM;
                    //}
                    if (filter.TIME_TO != null)
                    {
                        medicineBeanFilter.CREATE_TIME_TO = filter.TIME_TO;
                    }
                    listMedicineBean = new HisMedicineBeanManager().GetView(medicineBeanFilter);
                    GetMedicine();
                }
                //Group(listRdo);
                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex.Message);
                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                int count = 1;
                foreach (var room in listMediStock)
                {
                    if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                    System.Reflection.PropertyInfo piRoom = typeof(Title).GetProperty("MEDI_STOCK_NAME_" + count);
                    piRoom.SetValue(Title, room.MEDI_STOCK_NAME);
                    count++;
                }
                var list = listRdo;
                foreach (var item in list)
                {
                    count = 1;
                    foreach (var room in listMediStock)
                    {
                        if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                        System.Reflection.PropertyInfo piCRoom = typeof(Mrs00815RDO).GetProperty("A_MEDI_STOCK_" + count);
                        if (item.MEDI_STOCK_ID==room.ID) piCRoom.SetValue(item, item.TOTAL_AMOUNT);
                        count++;
                    }
                }
                var group = listRdo.GroupBy(x => new { x.MATE_MEDI_TYPE_ID, x.TYPE,x.MATE_MEDI_PARENT_NAME }).ToList();
                listRdo.Clear();
                foreach (var item in group)
                {
                    Mrs00815RDO rdo = new Mrs00815RDO();
                    rdo.MATE_MEDI_PARENT_NAME = item.First().MATE_MEDI_PARENT_NAME;
                   rdo.MATE_MEDI_BEAN_ID = item.First().MATE_MEDI_BEAN_ID;
                        rdo.MATE_MEDI_TYPE_ID = item.First().MATE_MEDI_TYPE_ID;
                        rdo.MATE_MEDI_TYPE_CODE = item.First().MATE_MEDI_TYPE_CODE;
                        rdo.MATE_MEDI_TYPE_NAME = item.First().MATE_MEDI_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = item.First().IMP_PRICE;
                        rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                        rdo.MEDI_STOCK_ID = item.First().MEDI_STOCK_ID;
                        rdo.MEDI_STOCK_NAME = item.First().MEDI_STOCK_NAME;
                        rdo.TOTAL_AMOUNT = item.Sum(x=>x.TOTAL_AMOUNT);
                        rdo.NUM_ORDER = item.First().NUM_ORDER;
                        rdo.EXPIRED_DATE = item.First().EXPIRED_DATE;
                        rdo.EXPIRED_DATE_STR = item.First().EXPIRED_DATE_STR;
                        rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                        rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                        rdo.MATE_MEDI_PARENT_NAME = item.First().MATE_MEDI_PARENT_NAME;
                        rdo.TYPE = item.First().TYPE;
                        rdo.TYPE_NAME = item.First().TYPE_NAME;
                        rdo.A_MEDI_STOCK_1 = item.Sum(x => x.A_MEDI_STOCK_1);
                        rdo.A_MEDI_STOCK_2 = item.Sum(x => x.A_MEDI_STOCK_2);
                        rdo.A_MEDI_STOCK_3 = item.Sum(x => x.A_MEDI_STOCK_3);
                        rdo.A_MEDI_STOCK_4 = item.Sum(x => x.A_MEDI_STOCK_4);
                        rdo.A_MEDI_STOCK_5 = item.Sum(x => x.A_MEDI_STOCK_5);
                        rdo.A_MEDI_STOCK_6 = item.Sum(x => x.A_MEDI_STOCK_6);
                        rdo.A_MEDI_STOCK_7 = item.Sum(x => x.A_MEDI_STOCK_7);
                        rdo.A_MEDI_STOCK_8 = item.Sum(x => x.A_MEDI_STOCK_8);
                        rdo.A_MEDI_STOCK_9 = item.Sum(x => x.A_MEDI_STOCK_9);
                        rdo.A_MEDI_STOCK_10 = item.Sum(x => x.A_MEDI_STOCK_10);
                        rdo.A_MEDI_STOCK_11 = item.Sum(x => x.A_MEDI_STOCK_11);
                        rdo.A_MEDI_STOCK_12 = item.Sum(x => x.A_MEDI_STOCK_12);
                        rdo.A_MEDI_STOCK_13 = item.Sum(x => x.A_MEDI_STOCK_13);
                        rdo.A_MEDI_STOCK_14 = item.Sum(x => x.A_MEDI_STOCK_14);
                        rdo.A_MEDI_STOCK_15 = item.Sum(x => x.A_MEDI_STOCK_15);
                        rdo.A_MEDI_STOCK_16 = item.Sum(x => x.A_MEDI_STOCK_16);
                        rdo.A_MEDI_STOCK_17 = item.Sum(x => x.A_MEDI_STOCK_17);
                        rdo.A_MEDI_STOCK_18 = item.Sum(x => x.A_MEDI_STOCK_18);
                        rdo.A_MEDI_STOCK_19 = item.Sum(x => x.A_MEDI_STOCK_19);
                        rdo.A_MEDI_STOCK_20 = item.Sum(x => x.A_MEDI_STOCK_20);
                        rdo.A_MEDI_STOCK_21 = item.Sum(x => x.A_MEDI_STOCK_21);
                        rdo.A_MEDI_STOCK_22 = item.Sum(x => x.A_MEDI_STOCK_22);
                        rdo.A_MEDI_STOCK_23 = item.Sum(x => x.A_MEDI_STOCK_23);
                        rdo.A_MEDI_STOCK_24 = item.Sum(x => x.A_MEDI_STOCK_24);
                        rdo.A_MEDI_STOCK_25 = item.Sum(x => x.A_MEDI_STOCK_25);
                        rdo.A_MEDI_STOCK_26= item.Sum(x => x.A_MEDI_STOCK_26);
                        rdo.A_MEDI_STOCK_27= item.Sum(x => x.A_MEDI_STOCK_27);
                        rdo.A_MEDI_STOCK_28= item.Sum(x => x.A_MEDI_STOCK_28);
                        rdo.A_MEDI_STOCK_29= item.Sum(x => x.A_MEDI_STOCK_29);
                        rdo.A_MEDI_STOCK_30= item.Sum(x => x.A_MEDI_STOCK_30);
                        rdo.A_MEDI_STOCK_31= item.Sum(x => x.A_MEDI_STOCK_31);
                        rdo.A_MEDI_STOCK_32= item.Sum(x => x.A_MEDI_STOCK_32);
                        rdo.A_MEDI_STOCK_33= item.Sum(x => x.A_MEDI_STOCK_33);
                        rdo.A_MEDI_STOCK_34= item.Sum(x => x.A_MEDI_STOCK_34);
                        rdo.A_MEDI_STOCK_35= item.Sum(x => x.A_MEDI_STOCK_35);
                        rdo.A_MEDI_STOCK_36= item.Sum(x => x.A_MEDI_STOCK_36);
                        rdo.A_MEDI_STOCK_37= item.Sum(x => x.A_MEDI_STOCK_37);
                        rdo.A_MEDI_STOCK_38= item.Sum(x => x.A_MEDI_STOCK_38);
                        rdo.A_MEDI_STOCK_39= item.Sum(x => x.A_MEDI_STOCK_39);
                        rdo.A_MEDI_STOCK_40= item.Sum(x => x.A_MEDI_STOCK_40);
                        rdo.A_MEDI_STOCK_41= item.Sum(x => x.A_MEDI_STOCK_41);
                        rdo.A_MEDI_STOCK_42= item.Sum(x => x.A_MEDI_STOCK_42);
                        rdo.A_MEDI_STOCK_43= item.Sum(x => x.A_MEDI_STOCK_43);
                        rdo.A_MEDI_STOCK_44= item.Sum(x => x.A_MEDI_STOCK_44);
                        rdo.A_MEDI_STOCK_45= item.Sum(x => x.A_MEDI_STOCK_45);
                        rdo.A_MEDI_STOCK_46= item.Sum(x => x.A_MEDI_STOCK_46);
                        rdo.A_MEDI_STOCK_47= item.Sum(x => x.A_MEDI_STOCK_47);
                        rdo.A_MEDI_STOCK_48= item.Sum(x => x.A_MEDI_STOCK_48);
                        rdo.A_MEDI_STOCK_49= item.Sum(x => x.A_MEDI_STOCK_49);
                        rdo.A_MEDI_STOCK_50= item.Sum(x => x.A_MEDI_STOCK_50);
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
        
        public void GetMedicine()
        {
            if (IsNotNullOrEmpty(listMedicineBean))
            {
                foreach (var item in listMedicineBean)
                {
                    Mrs00815RDO rdo = new Mrs00815RDO();
                    var medicine = listMedicineType.Where(x => x.ID == item.MEDICINE_TYPE_ID).FirstOrDefault();
                  
                        rdo.MATE_MEDI_BEAN_ID = item.MEDICINE_ID;
                        rdo.MATE_MEDI_TYPE_ID = item.MEDICINE_TYPE_ID;
                        rdo.MATE_MEDI_TYPE_CODE = item.MEDICINE_TYPE_CODE;
                        rdo.MATE_MEDI_TYPE_NAME = item.MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                        rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
                        rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                        rdo.TOTAL_AMOUNT = item.AMOUNT;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        if (item.EXPIRED_DATE.HasValue)
                        {
                            rdo.EXPIRED_DATE = Convert.ToInt64(item.EXPIRED_DATE.Value.ToString().Substring(0, 8));
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Convert.ToInt64(item.EXPIRED_DATE.Value));
                        }
                        var MediParent = listMedicineType.Where(x => x.ID == medicine.PARENT_ID).FirstOrDefault();
                        if (MediParent!=null)
                        {
                            rdo.MATE_MEDI_PARENT_NAME = MediParent.MEDICINE_TYPE_NAME;
                        }
                        else
                        {
                            rdo.MATE_MEDI_PARENT_NAME = "Không phân loại";
                        }
                    rdo.ACTIVE_INGR_BHYT_NAME = item.ACTIVE_INGR_BHYT_NAME;
                    rdo.ACTIVE_INGR_BHYT_CODE = item.ACTIVE_INGR_BHYT_CODE;
                    rdo.TYPE = 1;
                    rdo.TYPE_NAME = "Thuốc";
                    listRdo.Add(rdo);
                }
            }
        }
        public void GetMaterial()
        {
            List<V_HIS_MATERIAL_TYPE> materialType = new List<V_HIS_MATERIAL_TYPE>();
            //if (filter.IS_MATERIAL==true&&filter.IS_CHEMICAL_SUBSTANCE!=true)
            //{
            //    materialType = listMaterialType.Where(x => x.IS_CHEMICAL_SUBSTANCE == null).ToList();
            //}
            //if (filter.IS_CHEMICAL_SUBSTANCE==true&&filter.IS_MATERIAL!=true)
            //{
            //    materialType = listMaterialType.Where(x => x.IS_CHEMICAL_SUBSTANCE == 1).ToList();
            //}
            //if (filter.IS_MATERIAL==true&&filter.IS_CHEMICAL_SUBSTANCE==true)
            //{
            //    materialType = listMaterialType;
            //}
            if (IsNotNullOrEmpty(listMaterialBean))
            {
                foreach (var item in listMaterialBean)
                {
                    Mrs00815RDO rdo = new Mrs00815RDO();
                    var material = listMaterialType.Where(x => item.MATERIAL_TYPE_ID == x.ID).FirstOrDefault();
                        rdo.MATE_MEDI_BEAN_ID = item.MATERIAL_ID;
                        rdo.MATE_MEDI_TYPE_ID = item.MATERIAL_TYPE_ID;
                        rdo.MATE_MEDI_TYPE_CODE = item.MATERIAL_TYPE_CODE;
                        rdo.MATE_MEDI_TYPE_NAME = item.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.SERVICE_UNIT_NAME;
                        rdo.IMP_PRICE = item.IMP_PRICE;
                        rdo.PACKAGE_NUMBER = item.PACKAGE_NUMBER;
                        rdo.MEDI_STOCK_ID = item.MEDI_STOCK_ID;
                        rdo.MEDI_STOCK_NAME = item.MEDI_STOCK_NAME;
                        rdo.TOTAL_AMOUNT = item.AMOUNT;
                        rdo.NUM_ORDER = item.NUM_ORDER;
                        var materParent = listMaterialType.Where(x => x.ID == material.PARENT_ID).FirstOrDefault();
                        if (materParent != null)
                        {
                            rdo.MATE_MEDI_PARENT_NAME = materParent.MATERIAL_TYPE_NAME;
                        }
                        else
                        {
                            rdo.MATE_MEDI_PARENT_NAME = "Không phân loại";
                        }
                        if (item.EXPIRED_DATE.HasValue)
                        {
                            rdo.EXPIRED_DATE = Convert.ToInt64(item.EXPIRED_DATE.Value.ToString().Substring(0, 8));
                            rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Convert.ToInt64(item.EXPIRED_DATE.Value));
                        }
                       
                    if (material!=null)
                    {
                        rdo.ACTIVE_INGR_BHYT_NAME = material.HEIN_SERVICE_BHYT_NAME;
                        rdo.ACTIVE_INGR_BHYT_CODE = material.HEIN_SERVICE_BHYT_CODE;
                    }
                    if (material.IS_CHEMICAL_SUBSTANCE != null && material.IS_CHEMICAL_SUBSTANCE == 1)
                    {
                        rdo.TYPE = 3;//hóa học
                        rdo.TYPE_NAME = "Hóa Học";
                    }
                    else
                    {
                        rdo.TYPE = 2;//vật tư
                        rdo.TYPE_NAME = "Vật Tư";
                    }
                    listRdo.Add(rdo);
                }
            }
        }

        public List<Mrs00815RDO> Group(List<Mrs00815RDO> list)
        {

            var group = listRdo.GroupBy(x => new { x.TYPE, x.MEDI_STOCK_ID, x.IMP_PRICE, x.MATE_MEDI_TYPE_ID ,x.MATE_MEDI_PARENT_NAME}).ToList();
            listRdo.Clear();
            foreach (var item in group)
            {
                Mrs00815RDO rdo = new Mrs00815RDO();
                rdo.MATE_MEDI_BEAN_ID = item.First().MATE_MEDI_BEAN_ID;
                rdo.MATE_MEDI_TYPE_ID = item.First().MATE_MEDI_TYPE_ID;
                rdo.MATE_MEDI_TYPE_CODE = item.First().MATE_MEDI_TYPE_CODE;
                rdo.MATE_MEDI_TYPE_NAME = item.First().MATE_MEDI_TYPE_NAME;
                rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                rdo.IMP_PRICE = item.First().IMP_PRICE;
                rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                rdo.MEDI_STOCK_ID = item.First().MEDI_STOCK_ID;
                rdo.MEDI_STOCK_NAME = item.First().MEDI_STOCK_NAME;
                rdo.TOTAL_AMOUNT = item.Sum(x => x.TOTAL_AMOUNT);
                rdo.NUM_ORDER = item.First().NUM_ORDER;
                rdo.MATE_MEDI_PARENT_NAME = item.First().MATE_MEDI_PARENT_NAME;
                rdo.TYPE = item.First().TYPE;
                rdo.TYPE_NAME = item.First().TYPE_NAME;
                rdo.ACTIVE_INGR_BHYT_CODE = item.First().ACTIVE_INGR_BHYT_CODE;
                rdo.ACTIVE_INGR_BHYT_NAME = item.First().ACTIVE_INGR_BHYT_NAME;
                rdo.EXPIRED_DATE = item.First().EXPIRED_DATE;
                rdo.EXPIRED_DATE_STR = item.First().EXPIRED_DATE_STR;
                listRdo.Add(rdo);
            }
            return listRdo;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            for (int count = 0; count < listMediStock.Count; count++)
            {
                if (count > MAX_EXAM_SERVICE_TYPE_NUM - 1) break;
                var i = count + 1;
                System.Reflection.PropertyInfo piDepartment = typeof(Title).GetProperty("MEDI_STOCK_NAME_" + i);
                dicSingleTag.Add(string.Format("MEDI_STOCK_{0}", count + 1), piDepartment.GetValue(Title));
            }
            if (filter.IS_CHEMICAL_SUBSTANCE==true&&filter.IS_MATERIAL!=true)
            {
              listRdo =  listRdo.Where(x => x.TYPE_NAME != "Vật Tư").ToList();
            }
            objectTag.AddObjectData(store, "Report", listRdo.OrderBy(x=>x.MEDI_STOCK_ID).ToList());
            
            objectTag.AddObjectData(store, "Parent", listRdo.GroupBy(x => new { x.MATE_MEDI_PARENT_NAME, x.TYPE_NAME }).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "Parent", "Report", new string[] { "MATE_MEDI_PARENT_NAME", "TYPE_NAME" },new string[]{ "MATE_MEDI_PARENT_NAME","TYPE_NAME"});
            objectTag.AddObjectData(store, "TYPE", listRdo.GroupBy(x => x.TYPE_NAME).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "TYPE", "Parent", "TYPE_NAME", "TYPE_NAME");
        }
    }
}
