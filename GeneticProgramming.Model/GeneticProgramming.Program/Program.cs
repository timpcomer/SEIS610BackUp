using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticProgramming.Model;
using System.Configuration;
using System.Reflection;

namespace GeneticProgramming
{
    public static class Program
    {
        public static void Main(string[] args)
        {
           
            GenerationManager gm = new GenerationManager();

            //var operand = new Operand(OperandEnum.x);
            //var operand2 = new Operand(OperandEnum.three);
            //var operat = new Operator(OperatorEnum.add, operand, operand2);
            //var indiv = new Individual(operat);
            //indiv.Grow();
            //Console.WriteLine("\n{0}", indiv.TreeSize);
            //Operator op = new Operator(OperatorEnum.multiply, operat, operand);
            //int i;
            //Console.WriteLine("{0}", Settings.MaxTreeCrossoverHeight);
            //for (i = 0; i < 26; i++)
            //{
            //    Console.WriteLine("{0}", i);
            //    operand2 = new Operand();
            //    op = new Operator(OperatorEnum.multiply, operat, operand2);
            //    operat = op;
            //    Console.WriteLine("\n--{0}\n", op.Size());
            //}

            //Console.WriteLine("\n--{0}\n", op.Size());
            //var ind = new Individual(op);

            //ind.Shrink();
            //Console.WriteLine("\n{0}", ind.TreeSize);

            //int i;
            //for (i = 0; i < 10; i++)
            //{
            //    Individual test1 = new Individual();
            //    Individual test2 = new Individual();
            //    Console.WriteLine("{0} | {1}", test1.ToString(), test2.ToString());

            //    var crossoverTest = gm.crossover(test1, test2);
            //    Console.WriteLine("{0}\n", crossoverTest.ToString());
            //}
            //Console.WriteLine(test1.ToString());
            //Console.WriteLine(test1.TreeSize);
            //Console.WriteLine(test2.ToString());

            //List<Individual> test = gm.crossover(test1, test2);

            //Console.WriteLine(test[0].ToString());
            //Console.WriteLine(test[1].ToString());
            
            while (gm.Running)
            {
                Console.Write("\r{0}", "Gen" + gm.GenerationCount);
                Individual bestSoFar = gm.getNextGenerationTournamentStyle();
                //Individual bestSoFar = gm.getNextGeneration();
                //Console.ReadKey();
                
                
                //gm.printCurrentGenerationToConsole();

              
               //Console.WriteLine("Gen" + gm.GenerationCount + " " + bestSoFar.Fitness + " | " + bestSoFar.ToString());
               // Console.WriteLine("------------");
               // Console.ReadKey();
                if (gm.GenerationCount > Settings.HardResetGenerationCount)
                {
                    gm = new GenerationManager();
                }

            }
            

            ////Testing GetRandomNode(isOperator)
            //var ind = new Individual();
            //var operat = ind.GetRandomNode(true);
            //var operend = ind.GetRandomNode(false);
            //Console.WriteLine("{0}\n", ind.ToString());
            //try
            //{
            //    var oTor = (Operator)operat;
            //    Console.WriteLine("{0}\n", oTor.ToString());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Operator error\n");
            //}
            //try
            //{
            //    Console.WriteLine("{0}\n", operend.ToString());
            //    var oRend = (Operand)operend;
            //    Console.WriteLine("{0}\n", oRend.ToString());
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine("Operand Error\n");
            //}

            //Testing CreateCopy
            //var ind = new Individual();
            //var copy = ind.CreateCopy();
            //Console.WriteLine("{0}\n{1}",ind.ToString(), copy.ToString());
            //while (!ind.Mutate()) ;
            //Console.WriteLine("{0}\n{1}", ind.ToString(), copy.ToString());
 

            //Individual.GoalEval = eval;
            //var list = new List<Individual>();
            //int i;
            //for (i = 0; i < 100; i++)
            //{
                //var ind = new Individual();
                //Console.WriteLine("{0} -- {1}\n", ind.TreeSize, Settings.MaxTreeHeight);
                //list.Add(ind);
                //if (ind.TreeSize > Settings.MaxTreeHeight )
                //{
                //    Console.WriteLine("{0}\n", ind.ToString());
                //    var root = ind.Root;
                //    Console.WriteLine("HasParent: {0} | HasChild: {1} | IsleftChild: {2}\n", root.HasParent(), root.HasChildren(), root.IsLeftChild);
                //    var left = root.Left;
                //    Console.WriteLine("\tHasParent: {0} | HasChild: {1} | IsleftChild: {2}\n", left.HasParent(), left.HasChildren(), left.IsLeftChild);
                //    var right = root.Right;
                //    Console.WriteLine("\tHasParent: {0} | HasChild: {1} | IsleftChild: {2}\n", right.HasParent(), right.HasChildren(), right.IsLeftChild);
                //    individualCount++;
                //}
                //generationCount++;
                //var tree = new Operator();
                //if (tree.ToString().Contains('x'))
                //{
                //    Console.WriteLine("{0} : {1} : {2}\n", tree.ToString(), tree.Size(), tree.Evalate(1));
                //    count++;
                //}
                ////if(tree.Mutate())
                //    Console.WriteLine("{0} : {1}\n\n", tree.ToString(), tree.Evalate(1));
                //Console.WriteLine("{0}  {1}\n", ind.ToString() ,ind.CalculateFitness());
                //Console.WriteLine("{0}\n", ind.GetRandomNode().ToString());
            //}
            //Console.WriteLine(generationCount);
            Console.Read();
        }
    }
}
