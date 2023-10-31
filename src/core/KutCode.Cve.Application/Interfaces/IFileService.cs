
using Microsoft.AspNetCore.Http;

namespace KutCode.Cve.Application.Interfaces;

public interface IFileService
{
	Task SaveFormFileAsync(IFormFile file, Guid uid, CancellationToken ct= default);
	Task SaveFileAsync(Stream stream, Guid uid, CancellationToken ct= default);
	Task SaveFileAsync(ReadOnlyMemory<byte> bytes, Guid uid, CancellationToken ct= default);
	Task<byte[]> GetFileBytesAsync(Guid fileUid, CancellationToken ct= default);
	FileStream GetFileStream(Guid fileUid);
	void DeleteFile(Guid fileUid);
	(bool IsExist, long BytesSize) GetFileData(Guid? fileUid);
}