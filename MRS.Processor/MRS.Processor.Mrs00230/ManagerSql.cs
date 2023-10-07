using Inventec.Core;
using Inventec.Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MRS.MANAGER.Base;
using MOS.EFMODEL;
using MOS.EFMODEL.DataModels;
using MOS.MANAGER.HisExpMestMaterial;
using MOS.MANAGER.HisExpMestMedicine;
using MOS.DAO.Sql;
using System.Data;
using MRS.MANAGER.Config;

namespace MRS.Processor.Mrs00230
{
    public class ManagerSql
    {

        public List<COUNT_IN> GetVaoVien(Mrs00230Filter filter)
        {
            List<COUNT_IN> result = new List<COUNT_IN>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- tong so benh nhan vao vien\n");
            query += string.Format("select\n");
            query += string.Format("trea.icd_code,\n");
            query += string.Format("sum(1) total_vao_vien\n");

            query += string.Format("from his_treatment trea\n");
            query += string.Format("where 1=1\n");

            if (filter.DEPARTMENT_ID != null)
            {
                query += string.Format("and trea.last_department_id = {0}\n", filter.DEPARTMENT_ID);
            }


            if (filter.DEPARTMENT_IDs != null)
            {
                query += string.Format("and trea.last_department_id in ({0})\n", string.Join(",", filter.DEPARTMENT_IDs));
            }


            if (filter.TDL_PATIENT_TYPE_IDs != null)
            {
                query += string.Format("and trea.TDL_PATIENT_TYPE_ID in ({0})\n", string.Join(",", filter.TDL_PATIENT_TYPE_IDs));
            }

            query += string.Format("and trea.in_time between {0} and {1}\n", filter.DATE_TIME_FROM, filter.DATE_TIME_TO);
            query += string.Format("group by trea.icd_code\n");
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<COUNT_IN>(paramGet, query);
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00230");

            return result;
        }

