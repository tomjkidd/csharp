using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStructures {
    public class FiniteStateMachineParameter<S, I, O> {
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
        private Func<S, I, S> nextStateFunction;
        private Func<S, O> outputFunction;

        private S currentState;

        public FiniteStateMachine(FiniteStateMachineParameter<S,I,O> p) {
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
