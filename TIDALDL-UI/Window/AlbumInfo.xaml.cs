﻿using AIGS.Helper;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tidal;
namespace TIDALDL_UI
{
    /// <summary>
    /// AlbumInfo.xaml 的交互逻辑
    /// </summary>
    public partial class AlbumInfo : UserControl
    {
        Album m_Info;

        public AlbumInfo(Album info)
        {
            InitializeComponent();

            m_Info                 = info;
            m_CTitle.Content       = info.Title;
            m_CReleaseDate.Content = "Release date " + info.ReleaseDate;
            m_CIntro.Content       = string.Format("by {0}-{1} Tracks-{2}", info.Artist.Name, info.NumberOfTracks, TimeHelper.ConverIntToString(info.Duration));
            m_CList.ItemsSource    = info.Tracks;
            m_CImage.Source        = AIGS.Common.Convert.ConverByteArrayToBitmapImage(info.CoverData);
        }

        /// <summary>
        /// 用于关闭窗口
        /// </summary>
        private DialogSession _session;
        public void ExtendedOpenedEventHandler(object sender, DialogOpenedEventArgs eventargs)
        {
            _session = eventargs.Session;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            long lTotalSize = 0;
            ObservableCollection<SubItem> pList = new ObservableCollection<SubItem>();
            foreach (Track item in m_Info.Tracks)
            {
                pList.Add(new SubItem()
                {
                    data         = item,
                    Type         = "TRACK",
                    Status       = AIGS.Common.Status.Wait,
                    Name         = item.Title,
                    DownloadSize = 0,
                    Percentage   = 0,
                    TotalSize    = item.StreamUrl == null ? 0 : item.StreamUrl.FileSize,
                    DownloadUrl  = item.StreamUrl
                });
                lTotalSize += item.StreamUrl == null ? 0 : item.StreamUrl.FileSize;
            }

            Para.MainItems.Add(new MainItem()
            {
                data         = m_Info,
                Type         = "ALBUM",
                Status       = AIGS.Common.Status.Wait,
                Name         = m_Info.Title,
                DownloadSize = 0,
                Percentage   = 0,
                TotalSize    = lTotalSize,
                SubList      = pList
            });
            _session.Close();
        }

        private void m_CClose_Click(object sender, RoutedEventArgs e)
        {
            _session.Close();
        }
    }
}
