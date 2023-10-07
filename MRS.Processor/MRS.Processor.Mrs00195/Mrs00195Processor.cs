using MOS.MANAGER.HisService;
using MOS.MANAGER.HisReportTypeCat;
using MOS.MANAGER.HisServiceRetyCat;
using MOS.MANAGER.HisImpMestMaterial;
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

namespace MRS.Processor.Mrs00195
{
    internal class Mrs00195Processor : AbstractProcessor
    {
        List<VSarReportMrs00195RDO> _listSarReportMrs00195Rdos = new List<VSarReportMrs00195RDO>();
        private List<string> departmentNames = new List<string>();
        List<HIS_REPORT_TYPE_CAT> listReportTypeCat = new List<HIS_REPORT_TYPE_CAT>();
        Mrs00195Filter CastFilter;
        string reportTypeCode = "";
        public Mrs00195Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
            this.reportTypeCode = reportTypeCode;
        }

        public override Type FilterType()
        {
            return typeof(Mrs00195Filter);
        }

        protected override bool GetData()
        {
            var result = true;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00195Filter)this.reportFilter;

                //-------------------------------------------------------------------------------------------------- 
                //Vật tư xuất và vật tư nhập thu hồi
                var metyFilterHisExpMest = new HisExpMestViewFilterQuery
                {
                    FINISH_DATE_FROM = CastFilter.TIME_FROM,
                    FINISH_DATE_TO = CastFilter.TIME_TO,
                    MEDI_STOCK_IDs = CastFilter.MEDI_STOCK_IDs,
                    EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE,
                    EXP_MEST_TYPE_IDs = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__PL,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__HPKP,
                        IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_TYPE.ID__DPK
                    }
                };
                var listHisExpMest = new HisExpMestManager(paramGet).GetView(metyFilterHisExpMest);
                var listExpMestIds = listHisExpMest.Select(o => o.ID).ToList();
                var listExpMestMaterialView = new List<V_HIS_EXP_MEST_MATERIAL>();
                var listMobaImpMestViews = new List<V_HIS_IMP_MEST>();
                var skip = 0;
                while (listExpMestIds.Count - skip > 0)
                {
                    var listIds = listExpMestIds.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterExpMestMaterial = new HisExpMestMaterialViewFilterQuery
                    {
                        EXP_MEST_IDs = listIds,
                        IS_EXPORT = true
                    };
                    var expMestMaterialViews = new MOS.MANAGER.HisExpMestMaterial.HisExpMestMaterialManager(paramGet).GetView(metyFilterExpMestMaterial);
                    listExpMestMaterialView.AddRange(expMestMaterialViews);

                    HisImpMestViewFilterQuery mobaImpMestFilter = new HisImpMestViewFilterQuery
                    {
                        MOBA_EXP_MEST_IDs = listIds
                    };
                    var mobaImpMestViews = new HisImpMestManager(paramGet).GetView(mobaImpMestFilter);
                    listMobaImpMestViews.AddRange(mobaImpMestViews);
                }
                listMobaImpMestViews = listMobaImpMestViews.Where(o => o.IMP_MEST_STT_ID == 5).ToList();
                //~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds2 = listMobaImpMestViews.Select(s => s.ID).ToList();
                var listHisImpMestMaterial = new List<V_HIS_IMP_MEST_MATERIAL>();
                skip = 0;
                while (listImpMestIds2.Count - skip > 0)
                {
                    var listIds1 = listImpMestIds2.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterImpMestMaterial = new HisImpMestMaterialViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds1,
                    };
                    var impMestMaterialView = new MOS.MANAGER.HisImpMestMaterial.HisImpMestMaterialManager(paramGet).GetView(metyFilterImpMestMaterial);
                    listHisImpMestMaterial.AddRange(impMestMaterialView);
                }

                //--------------------------------------------------------------------------------------------------
                //Danh sách khoa

                var metyFilterHisDepartment = new HisDepartmentFilterQuery();
                var listHisDepartment = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).Get(metyFilterHisDepartment);
                //Sap xep theo thu tu alphabet va chi lay toi da so luong khoa phong ma bao cao co the dap ung
                listHisDepartment = listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Take(VSarReportMrs00195RDO.MAX_DEPARTMENT_NUM).ToList();
                this.departmentNames = listHisDepartment.Select(o => o.DEPARTMENT_NAME).ToList();

                //--------------------------------------------------------------------------------------------------
                //Thiết lập dịch vụ - nhóm loại báo cáo
                HisServiceRetyCatViewFilterQuery GroupFilter = new HisServiceRetyCatViewFilterQuery();
                GroupFilter.REPORT_TYPE_CODE__EXACT = reportTypeCode;
                List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat = new MOS.MANAGER.HisServiceRetyCat.HisServiceRetyCatManager(param).GetView(GroupFilter);
                //Nhóm loại báo cáo
                HisReportTypeCatFilterQuery reportxFilter = new HisReportTypeCatFilterQuery();
                reportxFilter.REPORT_TYPE_CODE__EXACT = reportTypeCode;
                listReportTypeCat = new MOS.MANAGER.HisReportTypeCat.HisReportTypeCatManager(param).Get(reportxFilter);
                //--------------------------------------------------------------------------------------------------

                ProcessFilterData(listExpMestMaterialView, listHisDepartment, listServiceRetyCat, listHisImpMestMaterial, listMobaImpMestViews);

                result = true;

            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void AddSingleData(Dictionary<string, object> dicSingleData, List<string> departmentNames)
        {
            for (int i = 0; i < departmentNames.Count; i++)
            {
                dicSingleData.Add(string.Format("DEPARTMENT_NAME_{0}", i + 1), departmentNames[i]);
            }
        }

        protected override bool ProcessData()
        {
            return true;
        }

        protected override void SetTag(Dictionary<string, object> dicSingleTag, Inventec.Common.FlexCellExport.ProcessObjectTag objectTag, Inventec.Common.FlexCellExport.Store store)
        {

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.TIME_TO));
            this.AddSingleData(dicSingleTag, this.departmentNames);

            objectTag.AddObjectData(store, "Report", _listSarReportMrs00195Rdos);
            objectTag.AddObjectData(store, "Report1", listReportTypeCat);
            objectTag.AddRelationship(store, "Report1", "Report", "CATEGORY_NAME", "CATEGORY_NAME");
        }

        private void ProcessFilterData(List<V_HIS_EXP_MEST_MATERIAL> listExpMestMaterialView, List<HIS_DEPARTMENT> listHisDepartment, List<V_HIS_SERVICE_RETY_CAT> listServiceRetyCat, List<V_HIS_IMP_MEST_MATERIAL> listHisImpMestMaterial, List<V_HIS_IMP_MEST> listMobaImpMestViews)
        {
            try
            {
                LogSystem.Info("Bat dau xu ly du lieu MRS00195 ===============================================================");
                if (IsNotNullOrEmpty(listExpMestMaterialView) && IsNotNullOrEmpty(listHisDepartment))
                {
                    List<V_HIS_EXP_MEST_MATERIAL> Complex = new List<V_HIS_EXP_MEST_MATERIAL>();
                    Complex.AddRange(listExpMestMaterialView);
                    foreach (var hisImpMestMaterial in listHisImpMestMaterial)
                    {
                        V_HIS_EXP_MEST_MATERIAL materialImp = new V_HIS_EXP_MEST_MATERIAL
                        {
                            MATERIAL_TYPE_NAME = hisImpMestMaterial.MATERIAL_TYPE_NAME,
                            SERVICE_UNIT_NAME = hisImpMestMaterial.SERVICE_UNIT_NAME,
                            REQ_DEPARTMENT_ID = hisImpMestMaterial.REQ_DEPARTMENT_ID ?? 0,
                            EXP_MEST_ID = hisImpMestMaterial.IMP_MEST_ID,
                            AMOUNT = hisImpMestMaterial.AMOUNT * (-1)
                        };
                        Complex.Add(materialImp);
                    }

                    var Group = Complex.GroupBy(g => g.MATERIAL_TYPE_ID).ToList();
                    foreach (var group in Group)
                    {
                        var listSub = group.ToList<V_HIS_EXP_MEST_MATERIAL>();
                        VSarReportMrs00195RDO rdo = new VSarReportMrs00195RDO();
                        List<Decimal> amountDepartment = new List<decimal> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                        foreach (var item in listSub)
                        {
                            if (item.AMOUNT >= 0)
                            {
                                //trong các khoa, khoa nào có mã khoa bằng mã khoa xuất vật tư thì cộng số lượng vật tư xuất vào cho khoa đó.
                                for (int i = 0; i < listHisDepartment.Count; i++)
                                    if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == item.REQ_DEPARTMENT_ID)
                                        amountDepartment[i] = amountDepartment[i] + item.AMOUNT;
                            }
                            else
                                //trong các khoa, khoa nào có mã khoa bằng mã khoa xuất vật tư "đã thu hồi" thì cộng số lượng vật tư xuất vào cho khoa đó.
                                for (int i = 0; i < listHisDepartment.Count; i++)
                                    if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == listExpMestMaterialView.Where(o => o.EXP_MEST_ID == listMobaImpMestViews.Where(p => p.ID == item.EXP_MEST_ID).Select(q => q.MOBA_EXP_MEST_ID).FirstOrDefault()).Select(r => r.REQ_DEPARTMENT_ID).FirstOrDefault())
                                        amountDepartment[i] = amountDepartment[i] + item.AMOUNT;
                        }

                        rdo.CATEGORY_NAME = listServiceRetyCat.Where(o => o.SERVICE_ID == group.Select(p => p.SERVICE_ID).FirstOrDefault()).Select(q => q.CATEGORY_NAME).FirstOrDefault();
                        rdo.MATERIAL_TYPE_NAME = group.Select(o => o.MATERIAL_TYPE_NAME).FirstOrDefault();
                        rdo.SERVICE_UNIT_NAME = group.Select(o => o.SERVICE_UNIT_NAME).FirstOrDefault();

                        rdo.D1 = amountDepartment[0];
                        rdo.D2 = amountDepartment[1];
                        rdo.D3 = amountDepartment[2];
                        rdo.D4 = amountDepartment[3];
                        rdo.D5 = amountDepartment[4];
                        rdo.D6 = amountDepartment[5];
                        rdo.D7 = amountDepartment[6];
                        rdo.D8 = amountDepartment[7];
                        rdo.D9 = amountDepartment[8];
                        rdo.D10 = amountDepartment[9];
                        rdo.D11 = amountDepartment[10];
                        rdo.D12 = amountDepartment[11];
                        rdo.D13 = amountDepartment[12];
                        rdo.D14 = amountDepartment[13];
                        rdo.D15 = amountDepartment[14];
                        rdo.D16 = amountDepartment[15];
                        rdo.D17 = amountDepartment[16];
                        rdo.D18 = amountDepartment[17];
                        rdo.D19 = amountDepartment[18];
                        rdo.D20 = amountDepartment[19];
                        rdo.D21 = amountDepartment[20];
                        rdo.D22 = amountDepartment[21];
                        rdo.D23 = amountDepartment[22];
                        rdo.D24 = amountDepartment[23];
                        rdo.D25 = amountDepartment[24];
                        rdo.D26 = amountDepartment[25];
                        rdo.D27 = amountDepartment[26];
                        rdo.D28 = amountDepartment[27];
                        rdo.D29 = amountDepartment[28];
                        rdo.D30 = amountDepartment[29];
                        _listSarReportMrs00195Rdos.Add(rdo);
                    }
                    //foreach (var hisImpMestMaterial in Complex)
                    //{


                    //    if (hisImpMestMaterial.MATERIAL_TYPE_NAME != materialTypeName)
                    //    {
                    //        VSarReportMrs00195RDO rdo = new VSarReportMrs00195RDO(); 
                    //        rdo.CATEGORY_NAME = listServiceRetyCat.Where(o => o.SERVICE_NAME == materialTypeName).Select(p => p.CATEGORY_NAME).FirstOrDefault(); 
                    //        rdo.MATERIAL_TYPE_NAME = materialTypeName; 
                    //        rdo.SERVICE_UNIT_NAME = serviceUnitName; 
                    //        rdo.D1 = amountDepartment[0]; 
                    //        rdo.D2 = amountDepartment[1]; 
                    //        rdo.D3 = amountDepartment[2]; 
                    //        rdo.D4 = amountDepartment[3]; 
                    //        rdo.D5 = amountDepartment[4]; 
                    //        rdo.D6 = amountDepartment[5]; 
                    //        rdo.D7 = amountDepartment[6]; 
                    //        rdo.D8 = amountDepartment[7]; 
                    //        rdo.D9 = amountDepartment[8]; 
                    //        rdo.D10 = amountDepartment[9]; 
                    //        rdo.D11 = amountDepartment[10]; 
                    //        rdo.D12 = amountDepartment[11]; 
                    //        rdo.D13 = amountDepartment[12]; 
                    //        rdo.D14 = amountDepartment[13]; 
                    //        rdo.D15 = amountDepartment[14]; 
                    //        rdo.D16 = amountDepartment[15]; 


                    //        _listSarReportMrs00195Rdos.Add(rdo); 
                    //        if (hisImpMestMaterial.AMOUNT>=0)
                    //        for (int i = 0;  i < listHisDepartment.Count;  i++)
                    //            if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == hisImpMestMaterial.REQ_DEPARTMENT_ID)
                    //                amountDepartment[i] = hisImpMestMaterial.AMOUNT; 
                    //            else

                    //                amountDepartment[i] = 0; 
                    //        else
                    //            for (int i = 0;  i < listHisDepartment.Count;  i++)
                    //                if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == listExpMestMaterialView.Where(o => o.EXP_MEST_ID == listMobaImpMestViews.Where(p=>p.IMP_MEST_ID==hisImpMestMaterial.EXP_MEST_ID).Select(q=>q.EXP_MEST_ID).FirstOrDefault()).Select(r=>r.REQ_DEPARTMENT_ID).FirstOrDefault())
                    //                    amountDepartment[i] = hisImpMestMaterial.AMOUNT; 
                    //                else

                    //                    amountDepartment[i] = 0; 
                    //    }
                    //    else
                    //        if (hisImpMestMaterial.AMOUNT >= 0)
                    //        {
                    //            for (int i = 0;  i < listHisDepartment.Count;  i++)

                    //                if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == hisImpMestMaterial.REQ_DEPARTMENT_ID)
                    //                    amountDepartment[i] = amountDepartment[i] + hisImpMestMaterial.AMOUNT; 
                    //        }
                    //        else
                    //            for (int i = 0;  i < listHisDepartment.Count;  i++)

                    //                if (listHisDepartment.OrderBy(o => o.DEPARTMENT_NAME).Select(o => o.ID).ToList()[i] == listExpMestMaterialView.Where(o => o.EXP_MEST_ID == listMobaImpMestViews.Where(p => p.IMP_MEST_ID == hisImpMestMaterial.EXP_MEST_ID).Select(q => q.EXP_MEST_ID).FirstOrDefault()).Select(r => r.REQ_DEPARTMENT_ID).FirstOrDefault())
                    //                    amountDepartment[i] = amountDepartment[i] + hisImpMestMaterial.AMOUNT; 

                    //    materialTypeName = hisImpMestMaterial.MATERIAL_TYPE_NAME; 
                    //    serviceUnitName = hisImpMestMaterial.SERVICE_UNIT_NAME; 


                    //}
                }

                LogSystem.Info("Ket thuc xu ly du lieu MRS00195 ===============================================================");
            }
            catch (Exception ex)
            {
                LogSystem.Info("Loi trong qua trinh xu ly du lieu ===============================================================");
                LogSystem.Error(ex);
            }
        }
    }

}
