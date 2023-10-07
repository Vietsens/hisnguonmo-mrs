using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestType;
using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisMedicineGroup;
using MOS.MANAGER.HisMedicineType;
using Inventec.Common.Logging;
using MOS.MANAGER.HisTransaction;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisEmployee;

namespace MRS.Processor.Mrs00297
{
    class Mrs00297Processor : AbstractProcessor
    {
        Mrs00297Filter Filter = null;

        List<Mrs00297RDO> listImport = new List<Mrs00297RDO>();
        List<Mrs00297RDO> listImportRdo = new List<Mrs00297RDO>();
        List<Mrs00297RDO> TotalImport = new List<Mrs00297RDO>();
        List<Mrs00297RDO> listExport = new List<Mrs00297RDO>();
        List<Mrs00297RDO> TotalExport = new List<Mrs00297RDO>();

        List<Mrs00297RDO> listImportDetail = new List<Mrs00297RDO>();
        List<Mrs00297RDO> listExport1 = new List<Mrs00297RDO>();
        List<Mrs00297RDO> TotalExport1 = new List<Mrs00297RDO>();

        List<Mrs00297RDO> ListRdo = new List<Mrs00297RDO>();

        List<V_HIS_EXP_MEST_MATERIAL> listMaterialOut = null;
        List<V_HIS_EXP_MEST_MEDICINE> listMedicineOut = null;
        List<V_HIS_IMP_MEST> listMobaIn = null;

        List<HIS_MEDICINE_TYPE> listMedicineType = null;

        List<HIS_EXP_MEST_TYPE> listExpMestType = new List<HIS_EXP_MEST_TYPE>();

        Dictionary<long, decimal> dicThExpMestMaterialPrice = new Dictionary<long, decimal>();

        Dictionary<long, decimal> dicThExpMestMaterialVat = new Dictionary<long, decimal>();

        Dictionary<long, decimal> dicThExpMestMedicinePrice = new Dictionary<long, decimal>();

        Dictionary<long, decimal> dicThExpMestMedicineVat = new Dictionary<long, decimal>();

        Dictionary<string, EXT_INFO> dicExtInfo = new Dictionary<string, EXT_INFO>();

        Dictionary<string, HIS_EMPLOYEE> dicEmployee = new Dictionary<string, HIS_EMPLOYEE>();


