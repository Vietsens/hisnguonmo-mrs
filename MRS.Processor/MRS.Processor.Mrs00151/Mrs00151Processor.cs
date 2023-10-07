using MOS.MANAGER.HisRoom;
using MOS.MANAGER.HisImpMestMedicine;
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
using MOS.Filter;
using MRS.MANAGER.Base;
using MRS.MANAGER.Config;
using MRS.SDO;
using MOS.MANAGER.HisImpMest;
using MRS.MANAGER.Core.MrsReport;

namespace MRS.Processor.Mrs00151
{
    internal class Mrs00151Processor : AbstractProcessor
    {
        List<VSarReportMrs00151RDO> _listSereServRdo1 = new List<VSarReportMrs00151RDO>();
        List<VSarReportMrs00151RDO> _listSereServRdo2 = new List<VSarReportMrs00151RDO>();
        List<VSarReportMrs00151RDO> _listSereServRdo3 = new List<VSarReportMrs00151RDO>();
        Mrs00151Filter CastFilter;
        private string DEPARTMENT_NAME;
        private string ROOM_NAME;

        public Mrs00151Processor(CommonParam param, string reportTypeCode)
            : base(param, reportTypeCode)
        {
        }

        public override Type FilterType()
        {
            return typeof(Mrs00151Filter);
        }

        protected override bool GetData()
        {
            var result = false;
            try
            {
                var paramGet = new CommonParam();
                CastFilter = (Mrs00151Filter)this.reportFilter;

                var departmentViews = new MOS.MANAGER.HisDepartment.HisDepartmentManager(paramGet).GetById(CastFilter.DEPARTMENT_ID);
                DEPARTMENT_NAME = departmentViews.DEPARTMENT_NAME;
                //--------------------------------------------------------------------------------------------------
                if (CastFilter.DEPARTMENT_ROOM_ID.HasValue)
                {
                    var roomViews = new MOS.MANAGER.HisRoom.HisRoomManager(paramGet).GetView(new HisRoomViewFilterQuery() { ID = CastFilter.DEPARTMENT_ROOM_ID.Value }).First();
                    ROOM_NAME = roomViews.ROOM_NAME;
                }
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST
                var metyFilterImpMest = new HisImpMestViewFilterQuery
                {
                    IMP_TIME_FROM = CastFilter.DATE_FROM,
                    IMP_TIME_TO = CastFilter.DATE_TO,
                    IMP_MEST_STT_IDs = new List<long> { IMSys.DbConfig.HIS_RS.HIS_IMP_MEST_STT.ID__IMPORT }
                };
                var listImpMestViews = new MOS.MANAGER.HisImpMest.HisImpMestManager(paramGet).GetView(metyFilterImpMest);
                var listMobaImpMestViews = listImpMestViews.Where(o => o.MOBA_EXP_MEST_ID > 0 && o.REQ_DEPARTMENT_ID == CastFilter.DEPARTMENT_ID).ToList();
                //-------------------------------------------------------------------------------------------------- V_HIS_IMP_MEST_MEDICINE
                var listImpMestIds2 = listMobaImpMestViews.Select(s => s.ID).ToList();
                var listImpMestMedicineViews = new List<V_HIS_IMP_MEST_MEDICINE>();
                var skip = 0;
                while (listImpMestIds2.Count - skip > 0)
                {
                    var listIds = listImpMestIds2.Skip(skip).Take(ManagerConstant.MAX_REQUEST_LENGTH_PARAM).ToList();
                    skip = skip + ManagerConstant.MAX_REQUEST_LENGTH_PARAM;
                    var metyFilterImpMestMedicine = new HisImpMestMedicineViewFilterQuery
                    {
                        IMP_MEST_IDs = listIds,
                    };
                    var impMestMedicineView = new MOS.MANAGER.HisImpMestMedicine.HisImpMestMedicineManager(paramGet).GetView(metyFilterImpMestMedicine);
                    listImpMestMedicineViews.AddRange(impMestMedicineView);
                }
                //--------------------------------------------------------------------------------------------------==============================================================="); 

                ProcessFilterData(listImpMestMedicineViews);

                result = true;
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

            dicSingleTag.Add("DATE_FROM", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_FROM));
            dicSingleTag.Add("DATE_TO", Inventec.Common.DateTime.Convert.TimeNumberToDateString(CastFilter.DATE_TO));
            dicSingleTag.Add("DEPARTMENT_NAME", DEPARTMENT_NAME);
            dicSingleTag.Add("ROOM_NAME", ROOM_NAME);
            dicSingleTag.Add("TOTAL_ALL_PRICE_MEDICINE_NEUROLOGICAL", _listSereServRdo1.Sum(s => s.TOTAL_PRICE));
            dicSingleTag.Add("TOTAL_ALL_PRICE_MEDICINE_ADDICTIVE", _listSereServRdo2.Sum(s => s.TOTAL_PRICE));
            dicSingleTag.Add("TOTAL_ALL_PRICE_MEDICINE_USUALLY", _listSereServRdo3.Sum(s => s.TOTAL_PRICE));
            var totalAllPrice = _listSereServRdo1.Sum(s => s.TOTAL_PRICE) +
                                _listSereServRdo2.Sum(s => s.TOTAL_PRICE) +
                                _listSereServRdo3.Sum(s => s.TOTAL_PRICE);
            dicSingleTag.Add("TOTAL_ALL_PRICE", totalAllPrice);
            dicSingleTag.Add("TOTAL_ALL_PRICE_TO_STRING", Inventec.Common.String.Convert.CurrencyToVneseString(totalAllPrice.ToString()));

            objectTag.AddObjectData(store, "Report1", _listSereServRdo1);
            objectTag.AddObjectData(store, "Report2", _listSereServRdo2);
            objectTag.AddObjectData(store, "Report3", _listSereServRdo3);

        }



