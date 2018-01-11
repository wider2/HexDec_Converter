using System;
using System.Collections.Generic;
using System.Globalization;

namespace HexDec_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            Boolean fileInput = false, fileOutput=false;
            string hexInput = "", fileNameInput = "", fileNameOutput="", prevArg ="";

            for (int i=0; i < args.Length; i++)
            {
                if (args[i] == "-i") fileInput = true;
                if (args[i] == "-o") fileOutput = true;
                if (prevArg == "-i") fileNameInput = args[i];
                if (prevArg == "-o") fileNameOutput = args[i];
                prevArg = args[i];
            }

            if (fileNameInput !="" && fileNameInput == fileNameOutput)
            {
                Console.WriteLine("Warning. You input the same for output file.");
                fileNameOutput = "output_" + fileNameInput;
            }
            if (fileInput == true)
            {
                hexInput = System.IO.File.ReadAllText(fileNameInput);
            }
            else
            {
                Console.Write("Enter Hexadecimal values: ");                
                hexInput = Console.ReadLine().ToUpper();
            }
            

            List<int> decimals = Parse.getHexList(hexInput.ToUpper());
            List<string> hexOutput = Parse.getDecimalList(decimals);


            if (fileOutput)
            {
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(fileNameOutput))
                {
                    for (int i = 0; i < decimals.Count; i++)
                    {
                        for (int j = 0; j < hexOutput.Count; j++)
                        {
                            if (i == j) file.WriteLine(decimals[i] + " " + hexOutput[j]);
                        }
                    }
                }
            } else
            {
                Console.WriteLine("");
                Console.WriteLine("Result for Decimal and Hexadecimal values: ");
                for (int i = 0; i < decimals.Count; i++)
                {
                    for (int j = 0; j < hexOutput.Count; j++)
                    {
                        if (i == j) Console.WriteLine(decimals[i] + " " + hexOutput[j]);
                    }
                }
            }
            
            Console.WriteLine("");
            if (!fileInput)
            {
                Console.WriteLine("Press Enter to exit");
                Console.ReadKey();
            }
        }
    }

    static class Parse
    {
        public static List<int> getHexList(string input)
        {
            List<int> list = new List<int>();
            string hexValue;
            int intValue, outValue;

            //Input can contain 1..N values, separated by " " or "," or ";" or a line break (CR, LF or CRLF).
            input = input.Replace(", ", ",");
            input = input.Replace(" ", ",");
            input = input.Replace(";", ",");
            input = input.Replace(Environment.NewLine, ",");
            input = input.Replace("\r\n", ",");
            input = input.Replace("\n\r", ",");
            input = input.Replace("\r", ",");
            input = input.Replace("\n", ",");
            if (!input.Contains(",")) input += ",";

            try
            {
                if (input.IndexOf(",") > 0)
                {
                    string[] words = input.Split(',');
                    for (int i = 0; i < words.Length; i++)
                    {
                        hexValue = words[i];
                        hexValue = hexValue.Replace(" ", "");
                        if (string.IsNullOrEmpty(hexValue)) hexValue = "";
                        if (hexValue.Length > 0)
                        {
                            if (hexValue.StartsWith("0X"))
                            {
                                list.Add(HexToDec(hexValue));
                            }
                            else
                            {
                                //if it is really decimal
                                if (int.TryParse(hexValue, out outValue))
                                {
                                    intValue = int.Parse(hexValue, CultureInfo.InvariantCulture);
                                    list.Add(intValue);
                                }
                                else
                                {
                                    list.Add(0);
                                }
                            }
                        }
                    }
                }
            } catch (Exception ex)
            {
                throw ex;
            }
            return list;
        }

        public static List<string> getDecimalList(List<int> input)
        {
            List<string> output = new List<string>();

            foreach (int item in input)
            {
                if (item == 0)
                {
                    output.Add("Invalid decimal value.");
                }
                else
                {
                    output.Add(Parse.DecToHex(item));
                }
            }
            return output;
        }


        public static int HexToDec(string hex)
        {
            if (string.IsNullOrEmpty(hex)) hex = "";
            hex = hex.Replace("0X", "");
            hex = hex.Replace(" ", "");

            int result = 0, count = hex.Length - 1;
            for (int i = 0; i < hex.Length; i++)
            {
                int temp = 0;
                switch (hex[i])
                {
                    case 'A': temp = 10; break;
                    case 'B': temp = 11; break;
                    case 'C': temp = 12; break;
                    case 'D': temp = 13; break;
                    case 'E': temp = 14; break;
                    case 'F': temp = 15; break;
                    default: temp = -48 + (int)hex[i]; break;
                }
                result += temp * (int)(Math.Pow(16, count));
                count--;
            }
            return result;
        }


        public static string DecToHex(int x)
        {
            string result = "";

            while (x != 0)
            {
                if ((x % 16) < 10)
                    result = x % 16 + result;
                else
                {
                    string temp = "";
                    switch (x % 16)
                    {
                        case 10: temp = "A"; break;
                        case 11: temp = "B"; break;
                        case 12: temp = "C"; break;
                        case 13: temp = "D"; break;
                        case 14: temp = "E"; break;
                        case 15: temp = "F"; break;
                    }
                    result = temp + result;
                }
                x /= 16;
            }
            return "0x" + result;
        }
    }


}