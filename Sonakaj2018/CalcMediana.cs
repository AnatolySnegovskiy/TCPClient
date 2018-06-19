using System;

namespace Sonakaj2018
{
    class CalcMediana
    {
        public double GetMedian(int[] sourceNumbers)
        {
            if (sourceNumbers == null || sourceNumbers.Length == 0)
            {
                throw new Exception("Median of empty array not defined.");
            }

            int[] sortedPNumbers = (int[])sourceNumbers.Clone();
            Array.Sort(sortedPNumbers);
            int size = sortedPNumbers.Length;
            int mid = size / 2;
            double median = (size % 2 != 0) ? sortedPNumbers[mid] : (sortedPNumbers[mid] + sortedPNumbers[mid - 1]) / 2;

            return median;
        }
    }
}
