using Inventec.Common.Repository;

namespace ACS.MANAGER.Base
{
    static class DAOWorker
    {
        internal static ACS.DAO.AcsRole.AcsRoleDAO AcsRoleDAO { get { return (ACS.DAO.AcsRole.AcsRoleDAO)Worker.Get<ACS.DAO.AcsRole.AcsRoleDAO>(); } }
        internal static ACS.DAO.AcsUser.AcsUserDAO AcsUserDAO { get { return (ACS.DAO.AcsUser.AcsUserDAO)Worker.Get<ACS.DAO.AcsUser.AcsUserDAO>(); } }
    }
}
