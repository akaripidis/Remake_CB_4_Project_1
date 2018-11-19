using Remake_CB_4_Project_1.Core.Domain;
using System;
using System.Collections.Generic;

namespace Remake_CB_4_Project_1.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        
        List<string> UserNamesToListOfStrings();
        
        List<Action> ActionsForMainMenuSignIn(User user);
        
    }
}
