using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticProgramming.Model;

namespace GeneticProgramming
{
    public class GenerationManager
    {
        List<Individual> currGen = new List<Individual>();
        List<Individual> nextGen = new List<Individual>();

        private int _generationCount = 0;
        public int GenerationCount { get { return _generationCount; } }

        private DateTime startTime, endTime;

        private bool _running = true;
        public bool Running { get { return _running; } }

        //build initial generation on instantiation
        public GenerationManager()
        {
            startTime = System.DateTime.Now;
            createInitialGeneration();
        }

        //Sort List of Individuals by Fitness
        public void sortGeneratartion(List<Individual> x)
        {
            IndvSortOrder comparer = new IndvSortOrder();
            //implement faster sortedList later
            //List<Individual> sortedList = new List<Individual>();
            x.Sort(comparer);
        }

        //get next generation attempting a tournement style selection.
        public Individual getNextGenerationTournamentStyle()
        {
            Individual newInd;
            for (int i = 0; i < Settings.GenerationSize; i++)
            {
                Individual tmp = currGen.ElementAt(i);
                tmp.CalculateFitness();

                //break condition?
                if (tmp.Fitness == 0 && !tmp.DivideByZero)
                {
                    endTime = System.DateTime.Now;
                    TimeSpan finalTime = endTime - startTime;

                    Console.Write("Mission Complete, target individual " + i.ToString() + "found in " + _generationCount + " generations in " + finalTime);
                    _running = false;
                    return tmp;
                }

                if (tmp.DivideByZero)
                {
                    currGen.Remove(tmp);
                    do
                    {
                        newInd = new Individual();
                        newInd.CalculateFitness();
                        tmp = newInd;
                        tmp.CalculateFitness();
                    } while (newInd.DivideByZero);

                    currGen.Insert(i, newInd);
                }


            }
            
            sortGeneratartion(currGen);
            Individual output = Util.DeepCopy(currGen.ElementAt(0));
            //printCurrentGenerationToConsole();

            int reproductionIndex = (int)Math.Ceiling(Settings.ReproductionProb * Settings.GenerationSize);
            int repro = 0;

            for (int i = 0; i < Settings.GenerationSize - reproductionIndex; )
            {

                double whatDo = Util.rand.NextDouble();
                //calculate index of top X%.  If Crossover produces two new tree the index has to update by two each time, meaning we need an even population size, and reproduction index?
               

                if (whatDo < Settings.ReproductionProb)
                {
                    Individual survivor = currGen.ElementAt(repro);
                    repro++;
                    nextGen.Add(survivor);
                    //currGen.Remove(survivor);
                    i++;
                }

                if (whatDo > Settings.ReproductionProb && whatDo < 1 - Settings.MutateProb)
                {
                    
                    Individual survivor = selectIndividual(currGen, 5);
                    Individual survivor2 = selectIndividual(currGen, 5);
                    var survivorCopy = survivor.CreateCopy();

                    var newInd1 = crossover(survivor, survivor2.CreateCopy());
                    //var newInd2 = crossover(survivor2, survivorCopy);
                    nextGen.Add(newInd1);
                    //nextGen.Add(newInd2);

                    i = i + 1;
                }

                if (whatDo > 1 - Settings.MutateProb)
                {
                    Individual survivor = selectIndividual(currGen, 3);
                    survivor.Mutate();
                    nextGen.Add(survivor);
                    i++;
                }
            }

            //add 10% new blood
            for (int i = Settings.GenerationSize - reproductionIndex; i < Settings.GenerationSize; i++)
            {
                nextGen.Insert(i, new Individual());
            }

            _generationCount++;
            
           
            //clear the current, make next current, clear next.  Memory management!?
            currGen.Clear();
            foreach (Individual i in nextGen)
            {
                currGen.Add(Util.DeepCopy(i));
            }
            nextGen.Clear();

            return output;
        }

