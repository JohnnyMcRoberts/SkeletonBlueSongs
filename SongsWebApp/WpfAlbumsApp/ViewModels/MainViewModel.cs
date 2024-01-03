using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Windows.Input;
using System.Windows.Forms;

using SongsDatabase.DataModels;

using SongsFileImportExport.Parser;
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
        /// The select playlists command.
        /// </summary>
        private ICommand _selectPlaylistsCommand;

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

        /// <summary>
        /// The export added albums command.
        /// </summary>
        private ICommand _exportAddedAlbumsCommand;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the playlists files to work with.
        /// </summary>
        public ObservableCollection<string> Playlists { get; private set; }
            = new ObservableCollection<string>();

        /// <summary>
        /// Gets or sets the display to file to expand.
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
        /// Gets the albums added to the library to display.
        /// </summary>
        public ObservableCollection<AlbumPlayed> AlbumsAdded{ get; private set; }
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
        /// Select the playlists command.
        /// </summary>
        public ICommand SelectPlaylistsCommand
        {
            get
            {
                return _selectPlaylistsCommand ??
                    (_selectPlaylistsCommand =
                        new CommandHandler(() => SelectPlaylistsCommandAction(), true));
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

        /// <summary>
        /// Export the Added Albums for a year command.
        /// </summary>
        public ICommand ExportAddedAlbumsCommand
        {
            get
            {
                return _exportAddedAlbumsCommand ??
                    (_exportAddedAlbumsCommand =
                        new CommandHandler(() => ExportAddedAlbumsCommandAction(), true));
            }
        }

        #endregion

        #region Command Handlers

        /// <summary>
        /// Selects the playlist file to copy from.
        /// </summary>
        public void SelectPlaylistsCommandAction()
        {
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.Filter = @"WPL File (.wpl)|*.wpl";
                fileDialog.FilterIndex = 4;
                fileDialog.RestoreDirectory = true;
                fileDialog.Multiselect = true;

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    Playlists.Clear();
                    foreach (string file in fileDialog.FileNames)
                    {
                        Playlists.Add(file);
                    }

                        
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
            if (Playlists.Any() && OutputDirectory.Length > 0)
            {
                List<AlbumPlayed> allAlbumsInPlaylists =
                    new List<AlbumPlayed>();
                foreach (string file in Playlists)
                {

                    List<AlbumPlayed> albumsInPlaylist =
                        PlaylistAlbumsParserWpl.GetAlbumsAddedFromPlaylist(file);
                    allAlbumsInPlaylists.AddRange(albumsInPlaylist);    
                }

                AlbumsAdded.Clear();
                foreach (AlbumPlayed album in allAlbumsInPlaylists)
                {
                    AlbumsAdded.Add(album);
                }

            }
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
                foreach (int year in years)
                {
                    Years.Add(year);
                }
            }
        }



        /// <summary>
        /// Exports the listened to albums to a file.
        /// </summary>
        public void ExportListenedAlbumsYearCommandAction()
        {
            if (SelectedYear > 0 && AlbumsListenedTo.Any() && OutputDirectory.Length > 0)
            {
                List<AlbumPlayed> albumsForYear =
                    AlbumsListenedTo.Where(x => x.Date.Year == SelectedYear).ToList();

                List<AlbumPlayed> albumsInOrder = GetAlbumsListenedToInOrder(albumsForYear);

                string fileName = OutputDirectory + "\\Listened-to" + SelectedYear + ".csv";

                AlbumsPlayedToCsvExporter.ExportToFile(albumsInOrder, fileName);
            }
        }


        /// <summary>
        /// Exports the listened to albums to a file.
        /// </summary>
        public void ExportAddedAlbumsCommandAction()
        {
            if (AlbumsAdded.Any() && OutputDirectory.Length > 0)
            {
                List<AlbumPlayed> albumsInOrder = AlbumsAdded.ToList();

                string fileName = OutputDirectory + "\\AddedAlbums.csv";

                AlbumsPlayedToCsvExporter.ExportToFile(albumsInOrder, fileName);
            }
        }


        private static List<AlbumPlayed> GetAlbumsListenedToInOrder(List<AlbumPlayed> albumsForYear)
        {
            Dictionary<string, List<AlbumPlayed>> albumsPlayed =
                new Dictionary<string, List<AlbumPlayed>>();

            foreach (AlbumPlayed album in albumsForYear)
            {
                string albumTitle = album.Artist + " - " + album.Album;
                if (albumsPlayed.ContainsKey(albumTitle))
                {
                    albumsPlayed[albumTitle].Add(album);
                }
                else
                {
                    albumsPlayed.Add(albumTitle, new List<AlbumPlayed>() { album });
                }
            }

            List<string> albumNames = albumsPlayed.Keys.OrderBy(x => x).ToList();
            List<AlbumPlayed> albumsInOrder = new List<AlbumPlayed>();
            foreach (string albumName in albumNames)
            {
                albumsInOrder.Add(albumsPlayed[albumName].First());
            }

            return albumsInOrder;
        }

        #endregion
    }
}
