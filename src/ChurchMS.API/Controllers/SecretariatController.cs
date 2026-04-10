using ChurchMS.Application.Features.Secretariat.Commands.IssueCertificate;
using ChurchMS.Application.Features.Secretariat.Commands.RecordBaptism;
using ChurchMS.Application.Features.Secretariat.Commands.RecordMarriage;
using ChurchMS.Application.Features.Secretariat.Commands.RegisterDocument;
using ChurchMS.Application.Features.Secretariat.DTOs;
using ChurchMS.Application.Features.Secretariat.Queries.GetBaptismList;
using ChurchMS.Application.Features.Secretariat.Queries.GetCertificateList;
using ChurchMS.Application.Features.Secretariat.Queries.GetDocumentList;
using ChurchMS.Application.Features.Secretariat.Queries.GetMarriageList;
using ChurchMS.Domain.Enums;
using ChurchMS.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChurchMS.API.Controllers;

/// <summary>
/// Secretariat module: documents, certificates, baptism records, marriage records.
/// </summary>
[Authorize]
public class SecretariatController : BaseApiController
{
    // ── Documents ──────────────────────────────────────────────────────────

    /// <summary>List documents, optionally filtered by type or member.</summary>
    [HttpGet("documents")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<DocumentDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDocuments(
        [FromQuery] DocumentType? type = null,
        [FromQuery] Guid? memberId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetDocumentListQuery(type, memberId, page, pageSize)));

    /// <summary>Register (upload) a new document.</summary>
    [HttpPost("documents")]
    [Authorize(Policy = "SecretaryOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<DocumentDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RegisterDocument([FromBody] RegisterDocumentCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Certificates ──────────────────────────────────────────────────────

    /// <summary>List certificates, optionally filtered by type or member.</summary>
    [HttpGet("certificates")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<CertificateDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCertificates(
        [FromQuery] CertificateType? type = null,
        [FromQuery] Guid? memberId = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetCertificateListQuery(type, memberId, page, pageSize)));

    /// <summary>Issue a new certificate for a member.</summary>
    [HttpPost("certificates")]
    [Authorize(Policy = "SecretaryOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<CertificateDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> IssueCertificate([FromBody] IssueCertificateCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Baptism Records ───────────────────────────────────────────────────

    /// <summary>List baptism records, optionally filtered by member or year.</summary>
    [HttpGet("baptisms")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<BaptismRecordDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBaptisms(
        [FromQuery] Guid? memberId = null,
        [FromQuery] int? year = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetBaptismListQuery(memberId, year, page, pageSize)));

    /// <summary>Record a baptism event for a member.</summary>
    [HttpPost("baptisms")]
    [Authorize(Policy = "SecretaryOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<BaptismRecordDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordBaptism([FromBody] RecordBaptismCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }

    // ── Marriage Records ──────────────────────────────────────────────────

    /// <summary>List marriage records, optionally filtered by member or year.</summary>
    [HttpGet("marriages")]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<MarriageRecordDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMarriages(
        [FromQuery] Guid? memberId = null,
        [FromQuery] int? year = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
        => Ok(await Mediator.Send(new GetMarriageListQuery(memberId, year, page, pageSize)));

    /// <summary>Record a marriage event.</summary>
    [HttpPost("marriages")]
    [Authorize(Policy = "SecretaryOrAbove")]
    [ProducesResponseType(typeof(ApiResponse<MarriageRecordDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> RecordMarriage([FromBody] RecordMarriageCommand command)
    {
        var result = await Mediator.Send(command);
        return result.Success ? StatusCode(StatusCodes.Status201Created, result) : BadRequest(result);
    }
}
