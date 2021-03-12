using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using VOD.Common.DTOModels;
using VOD.Common.Entities;
using VOD.Database.Contexts;
using VOD.Database.Interfaces;

namespace VOD.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ICRUDService _db;
        private readonly LinkGenerator _linkGenerator;

        public CoursesController(ICRUDService db, LinkGenerator linkGenerator)
        {
            _db = db;
            _linkGenerator = linkGenerator;
        }

        //GET: api/courses, från lektion
       [HttpGet]
        public async Task<IEnumerable<CourseDTO>> Get()
        {
            var courses = await _db.GetAsync<Course, CourseDTO>(true);
            return courses;
        }

        //GET ONE api/courses/4 från lektion
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CourseDTO>> Get(int id, bool include = false)
        {
            try
            {
                var dto = await _db.SingleAsync<Course, CourseDTO>(s => s.Id.Equals(id), include);
                if (dto == null) return NotFound();
                return dto;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Database Failure");
            }
        }

        //POST från lektion
        [HttpPost]
        public async Task<ActionResult<CourseDTO>> Post(CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");

                var exists = await _db.AnyAsync<Instructor>(a => a.Id.Equals(model.InstructorId));
                if (!exists) return NotFound("Could not find related entity");

                var id = await _db.CreateAsync<CourseDTO, Course>(model);
                if (id < 1) return BadRequest("Unable to add the entity");

                var dto = await _db.SingleAsync<Course, CourseDTO>(s => s.Id.Equals(id), true);
                if (dto == null) return BadRequest("Unable to add the entity");

                var uri = _linkGenerator.GetPathByAction("Get", "Courses", new {id});
                return Created(uri, dto);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to add the entity");
            }
        }

        //PUT från lektion
        [HttpPut("{id:int}")]
        public async Task<ActionResult<CourseDTO>> Put(int id, CourseDTO model)
        {
            try
            {
                if (model == null) return BadRequest("No entity provided");
                if (!id.Equals(model.CourseId)) return BadRequest("Differing ids");

                var exists = await _db.AnyAsync<Instructor>(a => a.Id.Equals(model.InstructorId));
                if (!exists) return NotFound("Could not find related entity");

                exists = await _db.AnyAsync<Course>(a => a.Id.Equals(id));
                if (!exists) return NotFound("Could not find entity");

                if (await _db.UpdateAsync<CourseDTO, Course>(model)) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update the entity");
            }
            return BadRequest("Unable to update the entity");
        }

        //DELETE från lektion
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var exists = await _db.AnyAsync<Course>(a => a.Id.Equals(id));
                if (!exists) return BadRequest("Could not find entity");

                if (await _db.DeleteAsync<Course>(d => d.Id.Equals(id))) return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete the entity");
            }

            return BadRequest("Failed to delete the entity");
        }


        //// GET: api/Courses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        //{
        //    return await _context.Courses.ToListAsync();
        //}

        //    // GET: api/Courses/5
        //    [HttpGet("{id}")]
        //    public async Task<ActionResult<Course>> GetCourse(int id)
        //    {
        //        var course = await _context.Courses.FindAsync(id);
        //        if (course == null)
        //        {
        //            return NotFound();
        //        }
        //        return course;
        //    }

        //    // PUT: api/Courses/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutCourse(int id, Course course)
        //    {
        //        if (id != course.Id)
        //        {
        //            return BadRequest();
        //        }
        //        _context.Entry(course).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!CourseExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return NoContent();
        //    }

        //    // POST: api/Courses
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for
        //    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        //    [HttpPost]
        //    public async Task<ActionResult<Course>> PostCourse(Course course)
        //    {
        //        _context.Courses.Add(course);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        //    }

        //    // DELETE: api/Courses/5
        //    [HttpDelete("{id}")]
        //    public async Task<ActionResult<Course>> DeleteCourse(int id)
        //    {
        //        var course = await _context.Courses.FindAsync(id);
        //        if (course == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Courses.Remove(course);
        //        await _context.SaveChangesAsync();
        //        return course;
        //    }

        //    private bool CourseExists(int id)
        //    {
        //        return _context.Courses.Any(e => e.Id == id);
        //    }
    }
}
