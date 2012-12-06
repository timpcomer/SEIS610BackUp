using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Model
{
    public static class Util
    {
        public static int GetInt(object o)
        {
            return System.Convert.ToInt32(o);
        }
        
        public static double GetDouble(object o)
        {
            return System.Convert.ToDouble(o);
        }

        /// <summary>
        /// returns an in-memory compiled executable with "Evaluate" as its sole
        /// exposed member which evaluates the input equation at a given int
        /// </summary>
        /// <param name="equation">the equation to be compiled with "x" as its variable, returns a double</param>
        /// <returns></returns>
        public static MethodInfo GetCompiledEquation(string equation)
        {
            var provider = new CSharpCodeProvider();
            var options = new CompilerParameters()
            {
                GenerateInMemory = true
            };
            var results = provider.CompileAssemblyFromSource(options, new[]
            {
                @"
                    public class Equation
                    {
                        public static double Evaluate(int x)
                        {
                            try { return " + equation + @"; }
                            catch { return double.MaxValue; }
                        }
                    }
                "
            });
            return results.CompiledAssembly.GetType("Equation").GetMethod("Evaluate");
        }

        /// <summary>
        /// returns a deep copy of the input object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">the object to be cloned</param>
        /// <returns></returns>
        public static T DeepCopy<T>(T source)
        {
            using (var ms = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, source);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// calls the "function" method, passing "inputX"
        /// </summary>
        /// <param name="function"></param>
        /// <param name="inputX"></param>
        /// <returns></returns>
        public static double CalculateAtX(MethodInfo function, int inputX)
        {
            return Util.GetDouble(function.Invoke(null, new object[] { inputX }));
        }

        public static Random rand = new Random();

        public static Dictionary<int, double> GetGoalData(MethodInfo method)
        {
            int current = Settings.MinInput,
                max = Settings.MaxInput,
                range = Math.Abs(current - max);

            if (current < 0 && max > 0)
                range++;//in case test data range spans 0

            Dictionary<int, double> vals = new Dictionary<int, double>(range);

            //calculate our goal foreach value in our range, and add to our dictionary
            while(current <= max)
            {
                vals[current] = Util.CalculateAtX(method, current);
                current++;
            }

            return vals;
        }
    }
}
