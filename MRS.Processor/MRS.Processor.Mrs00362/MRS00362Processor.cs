using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MRS.MANAGER.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisMedicineType;
using MOS.MANAGER.HisMaterialType;
using MOS.MANAGER.HisExpMestReason;
using FlexCel.Report;

namespace MRS.Processor.MRS00362
{
    class Mrs00362Processor : AbstractProcessor
    {
        private List<Mrs00362RDO> listMrs00362Rdos = new List<Mrs00362RDO>();
        private List<Mrs00362RDO> listExpMestss = new List<Mrs00362RDO>();
        Mrs00362Filter castFilter = null;

        List<V_HIS_EXP_MEST> listExpMests = new List<V_HIS_EXP_MEST>();
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterials = new List<V_HIS_EXP_MEST_MATERIAL>();
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicines = new List<V_HIS_EXP_MEST_MEDICINE>();
        List<HIS_IMP_MEST> listMobaImpMests = new List<HIS_IMP_MEST>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines = new List<V_HIS_IMP_MEST_MEDICINE>();
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterials = new List<V_HIS_IMP_MEST_MATERIAL>();
        List<V_HIS_MEDICINE_TYPE> ListMedicineType = new List<V_HIS_MEDICINE_TYPE>();
        List<V_HIS_MATERIAL_TYPE> ListMaterialType = new List<V_HIS_MATERIAL_TYPE>();
        List<HIS_EXP_MEST_REASON> ListReason = new List<HIS_EXP_MEST_REASON>();
        Dictionary<long, V_HIS_EXP_MEST> dicExpMest = new Dictionary<long, V_HIS_EXP_MEST>();

        public Mrs00362Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {

        }

        //List<Mrs00362RDO> listRdo = new List<Mrs00362RDO>(); 
        public override Type FilterType()
        {
            return typeof(Mrs00362Filter);
        }

        protected override bool GetData()
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00362Filter)this.reportFilter;
                var listExpMestId = new List<long>();

