using System.Collections.Generic;
using System.IO;
using System.Linq;
using static Sssh.FileManagement.FileHelper;

namespace Sssh.FileManagement
{
    public interface IFileHelper
    {
        IDictionary<int, double> loadFile(string filePath);
        IDictionary<int, double> Data { get; set; }
        string GetDaysForMaxProfit(IDictionary<int, double> dict);
    }

    public class FileHelper : IFileHelper
    {
        protected IDictionary<int, double> rawData;

        public FileHelper() { }
        public FileHelper(IDictionary<int, double> dict)
        {
            this.rawData = dict;
        }
        public IDictionary<int, double> loadFile(string filePath)
        {
            rawData = new Dictionary<int, double>();
            string rawContent = string.Empty;
            int count = 1;

            using (StreamReader sr = new StreamReader(filePath))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    rawContent = line;
                }
            }

            string[] split = rawContent.Split(',');

            foreach (string s in split)
            {
                rawData.Add(count, double.Parse(s));
                count++;
            }

            return rawData;
        }

        public string GetDaysForMaxProfit(IDictionary<int, double> dict)
        {
            int lowestDay = 0, highestDay = 0, buyingDay = 0, sellingDay = 0;
            double lowestPrice = 0, highestPrice = 0, buyingPrice = 0, sellingPrice = 0, profit = 0;

            var keyOfMinValue = dict.Aggregate((x, y) => x.Value < y.Value ? x : y).Key;
            var keyOfMaxValue = dict.Aggregate((x, y) => x.Value > y.Value ? x : y).Key;

            if (keyOfMinValue < keyOfMaxValue)
            {
                buyingDay = lowestDay = keyOfMinValue;
                buyingPrice = lowestPrice = dict[keyOfMinValue];
                sellingDay = highestDay = keyOfMaxValue;
                sellingPrice = highestPrice = dict[keyOfMaxValue];
            }
            else
            {
                foreach (KeyValuePair<int, double> kvp in dict)
                {
                    if (lowestPrice == 0 || kvp.Value < lowestPrice)
                    {
                        lowestDay = kvp.Key;
                        lowestPrice = kvp.Value;
                    }
                    else if (highestPrice == 0 || kvp.Value > highestPrice)
                    {
                        highestDay = kvp.Key;
                        highestPrice = kvp.Value;
                    }

                    if (buyingPrice == 0)
                    {
                        buyingDay = kvp.Key;
                        buyingPrice = kvp.Value;
                    }

                    if (sellingPrice - kvp.Value > profit)
                    {
                        // miss last day for buying as cannot sell within the month
                        if (kvp.Key < dict.Count)
                        {
                            buyingDay = kvp.Key;
                            buyingPrice = kvp.Value;
                        }
                    }
                    else
                    {
                        if (buyingDay > sellingDay)
                        {
                            sellingDay = kvp.Key;
                            sellingPrice = kvp.Value;
                        }
                    }
                    profit = sellingPrice - buyingPrice;
                }
            }

            profit = sellingPrice - buyingPrice;

            return "Lowest: " + lowestDay + ": " + lowestPrice +
                ",Highest: " + highestDay + ": " + highestPrice +
                ",Buy Day: " + buyingDay + ": " + buyingPrice +
                    ",Sell Day: " + sellingDay + ": " + sellingPrice +
                    ",Profit: " + profit;
        }

        public IDictionary<int, double> Data { get => rawData; set => rawData = value; }
    }
}
