using MOS.MANAGER.HisTreatment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MRS.Processor.Mrs00591
{
    public class Mrs00591Filter : HisTreatmentFilterQuery
    {
        public List<string> DTT { get; set; } //Đục thủy tinh thể
        public List<string> GLU { get; set; } //Glucôm
        public List<string> TKX { get; set; } //Tật khúc xạ
        public List<string> VKMC { get; set; } //Viêm kết mạc (cấp, mãn)
        public List<string> MON { get; set; } //Mộng thịt
        public List<string> QUA { get; set; } //Quặm
        public List<string> VLG { get; set; } //Viêm loét giác mạc
        public List<string> VTL { get; set; } //Viêm tắc lệ bộ
        public List<string> CHA { get; set; } //Chắp
        public List<string> LEO { get; set; } //Lẹo
        public List<string> LAC { get; set; } //Lắc mắt
        public List<string> CTM { get; set; } //Chấn thương mắt
        public List<string> DVG { get; set; } //Dị vật giác mạc
        public List<string> DVK { get; set; } //Dị vật kết mạc
        public List<string> XHT { get; set; } //Xuất huyết tiền phòng
        public List<string> UKM { get; set; } //U kết mạc
        public List<string> UMM { get; set; } //U mi mắt
        public List<string> UHM { get; set; } //U hốc mắt
        public List<string> VMB { get; set; } //Viêm màng bồ đào
        public List<string> VKMS { get; set; } //Viêm kết mạc sơ sinh
        public List<string> BDM { get; set; } //Bệnh đáy mắt
        public List<string> VBM { get; set; } //Viêm bờ mi
        public List<string> BON { get; set; } //Bỏng mắt
        public List<string> VTH { get; set; } //Viêm tấy hốc mắt

        public List<long> DEPARTMENT_IDs { get; set; }
        public List<long> EXAM_ROOM_IDs { get; set; }
    }
}
