using System.Collections.Generic;

namespace BusinessModel.Validation
{
	/// <summary>
	/// Base class used for validation
	/// </summary>

	public class BaseValidationResult
	{
		/// <summary>
		/// General message
		/// </summary>
		public string Message { get; set; }

		/// <summary>
		/// Errors with the request
		/// </summary>
		public List<RequestParamValidationError> ValidationErrors { get; set; }
	}
}