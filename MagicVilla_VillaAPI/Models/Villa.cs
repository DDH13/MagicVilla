﻿using System.ComponentModel.DataAnnotations;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class Villa
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int Occupancy { get; set; }
        public int Sqft { get; set; }

    }
}