        //next generation attempting the most basic method talked about in class.
        public Individual getNextGeneration()
        {
            Individual newInd;
            for (int i = 0; i < Settings.GenerationSize; i++)
            {
                Individual tmp = currGen.ElementAt(i);
                tmp.CalculateFitness();

                if (tmp.DivideByZero)
                {
                    currGen.Remove(tmp);
                    do
                    {
                        newInd = new Individual();
                        newInd.CalculateFitness();
                        tmp = newInd;
                    } while (newInd.DivideByZero);

                    currGen.Insert(i, newInd);
                }

                //break condition?
                if (tmp.Fitness == 0)
                {
                    endTime = System.DateTime.Now;
                    TimeSpan finalTime = endTime - startTime;
                    Console.Write("Mission Complete, target individual " + i.ToString() + "found in " + _generationCount + " generations in " + finalTime);
                    _running = false;
                    break;
                }
            }

            if (_running == false) { return null; }

            //printCurrentGenerationToConsole();
            sortGeneratartion(currGen);
            Individual output = currGen.ElementAt(0);
            //printCurrentGenerationToConsole();

            //calculate index of top X%.  If Crossover produces two new tree the index has to update by two each time, meaning we need an even population size, and reproduction index?
            int reproductionIndex = (int)Math.Ceiling(Settings.ReproductionProb * Settings.GenerationSize);

            //reproduce the best X %
            for (int i = 0; i < reproductionIndex; i++)
            {
                nextGen.Insert(i, Util.DeepCopy(currGen.ElementAt(i)));
            }

            //crossover the middle 80%
            for (int i = reproductionIndex; i < Settings.GenerationSize; i = i + 1)
            {
                Individual newCrossOverIndv = crossover(currGen.ElementAt(Util.rand.Next(0, Settings.GenerationSize-reproductionIndex)), currGen.ElementAt(Util.rand.Next(0, reproductionIndex)));
                nextGen.Add(newCrossOverIndv);
            }

            //traverse the new generation and mutate the current individual if the probability hits
            foreach (Individual ind in nextGen)
            {
                if (Settings.MutateProb > Util.rand.NextDouble())
                {
                    ind.Mutate();
                }
            }

            ////add 10% new blood
            //for (int i = Settings.GenerationSize - reproductionIndex; i < Settings.GenerationSize; i++ )
            //{
            //    nextGen.Insert(i, new Individual());
            //}

            _generationCount++;
           
            //Console.WriteLine(output.Fitness + "|" + output);
            //clear the current, make next current, clear next.  Memory management!?
            currGen.Clear();
            foreach (Individual i in nextGen)
            {
                currGen.Add(Util.DeepCopy(i));
            }
            nextGen.Clear();

            return output;
        }

        //create initial population
        private void createInitialGeneration()
        {
            for (int individualCount = 0; individualCount < Settings.GenerationSize; individualCount++)
            {
                Individual newIndv = new Individual();
                currGen.Add(newIndv);
            }
            _generationCount++;
        }

        //comparator to sort Individual objects based on fitness
        private class IndvSortOrder : IComparer<Individual>
        {
            public int Compare(Individual x, Individual y)
            {
                return x.Fitness.CompareTo(y.Fitness);
            }
        }

        public Individual getBestInd()
        {
            double fitessed = 10000;
            Individual tmp = null ;
            foreach (Individual i in currGen)
            {
                if (i.Fitness < fitessed)
                {
                    tmp = i;
                }
            }

            return tmp;
        }

        //debug print fitness and string representation of object
        public void printCurrentGenerationToConsole()
        {
            foreach (Individual i in currGen)
            {
                i.CalculateFitness();
            }
            sortGeneratartion(currGen);
            foreach (Individual i in currGen)
            {

                System.Console.Write(i.Fitness + "| " + i.ToString());
                System.Console.WriteLine();

            }

            Console.Write("--------");
            Console.WriteLine();
        }



        //placeholder till crossover is implemented
        /// <summary>
        /// Returns a new individual and toChanged from the currGen.
        /// </summary>
        /// <param name="toChange">Individual to be Modified</param>
        /// <param name="toTakeFrom">Individual that will be copied and then have a node to be modified from</param>
        /// <returns>The modified individual</returns>
        public Individual crossover(Individual toChange, Individual toTakeFrom)
        {
            var copyToTakeFrom = toTakeFrom.CreateCopy();
            Node fromToChange = toChange.GetRandomNode();
            Node toAdd;
            if (fromToChange.HasChildren())
                toAdd = copyToTakeFrom.GetRandomNode(true);
            else
                toAdd = copyToTakeFrom.GetRandomNode(false);

            if (!fromToChange.HasParent())
            {
                var ind = new Individual(toAdd);
                if (ind.TreeSize < Settings.MinTreeHeight)
                    ind.Grow();
                ind.CalculateFitness();
                currGen.Remove(toChange);
                return ind;
            }
            else
            {
                var parent = fromToChange.Parent;
                toAdd.Parent = parent;
                if (fromToChange.IsLeftChild)
                    parent.Left = toAdd;
                else
                    parent.Right = toAdd;
                if (toChange.TreeSize < Settings.MinTreeHeight)
                    toChange.Grow();
                if (toChange.TreeSize > Settings.MaxTreeCrossoverHeight)
                    toChange.Shrink();
                toChange.CalculateFitness();
                currGen.Remove(toChange);
                return toChange;
            }
            
        }
        public Individual selectIndividual(List<Individual> indv, int tournamentSize)
        {
            List<Individual> tournament = new List<Individual>();
            for (int i = 0; i < tournamentSize; i++)
            {
                tournament.Add(indv.ElementAt(Util.rand.Next(0, currGen.Count)));
            }

            Individual bestIndividual = new Individual();
            double bestFitness = 10000;
            foreach (Individual i in tournament)
            {
                if (i.Fitness < bestFitness)
                {
                    bestIndividual = i;
                    bestFitness = i.Fitness;
                }
            }


            return bestIndividual;



        }

    }
}



