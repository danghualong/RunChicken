using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunChicken.models
{
    public class Player
    {
        public string PlayerName { get; set; }
        public int Lives { get; set; }

        public int Position { get; set; }

        public string Avatar { get; set; }

        public bool IsOut { get; set; }
        /// <summary>
        /// 是否被其他玩家超越
        /// </summary>
        public bool IsExceeded { get; set; }

    }
}
