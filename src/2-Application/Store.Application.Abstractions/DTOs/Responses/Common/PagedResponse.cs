using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Application.Abstractions.DTOs.Responses.Common;

public record PagedResponse<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize)
{
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
}
