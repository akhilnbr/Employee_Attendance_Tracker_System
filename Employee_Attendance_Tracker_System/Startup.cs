using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Employee_Attendance_Tracker_System.Startup))]
namespace Employee_Attendance_Tracker_System
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
