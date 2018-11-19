using Remake_CB_4_Project_1.Core.Domain;
using Remake_CB_4_Project_1.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace Remake_CB_4_Project_1.Persistance.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MessageAppContext context) : base(context)
        {
        }

        public static User UserInCharge;

        public static User UserMarked;    



        ///////////////////////////////////////////////////////////////////////// User Controlers.

        public void SignUpControler()
        {
            
            

            Console.WriteLine("Type your username");
            string userName = ReadUserInput();
            
            do
            {
                if (CheckIfUserNameExist(userName) == true && userName!= CancelOperation)
                {
                    Console.WriteLine("This user name already exists, choose another one.");
                    userName = ReadUserInput();
                }

            } while (CheckIfUserNameExist(userName) == true && userName != CancelOperation);
            if(userName!=CancelOperation)
            {
                Console.WriteLine("Type your password");
                string userPassword = ReadUserInput();
                if (userPassword != CancelOperation)
                {
                    string salt = CreateSalt();
                    userPassword = userPassword + salt;
                    userPassword = PasswordToHash(userPassword);
                    SingUp(userName, userPassword, salt);
                }
            }
            MenuRegulator.ApplicationMenu(Menu.StartingMenu(), ActionsForStartingMenu());
        }

        public void SignInControler()
        {
            
            Console.WriteLine("Type your username.");
            string userName = ReadUserInput(); 
            do
            {
                if (CheckIfUserNameExist(userName) == false && userName!=CancelOperation)
                {
                    Console.WriteLine("This user name does not exist, choose another one.");
                    userName = ReadUserInput();
                }

            } while (CheckIfUserNameExist(userName) == false && userName != CancelOperation);
            if(userName!=CancelOperation)
            {
                Console.WriteLine($"Type your password user {userName}.");
                string password = ReadUserInput();
                if(password!=CancelOperation)
                {
                    string salt = GetSalt(userName);
                    password = password + salt;
                    password = PasswordToHash(password);
                    if (SignIn(userName, password) == true)
                    {
                        Console.WriteLine("you are singed in");

                        UserInCharge = GetUserByUserName(userName);
                        Thread.Sleep(1500);
                    }
                }
            }
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserInCharge), UnitOfWork.Messages.ActionsForMainMenuSignIn(UserInCharge));
        }

        public void ChangeAccessLevelControler()
        {
            UserMarked.AccessLevel = MenuRegulator.optionChoose + 1;
            UnitOfWork.Complete();
            Console.WriteLine("User access level changed");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserInCharge), UnitOfWork.Messages.ActionsForMainMenuSignIn(UserInCharge));

        }

        public void RemoveUserControler()
        {
            var user = GetSelectedUser();
            RemoveUser(user);
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserInCharge), UnitOfWork.Messages.ActionsForMainMenuSignIn(UserInCharge));
        }

        



        ///////////////////////////////////////////////////////////////////////////////// Encaptulated methods supporting controlers.

        

        private bool SignIn(string userName, string password)
        {
           
                var validateUser= UnitOfWork.Users.SingleOrDefault(user=> user.Name==userName);
                if(password == validateUser.Password)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Incorrect password");
                    Thread.Sleep(1500);

                    return false;
                }
           
        }

        private void SingUp(string userName, string password,string salt)
        {
            
                UnitOfWork.Users.Add(new User
                {
                    Name = userName,
                    Password = password,
                    Saltword = salt,
                    AccessLevel = 1

                });
                UnitOfWork.Complete();
            

            Console.WriteLine($"user {userName} created");
            Thread.Sleep(1500);
        }

        private bool CheckIfUserNameExist(string userInput)
        {
            var userList = GetAll();
            if (userList.SingleOrDefault(user => user.Name == userInput)==null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private string PasswordToHash(string userInput)
        {
            return userInput.GetHashCode().ToString();
        }

        private string CreateSalt()
        {
            char[] saltCharacters = "abcdefghigklmnopqrstuvxyzABCDEFGHIJKLMNOPQRSTUVXYZ1234567890".ToCharArray();
            string salt ="";
            var randomGenerator = new Random();
            for (int i = 0; i < 7; i++)
            {
                int randomNumber = randomGenerator.Next(0,saltCharacters.Length);
                salt += saltCharacters[randomNumber];
            }

            return salt;
        }

        private void RemoveUser(User user)
        {
           
                
            string userName = user.Name;
            var messages = UnitOfWork.Messages.Find(user1 => user1.SenterId.Id == user.Id || user1.ReceiverId.Id == user.Id);
            foreach(var message in messages)
            {
                UnitOfWork.Messages.Remove(message);
            }
               
            UnitOfWork.Users.Remove(user);
            UnitOfWork.Complete();
            
            Console.WriteLine($"User {userName} removed");
            Thread.Sleep(1500);
        }

        private string GetSalt(string userName)
        {
          
                var userMirror = UnitOfWork.Users.SingleOrDefault(user => user.Name == userName);
                return userMirror.Saltword;
          
        }

        public User GetSelectedUser()
        {
            var userList = GetUsersPaged(MessageRepository.PageIndex, MessageRepository.PagesSize);
            if (MessageRepository.PageIndex == 1)
            {
                return userList.ElementAt(MenuRegulator.optionChoose);
            }
            else
            {
                return userList.ElementAt(MenuRegulator.optionChoose - 1);
            }
        }


        ///////////////////////////////////////////////////////////////////////// methods accesed by message repository.



        public User GetUserByUserName(string userName)
        {
            var userlist = GetAll();
            return userlist.SingleOrDefault(user => user.Name == userName);
        }

        public IEnumerable<User> GetUsersPaged(int pageIndex, int pageSize)
        {
            return GetAll().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }

        

        //////////////////////////////////////////////////////////////////////// Action user menu support.





        public void ToChooseUserMenuToChangeAccessLevel()
        {
            MessageRepository.PageIndex = 1;
            Console.WriteLine();
            Console.WriteLine("Choose a user to change its access level");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToChangeAccessLevel());
        }

        public void ChooseWhichAccessLevel()
        {
            UserMarked = GetSelectedUser();
                       
            MenuRegulator.ApplicationMenu(Menu.AccessLevelStringList(), ActionsForChangingAccessLevel());

        }

        public void ToChooseUserToRemove()
        {
            MessageRepository.PageIndex = 1;
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToRemove());
        }

        private void BrowseUsersForAccessLevelMinus()
        {
            MessageRepository.PageIndex--;
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToChangeAccessLevel());
        }

        private void BrowseUsersForAccessLevelPlus()
        {
            MessageRepository.PageIndex++;
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToChangeAccessLevel());
        }

        private void BrowseUsersForRemovingMinus()
        {
            MessageRepository.PageIndex--;
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToRemove());
        }

        private void BrowseUsersForRemovingPlus()
        {
            MessageRepository.PageIndex++;
            MenuRegulator.ApplicationMenu(UnitOfWork.Messages.UserNamesToListOfStrings(), ActionsForChoosingUserToRemove());
        }


        ////////////////////////////////////////////////////////////////////////// String List populator.


        public List<string> GetUserListIndexed(int pageIndex, int pageSize)
        {
            var userStringList = new List<string>();
            var numberOfUsers = GetAll().Count();

            if (pageIndex > 1)
            {
                userStringList.Add("Previous eight");
            }

            IEnumerable<User> users = UnitOfWork.Users.GetAll().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            foreach (User user in users)
            {
                userStringList.Add(user.Name.ToString());
            }

            if ((numberOfUsers % (pageIndex * pageSize) > 0 && numberOfUsers % (pageIndex * pageSize) != numberOfUsers) || (numberOfUsers % (pageIndex * pageSize) == 0 && numberOfUsers > (pageIndex * pageSize)))
            {
                userStringList.Add("Next eight");
            }

            return userStringList;

        }



        ///////////////////////////////////////////////////////////////////////// Action List populator.



        public List<Action> ActionsForStartingMenu()
        {
            return new List<Action> { SignInControler, SignUpControler, ExitApp };
        }

        public List<Action> ActionsForChoosingUserToChangeAccessLevel()
        {
            var UserList = UnitOfWork.Messages.UserNamesToListOfStrings();
            var actionList = new List<Action>();
            foreach ( string name in UserList)
            {
                if (name == "Previous eight")
                {
                    actionList.Add(new Action(BrowseUsersForAccessLevelMinus));
                }
                else if (name == "Next eight")
                {
                    actionList.Add(new Action(BrowseUsersForAccessLevelPlus));
                }
                else
                {
                    actionList.Add(new Action(ChooseWhichAccessLevel));
                }
                
            }
            return actionList;
        }

        public List<Action> ActionsForChangingAccessLevel()
        {
            var StringList = Menu.AccessLevelStringList();
            var actionList = new List<Action>();
            foreach (string user in StringList)
            {
                actionList.Add(new Action(ChangeAccessLevelControler));
            }
            return actionList;
        }

        public List<Action> ActionsForChoosingUserToRemove()
        {
            var UserList = UnitOfWork.Messages.UserNamesToListOfStrings();
            var actionList = new List<Action>();
            foreach (string name in UserList)
            {
                if (name == "Previous eight")
                {
                    actionList.Add(new Action(BrowseUsersForRemovingMinus));
                }
                else if (name == "Next eight")
                {
                    actionList.Add(new Action(BrowseUsersForRemovingPlus));
                }
                else
                {
                    actionList.Add(new Action(RemoveUserControler));
                }
                    
            }
            return actionList;
        }



    }
}
