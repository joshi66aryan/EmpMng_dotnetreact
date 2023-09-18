using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagement.DatabaseConnection;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Cors;

namespace EmployeeManagement.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowLocalhost")]   // for cors policy. 
    [ApiController]
    public class EmployeeAuth : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;    // IWebHostEnvironment is interface is used to provide information
                                                                  // about the web hosting environment in which the application is running. 
                                                        


        public EmployeeAuth(DatabaseContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }



        // GET: api/EmployeeAuth

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeRegisterModel>>> Getemployee()   // ActionResult is used in .Net Web, whereas IActionResukt is used in .net core web.
        {
          if (_context.employee == null)
          {
              return NotFound();
          }
            return await _context.employee
                .Select( x => new EmployeeRegisterModel()
                {
                    Employeeid = x.Employeeid,
                    EmployeeName = x.EmployeeName,
                    Occupation = x.Occupation,
                    ImageName = x.ImageName,
                    ImageSrc = string.Format("{0}://{1}{2}/Images/{3}", Request.Scheme,Request.Host,Request.PathBase,x.ImageName)
                })
                .ToListAsync();
        }



        // GET: api/EmployeeAuth/5

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeRegisterModel>> GetEmployeeRegisterModel(int id)
        {
          if (_context.employee == null)
          {
              return NotFound();
          }
            var employeeRegisterModel = await _context.employee.FindAsync(id);

            if (employeeRegisterModel == null)
            {
                return NotFound();
            }

            return employeeRegisterModel;
        }


        // PUT: api/Employee/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployeeModel(int id, [FromForm] EmployeeRegisterModel employeeRegisterModel)
        {
            if (id != employeeRegisterModel.Employeeid)
            {
                return BadRequest();
            }

            if (employeeRegisterModel.ImageFile != null)
            {
                DeleteImage(employeeRegisterModel.ImageName);    // deleting previous image is use upload new image.
                employeeRegisterModel.ImageName = await SaveImage(employeeRegisterModel.ImageFile);  // saving new image.
            }

            _context.Entry(employeeRegisterModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeRegisterModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // POST: api/EmployeeAuth
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754

        [HttpPost]
        public async Task<ActionResult<EmployeeRegisterModel>> PostEmployeeRegisterModel([FromForm]EmployeeRegisterModel employeeRegisterModel)
        {
          if (_context.employee == null)
          {
              return Problem("Entity set 'DatabaseContext.employee'  is null.");
          }

            employeeRegisterModel.ImageName = await SaveImage(employeeRegisterModel.ImageFile);

            _context.employee.Add(employeeRegisterModel);
            await _context.SaveChangesAsync();


            return CreatedAtAction("GetEmployeeRegisterModel", new { id = employeeRegisterModel.Employeeid }, employeeRegisterModel);
        }




        // DELETE: api/EmployeeAuth/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeRegisterModel(int id)
        {
            if (_context.employee == null)
            {
                return NotFound();
            }
            var employeeRegisterModel = await _context.employee.FindAsync(id);

            if (employeeRegisterModel == null)
            {
                return NotFound();
            }

            DeleteImage(employeeRegisterModel.ImageName);
            _context.employee.Remove(employeeRegisterModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool EmployeeRegisterModelExists(int id)
        {
            return (_context.employee?.Any(e => e.Employeeid == id)).GetValueOrDefault();
        }




        [NonAction]   // This attribute indicates that this method should not be treated as an action method by the MVC framework
        public async Task<string> SaveImage(IFormFile imageFile)
        {

            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is null or empty.");
            }

                    // Generate a unique image name based on the original file name and current timestamp

            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);

                         // Create the full image path by combining the content root path and the "Images" folder with the generated image name

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

                         // Create a file stream to save the uploaded image file

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                        // Copy the content of the uploaded image file to the file stream

                await imageFile.CopyToAsync(fileStream);
                
            }

                        // Return the generated image name as the result

            return imageName;
        }

        [NonAction]     // delete old  image after user update new image.
        public void DeleteImage( string imageName)
        {

            var imagePath = Path.Combine(_hostEnvironment.ContentRootPath, "Images", imageName);

            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

        } 
        
    }
}
