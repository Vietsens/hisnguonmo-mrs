using MOS.EFMODEL.DataModels;
using MRS.MANAGER.Config;
using System; 
using System.Collections.Generic; 
using System.Linq; 
using System.Text; 
using System.Threading.Tasks; 

namespace MRS.Processor.Mrs00484
{
    public class Mrs00484RDO
    {
        public string RESULT_DATE_STR { get; set; }

        public long? RESULT_TIME { get; set; }//trả kq, 
        public string VIR_PATIENT_NAME { get;  set; }
        public long SERE_SERV_ID { get; set; }
        public long PATIENT_ID { get; set; }
        public long TREATMENT_ID { get;  set;  }
        public string PATIENT_CODE { get;  set;  }
        public string TREATMENT_CODE { get;  set;  }
        public long TREATMENT_TYPE_ID { get; set; }
        public decimal? VIR_EXPEND_PRICE { get; set; }
        public string MALE_YEAR { get;  set;  }
        public string FEMALE_YEAR { get;  set;  }
        public string VIR_ADDRESS { get; set; }
        public string IS_BHYT { get; set; }
        public string HEIN_CARD_NUMBER { get; set; }
        public string ICD_CODE { get;  set;  }
        public string ICD_NAME { get;  set;  }
        public string ICD_ALL { get;  set;  }
        public string ICD_MAIN_TEXT { get;  set;  }
        public string IN_CODE { get;  set;  }	//so vao vien

        public string ROOM_NAME { get;  set;  }
        public string DEPARTMENT_NAME { get;  set; }
        public string INTRUCTION_TIME { get; set; }
        public long INTRUCTION_TIME_NUM { get; set; }
        public string SERVICE_REQ_CODE { get;  set;  }
        public string BARCODE { get;  set;  }
        public string SERVICE_NAME { get;  set;  }
        public string EXECUTE_USERNAME { get;  set;  }
        public string FINISH_TIME { get;  set;  }
        public long? FINISH_TIME_NUM { get;  set;  }
        public string REQUEST_USERNAME { get;  set;  }

        public long? SAMPLE_TIME { get; set; }//thời gian nhân mẫu, 

        public long? APPROVAL_TIME { get; set; }//duyệt mẫu, 

        public string SAMPLE_USERNAME { get; set; }//người lấy mẫu, 

        public string APPROVAL_USERNAME { get; set; }//duyệt mẫu...

        public long IN_TIME { get; set; }//ngày vào, 

        public long? OUT_TIME { get; set; }//ngày ra viện
        public long REQUEST_ROOM_ID { get; set; }
        public long? TDL_PATIENT_GENDER_ID { get; set; }
        public long? TDL_PATIENT_DOB { get; set; }
        public long? TDL_PATIENT_TYPE_ID { get; set; }
        public long? PATIENT_TYPE_ID { get; set; }
        public long SERVICE_ID { get; set; }
        public long RESULT_DATE { get; set; }

        public string MACHINE_CODE { get; set; }

        public string MACHINE_NAME { get; set; }

        public string EXECUTE_MACHINE_CODE { get; set; }

        public string EXECUTE_MACHINE_NAME { get; set; }

        public string TDL_PATIENT_TYPE_CODE { get; set; }

        public string TDL_PATIENT_TYPE_NAME { get; set; }

        public string PATIENT_TYPE_CODE { get; set; }

        public string PATIENT_TYPE_NAME { get; set; }

        public Dictionary<string, string> DIC_VALUE { get; set; }

