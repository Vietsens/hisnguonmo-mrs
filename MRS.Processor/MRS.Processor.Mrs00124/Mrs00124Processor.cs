using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Core.MrsReport;
using MOS.MANAGER.HisExpMest;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.MANAGER.HisImpMest;
using MOS.MANAGER.HisImpMestMaterial;
using MOS.MANAGER.HisImpMestMedicine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00124
{
    public class Mrs00124Processor : AbstractProcessor
    {
        Mrs00124Filter castFilter = null;
        List<Mrs00124RDO> listRdoMedicine = new List<Mrs00124RDO>();
        List<Mrs00124RDO> listRdoMaterial = new List<Mrs00124RDO>();
        //List<Mrs00124RDO> listDepartment = new List<Mrs00124RDO>(); 
        //Dictionary<long, Mrs00124RDO> dicDepartment = new Dictionary<long, Mrs00124RDO>(); 
        Dictionary<string, object> dicSingleData = new Dictionary<string, object>();
        List<Mrs00124SDO> listSdoMedi = new List<Mrs00124SDO>();
        List<Mrs00124SDO> listSdoMate = new List<Mrs00124SDO>();
        string MEDI_STOCK_NAME = null;
        List<V_HIS_EXP_MEST> ListExpMest;

        public Mrs00124Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        { }

        public override Type FilterType()
        {
            return typeof(Mrs00124Filter);
        }

        protected override bool GetData()
        {
            bool result = false;
            try
            {
                castFilter = ((Mrs00124Filter)reportFilter);
                CommonParam paramGet = new CommonParam();
                Inventec.Common.Logging.LogSystem.Debug("Bat dau lay du lieu V_HIS_DEPA_EXP_MEST, V_HIS_PRESCRIPTION, V_HIS_CHMS_EXP_MEST, Mrs00124." + Inventec.Common.Logging.LogUtil.TraceData(Inventec.Common.Logging.LogUtil.GetMemberName(() => castFilter), castFilter));
                HisExpMestViewFilterQuery expFilter = new HisExpMestViewFilterQuery();
                expFilter.FINISH_TIME_FROM = castFilter.TIME_FROM;
                expFilter.FINISH_TIME_TO = castFilter.TIME_TO;
                expFilter.EXP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_EXP_MEST_STT.ID__DONE;
                ListExpMest = new HisExpMestManager(paramGet).GetView(expFilter);

                if (!paramGet.HasException)
                {
                    result = true;
                }
                else
                {
                    throw new DataMisalignedException("Co exception xay ra tai DAOGET trong qua trinh tong hop du lieu Mrs00124.");
                }
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
            bool result = false;
            try
            {
                ProcessListData(ListExpMest);
                result = true;
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                result = false;
            }
            return result;
        }

        private void ProcessListData(List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                CommonParam paramGet = new CommonParam();
                if (IsNotNullOrEmpty(ListExpMest))
                {
                    ListExpMest = ListExpMest.Where(o => o.MEDI_STOCK_ID == castFilter.MEDI_STOCK_ID).ToList();
                    ProcessListExpMest(paramGet, ListExpMest);
                }

                if (castFilter.IsMedicineType.HasValue)
                {
                    if (castFilter.IsMedicineType.Value)
                    {
                        ProcessListSdoIsMediTrue();
                    }
                    else
                    {
                        ProcessListSdoIsMediFalse();
                    }
                }
                else
                {
                    ProcessListSdoIsMediNull();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listSdoMedi.Clear();
                listSdoMate.Clear();
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();

            }
        }

        //Xử lý cho trường hợp lấy cả thuốc và vật tư
        private void ProcessListSdoIsMediNull()
        {
            try
            {
                Dictionary<long, Mrs00124RDO> dicMediRdo = new Dictionary<long, Mrs00124RDO>();
                Dictionary<long, Mrs00124RDO> dicMateRdo = new Dictionary<long, Mrs00124RDO>();
                Dictionary<int, HIS_DEPARTMENT> dicDepartment = new Dictionary<int, HIS_DEPARTMENT>();

                if (IsNotNullOrEmpty(listSdoMedi))
                {
                    int count = 1;
                    var GroupDepa = listSdoMedi.GroupBy(g => g.DEPARTMENT_ID).ToList();
                    foreach (var group in GroupDepa)
                    {
                        if (count > 20)
                        {
                            throw new DataMisalignedException("So luong khoa lon hon so luong co the xu ly, MRS00124");
                        }
                        var listSubDepa = group.ToList<Mrs00124SDO>();
                        HIS_DEPARTMENT depa = new HIS_DEPARTMENT();
                        depa.ID = listSubDepa.First().DEPARTMENT_ID;
                        depa.DEPARTMENT_NAME = listSubDepa.First().DEPARTMENT_NAME;
                        if (!dicDepartment.ContainsKey(count))
                        {
                            dicDepartment.Add(count, depa);
                        }
                        var GroupMedi = listSubDepa.GroupBy(o => o.MEDICINE_TYPE_ID).ToList();
                        foreach (var groupMedi in GroupMedi)
                        {
                            var listSub = groupMedi.ToList<Mrs00124SDO>();
                            if (dicMediRdo.ContainsKey(listSub.First().MEDICINE_TYPE_ID))
                            {
                                System.Reflection.PropertyInfo pi = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                pi.SetValue(dicMediRdo[listSub.First().MEDICINE_TYPE_ID], listSub.Sum(s => s.AMOUNT));
                            }
                            else
                            {
                                Mrs00124RDO rdo = new Mrs00124RDO();
                                rdo.MEDICINE_TYPE_ID = listSub.First().MEDICINE_TYPE_ID;
                                rdo.MEDICINE_TYPE_CODE = listSub.First().MEDICINE_TYPE_CODE;
                                rdo.MEDICINE_TYPE_NAME = listSub.First().MEDICINE_TYPE_NAME;
                                System.Reflection.PropertyInfo piDepaId = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_ID" + count);
                                System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + count);
                                System.Reflection.PropertyInfo piAmount = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                piDepaId.SetValue(rdo, listSubDepa.First().DEPARTMENT_ID);
                                piDepaName.SetValue(rdo, listSubDepa.First().DEPARTMENT_NAME);
                                piAmount.SetValue(rdo, listSub.Sum(s => s.AMOUNT));
                                dicMediRdo[listSub.First().MEDICINE_TYPE_ID] = rdo;
                            }
                        }
                        count += 1;
                    }
                }

                if (IsNotNullOrEmpty(listSdoMate))
                {
                    foreach (var dic in dicDepartment)
                    {
                        var listSubDepa = listSdoMate.Where(o => o.DEPARTMENT_ID == dic.Value.ID).ToList();
                        if (IsNotNullOrEmpty(listSubDepa))
                        {
                            var GroupMate = listSubDepa.GroupBy(o => o.MATERIAL_TYPE_ID).ToList();
                            foreach (var group in GroupMate)
                            {
                                var listSub = group.ToList<Mrs00124SDO>();
                                if (dicMateRdo.ContainsKey(listSub.First().MATERIAL_TYPE_ID))
                                {
                                    System.Reflection.PropertyInfo pi = typeof(Mrs00124RDO).GetProperty("AMOUNT" + dic.Key);
                                    pi.SetValue(dicMateRdo[listSub.First().MATERIAL_TYPE_ID], listSub.Sum(s => s.AMOUNT));
                                }
                                else
                                {
                                    Mrs00124RDO rdo = new Mrs00124RDO();
                                    rdo.MATERIAL_TYPE_ID = listSub.First().MATERIAL_TYPE_ID;
                                    rdo.MATERIAL_TYPE_CODE = listSub.First().MATERIAL_TYPE_CODE;
                                    rdo.MATERIAL_TYPE_NAME = listSub.First().MATERIAL_TYPE_NAME;
                                    System.Reflection.PropertyInfo piDepaId = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_ID" + dic.Key);
                                    System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + dic.Key);
                                    System.Reflection.PropertyInfo piAmount = typeof(Mrs00124RDO).GetProperty("AMOUNT" + dic.Key);
                                    piDepaId.SetValue(rdo, listSubDepa.First().DEPARTMENT_ID);
                                    piDepaName.SetValue(rdo, listSubDepa.First().DEPARTMENT_NAME);
                                    piAmount.SetValue(rdo, listSub.Sum(s => s.AMOUNT));
                                    dicMateRdo[listSub.First().MATERIAL_TYPE_ID] = rdo;
                                }
                            }
                        }
                    }

                    var listDepartId = dicDepartment.Select(o => o.Value).ToList().Select(s => s.ID).ToList();
                    var listExist = listSdoMate.Where(o => (!listDepartId.Contains(o.DEPARTMENT_ID))).ToList();
                    var count = dicDepartment.Count + 2;
                    if (IsNotNullOrEmpty(listExist))
                    {
                        var Group = listExist.GroupBy(o => o.DEPARTMENT_ID).ToList();
                        foreach (var group in Group)
                        {
                            if (count > 20)
                            {
                                throw new DataMisalignedException("So luong khoa lon hon so luong co the xu ly, MRS00124");
                            }
                            var listSubDepa = group.ToList<Mrs00124SDO>();
                            HIS_DEPARTMENT depa = new HIS_DEPARTMENT();
                            depa.ID = listSubDepa.First().DEPARTMENT_ID;
                            depa.DEPARTMENT_NAME = listSubDepa.First().DEPARTMENT_NAME;
                            if (!dicDepartment.ContainsKey(count))
                            {
                                dicDepartment.Add(count, depa);
                            }

                            var GroupMedi = listSubDepa.GroupBy(o => o.MATERIAL_TYPE_ID).ToList();
                            foreach (var groupMedi in GroupMedi)
                            {
                                var listSub = groupMedi.ToList<Mrs00124SDO>();
                                if (dicMateRdo.ContainsKey(listSub.First().MATERIAL_TYPE_ID))
                                {
                                    System.Reflection.PropertyInfo pi = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                    pi.SetValue(dicMateRdo[listSub.First().MATERIAL_TYPE_ID], listSub.Sum(s => s.AMOUNT));
                                }
                                else
                                {
                                    Mrs00124RDO rdo = new Mrs00124RDO();
                                    rdo.MATERIAL_TYPE_ID = listSub.First().MATERIAL_TYPE_ID;
                                    rdo.MATERIAL_TYPE_CODE = listSub.First().MATERIAL_TYPE_CODE;
                                    rdo.MATERIAL_TYPE_NAME = listSub.First().MATERIAL_TYPE_NAME;
                                    System.Reflection.PropertyInfo piDepaId = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_ID" + count);
                                    System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + count);
                                    System.Reflection.PropertyInfo piAmount = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                    piDepaId.SetValue(rdo, listSubDepa.First().DEPARTMENT_ID);
                                    piDepaName.SetValue(rdo, listSubDepa.First().DEPARTMENT_NAME);
                                    piAmount.SetValue(rdo, listSub.Sum(s => s.AMOUNT));
                                    dicMateRdo[listSub.First().MATERIAL_TYPE_ID] = rdo;
                                }
                            }
                            count += 1;
                        }
                    }
                }

                foreach (var dic in dicDepartment)
                {
                    dicSingleData.Add("DEPARTMENT_NAME" + dic.Key, dic.Value.DEPARTMENT_NAME);
                }

                if (dicMediRdo.Count > 0)
                {
                    listRdoMedicine = dicMediRdo.Select(s => s.Value).ToList();
                }
                if (dicMateRdo.Count > 0)
                {
                    listRdoMaterial = dicMateRdo.Select(s => s.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listSdoMedi.Clear();
                listSdoMate.Clear();
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();
            }
        }

        //Xử lý cho trường hợp lấy mỗi thuốc
        private void ProcessListSdoIsMediTrue()
        {
            try
            {
                Dictionary<long, Mrs00124RDO> dicMediRdo = new Dictionary<long, Mrs00124RDO>();
                Dictionary<int, HIS_DEPARTMENT> dicDepartment = new Dictionary<int, HIS_DEPARTMENT>();
                if (IsNotNullOrEmpty(listSdoMedi))
                {
                    int count = 1;
                    var GroupDepa = listSdoMedi.GroupBy(g => g.DEPARTMENT_ID).ToList();
                    foreach (var group in GroupDepa)
                    {
                        if (count > 20)
                        {
                            throw new DataMisalignedException("So luong khoa lon hon so luong co the xu ly, MRS00124");
                        }
                        var listSubDepa = group.ToList<Mrs00124SDO>();
                        HIS_DEPARTMENT depa = new HIS_DEPARTMENT();
                        depa.ID = listSubDepa.First().DEPARTMENT_ID;
                        depa.DEPARTMENT_NAME = listSubDepa.First().DEPARTMENT_NAME;
                        if (!dicDepartment.ContainsKey(count))
                        {
                            dicDepartment.Add(count, depa);
                        }
                        var GroupMedi = listSubDepa.GroupBy(o => o.MEDICINE_TYPE_ID).ToList();
                        foreach (var groupMedi in GroupMedi)
                        {
                            var listSub = groupMedi.ToList<Mrs00124SDO>();
                            if (dicMediRdo.ContainsKey(listSub.First().MEDICINE_TYPE_ID))
                            {
                                System.Reflection.PropertyInfo pi = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                pi.SetValue(dicMediRdo[listSub.First().MEDICINE_TYPE_ID], listSub.Sum(s => s.AMOUNT));
                            }
                            else
                            {
                                Mrs00124RDO rdo = new Mrs00124RDO();
                                rdo.MEDICINE_TYPE_ID = listSub.First().MEDICINE_TYPE_ID;
                                rdo.MEDICINE_TYPE_CODE = listSub.First().MEDICINE_TYPE_CODE;
                                rdo.MEDICINE_TYPE_NAME = listSub.First().MEDICINE_TYPE_NAME;
                                System.Reflection.PropertyInfo piDepaId = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_ID" + count);
                                System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + count);
                                System.Reflection.PropertyInfo piAmount = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                piDepaId.SetValue(rdo, listSubDepa.First().DEPARTMENT_ID);
                                piDepaName.SetValue(rdo, listSubDepa.First().DEPARTMENT_NAME);
                                piAmount.SetValue(rdo, listSub.Sum(s => s.AMOUNT));
                                dicMediRdo[listSub.First().MEDICINE_TYPE_ID] = rdo;
                            }
                        }
                        count += 1;
                    }
                    foreach (var dic in dicDepartment)
                    {
                        dicSingleData.Add("DEPARTMENT_NAME" + dic.Key, dic.Value.DEPARTMENT_NAME);
                    }

                    if (dicMediRdo.Count > 0)
                    {
                        listRdoMedicine = dicMediRdo.Select(s => s.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listSdoMedi.Clear();
                listSdoMate.Clear();
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();
            }
        }

        //Xử lý cho trường hợp lấy mỗi vật tư
        private void ProcessListSdoIsMediFalse()
        {
            try
            {
                Dictionary<long, Mrs00124RDO> dicMateRdo = new Dictionary<long, Mrs00124RDO>();
                Dictionary<int, HIS_DEPARTMENT> dicDepartment = new Dictionary<int, HIS_DEPARTMENT>();
                if (IsNotNullOrEmpty(listSdoMate))
                {
                    int count = 1;
                    var Group = listSdoMate.GroupBy(o => o.DEPARTMENT_ID).ToList();
                    foreach (var group in Group)
                    {
                        if (count > 20)
                        {
                            throw new DataMisalignedException("So luong khoa lon hon so luong co the xu ly, MRS00124");
                        }
                        var listSubDepa = group.ToList<Mrs00124SDO>();
                        HIS_DEPARTMENT depa = new HIS_DEPARTMENT();
                        depa.ID = listSubDepa.First().DEPARTMENT_ID;
                        depa.DEPARTMENT_NAME = listSubDepa.First().DEPARTMENT_NAME;
                        if (!dicDepartment.ContainsKey(count))
                        {
                            dicDepartment.Add(count, depa);
                        }

                        var GroupMedi = listSubDepa.GroupBy(o => o.MATERIAL_TYPE_ID).ToList();
                        foreach (var groupMedi in GroupMedi)
                        {
                            var listSub = groupMedi.ToList<Mrs00124SDO>();
                            if (dicMateRdo.ContainsKey(listSub.First().MATERIAL_TYPE_ID))
                            {
                                System.Reflection.PropertyInfo pi = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                pi.SetValue(dicMateRdo[listSub.First().MATERIAL_TYPE_ID], listSub.Sum(s => s.AMOUNT));
                            }
                            else
                            {
                                Mrs00124RDO rdo = new Mrs00124RDO();
                                rdo.MATERIAL_TYPE_ID = listSub.First().MATERIAL_TYPE_ID;
                                rdo.MATERIAL_TYPE_CODE = listSub.First().MATERIAL_TYPE_CODE;
                                rdo.MATERIAL_TYPE_NAME = listSub.First().MATERIAL_TYPE_NAME;
                                System.Reflection.PropertyInfo piDepaId = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_ID" + count);
                                System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + count);
                                System.Reflection.PropertyInfo piAmount = typeof(Mrs00124RDO).GetProperty("AMOUNT" + count);
                                piDepaId.SetValue(rdo, listSubDepa.First().DEPARTMENT_ID);
                                piDepaName.SetValue(rdo, listSubDepa.First().DEPARTMENT_NAME);
                                piAmount.SetValue(rdo, listSub.Sum(s => s.AMOUNT));
                                dicMateRdo[listSub.First().MATERIAL_TYPE_ID] = rdo;
                            }
                        }
                        count += 1;
                    }

                    foreach (var dic in dicDepartment)
                    {
                        dicSingleData.Add("DEPARTMENT_NAME" + dic.Key, dic.Value.DEPARTMENT_NAME);
                    }

                    if (dicMateRdo.Count > 0)
                    {
                        listRdoMaterial = dicMateRdo.Select(s => s.Value).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
                listSdoMedi.Clear();
                listSdoMate.Clear();
                listRdoMedicine.Clear();
                listRdoMaterial.Clear();
            }
        }

        /// <summary>
        /// xu lý xuất cho khoa phòng
        /// </summary>
        /// <param name="paramGet"></param>
        /// <param name="ListExpMest"> danh sách phiếu xuất cho khoa phòng</param>
        private void ProcessListExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> ListExpMest)
        {
            try
            {
                if (IsNotNullOrEmpty(ListExpMest))
                {
                    int start = 0;
                    int count = ListExpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        var hisExpMests = ListExpMest.Skip(start).Take(limit).ToList();
                        List<long> listExpMestId = hisExpMests.Select(s => s.ID).ToList();
                        HisExpMestMedicineViewFilterQuery mediFilter = new HisExpMestMedicineViewFilterQuery();
                        mediFilter.EXP_MEST_IDs = listExpMestId;
                        mediFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine = new HisExpMestMedicineManager(paramGet).GetView(mediFilter);

                        HisExpMestMaterialViewFilterQuery mateFilter = new HisExpMestMaterialViewFilterQuery();
                        mateFilter.EXP_MEST_IDs = listExpMestId;
                        mateFilter.IS_EXPORT = true;
                        List<V_HIS_EXP_MEST_MATERIAL> ListExpMestMaterial = new HisExpMestMaterialManager(paramGet).GetView(mateFilter);

                        HisImpMestViewFilterQuery mobaFilter = new HisImpMestViewFilterQuery();
                        mobaFilter.MOBA_EXP_MEST_IDs = listExpMestId;
                        mobaFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST> ListMobaImpMest = new HisImpMestManager(paramGet).GetView(mobaFilter);

                        if (!paramGet.HasException)
                        {
                            ProcessListExpMestMedicine(hisExpMests, ListExpMestMedicine);
                            ProcessListExpMestMaterial(hisExpMests, ListExpMestMaterial);
                            ProcessListImpMoveBackDepaExpMest(paramGet, hisExpMests, ListMobaImpMest);
                        }
                        else
                        {
                            return;
                        }

                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }


        //Xử lý danh sách vật tư xuất cho khoa phòng
        private void ProcessListExpMestMaterial(List<V_HIS_EXP_MEST> hisExpMests, List<V_HIS_EXP_MEST_MATERIAL> ListExpMestMaterial)
        {
            try
            {
                if (IsNotNullOrEmpty(ListExpMestMaterial))
                {
                    var Group = hisExpMests.GroupBy(g => g.REQ_DEPARTMENT_ID).ToList();
                    foreach (var group in Group)
                    {
                        var listSub = group.ToList<V_HIS_EXP_MEST>();
                        var listExpMestId = listSub.Select(s => s.ID).ToList();
                        var hisExpMestMates = ListExpMestMaterial.Where(o => listExpMestId.Contains(o.EXP_MEST_ID ?? 0)).ToList();
                        var GroupMate = hisExpMestMates.GroupBy(o => o.MATERIAL_TYPE_ID).ToList();
                        foreach (var groupMate in GroupMate)
                        {
                            var listSubMate = groupMate.ToList<V_HIS_EXP_MEST_MATERIAL>();
                            listSdoMate.Add(new Mrs00124SDO(listSubMate, listSub.First().REQ_DEPARTMENT_ID));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //Xử lý nhập thu hồi các phiếu xuất cho khoa phòng
        private void ProcessListImpMoveBackDepaExpMest(CommonParam paramGet, List<V_HIS_EXP_MEST> hisDepaExpMests, List<V_HIS_IMP_MEST> ListMobaImpMest)
        {
            try
            {
                if (IsNotNullOrEmpty(ListMobaImpMest))
                {
                    int start = 0;
                    int count = ListMobaImpMest.Count;
                    while (count > 0)
                    {
                        int limit = (count <= ManagerConstant.MAX_REQUEST_LENGTH_PARAM) ? count : ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        List<V_HIS_IMP_MEST> mobaImpMests = ListMobaImpMest.Skip(start).Take(limit).ToList();
                        HisImpMestMedicineViewFilterQuery impMediFilter = new HisImpMestMedicineViewFilterQuery();
                        impMediFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MEDICINE> ListImpMestMedicines = new HisImpMestMedicineManager(paramGet).GetView(impMediFilter);

                        HisImpMestMaterialViewFilterQuery impMateFilter = new HisImpMestMaterialViewFilterQuery();
                        impMateFilter.IMP_MEST_IDs = mobaImpMests.Select(s => s.ID).ToList();
                        impMediFilter.IMP_MEST_STT_ID = IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT;
                        List<V_HIS_IMP_MEST_MATERIAL> ListImpMestMaterials = new HisImpMestMaterialManager(paramGet).GetView(impMateFilter);

                        if (!paramGet.HasException)
                        {
                            if (IsNotNullOrEmpty(ListImpMestMedicines))
                            {
                                var Group = hisDepaExpMests.GroupBy(g => g.REQ_DEPARTMENT_ID).ToList();
                                foreach (var group in Group)
                                {
                                    var listDepa = group.ToList<V_HIS_EXP_MEST>();
                                    var listEmpMestId = listDepa.Select(s => s.ID).ToList();
                                    var listMoba = mobaImpMests.Where(o => listEmpMestId.Contains(o.ID)).ToList();
                                    if (IsNotNullOrEmpty(listMoba))
                                    {
                                        var listImpMestId = listMoba.Select(s => s.ID).ToList();
                                        var hisImpMestMedicines = ListImpMestMedicines.Where(o => listImpMestId.Contains(o.IMP_MEST_ID)).ToList();
                                        if (IsNotNullOrEmpty(hisImpMestMedicines))
                                        {
                                            var GroupMedi = hisImpMestMedicines.GroupBy(g => g.MEDICINE_TYPE_ID).ToList();
                                            foreach (var item in GroupMedi)
                                            {
                                                var listSub = item.ToList<V_HIS_IMP_MEST_MEDICINE>();
                                                listSdoMedi.Add(new Mrs00124SDO(listSub, listDepa.First().REQ_DEPARTMENT_ID));
                                            }
                                        }
                                    }

                                }
                            }
                            if (IsNotNullOrEmpty(ListImpMestMaterials))
                            {
                                var Group = hisDepaExpMests.GroupBy(g => g.REQ_DEPARTMENT_ID).ToList();
                                foreach (var group in Group)
                                {
                                    var listDepa = group.ToList<V_HIS_EXP_MEST>();
                                    var listEmpMestId = listDepa.Select(s => s.ID).ToList();
                                    var listMoba = mobaImpMests.Where(o => listEmpMestId.Contains(o.MOBA_EXP_MEST_ID ?? 0)).ToList();
                                    if (IsNotNullOrEmpty(listMoba))
                                    {
                                        var listImpMestId = listMoba.Select(s => s.ID).ToList();
                                        var hisImpMestMaterials = ListImpMestMaterials.Where(o => listImpMestId.Contains(o.IMP_MEST_ID)).ToList();
                                        if (IsNotNullOrEmpty(hisImpMestMaterials))
                                        {
                                            var GroupMate = hisImpMestMaterials.GroupBy(g => g.MATERIAL_TYPE_ID).ToList();
                                            foreach (var item in GroupMate)
                                            {
                                                var listSub = item.ToList<V_HIS_IMP_MEST_MATERIAL>();
                                                listSdoMate.Add(new Mrs00124SDO(listSub, listDepa.First().REQ_DEPARTMENT_ID));
                                            }
                                        }
                                    }

                                }
                            }
                        }
                        start += ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                        count -= ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        //xử lý danh sách thuốc xuất đơn thuốc
        private void ProcessListExpMestMedicine(List<V_HIS_EXP_MEST> hisPrescriptions, List<V_HIS_EXP_MEST_MEDICINE> ListExpMestMedicine)
        {
            try
            {
                if (IsNotNullOrEmpty(ListExpMestMedicine))
                {
                    var Group = hisPrescriptions.GroupBy(g => g.REQ_DEPARTMENT_ID).ToList();
                    foreach (var group in Group)
                    {
                        var listSub = group.ToList<V_HIS_EXP_MEST>();
                        var listExpMestId = listSub.Select(s => s.ID).ToList();
                        var hisExpMestMedis = ListExpMestMedicine.Where(o => listExpMestId.Contains(o.EXP_MEST_ID ?? 0)).ToList();
                        var GroupMedi = hisExpMestMedis.GroupBy(o => o.MEDICINE_TYPE_ID).ToList();
                        foreach (var groupMedi in GroupMedi)
                        {
                            var listSubMedi = groupMedi.ToList<V_HIS_EXP_MEST_MEDICINE>();
                            listSdoMedi.Add(new Mrs00124SDO(listSubMedi, listSub.First().REQ_DEPARTMENT_ID));
                        }
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
                #region Cac the Single
                if (castFilter.TIME_FROM > 0)
                {
                    dicSingleTag.Add("EXP_TIME_FROM_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_FROM));
                }
                if (castFilter.TIME_TO > 0)
                {
                    dicSingleTag.Add("EXP_TIME_TO_STR", Inventec.Common.DateTime.Convert.TimeNumberToTimeString(castFilter.TIME_TO));
                }

                dicSingleTag.Add("MEDI_STOCK_NAME", MEDI_STOCK_NAME);
                #endregion

                ProcessDepartmentName(dicSingleTag);

                listRdoMedicine = listRdoMedicine.OrderBy(o => o.MEDICINE_TYPE_NAME).ToList();
                listRdoMaterial = listRdoMaterial.OrderBy(o => o.MATERIAL_TYPE_NAME).ToList();
                if (castFilter.IsMedicineType.HasValue)
                {
                    if (castFilter.IsMedicineType.Value)
                    {
                        objectTag.AddObjectData(store, "Medicines", listRdoMedicine);
                    }
                    else
                    {
                        objectTag.AddObjectData(store, "Materials", listRdoMaterial);
                    }
                }
                else
                {
                    objectTag.AddObjectData(store, "Medicines", listRdoMedicine);
                    objectTag.AddObjectData(store, "Materials", listRdoMaterial);
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }

        private void ProcessDepartmentName(Dictionary<string, object> dicSingleTag)
        {
            try
            {
                if (IsNotNullOrEmpty(listRdoMedicine) || IsNotNullOrEmpty(listRdoMaterial))
                {
                    var listTotal = new List<Mrs00124RDO>();
                    listTotal.AddRange(listRdoMedicine);
                    listTotal.AddRange(listRdoMaterial);
                    if (listTotal != null)
                    {
                        dicSingleTag.Add("DEPARTMENT_NAME1", listTotal.Select(o=>o.DEPARTMENT_NAME1).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o))?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME2", listTotal.Select(o => o.DEPARTMENT_NAME2).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME3", listTotal.Select(o => o.DEPARTMENT_NAME3).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME4", listTotal.Select(o => o.DEPARTMENT_NAME4).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME5", listTotal.Select(o => o.DEPARTMENT_NAME5).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME6", listTotal.Select(o => o.DEPARTMENT_NAME6).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME7", listTotal.Select(o => o.DEPARTMENT_NAME7).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME8", listTotal.Select(o => o.DEPARTMENT_NAME8).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME9", listTotal.Select(o => o.DEPARTMENT_NAME9).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME10", listTotal.Select(o => o.DEPARTMENT_NAME10).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME11", listTotal.Select(o => o.DEPARTMENT_NAME11).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME12", listTotal.Select(o => o.DEPARTMENT_NAME12).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME13", listTotal.Select(o => o.DEPARTMENT_NAME13).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME14", listTotal.Select(o => o.DEPARTMENT_NAME14).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME15", listTotal.Select(o => o.DEPARTMENT_NAME15).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME16", listTotal.Select(o => o.DEPARTMENT_NAME16).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME17", listTotal.Select(o => o.DEPARTMENT_NAME17).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME18", listTotal.Select(o => o.DEPARTMENT_NAME18).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME19", listTotal.Select(o => o.DEPARTMENT_NAME19).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        dicSingleTag.Add("DEPARTMENT_NAME20", listTotal.Select(o => o.DEPARTMENT_NAME20).ToList().FirstOrDefault(o => !String.IsNullOrEmpty(o)) ?? "");
                        //var lisDepa = new List<Mrs00124SDO>();
                        //lisDepa.AddRange(listSdoMate);
                        //lisDepa.AddRange(listSdoMedi);
                        //var groupdepa = lisDepa.GroupBy(o => o.DEPARTMENT_ID).ToList();
                        //int count = 1;
                        //while (count < groupdepa.Count + 1)
                        //{
                        //    foreach (var item in listTotal)
                        //    {
                        //        System.Reflection.PropertyInfo piDepaName = typeof(Mrs00124RDO).GetProperty("DEPARTMENT_NAME" + count);
                        //        var name = (string)piDepaName.GetValue(item);
                        //        if (!String.IsNullOrEmpty(name) && !dicSingleTag.ContainsKey("DEPARTMENT_NAME" + count))
                        //        {
                        //            dicSingleTag.Add("DEPARTMENT_NAME" + count, name);
                        //            count += 1;
                        //            break;
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Inventec.Common.Logging.LogSystem.Error(ex);
            }
        }
    }
}
