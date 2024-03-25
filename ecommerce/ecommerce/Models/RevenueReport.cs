﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ecommerce.Models
{
    [Table("revenue_report")]
    public class RevenueReport
    {
        [Key]
        [Column("report_id")]
        public int ReportId { get; set; }

        [Column("date")]
        public DateTime Date { get; set; }
        [Column("year")]
        public int Year { get; set; }
        [Column("month")]
        public int Month { get; set; }
        [Column("day")]
        public int Day { get; set; }

        [Column("total_revenue")]
        public decimal TotalRevenue { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; } = null;
    }
}