        public List<ICD_GROUP_DETAIL> GetIcdGroupDetail(Mrs00230Filter filter)
        {
            List<ICD_GROUP_DETAIL> result = new List<ICD_GROUP_DETAIL>();
            CommonParam paramGet = new CommonParam();
            string query = "";
            query += string.Format("-- tong so benh nhan vao vien\n");
            query += string.Format(@"select (case
when substr(icd.icd_code,1,3) ='A00' then '12'
when substr(icd.icd_code,1,3) ='A01' then '13'
when substr(icd.icd_code,1,3) ='A03' then '14'
when substr(icd.icd_code,1,3) ='A06' then '15'
when substr(icd.icd_code,1,3) ='A09' then '16'
when substr(icd.icd_code,1,3) ='A02' or substr(icd.icd_code,1,3) between 'A04' and 'A05' or substr(icd.icd_code,1,3) between 'A07' and 'A08' then '17'
when substr(icd.icd_code,1,3) between 'A15' and 'A16' then '18'
when substr(icd.icd_code,1,3) between 'A17' and 'A19' then '19'
when substr(icd.icd_code,1,3) ='A20' then '20'
when substr(icd.icd_code,1,3) ='A23' then '21'
when substr(icd.icd_code,1,3) ='A30' then '22'
when substr(icd.icd_code,1,3) ='A33' then '23'
when substr(icd.icd_code,1,3) between 'A34' and 'A35' then '24'
when substr(icd.icd_code,1,3) ='A36' then '25'
when substr(icd.icd_code,1,3) ='A37' then '26'
when substr(icd.icd_code,1,3) ='A39' then '27'
when substr(icd.icd_code,1,3) between 'A40' and 'A41' then '28'
when substr(icd.icd_code,1,3) between 'A21' and 'A22' or substr(icd.icd_code,1,3) between 'A24' and 'A28' or substr(icd.icd_code,1,3) between 'A31' and 'A32' or substr(icd.icd_code,1,3) ='A38' or substr(icd.icd_code,1,3) between 'A42' and 'A49' then '29'
when substr(icd.icd_code,1,3) ='A49' then '30'
when substr(icd.icd_code,1,3) ='A50' then '31'
when substr(icd.icd_code,1,3) ='A51' then '32'
when substr(icd.icd_code,1,3) ='A54' then '33'
when substr(icd.icd_code,1,3) between 'A55' and 'A56' then '34'
when substr(icd.icd_code,1,3) between 'A57' and 'A64' then '35'
when substr(icd.icd_code,1,3) ='A68' then '36'
when substr(icd.icd_code,1,3) ='A71' then '37'
when substr(icd.icd_code,1,3) ='A75' then '38'
when substr(icd.icd_code,1,3) ='A80' then '39'
when substr(icd.icd_code,1,3) ='A82' then '40'
when substr(icd.icd_code,1,3) between 'A83' and 'A86' then '41'
when substr(icd.icd_code,1,3) ='A95' then '42'
when substr(icd.icd_code,1,3) between 'A90' and 'A94' or substr(icd.icd_code,1,3) between 'A96' and 'A99' then '43'
when substr(icd.icd_code,1,3) ='B00' then '44'
when substr(icd.icd_code,1,3) between 'B01' and 'B02' then '45'
when substr(icd.icd_code,1,3) ='B05' then '46'
when substr(icd.icd_code,1,3) ='B06' then '47'
when substr(icd.icd_code,1,3) ='B16' then '48'
when substr(icd.icd_code,1,3) ='B15' or substr(icd.icd_code,1,3) between 'B17' and 'B19' then '49'
when substr(icd.icd_code,1,3) between 'B20' and 'B24' then '50'
when substr(icd.icd_code,1,3) ='B16' then '51'
when substr(icd.icd_code,1,3) ='A81' or substr(icd.icd_code,1,3) between 'A87' and 'A89' or substr(icd.icd_code,1,3) between 'B03' and 'B04' or substr(icd.icd_code,1,3) between 'B07' and 'B09' or substr(icd.icd_code,1,3) ='B25' or substr(icd.icd_code,1,3) between 'B27' and 'b34' then '52'
when substr(icd.icd_code,1,3) between 'B35' and 'B49' then '53'
when substr(icd.icd_code,1,3) between 'B50' and 'B54' then '54'
when substr(icd.icd_code,1,3) ='B55' then '55'
when substr(icd.icd_code,1,3) between 'B56' and 'B57' then '56'
when substr(icd.icd_code,1,3) ='B65' then '57'
when substr(icd.icd_code,1,3) ='B66' then '58'
when substr(icd.icd_code,1,3) ='B67' then '59'
when substr(icd.icd_code,1,3) ='B72' then '60'
when substr(icd.icd_code,1,3) ='B73' then '61'
when substr(icd.icd_code,1,3) ='B74' then '62'
when substr(icd.icd_code,1,3) ='B76' then '63'
when substr(icd.icd_code,1,3) between 'B68' and 'B71' or substr(icd.icd_code,1,3) ='B75' or substr(icd.icd_code,1,3) between 'B77' and 'B83' then '64'
when substr(icd.icd_code,1,3) ='B90' then '65'
when substr(icd.icd_code,1,3) ='B91' then '66'
when substr(icd.icd_code,1,3) ='B92' then '67'
when substr(icd.icd_code,1,3) between 'A65' and 'A67' or substr(icd.icd_code,1,3) between 'A69' and 'A70' or substr(icd.icd_code,1,3) ='A74' or substr(icd.icd_code,1,3) between 'A77' and 'A79' or substr(icd.icd_code,1,3) between 'B58' and 'B64' or substr(icd.icd_code,1,3) between 'B85' and 'B89' or substr(icd.icd_code,1,3) ='B94' or substr(icd.icd_code,1,3) ='B99' then '68'
when substr(icd.icd_code,1,3) between 'C00' and 'C14' then '70'
when substr(icd.icd_code,1,3) ='C15' then '71'
when substr(icd.icd_code,1,3) ='C16' then '72'
when substr(icd.icd_code,1,3) ='C18' then '73'
when substr(icd.icd_code,1,3) between 'C19' and 'C21' then '74'
when substr(icd.icd_code,1,3) ='C22' then '75'
when substr(icd.icd_code,1,3) ='C25' then '76'
when substr(icd.icd_code,1,3) ='C17' or substr(icd.icd_code,1,3) between 'C23' and 'c24' or substr(icd.icd_code,1,3) ='c26' then '77'
when substr(icd.icd_code,1,3) ='C32' then '78'
when substr(icd.icd_code,1,3) between 'C33' and 'C34' then '79'
when substr(icd.icd_code,1,3) between 'C30' and 'C31' or substr(icd.icd_code,1,3) between 'C37' and 'c39' then '80'
when substr(icd.icd_code,1,3) between 'C40' and 'C41' then '81'
when substr(icd.icd_code,1,3) ='C43' then '82'
when substr(icd.icd_code,1,3) ='C44' then '83'
when substr(icd.icd_code,1,3) between 'C45' and 'C49' then '84'
when substr(icd.icd_code,1,3) ='C50' then '85'
when substr(icd.icd_code,1,3) between 'C51' and 'C52' then '86'
when substr(icd.icd_code,1,3) ='C53' then '87'
when substr(icd.icd_code,1,3) between 'C54' and 'C55' then '88'
when substr(icd.icd_code,1,3) ='C61' then '89'
when substr(icd.icd_code,1,3) ='C60' or substr(icd.icd_code,1,3) between 'C62' and 'C63' then '90'
when substr(icd.icd_code,1,3) ='C67' then '91'
when substr(icd.icd_code,1,3) between 'C64' and 'C66' or substr(icd.icd_code,1,3) ='C68' then '92'
when substr(icd.icd_code,1,3) ='C69' then '93'
when substr(icd.icd_code,1,3) ='C71' then '94'
when substr(icd.icd_code,1,3) ='C70' or substr(icd.icd_code,1,3) ='C72' then '95'
when substr(icd.icd_code,1,3) between 'C73' and 'C80' or substr(icd.icd_code,1,3) ='C97' then '96'
when substr(icd.icd_code,1,3) ='C81' then '97'
when substr(icd.icd_code,1,3) between 'C82' and 'C85' then '98'
when substr(icd.icd_code,1,3) between 'C91' and 'C95' then '99'
when substr(icd.icd_code,1,3) between 'C88' and 'C89' or substr(icd.icd_code,1,3) ='C96' then '100'
when substr(icd.icd_code,1,3) ='D06' then '101'
when substr(icd.icd_code,1,3) between 'D22' and 'D23' then '102'
when substr(icd.icd_code,1,3) ='D24' then '103'
when substr(icd.icd_code,1,3) ='D25' then '104'
when substr(icd.icd_code,1,3) ='D27' then '105'
when substr(icd.icd_code,1,3) ='D30' then '106'
when substr(icd.icd_code,1,3) ='D33' then '107'
when substr(icd.icd_code,1,3) between 'D00' and 'D05' or substr(icd.icd_code,1,3) between 'D07' and 'D21' or substr(icd.icd_code,1,3) ='D26' or substr(icd.icd_code,1,3) between 'D28' and 'D29' or substr(icd.icd_code,1,3) between 'D31' and 'D32' or substr(icd.icd_code,1,3) between 'D34' and 'D48' then '108'
when substr(icd.icd_code,1,3) ='D50' then '110'
when substr(icd.icd_code,1,3) between 'D51' and 'D64' then '111'
when substr(icd.icd_code,1,3) between 'D65' and 'D77' then '112'
when substr(icd.icd_code,1,3) between 'D80' and 'D89' then '113'
when substr(icd.icd_code,1,3) between 'E00' and 'E02' then '115'
when substr(icd.icd_code,1,3) ='E05' then '116'
when substr(icd.icd_code,1,3) between 'E03' and 'E04' or substr(icd.icd_code,1,3) between 'E06' and 'E07' then '117'
when substr(icd.icd_code,1,3) between 'E10' and 'E14' then '118'
when substr(icd.icd_code,1,3) between 'E40' and 'E46' then '119'
when substr(icd.icd_code,1,3) ='E50' then '120'
when substr(icd.icd_code,1,3) between 'E51' and 'E56' then '121'
when substr(icd.icd_code,1,3) ='E64' then '122'
when substr(icd.icd_code,1,3) ='E66' then '123'
when substr(icd.icd_code,1,3) ='E86' then '124'
when substr(icd.icd_code,1,3) between 'E15' and 'E35' or substr(icd.icd_code,1,3) between 'E58' and 'E63' or substr(icd.icd_code,1,3) between 'E65' and 'E67' or substr(icd.icd_code,1,3) between 'E85' and 'E87' or substr(icd.icd_code,1,3) ='E90' then '125'
when substr(icd.icd_code,1,3) between 'F00' and 'F03' then '127'
when substr(icd.icd_code,1,3) ='F10' then '128'
when substr(icd.icd_code,1,3) between 'F11' and 'F19' then '129'
when substr(icd.icd_code,1,3) between 'F20' and 'F29' then '130'
when substr(icd.icd_code,1,3) between 'F30' and 'F39' then '131'
when substr(icd.icd_code,1,3) between 'F40' and 'F48' then '132'
when substr(icd.icd_code,1,3) between 'F70' and 'F79' then '133'
when substr(icd.icd_code,1,3) between 'F04' and 'F09' or substr(icd.icd_code,1,3) between 'F50' and 'F69' or substr(icd.icd_code,1,3) between 'F80' and 'F99' then '134'
when substr(icd.icd_code,1,3) between 'G00' and 'G09' then '136'
when substr(icd.icd_code,1,3) ='G20' then '137'
when substr(icd.icd_code,1,3) ='G30' then '138'
when substr(icd.icd_code,1,3) ='G35' then '139'
when substr(icd.icd_code,1,3) between 'G40' and 'G41' then '140'
when substr(icd.icd_code,1,3) between 'G43' and 'G44' then '141'
when substr(icd.icd_code,1,3) ='G45' then '142'
when substr(icd.icd_code,1,3) between 'G50' and 'G59' then '143'
when substr(icd.icd_code,1,3) between 'G80' and 'G83' then '144'
when substr(icd.icd_code,1,3) between 'G10' and 'G13' or substr(icd.icd_code,1,3) between 'G21' and 'G26' or substr(icd.icd_code,1,3) between 'G31' and 'G32' or substr(icd.icd_code,1,3) between 'G36' and 'G37' or substr(icd.icd_code,1,3) between 'G46' and 'G47' or substr(icd.icd_code,1,3) between 'G60' and 'G73' or substr(icd.icd_code,1,3) between 'G90' and 'G99' then '145'
when substr(icd.icd_code,1,3) between 'H00' and 'H01' then '147'
when substr(icd.icd_code,1,3) between 'H10' and 'H13' then '148'
when substr(icd.icd_code,1,3) between 'H15' and 'H19' then '149'
when substr(icd.icd_code,1,3) between 'H25' and 'H28' then '150'
when substr(icd.icd_code,1,3) ='H33' then '151'
when substr(icd.icd_code,1,3) between 'H40' and 'H42' then '152'
when substr(icd.icd_code,1,3) between 'H49' and 'H50' then '153'
when substr(icd.icd_code,1,3) ='H52' then '154'
when substr(icd.icd_code,1,3) ='H54' then '155'
when substr(icd.icd_code,1,3) between 'H30' and 'H32' or substr(icd.icd_code,1,3) between 'H02' and 'H22' or substr(icd.icd_code,1,3) between 'H34' and 'H36' or substr(icd.icd_code,1,3) between 'H43' and 'H48' or substr(icd.icd_code,1,3) ='H51' or substr(icd.icd_code,1,3) between 'H53' and 'H55' or substr(icd.icd_code,1,3) ='H59' then '156'
when substr(icd.icd_code,1,3) between 'H65' and 'H75' then '158'
when substr(icd.icd_code,1,3) between 'H90' and 'H91' then '159'
when substr(icd.icd_code,1,3) between 'H60' and 'H62' or substr(icd.icd_code,1,3) between 'H80' and 'H83' or substr(icd.icd_code,1,3) between 'H92' and 'H95' then '160'
when substr(icd.icd_code,1,3) between 'I00' and 'I02' then '162'
when substr(icd.icd_code,1,3) between 'I05' and 'I09' then '163'
when substr(icd.icd_code,1,3) ='I10' then '164'
when substr(icd.icd_code,1,3) between 'I11' and 'I15' then '165'
when substr(icd.icd_code,1,3) between 'I21' and 'I22' then '166'
when substr(icd.icd_code,1,3) ='I20' or substr(icd.icd_code,1,3) between 'I23' and 'I25' then '167'
when substr(icd.icd_code,1,3) ='I26' then '168'
when substr(icd.icd_code,1,3) between 'I44' and 'I49' then '169'
when substr(icd.icd_code,1,3) ='I50' then '170'
when substr(icd.icd_code,1,3) between 'I27' and 'I43' or substr(icd.icd_code,1,3) between 'i51' and 'I52' then '171'
when substr(icd.icd_code,1,3) between 'I60' and 'I62' then '172'
when substr(icd.icd_code,1,3) ='I63' then '173'
when substr(icd.icd_code,1,3) ='I64' then '174'
when substr(icd.icd_code,1,3) between 'I65' and 'I69' then '175'
when substr(icd.icd_code,1,3) ='I70' then '176'
when substr(icd.icd_code,1,3) ='I73' then '177'
when substr(icd.icd_code,1,3) ='I74' then '178'
when substr(icd.icd_code,1,3) between 'I71' and 'I72' then '179'
when substr(icd.icd_code,1,3) between 'I80' and 'I82' then '180'
when substr(icd.icd_code,1,3) ='I83' then '181'
when substr(icd.icd_code,1,3) ='I84' then '182'
when substr(icd.icd_code,1,3) between 'I85' and 'I99' then '183'
when substr(icd.icd_code,1,3) between 'J02' and 'J03' then '185'
when substr(icd.icd_code,1,3) ='J04' then '186'
when substr(icd.icd_code,1,3) between 'J00' and 'J01' or substr(icd.icd_code,1,3) between 'J05' and 'J06' then '187'
when substr(icd.icd_code,1,3) between 'J10' and 'J11' then '188'
when substr(icd.icd_code,1,3) between 'J12' and 'J18' then '189'
when substr(icd.icd_code,1,3) between 'J20' and 'J21' then '190'
when substr(icd.icd_code,1,3) ='J32' then '191'
when substr(icd.icd_code,1,3) between 'J30' and 'J31' or substr(icd.icd_code,1,3) between 'J33' and 'J34' then '192'
when substr(icd.icd_code,1,3) ='J35' then '193'
when substr(icd.icd_code,1,3) between 'J36' and 'J39' then '194'
when substr(icd.icd_code,1,3) between 'J40' and 'J44' then '195'
when substr(icd.icd_code,1,3) between 'J45' and 'J46' then '196'
when substr(icd.icd_code,1,3) ='J47' then '197'
when substr(icd.icd_code,1,3) ='J60' then '198'
when substr(icd.icd_code,1,3) ='J22' or substr(icd.icd_code,1,3) between 'J66' and 'J99' then '199'
when substr(icd.icd_code,1,3) ='K02' then '201'
when substr(icd.icd_code,1,3) between 'K03' and 'K08' or substr(icd.icd_code,1,3) between 'K00' and 'K01' then '202'
when substr(icd.icd_code,1,3) between 'K09' and 'K14' then '203'
when substr(icd.icd_code,1,3) between 'K25' and 'K27' then '204'
when substr(icd.icd_code,1,3) ='K29' then '205'
when substr(icd.icd_code,1,3) between 'K20' and 'K23' or substr(icd.icd_code,1,3) ='K28' or substr(icd.icd_code,1,3) between 'K30' and 'K31' then '206'
when substr(icd.icd_code,1,3) between 'K35' and 'K38' then '207'
when substr(icd.icd_code,1,3) ='K40' then '208'
when substr(icd.icd_code,1,3) between 'K41' and 'K46' then '209'
when substr(icd.icd_code,1,3) between 'K50' and 'K51' then '210'
when substr(icd.icd_code,1,3) ='K56' then '211'
when substr(icd.icd_code,1,3) ='K57' then '212'
when substr(icd.icd_code,1,3) between 'K52' and 'K55' or substr(icd.icd_code,1,3) between 'K58' and 'K67' then '213'
when substr(icd.icd_code,1,3) ='K70' then '214'
when substr(icd.icd_code,1,3) between 'K71' and 'K77' then '215'
when substr(icd.icd_code,1,3) between 'K80' and 'K81' then '216'
when substr(icd.icd_code,1,3) between 'K85' and 'K86' then '217'
when substr(icd.icd_code,1,3) between 'K87' and 'K93' or substr(icd.icd_code,1,3) between 'K82' and 'K83' then '218'
when substr(icd.icd_code,1,3) between 'L00' and 'L08' then '220'
when substr(icd.icd_code,1,3) between 'L10' and 'L99' then '221'
when substr(icd.icd_code,1,3) between 'M05' and 'M14' then '223'
when substr(icd.icd_code,1,3) between 'M15' and 'M19' then '224'
when substr(icd.icd_code,1,3) between 'M20' and 'M21' then '225'
when substr(icd.icd_code,1,3) between 'M00' and 'M03' or substr(icd.icd_code,1,3) between 'M22' and 'M25' then '226'
when substr(icd.icd_code,1,3) between 'M30' and 'M36' then '227'
when substr(icd.icd_code,1,3) between 'M50' and 'M51' then '228'
when substr(icd.icd_code,1,3) between 'M40' and 'M49' or substr(icd.icd_code,1,3) between 'M53' and 'M54' then '229'
when substr(icd.icd_code,1,3) between 'M60' and 'M79' then '230'
when substr(icd.icd_code,1,3) between 'M80' and 'M85' then '231'
when substr(icd.icd_code,1,3) ='M86' then '232'
when substr(icd.icd_code,1,3) between 'M87' and 'M99' then '233'
when substr(icd.icd_code,1,3) between 'N00' and 'N01' then '235'
when substr(icd.icd_code,1,3) between 'N02' and 'N08' then '236'
when substr(icd.icd_code,1,3) between 'N10' and 'N16' then '237'
when substr(icd.icd_code,1,3) between 'N17' and 'N19' then '238'
when substr(icd.icd_code,1,3) between 'N20' and 'N23' then '239'
when substr(icd.icd_code,1,3) ='N30' then '240'
when substr(icd.icd_code,1,3) between 'N25' and 'N29' or substr(icd.icd_code,1,3) between 'N31' and 'N39' then '241'
when substr(icd.icd_code,1,3) ='N40' then '242'
when substr(icd.icd_code,1,3) between 'N41' and 'N42' then '243'
when substr(icd.icd_code,1,3) ='N43' then '244'
when substr(icd.icd_code,1,3) ='N47' then '245'
when substr(icd.icd_code,1,3) between 'N44' and 'N46' or substr(icd.icd_code,1,3) between 'N49' and 'N51' then '246'
when substr(icd.icd_code,1,3) between 'N60' and 'N64' then '247'
when substr(icd.icd_code,1,3) ='N70' then '248'
when substr(icd.icd_code,1,3) ='N72' then '249'
when substr(icd.icd_code,1,3) ='N71' or substr(icd.icd_code,1,3) between 'N73' and 'N77' then '250'
when substr(icd.icd_code,1,3) ='N80' then '251'
when substr(icd.icd_code,1,3) ='N81' then '252'
when substr(icd.icd_code,1,3) ='N83' then '253'
when substr(icd.icd_code,1,3) between 'N91' and 'N92' then '254'
when substr(icd.icd_code,1,3) ='N95' then '255'
when substr(icd.icd_code,1,3) ='N97' then '256'
when substr(icd.icd_code,1,3) ='N82' or substr(icd.icd_code,1,3) between 'N84' and 'N90' or substr(icd.icd_code,1,3) between 'N93' and 'N94' or substr(icd.icd_code,1,3) ='N96' or substr(icd.icd_code,1,3) between 'N98' and 'N99' then '257'
when substr(icd.icd_code,1,3) ='O03' then '259'
when substr(icd.icd_code,1,3) ='O04' then '260'
when substr(icd.icd_code,1,3) between 'O00' and 'O02' or substr(icd.icd_code,1,3) between 'O05' and 'O08' then '261'
when substr(icd.icd_code,1,3) between 'O10' and 'O16' then '262'
when substr(icd.icd_code,1,3) between '044' and 'O46' then '263'
when substr(icd.icd_code,1,3) between 'O30' and 'O43' or substr(icd.icd_code,1,3) between 'O47' and 'O48' then '264'
when substr(icd.icd_code,1,3) between 'O64' and 'O66' then '265'
when substr(icd.icd_code,1,3) ='O72' then '266'
when substr(icd.icd_code,1,3) between 'O20' and 'O29' or substr(icd.icd_code,1,3) between 'O60' and 'O63' or substr(icd.icd_code,1,3) between 'O67' and 'O71' or substr(icd.icd_code,1,3) between 'O73' and 'O75' or substr(icd.icd_code,1,3) between 'O81' and 'O84' then '267'
when substr(icd.icd_code,1,3) ='O80' then '268'
when substr(icd.icd_code,1,3) between 'O85' and 'O99' then '269'
when substr(icd.icd_code,1,3) between 'P00' and 'P04' then '271'
when substr(icd.icd_code,1,3) between 'P05' and 'P07' then '272'
when substr(icd.icd_code,1,3) between 'P10' and 'P15' then '273'
when substr(icd.icd_code,1,3) between 'P20' and 'P21' then '274'
when substr(icd.icd_code,1,3) between 'P22' and 'P28' then '275'
when substr(icd.icd_code,1,3) between 'P35' and 'P37' then '276'
when substr(icd.icd_code,1,3) between 'P38' and 'P39' then '277'
when substr(icd.icd_code,1,3) ='P55' then '278'
when substr(icd.icd_code,1,3) ='P08' or substr(icd.icd_code,1,3) ='P29' or substr(icd.icd_code,1,3) between 'P50' and 'P54' or substr(icd.icd_code,1,3) between 'P56' and 'P96' then '279'
when substr(icd.icd_code,1,3) ='Q05' then '281'
when substr(icd.icd_code,1,3) between 'Q00' and 'Q04' or substr(icd.icd_code,1,3) between 'Q06' and 'Q07' then '282'
when substr(icd.icd_code,1,3) between 'Q20' and 'Q28' then '283'
when substr(icd.icd_code,1,3) between 'Q35' and 'Q37' then '284'
when substr(icd.icd_code,1,3) ='Q41' then '285'
when substr(icd.icd_code,1,3) between 'Q38' and 'Q40' or substr(icd.icd_code,1,3) between 'Q42' and 'Q45' then '286'
when substr(icd.icd_code,1,3) ='Q53' then '287'
when substr(icd.icd_code,1,3) between 'Q50' and 'Q52' or substr(icd.icd_code,1,3) between 'Q54' and 'Q64' then '288'
when substr(icd.icd_code,1,3) ='Q65' then '289'
when substr(icd.icd_code,1,3) ='Q66' then '290'
when substr(icd.icd_code,1,3) between 'Q67' and 'Q79' then '291'
when substr(icd.icd_code,1,3) between 'Q10' and 'Q13' or substr(icd.icd_code,1,3) between 'Q30' and 'Q34' or substr(icd.icd_code,1,3) between 'Q80' and 'Q89' then '292'
when substr(icd.icd_code,1,3) between 'Q90' and 'Q99' then '293'
when substr(icd.icd_code,1,3) ='R10' then '295'
when substr(icd.icd_code,1,3) ='R50' then '296'
when substr(icd.icd_code,1,3) ='R54' then '297'
when substr(icd.icd_code,1,3) between 'R00' and 'R09' or substr(icd.icd_code,1,3) between 'R11' and 'R49' or substr(icd.icd_code,1,3) between 'R50' and 'R53' or substr(icd.icd_code,1,3) between 'R55' and 'R99' then '298'
when substr(icd.icd_code,1,3) ='S02' then '300'
when substr(icd.icd_code,1,3) ='S12' or substr(icd.icd_code,1,3) ='S22' or substr(icd.icd_code,1,3) ='S32' or substr(icd.icd_code,1,3) ='T08' then '301'
when substr(icd.icd_code,1,3) ='S72' then '302'
when substr(icd.icd_code,1,3) ='S42' or substr(icd.icd_code,1,3) ='S52' or substr(icd.icd_code,1,3) ='S62' or substr(icd.icd_code,1,3) ='S82' or substr(icd.icd_code,1,3) ='S92' or substr(icd.icd_code,1,3) ='T10' or substr(icd.icd_code,1,3) ='T12' then '303'
when substr(icd.icd_code,1,3) ='T02' then '304'
when substr(icd.icd_code,1,3) ='S03' or substr(icd.icd_code,1,3) ='S13' or substr(icd.icd_code,1,3) ='S23' or substr(icd.icd_code,1,3) ='S33' or substr(icd.icd_code,1,3) ='S43' or substr(icd.icd_code,1,3) ='S53' or substr(icd.icd_code,1,3) ='S63' or substr(icd.icd_code,1,3) ='S73' or substr(icd.icd_code,1,3) ='S83' or substr(icd.icd_code,1,3) ='S93' or substr(icd.icd_code,1,3) ='T03' then '305'
when substr(icd.icd_code,1,3) ='S05' then '306'
when substr(icd.icd_code,1,3) ='S06' then '307'
when substr(icd.icd_code,1,3) between 'S26' and 'S27' or substr(icd.icd_code,1,3) between 'S36' and 'S37' then '308'
when substr(icd.icd_code,1,3) between 'S07' and 'S08' or substr(icd.icd_code,1,3) between 'S17' and 'S18' or substr(icd.icd_code,1,3) ='S28' or substr(icd.icd_code,1,3) ='S38' or substr(icd.icd_code,1,3) between 'S47' and 'S48' or substr(icd.icd_code,1,3) between 'S57' and 'S58' or substr(icd.icd_code,1,3) between 'S67' and 'S68' or substr(icd.icd_code,1,3) between 'S77' and 'S78' or substr(icd.icd_code,1,3) between 'S87' and 'S88' or substr(icd.icd_code,1,3) between 'S97' and 'S98' or substr(icd.icd_code,1,3) between 'T04' and 'T05' then '309'
when substr(icd.icd_code,1,3) between 'S00' and 'S01' or substr(icd.icd_code,1,3) = 'S04' or substr(icd.icd_code,1,3) between 'S09' and 'S11' or substr(icd.icd_code,1,3) between 'S14' and 'S16' or substr(icd.icd_code,1,3) between 'S19' and 'S21' or substr(icd.icd_code,1,3) between 'S24' and 'S25' or substr(icd.icd_code,1,3) between 'S29' and 'S31' or substr(icd.icd_code,1,3) between 'S34' and 'S35' or substr(icd.icd_code,1,3) between 'S39' and 'S41' or substr(icd.icd_code,1,3) between 'S44' and 'S46' or substr(icd.icd_code,1,3) between 'S49' and 'S51' or substr(icd.icd_code,1,3) between 'S54' and 'S56' or substr(icd.icd_code,1,3) between 'S59' and 'S61' or substr(icd.icd_code,1,3) between 'S64' and 'S66' or substr(icd.icd_code,1,3) between 'S69' and 'S71' or substr(icd.icd_code,1,3) between 'S74' and 'S76' or substr(icd.icd_code,1,3) between 'S79' and 'S81' or substr(icd.icd_code,1,3) between 'S84' and 'S86' or substr(icd.icd_code,1,3) between 'S89' and 'S91' then '310'
when substr(icd.icd_code,1,3) between 'T15' and 'T19' then '311'
when substr(icd.icd_code,1,3) between 'T20' and 'T32' then '312'
when substr(icd.icd_code,1,3) between 'T36' and 'T50' then '313'
when substr(icd.icd_code,1,3) between 'T51' and 'T65' then '314'
when substr(icd.icd_code,1,3) ='T74' then '315'
when substr(icd.icd_code,1,3) between 'T33' and 'T35' or substr(icd.icd_code,1,3) between 'T66' and 'T73' or substr(icd.icd_code,1,3) between 'T75' and 'T78' then '316'
when substr(icd.icd_code,1,3) between 'T79' and 'T88' then '317'
when substr(icd.icd_code,1,3) between 'T90' and 'T98' then '318'
when substr(icd.icd_code,1,3) between 'V01' and 'V09' or substr(icd.icd_code,1,3) between 'W01' and 'W19' then '320'
when substr(icd.icd_code,1,3) between 'W20' and 'W64' then '321'
when substr(icd.icd_code,1,3) between 'W65' and 'W84' then '322'
when substr(icd.icd_code,1,3) between 'W85' and 'W99' then '323'
when substr(icd.icd_code,1,3) between 'X00' and 'X09' then '324'
when substr(icd.icd_code,1,3) between 'X10' and 'X19' then '325'
when substr(icd.icd_code,1,3) between 'X20' and 'X29' then '326'
when substr(icd.icd_code,1,3) between 'X40' and 'X49' then '327'
when substr(icd.icd_code,1,3) between 'X60' and 'X84' then '328'
when substr(icd.icd_code,1,3) between 'X85' and 'Y09' then '329'
when substr(icd.icd_code,1,3) between 'Y40' and 'Y59' then '330'
when substr(icd.icd_code,1,3) between 'Y60' and 'Y69' then '331'
when substr(icd.icd_code,1,3) between 'Y70' and 'Y82' then '332'
when substr(icd.icd_code,1,3) between 'Y90' and 'Y98' then '333'
when substr(icd.icd_code,1,3) between 'Z00' and 'Z01' then '335'
when substr(icd.icd_code,1,3) ='Z21' then '336'
when substr(icd.icd_code,1,3) ='Z20' or substr(icd.icd_code,1,3) between 'Z22' and 'Z29' then '337'
when substr(icd.icd_code,1,3) ='Z30' then '338'
when substr(icd.icd_code,1,3) between 'Z34' and 'Z36' then '339'
when substr(icd.icd_code,1,3) ='Z38' then '340'
when substr(icd.icd_code,1,3) ='Z39' then '341'
when substr(icd.icd_code,1,3) between 'Z40' and 'Z54' then '342'
when substr(icd.icd_code,1,3) between 'Z31' and 'Z33' or substr(icd.icd_code,1,3) ='Z37' or substr(icd.icd_code,1,3) between 'Z55' and 'Z99' then '343'
else '' end ) icd_group,
(case
when substr(icd.icd_code,1,3) between 'A00' and 'B99' then '11'
when substr(icd.icd_code,1,3) between 'C00' and 'D48' then '69'
when substr(icd.icd_code,1,3) between 'D50' and 'D89' then '109'
when substr(icd.icd_code,1,3) between 'E00' and 'E90' then '114'
when substr(icd.icd_code,1,3) between 'F00' and 'F99' then '126'
when substr(icd.icd_code,1,3) between 'G00' and 'G99' then '135'
when substr(icd.icd_code,1,3) between 'H00' and 'H59' then '146'
when substr(icd.icd_code,1,3) between 'H60' and 'H95' then '157'
when substr(icd.icd_code,1,3) between 'I00' and 'I99' then '161'
when substr(icd.icd_code,1,3) between 'J00' and 'J99' then '184'
when substr(icd.icd_code,1,3) between 'K00' and 'K93' then '200'
when substr(icd.icd_code,1,3) between 'L00' and 'L99' then '219'
when substr(icd.icd_code,1,3) between 'M00' and 'M99' then '222'
when substr(icd.icd_code,1,3) between 'N00' and 'N99' then '234'
when substr(icd.icd_code,1,3) between 'O00' and 'O99' then '258'
when substr(icd.icd_code,1,3) between 'P00' and 'P96' then '270'
when substr(icd.icd_code,1,3) between 'Q00' and 'Q99' then '280'
when substr(icd.icd_code,1,3) between 'R00' and 'R99' then '294'
when substr(icd.icd_code,1,3) between 'S00' and 'T98' then '299'
when substr(icd.icd_code,1,3) between 'V01' and 'Y98' then '319'
when substr(icd.icd_code,1,3) between 'Z00' and 'Z99' then '334'
else '' end) icd_parent_group,
icd.icd_code
from his_icd icd");
           
            Inventec.Common.Logging.LogSystem.Info("SQL: " + query);
            result = new MOS.DAO.Sql.SqlDAO().GetSql<ICD_GROUP_DETAIL>(paramGet, query.ToUpper());
            if (paramGet.HasException)
                throw new NullReferenceException("Co exception xay ra tai DAOGET trong qua trinh lay du lieu MRS00230");

            return result;
        }
    }
}
