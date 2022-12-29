using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwentyFifteen.Days
{
    internal class Day25
    {
    }
}


/*
 * 
 * 
 * 
 * 
 * using System;

class Program {

    static int SumAllNumbersInRange(int x, int y) {
      int i = 0;
      for (int i2 = x; i2 <= y; i2++)
      {
        i += i2;
      }
      return i;
    }

    static int FindSequence(int targetX, int targetY) 
  {
       int firstValueInColumnX = SumAllNumbersInRange(1, targetX);
      if (targetY == 1) return firstValueInColumnX;
      int actualValue = firstValueInColumnX + SumAllNumbersInRange(targetX, targetX + targetY - 2);

      return actualValue;
    }
  
  static void Main(string[] args) {
        int targetX = 3075;
        int targetY = 2981;

        int requiredIterations = FindSequence(targetX, targetY);

        long value = 20151125;
        for (int i = 1; i < requiredIterations; i++) 
        {
          value = value * 252533;
          value = value % 33554393; 
        }
       

      Console.WriteLine(value);
    }
}
 * 
 * 
 */