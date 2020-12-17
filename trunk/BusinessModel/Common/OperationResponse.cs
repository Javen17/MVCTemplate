namespace BusinessModel.Common
{
	/// <summary>
	/// Results of an operation
	/// </summary>
	public class OperationResponse
	{
		/// <summary>
		/// The result 
		/// </summary>
		public bool Result { get; set; }

		/// <summary>
		/// Any message that the service needs to pass back
		/// </summary>
		public string Message { get; set; }
	}
}