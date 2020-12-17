using System.Linq;

namespace Tests
{
	public static class UnitTestExtensions
	{
		public static int GetPropertyCount(this object obj, bool returnRaw = false)
		{
			int propertyCount = obj.GetType().GetProperties().Count();
			if (returnRaw)
				return propertyCount;
			
			return obj.GetType().GetProperties().Count();
		}
	}
}