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
                htmlSchedule = client.DownloadString("http://misc.hro.nl/roosterdienst/webroosters/CMI/kw3/14/r/r00009.htm");
            }

            return htmlSchedule;
        }
    }
}