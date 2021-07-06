using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//using APMv2.Helpers;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using APMv2.Model.Response;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : Controller
    {
        private readonly APMv2Context _context;
        public ProjectController(APMv2Context context)
        {
            _context = context;
        }

        [HttpGet("GetListProject")]
        //[Authorize]
        public IActionResult GetListProject(int userId)
        {
            var listProjUser = _context.ProjectUser.Where(x => x.UserId == userId).Include(en=>en.Project).ToList();
            if (listProjUser == null)
            {
                return BadRequest("Không tồn tại project");
            }
            var res = new List<ProjectResponse>();
            foreach(var data in listProjUser)
            {
                var memOfProj = _context.ProjectUser.Where(en => en.ProjectId == data.ProjectId).Include(en => en.User).ToList();
                var listMember = new List<UserInfo>();

                foreach (var user in memOfProj)
                {
                    var u = new UserInfo()
                    {
                        Id = user.User.Id,
                        FullName = user.User.FullName,
                        Username = user.User.Username
                    };
                    listMember.Add(u);
                }
                var proj = new ProjectResponse()
                {
                    Id = data.Id,
                    ProjectId = data.ProjectId,
                    Name = data.Project.Name,
                    Description = data.Project.Description,
                    ListUser = listMember,
                    TotalMember = listMember.Count()
                };
                res.Add(proj);
            }
            return Ok(res);
        }

        [HttpGet("GetProjectById")]
        //[Authorize]
        public IActionResult GetProjectById(int id)
        {
            var proj = _context.Project.Where(en => en.Id == id).FirstOrDefault();
            if (proj == null)
            {
                return BadRequest("Không tồn tại project");
            }
            return Ok(proj);
        }

        [HttpPost("UpdateProjectInfo")]
        public IActionResult UpdateProjectInfo([FromBody] ProjectRequest proj) 
        {
            var project = _context.Project.Where(en => en.Id == proj.Id).FirstOrDefault();
            if (project == null)
            {
                return BadRequest("Không tồn tại project");
            }
            project.Description = proj.Description;
            _context.SaveChanges();

            return Ok();
        }

        [HttpPost("AddProject")]
        //[Authorize]
        public IActionResult AddProject([FromBody]ProjectRequest model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            try
            {
                var proj = new Project()
                {
                    Name = model.Name,
                    Description = model.Description
                };
                _context.Project.Add(proj);
                _context.SaveChanges();
                AddUserToProject(proj.Id, model.CreatorId);
                AddSprintToProject(proj.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        private void AddUserToProject(int projectId, int userId)// add creator to member
        {
            try
            {
                var addUser = new ProjectUser()
                {
                    ProjectId = projectId,
                    UserId = userId,
                };
                _context.ProjectUser.Add(addUser);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        private void AddSprintToProject(int projectId)
        {
            try
            {
                var sprint = new Sprint()
                {
                    Name = "Sprint 1",
                    WorkingDay = 0,
                    ProjectId = projectId
                };
                _context.Sprint.Add(sprint);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}