using System;

namespace WebApplication4.DTO
{
    public class PatchCartDTO
    {
        public int? UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}