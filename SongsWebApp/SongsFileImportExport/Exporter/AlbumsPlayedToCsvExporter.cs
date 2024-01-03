using CsvHelper;
using SongsDatabase.DataModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace SongsFileImportExport.Exporter
{
    public static class AlbumsPlayedToCsvExporter
    {
        public static void ExportToFile(List<AlbumPlayed> albums, string fileName)
        {
            using (TextWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                CsvWriter csv = 
                    new CsvWriter(writer, CultureInfo.InvariantCulture);
                csv.WriteRecords(albums); // where values implements IEnumerable
            }
        }
    }
}
