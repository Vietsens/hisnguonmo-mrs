using FlexCel.Report;
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
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisMediStock;

namespace MRS.Processor.Mrs00306
{
    class Mrs00306Processor : AbstractProcessor
    {
        Mrs00306Filter castFilter = null;

        List<Mrs00306RDO> listRdo = new List<Mrs00306RDO>();
        List<Mrs00306RDO> listRdoParent = new List<Mrs00306RDO>();
        List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicine = new List<V_HIS_IMP_MEST_MEDICINE>();  // Ds Thuốc trả
        List<V_HIS_IMP_MEST_MATERIAL> listImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();  //Ds Vật tư trả
        List<V_HIS_EXP_MEST_MEDICINE> listExpMestMedicine = new List<V_HIS_EXP_MEST_MEDICINE>();  // Ds Thuốc xuất
        List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterial = new List<V_HIS_EXP_MEST_MATERIAL>();  // Ds vật tư xuất
        List<HIS_IMP_MEST> listMobaImpMest = new List<HIS_IMP_MEST>();  // Phiếu nhập thu hồi
        List<HIS_EXP_MEST> listSaleExpMest = new List<HIS_EXP_MEST>();  // Phiếu xuất
        List<HIS_EXP_MEST> listPrescription = new List<HIS_EXP_MEST>();  // Đơn bán

