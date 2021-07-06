using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APMv2.Model.Entities;
using APMv2.Model.Request;
using APMv2.Model.Response;
using APMv2.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APMv2.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly APMv2Context _context;
        public UserController(APMv2Context context)
        {
            _context = context;
        }

        [HttpGet("GetListUser")]
        //[Authorize]
        public IActionResult GetListUser()
        {
            var listUser = _context.User.Include(en=>en.ProjectUser).ThenInclude(en => en.Project).ToList();
            if (listUser == null)
            {
                return BadRequest("Không tồn tại project");
            }
            var res = new List<UserResponse>();
            foreach (var user in listUser)
            {
                var lstPr = new List<string>();
                foreach(var proj in user.ProjectUser)
                {
                    lstPr.Add(proj.Project.Name);
                }
                var u = new UserResponse()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    Dob = user.Dob,
                    Gender = user.Gender,
                    Position = user.Position,
                    PositionText = ((Position)user.Position).ToString(),
                    ListProjectName = lstPr,
                };
                res.Add(u);
            }
            return Ok(res);
        }

        [HttpGet("GetListUserByProjectId")]
        //[Authorize]
        public IActionResult GetListUserByProjectId(int projectId)
        {
            var listUser = _context.ProjectUser.Where(en => en.ProjectId == projectId).Include(en=>en.User).AsQueryable().ToList();
            if (listUser == null)
            {
                return BadRequest("Không tồn tại project");
            }
            var res = from data in listUser
                      select new
                      {
                          id = data.Id,
                          userId = data.UserId,
                          fullName = data.User.FullName,
                          username = data.User.Username,
                          email = data.User.Email
                      };

            return Ok(res);
        }

        [HttpPost("AddMemberToProject")]
        public IActionResult AddMemberToProject([FromBody] List<int> listUserId, int projectId)
        {
            try
            {
                foreach (var id in listUserId)
                {
                    var user = _context.User.Where(en => en.Id == id).FirstOrDefault();
                    if (user == null)
                    {
                        return BadRequest("Không tồn tại userId trong hệ thống");
                    }
                    var oldUser = _context.ProjectUser.Where(en => en.UserId == id && en.ProjectId == projectId).FirstOrDefault();
                    if (oldUser == null)
                    {
                        var projUser = new ProjectUser()
                        {
                            UserId = user.Id,
                            ProjectId = projectId
                        };
                        _context.ProjectUser.Add(projUser);
                    }
                    _context.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Ok();
        }

        [HttpGet("GetUserById")]
        //[Authorize]
        public IActionResult GetUserById(int id)
        {
            var user = _context.User.Where(en => en.Id == id).FirstOrDefault();
            if (user == null)
            {
                return BadRequest("Không tồn tại project");
            }
            var res = new UserResponse()
            {
                Id = user.Id,
                FullName = user.FullName,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role,
                Dob = user.Dob,
                Gender = user.Gender,
                Position = user.Position,
                PositionText = ((Position)user.Position).ToString(),
            };
            return Ok(res);
        }

        [HttpPost("UpdateUserInfo")]
        public IActionResult UpdateUserInfo([FromBody] UserRequest model)
        {
            var oldUser = _context.User.Where(en => en.Id == model.Id).FirstOrDefault();
            if (oldUser == null)
            {
                return BadRequest("Không tồn tại user trong hệ thống");
            }
            oldUser.FullName = model.FullName;
            oldUser.Email = model.Email;
            oldUser.Dob = model.Dob;
            oldUser.Gender = model.Gender;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost("CreateAccount")]
        public async Task<ActionResult> CreateAccountThrGmail([FromBody] CreateAccountRequest request)
        {
            try
            {
                var exist = _context.User.Where(en => en.Email == request.Email).FirstOrDefault();
                if (exist != null)
                {
                    return BadRequest("Đã tồn tại email trong hệ thống");
                }
                string passwordTemporary = await Utilities.MailUtils.SendMailGoogleSmtp(request.Email, 0);
                var newUser = new User()
                {
                    Email = request.Email,
                    FullName = request.Fullname,
                    Username = request.Email,
                    Password = passwordTemporary,
                    //Position = request.Position
                    //ActiveAccount = Activate.InActive
                };
                _context.User.Add(newUser);
                _context.SaveChanges();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        [HttpPut]
        //[Authorize]
        public IActionResult UpdateInfo(int id, [FromBody] User user)
        {
            var oldUser = _context.User.Where(en => en.Id == id).FirstOrDefault();
            if (oldUser == null)
            {
                return BadRequest("Không tồn tại user trong hệ thống");
            }
            try
            {
                oldUser.FullName = user.FullName;
                oldUser.Username = user.Username;
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok(true);
        }

        [HttpPost("SendEmailToChangePass")]
        public async Task<ActionResult> SendEmailToChangePass(string email)
        {
            var exist = _context.User.Where(en => en.Email == email).FirstOrDefault();
            if (exist == null)
            {
                return Ok(new
                {
                    IsValid = false,
                    ErrorMessage = "Không tồn tại email trong hệ thống!",
                });
            }
            try
            {
                string passwordTemporary = await Utilities.MailUtils.SendMailGoogleSmtp(email, 1);
                exist.Password = passwordTemporary;
                _context.SaveChanges();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return Ok(new
            {
                isValid = true,
            });
        }

        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword([FromBody]ChangePasswordRequest model) {
            var user = _context.User.Where(en => en.Email == model.Email && en.Password == model.OldPassword).FirstOrDefault();
            if (user == null)
            {
                return Ok(new
                {
                    IsValid = false,
                    errorMessage = "Không tồn tại email trong hệ thống!",
                });
            }
            user.Password = model.NewPassword;
            _context.SaveChanges();
            return Ok(new
            {
                IsValid = true,
                UserInfo = user,
            });
        }

        [HttpGet("GetTimeOfUserBySprintId")]
        public IActionResult GetTimeOfUserBySprintId(int sprintId, int projectId)
        {
            var listProjUser = _context.ProjectUser.Include(en=>en.User).Where(en => en.ProjectId == projectId).ToList();
            if (listProjUser == null)
            {
                return BadRequest();
            }
            var res = new List<UserTimeResponse>();

            var workingDay = _context.Sprint.Where(en => en.Id == sprintId).FirstOrDefault().WorkingDay;
            var listDayOffUser = _context.TimeWorkingOff.Where(en => en.SprintId == sprintId).ToList();
            var listTaskInSprint = _context.Tasks.Include(en => en.SprintBacklog).Where(en => en.SprintBacklog.SprintId == sprintId).ToList();

            if (listDayOffUser != null)
            {
                foreach (var projUser in listProjUser)
                {
                    var item = new UserTimeResponse();

                    item.Id = projUser.UserId;
                    item.FullName = projUser.User.FullName;
                    item.TotalTime = workingDay * 8;
                    item.TotalEstimatedTime = 0;
                    foreach (var userOff in listDayOffUser)
                    {
                        if (projUser.UserId == userOff.UserId)
                        {
                            //tính tổng số time làm việc 
                            item.TotalTime -= userOff.TotalDayOff * 8;
                        }
                    }
                    //tính tổng số time Estimate
                    foreach (var time in listTaskInSprint)
                    {
                        if (projUser.UserId == time.UserId)
                        {
                            item.TotalEstimatedTime += (double)time.EstimatedTime;
                        }
                    }
                    res.Add(item);
                }
            }
            return Ok(res);
        }
    }
}