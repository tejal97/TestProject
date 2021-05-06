using TestApplication.Domain.Marksheet;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TestApplication.Domain.StudentModels;
using System.Collections.Generic;
using System;
using System.Linq;
using TestApplication.Data.Enums;
using TestApplication.Data.Service.Mail;
using TestApplication.Data.Domain.Email;

namespace TestApplication.Controllers
{
    public class MarksheetController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IMailService _mailService;

        public MarksheetController(ApplicationDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        public IActionResult Index(string message = "")
        {
            return View(new MarksheetIndexModel() { Message = message });
        }
        [HttpGet]
        public async Task<IActionResult> SearchMarksheet(string stid)
        {
            var Student = await _context.Student.Where(student => student.StudentId == stid).FirstOrDefaultAsync();
            var Subject = await _context.Subjects.Where(subject => subject.StudentId == Student.StudentId && subject.Grade == Student.Grade).FirstOrDefaultAsync();
            if (Subject == null)
            {
                return RedirectToAction(nameof(Index), new { message = "Marksheet Not Found" });
            }
            var model = new StudentViewModel()
            {
                Student = Student,
                Subject = Subject
            };
            return View("StudentMarksheet", model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchMarksheet(MarksheetIndexModel marksheetDetails)
        {
            var Student = await _context.Student.Where(student => student.StudentId == marksheetDetails.Student).FirstOrDefaultAsync();
            var Subject = await _context.Subjects.Where(subject => subject.StudentId == marksheetDetails.Student && subject.Grade == Student.Grade).FirstOrDefaultAsync();
            if (Subject == null)
            {
                return RedirectToAction(nameof(Index), new { message = "Marksheet Not Found" });
            }
            var model = new StudentViewModel()
            {
                Student = Student,
                Subject = Subject
            };
            return View("StudentMarksheet", model);
        }
        [HttpPost]
        public async Task<IActionResult> SearchToppers(MarksheetIndexModel marksheetDetails)
        {
            var studentList = await _context.Student.Where(student => student.Grade == marksheetDetails.Grade).ToListAsync();
            var topStudentResult = (from subject in await _context.Subjects.ToListAsync()
                                    join student in studentList
                                    on new { id = subject.StudentId, grade = subject.Grade } equals new { id = student.StudentId, grade = student.Grade }
                                    orderby subject.Percentage descending
                                    where
                                      subject.Maths >= StaticDataValues.PassMarks &&
                                      subject.Social >= StaticDataValues.PassMarks &&
                                      subject.Science >= StaticDataValues.PassMarks &&
                                      subject.English >= StaticDataValues.PassMarks &&
                                      subject.Nepali >= StaticDataValues.PassMarks
                                    select new StudentRankedListModel() { StudentId = student.StudentId, StudentName = student.StudentName, Percentage = subject.Percentage.ToString(), TotalMarks = subject.TotalMarks.ToString() })
                                    .Take(3).ToList();

            return View("RankedStudentList", topStudentResult);
        }
        public async Task<IActionResult> SearchFailedStudents(MarksheetIndexModel marksheetDetails)
        {
            var studentList = await _context.Student.Where(student => student.Grade == marksheetDetails.Grade).ToListAsync();
            var failedStudentResult = (from subject in await _context.Subjects.ToListAsync()
                                       join student in studentList
                                       on new { id = subject.StudentId, grade = subject.Grade } equals new { id = student.StudentId, grade = student.Grade }
                                       orderby subject.Percentage descending
                                       where
                                       subject.Maths < StaticDataValues.PassMarks ||
                                       subject.Social < StaticDataValues.PassMarks ||
                                       subject.Science < StaticDataValues.PassMarks ||
                                       subject.English < StaticDataValues.PassMarks ||
                                       subject.Nepali < StaticDataValues.PassMarks
                                       select new FailedStudentModel() { StudentId = student.StudentId, StudentName = student.StudentName, Percentage = subject.Percentage.ToString() })

                                    .ToList();
            if (failedStudentResult.Count <= 0)
            {
                return RedirectToAction(nameof(Index), new { message = "No Students have failed for this grade." });
            }
            return View("FailedStudentList", failedStudentResult);
        }
        public async Task<IActionResult> GetStudentsByGrade(string id)
        {
            var studentList = await _context.Student.Where(item => item.Grade == id).ToListAsync();
            return Json(studentList);
        }
        public IActionResult Create()
        {
            return View(new StudentViewModel());
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentViewModel studentViewModel)
        {
            if (_context.Subjects.Where(item => item.Grade == studentViewModel.Subject.Grade && item.StudentId == studentViewModel.Subject.StudentId).Any())
            {
                var subjectEntity = await _context.Subjects.Where(item => item.Grade == studentViewModel.Subject.Grade && item.StudentId == studentViewModel.Subject.StudentId).FirstOrDefaultAsync();
                _context.Subjects.Remove(subjectEntity);
                await _context.SaveChangesAsync();
            }
            studentViewModel.Subject.SubjectId = Guid.NewGuid().ToString();
            studentViewModel.Subject.Percentage = StaticDataFunctions.CalculatePercentage(studentViewModel.Subject);
            studentViewModel.Subject.TotalMarks = StaticDataFunctions.CalculateTotal(studentViewModel.Subject);
            _context.Add(studentViewModel.Subject);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> MailMarkSheet(string id)
        {
            try
            {
                var Student = await _context.Student.Where(student => student.StudentId == id).FirstOrDefaultAsync();
                var Subject = await _context.Subjects.Where(subject => subject.StudentId == id && subject.Grade == Student.Grade).FirstOrDefaultAsync();
                if (Subject == null)
                {
                    return RedirectToAction(nameof(Index), new { message = "Marksheet Not Found" });
                }
                var studentDetails = new StudentViewModel()
                {
                    Student = Student,
                    Subject = Subject
                };
                var request = new MailRequest();
                request.ToEmail = "97tejal@gmail.com";
                request.Subject = $"Marksheet for {studentDetails.Student.StudentName}";

                request.Body = $" <table width=\"100 % \"><tr><th colspan = \"5\"> Student Details </th> </tr><tr><td colspan = \"2\"> Student Name </td><td colspan = \"3\"> {studentDetails.Student.StudentName} </td> </tr> <tr> <td colspan = \"2\"> Grade </td><td colspan = \"3\"> {studentDetails.Student.Grade }</td></tr><tr> <td colspan = \"2\"> Address </td> <td colspan = \"3\"> {studentDetails.Student.Address }</td> </tr>  <tr>  <th colspan = \"5\"> Marks Details </th>  </tr>  <tr style = \"background-color:#E6E6E6\">  <td> SN </td>  <td> Subject </td>  <td> Full Marks </td>  <td> Pass Marks </td>  <td> Obtained Marks </td>  </tr>  <tr>  <td> 1 </td>  <td> Maths </td><td>{StaticDataValues.FullMarks }</td>  <td> { StaticDataValues.PassMarks} </td> ";
                if (studentDetails.Subject.Maths < StaticDataValues.PassMarks)
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\" style=\"background-color: pink;\"> <u> {studentDetails.Subject.Maths} </u> </td>";
                }
                else
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\"> <u> {studentDetails.Subject.Maths} </u> </td>";
                }
                request.Body = request.Body + $" </tr>  <tr>  <td> 2 </td>  <td> English </td>  <td>{ StaticDataValues.FullMarks}</td>  <td>{ StaticDataValues.PassMarks}</td>";
                if (studentDetails.Subject.English < StaticDataValues.PassMarks)
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\" style=\"background-color: pink;\"> <u> {studentDetails.Subject.English} </u> </td>";
                }
                else
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\"> <u> {studentDetails.Subject.English} </u> </td>";
                }
                request.Body = request.Body + $"</tr> <tr> <td> 3 </td> <td> Nepali</td> <td> {StaticDataValues.FullMarks }</td> <td> {StaticDataValues.PassMarks }</td>";

                if (studentDetails.Subject.Nepali < StaticDataValues.PassMarks)
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\" style=\"background-color: pink;\"> <u> {studentDetails.Subject.Nepali} </u> </td>";
                }
                else
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\"> <u> {studentDetails.Subject.Nepali} </u> </td>";
                }
                request.Body = request.Body + $" </tr> <tr> <td> 4 </td> <td> Social </td> <td> {StaticDataValues.FullMarks }</td> <td> {StaticDataValues.PassMarks }</td> ";
                if (studentDetails.Subject.Social < StaticDataValues.PassMarks)
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\" style=\"background-color: pink;\"> <u> {studentDetails.Subject.Social} </u> </td>";
                }
                else
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\"> <u> {studentDetails.Subject.Social} </u> </td>";
                }
                request.Body = request.Body + $"</tr> <tr> <td> 5 </td> <td> Maths </td> <td> {StaticDataValues.FullMarks }</td> <td> {StaticDataValues.PassMarks }</td> ";
                if (studentDetails.Subject.Maths < StaticDataValues.PassMarks)
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\" style=\"background-color: pink;\"> <u> {studentDetails.Subject.Maths} </u> </td>";
                }
                else
                {
                    request.Body = request.Body + $" <td class= \"subjectTotal\"> <u> {studentDetails.Subject.Maths} </u> </td>";
                }
                request.Body = request.Body + $"</tr> <tr> <td colspan = \"2\"> Grand Total </td>  <td>{(StaticDataValues.FullMarks * 5)} </td>  <td>{ (StaticDataValues.PassMarks * 5) }</td>  <td id = \"GrandTotal\"> {studentDetails.Subject.TotalMarks }</td>  </tr>  <tr>  <td colspan = \"3\"> Percentage </td>  <td id = \"Percentage\"> {studentDetails.Subject.Percentage} %</td>  </tr>  </table> ";
                await _mailService.SendEmailAsync(request);
                return RedirectToAction(nameof(Index), new { message = "Mail Sent Sucessfully" });
            }
            catch (Exception ex)
            {
                return RedirectToAction(nameof(Index), new { message = "Mail sending failed" });
            }
        }
    }
}
