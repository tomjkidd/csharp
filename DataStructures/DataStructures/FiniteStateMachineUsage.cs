using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures {
    public class FiniteStateMachineUsage {
        public static Dictionary<string, InputSymbol> GetStringToInputSymbolDict() {
            var dict = new Dictionary<string, InputSymbol>();
            dict.Add("00", InputSymbol.ZeroZero);
            dict.Add("01", InputSymbol.ZeroOne);
            dict.Add("10", InputSymbol.OneZero);
            dict.Add("11", InputSymbol.OneOne);
            return dict;
        }

        public static Dictionary<OutputSymbol, string> GetOutputSymbolToStringDict() {
            var dict = new Dictionary<OutputSymbol, string>();
            dict.Add(OutputSymbol.Zero, "0");
            dict.Add(OutputSymbol.One, "1");
            return dict;
        }

        public static FiniteStateMachine<AdderState, InputSymbol, OutputSymbol> CreateBinaryAdderStateMachine() {
            var parameter = new FiniteStateMachineParameter<AdderState, InputSymbol, OutputSymbol>() {
                InitialState = AdderState.ZeroNoCarry,
                NextStateFunction = (state, input) => {
                    var output = AdderState.ZeroNoCarry;
                    if (state == AdderState.ZeroNoCarry) {
                        if (input.Equals(InputSymbol.ZeroZero)) {
                            output = AdderState.ZeroNoCarry;
                        } else if (input.Equals(InputSymbol.ZeroOne) || input.Equals(InputSymbol.OneZero)) {
                            output = AdderState.OneNoCarry;
                        } else if (input.Equals(InputSymbol.OneOne)) {
                            output = AdderState.ZeroWithCarry;
                        }
                    } else if (state == AdderState.ZeroWithCarry) {
                        if (input.Equals(InputSymbol.ZeroZero)) {
                            output = AdderState.OneNoCarry;
                        } else if (input.Equals(InputSymbol.ZeroOne) || input.Equals(InputSymbol.OneZero)) {
                            output = AdderState.ZeroWithCarry;
                        } else if (input.Equals(InputSymbol.OneOne)) {
                            output = AdderState.OneWithCarry;
                        }
                    } else if (state == AdderState.OneNoCarry) {
                        if (input.Equals(InputSymbol.ZeroZero)) {
                            output = AdderState.ZeroNoCarry;
                        } else if (input.Equals(InputSymbol.ZeroOne) || input.Equals(InputSymbol.OneZero)) {
                            output = AdderState.OneNoCarry;
                        } else if (input.Equals(InputSymbol.OneOne)) {
                            output = AdderState.ZeroWithCarry;
                        }
                    } else if (state == AdderState.OneWithCarry) {
                        if (input.Equals(InputSymbol.ZeroZero)) {
                            output = AdderState.OneNoCarry;
                        } else if (input.Equals(InputSymbol.ZeroOne) || input.Equals(InputSymbol.OneZero)) {
                            output = AdderState.ZeroWithCarry;
                        } else if (input.Equals(InputSymbol.OneOne)) {
                            output = AdderState.OneWithCarry;
                        }
                    }
                    return output;
                },
                OutputFunction = state => {
                    var output = OutputSymbol.Zero;
                    if (state == AdderState.ZeroNoCarry) {
                        output = OutputSymbol.Zero;
                    } else if (state == AdderState.ZeroWithCarry) {
                        output = OutputSymbol.Zero;
                    } else if (state == AdderState.OneNoCarry) {
                        output = OutputSymbol.One;
                    } else if (state == AdderState.OneWithCarry) {
                        output = OutputSymbol.One;
                    }
                    return output;
                }
            };
            var stateMachine = new FiniteStateMachine<AdderState, InputSymbol, OutputSymbol>(parameter);
            return stateMachine;
        }

        public static void FiniteStateMachineUsageMain() {
            // Perform a couple of tests
            Console.WriteLine(Compute("011","100"));
            Console.WriteLine(Compute("101", "011"));
            Console.WriteLine(Compute("010", "001"));
            Console.ReadLine();

            // Do some binary counting
            var currentInput = "000";
            while (!currentInput.EndsWith("1000")) {
                Console.WriteLine(currentInput);
                var next = Compute(currentInput, "001");
                currentInput = next;
            }
            Console.WriteLine(currentInput);
            Console.ReadLine();
        }

        public static string Compute(string inputA, string inputB) {
            var output = new List<OutputSymbol>();
            var fsm = CreateBinaryAdderStateMachine();
            var inputDict = GetStringToInputSymbolDict();
            var outputDict = GetOutputSymbolToStringDict();

            var first = inputA.Reverse();
            var second = inputB.Reverse();
            first.Zip(second, (a, b) => a.ToString() + b.ToString()).ToList().ForEach(inputSymbol => {
                // ApplyInput to push the State Machine into it's next State
                fsm.ApplyInput(inputDict[inputSymbol]);
                // Access the Output produced by the Input
                output.Add(fsm.Output);
            });

            // Perform one more iteration to handle if there was a carry on the last operation
            fsm.ApplyInput(InputSymbol.ZeroZero);
            output.Add(fsm.Output);

            // Prepare the output string
            output.Reverse();
            var rtn = output.Select(o => outputDict[o].ToString());
            return string.Join("", rtn);
        }
    }

    public enum InputSymbol { 
        ZeroZero,
        ZeroOne,
        OneZero,
        OneOne
    }

    public enum OutputSymbol { 
        Zero,
        One
    }

    public enum AdderState {
        ZeroNoCarry,
        ZeroWithCarry,
        OneNoCarry,
        OneWithCarry
    }

    
}
