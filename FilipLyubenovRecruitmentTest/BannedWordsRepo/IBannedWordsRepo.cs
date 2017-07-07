using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BannedWordsRepo
{
    public interface IBannedWordsRepo
    {
        List<string> GetBannedWordsList();
    }
}