        public Mrs00306Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00306Filter);
        }

        //get dữ liệu từ data base
        protected override bool GetData()///
        {
            bool result = true;
            try
            {
                this.castFilter = (Mrs00306Filter)this.reportFilter;
                CommonParam paramGet = new CommonParam(); //CastFilter = (Mrs00157Filter)this.reportFilter; 
                //Get các phiếu nhập thu hồi:
                HisImpMestFilterQuery MobaImpMestFilter = new HisImpMestFilterQuery(); //khai báo 
                MobaImpMestFilter.IMP_TIME_FROM = castFilter.TIME_FROM;  // Lọc theo time
                MobaImpMestFilter.IMP_TIME_TO = castFilter.TIME_TO;
                MobaImpMestFilter.IMP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__TH,
                        IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_TYPE.ID__BTL
                    };
                MobaImpMestFilter.MEDI_STOCK_IDs = this.castFilter.MEDI_STOCK_BUSINESS_IDs;
                listMobaImpMest = new HisImpMestManager(paramGet).Get(MobaImpMestFilter);

                //lọc theo ng nhập
                if (castFilter.LOGINNAME != "" && castFilter.LOGINNAME != null)
                    listMobaImpMest = listMobaImpMest.Where(o => o.IMP_LOGINNAME == castFilter.LOGINNAME).ToList();

                if (IsNotNullOrEmpty(castFilter.LOGINNAMEs))
                    listMobaImpMest = listMobaImpMest.Where(o => castFilter.LOGINNAMEs.Contains(o.IMP_LOGINNAME)).ToList();
                if (IsNotNullOrEmpty(listMobaImpMest))
                {
                   
                  
                    var skip = 0;
                    var impMestIds = listMobaImpMest.Select(o => o.ID).ToList();
                    while (impMestIds.Count - skip > 0)
                    {
                        var listIDs = impMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        // get bảng các thuốc từ phiếu nhập trên
                        HisImpMestMedicineViewFilterQuery ImpMestMedicineFilter = new HisImpMestMedicineViewFilterQuery();
                        ImpMestMedicineFilter.IMP_MEST_IDs = listIDs;
                        listImpMestMedicine.AddRange(new HisImpMestMedicineManager(paramGet).GetView(ImpMestMedicineFilter) ?? new List<V_HIS_IMP_MEST_MEDICINE>());
                        // get bảng các vật tư từ phiếu nhập trên
                        HisImpMestMaterialViewFilterQuery ImpMestMaterialFilter = new HisImpMestMaterialViewFilterQuery();
                        ImpMestMaterialFilter.IMP_MEST_IDs = listIDs;
                        listImpMestMaterial.AddRange(new HisImpMestMaterialManager(paramGet).GetView(ImpMestMaterialFilter) ?? new List<V_HIS_IMP_MEST_MATERIAL>());
                    }
                }
                if (IsNotNullOrEmpty(listMobaImpMest))
                {
                    var skip = 0;
                    var expMestIds = listMobaImpMest.Select(o => o.MOBA_EXP_MEST_ID ?? 0).Distinct().ToList();
                    while (expMestIds.Count - skip > 0)
                    {
                        var listIDs = expMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                        skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                        // get bảng thuốc từ phiếu xuất trên
                        HisExpMestMedicineViewFilterQuery ExpMestMedicineFilter = new HisExpMestMedicineViewFilterQuery();
                        ExpMestMedicineFilter.EXP_MEST_IDs = listIDs;
                        ExpMestMedicineFilter.IS_EXPORT = true;
                        listExpMestMedicine.AddRange(new HisExpMestMedicineManager(paramGet).GetView(ExpMestMedicineFilter) ?? new List<V_HIS_EXP_MEST_MEDICINE>());

                        // get bảng các vật tư từ phiếu xuất trên
                        HisExpMestMaterialViewFilterQuery ExpMestMaterialFilter = new HisExpMestMaterialViewFilterQuery();
                        ExpMestMaterialFilter.EXP_MEST_IDs = listIDs;
                        ExpMestMaterialFilter.IS_EXPORT = true;
                        listExpMestMaterial.AddRange(new HisExpMestMaterialManager(paramGet).GetView(ExpMestMaterialFilter) ?? new List<V_HIS_EXP_MEST_MATERIAL>());

                        HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                        expMestFilter.IDs = listIDs;
                        var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(expMestFilter);

                        if (IsNotNullOrEmpty(expMest))
                        {
                            listSaleExpMest.AddRange(expMest);
                        }

                    }

                    if (IsNotNullOrEmpty(listSaleExpMest))
                    {
                        List<long> prescriptionId = new List<long>();
                        prescriptionId = listSaleExpMest.Select(o => o.PRESCRIPTION_ID ?? 0).Distinct().ToList();
                        if (IsNotNullOrEmpty(prescriptionId))
                        {
                            skip = 0;
                            while (prescriptionId.Count - skip > 0)
                            {
                                var listIDs = prescriptionId.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                                skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;

                                HisExpMestFilterQuery expMestFilter = new HisExpMestFilterQuery();
                                expMestFilter.SERVICE_REQ_IDs = listIDs;
                                var expMest = new MOS.MANAGER.HisExpMest.HisExpMestManager(paramGet).Get(expMestFilter);
                                if (IsNotNullOrEmpty(expMest))
                                {
                                    listPrescription.AddRange(expMest);
                                }
                            }
                        }
                    }
                    if (paramGet.HasException)
                    {
                        throw new Exception("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00306");
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

        //xwrlys dữ liệu để tạo listRdo
        protected override bool ProcessData()
        {
            bool result = false;
            try
            {
                listRdo.Clear();
                if (listImpMestMedicine != null)
                {
                    foreach (var medicine in listImpMestMedicine)//Duyệt danh sách dữ liệu thuốc 
                    {
                        Mrs00306RDO rdo = new Mrs00306RDO();
                        if (IsNotNullOrEmpty(medicine.CONCENTRA))
                        {
                            rdo.SERVICE_NAME = medicine.MEDICINE_TYPE_NAME + " (" + medicine.CONCENTRA + ")";
                        }
                        else
                        {
                            rdo.SERVICE_NAME = medicine.MEDICINE_TYPE_NAME;
                        }
                        rdo.SERVICE_UNIT_NAME = medicine.SERVICE_UNIT_NAME;
                        rdo.NATIONAL_NAME = medicine.NATIONAL_NAME;
                        rdo.CONCENTRA = medicine.CONCENTRA;
                        HIS_IMP_MEST impMest = listMobaImpMest.FirstOrDefault(o => o.ID == medicine.IMP_MEST_ID) ?? new HIS_IMP_MEST();
                        var expMestMedicines = listExpMestMedicine.Where(o => o.ID == medicine.TH_EXP_MEST_MEDICINE_ID).ToList() ?? new List<V_HIS_EXP_MEST_MEDICINE>();
                        var expMests = listSaleExpMest.Where(o => expMestMedicines.Exists(p => p.EXP_MEST_ID == o.ID)).ToList() ?? new List<HIS_EXP_MEST>();
                        var pres = listPrescription.Where(o => expMests.Exists(p => p.PRESCRIPTION_ID == o.SERVICE_REQ_ID)).ToList() ?? new List<HIS_EXP_MEST>();
                        rdo.AMOUNT = medicine.AMOUNT;
                        rdo.IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(medicine.IMP_TIME ?? 0);
                        rdo.IMP_MEST_CODE = medicine.IMP_MEST_CODE;
                        
                        if (expMestMedicines != null)
                        {
                            rdo.PRICE = System.Math.Round((expMestMedicines.First().PRICE ?? 0) * (1 + (expMestMedicines.First().VAT_RATIO ?? 0)),0);
                            rdo.EXP_VAT = expMestMedicines.First().VAT_RATIO ?? 0;
                        }
                        else
                        {
                            rdo.PRICE = System.Math.Round(medicine.IMP_PRICE * (1 + medicine.IMP_VAT_RATIO),0);
                            rdo.EXP_VAT = medicine.IMP_VAT_RATIO;
                        }
                        rdo.VAT = medicine.IMP_VAT_RATIO;
                        rdo.IMP_VAT = medicine.IMP_VAT_RATIO;
                        rdo.IMP_PRICE_B4_VAT = medicine.IMP_PRICE;
                        rdo.IMP_PRICE_ORIGIN = medicine.IMP_PRICE * ((medicine.IMP_VAT_RATIO) + 1);
                        rdo.IMP_PRICE = System.Math.Round(medicine.IMP_PRICE * ((medicine.IMP_VAT_RATIO) + 1), 0);
                        rdo.EXP_TIME = string.Join(",", expMestMedicines.Select(o => Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.EXP_TIME ?? 0)).Distinct().ToList());
                        rdo.TOTAL_PRICE = System.Math.Round((rdo.PRICE * rdo.AMOUNT),0);
                        rdo.TOTAL_IMP_PRICE =System.Math.Round((rdo.IMP_PRICE * rdo.AMOUNT),0);
                        if (pres.Count > 0 && expMests.Count > 0)
                        {
                            rdo.CLIENT_NAME = string.Join(",", pres.Select(o => o.TDL_PATIENT_NAME).Distinct().ToList());
                        }
                        else if (expMests.Count > 0)
                        {
                            rdo.CLIENT_NAME = string.Join(",", expMests.Select(o => o.TDL_PATIENT_NAME).Distinct().ToList());
                        }
                        rdo.EXP_MEST_CODE = string.Join(",", expMestMedicines.Select(o => o.EXP_MEST_CODE).Distinct().ToList());
                        rdo.COUNT_IMP_EXP_MEST = listMobaImpMest.Count();
                        listRdo.Add(rdo);
                    }
                }

                if (listImpMestMaterial != null)
                {
                    foreach (var material in listImpMestMaterial)//Duyệt danh sách dữ liệu vật tư 
                    {
                        Mrs00306RDO rdo = new Mrs00306RDO();
                        rdo.SERVICE_NAME = material.MATERIAL_TYPE_NAME;
                        rdo.SERVICE_UNIT_NAME = material.SERVICE_UNIT_NAME;
                        rdo.NATIONAL_NAME = material.NATIONAL_NAME;

                        HIS_IMP_MEST impMest = listMobaImpMest.FirstOrDefault(o => o.ID == material.IMP_MEST_ID) ?? new HIS_IMP_MEST();

                        var expMestMaterials = listExpMestMaterial.Where(o => o.ID == material.TH_EXP_MEST_MATERIAL_ID).ToList() ?? new List<V_HIS_EXP_MEST_MATERIAL>();
                        var expMests = listSaleExpMest.Where(o => expMestMaterials.Exists(p => p.EXP_MEST_ID == o.ID)).ToList() ?? new List<HIS_EXP_MEST>();
                        var pres = listPrescription.Where(o => expMests.Exists(p => p.PRESCRIPTION_ID == o.SERVICE_REQ_ID)).ToList() ?? new List<HIS_EXP_MEST>();
                        rdo.AMOUNT = material.AMOUNT;
                        rdo.IMP_TIME = Inventec.Common.DateTime.Convert.TimeNumberToDateString(material.IMP_TIME ?? 0);
                        rdo.IMP_MEST_CODE = material.IMP_MEST_CODE;

                        if (expMestMaterials != null)
                        {
                            rdo.PRICE = System.Math.Round((expMestMaterials.First().PRICE ?? 0) * (1 + (expMestMaterials.First().VAT_RATIO ?? 0)), 0);
                            rdo.EXP_VAT = expMestMaterials.First().VAT_RATIO ?? 0;
                        }
                        else
                        {
                            rdo.PRICE = System.Math.Round((material.IMP_PRICE * (1 + material.IMP_VAT_RATIO)), 0);
                            rdo.EXP_VAT = expMestMaterials.First().IMP_VAT_RATIO;
                        }
                        rdo.VAT = material.IMP_VAT_RATIO;
                        rdo.IMP_VAT = material.IMP_VAT_RATIO;
                        rdo.IMP_PRICE_B4_VAT = material.IMP_PRICE;
                        rdo.IMP_PRICE_ORIGIN = material.IMP_PRICE * ((material.IMP_VAT_RATIO) + 1);
                        rdo.IMP_PRICE = System.Math.Round((material.IMP_PRICE * (1 + material.IMP_VAT_RATIO)),0);
                        rdo.EXP_TIME = string.Join(",", expMestMaterials.Select(o => Inventec.Common.DateTime.Convert.TimeNumberToDateString(o.EXP_TIME ?? 0)).Distinct().ToList());
                        rdo.TOTAL_PRICE =System.Math.Round((rdo.PRICE * rdo.AMOUNT),0);
                        rdo.TOTAL_IMP_PRICE =System.Math.Round((rdo.IMP_PRICE * rdo.AMOUNT),0);
                        if (pres.Count > 0 && expMests.Count > 0)
                        {
                            rdo.CLIENT_NAME = string.Join(",", pres.Select(o =>o.TDL_PATIENT_NAME).Distinct().ToList());
                        }
                        else if (expMests.Count > 0)
                        {
                            rdo.CLIENT_NAME = string.Join(",", expMests.Select(o => o.TDL_PATIENT_NAME).Distinct().ToList());
                        }
                        rdo.EXP_MEST_CODE = string.Join(",", expMestMaterials.Select(o => o.EXP_MEST_CODE).Distinct().ToList());
                        rdo.COUNT_IMP_EXP_MEST = listMobaImpMest.Count();
                        listRdo.Add(rdo);
                    }
                }
                result = true;
                listRdo = listRdo.OrderBy(o => o.IMP_TIME).ToList();
                listRdoParent = listRdo.GroupBy(o => o.IMP_MEST_CODE).Select(o => o.FirstOrDefault()).ToList();
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listRdo.Clear();
            }
            return result;
        }

        // xuất ra báo cáo
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {
            try
            {
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("TIME_FROM", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("TIME_TO", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }
                var filter = new HisMediStockFilterQuery();
                filter.IDs = castFilter.MEDI_STOCK_BUSINESS_IDs;
                dicSingleTag.Add("MEDI_STOCK_BUSINESS_IDs", String.Join(", ", new HisMediStockManager().Get(filter).Select(o => o.MEDI_STOCK_NAME)));
                Inventec.Common.Logging.LogSystem.Info("listRdo" + listRdo.Count);
                Inventec.Common.Logging.LogSystem.Info("listRdoParent" + listRdoParent.Count);
                if (IsNotNullOrEmpty(listMobaImpMest)) dicSingleTag.Add("IMP_LOGINNAME", string.Join(", ", listMobaImpMest.Select(s => s.IMP_LOGINNAME).ToList()));

                objectTag.AddObjectData(store, "Report", listRdo);
                objectTag.AddObjectData(store, "Parent", listRdoParent);
                objectTag.AddRelationship(store, "Parent", "Report", "IMP_MEST_CODE", "IMP_MEST_CODE");
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
