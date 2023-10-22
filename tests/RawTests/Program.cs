// See https://aka.ms/new-console-template for more information

using KutCode.Cve.Services.Loaders;

Console.WriteLine("Hello, World!");

MitreYearlyLoader loader = new MitreYearlyLoader();

var cve = await loader.LoadCveByYearAsync(2017);

Console.ReadLine();