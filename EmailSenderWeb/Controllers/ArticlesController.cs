using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmailSender.Models;
using Microsoft.AspNetCore.Authorization;
using EmailSender.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using EmailSender.Dat;

namespace EmailSender.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ArticleEmailService _emailService;
        private readonly UserManager<AspNetUser> _userManager;

        public ArticlesController(IUnitOfWork unitOfWork, ArticleEmailService emailService, UserManager<AspNetUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
        }

        // GET: Articles
        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(_unitOfWork.ArticleRepository.GetArticlesWithTopics(GetCurrentUser().Id));
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = _unitOfWork.ArticleRepository.GetArticleWithTopic(id.Value);
            if (article == null || !CanUserEditArticle(article))
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewBag.Topic = new SelectList(_unitOfWork.TopicsRepository.GetAll(), "TopicId", "Topic_name");
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
                article.AspNetUser = GetCurrentUser();
                _unitOfWork.ArticleRepository.Add(article);
                await _unitOfWork.SaveChangesAsync();
                if (article.date.Date == DateTime.Today)
                {
                    var usersToSend = _unitOfWork.ArticleRepository.GetUsersToSend(article);
                    string subject = "New article about " + _unitOfWork.TopicsRepository.Get(article.TopicID).Topic_name;
                    foreach (var user in usersToSend) {
                        var aspNetUser = new AspNetUser { Id = user.Id, Email = user.Email };
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

            var article = _unitOfWork.ArticleRepository.Get(id.Value);
            if (article ==  null || !CanUserEditArticle(article))
            {
                return NotFound();
            }
            ViewBag.Topic = new SelectList(_unitOfWork.TopicsRepository.GetAll(), "TopicId", "Topic_name");
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
                    Article articleInDb = _unitOfWork.ArticleRepository.Get(id);
                    if (articleInDb.CreatorID != GetCurrentUser().Id)
                    {
                        return Forbid();
                    }
                    articleInDb.TopicID = article.TopicID;
                    articleInDb.date = article.date;
                    articleInDb.Article_text = article.Article_text;
                    await _unitOfWork.SaveChangesAsync();
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

            var article = _unitOfWork.ArticleRepository.Get(id.Value);
            if (article == null || !CanUserEditArticle(article))
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
            
            var article = _unitOfWork.ArticleRepository.Get(id);
            if (CanUserEditArticle(article))
            {
                _unitOfWork.ArticleRepository.Remove(article);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return Forbid();
        }

        private bool ArticleExists(int id)
        {
            return _unitOfWork.ArticleRepository.ArtcileExists(id);
        }
        private bool CanUserEditArticle(Article article)
        {
            return article.CreatorID == GetCurrentUser().Id;
        }
        private AspNetUser GetCurrentUser()
        {
            ClaimsPrincipal currentUser = this.User;
            return _userManager.GetUserAsync(currentUser).Result;
        }
    }
}
