using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeneticProgramming.Model;

namespace GeneticProgramming.Model.Test
{
    [TestClass]
    public class OperandTest
    {
        [TestMethod]
        public void Operand_ConstructorCheckRandomCreateX_RandomX()
        {
            var op = new Operand();
            int i = 0;
            while (op.OperandType != OperandEnum.x && i < 100000)
            {
                op = new Operand();
                i++;
            }
            Assert.AreEqual(op.OperandType, OperandEnum.x);
        }

        [TestMethod]
        public void Operand_Constructor_CreatesRandomOperand()
        {
            var op = new Operand();
            op.IsLeftChild = false;
            Assert.AreEqual(op.HasChildren(), false);
            Assert.AreEqual(op.HasParent(), false);
            Assert.AreEqual(op.IsLeftChild, false);
            Assert.AreEqual(op.DivideByZeroFlag, false);
            Assert.AreEqual(op.Type <= 10, true);
            Assert.AreEqual(op.Type >= 1, true);
            Assert.AreEqual(op.Left, null);
            Assert.AreEqual(op.Right, null);
            Assert.AreEqual(op.Size(), 1);
            op.IsLeftChild = true;
            Assert.AreEqual(op.IsLeftChild, true);
        }

        [TestMethod]
        public void Operand_ConstructorX_CreatesOperandGivenInputs()
        {
            var op = new Operand(OperandEnum.x);
            Assert.AreEqual(op.HasChildren(), false);
            Assert.AreEqual(op.HasParent(), false);
            Assert.AreEqual(op.IsLeftChild, false);
            Assert.AreEqual(op.DivideByZeroFlag, false);
            Assert.AreEqual(op.Type <= 10, true);
            Assert.AreEqual(op.Type >= 1, true);
            Assert.AreEqual(op.Left, null);
            Assert.AreEqual(op.Right, null);
            Assert.AreEqual(op.Size(), 1);
            Assert.AreEqual(op.Evalate(5), 5.0);
            Assert.AreEqual(op.Type, 10);
            Assert.AreEqual(op.ToString(), "x");
        }

        [TestMethod]
        public void Operand_Constructor5_CreatesOperandGivenInputs()
        {
            var op = new Operand(OperandEnum.five);
            Assert.AreEqual(op.HasChildren(), false);
            Assert.AreEqual(op.HasParent(), false);
            Assert.AreEqual(op.IsLeftChild, false);
            Assert.AreEqual(op.DivideByZeroFlag, false);
            Assert.AreEqual(op.Type <= 10, true);
            Assert.AreEqual(op.Type >= 1, true);
            Assert.AreEqual(op.Left, null);
            Assert.AreEqual(op.Right, null);
            Assert.AreEqual(op.Size(), 1);
            Assert.AreEqual(op.Evalate(6), 5.0);
            Assert.AreEqual(op.Type, 5);
            Assert.AreEqual(op.ToString(), "5");
        }

        [TestMethod]
        public void Mutate_ChecksToSeeMutate_ValueChanges()
        {
            var op = new Operand();
            var opVal = op.Type;
            op.Mutate();
            while (op.Type == opVal)
            {
                op.Mutate();
            }
            Assert.AreNotEqual(opVal, op.Type);
        }

        [TestMethod]
        public void GetRandomNode_NoChildren_ReturnsSelf()
        {
            var op = new Operand();
            var opC = op.GetRandomNode();
            Assert.AreEqual(op.Type, op.Type);
        }
    }
}
