
using Remake_CB_4_Project_1.Core.Domain;
using System;
using System.Collections.Generic;

namespace Remake_CB_4_Project_1.Core.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        
        void ToChooseUserMenuToChangeAccessLevel();
        void ToChooseUserToRemove();
        
        User GetSelectedUser();
        
        
        List<string> GetUserListIndexed(int pageIndex, int pagesSize);

        List<Action> ActionsForStartingMenu();
        
    }
}
