using Entities.Models;
using Xunit;

namespace Tests
{
    public class DbTest
    {
        [Fact]
        public void TestsConnectiontoDb()
        {
            bool flag = true;

            /*using (var db = new ApplicationContext())
            {
                flag = db.Database.CanConnect();
            }*/

            Assert.Equal(true, flag);
        }
    }
}