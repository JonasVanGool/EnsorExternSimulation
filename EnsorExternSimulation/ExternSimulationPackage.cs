using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsorExternSimulation
{
    class ExternSimulationPackage
    {
        public Byte[] digOutputs;
        public Byte[] digInputs;
        public double[] numOutputs;
        public double[] numInputs;
        private int size;

        public ExternSimulationPackage()
        {
            digOutputs = new Byte[32];
            digInputs = new Byte[32];
            numOutputs = new double[32];
            numInputs = new double[32];
            size = 576;
        }

        public ExternSimulationPackage(Byte[] inputArray)
        {
            digOutputs = new Byte[32];
            digInputs = new Byte[32];
            numOutputs = new double[32];
            numInputs = new double[32];

            // read outputs
            for (int i = 0; i < inputArray.Length; i++)
            {
                if(i<32){
                    digOutputs[i] = inputArray[i];
                }else if(i < (32 +32)){
                    digInputs[i - 32] = inputArray[i];
                }else if(i < (32 +32 + 8*32)){
                    numOutputs[(i - 64)/8] = BitConverter.ToDouble(inputArray, i);
                    i += 7;
                }else{
                    numInputs[(i - (32 + 32 + 8 * 32))/8] = BitConverter.ToDouble(inputArray, i);
                    i += 7;
                }           
            }
        }

        public Byte[] ToByteArray()
        {
            Byte[] returnArray = new Byte[576];
            // read outputs
            for (int i = 0; i < returnArray.Length; i++)
            {
                if (i < 32)
                {
                    returnArray[i] = digOutputs[i];
                }
                else if (i < (32 + 32))
                {
                    returnArray[i] = digInputs[i-32];
                }
                else if (i < (32 + 32 + 8 * 32))
                {
                    BitConverter.GetBytes(numOutputs[(i - 64) / 8]).CopyTo(returnArray,i);
                    i += 7;
                }
                else
                {
                    BitConverter.GetBytes(numInputs[(i - (32 + 32 + 8 * 32)) / 8]).CopyTo(returnArray, i);
                    i += 7;
                }
            }

            return returnArray;
        }

        public int GetSize()
        {
            return size;
        }

    }
}
