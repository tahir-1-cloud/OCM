using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace OCMDomain.Repository.Edmx
{
    public class CourseMaterialValidation
    {
        public string Filename { get; set; }
        public string FileType { get; set; }
        public DateTime? UploadDate { get; set; }
        [Required(ErrorMessage = "FilePath  is requierd")]
        public string FilePath { get; set; }
        public bool? FileApprove { get; set; }
        public bool? FilePending { get; set; }
        public bool? FileReject { get; set; }
        [Required(ErrorMessage = "Video  is requierd")]
        public string VideoTitle { get; set; }
        public string VideoDescription { get; set; }
        public string VideoPath { get; set; }
        public string ImagePath { get; set; }
        [Required(ErrorMessage = "CoverPhoto  is requierd")]
        public IFormFile CoverPhoto { get; set; }
        [Required(ErrorMessage = "CoverPhoto1  is requierd")]
        public IFormFile CoverPhoto1 { get; set; }
        [Required(ErrorMessage = "Video  is requierd")]
        public IFormFile Video { get; set; }
    }
    [MetadataType(typeof(CourseMaterialValidation))]
    public partial class CourseMaterialTble
    {
        [NotMapped]
        public IFormFile CoverPhoto { get; set; }
        [NotMapped]
        public IFormFile CoverPhoto1 { get; set; }
        [NotMapped]
        public IFormFile Video { get; set; }
    }
}