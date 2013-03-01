﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace dotTwitchTV
{

    public class Twitch
    {
        #region Constants
        private const string userAgent = "Mozilla/5.0 (Windows NT 6.0; WOW64; rv:14.0) Gecko/20100101 Firefox/14.0.1";
        private const string channelJsonUrl = "http://api.justin.tv/api/stream/list.json?channel={0}&t={1}";
        #endregion

        
        #region Private properties
        private Timer bwDownloader;

        private CookieAwareWebClient wc;
        private bool prevOnlineState = false;
        private Channel currentChannel;
        private string currentChannelName;
        #endregion
        #region Events
        public event EventHandler<EventArgs> Live;
        public event EventHandler<EventArgs> Offline;
        private void DefaultEvent(EventHandler<EventArgs> evnt, EventArgs e)
        {
            EventHandler<EventArgs> handler = evnt;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void OnLive( EventArgs e)
        {
            DefaultEvent(Live, e);
        }
        private void OnOffline(EventArgs e)
        {
            DefaultEvent(Offline, e);
        }
        #endregion

        #region Public methods
        public Twitch(string channelName)
        {
            currentChannelName = channelName;

            wc = new CookieAwareWebClient();
            wc.Headers["User-Agent"] = userAgent;
        }
        public void Start()
        {
            bwDownloader = new Timer(new TimerCallback(bwDownloader_Tick), null, 0, 20000);
        }
        private void bwDownloader_Tick(object o)
        {
            CrawlTwitchChannel( currentChannelName );
        }
        private void CrawlTwitchChannel( string channel )
        {
            if( String.IsNullOrEmpty(currentChannelName) )
                return;
            
            try
            {
                wc.Headers["Cache-Control"] = "no-cache";
                var stream = wc.downloadURL(String.Format(channelJsonUrl, channel,(DateTime.UtcNow - new DateTime(1970,1,1,0,0,0)).TotalSeconds) );
                if (stream == null)
                {
                    Debug.Print("Can't download channel info of {0} result stream is null. Url: {1}", currentChannelName, channelJsonUrl);
                    return;
                }

                var tempChannel = ParseJson<List<Channel>>.ReadObject(stream).FirstOrDefault();

                                
                if (tempChannel == null)
                    Debug.Print("Can't parse json of {0}. Url: {1}", currentChannelName, channelJsonUrl);

                stream.Close();
                stream.Dispose();

                if (isAlive() && tempChannel == null)
                {
                    if (prevOnlineState != isAlive())
                    {
                        prevOnlineState = isAlive();
                        OnOffline(new EventArgs());
                    }
                }
                else if (!isAlive() && tempChannel != null)
                {
                    if (prevOnlineState != isAlive())
                    {
                        prevOnlineState = isAlive();
                        OnLive(new EventArgs());
                    }
                }
                currentChannel = tempChannel;


            }
            catch 
            { 
            }            
        }
        private bool isAlive()
        {
            return currentChannel == null;
        }
        #endregion
        #region Public properties
        public string Viewers
        {
            get { return isAlive() ? "0" : currentChannel.viewers; }
            set { }
        }
        public string Bitrate
        {
            get { return isAlive() ? "0" : currentChannel.videoBitrate; }
            set { }
        }
        #endregion

    }
}
