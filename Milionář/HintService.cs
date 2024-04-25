using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Milionář
{
    internal class HintService
    {
        double[] people;
        // {0.8,0.7,0.6,0.8,0.7,0.7,0.7,0.8};

        public HintService(double[] people)
        {
            this.people = people;
        }

        public int[] getPeopleHint(int category, int numAnsweras, int rightAnswer)
        {
            int percents = 100;
            Random rnd = new Random();
            int[] result = new int[numAnsweras];
            double temp = rnd.NextDouble();

            if (numAnsweras > 2)
            {
                if (temp < people[category - 1])
                {
                    result[rightAnswer] = rnd.Next(40, percents);
                }
                else
                {
                    result[rightAnswer] = rnd.Next(0, percents - 20);
                }
                percents -= result[rightAnswer];

                for (int i = 0; i < numAnsweras - 1; i++)
                {
                    if (i != rightAnswer)
                    {
                        result[i] = rnd.Next(0, percents - 1);
                        percents -= result[i];
                    }
                    if(i+1 == rightAnswer && i+1 == 3)
                    {
                        result[i] = percents;
                        break;
                    }
                }
            }
            else 
            {
                if (temp < people[category - 1])
                {
                    result[0] = rnd.Next(40, percents);
                }
                else
                {
                    result[0] = rnd.Next(0, percents - 20);
                }
                percents -= result[rightAnswer];
                result[1] = percents;
            }
            return result;

        }

        public int[] get5050Hint(int[] answers, int rightAnswer)
        {
            Random rnd = new Random();
            List<int> numberList = answers.ToList();
            numberList.RemoveAt(rightAnswer);
            numberList.RemoveAt(rnd.Next(0, answers.Length-1));
            int[] answers_2 = numberList.ToArray();
            return answers_2;
        }
    }
}
