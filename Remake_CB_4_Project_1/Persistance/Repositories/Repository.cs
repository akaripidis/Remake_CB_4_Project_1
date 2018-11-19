using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Remake_CB_4_Project_1.Core.Repositories;

namespace Remake_CB_4_Project_1.Persistance.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        public static readonly string CancelOperation = "`Exit";
        protected readonly DbContext Context;
        public static UnitOfWork UnitOfWork = UnitOfWork.Instance;  
        public Menu Menu { get { return new Menu(); } }
        public MenuRegulator MenuRegulator { get { return new MenuRegulator(); } }
    

        public Repository(DbContext context)
        {
            Context = context;
        }

        public TEntity Get(int id)
        {
            return Context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }


        public IEnumerable<TEntity> Find(Expression<Func<TEntity,bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().SingleOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public void SignOut()
        {
            UserRepository.UserInCharge = null;
            Console.WriteLine("you are signed out");
            Thread.Sleep(1500);
            MenuRegulator.ApplicationMenu(Menu.StartingMenu(), UnitOfWork.Users.ActionsForStartingMenu());
        }


        public void ExitApp()
        {
            Environment.Exit(0);
        }

        public void MenuBack()
        {
            MenuRegulator.ApplicationMenu(Menu.MainMenuSignedIn(UserRepository.UserInCharge), UnitOfWork.Messages.ActionsForMainMenuSignIn(UserRepository.UserInCharge));
        }

        public string ReadUserInput()
        {
            string userInput="";
            ConsoleKeyInfo keyPressed;
            do
            {
                keyPressed = Console.ReadKey();
                if(keyPressed.Key != ConsoleKey.Enter && keyPressed.Key != ConsoleKey.Escape && keyPressed.Key!=ConsoleKey.Backspace)
                {
                    char input = keyPressed.KeyChar;
                    userInput = (userInput + input).ToString();
                }
                if (keyPressed.Key == ConsoleKey.Backspace)
                {
                    userInput = userInput.Remove(userInput.Length-1);
                }

            } while (keyPressed.Key != ConsoleKey.Enter && keyPressed.Key != ConsoleKey.Escape);
            if(keyPressed.Key==ConsoleKey.Enter)
            {
                return userInput;
            }
            else
            {
                return CancelOperation;
            }
        }
       
    }
}
