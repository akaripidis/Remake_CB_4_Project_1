using Remake_CB_4_Project_1.Core.Domain;
using Remake_CB_4_Project_1.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;

namespace Remake_CB_4_Project_1.Persistance.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(DbContext context) : base(context)
        {
        }

  

        public static Message MessageMarked;
        public static int PageIndex;
        public static readonly int PagesSize = 8;

        //////////////////////////////////////////////////////////////////////////// Message controlers.


        public void CreateMessageControler()
        {
            
            int idReceiver = UnitOfWork.Users.GetSelectedUser().Id;
            int idSender = UserRepository.UserInCharge.Id;
            Console.WriteLine("Please type a title for your Message.");
            string title = ReadUserInput();
            if(title!=CancelOperation)
            {
                Console.WriteLine("Now type your message");
                string message = ReadUserInput();
                if(message!=CancelOperation)
                {
                    CreateMessage(title, message, idSender, idReceiver);
                    Console.WriteLine("Message created");
                    Thread.Sleep(1500);
                }
            }
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), ActionsForMainMenuSignIn(UserRepository.UserInCharge));
        }

        public void ReadInboxMessageControler()
        {
            var message= GetSelectedMessage(Find(message1 => message1.ReceiverId.Id == UserRepository.UserInCharge.Id).Skip((PageIndex - 1) * PagesSize).Take(PagesSize).ToList());
            Console.WriteLine($"Date: {message.MessageDate}, From: {message.SenterId.Name}");
            Console.WriteLine($"Title: {message.Title} ");
            Console.WriteLine($"Message: {message.ActualMessage}");
            message.IsRead = true;
            UnitOfWork.Complete();
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to menu");
            Console.ReadKey();


            MenuRegulator.ApplicationMenu(Menu.ReadMessageMenu(),ActionsForReadMessageMenu());
        }

        public void ReadOutboxMessageControler()
        {
            var message = GetSelectedMessage(Find(Message1 => Message1.SenterId.Id == UserRepository.UserInCharge.Id).Skip((PageIndex - 1) * PagesSize).Take(PagesSize).ToList());
            Console.WriteLine($"Date: {message.MessageDate}, From: {message.SenterId.Name}");
            Console.WriteLine($"Title: {message.Title} ");
            Console.WriteLine($"Message: {message.ActualMessage}");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to menu");
            Console.ReadKey();
            MenuRegulator.ApplicationMenu(Menu.ReadMessageMenu(),ActionsForReadMessageMenu());
        }

        public void ReadAllMessageControler()
        {
            var message = GetSelectedMessage(GetAll().Skip((PageIndex - 1) * PagesSize).Take(PagesSize).ToList());
            Console.WriteLine($"Date: {message.MessageDate}, From: {message.SenterId.Name}");
            Console.WriteLine($"Title: {message.Title} ");
            Console.WriteLine($"Message: {message.ActualMessage}");
            Console.WriteLine();
            Console.WriteLine("Press any key to go back to menu");
            Console.ReadKey();

            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), ActionsForMainMenuSignIn(UserRepository.UserInCharge));

        }

        public void EditMessageControler()
        {
            
            int partToEdit = MenuRegulator.optionChoose;
            if(partToEdit==0)
            {
                Console.WriteLine("Type a new title.");
                string userInput = ReadUserInput();
                if (userInput != CancelOperation)
                {
                    MessageMarked.Title = userInput;
                    UnitOfWork.Complete();
                    Console.WriteLine("Message edited.");
                    Thread.Sleep(1500);
                }
                
            }
            
            if(partToEdit==1)
            {
                Console.WriteLine("Type a new message.");
                string userInput = ReadUserInput();
                if (userInput != CancelOperation)
                {
                    MessageMarked.ActualMessage = userInput;
                    UnitOfWork.Complete();
                    Console.WriteLine("Message edited.");
                    Thread.Sleep(1500);
                }
            }
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), ActionsForMainMenuSignIn(UserRepository.UserInCharge));
        }

        public void DeleteMessageControler()
        {
            var message = GetSelectedMessage(GetAll().Skip((PageIndex - 1) * PagesSize).Take(PagesSize).ToList());
            UnitOfWork.Messages.Remove(message);
            UnitOfWork.Complete();
            Console.WriteLine("Message deleted.");
            Thread.Sleep(1500);

            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), ActionsForMainMenuSignIn(UserRepository.UserInCharge));

        }
            


        //////////////////////////////////////////////////////////////////////// Encptulated methods supporting controlers.



        private void CreateMessage(string title, string message, int idsenter, int idreceiver)
        {
          
            UnitOfWork.Messages.Add(new Message
            {
                Title = title,
                ActualMessage = message,
                SenterId = UnitOfWork.Users.Get(idsenter),
                ReceiverId = UnitOfWork.Users.Get(idreceiver),
                MessageDate = DateTime.Now,
                IsRead = false

            });
            UnitOfWork.Complete();
          
        }

        private Message GetSelectedMessage(IEnumerable<Message> messages)
        {

            if (PageIndex == 1)
            {
                var message = messages.ElementAt(MenuRegulator.optionChoose);
                return message;
            }
            else
            {
                var message = messages.ElementAt(MenuRegulator.optionChoose - 1);
                return message;
            }
            
        }



       
        /////////////////////////////////////////////////////////////////////////// Message menu caller.


        public void CallMessageUserToSend()
        {
            PageIndex = 1;
            Console.WriteLine("Choose a user to send message to.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(UserNamesToListOfStrings(), ActionsForChoosingUser());
        }

        public void ReadMessageMenu()
        {
            MenuRegulator.ApplicationMenu(Menu.ReadMessageMenu(), ActionsForReadMessageMenu());
        }

        public void ReadInboxListSelector()
        {
            PageIndex = 1;
            Console.WriteLine("Choose a message to read.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(InboxMessageStringList(),ActionsForInboxMessageList());
        }

        public void ReadOutboxListSelector()
        {
            PageIndex = 1;
            Console.WriteLine("Choose a message to read.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(OutboxMessageStringList(), ActionsForOutboxMessageList());
        }

        public void ReadAllListSelector()
        {
            PageIndex = 1;
            Console.WriteLine("Choose a message to read.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(AllMessageListSting(),ActionsForReadAllMessageList());
        }

        public void AllListSelectorToEdit()
        {
            PageIndex = 1;
            Console.WriteLine("Choose a message to edit.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(AllMessageListSting(), ActionsForEditMessageList());
        }

        public void MessagePartSelector()
        {
            MessageMarked = GetSelectedMessage(GetAll().Skip((PageIndex - 1) * PagesSize).Take(PagesSize).ToList());
            Console.WriteLine("Choose which part to edit.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(MessagePartListString(),ActionForEditMessagePartSelector());
        }

        public void MessageDeleteSelector()
        {
            PageIndex = 1;
            Console.WriteLine("Choose which message to delete.");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(AllMessageListSting(), ActionsForDeleteMessageSelector());
        }

        private void BrowseUsersMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(UnitOfWork.Users.GetUserListIndexed(PageIndex, PagesSize),ActionsForChoosingUser());

        }

        private void BrowseUsersPlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(UnitOfWork.Users.GetUserListIndexed(PageIndex, PagesSize), ActionsForChoosingUser());

        }

        private void BrowseInboxMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(GetInboxMessageListIndexed(PageIndex, PagesSize), ActionsForInboxMessageList());
        }

        private void BrowseInboxPlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(GetInboxMessageListIndexed(PageIndex, PagesSize), ActionsForInboxMessageList());
        }

        private void BrowseOutboxMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(GetOutboxMessageListIndexed(PageIndex, PagesSize), ActionsForOutboxMessageList());

        }

        private void BrowseOutboxPlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(GetOutboxMessageListIndexed(PageIndex, PagesSize), ActionsForOutboxMessageList());
        }

        private void BrowseAllMessageMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForReadAllMessageList());
        }

        private void BrowseAllMessagePlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForReadAllMessageList());
        }

        private void BrowseEditMessageMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForEditMessageList());

        }

        private void BrowseEditMessagePlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForEditMessageList());
        }

        private void BrowseDeleteMessageMinus()
        {
            PageIndex--;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForDeleteMessageSelector());

        }

        private void BrowseDeleteMessagePlus()
        {
            PageIndex++;
            MenuRegulator.ApplicationMenu(GetAllMessageListIndexed(PageIndex, PagesSize), ActionsForDeleteMessageSelector());

        }


        /////////////////////////////////////////////////////////////////////////// StringList populator.





        public List<string> UserNamesToListOfStrings()
        {
           return UnitOfWork.Users.GetUserListIndexed(PageIndex, PagesSize);
        }

        public List<string> InboxMessageStringList()
        {
            return GetInboxMessageListIndexed(PageIndex, PagesSize);  
        }

        private List<string> GetInboxMessageListIndexed(int pageIndex,int pageSize)
        {
            int counter = 0;
            List<string> messageStrings = new List<string>();

            var numberOfMessages = UnitOfWork.Messages.Find(message => message.ReceiverId.Id == UserRepository.UserInCharge.Id).Count();
            
            if (pageIndex>1)
            {
                messageStrings.Add("Previous eight");
            }

            var messageListInbox = UnitOfWork.Messages.Find(message => message.ReceiverId.Id == UserRepository.UserInCharge.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            foreach (var message in messageListInbox)
            {
                if (message.IsRead == true)
                {
                    messageStrings.Add($"Message from [{message.SenterId.Name.ToString()}]. Title [{message.Title.ToString()}].");
                    counter++;
                }
                else
                {
                    messageStrings.Add($"[New!] Message from [{message.SenterId.Name.ToString()}]. Title [{message.Title.ToString()}].");
                    counter++;
                }


            }
            if (counter > 0)
            {
                if ((numberOfMessages % (pageIndex * pageSize) > 0 && numberOfMessages % (pageIndex * pageSize) != numberOfMessages) || (numberOfMessages % (pageIndex * pageSize) == 0 && numberOfMessages > (pageIndex * pageSize)))
                {
                    messageStrings.Add("Next eight");
                }
                return messageStrings;
            }
            else
            {
                if (counter == 0)
                {
                    Console.WriteLine("There are no messages");
                    Thread.Sleep(1500);
                    MenuRegulator.ApplicationMenu(Menu.ReadMessageMenu(), ActionsForReadMessageMenu());
                }
                return new List<string>();
            }
        }
       
        public List<string> OutboxMessageStringList()
        {
            return GetOutboxMessageListIndexed(PageIndex, PagesSize);           
        }

        private List<string> GetOutboxMessageListIndexed(int pageIndex,int pageSize)
        {
            int counter = 0;
            List<string> messageStrings = new List<string>();

            var numberOfMessages = UnitOfWork.Messages.Find(message => message.SenterId.Id == UserRepository.UserInCharge.Id).Count();

            if (pageIndex > 1)
            {
                messageStrings.Add("Previous eight");
            }
            var messageListInbox = UnitOfWork.Messages.Find(message => message.SenterId.Id == UserRepository.UserInCharge.Id).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            foreach (var message in messageListInbox)
            {

                if (message.IsRead == true)
                {
                    messageStrings.Add($"Message to [{message.ReceiverId.Name.ToString()}]. Title [{message.Title.ToString()}].");
                    counter++;
                }
                else
                {
                    messageStrings.Add($"[Unread!] Message to [{message.ReceiverId.Name.ToString()}]. Title [{message.Title.ToString()}].");
                    counter++;
                }


            }
            if (counter > 0)
            {
                if ((numberOfMessages % (pageIndex * pageSize) > 0 && numberOfMessages % (pageIndex * pageSize) != numberOfMessages) || (numberOfMessages % (pageIndex * pageSize) == 0 && numberOfMessages > (pageIndex * pageSize)))
                {
                    messageStrings.Add("Next eight");
                }
                return messageStrings;
            }
            else
            {
                if (counter == 0)
                {
                    Console.WriteLine("There are no messages");
                    Thread.Sleep(1500);
                    MenuRegulator.ApplicationMenu(Menu.ReadMessageMenu(), ActionsForReadMessageMenu());

                }

                return new List<string>();
            }
        }

        public List<string> AllMessageListSting()
        {
            return GetAllMessageListIndexed(PageIndex, PagesSize);
        }

        private List<string> GetAllMessageListIndexed(int pageIndex,int pageSize)
        {
            int counter = 0;
            var numberOfMessages = GetAll().Count();
            
            var messagesList = GetAll().Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList(); 

            var messageStrings = new List<string>();
            if (pageIndex > 1)
            {
                messageStrings.Add("Previous eight");
            }
            foreach (var message in messagesList)
            {
                messageStrings.Add($"Message from [{message.SenterId.Name.ToString()}]. Title [{message.Title.ToString()}].");
                counter++;
            }
            if ((numberOfMessages % (pageIndex * pageSize) > 0 && numberOfMessages % (pageIndex * pageSize) != numberOfMessages) || (numberOfMessages % (pageIndex * pageSize) == 0 && numberOfMessages > (pageIndex * pageSize)))
            {
                messageStrings.Add("Next eight");
                
            }
            if (counter>0)
            {
                return messageStrings;
            }
            else
            {
                Console.WriteLine("There are no messages.");
                Thread.Sleep(1500);
                MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), ActionsForMainMenuSignIn(UserRepository.UserInCharge));

                return new List<string>();
            }

           
        }
        
        public List<string> MessagePartListString()
        {
            if(MessageMarked!=null)
            {
                var messageStrings = new List<string>
                {
                    $"Title [{MessageMarked.Title}]",
                    $"Message [{MessageMarked.ActualMessage}]"
                };

                return messageStrings;
            }
            else
            {
                return new List<string>();
            }
            
        }


        

        /////////////////////////////////////////////////////////////////////////////////// Action Populator.
        

        public List<Action> ActionsForMainMenuSignIn (User user)
        {
            if (user!=null)
            {
                if (user.AccessLevel == 1)
                {
                    return new List<Action> { ReadMessageMenu, CallMessageUserToSend, SignOut, ExitApp };
                }
                if (user.AccessLevel == 2)
                {
                    return new List<Action> { ReadMessageMenu, CallMessageUserToSend, ReadAllListSelector, SignOut, ExitApp };
                }
                if (user.AccessLevel == 3)
                {
                    return new List<Action> { ReadMessageMenu, CallMessageUserToSend, ReadAllListSelector, AllListSelectorToEdit, SignOut, ExitApp };
                }
                if (user.AccessLevel == 4)
                {
                    return new List<Action> { ReadMessageMenu, CallMessageUserToSend, ReadAllListSelector, AllListSelectorToEdit, MessageDeleteSelector, SignOut, ExitApp };
                }
                if (user.AccessLevel == 5)
                {
                    return new List<Action> { ReadMessageMenu, CallMessageUserToSend, UnitOfWork.Users.ToChooseUserMenuToChangeAccessLevel, UnitOfWork.Users.ToChooseUserToRemove, SignOut, ExitApp };
                }
                else
                {
                    return UnitOfWork.Users.ActionsForStartingMenu();
                }
            }
            else
            {
                return UnitOfWork.Users.ActionsForStartingMenu();
            }

        }

        public List<Action> ActionsForReadMessageMenu()
        {
            return new List<Action> { ReadInboxListSelector, ReadOutboxListSelector, MenuBack };
        }

        public List<Action> ActionsForChoosingUser()
        {
            var UserList = UserNamesToListOfStrings();
            var actionList = new List<Action>();
                        
            foreach(string name in UserList)
            {
                if(name== "Previous eight")
                {
                    actionList.Add(new Action(BrowseUsersMinus));
                }
                else if(name=="Next eight")
                {
                    actionList.Add(new Action(BrowseUsersPlus));
                }
                else
                {
                    actionList.Add(new Action(CreateMessageControler));
                }
                
            }

                 return actionList;
        }

        public List<Action> ActionsForInboxMessageList()
        {
            var messageList = InboxMessageStringList();
            var actionList = new List<Action>();
            
            foreach(var message in messageList)
            {
                if(message=="Previous eight")
                {
                    actionList.Add(new Action(BrowseInboxMinus));
                }
                else if (message=="Next eight")
                {
                    actionList.Add(new Action(BrowseInboxPlus));
                }
                else
                {
                    actionList.Add(new Action(ReadInboxMessageControler));
                }
                
            }
            return actionList;
        }

        public List<Action> ActionsForOutboxMessageList()
        {
            var messageList = OutboxMessageStringList();
            var actionList = new List<Action>();
            foreach (var message in messageList)
            {
                if (message == "Previous eight")
                {
                    actionList.Add(new Action(BrowseOutboxMinus));
                }
                else if (message == "Next eight")
                {
                    actionList.Add(new Action(BrowseOutboxPlus));
                }
                else
                {
                    actionList.Add(new Action(ReadOutboxMessageControler));
                }
                
            }
            return actionList;
        }

        public List<Action> ActionsForReadAllMessageList()
        {
            var messageList = AllMessageListSting();
            var actionList = new List<Action>();
            foreach (var message in messageList)
            {
                if (message == "Previous eight")
                {
                    actionList.Add(new Action(BrowseAllMessageMinus));
                }
                else if (message == "Next eight")
                {
                    actionList.Add(new Action(BrowseAllMessagePlus));
                }
                else
                {
                    actionList.Add(new Action(ReadAllMessageControler));
                }
                
            }
            return actionList;
        }

        public List<Action> ActionsForEditMessageList()
        {
            var messageList = AllMessageListSting();
            var actionList = new List<Action>();
            foreach (var message in messageList)
            {
                if (message == "Previous eight")
                {
                    actionList.Add(new Action(BrowseEditMessageMinus));
                }
                else if (message == "Next eight")
                {
                    actionList.Add(new Action(BrowseEditMessagePlus));
                }
                else
                {
                    actionList.Add(new Action(MessagePartSelector));
                }
                    
            }
            return actionList;

        }

        public List<Action> ActionForEditMessagePartSelector()
        {
            var messageList = MessagePartListString();
            var actionList = new List<Action>();
            foreach (var message in messageList)
            {
                actionList.Add(new Action(EditMessageControler));
            }
            return actionList;

        }

        public List<Action> ActionsForDeleteMessageSelector()
        {
            var messageList = AllMessageListSting();
            var actionList = new List<Action>();
            foreach (var message in messageList)
            {
                if (message == "Previous eight")
                {
                    actionList.Add(new Action(BrowseDeleteMessageMinus));
                }
                else if (message == "Next eight")
                {
                    actionList.Add(new Action(BrowseDeleteMessagePlus));
                }
                else
                {
                    actionList.Add(new Action(DeleteMessageControler));
                }
                    
            }
            return actionList;

        }

    }
}
