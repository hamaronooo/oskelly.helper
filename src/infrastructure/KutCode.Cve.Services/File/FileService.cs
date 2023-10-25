
using Microsoft.AspNetCore.Http;
using Serilog;

namespace KutCode.Cve.Services.File;

public sealed class FileService : IFileService
{
	private readonly string _rootPath;
	private readonly object _locker = new();
	public FileService(string rootPath)
	{
		_rootPath = rootPath;
	}

	/// <summary>
	///     Сохраняет файл под именем Uid в директорию uploads
	/// </summary>
	/// <exception cref="FileLoadException">If file Length is 0</exception>
	public async Task SaveFormFileAsync(IFormFile file, Guid uid, CancellationToken ct = default)
	{
		if (file.Length <= 0)
			throw new FileLoadException();

		var filePath = Path.Combine(GetUploadsDirPath(), uid.ToString());
		using var fileStream = new FileStream(filePath, FileMode.Create);
		await file.CopyToAsync(fileStream, ct);
	}

	public async Task SaveFileAsync(Stream stream, Guid uid, CancellationToken ct = default)
	{
		var filePath = Path.Combine(GetUploadsDirPath(), uid.ToString());
		stream.Position = 0;
		using var writeStream = System.IO.File.OpenWrite(filePath);
		await writeStream.CopyToAsync(stream, ct);
	}

	public async Task SaveFileAsync(ReadOnlyMemory<byte> bytes, Guid uid, CancellationToken ct = default)
	{
		var filePath = Path.Combine(GetUploadsDirPath(), uid.ToString());
		using var writeStream = System.IO.File.OpenWrite(filePath);
		await writeStream.WriteAsync(bytes);
	}

	/// <summary>
	///     Get file data as byte array
	/// </summary>
	/// <exception cref="FileNotFoundException" />
	public async Task<byte[]> GetFileBytesAsync(Guid fileUid, CancellationToken ct = default)
	{
		var fileInfo = new FileInfo(Path.Combine(GetUploadsDirPath(), fileUid.ToString()));
		if (fileInfo.Exists is false)
			throw new FileNotFoundException();
		using var stream = fileInfo.OpenRead();
		var buffer = new byte[stream.Length];
		_ = await stream.ReadAsync(buffer, ct);
		return buffer;
	}

	public FileStream? GetFileStream(Guid fileUid)
	{
		var fileInfo = new FileInfo(Path.Combine(GetUploadsDirPath(), fileUid.ToString()));
		if (fileInfo.Exists is false)
			throw new FileNotFoundException();
		return fileInfo.OpenRead();
	}

	public void DeleteFile(Guid fileUid)
	{
		lock (_locker) {
			var fileInfo = new FileInfo(Path.Combine(GetUploadsDirPath(), fileUid.ToString()));
			if (fileInfo.Exists)
				try {
					fileInfo.Delete();
				}
				catch (Exception e) {
					Log.Error(e, "Can't delete file");
					// swallow
				}
		}
	}
	
	/// <summary>
	///     Данные о файле, существует ли он и сколько весит
	/// </summary>
	public (bool IsExist, long BytesSize) GetFileData(Guid? fileUid)
	{
		if (fileUid.HasValue == false) return (false, 0); 
		var fileInfo = new FileInfo(Path.Combine(GetUploadsDirPath(), fileUid.Value.ToString()));
		if (fileInfo.Exists is false)
			return (false, 0);
		return (true, fileInfo.Length);
	}
	
	private string GetUploadsDirPath()
	{
		var uploadsDirectory = Path.Combine(_rootPath, "files");
		if (Directory.Exists(uploadsDirectory) is false)
			Directory.CreateDirectory(uploadsDirectory);
		return uploadsDirectory;
	}
}