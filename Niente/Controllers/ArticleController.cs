using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Niente.Models.ArticleViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

using Niente.Data;
using Niente.Common;
using Niente.Models;

namespace Niente.Controllers
{
    [Produces("application/json")]
    [Route("api/Articles")]
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger _logger;

        public ArticleController(ApplicationDbContext context,
                                     IHttpContextAccessor accessor,
                                     ILogger<ArticleController> logger)
        {
            _context = context;
            _accessor = accessor;
            _logger = logger;
        }

        // GET: api/Articles
        [HttpGet]
        public IEnumerable<Article> GetArticle()
        {
            _logger.LogInformation($"GET request: all articles");
            _logger.LogInformation($"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}");

            _logger.LogInformation($"Result: {_context.Articles.Count()} articles has been sent to the client");

            return _context.Articles;
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetArticle([FromRoute] int id)
        {
            string log = $"GET request: article, id={id}" + Environment.NewLine +
                         $"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}" + Environment.NewLine;

            if (!ModelState.IsValid)
            {
                log += "Bad model state";
                _logger.LogError(log);

                return BadRequest(ModelState);
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);

            if (article == null)
            {
                log += "Result: entry not found";
                _logger.LogInformation(log);
                return NotFound();
            }

            log += "Result: entry found, article has been sent to the client";
            _logger.LogInformation(log);
            return Ok(article);
        }

        // GET api/articlepreviews
        [HttpGet]
        [Route("~/api/articlepreviews")]
        public async Task<IActionResult> GetPreview([FromQuery]int limit = 5)
        {
            _logger.LogInformation($"Getting {limit} article previews...");
            _logger.LogInformation($"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}");

            var previews = await _context.Articles
                .Where(a => a.DisplayLevel == DisplayLevel.Default && a.Status == Status.Visible)
                .OrderBy(a => a.Id)
                .Take(limit)
                .Select(a => ToPreview(a))
                .ToArrayAsync();

            _logger.LogInformation($"Sent {previews.Count()} article previews to the client");

            return Ok(previews);
        }


        // PUT: api/Articles/5
        [HttpPut("{id}")]
        //[Authorize]
        public async Task<IActionResult> PutArticle([FromRoute] int id, [FromBody] ArticleEditViewModel article)
        {
            string log = $"PUT request: article, id={id}" + Environment.NewLine +
                         $"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}" + Environment.NewLine;

            if (!ModelState.IsValid)
            {
                log += "Bad model state";
                _logger.LogError(log);

                return BadRequest(ModelState);
            }

            var target_article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);

            var entry = new Article
            {
                Id = target_article.Id,
                Title = string.IsNullOrWhiteSpace(article.Title) ? target_article.Title : article.Title,
                Body = string.IsNullOrWhiteSpace(article.Body) ? target_article.Body : article.Body,
                PreviewText = string.IsNullOrWhiteSpace(article.PreviewText) ? target_article.PreviewText : article.PreviewText,
                PreviewImageUri = string.IsNullOrWhiteSpace(article.PreviewImageUri) ? target_article.PreviewImageUri : article.PreviewImageUri,
                CreateAt = target_article.CreateAt,
                LastEditAt = DateTime.Now
            };

            _context.DetachLocalInstance<Article>(id);
            _context.Entry(entry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    log += "The article does not exist";
                    _logger.LogWarning(log);

                    return NotFound();
                }
                else
                {
                    log += "The database is busy";
                    _logger.LogWarning(log);

                    throw;
                }
            }

            log += "Result: article has been updated";
            _logger.LogInformation(log);

            return NoContent();
        }

        // POST: api/Articles
        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> PostArticle([FromBody] ArticlePostViewModel article)
        {
            string log = $"POST request: article" + Environment.NewLine +
                         $"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}" + Environment.NewLine +
                         $"Title: {article.Title}" + Environment.NewLine;

            if (!ModelState.IsValid)
            {
                log += $"Error: bad model state, {ModelState.Values.First().Errors.First().ErrorMessage}";
                _logger.LogError(log);

                return BadRequest(ModelState);
            }

            var entry = new Article
            {
                Title = article.Title,
                Body = article.Body,
                PreviewText = article.PreviewText,
                PreviewImageUri = article.PreviewImageUri,
                CreateAt = DateTime.Now,
                LastEditAt = DateTime.Now
            };

            _context.Articles.Add(entry);
            await _context.SaveChangesAsync();

            log += "Result: article has been saved in the database";
            _logger.LogInformation(log);

            return CreatedAtAction("PostArticle", new { id = entry.Id }, entry);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteArticle([FromRoute] int id)
        {
            string log = $"DELETE request: article id={id}" + Environment.NewLine +
                         $"From IP: {_accessor.HttpContext.Connection.RemoteIpAddress.ToString()}" + Environment.NewLine;

            if (!ModelState.IsValid)
            {
                log += "Error: bad model state";
                _logger.LogError(log);

                return BadRequest(ModelState);
            }

            var article = await _context.Articles.SingleOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                log += "Result: not found";
                _logger.LogInformation(log);

                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            log += "Result: entry found, article has been deleted from the database";
            _logger.LogInformation(log);

            return Ok(article);
        }

        #region Helpers
        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }

        private ArticlePreviewViewModel ToPreview(Article article)
        {
            return new ArticlePreviewViewModel
            {
                Title = article.Title,
                Id = article.Id,
                CreateAt = article.CreateAt,
                LastEditAt = article.LastEditAt,
                PreviewImageUri = article.PreviewImageUri,
                PreviewText = article.PreviewText
            };
        }
        #endregion
    }
}
