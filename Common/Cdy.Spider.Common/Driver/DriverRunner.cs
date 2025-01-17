﻿//==============================================================
//  Copyright (C) 2020  Inc. All rights reserved.
//
//==============================================================
//  Create by 种道洋 at 2020/8/5 14:49:13.
//  Version 1.0
//  种道洋
//==============================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Cdy.Spider
{
    /// <summary>
    /// 驱动基类
    /// </summary>
    public abstract class DriverRunnerBase : IDriverRuntime, IDriverForFactory
    {

        #region ... Variables  ...

        protected ICommChannel2 mComm;

        /// <summary>
        /// 
        /// </summary>
        protected SortedDictionary<string, List<int>> mCachTags = new SortedDictionary<string, List<int>>();

      

        #endregion ...Variables...

        #region ... Events     ...

        #endregion ...Events...

        #region ... Constructor...

        #endregion ...Constructor...

        #region ... Properties ...

        /// <summary>
        /// 
        /// </summary>
        public IDeviceForDriver Device { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DriverData Data { get; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get { return Data.Name; }set { Data.Name = value; } }

        /// <summary>
        /// 
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ValueWriteType ValueType { get { return ValueWriteType.Bytes; } }

        #endregion ...Properties...

        #region ... Methods    ...

        #endregion ...Methods...

        #region ... Interfaces ...

        #endregion ...Interfaces...

        /// <summary>
        /// 
        /// </summary>
        public virtual void Init()
        {
            foreach (var vv in Device.ListTags())
            {
                if (!string.IsNullOrEmpty(vv.DeviceInfo))
                {
                    if (mCachTags.ContainsKey(vv.DeviceInfo))
                    {
                        mCachTags[vv.DeviceInfo].Add(vv.Id);
                    }
                    else
                    {
                        mCachTags.Add(vv.DeviceInfo, new List<int>() { vv.Id });
                    }
                }
            }

            mComm = Device.GetCommChannel();
            if (mComm != null)
            {
                mComm.CommChangedEvent += MComm_CommChangedEvent;
                RegistorReceiveCallBack(mComm);

                
                mComm.Init();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsChannelRaw()
        {
            return mComm.IsRawComm();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void RegistorReceiveCallBack(ICommChannel2 mComm)
        {
            mComm.RegistorReceiveCallBack(OnReceiveData);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MComm_CommChangedEvent(object sender, EventArgs e)
        {
            OnCommChanged(mComm.IsConnected);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        protected virtual void UpdateValue(string deviceInfo,object value)
        {
            if(mCachTags.ContainsKey(deviceInfo))
            {
                UpdateValue(mCachTags[deviceInfo], value);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        protected virtual void UpdateValue(List<int> id,object value)
        {
            Device?.UpdateDeviceValue(id, value);
        }

        /// <summary>
        /// 处理写硬件设备
        /// </summary>
        /// <param name="deviceInfo"></param>
        /// <param name="value"></param>
        /// <param name="valueType"></param>
        public virtual void WriteValue(string deviceInfo, object value, byte valueType)
        {
           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected byte[] ConvertToBytes(object value,byte valuetype)
        {
            switch ((TagType)valuetype)
            {
                case TagType.Bool:
                    return BitConverter.GetBytes((bool)value);
                case TagType.Byte:
                    return BitConverter.GetBytes(Convert.ToByte(value));
                case TagType.DateTime:
                    return BitConverter.GetBytes(Convert.ToInt64(value));
                case TagType.Double:
                    return BitConverter.GetBytes(Convert.ToDouble(value));
                case TagType.Float:
                    return BitConverter.GetBytes(Convert.ToSingle(value));
                case TagType.Int:
                    return BitConverter.GetBytes(Convert.ToInt32(value));
                case TagType.Long:
                    return BitConverter.GetBytes(Convert.ToInt64(value));
                case TagType.Short:
                    return BitConverter.GetBytes(Convert.ToInt16(value));
                case TagType.String:
                    return Encoding.UTF8.GetBytes(value.ToString());
                case TagType.UInt:
                    return BitConverter.GetBytes(Convert.ToUInt32(value));
                case TagType.ULong:
                    return BitConverter.GetBytes(Convert.ToUInt64(value));
                case TagType.UShort:
                    return BitConverter.GetBytes(Convert.ToUInt16(value));
                case TagType.IntPoint:
                    IntPoint ival = (IntPoint)value;
                    byte[] val = new byte[8];
                    BitConverter.GetBytes(ival.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ival.Y).CopyTo(val, 4);
                    return val;
                case TagType.IntPoint3:
                    IntPoint3 ival3 = (IntPoint3)value;
                    val = new byte[12];
                    BitConverter.GetBytes(ival3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ival3.Y).CopyTo(val, 4);
                    BitConverter.GetBytes(ival3.Z).CopyTo(val, 8);
                    return val;
                case TagType.UIntPoint3:
                    UIntPoint3 uival3 = (UIntPoint3)value;
                    val = new byte[12];
                    BitConverter.GetBytes(uival3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(uival3.Y).CopyTo(val, 4);
                    BitConverter.GetBytes(uival3.Z).CopyTo(val, 8);
                    return val;
                case TagType.UIntPoint:
                    UIntPoint uival = (UIntPoint)value;
                    val = new byte[8];
                    BitConverter.GetBytes(uival.X).CopyTo(val, 0);
                    BitConverter.GetBytes(uival.Y).CopyTo(val, 4);
                    return val;
                case TagType.LongPoint:
                    LongPoint lval = (LongPoint)value;
                    val = new byte[16];
                    BitConverter.GetBytes(lval.X).CopyTo(val, 0);
                    BitConverter.GetBytes(lval.Y).CopyTo(val, 8);
                    return val;
                case TagType.LongPoint3:
                    LongPoint3 lval3 = (LongPoint3)value;
                    val = new byte[24];
                    BitConverter.GetBytes(lval3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(lval3.Y).CopyTo(val, 8);
                    BitConverter.GetBytes(lval3.Z).CopyTo(val, 16);
                    return val;
                case TagType.ULongPoint:
                    ULongPoint ulval = (ULongPoint)value;
                    val = new byte[16];
                    BitConverter.GetBytes(ulval.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ulval.Y).CopyTo(val, 8);
                    return val;
                case TagType.ULongPoint3:
                    ULongPoint3 ulval3 = (ULongPoint3)value;
                    val = new byte[24];
                    BitConverter.GetBytes(ulval3.X).CopyTo(val, 0);
                    BitConverter.GetBytes(ulval3.Y).CopyTo(val, 8);
                    BitConverter.GetBytes(ulval3.Z).CopyTo(val, 16);
                    return val;
            }
            return null;
        }

        ///// <summary>
        ///// 处理写硬件设备
        ///// </summary>
        ///// <param name="values"></param>
        //public virtual void WriteValue(Dictionary<string, KeyValuePair<byte[], byte>> values)
        //{
        //    foreach (var vv in values)
        //    {
        //        WriteValue(vv.Key, vv.Value.Key, vv.Value.Value);
        //    }
        //}

        /// <summary>
        /// 处理写硬件设备
        /// </summary>
        /// <param name="values"></param>
        public void WriteValue(Dictionary<string, KeyValuePair<object, byte>> values)
        {
            foreach (var vv in values)
            {
                WriteValue(vv.Key, vv.Value.Key, vv.Value.Value);
            }
        }

        /// <summary>
        /// 通信状态改变
        /// </summary>
        protected virtual void OnCommChanged(bool result)
        {
            if(!result)
            {
                Device.UpdateAllTagQualityToCommBad();
            }
        }


        ///// <summary>
        ///// 接收到设备数据
        ///// <paramref name="key"/>
        ///// <paramref name="data"/>
        ///// </summary>
        //protected virtual byte[] OnReceiveData(string key,byte[] data,out bool handled)
        //{
        //    handled = false;
        //    return null;
        //}

        /// <summary>
        /// 接收到设备推送过来的数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        protected virtual object OnReceiveData(string key,object data,out bool handled)
        {
            handled = false;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object ReadValue(object value)
        {
            object re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                    re = mComm.ReadValue(value);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object ReadValue(Span<byte> value)
        {
            object re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                    re = mComm.ReadValue(value);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected object WriteValue(string key,object value)
        {
            if (!mComm.IsConnected) return false;

            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                  return  mComm.WriteValue(key, value);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool WriteValueNoWait(string key, object value)
        {
            if (!mComm.IsConnected) return false;

            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                    mComm.WriteValueNoWait(key, value);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return tre;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public byte[] SendAndWait(Span<byte> value)
        {
            byte[] re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();

            if (tre)
            {
                try
                {
                    re = mComm.SendAndWait(value);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="waitresultcount"></param>
        /// <returns></returns>
        protected byte[] SendAndWait(Span<byte> data, int waitresultcount)
        {
            byte[] re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();

            if (tre)
            {
                try
                {
                    re = mComm.SendAndWait(data, waitresultcount);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="startByte"></param>
        /// <param name="endByte"></param>
        /// <returns></returns>
        protected byte[] SendAndWait(Span<byte> data, byte startByte, byte endByte)
        {
            byte[] re = null;
            if (!mComm.IsConnected) return null;
            var tre = mComm.Take();

            if (tre)
            {
                try
                {
                    re = mComm.SendAndWait(data, startByte, endByte);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return re;
        }


        /// <summary>
        /// 异步发送数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool Send(Span<byte> data)
        {
            if (!mComm.IsConnected) return false;

            var tre = mComm.Take();
            if (tre)
            {
                try
                {
                    mComm.Send(data);
                }
                finally
                {
                    mComm.Release();
                }
            }
            return tre;
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected object SendObject(string key,object value)
        //{
        //    object re = null;
        //    if (!mComm.IsConnected) return null;
        //    var tre = mComm.Take();
        //    if(tre)
        //    {
        //        try
        //        {
        //            re = mComm.SendObject(key, value);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return re;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected byte[] SendObject(string key, Span<byte> value)
        //{
        //    byte[] re = null;
        //    if (!mComm.IsConnected) return null;
        //    var tre = mComm.Take();
        //    if (tre)
        //    {
        //        try
        //        {
        //            re = mComm.SendObject(key, value);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return re;
        //}


        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected bool SendObjectAsync(string key, object value)
        //{
        //    if (!mComm.IsConnected) return false;

        //    var tre = mComm.Take();
        //    if (tre)
        //    {
        //        try
        //        {
        //            mComm.SendObjectAsync(key,value);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return tre;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="value"></param>
        ///// <returns></returns>
        //protected bool SendObjectAsync(string key, Span<byte> value)
        //{
        //    if (!mComm.IsConnected) return false;

        //    var tre = mComm.Take();
        //    if (tre)
        //    {
        //        try
        //        {
        //            mComm.SendObjectAsync(key, value);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return tre;
        //}

        ///// <summary>
        ///// 发送数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //protected byte[] SendData(Span<byte> data)
        //{
        //    byte[] re = null;
        //    if (!mComm.IsConnected) return null;
        //    var tre = mComm.Take();

        //    if (tre)
        //    {
        //        try
        //        {
        //            re = mComm.Send(data);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return re;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="waitresultcount"></param>
        ///// <returns></returns>
        //protected byte[] SendData(Span<byte> data,int waitresultcount)
        //{
        //    byte[] re = null;
        //    if (!mComm.IsConnected) return null;
        //    var tre = mComm.Take();

        //    if (tre)
        //    {
        //        try
        //        {
        //            re = mComm.Send(data,waitresultcount);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return re;
        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="data"></param>
        ///// <param name="startByte"></param>
        ///// <param name="endByte"></param>
        ///// <returns></returns>
        //protected byte[] SendData(Span<byte> data, byte startByte,byte endByte)
        //{
        //    byte[] re = null;
        //    if (!mComm.IsConnected) return null;
        //    var tre = mComm.Take();

        //    if (tre)
        //    {
        //        try
        //        {
        //            re = mComm.Send(data, startByte,endByte);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return re;
        //}


        ///// <summary>
        ///// 异步发送数据
        ///// </summary>
        ///// <param name="key"></param>
        ///// <param name="data"></param>
        ///// <returns></returns>
        //protected bool SendDataAsync(Span<byte> data)
        //{
        //    if (!mComm.IsConnected) return false;

        //    var tre = mComm.Take();
        //    if (tre)
        //    {
        //        try
        //        {
        //            mComm.SendAsync(data);
        //        }
        //        finally
        //        {
        //            mComm.Release();
        //        }
        //    }
        //    return tre;
        //}

        /// <summary>
        /// 
        /// </summary>
        public virtual void Prepare()
        {
            using (ChannelPrepareContext ctx = new ChannelPrepareContext())
            {
                ctx.Add("Tags", mCachTags.Keys.ToList());
                ctx.Add("DeviceName", this.Device.Name);
                if(mComm!=null)
                mComm.Prepare(ctx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Start()
        {
            Prepare();
            if(mComm!=null)
            mComm.Open();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Stop()
        {
            if (mComm != null)
                mComm.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Dispose()
        {
            Device = null;
            mCachTags.Clear();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xe"></param>
        public virtual void Load(XElement xe)
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IDriverRuntime NewApi();


    }
}