        public string VALUE_1 { get; set; }
        public string VALUE_2 { get; set; }
        public string VALUE_3 { get; set; }
        public string VALUE_4 { get; set; }
        public string VALUE_5 { get; set; }
        public string VALUE_6 { get; set; }
        public string VALUE_7 { get; set; }
        public string VALUE_8 { get; set; }
        public string VALUE_9 { get; set; }
        public string VALUE_10 { get; set; }
        public string VALUE_11 { get; set; }
        public string VALUE_12 { get; set; }
        public string VALUE_13 { get; set; }
        public string VALUE_14 { get; set; }
        public string VALUE_15 { get; set; }
        public string VALUE_16 { get; set; }
        public string VALUE_17 { get; set; }
        public string VALUE_18 { get; set; }
        public string VALUE_19 { get; set; }
        public string VALUE_20 { get; set; }
        public string VALUE_21 { get; set; }
        public string VALUE_22 { get; set; }
        public string VALUE_23 { get; set; }
        public string VALUE_24 { get; set; }
        public string VALUE_25 { get; set; }
        public string VALUE_26 { get; set; }
        public string VALUE_27 { get; set; }
        public string VALUE_28 { get; set; }
        public string VALUE_29 { get; set; }
        public string VALUE_30 { get; set; }
        public string VALUE_31 { get; set; }
        public string VALUE_32 { get; set; }
        public string VALUE_33 { get; set; }
        public string VALUE_34 { get; set; }
        public string VALUE_35 { get; set; }
        public string VALUE_36 { get; set; }
        public string VALUE_37 { get; set; }
        public string VALUE_38 { get; set; }
        public string VALUE_39 { get; set; }
        public string VALUE_40 { get; set; }
        public string VALUE_41 { get; set; }
        public string VALUE_42 { get; set; }
        public string VALUE_43 { get; set; }
        public string VALUE_44 { get; set; }
        public string VALUE_45 { get; set; }
        public string VALUE_46 { get; set; }
        public string VALUE_47 { get; set; }
        public string VALUE_48 { get; set; }
        public string VALUE_49 { get; set; }
        public string VALUE_50 { get; set; }
        public string VALUE_51 { get; set; }
        public string VALUE_52 { get; set; }
        public string VALUE_53 { get; set; }
        public string VALUE_54 { get; set; }
        public string VALUE_55 { get; set; }
        public string VALUE_56 { get; set; }
        public string VALUE_57 { get; set; }
        public string VALUE_58 { get; set; }
        public string VALUE_59 { get; set; }
        public string VALUE_60 { get; set; }
        public string VALUE_61 { get; set; }
        public string VALUE_62 { get; set; }
        public string VALUE_63 { get; set; }
        public string VALUE_64 { get; set; }
        public string VALUE_65 { get; set; }
        public string VALUE_66 { get; set; }
        public string VALUE_67 { get; set; }
        public string VALUE_68 { get; set; }
        public string VALUE_69 { get; set; }
        public string VALUE_70 { get; set; }
        public string VALUE_71 { get; set; }
        public string VALUE_72 { get; set; }
        public string VALUE_73 { get; set; }
        public string VALUE_74 { get; set; }
        public string VALUE_75 { get; set; }
        public string VALUE_76 { get; set; }
        public string VALUE_77 { get; set; }
        public string VALUE_78 { get; set; }
        public string VALUE_79 { get; set; }
        public string VALUE_80 { get; set; }
        public string VALUE_81 { get; set; }
        public string VALUE_82 { get; set; }
        public string VALUE_83 { get; set; }
        public string VALUE_84 { get; set; }
        public string VALUE_85 { get; set; }
        public string VALUE_86 { get; set; }
        public string VALUE_87 { get; set; }
        public string VALUE_88 { get; set; }
        public string VALUE_89 { get; set; }
        public string VALUE_90 { get; set; }
        public string VALUE_91 { get; set; }
        public string VALUE_92 { get; set; }
        public string VALUE_93 { get; set; }
        public string VALUE_94 { get; set; }
        public string VALUE_95 { get; set; }
        public string VALUE_96 { get; set; }
        public string VALUE_97 { get; set; }
        public string VALUE_98 { get; set; }
        public string VALUE_99 { get; set; }
        public string VALUE_100 { get; set; }
        public string VALUE_101 { get; set; }
        public string VALUE_102 { get; set; }
        public string VALUE_103 { get; set; }
        public string VALUE_104 { get; set; }
        public string VALUE_105 { get; set; }
        public string VALUE_106 { get; set; }
        public string VALUE_107 { get; set; }
        public string VALUE_108 { get; set; }
        public string VALUE_109 { get; set; }
        public string VALUE_110 { get; set; }
        public string VALUE_111 { get; set; }
        public string VALUE_112 { get; set; }
        public string VALUE_113 { get; set; }
        public string VALUE_114 { get; set; }
        public string VALUE_115 { get; set; }
        public string VALUE_116 { get; set; }
        public string VALUE_117 { get; set; }
        public string VALUE_118 { get; set; }
        public string VALUE_119 { get; set; }
        public string VALUE_120 { get; set; }
        public string VALUE_121 { get; set; }
        public string VALUE_122 { get; set; }
        public string VALUE_123 { get; set; }
        public string VALUE_124 { get; set; }
        public string VALUE_125 { get; set; }
        public string VALUE_126 { get; set; }
        public string VALUE_127 { get; set; }
        public string VALUE_128 { get; set; }
        public string VALUE_129 { get; set; }
        public string VALUE_130 { get; set; }
        public string VALUE_131 { get; set; }
        public string VALUE_132 { get; set; }
        public string VALUE_133 { get; set; }
        public string VALUE_134 { get; set; }
        public string VALUE_135 { get; set; }
        public string VALUE_136 { get; set; }
        public string VALUE_137 { get; set; }
        public string VALUE_138 { get; set; }
        public string VALUE_139 { get; set; }
        public string VALUE_140 { get; set; }
        public string VALUE_141 { get; set; }
        public string VALUE_142 { get; set; }
        public string VALUE_143 { get; set; }
        public string VALUE_144 { get; set; }
        public string VALUE_145 { get; set; }
        public string VALUE_146 { get; set; }
        public string VALUE_147 { get; set; }
        public string VALUE_148 { get; set; }
        public string VALUE_149 { get; set; }
        public string VALUE_150 { get; set; }
        public string VALUE_151 { get; set; }
        public string VALUE_152 { get; set; }
        public string VALUE_153 { get; set; }
        public string VALUE_154 { get; set; }
        public string VALUE_155 { get; set; }
        public string VALUE_156 { get; set; }
        public string VALUE_157 { get; set; }
        public string VALUE_158 { get; set; }
        public string VALUE_159 { get; set; }
        public string VALUE_160 { get; set; }
        public string VALUE_161 { get; set; }
        public string VALUE_162 { get; set; }
        public string VALUE_163 { get; set; }
        public string VALUE_164 { get; set; }
        public string VALUE_165 { get; set; }
        public string VALUE_166 { get; set; }
        public string VALUE_167 { get; set; }
        public string VALUE_168 { get; set; }
        public string VALUE_169 { get; set; }
        public string VALUE_170 { get; set; }
        public string VALUE_171 { get; set; }
        public string VALUE_172 { get; set; }
        public string VALUE_173 { get; set; }
        public string VALUE_174 { get; set; }
        public string VALUE_175 { get; set; }
        public string VALUE_176 { get; set; }
        public string VALUE_177 { get; set; }
        public string VALUE_178 { get; set; }
        public string VALUE_179 { get; set; }
        public string VALUE_180 { get; set; }
        public string VALUE_181 { get; set; }
        public string VALUE_182 { get; set; }
        public string VALUE_183 { get; set; }
        public string VALUE_184 { get; set; }
        public string VALUE_185 { get; set; }
        public string VALUE_186 { get; set; }
        public string VALUE_187 { get; set; }
        public string VALUE_188 { get; set; }
        public string VALUE_189 { get; set; }
        public string VALUE_190 { get; set; }
        public string VALUE_191 { get; set; }
        public string VALUE_192 { get; set; }
        public string VALUE_193 { get; set; }
        public string VALUE_194 { get; set; }
        public string VALUE_195 { get; set; }
        public string VALUE_196 { get; set; }
        public string VALUE_197 { get; set; }
        public string VALUE_198 { get; set; }
        public string VALUE_199 { get; set; }
        public string VALUE_200 { get; set; }

        public Mrs00484RDO()
        {
            
        }
    }
    public class SSE
    {
        public long? ID { get; set; }
        public long? SERVICE_ID { get; set; }
        public long? SERE_SERV_ID { get; set; }
        public long? TEST_INDEX_ID { get; set; }
        public long? MACHINE_ID { get; set; }
        public string VALUE { get; set; }
    }
}
