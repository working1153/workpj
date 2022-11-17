using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp2.Data;
using MyWebApp2.Models;

namespace MyWebApp2.Controllers
{
    public class VocsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vocs
        public async Task<IActionResult> Index()
        {
            ViewBag.StackName = "No StackName";
            return View(await _context.Voc.ToListAsync());
        }


        // GET: Vocs/Index2 
        public async Task<IActionResult> Index2(int? StackId)
        {
            ViewBag.StackName = "No StackName";
            if (StackId != null)
            {
                Vocstack vocstack = await _context.Vocstack.FirstOrDefaultAsync(i => i.Id == StackId);

                ViewBag.StackName = vocstack.StackName;
                ViewBag.StackId = StackId; //stack id 
                return View("Index", await _context.Voc.Where(i => i.StackId == StackId).ToListAsync());
            }
            return View("Index", await _context.Voc.ToListAsync());
        }

        // GET: Vocs/ToQuest 從單字到考題
        public async Task<IActionResult> ToQuest(int? StackId)
        {
            //todo if id == null
            if(StackId == null)
            {
                return View("Index", await _context.Voc.ToListAsync());
            } 
            return RedirectToAction("Quest", "Vocstacks", new {StackId = StackId});
        }
        // GET: Vocs/ToVocstck 返回單字庫列表
        public async Task<IActionResult> ToVocstack()
        {

            return RedirectToAction("Index", "Vocstacks");
        }



        // GET: Vocs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Voc == null)
            {
                return NotFound();
            }

            var voc = await _context.Voc
                .FirstOrDefaultAsync(m => m.Id == id);
            if (voc == null)
            {
                return NotFound();
            }

            return View(voc);
        }

        // GET: Vocs/Create
        public IActionResult Create(int? StackId)
        {
  
            ViewBag.StackNmae = "No stack name";
            if(StackId != null)
            {
                ViewBag.StackId = StackId;
                Vocstack vocstack = _context.Vocstack.FirstOrDefault(i => i.Id == StackId);
                ViewBag.StackName = vocstack.StackName;
            } else
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Vocs/Create2file
        public IActionResult Create2file(int? StackId)
        {

            ViewBag.StackNmae = "No stack name";
            if (StackId != null)
            {
                ViewBag.StackId = StackId;
                Vocstack vocstack = _context.Vocstack.FirstOrDefault(i => i.Id == StackId);
                ViewBag.StackName = vocstack.StackName;
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // POST: Vocs/Create2fileConfirm
        [HttpPost]
        public async Task<IActionResult> Create2fileConfirm(int StackId, IFormFile? myfile)
        {
            /*
            if (myfile == null)
            {
                ViewBag.work = "No file";
            } else
            {
                ViewBag.work = "Upload success";
                ViewBag.fileLength = "Length: " + myfile.Length;
                ViewBag.header = "Headers: " + myfile.Headers;
                ViewBag.ContentDisposition = "ContentDisposition: " + myfile.ContentDisposition;
                ViewBag.ContentType = "ContentType : " + myfile.ContentType;
            }
            */
            bool filevalid = true;
            if(myfile == null)
            {
                filevalid = false;
            } else
            {
                if(myfile.Length > 5242880 || myfile.ContentType != "text/plain")
                {
                    filevalid = false;
                }
            }
            

            if(filevalid)
            {
                string s = "";
                
                using (StreamReader sr = new StreamReader(myfile.OpenReadStream()))
                {
                    string line;
                    // Read and display lines from the file until the end of
                    // the file is reached.
                    while ((line = sr.ReadLine()) != null)
                    {
                        
                        
                        try
                        {
                           
                            string[] st = line.Split('|');
                            if (st.Count() != 4)
                            {
                                filevalid =false;
                                break;
                            }
                            Voc vc = new Voc();
                            vc.StackId = StackId;
                            vc.Count = 0;
                            vc.Vocabulary = st[0];
                            vc.PartOfSpeeach = st[1];
                            vc.Meaning = st[2];
                            vc.Sentence = st[3];
                            _context.Add(vc);
                            await _context.SaveChangesAsync();
                            s += "";
                        }
                        catch (InvalidCastException e)
                        {
                            filevalid = false;
                            break;
                        }
                            
                    }
                }
                ViewBag.Content = s;
            }

            if (filevalid)
            {
                ViewBag.Message = "上傳成功";
            }
            else
            {
                ViewBag.Message = "上傳失敗";
            }
            ViewBag.isvalid = filevalid;
            ViewBag.StackId = StackId;
            //update vocstack count
            Vocstack vocstack = await _context.Vocstack.FirstOrDefaultAsync(i => i.Id == StackId);
            vocstack.NumOfVoc = _context.Voc.Where(i => i.StackId == StackId).Count();
            await _context.SaveChangesAsync();
            return View();
        }


        // POST: Vocs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Vocabulary,PartOfSpeeach,Meaning,Sentence,StackId,Count")] Voc voc)
        {
            if (ModelState.IsValid)
            {
                voc.Count = 0;
                

                _context.Add(voc);
                await _context.SaveChangesAsync();

                ///update numofvoc
                Vocstack vocstack = await _context.Vocstack.FirstOrDefaultAsync(i => i.Id == voc.StackId);
                vocstack.NumOfVoc = _context.Voc.Where(i => i.StackId == voc.StackId).Count();
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index2), new {StackId = voc.StackId});
            }
            return View(voc);
        }

        // GET: Vocs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Voc == null)
            {
                return NotFound();
            }

            var voc = await _context.Voc.FindAsync(id);
            ViewBag.StackId = voc.StackId;
            if (voc == null)
            {
                return NotFound();
            }
            return View(voc);
        }

        // POST: Vocs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Vocabulary,PartOfSpeeach,Meaning,Sentence,StackId,Count")] Voc voc)
        {
            if (id != voc.Id)
            {
                return NotFound();
            }
            var sid = voc.StackId;
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(voc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocExists(voc.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index2), new {StackId = sid});
            }
            return View(voc);
        }

        // GET: Vocs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            
            if (id == null || _context.Voc == null)
            {
                return NotFound();
            }

            var voc = await _context.Voc
                .FirstOrDefaultAsync(m => m.Id == id);

            //get stackid
            ViewBag.StackId = voc.StackId;

            if (voc == null)
            {
                return NotFound();
            }

            return View(voc);
        }

        // POST: Vocs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Voc == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Voc'  is null.");
            }
            var voc = await _context.Voc.FindAsync(id);
            int sid = voc.StackId;

            if (voc != null)
            {
                _context.Voc.Remove(voc);
            }
            
            await _context.SaveChangesAsync();

            //update numofv
            Vocstack vocstack = await _context.Vocstack.FirstOrDefaultAsync(i => i.Id == sid);
            vocstack.NumOfVoc = _context.Voc.Where(i => i.StackId == sid).Count();
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index2), new {StackId = sid});
        }

        private bool VocExists(int id)
        {
          return _context.Voc.Any(e => e.Id == id);
        }
    }
}
