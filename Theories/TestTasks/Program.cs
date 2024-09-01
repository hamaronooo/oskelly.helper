using System.ComponentModel.DataAnnotations;
using TestTasks.Task2;
using TestTasks.Task3;
using TestTasks.Task4;
using TestTasks.Task5;

// task 1
Console.Write("#1; Please, check ");
Console.Write("CustomSorter", Console.ForegroundColor = ConsoleColor.Red); 
Console.ResetColor();
Console.Write(" class in Task1 directory." + Environment.NewLine);
Console.WriteLine("----------------------");

// task 2
Console.WriteLine("#2; Ping-Pong sync:");
PingSyncronizator.Run();
Console.WriteLine("----------------------");

// task 3
var dev1 = DeviationCalculator.CalculateStandardDeviation(new double[] { 1,2,3,4,5,6,7,8,9,10 });
var dev2 = DeviationCalculator.CalculateStandardDeviation(new double[] { 10, 100, 1000, 10000, 100000 });
Console.WriteLine($"#3; Deviation-1: {dev1}");
Console.WriteLine($"#3; Deviation-2: {dev2}");

Console.WriteLine("----------------------");

// task 4 
var entries = TextSearcher.RegexCountPatternEntries("1dh81oBadACn19mk;BasaACasdBACmdaBAAACasdk1");
Console.WriteLine($"#4; Entries: {entries}");

Console.WriteLine("----------------------");

// task 5
Console.WriteLine($"#5; Looped. Is looped: {OneWayLinkedListDeterminator<int>
	.IsClosed(OneWayLinkedListDeterminator.LoopList)}");
Console.WriteLine($"#5; Not Looped. Is looped: {OneWayLinkedListDeterminator<int>
	.IsClosed(OneWayLinkedListDeterminator.NotLoopedList)}");