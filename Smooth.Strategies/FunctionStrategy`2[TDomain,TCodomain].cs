using System;
using Smooth.Operands;

namespace Smooth.Strategies
{
    public class FunctionStrategy<TDomain, TCodomain> : OperationStrategy<Func<TDomain, TCodomain>, FunctionContext<TDomain, TCodomain>, TDomain, TCodomain, Predicate<TDomain>, Predicate<TCodomain>>
        where TDomain : ISource
    {
        public FunctionStrategy(Func<TDomain, TCodomain> operation, Predicate<TDomain> preCondition = null, Predicate<TCodomain> postCondition = null, Predicate<FunctionContext<TDomain, TCodomain>> invariant = null)
            : base(FunctionalFormulations.Rewriter, operation, preCondition, postCondition, invariant)
        {
        }

        public FunctionStrategy(Func<Func<TDomain, TCodomain>> loadOperation, Predicate<TDomain> preCondition = null, Predicate<TCodomain> postCondition = null, Predicate<FunctionContext<TDomain, TCodomain>> invariant = null)
            : base(FunctionalFormulations.Rewriter, loadOperation, preCondition, postCondition, invariant)
        {
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
                if (operation == null) return context => { };
                return context => context.Codomain = operation(context.Domain);
            }

            public override Predicate<FunctionContext<TDomain, TCodomain>> RewritePreCondition(Predicate<TDomain> preCondition)
            {
                if (preCondition == null) return context => true;
                return context => preCondition(context.Domain);
            }

            public override Predicate<FunctionContext<TDomain, TCodomain>> RewritePostCondition(Predicate<TCodomain> postCondition)
            {
                if (postCondition == null) return context => true;
                return context => postCondition(context.Codomain);
            }
        }

        protected override FunctionContext<TDomain, TCodomain> Context(object[] input)
        { 
            return new FunctionContext<TDomain, TCodomain>((TDomain)input[0]);
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