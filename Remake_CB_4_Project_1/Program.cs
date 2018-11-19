using Remake_CB_4_Project_1.Persistance;
using System;

namespace Remake_CB_4_Project_1
{
    class Program
    {
        static void Main(string[] args)
        {



            MenuRegulator menuReg = new MenuRegulator();
            Menu menu = new Menu();
            UnitOfWork unitOfWork = UnitOfWork.Instance;

            menuReg.ApplicationMenu(menu.StartingMenu(), unitOfWork.Users.ActionsForStartingMenu());



            


            Console.ReadLine();

           

        }

       
    }
}
