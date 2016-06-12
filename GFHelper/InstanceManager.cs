using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GFHelper
{

    class InstanceManager
    {
        public Listener listener;
        public UIHelper uiHelper;
        public ServerHelper serverHelper;
        public Timer timer;
        public DataHelper dataHelper;
        public MainWindow mainWindow;
        public AutoOperation autoOperation;
        public Logger logger;

        public InstanceManager(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;

            this.timer = new Timer(this);
            this.uiHelper = new UIHelper(this);
            this.serverHelper = new ServerHelper(this);
            this.dataHelper = new DataHelper(this);
            this.listener = new Listener(this);
            this.autoOperation = new AutoOperation(this);
            this.logger = new Logger(this);
        }
    }
}
