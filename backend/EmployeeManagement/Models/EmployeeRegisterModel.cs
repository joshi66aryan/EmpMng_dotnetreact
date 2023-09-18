﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EmployeeManagement.Models
{
	public class EmployeeRegisterModel
	{
		[Key]
		public int Employeeid { get; set; }

		[Column(TypeName = "nvarchar(50)")]
		public string? EmployeeName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string? Occupation { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string? ImageName { get; set; }

		[NotMapped]
		public IFormFile? ImageFile { get; set; }

		[NotMapped]
		public string? ImageSrc { get; set; }

	}
}

