using Inventec.Common.Repository;

namespace MRS.Processor.Mrs00737
{
    static class DAOWorker
    {
       
        internal static MOS.DAO.Sql.SqlDAO SqlDAO { get { return (MOS.DAO.Sql.SqlDAO)Worker.Get<MOS.DAO.Sql.SqlDAO>(); } }
       
	}
}
