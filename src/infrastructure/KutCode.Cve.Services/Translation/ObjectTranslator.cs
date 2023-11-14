using System.Collections;
using System.Reflection;
using KutCode.Cve.Application.Interfaces.Translation;

namespace KutCode.Cve.Services.Translation;

public sealed class ObjectTranslator : IObjectTranslator
{
	private readonly ITranslator _translator;

	public ObjectTranslator(ITranslator translator)
	{
		_translator = translator;
	}

	/// <summary>
	///     Translates the given model and all its properties and fields recursively from one language to another.
	/// </summary>
	/// <typeparam name="T">The type of the model.</typeparam>
	/// <param name="model">The model to translate.</param>
	/// <param name="languageFromCode">The code of the language to translate from.</param>
	/// <param name="languageToCode">The code of the language to translate to.</param>
	/// <param name="ct">The cancellation token.</param>
	/// <returns>The translated model.</returns>
	public async Task<T> TranslateAsync<T>(
		T model,
		string languageFromCode,
		string languageToCode,
		CancellationToken ct = default)
	{
		// Translate the members of the model
		await TranslateMembersAsync(model, languageFromCode, languageToCode, ct);
		// Translate the properties
		var props = model!.GetType().GetProperties()
			.Where(x => !x.PropertyType.IsArray && x.PropertyType.GetInterface(nameof(IEnumerable)) == null)
			.Where(x => x.PropertyType.IsClass || x.PropertyType.IsInterface)
			.Where(x => x.PropertyType != typeof(string) && !x.PropertyType.IsPrimitive);
		foreach (var prop in props) {
			var val = prop.GetValue(model);
			if (val is null) continue;
			await TranslateAsync(val, languageFromCode, languageToCode, ct);
		}

		// Translate the fields
		var fields = model!.GetType().GetFields()
			.Where(x => !x.FieldType.IsArray && x.FieldType.GetInterface(nameof(IEnumerable)) == null)
			.Where(x => x.FieldType.IsClass || x.FieldType.IsInterface)
			.Where(x => x.FieldType != typeof(string) && !x.FieldType.IsPrimitive);
		foreach (var field in fields) {
			var val = field.GetValue(model);
			if (val is null) continue;
			await TranslateAsync(val, languageFromCode, languageToCode, ct);
		}

		return model;
	}

	private async Task TranslateMembersAsync<T>(T model, string languageFromCode, string languageToCode,
		CancellationToken ct)
	{
		foreach (var property in GetPropertiesForTranslation(model!.GetType())) {
			if (ct.IsCancellationRequested) break;
			var propValue = property.GetValue(model)?.ToString();
			if (string.IsNullOrEmpty(propValue)) continue;
			var translatedValue = await _translator.TranslateAsync(propValue, languageFromCode, languageToCode, ct);
			property.SetValue(model, translatedValue);
		}

		foreach (var property in GetFieldsForTranslation(model!.GetType())) {
			if (ct.IsCancellationRequested) break;
			var propValue = property.GetValue(model)?.ToString();
			if (string.IsNullOrEmpty(propValue)) continue;
			var translatedValue = await _translator.TranslateAsync(propValue, languageFromCode, languageToCode, ct);
			property.SetValue(model, translatedValue);
		}
	}

	private static IEnumerable<PropertyInfo> GetPropertiesForTranslation(Type objectType)
	{
		var stringType = typeof(string);
		foreach (var propertyInfo in objectType.GetProperties().Where(x => x.PropertyType == stringType)) {
			var attributeRaw = propertyInfo.GetCustomAttribute(typeof(TranslatableAttribute));
			if (attributeRaw is null || !(attributeRaw is TranslatableAttribute attribute) ||
			    !attribute.Enabled) continue;
			yield return propertyInfo;
		}
	}

	private static IEnumerable<FieldInfo> GetFieldsForTranslation(Type objectType)
	{
		var stringType = typeof(string);
		foreach (var propertyInfo in objectType.GetFields().Where(x => x.FieldType == stringType)) {
			var attributeRaw = propertyInfo.GetCustomAttribute(typeof(TranslatableAttribute));
			if (attributeRaw is null || !(attributeRaw is TranslatableAttribute attribute) ||
			    !attribute.Enabled) continue;
			yield return propertyInfo;
		}
	}
}