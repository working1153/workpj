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
    public class VocstacksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VocstacksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Vocstacks
        public async Task<IActionResult> Index()
        {
              return View(await _context.Vocstack.ToListAsync());
        }

        // GET: Vocstacks/ToVocs
        //go to current vocabulary stack
        public async Task<IActionResult> ToVocs(int? StackId)
        {

            return RedirectToAction("Index2", "Vocs", new { StackId = StackId });
        }


        // GET: Vocstacks/Quest
   
        public async Task<IActionResult> Quest(int? StackId)
        {
            ViewBag.StackId = StackId;
            List<Voc> vc = await _context.Voc.Where(i => i.StackId == StackId).OrderBy(x => x.Count).Take(10).ToListAsync();
            List<string> se = new List<string>();
            for(int i = 0; i < 10; i++)
            {
                string vocabulary = vc[i].Vocabulary;
                string sentence = vc[i].Sentence;
                string r = ""; //replace 
                for(int j = 0; j < vocabulary.Length; j++)
                {
                    if(j == 0 || j == vocabulary.Length-1 || j == vocabulary.Length - 2)
                    {
                        r += vocabulary[j];
                    } else
                    {
                        r += "_";
                    }
                }
                sentence = sentence.Replace(vocabulary, r);
                se.Add(sentence);
            }
            ViewData["Sentence"] = se;
            return View(vc);
        }



        // GET: Vocstacks/Getans
        [HttpPost]
        public async Task<IActionResult> Getans(string[] ans, int[] id)
        {
            List<Voc> voc = await _context.Voc.ToListAsync();
            //AddVoca c = v.Where(j => j.Id == id[0]).FirstOrDefault();
            Voc v;
            List<Voc> Right = new List<Voc>();
            List<Voc> Wrong = new List<Voc>();
            var score = 10;

            for (var i = 0; i < ans.Length; i++)
            {
                v = voc.Where(j => j.Id == id[i]).FirstOrDefault(); //get right answer
                if (v.Vocabulary.Equals(ans[i])) //if input == right answer 
                {
                    Right.Add(v); //push right
                    v.Count += 1; //count++
                    await _context.SaveChangesAsync();
                }
                else
                {
                    score--;
                    Wrong.Add(v);
                }
            }
            ViewData["Right"] = Right;
            ViewData["Wrong"] = Wrong;
            ViewBag.Score = score;


            v = voc.Where(j => j.Id == id[0]).FirstOrDefault();
            //add new score to scoreboard
            ScBoard scb = new ScBoard();
            scb.Score = score;
            scb.StackId = v.StackId;
            scb.Account = "Guest";
            if(User.Identity.Name != null)
            {
                scb.Account = User.Identity.Name;
            }
            _context.Add(scb);
            await _context.SaveChangesAsync();
            ViewBag.StackId = v.StackId;
            return View();
        }
        // GET: Vocstacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Vocstack == null)
            {
                return NotFound();
            }

            var vocstack = await _context.Vocstack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocstack == null)
            {
                return NotFound();
            }

            return View(vocstack);
        }

        // GET: Vocstacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Vocstacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StackName,NumOfVoc")] Vocstack vocstack)
        {
            if (ModelState.IsValid)
            {
                vocstack.NumOfVoc = 0;
                _context.Add(vocstack);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vocstack);
        }

        // GET: Vocstacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Vocstack == null)
            {
                return NotFound();
            }

            var vocstack = await _context.Vocstack.FindAsync(id);
            if (vocstack == null)
            {
                return NotFound();
            }
            return View(vocstack);
        }

        // POST: Vocstacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StackName,NumOfVoc")] Vocstack vocstack)
        {
            if (id != vocstack.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vocstack);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VocstackExists(vocstack.Id))
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
            return View(vocstack);
        }

        // GET: Vocstacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Vocstack == null)
            {
                return NotFound();
            }

            var vocstack = await _context.Vocstack
                .FirstOrDefaultAsync(m => m.Id == id);
            if (vocstack == null)
            {
                return NotFound();
            }

            return View(vocstack);
        }

        // POST: Vocstacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Vocstack == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Vocstack'  is null.");
            }
            var vocstack = await _context.Vocstack.FindAsync(id);
            if (vocstack != null)
            {
                _context.Vocstack.Remove(vocstack);
            }
            
            await _context.SaveChangesAsync();

            //刪除相同單字庫的所有單字
            var voc = await _context.Voc.Where(i => i.StackId == id).ToListAsync();
            if (voc != null)
            {
                _context.Voc.RemoveRange(voc);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool VocstackExists(int id)
        {
          return _context.Vocstack.Any(e => e.Id == id);
        }
    }
}
