using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using APMv2.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class BacklogController : Controller
    {
        private readonly APMv2Context _context;
        public BacklogController(APMv2Context context)
        {
            _context = context;
        }

        [HttpGet("GetListBacklog")]
        //[Authorize]
        public IActionResult GetListBacklog(int projectId)
        {
            var listBacklog = _context.Backlog.Where(en=>en.ProjectId == projectId).Include(en => en.User).AsQueryable().ToList();//.Include(en=>en.User)
            if (listBacklog == null)
            {
                return BadRequest("Không tồn tại project");
            }
            var lstRet = from data in listBacklog
                         select new
                         {
                             id = data.Id,
                             name = data.Name,
                             description = data.Description,
                             userId = data.UserId,
                             performerName = data.User.FullName,
                             category = data.Category,
                             status = data.Status,
                             statusText = ((Status)data.Status).ToString(),
                             priority = data.Priority,
                             percentageRemain = data.PercentageRemain,
                         };
            return Ok(lstRet);
        }

        [HttpGet("GetBacklogById")]
        //[Authorize]
        public IActionResult GetBacklogById(int id)
        {
            var item = _context.Backlog.AsQueryable();
            if (item == null)
            {
                return BadRequest("Không tồn tại project");
            }
            return Ok(item);
        }

        [HttpPost("AddBacklog")]
        //[Authorize]
        public IActionResult AddBacklog([FromBody]BacklogRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var item = new Backlog()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Priority = model.Priority,
                    ProjectId = model.ProjectId,
                    UserId = model.UserId,
                    Category = model.Category,
                    Status = model.Status,
                    PercentageRemain = 100,
                };
                _context.Backlog.Add(item);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        [HttpPut("UpdateBacklog")]
        //[Authorize]
        public IActionResult UpdateBacklog([FromBody]BacklogRequest model)
        {
            var old = _context.Backlog.Where(en => en.Id == model.Id).FirstOrDefault();
            if (old == null)
            {
                return BadRequest("Không tồn tại");
            }
            try
            {
                old.Name = model.Name;
                old.Description = model.Description;
                old.Priority = model.Priority;
                old.UserId = model.UserId;
                old.Category = model.Category;
                old.Status = model.Status;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }
    }
}