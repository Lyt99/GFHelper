using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace GFHelper.Models
{
    class AutoOperationInfo
    {
        public AutoOperationInfo(int teamid, int operationid)
        {
            this._textLastTime = new TextBlock();
            this._teamId = teamid;
            this._operationId = operationid;
            SetDefaultLastTime();
        }

        public void SetDefaultLastTime()
        {
            this.LastTime = Data.operationInfo[_operationId].duration;
        }

        public TextBlock getTextBlock()
        {
            return this._textLastTime;
        }

        public string OperationName
        {
            get
            {
                return Data.operationInfo[this._operationId].name;
            }
        }

        public string TeamName
        {
            get
            {
                return String.Format("梯队{0}({1})", this._teamId, Data.gunInfo[Data.teamInfo[this._teamId][1].GunID].name);
            }
        }


        public string TextLastTime
        {
            get
            {
                return this._textLastTime.Text;
            }
        }

        public int LastTime
        {
            get
            {
                return this._lastTime;
            }
            set
            {
                this._lastTime = value;
                this._textLastTime.Text = CommonHelper.formatDuration(value);
            }
        }




        public int _operationId;

        public int _teamId;

        private TextBlock _textLastTime;

        private int _lastTime;
            
    }
}
