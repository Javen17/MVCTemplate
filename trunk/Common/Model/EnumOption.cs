using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Common.Utilities;

namespace Common.Model
{
	/// <summary>
	/// This is a simple class that exposes a int and an string that is mapped for an enum
	/// </summary>
	public class EnumOption
	{
		/// <summary>
		/// 
		/// </summary>
		public object Value { get; set; }
		/// <summary>
		/// 
		/// </summary>
		public object FilterValue { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string Path { get; set; }

		/// <summary>
		/// Used to get enum options for a given enum type
		/// </summary>
		/// <param name="enumType">the enum type to use</param>
		/// <param name="useDisplayAttribute">if we should use a display attribute for each value name</param>
		/// <returns></returns>
		public static List<EnumOption> GetOptionsForEnum(Type enumType, bool useDisplayAttribute = false)
		{
			List<EnumOption> items = new List<EnumOption>();
			FieldInfo[] fields = enumType.GetFields();
			foreach (var field in fields.Where(o => !o.IsSpecialName))
			{
				string value = field.GetRawConstantValue().ToString();
				items.Add(new EnumOption
				{
					Name = useDisplayAttribute ? (field.GetCustomAttributes(typeof(DisplayAttribute), false)[0] as DisplayAttribute).GetName() : field.Name.PascalToHuman(),
					Value = value
				});
			}
			return items;
		}

		public string GetIntJson()
		{
			return $"{{\"Value\": {Value?.ToString() ?? "null"}, \"Name\": \"{Name}\"}}";
		}
	}
}
