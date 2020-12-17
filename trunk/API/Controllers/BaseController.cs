using BusinessModel.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class BaseController : Controller
    {
		/// <summary>
		/// used to validate a model and return default errors
		/// </summary>
		/// <param name="validationData"></param>
		protected void ProcessDefaultModelValidationErrors(BaseValidationResult validationData)
		{
			foreach (var key in ModelState.Keys)
			{
				validationData.ValidationErrors.Add(new RequestParamValidationError
				{
					FieldName = key,
					Message = string.Join("\n", ModelState[key].Errors.Select(o => o.ErrorMessage))
				});
			}
		}


		public long UserId { get; set; }
	}
}
