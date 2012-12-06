using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Model
{
    [Serializable]
    public class Individual
    {
        private double _fitness;
        private Node _root;
        public Node Root { get { return _root; } }
        public int TreeSize { get { return _root.Size(); } }
        public double Fitness { get { return _fitness; } }
        public bool DivideByZero { get { return _root.DivideByZeroFlag; } }

        /// <summary>
        /// Creates a new individual with a random nodes.
        /// </summary>
        public Individual() { _root = new Operator(); }

        /// <summary>
        /// Creates a Node with the root of the tree specified.
        /// </summary>
        /// <param name="root">The root node of the tree the individual contains</param>
        public Individual(Node root) { _root = root; }

        /// <summary>
        /// Calcuates the fitness of this individual.
        /// </summary>
        /// <returns>Double:The sum of the absolute value of differences between this tree's values and the goal's</returns>
        public void CalculateFitness()
        {
            double error = 0.0;
            foreach(int key in Settings.FitnessData.Keys)
            {
                error += Math.Abs(Settings.FitnessData[key] - _root.Evalate(key));
            }
            _fitness = error;
        }
        /// <summary>
        /// Mutates a single Node in the tree
        /// </summary>
        /// <returns>Whether there was a mutation or not, true for mutation, false for no change</returns>
        public bool Mutate() { return _root.Mutate(); }
        /// <summary>
        /// Selects a radom node
        /// </summary>
        /// <returns></returns>
        public Node SelectRandomNode()
        {
            return new Operator();
        }
        public override string ToString()
        {
            return _root.ToString();
        }
        /// <summary>
        /// Gets a random node of this individual.
        /// </summary>
        /// <returns>Random Node</returns>
        public Node GetRandomNode()
        {
            return _root.GetRandomNode();
        }

        /// <summary>
        /// Gets a random node of either type operator or operand
        /// True is for operator and false is operend
        /// </summary>
        /// <param name="isOperator">True=Operator, False=Operand</param>
        /// <returns></returns>
        public Node GetRandomNode(bool isOperator)
        {
            return _root.GetRandomNode(isOperator);
        }

        /// <summary>
        /// Creates a copy of this individual as a new instance.
        /// </summary>
        /// <returns>A new instance of this individaul</returns>
        public Individual CreateCopy()
        {
            var root = _root.GetCopy();
            root.IsLeftChild = false;
            var ind = new Individual(root);
            return ind;
        }

        /// <summary>
        /// Returns true if the individual has grown.
        /// Returns false if the individual does not need to grow.
        /// </summary>
        /// <returns></returns>
        public bool Grow()
        {
            if (_root.Size() < Settings.MinTreeHeight)
                if (_root.HasChildren())
                    return _root.Grow(1);
                else
                {
                    _root = new Operator();
                    return true;
                }
            else
                return false;
        }

        /// <summary>
        /// Returns true if the individual has shrunk
        /// Return false if it is does not need to shrink
        /// </summary>
        /// <returns></returns>
        public bool Shrink()
        {
            if (_root.Size() > Settings.MaxTreeCrossoverHeight)
                return _root.Shrink(1);
            else
                return false;
        }
    }
}
