﻿using ByteBank.Infraestrutura;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByteBank
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var prefixos = new string[] { "http://localhost:5342/" };

            var webApplication = new WebApplication(prefixos);

            webApplication.Iniciar();
        }
    }
}
