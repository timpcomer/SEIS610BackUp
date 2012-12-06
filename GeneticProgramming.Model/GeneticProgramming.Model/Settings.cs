using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace GeneticProgramming.Model
{
    public static class Settings
    {
        public static double ReproductionProb { get { return Util.GetDouble(ConfigurationManager.AppSettings["ReproductionProb"]); } }
        public static double MutateProb { get { return Util.GetDouble(ConfigurationManager.AppSettings["MutateProb"]); } }
        public static double OperandProb { get { return Util.GetDouble(ConfigurationManager.AppSettings["OperandProb"]); } }
        public static double VariableProb { get { return Util.GetDouble(ConfigurationManager.AppSettings["VariableProb"]); } }
        public static int MaxTreeHeight { get { return Util.GetInt(ConfigurationManager.AppSettings["MaxTreeHeight"]); } }
        public static int MaxTreeCrossoverHeight { get { return (int)(MaxTreeHeight * Util.GetDouble(ConfigurationManager.AppSettings["MaxTreeCrossoverHeightModifier"])); } }
        public static int MinTreeHeight { get { return Util.GetInt(ConfigurationManager.AppSettings["MinTreeHeight"]); } }
        public static int MinInput { get { return Util.GetInt(ConfigurationManager.AppSettings["MinInput"]); } }
        public static int MaxInput { get { return Util.GetInt(ConfigurationManager.AppSettings["MaxInput"]); } }
        public static int GenerationSize { get { return Util.GetInt(ConfigurationManager.AppSettings["GenerationSize"]); } }
        public static int HardResetGenerationCount { get { return Util.GetInt(ConfigurationManager.AppSettings["HardResetGenerationCount"]); } }

        private static Dictionary<int, double> _goalData = null;
        public static Dictionary<int, double> FitnessData
        {
            get
            {
                if (_goalData == null)
                    _goalData = Util.GetGoalData(Util.GetCompiledEquation(ConfigurationManager.AppSettings["GoalEquation"]));
                return _goalData;
            }
        }
    }
}
