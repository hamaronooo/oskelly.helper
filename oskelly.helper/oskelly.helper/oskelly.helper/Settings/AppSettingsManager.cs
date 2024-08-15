
using Newtonsoft.Json;

namespace oskelly.helper.Settings;

public sealed class AppSettingsManager
{
	public static AppSettings? Settings { get; private set; }
	public static bool Exists => Settings is not null;

#if DEBUG
	private static string _settingsFilePath = Path.Combine("__appsettings.json");
#else 
	private static string _settingsFilePath = Path.Combine("appsettings.json");
#endif

	 static AppSettingsManager()
	{
		if (File.Exists(_settingsFilePath)) {
			var text = File.ReadAllText(_settingsFilePath);
			Settings = JsonConvert.DeserializeObject<AppSettings>(text);
		}
	}

	public static void Override(AppSettings appSettings)
	{
		Settings = appSettings;
		if (!File.Exists(_settingsFilePath))
			File.Create(_settingsFilePath).Dispose();
		File.WriteAllText(_settingsFilePath, JsonConvert.SerializeObject(appSettings));
	}
}