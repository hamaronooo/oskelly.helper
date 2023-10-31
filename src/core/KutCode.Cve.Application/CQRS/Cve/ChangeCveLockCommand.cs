using KutCode.Cve.Application.Database;

namespace KutCode.Cve.Application.CQRS.Cve;

public sealed record ChangeCveLockCommand(CveId CveId, bool Locked = true) : IRequest;
public sealed class ChangeCveLockCommandHandler : IRequestHandler<ChangeCveLockCommand>
{
	private readonly MainDbContext _context;

	public ChangeCveLockCommandHandler(MainDbContext context)
	{
		_context = context;
	}

	public async Task Handle(ChangeCveLockCommand request, CancellationToken ct)
	{
		_context.Cve.Entry(new CveEntity(request.CveId) {Locked = request.Locked}).Property(x => x.Locked).IsModified = true;
		await _context.SaveChangesAsync(ct);
	}
}