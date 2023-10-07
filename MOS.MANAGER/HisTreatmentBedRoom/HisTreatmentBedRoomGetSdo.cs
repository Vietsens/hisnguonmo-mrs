using AutoMapper;
using Inventec.Common.Logging;
using Inventec.Core;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.Base;
using MOS.MANAGER.HisTreatment;
using MOS.SDO;
using System;
using System.Linq;
using System.Collections.Generic;
using MOS.MANAGER.HisServiceReq;

namespace MOS.MANAGER.HisTreatmentBedRoom
{
    partial class HisTreatmentBedRoomGet : BusinessBase
    {
        internal List<HisTreatmentBedRoomViewSDO> GetViewSdoCurrentInByBedRoomId(long bedRoomId)
        {
            try
            {
                List<HisTreatmentBedRoomViewSDO> result = null;
                List<V_HIS_TREATMENT_BED_ROOM> treatmentBedRooms = this.GetViewCurrentIn(bedRoomId, null);

                if (IsNotNullOrEmpty(treatmentBedRooms))
                {
                    Mapper.CreateMap<V_HIS_TREATMENT_BED_ROOM, HisTreatmentBedRoomViewSDO>();
                    HisTreatmentGet treatmentGet = new HisTreatmentGet();
                    List<long> treatmentIds = treatmentBedRooms.Select(o => o.TREATMENT_ID).ToList();
                    long startDay = Inventec.Common.DateTime.Get.StartDay().Value;
                    long endDay = Inventec.Common.DateTime.Get.EndDay().Value;

                    List<long> serviceReqTypeIds = new List<long>()
                    {
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONK,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONM,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONNT,
                        IMSys.DbConfig.HIS_RS.HIS_SERVICE_REQ_TYPE.ID__DONTT,
                    };

                    List<HIS_SERVICE_REQ> todayPres = new HisServiceReqGet().GetByInstructionTime(startDay, endDay, serviceReqTypeIds, treatmentIds);
                    List<V_HIS_TREATMENT_FEE_1> treatmentFees = null;

                    //Neu ko co cau hinh canh bao thi tra ve luon du lieu
                    if (ExeCFG.WARNING_UNPAID_AMOUNT__BED_ROOM.HasValue && ExeCFG.WARNING_UNPAID_AMOUNT__BED_ROOM.Value >= 0)
                    {
                        treatmentFees = treatmentGet.GetFeeView1ByIds(treatmentIds);
                    }
                    result = new List<HisTreatmentBedRoomViewSDO>();


                    foreach (V_HIS_TREATMENT_BED_ROOM treatmentBedRoom in treatmentBedRooms)
                    {
                        HisTreatmentBedRoomViewSDO sdo = Mapper.Map<HisTreatmentBedRoomViewSDO>(treatmentBedRoom);
                        if (ExeCFG.WARNING_UNPAID_AMOUNT__BED_ROOM.HasValue && ExeCFG.WARNING_UNPAID_AMOUNT__BED_ROOM.Value >= 0)
                        {
                            sdo.UnpaidAmount = treatmentGet.GetUnpaid(treatmentBedRoom.TREATMENT_ID, treatmentFees);
                            sdo.IsUnpaidWarning = sdo.UnpaidAmount.HasValue && sdo.UnpaidAmount.Value > ExeCFG.WARNING_UNPAID_AMOUNT__BED_ROOM.Value;
                        }
                        sdo.HasTodayPrescription = IsNotNullOrEmpty(todayPres) ? todayPres.Where(o => o.TREATMENT_ID == sdo.TREATMENT_ID).Any() : false;
                        result.Add(sdo);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                LogSystem.Error(ex);
                param.HasException = true;
                return null;
            }
        }
    }
}
