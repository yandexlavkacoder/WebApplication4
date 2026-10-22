using System.Collections.Generic;

namespace WebApplication4.DTO
{
    /// <summary>
    /// Страница данных + общее количество записей (для заголовка X-Total-Count).
    /// </summary>
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = new List<T>();

        public int TotalCount { get; init; }

        public int Page { get; init; }

        public int PageSize { get; init; }
    }
}
