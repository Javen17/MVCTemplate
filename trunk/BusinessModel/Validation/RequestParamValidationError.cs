namespace BusinessModel.Validation
{
	/// <summary>
	/// Container for details about an error in validation
	/// </summary>
	public class RequestParamValidationError
	{
		/// <summary>
		/// The name of the filed with the validation issue
		/// </summary>
		public string FieldName { get; set; }
		/// <summary>
		/// Details about the issue with this field .
		/// </summary>
		public string Message { get; set; }
	}
}
