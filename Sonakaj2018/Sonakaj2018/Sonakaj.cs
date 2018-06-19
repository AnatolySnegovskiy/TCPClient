using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sonakaj2018
{
    class Sonakaj
    {
        private int[] numberArray = new int[2018];
        private int partСounter;
        DateTime start;

        public Sonakaj()
        {
            start = DateTime.Now;
            CreateArray();
   
            bool result = Int32.TryParse(Console.ReadLine(), out int ThreadCount);

            if (!result)
            {
                ThreadCount = 5;
            }

            Dictionary<int, int> PartArray = new Dictionary<int, int>();
            int PartSize = numberArray.Length / ThreadCount;
            int PartArrayId = 0;
            int ThreadCounter = 0;
            Task[] Tasks = new Task[ThreadCount + 1];

            for (int i = 0; i < numberArray.Length; i++)
            {
                PartArray[i] = numberArray[i];
                PartArrayId++;

                if (PartArrayId == PartSize)
                {
                    PartArrayId = 0;
                    Tasks[ThreadCounter] = FactorialAsync(PartArray);
                    PartArray = new Dictionary<int, int>();
                    ThreadCounter++;
                }
            }

            Tasks[ThreadCounter] = FactorialAsync(PartArray);
            RunWaiteAllResponce(Tasks);
            
        }

        private void RunWaiteAllResponce(Task[] Tasks)
        {
            Task.WaitAll(Tasks);
            Console.WriteLine("Асинхроны завершены");

            WriteDataToFile();

            var mediana = new CalcMediana().GetMedian(numberArray);
            Console.WriteLine("Медиана = {0}", mediana);
            DateTime end = DateTime.Now;
            Console.WriteLine((end - start).ToString());
            Console.ReadLine();
        }

        private void WriteDataToFile()
        {
            string newstring = "";

            for (int i = 0; i < numberArray.Length; i++)
            {
                newstring += i.ToString() + " => " + numberArray[i].ToString() + "\n";
            }

            using (FileStream fstream = new FileStream(@"D:\note.txt", FileMode.OpenOrCreate))
            {
                byte[] array = System.Text.Encoding.Default.GetBytes(newstring);
                fstream.Write(array, 0, array.Length);
                Console.WriteLine("Текст записан в файл");
            }
        }

        private void CreateArray()
        {
            for (int i = 1; i <= 2018; i++)
            {
                numberArray[i - 1] = i;
            }
        }

        private Task FactorialAsync(Dictionary<int, int> PartArray)
        {
            return Task.Run(() =>
            {
                partСounter++;

                foreach (KeyValuePair<int, int> kvp in PartArray)
                {
                    numberArray[kvp.Key] = GetDataTCP(kvp.Value);
                    Console.WriteLine(kvp.Key.ToString() + " => " + kvp.Value.ToString());
                }
            });
        }

        private int GetDataTCP(int data)
        {
            TCPClient TCP = new TCPClient();
            int intData = 0;

            if (TCP.Write(data.ToString()))
            {
                string str = TCP.Read();
                Regex regexObj = new Regex(@"[^\d]");
                str = regexObj.Replace(str, "");
                Int32.TryParse(str, out intData);
                Console.WriteLine(intData.ToString());
            }

            return intData;
        }
    }
}
