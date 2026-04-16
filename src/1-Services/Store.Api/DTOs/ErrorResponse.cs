namespace Store.Api.DTOs;

public record ErrorResponse(int StatusCode, string Message, IEnumerable<string>? Errors = null);
