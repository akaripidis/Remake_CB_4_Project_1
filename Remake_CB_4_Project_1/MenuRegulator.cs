using Remake_CB_4_Project_1.Persistance;
using System;
using System.Collections.Generic;

namespace Remake_CB_4_Project_1
{
    public class MenuRegulator
    {

        public UnitOfWork UnitOfWork = UnitOfWork.Instance;
        public static int CurrentMenu;
        public static int optionChoose;



        public MenuRegulator()
        {
            
        }



        public void ApplicationMenu(List<string> consoleOptions, List<Action> optionActions)
        {

            int option = 0;

            while (true)
            {

                if (option > consoleOptions.Count - 1)
                {
                    option = 0;
                }
                Console.Clear();
                for (int i = 0; i < consoleOptions.Count; i++)
                {
                    if (option == i)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    Console.WriteLine("{0}.{1}", i, consoleOptions[i]);

                    if (option == i)
                    {
                        Console.ResetColor();
                    }
                }
                var keyPressed = Console.ReadKey();
                if (keyPressed.Key == ConsoleKey.DownArrow)
                {
                    if (option != consoleOptions.Count - 1)
                    {
                        option++;
                    }
                }
                else if (keyPressed.Key == ConsoleKey.UpArrow)
                {
                    if (option != 0)
                    {
                        option--;
                    }
                }
                

                if (keyPressed.Key == ConsoleKey.Enter||keyPressed.Key==ConsoleKey.Escape)
                {
                    if (keyPressed.Key == ConsoleKey.Escape)
                    {
                        option = 10;
                    }
                    
                    switch (option)
                    {
                        case 0:
                            Console.Clear();
                            optionChoose = 0;
                            optionActions[0]();
                            break;
                        case 1:
                            Console.Clear();
                            optionChoose = 1;
                            optionActions[1]();
                            break;
                        case 2:
                            Console.Clear();
                            optionChoose = 2;
                            optionActions[2]();
                            break;
                        case 3:
                            Console.Clear();
                            optionChoose = 3;
                            optionActions[3]();
                            break;
                        case 4:
                            Console.Clear();
                            optionChoose = 4;
                            optionActions[4]();
                            break;
                        case 5:
                            Console.Clear();
                            optionChoose = 5;
                            optionActions[5]();
                            break;
                        case 6:
                            Console.Clear();
                            optionChoose = 6;
                            optionActions[6]();
                            break;
                        case 7:
                            Console.Clear();
                            optionChoose = 7;
                            optionActions[7]();
                            break;
                        case 8:
                            Console.Clear();
                            optionChoose = 8;
                            optionActions[8]();
                            break;
                        case 9:
                            Console.Clear();
                            optionChoose = 9;
                            optionActions[9]();
                            break;
                        case 10:
                            Console.Clear();
                            UnitOfWork.Users.MenuBack();
                            break;
                    }
                }
            }
        }
    }
}
