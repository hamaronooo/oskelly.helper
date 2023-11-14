namespace KutCode.Cve.Application.Interfaces.Translation;

public interface ITranslator
{
	/// <summary>
	/// Translates input string from one language to another
	/// </summary>
	/// <param name="input">Input string</param>
	/// <returns>Translated string</returns>
	Task<string> TranslateAsync(string input, string languageFromCode, string languageToCode, CancellationToken ct = default);
}