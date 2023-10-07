using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Config;
using MRS.Processor.Mrs00389;
using Inventec.Common.FlexCellExport;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisReportTypeCat;
using Inventec.Common.Logging;
using MOS.MANAGER.HisExpMest;

namespace MRS.Processor.Mrs00389
{
    public class Mrs00389Processor : AbstractProcessor
    {
        CommonParam paramGet = new CommonParam();
        List<Mrs00389RDO> ListRdo = new List<Mrs00389RDO>();
        List<Mrs00389RDO> ListRdoParent = new List<Mrs00389RDO>();

        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<long> aggrExpMestId = new List<long>();
        public Mrs00389Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        public override Type FilterType()
        {
            return typeof(Mrs00389Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {

                // Get bảng V_HIST_EXP_MEST_MEDICINE

                var ExpMestMedicineFilter = new HisExpMestMedicineViewFilterQuery
                {
                    EXP_TIME_FROM = ((Mrs00389Filter)this.reportFilter).TIME_FROM,
                    EXP_TIME_TO = ((Mrs00389Filter)this.reportFilter).TIME_TO,
                    MEDI_STOCK_IDs = ((Mrs00389Filter)this.reportFilter).MEDI_STOCK_IDs,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                    IS_EXPORT = true,
                    REQ_DEPARTMENT_IDs = ((Mrs00389Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs,
                    REQ_ROOM_IDs = ((Mrs00389Filter)this.reportFilter).REQ_ROOM_IDs
                };
                listExpMestMedicine = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicineFilter);
                if (listExpMestMedicine != null)
                    aggrExpMestId.AddRange(listExpMestMedicine.Select(p => p.AGGR_EXP_MEST_ID??p.EXP_MEST_ID??0).Distinct().ToList());
                // Get bảng V_HIST_EXP_MEST_MATERIAL

                var ExpMestMaterialFilter = new HisExpMestMaterialViewFilterQuery
                {
                    EXP_TIME_FROM = ((Mrs00389Filter)this.reportFilter).TIME_FROM,
                    EXP_TIME_TO = ((Mrs00389Filter)this.reportFilter).TIME_TO,
                    MEDI_STOCK_IDs = ((Mrs00389Filter)this.reportFilter).MEDI_STOCK_IDs,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                    IS_EXPORT = true,
                    REQ_DEPARTMENT_IDs = ((Mrs00389Filter)this.reportFilter).REQUEST_DEPARTMENT_IDs,
                    REQ_ROOM_IDs = ((Mrs00389Filter)this.reportFilter).REQ_ROOM_IDs
                };
                listExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialFilter);
                if (listExpMestMaterial != null)
                    aggrExpMestId.AddRange(listExpMestMaterial.Select(p => p.AGGR_EXP_MEST_ID ?? p.EXP_MEST_ID ?? 0).Distinct().ToList());
                aggrExpMestId = aggrExpMestId.Distinct().ToList();
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);

                result = false;
            }
            return result;
        }

        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                ListRdo.Clear();
                //khi có điều kiện lọc từ template thì đổi sang key từ template
                string mainKeyGroup = "{0}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_EXP") && this.dicDataFilter["KEY_GROUP_EXP"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_EXP"].ToString()))
                {
                    mainKeyGroup = this.dicDataFilter["KEY_GROUP_EXP"].ToString();
                }
                if (IsNotNullOrEmpty(listExpMestMedicine))
                {
                    var listMedicineGroupbyMedicineID = listExpMestMedicine.GroupBy(o => string.Format(mainKeyGroup, o.MEDICINE_ID,o.REQ_DEPARTMENT_ID)).ToList();
                    foreach (var medicine in listMedicineGroupbyMedicineID)
                    {
                        var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == medicine.First().REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                        var roomIds = medicine.Select(o => o.REQ_ROOM_ID).Distinct().ToList();
                        var reqRoom = HisRoomCFG.HisRooms.Where(o => roomIds.Contains(o.ID)).ToList() ?? new List<V_HIS_ROOM>();
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == medicine.First().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK();
                        Mrs00389RDO rdo = new Mrs00389RDO();
                        rdo.SERVICE_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                        rdo.REQ_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                        rdo.REQ_ROOM_NAME = string.Join(", ",reqRoom.Select(o=>o.ROOM_NAME).ToList());
                        rdo.TYPE = "THUỐC";
                        rdo.SERVICE_ID = medicine.First().MEDICINE_ID ?? 0;
                        rdo.SERVICE_TYPE_CODE = medicine.First().MEDICINE_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = medicine.First().MEDICINE_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = medicine.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = medicine.First().PACKAGE_NUMBER;
                        rdo.AMOUNT = medicine.Sum(p => p.AMOUNT);
                        rdo.IMP_PRICE = medicine.First().IMP_PRICE * (1 + medicine.First().IMP_VAT_RATIO);
                        rdo.TOTAL_IMP_PRICE = rdo.IMP_PRICE * rdo.AMOUNT;
                        rdo.CONCENTRA = medicine.First().CONCENTRA;
                        ListRdo.Add(rdo);

                    }
                }

                if (IsNotNullOrEmpty(listExpMestMaterial))
                {
                    var listMaterialGroupbyMaterialID = listExpMestMaterial.GroupBy(o => string.Format(mainKeyGroup, o.MATERIAL_ID, o.REQ_DEPARTMENT_ID)).ToList();
                    foreach (var Material in listMaterialGroupbyMaterialID)
                    {
                        var reqDepartment = HisDepartmentCFG.DEPARTMENTs.FirstOrDefault(o => o.ID == Material.First().REQ_DEPARTMENT_ID) ?? new HIS_DEPARTMENT();
                        var roomIds = Material.Select(o => o.REQ_ROOM_ID).Distinct().ToList();
                        var reqRoom = HisRoomCFG.HisRooms.Where(o => roomIds.Contains(o.ID)).ToList() ?? new List<V_HIS_ROOM>();
                        var mediStock = HisMediStockCFG.HisMediStocks.FirstOrDefault(o => o.ID == Material.First().MEDI_STOCK_ID) ?? new V_HIS_MEDI_STOCK();
                        Mrs00389RDO rdo = new Mrs00389RDO();
                        rdo.SERVICE_STOCK_NAME = mediStock.MEDI_STOCK_NAME;
                        rdo.REQ_DEPARTMENT_NAME = reqDepartment.DEPARTMENT_NAME;
                        rdo.REQ_ROOM_NAME = string.Join(", ", reqRoom.Select(o => o.ROOM_NAME).ToList());
                        rdo.TYPE = "VẬT TƯ";
                        rdo.SERVICE_ID = Material.First().MATERIAL_ID ?? 0;
                        rdo.SERVICE_TYPE_CODE = Material.First().MATERIAL_TYPE_CODE;
                        rdo.SERVICE_TYPE_NAME = Material.First().MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = Material.First().SERVICE_UNIT_NAME;
                        rdo.PACKAGE_NUMBER = Material.First().PACKAGE_NUMBER;
                        rdo.AMOUNT = Material.Sum(p => p.AMOUNT);
                        rdo.IMP_PRICE = Material.First().IMP_PRICE * (1 + Material.First().IMP_VAT_RATIO);
                        rdo.TOTAL_IMP_PRICE = rdo.IMP_PRICE * rdo.AMOUNT;
                        ListRdo.Add(rdo);

                    }
                }

                result = true;
                ListRdoParent = ListRdo.GroupBy(o => o.SERVICE_STOCK_NAME).Select(o => o.First()).ToList();
            }

            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                ListRdo.Clear();

            }
            return result;

        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, ProcessObjectTag objectTag, Store store)
        {
            try
            {

                dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00389Filter)this.reportFilter).TIME_FROM));
                dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(((Mrs00389Filter)this.reportFilter).TIME_TO));
                dicSingleTag.Add("COUNT_AGGR_EXP_MEST", aggrExpMestId.Count);
                objectTag.AddObjectData(store, "Report", ListRdo.OrderBy(o => o.TYPE).ThenBy(p => p.SERVICE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "Parent", ListRdoParent);
                objectTag.AddRelationship(store, "Parent", "Report", "SERVICE_STOCK_NAME", "SERVICE_STOCK_NAME");
                objectTag.AddObjectData(store, "ReqDepartment", ListRdo.GroupBy(o=>o.REQ_DEPARTMENT_NAME).Select(p=>p.First()).ToList());
                objectTag.AddRelationship(store, "ReqDepartment", "Report", "REQ_DEPARTMENT_NAME", "REQ_DEPARTMENT_NAME");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);

            }

        }
    }
}