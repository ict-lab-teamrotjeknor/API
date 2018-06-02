using System.Net;

namespace ScheduleCrawler
{
    public class Download
    {
        public string GetHTMLPageSchedule()
        {
            var htmlSchedule = string.Empty;
            
            using (WebClient client = new WebClient()) 
            {
                htmlSchedule = client.DownloadString("http://misc.hro.nl/roosterdienst/webroosters/CMI/kw4/22/r/r00019.htm");
            }

            return htmlSchedule;
        }
    }
}