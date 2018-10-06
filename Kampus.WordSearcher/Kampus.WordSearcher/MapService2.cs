using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kampus.WordSearcher
{
    class MapService2
    {
        Helper helpClass = new Helper();
        Matrix startMap = new Matrix();
        WordService wordService = new WordService();
        MatrService matrService = new MatrService();
        public void Run(Helper helperes)
        {
            helpClass = helperes;
            Matrix startMap = new Matrix();
            StartPosition startPosition = new StartPosition();
            startPosition.Run(helpClass);
            startPosition.GetFirstPartLeter(startMap, BasePatch.start);
            for (int i = 0; i < 13; i++)
            {
                helpClass.ClientHttp.SendRequest(BasePatch.left);
            }
            for (int i = 0; i < 4; i++)
            {
                helpClass.ClientHttp.SendRequest(BasePatch.down);
            }
            startMap=startPosition.GetFirstPartLeter(startMap, BasePatch.down);
        }
        public void GetFirstLine()
        {



        }

    }
}
