using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 玩家的一些临时属性,
/// </summary>
namespace ServerFramework.DAO
{
    class UserTempData
    {
        public UserTempData()
        {
            status = Status.None;
        }
        //状态
        public enum Status
        {
            None,
            Room,
            Fight,
        }
        public Status status;
        //room状态
        //public Room room;
    }
}
