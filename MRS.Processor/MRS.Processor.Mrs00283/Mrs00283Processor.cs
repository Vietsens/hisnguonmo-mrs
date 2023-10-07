using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisSupplier;
using ACS.Filter; 
using AutoMapper; 
using FlexCel.Report; 
using Inventec.Common.FlexCellExport; 
using Inventec.Common.Logging; 
using Inventec.Core; 
using MOS.EFMODEL.DataModels; 
using MOS.Filter; 
using MRS.MANAGER.Base; 
using MRS.MANAGER.Config; 
using MRS.MANAGER.Core.MrsReport; 
 
using MOS.MANAGER.HisImpMestMaterial; 
using MOS.MANAGER.HisImpMestMedicine; 
using MOS.MANAGER.HisImpSource; 
using MOS.MANAGER.HisMaterial; 
using MOS.MANAGER.HisMedicine; 
using MOS.MANAGER.HisMediStock; 
using MRS.SDO; 
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisTreatment;
using FlexCel.Core;
using ACS.EFMODEL.DataModels;
using MOS.MANAGER.HisMaterialType; 

namespace MRS.Processor.Mrs00283
{
    class Mrs00283Processor : AbstractProcessor
    {
        List<V_HIS_IMP_MEST> listManuImpMest = new List<V_HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>(); 
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>(); 
        List<V_HIS_MEDICINE> listMedicine = new List<V_HIS_MEDICINE>(); 
        List<V_HIS_MATERIAL> listMaterial = new List<V_HIS_MATERIAL>();
        List<HIS_MATERIAL_TYPE> listMaterialTypes = new List<HIS_MATERIAL_TYPE>();
        List<HIS_MEDICINE_TYPE> listMedicineTypes = new List<HIS_MEDICINE_TYPE>();
        List<HIS_IMP_SOURCE> listImpSource = new List<HIS_IMP_SOURCE>();
        List<HIS_MEDICAL_CONTRACT> listContract = new List<HIS_MEDICAL_CONTRACT>();
        List<HIS_TREATMENT> listTreatment = new List<HIS_TREATMENT>();

        //Dictionary<long, HIS_IMP_SOURCE> dicImpSource = new Dictionary<long, HIS_IMP_SOURCE>(); 
        //Dictionary<long, V_HIS_MATERIAL> dicMaterial = new Dictionary<long, V_HIS_MATERIAL>(); 
        //Dictionary<long, V_HIS_MEDICINE> dicMedicine = new Dictionary<long, V_HIS_MEDICINE>(); 
        CommonParam paramGet = new CommonParam(); 
        List<Mrs00283RDO> ListRdo = new List<Mrs00283RDO>();
        List<Mrs00283RDO> ListRdo1 = new List<Mrs00283RDO>();
        List<Mrs00283RDO> ListRdo2 = new List<Mrs00283RDO>();
        List<Mrs00283RDO> ListRdo3 = new List<Mrs00283RDO>();
        List<Mrs00283RDO> ListRdo4 = new List<Mrs00283RDO>(); 
        List<Mrs00283RDO> ListSumSupplierRdo = new List<Mrs00283RDO>();
        List<Mrs00283RDO> ListDetailRdo = new List<Mrs00283RDO>();
        List<HIS_PATIENT_CLASSIFY> listPatientClassify = new List<HIS_PATIENT_CLASSIFY>();
        List<V_HIS_EXP_MEST> ListExpMest = new List<V_HIS_EXP_MEST>();
        List<HIS_STORAGE_CONDITION> listStorageCondition = new List<HIS_STORAGE_CONDITION>();

        //List<V_HIS_EXP_MEST_MEDICINE> ListExpMedi = new List<V_HIS_EXP_MEST_MEDICINE>();
        //List<V_HIS_EXP_MEST_MATERIAL> ListExpMate = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<long> cabinIds = new List<long>();
        List<long> bigMstIds = new List<long>();

