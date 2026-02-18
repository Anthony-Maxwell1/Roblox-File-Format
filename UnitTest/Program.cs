using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace RobloxFiles.UnitTest
{
    static class Program
    {
        static void PrintTreeImpl(Instance inst, int stack = 0)
        {
            string padding = "";
            string extension = "";

            for (int i = 0; i < stack; i++)
                padding += '\t';

            switch (inst.ClassName)
            {
                case "Script":
                {
                    extension = ".server.lua";
                    break;
                }
                case "LocalScript":
                {
                    extension = ".client.lua";
                    break;
                }
                case "ModuleScript":
                {
                    extension = ".lua";
                    break;
                }
            }

            Console.WriteLine($"{padding}{inst.Name}{extension}");

            var children = inst
                .GetChildren()
                .ToList();

            children.ForEach(child => PrintTreeImpl(child, stack + 1));
        }

        static void PrintTree(string path)
        {
            Console.WriteLine("Opening file...");
            RobloxFile target = RobloxFile.Open(path);

            foreach (Instance child in target.GetChildren())
                PrintTreeImpl(child);

            Debugger.Break();
        }
        
        [STAThread]
        static void Main(string[] args)
        {
            RobloxFile.LogErrors = true;

            if (args.Length > 0)
            {
                string path = args[0];
                PrintTree(path);
            }
            else
            {
                string binaryPath = Path.Combine("Files", "Binary.rbxl");
                string xmlPath = Path.Combine("Files", "Xml.rbxlx");

                RobloxFile bin = RobloxFile.Open(binaryPath);
                RobloxFile xml = RobloxFile.Open(xmlPath);

                Console.WriteLine("Files opened! Pausing execution for debugger analysis...");
                Debugger.Break();

                string binarySavePath = Path.Combine("Files", "Binary_SaveTest.rbxl");
                string xmlSavePath = Path.Combine("Files", "Xml_SaveTest.rbxlx");

                bin.Save(binarySavePath);
                xml.Save(xmlSavePath);
                
                Console.WriteLine("Files saved! Pausing execution for debugger analysis...");
                Debugger.Break();
            }
        }
    }
}
