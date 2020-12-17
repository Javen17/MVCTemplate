using System;
using System.Threading;
using System.Transactions;
using BusinessLogicLayer.Security;
using Common.Security;

namespace BusinessLogicLayer.Business.Framework
{
	public class BusinessLogicComponent
	{
		protected ICommonPrincipleAccessor PrincipleAccessor { get; }
		protected BusinessLogicComponent(ICommonPrincipleAccessor commonPrincipleAccessor)
		{
			PrincipleAccessor = commonPrincipleAccessor;
		}

		protected CommonPrincipal CurrentPrincipal => PrincipleAccessor.CurrentPrincipal;
		public static class TransactionScopeFactory
		{
			private static readonly TransactionOptions _transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted };

			/// <summary>
			/// Constructs a new <see cref="TransactionScope"/>.
			/// </summary>
			/// <returns></returns>
			public static TransactionScope ConstructScope()
			{
				return ConstructScope(TransactionScopeOption.Required);
			}

			/// <summary>
			/// Constructs the specified option.
			/// </summary>
			/// <param name="option">The option.</param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static TransactionScope ConstructScope(TransactionScopeOption option)
			{
				return new TransactionScope(option, _transactionOptions, TransactionScopeAsyncFlowOption.Suppress);
			}

			/// <summary>
			/// Constructs the specified option.
			/// </summary>
			/// <param name="option">The option.</param>
			/// <param name="timeout">The timeout.</param>
			/// <returns></returns>
			/// <remarks></remarks>
			public static TransactionScope ConstructScope(TransactionScopeOption option, TimeSpan timeout)
			{
				TransactionOptions options = new TransactionOptions();
				options.Timeout = timeout;
				options.IsolationLevel = IsolationLevel.ReadCommitted;

				return new TransactionScope(option, options, TransactionScopeAsyncFlowOption.Enabled);
			}

			/// <summary>
			/// Constructs the specified option.
			/// </summary>
			/// <param name="option">The option.</param>
			/// <param name="timeout">The timeout.</param>
			/// <param name="isolationLevel"></param>
			/// <returns></returns>
			public static TransactionScope ConstructScope(TransactionScopeOption option, TimeSpan timeout, IsolationLevel isolationLevel)
			{
				TransactionOptions options = new TransactionOptions();
				options.Timeout = timeout;

				options.IsolationLevel = isolationLevel;

				return new TransactionScope(option, options, TransactionScopeAsyncFlowOption.Enabled);
			}
		}
	}
}
