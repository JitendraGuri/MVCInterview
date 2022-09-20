using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVcInterview.Models.CandidateDetailsT;
using Microsoft.Extensions.Configuration;
using System.IO;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Hosting;
using static System.Net.WebRequestMethods;

namespace MVcInterview.Controllers
{
    public class CandidateDetailsTsController : Controller
    {
        private IConfiguration Configuration;
        private readonly JeetuContext _context;
        
        public CandidateDetailsTsController(JeetuContext context,IConfiguration _configuration)
        {
            Configuration = _configuration;
            _context = context;
        }

        // GET: CandidateDetailsTs
        public async Task<IActionResult> Index()
        {
              return View(await _context.CandidateDetailsTs.ToListAsync());
        }

        // GET: CandidateDetailsTs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }

            return View(candidateDetailsT);
        }

        // GET: CandidateDetailsTs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CandidateDetailsTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CandidateId,CandidateName,CandidateExperience,CandidatePhoneNo,CandidateMailId,CandidateSkills,CreatedDate")] CandidateDetailsT candidateDetailsT, List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                _context.Add(candidateDetailsT);
                await _context.SaveChangesAsync();
                

                if (files != null)
                {
                    //long size = files.Sum(f => f.Length);
                    string path = "";
                    var filePaths = new List<string>();
                    foreach (var formFile in files)
                    {
                        if (formFile.Length > 0)
                        {
                            //string filename = Path.GetExtension(formFile.FileName);
                            string filename =DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss").Replace(":","-").Replace(" ","_")+"_" + formFile.FileName;
                            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "ResumeFilesUploaded"));
                            using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                            {
                                await formFile.CopyToAsync(filestream);


                                string ConnStr = this.Configuration.GetConnectionString("DefaultConnection");
                                SqlConnection _con = new SqlConnection(ConnStr);
                                SqlCommand cmd = new SqlCommand("Usp_CandidateFiles", _con);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@candidateid", candidateDetailsT.CandidateId);
                                cmd.Parameters.AddWithValue("@CandidatefileName", filename);
                                cmd.Parameters.AddWithValue("@Candidatefilepath", path);
                                cmd.Parameters.AddWithValue("@Tran", "Save");
                                _con.Open();
                                cmd.ExecuteNonQuery();
                                _con.Close();
                            }
                        }
                    }
                }

                return RedirectToAction(nameof(Index));
            }
            // string id = TempData["CandidateId"].ToString();
            return View(candidateDetailsT);
            //return View();
        }
        [HttpPost]
        public async Task<IActionResult> CandidateUploadfile(List<IFormFile> files)
        {
            if (files != null)
            {
                //long size = files.Sum(f => f.Length);
                string path = "";
                var filePaths = new List<string>();
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        string filename = Guid.NewGuid() + Path.GetExtension(formFile.FileName);
                        path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "ResumeFilesUploaded"));
                        using (var filestream = new FileStream(Path.Combine(path, filename), FileMode.Create))
                        {
                            await formFile.CopyToAsync(filestream);

                        }
                    }
                }
            }
            return RedirectToAction(nameof(Index));
        }
            [HttpPost]
        public JsonResult Save(CandidateDetailsT _candetails)
        {
            string ConnStr = this.Configuration.GetConnectionString("DefaultConnection");
            SqlConnection _con = new SqlConnection(ConnStr);
            SqlCommand cmd = new SqlCommand("Usp_Candidate", _con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@candidateid", int.Parse("0"));
            cmd.Parameters.AddWithValue("@CandidateName", _candetails.CandidateName);
            cmd.Parameters.AddWithValue("@CandidateExp", _candetails.CandidateExperience);
            cmd.Parameters.AddWithValue("@CandidatePhoneNo", _candetails.CandidatePhoneNo);
            cmd.Parameters.AddWithValue("@CandidateMailid", _candetails.CandidateMailId);
            cmd.Parameters.AddWithValue("@CandidateSkills", _candetails.CandidateSkills);
            cmd.Parameters.AddWithValue("@Tran", "Save");
            cmd.Parameters.Add("@CanSaveid", SqlDbType.Int);
            cmd.Parameters["@CanSaveid"].Direction = ParameterDirection.Output;
            _con.Open();
            cmd.ExecuteNonQuery();
            _con.Close();
            string CandidateId =cmd.Parameters["@CanSaveid"].Value.ToString();
            //TempData["CandidateId"] = CandidateId;
            return Json(CandidateId, new Newtonsoft.Json.JsonSerializerSettings());
        }

        // GET: CandidateDetailsTs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs.FindAsync(id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }
            return View(candidateDetailsT);
        }

        // POST: CandidateDetailsTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,CandidateName,CandidateExperience,CandidatePhoneNo,CandidateMailId,CandidateSkills,CreatedDate")] CandidateDetailsT candidateDetailsT)
        {
            if (id != candidateDetailsT.CandidateId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(candidateDetailsT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateDetailsTExists(candidateDetailsT.CandidateId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(candidateDetailsT);
        }

        // GET: CandidateDetailsTs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CandidateDetailsTs == null)
            {
                return NotFound();
            }

            var candidateDetailsT = await _context.CandidateDetailsTs
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidateDetailsT == null)
            {
                return NotFound();
            }

            return View(candidateDetailsT);
        }

        // POST: CandidateDetailsTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.CandidateDetailsTs == null)
            {
                return Problem("Entity set 'JeetuContext.CandidateDetailsTs'  is null.");
            }
            var candidateDetailsT = await _context.CandidateDetailsTs.FindAsync(id);
            if (candidateDetailsT != null)
            {
                _context.CandidateDetailsTs.Remove(candidateDetailsT);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CandidateDetailsTExists(int id)
        {
          return _context.CandidateDetailsTs.Any(e => e.CandidateId == id);
        }
    }
}
