using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnsorExternSimulation
{
    class ExternSimulationPackage
    {
        private const int NR_BYTE_DOUTPUT = 32;
        private const int NR_BYTE_DINPUT = 32;
        private const int NR_NUMOUTPUT = 64;
        private const int NR_NUMINPUT = 64;

        public Byte[] digOutputs;
        public Byte[] digInputs;
        public double[] numOutputs;
        public double[] numInputs;
        private int size;

        public ExternSimulationPackage()
        {
            digOutputs = new Byte[NR_BYTE_DOUTPUT];
            digInputs = new Byte[NR_BYTE_DINPUT];
            numOutputs = new double[NR_NUMOUTPUT];
            numInputs = new double[NR_NUMINPUT];
            size = NR_BYTE_DINPUT + NR_BYTE_DOUTPUT + NR_NUMINPUT * sizeof(double) + NR_NUMOUTPUT * sizeof(double);
        }

        public ExternSimulationPackage(Byte[] inputArray)
        {
            digOutputs = new Byte[NR_BYTE_DOUTPUT];
            digInputs = new Byte[NR_BYTE_DINPUT];
            numOutputs = new double[NR_NUMOUTPUT];
            numInputs = new double[NR_NUMINPUT];

            size = NR_BYTE_DINPUT + NR_BYTE_DOUTPUT + NR_NUMINPUT * sizeof(double) + NR_NUMOUTPUT * sizeof(double);
            // read outputs
            for (int i = 0; i < inputArray.Length; i++)
            {
                if(i<NR_BYTE_DOUTPUT){
                    digOutputs[i] = inputArray[i];
                }else if(i < (NR_BYTE_DOUTPUT + NR_BYTE_DINPUT)){
                    digInputs[i - NR_BYTE_DOUTPUT] = inputArray[i];
                }else if(i < (NR_BYTE_DOUTPUT +NR_BYTE_DINPUT + sizeof(double)*NR_NUMOUTPUT)){
                    numOutputs[(i - (NR_BYTE_DINPUT + NR_BYTE_DOUTPUT))/sizeof(double)] = BitConverter.ToDouble(inputArray, i);
                    i += 7;
                }else{
                    numInputs[(i - (NR_BYTE_DINPUT + NR_BYTE_DOUTPUT + sizeof(double) * NR_NUMOUTPUT))/(sizeof(double))] = BitConverter.ToDouble(inputArray, i);
                    i += 7;
                }           
            }
        }

        public Byte[] ToByteArray()
        {
            Byte[] returnArray = new Byte[size];
            // read outputs
            for (int i = 0; i < returnArray.Length; i++)
            {
                if (i < NR_BYTE_DOUTPUT)
                {
                    returnArray[i] = digOutputs[i];
                }
                else if (i < (NR_BYTE_DOUTPUT + NR_BYTE_DINPUT))
                {
                    returnArray[i] = digInputs[i-NR_BYTE_DOUTPUT];
                }
                else if (i < (NR_BYTE_DINPUT + NR_BYTE_DOUTPUT + sizeof(double) * NR_NUMOUTPUT))
                {
                    BitConverter.GetBytes(numOutputs[(i - (NR_BYTE_DOUTPUT + NR_BYTE_DINPUT)) / sizeof(double)]).CopyTo(returnArray,i);
                    i += 7;
                }
                else
                {
                    BitConverter.GetBytes(numInputs[(i - (NR_BYTE_DINPUT + NR_BYTE_DOUTPUT + sizeof(double) * NR_NUMOUTPUT)) / sizeof(double)]).CopyTo(returnArray, i);
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
