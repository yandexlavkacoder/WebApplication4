using System;

namespace WebApplication4.DTO
{
    public class UpdateCartDTO
    {
        public int UserId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}