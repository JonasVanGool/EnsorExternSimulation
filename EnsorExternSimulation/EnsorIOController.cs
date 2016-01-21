using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace EnsorExternSimulation
{
    class EnsorIOController
    {
        private bool GUI_BusyWriting;
        private bool SOCKET_Update;
        public LinkedList<DigOutput> digOutputs;
        public LinkedList<DigInput> digInputs;
        public LinkedList<NumOutput> numOutputs;
        public LinkedList<NumInput> numInputs;

        public EnsorIOController(String configFilePath)
        {
            // reset busy writing from gui flag
            GUI_BusyWriting = false;
            SOCKET_Update = false;
            // Check if file exists
            if (!File.Exists(configFilePath))
                return;

            digOutputs = new LinkedList<DigOutput>();
            digInputs = new LinkedList<DigInput>();
            numOutputs = new LinkedList<NumOutput>();
            numInputs = new LinkedList<NumInput>();

            // Read config file
            string[] lines = System.IO.File.ReadAllLines(configFilePath);

            for (int i = 0; i < lines.Length; i++)
            {
                // Read config dig inputs
                if (lines[i].Contains("<Digital_Inputs_Configuration>"))
                {
                    i += 3;
                    while (!lines[i + 2].Contains("</Digital_Inputs_Configuration>"))
                    {
                        i++;
                        string[] tempArray = lines[i].Split(' ');
                        DigInput tempDigInput = new DigInput();
                        tempArray = tempArray.Where(val => val != "").ToArray();
                        tempDigInput.IdxNr = int.Parse(tempArray[0]);
                        tempDigInput.Symbol = tempArray[1];
                        tempDigInput.DevIdx = int.Parse(tempArray[2]);
                        tempDigInput.SlotNr = int.Parse(tempArray[3]);
                        tempDigInput.IONr = int.Parse(tempArray[4]);
                        tempDigInput.Logic = int.Parse(tempArray[5]);
                        tempDigInput.Options = tempArray.Length > 6 ? tempArray[6] : "";
                        digInputs.AddLast(tempDigInput);
                    }
                }

                // Read config dig outputs
                if (lines[i].Contains("<Digital_Outputs_Configuration>"))
                {
                    i += 3;
                    while (!lines[i + 2].Contains("</Digital_Outputs_Configuration>"))
                    {
                        i++;
                        string[] tempArray = lines[i].Split(' ');                
                        tempArray = tempArray.Where(val => val != "").ToArray();
                        DigOutput tempDigOutput = new DigOutput();
                        tempArray = tempArray.Where(val => val != "").ToArray();
                        tempDigOutput.IdxNr = int.Parse(tempArray[0]);
                        tempDigOutput.Symbol = tempArray[1];
                        tempDigOutput.DevIdx = int.Parse(tempArray[2]);
                        tempDigOutput.SlotNr = int.Parse(tempArray[3]);
                        tempDigOutput.IONr = int.Parse(tempArray[4]);
                        tempDigOutput.Logic = int.Parse(tempArray[5]);
                        tempDigOutput.Options = tempArray.Length > 6 ? tempArray[6] : "";
                        digOutputs.AddLast(tempDigOutput);
                    }
                }

                // Read config  num inputs
                if (lines[i].Contains("<Numeric_Inputs_Configuration>"))
                {
                    i += 3;
                    while (!lines[i + 2].Contains("</Numeric_Inputs_Configuration>"))
                    {
                        i++;
                        string[] tempArray = lines[i].Split(' ');
                        tempArray = tempArray.Where(val => val != "").ToArray();
                        NumInput tempNumInput = new NumInput();
                        tempNumInput.IdxNr = int.Parse(tempArray[0]);
                        tempNumInput.Symbol = tempArray[1];
                        tempNumInput.DevIdx = int.Parse(tempArray[2]);
                        tempNumInput.Port = int.Parse(tempArray[3]);
                        tempNumInput.IONr = int.Parse(tempArray[4]);
                        tempNumInput.MinDVal = double.Parse(tempArray[5]);
                        tempNumInput.MaxDVal = double.Parse(tempArray[6]);
                        tempNumInput.MinVal = double.Parse(tempArray[7]);
                        tempNumInput.MaxVal = double.Parse(tempArray[8]);
                        tempNumInput.UnitID = tempArray[9];
                        tempNumInput.Options = tempArray.Length>10? tempArray[10]:"";
                        numInputs.AddLast(tempNumInput);
                    }
                }

                // Read config num outputs
                if (lines[i].Contains("<Numeric_Outputs_Configuration>"))
                {
                    i += 3;
                    while (!lines[i + 2].Contains("</Numeric_Outputs_Configuration>"))
                    {
                        i++;
                        string[] tempArray = lines[i].Split(' ');
                        tempArray = tempArray.Where(val => val != "").ToArray();
                        NumOutput tempNumOutput = new NumOutput();
                        tempNumOutput.IdxNr = int.Parse(tempArray[0]);
                        tempNumOutput.Symbol = tempArray[1];
                        tempNumOutput.DevIdx = int.Parse(tempArray[2]);
                        tempNumOutput.Port = int.Parse(tempArray[3]);
                        tempNumOutput.IONr = int.Parse(tempArray[4]);
                        tempNumOutput.MinDVal = double.Parse(tempArray[5]);
                        tempNumOutput.MaxDVal = double.Parse(tempArray[6]);
                        tempNumOutput.MinVal = double.Parse(tempArray[7]);
                        tempNumOutput.MaxVal = double.Parse(tempArray[8]);
                        tempNumOutput.DefVal = double.Parse(tempArray[9]);
                        tempNumOutput.UnitID = tempArray[10];
                        tempNumOutput.Options = tempArray.Length > 11 ? tempArray[11] : "";
                        numOutputs.AddLast(tempNumOutput);
                    }
                }
            }
        }

        public void SetDigOutputByGUI(String Symbol, bool value){
            GUI_BusyWriting = true;
            foreach(DigOutput digOutput in digOutputs)
                if (digOutput.Symbol.Equals(Symbol))
                {
                    digOutput.CurrentVal = value;
                    return;
                }
        }

        public void SetDigOutputBySocket(int index , bool value){
            if (GUI_BusyWriting)
                return;
            foreach(DigOutput digOutput in digOutputs)
                if (digOutput.IdxNr == index)
                {
                    SOCKET_Update = true;
                    digOutput.CurrentVal = value;
                    return;
                }
                    
        }

        public void SetDigInputByGUI(String Symbol, bool value)
        {
            GUI_BusyWriting = true;
            foreach (DigInput digInput in digInputs)
                if (digInput.Symbol.Equals(Symbol))
                {
                    digInput.CurrentVal = value;
                    return;
                }
        }

        public void SetDigInputBySocket(int index, bool value)
        {
            if (GUI_BusyWriting)
                return;
            foreach (DigInput digInput in digInputs)
                if (digInput.IdxNr == index)
                {
                    SOCKET_Update = true;
                    digInput.CurrentVal = value;
                    return;
                }
                    
        }

        public void SetNumInputByGUI(String Symbol, double value)
        {
            GUI_BusyWriting = true;
            foreach (NumInput numInput in numInputs)
                if (numInput.Symbol.Equals(Symbol))
                {
                    numInput.CurrentVal = value;
                    return;
                }
        }

        public void SetNumInputBySocket(int index, double value)
        {
            if (GUI_BusyWriting)
                return;
            foreach (NumInput numInput in numInputs)
                if (numInput.IdxNr == index)
                {
                    SOCKET_Update = true;
                    numInput.CurrentVal = value;
                    return;
                }              
        }

        public void SetNumOuputByGUI(String Symbol, double value)
        {
            GUI_BusyWriting = true;
            foreach (NumOutput numOutput in numOutputs)
                if (numOutput.Symbol.Equals(Symbol))
                {
                    numOutput.CurrentVal = value;
                    return;
                }
                    
        }

        public void SetNumOutputBySocket(int index, double value)
        {
            if (GUI_BusyWriting)
                return;
            foreach (NumOutput numOutput in numOutputs)
                if (numOutput.IdxNr == index)
                {
                    SOCKET_Update = true;
                    numOutput.CurrentVal = value;
                    return;
                }
                    
        }

        public bool GetSocketUpdate()
        {
            return SOCKET_Update;
        }
        public void ResetSocketUpdate()
        {
            SOCKET_Update = false;
        }
        public void ResetGUIBusyWriting(){
            GUI_BusyWriting = false;
        }
    }

    class DigOutput
    {
        public bool CurrentVal;
        public int IdxNr;
        public String Symbol;
        public int DevIdx;
        public int SlotNr;
        public int IONr;
        public int Logic;
        public String Options;
    }

    class DigInput
    {
        public bool CurrentVal;
        public int IdxNr;
        public String Symbol;
        public int DevIdx;
        public int SlotNr;
        public int IONr;
        public int Logic;
        public String Options;
    }

    class NumInput
    {
        public double CurrentVal;
        public int IdxNr;
        public String Symbol;
        public int DevIdx;
        public int Port;
        public int IONr;
        public double MinDVal;
        public double MaxDVal;
        public double MinVal;
        public double MaxVal;
        public String UnitID;
        public String Options;
    }

    class NumOutput
    {
        public double CurrentVal;
        public int IdxNr;
        public String Symbol;
        public int DevIdx;
        public int Port;
        public int IONr;
        public double MinDVal;
        public double MaxDVal;
        public double MinVal;
        public double MaxVal;
        public double DefVal;
        public String UnitID;
        public String Options;
    }
}
