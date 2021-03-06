﻿using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleAppHost2
{
    class Program
    {
        static void Main(string[] args)
        {
            //this statement loads MyWebApi assembly
            var controllerType = typeof(MyWebApi.Controllers.EmployeesController);

            using (WebApp.Start<Startup>("http://localhost:5000"))
            {

                Console.WriteLine("Server ready... Press Enter to quit.");

                Console.ReadLine();
            }
        }
    }
}
