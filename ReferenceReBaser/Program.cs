using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Konfidence.ReferenceReBaserApp
{
    class Program
    {
        static void Main(string[] args)
        {

            if (args.Length == 0)
            {
                Console.WriteLine("geen solution folder op de commandline meegegeven!");
            }
            else
            {
                string solutionFolder = args[0];

                solutionFolder = solutionFolder.TrimEnd('"'); // bug in visualstudio macros

                if (!solutionFolder.EndsWith(@"\"))
                {
                    solutionFolder += @"\";
                }

                MainReferenceReBaser mainReferenceReBaser = new MainReferenceReBaser();

                mainReferenceReBaser.Execute(solutionFolder);
            }
        }
    }
}
