using System.Reflection;

namespace KutCode.Cve.Domain.Enums;

public static class EnumHelper
{
	/// <summary>
	/// Get value of <see cref="System.ComponentModel.DescriptionAttribute"/>
	/// </summary>
	/// <returns>Description string or <see cref="string.Empty"/> if enum value is null or description is not presented</returns>
	public static string GetDescriptionValue<TEnum>(TEnum? enumValue) where TEnum : Enum
	{
		if (enumValue is null) return string.Empty;
		FieldInfo fi = enumValue.GetType().GetField(enumValue.ToString())!;
		var descAttrs = fi.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false)
			as System.ComponentModel.DescriptionAttribute[];
		if (descAttrs is null || descAttrs.Length == 0) return string.Empty;
		foreach (var attr in descAttrs)
			if (!string.IsNullOrEmpty(attr.Description))
				return attr.Description;
		return string.Empty;
	}
}