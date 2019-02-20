using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Niente.Models;

namespace Niente.Data
{
    public static class RemoteSeedArticlesDatabase
    {
        public static async Task Seed(IConfiguration configuration, ILogger log)
        {
            const string databaseName = "Niente";
            const string collectionName = "Articles";

            // Create a CosmosDb client with endpoint URI and primary keys
            string endpointUri = configuration["CosmosDb:EndpointUri"];
            string primaryKey = configuration["CosmosDb:PrimaryKey"];
            DocumentClient client = new DocumentClient(new Uri(endpointUri), primaryKey); 

            // Create the database if not already existed
            await client.CreateDatabaseIfNotExistsAsync(new Database { Id = databaseName });

            // Create the Articles collection if not already existed
            await client.CreateDocumentCollectionIfNotExistsAsync(UriFactory.CreateDatabaseUri(databaseName),
                new DocumentCollection { Id = collectionName });

            await CreateAllArticleDocumentsIfNotExistsAsync();


            async Task CreateArticleDocumentIfNotExistsAsync(ArticleDocument article)
            {
                try
                {
                    await client.ReadDocumentAsync(UriFactory.CreateDocumentUri(databaseName, collectionName, article.Id.ToString()));
                    log.LogInformation($"Article document <<{article.Title}>> already created");
                }
                catch (DocumentClientException de)
                {
                    if (de.StatusCode == HttpStatusCode.NotFound)
                    {
                        await client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionName), article);
                        log.LogInformation($"Created article {article.Id}: {article.Title}");
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            async Task CreateAllArticleDocumentsIfNotExistsAsync()
            {
                ArticleDocument article1 = new ArticleDocument
                {
                    Id = 1,
                    Title = "ヒナまつり",
                    Subtitle = "黑社会的超能力女儿",
                    Body = "支持着芦川组的年轻智力型黑道新田义史，过着被喜欢的壶所包围的悠然自得单身生活。但某一天，随着装在神秘物体里的少女雏来到他家，他的生活为之一变。他被能使用念动力的雏所胁迫，迫不得已开始和她同居！\n" +
                                  "容易暴走的雏，不论在组里还是在学校都为所欲为。新田为此头痛不已，但由于自己那老好人的性格而陷入总是要照顾她的境地。究竟这种生活会变得怎样呢？\n" +
                                  "老好人不法之徒与任性超能力少女的危险而热闹的日常开始了！",
                    LastEditDate = DateTime.Now,
                    CreateAt = DateTime.Now
                };

                ArticleDocument article2 = new ArticleDocument
                {
                    Id = 2,
                    Title = "シュタインズ・ゲート ゼロ",
                    Subtitle = "命运石之门 0",
                    Body = "2010年11月β世界线——\n" +
                                      "主人公·冈部伦太郎跨越无数的苦难与悲哀，终究还是放弃拯救“她”的世界线。\n" +
                                      "跌落失意谷底的冈部伦太郎。担心着他的同伴们。\n" +
                                      "没能得到拯救的“她”将会变得如何？\n" +
                                      "迎来新的角色，描绘而成的“零”之物语。",
                    LastEditDate = DateTime.Now,
                    CreateAt = DateTime.Now
                };

                ArticleDocument article3 = new ArticleDocument
                {
                    Id = 3,
                    Title = "メガロボクス",
                    Subtitle = "Megalo Box",
                    Body = "将肉体与“齿轮技术”融合的究极格斗技——“MEGALO BOX”，将自己的全部赌在上面的男人们的热血战斗开始！\n" +
                                      "今天也立于未认可地区的赌博比赛赛场上的MEGALO拳击手“Junk Dog”。虽然具备实力，却只有靠比赛造假赚钱这一条生存之道，他为自己的“现在”感到心焦。\n" +
                                      "但，他与孤高的冠军·勇利相遇，作为MEGALO拳击手，作为男人，向自己的“现在”发起挑战——。",
                    LastEditDate = DateTime.Now,
                    CreateAt = DateTime.Now
                };

                await CreateArticleDocumentIfNotExistsAsync(article1);
                await CreateArticleDocumentIfNotExistsAsync(article2);
                await CreateArticleDocumentIfNotExistsAsync(article3);
            }
        }      
    }
}
