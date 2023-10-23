// See https://aka.ms/new-console-template for more information

using KutCode.Cve.Services.CveLoad;

Console.WriteLine("Hello, World!");

MitreCveLoader cveLoader = new MitreCveLoader();

var cve = await cveLoader.LoadCveByYearAsync(2017);

Console.ReadLine();