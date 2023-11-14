using FakeItEasy;
using KutCode.Cve.Application.Interfaces.Translation;
using KutCode.Cve.Services.Translation;

namespace KutCode.Cve.Services.Tests.Translation;

[TestFixture]
public sealed class ObjectTranslatorTests
{
	[Test]
	public async Task Translate_TopLevel_Test()
	{
		ITranslator translator = A.Fake<ITranslator>();
		A.CallTo(() => translator
			.TranslateAsync(A<string>.Ignored, A<string>.Ignored,A<string>.Ignored, A<CancellationToken>.Ignored))
			.Returns(Task.FromResult("translated"));
		
		var ot = new ObjectTranslator(translator);
		var result = await ot.TranslateAsync(new ModelForTranslationLvl1(), "en", "ru", CancellationToken.None);
		Assert.NotNull(result);
		Assert.That(result.ForTranslation, Is.EqualTo("translated"));
		Assert.That(result.NotForTranslation, Is.EqualTo("default"));
		Assert.That(result.NotForTranslationAttr, Is.EqualTo("default"));
	}
	
	[Test]
	public async Task Translate_WithNested_Test()
	{
		ITranslator translator = A.Fake<ITranslator>();
		A.CallTo(() => translator
				.TranslateAsync(A<string>.Ignored, A<string>.Ignored,A<string>.Ignored, A<CancellationToken>.Ignored))
			.Returns(Task.FromResult("translated"));
		
		var ot = new ObjectTranslator(translator);
		var result = await ot.TranslateAsync(new ModelForTranslationLvl1(), "en", "ru", CancellationToken.None);
		
		Assert.NotNull(result);
		Assert.That(result.ForTranslation, Is.EqualTo("translated"));
		Assert.That(result.NotForTranslation, Is.EqualTo("default"));
		Assert.That(result.NotForTranslationAttr, Is.EqualTo("default"));
		
		Assert.That(result.NestedModel.ForTranslation, Is.EqualTo("translated"));
		Assert.That(result.NestedModel.NotForTranslation, Is.EqualTo("default"));
		Assert.That(result.NestedModel.NotForTranslationAttr, Is.EqualTo("default"));
		
		Assert.That(result.NestedModel.NestedModel.ForTranslation, Is.EqualTo("translated"));
		Assert.That(result.NestedModel.NestedModel.NotForTranslation, Is.EqualTo("default"));
		Assert.That(result.NestedModel.NestedModel.NotForTranslationAttr, Is.EqualTo("default"));
	}
}

public class ModelForTranslationLvl1
{
	[Translatable]
	public string ForTranslation { get; set; } = "default";
	public string NotForTranslation { get; set; } = "default";
	[Translatable(false)]
	public string NotForTranslationAttr { get; set; } = "default";
	public int JustInt { get; set; } = 1;
	public ModelForTranslationLvl2 NestedModel { get; set; } = new();

}
public class ModelForTranslationLvl2
{
	[Translatable]
	public string ForTranslation { get; set; } = "default";
	public string NotForTranslation { get; set; } = "default";
	[Translatable(false)]
	public string NotForTranslationAttr { get; set; } = "default";
	public int JustInt { get; set; } = 2;
	public ModelForTranslationLvl3 NestedModel { get; set; } = new();
	
}
public class ModelForTranslationLvl3
{
	[Translatable]
	public string ForTranslation { get; set; } = "default";
	public string NotForTranslation { get; set; } = "default";
	[Translatable(false)]
	public string NotForTranslationAttr { get; set; } = "default";
	public int JustInt { get; set; } = 3;
	
}