                var menuExpMestMedicineViewFilter = new HisExpMestMedicineViewFilterQuery()
                {
                    EXP_TIME_FROM = castFilter.TIME_FROM,
                    EXP_TIME_TO = castFilter.TIME_TO,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC,
                    MEDI_STOCK_IDs = castFilter.MEDI_BIG_STOCK_IDs,
                    EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    IS_EXPORT = true
                };
                listExpMestMedicines = new HisExpMestMedicineManager(param).GetView(menuExpMestMedicineViewFilter);
                if (listExpMestMedicines != null)
                {
                    listExpMestId.AddRange(listExpMestMedicines.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                
                var menuExpMestMaterialViewFilter = new HisExpMestMaterialViewFilterQuery()
                {
                    EXP_TIME_FROM = castFilter.TIME_FROM,
                    EXP_TIME_TO = castFilter.TIME_TO,
                    EXP_MEST_TYPE_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__KHAC,
                    MEDI_STOCK_IDs = castFilter.MEDI_BIG_STOCK_IDs,
                    EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    IS_EXPORT = true
                };
                listExpMestMaterials = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(param).GetView(menuExpMestMaterialViewFilter);
                if (listExpMestMaterials != null)
                {
                    listExpMestId.AddRange(listExpMestMaterials.Select(o => o.EXP_MEST_ID ?? 0).ToList());
                }
                //--------------------------------------------------------------------------------------------------HIS_MOBA_IMP_MEST - HIS_EXP_MEST
                listExpMestId = listExpMestId.Distinct().ToList();
                var skip = 0;
                while (listExpMestId.Count() - skip > 0)
                {
                    var ListDSs = listExpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var menuImpMestFilter = new HisImpMestFilterQuery()
                    {
                        MOBA_EXP_MEST_IDs = ListDSs,
                        IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT,
                    };
                    var listMobaImpMest = new HisImpMestManager(param).Get(menuImpMestFilter);
                    if (listMobaImpMest != null)
                    {
                        listMobaImpMests.AddRange(listMobaImpMest);
                    }

                    var menuExpMestFilter = new HisExpMestViewFilterQuery()
                    {
                        IDs = ListDSs,
                        EXP_MEST_REASON_IDs = castFilter.EXP_MEST_REASON_IDs
                    };
                    var listExpMest = new HisExpMestManager(param).GetView(menuExpMestFilter);
                    if (listExpMest != null)
                    {
                        listExpMests.AddRange(listExpMest);
                    }
                }

                //--------------------------------------------------------------------------------------------------V_IMP_MEST_MEDICINE - V_IMP_MEST_MATERIAL
                var mobaImpMestId = listMobaImpMests.Select(o => o.ID).Distinct().ToList();
                skip = 0;
                while (mobaImpMestId.Count() - skip > 0)
                {
                    var ListDSs = mobaImpMestId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var menuImpMestMedicineViewFilter = new HisImpMestMedicineViewFilterQuery()
                    {
                        IMP_MEST_IDs = ListDSs,
                        IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT,
                    };
                    var listImpMestMedicine = new HisImpMestMedicineManager(param).GetView(menuImpMestMedicineViewFilter);
                    listImpMestMedicines.AddRange(listImpMestMedicine);
                    var menuImpMestMaterialViewFilter = new HisImpMestMaterialViewFilterQuery()
                    {
                        IMP_MEST_IDs = ListDSs,
                        IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT,
                    };
                    var listImpMestMaterial = new HisImpMestMaterialManager(param).GetView(menuImpMestMaterialViewFilter);
                    listImpMestMaterials.AddRange(listImpMestMaterial);
                }
                ListMedicineType = new HisMedicineTypeManager().GetView(new HisMedicineTypeViewFilterQuery());
                ListMaterialType = new HisMaterialTypeManager().GetView(new HisMaterialTypeViewFilterQuery());
                ListReason = new HisExpMestReasonManager().Get(new HisExpMestReasonFilterQuery());
                dicExpMest = listExpMests.GroupBy(o => o.ID).ToDictionary(p => p.Key, q => q.First());
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
                if (castFilter.EXP_MEST_REASON_IDs != null)
                {
                    listExpMestMedicines = listExpMestMedicines.Where(q => q.MEDICINE_ID.HasValue).Where(o => listExpMests.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                    listExpMestMaterials = listExpMestMaterials.Where(q => q.MATERIAL_ID.HasValue).Where(o => listExpMests.Exists(p => p.ID == o.EXP_MEST_ID)).ToList();
                }
                //khi có điều kiện lọc từ template thì đổi sang key từ template
                string mainKeyGroup = "{0}";
                if (this.dicDataFilter.ContainsKey("KEY_GROUP_REASON") && this.dicDataFilter["KEY_GROUP_REASON"] != null && !string.IsNullOrWhiteSpace(this.dicDataFilter["KEY_GROUP_REASON"].ToString()))
                {
                    mainKeyGroup = this.dicDataFilter["KEY_GROUP_REASON"].ToString();
                }
                var groupByMedicine = listExpMestMedicines.GroupBy(s => string.Format(mainKeyGroup, s.MEDICINE_ID, s.EXP_MEST_CODE, dicExpMest.ContainsKey(s.EXP_MEST_ID??0)?dicExpMest[s.EXP_MEST_ID??0].EXP_MEST_REASON_CODE:"")).ToList();
                foreach (var item in groupByMedicine)
                {
                    Mrs00362RDO rdo = new Mrs00362RDO();
                    var listMobaImpmest = listMobaImpMests.Where(s => s.MOBA_EXP_MEST_ID == item.First().EXP_MEST_ID).ToList();
                    var listImpMestMedicine = listMobaImpmest != null ? listImpMestMedicines.Where(s => listMobaImpmest.Exists(p => p.ID == s.IMP_MEST_ID) && s.MEDICINE_ID == item.First().MEDICINE_ID) : null;
                    rdo.TYPE = 1;
                    rdo.MEMA_ID = item.First().MEDICINE_ID ?? 0;
                    rdo.EXPIRED_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.First().EXPIRED_DATE??0);
                    rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                    rdo.SERVICE_ID = item.First().SERVICE_ID;
                    rdo.EXP_MEST_CODE = item.First().EXP_MEST_CODE;
                    rdo.SERVICE_CODE = item.First().MEDICINE_TYPE_CODE;
                    rdo.SERVICE_NAME = item.First().MEDICINE_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.CONCENTRA = item.First().CONCENTRA;
                    rdo.MANUFACTURER = item.First().MANUFACTURER_NAME;
                    rdo.NATIONAL_NAME = item.First().NATIONAL_NAME;
                    rdo.EXP_MEST_REASON_NAME = dicExpMest.ContainsKey(item.First().EXP_MEST_ID ?? 0) ? dicExpMest[item.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_NAME : "";

                    if (listImpMestMedicine != null)
                    {
                        rdo.AMOUNT = item.Sum(s => s.AMOUNT) - listImpMestMedicine.Sum(s => s.AMOUNT);
                    }
                    else
                    {
                        rdo.AMOUNT = item.Sum(s => s.AMOUNT);
                    }
                    var price = item.FirstOrDefault(o => o.PRICE.HasValue);
                    rdo.PRICE = price != null ? (price.PRICE ?? 0) * (1 + (item.First().VAT_RATIO ?? 0)) : (item.First().IMP_PRICE * (1 + item.First().IMP_VAT_RATIO));
                    rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                    rdo.EXP_TIME = item.First().EXP_TIME;
                    rdo.CREAT_TIME = item.First().CREATE_TIME;
                    ProcessExpMestReason(item.ToList(), ref rdo);

                    listMrs00362Rdos.Add(rdo);
                }
                
                var groupByMaterial = listExpMestMaterials.GroupBy(s => string.Format(mainKeyGroup, s.MATERIAL_ID, s.EXP_MEST_CODE, dicExpMest.ContainsKey(s.EXP_MEST_ID??0)?dicExpMest[s.EXP_MEST_ID??0].EXP_MEST_REASON_CODE:"")).ToList();
                foreach (var item in groupByMaterial)
                {
                    Mrs00362RDO rdo = new Mrs00362RDO();
                    var listMobaImpmest = listMobaImpMests.Where(s => s.MOBA_EXP_MEST_ID == item.First().EXP_MEST_ID).ToList();
                    var listImpMestMaterial = listMobaImpmest != null ? listImpMestMaterials.Where(s => listMobaImpmest.Exists(p => p.ID == s.IMP_MEST_ID) && s.MATERIAL_ID == item.First().MATERIAL_ID) : null;
                    rdo.TYPE = 2;
                    rdo.MEMA_ID = item.First().MATERIAL_ID ?? 0;
                    rdo.EXPIRED_DATE = Inventec.Common.DateTime.Convert.TimeNumberToTimeString(item.First().EXPIRED_DATE ?? 0);
                    rdo.PACKAGE_NUMBER = item.First().PACKAGE_NUMBER;
                    rdo.SERVICE_ID = item.First().SERVICE_ID;
                    rdo.EXP_MEST_CODE = item.First().EXP_MEST_CODE;
                    rdo.SERVICE_CODE = item.First().MATERIAL_TYPE_CODE;
                    rdo.SERVICE_NAME = item.First().MATERIAL_TYPE_NAME;
                    rdo.SERVICE_UNIT_NAME = item.First().SERVICE_UNIT_NAME;
                    rdo.MANUFACTURER = item.First().MANUFACTURER_NAME;
                    rdo.NATIONAL_NAME = item.First().NATIONAL_NAME;
                    var maty = ListMaterialType.Where(x => x.ID == item.First().MATERIAL_TYPE_ID).FirstOrDefault();
                    var expMest = listExpMests.Where(x => x.ID == item.First().EXP_MEST_ID).FirstOrDefault();

                    rdo.EXP_MEST_REASON_NAME = dicExpMest.ContainsKey(item.First().EXP_MEST_ID ?? 0) ? dicExpMest[item.First().EXP_MEST_ID ?? 0].EXP_MEST_REASON_NAME : "";

                    if (listImpMestMaterial != null)
                    {
                        rdo.AMOUNT = item.Sum(s => s.AMOUNT) - listImpMestMaterial.Sum(s => s.AMOUNT);
                    }
                    else
                    {
                        rdo.AMOUNT = item.Sum(s => s.AMOUNT);
                    }
                    var price = item.FirstOrDefault(o => o.PRICE.HasValue);
                    rdo.PRICE = price != null ? (price.PRICE ?? 0) * (1 + (item.First().VAT_RATIO ?? 0)) : (item.First().IMP_PRICE * (1 + item.First().IMP_VAT_RATIO));
                    rdo.TOTAL_PRICE = rdo.PRICE * rdo.AMOUNT;
                    rdo.EXP_TIME = item.First().EXP_TIME;
                    rdo.CREAT_TIME = item.First().CREATE_TIME;
                    ProcessExpMestReason(item.ToList(), ref rdo);
                    listMrs00362Rdos.Add(rdo);
                }

                AddInfoGroup(listMrs00362Rdos);
                listExpMestss = listMrs00362Rdos.GroupBy(g => g.EXP_MEST_CODE).Select(s => new Mrs00362RDO
                {
                    EXP_MEST_CODE = s.First().EXP_MEST_CODE,
                    CREAT_TIME = s.First().CREAT_TIME,
                    EXP_TIME = s.First().EXP_TIME
                }).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessExpMestReason(List<V_HIS_EXP_MEST_MATERIAL> list, ref Mrs00362RDO rdo)
        {
            try
            {
                if (IsNotNullOrEmpty(list) && IsNotNullOrEmpty(ListReason))
                {
                    var lisExp = listExpMests.Where(o => list.Select(s => s.EXP_MEST_ID).Contains(o.ID)).ToList();
                    var grReason = lisExp.GroupBy(o => o.EXP_MEST_REASON_ID ?? 0).ToList();
                    foreach (var gr in grReason)
                    {
                        var expMedicine = list.Where(o => gr.Select(s => s.ID).Contains(o.EXP_MEST_ID ?? 0)).ToList();
                        var reason = ListReason.FirstOrDefault(o => o.ID == gr.Key);
                        //if (rs != null)
                        //{
                        //    rdo.EXP_MEST_REASON_NAME = rs.EXP_MEST_REASON_NAME;
                        //}
                        if (reason != null)
                        {
                            rdo.EXP_MEST_REASON_NAME = reason.EXP_MEST_REASON_NAME;
                            rdo.DIC_REASON[reason.EXP_MEST_REASON_CODE] = expMedicine.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0));
                        }
                        else
                        {
                            rdo.DIC_REASON["00"] = expMedicine.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessExpMestReason(List<V_HIS_EXP_MEST_MEDICINE> list, ref Mrs00362RDO rdo)
        {
            try
            {
                if (IsNotNullOrEmpty(list) && IsNotNullOrEmpty(ListReason))
                {
                    var lisExp = listExpMests.Where(o => list.Select(s => s.EXP_MEST_ID).Contains(o.ID)).ToList();
                    var grReason = lisExp.GroupBy(o => o.EXP_MEST_REASON_ID ?? 0).ToList();
                    foreach (var gr in grReason)
                    {
                        var expMedicine = list.Where(o => gr.Select(s => s.ID).Contains(o.EXP_MEST_ID ?? 0)).ToList();
                        //var rs = ListReason.Where(x => x.ID == gr.First().EXP_MEST_REASON_ID).FirstOrDefault();
                        var reason = ListReason.FirstOrDefault(o => o.ID == gr.Key);
                        //if (rs!=null)
                        //{
                        //    rdo.EXP_MEST_REASON_NAME = rs.EXP_MEST_REASON_NAME;
                        //}
                        if (reason != null)
                        {
                            rdo.EXP_MEST_REASON_NAME = reason.EXP_MEST_REASON_NAME;
                            rdo.DIC_REASON[reason.EXP_MEST_REASON_CODE] = expMedicine.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0));
                        }
                        else
                        {
                            rdo.DIC_REASON["00"] = expMedicine.Sum(s => s.AMOUNT - (s.TH_AMOUNT ?? 0));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void AddInfoGroup(List<Mrs00362RDO> ListRdo)
        {
            foreach (var item in ListRdo)
            {
                var medicineType = ListMedicineType.FirstOrDefault(o => o.SERVICE_ID == item.SERVICE_ID);
                if (item.TYPE == 1)
                {
                    if (medicineType != null && medicineType.MEDICINE_GROUP_ID.HasValue)
                    {
                        item.MEDICINE_GROUP_ID = medicineType.MEDICINE_GROUP_ID.Value;
                        item.MEDICINE_GROUP_CODE = medicineType.MEDICINE_GROUP_CODE;
                        item.MEDICINE_GROUP_NAME = medicineType.MEDICINE_GROUP_NAME;
                    }
                    else
                    {
                        item.MEDICINE_GROUP_ID = 0;
                        item.MEDICINE_GROUP_CODE = "NTK";
                        item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                    }
                    if(castFilter.IS_GROUP_TO_PARENT==true)
                    {
                        if (medicineType != null && medicineType.PARENT_ID.HasValue)
                        {
                            var parent = ListMedicineType.FirstOrDefault(o=>o.ID== medicineType.PARENT_ID);
                            if(parent!=null)
                            {
                                item.MEDICINE_GROUP_ID = parent.ID;
                                item.MEDICINE_GROUP_CODE =parent.MEDICINE_TYPE_CODE;
                                item.MEDICINE_GROUP_NAME = parent.MEDICINE_TYPE_NAME;

                            }
                            else
                            {
                                item.MEDICINE_GROUP_ID = 0;
                                item.MEDICINE_GROUP_CODE = "NTK";
                                item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                            }
                        }
                        else
                        {
                            item.MEDICINE_GROUP_ID = 0;
                            item.MEDICINE_GROUP_CODE = "NTK";
                            item.MEDICINE_GROUP_NAME = "Nhóm thuốc khác";
                        }
                    }    
                }
                if (item.TYPE == 2)
                {
                    item.MEDICINE_GROUP_ID = -1;
                    item.MEDICINE_GROUP_CODE = "DVT";
                    item.MEDICINE_GROUP_NAME = "Vật tư";
                }
            }
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                dicSingleTag.Add("TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                dicSingleTag.Add("TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                if (castFilter.MEDI_BIG_STOCK_IDs != null)
                {
                    dicSingleTag.Add("MEDI_STOCK_IDs", string.Join(", ", HisMediStockCFG.HisMediStocks.Where(o => castFilter.MEDI_BIG_STOCK_IDs.Contains(o.ID)).Select(p => p.MEDI_STOCK_NAME).ToList()));
                }

                if (castFilter.EXP_MEST_REASON_IDs != null)
                {
                    dicSingleTag.Add("EXP_MEST_REASON_NAME", string.Join(", ", (new HisExpMestReasonManager().Get(new HisExpMestReasonFilterQuery() { IDs = castFilter.EXP_MEST_REASON_IDs }) ?? new List<HIS_EXP_MEST_REASON>()).Select(p => p.EXP_MEST_REASON_NAME).ToList()));
                }

                objectTag.AddObjectData(store, "Parent", listMrs00362Rdos.OrderBy(q => q.TYPE).GroupBy(o => o.MEDICINE_GROUP_ID).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "Report1", listExpMestss);
                objectTag.AddObjectData(store, "Report", listMrs00362Rdos.OrderBy(o=>o.TYPE).ThenBy(p=>p.SERVICE_NAME).ToList());
                objectTag.AddRelationship(store, "Report1", "Report", "EXP_MEST_CODE", "EXP_MEST_CODE");
                objectTag.AddRelationship(store, "Parent", "Report", "MEDICINE_GROUP_ID", "MEDICINE_GROUP_ID");
                objectTag.AddObjectData(store, "ExpMestMedicine", listExpMestMedicines.OrderBy(o=>o.MEDICINE_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ExpMestMaterial", listExpMestMaterials.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList());
                objectTag.AddObjectData(store, "ExpMest", listExpMests);
                objectTag.AddRelationship(store, "ExpMest", "ExpMestMedicine", "EXP_MEST_CODE", "EXP_MEST_CODE");
                objectTag.AddRelationship(store, "ExpMest", "ExpMestMaterial", "EXP_MEST_CODE", "EXP_MEST_CODE");

                objectTag.AddObjectData(store, "listService", listMrs00362Rdos.OrderBy(q => q.TYPE).GroupBy(o => new { o.SERVICE_ID, o.TYPE }).Select(p => p.First()).ToList());
                objectTag.AddObjectData(store, "listPackage", listMrs00362Rdos.Where(o => listMrs00362Rdos.Exists(p => p.TYPE == o.TYPE && p.MEMA_ID != o.MEMA_ID && o.SERVICE_ID == p.SERVICE_ID)).ToList());
                objectTag.AddRelationship(store, "listService", "listPackage", new string[] { "SERVICE_ID", "TYPE" }, new string[] { "SERVICE_ID", "TYPE" });

                objectTag.SetUserFunction(store, "Element", new RDOElement());
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }

    public class RDOElement : TFlexCelUserFunction
    {
        object result = null;
        public override object Evaluate(object[] parameters)
        {
            if (parameters == null || parameters.Length < 2)
                throw new ArgumentException("Bad parameter count in call to Orders() user-defined function");

            try
            {
                string KeyGet = Convert.ToString(parameters[1]);
                if (parameters[0] is Dictionary<string, int>)
                {
                    Dictionary<string, int> DicGet = parameters[0] as Dictionary<string, int>;

                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return null;
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, long>)
                {
                    Dictionary<string, long> DicGet = parameters[0] as Dictionary<string, long>;

                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return null;
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, decimal>)
                {
                    Dictionary<string, decimal> DicGet = parameters[0] as Dictionary<string, decimal>;

                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return null;
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else if (parameters[0] is Dictionary<string, string>)
                {
                    Dictionary<string, string> DicGet = parameters[0] as Dictionary<string, string>;

                    if (!DicGet.ContainsKey(KeyGet))
                    {
                        return null;
                        Inventec.Common.Logging.LogSystem.Info("Not exists key: " + KeyGet);
                    }
                    result = DicGet[KeyGet];
                }
                else
                {
                    result = null;
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                return null;
            }

            return result;
        }
    }
}
