namespace TestTasks.Task2;

public class PingSyncronizator
{
	/// <summary>
	/// 2.	There are two threads. The first one prints “ping”, the second one prints “pong”.
	/// Please write code that synchronizes these threads and sequentially prints “ping” and “pong”.
	/// </summary>
	public static void Run()
	{
		var barrier = new Barrier(2);
		var pingThread = new Thread(() => {
			Console.WriteLine("ping");
			barrier.SignalAndWait();
		});
		var pongThread = new Thread(() => {
			barrier.SignalAndWait();
			Console.WriteLine("pong");
		});
		pingThread.Start();
		pongThread.Start();
		
		// for consistent console output
		while (pingThread.ThreadState == ThreadState.Running || pongThread.ThreadState == ThreadState.Running)
			Thread.Sleep(10);
	}
}