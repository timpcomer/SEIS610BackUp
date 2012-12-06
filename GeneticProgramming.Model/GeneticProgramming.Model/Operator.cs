using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Model
{
    [Serializable]
    public class Operator : Node
    {
        /// <summary>
        /// Type as a operator 0-3
        /// </summary>
        public OperatorEnum OperatorType { get { return (OperatorEnum)_type; } set { _type = (int)value; } }

        /// <summary>
        /// Used to create a Tree of Random size set by bounds in the settings.
        /// This Creates the root node wich then creates starts creating its children.
        /// </summary>
        public Operator()
        {
            _isLeftChild = false;
            int currHeight = 1;
            CreateCurrentNode(currHeight, false);
        }
        /// <summary>
        /// Used internally to create nodes of a tree
        /// </summary>
        /// <param name="currHeight"></param>
        /// <param name="minReached"></param>
        private Operator(int currHeight, bool minReached = true)
        {            
            CreateCurrentNode(currHeight, minReached);
        }

        /// <summary>
        /// Used to create a controled tree with the nodes being specified and the type
        /// </summary>
        /// <param name="operatorType"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        public Operator(OperatorEnum operatorType, Node left, Node right)
        {
            OperatorType = operatorType;
            _left = left;
            _right = right;
            _divideByZeroFlag = false;
        }


        /// <summary>
        /// Operators always have children
        /// </summary>
        /// <returns></returns>
        public override bool HasChildren() { return true; }

        /// <summary>
        /// Evalutes the subtree with any variables at x.
        /// </summary>
        /// <param name="valueX"></param>
        /// <returns></returns>
        public override double Evalate(int valueX)
        {
            var leftValue = _left.Evalate(valueX);
            var rightValue = _right.Evalate(valueX);

            if (OperatorType == OperatorEnum.add) { return leftValue + rightValue; }
            else if (OperatorType == OperatorEnum.subtract) { return leftValue - rightValue; } 
            else if (OperatorType == OperatorEnum.multiply) { return leftValue * rightValue; }
            else
            {
                if (rightValue == 0)
                {
                    _divideByZeroFlag = true;
                    return double.MaxValue;
                }
                else { return leftValue / rightValue; }
            }
        }

        /// <summary>
        /// Randomaly mutates chooses what node of the subtree to mutate.
        /// </summary>
        /// <returns></returns>
        public override bool Mutate()
        {
            if (_rGen.NextDouble() < (1.0/(Size() + 1)))
            {
                var oldOpType = OperatorType;
                OperatorType = (OperatorEnum)_rGen.Next((int)OperatorEnum.add+1);
                return oldOpType != OperatorType;
            }
            else if (_rGen.NextDouble() < 0.5) { return _right.Mutate(); }
            else { return _left.Mutate(); }
         }

        /// <summary>
        /// Chooses a random node from the subtree.
        /// </summary>
        /// <returns></returns>
        public override Node GetRandomNode()
        {
            if (_rGen.NextDouble() < (1.0 / (Size() + 1)))
                return this;
            else if (_rGen.NextDouble() < 0.5)
                return _left.GetRandomNode();
            else
                return _right.GetRandomNode();
        }

        public override Node GetRandomNode(bool isOperator)
        {
            if (isOperator && _rGen.NextDouble() < (1.0 / (Size() + 1.0)))
                return this;
            else if (_rGen.NextDouble() < 0.5)
                return Left.GetRandomNode(isOperator);
            else
                return Right.GetRandomNode(isOperator);
        }

        /// <summary>
        /// Gets the height of the subtree
        /// </summary>
        /// <returns></returns>
        public override int Size()
        {
            if (_left.Size() > _right.Size())
                return _left.Size() + 1;
            else return _right.Size() + 1;
        }

        public override string ToString()
        {
            string op = string.Empty;
            switch (OperatorType)
            {
                case OperatorEnum.add:
                    op = "+";
                    break;
                case OperatorEnum.subtract:
                    op = "-";
                    break;
                case OperatorEnum.divide:
                    op = "/";
                    break;
                case OperatorEnum.multiply:
                    op = "*";
                    break;
            }
            return string.Format("({0}{1}{2})", _left.ToString(), op, _right.ToString());
        }

        /// <summary>
        /// Creates the current Node.
        /// Sets its value, flags value, and children
        /// </summary>
        /// <param name="currHeight"></param>
        /// <param name="minReached"></param>
        private void CreateCurrentNode(int currHeight, bool minReached)
        {
            OperatorType = (OperatorEnum)_rGen.Next((int)OperatorEnum.add + 1);
            _divideByZeroFlag = false;
            if (_rGen.NextDouble() < 0.5)
            {
                CreateRight(currHeight + 1);
                if (_right.Size() + currHeight >= Settings.MinTreeHeight || minReached)
                { CreateLeft(currHeight + 1); }
                else
                { CreateLeft(currHeight + 1, minReached); }
            }
            else
            {
                CreateLeft(currHeight + 1);
                if (_left.Size() + 1 >= Settings.MinTreeHeight || minReached)
                { CreateRight(currHeight + 1); }
                else
                { CreateRight(currHeight + 1, minReached); }
            }
        }

        /// <summary>
        /// Creates the left child based upon the given inputs
        /// </summary>
        /// <param name="currHeight"></param>
        /// <param name="minReachedRight"></param>
        private void CreateLeft(int currHeight, bool minReachedRight=true)
        {
            if (currHeight + 1 < Settings.MaxTreeHeight)
            {
                if (currHeight < Settings.MinTreeHeight && !minReachedRight)
                { _left = new Operator(currHeight + 1, minReachedRight); }
                else
                {
                    if (_rGen.NextDouble() < Settings.OperandProb) 
                    { _left = new Operand(); }
                    else 
                    { _left = new Operator(currHeight + 1, minReachedRight); }
                }
            } 
            else { _left = new Operand(); }
            _left.Parent = this;
            _left.IsLeftChild = true;
        }

        /// <summary>
        /// Creates the right child based upon the given inputs
        /// </summary>
        /// <param name="currHeight"></param>
        /// <param name="minReachedLeft"></param>
        private void CreateRight(int currHeight, bool minReachedLeft = true)
        {
            if (currHeight + 1 < Settings.MaxTreeHeight)
            {
                if (currHeight < Settings.MinTreeHeight && !minReachedLeft)
                { _right = new Operator(currHeight + 1, minReachedLeft); }
                else
                {
                    if (_rGen.NextDouble() < Settings.OperandProb)
                    { _right = new Operand(); }
                    else
                    { _right = new Operator(currHeight + 1, minReachedLeft); }
                }
            }
            else { _right = new Operand(); }
            _right.Parent = this;
            _right.IsLeftChild = false;
        }

        /// <summary>
        /// Gets a copy of the current node
        /// </summary>
        /// <returns></returns>
        public override Node GetCopy()
        {
            var left = _left.GetCopy();
            left.IsLeftChild = true;
            var right = _right.GetCopy();
            right.IsLeftChild = false;
            var op = new Operator(this.OperatorType, left, right);
            left.Parent = op;
            right.Parent = op;
            return op;
        }
        /// <summary>
        /// Increases it to min height
        /// </summary>
        /// <param name="currHeight"></param>
        /// <returns></returns>
        public override bool Grow(int currHeight)
        {
            if (Left.Size() == Right.Size())
            {
                if (_rGen.NextDouble() < 0.5)
                {
                    if (!Left.Grow(currHeight+1))
                        _left = new Operator(currHeight + 1, false);
                }
                else
                {
                    if (!Right.Grow(currHeight + 1))
                        _right = new Operator(currHeight + 1, false);
                }
            }
            else if (Left.Size() > Right.Size())
            {
                if (!Left.Grow(currHeight + 1))
                    _left = new Operator(currHeight + 1, false);
            }
            else
            {
                if (!Right.Grow(currHeight + 1))
                    _right = new Operator(currHeight + 1, false);
            }
            return true;
        }

        /// <summary>
        /// Shrinks the tree to maxTreeCrossoverHeight
        /// </summary>
        /// <param name="currHeight"></param>
        /// <returns></returns>
        public override bool Shrink(int currHeight)
        {
            if ((currHeight + 1) >= (Settings.MaxTreeCrossoverHeight))
            {
                _left = new Operand();
                _right = new Operand();
                return true;
            }
            else
            {
                return _left.Shrink(currHeight + 1) || _right.Shrink(currHeight + 1);
            }

        }

    }
}
