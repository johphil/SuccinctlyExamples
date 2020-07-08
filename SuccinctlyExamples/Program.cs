//#define EXERCISE6_1
#define EXERCISE6_2

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace SuccinctlyExamples
{
    class Program
    {
        static void Main(string[] args)
        {
#if (EXERCISE6_1)
            Exercise6_1 e = new Exercise6_1();
            e.Menu();
#endif //EXERCISE6_1
#if (EXERCISE6_2)
            Exercise6_2 e = new Exercise6_2();
            e.Menu();
#endif //EXERCISE6_2
        }
    }
}
