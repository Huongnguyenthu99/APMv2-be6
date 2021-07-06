using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using APMv2.Model.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class SprintBacklogController : Controller
    {
        private readonly APMv2Context _context;
        public SprintBacklogController(APMv2Context context)
        {
            _context = context;
        }

        [HttpGet("GetListSprintBacklogBySprintId")]
        public IActionResult GetListSprintBacklogBySprintId(int sprintId)
        {
            var listSprintBacklog = _context.SprintBacklog.Where(en => en.SprintId == sprintId).Include(en=>en.Backlog).ToList();
            var lstRet = from data in listSprintBacklog
                         select new
                         {
                             id = data.Id,
                             backlogName = data.Backlog.Name,
                             name = data.Name,
                             percentageRemain = data.PercentageRemain,
                             userId = data.Backlog.UserId,
                             priority = data.Backlog.Priority,
                             status = data.Status,
                         };
            return Ok(lstRet);
        }

        [HttpGet("GetListSprintBacklogOrderTask")]
        //[Authorize]
        public IActionResult GetListSprintBacklogOrderTask(int sprintId)
        {
            var listSprintBacklog = _context.SprintBacklog.Include(en => en.Tasks).ThenInclude(e => e.User).Include(en => en.Backlog).Where(en => en.SprintId == sprintId).ToList();

            if (listSprintBacklog == null)
            {
                return BadRequest("Không tồn tại project");
            }
            List<SprintBacklogResponse> res = new List<SprintBacklogResponse>();

            foreach(var i in listSprintBacklog)
            {
                SprintBacklogResponse item = new SprintBacklogResponse();
                item.Id = i.Id;
                item.SprintId = i.SprintId;
                item.BacklogId = i.BacklogId;
                item.BacklogName = i.Backlog.Name;
                item.Name = i.Name;
                item.PercentageRemain = i.PercentageRemain;
                item.Status = i.Status;
                item.Priority = i.Priority;

                List<TasksResponse> tasks = new List<TasksResponse>();
                item.DoneTask = 0;
                foreach(var k in i.Tasks)
                {
                    TasksResponse task = new TasksResponse();
                    task.Id = k.Id;//UserId
                    task.UserId = k.UserId;
                    task.SprintBacklogId = k.SprintBacklogId;
                    task.FullName = k.User.FullName;
                    task.Name = k.Name;
                    task.Status = k.Status;
                    task.Priority = k.Priority;
                    task.Description = k.Description;
                    task.Note = k.Note;
                    task.EstimatedTime = k.EstimatedTime;
                    tasks.Add(task);
                    if (task.Status == 2) item.DoneTask++;
                }
                item.ListTasks = tasks;
                item.TotalTask = tasks.Count();
                res.Add(item);
            }
            //var a = Newtonsoft.Json.JsonConvert.SerializeObject(listSprintBacklog);
            return Ok(res);
        }

        [HttpGet("GetSprintBacklogById")]
        //[Authorize]
        public IActionResult GetSprintBacklogById(int id)
        {
            var item = _context.SprintBacklog.Where(en=>en.Id==id).FirstOrDefault();
            if (item == null)
            {
                return BadRequest("Không tồn tại project");
            }
            return Ok(item);
        }

        [HttpPost("ConvertBacklog")]
        public IActionResult ConvertBacklog(int sprintId, [FromBody] List<SprintBacklogRequest> listBacklog)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Tham số truyền vào không đúng");
            }
            try
            {
                var sprint = _context.Sprint.Where(en => en.Id == sprintId).FirstOrDefault();
                if (sprint == null)
                {
                    return BadRequest("Không tồn tại sprint");
                }

                for (var i = 0; i < listBacklog.Count(); i++)
                {
                    var backlog = _context.Backlog.Where(en => en.Id == listBacklog[i].BacklogId).FirstOrDefault();
                    if (backlog == null)
                    {
                        return BadRequest("Không tồn tại backlog");
                    }
                    
                    var oldBacklog = _context.SprintBacklog.Where(en => en.SprintId == listBacklog[i].SprintId).FirstOrDefault();
                    if(oldBacklog != null)
                    {
                        oldBacklog.SprintId = sprintId;
                        oldBacklog.BacklogId = listBacklog[i].BacklogId;
                        oldBacklog.Name = listBacklog[i].Name;
                        oldBacklog.PercentageRemain = listBacklog[i].PercentageRemain;
                        oldBacklog.Status = listBacklog[i].Status;
                        oldBacklog.Priority = listBacklog[i].Priority;
                    }
                    else
                    {
                        var sprintBacklog = new SprintBacklog()
                        {
                            SprintId = sprintId,
                            BacklogId = backlog.Id,
                            Name = backlog.Name,
                            PercentageRemain = backlog.PercentageRemain,
                            Status = backlog.Status,
                            Priority = backlog.Priority
                        };
                        _context.SprintBacklog.Add(sprintBacklog);
                        backlog.Status = 1;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpPut("UpdateSprintBacklog")]
        //[Authorize]
        public IActionResult UpdateSprintBacklog([FromBody]SprintBacklogRequest model)
        {
            var old = _context.SprintBacklog.Where(en => en.Id == model.Id).FirstOrDefault();
            if (old == null)
            {
                return BadRequest("Không tồn tại");
            }
            try
            {
                //old.SprintId = model.SprintId;
                //old.BacklogId = model.BacklogId;
                old.Name = model.Name;
                old.PercentageRemain= model.PercentageRemain;
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