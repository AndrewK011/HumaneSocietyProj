using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class CsvFileParse
    {
        internal static List<Dictionary<int,string>> CsvParse()
        {
            Dictionary<int, string> csvDictionary = new Dictionary<int, string>();
            var lines = File.ReadAllLines(@"C: \Users\drumm\Desktop\DevCodeCamp\HumaneSocietyStarter\HumaneSociety\animals.csv").Select(x => x.Split(','));
            
            int lineLength = lines.First().Count();
            var CSV = lines.SelectMany(x => x);
            List<Dictionary<int,string>> newAnimalList = new List<Dictionary<int,string>>();

            int i = 1;
            foreach (var data in CSV)
            {
                
                csvDictionary.Add(i, data);
                if (i % lineLength == 0)
                {
                    i = 0;
                    newAnimalList.Add(csvDictionary);
                    csvDictionary = new Dictionary<int, string>();
                }
                i++;
            }
            return newAnimalList;
        }

    }
}
