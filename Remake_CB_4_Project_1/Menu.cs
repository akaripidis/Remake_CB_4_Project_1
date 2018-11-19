using Remake_CB_4_Project_1.Core.Domain;
using System.Collections.Generic;

namespace Remake_CB_4_Project_1
{
    public class Menu 
     {
       

        public Menu()
        {
           
        }


        public List<string> StartingMenu()
        {
            return new List<string>() { "Sign in", "Sign up", "Exit" };
        }

        public  List<string> MainMenuSignedIn(User user)
        {
            if (user != null)
            {
                if (user.AccessLevel == 1)
                {
                    return new List<string> { "Read Messages", "Sent Message", "SignOut", "Exit" };
                }
                if (user.AccessLevel == 2)
                {
                    return new List<string> { "Read Messages", "Sent Message", "Read messages (Access level 1)", "SignOut", "Exit" };
                }
                if (user.AccessLevel == 3)
                {
                    return new List<string> { "Read Messages", "Sent Message", "Read messages (Access level 1)", "Edit messages (Access level 2)", "SignOut", "Exit" };
                }
                if (user.AccessLevel == 4)
                {
                    return new List<string> { "Read Messages", "Sent Message", "Read messages (Access level 1)", "Edit messages (Access level 2)", "Delete messages (Access level 3)", "SignOut", "Exit" };
                }
                if (user.AccessLevel == 5)
                {
                    return new List<string> { "Read Messages", "Sent Message", "Change user's access level", "Remove User", "SignOut", "Exit" };
                }
                else
                {
                    return StartingMenu();
                }
            }

            else
            {
                return StartingMenu();
            }
        }


        public List<string> ReadMessageMenu()
        {
            return new List<string> { "Inbox", "Outbox", "Back" };
        }

        public List<string> AccessLevelStringList()
        {
            return new List<string>() {"Basic access", "Access level 1", "Access level 2", "Access level 3" };
        }
             
     }
}
