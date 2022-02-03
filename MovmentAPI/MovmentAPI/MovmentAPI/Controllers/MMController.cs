using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Xml;
using System.ServiceModel.Syndication;

namespace MovmentAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MMController : ControllerBase
    {

        #region GetRssPosts
        [Route("GetRssPosts")]
        [HttpPost]
        public List<Post> GetRssPosts([FromBody()] string sorce)
        {
            string url = "";
            List<Post> posts = new List<Post>();

            if (sorce == "any") // when the user choose any sorce
            {

                string[] sorces = new string[3] { "https://rss.walla.co.il/feed/1",
                                                  "http://www.ynet.co.il/Integration/StoryRss2.xml",
                                                  "https://www.maariv.co.il/Rss/RssChadashot" };

                string[] sorcesNames = new string[3] { "walla", "ynet", "maariv" };



                try
                {
                    for (int i = 0; i < sorces.Length; i++)
                    {
                        XmlReader reader = XmlReader.Create(sorces[i]);
                        SyndicationFeed feed = SyndicationFeed.Load(reader);
                        reader.Close();

                        foreach (SyndicationItem item in feed.Items)
                        {
                            if (item != null)
                                posts.Add(new Post(sorcesNames[i],
                                                   item.Title.Text.ToString(),
                                                   item.Links[0].Uri.ToString(),
                                                   item.PublishDate.UtcDateTime.ToString("dd/MM/yyyy HH:mm")));
                        }
                    }

                    posts = posts.OrderByDescending(x => x.Date).ToList();

                    return posts;
                }
                catch (Exception)
                {

                    throw;
                }

            }
            else // when the user choose ine sorce
            {


                switch (sorce)
                {
                    case "walla":
                        url = "https://rss.walla.co.il/feed/1";
                        break;

                    case "ynet":
                        url = "http://www.ynet.co.il/Integration/StoryRss2.xml";
                        break;

                    case "maariv":
                        url = "https://www.maariv.co.il/Rss/RssChadashot";
                        break;

                    default:
                        break;
                }

                try
                {


                    XmlReader reader = XmlReader.Create(url);
                    SyndicationFeed feed = SyndicationFeed.Load(reader);
                    reader.Close();

                    foreach (SyndicationItem item in feed.Items)
                    {
                        if (item != null)
                            posts.Add(new Post(sorce,
                                               item.Title.Text.ToString(),
                                               item.Links[0].Uri.ToString(),
                                               item.PublishDate.UtcDateTime.ToString("dd/MM/yyyy HH:mm")));
                    }


                    posts = posts.OrderByDescending(x => x.Date).ToList();

                    return posts;

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        #endregion
    }





    public class Post
    {
        public Post(
            string makor,
            string title,
            string link,
            string Date
          )
        {
            this.makor = makor;
            this.title = title;
            this.link = link;
            this.Date = Date;

        }

        public string makor { get; set; }
        public string title { get; set; }
        public string link { get; set; }
        public string Date { get; set; }

    }




}


