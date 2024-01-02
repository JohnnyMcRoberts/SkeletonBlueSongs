using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;
using System.IO;
using System.Xml;
using System.Collections.ObjectModel;
using System.Windows.Forms;
using SongsFileImportExport.Parser;
using SongsDatabase.DataModels;
using SongsFileImportExport.Exporter;

namespace WpfAlbumsApp.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {

        #region INotifyPropertyChanged Members

        void OnPropertyChanged<T>(Expression<Func<T>> sExpression)
        {
            if (sExpression == null) throw new ArgumentNullException("sExpression");

            MemberExpression body = sExpression.Body as MemberExpression;
            if (body == null)
            {
                throw new ArgumentException("Body must be a member expression");
            }
            OnPropertyChanged(body.Member.Name);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion // INotifyPropertyChanged Members

        #region Private variables

        /// <summary>
        /// The select playlist command.
        /// </summary>
        private ICommand _selectPlaylistCommand;

        /// <summary>
        /// The select listened albums command.
        /// </summary>
        private ICommand _selectListenedAlbumsCommand;

        /// <summary>
        /// The select output directory command.
        /// </summary>
        private ICommand _selectOutputDirectoryCommand;        

        /// <summary>
        /// The show playlist command.
        /// </summary>
        private ICommand _showPlaylistCommand;

        /// <summary>
        /// The show listened albums command.
        /// </summary>
        private ICommand _showListenedAlbumsCommand;

        /// <summary>
        /// The export listened albums year command.
        /// </summary>
        private ICommand _exportListenedAlbumsYearCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the playlist file to expand.
        /// </summary>
        public string Playlist { get; private set; }

        /// <summary>
        /// Gets or sets the listened to file to expand.
        /// </summary>
        public string ListenedAlbums { get; private set; }

        /// <summary>
        /// Gets or sets the listened to file to expand.
        /// </summary>
        public string OutputDirectory { get; private set; }

        /// <summary>
        /// Gets the listened to albums to display.
        /// </summary>
        public ObservableCollection<AlbumPlayed> AlbumsListenedTo { get; private set; }
            = new ObservableCollection<AlbumPlayed>();


        /// <summary>
        /// Gets the listened to albums to display.
        /// </summary>
        public ObservableCollection<int> Years { get; private set; }
            = new ObservableCollection<int>();

        /// <summary>
        /// Gets or sets the listened to year to get.
        /// </summary>
        public int SelectedYear { get; set; } = 0;
        

        #endregion

        #region Commands 

        /// <summary>
        /// Select the playlist command.
        /// </summary>
        public ICommand SelectPlaylistCommand
        {
            get
            {
                return _selectPlaylistCommand ??
                    (_selectPlaylistCommand =
                        new CommandHandler(() => SelectPlaylistCommandAction(), true));
            }
        }

        /// <summary>
        /// Select the Listened Albums command.
        /// </summary>
        public ICommand SelectListenedAlbumsCommand
        {
            get
            {
                return _selectListenedAlbumsCommand ??
                    (_selectListenedAlbumsCommand =
                        new CommandHandler(() => SelectListenedAlbumsCommandAction(), true));
            }
        }

        /// <summary>
        /// Select the Listened Albums command.
        /// </summary>
        public ICommand SelectOutputDirectoryCommand
        {
            get
            {
                return _selectOutputDirectoryCommand ??
                    (_selectOutputDirectoryCommand =
                        new CommandHandler(() => SelectOutputDirectoryCommandAction(), true));
            }
        }

        

        /// <summary>
        /// Show the playlist command.
        /// </summary>
        public ICommand ShowPlaylistCommand
        {
            get
            {
                return _showPlaylistCommand ??
                    (_showPlaylistCommand =
                        new CommandHandler(() => ShowPlaylistCommandAction(), true));
            }
        }

        /// <summary>
        /// Show the Listened Albums command.
        /// </summary>
        public ICommand ShowListenedAlbumsCommand
        {
            get
            {
                return _showListenedAlbumsCommand ??
                    (_showListenedAlbumsCommand =
                        new CommandHandler(() => ShowListenedAlbumsCommandAction(), true));
            }
        }


        /// <summary>
        /// Export the Listened Albums for a year command.
        /// </summary>
        public ICommand ExportListenedAlbumsYearCommand
        {
            get
            {
                return _exportListenedAlbumsYearCommand ??
                    (_exportListenedAlbumsYearCommand =
                        new CommandHandler(() => ExportListenedAlbumsYearCommandAction(), true));
            }
        }
        
        #endregion

        #region Command Handlers

        /// <summary>
        /// Selects the playlist file to copy from.
        /// </summary>
        public void SelectPlaylistCommandAction()
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = @"WPL File (.wpl)|*.wpl";
                fileDialog.FilterIndex = 4;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Playlist = fileDialog.FileName;
                    OnPropertyChanged(() => Playlist);
                }
            }
        }


        /// <summary>
        /// Selects the listened albums file to copy from.
        /// </summary>
        public void SelectListenedAlbumsCommandAction()
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = @"TXT File (.txt)|*.txt";
                fileDialog.FilterIndex = 4;
                fileDialog.RestoreDirectory = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    ListenedAlbums = fileDialog.FileName;
                    OnPropertyChanged(() => ListenedAlbums);
                }
            }
        }

        /// <summary>
        /// The selects the output directory to put the files into.
        /// </summary>
        public void SelectOutputDirectoryCommandAction()
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.ShowNewFolderButton = true;
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    OutputDirectory = dialog.SelectedPath;
                    OnPropertyChanged(() => OutputDirectory);
                }
            }
        }
        

        /// <summary>
        /// Shows the playlist file to copy from.
        /// </summary>
        public void ShowPlaylistCommandAction()
        {
        }

        /// <summary>
        /// Shows the listened albums file to copy from.
        /// </summary>
        public void ShowListenedAlbumsCommandAction()
        {
            if (ListenedAlbums.Length > 0)
            {
                List<AlbumPlayed> results =
                    SongFileParser.GetAlbumsPlayedFromFile(ListenedAlbums);

                AlbumsListenedTo.Clear();
                foreach (AlbumPlayed album in results)
                {
                    AlbumsListenedTo.Add(album);
                }

                List<int> years = results.Select(x => x.Date.Year).Distinct().ToList();
                Years.Clear();
                foreach(int year in years)
                {
                    Years.Add(year);
                }
            }
        }



        /// <summary>
        /// Shows the listened albums file to copy from.
        /// </summary>
        public void ExportListenedAlbumsYearCommandAction()
        {
            if (SelectedYear > 0 && AlbumsListenedTo.Any() && OutputDirectory.Length > 0)
            {
                List<AlbumPlayed> albumsForYear =
                    AlbumsListenedTo.Where(x => x.Date.Year == SelectedYear).ToList();

                string fileName =
                    OutputDirectory + "\\Listened-to" + SelectedYear + ".csv";

                AlbumsPlayedToCsvExporter.ExportToFile(albumsForYear, fileName);

                Dictionary<string, List<AlbumPlayed>> albumsPlayed =
                    new Dictionary<string, List<AlbumPlayed>>();

                foreach(AlbumPlayed album in albumsForYear)
                {
                    string albumTitle = album.Artist + " - " + album.Album;
                    if(albumsPlayed.ContainsKey(albumTitle))
                    {
                        albumsPlayed[albumTitle].Add(album);
                    }
                    else
                    {
                        albumsPlayed.Add(albumTitle, new List<AlbumPlayed>() { album });
                    }
                }

                var allAlbumsPlayed = albumsPlayed;
            }

        }

        #endregion
    }
}
