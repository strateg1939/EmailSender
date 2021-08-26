﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailSender.Models;
using Microsoft.AspNetCore.Authorization;
using EmailSender.Services;

namespace EmailSender.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public ArticlesController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Articles
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Articles.Include(article => article.Topic).ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.Include(m => m.Topic)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewBag.Topic = new SelectList(_context.Topics.ToList(), "TopicId", "Topic_name");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,TopicID,date,Article_text")] Article article)
        {
            if (ModelState.IsValid)
            {
                _context.Add(article);
                await _context.SaveChangesAsync();
                if (article.date.Date == DateTime.Today)
                {
                    var usersToSend = _context.connection_user_topic.Where(con => con.TopicID == article.TopicID).Include(con => con.AspNetUser).Select(con => new { con.AspNetUserID, con.AspNetUser.Email});
                    string subject = "New article about " + _context.Topics.FirstOrDefault(topic => topic.TopicId == article.TopicID).Topic_name;
                    foreach (var user in usersToSend) {
                        var aspNetUser = new AspNetUser { Id = user.AspNetUserID, Email = user.Email };
                        await _emailService.SendArticleEmail(article, aspNetUser, subject);
                    }
                }                
                return RedirectToAction(nameof(Index));
            }
            return View(article);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewBag.Topic = new SelectList(_context.Topics.ToList(), "TopicId", "Topic_name");
            return View(article);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,TopicID,date,Article_text")] Article article)
        {
            if (id != article.ArticleId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.ArticleId))
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
            return View(article);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _context.Articles
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.ArticleId == id);
        }
    }
}
