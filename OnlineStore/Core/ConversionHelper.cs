using Microsoft.VisualBasic;
using System;
using System.ComponentModel;
using System.Globalization;

namespace GlideBuy.Core
{
	public static class ConversionHelper
	{
		public static T? ConvertTo<T>(object value)
		{
			return (T?)ConvertTo(value, typeof(T));
		}

		public static object? ConvertTo(object value, Type destinationType)
		{
			return ConvertTo(value, destinationType, CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// A universal type conversion helper, and more flexible than Convert.ChangeType()
		/// because it supports custom type converters, enum conversions, and culture-aware conversions.
		/// 
		/// The main use case: Convert strings (Settings stored in the database) back to their
		/// original types, while taking culture info into account.
		/// Convert back strings to bool, int, and/or enums.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="destinationType"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public static object? ConvertTo(object value, Type destinationType, CultureInfo culture)
		{
			// TODO: if you pass null and destinationType is a non-nullable value type (like int),
			// you’ll get null instead of 0 or an exception. Depending on the caller, that could be a silent bug.
			if (value == null)
			{
				return null;
			}

			var sourceType = value.GetType();

			/**
			 * All type converters subclass TypeConverter in System.ComponentModel.
			 * To obtain a TypeConverter, call TypeDescriptor.GetConverter.
			 * 
			 * If a type has no [TypeConverter] attribute, we’ll still get some converter
			 * object (like an internal TypeConverter base stub), but it might not be able
			 * to convert from or to anything meaningful, and CanConvertFrom() or CanConvertTo()
			 * will return false for most types.
			 */

			// Try the destination type's converter.
			var destinationConvertor = TypeDescriptor.GetConverter(destinationType);
			if (destinationConvertor.CanConvertFrom(sourceType))
			{
				return destinationConvertor.ConvertFrom(null, culture, value);
			}

			// Try the source type's converter.
			var sourceConverter = TypeDescriptor.GetConverter(sourceType);
			if (sourceConverter.CanConvertTo(destinationType))
			{
				return sourceConverter.ConvertTo(null, culture, value, destinationType);
			}

			// This is a safety net for enum conversions, because Convert.ChangeType()
			// doesn’t handle enums well.
			if (destinationType.IsEnum && value is int)
			{
				return Enum.ToObject(destinationType, (int)value);
			}

			// This covers most primitives (int, double, decimal, string, etc.) with
			// culture-aware conversion.
			// Sometimes the value might already be of the desired type.
			if (!destinationType.IsInstanceOfType(value))
			{
				return Convert.ChangeType(value, destinationType, culture);
			}

			return value;
		}
	}
}
