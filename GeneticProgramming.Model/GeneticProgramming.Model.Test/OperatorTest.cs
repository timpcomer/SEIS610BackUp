using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticProgramming.Model;

namespace GeneticProgramming.Model.Test
{
    [TestClass]
    public class OperatorTest
    {
        [TestMethod]
        public void Operator_Constructor_CreatesRandomTree()
        {
            var op = new Operator();
            Assert.AreEqual(true, op.HasChildren(), "children");
            Assert.AreEqual(false, op.HasParent(), "parent");
            Assert.AreEqual(true, op.Type <= 3, "type <");
            Assert.AreEqual(true, op.Type >= 0, "type >");
            Assert.AreEqual(true, op.Size() >= Settings.MinTreeHeight, "min height");
            Assert.AreEqual(true, op.Size() <= Settings.MaxTreeHeight, "max height");
        }

        [TestMethod]
        public void Operator_ConstructorInputsPlus_CreatesOperatorTreeBasedUponInputs()
        {
            var left = new Operand(OperandEnum.x);
            var Right = new Operand(OperandEnum.three);
            var op = new Operator(OperatorEnum.add, left, Right);
            Assert.AreEqual(left.Type, op.Left.Type, "left type");
            Assert.AreEqual(Right.Type, op.Right.Type, "right type");
            Assert.AreEqual("(x+3)", op.ToString(), "to string");
            Assert.AreEqual(8, op.Evalate(5), "evaluate");
            Assert.AreEqual(false, op.DivideByZeroFlag, "divide by zero flag");
            Assert.AreEqual(2, op.Size(), "height");
            Assert.AreEqual(true, op.GetRandomNode().Type == op.Type 
                || op.GetRandomNode().Type == left.Type 
                || op.GetRandomNode().Type == Right.Type, "random node");
        }

        [TestMethod]
        public void Operator_ConstructorInputsMinus_CreatesOperatorTreeBasedUponInputs()
        {
            var left = new Operand(OperandEnum.x);
            var Right = new Operand(OperandEnum.three);
            var op = new Operator(OperatorEnum.subtract, left, Right);
            Assert.AreEqual(left.Type, op.Left.Type, "left type");
            Assert.AreEqual(Right.Type, op.Right.Type, "rigth type");
            Assert.AreEqual("(x-3)", op.ToString(), "to string");
            Assert.AreEqual(2, op.Evalate(5), "evaluate");
            Assert.AreEqual(false, op.DivideByZeroFlag, "divide by zero flage");
            Assert.AreEqual(2, op.Size(), "height");
            Assert.AreEqual(true, op.GetRandomNode().Type == op.Type
                || op.GetRandomNode().Type == left.Type
                || op.GetRandomNode().Type == Right.Type, "random node");
        }

        [TestMethod]
        public void Operator_ConstructorInputsMultiply_CreatesOperatorTreeBasedUponInputs()
        {
            var left = new Operand(OperandEnum.x);
            var right = new Operand(OperandEnum.three);
            var op = new Operator(OperatorEnum.multiply, left, right);
            Assert.AreEqual(left.Type, op.Left.Type);
            Assert.AreEqual(right.Type, op.Right.Type);
            Assert.AreEqual("(x*3)", op.ToString());
            Assert.AreEqual(15, op.Evalate(5));
            Assert.AreEqual(false, op.DivideByZeroFlag);
            Assert.AreEqual(2, op.Size());
            Assert.AreEqual(true, op.GetRandomNode().Type == op.Type
                || op.GetRandomNode().Type == left.Type
                || op.GetRandomNode().Type == right.Type);
        }

        [TestMethod]
        public void Operator_ConstructorInputsDivide_CreatesOperatorTreeBasedUponInputs()
        {
            var left = new Operand(OperandEnum.x);
            var Right = new Operand(OperandEnum.three);
            var op = new Operator(OperatorEnum.divide, left, Right);
            Assert.AreEqual(left.Type, op.Left.Type);
            Assert.AreEqual(Right.Type, op.Right.Type);
            Assert.AreEqual("(x/3)", op.ToString());
            Assert.AreEqual(2, op.Evalate(6));
            Assert.AreEqual(false, op.DivideByZeroFlag);
            Assert.AreEqual(2, op.Size());
            Assert.AreEqual(true, op.GetRandomNode().Type == op.Type
                || op.GetRandomNode().Type == left.Type
                || op.GetRandomNode().Type == Right.Type);
        }

        [TestMethod]
        public void Operator_ConstructorInputsDivideWithZero_CreatesOperatorTreeBasedUponInputs()
        {
            var left = new Operand(OperandEnum.x);
            var right = new Operand(OperandEnum.zero);
            var op = new Operator(OperatorEnum.divide, left, right);
            Assert.AreEqual(left.Type, op.Left.Type, "type of left");
            Assert.AreEqual(right.Type, op.Right.Type, "type of right");
            Assert.AreEqual("(x/0)", op.ToString(), "string");
            Assert.AreEqual(double.MaxValue, op.Evalate(6), "evaluate");
            Assert.AreEqual(true, op.DivideByZeroFlag, "divide by zero flage");
            Assert.AreEqual(2, op.Size(), "height");
            //Assert.AreEqual(4, op.GetRandomNode().Type);
            Assert.AreEqual(true, (op.GetRandomNode().Type == op.Type
                || op.GetRandomNode().Type == left.Type
                || op.GetRandomNode().Type == right.Type), "random node");
        }
    }
}
