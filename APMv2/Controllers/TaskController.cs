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
    public class TaskController : Controller
    {
        private readonly APMv2Context _context;
        public TaskController(APMv2Context context)
        {
            _context = context;
        }
        [HttpGet("GetListTask")]
        public IActionResult GetListTaskBySprintId(int sprintId)
        {
            var listTask = _context.Tasks.Include(en => en.SprintBacklog).Include(en => en.User).Where(en => en.SprintBacklog.SprintId == sprintId).OrderBy(en => en.SprintBacklogId).ToList();
            List<TasksResponse> res = new List<TasksResponse>();
            foreach (var data in listTask)
            {
                TasksResponse item = new TasksResponse();
                item.Id = data.Id;
                item.SprintBacklogId = data.SprintBacklogId;
                item.UserId = data.UserId;
                item.FullName = data.User.FullName;
                item.Name = data.Name;
                item.Status = data.Status;
                item.Priority = data.Priority;
                item.Description = data.Description;
                item.Note = data.Note;
                item.EstimatedTime = data.EstimatedTime;
                res.Add(item);
            }
            return Ok(res);
        }

        [HttpGet("GetListTaskBySprintBacklogId")]
        public IActionResult GetListTaskBySprintBacklogId(int sprintBacklogId)
        {

            var listTask = _context.Tasks.Where(en => en.SprintBacklogId == sprintBacklogId).ToList();

            return Ok(listTask);
        }

        [HttpGet("GetTaskById")]
        public IActionResult GetTaskById(int id)
        {
            var task = _context.Tasks.Where(en => en.Id == id);
            return Ok(task);
        }

        [HttpPost("AddListTaskBySprintBacklogId")]
        public IActionResult AddListTaskBySprintBacklogId(int sprintBacklogId, [FromBody] List<TaskRequest> listTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            for (var i = 0; i < listTask.Count(); i++)
            {
                var newTask = new Tasks()
                {
                    SprintBacklogId = sprintBacklogId,
                    UserId = listTask[i].UserId,
                    Name = listTask[i].Name,
                    Status = listTask[i].Status,
                    Priority = listTask[i].Priority,
                    Description = listTask[i].Description,
                    Note = listTask[i].Note,
                    EstimatedTime = listTask[i].EstimatedTime,
                };
                _context.Tasks.Add(newTask);
            }

            _context.SaveChanges();
            return Ok();
        }
        [HttpPost("UpdateTask")]
        public IActionResult UpdateListTaskInSprintBacklog(int sprintBacklogId, [FromBody] List<TaskRequest> listTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var checkExist = _context.SprintBacklog.Where(en => en.Id == sprintBacklogId).Include(en => en.Tasks).FirstOrDefault();
            if (checkExist == null)
            {
                return BadRequest("Không tồn tại sprintBacklog");
            }
            for (var i = 0; i < listTask.Count(); i++)
            {
                var oldTask = _context.Tasks.Where(en => en.Id == listTask[i].Id).FirstOrDefault();
                if (oldTask != null)
                {//update
                    oldTask.SprintBacklogId = sprintBacklogId;
                    oldTask.UserId = listTask[i].UserId;
                    oldTask.Name = listTask[i].Name;
                    oldTask.Status = listTask[i].Status;
                    oldTask.Priority = listTask[i].Priority;
                    oldTask.Description = listTask[i].Description;
                    oldTask.Note = listTask[i].Note;
                    oldTask.EstimatedTime = listTask[i].EstimatedTime;
                    
                }
                else
                {//add new
                    var newTask = new Tasks()
                    {
                        SprintBacklogId = sprintBacklogId,
                        UserId = listTask[i].UserId,
                        Name = listTask[i].Name,
                        Status = 0,
                        Priority = checkExist.Priority,//priority task = priority sprint backlog
                        Description = listTask[i].Description,
                        Note = listTask[i].Note,
                        EstimatedTime = listTask[i].EstimatedTime,
                    };
                    _context.Tasks.Add(newTask);
                }
            }
            var totalTask = checkExist.Tasks.Count();
            var totalDoneTask = checkExist.Tasks.Where(en => en.Status == 2).Count();
            if (totalTask == totalDoneTask) checkExist.Status = 2;
            _context.SaveChanges();
            return Ok();
        }
    }
}