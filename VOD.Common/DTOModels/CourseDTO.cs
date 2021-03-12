using System;
using System.Collections.Generic;
using System.Text;
using VOD.Common.Entities;

namespace VOD.Common.DTOModels
{
    public class CourseDTO
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; }
        public string CourseDescription { get; set; }
        public string MarqueeImageUrl { get; set; }
        public string CourseImageUrl { get; set; }
        public int InstructorId { get; set; }
        public string Instructor { get; set; }
        public List<ModuleDTO> Modules { get; set; }
    }
}
