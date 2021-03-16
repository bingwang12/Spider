﻿using System;
using System.Xml.Linq;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace Cdy.Spider
{
    public class OpcDriver : TimerDriverRunner
    {

        #region ... Variables  ...
        private OpcDriverData mData;
        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...
        
        /// <summary>
        /// 
        /// </summary>
        public override string TypeName => "OpcDriver";

        /// <summary>
        /// 
        /// </summary>
        public override DriverData Data => mData;

        #endregion ...Properties...

        #region ... Methods    ...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mComm"></param>
        public override void RegistorReceiveCallBack(ICommChannel mComm)
        {
            mComm.RegistorReceiveCallBack(this.OnReceiveData2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected override object OnReceiveData2(string key, object data, out bool handled)
        {
            this.UpdateValue(key, data);
            handled = true;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Prepare()
        {
            if(this.Data.Model == WorkMode.Passivity)
            {
                this.mComm.Prepare(this.mCachTags.Keys.ToList());
            }
            base.Prepare();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public override void WriteValue(string deviceInfo, object value, byte valueType)
        {
            mComm.SendAndWait(deviceInfo, value, 1);
            base.WriteValue(deviceInfo, value, valueType);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ProcessTimerElapsed()
        {
            int count = this.mCachTags.Count / 100;
            count = this.mCachTags.Count % 100 > 0 ? count + 1 : count;

            for (int i = 0; i < count; i++)
            {
                int icount = (i + 1) * 100;
                if (icount > this.mCachTags.Count)
                {
                    icount = this.mCachTags.Count - i * 100;
                }
                var vkeys = this.mCachTags.Keys.Skip(i * 100).Take(icount);
                SendGroupTags(vkeys.ToList());
                Thread.Sleep(10);
            }

            base.ProcessTimerElapsed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tags"></param>
        private void SendGroupTags(List<string> tags)
        {
            var result = mComm.SendAndWait("", tags);
            if(result!=null)
            {
                var irest = result as IEnumerable<object>;
                if(tags.Count== irest.Count())
                {
                    int i = 0;
                    foreach(var vv in irest)
                    {
                        UpdateValue(tags[i], vv);
                        i++;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDriverRuntime NewApi()
        {
            return new OpcDriver();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public override void Load(XElement xe)
        {
            mData = new OpcDriverData();
            mData.LoadFromXML(xe);
            base.Load(xe);
        }


        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...



    }
}