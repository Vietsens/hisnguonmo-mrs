using MOS.MANAGER.HisPatientType;
using MOS.MANAGER.HisService;
using MOS.MANAGER.HisPatient;
using MOS.MANAGER.HisTreatment;
using MOS.MANAGER.HisServiceReq;
using MOS.MANAGER.HisSereServ;
using MOS.MANAGER.HisPatientTypeAlter;
using MOS.MANAGER.HisImpMestMedicine;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisDepartment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Inventec.Common.DateTime;
using Inventec.Common.FlexCellExport;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisImpMest;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00194
{
    internal class Mrs00194Processor : AbstractProcessor
    {
        List<VSarReportMrs00194RDO> _listSarReportMrs00194Rdos = new List<VSarReportMrs00194RDO>();
        Mrs00194Filter CastFilter;
        private string DEPARTMENT_NAME { get; set; }
        private string PATIENT_TYPE_NAME { get; set; }
        public Mrs00194Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00194Filter);
        }
        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00194Filter)this.reportFilter;

                //ten khoa
                DEPARTMENT_NAME = new HisDepartmentManager(paramGet).GetById(CastFilter.DEPARTMENT_ID).DEPARTMENT_NAME;
                //ten loai doi tuong
                var patientTypeFilter = new HisPatientTypeFilterQuery
                {
                    IDs = CastFilter.PATIENT_TYPE_IDs
                };
                var listPatientTypeName = new HisPatientTypeManager(paramGet).Get(patientTypeFilter).Select(s => s.PATIENT_TYPE_NAME).ToList();
                PATIENT_TYPE_NAME = string.Join(", ", listPatientTypeName);
                //Yeu cau
                var serviceReqFilter = new HisServiceReqFilterQuery
                {
                    INTRUCTION_TIME_FROM = CastFilter.DATE_TIME_FROM,
                    INTRUCTION_TIME_TO = CastFilter.DATE_TIME_TO,
                    REQUEST_DEPARTMENT_ID = CastFilter.DEPARTMENT_ID
                };
                var listServiceReqs = new HisServiceReqManager(paramGet).Get(serviceReqFilter);
                //HSDT
                var listTreatmentIdInRequest = listServiceReqs.Select(s => s.TREATMENT_ID).ToList();
                var listTreatments = new List<HIS_TREATMENT>();
                var skip = 0;
                while (listTreatmentIdInRequest.Count - skip > 0)
                {
                    var listIds = listTreatmentIdInRequest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var treatmentFilter = new HisTreatmentFilterQuery
                    {
                        IDs = listIds,
                        IS_ACTIVE = IMSys.DbConfig.HIS_RS.COMMON.IS_ACTIVE__FALSE
                    };
                    var treatments = new HisTreatmentManager(paramGet).Get(treatmentFilter);
                    listTreatments.AddRange(treatments);
                }
                //YC DV
                var listSereServs = new List<HIS_SERE_SERV>();
                skip = 0;
                while (listTreatmentIdInRequest.Count - skip > 0)
                {
                    var listIds = listTreatmentIdInRequest.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var sereServFilter = new HisSereServFilterQuery
                    {
                        TREATMENT_IDs = listIds,
                        PATIENT_TYPE_IDs = CastFilter.PATIENT_TYPE_IDs
                    };
                    var sereServViews = new HisSereServManager(paramGet).Get(sereServFilter);
                    listSereServs.AddRange(sereServViews);
                }
                listSereServs = listSereServs.Where(o => listServiceReqs.Exists(p => p.ID == o.SERVICE_REQ_ID)).ToList();

                //Chuyen doi tuong
                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();
                var listPatientTypeAlters = new MOS.MANAGER.HisPatientTypeAlter.HisPatientTypeAlterManager(paramGet).GetByTreatmentIds(listTreatmentIds);

                //don thuoc
                var listServiceTypeMedicineMaterial = listSereServs.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC || s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT).Select(s => s.SERVICE_REQ_ID.Value).ToList();
                var listPrecriptions = new List<HIS_EXP_MEST>();
                skip = 0;
                while (listServiceTypeMedicineMaterial.Count - skip > 0)
                {
                    var listIds = listServiceTypeMedicineMaterial.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var prescriptionFilter = new HisExpMestFilterQuery
                    {
                        SERVICE_REQ_IDs = listIds
                    };
                    var prescriptions = new HisExpMestManager(paramGet).Get(prescriptionFilter);
                    listPrecriptions.AddRange(prescriptions);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_EXP_MEST_MEDICINE
                var listExpMestIds = listPrecriptions.Select(s => s.ID).ToList();
                var listExpMestMedicineViews = new List<HIS_EXP_MEST_MEDICINE>();
                skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var expMestMedicineFilter = new HisExpMestMedicineFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        IS_EXPORT = true
                    };
                    var expMestMedicineViews = new MOS.MANAGER.HisExpMestMedicine.HisExpMestMedicineManager(paramGet).Get(expMestMedicineFilter);
                    listExpMestMedicineViews.AddRange(expMestMedicineViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_EXP_MEST_MATERIAL
                var listExpMestMaterialViews = new List<HIS_EXP_MEST_MATERIAL>();
                skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var expMestMaterialFilter = new HisExpMestMaterialFilterQuery
                    {
                        EXP_MEST_IDs = listIds
                    };
                    var expMestMaterialViews = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).Get(expMestMaterialFilter);
                    listExpMestMaterialViews.AddRange(expMestMaterialViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_MOBA_IMP_MEST
                var listMobaImpMests = new List<HIS_IMP_MEST>();
                skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var mobaImpMestFilter = new HisImpMestFilterQuery
                    {
                        MOBA_EXP_MEST_IDs = listIds
                    };
                    var mobaImpMestViews = new HisImpMestManager(paramGet).Get(mobaImpMestFilter);
                    listMobaImpMests.AddRange(mobaImpMestViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds = listMobaImpMests.Select(s => s.ID).ToList();
                var listImpMestMedicineViews = new List<HIS_IMP_MEST_MEDICINE>();
                skip = 0;
                while (listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var impMestMedicineFilter = new HisImpMestMedicineFilterQuery
                    {
                        IMP_MEST_IDs = listIds
                    };
                    var impMestMedicineViews = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).Get(impMestMedicineFilter);
                    listImpMestMedicineViews.AddRange(impMestMedicineViews);
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST_MATERIAL
                var listImpMestMaterialViews = new List<HIS_IMP_MEST_MATERIAL>();
                skip = 0;
                while (listImpMestIds.Count - skip > 0)
                {
                    var listIds = listImpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var impMestMaterialFilter = new HisImpMestMaterialFilterQuery
                    {
                        IMP_MEST_IDs = listIds
                    };
                    var impMestMaterialViews = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).Get(impMestMaterialFilter);
                    listImpMestMaterialViews.AddRange(impMestMaterialViews);
                }
                //--------------------------------------------------------------------------------------------------

                ProcessFilterData(listServiceReqs, listTreatments, listSereServs, listPatientTypeAlters,
                    listPrecriptions, listExpMestMedicineViews, listExpMestMaterialViews, listMobaImpMests, listImpMestMedicineViews, listImpMestMaterialViews);

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
            return true;
        }
        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TIME_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
            dicSingleTag.Add("PATIENT_TYPE_NAME", PATIENT_TYPE_NAME);
            objectTag.AddObjectData(store, "Report", _listSarReportMrs00194Rdos);

        }


        private void ProcessFilterData(List<HIS_SERVICE_REQ> listServiceReqs, List<HIS_TREATMENT> listTreatments, List<HIS_SERE_SERV> listSereServs,
            List<HIS_PATIENT_TYPE_ALTER> listPatientTypeAlters, List<HIS_EXP_MEST> listPrecriptionViews,
            List<HIS_EXP_MEST_MEDICINE> listExpMestMedicineViews, List<HIS_EXP_MEST_MATERIAL> listExpMestMaterialViews, List<HIS_IMP_MEST> listMobaImpMestViews,
            List<HIS_IMP_MEST_MEDICINE> listImpMestMedicineViews, List<HIS_IMP_MEST_MATERIAL> listImpMestMaterialViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00194 ===============================================================");
                var listTreatmentIds = listTreatments.Select(s => s.ID).ToList();
                var listServiceReqViews = listServiceReqs.Where(s => listTreatmentIds.Contains(s.TREATMENT_ID)).ToList();
                var listServiceReqIds = listServiceReqViews.Select(s => s.ID).ToList();
                var listSereServViews = listSereServs.Where(s => listServiceReqIds.Contains(s.SERVICE_REQ_ID.Value)).ToList();
                var listSereServGroupByTreatmentIds = listSereServViews.GroupBy(s => s.TDL_TREATMENT_ID).ToList();
                HIS_TREATMENT treatment = new HIS_TREATMENT();
                foreach (var listSereServGroupByTreatmentId in listSereServGroupByTreatmentIds)
                {
                    treatment = listTreatments.FirstOrDefault(o => o.ID == listSereServGroupByTreatmentId.First().TDL_TREATMENT_ID) ?? new HIS_TREATMENT();
                    var treatmentId = listSereServGroupByTreatmentId.Key;
                    var listServiceReqIdByTreatments = listSereServGroupByTreatmentId.Select(s => s.SERVICE_REQ_ID).ToList();
                    var listServiceIds = listSereServGroupByTreatmentId.Where(s => s.IS_EXPEND == null).Select(s => s.SERVICE_ID).ToList();

                    var patientCode = treatment.TDL_PATIENT_CODE;
                    var patientName = treatment.TDL_PATIENT_NAME;
                    var dateOfBirth = Inventec.Common.DateTime.Convert.TimeNumberToDateString(treatment.TDL_PATIENT_DOB);
                    var genderName = treatment.TDL_PATIENT_GENDER_NAME;
                    var listPatientTypeAlterViews = listPatientTypeAlters.Where(s => s.TREATMENT_ID == treatmentId).ToList();
                    var listPatientTypeAlterIds = listPatientTypeAlterViews.Select(s => s.ID).ToList();
                    var heinCardCardNumber = string.Empty;
                    var heinMediOrgCode = string.Empty;
                    if (IsNotNullOrEmpty(listPatientTypeAlterViews))
                    {
                        var firstPatientTypeAlterViews = listPatientTypeAlterViews.First();
                        heinCardCardNumber = firstPatientTypeAlterViews.HEIN_CARD_NUMBER;
                        heinMediOrgCode = firstPatientTypeAlterViews.HEIN_MEDI_ORG_CODE;
                    }
                    var firstTreatmentViews = listTreatments.First(s => s.ID == treatmentId);
                    var inTime = Inventec.Common.DateTime.Convert.TimeNumberToDateString(firstTreatmentViews.IN_TIME);
                    var outTime = string.Empty;
                    if (firstTreatmentViews.OUT_TIME.HasValue)
                        outTime = Inventec.Common.DateTime.Convert.TimeNumberToDateString(firstTreatmentViews.OUT_TIME.Value);

                    var countExam = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__KH).ToList();
                    var priceExam = CalculatorPriceService(countExam);

                    var countBed = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__G).ToList();
                    var priceBed = CalculatorPriceService(countBed);

                    var countTest = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__XN).ToList();
                    var priceTest = CalculatorPriceService(countTest);

                    var countDiim = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__CDHA).ToList();
                    var priceDiim = CalculatorPriceService(countDiim);

                    var countFuex = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TDCN).ToList();
                    var priceFuex = CalculatorPriceService(countFuex);

                    var countSurgAndMisu = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__PT ||
                        s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__TT).ToList();
                    var priceSurgAndMisu = CalculatorPriceService(countSurgAndMisu);
                    //===================================================================================================== tính toán số lượng thuốc đã xuất thực tế cho BN (xuất - thu hồi)
                    var listExpMestIds = listPrecriptionViews.Where(s => listServiceReqIdByTreatments.Contains(s.SERVICE_REQ_ID)).Select(s => s.ID).ToList(); //lấy các phiếu yêu cầu xuất thuốc và vật tư cho bệnh nhân

                    var listExpMestMedicines = listExpMestMedicineViews.Where(s=> listServiceReqIdByTreatments.Contains(s.TDL_SERVICE_REQ_ID)).ToList(); //số lượng thuốc đã xuất cho BN
                    var listExpMestMaterials = listExpMestMaterialViews.Where(s => listServiceReqIdByTreatments.Contains(s.TDL_SERVICE_REQ_ID)).ToList(); //số lượng vật tư đã xuất cho BN
                    var listimpMestIds = listMobaImpMestViews.Where(s => listExpMestIds.Contains(s.MOBA_EXP_MEST_ID??0)).Select(s => s.ID).ToList(); //lấy phiếu thu hồi các loại thuốc đã xuất cho BN
                    var listImpMestMedicines = listImpMestMedicineViews.Where(s => listimpMestIds.Contains(s.IMP_MEST_ID)).ToList(); //lấy thuốc thu hồi đã xuất cho BN
                    var listImpMestMaterials = listImpMestMaterialViews.Where(s => listimpMestIds.Contains(s.IMP_MEST_ID)).ToList(); //lấy vật tư thu hồi đã xuất cho BN

                    var listPriceExpMestMedicines = new List<PriceExpMest>();
                    foreach (var listExpMestMedicine in listExpMestMedicines)
                    {
                        var priceExpMest = new PriceExpMest
                        {
                            MEDICINE_ID = listExpMestMedicine.MEDICINE_ID ?? 0,
                            PRICE_EXP = listExpMestMedicine.PRICE
                        };
                        listPriceExpMestMedicines.Add(priceExpMest);
                    }

                    var listPriceExpMestMaterials = new List<PriceExpMest>();
                    foreach (var listExpMestMaterial in listExpMestMaterials)
                    {
                        var priceExpMest = new PriceExpMest
                        {
                            MATERIAL_ID = listExpMestMaterial.MATERIAL_ID ?? 0,
                            PRICE_EXP = listExpMestMaterial.PRICE
                        };
                        listPriceExpMestMaterials.Add(priceExpMest);
                    }

                    var totalPriceExpMestMedicine = CalculatorPriceMedicineMaterial(listExpMestMedicines, null, null, null, null);
                    var totalPriceExpMestMaterial = CalculatorPriceMedicineMaterial(null, listExpMestMaterials, null, null, null);
                    var totalPriceImpMestMedicine = CalculatorPriceMedicineMaterial(null, null, listImpMestMedicines, null, listPriceExpMestMedicines);
                    var totalPriceImpMestMaterial = CalculatorPriceMedicineMaterial(null, null, null, listImpMestMaterials, listPriceExpMestMaterials);

                    var priceMedicine = totalPriceExpMestMedicine - totalPriceImpMestMedicine;
                    var priceMaterial = totalPriceExpMestMaterial - totalPriceImpMestMaterial;
                    //=====================================================================================================
                    var countBlood = listSereServGroupByTreatmentId.Where(s => s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__MAU).ToList();
                    var priceBlood = CalculatorPriceService(countBlood);

                    var countTranPort = listSereServGroupByTreatmentId.Where(s => s.TDL_HEIN_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_HEIN_SERVICE_TYPE.ID__VC).ToList();
                    var priceTranPort = CalculatorPriceService(countTranPort);

                    var totalAll = priceExam + priceBed + priceTest + priceDiim + priceFuex + priceSurgAndMisu + priceMedicine + priceMaterial + priceBlood + priceTranPort;

                    var countExpendMedicine = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__THUOC && s.IS_EXPEND != null).ToList();
                    var priceExpendMedicine = CalculatorPriceService(countExpendMedicine);

                    var countExpendMaterial = listSereServGroupByTreatmentId.Where(s => s.TDL_SERVICE_TYPE_ID == IMSys.DbConfig.HIS_RS.HIS_SERVICE_TYPE.ID__VT && s.IS_EXPEND != null).ToList();
                    var priceExpendMaterial = CalculatorPriceService(countExpendMaterial);

                    var totalPriceExpend = priceExpendMedicine + priceExpendMaterial;
                    var rdo = new VSarReportMrs00194RDO
                    {
                        PATIENT_CODE = patientCode,//mã bệnh nhân
                        PATIENT_NAME = patientName,//tên bệnh nhân
                        DATE_OF_BIRTH = dateOfBirth,//ngày sinh
                        GENDER_NAME = genderName,//giới tính
                        HEIN_CARD_NUMBER = heinCardCardNumber,//số thẻ BHYT
                        HEIN_MEDI_ORG_CODE = heinMediOrgCode,//mã đăng ký khám chữa bệnh ban đầu
                        IN_TIME = inTime,//ngày vào viện
                        OUT_TIME = outTime,//ngày ra viện
                        PRICE_EXAM = priceExam,//tiền khám
                        PRICE_BED = priceBed,//tiền giường
                        PRICE_TEST = priceTest,//tiền xét nghiệm
                        PRICE_DIIM = priceDiim,//tiền chuẩn đoán hình ảnh
                        PRICE_FUEX = priceFuex,//tiền thăm dò chức năng
                        PRICE_SURG_AND_MISU = priceSurgAndMisu,//tiền phẫu thuật, thủ thuật
                        PRICE_MEDICINE = priceMedicine,//tiền thuốc
                        PRICE_MATERIAL = priceMaterial,//tiền vật tư
                        PRICE_BLOOD = priceBlood,//máu và chế phẩm máu
                        PRICE_TRANSPORT = priceTranPort,//vận chuyển
                        TOTAL_ALL = totalAll,//tổng tiền
                        PRICE_EXPEND_MATERIAL = priceExpendMedicine,//hao phí thuốc
                        PRICE_EXPEND_MEDICINE = priceExpendMaterial,//hap phí vật tư
                        TOTAL_PRICE_EXPEND = totalPriceExpend,//tổng hao phí

                    };
                    _listSarReportMrs00194Rdos.Add(rdo);
                }
                LogSystem.Info("Ket thuc xu ly du lieu MRS00194 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }

        private decimal CalculatorPriceService(List<HIS_SERE_SERV> listSereServs)
        {
            decimal totalPrice = 0;
            foreach (var listSereServ in listSereServs)
            {
                if (listSereServ.VIR_PRICE != null && listSereServ.IS_EXPEND == null)
                {
                    var price = listSereServ.AMOUNT * listSereServ.VIR_PRICE.Value;
                    totalPrice = totalPrice + price;
                }
                else
                {
                    var price = listSereServ.AMOUNT * listSereServ.PRICE;
                    totalPrice = totalPrice + price;
                }
            }
            return totalPrice;
        }

        private decimal CalculatorPriceMedicineMaterial(List<HIS_EXP_MEST_MEDICINE> listExpMestMedicines, List<HIS_EXP_MEST_MATERIAL> listExpMestMaterials,
            List<HIS_IMP_MEST_MEDICINE> listImpMestMedicines, List<HIS_IMP_MEST_MATERIAL> listImpMestMaterials, List<PriceExpMest> listPriceExpMests)
        {
            try
            {
                decimal totalPrice = 0;
                if (listExpMestMedicines != null && listExpMestMedicines.Count > 0)
                {
                    foreach (var listExpMestMedicine in listExpMestMedicines)
                    {
                        if (listExpMestMedicine.PRICE == null) continue;
                        var price = listExpMestMedicine.PRICE.Value * listExpMestMedicine.AMOUNT;
                        totalPrice = (totalPrice + price);
                    }
                }
                if (listExpMestMaterials != null && listExpMestMaterials.Count > 0)
                {
                    foreach (var listExpMestMaterial in listExpMestMaterials)
                    {
                        if (listExpMestMaterial.PRICE == null) continue;
                        var price = listExpMestMaterial.PRICE.Value * listExpMestMaterial.AMOUNT;
                        totalPrice = (totalPrice + price);
                    }
                }
                if (listImpMestMedicines != null && listImpMestMedicines.Count > 0)
                {
                    foreach (var listImpMestMedicine in listImpMestMedicines)
                    {

                        var priceExp = listPriceExpMests.First(s => s.MEDICINE_ID == listImpMestMedicine.MEDICINE_ID).PRICE_EXP;
                        if (priceExp == null) continue;
                        var priceExpMest = priceExp.Value;
                        var price = listImpMestMedicine.AMOUNT * priceExpMest;
                        totalPrice = (totalPrice + price);
                    }
                }
                if (listImpMestMaterials != null && listImpMestMaterials.Count > 0)
                {
                    foreach (var listImpMestMaterial in listImpMestMaterials)
                    {
                        var priceExp = listPriceExpMests.First(s => s.MATERIAL_ID == listImpMestMaterial.MATERIAL_ID).PRICE_EXP;
                        if (priceExp == null) continue;
                        var priceExpMest = priceExp.Value;
                        var price = listImpMestMaterial.AMOUNT * priceExpMest;
                        totalPrice = (totalPrice + price);
                    }
                }
                return totalPrice;
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh tinh toan so tien xuat thuoc va vat tu ===============================================================");
                LogSystem.Error(ex);
                return 0;
            }
        }

        private class PriceExpMest
        {
            public long MEDICINE_ID { get; set; }
            public long MATERIAL_ID { get; set; }

            public decimal? PRICE_EXP { get; set; }
        }
    }
}
