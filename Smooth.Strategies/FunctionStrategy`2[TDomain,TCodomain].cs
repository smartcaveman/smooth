using System;
using System.Diagnostics.Contracts;
using Smooth.Operands;
using Smooth.OperationModel;
using Smooth.ProcessModel;

namespace Smooth.Strategies
{
    public  class FunctionStrategy<TDomain, TCodomain> : OperationStrategy<Func<TDomain, TCodomain>, FunctionContext<TDomain, TCodomain>, TDomain, TCodomain, Predicate<TDomain>, Predicate<TCodomain>>
        where TDomain : ISource
    {
        private readonly Func<object[], TDomain> getInput;

        public FunctionStrategy(IOperator<TDomain, TCodomain> @operator, Func<TDomain, TCodomain> operation, Func<object[], TDomain> input, Predicate<TDomain> preCondition = null, Predicate<TCodomain> postCondition = null, Predicate<FunctionContext<TDomain, TCodomain>> invariant = null)
            : base(@operator, FunctionalFormulations.Rewriter, operation, preCondition, postCondition, invariant)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            this.getInput = input;
        }

        public FunctionStrategy(IOperator<TDomain, TCodomain> @operator, Func<Func<TDomain, TCodomain>> loadOperation, Func<object[], TDomain> input, Predicate<TDomain> preCondition = null, Predicate<TCodomain> postCondition = null, Predicate<FunctionContext<TDomain, TCodomain>> invariant = null)
            : base(@operator, FunctionalFormulations.Rewriter, loadOperation, preCondition, postCondition, invariant)
        {
            Contract.Requires<ArgumentNullException>(!ReferenceEquals(input, null));
            this.getInput = input;
        }

        protected override TDomain Input(object[] input)
        {
            return this.getInput(input);
        }
        public sealed override IOperationResult<TCodomain> Apply(TDomain input)
        {
            FunctionContext<TDomain, TCodomain> context = Context(input);
            ProcessResult result = Action.Execute(context);
            return new Operation<TDomain, TCodomain>(this, input).CreateResult(result, Output(context));
        }

        protected class FunctionalFormulations : DelegateRewriter
        {
            public static readonly DelegateRewriter Rewriter;

            static FunctionalFormulations()
            {
                Rewriter = new FunctionalFormulations();
            }

            public override Action<FunctionContext<TDomain, TCodomain>> RewriteAction(Func<TDomain, TCodomain> operation)
            {
                return operation == null
                           ? (Action<FunctionContext<TDomain, TCodomain>>) (context => { })
                           : (context => context.Codomain = operation(context.Domain));
            }

            public override Predicate<FunctionContext<TDomain, TCodomain>> RewritePreCondition(Predicate<TDomain> preCondition)
            {
                return preCondition == null
                           ? (Predicate<FunctionContext<TDomain, TCodomain>>) (context => true)
                           : (context => preCondition(context.Domain));
            }

            public override Predicate<FunctionContext<TDomain, TCodomain>> RewritePostCondition(Predicate<TCodomain> postCondition)
            {
                return postCondition == null
                           ? (Predicate<FunctionContext<TDomain, TCodomain>>) (context => true)
                           : (context => postCondition(context.Codomain));
            }
        }

        protected override FunctionContext<TDomain, TCodomain> Context(TDomain input)
        {
            return new FunctionContext<TDomain, TCodomain>(input);
        }

        protected override TCodomain Output(FunctionContext<TDomain, TCodomain> context)
        {
            return context.Codomain;
        }

        protected override Func<TDomain, TCodomain> Operation(Func<TDomain, TCodomain> func)
        {
            return func;
        }
    }
}