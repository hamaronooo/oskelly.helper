﻿
using KutCode.Cve.Application.CQRS.Report;
using KutCode.Cve.Domain.Dto;
using KutCode.Cve.Domain.Dto.Entities.Report;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KutCode.Cve.Api.Endpoints.Report.Pagination;

public sealed class Endpoint : Endpoint<PaginationRequest, PaginationResponse<ReportRequestDto>>
{
	private readonly IMediator _mediator;
	public Endpoint(IMediator mediator)
	{
		_mediator = mediator;
	}

	public override void Configure()
	{
		AllowAnonymous();
		Version(1);
		Get("report/pagination");
	}

	public override  async Task HandleAsync([FromQuery] PaginationRequest req, CancellationToken ct)
	{
		var result = await _mediator.Send(new ReportPaginationQuery(req), ct);
		await SendOkAsync(result, ct);
	}
}