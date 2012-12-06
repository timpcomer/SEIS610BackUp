using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Model
{
    [Serializable]
    public abstract class Node
    {
        protected Node _parent;
        protected Node _left;
        protected Node _right;
        protected int _type;
        protected bool _divideByZeroFlag;
        protected bool _isLeftChild;
        //Random number generator for all nodes
        protected static Random _rGen = new Random();

        /// <summary>
        /// Access to the left child
        /// </summary>
        public Node Left { get { return _left; } set { _left = value; } }
        /// <summary>
        /// Access to the right child
        /// </summary>
        public Node Right { get { return _right; } set { _right = value; } }
        /// <summary>
        /// Access to the parent
        /// </summary>
        public Node Parent { get { return _parent; } set { _parent = value; } }
        /// <summary>
        /// Type stored generically as int.
        /// Values will be 0-10 for operands and 0-3 for operators
        /// </summary>
        public int Type { get { return _type; } }
        /// <summary>
        /// True if it is a left child.
        /// False if it is a right child or if it has no parent.
        /// </summary>
        public bool IsLeftChild { get { return _isLeftChild; } set { _isLeftChild = value; } }
        /// <summary>
        /// Flag for if the there is a zero evaluated as a denominator.
        /// </summary>
        public bool DivideByZeroFlag 
        { 
            get 
            { 
                return _divideByZeroFlag ||
                    (HasChildren()? (_left.DivideByZeroFlag || _right.DivideByZeroFlag) : false); 
            } 
        }

        //Classes that must be included in both operand subclass and operator subclass

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public abstract bool HasChildren();
        /// <summary>
        /// Evaluates the current node and its children
        /// </summary>
        /// <param name="valueX"></param>
        /// <returns></returns>
        public abstract double Evalate(int valueX);
        /// <summary>
        /// Tries to mutate children and if it fails tries to mutate itself
        /// </summary>
        /// <returns></returns>
        public abstract bool Mutate();
        /// <summary>
        /// Returns the size the subtree with the current node as root
        /// </summary>
        /// <returns></returns>
        public abstract int Size();
        /// <summary>
        /// Selects itself or one of its children
        /// </summary>
        /// <returns></returns>
        public abstract Node GetRandomNode();
        public abstract Node GetRandomNode(bool isOperator);
        public virtual bool HasParent()
        {
            return _parent != null;
        }
        public abstract Node GetCopy();
        public abstract bool Grow(int currHeight);
        public abstract bool Shrink(int currHeight);
    }
}
