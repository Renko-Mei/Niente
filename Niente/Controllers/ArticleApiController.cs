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
    public class ArticleApiController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly ILogger _logger;

        public ArticleApiController(ApplicationDbContext context,
                                     IHttpContextAccessor accessor,
                                     ILogger<ArticleApiController> logger)
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

        // PUT: api/Articles/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutArticle([FromRoute] int id, [FromBody] EditArticleViewModel article)
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
                Body = string.IsNullOrWhiteSpace(article.Content) ? target_article.Body : article.Content,
                Subtitle = string.IsNullOrWhiteSpace(article.Subtitle) ? target_article.Subtitle : article.Subtitle,
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
        [Authorize]
        public async Task<IActionResult> PostArticle([FromBody] PostArticleViewModel article)
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
                Body = article.Content,
                Subtitle = article.Subtitle,
                CreateAt = DateTime.Now,
                LastEditAt = DateTime.Now
            };

            _context.Articles.Add(entry);
            await _context.SaveChangesAsync();

            log += "Result: article has been saved in the database";
            _logger.LogInformation(log);

            return CreatedAtAction("GetArticle", new { id = entry.Id }, entry);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        [Authorize]
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
        #endregion
    }
}
