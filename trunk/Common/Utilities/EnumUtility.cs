using Common.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Common.Model;

namespace Common.Utilities
{
	/// <summary>
	/// Contains a set of utility operations for helping with enums 
	/// </summary>
	public static class EnumUtility
	{
		/// <summary>
		/// used to return a culture based name for the enum
		/// </summary>
		/// <param name="enumInstace">the instance to get the localized name from</param>
		/// <returns></returns>
		public static string GetLocalizedName(Enum enumInstace)
		{
			if (enumInstace == null)
				return "";
			var field = enumInstace.GetType().GetField(enumInstace.ToString());
			if (field == null)
				return "";
			var displayAttributes = field.GetCustomAttributes(typeof(DisplayAttribute), false);
			if (displayAttributes.Length == 1)
				return (displayAttributes[0] as DisplayAttribute).GetName();
			return field.Name;
		}

		/// <summary>
		/// Used to take an enum and parse out all the attributes and get list of values
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="useDisplayAttribute"></param>
		/// <returns></returns>
		public static List<EnumOption> GetLocalizedEnumeratedValues(Type enumType, bool useDisplayAttribute)
		{
			FieldInfo[] fields = enumType.GetFields();
			List<EnumOption> options = new List<EnumOption>();
			foreach (var field in fields.Where(o => !o.IsSpecialName))
			{
				string value = field.GetRawConstantValue().ToString();
				string name = field.Name.PascalToHuman();
				if (useDisplayAttribute)
				{
					var displayNames = field.GetCustomAttributes(typeof(DisplayAttribute), false);
					if (displayNames.Length > 0)
						name = (displayNames[0] as DisplayAttribute).GetName();
				}
				options.Add(new EnumOption
				{
					Name = name,
					Value = value
				});
			}

			if (options.Count > 6)
				options = options.OrderBy(o => o.Name).ToList();
			return options;
		}

		/// <summary>
		/// Used to directly check for a given flag 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool Has<T>(this Enum type, T value)
		{
			try
			{
				return (((int)(object)type & (int)(object)value) == (int)(object)value);
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Used to check if a value is the same across different flags
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static bool Is<T>(this Enum type, T value)
		{
			try
			{
				return (int)(object)type == (int)(object)value;
			}
			catch
			{
				return false;
			}
		}

		/// <summary>
		/// Used to add a flag to an enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Add<T>(this Enum type, T value)
		{
			try
			{
				return (T)(object)(((int)(object)type | (int)(object)value));
			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("Could not append value from enumerated type '{0}'.", typeof(T).Name), ex);
			}
		}

		/// <summary>
		/// Used to remove a flag from and enum
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="type"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Remove<T>(this Enum type, T value)
		{

			try
			{
				TypeCode code = Type.GetTypeCode(type.GetType());
				switch (code)
				{
					case TypeCode.Byte:
						return (T)Enum.ToObject(type.GetType(), (((byte)(object)type & ~(byte)(object)value)));
					case TypeCode.Int16:
						return (T)Enum.ToObject(type.GetType(), (((short)(object)type & ~(short)(object)value)));
					case TypeCode.Int64:
						return (T)Enum.ToObject(type.GetType(), (((long)(object)type & ~(long)(object)value)));
					default:
						return (T)Enum.ToObject(type.GetType(), (((int)(object)type & ~(int)(object)value)));
				}

			}
			catch (Exception ex)
			{
				throw new ArgumentException(string.Format("Could not remove value from enumerated type '{0}'.", typeof(T).Name), ex);
			}
		}
	}
}