import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';


@Injectable(({
    providedIn: 'root',
}) as any)
export class LastFmArtistService
{
    API_KEY_LASTFM = "778c88cb555d578f77f0f4d77ffe9b2d";

    constructor(private http: HttpClient) { }

    searchForArtist(artistName: string, queryType: string) 
    {
        return this.http.get(`http://ws.audioscrobbler.com/2.0/?method=artist.${queryType}&artist=${artistName}&api_key=${this.API_KEY_LASTFM}&format=json`);
    }

    public searchForArtistResponse: any;
    async asyncSearchForArtist(artistName: string, queryType: string)
    {
        this.searchForArtistResponse =
            await this.http.get(
                `http://ws.audioscrobbler.com/2.0/?method=artist.${queryType}&artist=${artistName}&api_key=${this.API_KEY_LASTFM}&format=json`
                ).toPromise();

        console.log('asyncSearchForArtist: No issues, waiting until promise is resolved...');
    }

    searchForAlbum(artistName: string, album: string)
    {
        const url =
            `http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=${artistName}&album=${album}&api_key=${this.API_KEY_LASTFM}&format=json`;
        return this.http.get(url);
    }

    public searchForAlbumResponse: any;
    async asyncSearchForAlbum(artistName: string, album: string)
    {
        const url =
            `http://ws.audioscrobbler.com/2.0/?method=album.getinfo&artist=${artistName}&album=${album}&api_key=${this.API_KEY_LASTFM}&format=json`;

        this.searchForAlbumResponse =
            await this.http.get(url).toPromise();

        console.log('asyncSearchForAlbum: No issues, waiting until promise is resolved...');
    }

    searchMusicBrainz(mbid: string)
    {
        return this.http.get(`https://musicbrainz.org/ws/2/artist/${mbid}?inc=url-rels&fmt=json`);
    }


}