        private void ProcessFilterData(List<V_HIS_IMP_MEST_MEDICINE> listImpMestMedicines)
        {
            //huong than
            var listNeurological = listImpMestMedicines.Where(s => s.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__HT).ToList();
            //gay nghien
            var listIsActive = listImpMestMedicines.Where(s => s.MEDICINE_GROUP_ID == IMSys.DbConfig.HIS_RS.HIS_MEDICINE_GROUP.ID__GN).ToList();
            //thuoc thuong
            var listMedicine = listImpMestMedicines.Where(s => !listNeurological.Contains(s) && !listIsActive.Contains(s)).ToList();

            var list = new List<List<V_HIS_IMP_MEST_MEDICINE>>
            {
                listNeurological,
                listIsActive,
                listMedicine
            };

            var number = 1;
            var numberOrder = 0;
            foreach (var a in list)
            {
                var listImpMestMedicineGroupByMedycineTypeIds = a.GroupBy(s => s.MEDICINE_TYPE_ID).ToList();
                foreach (var impMestMedicines in listImpMestMedicineGroupByMedycineTypeIds)
                {
                    var listImpMestMedicneGroupByPrices = impMestMedicines.GroupBy(s => s.PRICE).ToList();
                    foreach (var listImpMestMedicneGroupByPrice in listImpMestMedicneGroupByPrices)
                    {
                        var listImpMestMedicineGroupByNationals = listImpMestMedicneGroupByPrice.GroupBy(s => s.NATIONAL_NAME).ToList();
                        foreach (var listImpMestMedicineGroupByNational in listImpMestMedicineGroupByNationals)
                        {
                            numberOrder = numberOrder + 1;
                            var price = listImpMestMedicineGroupByNational.First().IMP_PRICE;
                            var amount = listImpMestMedicineGroupByNational.Sum(s => s.AMOUNT);
                            var rdo = new VSarReportMrs00151RDO
                            {
                                NUMBER_ORDER = numberOrder,
                                MEDICINE_TYPE_NAME = listImpMestMedicineGroupByNational.First().MEDICINE_TYPE_NAME,
                                SERVICE_UNIT_NAME = listImpMestMedicineGroupByNational.First().SERVICE_UNIT_NAME,
                                NATIONAL_NAME = listImpMestMedicineGroupByNational.First().NATIONAL_NAME,
                                PRICE = price,
                                AMOUNT = amount,
                                TOTAL_PRICE = price * amount
                            };
                            if (number == 1)
                                _listSereServRdo1.Add(rdo);
                            else if (number == 2)
                                _listSereServRdo2.Add(rdo);
                            else if (number == 3)
                                _listSereServRdo3.Add(rdo);
                        };
                    };
                };
                number = number + 1;
            };
        }
    }
}
