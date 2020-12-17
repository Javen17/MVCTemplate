using Common.Security;

namespace BusinessLogicLayer.Security
{
	public interface ICommonPrincipleAccessor
	{
		CommonPrincipal CurrentPrincipal { get; }
	}
}