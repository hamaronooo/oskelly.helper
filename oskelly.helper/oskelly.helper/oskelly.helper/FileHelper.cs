namespace oskelly.helper;

public class FileHelper
{
	public static string? GetCreateCommentText()
	{
		var filePath = Path.Combine("config", "comment-to-create.txt");
		if (File.Exists(filePath) is false) return string.Empty;
		return File.ReadAllText(filePath)?.Trim();
	}
	
	public static string? GetRemoveCommentText()
	{
		var filePath = Path.Combine("config", "comment-to-remove.txt");
		if (File.Exists(filePath) is false) return string.Empty;
		return File.ReadAllText(filePath)?.Trim();
	}
}