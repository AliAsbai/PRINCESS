/** authors:
 *          @Gabriel Vega
 *          
 *  App settings class contains properties defined in the appsettings.json file and is user for
 *  accessing application settings via objects that are injected into classes using the ASP.NET Core built in dependency
 *  injection system.
 **/

namespace PRINCESS.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
    }
}
