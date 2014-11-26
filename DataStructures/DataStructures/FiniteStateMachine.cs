using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures {
    public class FiniteStateMachineParameter<S, I, O> {
        //public List<S> States { get; set; }
        //public List<I> InputSymbols { get; set; }
        //public List<O> OutputSymbols { get; set; }
        public S InitialState { get; set; }
        public Func<S, I, S> NextStateFunction { get; set; }
        public Func<S, O> OutputFunction { get; set; }
    }
    /// <summary>
    /// M = [S, I, O, fs, fo]
    /// S: Set of finite states
    /// I: Set of input symbols
    /// O: Set of output symbols
    /// fs: next-state function, fs(state, input) -> output state
    /// fo: output function, fo(state) -> output
    /// </summary>
    public class FiniteStateMachine <S,I,O> {
        // What is a good data structure for S?
        // What is a good interface for iterating through states?
        //private List<S> states;
        //private List<I> inputSymbols;
        //private List<O> outputSymbols;
        private Func<S, I, S> nextStateFunction;
        private Func<S, O> outputFunction;

        private S currentState;

        public FiniteStateMachine(FiniteStateMachineParameter<S,I,O> p) {
            //this.states = p.States;
            //this.inputSymbols = p.InputSymbols;
            //this.outputSymbols = p.OutputSymbols;
            this.nextStateFunction = p.NextStateFunction;
            this.outputFunction = p.OutputFunction;
            this.currentState = p.InitialState;
        }

        public void ApplyInput(I inputSymbol) {
            currentState = nextStateFunction(currentState, inputSymbol);
        }

        public S State {
            get { return currentState; }
        }

        public O Output {
            get { return outputFunction(currentState) ; }
        }
    }
}
