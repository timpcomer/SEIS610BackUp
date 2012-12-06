using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticProgramming.Model
{
    [Serializable]
    public class Operand : Node
    {
        /// <summary>
        /// The evaluation of a operand node
        /// </summary>
        public OperandEnum OperandType { get { return (OperandEnum)_type; } set { _type = (int)value; } }
        
        /// <summary>
        /// Constructor for random creation
        /// </summary>
        public Operand()
        {
            if (_rGen.NextDouble() < Settings.VariableProb)
                OperandType = OperandEnum.x;
            else
                OperandType = (OperandEnum)_rGen.Next(1,(int)OperandEnum.x);
            _divideByZeroFlag = false;            
        }

        /// <summary>
        /// Constructor for creating an operand Node as at a specific value
        /// </summary>
        /// <param name="operandType"></param>
        public Operand(OperandEnum operandType)
        {
            OperandType = operandType;
            _divideByZeroFlag = false;
        }

        /// <summary>
        /// operands have no children
        /// </summary>
        /// <returns></returns>
        public override bool HasChildren()
        {
            return false;
        }

        /// <summary>
        /// Returns the value passed if it is an x variable otherwise its own value
        /// </summary>
        /// <param name="valueX"></param>
        /// <returns></returns>
        public override double Evalate(int valueX)
        {
            if (OperandType == OperandEnum.x)
                return valueX;
            else
                return _type;
        }

        /// <summary>
        /// Tries to mutate and returns false if it mutates to the same thing
        /// </summary>
        /// <returns></returns>
        public override bool Mutate()
        {
            var oldOpType = OperandType;
            if (_rGen.NextDouble() < (Settings.VariableProb / 2))
                OperandType = OperandEnum.x;
            else
                OperandType = (OperandEnum)_rGen.Next((int)OperandEnum.x+1);
            return oldOpType != OperandType;
        }

        /// <summary>
        /// Returns size of subtree, operand is always 1
        /// </summary>
        /// <returns></returns>
        public override int Size()
        {
            return 1;
        }

        /// <summary>
        /// Randomly selects one of the nodes in the subtree as this being the root.
        /// Always return self, operand has no children
        /// </summary>
        /// <returns></returns>
        public override Node GetRandomNode()
        {
            return this;
        }

        /// <summary>
        /// Returns a random node of either type operator or type operand.
        /// </summary>
        /// <param name="isOperator">True to get an operator, false to get an operand</param>
        /// <returns></returns>
        public override Node GetRandomNode(bool isOperator)
        {
            if (isOperator)
                return this.Parent;
            else
                return this;
        }

        /// <summary>
        /// Returns subtree as a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return OperandType == OperandEnum.x ? "x" : _type.ToString();
        }

        public override Node GetCopy()
        {
            var op = new Operand(this.OperandType);
            return op;
        }

        public override bool Grow(int currHeight)
        {
            return false;
        }

        public override bool Shrink(int currHeight)
        {
            return false;
        }

    }
}
