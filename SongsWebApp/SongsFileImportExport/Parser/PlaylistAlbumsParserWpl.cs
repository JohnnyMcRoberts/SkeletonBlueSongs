using PlaylistsNET.Content;
using PlaylistsNET.Models;
using SongsDatabase.DataModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SongsFileImportExport.Parser
{
    public static class PlaylistAlbumsParserWpl
    {
        public static List<AlbumPlayed> GetAlbumsAddedFromPlaylist(string filePath)
        {
            List<AlbumPlayed> albums = new List<AlbumPlayed>();

            WplContent content = new WplContent();
            StreamReader filestream = new StreamReader(filePath);
            WplPlaylist playlist = content.GetFromStream(filestream.BaseStream);

            Dictionary<string, AlbumPlayed> list = new Dictionary<string, AlbumPlayed>();

            foreach (WplPlaylistEntry track in playlist.PlaylistEntries)
            {
                string[] directories = track.Path.Split(Path.DirectorySeparatorChar);

                if (directories.Length == 4)
                {
                    string album = directories[1] + " - " + directories[2];

                    if (!list.ContainsKey(album))
                    {
                        AlbumPlayed albumImported =
                            new AlbumPlayed() { Album = directories[2], Artist = directories[1], Date = DateTime.Today };
                        list.Add(album, albumImported);
                        albums.Add(albumImported);
                    }
                }
            }

            return albums;
        }
    }
}
