using Humanizer;
using oskelly.helper.OskellyRespository.Models;

namespace oskelly.helper;

public class ConsoleHelper
{
	public static void WriteWelcome()
	{
		Console.ForegroundColor = ConsoleColor.Yellow;
		Console.WriteLine($"===[ Oskelly Helper ]====================={Environment.NewLine}" +
		                  $"=====================[ Kutumov Nikita ]==={Environment.NewLine}");
		Console.ResetColor();
	}

	public static (string login, string password) GetLoginAndPassword()
	{
		Console.WriteLine();
		Console.Write("Login: ");
		string? login;
		string? password;
		while ((login = Console.ReadLine()?.Trim()) != null && string.IsNullOrEmpty(login)) {
			ClearLastLine(2);
			Console.WriteLine("Wrong login!");
			Console.Write("Login: ");
		}

		Console.WriteLine();
		Console.Write("Password: ");
		while ((password = Console.ReadLine()?.Trim()) != null && string.IsNullOrEmpty(password)) {
			ClearLastLine(2);
			Console.WriteLine("Wrong password format!");
			Console.Write("Password: ");
		}
		return (login, password);
	}

	public static void ShowSuccessAuthBanner(AuthorizationResponse authResponse)
	{
		Console.ForegroundColor = ConsoleColor.DarkGreen;
		Console.WriteLine($"===[ Authorized ]====[ User: {authResponse.Data?.Id} ]===");
		Console.ResetColor();
	}
	
	public static void ShowFailedAuthBanner(AuthorizationResponse authResponse)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"===[ Failed ]====[ {authResponse.Message.Truncate(40)} ]===");
		Console.ResetColor();
	}
	
	public static void ShowRemoveCommentNotFound()
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"Comment to REMOVE not found or empty!");
		Console.Write($">>>   Press any key to continue");
		Console.ResetColor();
		Console.Read();
	}
	
	public static void ShowCreateCommentNotFound()
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"Comment to CREATE not found or empty!");
		Console.Write($">>>   Press any key to continue");
		Console.ResetColor();
		Console.Read();
	}
	
	public static void ClearLastLine(int linesCount = 1)
	{
		for (int i = 0; i < linesCount-1; i++) {
			Console.SetCursorPosition(0, Console.CursorTop - 1);
			Console.Write(new string(' ', Console.BufferWidth));
			Console.SetCursorPosition(0, Console.CursorTop - 1);
		}
	}
}