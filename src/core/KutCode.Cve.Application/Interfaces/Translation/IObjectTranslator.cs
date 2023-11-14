namespace KutCode.Cve.Application.Interfaces.Translation;

/// <summary>
///     Translates input object fields marked with <see cref="TranslatableAttribute"/> from one language to another
/// </summary>
public interface IObjectTranslator
{
	/// <summary>
	///  Translates input object fields marked with <see cref="TranslatableAttribute"/> from one language to another
	/// </summary>
	Task<T> TranslateAsync<T>(T model, string languageFromCode, string languageToCode, CancellationToken ct = default);
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public sealed class TranslatableAttribute : Attribute
{
	public TranslatableAttribute(bool enabled = true)
	{
		Enabled = enabled;
	}
	public bool Enabled { get; }
}