        public Mrs00297Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00297Filter);
        }

        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.Filter = (Mrs00297Filter)this.reportFilter;
                //xử lý khi chỉ lọc thuốc hoặc vật tư
                ProcessFilterMetyMaty(ref this.Filter);
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu MRS00297: " + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => Filter), Filter));
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 

                HisExpMestMaterialViewFilterQuery materialFilter = new HisExpMestMaterialViewFilterQuery();
                materialFilter.EXP_TIME_FROM = Filter.TIME_FROM;
                materialFilter.EXP_TIME_TO = Filter.TIME_TO;
                materialFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_ID;
                materialFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_BUSINESS_ID;
                materialFilter.MEDI_STOCK_IDs = Filter.MEDI_STOCK_BUSINESS_IDs;
                materialFilter.IS_EXPORT = true;
                materialFilter.EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN;
                materialFilter.MATERIAL_TYPE_IDs = Filter.MATERIAL_TYPE_IDs;
                listMaterialOut = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(materialFilter);

                //if (Filter.IS_ONLY_SALE == true)
                //{
                //    listMaterialOut = listMaterialOut.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList();
                //}
                HisExpMestMedicineViewFilterQuery medicineFilter = new HisExpMestMedicineViewFilterQuery();
                medicineFilter.EXP_TIME_FROM = Filter.TIME_FROM;
                medicineFilter.EXP_TIME_TO = Filter.TIME_TO;
                medicineFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_ID;
                medicineFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_BUSINESS_ID;
                medicineFilter.MEDI_STOCK_IDs = Filter.MEDI_STOCK_BUSINESS_IDs;
                medicineFilter.IS_EXPORT = true;
                medicineFilter.EXP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN };
                medicineFilter.MEDICINE_TYPE_IDs = Filter.MEDICINE_TYPE_IDs;
                listMedicineOut = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(medicineFilter);
                //if(Filter.IS_ONLY_SALE == true)
                //{
                //    listMedicineOut = listMedicineOut.Where(o => o.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).ToList();
                //}  

                HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                mobaFilter.IMP_TIME_FROM = Filter.TIME_FROM;
                mobaFilter.IMP_TIME_TO = Filter.TIME_TO;
                mobaFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_ID;
                mobaFilter.MEDI_STOCK_ID = Filter.MEDI_STOCK_BUSINESS_ID;
                mobaFilter.MEDI_STOCK_IDs = Filter.MEDI_STOCK_BUSINESS_IDs;
                mobaFilter.IMP_MEST_TYPE_IDs = new List<long>() { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL };

                mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                listMobaIn = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(mobaFilter);

                //thong tin nhan vien
                var employees = new HisEmployeeManager().Get(new HisEmployeeFilterQuery());
                dicEmployee = employees.ToDictionary(o => o.LOGINNAME, p => p);

                //thong tin xuat bo sung
                AddToDicExtInfo(listMobaIn, ref dicExtInfo);
                AddToDicExtInfo(listMedicineOut, listMaterialOut, ref dicExtInfo);


                listMedicineType = new HisMedicineTypeManager().Get(new HisMedicineTypeFilterQuery());

                string query = "select * from his_exp_mest_type";
                listExpMestType = new MOS.DAO.Sql.SqlDAO().GetSql<HIS_EXP_MEST_TYPE>(query);
                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00289");
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessFilterMetyMaty(ref Mrs00297Filter mrs00297Filter)
        {
            if (mrs00297Filter.MEDICINE_TYPE_IDs != null && mrs00297Filter.MATERIAL_TYPE_IDs == null)
            {
                mrs00297Filter.MATERIAL_TYPE_IDs = new List<long>() { -1 };
            }
            if (mrs00297Filter.MATERIAL_TYPE_IDs != null && mrs00297Filter.MEDICINE_TYPE_IDs == null)
            {
                mrs00297Filter.MEDICINE_TYPE_IDs = new List<long>() { -1 };
            }
        }

        protected override bool ProcessData()
        {
            bool result = true;
            try
            {
                CommonParam paramGet = new CommonParam();
                List<V_HIS_IMP_MEST_MATERIAL> listMaterialIn = new List<V_HIS_IMP_MEST_MATERIAL>();
                List<V_HIS_IMP_MEST_MEDICINE> listMedicineIn = new List<V_HIS_IMP_MEST_MEDICINE>();
                List<V_HIS_ROOM> rooms = new List<V_HIS_ROOM>();
                List<V_HIS_MEDI_STOCK> mediStock = new List<V_HIS_MEDI_STOCK>();

                if (listMobaIn != null && listMobaIn.Count > 0)
                {
                    listMaterialIn = GetMaterialIn(ref paramGet);
                    listMedicineIn = GetMEdicineIn(ref paramGet);
                    List<long> thExpMestMedicineIds = listMedicineIn.Select(o => o.TH_EXP_MEST_MEDICINE_ID ?? 0).Distinct().ToList();

                    if (IsNotNullOrEmpty(thExpMestMedicineIds))
                    {
                        var skip = 0;
                        while (thExpMestMedicineIds.Count - skip > 0)
                        {
                            var lists = thExpMestMedicineIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisExpMestMedicineViewFilterQuery ExpMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                            ExpMestMedicineFilter.IDs = lists;
                            ExpMestMedicineFilter.MEDICINE_TYPE_IDs = Filter.MEDICINE_TYPE_IDs;
                            var listThExpMestMedicineSub = new HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicineFilter);
                            if (this.IsNotNullOrEmpty(listThExpMestMedicineSub)) //theo nguoi ban
                            {
                                foreach (var item in listThExpMestMedicineSub)
                                {
                                    if (!dicThExpMestMedicinePrice.ContainsKey(item.ID))
                                    {
                                        dicThExpMestMedicinePrice[item.ID] = (item.PRICE ?? 0);
                                    }
                                    if (!dicThExpMestMedicineVat.ContainsKey(item.ID))
                                    {
                                        dicThExpMestMedicineVat[item.ID] = (item.VAT_RATIO ?? 0);
                                    }
                                }
                            }

                        }
                    }
                    List<long> thExpMestMaterialIds = listMaterialIn.Select(o => o.TH_EXP_MEST_MATERIAL_ID ?? 0).Distinct().ToList();

                    if (IsNotNullOrEmpty(thExpMestMaterialIds))
                    {
                        var skip = 0;
                        while (thExpMestMaterialIds.Count - skip > 0)
                        {
                            var lists = thExpMestMaterialIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                            skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                            HisExpMestMaterialViewFilterQuery ExpMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                            ExpMestMaterialFilter.IDs = lists;
                            ExpMestMaterialFilter.MATERIAL_TYPE_IDs = Filter.MATERIAL_TYPE_IDs;
                            var listThExpMestMaterialSub = new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialFilter);
                            if (this.IsNotNullOrEmpty(listThExpMestMaterialSub)) //theo nguoi ban
                            {
                                foreach (var item in listThExpMestMaterialSub)
                                {
                                    if (!dicThExpMestMaterialPrice.ContainsKey(item.ID))
                                    {
                                        dicThExpMestMaterialPrice[item.ID] = (item.PRICE ?? 0);
                                    }
                                    if (!dicThExpMestMaterialVat.ContainsKey(item.ID))
                                    {
                                        dicThExpMestMaterialVat[item.ID] = (item.VAT_RATIO ?? 0);
                                    }
                                }
                            }

                        }
                    }
                }

                if (paramGet.HasException)
                {
                    throw new Exception("Co exception xay ra tai DAOGET trong qua tring tong hop du lieu MRS00297");
                }

                if (Filter.BRANCH_ID.HasValue)
                {
                    // lấy ra các roomId là kho thuộc chi nhánh
                    rooms = MANAGER.Config.HisRoomCFG.HisRooms.Where(o => o.BRANCH_ID == Filter.BRANCH_ID.Value && o.ROOM_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_ROOM_TYPE.ID__KHO).ToList();

                    var roomIds = rooms.Select(o => o.ID).ToList();

                    //lấy ra các mediStockId tương ứng với roomId
                    var mediStockIdList = MANAGER.Config.HisMediStockCFG.HisMediStocks.Where(o => roomIds.Contains(o.ROOM_ID)).Select(o => o.ID).ToList();

                    //lấy ra các thuốc/vật tư có mediStockId tương ứng
                    listMedicineIn = listMedicineIn.Where(o => mediStockIdList.Contains(o.MEDI_STOCK_ID)).ToList();
                    listMaterialIn = listMaterialIn.Where(o => mediStockIdList.Contains(o.MEDI_STOCK_ID)).ToList();
                    listMaterialOut = listMaterialOut.Where(o => mediStockIdList.Contains(o.MEDI_STOCK_ID)).ToList();
                    listMedicineOut = listMedicineOut.Where(o => mediStockIdList.Contains(o.MEDI_STOCK_ID)).ToList();
                }
                //loc theo khoa chi dinh
                if (Filter.REQUEST_DEPARTMENT_IDs != null)
                {
                    listMedicineIn = listMedicineIn.Where(o => dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) && Filter.REQUEST_DEPARTMENT_IDs.Contains(dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID)).ToList();
                    listMaterialIn = listMaterialIn.Where(o => dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) && Filter.REQUEST_DEPARTMENT_IDs.Contains(dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID)).ToList();
                    listMaterialOut = listMaterialOut.Where(o => dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) && Filter.REQUEST_DEPARTMENT_IDs.Contains(dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID)).ToList();
                    listMedicineOut = listMedicineOut.Where(o => dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) && Filter.REQUEST_DEPARTMENT_IDs.Contains(dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID)).ToList();
                }

                CreateListData(listMedicineIn, listMaterialIn, listMedicineOut, listMaterialOut, dicExtInfo);
                CreateTotal();

            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddToDicExtInfo(List<V_HIS_EXP_MEST_MEDICINE> expMedi, List<V_HIS_EXP_MEST_MATERIAL> expMate, ref Dictionary<string, EXT_INFO> dicExtInfo)
        {
            try
            {
                var listExpMestId = expMedi.Select(o => o.EXP_MEST_ID ?? 0).ToList();
                listExpMestId.AddRange(expMate.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                listExpMestId = listExpMestId.Distinct().ToList();
                int skip = 0;
                int count = listExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listIDs = listExpMestId.Skip(skip).Take(limit).ToList();
                    HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                    filter.IDs = listIDs;
                    var import = new HisExpMestManager().Get(filter);
                    if (import != null && import.Count > 0)
                    {
                        var billIds = import.Select(o => o.BILL_ID ?? 0).ToList();
                        HisTransactionFilterQuery tranfilter = new HisTransactionFilterQuery();
                        tranfilter.IDs = billIds;
                        var bills = new HisTransactionManager().Get(tranfilter) ?? new List<HIS_TRANSACTION>();

                        var serviceReqIds = import.Select(o => o.PRESCRIPTION_ID ?? o.SERVICE_REQ_ID ?? 0).ToList();
                        HisServiceReqFilterQuery serviceReqfilter = new HisServiceReqFilterQuery();
                        serviceReqfilter.IDs = serviceReqIds;
                        var serviceRerqs = new HisServiceReqManager().Get(serviceReqfilter) ?? new List<HIS_SERVICE_REQ>();

                        foreach (var item in import)
                        {
                            if (!dicExtInfo.ContainsKey("EXP_" + item.EXP_MEST_CODE))
                            {
                                dicExtInfo.Add("EXP_" + item.EXP_MEST_CODE, new EXT_INFO());
                            }
                            dicExtInfo["EXP_" + item.EXP_MEST_CODE].EXP_MEST_CODE = item.EXP_MEST_CODE;

                            var bill = bills.FirstOrDefault(o => item.BILL_ID == o.ID);
                            if (bill != null)
                            {
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].CASHIER_LOGINNAME = bill.CASHIER_LOGINNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].CASHIER_USERNAME = bill.CASHIER_USERNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].EXEMPTION = bill.EXEMPTION??0;
                            }
                            else
                            {
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].CASHIER_LOGINNAME = item.CASHIER_LOGINNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].CASHIER_USERNAME = item.CASHIER_USERNAME;
                            }
                            var serviceReq = serviceRerqs.FirstOrDefault(o => (item.PRESCRIPTION_ID ?? item.SERVICE_REQ_ID) == o.ID);
                            if (serviceReq != null)
                            {
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID = serviceReq.REQUEST_DEPARTMENT_ID;
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID);
                                if (department != null)
                                {
                                    dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                    dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                }
                            }
                            else
                            {
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_LOGINNAME = item.TDL_PRESCRIPTION_REQ_LOGINNAME;
                                var employee = (item.TDL_PRESCRIPTION_REQ_LOGINNAME != null && dicEmployee.ContainsKey(item.TDL_PRESCRIPTION_REQ_LOGINNAME)) ? dicEmployee[item.TDL_PRESCRIPTION_REQ_LOGINNAME] : new HIS_EMPLOYEE();
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_USERNAME = item.TDL_PRESCRIPTION_REQ_USERNAME ?? employee.TDL_USERNAME;
                                dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID = employee.DEPARTMENT_ID ?? item.REQ_DEPARTMENT_ID;
                                var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (employee.DEPARTMENT_ID ?? item.REQ_DEPARTMENT_ID));
                                if (department != null)
                                {
                                    dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                    dicExtInfo["EXP_" + item.EXP_MEST_CODE].REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                }

                            }
                        }
                    }
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddToDicExtInfo(List<V_HIS_IMP_MEST> impMest, ref Dictionary<string, EXT_INFO> dicExtInfo)
        {
            try
            {
                var listExpMestId = impMest.Select(o => o.MOBA_EXP_MEST_ID ?? 0).ToList();
                listExpMestId = listExpMestId.Distinct().ToList();
                int skip = 0;
                int count = listExpMestId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listIDs = listExpMestId.Skip(skip).Take(limit).ToList();
                    HisExpMestFilterQuery filter = new HisExpMestFilterQuery();
                    filter.IDs = listIDs;
                    var import = new HisExpMestManager().Get(filter);
                    if (import != null && import.Count > 0)
                    {
                        var billIds = import.Select(o => o.BILL_ID ?? 0).ToList();
                        HisTransactionFilterQuery tranfilter = new HisTransactionFilterQuery();
                        tranfilter.IDs = billIds;
                        var bills = new HisTransactionManager().Get(tranfilter);
                        var serviceReqIds = import.Select(o => o.PRESCRIPTION_ID ?? o.SERVICE_REQ_ID ?? 0).ToList();
                        HisServiceReqFilterQuery serviceReqfilter = new HisServiceReqFilterQuery();
                        serviceReqfilter.IDs = serviceReqIds;
                        var serviceRerqs = new HisServiceReqManager().Get(serviceReqfilter) ?? new List<HIS_SERVICE_REQ>();
                        var imps = impMest.Where(o => listIDs.Contains(o.MOBA_EXP_MEST_ID ?? 0)).ToList();
                        foreach (var item in imps)
                        {
                            if (!dicExtInfo.ContainsKey("IMP_" + item.IMP_MEST_CODE))
                            {
                                dicExtInfo.Add("IMP_" + item.IMP_MEST_CODE, new EXT_INFO());
                            }
                            var exp = import.FirstOrDefault(o => item.MOBA_EXP_MEST_ID == o.ID);
                            if (exp != null)
                            {
                                dicExtInfo["IMP_" + item.IMP_MEST_CODE].EXP_MEST_CODE = exp.EXP_MEST_CODE;
                                var bill = bills.FirstOrDefault(o => exp.BILL_ID == o.ID);
                                if (bill != null)
                                {
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].CASHIER_LOGINNAME = bill.CASHIER_LOGINNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].CASHIER_USERNAME = bill.CASHIER_USERNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].EXEMPTION = bill.EXEMPTION ?? 0;
                                }
                                else
                                {
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].CASHIER_LOGINNAME = exp.CASHIER_LOGINNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].CASHIER_USERNAME = exp.CASHIER_USERNAME;
                                }
                                var serviceReq = serviceRerqs.FirstOrDefault(o => (exp.PRESCRIPTION_ID ?? exp.SERVICE_REQ_ID) == o.ID);
                                if (serviceReq != null)
                                {
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_LOGINNAME = serviceReq.REQUEST_LOGINNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_USERNAME = serviceReq.REQUEST_USERNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID = serviceReq.REQUEST_DEPARTMENT_ID;
                                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == serviceReq.REQUEST_DEPARTMENT_ID);
                                    if (department != null)
                                    {
                                        dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                        dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    }
                                }
                                else
                                {
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_LOGINNAME = exp.TDL_PRESCRIPTION_REQ_LOGINNAME;
                                    var employee = (exp.TDL_PRESCRIPTION_REQ_LOGINNAME != null && dicEmployee.ContainsKey(exp.TDL_PRESCRIPTION_REQ_LOGINNAME)) ? dicEmployee[exp.TDL_PRESCRIPTION_REQ_LOGINNAME] : new HIS_EMPLOYEE();
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_USERNAME = exp.TDL_PRESCRIPTION_REQ_USERNAME ?? employee.TDL_USERNAME;
                                    dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID = employee.DEPARTMENT_ID??exp.REQ_DEPARTMENT_ID;
                                    var department = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == (employee.DEPARTMENT_ID ?? exp.REQ_DEPARTMENT_ID));
                                    if (department != null)
                                    {
                                        dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_CODE = department.DEPARTMENT_CODE;
                                        dicExtInfo["IMP_" + item.IMP_MEST_CODE].REQUEST_DEPARTMENT_NAME = department.DEPARTMENT_NAME;
                                    }

                                }
                            }
                        }
                    }
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private List<V_HIS_IMP_MEST_MATERIAL> GetMaterialIn(ref CommonParam paramGet)
        {
            List<V_HIS_IMP_MEST_MATERIAL> result = new List<V_HIS_IMP_MEST_MATERIAL>();
            try
            {
                var listId = listMobaIn.Select(o => o.ID).ToList();
                int skip = 0;
                int count = listId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listIDs = listId.Skip(skip).Take(limit).ToList();
                    HisImpMestMaterialViewFilterQuery filter = new HisImpMestMaterialViewFilterQuery();
                    filter.IMP_MEST_IDs = listIDs;
                    filter.MATERIAL_TYPE_IDs = Filter.MATERIAL_TYPE_IDs;
                    var import = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(filter);
                    if (import != null && import.Count > 0)
                    {
                        result.AddRange(import);
                    }
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = null;
            }
            return result;
        }

        private List<V_HIS_IMP_MEST_MEDICINE> GetMEdicineIn(ref CommonParam paramGet)
        {
            List<V_HIS_IMP_MEST_MEDICINE> result = new List<V_HIS_IMP_MEST_MEDICINE>();
            try
            {
                var listId = listMobaIn.Select(o => o.ID).ToList();
                int skip = 0;
                int count = listId.Count;
                while (count > 0)
                {
                    int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var listIDs = listId.Skip(skip).Take(limit).ToList();
                    HisImpMestMedicineViewFilterQuery filter = new HisImpMestMedicineViewFilterQuery();
                    filter.IMP_MEST_IDs = listIDs;
                    filter.MEDICINE_TYPE_IDs = Filter.MEDICINE_TYPE_IDs;
                    var import = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(filter);
                    if (import != null && import.Count > 0)
                    {
                        result.AddRange(import);
                    }
                    skip += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
            return result;
        }

        private void CreateListData(List<V_HIS_IMP_MEST_MEDICINE> listMedicineIn, List<V_HIS_IMP_MEST_MATERIAL> listMaterialIn, List<V_HIS_EXP_MEST_MEDICINE> listMedicineOut, List<V_HIS_EXP_MEST_MATERIAL> listMaterialOut, Dictionary<string, EXT_INFO> dicExtInfo)
        {
            try
            {
                string keyGroup = "{0}_{1}_{2}_{3}_{4}";

                //khi có điều kiện lọc từ template thì đổi sang key từ template
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
                {
                    keyGroup = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                if (IsNotNullOrEmpty(listMedicineIn))
                {
                    listMedicineIn = listMedicineIn.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                    var Groups = listMedicineIn.GroupBy(o => string.Format(keyGroup, o.MEDICINE_TYPE_ID, o.PRICE, o.IMP_PRICE, o.VAT_RATIO, o.IMP_VAT_RATIO,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID : 0,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_LOGINNAME : null,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].CASHIER_LOGINNAME : null,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].EXP_MEST_CODE : null));
                    foreach (var Group in Groups)
                    {
                        Mrs00297RDO ADO = new Mrs00297RDO();
                        ADO.MEDICINE_TYPE_NAME = Group.First().MEDICINE_TYPE_NAME;
                        ADO.MEDICINE_TYPE_CODE = Group.First().MEDICINE_TYPE_CODE;
                        ADO.PARENT_MEDICINE_TYPE_NAME = "Nhóm khác";
                        ADO.PARENT_MEDICINE_TYPE_CODE = "KHAC";
                        var medicineType = this.listMedicineType.FirstOrDefault(o => o.ID == Group.First().MEDICINE_TYPE_ID);
                        if (medicineType != null)
                        {
                            var parent = this.listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                            if (parent != null)
                            {
                                ADO.PARENT_MEDICINE_TYPE_NAME = parent.MEDICINE_TYPE_NAME ?? "Nhóm khác";
                                ADO.PARENT_MEDICINE_TYPE_CODE = parent.MEDICINE_TYPE_CODE ?? "KHAC";
                            }
                        }
                        ADO.IMP_MEST_TYPE_ID = Group.First().IMP_MEST_TYPE_ID;
                        ADO.MEDICINE_TYPE_CODE = Group.First().MEDICINE_TYPE_CODE;
                        ADO.SERVICE_UNIT_NAME = Group.First().SERVICE_UNIT_NAME;
                        ADO.AMOUNT = Group.Sum(o => o.AMOUNT);
                        ADO.IMP_PRICE = Group.First().IMP_PRICE * (1 + (Group.First().IMP_VAT_RATIO));
                        ADO.TOTAL_IMP_PRICE = ADO.AMOUNT * ADO.IMP_PRICE;
                        ADO.PRICE_1 = Group.First().TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicinePrice.ContainsKey(Group.First().TH_EXP_MEST_MEDICINE_ID ?? 0)
                           ? dicThExpMestMedicinePrice[Group.First().TH_EXP_MEST_MEDICINE_ID ?? 0]
                           : Group.First().IMP_PRICE;

                        ADO.VAT_RATIO_1 = Group.First().TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicineVat.ContainsKey(Group.First().TH_EXP_MEST_MEDICINE_ID ?? 0)
                            ? dicThExpMestMedicineVat[Group.First().TH_EXP_MEST_MEDICINE_ID ?? 0]
                            : Group.First().IMP_VAT_RATIO;
                        ADO.TOTAL_PRICE_1 = Group.Sum(o => o.AMOUNT * (o.TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicinePrice.ContainsKey(o.TH_EXP_MEST_MEDICINE_ID ?? 0)
                           ? dicThExpMestMedicinePrice[o.TH_EXP_MEST_MEDICINE_ID ?? 0]
                           : o.IMP_PRICE) * (1 + (o.TH_EXP_MEST_MEDICINE_ID != null && dicThExpMestMedicineVat.ContainsKey(o.TH_EXP_MEST_MEDICINE_ID ?? 0)
                            ? dicThExpMestMedicineVat[o.TH_EXP_MEST_MEDICINE_ID ?? 0]
                            : o.IMP_VAT_RATIO)));
                        ADO.INTEREST_PRICE_1 = (ADO.TOTAL_IMP_PRICE ?? 0)- (ADO.TOTAL_PRICE_1) ;
                        ADO.TYPE_ID = 1;
                        ADO.TYPE_NAME = "Thuốc";
                        if (dicExtInfo.ContainsKey("IMP_" + Group.First().IMP_MEST_CODE))
                        {
                            ADO.CASHIER_LOGINNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].CASHIER_LOGINNAME;
                            ADO.CASHIER_USERNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].CASHIER_USERNAME;
                            ADO.REQUEST_LOGINNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_LOGINNAME;
                            ADO.REQUEST_USERNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_USERNAME;
                            ADO.REQUEST_DEPARTMENT_ID = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_ID;
                            ADO.REQUEST_DEPARTMENT_CODE = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_CODE;
                            ADO.REQUEST_DEPARTMENT_NAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_NAME;
                            ADO.EXEMPTION = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].EXEMPTION;
                            ADO.EXP_MEST_CODE = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].EXP_MEST_CODE;
                        }
                        listImportRdo.Add(ADO);
                    }
                }

                if (IsNotNullOrEmpty(listMaterialIn))
                {
                    listMaterialIn = listMaterialIn.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
                    var Groups = listMaterialIn.GroupBy(o => string.Format(keyGroup, o.MATERIAL_TYPE_ID, o.PRICE, o.IMP_PRICE, o.VAT_RATIO, o.IMP_VAT_RATIO,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_DEPARTMENT_ID : 0,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].REQUEST_LOGINNAME : null,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].CASHIER_LOGINNAME : null,
                        dicExtInfo.ContainsKey("IMP_" + o.IMP_MEST_CODE) ? dicExtInfo["IMP_" + o.IMP_MEST_CODE].EXP_MEST_CODE : null));
                    foreach (var Group in Groups)
                    {
                        Mrs00297RDO ADO = new Mrs00297RDO();
                        ADO.MEDICINE_TYPE_NAME = Group.First().MATERIAL_TYPE_NAME;
                        ADO.MEDICINE_TYPE_CODE = Group.First().MATERIAL_TYPE_CODE;
                        ADO.PARENT_MEDICINE_TYPE_NAME = "VTYT";
                        ADO.PARENT_MEDICINE_TYPE_CODE = "Vật tư";
                        ADO.SERVICE_UNIT_NAME = Group.First().SERVICE_UNIT_NAME;
                        ADO.AMOUNT = Group.Sum(o => o.AMOUNT);
                        ADO.IMP_PRICE = Group.First().IMP_PRICE * (1 + (Group.First().IMP_VAT_RATIO));
                        ADO.IMP_MEST_TYPE_ID = Group.First().IMP_MEST_TYPE_ID;
                        ADO.TOTAL_IMP_PRICE = ADO.AMOUNT * ADO.IMP_PRICE;
                        ADO.PRICE_1 = Group.First().TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialPrice.ContainsKey(Group.First().TH_EXP_MEST_MATERIAL_ID ?? 0)
                           ? dicThExpMestMaterialPrice[Group.First().TH_EXP_MEST_MATERIAL_ID ?? 0]
                           : Group.First().IMP_PRICE;
                        ADO.VAT_RATIO_1 = Group.First().TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialVat.ContainsKey(Group.First().TH_EXP_MEST_MATERIAL_ID ?? 0)
                            ? dicThExpMestMaterialVat[Group.First().TH_EXP_MEST_MATERIAL_ID ?? 0]
                            : Group.First().IMP_VAT_RATIO;
                        ADO.TOTAL_PRICE_1 = Group.Sum(o => o.AMOUNT * (o.TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialPrice.ContainsKey(o.TH_EXP_MEST_MATERIAL_ID ?? 0)
                           ? dicThExpMestMaterialPrice[o.TH_EXP_MEST_MATERIAL_ID ?? 0]
                           : o.IMP_PRICE) * (1 + (o.TH_EXP_MEST_MATERIAL_ID != null && dicThExpMestMaterialVat.ContainsKey(o.TH_EXP_MEST_MATERIAL_ID ?? 0)
                            ? dicThExpMestMaterialVat[o.TH_EXP_MEST_MATERIAL_ID ?? 0]
                            : o.IMP_VAT_RATIO)));
                        ADO.INTEREST_PRICE_1 = (ADO.TOTAL_IMP_PRICE ?? 0) - (ADO.TOTAL_PRICE_1);
                        ADO.TYPE_ID = 2;
                        ADO.TYPE_NAME = "Vật tư";
                        if (dicExtInfo.ContainsKey("IMP_" + Group.First().IMP_MEST_CODE))
                        {
                            ADO.CASHIER_LOGINNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].CASHIER_LOGINNAME;
                            ADO.CASHIER_USERNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].CASHIER_USERNAME;
                            ADO.REQUEST_LOGINNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_LOGINNAME;
                            ADO.REQUEST_USERNAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_USERNAME;
                            ADO.REQUEST_DEPARTMENT_ID = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_ID;
                            ADO.REQUEST_DEPARTMENT_CODE = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_CODE;
                            ADO.REQUEST_DEPARTMENT_NAME = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].REQUEST_DEPARTMENT_NAME;
                            ADO.EXEMPTION = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].EXEMPTION;
                            ADO.EXP_MEST_CODE = dicExtInfo["IMP_" + Group.First().IMP_MEST_CODE].EXP_MEST_CODE;
                        }
                        listImportRdo.Add(ADO);

                    }

                }

                if (IsNotNullOrEmpty(listMedicineOut))
                {
                    listMedicineOut = listMedicineOut.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                    var Groups = listMedicineOut.GroupBy(o => string.Format(keyGroup, o.MEDICINE_TYPE_ID, o.PRICE, o.IMP_PRICE, o.VAT_RATIO, o.IMP_VAT_RATIO, o.EXP_MEST_TYPE_ID,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID : 0,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_LOGINNAME : null,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].CASHIER_LOGINNAME : null,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].EXP_MEST_CODE : null));
                    foreach (var Group in Groups)
                    {


                        Mrs00297RDO ADO = new Mrs00297RDO();
                        ADO.MEDICINE_TYPE_NAME = Group.First().MEDICINE_TYPE_NAME;
                        ADO.MEDICINE_TYPE_CODE = Group.First().MEDICINE_TYPE_CODE;
                        ADO.PARENT_MEDICINE_TYPE_NAME = "Nhóm khác";
                        ADO.PARENT_MEDICINE_TYPE_CODE = "KHAC";
                        var medicineType = this.listMedicineType.FirstOrDefault(o => o.ID == Group.First().MEDICINE_TYPE_ID);
                        if (medicineType != null)
                        {
                            var parent = this.listMedicineType.FirstOrDefault(o => o.ID == medicineType.PARENT_ID);
                            if (parent != null)
                            {
                                ADO.PARENT_MEDICINE_TYPE_NAME = parent.MEDICINE_TYPE_NAME ?? "Nhóm khác";
                                ADO.PARENT_MEDICINE_TYPE_CODE = parent.MEDICINE_TYPE_CODE ?? "KHAC";
                            }
                        }

                        ADO.EXP_MEST_TYPE_ID = Group.First().EXP_MEST_TYPE_ID;
                        ADO.EXP_MEST_TYPE_CODE = listExpMestType.Where(p => p.ID == ADO.EXP_MEST_TYPE_ID).FirstOrDefault().EXP_MEST_TYPE_CODE;
                        ADO.EXP_MEST_TYPE_NAME = listExpMestType.Where(p => p.ID == ADO.EXP_MEST_TYPE_ID).FirstOrDefault().EXP_MEST_TYPE_NAME;

                        if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN)
                        {
                            if (IsNotNullOrEmpty(listMedicineIn))
                            {

                                ADO.RETURN_AMOUNT = listMedicineIn.Where(p => p.MEDICINE_TYPE_ID == Group.First().MEDICINE_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL).Sum(o => o.AMOUNT);

                            }

                        }

                        else if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT)
                        {
                            if (IsNotNullOrEmpty(listMedicineIn))
                            {

                                ADO.RETURN_AMOUNT = listMedicineIn.Where(p => p.MEDICINE_TYPE_ID == Group.First().MEDICINE_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL).Sum(o => o.AMOUNT);

                            }
                        }

                        else if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                        {
                            if (IsNotNullOrEmpty(listMedicineIn))
                            {

                                ADO.RETURN_AMOUNT = listMedicineIn.Where(p => p.MEDICINE_TYPE_ID == Group.First().MEDICINE_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL).Sum(o => o.AMOUNT);

                            }

                        }
                        else
                        {
                            ADO.RETURN_AMOUNT = 0;
                            ADO.RETURN_PRICE = 0;
                        }

                        ADO.CONCENTRA = Group.First().CONCENTRA;
                        ADO.MEDICINE_NAME = ADO.MEDICINE_TYPE_NAME + " - " + ADO.CONCENTRA;
                        ADO.SERVICE_UNIT_NAME = Group.First().SERVICE_UNIT_NAME;
                        ADO.AMOUNT = Group.Sum(o => o.AMOUNT);
                        ADO.IMP_PRICE = Group.First().IMP_PRICE * (1 + (Group.First().IMP_VAT_RATIO));
                        ADO.PRICE = (Group.First().PRICE ?? 0) * (1 + (Group.First().VAT_RATIO ?? 0));
                        ADO.VAT_RATIO = Group.First().VAT_RATIO;
                        ADO.TOTAL_IMP_PRICE = ADO.AMOUNT * ADO.IMP_PRICE;
                        ADO.TOTAL_RETURN_PRICE = ADO.RETURN_AMOUNT * ADO.IMP_PRICE;
                        ADO.TOTAL_PRICE = ADO.AMOUNT * (ADO.PRICE ?? 0);
                        ADO.INTEREST_PRICE = (ADO.TOTAL_PRICE ?? 0) - (ADO.TOTAL_IMP_PRICE ?? 0);
                        ADO.TYPE_ID = 1;
                        ADO.TYPE_NAME = "Thuốc";
                        ADO.EXP_TIME = Group.First().EXP_DATE;
                        ADO.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(Group.First().EXP_DATE ?? 0);
                        if (dicExtInfo.ContainsKey("EXP_" + Group.First().EXP_MEST_CODE))
                        {
                            ADO.CASHIER_LOGINNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].CASHIER_LOGINNAME;
                            ADO.CASHIER_USERNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].CASHIER_USERNAME;
                            ADO.REQUEST_LOGINNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_LOGINNAME;
                            ADO.REQUEST_USERNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_USERNAME;
                            ADO.REQUEST_DEPARTMENT_ID = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_ID;
                            ADO.REQUEST_DEPARTMENT_CODE = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_CODE;
                            ADO.REQUEST_DEPARTMENT_NAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_NAME;
                            ADO.EXEMPTION = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].EXEMPTION;
                            ADO.EXP_MEST_CODE = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].EXP_MEST_CODE;
                        }
                        ListRdo.Add(ADO);

                    }

                    var time = listMedicineOut.GroupBy(o => new { o.MEDICINE_TYPE_ID, o.EXP_DATE });
                    foreach (var item in time)
                    {
                        Mrs00297RDO rdo = new Mrs00297RDO();
                        rdo.EXP_TIME = item.First().EXP_TIME;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.First().EXP_TIME ?? 0);
                        rdo.EXP_MEST_TYPE_ID = item.First().EXP_MEST_TYPE_ID;
                        rdo.TOTAL_PRICE = item.First().PRICE * (1 + item.First().VAT_RATIO) * item.Sum(p => p.AMOUNT);
                        rdo.TOTAL_IMP_PRICE = item.First().IMP_PRICE * (1 + item.First().IMP_VAT_RATIO) * item.Sum(p => p.AMOUNT);
                        rdo.INTEREST_PRICE = rdo.TOTAL_PRICE ?? 0 - rdo.TOTAL_IMP_PRICE ?? 0;
                        rdo.EXP_MEST_CODE = item.First().EXP_MEST_CODE;

                        if (item.First().EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN)
                        {
                            if (IsNotNullOrEmpty(listMedicineIn))
                            {
                                var returnAmount = listMedicineIn.FirstOrDefault(p => p.MEDICINE_TYPE_ID == item.First().MEDICINE_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL);
                                if (returnAmount != null)
                                {
                                    rdo.MEDICINE_TYPE_NAME = returnAmount.MEDICINE_TYPE_NAME;
                                    rdo.IMP_MEST_CODE = returnAmount.IMP_MEST_CODE;
                                    rdo.RETURN_AMOUNT = returnAmount.AMOUNT;
                                    rdo.IMP_VAT_RATIO = returnAmount.IMP_VAT_RATIO;
                                    rdo.IMP_PRICE = returnAmount.IMP_PRICE;
                                }
                            }
                        }
                        listExport1.Add(rdo);
                    }


                }

                if (IsNotNullOrEmpty(listMaterialOut))
                {
                    listMaterialOut = listMaterialOut.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
                    var Groups = listMaterialOut.GroupBy(o => string.Format(keyGroup, o.MATERIAL_TYPE_ID, o.PRICE, o.IMP_PRICE, o.VAT_RATIO, o.IMP_VAT_RATIO,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_DEPARTMENT_ID : 0,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].REQUEST_LOGINNAME : null,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].CASHIER_LOGINNAME : null,
                        dicExtInfo.ContainsKey("EXP_" + o.EXP_MEST_CODE) ? dicExtInfo["EXP_" + o.EXP_MEST_CODE].EXP_MEST_CODE : null));
                    foreach (var Group in Groups)
                    {
                        Mrs00297RDO ADO = new Mrs00297RDO();
                        ADO.MEDICINE_TYPE_NAME = Group.First().MATERIAL_TYPE_NAME;
                        ADO.MEDICINE_TYPE_CODE = Group.First().MATERIAL_TYPE_CODE;
                        ADO.PARENT_MEDICINE_TYPE_NAME = "VTYT";
                        ADO.PARENT_MEDICINE_TYPE_CODE = "Vật tư";
                        ADO.SERVICE_UNIT_NAME = Group.First().SERVICE_UNIT_NAME;
                        ADO.AMOUNT = Group.Sum(o => o.AMOUNT);
                        ADO.IMP_PRICE = Group.First().IMP_PRICE * (1 + (Group.First().IMP_VAT_RATIO));
                        ADO.PRICE = (Group.First().PRICE ?? 0) * (1 + (Group.First().VAT_RATIO ?? 0));
                        ADO.VAT_RATIO = Group.First().VAT_RATIO;
                        ADO.TOTAL_IMP_PRICE = ADO.AMOUNT * ADO.IMP_PRICE;
                        ADO.TOTAL_PRICE = ADO.AMOUNT * (ADO.PRICE ?? 0);
                        ADO.INTEREST_PRICE = (ADO.TOTAL_PRICE ?? 0) - (ADO.TOTAL_IMP_PRICE ?? 0);
                        ADO.TYPE_ID = 2;
                        ADO.TYPE_NAME = "Vật tư";
                        ADO.EXP_MEST_TYPE_ID = Group.First().EXP_MEST_TYPE_ID;
                        ADO.EXP_MEST_TYPE_CODE = listExpMestType.Where(p => p.ID == ADO.EXP_MEST_TYPE_ID).FirstOrDefault().EXP_MEST_TYPE_CODE;
                        ADO.EXP_MEST_TYPE_NAME = listExpMestType.Where(p => p.ID == ADO.EXP_MEST_TYPE_ID).FirstOrDefault().EXP_MEST_TYPE_NAME;
                        if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN)
                        {
                            if (IsNotNullOrEmpty(listMaterialIn))
                            {

                                ADO.RETURN_AMOUNT = listMaterialIn.Where(p => p.MATERIAL_TYPE_ID == Group.First().MATERIAL_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL).Sum(o => o.AMOUNT);

                            }

                        }

                        else if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DDT)
                        {
                            if (IsNotNullOrEmpty(listMaterialIn))
                            {

                                ADO.RETURN_AMOUNT = listMaterialIn.Where(p => p.MATERIAL_TYPE_ID == Group.First().MATERIAL_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DNTTL).Sum(o => o.AMOUNT);

                            }
                        }

                        else if (ADO.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK)
                        {
                            if (IsNotNullOrEmpty(listMaterialIn))
                            {

                                ADO.RETURN_AMOUNT = listMaterialIn.Where(p => p.MATERIAL_TYPE_ID == Group.First().MATERIAL_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__DONKTL).Sum(o => o.AMOUNT);

                            }

                        }
                        else
                        {
                            ADO.RETURN_AMOUNT = 0;
                            ADO.RETURN_PRICE = 0;
                        }
                        if (dicExtInfo.ContainsKey("EXP_" + Group.First().EXP_MEST_CODE))
                        {
                            ADO.CASHIER_LOGINNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].CASHIER_LOGINNAME;
                            ADO.CASHIER_USERNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].CASHIER_USERNAME;
                            ADO.REQUEST_LOGINNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_LOGINNAME;
                            ADO.REQUEST_USERNAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_USERNAME;
                            ADO.REQUEST_DEPARTMENT_ID = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_ID;
                            ADO.REQUEST_DEPARTMENT_CODE = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_CODE;
                            ADO.REQUEST_DEPARTMENT_NAME = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].REQUEST_DEPARTMENT_NAME;
                            ADO.EXEMPTION = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].EXEMPTION;
                            ADO.EXP_MEST_CODE = dicExtInfo["EXP_" + Group.First().EXP_MEST_CODE].EXP_MEST_CODE;
                        }
                        ListRdo.Add(ADO);
                    }
                    var time = listMaterialOut.GroupBy(o => new { o.MATERIAL_TYPE_ID, o.EXP_DATE });
                    foreach (var item in time)
                    {
                        Mrs00297RDO rdo = new Mrs00297RDO();
                        rdo.EXP_TIME = item.First().EXP_TIME;
                        rdo.EXP_TIME_STR = Inventec.Common.DateTime.Convert.TimeNumberToDateString(item.First().EXP_TIME ?? 0);
                        rdo.EXP_MEST_TYPE_ID = item.First().EXP_MEST_TYPE_ID;
                        rdo.TOTAL_PRICE = item.First().PRICE * (1 + item.First().VAT_RATIO) * item.Sum(p => p.AMOUNT);
                        rdo.TOTAL_IMP_PRICE = item.First().IMP_PRICE * (1 + item.First().IMP_VAT_RATIO) * item.Sum(p => p.AMOUNT);
                        rdo.INTEREST_PRICE = rdo.TOTAL_PRICE ?? 0 - rdo.TOTAL_IMP_PRICE ?? 0;
                        rdo.EXP_MEST_CODE = item.First().EXP_MEST_CODE;

                        if (item.First().EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN)
                        {
                            if (IsNotNullOrEmpty(listMaterialIn))
                            {
                                var returnAmount = listMaterialIn.FirstOrDefault(p => p.MATERIAL_TYPE_ID == item.First().MATERIAL_TYPE_ID && p.IMP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL);
                                if (returnAmount != null)
                                {
                                    rdo.MEDICINE_TYPE_NAME = item.First().MATERIAL_TYPE_NAME;
                                    rdo.IMP_MEST_CODE = returnAmount.IMP_MEST_CODE;
                                    rdo.RETURN_AMOUNT = returnAmount.AMOUNT;
                                    rdo.IMP_VAT_RATIO = returnAmount.IMP_VAT_RATIO;
                                    rdo.IMP_PRICE = returnAmount.IMP_PRICE;
                                }
                            }
                        }
                        listExport1.Add(rdo);
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void CreateTotal()
        {
            try
            {
                if (IsNotNullOrEmpty(listImportRdo))
                {
                    var Groups = listImportRdo.GroupBy(o => o.TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00297RDO>();
                        Mrs00297RDO rdo = new Mrs00297RDO();
                        rdo.TYPE_ID = listSub.First().TYPE_ID;
                        rdo.TYPE_NAME = listSub.First().TYPE_NAME;

                        rdo.TYPE_TOTAL_IMP_PRICE = listSub.Sum(s => s.TOTAL_IMP_PRICE);

                        rdo.TYPE_TOTAL_PRICE_1 = listSub.Sum(s => s.TOTAL_PRICE_1);
                        rdo.TYPE_TOTAL_1 = rdo.TYPE_TOTAL_IMP_PRICE - rdo.TYPE_TOTAL_PRICE_1;
                        listImport.Add(rdo);
                    }
                    if (IsNotNullOrEmpty(listImport))
                    {
                        Mrs00297RDO total = new Mrs00297RDO();
                        total.TYPE_TOTAL_1 = listImport.Sum(o => o.TYPE_TOTAL_1 ?? 0);
                        total.TOTAL_TYPE_IMP_PRICE = listImport.Sum(o => o.TYPE_TOTAL_IMP_PRICE ?? 0);

                        total.TYPE_TOTAL_PRICE_1 = listImport.Sum(s => s.TYPE_TOTAL_PRICE_1);
                        TotalImport.Add(total);
                    }
                }

                if (IsNotNullOrEmpty(ListRdo))
                {
                    var Groups = ListRdo.GroupBy(o => o.TYPE_ID).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00297RDO>();
                        Mrs00297RDO rdo = new Mrs00297RDO();

                        rdo.TYPE_ID = listSub.First().TYPE_ID;
                        rdo.TYPE_NAME = listSub.First().TYPE_NAME;
                        rdo.TYPE_TOTAL_IMP_PRICE = listSub.Sum(s => s.TOTAL_IMP_PRICE);
                        rdo.TYPE_TOTAL_PRICE = listSub.Sum(o => o.TOTAL_PRICE);
                        rdo.TYPE_TOTAL = rdo.TYPE_TOTAL_PRICE - rdo.TYPE_TOTAL_IMP_PRICE;
                        listExport.Add(rdo);
                    }
                    if (IsNotNullOrEmpty(listExport))
                    {
                        Mrs00297RDO total = new Mrs00297RDO();
                        total.TOTAL_TYPE = listExport.Sum(o => o.TYPE_TOTAL ?? 0);
                        total.TOTAL_TYPE_IMP_PRICE = listExport.Sum(o => o.TYPE_TOTAL_IMP_PRICE ?? 0);
                        total.TOTAL_TYPE_PRICE = listExport.Sum(o => o.TYPE_TOTAL_PRICE ?? 0);
                        TotalExport.Add(total);
                    }
                }
                LogSystem.Info("COUNT: " + listExport1.Count());
                if (IsNotNullOrEmpty(listExport1))
                {
                    var Groups = listExport1.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN).GroupBy(o => o.EXP_TIME_STR).ToList();
                    foreach (var group in Groups)
                    {
                        var listSub = group.ToList<Mrs00297RDO>();
                        Mrs00297RDO rdo = new Mrs00297RDO();

                        rdo.EXP_TIME = listSub.First().EXP_TIME;
                        rdo.EXP_TIME_STR = listSub.First().EXP_TIME_STR;
                        rdo.TOTAL_IMP_PRICE = listSub.Sum(s => s.TOTAL_IMP_PRICE);
                        rdo.TOTAL_PRICE = listSub.Sum(o => o.TOTAL_PRICE);
                        rdo.INTEREST_PRICE = listSub.Sum(o => o.INTEREST_PRICE);
                        TotalExport1.Add(rdo);
                    }

                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }



        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (Filter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(Filter.TIME_FROM));
                }
                if (Filter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToDateString(Filter.TIME_TO));
                }

                if (Filter.MEDI_STOCK_ID.HasValue)
                {
                    dicSingleTag.Add("MEDI_STOCK_NAME", HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == Filter.MEDI_STOCK_ID).MEDI_STOCK_NAME);
                }
                if (Filter.BRANCH_ID.HasValue)
                {
                    dicSingleTag.Add("BRANCH_NAME", HisBranchCFG.HisBranchs.FirstOrDefault(o => o.ID == Filter.BRANCH_ID).BRANCH_NAME);
                }
                bool exportSuccess = true;
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Export", listExport);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ExMestList", ListRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Export", "ExMestList", "TYPE_ID", "TYPE_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TotalExport", TotalExport);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "MedicineGroups", ListRdo.GroupBy(o => o.PARENT_MEDICINE_TYPE_CODE).Select(p => p.First()).ToList());

                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "Import", listImport);
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "ImMestList", listImportRdo);
                exportSuccess = exportSuccess && objectTag.AddRelationship(store, "Import", "ImMestList", "TYPE_ID", "TYPE_ID");
                exportSuccess = exportSuccess && objectTag.AddObjectData(store, "TotalImport", TotalImport);
                exportSuccess = exportSuccess && store.SetCommonFunctions();

                objectTag.AddObjectData(store, "MedicineExport", ListRdo);
                objectTag.AddObjectData(store, "ExMestTypeExport", ListRdo.GroupBy(p => p.EXP_MEST_TYPE_CODE).Select(q => q.First()).ToList());
                objectTag.AddRelationship(store, "ExMestTypeExport", "MedicineExport", "EXP_MEST_TYPE_CODE", "EXP_MEST_TYPE_CODE");

                objectTag.AddObjectData(store, "Export1", listExport1.Where(p => p.EXP_MEST_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__BAN && p.RETURN_AMOUNT > 0).OrderBy(p => p.EXP_TIME).ToList());
                objectTag.AddObjectData(store, "TotalExport1", TotalExport1.OrderBy(p => p.EXP_TIME).ToList());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
