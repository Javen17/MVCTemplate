namespace BusinessModel.Validation
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class GenericValidationResult<TEntity> : BaseValidationResult
	{

		/// <summary>
		/// The entity that was sent in the request
		/// </summary>
		public TEntity Request { get; set; }
	}
}