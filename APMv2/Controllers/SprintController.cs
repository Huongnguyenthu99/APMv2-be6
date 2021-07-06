using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using Microsoft.AspNetCore.Mvc;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class SprintController : Controller
    {
        private readonly APMv2Context _context;
        public SprintController(APMv2Context context)
        {
            _context = context;
        }

        [HttpGet("GetListSprintByProjectId")]
        public IActionResult GetListSprintByProjectId(int projectId)
        {
            var listTask = _context.Sprint.Where(en => en.ProjectId == projectId).OrderByDescending(en => en.Id).ToList();
            return Ok(listTask);
        }

        [HttpGet("GetSprintById")]
        //[Authorize]
        public IActionResult GetSprintById(int id)
        {
            var item = _context.Sprint.AsQueryable();
            if (item == null)
            {
                return BadRequest("Không tồn tại project");
            }
            return Ok(item);
        }

        [HttpPost("AddSprint")]
        public IActionResult AddSprint([FromBody]SprintRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Tham số truyền vào không đúng");
            }
            var sprint = new Sprint()
            {
                ProjectId = model.ProjectId,
                Name = model.Name,
                TimeStart = model.TimeStart,
                TimeEnd = model.TimeEnd,
                WorkingDay = model.WorkingDay,
            };
            _context.Sprint.Add(sprint);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("UpdateSprint")]
        //[Authorize]
        public IActionResult UpdateSprint([FromBody]SprintRequest model)
        {
            var old = _context.Sprint.Where(en => en.Id == model.Id).FirstOrDefault();
            if (old == null)
            {
                return BadRequest("Không tồn tại");
            }
            try
            {
                old.Name = model.Name;
                //old.Description = model.Description;
                old.TimeStart = model.TimeStart;
                old.TimeEnd = model.TimeEnd;
                old.WorkingDay = model.WorkingDay;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        [HttpGet("GetListTimeOff")]
        public IActionResult GetListTimeOff(int sprintId)
        {
            var list = _context.TimeWorkingOff.Where(en => en.SprintId == sprintId).ToList();
            return Ok(new
            {
                total = (list == null) ? 0 : list.Count(),
                listTime = list,
            });
        }

        [HttpPost("UpdateTimeOff")]
        public IActionResult UpdateTimeOff([FromBody] List<TimeWorkingOffRequest> listTime)
        {
            foreach (var data in listTime)
            {
                var old = _context.TimeWorkingOff.Where(en => en.Id == data.Id).FirstOrDefault();
                if (old != null)
                {
                    old.UserId = data.UserId;
                    old.SprintId = data.SprintId;
                    old.DayOff = data.DayOff;
                    old.TotalDayOff = data.TotalDayOff;
                }
                else
                {
                    var newTime = new TimeWorkingOff()
                    {
                        UserId = data.UserId,
                        SprintId = data.SprintId,
                        DayOff = data.DayOff,
                        TotalDayOff = data.TotalDayOff,
                    };
                    _context.TimeWorkingOff.Add(newTime);
                }
            }
            _context.SaveChanges();
            return Ok(new
            {
                IsValid = true,
            });
        }
    }
}