        public Mrs00283Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00283Filter); 
        }

        protected override bool GetData()
        {
            var filter = ((Mrs00283Filter)reportFilter); 
            bool result = true; 
            try
            {
                string queryFilter = "";
                queryFilter += "left join his_treatment trea on im.tdl_treatment_id = trea.id\n";
                queryFilter += string.Format("where im.imp_time between {0} and {1}\n", filter.TIME_FROM, filter.TIME_TO);
                if (filter.IMP_MEST_STT_IDs != null)
                {
                    queryFilter += string.Format("and im.imp_mest_stt_id in ({0}) \n", string.Join(",", filter.IMP_MEST_STT_IDs));
                }
                if (filter.SUPPLIER_ID != null)
                {
                    queryFilter += string.Format("and im.supplier_id = {0}\n", filter.SUPPLIER_ID); //nhà cung cấp
                }
                if (filter.SUPPLIER_IDs != null)
                {
                    queryFilter += string.Format("and im.supplier_id = {0}\n",string.Join(",", filter.SUPPLIER_IDs)); //nhà cung cấp
                }
                if (filter.MEDI_STOCK_ID != null)
                {
                    queryFilter += string.Format("and im.medi_stock_id = {0}\n", filter.MEDI_STOCK_ID); //kho nhập
                }
                if (filter.MEDI_STOCK_IDs != null)
                {
                    queryFilter += string.Format("and im.medi_stock_id in ({0})\n", string.Join(",", filter.MEDI_STOCK_IDs)); //kho nhập cho chọn nhiều
                }
                if (filter.EXP_MEDI_STOCK_IDs != null)
                {
                    queryFilter += string.Format("and im.chms_medi_stock_id in ({0})\n", string.Join(",", filter.EXP_MEDI_STOCK_IDs)); //kho xuất cho chọn nhiều
                }
                if (filter.IMP_MEST_TYPE_ID != null)
                {
                    queryFilter += string.Format("and im.imp_mest_type_id = {0}\n", filter.IMP_MEST_TYPE_ID); //loại nhập
                }
                if (filter.DEPARTMENT_IDs != null)
                {
                    queryFilter += string.Format("and im.req_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs)); //khoa yêu cầu
                }
                if (filter.EXAM_ROOM_IDs != null)
                {
                    queryFilter += string.Format("and im.req_room_id in ({0})\n", string.Join(",", filter.EXAM_ROOM_IDs)); //phòng khám
                }
                if (filter.REQ_LOGINNAMEs != null)
                {
                    queryFilter += string.Format("and im.req_loginname in ('{0}')\n", string.Join("','", filter.REQ_LOGINNAMEs)); //người yêu cầu
                }
                if (!string.IsNullOrWhiteSpace(filter.DOCUMENT_NUMBER))
                {
                    queryFilter += string.Format("and im.document_number like '%{0}%'\n", filter.DOCUMENT_NUMBER);
                }
                if (filter.IMP_MEST_TYPE_IDs != null)
                {
                    queryFilter += string.Format("and im.imp_mest_type_id in ({0})\n", string.Join(",", filter.IMP_MEST_TYPE_IDs)); //loại nhập cho chọn nhiều
                }
                if (filter.PATIENT_TYPE_IDs != null)
                {
                    queryFilter += string.Format("and trea.tdl_patient_type_id in ({0})\n", string.Join(",", filter.PATIENT_TYPE_IDs));
                }
                if (filter.PATIENT_CLASSIFY_IDs != null)
                {
                    queryFilter += string.Format("and trea.TDL_PATIENT_CLASSIFY_ID in ({0})\n", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                }
                if (1 == 1)
                {
                    //get dữ liệu phiếu nhập:
                    string query = "select\n";
                    query += "im.*\n";
                    query += "from v_his_imp_mest im\n";
                    query += queryFilter;
                    listManuImpMest = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST>(query);
                    LogSystem.Info("SQL: " + query);
                }

                var expMestIds = listManuImpMest.Where(x=>x.MOBA_EXP_MEST_ID!=null).Select(x => x.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList();
                var skip = 0;
                while (expMestIds.Count- skip>0)
                {
                    var limit = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    HisExpMestViewFilterQuery expMestFilter = new HisExpMestViewFilterQuery();
                    expMestFilter.IDs = limit;
                    var expMests = new HisExpMestManager().GetView(expMestFilter);
                    ListExpMest.AddRange(expMests);
                }
                //if (((Mrs00283Filter)base.reportFilter).IS_GROUP_BY_MEDICINE != true) {
                //    var impIds = listManuImpMest.Where(x => x.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT).Select(x => x.ID).Distinct().ToList();
                //    listManuImpMest = listManuImpMest.Where(x => x.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT).ToList();
                //    HisImpMestViewFilterQuery himFilter = new HisImpMestViewFilterQuery();
                //    himFilter.AGGR_IMP_MEST_IDs = impIds;
                //    var impMests = new HisImpMestManager().GetView(himFilter);
                //    listManuImpMest.AddRange(impMests);
                //}
                if (1 == 1)
                {
                    //get dữ liệu thuốc trong phiếu nhập:
                    string query = "select\n";
                    query += "immm.*\n";
                    query += "from v_his_imp_mest_medicine immm\n";
                    query += "join v_his_imp_mest im on nvl(immm.aggr_imp_mest_id,immm.imp_mest_id)=im.id\n";
                    query += "join v_his_medicine me on immm.medicine_id = me.id \n";
                    query += queryFilter;
                    if (filter.MEDICINE_TYPE_ID != null)
                    {
                        query += string.Format("and me.MEDICINE_TYPE_ID = {0}\n", filter.MEDICINE_TYPE_ID); //loại thuốc
                    }
                    if (filter.MEDICINE_TYPE_IDs != null)
                    {
                        query += string.Format("and me.MEDICINE_TYPE_ID in ({0})\n", string.Join(",", filter.MEDICINE_TYPE_IDs)); //loại thuốc
                    }
                    if (filter.MATERIAL_TYPE_ID != null)
                    {
                        query += string.Format("and me.MEDICINE_TYPE_ID = 0\n", filter.MATERIAL_TYPE_ID); //loại thuốc
                    }
                    if (filter.MATERIAL_TYPE_IDs != null)
                    {
                        query += string.Format("and me.MEDICINE_TYPE_ID in (0)\n", string.Join(",", filter.MATERIAL_TYPE_IDs)); //loại thuốc
                    }
                    if (filter.IMP_SOURCE_IDs != null)
                    {
                        query += string.Format("and me.imp_source_id in ({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
                    }
                    if (filter.IMP_SOURCE_ID != null)
                    {
                        query += string.Format("and me.imp_source_id = {0} \n", filter.IMP_SOURCE_ID);
                    }
                    listImpMestMedicine = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MEDICINE>(query);
                    LogSystem.Info("SQL: " + query);
                }
                
                //Vật tư:
               
                if (1 == 1)
                {
                    //get dữ liệu thuốc trong phiếu nhập:
                    string query = "select\n";
                    query += "(case when immm.imp_mest_type_id= 17 then 0 else immm.imp_price end) imp_price,\n";
                    query += "immm.*\n";
                    query += "from v_his_imp_mest_material immm\n";
                    query += "join v_his_imp_mest im on nvl(immm.aggr_imp_mest_id,immm.imp_mest_id)=im.id\n";
                    query += "join v_his_material ma on immm.material_id = ma.id \n";
                    query += queryFilter;
                    if (filter.MATERIAL_TYPE_ID != null)
                    {
                        query += string.Format("and ma.MATERIAL_TYPE_ID = {0}\n", filter.MATERIAL_TYPE_ID); //loại vật tư
                    }
                    if (filter.MATERIAL_TYPE_IDs != null)
                    {
                        query += string.Format("and ma.MATERIAL_TYPE_ID in ({0})\n", string.Join(",", filter.MATERIAL_TYPE_IDs)); //loại thuốc
                    }
                    if (filter.MEDICINE_TYPE_ID != null)
                    {
                        query += string.Format("and ma.MATERIAL_TYPE_ID = 0\n", filter.MEDICINE_TYPE_ID); //loại thuốc
                    }
                    if (filter.MEDICINE_TYPE_IDs != null)
                    {
                        query += string.Format("and ma.MATERIAL_TYPE_ID in (0)\n", string.Join(",", filter.MEDICINE_TYPE_IDs)); //loại thuốc
                    }
                    if (filter.IMP_SOURCE_IDs != null)
                    {
                        query += string.Format("and ma.imp_source_id in ({0}) \n", string.Join(",", filter.IMP_SOURCE_IDs));
                    }
                    if (filter.IMP_SOURCE_ID != null)
                    {
                        query += string.Format("and ma.imp_source_id = {0} \n", filter.IMP_SOURCE_ID);
                    }
                    listImpMestMaterial = new MOS.DAO.Sql.SqlDAO().GetSql<V_HIS_IMP_MEST_MATERIAL>(query);
                    LogSystem.Info("SQL: " + query);
                }

                listImpSource = new HisImpSourceManager().Get(new HisImpSourceFilterQuery());
                //if (listImpSource != null)
                //{
                //    foreach (var i in listImpSource) if (!dicImpSource.ContainsKey(i.ID)) dicImpSource[i.ID] = i;
                //}

                string query1 = "select * from his_medical_contract";
                listContract = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_MEDICAL_CONTRACT>(query1);

                //get lô thuốc vật tư
                GetMediMate();
                GetStorageCondition();

                //get danh sách id tủ trực, id kho
                GetCabinIdMstId();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex); 
                result = false; 
            }
            return result; 
        }

        private void GetCabinIdMstId()
        {
            cabinIds = HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET == IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ID).ToList();
            bigMstIds = HisMediStockCFG.HisMediStocks.Where(o => o.IS_CABINET != IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__TRUE).Select(p => p.ID).ToList();
        }

        private void GetStorageCondition()
        {
            listStorageCondition = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_STORAGE_CONDITION>("select * from his_storage_condition");
        }


        private void GetMediMate()
        {
            try
            {
                List<long> medicineIds = new List<long>();

                if (listImpMestMedicine != null)
                {
                    medicineIds.AddRange(listImpMestMedicine.Select(o => o.MEDICINE_ID).ToList());
                }


                medicineIds = medicineIds.Distinct().ToList();

                if (medicineIds != null && medicineIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineIds.Count - skip > 0)
                    {
                        var limit = medicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineViewFilterQuery Medicinefilter = new HisMedicineViewFilterQuery();
                        Medicinefilter.IDs = limit;

                        var MedicineSub = new HisMedicineManager().GetView(Medicinefilter);
                        listMedicine.AddRange(MedicineSub);
                    }
                }

                List<long> materialIds = new List<long>();
                List<long> materialTypeIds = new List<long>();

                if (listImpMestMaterial != null)
                {
                    materialIds.AddRange(listImpMestMaterial.Select(o => o.MATERIAL_ID).ToList());
                    materialTypeIds.AddRange(listImpMestMaterial.Select(o => o.MATERIAL_TYPE_ID).ToList());
                }
              
                materialIds = materialIds.Distinct().ToList();

                if (materialIds != null && materialIds.Count > 0)
                {
                    var skip = 0;
                    while (materialIds.Count - skip > 0)
                    {
                        var limit = materialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialViewFilterQuery Materialfilter = new HisMaterialViewFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialManager().GetView(Materialfilter);
                        listMaterial.AddRange(MaterialSub);
                    }
                }

                materialTypeIds = materialTypeIds.Distinct().ToList();

                if (materialTypeIds != null && materialTypeIds.Count > 0)
                {
                    var skip = 0;
                    while (materialTypeIds.Count - skip > 0)
                    {
                        var limit = materialTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMaterialTypeFilterQuery Materialfilter = new HisMaterialTypeFilterQuery();
                        Materialfilter.IDs = limit;
                        var MaterialSub = new HisMaterialTypeManager().Get(Materialfilter);
                        listMaterialTypes.AddRange(MaterialSub);
                    }
                }

                
                List<long> medicineTypeIds = new List<long>();
                if (listImpMestMedicine != null)
                {
                    medicineTypeIds.AddRange(listImpMestMedicine.Select(p => p.MEDICINE_TYPE_ID).Distinct().ToList());
                }

                if (medicineTypeIds != null && medicineTypeIds.Count > 0)
                {
                    var skip = 0;
                    while (medicineTypeIds.Count - skip > 0)
                    {
                        var limit = medicineTypeIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        HisMedicineTypeFilterQuery Medicinefilter = new HisMedicineTypeFilterQuery();
                        Medicinefilter.IDs = limit;
                        var MedicineSub = new HisMedicineTypeManager().Get(Medicinefilter);
                        listMedicineTypes.AddRange(MedicineSub);
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
            bool result = true;
            try
            {
                ListRdo.Clear();
                string text = "select pr.id parent_id, pr.medicine_type_code parent_code, pr.medicine_type_name parent_name, mt.id medi_mate_id from his_medicine_type pr join his_medicine_type mt on pr.id = mt.parent_id";
                LogSystem.Info("ParentMedicine: " + text);
                List<Mrs00283ParentRDO> sql = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00283ParentRDO>(text, new object[0]);
                string text2 = "select pr.id parent_id, pr.material_type_code parent_code, pr.material_type_name parent_name, mt.id medi_mate_id from his_material_type pr join his_material_type mt on pr.id = mt.parent_id";
                LogSystem.Info("ParentMaterial: " + text2);
                List<Mrs00283ParentRDO> sql2 = new MOS.DAO.Sql.SqlDAO().GetSql<Mrs00283ParentRDO>(text2, new object[0]);
                string text3 = "select * from his_patient_classify";
                LogSystem.Info("PatientClassify: " + text3);
                listPatientClassify = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(text3, new object[0]);
                if (((Mrs00283Filter)base.reportFilter).IS_GROUP_BY_MEDICINE == true)
                {
                    ProcessGroupByMedicine(listImpMestMedicine, listImpMestMaterial, sql, sql2);
                }
                else
                {
                    ProcessAll(listManuImpMest);
                }
                ProcessOther(listImpMestMedicine, listImpMestMaterial);
                ProcessDetail(listImpMestMedicine, listImpMestMaterial);
            }
            catch (Exception ex)
            {
                result = false;
                LogSystem.Error(ex);
            }
            return result;
        }

        private void ProcessGroupByMedicine(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial, List<Mrs00283ParentRDO> listMedicineType, List<Mrs00283ParentRDO> listMaterialType)
        {
            try
            {
                if (IsNotNullOrEmpty(listImpMestMedicine))
                {
                    var group = listImpMestMedicine.GroupBy(p => new { p.MEDICINE_TYPE_ID, p.IMP_PRICE, p.IMP_VAT_RATIO }).ToList();

                    foreach (var item in group)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listSub = item.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        var parentMedi = listMedicineType.FirstOrDefault(p => p.MEDI_MATE_ID == listSub.First().MEDICINE_TYPE_ID);
                        var medicine = listMedicineTypes.FirstOrDefault(p => p.ID == listSub.First().MEDICINE_TYPE_ID) ?? new HIS_MEDICINE_TYPE();
                        var storageCondition = listStorageCondition.FirstOrDefault(p => p.ID == medicine.STORAGE_CONDITION_ID) ?? new HIS_STORAGE_CONDITION();
                        Mrs00283RDO rdo = new Mrs00283RDO();
                        rdo.STORAGE_CONDITION_NAME = storageCondition.STORAGE_CONDITION_NAME;
                        rdo.MEDI_MATE_CODE = listSub[0].MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_NAME = listSub[0].MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = listSub[0].CONCENTRA;

                        if (parentMedi != null)
                        {
                            rdo.PARENT_NAME = parentMedi.PARENT_NAME;
                            rdo.PARENT_CODE = parentMedi.PARENT_CODE;
                        }
                        else
                        {
                            rdo.PARENT_NAME = "Thuốc khác";
                            rdo.PARENT_CODE = "DIFERRENT_MEDICINE";
                        }
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        rdo.CHMS_AMOUNT = listSub.Where(o => o.CHMS_TYPE_ID != null).Sum(p => p.AMOUNT);
                        List<long> impMestIdSub = listSub.Select(o=>o.IMP_MEST_ID).Distinct().ToList();
                        var impMestSub = listManuImpMest.Where(o => impMestIdSub.Contains(o.ID)).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o=>bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID??0)).Select(p=>p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = listSub.Where(o => impMestCabinToMstIds.Contains(o.IMP_MEST_ID)).Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        ListRdo1.Add(rdo);

                    }
                    var group1 = listImpMestMedicine.GroupBy(p => new { p.MEDICINE_ID, p.IMP_PRICE, p.IMP_VAT_RATIO, p.IMP_MEST_TYPE_ID }).ToList();
                    foreach (var item1 in group1)
                    {
                        List<V_HIS_IMP_MEST_MEDICINE> listSub = item1.ToList<V_HIS_IMP_MEST_MEDICINE>();
                        var parentMedi = listMedicineType.FirstOrDefault(p => p.MEDI_MATE_ID == listSub.First().MEDICINE_TYPE_ID);
                        var medicine = listMedicineTypes.FirstOrDefault(p => p.ID == listSub.First().MEDICINE_TYPE_ID) ?? new HIS_MEDICINE_TYPE();
                        var storageCondition = listStorageCondition.FirstOrDefault(p => p.ID == medicine.STORAGE_CONDITION_ID) ?? new HIS_STORAGE_CONDITION();
                        Mrs00283RDO rdo = new Mrs00283RDO();
                        rdo.STORAGE_CONDITION_NAME = storageCondition.STORAGE_CONDITION_NAME;
                        rdo.MEDI_MATE_CODE = listSub[0].MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_NAME = listSub[0].MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        rdo.CONCENTRA = listSub[0].CONCENTRA;
                        rdo.IMP_MEST_TYPE_ID = listSub[0].IMP_MEST_TYPE_ID;
                        if (parentMedi != null)
                        {
                            rdo.PARENT_NAME = parentMedi.PARENT_NAME;
                            rdo.PARENT_CODE = parentMedi.PARENT_CODE;
                        }
                        else
                        {
                            rdo.PARENT_NAME = "Thuốc khác";
                            rdo.PARENT_CODE = "DIFERRENT_MEDICINE";
                        }
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        List<long> impMestIdSub = listSub.Select(o => o.IMP_MEST_ID).Distinct().ToList();
                        var impMestSub = listManuImpMest.Where(o => impMestIdSub.Contains(o.ID)).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = listSub.Where(o => impMestCabinToMstIds.Contains(o.IMP_MEST_ID)).Sum(p => p.AMOUNT);
                        rdo.CHMS_AMOUNT = listSub.Where(o => o.CHMS_TYPE_ID != null).Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        ListRdo3.Add(rdo);

                    }
                }
                if (IsNotNullOrEmpty(listImpMestMaterial))
                {
                    var group = listImpMestMaterial.GroupBy(p => new { p.MATERIAL_TYPE_ID, p.IMP_PRICE, p.IMP_VAT_RATIO }).ToList();
                    foreach (var item in group)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listSub = item.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        var parentMate = listMaterialType.FirstOrDefault(p => p.MEDI_MATE_ID == listSub.First().MATERIAL_TYPE_ID);
                        Mrs00283RDO rdo = new Mrs00283RDO();
                        
                        rdo.MEDI_MATE_CODE = listSub[0].MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_NAME = listSub[0].MATERIAL_TYPE_NAME;
                        //rdo.SERVICE_UNIT_NAME = listSub[0].IMP_UNIT_NAME;
                        rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                        //rdo.CONCENTRA = listSub[0].CONCENTRA;

                        if (parentMate != null)
                        {
                            rdo.PARENT_NAME = parentMate.PARENT_NAME;
                            rdo.PARENT_CODE = parentMate.PARENT_CODE;
                        }
                        else
                        {
                            rdo.PARENT_NAME = "Vật tư khác";
                            rdo.PARENT_CODE = "DIFERRENT_MATERIAL";
                        }
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        rdo.CHMS_AMOUNT = listSub.Where(o => o.CHMS_TYPE_ID != null).Sum(p => p.AMOUNT);
                        List<long> impMestIdSub = listSub.Select(o => o.IMP_MEST_ID).Distinct().ToList();
                        var impMestSub = listManuImpMest.Where(o => impMestIdSub.Contains(o.ID)).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = listSub.Where(o => impMestCabinToMstIds.Contains(o.IMP_MEST_ID)).Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        ListRdo1.Add(rdo);

                    }
                    var group1 = listImpMestMaterial.GroupBy(p => new { p.MATERIAL_ID, p.IMP_PRICE, p.IMP_VAT_RATIO, p.IMP_MEST_TYPE_ID }).ToList();
                    foreach (var item1 in group1)
                    {
                        List<V_HIS_IMP_MEST_MATERIAL> listSub = item1.ToList<V_HIS_IMP_MEST_MATERIAL>();
                        var parentMate = listMaterialType.FirstOrDefault(p => p.MEDI_MATE_ID == listSub.First().MATERIAL_TYPE_ID);
                        Mrs00283RDO rdo = new Mrs00283RDO();
                        rdo.MEDI_MATE_CODE = listSub[0].MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_NAME = listSub[0].MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = listSub[0].SERVICE_UNIT_NAME;
                        //rdo.CONCENTRA = listSub[0].CONCENTRA;
                        rdo.IMP_MEST_TYPE_ID = listSub[0].IMP_MEST_TYPE_ID;
                        if (parentMate != null)
                        {
                            rdo.PARENT_NAME = parentMate.PARENT_NAME;
                            rdo.PARENT_CODE = parentMate.PARENT_CODE;
                        }
                        else
                        {
                            rdo.PARENT_NAME = "Vật tư khác";
                            rdo.PARENT_CODE = "DIFERRENT_MATERIAL";
                        }
                        rdo.AMOUNT = listSub.Sum(p => p.AMOUNT);
                        List<long> impMestIdSub = listSub.Select(o => o.IMP_MEST_ID).Distinct().ToList();
                        var impMestSub = listManuImpMest.Where(o => impMestIdSub.Contains(o.ID)).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = listSub.Where(o => impMestCabinToMstIds.Contains(o.IMP_MEST_ID)).Sum(p => p.AMOUNT);
                        rdo.CHMS_AMOUNT = listSub.Where(o => o.CHMS_TYPE_ID != null).Sum(p => p.AMOUNT);
                        rdo.PRICE = listSub[0].IMP_PRICE;
                        rdo.VAT = listSub[0].IMP_VAT_RATIO;
                        ListRdo3.Add(rdo);

                    }
                }
                
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessAll(List<V_HIS_IMP_MEST> listManuImpMest)
        {
            
            try
            {
                if (IsNotNullOrEmpty(listManuImpMest))
                {
                    var suppliers = new MOS.MANAGER.HisSupplier.HisSupplierManager().Get(new HisSupplierFilterQuery());
                    foreach (var manuImpMest in listManuImpMest)
                    {
                        var ime = listImpMestMedicine.Where(o => o.IMP_MEST_ID == manuImpMest.ID || o.AGGR_IMP_MEST_ID == manuImpMest.ID).ToList();
                        var ima = listImpMestMaterial.Where(o => o.IMP_MEST_ID == manuImpMest.ID || o.AGGR_IMP_MEST_ID == manuImpMest.ID).ToList();
                        if ((this.reportFilter as Mrs00283Filter).MATERIAL_TYPE_IDs != null || (this.reportFilter as Mrs00283Filter).MATERIAL_TYPE_ID != null)
                        {
                            if (ima.Count == 0) continue;

                        }
                        if ((this.reportFilter as Mrs00283Filter).MEDICINE_TYPE_IDs != null || (this.reportFilter as Mrs00283Filter).MEDICINE_TYPE_ID != null)
                        {
                            if (ime.Count == 0) continue;

                        }
                        var contract = listContract.Where(o => o.ID == manuImpMest.MEDICAL_CONTRACT_ID).ToList();
                        var treatment = listTreatment.FirstOrDefault(o => o.ID == manuImpMest.TDL_TREATMENT_ID);

                        Mrs00283RDO rdo = new Mrs00283RDO();
                        if (treatment != null)
                        {
                            rdo.PATIENT_CODE = treatment.TDL_PATIENT_CODE;
                            rdo.PATIENT_NAME = treatment.TDL_PATIENT_NAME;
                        }
                        rdo.DOCUMENT_NUMBER = manuImpMest.DOCUMENT_NUMBER;
                        rdo.IMP_MEST_ID = manuImpMest.ID;
                        rdo.IMP_MEST_CODE = manuImpMest.IMP_MEST_CODE;
                        rdo.IMP_MEST_TYPE_CODE = manuImpMest.IMP_MEST_TYPE_CODE;
                        rdo.IMP_MEST_TYPE_NAME = manuImpMest.IMP_MEST_TYPE_NAME;
                        rdo.AGGR_IMP_MEST_CODE = manuImpMest.TDL_AGGR_IMP_MEST_CODE ?? manuImpMest.IMP_MEST_CODE;
                        var aggr = listManuImpMest.Where(x => x.ID == manuImpMest.AGGR_IMP_MEST_ID).FirstOrDefault();
                        if (aggr!=null)
                        {
                            rdo.AGGR_IMP_MEST_TYPE_NAME = aggr.IMP_MEST_TYPE_NAME;
                        }
                        //rdo.AGGR_IMP_MEST_TYPE_NAME = manuImpMest.AGGR_IMP_MEST_ID > 0 ? "Tổng hợp trả" : manuImpMest.IMP_MEST_TYPE_NAME;
                        rdo.IMP_TIME_NUMBER = manuImpMest.IMP_TIME ?? 0;
                        rdo.IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest.IMP_TIME ?? 0);
                        rdo.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(manuImpMest.IMP_TIME ?? 0);
                        rdo.MEDI_STOCK = manuImpMest.MEDI_STOCK_NAME;

                        rdo.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest.DOCUMENT_DATE ?? 0);
                        var supplier = suppliers.FirstOrDefault(o => o.ID == manuImpMest.SUPPLIER_ID);
                        rdo.SUPPLIER = supplier != null ? supplier.SUPPLIER_NAME : "";
                        rdo.SUPPLIER_ID = supplier != null ? supplier.ID : 0;
                        rdo.AMOUNT = ime.Sum(s => s.AMOUNT) + ima.Sum(s => s.AMOUNT);
                        rdo.CHMS_AMOUNT = ime.Where(o => o.CHMS_TYPE_ID != null).Sum(s => s.AMOUNT) + ima.Where(o => o.CHMS_TYPE_ID != null).Sum(s => s.AMOUNT);
                        if (bigMstIds.Contains(manuImpMest.MEDI_STOCK_ID) && cabinIds.Contains(manuImpMest.CHMS_MEDI_STOCK_ID ?? 0))
                        {
                            rdo.CABIN_TO_MST_AMOUNT = ime.Sum(s => s.AMOUNT) + ima.Sum(s => s.AMOUNT);
                        }
                        rdo.PRICE = ime.Sum(s => s.AMOUNT * s.IMP_PRICE) + ima.Sum(s => s.AMOUNT * s.IMP_PRICE);
                        rdo.VAT = ime.Sum(s => s.IMP_PRICE * s.IMP_VAT_RATIO * s.AMOUNT) + ima.Sum(s => s.IMP_PRICE * s.IMP_VAT_RATIO * s.AMOUNT);
                        rdo.TOTAL_PRICE = ime.Sum(s => s.AMOUNT * s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) + ima.Sum(s => s.AMOUNT * s.IMP_PRICE * (1 + s.IMP_VAT_RATIO));
                        rdo.TOTAL_PRICE_IMP = ime.Sum(s => s.AMOUNT * s.IMP_PRICE * (1 + s.IMP_VAT_RATIO)) + ima.Sum(s => s.AMOUNT * s.IMP_PRICE * (1 + s.IMP_VAT_RATIO));
                        rdo.DOCUMENT_PRICE = manuImpMest.DOCUMENT_PRICE ?? 0;
                        rdo.DOCUMENT_SUPPLIER_CODE = manuImpMest.DOCUMENT_SUPPLIER_CODE;
                        rdo.DOCUMENT_SUPPLIER_NAME = manuImpMest.DOCUMENT_SUPPLIER_NAME;
                        rdo.IMP_MEST_TYPE_ID = manuImpMest.IMP_MEST_TYPE_ID;
                        if (manuImpMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK)
                        {
                            rdo.EXP_MEST_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                            rdo.EXP_MEST_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                            if (manuImpMest.CHMS_TYPE_ID== 1)
                            {
                                rdo.IMP_MEST_TYPE_NAME = "Bổ sung cơ số tủ trực";
                            }
                            if (manuImpMest.CHMS_TYPE_ID == 2)
                            {
                                rdo.IMP_MEST_TYPE_NAME = "Thu hồi cơ số tủ trực";
                            }

                        }
                        else
                        {
                            rdo.EXP_MEST_STOCK_CODE = manuImpMest.REQ_DEPARTMENT_CODE;
                            rdo.EXP_MEST_STOCK_NAME = manuImpMest.REQ_DEPARTMENT_NAME;
                        }
                        var checkContract = contract.FirstOrDefault();
                        if (checkContract != null)
                        {
                            rdo.MEDICAL_CONTRACT_ID = checkContract.ID;
                            rdo.MEDICAL_CONTRACT_CODE = checkContract.MEDICAL_CONTRACT_CODE;
                            rdo.MEDICAL_CONTRACT_NAME = checkContract.MEDICAL_CONTRACT_NAME;
                        }
                        var mobaExp = ListExpMest.Where(x => x.ID == manuImpMest.MOBA_EXP_MEST_ID).FirstOrDefault();
                        if (mobaExp != null)
                        {
                            if (mobaExp.EXP_MEST_TYPE_NAME == "Xuất dùng chung")
                            {
                                rdo.AGGR_IMP_MEST_TYPE_NAME = "Nhập xuất dùng chung trả lại";
                                rdo.IMP_MEST_TYPE_NAME = "Nhập xuất dùng chung trả lại";
                            }
                        }
                        var medi = ime.Select(o => o.MEDICINE_ID).Distinct().ToList();

                        if (medi != null)
                        {
                            var medicine = listMedicine.Where(o => medi.Contains(o.ID)).ToList();
                            if (medicine != null && medicine.Count > 0)
                            {
                                var impSource = listImpSource.Where(o => medicine.Select(p => p.IMP_SOURCE_ID ?? 0).Contains(o.ID)).ToList();

                                if (impSource != null && impSource.Count > 0)
                                    rdo.IMP_SOURCE_NAME = string.Join(";", impSource.Select(o => o.IMP_SOURCE_NAME).ToList());
                            }
                        }
                        if (rdo.IMP_SOURCE_NAME == null)
                        {
                            var mate = ime.Select(o => o.MEDICINE_ID).Distinct().ToList();
                            if (mate != null)
                            {
                                var material = listMaterial.Where(o => mate.Contains(o.ID)).ToList();
                                if (material != null && material.Count > 0)
                                {
                                    var impSource = listImpSource.Where(o => material.Select(p => p.IMP_SOURCE_ID ?? 0).Contains(o.ID)).ToList();
                                    if (impSource != null && impSource.Count > 0)
                                        rdo.IMP_SOURCE_NAME = string.Join(";", impSource.Select(o => o.IMP_SOURCE_NAME).ToList());
                                }
                            }
                        }
                        if (manuImpMest.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK)
                        {
                            rdo.IMP_SOURCE_NAME = "";
                        }
                        ListRdo.Add(rdo);
                    }

                    ListRdo = ListRdo.OrderBy(o => o.SUPPLIER_ID).ThenBy(p => p.IMP_TIME_NUMBER).ToList();
                    ListRdo2 = ListRdo2.OrderBy(p => p.IMP_MEST_CODE).ThenBy(P => P.MEDI_MATE_CODE).ToList();
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessOther(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial)
        {
            try
            {
                var result = false;
                if (IsNotNullOrEmpty(listImpMestMedicine))
                {

                    foreach (var medi in listImpMestMedicine)
                    {

                        var medicine = listMedicineTypes.FirstOrDefault(p => p.ID == medi.MEDICINE_TYPE_ID) ?? new HIS_MEDICINE_TYPE();
                        var storageCondition = listStorageCondition.FirstOrDefault(p => p.ID == medicine.STORAGE_CONDITION_ID) ?? new HIS_STORAGE_CONDITION();
                        Mrs00283RDO rdo = new Mrs00283RDO();
                        rdo.STORAGE_CONDITION_NAME = storageCondition.STORAGE_CONDITION_NAME;

                        rdo.MEDI_MATE_TYPE_CODE = medi.MEDICINE_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = medi.MEDICINE_TYPE_NAME;
                        rdo.CONCENTRA = medi.CONCENTRA;
                        rdo.ACTIVE_INGR_BHYT_NAME = medi.ACTIVE_INGR_BHYT_NAME;
                        rdo.MANUFACTURER_NAME = medi.MANUFACTURER_NAME;
                        rdo.SERVICE_UNIT_NAME = medi.SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = medi.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medi.EXPIRED_DATE ?? 0);

                        rdo.PRICE = medi.IMP_PRICE;
                        rdo.VAT = medi.IMP_VAT_RATIO;
                        rdo.MEDI_MATE_PRICE = medi.IMP_PRICE * (1 + medi.IMP_VAT_RATIO);
                        rdo.MEDI_MATE_AMOUNT = medi.AMOUNT;
                        if (medi.CHMS_TYPE_ID != null)
                        {
                            rdo.CHMS_AMOUNT = medi.AMOUNT;
                        }
                        var impMestSub = listManuImpMest.Where(o => medi.IMP_MEST_ID==o.ID).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = impMestCabinToMstIds.Count>0?medi.AMOUNT:0;
                        rdo.MEDI_MATE_TOTAL_PRICE = medi.IMP_PRICE * (1 + medi.IMP_VAT_RATIO) * medi.AMOUNT;
                        rdo.IMP_MEST_CODE = medi.IMP_MEST_CODE;
                        ListRdo2.Add(rdo);
                    }
                }

                if (IsNotNullOrEmpty(listImpMestMaterial))
                {

                    foreach (var mate in listImpMestMaterial)
                    {

                        Mrs00283RDO rdo = new Mrs00283RDO();

                        rdo.MEDI_MATE_TYPE_CODE = mate.MATERIAL_TYPE_CODE;
                        rdo.MEDI_MATE_TYPE_NAME = mate.MATERIAL_TYPE_NAME;

                        rdo.MANUFACTURER_NAME = mate.MANUFACTURER_NAME;
                        rdo.SERVICE_UNIT_NAME = mate.SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = mate.PACKAGE_NUMBER;
                        rdo.EXPIRED_DATE_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(mate.EXPIRED_DATE ?? 0);

                        rdo.PRICE = mate.IMP_PRICE;
                        rdo.VAT = mate.IMP_VAT_RATIO;
                        rdo.MEDI_MATE_PRICE = mate.IMP_PRICE * (1 + mate.IMP_VAT_RATIO);
                        rdo.MEDI_MATE_AMOUNT = mate.AMOUNT;
                        if (mate.CHMS_TYPE_ID != null)
                        {
                            rdo.CHMS_AMOUNT = mate.AMOUNT;
                        }
                        var impMestSub = listManuImpMest.Where(o => mate.IMP_MEST_ID == o.ID).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo.CABIN_TO_MST_AMOUNT = impMestCabinToMstIds.Count > 0 ? mate.AMOUNT : 0;
                        rdo.MEDI_MATE_TOTAL_PRICE = mate.IMP_PRICE * (1 + mate.IMP_VAT_RATIO) * mate.AMOUNT;
                        rdo.IMP_MEST_CODE = mate.IMP_MEST_CODE;
                        ListRdo2.Add(rdo);

                    }
                }

                ListRdo4 = ListRdo2.GroupBy(p => new { p.MEDI_MATE_CODE, p.PRICE, p.VAT }).Select(o => new Mrs00283RDO
                {
                    MEDI_MATE_TYPE_CODE = o.First().MEDI_MATE_TYPE_CODE,
                    MEDI_MATE_TYPE_NAME = o.First().MEDI_MATE_TYPE_NAME,
                    CONCENTRA = o.First().CONCENTRA,
                    SERVICE_UNIT_NAME = o.First().SERVICE_UNIT_NAME,
                    PRICE = o.First().PRICE,
                    VAT = o.First().VAT,
                    STORAGE_CONDITION_NAME = o.First().STORAGE_CONDITION_NAME,
                    MEDI_MATE_PRICE = o.Sum(p => p.MEDI_MATE_PRICE),
                    MEDI_MATE_AMOUNT = o.Sum(p => p.MEDI_MATE_AMOUNT),
                    CHMS_AMOUNT = o.Sum(p => p.CHMS_AMOUNT),
                    CABIN_TO_MST_AMOUNT = o.Sum(p => p.CABIN_TO_MST_AMOUNT),
                    MEDI_MATE_TOTAL_PRICE = o.Sum(p => p.MEDI_MATE_TOTAL_PRICE)
                }).ToList();

                result = true;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        private void ProcessDetail(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine, List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial)
        {
            
            try
            {
                List<HIS_SUPPLIER> source;
                
                if (IsNotNullOrEmpty(listImpMestMedicine))
                {
                    source = new HisSupplierManager().Get(new HisSupplierFilterQuery());
                    List<long> list2 = new List<long>() {5,6,7,21,41};
                    
                    foreach (var item2 in listImpMestMedicine)
                    {
                        List<V_HIS_IMP_MEST> source2 = listManuImpMest;
                        Func<V_HIS_IMP_MEST, bool> predicate = (V_HIS_IMP_MEST p) => p.ID == item2.IMP_MEST_ID || p.ID == item2.AGGR_IMP_MEST_ID;
                        V_HIS_IMP_MEST manuImpMest2 = source2.FirstOrDefault(predicate) ?? new V_HIS_IMP_MEST();
                        HIS_TREATMENT val = listTreatment.FirstOrDefault((HIS_TREATMENT o) => o.ID == manuImpMest2.TDL_TREATMENT_ID);
                        var medicine = listMedicineTypes.FirstOrDefault(p => p.ID == item2.MEDICINE_TYPE_ID) ?? new HIS_MEDICINE_TYPE();
                        var storageCondition = listStorageCondition.FirstOrDefault(p => p.ID == medicine.STORAGE_CONDITION_ID) ?? new HIS_STORAGE_CONDITION();
                        Mrs00283RDO rdo2 = new Mrs00283RDO();
                        rdo2.STORAGE_CONDITION_NAME = storageCondition.STORAGE_CONDITION_NAME;
                        if (val != null)
                        {
                            rdo2.PATIENT_CODE = val.TDL_PATIENT_CODE;
                            rdo2.PATIENT_NAME = val.TDL_PATIENT_NAME;
                            LogSystem.Info("id: " + (val.TDL_PATIENT_CLASSIFY_ID ?? 0));
                            if (val.TDL_PATIENT_CLASSIFY_ID.HasValue)
                            {
                                rdo2.PATIENT_CLASSIFY_ID = ((!list2.Contains(val.TDL_PATIENT_CLASSIFY_ID ?? 0) && val.TDL_PATIENT_TYPE_ID.HasValue && val.TDL_PATIENT_TYPE_ID == 3) ? 5 : (val.TDL_PATIENT_CLASSIFY_ID ?? 0));
                                var val2 = listPatientClassify.FirstOrDefault(p => p.ID == rdo2.PATIENT_CLASSIFY_ID) ?? new HIS_PATIENT_CLASSIFY();
                                rdo2.PATIENT_CLASSIFY_CODE = val2.PATIENT_CLASSIFY_CODE;
                                rdo2.PATIENT_CLASSIFY_NAME = val2.PATIENT_CLASSIFY_NAME;
                            }
                            else
                            {
                                rdo2.PATIENT_CLASSIFY_CODE = "OTHER_CLASSIFY";
                                rdo2.PATIENT_CLASSIFY_NAME = "Đối tượng chi tiêt khác";
                            }
                        }
                        else
                        {
                            rdo2.PATIENT_CLASSIFY_CODE = "OTHER_CLASSIFY";
                            rdo2.PATIENT_CLASSIFY_NAME = "Đối tượng chi tiết khác";
                        }
                        rdo2.MEDI_MATE_TYPE_CODE = item2.MEDICINE_TYPE_CODE;
                        rdo2.MEDI_MATE_TYPE_NAME = item2.MEDICINE_TYPE_NAME;
                        rdo2.TYPE = "THUỐC";
                        rdo2.SERVICE_UNIT_NAME = item2.SERVICE_UNIT_NAME;
                        rdo2.CONCENTRA = item2.CONCENTRA;
                        rdo2.DOCUMENT_NUMBER = manuImpMest2.DOCUMENT_NUMBER;
                        rdo2.IMP_MEST_ID = manuImpMest2.ID;
                        rdo2.IMP_MEST_CODE = manuImpMest2.IMP_MEST_CODE;
                        rdo2.IMP_MEST_TYPE_CODE = manuImpMest2.IMP_MEST_TYPE_CODE;
                        rdo2.IMP_MEST_TYPE_NAME = manuImpMest2.IMP_MEST_TYPE_NAME;
                        rdo2.AGGR_IMP_MEST_CODE = manuImpMest2.TDL_AGGR_IMP_MEST_CODE ?? manuImpMest2.IMP_MEST_CODE;
                        rdo2.AGGR_IMP_MEST_TYPE_NAME = ((manuImpMest2.AGGR_IMP_MEST_ID > 0) ? "Tổng hợp trả" : manuImpMest2.IMP_MEST_TYPE_NAME);
                        rdo2.IMP_TIME_NUMBER = manuImpMest2.IMP_TIME ?? 0;
                        rdo2.IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest2.IMP_TIME ?? 0);
                        rdo2.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(manuImpMest2.IMP_TIME ?? 0);
                        rdo2.MEDI_STOCK = manuImpMest2.MEDI_STOCK_NAME;
                        rdo2.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest2.DOCUMENT_DATE ?? 0);
                        HIS_SUPPLIER val3 = source.FirstOrDefault((HIS_SUPPLIER o) => o.ID == manuImpMest2.SUPPLIER_ID);
                        rdo2.SUPPLIER = ((val3 != null) ? val3.SUPPLIER_NAME : "");
                        rdo2.SUPPLIER_ID = ((val3 != null) ? val3.ID : 0);
                        rdo2.AMOUNT = item2.AMOUNT;
                        rdo2.CHMS_AMOUNT = (item2.CHMS_TYPE_ID != null) ? item2.AMOUNT : 0;
                        var impMestSub = listManuImpMest.Where(o => item2.IMP_MEST_ID == o.ID).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo2.PRICE = item2.PRICE ?? item2.IMP_PRICE;
                        rdo2.VAT = item2.VAT_RATIO ?? item2.IMP_VAT_RATIO;
                        Mrs00283RDO mrs00283RDO = rdo2;
                        rdo2.PRICE = item2.PRICE ?? item2.IMP_PRICE;
                        
                        rdo2.TOTAL_PRICE = rdo2.PRICE * (1 + rdo2.VAT) * rdo2.AMOUNT;
                        rdo2.TOTAL_PRICE_IMP = item2.IMP_PRICE * (item2.IMP_VAT_RATIO + 1);
                        rdo2.DOCUMENT_PRICE = manuImpMest2.DOCUMENT_PRICE ?? 0;
                        rdo2.DOCUMENT_SUPPLIER_CODE = manuImpMest2.DOCUMENT_SUPPLIER_CODE;
                        rdo2.DOCUMENT_SUPPLIER_NAME = manuImpMest2.DOCUMENT_SUPPLIER_NAME;
                        rdo2.IMP_MEST_TYPE_ID = manuImpMest2.IMP_MEST_TYPE_ID;
                        rdo2.IMP_MEST_TYPE_ID = manuImpMest2.IMP_MEST_TYPE_ID;
                        rdo2.IMP_MEST_TYPE_CODE = manuImpMest2.IMP_MEST_TYPE_CODE;
                        rdo2.IMP_MEST_TYPE_NAME = manuImpMest2.IMP_MEST_TYPE_NAME;
                        if (manuImpMest2.IMP_MEST_TYPE_ID == 1)
                        {
                            rdo2.EXP_MEST_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest2.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                            rdo2.EXP_MEST_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest2.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                        }
                        else
                        {
                            rdo2.EXP_MEST_STOCK_CODE = manuImpMest2.REQ_DEPARTMENT_CODE;
                            rdo2.EXP_MEST_STOCK_NAME = manuImpMest2.REQ_DEPARTMENT_NAME;
                        }
                        ListDetailRdo.Add(rdo2);
                    }
                }
                if (IsNotNullOrEmpty(listImpMestMaterial))
                {
                    source = new HisSupplierManager().Get(new HisSupplierFilterQuery());
                    List<long> list2 = new List<long>() {5,6,7,21,41};
                    
                    foreach (var item2 in listImpMestMaterial)
                    {
                        List<V_HIS_IMP_MEST> source2 = listManuImpMest;
                        Func<V_HIS_IMP_MEST, bool> predicate = (V_HIS_IMP_MEST p) => p.ID == item2.IMP_MEST_ID || p.ID == item2.AGGR_IMP_MEST_ID;
                        V_HIS_IMP_MEST manuImpMest2 = source2.FirstOrDefault(predicate) ?? new V_HIS_IMP_MEST();
                        HIS_TREATMENT val = listTreatment.FirstOrDefault((HIS_TREATMENT o) => o.ID == manuImpMest2.TDL_TREATMENT_ID);
                        var materialType = listMaterialTypes.FirstOrDefault(o => o.ID == item2.MATERIAL_TYPE_ID) ?? new HIS_MATERIAL_TYPE();
                        Mrs00283RDO rdo2 = new Mrs00283RDO();
                        if (val != null)
                        {
                            rdo2.PATIENT_CODE = val.TDL_PATIENT_CODE;
                            rdo2.PATIENT_NAME = val.TDL_PATIENT_NAME;
                            LogSystem.Info("id: " + (val.TDL_PATIENT_CLASSIFY_ID ?? 0));
                            if (val.TDL_PATIENT_CLASSIFY_ID.HasValue)
                            {
                                rdo2.PATIENT_CLASSIFY_ID = ((!list2.Contains(val.TDL_PATIENT_CLASSIFY_ID ?? 0) && val.TDL_PATIENT_TYPE_ID.HasValue && val.TDL_PATIENT_TYPE_ID == 3) ? 5 : (val.TDL_PATIENT_CLASSIFY_ID ?? 0));
                                var val2 = listPatientClassify.FirstOrDefault(p => p.ID == rdo2.PATIENT_CLASSIFY_ID) ?? new HIS_PATIENT_CLASSIFY();
                                rdo2.PATIENT_CLASSIFY_CODE = val2.PATIENT_CLASSIFY_CODE;
                                rdo2.PATIENT_CLASSIFY_NAME = val2.PATIENT_CLASSIFY_NAME;
                            }
                            else
                            {
                                rdo2.PATIENT_CLASSIFY_CODE = "OTHER_CLASSIFY";
                                rdo2.PATIENT_CLASSIFY_NAME = "Đối tượng chi tiêt khác";
                            }
                        }
                        else
                        {
                            rdo2.PATIENT_CLASSIFY_CODE = "OTHER_CLASSIFY";
                            rdo2.PATIENT_CLASSIFY_NAME = "Đối tượng chi tiết khác";
                        }
                        rdo2.MEDI_MATE_TYPE_CODE = item2.MATERIAL_TYPE_CODE;
                        rdo2.MEDI_MATE_TYPE_NAME = item2.MATERIAL_TYPE_NAME;
                        if (materialType.IS_CHEMICAL_SUBSTANCE != null && materialType.IS_CHEMICAL_SUBSTANCE == 1)
                        {
                            rdo2.TYPE = "HÓA CHẤT";
                        }
                        else
                        {
                            rdo2.TYPE = "VẬT TƯ";
                        }
                        rdo2.SERVICE_UNIT_NAME = item2.SERVICE_UNIT_NAME;
                        rdo2.CONCENTRA = "";
                        rdo2.DOCUMENT_NUMBER = manuImpMest2.DOCUMENT_NUMBER;
                        rdo2.IMP_MEST_ID = manuImpMest2.ID;
                        rdo2.IMP_MEST_CODE = manuImpMest2.IMP_MEST_CODE;
                        rdo2.IMP_MEST_TYPE_CODE = manuImpMest2.IMP_MEST_TYPE_CODE;
                        rdo2.IMP_MEST_TYPE_NAME = manuImpMest2.IMP_MEST_TYPE_NAME;
                        rdo2.AGGR_IMP_MEST_CODE = manuImpMest2.TDL_AGGR_IMP_MEST_CODE ?? manuImpMest2.IMP_MEST_CODE;
                        rdo2.AGGR_IMP_MEST_TYPE_NAME = ((manuImpMest2.AGGR_IMP_MEST_ID > 0) ? "Tổng hợp trả" : manuImpMest2.IMP_MEST_TYPE_NAME);
                        rdo2.IMP_TIME_NUMBER = manuImpMest2.IMP_TIME ?? 0;
                        rdo2.IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest2.IMP_TIME ?? 0);
                        rdo2.IMP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(manuImpMest2.IMP_TIME ?? 0);
                        rdo2.MEDI_STOCK = manuImpMest2.MEDI_STOCK_NAME;
                        rdo2.DOCUMENT_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(manuImpMest2.DOCUMENT_DATE ?? 0);
                        HIS_SUPPLIER val3 = source.FirstOrDefault((HIS_SUPPLIER o) => o.ID == manuImpMest2.SUPPLIER_ID);
                        rdo2.SUPPLIER = ((val3 != null) ? val3.SUPPLIER_NAME : "");
                        rdo2.SUPPLIER_ID = ((val3 != null) ? val3.ID : 0);
                        rdo2.AMOUNT = item2.AMOUNT;
                        var impMestSub = listManuImpMest.Where(o => item2.IMP_MEST_ID == o.ID).ToList();
                        var impMestCabinToMstIds = impMestSub.Where(o => bigMstIds.Contains(o.MEDI_STOCK_ID) && cabinIds.Contains(o.CHMS_MEDI_STOCK_ID ?? 0)).Select(p => p.ID).ToList();
                        rdo2.CHMS_AMOUNT = (item2.CHMS_TYPE_ID!= null) ?item2.AMOUNT:0;
                        rdo2.PRICE = item2.PRICE ?? item2.IMP_PRICE;
                        rdo2.VAT = item2.VAT_RATIO ?? item2.IMP_VAT_RATIO;
                        Mrs00283RDO mrs00283RDO = rdo2;
                        rdo2.PRICE = item2.PRICE ?? item2.IMP_PRICE;
                        
                        rdo2.TOTAL_PRICE = rdo2.PRICE * (1 + rdo2.VAT) * rdo2.AMOUNT;
                        rdo2.TOTAL_PRICE_IMP = item2.IMP_PRICE * (item2.IMP_VAT_RATIO + 1);
                        rdo2.DOCUMENT_PRICE = manuImpMest2.DOCUMENT_PRICE ?? 0;
                        rdo2.DOCUMENT_SUPPLIER_CODE = manuImpMest2.DOCUMENT_SUPPLIER_CODE;
                        rdo2.DOCUMENT_SUPPLIER_NAME = manuImpMest2.DOCUMENT_SUPPLIER_NAME;
                        rdo2.IMP_MEST_TYPE_ID = manuImpMest2.IMP_MEST_TYPE_ID;
                        rdo2.IMP_MEST_TYPE_CODE = manuImpMest2.IMP_MEST_TYPE_CODE;
                        rdo2.IMP_MEST_TYPE_NAME = manuImpMest2.IMP_MEST_TYPE_NAME;
                        if (manuImpMest2.IMP_MEST_TYPE_ID == 1)
                        {
                            rdo2.EXP_MEST_STOCK_CODE = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest2.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_CODE;
                            rdo2.EXP_MEST_STOCK_NAME = (HisMediStockCFG.HisMediStocks.FirstOrDefault(p => p.ID == manuImpMest2.CHMS_MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK()).MEDI_STOCK_NAME;
                        }
                        else
                        {
                            rdo2.EXP_MEST_STOCK_CODE = manuImpMest2.REQ_DEPARTMENT_CODE;
                            rdo2.EXP_MEST_STOCK_NAME = manuImpMest2.REQ_DEPARTMENT_NAME;
                        }
                        ListDetailRdo.Add(rdo2);
                    }
                }
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            if (((Mrs00283Filter)reportFilter).TIME_FROM > 0)
            {
                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00283Filter)reportFilter).TIME_FROM)); 
            }
            if (((Mrs00283Filter)reportFilter).TIME_TO > 0)
            {
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString((long)((Mrs00283Filter)reportFilter).TIME_TO)); 
            }
            if (((Mrs00283Filter)reportFilter).MEDI_STOCK_ID > 0)
            {
                var x = new HisMediStockManager().Get(new HisMediStockFilterQuery()).Where(o => ((Mrs00283Filter)reportFilter).MEDI_STOCK_ID == o.ID).ToList();
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("MEDI_STOCK_NAME", x.First().MEDI_STOCK_NAME);
            }
            else
            {
                string a = "NOTHING";
                dicSingleTag.Add("MEDI_STOCK_NAME", a);
            }
            if (((Mrs00283Filter)reportFilter).IMP_SOURCE_ID > 0)
            {
                var x = new HisImpSourceManager(paramGet).Get(new HisImpSourceFilterQuery()).Where(o => ((Mrs00283Filter)reportFilter).IMP_SOURCE_ID == o.ID).ToList(); 
                if (IsNotNullOrEmpty(x))
                    dicSingleTag.Add("IMP_SOURCE_NAME", x.First().IMP_SOURCE_NAME); 
            }
            if (((Mrs00283Filter)reportFilter).IMP_MEST_TYPE_ID != null)
            {
                string query = string.Format("select * from his_imp_mest_type where id = {0}", ((Mrs00283Filter)reportFilter).IMP_MEST_TYPE_ID);
                var listType = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_IMP_MEST_TYPE>(query);
                if (listType != null)
                {
                    dicSingleTag.Add("IMP_MEST_TYPE", listType.FirstOrDefault().IMP_MEST_TYPE_NAME);
                }
            }
            ListSumSupplierRdo = ListRdo.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).GroupBy(o => o.SUPPLIER).Select(p => new Mrs00283RDO { SUPPLIER = p.First().SUPPLIER, TOTAL_PRICE = p.Sum(q => q.TOTAL_PRICE) }).ToList();
            var ListImpSourceRdo = ListRdo.GroupBy(o => o.IMP_SOURCE_NAME).Select(p => new Mrs00283RDO { IMP_SOURCE_NAME = p.First().IMP_SOURCE_NAME != "" ? p.First().IMP_SOURCE_NAME : "Nguồn Khác", TOTAL_PRICE = p.Sum(q => q.TOTAL_PRICE), DOCUMENT_PRICE = p.Sum(q => q.DOCUMENT_PRICE) }).ToList();

            objectTag.AddObjectData(store, "ImpSource", ListImpSourceRdo.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).OrderBy(p => p.IMP_TIME_NUMBER).ToList());
            objectTag.AddObjectData(store, "ReportAll", ListRdo.Where(p => p.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL).OrderBy(p => p.IMP_MEST_TYPE_CODE).ThenBy(p => p.IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "ReportAllImpMestType", ListRdo.Where(p => p.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT).OrderBy(p => p.IMP_MEST_TYPE_CODE).ThenBy(p => p.IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "ReportDetail", ListRdo.Where(p => p.IMP_MEST_TYPE_ID != IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__THT).OrderBy(p => p.IMP_MEST_TYPE_CODE).ThenBy(p => p.IMP_MEST_CODE).ToList());
            objectTag.AddObjectData(store, "Report", ListRdo.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).OrderByDescending(x=>x.DOCUMENT_DATE).ThenBy(p => p.IMP_TIME_NUMBER).ToList());
            objectTag.AddObjectData(store, "Report0", ListRdo.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).Where(p => !string.IsNullOrWhiteSpace(p.DOCUMENT_SUPPLIER_CODE)).OrderBy(p => p.IMP_TIME_NUMBER).ToList());
            objectTag.AddObjectData(store, "Report1", ListRdo.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__NCC).Where(p => string.IsNullOrWhiteSpace(p.DOCUMENT_SUPPLIER_CODE)).OrderBy(p => p.IMP_TIME_NUMBER).ToList());
            objectTag.AddObjectData(store, "SumSupplier", ListSumSupplierRdo.OrderBy(p => p.IMP_TIME_NUMBER).ToList()); 
            objectTag.AddRelationship(store, "SumSupplier", "Report", "SUPPLIER", "SUPPLIER"); 
            objectTag.SetUserFunction(store, "FuncSameTitleCol", new CustomerFuncMergeSameData());
            dicSingleTag.Add("BILL_COUNT", ListRdo.Select(p => p.DOCUMENT_NUMBER).Count());
            //List<long> mestMediID = new List<long>();
            //LogSystem.Info("ListExpMedi" + ListExpMedi.Count);
            //LogSystem.Info("ListExpMate" + ListExpMate.Count);
            //mestMediID.AddRange(ListExpMedi.Select(p => p.EXP_MEST_ID ?? 0).ToList());
            //mestMediID.AddRange(ListExpMate.Select(p => p.EXP_MEST_ID ?? 0).ToList());
            dicSingleTag.Add("EXP_COUNT", ListRdo.Select(p => p.EXP_MEST_CODE).Distinct().Count());
            objectTag.AddObjectData(store, "MediMate", ListRdo1.OrderBy(p => p.PARENT_NAME).ThenBy(p => p.MEDI_MATE_CODE).ToList());
            objectTag.AddObjectData(store, "MediMate1", ListRdo3.Where(p => p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__CK).OrderBy(p => p.PARENT_NAME).ThenBy(p => p.MEDI_MATE_CODE).ToList());
            objectTag.AddObjectData(store, "ImpMest", ListRdo2.OrderBy(p => p.MEDI_MATE_NAME).ToList());
            objectTag.AddObjectData(store, "ImpMestCode", ListRdo2.GroupBy(p => p.IMP_MEST_CODE).Select(p => p.First()).ToList());
            objectTag.AddRelationship(store, "ImpMestCode", "ImpMest", "IMP_MEST_CODE", "IMP_MEST_CODE");

            objectTag.AddObjectData(store, "Imp", ListRdo4.OrderBy(p => p.MEDI_MATE_NAME).ToList());
            objectTag.AddObjectData(store, "Details", ListDetailRdo.OrderBy(p => p.IMP_TIME_NUMBER).ToList());
            HisMedicineTypeFilterQuery mediTypeFilter = new HisMedicineTypeFilterQuery();
            if (((Mrs00283Filter)reportFilter).MEDICINE_TYPE_ID != null)
            {
                mediTypeFilter.ID = ((Mrs00283Filter)reportFilter).MEDICINE_TYPE_ID;
            }
            List<HIS_MEDICINE_TYPE> listMediType = new HisMedicineTypeManager().Get(mediTypeFilter);
            if (listMediType != null && ((Mrs00283Filter)reportFilter).MEDICINE_TYPE_ID != null)
            {
                dicSingleTag.Add("MEDICINE_TYPE_NAME", listMediType.First().MEDICINE_TYPE_NAME);
                dicSingleTag.Add("MEDICINE_TYPE_CODE", listMediType.First().MEDICINE_TYPE_CODE);
            }
            else
            {
                string a = "NOTHING";
                dicSingleTag.Add("MEDICINE_TYPE_NAME", a);
                dicSingleTag.Add("MEDICINE_TYPE_CODE", a);
            }
            dicSingleTag.Add("COUNT_ALL", ListRdo1.Count());

            #region Key của các trường lọc
            Mrs00283Filter filter = (Mrs00283Filter)reportFilter;

            //Loại nhập nhưng chỉ chọn 1
            if (filter.IMP_MEST_TYPE_ID != null)
            {
                string query = string.Format("select * from his_imp_mest_type where id = {0}", filter.IMP_MEST_TYPE_ID);
                var listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_IMP_MEST_TYPE>(query);
                if (listSub != null)
                {
                    dicSingleTag.Add("IMP_MEST_TYPE_NAME", listSub.FirstOrDefault().IMP_MEST_TYPE_NAME);
                }
            }

            //Loại nhập nhưng cho phép chọn nhiều
            if (filter.IMP_MEST_TYPE_IDs != null)
            {
                string query = string.Format("select * from his_imp_mest_type where id in ({0})", string.Join(",", filter.IMP_MEST_TYPE_IDs));
                var listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_IMP_MEST_TYPE>(query);
                if (listSub != null)
                {
                    dicSingleTag.Add("IMP_MEST_TYPE_NAMEs", string.Join(", ", listSub.Select(p => p.IMP_MEST_TYPE_NAME).ToList()));
                }
            }

            //Loại thuốc
            //if (filter.MEDICINE_TYPE_ID != null)
            //{
            //    HisMedicineTypeFilterQuery mediFilter = new HisMedicineTypeFilterQuery();
            //    mediFilter.ID = filter.MEDICINE_TYPE_ID;
            //    var listSub = new HisMedicineTypeManager().Get(mediFilter);
            //    if (listSub != null)
            //    {
            //        dicSingleTag.Add("MEDICINE_TYPE_NAME", listSub.Select(p => p.MEDICINE_TYPE_NAME).First());
            //    }
                
            //}

            //Đối tượng chi tiết
            if (filter.PATIENT_CLASSIFY_IDs != null)
            {
                string query = string.Format("select * from his_patient_classify where id in ({0})", string.Join(",", filter.PATIENT_CLASSIFY_IDs));
                var listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_CLASSIFY>(query);
                if (listSub != null)
                {
                    dicSingleTag.Add("PATIENT_CLASSIFY_NAMEs", string.Join(", ", listSub.Select(p => p.PATIENT_CLASSIFY_NAME).ToList()));
                }
            }

            //Đối tượng bệnh nhân
            if (filter.PATIENT_TYPE_IDs != null)
            {
                string query = string.Format("select * from his_patient_type where id in ({0})", string.Join(",", filter.PATIENT_TYPE_IDs));
                var listSub = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_PATIENT_TYPE>(query);
                if (listSub != null)
                {
                    dicSingleTag.Add("PATIENT_TYPE_NAMEs", string.Join(", ", listSub.Select(p => p.PATIENT_TYPE_NAME).ToList()));
                }
            }

            //Kho xuất
            if (filter.EXP_MEDI_STOCK_IDs != null)
            {
                var expMediStock = HisMediStockCFG.HisMediStocks.Where(p => filter.EXP_MEDI_STOCK_IDs.Contains(p.ID)).Select(p => p.MEDI_STOCK_NAME).ToList();
                if (expMediStock != null)
                {
                    dicSingleTag.Add("EXP_MEDI_STOCK_NAMEs", string.Join(", ", expMediStock));
                }
            }

            //Kho nhập
            if (filter.MEDI_STOCK_IDs != null)
            {
                var MediStock = HisMediStockCFG.HisMediStocks.Where(p => filter.MEDI_STOCK_IDs.Contains(p.ID)).Select(p => p.MEDI_STOCK_NAME).ToList();
                if (MediStock != null)
                {
                    dicSingleTag.Add("IMP_MEDI_STOCK_NAMEs", string.Join(", ", MediStock));
                }
            }

            //khoa
            if (filter.DEPARTMENT_IDs != null)
            {
                var department = HisDepartmentCFG.DEPARTMENTs.Where(p => filter.DEPARTMENT_IDs.Contains(p.ID)).Select(p => p.DEPARTMENT_NAME).ToList();
                if (department != null)
                {
                    dicSingleTag.Add("DEPARTMENT_NAMEs", string.Join(", ", department));
                }
            }

            //phòng khám
            if (filter.EXAM_ROOM_IDs != null)
            {
                var room = HisRoomCFG.HisRooms.Where(p => filter.EXAM_ROOM_IDs.Contains(p.ID)).Select(p => p.ROOM_NAME).ToList();
                if (room != null)
                {
                    dicSingleTag.Add("ROOM_NAMEs", string.Join(", ", room));
                }
            }

            //Số hóa đơn dùng luôn DOCUMENT_NUMBER

            //Tên bác sỹ (người yêu cầu)
            if (filter.REQ_LOGINNAMEs != null)
            {
                string query = string.Format("select * from acs_user where loginname in ('{0}')", string.Join("','", filter.REQ_LOGINNAMEs));
                List<ACS_USER> listSub = new MOS.DAO.Sql.MyAppContext().GetSql<ACS_USER>(query);
                if (listSub != null)
                {
                    dicSingleTag.Add("REQUEST_USERNAMEs", string.Join(", ", listSub.Select(p => p.USERNAME).ToList()));
                }
            }

            //Nhà cung cấp
            if (filter.SUPPLIER_ID != null)
            {
                HisSupplierFilterQuery supFilter = new HisSupplierFilterQuery();
                supFilter.ID = filter.SUPPLIER_ID;
                var listSub = new HisSupplierManager().Get(supFilter);
                if (listSub != null)
                {
                    dicSingleTag.Add("SUPPLIER_NAME", listSub.Select(p => p.SUPPLIER_NAME).First());
                }
            }
            
            //Nguồn nhập
            if (filter.IMP_SOURCE_ID != null)
            {
                HisImpSourceFilterQuery sourceFilter = new HisImpSourceFilterQuery();
                sourceFilter.ID = filter.IMP_SOURCE_ID;
                var listSub = new HisImpSourceManager().Get(sourceFilter);
                if (listSub != null)
                {
                    dicSingleTag.Add("IMP_SOURCE_NAME", listSub.Select(p => p.IMP_SOURCE_NAME).First());
                }
            }

            if (filter.IMP_SOURCE_IDs != null)
            {
                HisImpSourceFilterQuery sourceFilter = new HisImpSourceFilterQuery();
                sourceFilter.IDs = filter.IMP_SOURCE_IDs;
                var listSub = new HisImpSourceManager().Get(sourceFilter);
                if (listSub != null)
                {
                    dicSingleTag.Add("IMP_SOURCE_NAMEs", string.Join(", ", listSub.Select(p => p.IMP_SOURCE_NAME)));
                }
            }
            #endregion

            //if (((Mrs00283Filter)reportFilter).MEDI_STOCK_ID > 0)
            //{
            //    var exp_code = listManuExpMest.Where(p => p.MEDI_STOCK_ID == ((Mrs00283Filter)reportFilter).MEDI_STOCK_ID).Select(o => o.EXP_MEST_CODE).Distinct().ToList();
            //    dicSingleTag.Add("EXP_COUNT", exp_code.Count());
            //}
        }
    }

    class CustomerFuncMergeSameData : TFlexCelUserFunction
    {
        long? SupplierId; 

        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 1)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function"); 
            bool result = false; 
            try
            {
                long SupplierId_ = Convert.ToInt64(parameters[0]); 

                if (SupplierId_ != null)
                {
                    if (SupplierId_ == SupplierId)
                    {
                        return true; 
                    }
                    else
                    {
                        SupplierId = SupplierId_; 
                        return false; 
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Debug(ex); 
            }
            return result; 
        }
    }

    
}
